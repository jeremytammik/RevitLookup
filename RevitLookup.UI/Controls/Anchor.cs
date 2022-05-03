// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Diagnostics;
using System.Windows;

// https://docs.microsoft.com/en-us/fluent-ui/web-components/components/anchor

namespace RevitLookup.UI.Controls;

/// <summary>
///     Creates a hyperlink to web pages, files, email addresses, locations in the same page, or anything else a URL can address.
/// </summary>
public class Anchor : Button
{
    /// <summary>
    ///     Property for <see cref="NavigateUri" />.
    /// </summary>
    public static readonly DependencyProperty NavigateUriProperty =
        DependencyProperty.Register(nameof(NavigateUri), typeof(string), typeof(Anchor),
            new PropertyMetadata(string.Empty));

    /// <summary>
    ///     Gets or sets the URL that the hyperlink points to.
    /// </summary>
    public string NavigateUri
    {
        get => (string) GetValue(NavigateUriProperty);
        set => SetValue(NavigateUriProperty, value);
    }

    /// <summary>
    ///     This virtual method is called when button is clicked and it raises the Click event
    /// </summary>
    protected override void OnClick()
    {
        var newEvent = new RoutedEventArgs(ClickEvent, this);
        RaiseEvent(newEvent);

#if DEBUG
        Debug.WriteLine($"INFO | Anchor clicked, with href: {NavigateUri}", "RevitLookup.UI.Anchor");
#endif
        if (string.IsNullOrEmpty(NavigateUri))
            return;
        ProcessStartInfo sInfo = new(new Uri(NavigateUri).AbsoluteUri)
        {
            UseShellExecute = true
        };

        Process.Start(sInfo);
    }
}