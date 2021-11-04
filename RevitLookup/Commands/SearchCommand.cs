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

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitLookup.Core.Snoop;
using RevitLookup.Forms;

// Each command is implemented as a class that provides the IExternalCommand Interface

namespace RevitLookup.Commands
{
#pragma warning disable CS4014 // Because calls to SnoopAndShow method, which is async, are not awaited, execution of the commands continues before the calls are completed

    /// <summary>
    ///     Search by and Snoop command: Browse
    ///     elements found by the condition
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class SearchCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet elems)
        {
            var revitDoc = cmdData.Application.ActiveUIDocument;
            var dbdoc = revitDoc.Document;
            var form = new SearchBy(dbdoc);
            ModelessWindowFactory.Show(form);

            return Result.Succeeded;
        }
    }
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
}