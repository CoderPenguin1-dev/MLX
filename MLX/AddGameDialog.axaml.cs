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
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.JavaScript;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System.Security.Cryptography;

namespace MLX;

public partial class AddGameDialog : Window
{
    /// <summary>
    /// Contains the names of known games.
    /// </summary>
    private readonly string[] _gameNames =
    [
        "The Ultimate Doom", "DOOM Shareware", "Doom II: Hell On Earth", 
        "TNT: Evilution", "The Plutonia Experiment", "Chex Quest",
        "Chex Quest 3: Vanilla", "Chex Quest 3", "Hexen: Beyond Heretic",
        "Heretic"
    ];
    
    /// <summary>
    /// Contains the MD5 hashes of each game.
    /// </summary>
    private readonly string[] _gameHashes =
    [
        "c4fe9fd920207691a9f493668e0a2083", "f0cefca49926d00903cf57551d901abe", "25e1459ca71d321525f84628f45ca8cd",
        "4e158d9953c79ccf97bd0663244cc6b6", "75c8cf89566741fa9d22447604053bd7", "25485721882b050afa96a56e5758dd52",
        "99325880d3d91ee1f8b47bfd9665a887", "bce163d06521f9d15f9686786e64df13", "abb033caf81e26f12a2103e1fa25453f",
        "66d686b1ed6d35ff103f15dbd30e0341"
    ];

    /// <summary>
    /// Calculates the MD5 hash of a file.
    /// </summary>
    /// <param name="path">The path of the file.</param>
    /// <returns>The MD5 hash as a string.</returns>
    private static string GetHash(string path)
    {
        var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        byte[] hash = MD5.Create().ComputeHash(stream);
        return Convert.ToHexStringLower(hash);
    }
    
    public AddGameDialog()
    {
        InitializeComponent();
    }

    private async void OpenGamePathSelector()
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
            GamePathTextBox.Text = files[0].TryGetLocalPath();
            string md5 = GetHash(files[0].TryGetLocalPath());
            Console.WriteLine(md5);
            for (int i = 0; i < _gameNames.Length; i++)
            {
                if (_gameHashes[i].Equals(md5, StringComparison.CurrentCultureIgnoreCase))
                {
                    GameNameTextBox.Text = _gameNames[i];
                    break;
                }
            }
        }
    }

    private void SaveGameButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (GameNameTextBox.Text != null && GamePathTextBox.Text != null && GameNameTextBox.Text.ToLower() != "none")
        {
            string[] gameFile = [GamePathTextBox.Text];
            string gameName = StringKeyCode.ToKeyCode(GameNameTextBox.Text);
            File.WriteAllLines($"{Constants.GamesFolder}/{gameName}.{Constants.GameExtension}", gameFile);
            Close(GameNameTextBox.Text);
        }
    }

    private void GamePathButton_OnClick(object? sender, RoutedEventArgs e)
    {
        OpenGamePathSelector();
    }
}