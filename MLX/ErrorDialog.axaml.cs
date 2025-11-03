using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MLX;

public partial class ErrorDialog : Window
{
    internal string Message { get; set; } = "";
    public ErrorDialog()
    {
        InitializeComponent();
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        ErrorLogTextBox.Text = Message;
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}