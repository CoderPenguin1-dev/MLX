using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MLX;

public partial class ErrorDialog : Window
{
    internal string ErrorText { get; set; } = "";
    public ErrorDialog()
    {
        InitializeComponent();
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        ErrorLogTextBox.Text = ErrorText;
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}