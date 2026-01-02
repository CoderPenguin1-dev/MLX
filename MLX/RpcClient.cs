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

using DiscordRPC;
using Path = System.IO.Path;

namespace MLX;

internal static class RpcClient
{
    private static DiscordRpcClient Client;
    internal static bool Initialized { get; private set; }

    //Called when your application first starts. Sets up RPC
    internal static void Initialize()
    {
        Client = new DiscordRpcClient("1433609491924123782");
        Client.Initialize();
        Initialized = true;
    }
    
    internal static void Dispose()
    {
        Client.ClearPresence();
        Client.Dispose();
        Initialized = false;
    }

    internal static void SetPresence(string details, string? status)
    {
        Client.SetPresence(new RichPresence()
        {
            Details = details,
            State = status,
            Assets = new Assets()
            {
                LargeImageKey = "icon"
            },
            Timestamps = new Timestamps()
            {
                Start = Timestamps.Now.Start
            },
            Buttons = [new Button()
            {
                Label = "View On GitHub",
                Url = "https://github.com/CoderPenguin1-dev/MLX"
            }],
        });
    }

    // Adapted from Minty Launcher.
    /// <summary>
    /// Generates the Discord RPC state.
    /// </summary>
    /// <param name="externalFiles"></param>
    /// <param name="gameName"></param>
    /// <returns>The RPC state as a string.</returns>
    internal static string PlayingPresenceState(string[] externalFiles, string gameName)
    {
        string state = $"{gameName} [";
        if (externalFiles.Length == 0)
            state += "Vanilla]";
        else
        {
            state += Path.GetFileName(externalFiles[0]);
            for (int i = 1; i < externalFiles.Length; i++)
            {
                state += ", ";
                if (i == 2)
                {
                    state += $"+ {externalFiles.Length - 2} more";
                    break;
                }
                
                state += Path.GetFileName(externalFiles[i]);
            }
            state += "]";
        }
        return state;
    }
}