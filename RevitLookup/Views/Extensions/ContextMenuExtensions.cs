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
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace RevitLookup.Views.Extensions;

public static class ContextMenuExtensions
{
    public static MenuItem AddMenuItem(this ContextMenu menu, string text, ICommand command)
    {
        var item = new MenuItem
        {
            Header = text,
            Command = command
        };

        menu.Items.Add(item);
        return item;
    }

    public static MenuItem AddMenuItem(this ContextMenu menu, string text, Action command)
    {
        var item = new MenuItem
        {
            Header = text,
            Command = new RelayCommand(command)
        };

        menu.Items.Add(item);
        return item;
    }

    public static MenuItem AddMenuItem<T>(this ContextMenu menu, string text, T parameter, Action<T> command)
    {
        var item = new MenuItem
        {
            Header = text,
            Command = new RelayCommand<T>(command),
            CommandParameter = parameter
        };

        menu.Items.Add(item);
        return item;
    }

    public static void AddShortcut(this MenuItem item, UIElement bindableElement, KeyGesture gesture)
    {
        bindableElement.InputBindings.Add(new InputBinding(item.Command, gesture) {CommandParameter = item.CommandParameter});
        item.InputGestureText = gesture.GetDisplayStringForCulture(CultureInfo.InvariantCulture);
    }

    public static void AddShortcut(this MenuItem item, UIElement bindableElement, ModifierKeys modifiers, Key key)
    {
        var inputGesture = new KeyGesture(key, modifiers);
        bindableElement.InputBindings.Add(new InputBinding(item.Command, inputGesture) {CommandParameter = item.CommandParameter});
        item.InputGestureText = inputGesture.GetDisplayStringForCulture(CultureInfo.InvariantCulture);
    }
}