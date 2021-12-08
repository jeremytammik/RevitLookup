#region Header

//
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
//

#endregion // Header

using System.Collections;
using Autodesk.Revit.DB;
using RevitLookup.Views;
using Form = System.Windows.Forms.Form;

namespace RevitLookup.Core.RevitTypes;

/// <summary>
///     Snoop.Data class to hold and format an Enumerable value.  This class can be used
///     for any object that supports the IEnumerable interface.  However, some classes,
///     such as a Map, are better seen visually in Key/Value pairs vs a straight list of
///     enumerated objects.  Use this when it works well, but write your own Snoop.Data object
///     for Enumerable cases that need better feedback to the user.
/// </summary>
public class Enumerable : Data
{
    private readonly ArrayList _objects = new();
    private readonly IEnumerable _value;

    public Enumerable(string label, IEnumerable val) : base(label)
    {
        _value = val;
        if (_value is null) return;
        foreach (var value in _value) _objects.Add(value);
    }

    public Enumerable(string label, IEnumerable val, Document doc) : base(label)
    {
        _value = val;
        if (_value is null) return;

        foreach (var value in _value)
        {
            var elementId = value as Autodesk.Revit.DB.ElementId;
            if (elementId is not null && doc is not null)
            {
                var elem = doc.GetElement(elementId);
                if (elem is null) // Likely a category
                    _objects.Add(Category.GetCategory(doc, elementId));
                else
                    _objects.Add(elem); // it's more useful for user to view element rather than element id.
            }
            else
            {
                _objects.Add(value);
            }
        }
    }

    public override bool HasDrillDown => _value is not null && _objects.Count != 0;

    public override string StrValue()
    {
        return Utils.ObjToLabelStr(_value);
    }

    public override Form DrillDown()
    {
        if (_value is null || _objects.Count == 0) return null;
        var form = new Objects(_objects);
        return form;
    }
}