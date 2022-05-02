// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls.Primitives;

// https://docs.microsoft.com/en-us/fluent-ui/web-components/components/anchor

namespace RevitLookup.UI.Controls;

/// <summary>
/// Creates a hyperlink to web pages, files, email addresses, locations in the same page, or anything else a URL can address.
/// </summary>
public class Anchor : Button
{
    /// <summary>
    /// Property for <see cref="Href"/>.
    /// </summary>
    public static readonly DependencyProperty HrefProperty =
        DependencyProperty.Register(nameof(Href), typeof(string), typeof(Anchor),
            new PropertyMetadata(string.Empty));

    /// <summary>
    /// Gets or sets the URL that the hyperlink points to.
    /// </summary>
    public string Href
    {
        get => (string)GetValue(HrefProperty);
        set => SetValue(HrefProperty, value);
    }

    /// <summary>
    /// This virtual method is called when button is clicked and it raises the Click event
    /// </summary>
    protected override void OnClick()
    {
        var newEvent = new RoutedEventArgs(ClickEvent, this);
        RaiseEvent(newEvent);
        if (string.IsNullOrEmpty(Href)) return;
        System.Diagnostics.ProcessStartInfo sInfo = new(new Uri(Href).AbsoluteUri)
        {
            UseShellExecute = true
        };

        System.Diagnostics.Process.Start(sInfo);
    }
}
