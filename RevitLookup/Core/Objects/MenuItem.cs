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

using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace RevitLookup.Core.Objects;

public sealed class MenuItem
{
    private MenuItem()
    {
    }

    public static MenuItem Create(string name)
    {
        return new MenuItem
        {
            Name = name
        };
    }

    public string Name { get; private init; }
    public KeyGesture Gesture { get; private set; }
    public ICommand Command { get; private set; }
    public object Parameter { get; private set; }

    public MenuItem AddCommand(Action command)
    {
        Command = new RelayCommand(command);
        return this;
    }

    public MenuItem AddCommand<T>(T parameter, Action<T> command)
    {
        Command = new RelayCommand<T>(command);
        Parameter = parameter;
        return this;
    }

    public MenuItem AddGesture(ModifierKeys modifiers, Key key)
    {
        Gesture = new KeyGesture(key, modifiers);
        return this;
    }
}