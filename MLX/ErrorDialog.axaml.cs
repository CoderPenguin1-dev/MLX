using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Tmds.DBus.Protocol;

namespace MLX;

public partial class ErrorDialog : Window
{
    private string Message { get; set; } = "";
    
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

    internal void ShowErrorDialog(Window owner, string message)
    {
        Message = message;
        ShowDialog(owner);
    }
}