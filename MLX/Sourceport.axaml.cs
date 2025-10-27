using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices.JavaScript;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace MLX;

public partial class Sourceport : Window
{
    public Sourceport()
    {
        InitializeComponent();
    }

    private async void OpenPortSelector()
    {
        FilePickerOpenOptions options = new()
        {
            Title = "Select source port",
            AllowMultiple = false
        };
        var files = await StorageProvider.OpenFilePickerAsync(options);
        if (files?.Count > 0)
            SourceportPathTextBox.Text = files[0].TryGetLocalPath();
        else
            SourceportPathTextBox.Text = null;
    }

    private void SaveSourceportButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (PortNameTextBox.Text != null && SourceportPathTextBox.Text != null && PortNameTextBox.Text.ToLower() != "none")
        {
            string[] portFile = [SourceportPathTextBox.Text, SourceportArgumentsTextBox.Text];
            File.WriteAllLines($"{Constants.MLX_PORTS}/{PortNameTextBox.Text}.{Constants.MLX_PORT_EXT}", portFile);
            Close(PortNameTextBox.Text);
        }
    }

    private void SourceportPathButton_OnClick(object? sender, RoutedEventArgs e)
    {
        OpenPortSelector();
    }
}