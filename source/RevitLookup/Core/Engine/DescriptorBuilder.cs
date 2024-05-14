// Copyright 2003-2024 by Autodesk, Inc.
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

using RevitLookup.Core.Diagnostic;
using RevitLookup.Core.Objects;
using RevitLookup.Services.Contracts;

namespace RevitLookup.Core.Engine;

public sealed partial class DescriptorBuilder
{
    private readonly List<Descriptor> _descriptors;
    private readonly ISettingsService _settings;
    private Descriptor _currentDescriptor;
    private object _obj;
    private Type _type;
    private int _depth;
    
    private readonly ClockDiagnoser _clockDiagnoser = new();
    private readonly MemoryDiagnoser _memoryDiagnoser = new();

    private DescriptorBuilder()
    {
        _descriptors = new List<Descriptor>(16);
        _settings = Host.GetService<ISettingsService>();
    }

    public Document Context { get; private set; }
}