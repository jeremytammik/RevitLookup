#region Header
//
// Copyright 2003-2014 by Autodesk, Inc. 
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

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace RevitLookup.Test.SDKSamples.StructuralSample
{
  /// <summary>
  ///  .Net sample: StructSample.
  /// 
  ///   This command places a set of coulmns in the selected wall.    /// 
  ///   Note that Revit uses Feet as an internal length unit.     
  /// 
  ///   (1) Draw some walls, and constrain their top and bottom to the levels in the properties dialog. 
  ///   (2) Run the test. 
  ///       It will place columns along each wall with the interval of 5 feet. (The interval is also hard coded.) 
  ///   
  /// </summary>

  public class StructSample
  {
    Autodesk.Revit.UI.UIApplication m_app = null;
    //Autodesk.Revit.DB.ElementSet m_ss = null;
    ICollection<ElementId> m_ids = null;

    public StructSample( Autodesk.Revit.UI.UIApplication app, ICollection<ElementId> ids )
    {
      m_app = app;
      //m_ss = ss;
      m_ids = ids;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Boolean ExecuteStructSample()
    {
      Document rvtDoc = m_app.ActiveUIDocument.Document;
      FamilySymbol colType = null;

      List<Wall> walls = new List<Wall>();

      //  iterate through a selection set, and collect walls which are constrained at the top and the bottom.
      //
      foreach( ElementId id in m_ids )
      {
        Element elem = rvtDoc.GetElement( id );

        if( elem.GetType() == typeof( Autodesk.Revit.DB.Wall ) )
        {
          if( elem.get_Parameter( BuiltInParameter.WALL_HEIGHT_TYPE ) != null && elem.get_Parameter( BuiltInParameter.WALL_BASE_CONSTRAINT ) != null )
          {
            walls.Add( elem as Wall );
          }
        }
      }

      //  how many did we get? 
      MessageBox.Show( "Number of constrained walls in the selection set is " + walls.Count );

      if( walls.Count == 0 )
      {
        DialogResult dlgRes = MessageBox.Show( "You must select some walls that are constrained top or bottom", "Structural Sample", MessageBoxButtons.OK, MessageBoxIcon.Information );

        if( dlgRes == System.Windows.Forms.DialogResult.OK )
        {
          return false;
        }
      }

      //  we have some # of walls now.            

      // Load the required family

      FilteredElementCollector collect = new FilteredElementCollector( rvtDoc );
      ElementClassFilter familyAreWanted = new ElementClassFilter( typeof( Family ) );
      collect.WherePasses( familyAreWanted );
      List<Element> elements = collect.ToElements() as List<Element>;

      foreach( Element e in elements )
      {
        Family fam = e as Family;
        if( fam != null )
        {
          if( fam.Name == "M_Wood Timber Column" )
          {
            colType = Utils.FamilyUtil.GetFamilySymbol( fam, "191 x 292mm" );
          }
        }
      }


      String fileName = "../Data/Platform/Metric/Library/Architectural/Columns/M_Wood Timber Column.rfa";
      Family columnFamily;

      if( colType == null )
      {
        m_app.ActiveUIDocument.Document.LoadFamily( fileName, out columnFamily );
        colType = Utils.FamilyUtil.GetFamilySymbol( columnFamily, "191 x 292mm" );
      }

      if( colType == null )
      {
        MessageBox.Show( "Please load the M_Wood Timber Column : 191 x 292mm family" );
        Utils.UserInput.LoadFamily( null, m_app.ActiveUIDocument.Document );
      }

      //
      //  place columns.
      // 
      double spacing = 5;  //  Spacing in feet hard coded. Note: Revit's internal length unit is feet. 

      foreach( Autodesk.Revit.DB.Wall wall in walls )
      {
        FrameWall( m_app.Application, wall, spacing, colType );
      }

      return true;
    }

    /// <summary>
    /// Frame a wall.
    /// </summary>
    /// <param name="rvtApp"></param>
    /// <param name="wall"></param>
    /// <param name="spacing"></param>
    /// <param name="columnType"></param>
    private void
    FrameWall( Autodesk.Revit.ApplicationServices.Application rvtApp, Autodesk.Revit.DB.Wall wall, double spacing, FamilySymbol columnType )
    {
      Document rvtDoc = wall.Document;

      LocationCurve loc = (LocationCurve) wall.Location;
      XYZ startPt = loc.Curve.GetEndPoint( 0 );
      XYZ endPt = loc.Curve.GetEndPoint( 1 );

      UV wallVec = new UV( endPt.X - startPt.X, endPt.Y - startPt.Y );

      UV axis = new UV( 1.0, 0.0 );


      ElementId baseLevelId = wall.get_Parameter( BuiltInParameter.WALL_BASE_CONSTRAINT ).AsElementId();
      ElementId topLevelId = wall.get_Parameter( BuiltInParameter.WALL_HEIGHT_TYPE ).AsElementId();

      double wallLength = VecLength( wallVec );
      wallVec = VecNormalise( wallVec );

      int nmax = (int) ( wallLength / spacing );

      MessageBox.Show( "Wall Length = " + wallLength + "\nSpacing = " + spacing + "\nMax Number = " + nmax, "Structural Sample", MessageBoxButtons.OK, MessageBoxIcon.Information );

      double angle = VecAngle( wallVec, axis );

      XYZ loc2 = startPt;

      double dx = wallVec.U * spacing;
      double dy = wallVec.V * spacing;

      for( int i = 0; i < nmax; i++ )
      {
        PlaceColumn( rvtApp, rvtDoc, loc2, angle, columnType, baseLevelId, topLevelId );
        loc2 = new XYZ( startPt.X + dx, startPt.Y + dy, startPt.Z );
      }

      PlaceColumn( rvtApp, rvtDoc, endPt, angle, columnType, baseLevelId, topLevelId );
    }

    /// <summary>
    /// Create a column instance and place it on the wall line. 
    /// </summary>
    /// <param name="rvtApp"></param>
    /// <param name="rvtDoc"></param>
    /// <param name="point2"></param>
    /// <param name="angle"></param>
    /// <param name="columnType"></param>
    /// <param name="baseLevelId"></param>
    /// <param name="topLevelId"></param>
    private void
    PlaceColumn( Autodesk.Revit.ApplicationServices.Application rvtApp, Document rvtDoc, Autodesk.Revit.DB.XYZ point2, double angle, FamilySymbol columnType, ElementId baseLevelId, ElementId topLevelId )
    {
      Autodesk.Revit.DB.XYZ point = point2;
      // Note: Must use level-hosted NewFamilyInstance!
      Level instLevel = (Level) rvtDoc.GetElement( baseLevelId );
      Autodesk.Revit.DB.FamilyInstance column = rvtDoc.Create.NewFamilyInstance( point, columnType, instLevel, StructuralType.Column );

      if( column == null )
      {
        MessageBox.Show( "failed to create an instance of a column." );
        return;
      }

      Autodesk.Revit.DB.XYZ zVec = new Autodesk.Revit.DB.XYZ( 0, 0, 1 );


      Autodesk.Revit.DB.Line axis = Line.CreateUnbound( point, zVec );

      column.Location.Rotate( axis, angle );

      // Set the level Ids
      Parameter baseLevelParameter = column.get_Parameter( Autodesk.Revit.DB.BuiltInParameter.FAMILY_BASE_LEVEL_PARAM );
      Parameter topLevelParameter = column.get_Parameter( Autodesk.Revit.DB.BuiltInParameter.FAMILY_TOP_LEVEL_PARAM ); ;
      baseLevelParameter.Set( baseLevelId );
      topLevelParameter.Set( topLevelId );

    }

    //
    // helper functions. 
    //

    /// <summary>
    /// 
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    private double
    VecLength( UV v )
    {
      double len = v.U * v.U + v.V * v.V;
      len = Math.Sqrt( len );
      return ( len );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    private UV
    VecNormalise( UV v )
    {
      double len = VecLength( v );
      UV newV = new UV( v.U / len, v.V / len );
      return ( newV );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="va"></param>
    /// <param name="vb"></param>
    /// <returns></returns>
    private double
    VecDot( UV va, UV vb )
    {
      return ( ( va.U * vb.U ) + ( va.V * vb.V ) );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    private double
    ACos( double d )
    {
      //  we probably need to consider tolerance here.
      if( Math.Abs( d ) != 1.0 )
      {
        return ( 1.5707963267949 - Math.Atan( d / Math.Sqrt( 1.0 - d * d ) ) );
      }
      if( d == -1.0 )
      {
        return 3.14159265358979;
      }
      return 0.0;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="va"></param>
    /// <param name="vb"></param>
    /// <returns></returns>
    private double
    VecAngle( UV va, UV vb )
    {
      UV vecA = VecNormalise( va );
      UV vecB = VecNormalise( vb );

      return ( ACos( VecDot( vecA, vecB ) ) );
    }
  }
}