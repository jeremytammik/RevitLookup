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

namespace RevitLookup.Core.RevitTypes;

/// <summary>
///     This is the base class for all types of "Snoop.Data".  Basically, we
///     want something smarter than the original data type so that we can
///     hook it up to an editor and allow its output to go different places.
/// </summary>
public abstract class Data
{
    protected Data(string label)
    {
        Label = label;
    }

    /// <summary>
    ///     The Label value for the property (e.g. "Radius" for a Circle
    /// </summary>

    public string Label { get; set; }

    /// <summary>
    ///     Is there more information available about this property.  For instance,
    ///     a type double would not have anything further to show.  But, a Collection
    ///     can bring up a nested dialog showing all those objects.
    /// </summary>

    public virtual bool HasDrillDown => false;

    /// <summary>
    ///     Is this real data, or just a logical category separator?
    /// </summary>

    public virtual bool IsSeparator => false;

    /// <summary>
    ///     Is this an error condition
    /// </summary>

    public virtual bool IsError => false;

    /// <summary>
    ///     The value for the Property, expressed as a string
    /// </summary>
    /// <returns>The value formatted as a string</returns>
    public abstract string AsValueString();

    /// <summary>
    ///     Format the Label and Value as a single string.  The Snoop Forms will
    ///     handle the Label/Value pair individually, but in other contexts, this
    ///     could be used to make a flat list of Label/Value pairs.
    /// </summary>
    /// <returns>Label/Value pair as a string</returns>
    public override string ToString()
    {
        return $"{Label}: {AsValueString()}";
    }

    /// <summary>
    ///     Do the act of drilling down on the data
    /// </summary>
    public virtual Form DrillDown()
    {
        return null; // do nothing by default
    }
}