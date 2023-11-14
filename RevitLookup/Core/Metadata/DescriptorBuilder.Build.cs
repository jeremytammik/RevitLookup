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

using System.Reflection;
using RevitLookup.Core.Objects;
using RevitLookup.Core.Utils;

namespace RevitLookup.Core.Metadata;

public sealed partial class DescriptorBuilder
{
    private IReadOnlyCollection<Descriptor> BuildInstanceObject(Type type)
    {
        var types = new List<Type>();
        while (type.BaseType is not null)
        {
            types.Add(type);
            type = type.BaseType;
        }

        for (var i = types.Count - 1; i >= 0; i--)
        {
            _type = types[i];
            _currentDescriptor = DescriptorUtils.FindSuitableDescriptor(_obj, _type);

            var flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            if (_settings.IsStaticAllowed) flags |= BindingFlags.Static;
            if (_settings.IsPrivateAllowed) flags |= BindingFlags.NonPublic;

            AddProperties(flags);
            AddMethods(flags);
            AddFields(flags);
            AddEvents(flags);
            AddExtensions();

            _depth--;
        }

        AddEnumerableItems();

        return _descriptors;
    }

    private IReadOnlyCollection<Descriptor> BuildStaticObject(Type type)
    {
        _type = type;

        var flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;
        if (_settings.IsPrivateAllowed) flags |= BindingFlags.NonPublic;

        AddProperties(flags);
        AddMethods(flags);
        AddFields(flags);

        return _descriptors;
    }
}