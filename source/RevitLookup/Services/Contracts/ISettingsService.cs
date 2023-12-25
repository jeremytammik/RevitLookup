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

using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitLookup.Services.Contracts;

public interface ISettingsService
{
    //User interface
    ApplicationTheme Theme { get; set; }
    WindowBackdropType Background { get; set; }
    int TransitionDuration { get; }
    bool UseHardwareRendering { get; set; }
    bool ShowTimeColumn { get; set; }
    
    //Window
    bool UseSizeRestoring { get; set; }
    double WindowWidth { get; set; }
    double WindowHeight { get; set; }

    // Descriptor builder
    bool IncludeUnsupported { get; set; }
    bool IncludePrivate { get; set; }
    bool IncludeStatic { get; set; }
    bool IncludeFields { get; set; }
    bool IncludeEvents { get; set; }
    bool IncludeExtensions { get; set; }
    bool IncludeRootHierarchy { get; set; }

    // Ribbon
    bool UseModifyTab { get; set; }

    int ApplyTransition(bool value);
    void Save();
}