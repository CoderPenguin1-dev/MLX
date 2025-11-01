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
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace MLX;

public partial class MainWindow : Window
{
    private List<string> _externalFilePaths = [];
    public MainWindow()
    {
        InitializeComponent();
    }


    #region Refresh Boxes
    private void RefreshComboBox(ref ComboBox comboBox, string path, string extension)
    {
        string selectedItem = (string)comboBox.SelectedItem;
        comboBox.Items.Clear();
        comboBox.Items.Add("None");
        foreach (var item in Directory.GetFiles(path))
        {
            if (item.EndsWith(extension))
                comboBox.Items.Add(Path.GetFileNameWithoutExtension(item));
        }
        if (comboBox.Items.Contains(selectedItem))
            comboBox.SelectedItem = selectedItem;
        else comboBox.SelectedIndex = 0;
    }
    
    private void RefreshPresetsComboBox() =>
        RefreshComboBox(ref PresetsComboBox, Constants.MLX_PRESETS, Constants.MLX_PRESET_EXT);
    
    private void RefreshSourcePortsComboBox() =>
        RefreshComboBox(ref SourceportComboBox, Constants.MLX_PORTS, Constants.MLX_PORT_EXT);

    private void RefreshIWADsComboBox() =>
        RefreshComboBox(ref IWADComboBox, Constants.MLX_IWADS, Constants.MLX_IWAD_EXT);

    private void RefreshExternalFilesListBox()
    {
        ExternalFilesListBox.Items.Clear();
        foreach (var file in _externalFilePaths)
            ExternalFilesListBox.Items.Add(Path.GetFileName(file));
    }
    #endregion
    
    private void InitializeLauncher(object? sender, RoutedEventArgs e)
    {
        if (Directory.Exists(Constants.MLX_PATH))
        {
            RefreshPresetsComboBox();
            RefreshSourcePortsComboBox();
            RefreshIWADsComboBox();
        }
        else
        {
            Directory.CreateDirectory(Constants.MLX_PATH);
            Directory.CreateDirectory(Constants.MLX_IWADS);
            Directory.CreateDirectory(Constants.MLX_PRESETS);
            Directory.CreateDirectory(Constants.MLX_PORTS);
        }
        
        if (RpcClient.Initialized) // This is here literally just for the visual designer. Wow.
            RpcClient.SetPresence("Idle In Launcher", null);
    }

    private void LaunchButton_OnClick(object? sender, RoutedEventArgs e)
    {
        // Make sure either of these are actually selected.
        if (IWADComboBox.SelectedIndex == 0) return;
        if (SourceportComboBox.SelectedIndex == 0) return;

        string[] portInfo =
            File.ReadAllLines($"{Constants.MLX_PORTS}/{SourceportComboBox.SelectedItem}.{Constants.MLX_PORT_EXT}");
        string portPath = portInfo[0];
        string portArgs = portInfo[1];
        
        // Set up the arguments given to the port.
        string args = "";
        if (portInfo.Length > 0) 
            args += $"{portArgs} ";
        
        string iwadPath = 
            File.ReadAllLines($"{Constants.MLX_IWADS}/{IWADComboBox.SelectedItem}.{Constants.MLX_IWAD_EXT}")[0];
        args += $"-iwad {iwadPath}";
        
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

        
        RpcClient.SetPresence($"Playing in {SourceportComboBox.SelectedItem}", 
            RpcClient.PlayingPresenceState(_externalFilePaths.ToArray(), (string)IWADComboBox.SelectedItem));

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
                ErrorDialog errorDialog = new()
                {
                    ErrorText = process.StandardError.ReadToEnd()
                };
                errorDialog.ShowDialog(this);
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
            ExternalFilesListBox.Items.RemoveAt(ExternalFilesListBox.SelectedIndex);
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

    private void ClearButton_OnClick(object? sender, RoutedEventArgs e)
    {
        _externalFilePaths.Clear();
        ExternalFilesListBox.Items.Clear();
    }
    
    #endregion

    #region Presets
    private async void AddPresetButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Preset presetDialog = new Preset();
        presetDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        string? presetName = await presetDialog.ShowDialog<string?>(this);
        if (presetName != null)
        {
            List<string> presetFile =
                [(string)SourceportComboBox.SelectedItem, (string)IWADComboBox.SelectedItem, ExtraParametersTextBox.Text];

            string files = "";
            foreach (string file in _externalFilePaths)
                files += $"{file},";
            files = files.TrimEnd(',');
            presetFile.Add(files);
            
            File.WriteAllLines($"{Constants.MLX_PRESETS}/{presetName}.{Constants.MLX_PRESET_EXT}", presetFile);
            RefreshPresetsComboBox();
            PresetsComboBox.SelectedItem = presetName;
        }
    }
    
    private void RemovePresetButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (PresetsComboBox.SelectedIndex != 0)
        {
            File.Delete($"{Constants.MLX_PRESETS}/{PresetsComboBox.SelectedItem}.{Constants.MLX_PRESET_EXT}");
            PresetsComboBox.Items.RemoveAt(PresetsComboBox.SelectedIndex);
            PresetsComboBox.SelectedIndex = 0; // Reset to "None."
        }
    }

    private void PresetsComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        // None selected.
        if (PresetsComboBox.SelectedIndex == 0)
        {
            IWADComboBox.SelectedIndex = 0;
            SourceportComboBox.SelectedIndex = 0;
            ExtraParametersTextBox.Text = null;
            _externalFilePaths.Clear();
        }
        else if (PresetsComboBox.SelectedIndex > 0)
        {
            string[] presetFile = 
                File.ReadAllLines($"{Constants.MLX_PRESETS}/{PresetsComboBox.SelectedItem}.{Constants.MLX_PRESET_EXT}");

            if (SourceportComboBox.Items.Contains(presetFile[0]))
                SourceportComboBox.SelectedItem = presetFile[0];
            else SourceportComboBox.SelectedIndex = 0;
            
            if (IWADComboBox.Items.Contains(presetFile[1]))
                IWADComboBox.SelectedItem = presetFile[1];
            else IWADComboBox.SelectedIndex = 0;
            
            ExtraParametersTextBox.Text =  presetFile[2]; 
            
            _externalFilePaths.Clear();
            if (presetFile[3].Length > 0)
                foreach (string file in presetFile[3].Split(','))
                    _externalFilePaths.Add(file);
        }
        RefreshExternalFilesListBox();
    }
    #endregion
    
    private async void AddSourcePortButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var portWindow = new Sourceport();
        portWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        string? portName = await portWindow.ShowDialog<string?>(this);
        RefreshSourcePortsComboBox();
        if (portName != null)
            SourceportComboBox.SelectedItem = portName;
    }

    private void RemoveSourcePortButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (SourceportComboBox.SelectedIndex != 0)
        {
            File.Delete($"{Constants.MLX_PORTS}/{SourceportComboBox.SelectedItem}.{Constants.MLX_PORT_EXT}");
            SourceportComboBox.Items.RemoveAt(SourceportComboBox.SelectedIndex);
            SourceportComboBox.SelectedIndex = 0; // Reset to "None."
        }
    }

    private async void AddIWADButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var iwadWindow = new IWAD();
        iwadWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        string? iwadName = await iwadWindow.ShowDialog<string?>(this);
        RefreshIWADsComboBox();
        if (iwadName != null)
            IWADComboBox.SelectedItem = iwadName;
    }

    private void RemoveIWADButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (IWADComboBox.SelectedIndex != 0)
        {
            File.Delete($"{Constants.MLX_IWADS}/{IWADComboBox.SelectedItem}.{Constants.MLX_IWAD_EXT}");
            IWADComboBox.Items.RemoveAt(IWADComboBox.SelectedIndex);
            IWADComboBox.SelectedIndex = 0; // Reset to "None."
        }
    }
}