/* GPL Version 3
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
using System.IO;
using System.Net;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace MLX;

public partial class AddPresetDialog : Window
{
    public string SelectedPreset { get; set; } = "";
    public AddPresetDialog()
    {
        InitializeComponent();
    }

    private void SavePresetButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (PresetNameTextBox.Text != null && PresetNameTextBox.Text.ToLower() != "none")
        {
            string[] settings = [PresetNameTextBox.Text, UsePresetNameCheckBox.IsChecked.ToString()];
            Close(settings);
        }
    }

    private void ToggleButton_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        PresetNameTextBox.IsEnabled = !UseCurrentPresetCheckBox.IsChecked ?? false;
        if (UseCurrentPresetCheckBox.IsChecked ?? false)
            PresetNameTextBox.Text = SelectedPreset;
        else PresetNameTextBox.Text = string.Empty;
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (SelectedPreset == "None")
            UseCurrentPresetCheckBox.IsEnabled = false;
    }
}