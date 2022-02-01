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

using Autodesk.Revit.DB;
using RevitLookup.Core.RevitTypes.PlaceHolders;

namespace RevitLookup.Core.Collectors;

/// <summary>
///     This is really a collector for any object of type System.Object.  In non .NET environments, you need
///     multiple Collector objects to handle the fact that not everything is derived from a common class
///     hierarchy.  In .NET, System.Object is the root of everything, so its easy to make a single Collector.
/// </summary>
public class CollectorObj : Collector
{
    public Document Document { get; set; }

    /// <summary>
    ///     This is the point where the ball starts rolling.  We'll walk down the object's class hierarchy,
    ///     continually trying to cast it to objects we know about.  NOTE: this is intentionally not Reflection.
    ///     We can do that elsewhere, but here we want to explicitly control how data is formatted and navigated,
    ///     so we will manually walk the entire hierarchy.
    /// </summary>
    /// <param name="obj">Object to collect data for</param>
    public Task Collect(object obj)
    {
        Data.Clear();

        return obj is null
            ? Task.CompletedTask
            : ExternalExecutor.ExecuteInRevitContextAsync(_ => Collect(Document, this, obj));
    }

    private void Collect(Document document, CollectorObj collector, object objectToSnoop)
    {
        var transaction = document is {IsModifiable: false} ? new Transaction(document, GetType().Name) : null;
        transaction?.Start();

        if (objectToSnoop is ISnoopPlaceholder placeholder) objectToSnoop = placeholder.GetObject(document);

        try
        {
            var collectorExtElement = new CollectorExtensions.CollectorExtensions(document);
            collectorExtElement.Collect(collector, new CollectorEventArgs(objectToSnoop));
        }
        finally
        {
            transaction?.RollBack();
        }
    }
}