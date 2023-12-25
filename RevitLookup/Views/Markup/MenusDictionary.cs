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

using System.Windows;
using System.Windows.Markup;

namespace RevitLookup.Views.Markup;

[Localizability(LocalizationCategory.Ignore)]
[Ambient]
[UsableDuringInitialization(true)]
public class MenusDictionary : ResourceDictionary
{
    private const string DictionaryUri = "pack://application:,,,/RevitLookup;component/Views/Resources/Menus.xaml";

    /// <summary>
    /// Initializes a new instance of the <see cref="MenusDictionary"/> class.
    /// Default constructor defining <see cref="ResourceDictionary.Source"/> of the <c>RevitLookup UI</c> controls dictionary.
    /// </summary>
    public MenusDictionary()
    {
        Source = new Uri(DictionaryUri, UriKind.Absolute);
    }
}
