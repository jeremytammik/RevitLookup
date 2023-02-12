// Copyright 2003-2022 by Autodesk, Inc.
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

using CommunityToolkit.Mvvm.ComponentModel;
using RevitLookup.ViewModels.Objects;

namespace RevitLookup.ViewModels.Dialogs;

public sealed class OpenSourceDialogViewModel : ObservableObject
{
    public OpenSourceDialogViewModel()
    {
        Software.Add(new OpenSourceSoftware()
            .AddSoftware("Nice3point.Revit.Extensions", "https://github.com/Nice3point/RevitExtensions")
            .AddLicense("MIT License", "https://github.com/Nice3point/RevitExtensions/blob/main/License.md"));
        Software.Add(new OpenSourceSoftware()
            .AddSoftware("Nice3point.Revit.Toolkit", "https://github.com/Nice3point/RevitToolkit")
            .AddLicense("MIT License", "https://github.com/Nice3point/RevitToolkit/blob/develop/Readme.md"));
        Software.Add(new OpenSourceSoftware()
            .AddSoftware("Nice3point.Revit.Templates", "https://github.com/Nice3point/RevitTemplates")
            .AddLicense("MIT License", "https://github.com/Nice3point/RevitTemplates/blob/main/License.md"));
        Software.Add(new OpenSourceSoftware()
            .AddSoftware("CommunityToolkit", "https://github.com/CommunityToolkit/dotnet")
            .AddLicense("MIT License", "https://github.com/CommunityToolkit/dotnet/blob/main/License.md"));
        Software.Add(new OpenSourceSoftware()
            .AddSoftware("WPF-UI", "https://github.com/lepoco/wpfui")
            .AddLicense("MIT License", "https://github.com/lepoco/wpfui/blob/main/LICENSE"));
    }

    public List<OpenSourceSoftware> Software { get; } = new();
}