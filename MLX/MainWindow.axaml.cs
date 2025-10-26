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
        InitializeLauncher();
    }

    private void RefreshPresetsComboBox()
    {
        string selectedItem = (string)PresetsComboBox.SelectedItem;
        PresetsComboBox.Items.Clear();
        PresetsComboBox.Items.Add("None");
        foreach (var preset in Directory.GetFiles(Constants.MLX_PRESETS))
            PresetsComboBox.Items.Add(Path.GetFileNameWithoutExtension(preset));
        if (PresetsComboBox.Items.Contains(selectedItem))
            PresetsComboBox.SelectedItem = selectedItem;
        else PresetsComboBox.SelectedIndex = 0;
    }
    
    private void RefreshSourcePortsComboBox()
    {
        string selectedItem = (string)SourceportComboBox.SelectedItem;
        SourceportComboBox.Items.Clear();
        SourceportComboBox.Items.Add("None");
        foreach (var port in Directory.GetFiles(Constants.MLX_PORTS))
            SourceportComboBox.Items.Add(Path.GetFileNameWithoutExtension(port));
        if (SourceportComboBox.Items.Contains(selectedItem))
            SourceportComboBox.SelectedItem = selectedItem;
        else SourceportComboBox.SelectedIndex = 0;
    }

    private void RefreshExternalFilesListBox()
    {
        ExternalFilesListBox.Items.Clear();
        foreach (var file in _externalFilePaths)
            ExternalFilesListBox.Items.Add(Path.GetFileName(file));
    }
    
    private void InitializeLauncher()
    {
        if (Directory.Exists(Constants.MLX_PATH))
        {
            // IWADs
            foreach (var file in Directory.GetFiles(Constants.MLX_IWADS))
                IWADComboBox.Items.Add(Path.GetFileName(file));
            
            RefreshPresetsComboBox();
            RefreshSourcePortsComboBox();
        }
        else
        {
            Directory.CreateDirectory(Constants.MLX_PATH);
            Directory.CreateDirectory(Constants.MLX_IWADS);
            Directory.CreateDirectory(Constants.MLX_PRESETS);
            Directory.CreateDirectory(Constants.MLX_PORTS);
            
        }
    }

    private void LaunchButton_OnClick(object? sender, RoutedEventArgs e)
    {
        // Make sure either of these are actually selected.
        if (IWADComboBox.SelectedIndex == 0) return;
        if (SourceportComboBox.SelectedIndex == 0) return;
        
        string portPath = File.ReadAllLines($"{Constants.MLX_PORTS}/{SourceportComboBox.SelectedItem}.{Constants.MLX_PORT_EXT}")[0];
        
        // Set up the arguments given to the port.
        string args = $"-iwad {Constants.MLX_IWADS}/{IWADComboBox.SelectedItem}";
        if (_externalFilePaths.Count > 0)
        {
            List<string> dehFiles = [];
            args += " -file";
            foreach (string file in _externalFilePaths)
            {
                if (file.EndsWith(".deh"))
                    dehFiles.Add(file);
                else args += $" \"{file}\"";
            }
            
            // Handle found dehacked files.
            if (dehFiles.Count > 0)
            {
                foreach (string file in dehFiles)
                    args += $" -deh {file}";
            }
        }
        
        if (ExtraParametersTextBox.Text != null)
            args += $" {ExtraParametersTextBox.Text}";

        ProcessStartInfo startInfo = new()
        {
            FileName = portPath,
            Arguments = args
        };
        Process.Start(startInfo).WaitForExit();
    }

    #region External File Manager
    private async void AddFileButton_OnClick(object? sender, RoutedEventArgs e)
    {
        FilePickerOpenOptions options = new()
        {
            Title = "Select file.",
            AllowMultiple = true
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
        string presetName = await presetDialog.ShowDialog<string>(this);
        List<string> presetFile =
            [(string)SourceportComboBox.SelectedItem, (string)IWADComboBox.SelectedItem, ExtraParametersTextBox.Text];

        string files = "";
        foreach (string file in _externalFilePaths)
        {
            files += $"{file},";
        }
        files = files.TrimEnd(',');
        presetFile.Add(files);
        
        File.WriteAllLines($"{Constants.MLX_PRESETS}/{presetName}.{Constants.MLX_PRESET_EXT}", presetFile);
        RefreshPresetsComboBox();
        PresetsComboBox.SelectedItem = presetName;
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
            ExternalFilesListBox.Items.Clear();
        }
        else if (PresetsComboBox.SelectedIndex > 0)
        {
            string[] presetFile = File.ReadAllLines($"{Constants.MLX_PRESETS}/{PresetsComboBox.SelectedItem}.{Constants.MLX_PRESET_EXT}");

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
            RefreshExternalFilesListBox();
        }
    }
    #endregion
    private async void AddSourcePortButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var portWindow = new Sourceport();
        string portName = await portWindow.ShowDialog<string>(this);
        RefreshSourcePortsComboBox();
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
}