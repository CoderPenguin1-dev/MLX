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

    // Here so I could get a custom message.
    internal static readonly FilePickerFileType All = new("All Files")
    {
        Patterns = ["*.*"]
    };
}