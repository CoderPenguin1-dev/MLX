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
using System.IO;
using System.Net;
using System.Runtime.InteropServices.JavaScript;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace MLX;

public partial class Sourceport : Window
{
    public Sourceport()
    {
        InitializeComponent();
    }

    private async void OpenPortSelector()
    {
        FilePickerOpenOptions options = new()
        {
            Title = "Select source port executable",
            AllowMultiple = false,
            FileTypeFilter = [FileFilters.Applications, FileFilters.All]
        };
        var files = await StorageProvider.OpenFilePickerAsync(options);
        if (files?.Count > 0)
            SourceportPathTextBox.Text = files[0].TryGetLocalPath();
    }

    private void SaveSourceportButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (PortNameTextBox.Text != null && SourceportPathTextBox.Text != null && PortNameTextBox.Text.ToLower() != "none")
        {
            string[] portFile = [SourceportPathTextBox.Text, SourceportArgumentsTextBox.Text];
            File.WriteAllLines($"{Constants.MLX_PORTS}/{PortNameTextBox.Text}.{Constants.MLX_PORT_EXT}", portFile);
            Close(PortNameTextBox.Text);
        }
    }

    private void SourceportPathButton_OnClick(object? sender, RoutedEventArgs e)
    {
        OpenPortSelector();
    }
}