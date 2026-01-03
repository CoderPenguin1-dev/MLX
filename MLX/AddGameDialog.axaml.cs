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
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System.Security.Cryptography;

namespace MLX;

public partial class AddGameDialog : Window
{
    /// <summary>
    /// Contains the names (key) and hashes (value) of every known game.
    /// </summary>
    // The hashes were gathered from the game's respective DoomWiki (doomwiki.org) page.
    private readonly Dictionary<string, string[]> _knownGames = new()
    {
        { 
            "The Ultimate Doom", 
            [
                "c4fe9fd920207691a9f493668e0a2083", "fb35c4a5a9fd49ec29ab6e900572c524", "fb35c4a5a9fd49ec29ab6e900572c524",
                "4461d4511386518e784c647e3128e7bc", "3b37188f6337f15718b617c16e6e7a9c"
            ]
        },
        {
            "Doom",
            [
                "1cd63c5ddff1bf8ce844237f580e9cf3", "981b03e6d1dc033301aa3095acc437ce", "792fd1fea023d61210857089a7c1e351",
                "54978d12de87f162b9bcc011676cb3c0", "11e1cd216801ea2657723abc86ecb01f"
            ]
        },
        {
            "Doom Shareware", 
            [
                "f0cefca49926d00903cf57551d901abe", "90facab21eede7981be10790e3f82da2", "cea4989df52b65f4d481b706234a3dca",
                "52cbc8882f445573ce421fa5453513c1", "2a380f28e813fb0989cae5e4762ebb4c", "30aa5beb9e5ebfbbe1e1765561c08f38",
                "17aebd6b5f2ed8ce07aa526a32af8d99", "a21ae40c388cb6f2c3cc1b95589ee693", "e280233d533dcc28c1acd6ccdc7742d4",
                "762fd6d4b960d4b759730f01387a50a1", "c428ea394dc52835f2580d5bfd50d76f", "5f4eb849b1af12887dec04a2a12e5e62"
            ]
        },
        {
            "Doom II: Hell On Earth", 
            [
                "25e1459ca71d321525f84628f45ca8cd", "b96683d113c4f4e9a916e1c7d1d71ffd", "c3bea40570c23e511a7ed3ebcd9865f7",
                "8ab6d0527a29efdc1ef200e5687b5cae", "9aa3cbf65b961d0bdac98ec403b832e1", "64a4c88a871da67492aaa2020a068cd8",
                "d9153ced9fd5b898b36cc5844e35b520", "30e3c2d0350b67bfbf47271970b74b2f", "ea74a47a791fdef2e9f2ea8b8a9da13b",
                "d7a07e5d3f4625074312bc299d7ed33f", "3cb02349b3df649c86290907eed64e7b", "3cb02349b3df649c86290907eed64e7b"
            ]
        },
        {
            "TNT: Evilution",
            [
                "4e158d9953c79ccf97bd0663244cc6b6", "1d39e405bf6ee3df69a8d2646c8d5c49", "a6685de59ddf2c07f45deeec95296d98",
                "f5528f6fd55cf9629141d79eda169630", "8974e3117ed4a1839c752d5e11ab1b7b", "ad7885c17a6b9b79b09d7a7634dd7e2c"
            ]
        },
        {
            "The Plutonia Experiment",
            [
                "75c8cf89566741fa9d22447604053bd7", "3493be7e1e2588bc9c8b31eab2587a04", "0b381ff7bae93bde6496f9547463619d",
                "ae76c20366ff685d3bb9fab11b148b84", "24037397056e919961005e08611623f4", "e47cf6d82a0ccedf8c1c16a284bb5937"
            ]
        },
        {
            "Chex Quest",
            [
                "25485721882b050afa96a56e5758dd52"
            ]
        },
        {
            "Chex Quest 3",
            [
                "bce163d06521f9d15f9686786e64df13"
            ]
        },
        {
            "Chex Quest 3: Vanilla",
            [
                "b43db49801c6c9577b89810a8650cf1d", "633d63dd5c2d3965ea196d930df48666", "9d2674c2f09cb8f01ef55d430c82fa8b",
                "99325880d3d91ee1f8b47bfd9665a887"
            ]
        },
        {
            "Hexen: Beyond Heretic",
            [
                "abb033caf81e26f12a2103e1fa25453f", "a66ae0448436a990b3aecd018bc2708a", "1ac69f8f51de55452bf853831c017aec",
                "ff3f090163719f8ecb1060886027dcd4", "b2543a03521365261d0a0f74d5dd90f0"
            ]
        },
        {
            "Hexen: Beyond Heretic Demo",
            [
                "876a5a44c7b68f04b3bb9bc7a5bd69d6"
            ]
        },
        {
            "Heretic",
            [
                "66d686b1ed6d35ff103f15dbd30e0341", "a5de95a3162e71b5ffc568ba2343cd46", "1636b642aa8fcfd5e0096eefbea1f6d9",
                "fb50b962dc6570630378a8d9f4679e70", "3117e399cdb4298eaa3941625f4b2923", "1e4cb4ef075ad344dd63971637307e04"
            ]
        },
        {
            "Hacx",
            [
                "65ed74d522bdf6649c2831b13b9e02b4", "402ca45bb90520bfef0dec6baac5889e", "1511a7032ebc834a3884cf390d7f186e",
                "b7fd2f43f3382cf012dc6b097a3cb182", "793f07ebadb3d7353ee5b6b6429d9afa", "ad8db94ef3bdb8522f57d4c5b3b92bd7"
            ]
        },
        {
            "Strife",
            [
                "2fed2031a5b03892106e0f117f17901f", "8f2d3a6a289f5d2f2f9c1eec02b47299"
            ]
        },
        {
            "Strife Demo",
            [
                "bb545b9c4eca0ff92c14d466b3294023", "de2c8dcad7cca206292294bdab524292"
            ]
        }
    };

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

    private async void GamePathButton_OnClick(object? sender, RoutedEventArgs e)
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
            foreach (var game in _knownGames)
            {
                bool foundGame = false;
                foreach (string hash in game.Value)
                {
                    if (hash == md5)
                    {
                        foundGame = true;
                        break;
                    }
                }

                if (foundGame)
                {
                    GameNameTextBox.Text = game.Key;
                    break;
                }
            }
        }
    }
}