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
using System.Collections.Generic;

namespace MLX;

/// <summary>
/// Contains a set of functions to convert invalid path characters into key codes and vise versa.
/// </summary>
internal static class StringKeyCode
{
    /// <summary>
    /// Converts a string into an array of each character.
    /// </summary>
    /// <param name="str">The string to convert.</param>
    /// <returns>A string array with each element containing a single character.</returns>
    private static string[] ToStringArray(this string str)
    {
        var list = new List<string>();
        foreach (var c in str)
            list.Add(c.ToString());
        
        return list.ToArray();
    }
    
    /// <summary>
    /// Takes a string and converts the keycodes into a pure, readable string.
    /// </summary>
    /// <param name="str">The string to convert.</param>
    /// <returns></returns>
    internal static string FromKeyCode(string str)
    {
        string[] characters = str.ToStringArray();
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] == "$")
            {
                string character = "";
                switch (characters[i + 1])
                {
                    case "0":
                        character = "$";
                        break;
                    case "1":
                        character = ":";
                        break;
                    case "2":
                        character = "*";
                        break;
                    case "3":
                        character = ">";
                        break;
                    case "4":
                        character = "<";
                        break;
                    case "5":
                        character = "?";
                        break;
                    case "6":
                        character = "/";
                        break;
                    case "7":
                        character = "\\";
                        break;
                    case "8":
                        character = "|";
                        break;
                }
                characters[i] = character;
                characters[i + 1] = "";
            }
        }
        return string.Join("", characters);
    }

    /// <summary>
    /// Takes a pure, readable string and converts the invalid characters into key codes.
    /// </summary>
    /// <param name="str">The string to convert.</param>
    /// <returns></returns>
    internal static string ToKeyCode(string str)
    {
        string[] characters = str.ToStringArray();
        for (int i = 0; i < characters.Length; i++)
        {
            switch (characters[i])
            {
                case "$":
                    characters[i] = "$0";
                    break;
                case ":":
                    characters[i] = "$1";
                    break;
                case "*":
                    characters[i] = "$2";
                    break;
                case ">":
                    characters[i] = "$3";
                    break;
                case "<":
                    characters[i] = "$4";
                    break;
                case "?":
                    characters[i] = "$5";
                    break;
                case "/":
                    characters[i] = "$6";
                    break;
                case "\\":
                    characters[i] = "$7";
                    break;
                case "|":
                    characters[i] = "$8";
                    break;
            }
        }
        return string.Join("", characters);
    }
}