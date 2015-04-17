#region Header
//
// Copyright 2003-2015 by Autodesk, Inc. 
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
using System.Collections;
using System.Diagnostics;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Analysis;
using Autodesk.Revit.DB.ExtensibleStorage;

using RevitLookup.Snoop.Collectors;

using ArGe = Autodesk.Revit.DB;
using System.Collections.Generic;

namespace RevitLookup.Snoop.CollectorExts
{
  /// <summary>
  /// Provide Snoop.Data for any classes related to an Element.
  /// </summary>

  public class CollectorExtElement : CollectorExt
  {
    public CollectorExtElement()
    {
    }

    protected override void CollectEvent( object sender, CollectorEventArgs e )
    {
      // cast the sender object to the SnoopCollector we are expecting
      Collector snoopCollector = sender as Collector;
      if( snoopCollector == null )
      {
        Debug.Assert( false );    // why did someone else send us the message?
        return;
      }

      // if its not even an Element, bail early
      Element elem = e.ObjToSnoop as Element;
      if( elem != null )
        Stream( snoopCollector.Data(), elem );
      else
        return;

      // branch to all Element derived classes that we deal with

    }

    private void Stream( ArrayList data, Element elem )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Element ) ) );

      try
      {
        data.Add( new Snoop.Data.String( "Name", elem.Name ) );
      }
      catch( Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Name", ex ) );
      }

      data.Add( new Snoop.Data.Int( "ID", elem.Id.IntegerValue ) );
      data.Add( new Snoop.Data.String( "Unique ID", elem.UniqueId ) );
      data.Add( new Snoop.Data.Object( "Category", elem.Category ) );
      data.Add( new Snoop.Data.ElementId( "Object type", elem.GetTypeId(), elem.Document ) );
      data.Add( new Snoop.Data.Object( "Level", elem.LevelId ) );
      data.Add( new Snoop.Data.Object( "Document", elem.Document ) );
      data.Add( new Snoop.Data.Object( "Location", elem.Location ) );

      try
      {
        data.Add( new Snoop.Data.Enumerable( "Materials", elem.GetMaterialIds( false ), elem.Document ) );
      }
      catch( Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Materials", ex ) );
      }

      data.Add( new Snoop.Data.ParameterSet( "Parameters", elem, elem.Parameters ) );
      data.Add( new Snoop.Data.Enumerable( "Parameters map", elem.ParametersMap ) );
      data.Add( new Snoop.Data.Object( "Design option", elem.DesignOption ) );
      data.Add( new Snoop.Data.ElementId( "Group Id", elem.GroupId, elem.Document ) );
      data.Add( new Snoop.Data.ElementId( "Created phase", elem.CreatedPhaseId, elem.Document ) );
      data.Add( new Snoop.Data.ElementId( "Demolished phase", elem.DemolishedPhaseId, elem.Document ) );

      try
      {
        data.Add( new Snoop.Data.ElementSet( "Similar object types", elem.GetValidTypes(), elem.Document ) );
      }
      catch( Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Similar object types", ex ) );
      }

      data.Add( new Snoop.Data.Bool( "Pinned", elem.Pinned ) );
      data.Add( new Snoop.Data.ElementGeometry( "Geometry", elem, m_app.Application ) );

      data.Add( new Snoop.Data.Object( "Analytical model", elem.GetAnalyticalModel() ) );

      // try to access the extensible storage of this element.
      foreach( Schema schema in Schema.ListSchemas() )
      {
        String objectName = "Entity with Schema [" + schema.SchemaName + "]";
        try
        {
          Entity entity = elem.GetEntity( schema );
          if( !entity.IsValid() )
            continue;
          data.Add( new Snoop.Data.Object( objectName, entity ) );
        }
        catch( System.Exception ex )
        {
          data.Add( new Snoop.Data.Exception( objectName, ex ) );
        }
      }



      // see if it is a type we are responsible for

      Area area = elem as Area;
      if( area != null )
      {
        Stream( data, area );
        return;
      }

      AreaReinforcement areaReinforcement = elem as AreaReinforcement;
      if( areaReinforcement != null )
      {
        Stream( data, areaReinforcement );
        return;
      }

      AreaReinforcementCurve areaReinforcementCurve = elem as AreaReinforcementCurve;
      if( areaReinforcementCurve != null )
      {
        Stream( data, areaReinforcementCurve );
        return;
      }

      AreaTag areaTag = elem as AreaTag;
      if( areaTag != null )
      {
        Stream( data, areaTag );
        return;
      }

      BaseArray baseArray = elem as BaseArray;
      if( baseArray != null )
      {
        Stream( data, baseArray );
        return;
      }

      BasePoint basePoint = elem as BasePoint;
      if( basePoint != null )
      {
        Stream( data, basePoint );
        return;
      }

      BeamSystem beamSys = elem as BeamSystem;
      if( beamSys != null )
      {
        Stream( data, beamSys );
        return;
      }

      BoundaryConditions bndCnd = elem as BoundaryConditions;
      if( bndCnd != null )
      {
        Stream( data, bndCnd );
        return;
      }

      CombinableElement combElem = elem as CombinableElement;
      if( combElem != null )
      {
        Stream( data, combElem );
        return;
      }

      Control ctrl = elem as Control;
      if( ctrl != null )
      {
        Stream( data, ctrl );
        return;
      }

      CurveElement curElem = elem as CurveElement;
      if( curElem != null )
      {
        Stream( data, curElem );
        return;
      }

      DesignOption designOpt = elem as DesignOption;
      if( designOpt != null )
      {
        Stream( data, designOpt );
        return;
      }

      Dimension dim = elem as Dimension;
      if( dim != null )
      {
        Stream( data, dim );
        return;
      }
      //TF
      FabricArea fabricArea = elem as FabricArea;
      if( fabricArea != null )
      {
        Stream( data, fabricArea );
        return;
      }

      MultiReferenceAnnotation mra = elem as MultiReferenceAnnotation;
      if( mra != null )
      {
        Stream( data, mra );
        return;
      }
      //TFEND
      Family family = elem as Family;
      if( family != null )
      {
        Stream( data, family );
        return;
      }

      GenericForm genForm = elem as GenericForm;
      if( genForm != null )
      {
        Stream( data, genForm );
        return;
      }

      Grid grid = elem as Grid;
      if( grid != null )
      {
        Stream( data, grid );
        return;
      }

      Group group = elem as Group;
      if( group != null )
      {
        Stream( data, group );
        return;
      }

      HostObject host = elem as HostObject;
      if( host != null )
      {
        Stream( data, host );
        return;
      }

      IndependentTag indepTag = elem as IndependentTag;
      if( indepTag != null )
      {
        Stream( data, indepTag );
        return;
      }

      Instance inst = elem as Instance;
      if( inst != null )
      {
        Stream( data, inst );
        return;
      }

      Level level = elem as Level;
      if( level != null )
      {
        Stream( data, level );
        return;
      }

      LoadBase loadBase = elem as LoadBase;
      if( loadBase != null )
      {
        Stream( data, loadBase );
        return;
      }

      LoadCase loadCase = elem as LoadCase;
      if( loadCase != null )
      {
        Stream( data, loadCase );
        return;
      }

      LoadCombination loadCombo = elem as LoadCombination;
      if( loadCombo != null )
      {
        Stream( data, loadCombo );
        return;
      }

      LoadNature loadNature = elem as LoadNature;
      if( loadNature != null )
      {
        Stream( data, loadNature );
        return;
      }

      LoadUsage loadUsage = elem as LoadUsage;
      if( loadUsage != null )
      {
        Stream( data, loadUsage );
        return;
      }

      Autodesk.Revit.DB.Material mat = elem as Autodesk.Revit.DB.Material;
      if( mat != null )
      {
        Stream( data, mat );
        return;
      }

      Opening opn = elem as Opening;
      if( opn != null )
      {
        Stream( data, opn );
        return;
      }

      PathReinforcement pathReinf = elem as PathReinforcement;
      if( pathReinf != null )
      {
        Stream( data, pathReinf );
        return;
      }

      Phase phase = elem as Phase;
      if( phase != null )
      {
        Stream( data, phase );
        return;
      }

      PrintSetting printSetting = elem as PrintSetting;
      if( printSetting != null )
      {
        Stream( data, printSetting );
        return;
      }

      ProjectInfo projInfo = elem as ProjectInfo;
      if( projInfo != null )
      {
        Stream( data, projInfo );
        return;
      }

      Rebar rebar = elem as Rebar;
      if( rebar != null )
      {
        Stream( data, rebar );
        return;
      }

      ReferencePlane refPlane = elem as ReferencePlane;
      if( refPlane != null )
      {
        Stream( data, refPlane );
        return;
      }

      ReferencePoint refPoint = elem as ReferencePoint;
      if( refPoint != null )
      {
        Stream( data, refPoint );
        return;
      }

      Room room = elem as Room;
      if( room != null )
      {
        Stream( data, room );
        return;
      }

      RoomTag roomTag = elem as RoomTag;
      if( roomTag != null )
      {
        Stream( data, roomTag );
        return;
      }

      SketchBase sketchBase = elem as SketchBase;
      if( sketchBase != null )
      {
        Stream( data, sketchBase );
        return;
      }

      SketchPlane sketchPlane = elem as SketchPlane;
      if( sketchPlane != null )
      {
        Stream( data, sketchPlane );
        return;
      }

      Space space = elem as Space;
      if( space != null )
      {
        Stream( data, space );
        return;
      }

      SpaceTag spaceTag = elem as SpaceTag;
      if( spaceTag != null )
      {
        Stream( data, spaceTag );
        return;
      }

      TextElement textElem = elem as TextElement;
      if( textElem != null )
      {
        Stream( data, textElem );
        return;
      }

      Truss truss = elem as Truss;
      if( truss != null )
      {
        Stream( data, truss );
        return;
      }

      View view = elem as View;
      if( view != null )
      {
        Stream( data, view );
        return;
      }

      ViewSheetSet viewSheetSet = elem as ViewSheetSet;
      if( viewSheetSet != null )
      {
        Stream( data, viewSheetSet );
        return;
      }

      Zone zone = elem as Zone;
      if( zone != null )
      {
        Stream( data, zone );
        return;
      }

      GraphicsStyle graphStyle = elem as GraphicsStyle;
      if( graphStyle != null )
      {
        Stream( data, graphStyle );
        return;
      }

      ImportInstance impInst = elem as ImportInstance;
      if( impInst != null )
      {
        Stream( data, impInst );
        return;
      }

      ModelText modelTxt = elem as ModelText;
      if( modelTxt != null )
      {
        Stream( data, modelTxt );
        return;
      }

      PropertyLine propLine = elem as PropertyLine;
      if( propLine != null )
      {
        Stream( data, propLine );
        return;
      }

      AreaScheme areaScheme = elem as AreaScheme;
      if( areaScheme != null )
      {
        Stream( data, areaScheme );
        return;
      }

      ConnectorElement connElem = elem as ConnectorElement;
      if( connElem != null )
      {
        Stream( data, connElem );
        return;
      }

      MEPSystem mepSys = elem as MEPSystem;
      if( mepSys != null )
      {
        Stream( data, mepSys );
        return;
      }

      InsulationType insType = elem as InsulationType;
      if( insType != null )
      {
        Stream( data, insType );
        return;
      }


      TemperatureRatingType tempRatingType = elem as TemperatureRatingType;
      if( tempRatingType != null )
      {
        Stream( data, tempRatingType );
        return;
      }

      WireMaterialType wireMatType = elem as WireMaterialType;
      if( wireMatType != null )
      {
        Stream( data, wireMatType );
        return;
      }

      AnalyticalLink link = elem as AnalyticalLink;
      if( link != null )
      {
        Stream( data, link );
        return;
      }
    }

    private void Stream( ArrayList data, Area area )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Area ) ) );
      data.Add( new Snoop.Data.Enumerable( "Boundary", area.GetBoundarySegments( new SpatialElementBoundaryOptions() ) ) );
      data.Add( new Snoop.Data.String( "Number", area.Number ) );
    }

    private void Stream( ArrayList data, AreaTag areaTag )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( AreaTag ) ) );

      data.Add( new Snoop.Data.Object( "Area", areaTag.Area ) );
      data.Add( new Snoop.Data.Object( "Area tag type", areaTag.AreaTagType ) );
      data.Add( new Snoop.Data.Bool( "Leader", areaTag.HasLeader ) );
      data.Add( new Snoop.Data.Object( "View", areaTag.View ) );
    }

    private void Stream( ArrayList data, Autodesk.Revit.DB.Mechanical.SpaceTag spaceTag )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Autodesk.Revit.DB.Mechanical.SpaceTag ) ) );

      data.Add( new Snoop.Data.Object( "Space", spaceTag.Space ) );
      data.Add( new Snoop.Data.Object( "Space tag type", spaceTag.SpaceTagType ) );
      data.Add( new Snoop.Data.Bool( "Leader", spaceTag.HasLeader ) );
      data.Add( new Snoop.Data.Object( "View", spaceTag.View ) );
    }


    private void Stream( ArrayList data, Level level )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Level ) ) );

      data.Add( new Snoop.Data.Double( "Elevation", level.Elevation ) );
      data.Add( new Snoop.Data.Double( "Project elevation", level.ProjectElevation ) );
      data.Add( new Snoop.Data.Object( "Level type", level.LevelType ) );
    }

    private void Stream( ArrayList data, HostObject hostObj )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( HostObject ) ) );

      Wall wall = hostObj as Wall;
      if( wall != null )
      {
        Stream( data, wall );
        return;
      }

      CeilingAndFloor ceilFloor = hostObj as CeilingAndFloor;
      if( ceilFloor != null )
      {
        Stream( data, ceilFloor );
        return;
      }

      ContFooting contFooting = hostObj as ContFooting;
      if( contFooting != null )
      {
        Stream( data, contFooting );
        return;
      }

      CurtainGridLine gridLine = hostObj as CurtainGridLine;
      if( gridLine != null )
      {
        Stream( data, gridLine );
        return;
      }

      CurtainSystemBase curtainSysBase = hostObj as CurtainSystemBase;
      if( curtainSysBase != null )
      {
        Stream( data, curtainSysBase );
        return;
      }

      HostedSweep hostedSweep = hostObj as HostedSweep;
      if( hostedSweep != null )
      {
        Stream( data, hostedSweep );
        return;
      }

      RoofBase roofBase = hostObj as RoofBase;
      if( roofBase != null )
      {
        Stream( data, roofBase );
        return;
      }

      MEPCurve mepCur = hostObj as MEPCurve;
      if( mepCur != null )
      {
        Stream( data, mepCur );
        return;
      }

      Wire wire = hostObj as Wire;
      if( wire != null )
      {
        Stream( data, wire );
        return;
      }
    }

    private void Stream( ArrayList data, Wall wall )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Wall ) ) );

      data.Add( new Snoop.Data.Double( "Width", wall.Width ) );
      data.Add( new Snoop.Data.Object( "Wall type", wall.WallType ) );
      data.Add( new Snoop.Data.Bool( "Flipped", wall.Flipped ) );

      /// uncomment this to see the length of the wall
      //data.Add(new Snoop.Data.Double("Length", wall.get_Parameter( Autodesk.Revit.Parameters.BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble()));

      // TBD: Curtain Wall throws an exception when the Orientation property is accessed
      // Exception of type 'System.Exception' is thrown.
      //if (wall.WallType.Kind != WallType.WallKind.Curtain)

      try
      {
        data.Add( new Snoop.Data.Xyz( "Orientation", wall.Orientation ) );
      }
      catch( Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Orientation", ex ) );
      }

      data.Add( new Snoop.Data.Object( "Curtain grid", wall.CurtainGrid ) );
      data.Add( new Snoop.Data.String( "Structural usage", wall.StructuralUsage.ToString() ) );

      // TBD: Crash-Assert if called on Non-Bearing wall (seems a little harsh)
      //if (wall.StructuralUsage != WallUsage.NonBearing)
      data.Add( new Snoop.Data.Object( "Anaylytical model", wall.GetAnalyticalModel() ) );
    }

    private void Stream( ArrayList data, CeilingAndFloor ceilFloor )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( CeilingAndFloor ) ) );

      // Nothing at this level yet!

      Floor floor = ceilFloor as Floor;
      if( floor != null )
      {
        Stream( data, floor );
        return;
      }
    }

    private void Stream( ArrayList data, ContFooting contFooting )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( ContFooting ) ) );

      data.Add( new Snoop.Data.Object( "Analytical model", contFooting.GetAnalyticalModel() ) );
      data.Add( new Snoop.Data.Object( "Footing type", contFooting.FootingType ) );
    }

    private void Stream( ArrayList data, Floor floor )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Floor ) ) );

      data.Add( new Snoop.Data.Object( "Floor type", floor.FloorType ) );
      data.Add( new Snoop.Data.Object( "Analytical model", floor.GetAnalyticalModel() ) );
      data.Add( new Snoop.Data.String( "Structural usage", floor.StructuralUsage.ToString() ) );
      data.Add( new Snoop.Data.Enumerable( "Span direction symbols", floor.GetSpanDirectionSymbolIds(), floor.Document ) );

      // Works only for Revit Structure
      if( floor.GetAnalyticalModel() != null )
      {
        data.Add( new Snoop.Data.Angle( "Span direction angle", floor.SpanDirectionAngle ) );
      }

      data.Add( new Snoop.Data.Object( "Slab shape editor", floor.SlabShapeEditor ) );
    }

    private void Stream( ArrayList data, CurtainGridLine gridLine )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( CurtainGridLine ) ) );

      data.Add( new Snoop.Data.Enumerable( "All segment curves", gridLine.AllSegmentCurves ) );
      data.Add( new Snoop.Data.Enumerable( "Existing segment curves", gridLine.ExistingSegmentCurves ) );
      data.Add( new Snoop.Data.Enumerable( "Skipped segment curves", gridLine.SkippedSegmentCurves ) );
      data.Add( new Snoop.Data.Object( "Full curve", gridLine.FullCurve ) );
      data.Add( new Snoop.Data.Bool( "Is U grid line", gridLine.IsUGridLine ) );
      data.Add( new Snoop.Data.Bool( "Lock", gridLine.Lock ) );
    }

    private void Stream( ArrayList data, CurtainSystemBase curtainSysBase )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( CurtainSystemBase ) ) );

      CurtainSystem curtainSys = curtainSysBase as CurtainSystem;
      if( curtainSys != null )
      {
        Stream( data, curtainSys );
        return;
      }
    }

    private void Stream( ArrayList data, CurtainSystem curtainSys )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( CurtainSystem ) ) );

      data.Add( new Snoop.Data.Enumerable( "Curtain grids", curtainSys.CurtainGrids ) );
      data.Add( new Snoop.Data.Object( "Curtain system type", curtainSys.CurtainSystemType ) );
    }

    private void Stream( ArrayList data, HostedSweep hostedSweep )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( HostedSweep ) ) );

      data.Add( new Snoop.Data.Angle( "Angle", hostedSweep.Angle ) );
      data.Add( new Snoop.Data.Bool( "Horizontal flip", hostedSweep.HorizontalFlipped ) );
      data.Add( new Snoop.Data.Double( "Horizontal offset", hostedSweep.HorizontalOffset ) );
      data.Add( new Snoop.Data.Bool( "Vertical flip", hostedSweep.VerticalFlipped ) );
      data.Add( new Snoop.Data.Double( "Vertical offset", hostedSweep.VerticalOffset ) );
      data.Add( new Snoop.Data.Double( "Length", hostedSweep.Length ) );

      Fascia fascia = hostedSweep as Fascia;
      if( fascia != null )
      {
        Stream( data, fascia );
        return;
      }

      Gutter gutter = hostedSweep as Gutter;
      if( gutter != null )
      {
        Stream( data, gutter );
        return;
      }

      SlabEdge slabEdge = hostedSweep as SlabEdge;
      if( slabEdge != null )
      {
        Stream( data, slabEdge );
        return;
      }
    }

    private void Stream( ArrayList data, Fascia fascia )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Fascia ) ) );

      data.Add( new Snoop.Data.Object( "Fascia type", fascia.FasciaType ) );
    }

    private void Stream( ArrayList data, Gutter gutter )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Gutter ) ) );

      data.Add( new Snoop.Data.Object( "Gutter type", gutter.GutterType ) );
    }

    private void Stream( ArrayList data, SlabEdge slabEdge )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( SlabEdge ) ) );

      data.Add( new Snoop.Data.Object( "Slab edge type", slabEdge.SlabEdgeType ) );
    }

    private void Stream( ArrayList data, RoofBase roofBase )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( RoofBase ) ) );

      data.Add( new Snoop.Data.String( "Eave cuts", roofBase.EaveCuts.ToString() ) );

      try
      {
        data.Add( new Snoop.Data.Double( "Fascia depth", roofBase.FasciaDepth ) );
      }
      catch( Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Fascia depth", ex ) );
      }

      data.Add( new Snoop.Data.Object( "Roof type", roofBase.RoofType ) );
      data.Add( new Snoop.Data.Object( "Slab shape editor", roofBase.SlabShapeEditor ) );

      FootPrintRoof footPrintRoof = roofBase as FootPrintRoof;
      if( footPrintRoof != null )
      {
        Stream( data, footPrintRoof );
        return;
      }

      ExtrusionRoof extrRoof = roofBase as ExtrusionRoof;
      if( extrRoof != null )
      {
        Stream( data, extrRoof );
        return;
      }
    }

    private void Stream( ArrayList data, MEPCurve mepCur )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( MEPCurve ) ) );

      data.Add( new Snoop.Data.Object( "Connector manager", mepCur.ConnectorManager ) );

      try
      {
        data.Add( new Snoop.Data.Double( "Diameter", mepCur.Diameter ) );
      }
      catch( Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Diameter", ex ) );
      }

      try
      {
        data.Add( new Snoop.Data.Double( "Height", mepCur.Height ) );
      }
      catch( Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Height", ex ) );
      }

      try
      {
        data.Add( new Snoop.Data.Double( "Width", mepCur.Width ) );
      }
      catch( Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Width", ex ) );
      }
	  try
	  {
		  data.Add(new Snoop.Data.Double("Level offset", mepCur.LevelOffset));
	  }
	  catch (Exception ex)
	  {
		  data.Add(new Snoop.Data.Exception("Level offset", ex));
	  }
	  try
	  {
		  data.Add(new Snoop.Data.Object("MEP system", mepCur.MEPSystem));
	  }
	  catch (Exception ex)
	  {
		  data.Add(new Snoop.Data.Exception("Level offset", ex));
	  }
      data.Add( new Snoop.Data.Object( "Reference level", mepCur.ReferenceLevel ) );

      Duct duct = mepCur as Duct;
      if( duct != null )
      {
        Stream( data, duct );
        return;
      }

      FlexDuct flexDuct = mepCur as FlexDuct;
      if( flexDuct != null )
      {
        Stream( data, flexDuct );
        return;
      }

      Pipe pipe = mepCur as Pipe;
      if( pipe != null )
      {
        Stream( data, pipe );
        return;
      }

      FlexPipe flexPipe = mepCur as FlexPipe;
      if( flexPipe != null )
      {
        Stream( data, flexPipe );
        return;
      }
    }

    private void Stream( ArrayList data, Duct duct )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Duct ) ) );

      data.Add( new Snoop.Data.Object( "Duct type", duct.DuctType ) );
    }

    private void Stream( ArrayList data, FlexDuct flexDuct )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( FlexDuct ) ) );

      data.Add( new Snoop.Data.Object( "Flex duct type", flexDuct.FlexDuctType ) );
      data.Add( new Snoop.Data.Enumerable( "Points", flexDuct.Points ) );
    }

    private void Stream( ArrayList data, Pipe pipe )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Pipe ) ) );

      data.Add( new Snoop.Data.String( "Flow state", pipe.FlowState.ToString() ) );
      data.Add( new Snoop.Data.Object( "Pipe type", pipe.PipeType ) );
    }

    private void Stream( ArrayList data, FlexPipe flexPipe )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( FlexPipe ) ) );

      data.Add( new Snoop.Data.Object( "Flex pipe type", flexPipe.FlexPipeType ) );
      data.Add( new Snoop.Data.String( "Flow state", flexPipe.FlowState.ToString() ) );
      data.Add( new Snoop.Data.Enumerable( "Points", flexPipe.Points ) );
    }

    private void Stream( ArrayList data, Wire wire )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Wire ) ) );

      data.Add( new Snoop.Data.Object( "Flex pipe type", wire.ConnectorManager ) );
      data.Add( new Snoop.Data.Int( "Ground conductor num", wire.GroundConductorNum ) );
      data.Add( new Snoop.Data.Int( "Hot conductor num", wire.HotConductorNum ) );
      data.Add( new Snoop.Data.Int( "Neutral conductor num", wire.NeutralConductorNum ) );
      data.Add( new Snoop.Data.String( "Wiring type", wire.WiringType.ToString() ) );
    }

    private void Stream( ArrayList data, FootPrintRoof footPrintRoof )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( FootPrintRoof ) ) );

      data.Add( new Snoop.Data.Enumerable( "Curtain grids", footPrintRoof.CurtainGrids ) );

      // TBD: how to display Profiles and other functions?
    }

    private void Stream( ArrayList data, ExtrusionRoof extrusionRoof )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( ExtrusionRoof ) ) );

      data.Add( new Snoop.Data.Enumerable( "Curtain grids", extrusionRoof.CurtainGrids ) );

      // TBD: how to display Profiles and other functions?
    }

    private void Stream( ArrayList data, Family fam )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Family ) ) );

      Category category = fam.FamilyCategory;
      data.Add( new Snoop.Data.Object( "Family category", category ) );

      //List<FamilySymbol> symbols = new List<FamilySymbol>();
      //foreach (ElementId id in fam.GetFamilySymbolIds())
      //{
      //  symbols.Add( fam.Document.GetElement( id ) as FamilySymbol );
      //}

      //data.Add( new Snoop.Data.Enumerable( "Symbols", fam.Symbols ) ); // 2015, jeremy: 'Autodesk.Revit.DB.Family.Symbols' is obsolete: 'This property is obsolete in Revit 2015.  Use Family.GetFamilySymbolIds() instead.'
      data.Add( new Snoop.Data.Enumerable( "Symbols", fam.GetFamilySymbolIds() ) ); // 2016, jeremy

      data.Add( new Snoop.Data.Bool( "Is InPlace", fam.IsInPlace ) );
      data.Add( new Snoop.Data.Bool( "Is CurtainPanelFamily", fam.IsCurtainPanelFamily ) );

      if( fam.IsInPlace == false )
      {
        try
        {
          data.Add( new Snoop.Data.Object( "Family Document", fam.Document.EditFamily( fam ) ) );
        }
        catch( Exception ex )
        {
          data.Add( new Snoop.Data.Exception( "FamilyDocument", new Exception( ex.Message, ex ) ) );
        }

      }

      if( fam.IsCurtainPanelFamily )
      {
        data.Add( new Snoop.Data.Double( "Curtain Panel Horiz Spacing", fam.CurtainPanelHorizontalSpacing ) );
        data.Add( new Snoop.Data.Double( "Curtain Panel Vert Spacing", fam.CurtainPanelVerticalSpacing ) );
        data.Add( new Snoop.Data.String( "Curtain Panel Tile Spacing", fam.CurtainPanelTilePattern.ToString() ) );
      }

      // data.Add(new Snoop.Data.Enumerable("Components", fam.Components));
      // data.Add(new Snoop.Data.Enumerable("Loaded symbols", fam.LoadedSymbols));			
    }

    private void Stream( ArrayList data, Instance insInst )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Instance ) ) );

      data.Add( new Snoop.Data.Object( "Total Transform", insInst.GetTotalTransform() ) );
      data.Add( new Snoop.Data.Object( "Transform", insInst.GetTransform() ) );

      FamilyInstance famInst = insInst as FamilyInstance;
      if( famInst != null )
      {
        Stream( data, famInst );
        return;
      }

      RevitLinkInstance linkInst = insInst as RevitLinkInstance;
      if( linkInst != null )
      {
        Stream( data, linkInst );
        return;
      }
    }

    private void Stream( ArrayList data, FamilyInstance famInst )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( FamilyInstance ) ) );

      data.Add( new Snoop.Data.Object( "Host", famInst.Host ) );
      data.Add( new Snoop.Data.Object( "Symbol", famInst.Symbol ) );

      data.Add( new Snoop.Data.OriginalInstanceGeometry( "Original Geometry", famInst, m_app.Application ) );

      try
      {
        data.Add( new Snoop.Data.Enumerable( "Sub components", famInst.GetSubComponentIds(), famInst.Document ) );
      }
      catch( Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Sub components", ex ) );
      }

      try
      {
        data.Add( new Snoop.Data.Object( "Super component", famInst.SuperComponent ) );
      }
      catch( Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Super component", ex ) );
      }

      try
      {
        data.Add( new Snoop.Data.Object( "Room", famInst.Room ) );
      }
      catch( Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Room", ex ) );
      }

      try
      {
        data.Add( new Snoop.Data.Object( "From room", famInst.FromRoom ) );
      }
      catch( Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "From room", ex ) );
      }

      try
      {
        data.Add( new Snoop.Data.Object( "To room", famInst.ToRoom ) );
      }
      catch( Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "To room", ex ) );
      }

      data.Add( new Snoop.Data.Enumerable( "Material", famInst.GetMaterialIds( false ), famInst.Document ) );
      try
      {
        data.Add( new Snoop.Data.Object( "Space", famInst.Space ) );
      }
      catch( Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Space", ex ) );
      }
      FilteredElementCollector filter = new FilteredElementCollector( famInst.Document );
      IList<Element> phases = filter.OfClass( typeof( Phase ) ).ToElements();
      foreach( Element e in phases )
      {
        try
        {
          data.Add( new Snoop.Data.Object( "get_Space(" + e.Name + ")", famInst.get_Space( e as Phase ) ) );
        }
        catch( Exception ex )
        {
          data.Add( new Snoop.Data.Exception( "get_Space(" + e.Name + ")", ex ) );
        }
      }
      data.Add( new Snoop.Data.String( "Structural type", famInst.StructuralType.ToString() ) );

      // TBD: throws an exception if not the right type!
      try
      {
        data.Add( new Snoop.Data.String( "Structural usage", famInst.StructuralUsage.ToString() ) );
      }
      catch( Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Structural usage", ex ) );
      }

      data.Add( new Snoop.Data.String( "StructuralMaterialType", famInst.StructuralMaterialType.ToString() ) );
      data.Add( new Snoop.Data.ElementId( "StructuralMaterialId", famInst.StructuralMaterialId, famInst.Document ) );

      data.Add( new Snoop.Data.Object( "MEP model", famInst.MEPModel ) );

      data.Add( new Snoop.Data.Bool( "Can flip facing", famInst.CanFlipFacing ) );
      data.Add( new Snoop.Data.Bool( "Facing flipped", famInst.FacingFlipped ) );
      data.Add( new Snoop.Data.Bool( "Can flip hand", famInst.CanFlipHand ) );
      data.Add( new Snoop.Data.Bool( "Hand flipped", famInst.HandFlipped ) );
      data.Add( new Snoop.Data.Bool( "Can rotate", famInst.CanRotate ) );
      data.Add( new Snoop.Data.Xyz( "Facing orientation", famInst.FacingOrientation ) );
      data.Add( new Snoop.Data.Xyz( "Hand orientation", famInst.HandOrientation ) );
      data.Add( new Snoop.Data.Object( "Location", famInst.Location ) );
      data.Add( new Snoop.Data.Bool( "Mirrored", famInst.Mirrored ) );
      data.Add( new Snoop.Data.Enumerable( "Copings", famInst.GetCopingIds(), famInst.Document ) );

      AnnotationSymbol annoSym = famInst as AnnotationSymbol;
      if( annoSym != null )
      {
        Stream( data, annoSym );
        return;
      }

      Panel panel = famInst as Panel;
      if( panel != null )
      {
        Stream( data, panel );
        return;
      }

      Mullion mullion = famInst as Mullion;
      if( mullion != null )
      {
        Stream( data, mullion );
        return;
      }
    }

    private void Stream( ArrayList data, RevitLinkInstance linkInst )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( RevitLinkInstance ) ) );

      data.Add( new Snoop.Data.Object( "Link Document", linkInst.GetLinkDocument() ) );
    }

    private void Stream( ArrayList data, LoadBase loadbase )
    {

      // Works only for Revit Structure!!

      data.Add( new Snoop.Data.ClassSeparator( typeof( LoadBase ) ) );

      data.Add( new Snoop.Data.Object( "Host element", loadbase.HostElement ) );
      data.Add( new Snoop.Data.String( "Load case name", loadbase.LoadCaseName ) );
      data.Add( new Snoop.Data.String( "Load category name", loadbase.LoadCategoryName ) );
      data.Add( new Snoop.Data.String( "Load nature name", loadbase.LoadNatureName ) );

      AreaLoad areaload = loadbase as AreaLoad;
      if( areaload != null )
      {
        Stream( data, areaload );
        return;
      }

      LineLoad lineload = loadbase as LineLoad;
      if( lineload != null )
      {
        Stream( data, lineload );
        return;
      }

      PointLoad pointload = loadbase as PointLoad;
      if( pointload != null )
      {
        Stream( data, pointload );
        return;
      }
    }

    private void
    Stream( ArrayList data, AreaLoad areaload )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( AreaLoad ) ) );

      data.Add( new Snoop.Data.Xyz( "Force 1", areaload.Force1 ) );
      data.Add( new Snoop.Data.Xyz( "Force 2", areaload.Force2 ) );
      data.Add( new Snoop.Data.Xyz( "Force 3", areaload.Force3 ) );

      data.Add( new Snoop.Data.CategorySeparator( "Loops" ) );
      data.Add( new Snoop.Data.Int( "Number of loops", areaload.NumLoops ) );
      for( int i = 0; i < areaload.NumLoops; i++ )
      {
        for( int j = 0; j < areaload.get_NumCurves( i ); j++ )
          data.Add( new Snoop.Data.Object( string.Format( "Loop [{0:d}], Curve [{1:d}]", i, j ), areaload.get_Curve( i, j ) ) );
      }

      data.Add( new Snoop.Data.CategorySeparator( "Reference Points" ) );
      data.Add( new Snoop.Data.Int( "Number of reference points", areaload.NumRefPoints ) );
      for( int i = 0; i < areaload.NumRefPoints; i++ )
      {
        data.Add( new Snoop.Data.Xyz( string.Format( "Reference PT [{0:d}]", i ), areaload.get_RefPoint( i ) ) );
      }
    }

    private void Stream( ArrayList data, LineLoad lineload )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( LineLoad ) ) );

      data.Add( new Snoop.Data.Bool( "Projected load", lineload.ProjectedLoad ) );
      data.Add( new Snoop.Data.Bool( "Uniform load", lineload.UniformLoad ) );
      data.Add( new Snoop.Data.Xyz( "Start point", lineload.StartPoint ) );
      data.Add( new Snoop.Data.Xyz( "End point", lineload.EndPoint ) );
      data.Add( new Snoop.Data.Xyz( "Force 1", lineload.Force1 ) );
      data.Add( new Snoop.Data.Xyz( "Force 2", lineload.Force2 ) );
      data.Add( new Snoop.Data.Xyz( "Moment 1", lineload.Moment1 ) );
      data.Add( new Snoop.Data.Xyz( "Moment 2", lineload.Moment2 ) );
    }

    private void Stream( ArrayList data, PointLoad pointload )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( PointLoad ) ) );

      data.Add( new Snoop.Data.Xyz( "Point", pointload.Point ) );
      data.Add( new Snoop.Data.Xyz( "Force", pointload.Force ) );
      data.Add( new Snoop.Data.Xyz( "Moment", pointload.Moment ) );
    }


    private void Stream( ArrayList data, LoadCase loadcase )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( LoadCase ) ) );

      // Nothing at this level yet!
    }

    private void Stream( ArrayList data, LoadCombination loadcombo )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( LoadCombination ) ) );

      data.Add( new Snoop.Data.String( "Name", loadcombo.Name ) );
      data.Add( new Snoop.Data.String( "Combination type", loadcombo.CombinationType ) );
      data.Add( new Snoop.Data.Int( "Combination type index", loadcombo.CombinationTypeIndex ) );
      data.Add( new Snoop.Data.String( "Combination state", loadcombo.CombinationState ) );
      data.Add( new Snoop.Data.Int( "Combination state index", loadcombo.CombinationStateIndex ) );

      data.Add( new Snoop.Data.CategorySeparator( "Components" ) );
      data.Add( new Snoop.Data.Int( "Number of components", loadcombo.NumberOfComponents ) );

      for( int i = 0; i < loadcombo.NumberOfComponents; i++ )
      {
        data.Add( new Snoop.Data.String( string.Format( "Combination case name [{0:d}]", i ), loadcombo.get_CombinationCaseName( i ) ) );
        data.Add( new Snoop.Data.String( string.Format( "Combination nature name [{0:d}]", i ), loadcombo.get_CombinationNatureName( i ) ) );
        data.Add( new Snoop.Data.Double( string.Format( "Factor [{0:d}]", i ), loadcombo.get_Factor( i ) ) );
      }

      data.Add( new Snoop.Data.CategorySeparator( "Usages" ) );
      data.Add( new Snoop.Data.Int( "Number of usages", loadcombo.NumberOfUsages ) );

      for( int i = 0; i < loadcombo.NumberOfUsages; i++ )
      {
        data.Add( new Snoop.Data.String( string.Format( "Usage name [{0:d}]", i ), loadcombo.get_UsageName( i ) ) );
      }
    }

    private void Stream( ArrayList data, LoadNature loadNature )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( LoadNature ) ) );

      // Nothing at this level yet!
    }

    private void Stream( ArrayList data, LoadUsage loadUsage )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( LoadUsage ) ) );

      // Nothing at this level yet!
    }

    private void Stream( ArrayList data, DesignOption designOpt )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( DesignOption ) ) );

      // Nothing at this level yet!
    }

    private void Stream( ArrayList data, Grid grid )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Grid ) ) );

      data.Add( new Snoop.Data.Bool( "Is curved", grid.IsCurved ) );
      data.Add( new Snoop.Data.Object( "Curve", grid.Curve ) );
      data.Add( new Snoop.Data.Object( "Grid type", grid.GridType ) );
    }

    private void Stream( ArrayList data, Group group )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Group ) ) );

      data.Add( new Snoop.Data.Object( "Group type", group.GroupType ) );
      data.Add( new Snoop.Data.Enumerable( "Members", group.GetMemberIds(), group.Document ) );
    }

    private void Stream( ArrayList data, Autodesk.Revit.DB.Material mat )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Autodesk.Revit.DB.Material ) ) );

      {
        ElementId aspectId = mat.StructuralAssetId;
        PropertySetElement aspect = null;
        try
        {
          aspect = mat.Document.GetElement( aspectId ) as PropertySetElement;
        }
        catch( System.Exception )
        {
          aspect = null;
        }

        data.Add( new Snoop.Data.Object( "Structural Asset", aspect != null ? aspect.GetStructuralAsset() : null ) );
        data.Add( new Snoop.Data.ElementId( "Structural aspect", aspectId, mat.Document ) );
      }

      data.Add( new Snoop.Data.Object( "Color", mat.Color ) );
      data.Add( new Snoop.Data.ElementId( "Cut pattern", mat.CutPatternId, mat.Document ) );
      data.Add( new Snoop.Data.Object( "Cut pattern color", mat.CutPatternColor ) );
      data.Add( new Snoop.Data.ElementId( "Surface pattern", mat.SurfacePatternId, mat.Document ) );
      data.Add( new Snoop.Data.Object( "Surface pattern color", mat.SurfacePatternColor ) );
      data.Add( new Snoop.Data.Bool( "Glow", mat.Glow ) );

      try
      {
        AppearanceAssetElement aae = m_app.ActiveUIDocument.Document.GetElement( mat.AppearanceAssetId ) as AppearanceAssetElement;
        if( null != aae )
        {
          data.Add( new Snoop.Data.Object( "Render appearance", aae.GetRenderingAsset() ) );
        }

      }
      catch( Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Render appearance", ex ) );
      }

      data.Add( new Snoop.Data.Int( "Shininess", mat.Shininess ) );
      data.Add( new Snoop.Data.Double( "Smoothness", mat.Smoothness ) );
      data.Add( new Snoop.Data.Double( "Transparency", mat.Transparency ) );

      //MaterialConcrete matConcrete = mat as MaterialConcrete;
      //if (matConcrete != null)
      //{
      //   Stream(data, matConcrete);
      //   return;
      //}

      //MaterialGeneric matGeneric = mat as MaterialGeneric;
      //if (matGeneric != null)
      //{
      //   Stream(data, matGeneric);
      //   return;
      //}

      //MaterialOther matOther = mat as MaterialOther;
      //if (matOther != null)
      //{
      //   Stream(data, matOther);
      //   return;
      //}

      //MaterialSteel matSteel = mat as MaterialSteel;
      //if (matSteel != null)
      //{
      //   Stream(data, matSteel);
      //   return;
      //}

      //MaterialWood matWood = mat as MaterialWood;
      //if (matWood != null)
      //{
      //   Stream(data, matWood);
      //   return;
      //}
    }

    //private void Stream(ArrayList data, MaterialConcrete mat)
    //{
    //   data.Add(new Snoop.Data.ClassSeparator(typeof(MaterialConcrete)));

    //   data.Add(new Snoop.Data.String("Behavior", mat.Behavior.ToString()));
    //   data.Add(new Snoop.Data.Double("Concrete compression", mat.ConcreteCompression));
    //   data.Add(new Snoop.Data.Double("Damping ratio", mat.DampingRatio));
    //   data.Add(new Snoop.Data.Bool("Light weight", mat.LightWeight));
    //   data.Add(new Snoop.Data.Double("Poisson modulus X", mat.PoissonModulusX));
    //   data.Add(new Snoop.Data.Double("Poisson modulus Y", mat.PoissonModulusY));
    //   data.Add(new Snoop.Data.Double("Poisson modulus Z", mat.PoissonModulusZ));
    //   data.Add(new Snoop.Data.Double("Shear modulus X", mat.ShearModulusX));
    //   data.Add(new Snoop.Data.Double("Shear modulus Y", mat.ShearModulusY));
    //   data.Add(new Snoop.Data.Double("Shear modulus Z", mat.ShearModulusZ));
    //   data.Add(new Snoop.Data.Double("Shear strength reduction", mat.ShearStrengthReduction));
    //   data.Add(new Snoop.Data.Double("Thermal expansion coefficient X", mat.ThermalExpansionCoefficientX));
    //   data.Add(new Snoop.Data.Double("Thermal expansion coefficient Y", mat.ThermalExpansionCoefficientY));
    //   data.Add(new Snoop.Data.Double("Thermal expansion coefficient Z", mat.ThermalExpansionCoefficientZ));
    //   data.Add(new Snoop.Data.Double("Unit weight", mat.UnitWeight));
    //   data.Add(new Snoop.Data.Double("Young modulus X", mat.YoungModulusX));
    //   data.Add(new Snoop.Data.Double("Young modulus Y", mat.YoungModulusY));
    //   data.Add(new Snoop.Data.Double("Young modulus Z", mat.YoungModulusZ));
    //}

    //private void Stream(ArrayList data, MaterialGeneric mat)
    //{
    //   data.Add(new Snoop.Data.ClassSeparator(typeof(MaterialGeneric)));

    //   data.Add(new Snoop.Data.String("Behavior", mat.Behavior.ToString()));
    //   data.Add(new Snoop.Data.Double("Damping ratio", mat.DampingRatio));
    //   data.Add(new Snoop.Data.Double("Minimum yield stress", mat.MinimumYieldStress));
    //   data.Add(new Snoop.Data.Double("Poisson modulus X", mat.PoissonModulusX));
    //   data.Add(new Snoop.Data.Double("Poisson modulus Y", mat.PoissonModulusY));
    //   data.Add(new Snoop.Data.Double("Poisson modulus Z", mat.PoissonModulusZ));
    //   data.Add(new Snoop.Data.Double("Reduction factor", mat.ReductionFactor));
    //   data.Add(new Snoop.Data.Double("Shear modulus X", mat.ShearModulusX));
    //   data.Add(new Snoop.Data.Double("Shear modulus Y", mat.ShearModulusY));
    //   data.Add(new Snoop.Data.Double("Shear modulus Z", mat.ShearModulusZ));
    //   data.Add(new Snoop.Data.Double("Thermal expansion coefficient X", mat.ThermalExpansionCoefficientX));
    //   data.Add(new Snoop.Data.Double("Thermal expansion coefficient Y", mat.ThermalExpansionCoefficientY));
    //   data.Add(new Snoop.Data.Double("Thermal expansion coefficient Z", mat.ThermalExpansionCoefficientZ));
    //   data.Add(new Snoop.Data.Double("Unit weight", mat.UnitWeight));
    //   data.Add(new Snoop.Data.Double("Young modulus X", mat.YoungModulusX));
    //   data.Add(new Snoop.Data.Double("Young modulus Y", mat.YoungModulusY));
    //   data.Add(new Snoop.Data.Double("Young modulus Z", mat.YoungModulusZ));
    //}

    //private void Stream(ArrayList data, MaterialOther mat)
    //{
    //   data.Add(new Snoop.Data.ClassSeparator(typeof(MaterialOther)));

    //   // Nothing at this level yet!
    //}

    //private void Stream(ArrayList data, MaterialSteel mat)
    //{
    //   data.Add(new Snoop.Data.ClassSeparator(typeof(MaterialSteel)));

    //   data.Add(new Snoop.Data.String("Behavior", mat.Behavior.ToString()));
    //   data.Add(new Snoop.Data.Double("Damping ratio", mat.DampingRatio));
    //   data.Add(new Snoop.Data.Double("Minimum tensile stress", mat.MinimumTensileStrength));
    //   data.Add(new Snoop.Data.Double("Minimum yield stress", mat.MinimumYieldStress));
    //   data.Add(new Snoop.Data.Double("Poisson modulus X", mat.PoissonModulusX));
    //   data.Add(new Snoop.Data.Double("Poisson modulus Y", mat.PoissonModulusY));
    //   data.Add(new Snoop.Data.Double("Poisson modulus Z", mat.PoissonModulusZ));
    //   data.Add(new Snoop.Data.Double("Reduction factor", mat.ReductionFactor));
    //   data.Add(new Snoop.Data.Double("Shear modulus X", mat.ShearModulusX));
    //   data.Add(new Snoop.Data.Double("Shear modulus Y", mat.ShearModulusY));
    //   data.Add(new Snoop.Data.Double("Shear modulus Z", mat.ShearModulusZ));
    //   data.Add(new Snoop.Data.Double("Thermal expansion coefficient X", mat.ThermalExpansionCoefficientX));
    //   data.Add(new Snoop.Data.Double("Thermal expansion coefficient Y", mat.ThermalExpansionCoefficientY));
    //   data.Add(new Snoop.Data.Double("Thermal expansion coefficient Z", mat.ThermalExpansionCoefficientZ));
    //   data.Add(new Snoop.Data.Double("Unit weight", mat.UnitWeight));
    //   data.Add(new Snoop.Data.Double("Young modulus X", mat.YoungModulusX));
    //   data.Add(new Snoop.Data.Double("Young modulus Y", mat.YoungModulusY));
    //   data.Add(new Snoop.Data.Double("Young modulus Z", mat.YoungModulusZ));
    //}

    //private void Stream(ArrayList data, MaterialWood mat)
    //{
    //   data.Add(new Snoop.Data.ClassSeparator(typeof(MaterialWood)));

    //   data.Add(new Snoop.Data.Double("Bending", mat.Bending));
    //   data.Add(new Snoop.Data.Double("Compression parallel", mat.CompressionParallel));
    //   data.Add(new Snoop.Data.Double("Compression perpindicular", mat.CompressionPerpendicular));
    //   data.Add(new Snoop.Data.String("Grade", mat.Grade));
    //   data.Add(new Snoop.Data.Double("Poisson modulus", mat.PoissonModulus));
    //   data.Add(new Snoop.Data.Double("Shear modulus", mat.ShearModulus));
    //   data.Add(new Snoop.Data.Double("Shear parallel", mat.ShearParallel));
    //   data.Add(new Snoop.Data.Double("Shear perpindicular", mat.ShearPerpendicular));
    //   data.Add(new Snoop.Data.String("Species", mat.Species));
    //   data.Add(new Snoop.Data.Double("Thermal expansion coefficient", mat.ThermalExpansionCoefficient));
    //   data.Add(new Snoop.Data.Double("Unit weight", mat.UnitWeight));
    //   data.Add(new Snoop.Data.Double("Young modulus", mat.YoungModulus));
    //}

    private void Stream( ArrayList data, Phase phase )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Phase ) ) );

      // Nothing at this level yet!
    }

    private void Stream( ArrayList data, PrintSetting printSetting )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( PrintSetting ) ) );

      data.Add( new Snoop.Data.String( "Printer name", printSetting.Name ) );
      data.Add( new Snoop.Data.Object( "Printer parameters", printSetting.PrintParameters ) );
    }

    private void Stream( ArrayList data, Rebar rebar )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Rebar ) ) );

      data.Add( new Snoop.Data.ElementId( "Bar type", rebar.GetTypeId(), rebar.Document ) );
      data.Add( new Snoop.Data.ElementId( "Rebar shape", rebar.RebarShapeId, rebar.Document ) );
      data.Add( new Snoop.Data.ElementId( "Host", rebar.GetHostId(), rebar.Document ) );
      data.Add( new Snoop.Data.Object( "Distribution path", rebar.GetDistributionPath() ) );
      data.Add( new Snoop.Data.Enumerable( "GetCenterlineCurves(false, false, false)", rebar.GetCenterlineCurves( false, false, false ) ) );
      data.Add( new Snoop.Data.String( "LayoutRule", rebar.LayoutRule.ToString() ) );
      if( rebar.LayoutRule != RebarLayoutRule.Single )
      {
        data.Add( new Snoop.Data.Double( "Distribution path length", rebar.ArrayLength ) );
        data.Add( new Snoop.Data.Int( "Quantity", rebar.Quantity ) );
        data.Add( new Snoop.Data.Int( "NumberOfBarPositions", rebar.NumberOfBarPositions ) );
        data.Add( new Snoop.Data.Double( "MaxSpacing", rebar.MaxSpacing ) );
      }

      //TF
      data.Add( new Snoop.Data.Object( "ConstraintsManager", rebar.GetRebarConstraintsManager() ) );
      //TFEND
    }

    //TF
    private void Stream( ArrayList data, FabricArea fabricArea )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( FabricArea ) ) );

      data.Add( new Snoop.Data.Double( "Cover Offset", fabricArea.CoverOffset ) );
      data.Add( new Snoop.Data.Xyz( "Direction", fabricArea.Direction ) );
      data.Add( new Snoop.Data.String( "Lap Splice Position", fabricArea.LapSplicePosition.ToString() ) );
      data.Add( new Snoop.Data.String( "Fabric Location", fabricArea.FabricLocation.ToString() ) );
      data.Add( new Snoop.Data.String( "Fabric Area Type", fabricArea.FabricAreaType.ToString() ) );
      data.Add( new Snoop.Data.Double( "Major lap splice length", fabricArea.MajorLapSpliceLength ) );
      data.Add( new Snoop.Data.Double( "Minor lap splice length", fabricArea.MinorLapSpliceLength ) );
      data.Add( new Snoop.Data.String( "Major Sheet Alignment", fabricArea.MajorSheetAlignment.ToString() ) );
      data.Add( new Snoop.Data.String( "Minor Sheet Alignment", fabricArea.MinorSheetAlignment.ToString() ) );
      data.Add( new Snoop.Data.ElementId( "Sheet Type", fabricArea.FabricSheetTypeId, m_app.ActiveUIDocument.Document ) );
      data.Add( new Snoop.Data.ElementId( "Sketch", fabricArea.SketchId, m_app.ActiveUIDocument.Document ) );
      data.Add( new Snoop.Data.ElementId( "Tag View", fabricArea.TagViewId, m_app.ActiveUIDocument.Document ) );
      data.Add( new Snoop.Data.ElementId( "Host", fabricArea.HostId, m_app.ActiveUIDocument.Document ) );
    }

    private void Stream( ArrayList data, FabricSheet fabricSheet )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( FabricSheet ) ) );

      data.Add( new Snoop.Data.Double( "Cut Overall Length", fabricSheet.CutOverallLength ) );
      data.Add( new Snoop.Data.Double( "Cut Overall Width", fabricSheet.CutOverallWidth ) );
      data.Add( new Snoop.Data.Double( "Cut Sheet Mass", fabricSheet.CutSheetMass ) );
      data.Add( new Snoop.Data.ElementId( "Fabric Area Owner", fabricSheet.FabricAreaOwnerId, m_app.ActiveUIDocument.Document ) );
      data.Add( new Snoop.Data.ElementId( "Sheet Type", fabricSheet.GetTypeId(), m_app.ActiveUIDocument.Document ) );
    }

    //TFEND

    private void Stream( ArrayList data, Room room )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Room ) ) );

      data.Add( new Snoop.Data.String( "Number", room.Number ) );
      data.Add( new Snoop.Data.Double( "Perimeter", room.Perimeter ) );
      data.Add( new Snoop.Data.Double( "Area", room.Area ) );
      data.Add( new Snoop.Data.Double( "Volume", room.Volume ) );
      data.Add( new Snoop.Data.Double( "Base offset", room.BaseOffset ) );
      data.Add( new Snoop.Data.Double( "Limit offset", room.LimitOffset ) );
      data.Add( new Snoop.Data.Enumerable( "Boundary", room.GetBoundarySegments( new SpatialElementBoundaryOptions() ) ) );
      data.Add( new Snoop.Data.Object( "Location", room.Location ) );
      data.Add( new Snoop.Data.Object( "Closed shell", room.ClosedShell ) );
      data.Add( new Snoop.Data.Double( "Unbounded height", room.UnboundedHeight ) );
      data.Add( new Snoop.Data.Object( "Upper limit", room.UpperLimit ) );
    }

    private void Stream( ArrayList data, RoomTag roomTag )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( RoomTag ) ) );

      data.Add( new Snoop.Data.Bool( "Leader", roomTag.HasLeader ) );
      data.Add( new Snoop.Data.Object( "Location", roomTag.Location ) );
      data.Add( new Snoop.Data.Object( "Room", roomTag.Room ) );
      data.Add( new Snoop.Data.Object( "Room tag type", roomTag.RoomTagType ) );
      data.Add( new Snoop.Data.Object( "View", roomTag.View ) );
    }

    private void Stream( ArrayList data, AreaReinforcement areaReinf )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( AreaReinforcement ) ) );

      data.Add( new Snoop.Data.Object( "Area reinforcement type", areaReinf.AreaReinforcementType ) );
      // jeremy migrated from Revit 2014 to 2015:
      //data.Add( new Snoop.Data.Enumerable( "Curves", areaReinf.GetCurveElementIds(), areaReinf.Document ) ); // 2014
      data.Add( new Snoop.Data.Enumerable( "Curves", areaReinf.GetBoundaryCurveIds(), areaReinf.Document ) ); // 2015
      data.Add( new Snoop.Data.Xyz( "Direction", areaReinf.Direction ) );

      data.Add( new Snoop.Data.CategorySeparator( "Bar Descriptions" ) );
      System.Collections.Generic.IList<ElementId> rebarIds = areaReinf.GetRebarInSystemIds();
      data.Add( new Snoop.Data.Int( "Number of RebarInSystem", rebarIds.Count ) );
      for( int i = 0; i < rebarIds.Count; i++ )
      {
        data.Add( new Snoop.Data.Object( string.Format( "RebarInSystem [{0:d}]", i ), areaReinf.Document.GetElement( rebarIds[i] ) ) );
      }
    }

    private void Stream( ArrayList data, AreaReinforcementCurve areaReinf )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( AreaReinforcementCurve ) ) );

      data.Add( new Snoop.Data.Object( "Curve", areaReinf.Curve ) );
    }

    private void Stream( ArrayList data, View view )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( View ) ) );

      Utils.StreamWithReflection( data, typeof( View ), view );

      /*
      data.Add(new Snoop.Data.Bool("Can be printed", view.CanBePrinted));
      data.Add(new Snoop.Data.Object("Crop box", view.CropBox));
      data.Add(new Snoop.Data.Bool("Crop box active", view.CropBoxActive));
      data.Add(new Snoop.Data.Bool("Crop box visible", view.CropBoxVisible));
      data.Add(new Snoop.Data.Xyz("Origin", view.Origin));
      data.Add(new Snoop.Data.Object("Outline", view.Outline));
      data.Add(new Snoop.Data.Object("Gen level", view.GenLevel));
      data.Add(new Snoop.Data.Object("Sketch plane", view.SketchPlane));
      data.Add(new Snoop.Data.Xyz("Right direction", view.RightDirection));
      data.Add(new Snoop.Data.Xyz("Up direction", view.UpDirection));
      data.Add(new Snoop.Data.Xyz("View direction", view.ViewDirection));
      data.Add(new Snoop.Data.String("View name", view.ViewName));
      data.Add(new Snoop.Data.String("View type", view.ViewType.ToString()));

      try
      {
        data.Add(new Snoop.Data.Int("Scale", view.Scale));
      }
      catch (Exception ex)
      {
        data.Add(new Snoop.Data.Exception("Scale", ex));
      }
      */

      View3D view3d = view as View3D;
      if( view3d != null )
      {
        Stream( data, view3d );
        return;
      }

      ViewDrafting viewDrafting = view as ViewDrafting;
      if( viewDrafting != null )
      {
        Stream( data, viewDrafting );
        return;
      }

      ViewPlan viewPlan = view as ViewPlan;
      if( viewPlan != null )
      {
        Stream( data, viewPlan );
        return;
      }

      ViewSection viewSection = view as ViewSection;
      if( viewSection != null )
      {
        Stream( data, viewSection );
        return;
      }

      ViewSheet viewSheet = view as ViewSheet;
      if( viewSheet != null )
      {
        Stream( data, viewSheet );
        return;
      }

    }

    private void Stream( ArrayList data, View3D view )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( View3D ) ) );
      Utils.StreamWithReflection( data, typeof( View3D ), view );
      
      /*
      data.Add(new Snoop.Data.Xyz("Eye position", view.EyePosition));
      data.Add(new Snoop.Data.Bool("Is perspective", view.IsPerspective));
      data.Add(new Snoop.Data.Object("Section box", view.SectionBox));*/
    }

    private void Stream( ArrayList data, ViewDrafting view )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( ViewDrafting ) ) );
      Utils.StreamWithReflection( data, typeof( ViewDrafting ), view );

      // nothing at this level yet
    }

    private void Stream( ArrayList data, ViewPlan view )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( ViewPlan ) ) );
      Utils.StreamWithReflection( data, typeof( ViewPlan ), view );
      // nothing at this level yet
    }

    private void Stream( ArrayList data, ViewSection view )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( ViewSection ) ) );
      Utils.StreamWithReflection( data, typeof( ViewSection ), view );
      // nothing at this level yet
    }

    private void Stream( ArrayList data, ViewSheet view )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( ViewSheet ) ) );
      Utils.StreamWithReflection( data, typeof( ViewSheet ), view );
      //data.Add(new Snoop.Data.String("Sheet number", view.SheetNumber));
      //data.Add(new Snoop.Data.Enumerable("Views", view.Views));
    }

    private void Stream( ArrayList data, BeamSystem beamSys )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( BeamSystem ) ) );

      data.Add( new Snoop.Data.Object( "Beam system type", beamSys.BeamSystemType ) );
      data.Add( new Snoop.Data.Object( "Beam type", beamSys.BeamType ) );
      data.Add( new Snoop.Data.Xyz( "Direction", beamSys.Direction ) );
      data.Add( new Snoop.Data.Double( "Elevation", beamSys.Elevation ) );
      data.Add( new Snoop.Data.Object( "Layout rule", beamSys.LayoutRule ) );
      data.Add( new Snoop.Data.Enumerable( "Profile", beamSys.Profile ) );
      data.Add( new Snoop.Data.Enumerable( "Get all beams", beamSys.GetBeamIds(), beamSys.Document ) );

      // TBD: Level seems to be overridden here but not specified as "overridden"
    }

    private void Stream( ArrayList data, BoundaryConditions bndCnd )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( BoundaryConditions ) ) );

      data.Add( new Snoop.Data.Object( "Associated load", bndCnd.AssociatedLoad ) );
      data.Add( new Snoop.Data.Object( "Host element", bndCnd.HostElement ) );
      data.Add( new Snoop.Data.Xyz( "Point", bndCnd.Point ) );

      data.Add( new Snoop.Data.CategorySeparator( "Curves" ) );
      data.Add( new Snoop.Data.Int( "Number of curves", bndCnd.NumCurves ) );
      for( int i = 0; i < bndCnd.NumCurves; i++ )
      {
        data.Add( new Snoop.Data.Object( string.Format( "Curve [{0:d}]", i ), bndCnd.get_Curve( i ) ) );
      }
    }

    private void Stream( ArrayList data, CombinableElement combElem )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( CombinableElement ) ) );

      data.Add( new Snoop.Data.Enumerable( "Combinations", combElem.Combinations ) );

      GenericForm genForm = combElem as GenericForm;
      if( genForm != null )
      {
        Stream( data, genForm );
        return;
      }

      GeomCombination geomComb = combElem as GeomCombination;
      if( geomComb != null )
      {
        Stream( data, geomComb );
        return;
      }

    }

    private void Stream( ArrayList data, GeomCombination geomComb )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( GeomCombination ) ) );

      data.Add( new Snoop.Data.Enumerable( "All members", geomComb.AllMembers ) );
    }

    private void Stream( ArrayList data, Control ctrl )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Control ) ) );

      data.Add( new Snoop.Data.Xyz( "Origin", ctrl.Origin ) );
      data.Add( new Snoop.Data.String( "Shape", ctrl.Shape.ToString() ) );
      data.Add( new Snoop.Data.Object( "View", ctrl.View ) );
    }

    private void Stream( ArrayList data, CurveElement curElem )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( CurveElement ) ) );

      data.Add( new Snoop.Data.Object( "Geometry curve", curElem.GeometryCurve ) );
      data.Add( new Snoop.Data.Object( "Line style", curElem.LineStyle ) );
      data.Add( new Snoop.Data.Enumerable( "Line styles", curElem.GetLineStyleIds(), curElem.Document ) );
      data.Add( new Snoop.Data.Object( "Sketch plane", curElem.SketchPlane ) );

      CurveByPoints curPts = curElem as CurveByPoints;
      if( curPts != null )
      {
        Stream( data, curPts );
        return;
      }

      DetailCurve detCurve = curElem as DetailCurve;
      if( detCurve != null )
      {
        Stream( data, detCurve );
        return;
      }

      ModelCurve modelCurve = curElem as ModelCurve;
      if( modelCurve != null )
      {
        Stream( data, modelCurve );
        return;
      }

      SymbolicCurve symCurve = curElem as SymbolicCurve;
      if( symCurve != null )
      {
        Stream( data, symCurve );
        return;
      }
    }

    private void Stream( ArrayList data, CurveByPoints curPts )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( CurveByPoints ) ) );

      data.Add( new Snoop.Data.Bool( "Is reference line", curPts.IsReferenceLine ) );
      data.Add( new Snoop.Data.Enumerable( "Points", curPts.GetPoints() ) );
      data.Add( new Snoop.Data.String( "Reference type", curPts.ReferenceType.ToString() ) );
      data.Add( new Snoop.Data.Object( "Subcategory", curPts.Subcategory ) );
      data.Add( new Snoop.Data.Object( "Visibility", curPts.GetVisibility() ) );
      data.Add( new Snoop.Data.Bool( "Visible", curPts.Visible ) );
    }

    private void Stream( ArrayList data, DetailCurve detCurve )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( DetailCurve ) ) );

      data.Add( new Snoop.Data.Object( "Geometry curve", detCurve.GeometryCurve ) );
      data.Add( new Snoop.Data.Object( "Sketch plane", detCurve.SketchPlane ) );
      data.Add( new Snoop.Data.ElementId( "Line style", detCurve.LineStyle.Id, m_app.ActiveUIDocument.Document ) );
      data.Add( new Snoop.Data.Enumerable( "Line styles", detCurve.GetLineStyleIds(), m_app.ActiveUIDocument.Document ) );

      DetailArc detArc = detCurve as DetailArc;
      if( detArc != null )
      {
        Stream( data, detArc );
        return;
      }

      DetailEllipse detEllipse = detCurve as DetailEllipse;
      if( detEllipse != null )
      {
        Stream( data, detEllipse );
        return;
      }

      DetailLine detLine = detCurve as DetailLine;
      if( detLine != null )
      {
        Stream( data, detLine );
        return;
      }

      DetailNurbSpline detSpline = detCurve as DetailNurbSpline;
      if( detSpline != null )
      {
        Stream( data, detSpline );
        return;
      }
    }

    private void Stream( ArrayList data, DetailArc detArc )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( DetailArc ) ) );

      // nothing at this level
    }

    private void Stream( ArrayList data, DetailEllipse detEllipse )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( DetailEllipse ) ) );

      // nothing at this level
    }

    private void Stream( ArrayList data, DetailLine detLine )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( DetailLine ) ) );

      // nothing at this level
    }

    private void Stream( ArrayList data, DetailNurbSpline detNurbSpline )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( DetailNurbSpline ) ) );

      // nothing at this level
    }

    private void Stream( ArrayList data, ModelCurve modelCurve )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( ModelCurve ) ) );

      data.Add( new Snoop.Data.Object( "Geometry curve", modelCurve.GeometryCurve ) );
      data.Add( new Snoop.Data.Object( "Sketch plane", modelCurve.SketchPlane ) );
      data.Add( new Snoop.Data.ElementId( "Line style", modelCurve.LineStyle.Id, m_app.ActiveUIDocument.Document ) );
      data.Add( new Snoop.Data.Enumerable( "Line styles", modelCurve.GetLineStyleIds(), m_app.ActiveUIDocument.Document ) );

      ModelArc modelArc = modelCurve as ModelArc;
      if( modelArc != null )
      {
        Stream( data, modelArc );
        return;
      }

      ModelEllipse modelEllipse = modelCurve as ModelEllipse;
      if( modelEllipse != null )
      {
        Stream( data, modelEllipse );
        return;
      }

      ModelHermiteSpline modelHSpline = modelCurve as ModelHermiteSpline;
      if( modelHSpline != null )
      {
        Stream( data, modelHSpline );
        return;
      }

      ModelNurbSpline modelNSpline = modelCurve as ModelNurbSpline;
      if( modelNSpline != null )
      {
        Stream( data, modelNSpline );
        return;
      }

      ModelLine modelLine = modelCurve as ModelLine;
      if( modelLine != null )
      {
        Stream( data, modelLine );
        return;
      }
    }

    private void Stream( ArrayList data, ModelArc modelCrv )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( ModelArc ) ) );

      // no data at this level
    }

    private void Stream( ArrayList data, ModelEllipse modelCrv )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( ModelEllipse ) ) );

      // no data at this level
    }

    private void Stream( ArrayList data, ModelHermiteSpline modelCrv )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( ModelHermiteSpline ) ) );

      // no data at this level
    }

    private void Stream( ArrayList data, ModelLine modelCrv )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( ModelLine ) ) );

      // no data at this level
    }

    private void Stream( ArrayList data, ModelNurbSpline modelCrv )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( ModelNurbSpline ) ) );

      // no data at this level
    }

    private void Stream( ArrayList data, SymbolicCurve symCurve )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( SymbolicCurve ) ) );

      data.Add( new Snoop.Data.Bool( "Is drawn in foreground", symCurve.IsDrawnInForeground ) );
      data.Add( new Snoop.Data.String( "Reference type", symCurve.ReferenceType.ToString() ) );
      data.Add( new Snoop.Data.Object( "Sub category", symCurve.Subcategory ) );
    }


    private void Stream( ArrayList data, Dimension dim )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Dimension ) ) );

      data.Add( new Snoop.Data.Bool( "Are segments equal", dim.AreSegmentsEqual ) );
      data.Add( new Snoop.Data.String( "Name", dim.Name ) );
      data.Add( new Snoop.Data.Object( "Curve", dim.Curve ) );
      data.Add( new Snoop.Data.Object( "Dimension shape", dim.DimensionShape ) );
      data.Add( new Snoop.Data.Object( "Dimension type", dim.DimensionType ) );
      try
      {
        data.Add( new Snoop.Data.Bool( "Is locked", dim.IsLocked ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Is locked", ex ) );
      }

      if( dim.Document.IsFamilyDocument )
      {
        try
        {
          data.Add( new Snoop.Data.Object( "Label", dim.FamilyLabel ) );
        }
        catch( System.Exception ex )
        {
          data.Add( new Snoop.Data.Exception( "Label", ex ) );
        }
      }
      data.Add( new Snoop.Data.Int( "Number of segments", dim.NumberOfSegments ) );
      data.Add( new Snoop.Data.Enumerable( "Segments", dim.Segments ) );

      if( dim.Value != null )
        data.Add( new Snoop.Data.Double( "Value", dim.Value.Value ) );
      data.Add( new Snoop.Data.String( "Value string", dim.ValueString ) );
      data.Add( new Snoop.Data.Enumerable( "References", dim.References, m_activeDoc ) );
      data.Add( new Snoop.Data.Object( "View", dim.View ) );

      // TBD: Name overridden but doesn't appear to have correct keywords

      SpotDimension spotDim = dim as SpotDimension;
      if( spotDim != null )
      {
        Stream( data, spotDim );
        return;
      }
    }

    private void Stream( ArrayList data, SpotDimension dim )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( SpotDimension ) ) );

      data.Add( new Snoop.Data.Object( "Spot dimension type", dim.SpotDimensionType ) );
    }

    private void Stream( ArrayList data, Opening opn )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Opening ) ) );

      data.Add( new Snoop.Data.Enumerable( "Boundary curves", opn.BoundaryCurves ) );
      data.Add( new Snoop.Data.Bool( "Is boundary rect", opn.IsRectBoundary ) );
      data.Add( new Snoop.Data.Enumerable( "Boundary rect", opn.BoundaryRect ) );
      data.Add( new Snoop.Data.Object( "Host", opn.Host ) );

    }

    private void Stream( ArrayList data, PathReinforcement pathReinf )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( PathReinforcement ) ) );

      data.Add( new Snoop.Data.Object( "Path reinforcement type", pathReinf.PathReinforcementType ) );
      data.Add( new Snoop.Data.Enumerable( "Curves", pathReinf.GetCurveElementIds(), pathReinf.Document ) );

      data.Add( new Snoop.Data.CategorySeparator( "Bar Descriptions" ) );
      System.Collections.Generic.IList<ElementId> rebarIds = pathReinf.GetRebarInSystemIds();
      data.Add( new Snoop.Data.Int( "Number of RebarInSystem", rebarIds.Count ) );
      for( int i = 0; i < rebarIds.Count; i++ )
      {
        data.Add( new Snoop.Data.Object( string.Format( "RebarInSystem [{0:d}]", i ), pathReinf.Document.GetElement( rebarIds[i] ) ) );
      }
    }

    private void Stream( ArrayList data, ReferencePlane refPlane )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( ReferencePlane ) ) );

      data.Add( new Snoop.Data.String( "Name", refPlane.Name ) );     // TBD: overridden but not using keyword!
      data.Add( new Snoop.Data.Xyz( "Bubble end", refPlane.BubbleEnd ) );
      data.Add( new Snoop.Data.Xyz( "Free end", refPlane.FreeEnd ) );
      data.Add( new Snoop.Data.Xyz( "Direction", refPlane.Direction ) );
      data.Add( new Snoop.Data.Xyz( "Normal", refPlane.Normal ) );
      data.Add( new Snoop.Data.Object( "Plane", refPlane.Plane ) );
      data.Add( new Snoop.Data.Object( "Reference", refPlane.Reference ) );
    }

    private void Stream( ArrayList data, ReferencePoint refPoint )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( ReferencePoint ) ) );

      data.Add( new Snoop.Data.Object( "Coord plane ref XY", refPoint.GetCoordinatePlaneReferenceXY() ) );
      data.Add( new Snoop.Data.Object( "Coord plane ref XZ", refPoint.GetCoordinatePlaneReferenceXZ() ) );
      data.Add( new Snoop.Data.Object( "Coord plane ref YZ", refPoint.GetCoordinatePlaneReferenceYZ() ) );
      data.Add( new Snoop.Data.String( "Coord plane visibility", refPoint.CoordinatePlaneVisibility.ToString() ) );
      data.Add( new Snoop.Data.Object( "Coord system", refPoint.GetCoordinateSystem() ) );
      data.Add( new Snoop.Data.Enumerable( "Interpolating curves", refPoint.GetInterpolatingCurves() ) );
      data.Add( new Snoop.Data.Xyz( "Position", refPoint.Position ) );
      data.Add( new Snoop.Data.Object( "Reference", refPoint.GetPointElementReference() ) );
      //data.Add(new Snoop.Data.Double("Size", refPoint.Size));
      data.Add( new Snoop.Data.Bool( "Visible", refPoint.Visible ) );
    }

    private void Stream( ArrayList data, SketchPlane sketchPlane )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( SketchPlane ) ) );

      data.Add( new Snoop.Data.Object( "Plane", sketchPlane.GetPlane() ) );
    }

    private void Stream( ArrayList data, Space space )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Space ) ) );

      data.Add( new Snoop.Data.Double( "Actual exhaust air flow", space.ActualExhaustAirflow ) );
      data.Add( new Snoop.Data.Double( "Actual HVAC load", space.ActualHVACLoad ) );
      data.Add( new Snoop.Data.Double( "Actual lighting load", space.ActualLightingLoad ) );
      data.Add( new Snoop.Data.Double( "Actual other load", space.ActualOtherLoad ) );
      data.Add( new Snoop.Data.Double( "Actual power load", space.ActualPowerLoad ) );
      data.Add( new Snoop.Data.Double( "Actual return airflow", space.ActualReturnAirflow ) );
      data.Add( new Snoop.Data.Double( "Actual supply airflow", space.ActualSupplyAirflow ) );

      data.Add( new Snoop.Data.Double( "Area", space.Area ) );
      data.Add( new Snoop.Data.Double( "Perimeter", space.Perimeter ) );
      data.Add( new Snoop.Data.Double( "Volume", space.Volume ) );

      try
      {
        data.Add( new Snoop.Data.Double( "Area per person", space.AreaperPerson ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Area per person", ex ) );
      }
      data.Add( new Snoop.Data.Double( "Average esitmated illumination", space.AverageEstimatedIllumination ) );
      data.Add( new Snoop.Data.Double( "Base offset", space.BaseOffset ) );
      data.Add( new Snoop.Data.Double( "Limit offset", space.LimitOffset ) );
      data.Add( new Snoop.Data.Enumerable( "Boundary", space.GetBoundarySegments( new SpatialElementBoundaryOptions() ) ) );

      try
      {
        data.Add( new Snoop.Data.Double( "Calculated cooling load", space.CalculatedCoolingLoad ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Calculated cooling load", ex ) );
      }
      try
      {
        data.Add( new Snoop.Data.Double( "Calculated heating load", space.CalculatedHeatingLoad ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Calculated heating load", ex ) );
      }
      try
      {
        data.Add( new Snoop.Data.Double( "Calculated supply airflow", space.CalculatedSupplyAirflow ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Calculated supply airflow", ex ) );
      }

      data.Add( new Snoop.Data.Double( "Design exhaust airflow", space.DesignExhaustAirflow ) );
      data.Add( new Snoop.Data.Double( "Design return airflow", space.DesignReturnAirflow ) );
      data.Add( new Snoop.Data.Double( "Design supply airflow", space.DesignSupplyAirflow ) );
      data.Add( new Snoop.Data.Double( "Design cooling load", space.DesignCoolingLoad ) );
      data.Add( new Snoop.Data.Double( "Design heating load", space.DesignHeatingLoad ) );
      data.Add( new Snoop.Data.Double( "Design HVAC load per area", space.DesignHVACLoadperArea ) );
      data.Add( new Snoop.Data.Double( "Design other load per area", space.DesignOtherLoadperArea ) );
      try
      {
        data.Add( new Snoop.Data.Double( "Design lighting load", space.DesignLightingLoad ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Design lighting load", ex ) );
      }
      try
      {
        data.Add( new Snoop.Data.Double( "Design power load", space.DesignPowerLoad ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Design power load", ex ) );
      }

      data.Add( new Snoop.Data.Double( "Ceiling reflectance", space.CeilingReflectance ) );
      data.Add( new Snoop.Data.Double( "Floor reflectance", space.FloorReflectance ) );
      data.Add( new Snoop.Data.Double( "Wall reflectance", space.WallReflectance ) );
      data.Add( new Snoop.Data.Object( "Closed shell", space.ClosedShell ) );
      data.Add( new Snoop.Data.String( "Condition type", space.ConditionType.ToString() ) );

      try
      {
        data.Add( new Snoop.Data.Double( "Latent heat gain per person", space.LatentHeatGainperPerson ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Latent heat gain per person", ex ) );
      }
      try
      {
        data.Add( new Snoop.Data.Double( "Sensible heat gain per person", space.SensibleHeatGainperPerson ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Sensible heat gain per person", ex ) );
      }
      data.Add( new Snoop.Data.Double( "Lighting calculation workplane", space.LightingCalculationWorkplane ) );
      data.Add( new Snoop.Data.String( "Lighting load unit", space.LightingLoadUnit.ToString() ) );
      data.Add( new Snoop.Data.String( "Occupancy unit", space.OccupancyUnit.ToString() ) );
      data.Add( new Snoop.Data.String( "Power load unit", space.PowerLoadUnit.ToString() ) );
      data.Add( new Snoop.Data.Bool( "Occupiable", space.Occupiable ) );
      data.Add( new Snoop.Data.String( "Number", space.Number ) );
      try
      {
        data.Add( new Snoop.Data.Double( "Number of people", space.NumberofPeople ) );  // TBD: wrong came-case
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Number of people", ex ) );
      }
      data.Add( new Snoop.Data.Bool( "Plenum", space.Plenum ) );
      data.Add( new Snoop.Data.String( "Return air flow", space.ReturnAirflow.ToString() ) );
      data.Add( new Snoop.Data.Object( "Room", space.Room ) );
      data.Add( new Snoop.Data.Object( "Zone", space.Zone ) );
      data.Add( new Snoop.Data.Double( "Space cavity ratio", space.SpaceCavityRatio ) );
      data.Add( new Snoop.Data.Object( "Space construction", space.SpaceConstruction ) );
      data.Add( new Snoop.Data.String( "Space type", space.SpaceType.ToString() ) );
      data.Add( new Snoop.Data.Double( "Unbounded height", space.UnboundedHeight ) );
      data.Add( new Snoop.Data.Object( "Upper limit", space.UpperLimit ) );
    }

    private void Stream( ArrayList data, Autodesk.Revit.DB.Analysis.EnergyDataSettings gbXml )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( EnergyDataSettings ) ) );

      data.Add( new Snoop.Data.String( "Building service", gbXml.ServiceType.ToString() ) );
      data.Add( new Snoop.Data.String( "Building type", gbXml.BuildingType.ToString() ) );
      data.Add( new Snoop.Data.Object( "Ground plane", gbXml.GroundPlane ) );
      //data.Add(new Snoop.Data.String("Postal code", gbXml.));
      //data.Add(new Snoop.Data.Object("Project location", gbXml.ProjectLocation));
      data.Add( new Snoop.Data.Object( "Project phase", gbXml.ProjectPhase ) );
      data.Add( new Snoop.Data.String( "Export Complexity", gbXml.ExportComplexity.ToString() ) );
    }

    private void Stream( ArrayList data, ProjectInfo projInfo )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( ProjectInfo ) ) );

      data.Add( new Snoop.Data.String( "Name", projInfo.Name ) );
      data.Add( new Snoop.Data.String( "Number", projInfo.Number ) );
      data.Add( new Snoop.Data.String( "Address", projInfo.Address ) );
      data.Add( new Snoop.Data.String( "Client name", projInfo.ClientName ) );
      data.Add( new Snoop.Data.String( "Issue date", projInfo.IssueDate ) );
      data.Add( new Snoop.Data.String( "Status", projInfo.Status ) );

    }

    private void Stream( ArrayList data, Units projUnit )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Units ) ) );

      data.Add( new Snoop.Data.String( "Decimal symbol", projUnit.DecimalSymbol.ToString() ) );
      data.Add( new Snoop.Data.String( "Digit grouping amount", projUnit.DigitGroupingAmount.ToString() ) );
      data.Add( new Snoop.Data.String( "Digit grouping symbol", projUnit.DigitGroupingSymbol.ToString() ) );
    }

    private void Stream( ArrayList data, AnnotationSymbol annoSym )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( AnnotationSymbol ) ) );

      data.Add( new Snoop.Data.Object( "Annotation symbol type", annoSym.AnnotationSymbolType ) );
      data.Add( new Snoop.Data.Enumerable( "Leaders", annoSym.Leaders ) );
    }

    private void Stream( ArrayList data, BaseArray baseArray )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( BaseArray ) ) );

      data.Add( new Snoop.Data.String( "Name", baseArray.Name ) );
      data.Add( new Snoop.Data.Int( "Number of members", baseArray.NumMembers ) );
      data.Add( new Snoop.Data.Enumerable( "Original members", baseArray.GetOriginalMemberIds(), baseArray.Document ) );
      data.Add( new Snoop.Data.Enumerable( "Copy members", baseArray.GetCopiedMemberIds(), baseArray.Document ) );

      LinearArray linearArray = baseArray as LinearArray;
      if( linearArray != null )
      {
        Stream( data, linearArray );
        return;
      }

      RadialArray radialArray = baseArray as RadialArray;
      if( radialArray != null )
      {
        Stream( data, radialArray );
        return;
      }
    }

    private void Stream( ArrayList data, LinearArray linearArray )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( LinearArray ) ) );

      // no data at this level
    }

    private void Stream( ArrayList data, RadialArray radialArray )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( RadialArray ) ) );

      // no data at this level
    }

    private void Stream( ArrayList data, BasePoint basePt )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( BasePoint ) ) );

      data.Add( new Snoop.Data.Bool( "Is shared", basePt.IsShared ) );
    }

    private void Stream( ArrayList data, GenericForm genForm )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( GenericForm ) ) );

      data.Add( new Snoop.Data.Bool( "Visible", genForm.Visible ) );
      data.Add( new Snoop.Data.Bool( "Is solid", genForm.IsSolid ) );

      try
      {
        data.Add( new Snoop.Data.Object( "Visibillity", genForm.GetVisibility() ) );
      }
      catch { }

      Blend blend = genForm as Blend;
      if( blend != null )
      {
        Stream( data, blend );
        return;
      }

      Extrusion ext = genForm as Extrusion;
      if( ext != null )
      {
        Stream( data, ext );
        return;
      }

      Revolution rev = genForm as Revolution;
      if( rev != null )
      {
        Stream( data, rev );
        return;
      }

      Sweep sweep = genForm as Sweep;
      if( sweep != null )
      {
        Stream( data, sweep );
        return;
      }

      Form form = genForm as Form;
      if( form != null )
      {
        Stream( data, form );
        return;
      }

      SweptBlend sweptBlend = genForm as SweptBlend;
      if( sweptBlend != null )
      {
        Stream( data, sweptBlend );
        return;
      }
    }

    private void Stream( ArrayList data, Blend blend )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Blend ) ) );

      data.Add( new Snoop.Data.Double( "Bottom offset", blend.BottomOffset ) );
      data.Add( new Snoop.Data.Object( "Bottom sketch", blend.BottomSketch ) );
      data.Add( new Snoop.Data.Object( "Top sketch", blend.TopSketch ) );
      data.Add( new Snoop.Data.Double( "Top offset", blend.TopOffset ) );
    }

    private void Stream( ArrayList data, Extrusion ext )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Extrusion ) ) );

      data.Add( new Snoop.Data.Double( "End offset", ext.EndOffset ) );
      data.Add( new Snoop.Data.Double( "Start offset", ext.StartOffset ) );
      data.Add( new Snoop.Data.Object( "Sketch", ext.Sketch ) );
    }

    private void Stream( ArrayList data, Revolution rev )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Revolution ) ) );

      data.Add( new Snoop.Data.Object( "Axis", rev.Axis ) );
      data.Add( new Snoop.Data.Angle( "Start angle", rev.StartAngle ) );
      data.Add( new Snoop.Data.Angle( "End angle", rev.EndAngle ) );
      data.Add( new Snoop.Data.Object( "Sketch", rev.Sketch ) );
    }

    private void Stream( ArrayList data, Sweep sweep )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Sweep ) ) );

      data.Add( new Snoop.Data.Bool( "Is trajectory segmentation enabled", sweep.IsTrajectorySegmentationEnabled ) );
      data.Add( new Snoop.Data.Angle( "Max segment angle", sweep.MaxSegmentAngle ) );
      data.Add( new Snoop.Data.Object( "Path 3D", sweep.Path3d ) );
      data.Add( new Snoop.Data.Object( "Path sketch", sweep.PathSketch ) );
      data.Add( new Snoop.Data.Object( "Profile sketch", sweep.ProfileSketch ) );
      data.Add( new Snoop.Data.Object( "Profile symbol", sweep.ProfileSymbol ) );
    }

    private void Stream( ArrayList data, Form form )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Form ) ) );

      data.Add( new Snoop.Data.Bool( "Has one or more reference profiles", form.HasOneOrMoreReferenceProfiles ) );
      //data.Add(new Snoop.Data.Bool("Locked", form.Locked));
      data.Add( new Snoop.Data.Int( "Path curve count", form.PathCurveCount ) );
      data.Add( new Snoop.Data.Int( "Profile count", form.ProfileCount ) );
    }

    private void Stream( ArrayList data, SweptBlend sweptBlend )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( SweptBlend ) ) );

      data.Add( new Snoop.Data.Enumerable( "Bottom profile", sweptBlend.BottomProfile ) );
      data.Add( new Snoop.Data.Enumerable( "Top profile", sweptBlend.TopProfile ) );
      data.Add( new Snoop.Data.Object( "Bottom profile symbol", sweptBlend.BottomProfileSymbol ) );
      data.Add( new Snoop.Data.Object( "Top profile symbol", sweptBlend.TopProfileSymbol ) );
      data.Add( new Snoop.Data.Object( "Bottom sketch", sweptBlend.BottomSketch ) );
      data.Add( new Snoop.Data.Object( "Top sketch", sweptBlend.TopSketch ) );
      data.Add( new Snoop.Data.Object( "Path sketch", sweptBlend.PathSketch ) );
      data.Add( new Snoop.Data.Object( "Selected path", sweptBlend.SelectedPath ) );


    }

    private void Stream( ArrayList data, IndependentTag tag )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( IndependentTag ) ) );

      try
      {
        data.Add( new Snoop.Data.Xyz( "Leader elbow", tag.LeaderElbow ) );
      }
      catch( Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Leader elbow", ex ) );
      }

      try
      {
        data.Add( new Snoop.Data.Xyz( "Leader end", tag.LeaderEnd ) );
      }
      catch( Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Leader end", ex ) );
      }


      data.Add( new Snoop.Data.String( "Tag orientation", tag.TagOrientation.ToString() ) );
      data.Add( new Snoop.Data.Xyz( "Tag head position", tag.TagHeadPosition ) );
      data.Add( new Snoop.Data.ElementId( "TaggedLocalElementId", tag.TaggedLocalElementId, tag.Document ) );
      try
      {
        if( tag.TagText != null ) data.Add( new Snoop.Data.String( "Tag text", tag.TagText ) );
      }
      catch( Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Tag text", ex ) );
      }
    }

    private void Stream( ArrayList data, MultiReferenceAnnotation mra )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( MultiReferenceAnnotation ) ) );

      data.Add( new Snoop.Data.ElementId( "TagId", mra.TagId, mra.Document ) );
      data.Add( new Snoop.Data.ElementId( "DimensionId", mra.DimensionId, mra.Document ) );
    }

    private void Stream( ArrayList data, SketchBase sketchBase )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( SketchBase ) ) );

      Path3d path3d = sketchBase as Path3d;
      if( path3d != null )
      {
        Stream( data, path3d );
        return;
      }

      Sketch sketch = sketchBase as Sketch;
      if( sketch != null )
      {
        Stream( data, sketch );
        return;
      }
    }

    private void Stream( ArrayList data, Panel panel )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Panel ) ) );

      data.Add( new Snoop.Data.Object( "Panel type", panel.PanelType ) );
      data.Add( new Snoop.Data.Object( "Transform", panel.Transform ) );
      data.Add( new Snoop.Data.Bool( "Lockable", panel.Lockable ) );
    }

    private void Stream( ArrayList data, Mullion mullion )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Mullion ) ) );

      data.Add( new Snoop.Data.Object( "Mullion type", mullion.MullionType ) );
      data.Add( new Snoop.Data.Bool( "Lock", mullion.Lock ) );
      data.Add( new Snoop.Data.Bool( "Lockable", mullion.Lockable ) );
      data.Add( new Snoop.Data.Object( "Location curve", mullion.LocationCurve ) );
    }

    private void Stream( ArrayList data, Path3d path3d )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Path3d ) ) );

      data.Add( new Snoop.Data.Enumerable( "All curve loops", path3d.AllCurveLoops ) );
      data.Add( new Snoop.Data.Int( "Number of curve loops", path3d.NumCurveLoops ) );
      data.Add( new Snoop.Data.CategorySeparator( "Curve loops" ) );
      for( int i = 0; i < path3d.NumCurveLoops; i++ )
      {
        data.Add( new Snoop.Data.Object( string.Format( "Curve loop [{0:d}]", i ), path3d.get_CurveLoop( i ) ) );
      }
    }

    private void Stream( ArrayList data, Sketch sketch )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Sketch ) ) );

      data.Add( new Snoop.Data.Enumerable( "Profile", sketch.Profile ) );
      data.Add( new Snoop.Data.Object( "Sketch plane", sketch.SketchPlane ) );
    }

    private void Stream( ArrayList data, TextElement textElem )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( TextElement ) ) );

      data.Add( new Snoop.Data.Object( "Symbol", textElem.Symbol ) );
      data.Add( new Snoop.Data.String( "Text", textElem.Text ) );

      TextNote textNote = textElem as TextNote;
      if( textNote != null )
      {
        Stream( data, textNote );
        return;
      }
    }

    private void Stream( ArrayList data, Truss truss )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Truss ) ) );

      data.Add( new Snoop.Data.Enumerable( "Curves", truss.Curves ) );
      data.Add( new Snoop.Data.Enumerable( "Members", truss.Members ) );
      data.Add( new Snoop.Data.Object( "Truss type", truss.TrussType ) );
    }

    private void Stream( ArrayList data, ViewSheetSet viewSheetSet )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( ViewSheetSet ) ) );

      data.Add( new Snoop.Data.Enumerable( "Views", viewSheetSet.Views ) );
    }

    private void Stream( ArrayList data, TextNote textNote )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( TextNote ) ) );

      data.Add( new Snoop.Data.Xyz( "Coord", textNote.Coord ) );
      data.Add( new Snoop.Data.Enumerable( "Leaders", textNote.Leaders ) );
      data.Add( new Snoop.Data.Double( "Width", textNote.Width ) );
      data.Add( new Snoop.Data.Object( "Text note type", textNote.TextNoteType ) );
    }

    private void Stream( ArrayList data, Zone zone )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Zone ) ) );

      data.Add( new Snoop.Data.Double( "Area", zone.Area ) );
      data.Add( new Snoop.Data.Double( "Gross area", zone.GrossArea ) );
      data.Add( new Snoop.Data.Double( "Volume", zone.Volume ) );
      try
      {
        data.Add( new Snoop.Data.Double( "Gross volume", zone.GrossVolume ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Gross volume", ex ) );
      }
      data.Add( new Snoop.Data.Double( "Perimieter", zone.Perimeter ) );
      data.Add( new Snoop.Data.Enumerable( "Boundary", zone.Boundary ) );
      try
      {
        data.Add( new Snoop.Data.Double( "Calculated cooling load", zone.CalculatedCoolingLoad ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Calculated cooling load", ex ) );
      }
      try
      {
        data.Add( new Snoop.Data.Double( "Calculated heating load", zone.CalculatedHeatingLoad ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Calculated heating load", ex ) );
      }
      try
      {
        data.Add( new Snoop.Data.Double( "Calculated supply airflow", zone.CalculatedSupplyAirflow ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Calculated supply airflow", ex ) );
      }
      try
      {
        data.Add( new Snoop.Data.Double( "Cooling air temperature", zone.CoolingAirTemperature ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Cooling air temperature", ex ) );
      }
      try
      {
        data.Add( new Snoop.Data.Double( "Cooling set point", zone.CoolingSetPoint ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Cooling set point", ex ) );
      }
      try
      {
        data.Add( new Snoop.Data.Double( "Dehumidification point", zone.DehumidificationSetPoint ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Dehumidification point", ex ) );
      }
      try
      {
        data.Add( new Snoop.Data.Double( "Heating air temperature", zone.HeatingAirTemperature ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Heating air temperature", ex ) );
      }
      try
      {
        data.Add( new Snoop.Data.Double( "Heating set point", zone.HeatingSetPoint ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Heating set point", ex ) );
      }
      data.Add( new Snoop.Data.Double( "Humidification set point", zone.HumidificationSetPoint ) );
      data.Add( new Snoop.Data.Bool( "Is default zone", zone.IsDefaultZone ) );
      try
      {
        data.Add( new Snoop.Data.Double( "Outdoor air per area", zone.OutDoorAirPerArea ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Outdoor air per area", ex ) );
      }
      try
      {
        data.Add( new Snoop.Data.Double( "Outdoor air per person", zone.OutDoorAirPerPerson ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Outdoor air per person", ex ) );
      }
      try
      {
        data.Add( new Snoop.Data.Double( "Outdoor air rate per air changes per hour", zone.OutdoorAirRatePerAirChangesPerHour ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Outdoor air rate per air changes per hour", ex ) );
      }
      data.Add( new Snoop.Data.Object( "Phase", zone.Phase ) );
      data.Add( new Snoop.Data.String( "Service type", zone.ServiceType.ToString() ) );
      data.Add( new Snoop.Data.Enumerable( "Spaces", zone.Spaces ) );
    }

    private void Stream( ArrayList data, GraphicsStyle graphStyle )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( GraphicsStyle ) ) );

      data.Add( new Snoop.Data.Object( "Graphics style category", graphStyle.GraphicsStyleCategory ) );
      data.Add( new Snoop.Data.String( "Graphics style type", graphStyle.GraphicsStyleType.ToString() ) );
    }

    private void Stream( ArrayList data, ImportInstance impInst )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( ImportInstance ) ) );

      data.Add( new Snoop.Data.Bool( "Pinned", impInst.Pinned ) );
      data.Add( new Snoop.Data.Object( "Visibility", impInst.GetVisibility() ) );
    }

    private void Stream( ArrayList data, ModelText modelTxt )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( ModelText ) ) );

      data.Add( new Snoop.Data.Double( "Depth", modelTxt.Depth ) );
      data.Add( new Snoop.Data.String( "Horizontal alignment", modelTxt.HorizontalAlignment.ToString() ) );
      data.Add( new Snoop.Data.Object( "Location", modelTxt.Location ) );
      data.Add( new Snoop.Data.Object( "Model text type", modelTxt.ModelTextType ) );
      if( modelTxt.Document.IsFamilyDocument )
      {
        data.Add( new Snoop.Data.Object( "Sub category", modelTxt.Subcategory ) );
      }
      else
      {
        data.Add( new Snoop.Data.String( "Sub category", "This property can only be gotten when the document is a Family document." ) );
      }
      data.Add( new Snoop.Data.String( "Text", modelTxt.Text ) );
    }

    private void Stream( ArrayList data, PropertyLine propLine )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( PropertyLine ) ) );

      // No data at this level yet!           
    }

    private void Stream( ArrayList data, AreaScheme areaScheme )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( AreaScheme ) ) );

      data.Add( new Snoop.Data.Bool( "Is gross building area", areaScheme.IsGrossBuildingArea ) );
    }

    private void Stream( ArrayList data, MEPSystem mepSys )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( MEPSystem ) ) );

      data.Add( new Snoop.Data.Object( "Base equipment", mepSys.BaseEquipment ) );
      data.Add( new Snoop.Data.Object( "Base equipment connector", mepSys.BaseEquipmentConnector ) );
      data.Add( new Snoop.Data.Object( "Connector manager", mepSys.ConnectorManager ) );
      data.Add( new Snoop.Data.Enumerable( "Elements", mepSys.Elements ) );

      ElectricalSystem elecSys = mepSys as ElectricalSystem;
      if( elecSys != null )
      {
        Stream( data, elecSys );
        return;
      }

      MechanicalSystem mechSys = mepSys as MechanicalSystem;
      if( mechSys != null )
      {
        Stream( data, mechSys );
        return;
      }

      PipingSystem pipingSys = mepSys as PipingSystem;
      if( pipingSys != null )
      {
        Stream( data, pipingSys );
        return;
      }
    }

    private void Stream( ArrayList data, ElectricalSystem elecSys )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( ElectricalSystem ) ) );

      data.Add( new Snoop.Data.Double( "Apparent current", elecSys.ApparentCurrent ) );
      data.Add( new Snoop.Data.Double( "Apparent current phase A", elecSys.ApparentCurrentPhaseA ) );
      data.Add( new Snoop.Data.Double( "Apparent current phase B", elecSys.ApparentCurrentPhaseB ) );
      data.Add( new Snoop.Data.Double( "Apparent current phase C", elecSys.ApparentCurrentPhaseC ) );
      data.Add( new Snoop.Data.Double( "Apparent load", elecSys.ApparentLoad ) );
      data.Add( new Snoop.Data.Double( "Apparent load phase A", elecSys.ApparentLoadPhaseA ) );
      data.Add( new Snoop.Data.Double( "Apparent load phase B", elecSys.ApparentLoadPhaseB ) );
      data.Add( new Snoop.Data.Double( "Apparent load phase C", elecSys.ApparentLoadPhaseC ) );
      data.Add( new Snoop.Data.Bool( "Balanced load", elecSys.BalancedLoad ) );
      data.Add( new Snoop.Data.String( "Circuit number", elecSys.CircuitNumber ) );
      data.Add( new Snoop.Data.Int( "Ground conductors number", elecSys.GroundConductorsNumber ) );
      data.Add( new Snoop.Data.Int( "Hot conductors number", elecSys.HotConductorsNumber ) );
      data.Add( new Snoop.Data.Int( "Neutral conductors number", elecSys.NeutralConductorsNumber ) );
      data.Add( new Snoop.Data.Double( "Length", elecSys.Length ) );
      data.Add( new Snoop.Data.Object( "Load classifications", elecSys.LoadClassifications ) );
      data.Add( new Snoop.Data.String( "Load name", elecSys.LoadName ) );
      data.Add( new Snoop.Data.String( "Panel name", elecSys.PanelName ) );
      data.Add( new Snoop.Data.Int( "Poles number", elecSys.PolesNumber ) );
      data.Add( new Snoop.Data.Int( "Runs number", elecSys.RunsNumber ) );
      data.Add( new Snoop.Data.Double( "Power factor", elecSys.PowerFactor ) );
      data.Add( new Snoop.Data.String( "Power factor state", elecSys.PowerFactorState.ToString() ) );
      data.Add( new Snoop.Data.Double( "Rating", elecSys.Rating ) );
      data.Add( new Snoop.Data.String( "System type", elecSys.SystemType.ToString() ) );
      data.Add( new Snoop.Data.Double( "True current", elecSys.TrueCurrent ) );
      data.Add( new Snoop.Data.Double( "True current phase A", elecSys.TrueCurrentPhaseA ) );
      data.Add( new Snoop.Data.Double( "True current phase B", elecSys.TrueCurrentPhaseB ) );
      data.Add( new Snoop.Data.Double( "True current phase C", elecSys.TrueCurrentPhaseC ) );
      data.Add( new Snoop.Data.Double( "True load", elecSys.TrueLoad ) );
      data.Add( new Snoop.Data.Double( "True load phase A", elecSys.TrueLoadPhaseA ) );
      data.Add( new Snoop.Data.Double( "True load phase B", elecSys.TrueLoadPhaseB ) );
      data.Add( new Snoop.Data.Double( "True load phase C", elecSys.TrueLoadPhaseC ) );
      data.Add( new Snoop.Data.Double( "Voltage", elecSys.Voltage ) );
      data.Add( new Snoop.Data.Double( "Voltage drop", elecSys.VoltageDrop ) );
      data.Add( new Snoop.Data.String( "Wire size string", elecSys.WireSizeString ) );
      data.Add( new Snoop.Data.Object( "Wire type", elecSys.WireType ) );
    }

    private void Stream( ArrayList data, MechanicalSystem mechSys )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( MechanicalSystem ) ) );

      data.Add( new Snoop.Data.Object( "Base equipment connector", mechSys.BaseEquipmentConnector ) );
      data.Add( new Snoop.Data.Enumerable( "Duct network", mechSys.DuctNetwork ) );
      data.Add( new Snoop.Data.Double( "Flow", mechSys.Flow ) );
      data.Add( new Snoop.Data.Bool( "Is well connected", mechSys.IsWellConnected ) );
      data.Add( new Snoop.Data.Double( "Static pressure", mechSys.StaticPressure ) );
      data.Add( new Snoop.Data.String( "System type", mechSys.SystemType.ToString() ) );
    }

    private void Stream( ArrayList data, PipingSystem pipingSys )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( PipingSystem ) ) );

      data.Add( new Snoop.Data.Object( "Base equipment connector", pipingSys.BaseEquipmentConnector ) );
      data.Add( new Snoop.Data.Double( "Flow", pipingSys.Flow ) );
      data.Add( new Snoop.Data.Bool( "Is well connected", pipingSys.IsWellConnected ) );
      data.Add( new Snoop.Data.ElementSet( "Piping network", pipingSys.PipingNetwork ) );
      try
      {
        data.Add( new Snoop.Data.Double( "Static pressure", pipingSys.StaticPressure ) );
      }
      catch( Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Static Pressure", ex ) );
      }
      data.Add( new Snoop.Data.String( "System type", pipingSys.SystemType.ToString() ) );
    }

    private void Stream( ArrayList data, InsulationType insType )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( InsulationType ) ) );

      data.Add( new Snoop.Data.Bool( "Is in use", insType.IsInUse ) );
    }

    private void Stream( ArrayList data, ConnectorElement connElem )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( ConnectorElement ) ) );

      data.Add( new Snoop.Data.Object( "Coordinate system", connElem.CoordinateSystem ) );
      data.Add( new Snoop.Data.String( "Domain", connElem.Domain.ToString() ) );
      data.Add( new Snoop.Data.Double( "Height", connElem.Height ) );
      data.Add( new Snoop.Data.Double( "Width", connElem.Width ) );
      data.Add( new Snoop.Data.Bool( "Is primary", connElem.IsPrimary ) );
      data.Add( new Snoop.Data.Object( "Linked connector", connElem.GetLinkedConnectorElement() ) );
      data.Add( new Snoop.Data.Xyz( "Origin", connElem.Origin ) );
      data.Add( new Snoop.Data.Double( "Radius", connElem.Radius ) );
      data.Add( new Snoop.Data.String( "Shape", connElem.Shape.ToString() ) );
      //data.Add(new Snoop.Data.String("System type", connElem.SystemType.ToString()));
    }

    private void Stream( ArrayList data, TemperatureRatingType tempRatingType )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( TemperatureRatingType ) ) );

      data.Add( new Snoop.Data.Enumerable( "Correction factors", tempRatingType.CorrectionFactors ) );
      data.Add( new Snoop.Data.Enumerable( "Insulation types", tempRatingType.InsulationTypes ) );
      data.Add( new Snoop.Data.Bool( "Is in use", tempRatingType.IsInUse ) );
      data.Add( new Snoop.Data.Object( "Material type", tempRatingType.MaterialType ) );
      data.Add( new Snoop.Data.Enumerable( "Wire sizes", tempRatingType.WireSizes ) );
    }

    private void Stream( ArrayList data, WireMaterialType wireMatType )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( WireMaterialType ) ) );

      data.Add( new Snoop.Data.Enumerable( "Ground conductor sizes", wireMatType.GroundConductorSizes ) );
      data.Add( new Snoop.Data.Bool( "Is in use", wireMatType.IsInUse ) );
      data.Add( new Snoop.Data.Enumerable( "Temperature ratings", wireMatType.TemperatureRatings ) );
    }

    private void Stream( ArrayList data, AnalyticalLink link )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( AnalyticalLink ) ) );

      data.Add( new Snoop.Data.Xyz( "Start", link.Start ) );
      data.Add( new Snoop.Data.Xyz( "End", link.End ) );
      data.Add( new Snoop.Data.ElementId( "Start Hub", link.StartHubId, link.Document ) );
      data.Add( new Snoop.Data.ElementId( "End Hub", link.EndHubId, link.Document ) );
    }
  }
}
