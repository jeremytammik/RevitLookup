#region Header
//
// Copyright 2003-2016 by Autodesk, Inc. 
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
using System.Collections.Generic;
using System.Windows.Forms;
using System.Collections;
using System.Data;
using System.Linq;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitLookup.Test;
using Autodesk.Revit.Attributes;
using System.Reflection;

// Each command is implemented as a class that provides the IExternalCommand Interface
//

namespace RevitLookup
{
  /// <summary>
  /// The classic bare-bones test.  Just brings up an Alert box to show that the connection to the external module is working.
  /// </summary>
  [Transaction( TransactionMode.Manual )]
  [Regeneration( RegenerationOption.Manual )]
  public class HelloWorld : IExternalCommand
  {
    public Result Execute( ExternalCommandData cmdData, ref string msg, ElementSet elems )
    {
      Assembly a = Assembly.GetExecutingAssembly();
      string version = a.GetName().Version.ToString();
      TaskDialog helloDlg = new TaskDialog( "Autodesk Revit" );
      helloDlg.MainContent = "Hello World from " + a.Location + " v" + version;
      helloDlg.Show();
      return Result.Cancelled;
    }
  }

  /// <summary>
  /// SnoopDB command:  Browse all Elements in the current Document
  /// </summary>

  [Transaction( TransactionMode.Manual )]
  [Regeneration( RegenerationOption.Manual )]
  public class CmdSnoopDb : IExternalCommand
  {
    public Result Execute( ExternalCommandData cmdData, ref string msg, ElementSet elems )
    {
      Result result;

      try
      {
        Snoop.CollectorExts.CollectorExt.m_app = cmdData.Application;	// TBD: see note in CollectorExt.cs
        Snoop.CollectorExts.CollectorExt.m_activeDoc = cmdData.Application.ActiveUIDocument.Document;

        // iterate over the collection and put them in an ArrayList so we can pass on
        // to our Form
        Autodesk.Revit.DB.Document doc = cmdData.Application.ActiveUIDocument.Document;
        FilteredElementCollector elemTypeCtor = ( new FilteredElementCollector( doc ) ).WhereElementIsElementType();
        FilteredElementCollector notElemTypeCtor = ( new FilteredElementCollector( doc ) ).WhereElementIsNotElementType();
        FilteredElementCollector allElementCtor = elemTypeCtor.UnionWith( notElemTypeCtor );
        ICollection<Element> founds = allElementCtor.ToElements();

        ArrayList objs = new ArrayList();
        foreach( Element element in founds )
        {
          objs.Add( element );
        }

        System.Diagnostics.Trace.WriteLine( founds.Count.ToString() );
        Snoop.Forms.Objects form = new Snoop.Forms.Objects( objs );
        ActiveDoc.UIApp = cmdData.Application;
        form.ShowDialog();

        result = Result.Succeeded;
      }
      catch( System.Exception e )
      {
        msg = e.Message;
        result = Result.Failed;
      }

      return result;
    }
  }


  /// <summary>
  /// SnoopDB command:  Browse the current view...
  /// </summary>

  [Transaction( TransactionMode.Manual )]
  [Regeneration( RegenerationOption.Manual )]
  public class CmdSnoopActiveView : IExternalCommand
  {
    public Result Execute( ExternalCommandData cmdData, ref string msg, ElementSet elems )
    {
      Result result;

      try
      {
        Snoop.CollectorExts.CollectorExt.m_app = cmdData.Application;	// TBD: see note in CollectorExt.cs
        Snoop.CollectorExts.CollectorExt.m_activeDoc = cmdData.Application.ActiveUIDocument.Document;

        // iterate over the collection and put them in an ArrayList so we can pass on
        // to our Form
        Autodesk.Revit.DB.Document doc = cmdData.Application.ActiveUIDocument.Document;
        if( doc.ActiveView == null )
        {
          TaskDialog.Show( "RevitLookup", "The document must have an active view!" );
          return Result.Cancelled;
        }

        Snoop.Forms.Objects form = new Snoop.Forms.Objects( doc.ActiveView );
        form.ShowDialog();

        result = Result.Succeeded;
      }
      catch( System.Exception e )
      {
        msg = e.Message;
        result = Result.Failed;
      }

      return result;
    }
  }

  /// <summary>
  /// Snoop ModScope command:  Browse all Elements in the current selection set
  ///                          In case nothing is selected: browse visible elements
  /// </summary>

  [Transaction( TransactionMode.Manual )]
  [Regeneration( RegenerationOption.Manual )]
  public class CmdSnoopModScope : IExternalCommand
  {
    public Result Execute( ExternalCommandData cmdData, ref string msg, ElementSet elems )
    {
      Result result;

      try
      {
        Snoop.CollectorExts.CollectorExt.m_app = cmdData.Application;
        UIDocument revitDoc = cmdData.Application.ActiveUIDocument;
        Document dbdoc = revitDoc.Document;
        Snoop.CollectorExts.CollectorExt.m_activeDoc = dbdoc; // TBD: see note in CollectorExt.cs
        Autodesk.Revit.DB.View view = dbdoc.ActiveView;

        //ElementSet ss = cmdData.Application.ActiveUIDocument.Selection.Elements; // 2015, jeremy: 'Selection.Selection.Elements' is obsolete: 'This property is deprecated in Revit 2015. Use GetElementIds() and SetElementIds instead.'
        //if (ss.Size == 0)
        //{
        //  FilteredElementCollector collector = new FilteredElementCollector( revitDoc.Document, view.Id );
        //  collector.WhereElementIsNotElementType();
        //  FilteredElementIterator i = collector.GetElementIterator();
        //  i.Reset();
        //  ElementSet ss1 = cmdData.Application.Application.Create.NewElementSet();
        //  while( i.MoveNext() )
        //  {
        //    Element e = i.Current as Element;
        //    ss1.Insert( e );
        //  }
        //  ss = ss1;
        //}

        ICollection<ElementId> ids = cmdData.Application.ActiveUIDocument.Selection.GetElementIds(); // 2016, jeremy
        if( 0 == ids.Count )
        {
          FilteredElementCollector collector
            = new FilteredElementCollector( revitDoc.Document, view.Id )
              .WhereElementIsNotElementType();
          ids = collector.ToElementIds();
        }

        //ICollection<Element> elements 
        //  = new List<Element>( ids.Select<ElementId,Element>( 
        //    id => dbdoc.GetElement( id ) ) );

        Snoop.Forms.Objects form = new Snoop.Forms.Objects( dbdoc, ids );
        ActiveDoc.UIApp = cmdData.Application;
        form.ShowDialog();

        result = Result.Succeeded;
      }
      catch( System.Exception e )
      {
        msg = e.Message;
        result = Result.Failed;
      }

      return result;
    }
  }

  /// <summary>
  /// Snoop App command:  Browse all objects that are part of the Application object
  /// </summary>

  [Transaction( TransactionMode.Manual )]
  [Regeneration( RegenerationOption.Manual )]
  public class CmdSnoopApp : IExternalCommand
  {
    public Result Execute( ExternalCommandData cmdData, ref string msg, ElementSet elems )
    {
      Result result;

      try
      {
        Snoop.CollectorExts.CollectorExt.m_app = cmdData.Application;
        Snoop.CollectorExts.CollectorExt.m_activeDoc = cmdData.Application.ActiveUIDocument.Document;	// TBD: see note in CollectorExt.cs

        Snoop.Forms.Objects form = new Snoop.Forms.Objects( cmdData.Application.Application );
        form.ShowDialog();
        ActiveDoc.UIApp = cmdData.Application;
        result = Result.Succeeded;
      }
      catch( System.Exception e )
      {
        msg = e.Message;
        result = Result.Failed;
      }

      return result;
    }
  }

  /// <summary>
  /// TestShell command:  The TestShell is a framework for adding small tests.  Each test is
  /// plugged into the TestShell UI so that you don't have to write a new external command for
  /// each test and occupy additional menu items.  Create a Test by adding new RevitLookupTestFuncs objects.
  /// </summary>

  [Transaction( TransactionMode.Manual )]
  [Regeneration( RegenerationOption.Manual )]
  public class CmdTestShell : IExternalCommand
  {
    ArrayList m_tests = new ArrayList();
    UIApplication m_app = null;

    public Result Execute( ExternalCommandData cmdData, ref string msg, ElementSet elems )
    {
      m_app = cmdData.Application;

      Snoop.CollectorExts.CollectorExt.m_app = cmdData.Application;
      Snoop.CollectorExts.CollectorExt.m_activeDoc = cmdData.Application.ActiveUIDocument.Document;	// TBD: see note in CollectorExt.cs
      Result result;

      try
      {
        // Since we dont have an "App" as such, there is no
        // app-wide data - so just create tests for the duration
        // of the cmd and then destroy them
        CreateAndAddTests();

        Test.TestForm form = new Test.TestForm( RevitLookupTestFuncs.RegisteredTestFuncs() );
        if( form.ShowDialog() == DialogResult.OK )
          form.DoTest();

        result = Result.Succeeded;
      }
      catch( System.Exception e )
      {	// we want to catch it so we can see the problem, otherwise it just silently bails out

        /*System.Exception innerException = e.InnerException;

        while (innerException != null) {
            innerException = innerException.InnerException;
        }

        msg = innerException.Message;*/
        msg = e.Message;
        MessageBox.Show( msg );


        result = Result.Failed;
      }

      finally
      {
        // if an exception happens anywhere, clean up before quitting
        RemoveAndFreeTestFuncs();
      }

      return result;
    }

    /// <summary>
    ///
    /// </summary>
    private void CreateAndAddTests()
    {
      m_tests.Add( new Test.TestElements( m_app ) );
      m_tests.Add( new Test.TestDocument( m_app ) );
      m_tests.Add( new Test.TestEStorage( m_app ) );
      m_tests.Add( new Test.TestGeometry( m_app ) );
      m_tests.Add( new Test.TestGraphicsStream( m_app ) );
      m_tests.Add( new Test.TestImportExport( m_app ) );
      m_tests.Add( new Test.TestUi( m_app ) );
      m_tests.Add( new Test.SDKSamples.SDKTestFuncs( m_app ) );
      m_tests.Add( new Test.TestApplication( m_app ) );

      foreach( RevitLookupTestFuncs testFunc in m_tests )
      {
        RevitLookupTestFuncs.AddTestFuncsToFramework( testFunc );
      }
    }

    /// <summary>
    ///
    /// </summary>
    private void
    RemoveAndFreeTestFuncs()
    {
      foreach( RevitLookupTestFuncs testFunc in m_tests )
      {
        RevitLookupTestFuncs.RemoveTestFuncsFromFramework( testFunc );
      }
    }
  }

  /// <summary>
  /// Snoop ModScope command:  Browse all Elements in the current selection set
  /// </summary>

  [Transaction( TransactionMode.Manual )]
  [Regeneration( RegenerationOption.Manual )]
  public class CmdSampleMenuItemCallback : IExternalCommand
  {
    public Result Execute( ExternalCommandData cmdData, ref string msg, ElementSet elems )
    {
      Result result;

      try
      {
        MessageBox.Show( "Called back into RevitLookup by picking toolbar or menu item" );
        result = Result.Succeeded;
      }
      catch( System.Exception e )
      {
        msg = e.Message;
        result = Result.Failed;
      }

      return result;
    }
  }

  [Transaction( TransactionMode.Manual )]
  [Regeneration( RegenerationOption.Manual )]
  public class CmdEvents : IExternalCommand
  {
    public Result Execute( ExternalCommandData cmdData, ref string msg, ElementSet elems )
    {
      EventTrack.Events.ApplicationEvents.m_app = cmdData.Application.Application;
      EventTrack.Events.DocEvents.m_docSet = cmdData.Application.Application.Documents;

      Result result;

      try
      {
        RevitLookup.EventTrack.Forms.EventsForm dbox = new RevitLookup.EventTrack.Forms.EventsForm();
        dbox.ShowDialog();

        result = Result.Succeeded;
      }
      catch( System.Exception e )
      {
        msg = e.Message;
        result = Result.Failed;
      }

      return result;
    }
  }
}
