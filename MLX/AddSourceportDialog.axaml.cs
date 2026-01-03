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
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace MLX;

public partial class AddSourceportDialog : Window
{
    /// <summary>
    /// Contains a list of a port's executable name (key) and their proper name (value).
    /// </summary>
    private readonly Dictionary<string, string> _knownPorts = new()
    {
        { "dsda-doom", "DSDA-Doom" }, { "nyan-doom", "Nyan Doom" }, { "lzdoom", "LZDoom" },
        { "uzdoom", "UZDoom" }, { "gzdoom", "GZDoom" }, { "zdoom", "ZDoom" },
        { "chocolate-doom", "Chocolate Doom" }, { "chocolate-heretic", "Chocolate Heretic" }, { "chocolate-hexen", "Chocolate Hexen" },
        { "chocolate-strife", "Chocolate Strife" }, { "crispy-doom", "Crispy Doom" }, { "crispy-heretic", "Crispy Heretic" },
        { "crispy-hexen", "Crispy Hexen" }, { "crispy-strife", "Crispy Strife" }, { "cherry-doom", "Cherry Doom" },
        { "woof", "Woof!" }, { "nugget-doom", "Nugget Doom" }, { "doomretro", "DOOM Retro" },
        { "fdwl", "From Doom With Love" }, { "prboom-plus", "PRBoom+" }, { "prboom", "PRBoom" }
    };
    
    public AddSourceportDialog()
    {
        InitializeComponent();
    }

    private void SaveSourceportButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (PortNameTextBox.Text != null && SourceportPathTextBox.Text != null && PortNameTextBox.Text.ToLower() != "none")
        {
            string[] portFile = [SourceportPathTextBox.Text, SourceportArgumentsTextBox.Text];
            string portName = StringKeyCode.ToKeyCode(PortNameTextBox.Text);
            File.WriteAllLines($"{Constants.PortsFolder}/{portName}.{Constants.PortExtension}", portFile);
            Close(PortNameTextBox.Text);
        }
    }

    private async void SourceportPathButton_OnClick(object? sender, RoutedEventArgs e)
    {
        FilePickerOpenOptions options = new()
        {
            Title = "Select source port executable",
            AllowMultiple = false,
            FileTypeFilter = [FileFilters.Applications, FileFilters.All]
        };
        var files = await StorageProvider.OpenFilePickerAsync(options);
        if (files?.Count > 0)
        {
            SourceportPathTextBox.Text = files[0].TryGetLocalPath();
            
            // Automatically fill in the port name.
            string portFileName = Path.GetFileNameWithoutExtension(files[0].TryGetLocalPath());
            foreach (var port in _knownPorts)
            {
                if (portFileName.Contains(port.Key, StringComparison.CurrentCultureIgnoreCase))
                {
                    PortNameTextBox.Text = port.Value;
                    break;
                }
            }
        }

    }
}