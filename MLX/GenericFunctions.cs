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

using System;
using System.IO;
using Avalonia.Controls;

namespace MLX;

internal static class GenericFunctions
{
    internal static void RefreshComboBox(ref ComboBox comboBox, string folder, string extension)
    {
        string selectedItem = (string)comboBox.SelectedItem;
        comboBox.Items.Clear();
        comboBox.Items.Add("None");
        foreach (var item in Directory.GetFiles(folder))
        {
            if (item.EndsWith(extension))
            {
                string name = StringKeyCode.FromKeyCode(Path.GetFileNameWithoutExtension(item));
                comboBox.Items.Add(name);
            }
        }
        if (comboBox.Items.Contains(selectedItem))
            comboBox.SelectedItem = selectedItem;
        else comboBox.SelectedIndex = 0;
    }

    internal static void RemoveSelectedItemFromComboBox(ref ComboBox comboBox, string folder, string extension, Action refresh)
    {
        if (comboBox.SelectedIndex != 0)
        {
            string iwadName = StringKeyCode.ToKeyCode((string)comboBox.SelectedItem);
            File.Delete($"{folder}/{iwadName}.{extension}");
            refresh();
        }
    }
}