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
using System.Collections.Generic;
using System.Linq;
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
  /// <summary>
  /// The classic bare-bones test.  Just brings up an Alert box to show that the connection to the external module is working.
  /// </summary>
  [Transaction( TransactionMode.Manual )]
  public class HelloWorld : IExternalCommand
  {
    public Result Execute( 
      ExternalCommandData cmdData, 
      ref string msg, 
      ElementSet elems )
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
  public class CmdSnoopDb : IExternalCommand
  {
    public Result Execute( 
      ExternalCommandData cmdData, 
      ref string msg, 
      ElementSet elems )
    {
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
          ModelessWindowFactory.Show(form);  

          return Result.Succeeded;
    }
  }

  [Transaction( TransactionMode.Manual )]
  public class CmdSnoopModScopePickSurface : IExternalCommand
  {
    public Result Execute( 
      ExternalCommandData cmdData, 
      ref string msg, 
      ElementSet elems )
    {
        Reference refElem = null;

        try
        {
          refElem = cmdData.Application.ActiveUIDocument
              .Selection.PickObject( Autodesk.Revit.UI.Selection.ObjectType.Face );
        }
        catch
        {
          return Result.Succeeded;
        }

        //GeometryObject geoObject = cmdData.Application.ActiveUIDocument.Document.GetElement(refElem)
        //    .GetGeometryObjectFromReference(refElem);

        Snoop.Forms.Objects form = new Snoop.Forms.Objects( refElem );       
        ModelessWindowFactory.Show(form);       

        return Result.Succeeded;
    }
  }

  [Transaction( TransactionMode.Manual )]
  public class CmdSnoopModScopePickEdge : IExternalCommand
  {
    public Result Execute( ExternalCommandData cmdData, ref string msg, ElementSet elems )
    {
        Reference refElem = null;
        try
        {
          refElem = cmdData.Application.ActiveUIDocument
              .Selection.PickObject( Autodesk.Revit.UI.Selection.ObjectType.Edge );
        }
        catch
        {
          return Result.Succeeded;
        }

        //GeometryObject geoObject = cmdData.Application.ActiveUIDocument.Document.GetElement(refElem)
        //    .GetGeometryObjectFromReference(refElem);

        Snoop.Forms.Objects form = new Snoop.Forms.Objects( refElem );       
        ModelessWindowFactory.Show(form);     

        return Result.Succeeded;
    }
  }

  [Transaction( TransactionMode.Manual )]
  public class CmdSnoopModScopeLinkedElement : IExternalCommand
  {
    public Result Execute( ExternalCommandData cmdData, ref string msg, ElementSet elems )
    {
        Document doc =
            cmdData.Application.ActiveUIDocument.Document;

        Reference refElem = null;
        try
        {
          refElem = cmdData.Application.ActiveUIDocument
              .Selection.PickObject( Autodesk.Revit.UI.Selection.ObjectType.LinkedElement );
        }
        catch
        {
          return Result.Succeeded;
        }

        string stableReflink = refElem.ConvertToStableRepresentation( doc ).Split( ':' )[0];
        Reference refLink = Reference.ParseFromStableRepresentation( doc, stableReflink );
        RevitLinkInstance rli_return = doc.GetElement( refLink ) as RevitLinkInstance;
        var m_activeDoc = rli_return.GetLinkDocument(); 
        Element e = m_activeDoc.GetElement( refElem.LinkedElementId );
       
       
        Snoop.Forms.Objects form = new Snoop.Forms.Objects( e );      
        ModelessWindowFactory.Show(form);
        
      return Result.Succeeded;
    }
  }

  /// <summary>
  /// Snoop dependent elements using
  /// Element.GetDependentElements
  /// </summary>
  [Transaction( TransactionMode.Manual )]
  public class CmdSnoopModScopeDependents : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData cmdData,
      ref string msg,
      ElementSet elems )
    {
     
        UIDocument uidoc = cmdData.Application.ActiveUIDocument;
        ICollection<ElementId> idPickfirst = uidoc.Selection.GetElementIds();
        Document doc = uidoc.Document;

        ICollection<Element> elemSet = new List<Element>(
          idPickfirst.Select<ElementId, Element>(
            id => doc.GetElement( id ) ) );

        ICollection<ElementId> ids = elemSet.SelectMany(
          t => t.GetDependentElements( null ) ).ToList();

        Snoop.Forms.Objects form = new Snoop.Forms.Objects( doc, ids );
        
        ModelessWindowFactory.Show(form);

      return Result.Succeeded;
    }
  }

  /// <summary>
  /// SnoopDB command:  Browse the current view...
  /// </summary>
  [Transaction( TransactionMode.Manual )]
  public class CmdSnoopActiveView : IExternalCommand
  {
    public Result Execute( ExternalCommandData cmdData, ref string msg, ElementSet elems )
    {
        // iterate over the collection and put them in an ArrayList so we can pass on
        // to our Form
        Autodesk.Revit.DB.Document doc = cmdData.Application.ActiveUIDocument.Document;
        if( doc.ActiveView == null )
        {
        TaskDialog.Show( "RevitLookup", "The document must have an active view!" );
        return Result.Cancelled;
        }

        Snoop.Forms.Objects form = new Snoop.Forms.Objects( doc.ActiveView );
        ModelessWindowFactory.Show(form);

      
        return Result.Succeeded;
    }
  }

  /// <summary>
  /// Snoop ModScope command:  Browse all Elements in the current selection set
  ///                          In case nothing is selected: browse visible elements
  /// </summary>
  [Transaction( TransactionMode.Manual )]
  public class CmdSnoopModScope : IExternalCommand
  {
        public Result Execute(ExternalCommandData cmdData, ref string msg, ElementSet elems)
        {
            UIDocument revitDoc = cmdData.Application.ActiveUIDocument;
            Document dbdoc = revitDoc.Document;
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
            if (0 == ids.Count)
            {
                FilteredElementCollector collector
                  = new FilteredElementCollector(revitDoc.Document, view.Id)
                    .WhereElementIsNotElementType();
                ids = collector.ToElementIds();
            }

            //ICollection<Element> elements
            //  = new List<Element>( ids.Select<ElementId,Element>(
            //    id => dbdoc.GetElement( id ) ) );

            Snoop.Forms.Objects form = new Snoop.Forms.Objects(dbdoc, ids);
            ModelessWindowFactory.Show(form);

            return Result.Succeeded;
        }
    }

  /// <summary>
  /// Snoop App command:  Browse all objects that are part of the Application object
  /// </summary>
  [Transaction( TransactionMode.Manual )]
  public class CmdSnoopApp : IExternalCommand
  {
    public Result Execute( ExternalCommandData cmdData, ref string msg, ElementSet elems )
    {
         Snoop.Forms.Objects form = new Snoop.Forms.Objects( cmdData.Application.Application );
         ModelessWindowFactory.Show(form);

         return Result.Succeeded;
    }
  }

  /// <summary>
  /// Snoop ModScope command:  Browse all Elements in the current selection set
  /// </summary>
  [Transaction( TransactionMode.Manual )]
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

  /// <summary>
  /// Search by and Snoop command: Browse
  /// elements found by the condition
  /// </summary>
  [Transaction( TransactionMode.Manual )]
  public class CmdSearchBy : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData cmdData,
      ref string msg,
      ElementSet elems )
    {        
        UIDocument revitDoc = cmdData.Application.ActiveUIDocument;
        Document dbdoc = revitDoc.Document;
        SearchBy form = new SearchBy( dbdoc );
        ModelessWindowFactory.Show(form);

        return Result.Succeeded;
    }
  }
}
