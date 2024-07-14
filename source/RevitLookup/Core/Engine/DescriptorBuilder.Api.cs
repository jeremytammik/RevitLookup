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

namespace RevitLookup.Core.Engine;

public sealed partial class DescriptorBuilder
{
    public static IList<Descriptor> Build(Type type)
    {
        var builder = new DescriptorBuilder
        {
            _obj = null,
            Context = Nice3point.Revit.Toolkit.Context.Document
        };

        return builder.BuildStaticObject(type);
    }

    public static IList<Descriptor> Build(object obj, Document context)
    {
        if (obj is null) return Array.Empty<Descriptor>();

        var builder = new DescriptorBuilder();

        switch (obj)
        {
            case Type staticObjectType:
                builder._obj = null;
                builder.Context = context;
                return builder.BuildStaticObject(staticObjectType);
            default:
                builder._obj = obj;
                builder.Context = context;
                return builder.BuildInstanceObject(obj.GetType());
        }
    }
}