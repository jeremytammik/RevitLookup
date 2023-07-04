// Copyright 2003-2023 by Autodesk, Inc.
// 
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
// 
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
// 
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.

using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using RevitLookup.ViewModels.Converters;

namespace RevitLookup.Views.Extensions;

public static class ContextMenuExtensions
{
    public static void AddSeparator(this ContextMenu menu)
    {
        var separator = new Separator();
        menu.Items.Add(separator);
    }

    public static void AddLabel(this ContextMenu menu, string text)
    {
        var label = (MenuItem) menu.Resources["Label"];
        label.Header = text;
        menu.Items.Add(label);
    }

    public static MenuItem AddMenuItem(this ContextMenu menu)
    {
        var item = new Wpf.Ui.Controls.MenuItem();
        menu.Items.Add(item);

        return item;
    }

    public static MenuItem AddMenuItem(this ContextMenu menu, string resource)
    {
        var item = (MenuItem) menu.Resources[resource];
        menu.Items.Add(item);

        return item;
    }

    public static MenuItem SetCommand(this MenuItem item, Action command)
    {
        item.Command = new RelayCommand(command);

        return item;
    }

    public static MenuItem SetCommand(this MenuItem item, ICommand command)
    {
        item.Command = command;

        return item;
    }

    public static MenuItem SetCommand<T>(this MenuItem item, T parameter, Action<T> command)
    {
        item.CommandParameter = parameter;
        item.Command = new RelayCommand<T>(command);

        return item;
    }

    public static MenuItem SetCommand(this MenuItem item, bool isChecked, Action<bool> command)
    {
        item.IsChecked = isChecked;
        item.SetBinding(MenuItem.CommandParameterProperty, new Binding
        {
            Source = item,
            Converter = new InverseBooleanConverter(),
            Path = new PropertyPath(nameof(MenuItem.IsChecked))
        });
        item.Command = new RelayCommand<bool>(command);

        return item;
    }

    public static MenuItem SetCommand(this MenuItem item, bool isChecked, Func<bool, Task> command)
    {
        item.IsChecked = isChecked;
        item.SetBinding(MenuItem.CommandParameterProperty, new Binding
        {
            Source = item,
            Converter = new InverseBooleanConverter(),
            Path = new PropertyPath(nameof(MenuItem.IsChecked))
        });
        item.Command = new AsyncRelayCommand<bool>(command);

        return item;
    }

    public static MenuItem SetShortcut(this MenuItem item, UIElement bindableElement, KeyGesture gesture)
    {
        bindableElement.InputBindings.Add(new InputBinding(item.Command, gesture) {CommandParameter = item.CommandParameter});
        item.InputGestureText = gesture.GetDisplayStringForCulture(CultureInfo.InvariantCulture);

        return item;
    }

    public static MenuItem SetShortcut(this MenuItem item, UIElement bindableElement, ModifierKeys modifiers, Key key)
    {
        var inputGesture = new KeyGesture(key, modifiers);
        bindableElement.InputBindings.Add(new InputBinding(item.Command, inputGesture) {CommandParameter = item.CommandParameter});
        item.InputGestureText = inputGesture.GetDisplayStringForCulture(CultureInfo.InvariantCulture);

        return item;
    }

    public static MenuItem SetShortcut(this MenuItem item, UIElement bindableElement, Key key)
    {
        var inputGesture = new KeyGesture(key);
        bindableElement.InputBindings.Add(new InputBinding(item.Command, inputGesture) {CommandParameter = item.CommandParameter});
        item.InputGestureText = inputGesture.GetDisplayStringForCulture(CultureInfo.InvariantCulture);

        return item;
    }

    public static MenuItem SetHeader(this MenuItem item, string text)
    {
        item.Header = text;

        return item;
    }

    public static MenuItem SetGestureText(this MenuItem item, Key key)
    {
        item.InputGestureText = new KeyGesture(key).GetDisplayStringForCulture(CultureInfo.InvariantCulture);

        return item;
    }

    public static MenuItem SetAvailability(this MenuItem item, bool condition)
    {
        item.IsEnabled = condition;

        return item;
    }
}