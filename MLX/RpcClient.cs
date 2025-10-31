using System;
using DiscordRPC;
using DiscordRPC.Logging;
using System.Diagnostics;

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

    internal static void Dispose()
    {
        Client.ClearPresence();
        Client.Dispose();
    }
}