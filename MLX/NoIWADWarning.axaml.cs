using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MLX;

public partial class NoIWADWarning : Window
{
    public NoIWADWarning()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e) => Environment.Exit(0);

    private void Window_OnClosed(object? sender, EventArgs e) => Environment.Exit(0);
}