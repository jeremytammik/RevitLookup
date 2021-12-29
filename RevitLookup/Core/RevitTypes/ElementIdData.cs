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
///     Snoop.Data class to hold and format an ElementId value.
/// </summary>
public class ElementIdData : Data
{
    private readonly Element _element;
    private readonly ElementId _value;

    public ElementIdData(string label, ElementId val, Document doc) : base(label)
    {
        _value = val;
        _element = doc.GetElement(val);
    }

    public override bool HasDrillDown => _element is not null;

    public override string AsValueString()
    {
        if (_element is not null) return Utils.ObjToLabelStr(_element);
        return _value != ElementId.InvalidElementId ? _value.ToString() : Utils.ObjToLabelStr(null);
    }

    public override Form DrillDown()
    {
        if (_element is null) return null;

        var form = new Objects(_element);
        return form;
    }
}