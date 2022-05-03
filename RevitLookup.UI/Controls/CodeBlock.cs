// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using RevitLookup.UI.Appearance;
using RevitLookup.UI.Common;
using RevitLookup.UI.Syntax;
using Clipboard = RevitLookup.UI.Common.Clipboard;

namespace RevitLookup.UI.Controls;

/// <summary>
///     Formats and display a fragment of the source code.
/// </summary>
public class CodeBlock : ContentControl
{
    /// <summary>
    ///     Property for <see cref="SyntaxContent" />.
    /// </summary>
    public static readonly DependencyProperty SyntaxContentProperty = DependencyProperty.Register(nameof(SyntaxContent),
        typeof(object), typeof(CodeBlock),
        new PropertyMetadata(null));

    /// <summary>
    ///     Property for <see cref="ButtonCommand" />.
    /// </summary>
    public static readonly DependencyProperty ButtonCommandProperty =
        DependencyProperty.Register(nameof(NumberBox),
            typeof(IRelayCommand), typeof(CodeBlock), new PropertyMetadata(null));

    private string _sourceCode = string.Empty;

    /// <summary>
    ///     Creates new instance and assigns <see cref="ButtonCommand" /> default action.
    /// </summary>
    public CodeBlock()
    {
        SetValue(ButtonCommandProperty, new RelayCommand(o => Button_Click(this, o)));

        Theme.Changed += ThemeOnChanged;
    }

    /// <summary>
    ///     Formatted <see cref="System.Windows.Controls.ContentControl.Content" />.
    /// </summary>
    public object SyntaxContent
    {
        get => GetValue(SyntaxContentProperty);
        internal set => SetValue(SyntaxContentProperty, value);
    }

    /// <summary>
    ///     Command triggered after clicking the control button.
    /// </summary>
    public IRelayCommand ButtonCommand => (IRelayCommand) GetValue(ButtonCommandProperty);

    private void ThemeOnChanged(ThemeType currentTheme, Color systemAccent)
    {
        UpdateSyntax();
    }

    /// <summary>
    ///     This method is invoked when the Content property changes.
    /// </summary>
    /// <param name="oldContent">The old value of the Content property.</param>
    /// <param name="newContent">The new value of the Content property.</param>
    protected override void OnContentChanged(object oldContent, object newContent)
    {
        UpdateSyntax();
    }

    protected virtual void UpdateSyntax()
    {
        _sourceCode = Highlighter.Clean(Content as string ?? string.Empty);
        SyntaxContent = Highlighter.Format(_sourceCode);
    }

    private void Button_Click(object sender, object parameter)
    {
#if DEBUG
        Debug.WriteLine($"INFO | CodeBlock source: \n{_sourceCode}", "RevitLookup.UI.CodeBlock");
#endif
        Clipboard.SetText(_sourceCode);
    }
}