#region Header
//
// Copyright 2003-2017 by Autodesk, Inc. 
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
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit.DB;

namespace RevitLookup.Test.SDKSamples
{

  /// <summary>
  /// These are all the tests taken directly from or heavily inspired by the SDK examples.  The idea
  /// is to consolidate everything in one tool so people don't have to build 4000 different projects
  /// and edit the revit.ini file everytime they want to test one.
  /// </summary>

  public class SDKTestFuncs : RevitLookupTestFuncs
  {
    public SDKTestFuncs( Autodesk.Revit.UI.UIApplication app )
      : base( app )
    {
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Generate Sheet", "Generate a new sheet with a set of selected views", "SDK Samples", new RevitLookupTestFuncInfo.TestFunc( GenerateSheet ), RevitLookupTestFuncInfo.TestType.Create ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Analytical Support Data", "Show Analytical data for selected objects (Must be using Revit Structure)", "SDK Samples", new RevitLookupTestFuncInfo.TestFunc( AnalyticalSupportData ), RevitLookupTestFuncInfo.TestType.Query ) );

      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Create Shared Parameters", "Create new shared parameters", "SDK Samples", new RevitLookupTestFuncInfo.TestFunc( CreateSharedParams ), RevitLookupTestFuncInfo.TestType.Create ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Type Selector", "Change the Type of an Element", "SDK Samples", new RevitLookupTestFuncInfo.TestFunc( TypeSelector ), RevitLookupTestFuncInfo.TestType.Modify ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Move Linear Bound Element", "Move a single Element to a new location", "SDK Samples", new RevitLookupTestFuncInfo.TestFunc( MoveLinearBound ), RevitLookupTestFuncInfo.TestType.Modify ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Structural Sample", "Place a set of columns at equal intervals on wall's", "SDK Samples", new RevitLookupTestFuncInfo.TestFunc( StructSample ), RevitLookupTestFuncInfo.TestType.Create ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Level Properties", "Add, remove and modify levels", "SDK Samples", new RevitLookupTestFuncInfo.TestFunc( LevelProps ), RevitLookupTestFuncInfo.TestType.Query ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Fire Rating - Create Shared Parameter", "Create the \"Fire Rating\" Parameter", "SDK Samples", new RevitLookupTestFuncInfo.TestFunc( FireRatingSharedParam ), RevitLookupTestFuncInfo.TestType.Create ) );


      // SDK Samples not implemented:
      // DeleteObject             (See TestDocument.Delete)
      // DesignOptionsReader      (use Snoop)
      // RevitCommands            (use Snoop)
      // BrowseBindings           (use Snoop)
      // ParameterUtils           (use Snoop)
    }

    public void
    GenerateSheet()
    {
      Document doc = m_revitApp.ActiveUIDocument.Document;
      CreateSheet.Views view = new CreateSheet.Views( doc );

      CreateSheet.AllViewsForm dlg = new CreateSheet.AllViewsForm( view );
      if( dlg.ShowDialog() == DialogResult.OK )
        view.GenerateSheet( doc );
    }

    public void
    AnalyticalSupportData()
    {
      //ElementSet selectedElements = m_revitApp.ActiveUIDocument.Selection.Elements;
      ICollection<ElementId> selectedElementIds = m_revitApp.ActiveUIDocument.Selection.GetElementIds();

      //if (selectedElements.IsEmpty) 

      if( 0 == selectedElementIds.Count )
      {
        MessageBox.Show( "No Elements were selected." );
        return;
      }

      AnalyticalSupportData.Info analysisData = new AnalyticalSupportData.Info(
        m_revitApp.ActiveUIDocument.Document, selectedElementIds );

      if( analysisData.ElementInformation.Rows.Count == 0 )
      {
        MessageBox.Show( "No Elements with Analytical Support Data were selected." );
        return;
      }

      AnalyticalSupportData.InfoForm form = new AnalyticalSupportData.InfoForm( analysisData );
      form.ShowDialog();
    }

    public void CreateSharedParams()
    {
      SharedParams.Create creator = new SharedParams.Create( m_revitApp );
      creator.AddSharedParamsToFile();
      creator.AddSharedParamsToWalls();
    }

    public void
    TypeSelector()
    {
      //ElementSet selectedElements = m_revitApp.ActiveUIDocument.Selection.Elements;

      //if( selectedElements.IsEmpty )
      //{
      //  MessageBox.Show( "No elements were selected." );
      //  return;
      //}

      //TypeSelector.TypeSelectorForm form = new TypeSelector.TypeSelectorForm( m_revitApp, selectedElements );

      ICollection<ElementId> selectedElementIds = m_revitApp.ActiveUIDocument.Selection.GetElementIds();

      if( 0 == selectedElementIds.Count )
      {
        MessageBox.Show( "No elements were selected." );
        return;
      }

      TypeSelector.TypeSelectorForm form = new TypeSelector.TypeSelectorForm( m_revitApp, selectedElementIds );
      form.ShowDialog();
    }

    public void
    MoveLinearBound()
    {
      Document doc = m_revitApp.ActiveUIDocument.Document;

      // TBD: this usually produces a bunch of errors unless you pick one simple element
      // by itself, like a single Wall in isolation.
      int moveCount = 0;
      int rejectedCount = 0;

      //foreach( Element elem in m_revitApp.ActiveUIDocument.Selection.Elements ) // 2015, jeremy: 'Autodesk.Revit.UI.Selection.Selection.Elements' is obsolete: 'This property is deprecated in Revit 2015. Use GetElementIds() and SetElementIds instead.'
      foreach( ElementId id in m_revitApp.ActiveUIDocument.Selection.GetElementIds() ) // 2016, jeremy
      {
        Element elem = doc.GetElement( id );
        Location loc = elem.Location;
        LocationCurve lineLoc = loc as LocationCurve;
        if( lineLoc != null )
        {
          Line line;
          XYZ newStart;
          XYZ newEnd;

          newStart = lineLoc.Curve.GetEndPoint( 0 );
          newEnd = lineLoc.Curve.GetEndPoint( 1 );

          newStart = new XYZ( newStart.X + 100, newStart.Y + 100, newStart.Z );
          newEnd = new XYZ( newEnd.X + 75, newEnd.Y + 75, newEnd.Z );

          line = Line.CreateBound( newStart, newEnd );
          lineLoc.Curve = line;

          moveCount++;
        }
        else
        {
          rejectedCount++;
        }
      }

      string msgStr = string.Format( "Moved {0} elements.  {1} were not Linear-bound.", moveCount, rejectedCount );
      MessageBox.Show( msgStr );
    }

    public void
    StructSample()
    {
      //// first filter out to only Wall elements
      //ElementSet selectedElements = Utils.Selection.FilterToCategory( m_revitApp.ActiveUIDocument.Selection.Elements,
      //                                              BuiltInCategory.OST_Walls, false, m_revitApp.ActiveUIDocument.Document );

      //if( selectedElements.IsEmpty )
      //{
      //  MessageBox.Show( "No wall elements are currently selected" );
      //  return;
      //}

      //StructuralSample.StructSample sample = new StructuralSample.StructSample( m_revitApp, selectedElements );

      // First filter out to only Wall elements
      ICollection<ElementId> selectedElementIds
        = Utils.Selection.FilterToCategory(
          m_revitApp.ActiveUIDocument.Selection.GetElementIds(),
          BuiltInCategory.OST_Walls, false,
          m_revitApp.ActiveUIDocument.Document );

      if( 0 == selectedElementIds.Count )
      {
        MessageBox.Show( "No wall elements are currently selected" );
        return;
      }

      StructuralSample.StructSample sample = new StructuralSample.StructSample( m_revitApp, selectedElementIds );
      sample.ExecuteStructSample();
    }

    public void
    LevelProps()
    {
      LevelProperties.LevelsCommand levCommand = new RevitLookup.Test.SDKSamples.LevelProperties.LevelsCommand( m_revitApp );
      levCommand.Execute();
    }

    public void
    FireRatingSharedParam()
    {
      FireRating.SharedParam creator = new FireRating.SharedParam( m_revitApp );
      creator.AddSharedParamToFile();
    }
  }
}
