/* GPL-3.0-only
   MLX - A cross-platform Doom/id Tech 1 launcher
   Copyright (C) 2025 CoderPenguin1 @ coderpenguin1.dev@gmail.com
   
   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, version 3 of the License.
   
   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.
   
   You should have received a copy of the GNU General Public License
   along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace MLX;

public partial class MainWindow : Window
{
    private List<string> _externalFilePaths = [];
    private bool _usePresetNameWithRpc = false;
    public MainWindow()
    {
        InitializeComponent();
    }


    #region Refresh Boxes
    private void RefreshPresetsComboBox() =>
        GenericFunctions.RefreshComboBox(ref PresetsComboBox, Constants.PresetsFolder, Constants.PresetExtension);
    
    private void RefreshSourcePortsComboBox() =>
        GenericFunctions.RefreshComboBox(ref SourceportComboBox, Constants.PortsFolder, Constants.PortExtension);

    private void RefreshGamesComboBox() =>
        GenericFunctions.RefreshComboBox(ref GameComboBox, Constants.GamesFolder, Constants.GameExtension);

    private void RefreshExternalFilesListBox()
    {
        ExternalFilesListBox.Items.Clear();
        foreach (var file in _externalFilePaths)
            ExternalFilesListBox.Items.Add(Path.GetFileName(file));
    }
    #endregion
    
    private void InitializeLauncher(object? sender, RoutedEventArgs e)
    {
        // Create the data folder.
        if (!Directory.Exists(Constants.DataFolder))
        {
            Directory.CreateDirectory(Constants.DataFolder);
            Directory.CreateDirectory(Constants.GamesFolder);
            Directory.CreateDirectory(Constants.PresetsFolder);
            Directory.CreateDirectory(Constants.PortsFolder);
        }
        
        // Refresh all combo boxes.
        RefreshPresetsComboBox();
        RefreshSourcePortsComboBox();
        RefreshGamesComboBox();
        
        if (RpcClient.Initialized) // This is here literally just for the visual designer. Wow.
            RpcClient.SetPresence("Idle In Launcher", null);

        // Show the error dialog if the previous instance had crashed.
        if (File.Exists("mlx.error.log"))
        {
            string error = File.ReadAllText("mlx.error.log");
            File.Delete("mlx.error.log");
            string message =
                "This happened in your previous instance of MLX.\n" +
                "Please report this to the GitHub, alongside this log.\n" +
                "============\n" + error;
            new ErrorDialog().ShowErrorDialog(this, message);
        }
    }

    private void LaunchButton_OnClick(object? sender, RoutedEventArgs e)
    {
        // Make sure either of these are actually selected.
        if (GameComboBox.SelectedIndex == 0) return;
        if (SourceportComboBox.SelectedIndex == 0) return;

        string portName = StringKeyCode.ToKeyCode((string)SourceportComboBox.SelectedItem);
        string[] portInfo =
            File.ReadAllLines($"{Constants.PortsFolder}/{portName}.{Constants.PortExtension}");
        string portPath = portInfo[0];
        string portArgs = portInfo[1];
        
        // Set up the arguments given to the port.
        string args = "";
        if (portInfo.Length > 0) 
            args += $"{portArgs} ";
        
        string gameName =  StringKeyCode.ToKeyCode((string)GameComboBox.SelectedItem);
        string gamePath = 
            File.ReadAllLines($"{Constants.GamesFolder}/{gameName}.{Constants.GameExtension}")[0];
        args += $"-iwad \"{gamePath}\"";
        
        if (_externalFilePaths.Count > 0)
        {
            // Sort file types.
            List<string> dehFiles = [];
            List<string> bexFiles = [];
            List<string> exFiles = [];
            foreach (string file in _externalFilePaths)
            {
                if (file.ToLower().EndsWith(".deh"))
                    dehFiles.Add(file);
                else if (file.ToLower().EndsWith(".bex"))
                    bexFiles.Add(file);
                else exFiles.Add(file);
            }

            // Handle general file types.
            if (exFiles.Count > 0)
            {
                args += " -file";
                foreach (string file in exFiles)
                    args += $" \"{file}\"";
            }
            
            // Handle found dehacked and bex patches.
            if (dehFiles.Count > 0)
            {
                foreach (string file in dehFiles)
                    args += $" -deh \"{file}\"";
            }

            if (bexFiles.Count > 0)
            {
                foreach (string file in bexFiles)
                    args += $" -bex \"{file}\"";
            }
        }
        
        if (ExtraParametersTextBox.Text != null)
            args += $" {ExtraParametersTextBox.Text}";

        string details = $"Playing in {SourceportComboBox.SelectedItem}";
        string state;
        if (_usePresetNameWithRpc) state = $"{GameComboBox.SelectedItem} [{PresetsComboBox.SelectedItem}]";
        else state = RpcClient.PlayingPresenceState(_externalFilePaths.ToArray(), (string) GameComboBox.SelectedItem);
        RpcClient.SetPresence(details, state);
        
        ProcessStartInfo startInfo = new()
        {
            FileName = portPath,
            Arguments = args,
            UseShellExecute = false,
            RedirectStandardError = true
        };

        using (var process = Process.Start(startInfo))
        {
            process.WaitForExit();
            // Show the error dialog if an error was detected.
            if (process.ExitCode != 0)
            {
                new ErrorDialog().ShowErrorDialog(this, 
                    process.StandardError.ReadToEnd());
            }
        }
        
        RpcClient.SetPresence("Idle In Launcher", null);
    }

    #region External File Manager
    private async void AddFileButton_OnClick(object? sender, RoutedEventArgs e)
    {
        FilePickerOpenOptions options = new()
        {
            Title = "Select file",
            AllowMultiple = true,
            FileTypeFilter = 
                [FileFilters.DoomModFiles, FileFilters.WAD, FileFilters.PK3, 
                FileFilters.PK7, FileFilters.PKE, FileFilters.DEH,
                FileFilters.BEX, FileFilters.All]
        };
        var files = await StorageProvider.OpenFilePickerAsync(options);

        if (files.Count > 0)
        {
            foreach (var file in files)
                _externalFilePaths.Add(file.TryGetLocalPath());
            
            RefreshExternalFilesListBox();
        }
    }

    private void RemoveFileButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (ExternalFilesListBox.SelectedIndex != -1)
        {
            _externalFilePaths.RemoveAt(ExternalFilesListBox.SelectedIndex);
            RefreshExternalFilesListBox();
        }
    }

    private void ReorderFileUpButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (ExternalFilesListBox.SelectedIndex > 0)
        {
            int index = ExternalFilesListBox.SelectedIndex;
            string data = _externalFilePaths[index];
            List<string> files = [.. _externalFilePaths];
            // Move the item
            files.RemoveAt(index);
            files.Insert(index - 1, data);
            _externalFilePaths = [.. files];
            RefreshExternalFilesListBox();
            ExternalFilesListBox.SelectedIndex = index - 1; // Set the cursor to the new position.
        }
    }

    private void ReorderFileDownButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (ExternalFilesListBox.SelectedIndex < _externalFilePaths.Count - 1)
        {
            int index = ExternalFilesListBox.SelectedIndex;
            string data = _externalFilePaths[index];
            List<string> files = [.. _externalFilePaths];
            // Move the item
            files.RemoveAt(index);
            files.Insert(index + 1, data);
            _externalFilePaths = [.. files];
            RefreshExternalFilesListBox();
            ExternalFilesListBox.SelectedIndex = index + 1; // Set the cursor to the new position.
        }
    }

    private void ClearFilesButton_OnClick(object? sender, RoutedEventArgs e)
    {
        _externalFilePaths.Clear();
        ExternalFilesListBox.Items.Clear();
    }
    
    #endregion

    #region Presets
    private async void AddPresetButton_OnClick(object? sender, RoutedEventArgs e)
    {
        AddPresetDialog presetDialog = new AddPresetDialog();
        presetDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        
        string[]? presetSettings = await presetDialog.ShowDialog<string[]?>(this);
        if (presetSettings != null)
        {
            List<string> presetFile =
                [(string)SourceportComboBox.SelectedItem, (string)GameComboBox.SelectedItem, ExtraParametersTextBox.Text];

            string files = "";
            foreach (string file in _externalFilePaths)
                files += $"{file},";
            files = files.TrimEnd(',');
            presetFile.Add(files);
            presetFile.Add(presetSettings[1]);
            string presetName = StringKeyCode.ToKeyCode(presetSettings[0]);
            File.WriteAllLines($"{Constants.PresetsFolder}/{presetName}.{Constants.PresetExtension}", presetFile);
            RefreshPresetsComboBox();
            PresetsComboBox.SelectedItem = presetSettings[0];
        }
    }
    
    private void RemovePresetButton_OnClick(object? sender, RoutedEventArgs e) =>
        GenericFunctions.RemoveSelectedItemFromComboBox
            (ref PresetsComboBox, Constants.PresetsFolder, Constants.PresetExtension, RefreshPresetsComboBox);

    private void PresetsComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        try // loading the preset.
        {
            // "None" selected.
            if (PresetsComboBox.SelectedIndex == 0)
            {
                GameComboBox.SelectedIndex = 0;
                SourceportComboBox.SelectedIndex = 0;
                ExtraParametersTextBox.Text = null;
                _externalFilePaths.Clear();
                _usePresetNameWithRpc = false;
            }
            else if (PresetsComboBox.SelectedIndex > 0)
            {
                string presetName = StringKeyCode.ToKeyCode((string)PresetsComboBox.SelectedItem);
                string[] presetFile = 
                    File.ReadAllLines($"{Constants.PresetsFolder}/{presetName}.{Constants.PresetExtension}");

                if (SourceportComboBox.Items.Contains(presetFile[0]))
                    SourceportComboBox.SelectedItem = presetFile[0];
                else SourceportComboBox.SelectedIndex = 0;
            
                if (GameComboBox.Items.Contains(presetFile[1]))
                    GameComboBox.SelectedItem = presetFile[1];
                else GameComboBox.SelectedIndex = 0;
            
                ExtraParametersTextBox.Text =  presetFile[2]; 
            
                _externalFilePaths.Clear();
                if (presetFile[3].Length > 0)
                    foreach (string file in presetFile[3].Split(','))
                        _externalFilePaths.Add(file);
            
                _usePresetNameWithRpc = bool.Parse(presetFile[4]);
            }
        }
        catch
        {
            string message =
                "Preset file failed to be loaded completely.\n" +
                "Please delete the preset file.\n" +
                "Possible reasons could include the following:\n" +
                "   * Corrupted file data.\n" +
                "   * Using a preset from a previous version.";
            new ErrorDialog().ShowErrorDialog(this, message);
        }
        RefreshExternalFilesListBox();
    }
    #endregion
    
    private async void AddSourcePortButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var portDialog = new AddSourceportDialog()
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
        };
        portDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        string? portName = await portDialog.ShowDialog<string?>(this);
        RefreshSourcePortsComboBox();
        if (portName != null)
            SourceportComboBox.SelectedItem = portName;
    }

    private void RemoveSourcePortButton_OnClick(object? sender, RoutedEventArgs e) =>
        GenericFunctions.RemoveSelectedItemFromComboBox
            (ref SourceportComboBox, Constants.PortsFolder, Constants.PortExtension, RefreshSourcePortsComboBox);

    private async void AddGameButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var gameDialog = new AddGameDialog()
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };
        string? gameName = await gameDialog.ShowDialog<string?>(this);
        RefreshGamesComboBox();
        if (gameName != null)
            GameComboBox.SelectedItem = gameName;
    }

    private void RemoveGameButton_OnClick(object? sender, RoutedEventArgs e) =>
        GenericFunctions.RemoveSelectedItemFromComboBox
            (ref GameComboBox, Constants.GamesFolder, Constants.GameExtension, RefreshGamesComboBox);

    private async void ExtraParametersTextBox_OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            FilePickerOpenOptions options = new()
            {
                Title = "Select file",
                FileTypeFilter = 
                [FileFilters.DoomModFiles, FileFilters.WAD, FileFilters.PK3, 
                    FileFilters.PK7, FileFilters.PKE, FileFilters.DEH,
                    FileFilters.BEX, FileFilters.All]
            };

            var files = await StorageProvider.OpenFilePickerAsync(options);
            if (files.Count > 0)
                ExtraParametersTextBox.Text += $"\"{files[0].TryGetLocalPath()}\"";
        }
    }
}