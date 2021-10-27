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

using System;
using System.Reflection;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitLookup.Snoop;
using RevitLookup.Snoop.Forms;

// Each command is implemented as a class that provides the IExternalCommand Interface

namespace RevitLookup
{
#pragma warning disable CS4014 // Because calls to SnoopAndShow method, which is async, are not awaited, execution of the commands continues before the calls are completed

  /// <summary>
  ///     The classic bare-bones test.  Just brings up an Alert box to show that the connection to the external module is working.
  /// </summary>
  [Transaction(TransactionMode.Manual)]
    public class HelloWorld : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData cmdData,
            ref string msg,
            ElementSet elems)
        {
            var a = Assembly.GetExecutingAssembly();
            var version = a.GetName().Version.ToString();
            var helloDlg = new TaskDialog("Autodesk Revit");
            helloDlg.MainContent = "Hello World from " + a.Location + " v" + version;
            helloDlg.Show();
            return Result.Cancelled;
        }
    }

  /// <summary>
  ///     SnoopDB command:  Browse all Elements in the current Document
  /// </summary>
  [Transaction(TransactionMode.Manual)]
    public class CmdSnoopDb : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData cmdData,
            ref string msg,
            ElementSet elems)
        {
            var form = new Objects();
            form.SnoopAndShow(Selector.SnoopDb);
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class CmdSnoopModScopePickSurface : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData cmdData,
            ref string msg,
            ElementSet elems)
        {
            var form = new Objects();
            form.SnoopAndShow(Selector.SnoopPickFace);
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class CmdSnoopModScopePickEdge : IExternalCommand
    {
        public Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet elems)
        {
            var form = new Objects();
            form.SnoopAndShow(Selector.SnoopPickEdge);
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class CmdSnoopModScopeLinkedElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet elems)
        {
            var form = new Objects();
            form.SnoopAndShow(Selector.SnoopLinkedElement);
            return Result.Succeeded;
        }
    }

    /// <summary>
    ///     Snoop dependent elements using
    ///     Element.GetDependentElements
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class CmdSnoopModScopeDependents : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData cmdData,
            ref string msg,
            ElementSet elems)
        {
            var form = new Objects();
            form.SnoopAndShow(Selector.SnoopDependentElements);
            return Result.Succeeded;
        }
    }

    /// <summary>
    ///     SnoopDB command:  Browse the current view...
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class CmdSnoopActiveView : IExternalCommand
    {
        public Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet elems)
        {
            var form = new Objects();
            form.SnoopAndShow(Selector.SnoopActiveView);
            return Result.Succeeded;
        }
    }

    /// <summary>
    ///     Snoop ModScope command:  Browse all Elements in the current selection set
    ///     In case nothing is selected: browse visible elements
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class CmdSnoopModScope : IExternalCommand
    {
        public Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet elems)
        {
            var form = new Objects();
            form.SnoopAndShow(Selector.SnoopCurrentSelection);
            return Result.Succeeded;
        }
    }

    /// <summary>
    ///     Snoop App command:  Browse all objects that are part of the Application object
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class CmdSnoopApp : IExternalCommand
    {
        public Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet elems)
        {
            var form = new Objects();
            form.SnoopAndShow(Selector.SnoopApplication);
            return Result.Succeeded;
        }
    }

    /// <summary>
    ///     Snoop ModScope command:  Browse all Elements in the current selection set
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class CmdSampleMenuItemCallback : IExternalCommand
    {
        public Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet elems)
        {
            Result result;

            try
            {
                MessageBox.Show("Called back into RevitLookup by picking toolbar or menu item");
                result = Result.Succeeded;
            }
            catch (Exception e)
            {
                msg = e.Message;
                result = Result.Failed;
            }

            return result;
        }
    }

    /// <summary>
    ///     Search by and Snoop command: Browse
    ///     elements found by the condition
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class CmdSearchBy : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData cmdData,
            ref string msg,
            ElementSet elems)
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