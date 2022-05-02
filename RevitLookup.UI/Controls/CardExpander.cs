// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;
using RevitLookup.UI.Common;
using RevitLookup.UI.Controls.Interfaces;

namespace RevitLookup.UI.Controls;

/// <summary>
///     Inherited from the <see cref="System.Windows.Controls.Expander" /> control which can hide the collapsible content.
/// </summary>
public class CardExpander : Expander, IIconControl
{
    /// <summary>
    ///     Property for <see cref="Subtitle" />.
    /// </summary>
    public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register(nameof(Subtitle),
        typeof(string), typeof(CardExpander), new PropertyMetadata(null));

    /// <summary>
    ///     Property for <see cref="Icon" />.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
        typeof(SymbolRegular), typeof(CardExpander), new PropertyMetadata(SymbolRegular.Empty));

    /// <summary>
    ///     Property for <see cref="IconFilled" />.
    /// </summary>
    public static readonly DependencyProperty IconFilledProperty = DependencyProperty.Register(nameof(IconFilled),
        typeof(bool), typeof(CardExpander), new PropertyMetadata(false));

    /// <summary>
    ///     Property for <see cref="HeaderContent" />.
    /// </summary>
    public static readonly DependencyProperty HeaderContentProperty =
        DependencyProperty.Register(nameof(HeaderContent), typeof(object), typeof(CardExpander),
            new PropertyMetadata(null));

    /// <summary>
    ///     Gets or sets text displayed under main <see cref="HeaderContent" />.
    /// </summary>
    public string Subtitle
    {
        get => (string) GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    /// <summary>
    ///     Gets or sets additional content displayed next to the chevron.
    /// </summary>
    public object HeaderContent
    {
        get => GetValue(HeaderContentProperty);
        set => SetValue(HeaderContentProperty, value);
    }

    /// <inheritdoc />
    public SymbolRegular Icon
    {
        get => (SymbolRegular) GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <inheritdoc />
    public bool IconFilled
    {
        get => (bool) GetValue(IconFilledProperty);
        set => SetValue(IconFilledProperty, value);
    }
}