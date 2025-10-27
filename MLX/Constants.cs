/*
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

namespace MLX;

internal static class Constants
{
    // Paths
    internal const string MLX_PATH = "MLX_Data";
    internal const string MLX_PORTS = $"{MLX_PATH}/Sourceports";
    internal const string MLX_IWADS = $"{MLX_PATH}/IWADs";
    internal const string MLX_PRESETS = $"{MLX_PATH}/Presets";
    
    // File Extensions
    internal const string MLX_PORT_EXT = "mlx_port";
    internal const string MLX_PRESET_EXT = "mlx_preset";
}