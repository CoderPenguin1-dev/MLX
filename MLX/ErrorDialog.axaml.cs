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

using Avalonia.Controls;
using Avalonia.Interactivity;

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