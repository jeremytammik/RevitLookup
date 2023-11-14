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

namespace RevitLookup.Core.Utils;

public static class ContextUtils
{
    public static Document FindSuitableContext(object obj)
    {
        var context = FindContext(obj);
        if (context is not null) return context;

        if (RevitApi.UiApplication is null) return null;
        return RevitApi.Document;
    }

    public static Document FindSuitableContext(object obj, Document context)
    {
        var actualContext = FindContext(obj);

        if (actualContext is null) return context;
        if (!actualContext.Equals(context)) return actualContext;
        return context;
    }

    private static Document FindContext(object obj)
    {
        return obj switch
        {
            Element element => element.Document,
            Parameter {Element: not null} parameter => parameter.Element.Document,
            Document document => document,
            _ => null
        };
    }
}