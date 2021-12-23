// Copyright 2003-2021 by Autodesk, Inc. 
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
using RevitLookup.Views;
using Form = System.Windows.Forms.Form;

namespace RevitLookup.Core.RevitTypes;

/// <summary>
///     Snoop.Data class to hold and format an Object value.
/// </summary>
public class ElementGeometry : Data
{
    private readonly Autodesk.Revit.ApplicationServices.Application _app;
    private readonly bool _hasGeometry;
    private readonly Element _value;

    public ElementGeometry(string label, Element val, Autodesk.Revit.ApplicationServices.Application app) : base(label)
    {
        _value = val;
        _app = app;
        _hasGeometry = false;
        if (_value is not null && _app is not null) _hasGeometry = HasModelGeometry() || HasViewSpecificGeometry();
    }

    public override bool HasDrillDown => _hasGeometry;

    public override string AsValueString()
    {
        return "<Geometry.Element>";
    }

    public override Form DrillDown()
    {
        if (!_hasGeometry) return null;
        var form = new Geometry(_value, _app);
        return form;
    }

    private bool HasModelGeometry()
    {
        return Enum
            .GetValues(typeof(ViewDetailLevel))
            .Cast<ViewDetailLevel>()
            .Select(x => new Options {DetailLevel = x})
            .Any(x => _value.get_Geometry(x) is not null);
    }

    private bool HasViewSpecificGeometry()
    {
        var view = _value.Document.ActiveView;
        if (view is null) return false;

        var options = new Options
        {
            View = view,
            IncludeNonVisibleObjects = true
        };

        return _value.get_Geometry(options) is not null;
    }
}