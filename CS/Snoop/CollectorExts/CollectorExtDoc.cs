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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI.Selection;
using RevitLookup.Snoop.Collectors;

namespace RevitLookup.Snoop.CollectorExts
{
  /// <summary>
  /// Provide Snoop.Data for any classes related to a Document.
  /// </summary>

  public class CollectorExtDoc : CollectorExt
  {
    public
    CollectorExtDoc()
    {
    }

    protected override void
    CollectEvent( object sender, CollectorEventArgs e )
    {
      // cast the sender object to the SnoopCollector we are expecting
      Collector snoopCollector = sender as Collector;
      if( snoopCollector == null )
      {
        Debug.Assert( false );    // why did someone else send us the message?
        return;
      }

      // see if it is a type we are responsible for
      Document doc = e.ObjToSnoop as Document;
      if( doc != null )
      {
        Stream( snoopCollector.Data(), doc );
        return;
      }

      Selection sel = e.ObjToSnoop as Selection;
      if( sel != null )
      {
        Stream( snoopCollector.Data(), sel, doc );
        return;
      }

      Settings settings = e.ObjToSnoop as Settings;
      if( settings != null )
      {
        Stream( snoopCollector.Data(), settings );
        return;
      }

      Category cat = e.ObjToSnoop as Category;
      if( cat != null )
      {
        Stream( snoopCollector.Data(), cat );
        return;
      }

      PaperSize paperSize = e.ObjToSnoop as PaperSize;
      if( paperSize != null )
      {
        Stream( snoopCollector.Data(), paperSize );
        return;
      }

      PaperSource paperSource = e.ObjToSnoop as PaperSource;
      if( paperSource != null )
      {
        Stream( snoopCollector.Data(), paperSource );
        return;
      }

      PrintSetup prnSetup = e.ObjToSnoop as PrintSetup;
      if( prnSetup != null )
      {
        Stream( snoopCollector.Data(), prnSetup );
        return;
      }

      PrintParameters prnParams = e.ObjToSnoop as PrintParameters;
      if( prnParams != null )
      {
        Stream( snoopCollector.Data(), prnParams );
        return;
      }

      PlanTopology planTopo = e.ObjToSnoop as PlanTopology;
      if( planTopo != null )
      {
        Stream( snoopCollector.Data(), planTopo );
        return;
      }

      PlanCircuit planCircuit = e.ObjToSnoop as PlanCircuit;
      if( planCircuit != null )
      {
        Stream( snoopCollector.Data(), planCircuit );
        return;
      }

      PrintManager printManager = e.ObjToSnoop as PrintManager;
      if( printManager != null )
      {
        Stream( snoopCollector.Data(), printManager );
        return;
      }
    }

    public IEnumerable collectorOfClass<T>( Document doc )
    {
      return new FilteredElementCollector( doc ).OfClass( typeof( T ) );
    }

    public IEnumerable collectorOfCategoryForFamilySymbol( Document doc, BuiltInCategory builtInCategory )
    {
      return new FilteredElementCollector( doc ).OfCategory( builtInCategory ).OfClass( typeof( FamilySymbol ) );
    }

    public IEnumerable collectorForAnnotationSymbolTypes( Document doc )
    {
      IEnumerable<FamilySymbol> familySymbols = from obj in new FilteredElementCollector( doc ).OfClass( typeof( FamilySymbol ) ).Cast<FamilySymbol>()
                                                let type = obj as FamilySymbol
                                                where type is AnnotationSymbolType
                                                select obj;
      return familySymbols;
    }

    private void
    Stream( ArrayList data, Document doc )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Document ) ) );

      data.Add( new Snoop.Data.Object( "Application", doc.Application ) );
      data.Add( new Snoop.Data.String( "Title", doc.Title ) );
      data.Add( new Snoop.Data.String( "Pathname", doc.PathName ) );
      data.Add( new Snoop.Data.Object( "Settings", doc.Settings ) );
      if( !doc.IsFamilyDocument )
        data.Add( new Snoop.Data.BindingMap( "Parameter bindings", doc.ParameterBindings ) );
      data.Add( new Snoop.Data.Enumerable( "Phases", doc.Phases ) );
      data.Add( new Snoop.Data.Bool( "Reactions are up to date", doc.ReactionsAreUpToDate ) );
      data.Add( new Snoop.Data.Object( "Active View", doc.ActiveView ) );
      data.Add( new Snoop.Data.String( "Display unit system", doc.DisplayUnitSystem.ToString() ) );
      data.Add( new Snoop.Data.Object( "Active project location", doc.ActiveProjectLocation ) );
      data.Add( new Snoop.Data.Object( "Project information", doc.ProjectInformation ) );
      data.Add( new Snoop.Data.Enumerable( "Project locations", doc.ProjectLocations ) );
      data.Add( new Snoop.Data.Object( "Site location", doc.SiteLocation ) );
      data.Add( new Snoop.Data.Object( "Project unit", doc.GetUnits() ) );
      try
      {
        Transaction t = new Transaction( doc );
        t.SetName( "PlanTopologies" );
        t.Start();
        data.Add( new Snoop.Data.Enumerable( "Plan topologies", doc.PlanTopologies ) );
        t.RollBack();
      }
      catch( Autodesk.Revit.Exceptions.ArgumentException )
      {
        //catch exception caused because of inability to create transaction for linked document
      }
      ElementSet elemSet1 = new ElementSet();
      FilteredElementCollector fec = new FilteredElementCollector( doc );
      ElementClassFilter wallFilter = new ElementClassFilter( typeof( Wall ) );
      ElementClassFilter notWallFilter = new ElementClassFilter( typeof( Wall ), true );
      LogicalOrFilter orFilter = new LogicalOrFilter( wallFilter, notWallFilter );
      fec.WherePasses( orFilter );
      List<Element> elements = fec.ToElements() as List<Element>;

      foreach( Element element in elements )
      {
        elemSet1.Insert( element );
      }

      data.Add( new Snoop.Data.ElementSet( "Elements", elemSet1 ) );

      data.Add( new Snoop.Data.Bool( "Is modified", doc.IsModified ) );
      data.Add( new Snoop.Data.Bool( "Is workshared", doc.IsWorkshared ) );

      data.Add( new Snoop.Data.Bool( "Is A Family Document", doc.IsFamilyDocument ) );
      if( doc.IsFamilyDocument )
      {
        data.Add( new Snoop.Data.Object( "Family Manager", doc.FamilyManager ) );
        data.Add( new Snoop.Data.Object( "Owner Family", doc.OwnerFamily ) );
      }

      if( doc.GetWorksharingCentralModelPath() != null )
      {
        ModelPath mp = doc.GetWorksharingCentralModelPath();
        string userVisiblePath = ModelPathUtils.ConvertModelPathToUserVisiblePath( mp );
        data.Add( new Snoop.Data.String( "Worksharing central model path", userVisiblePath ) );
        if( ( mp.ServerPath ) )
        {
          try
          {
            // this is a bit weird, the ModelPath is in an abstract format, we must re-make as a ServerPath
            string prefix = "RSN://" + mp.CentralServerPath;
            string serverRelativePath = userVisiblePath.Substring( prefix.Length );
            ServerPath serverPath = new ServerPath( mp.CentralServerPath, serverRelativePath );
            Guid g = doc.Application.GetWorksharingCentralGUID( serverPath );
            data.Add( new Snoop.Data.String( "Central Model Guid", ( g != null ) ? g.ToString() : "null" ) );
          }
          catch( Exception ex )
          {
            data.Add( new Snoop.Data.Exception( "Central Model Guid", ex ) );
          }
        }
      }
      else
      {
        data.Add( new Snoop.Data.String( "Worksharing central model path", "" ) );
      }

      try
      {
        data.Add(new Snoop.Data.Object("Print manager", doc.PrintManager));
      }
      catch (Exception ex)
      {
        data.Add(new Snoop.Data.Exception("Print manager", ex));
      }

      //data.Add(new Snoop.Data.Enumerable("Print settings", doc.PrintSettings));  //TBD: Behaves badly, need to investigate.                

      data.Add( new Snoop.Data.Enumerable( "Beam system types", collectorOfClass<BeamSystemType>( doc ) ) );
      data.Add( new Snoop.Data.Enumerable( "Continuous footing types", collectorOfClass<ContFootingType>( doc ) ) );
      data.Add( new Snoop.Data.Enumerable( "Curtain system types", collectorOfClass<CurtainSystemType>( doc ) ) );
      data.Add( new Snoop.Data.Enumerable( "Dimension types", collectorOfClass<DimensionType>( doc ) ) );
      data.Add( new Snoop.Data.Enumerable( "Electrical equipment types", collectorOfCategoryForFamilySymbol( doc, BuiltInCategory.OST_ElectricalEquipment ) ) );
      data.Add( new Snoop.Data.Enumerable( "Fascia types", collectorOfClass<FasciaType>( doc ) ) );
      data.Add( new Snoop.Data.Enumerable( "Floor types", collectorOfClass<FloorType>( doc ) ) );
      data.Add( new Snoop.Data.Enumerable( "Grid types", collectorOfClass<GridType>( doc ) ) );
      data.Add( new Snoop.Data.Enumerable( "Gutter types", collectorOfClass<GutterType>( doc ) ) );
      data.Add( new Snoop.Data.Enumerable( "Level types", collectorOfClass<LevelType>( doc ) ) );
      data.Add( new Snoop.Data.Enumerable( "Lighting device types", collectorOfCategoryForFamilySymbol( doc, BuiltInCategory.OST_LightingDevices ) ) );
      data.Add( new Snoop.Data.Enumerable( "Lighting fixture types", collectorOfCategoryForFamilySymbol( doc, BuiltInCategory.OST_LightingFixtures ) ) );
      data.Add( new Snoop.Data.Enumerable( "Mechanical equipment types", collectorOfCategoryForFamilySymbol( doc, BuiltInCategory.OST_MechanicalEquipment ) ) );
      data.Add( new Snoop.Data.Enumerable( "Mullion types", doc.MullionTypes ) );
      data.Add( new Snoop.Data.Enumerable( "Panel types", collectorOfClass<PanelType>( doc ) ) );
      data.Add( new Snoop.Data.Enumerable( "Annotation symbol types", collectorForAnnotationSymbolTypes( doc ) ) );
      data.Add( new Snoop.Data.Enumerable( "Text note types", collectorOfClass<TextNoteType>( doc ) ) );
      data.Add( new Snoop.Data.Enumerable( "Rebar bar types", collectorOfClass<RebarBarType>( doc ) ) );
      data.Add( new Snoop.Data.Enumerable( "Rebar cover types", collectorOfClass<RebarCoverType>( doc ) ) );
      data.Add( new Snoop.Data.Enumerable( "Rebar hook types", collectorOfClass<RebarHookType>( doc ) ) );
      data.Add( new Snoop.Data.Enumerable( "Rebar shapes", collectorOfClass<RebarShape>( doc ) ) );
      data.Add( new Snoop.Data.Enumerable( "Roof types", collectorOfClass<RoofType>( doc ) ) );
      data.Add( new Snoop.Data.Enumerable( "Room tag types", collectorOfCategoryForFamilySymbol( doc, BuiltInCategory.OST_RoomTags ) ) );
      data.Add( new Snoop.Data.Enumerable( "Slab edge types", collectorOfClass<SlabEdgeType>( doc ) ) );
      data.Add( new Snoop.Data.Enumerable( "Space tag types", collectorOfCategoryForFamilySymbol( doc, BuiltInCategory.OST_MEPSpaceTags ) ) );
      data.Add( new Snoop.Data.Enumerable( "Spot dimension types", collectorOfClass<SpotDimensionType>( doc ) ) );
      data.Add( new Snoop.Data.Enumerable( "Text note types", collectorOfClass<TextNoteType>( doc ) ) );
      data.Add( new Snoop.Data.Enumerable( "Title blocks", collectorOfCategoryForFamilySymbol( doc, BuiltInCategory.OST_TitleBlocks ) ) );
      data.Add( new Snoop.Data.Enumerable( "Truss types", collectorOfCategoryForFamilySymbol( doc, BuiltInCategory.OST_Truss ) ) );
      data.Add( new Snoop.Data.Enumerable( "View sheet sets", collectorOfClass<ViewSheetSet>( doc ) ) );
      data.Add( new Snoop.Data.Enumerable( "Wall types", collectorOfClass<WallType>( doc ) ) );
    }

    private void
    Stream( ArrayList data, Selection sel, Document doc )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Selection ) ) );
      ElementSet elemSet = new ElementSet();
      var elemIds = sel.GetElementIds();
      foreach( ElementId id in elemIds )
      {
        elemSet.Insert( doc.GetElement( id ) );
      }
      data.Add( new Snoop.Data.ElementSet( "Elements", elemSet ) );
    }

    private void
    Stream( ArrayList data, Settings settings )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Settings ) ) );

      data.Add( new Snoop.Data.Enumerable( "Categories", settings.Categories ) );
      //To get FillPatterns, first filter FillPatternElement out and use FillPatternElement.GetFillPattern()
      //Same for LinePatterns
      //data.Add(new Snoop.Data.Enumerable("Fill patterns", settings.FillPatterns));
      //data.Add(new Snoop.Data.Enumerable("Line patterns", settings.LinePatterns));

      //settings.Materials does not exist in Revit 2013
      //data.Add(new Snoop.Data.Enumerable("Materials", settings.Materials));

      try
      {
        data.Add( new Snoop.Data.Object( "Electrical setting", settings.ElectricalSetting ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Electrical setting", ex ) );
      }
    }

    private void
    Stream( ArrayList data, Category cat )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( Category ) ) );

      data.Add( new Snoop.Data.Bool( "Allow bound parameters", cat.AllowsBoundParameters ) );
      data.Add( new Snoop.Data.Bool( "Can add sub-category", cat.CanAddSubcategory ) );
      data.Add( new Snoop.Data.Bool( "Has material quantities", cat.HasMaterialQuantities ) );
      data.Add( new Snoop.Data.Bool( "Is cuttable", cat.IsCuttable ) );
      data.Add( new Snoop.Data.Object( "Line color", cat.LineColor ) );
      data.Add( new Snoop.Data.String( "Name", cat.Name ) );
      data.Add( new Snoop.Data.Int( "Element Id", cat.Id.IntegerValue ) );
      data.Add( new Snoop.Data.String( "Built-in category", ( (BuiltInCategory) cat.Id.IntegerValue ).ToString() ) );
      data.Add( new Snoop.Data.Object( "Material", cat.Material ) );
      data.Add( new Snoop.Data.Object( "Parent", cat.Parent ) );
      data.Add( new Snoop.Data.CategoryNameMap( "Sub categories", cat.SubCategories ) );
      data.Add(new Snoop.Data.String("Category Type", cat.CategoryType.ToString()));
    }

    private void Stream( ArrayList data, FamilyManager mgr )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( FamilyManager ) ) );

      try
      {
        data.Add( new Snoop.Data.Object( "Current Type", mgr.CurrentType ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Current Type", ex ) );
      }

      data.Add( new Snoop.Data.Enumerable( "Parameters", mgr.Parameters ) );

      data.Add( new Snoop.Data.Enumerable( "Types", mgr.Types ) );


    }

    private void
    Stream( ArrayList data, PaperSize paperSize )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( PaperSize ) ) );

      data.Add( new Snoop.Data.String( "Name", paperSize.Name ) );
    }

    private void
    Stream( ArrayList data, PaperSource paperSource )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( PaperSource ) ) );

      data.Add( new Snoop.Data.String( "Name", paperSource.Name ) );
    }

    private void
    Stream( ArrayList data, PrintSetup prnSetup )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( PrintSetup ) ) );

      data.Add( new Snoop.Data.Object( "Current print setting", prnSetup.CurrentPrintSetting ) );
      //data.Add(new Snoop.Data.Enumerable("Paper sizes", prnSetup.PaperSizes));
      //data.Add(new Snoop.Data.Enumerable("Paper sources", prnSetup.PaperSources));
    }

    private void
    Stream( ArrayList data, PrintParameters prnParams )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( PrintParameters ) ) );

      data.Add( new Snoop.Data.String( "Color depth", prnParams.ColorDepth.ToString() ) );
      data.Add( new Snoop.Data.String( "Hidden line views", prnParams.HiddenLineViews.ToString() ) );
      data.Add( new Snoop.Data.Bool( "Hide crop boundaries", prnParams.HideCropBoundaries ) );
      data.Add( new Snoop.Data.Bool( "Hide refor work planes", prnParams.HideReforWorkPlanes ) ); // TBD - Check property name (seems to be spelt wrong)
      data.Add( new Snoop.Data.Bool( "Hide scope boxes", prnParams.HideScopeBoxes ) );
      data.Add( new Snoop.Data.Bool( "Hide unreferenced view tags", prnParams.HideUnreferencedViewTags ) ); // TBD - Check property name (seems to be spelt wrong)
      data.Add( new Snoop.Data.String( "Margin type", prnParams.MarginType.ToString() ) );
      data.Add( new Snoop.Data.String( "Page orientation", prnParams.PageOrientation.ToString() ) );

      try
      {
        data.Add( new Snoop.Data.String( "Paper placement", prnParams.PaperPlacement.ToString() ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Paper placement", ex ) );
      }

      data.Add( new Snoop.Data.Object( "Paper size", prnParams.PaperSize ) );
      data.Add( new Snoop.Data.Object( "Paper source", prnParams.PaperSource ) );

      try
      {
        data.Add( new Snoop.Data.String( "Raster quality", prnParams.RasterQuality.ToString() ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Raster quality", ex ) );
      }

      try
      {
        data.Add( new Snoop.Data.Double( "User defined margin X", prnParams.UserDefinedMarginX ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "User defined margin X", ex ) );
      }

      try
      {
        data.Add( new Snoop.Data.Double( "User defined margin Y", prnParams.UserDefinedMarginY ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "User defined margin Y", ex ) );
      }

      data.Add( new Snoop.Data.Bool( "View links in blue", prnParams.ViewLinksinBlue ) );

      try
      {
        data.Add( new Snoop.Data.Int( "Zoom", prnParams.Zoom ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Zoom", ex ) );
      }

      try
      {
        data.Add( new Snoop.Data.String( "Zoom type", prnParams.ZoomType.ToString() ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Zoom type", ex ) );
      }
    }

    private void
    Stream( ArrayList data, PlanTopology planTopo )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( PlanTopology ) ) );
      try
      {
        data.Add( new Snoop.Data.Enumerable( "Circuits", planTopo.Circuits ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Circuits", ex ) );
      }
      try
      {
        data.Add( new Snoop.Data.Object( "Level", planTopo.Level ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Level", ex ) );
      }
      data.Add( new Snoop.Data.Object( "Phase", planTopo.Phase ) );
      try
      {
        data.Add( new Snoop.Data.Enumerable( "Rooms", planTopo.GetRoomIds(), m_activeDoc ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Rooms", ex ) );
      }
    }

    private void
    Stream( ArrayList data, PlanCircuit planCircuit )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( PlanCircuit ) ) );

      data.Add( new Snoop.Data.Double( "Area", planCircuit.Area ) );
      /// TBD: This always throws an exception
      /// 01/24/07
      try
      {
        data.Add( new Snoop.Data.Bool( "Is room located", planCircuit.IsRoomLocated ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Is room located", ex ) );
      }
      data.Add( new Snoop.Data.Int( "Side num", planCircuit.SideNum ) );
    }

    private void
    Stream( ArrayList data, PrintManager printManager )
    {
      data.Add( new Snoop.Data.ClassSeparator( typeof( PrintManager ) ) );

      try
      {
        data.Add( new Snoop.Data.Bool( "Collate", printManager.Collate ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Collate", ex ) );
      }

      try
      {
        data.Add( new Snoop.Data.Bool( "Combined file", printManager.CombinedFile ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Combined file", ex ) );
      }

      data.Add( new Snoop.Data.Int( "Copy number", printManager.CopyNumber ) );
      data.Add( new Snoop.Data.String( "Is virtual", printManager.IsVirtual.ToString() ) );
      data.Add( new Snoop.Data.String( "Printer name", printManager.PrinterName ) );
      data.Add( new Snoop.Data.Bool( "Printer order reverse", printManager.PrintOrderReverse ) );
      data.Add( new Snoop.Data.String( "Print range", printManager.PrintRange.ToString() ) );
      data.Add( new Snoop.Data.Object( "Print setup", printManager.PrintSetup ) );

      try
      {
        data.Add( new Snoop.Data.Bool( "Print to file", printManager.PrintToFile ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Print to file", ex ) );
      }

      try
      {
        data.Add( new Snoop.Data.String( "Print to file name", printManager.PrintToFileName ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "Print to file name", ex ) );
      }

      try
      {
        data.Add( new Snoop.Data.Object( "View sheet setting", printManager.ViewSheetSetting ) );
      }
      catch( System.Exception ex )
      {
        data.Add( new Snoop.Data.Exception( "View sheet setting", ex ) );
      }
    }
  }
}
