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

using Avalonia.Platform.Storage;

namespace MLX;

internal static class FileFilters
{
    internal static readonly FilePickerFileType DoomModFiles = new("All Mod Files")
    {
        Patterns = ["*.wad", "*.pk3", "*.pke", "*.pk7", "*.deh", "*.bex"]
    };

    internal static readonly FilePickerFileType Games = new("Game")
    {
        Patterns = ["*.wad", "*.ipk3"]
    };
    
    internal static readonly FilePickerFileType WAD = new("WAD")
    {
        Patterns = ["*.wad"]
    };

    internal static readonly FilePickerFileType PK3 = new("PK3")
    {
        Patterns = ["*.pk3"]
    };

    internal static readonly FilePickerFileType PKE = new("PKE")
    {
        Patterns = ["*.pke"]
    };

    internal static readonly FilePickerFileType PK7 = new("PK7")
    {
        Patterns = ["*.pk7"]
    };

    internal static readonly FilePickerFileType DEH = new("DEH")
    {
        Patterns = ["*.deh"]
    };

    internal static readonly FilePickerFileType BEX = new("BEX")
    {
        Patterns = ["*.bex"]
    };

    internal static readonly FilePickerFileType IPK3 = new("IPK3")
    {
        Patterns = ["*.ipk3"]
    };

    internal static readonly FilePickerFileType Applications = new("Executables")
    {
        Patterns = ["*.exe", "*.appimage"]
    };

    // Here so I could get a custom message.
    internal static readonly FilePickerFileType All = new("All Files")
    {
        Patterns = ["*.*"]
    };
}