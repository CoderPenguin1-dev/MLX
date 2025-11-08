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

namespace MLX;

internal static class Constants
{
    // Paths
    internal const string DataFolder = "MLX_Data";
    internal const string PortsFolder = $"{DataFolder}/Sourceports";
    internal const string GamesFolder = $"{DataFolder}/IWADs";
    internal const string PresetsFolder = $"{DataFolder}/Presets";
    
    // File Extensions
    internal const string PortExtension = "mlx_port";
    internal const string PresetExtension = "mlx_preset";
    internal const string GameExtension = "mlx_iwad";
}