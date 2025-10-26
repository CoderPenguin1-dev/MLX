using System;
using System.IO;
using System.Net;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace MLX;

public partial class Preset : Window
{
    public Preset()
    {
        InitializeComponent();
    }

    private void SavePresetButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (PresetNameTextBox.Text != null && PresetNameTextBox.Text.ToLower() != "none")
        {
            Close(PresetNameTextBox.Text);
        }
    }
}