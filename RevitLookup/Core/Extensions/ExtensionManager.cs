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

using Autodesk.Revit.DB;
using RevitLookup.Core.Contracts;

namespace RevitLookup.Core.Extensions;

public sealed class ExtensionManager : IExtensionManager
{
    private readonly Document _context;

    public ExtensionManager(Document context)
    {
        _context = context;
    }

    public Dictionary<string, object> ValuesMap { get; } = new();

    public void Register<T>(T value, Action<DescriptorExtension<T>> extension)
    {
        var descriptorExtension = new DescriptorExtension<T>
        {
            Value = value,
            Context = _context
        };

        try
        {
            extension.Invoke(descriptorExtension);
            ValuesMap.Add(descriptorExtension.Name, descriptorExtension.Result);
        }
        catch (Exception exception)
        {
            ValuesMap.Add(descriptorExtension.Name, exception);
        }
    }
}