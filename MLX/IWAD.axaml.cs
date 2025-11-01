/* GPL Version 3
   MLX - A cross-platform Doom/id Tech 1 launcher
   Copyright (C) 2025 CoderPenguin1 @ coderpenguin1.dev@gmail.com
   
   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.
   
   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.
   
   You should have received a copy of the GNU General Public License
   along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.JavaScript;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System.Security.Cryptography;

namespace MLX;

public partial class IWAD : Window
{
    // The names of known, non-updated games.
    // Freedoom is not included due to constantly changing hashes.
    // Hashes are from doomwiki.org or hashed myself.
    private readonly string[] _gameNames =
    [
        "The Ultimate Doom", "DOOM Shareware", "Doom II: Hell On Earth", 
        "TNT: Evilution", "The Plutonia Experiment", "Chex Quest",
        "Chex Quest 3: Vanilla", "Chex Quest 3", "Hexen: Beyond Heretic",
        "Heretic"
    ];
    private readonly string[] _gameMD5 =
    [
        "c4fe9fd920207691a9f493668e0a2083", "f0cefca49926d00903cf57551d901abe", "25e1459ca71d321525f84628f45ca8cd",
        "4e158d9953c79ccf97bd0663244cc6b6", "75c8cf89566741fa9d22447604053bd7", "25485721882b050afa96a56e5758dd52",
        "99325880d3d91ee1f8b47bfd9665a887", "bce163d06521f9d15f9686786e64df13", "abb033caf81e26f12a2103e1fa25453f",
        "66d686b1ed6d35ff103f15dbd30e0341"
    ];

    private string GetMD5(string path)
    {
        var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        byte[] hash = MD5.Create().ComputeHash(stream);
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
    
    public IWAD()
    {
        InitializeComponent();
    }

    private async void OpenIWADSelector()
    {
        FilePickerOpenOptions options = new()
        {
            Title = "Select game file",
            AllowMultiple = false,
            FileTypeFilter = [FileFilters.Games, FileFilters.WAD, FileFilters.IPK3, FileFilters.All]
        };
        var files = await StorageProvider.OpenFilePickerAsync(options);
        if (files?.Count > 0)
        {
            IWADPathTextBox.Text = files[0].TryGetLocalPath();
            string md5 = GetMD5(files[0].TryGetLocalPath());
            Console.WriteLine(md5);
            for (int i = 0; i < _gameNames.Length; i++)
            {
                if (_gameMD5[i].Equals(md5, StringComparison.CurrentCultureIgnoreCase))
                {
                    IWADNameTextBox.Text = _gameNames[i];
                    break;
                }
            }
        }
    }

    private void SaveIWADButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (IWADNameTextBox.Text != null && IWADPathTextBox.Text != null && IWADNameTextBox.Text.ToLower() != "none")
        {
            string[] IWADFile = [IWADPathTextBox.Text];
            File.WriteAllLines($"{Constants.MLX_IWADS}/{IWADNameTextBox.Text}.{Constants.MLX_IWAD_EXT}", IWADFile);
            Close(IWADNameTextBox.Text);
        }
    }

    private void IWADPathButton_OnClick(object? sender, RoutedEventArgs e)
    {
        OpenIWADSelector();
    }
}