using System;
using DiscordRPC;
using DiscordRPC.Logging;
using System.Diagnostics;
using Avalonia.Controls.Shapes;
using Path = System.IO.Path;

namespace MLX;

internal static class RpcClient
{
    private static DiscordRpcClient Client;

    //Called when your application first starts. Sets up RPC
    internal static void Initialize()
    {
        Client = new DiscordRpcClient("1433609491924123782");

        //Connect to the RPC
        Client.Initialize();
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
            }
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

    internal static void Dispose()
    {
        Client.ClearPresence();
        Client.Dispose();
    }
}