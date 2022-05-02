// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

namespace RevitLookup.UI.Common;

/// <summary>
///     Set of static methods to operate on <see cref="SymbolRegular" /> and <see cref="SymbolFilled" />.
/// </summary>
public static class Glyph
{
    /// <summary>
    ///     If the icon is not found in some places, this one will be displayed.
    /// </summary>
    public const SymbolRegular DefaultIcon = SymbolRegular.Heart28;

    /// <summary>
    ///     If the filled icon is not found in some places, this one will be displayed.
    /// </summary>
    public const SymbolFilled DefaultFilledIcon = SymbolFilled.Heart28;

    /// <summary>
    ///     Finds icon based on name.
    /// </summary>
    /// <param name="name">Name of the icon.</param>
    public static SymbolRegular Parse(string name)
    {
        if (string.IsNullOrEmpty(name))
            return DefaultIcon;

        return (SymbolRegular) Enum.Parse(typeof(SymbolRegular), name);
    }

    /// <summary>
    ///     Finds icon based on name.
    /// </summary>
    /// <param name="name">Name of the icon.</param>
    public static SymbolFilled ParseFilled(string name)
    {
        if (string.IsNullOrEmpty(name))
            return DefaultFilledIcon;

        return (SymbolFilled) Enum.Parse(typeof(SymbolFilled), name);
    }
}