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
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

using Revit = Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;


using Autodesk.Revit.Creation;


namespace RevitLookup.Test
{
  /// <summary>
  /// These are all the tests concerning Elements.  If there become alot of them, then we can break this up into
  /// mulitple RevitLookupTestFuncs objects.
  /// </summary>

  public class TestElements : RevitLookupTestFuncs
  {

    private static List<ElementId> m_shedElements = new List<ElementId>();

    public TestElements( Autodesk.Revit.UI.UIApplication app )
      : base( app )
    {
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Windows at 18\" sills", "Change all Windows to have an 18\" sill height", typeof( Revit.Element ), new RevitLookupTestFuncInfo.TestFunc( WindowSill_18 ), RevitLookupTestFuncInfo.TestType.Modify ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Center Windows Vertically", "Change all Windows to be centered in the wall", typeof( Revit.Element ), new RevitLookupTestFuncInfo.TestFunc( WindowCenterVertical ), RevitLookupTestFuncInfo.TestType.Modify ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Parameter Enum Mapping", "Show enum -> param name mapping", "Enum Mappings", new RevitLookupTestFuncInfo.TestFunc( ParameterEnums ), RevitLookupTestFuncInfo.TestType.Other ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Parameter Enum Mapping (no duplicates)", "Show enum -> param name mapping", "Enum Mappings", new RevitLookupTestFuncInfo.TestFunc( ParameterEnumsNoDups ), RevitLookupTestFuncInfo.TestType.Other ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Category Enum Mapping", "Show enum -> category name mapping", "Enum Mappings", new RevitLookupTestFuncInfo.TestFunc( CategoryEnums ), RevitLookupTestFuncInfo.TestType.Other ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Simple Wall", "Create a hardwired wall", typeof( Revit.Element ), new RevitLookupTestFuncInfo.TestFunc( SimpleWall ), RevitLookupTestFuncInfo.TestType.Create ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Simple Floor", "Add a floor for the selected walls", typeof( Revit.Element ), new RevitLookupTestFuncInfo.TestFunc( SimpleFloor ), RevitLookupTestFuncInfo.TestType.Create ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Hardwired Shed", "Create some hardwired walls, floors, windows and doors", typeof( Revit.Element ), new RevitLookupTestFuncInfo.TestFunc( SimpleShed ), RevitLookupTestFuncInfo.TestType.Create ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Swap Family", "Swap any door with a double door", typeof( Revit.Element ), new RevitLookupTestFuncInfo.TestFunc( SimpleSwap ), RevitLookupTestFuncInfo.TestType.Modify ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Level Iteration", "Iterate over levels and change floor to floor height", typeof( Revit.Element ), new RevitLookupTestFuncInfo.TestFunc( SimpleLevelIteration ), RevitLookupTestFuncInfo.TestType.Modify ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Hardwired Classroom", "Create curtain walls, door and furniture", typeof( Revit.Element ), new RevitLookupTestFuncInfo.TestFunc( ClassRoom ), RevitLookupTestFuncInfo.TestType.Create ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Flip element(s)", "Rotates element(s) by 180 degrees", typeof( Revit.Element ), new RevitLookupTestFuncInfo.TestFunc( Flip ), RevitLookupTestFuncInfo.TestType.Modify ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Mirror", "Mirrors selected elements along X axis (Draw above X Axis)", typeof( Revit.Element ), new RevitLookupTestFuncInfo.TestFunc( Mirror ), RevitLookupTestFuncInfo.TestType.Create ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Annotation Symbol", "Creates a new annotation symbol", typeof( Revit.Element ), new RevitLookupTestFuncInfo.TestFunc( AnnoSymbol ), RevitLookupTestFuncInfo.TestType.Create ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Beam System", "Creates a hardwired beam system", typeof( Revit.Element ), new RevitLookupTestFuncInfo.TestFunc( BeamSystemHardWired ), RevitLookupTestFuncInfo.TestType.Create ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Detail Curves", "Creates hardwired detail curves", typeof( Revit.Element ), new RevitLookupTestFuncInfo.TestFunc( DetailCurveHardWired ), RevitLookupTestFuncInfo.TestType.Create ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Dimension", "Creates hardwired lines and a dimension for their distance", typeof( Revit.Element ), new RevitLookupTestFuncInfo.TestFunc( DimensionHardWired ), RevitLookupTestFuncInfo.TestType.Create ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Foundation Slab", "Creates a hardwired foundation slab", typeof( Revit.Element ), new RevitLookupTestFuncInfo.TestFunc( FoundationSlabHardWired ), RevitLookupTestFuncInfo.TestType.Create ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Text Note", "Creates a hardwired text note", typeof( Revit.Element ), new RevitLookupTestFuncInfo.TestFunc( TextNoteHardWired ), RevitLookupTestFuncInfo.TestType.Create ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Simple Slab", "Creates a hardwired slab", typeof( Revit.Element ), new RevitLookupTestFuncInfo.TestFunc( SimpleSlab ), RevitLookupTestFuncInfo.TestType.Create ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "View Section", "Creates a view section", typeof( Revit.Element ), new RevitLookupTestFuncInfo.TestFunc( SimpleViewSection ), RevitLookupTestFuncInfo.TestType.Create ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "View Plan", "Creates a floor view plan", typeof( Revit.Element ), new RevitLookupTestFuncInfo.TestFunc( FloorViewPlan ), RevitLookupTestFuncInfo.TestType.Create ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "View and Sheet Addition", "Adds Floor Plan (Level 1) view to new sheet", typeof( Revit.Element ), new RevitLookupTestFuncInfo.TestFunc( ViewToNewSheetHardwired ), RevitLookupTestFuncInfo.TestType.Create ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Simple Tag", "Add a tag to the selected elements", typeof( Revit.Element ), new RevitLookupTestFuncInfo.TestFunc( SimpleTag ), RevitLookupTestFuncInfo.TestType.Create ) );
      m_testFuncs.Add( new RevitLookupTestFuncInfo( "Update Wall Width", "Update the WallType layer thickness to change the width of the wall.", typeof( Revit.Element ), new RevitLookupTestFuncInfo.TestFunc( ModifyWallWidth ), RevitLookupTestFuncInfo.TestType.Modify ) );
    }


    public void
    WindowSill_18()
    {
      // 2015, jeremy:

      //// first filter out to only Window elements
      //Revit.ElementSet windowSet = Utils.Selection.FilterToCategory( m_revitApp.ActiveUIDocument.Selection.Elements, Revit.BuiltInCategory.OST_Windows, false, m_revitApp.ActiveUIDocument.Document );

      //if( windowSet.IsEmpty )
      //{
      //  MessageBox.Show( "No Window elements are currently selected" );
      //  return;
      //}

      //// go through each window and set the Sill parameter
      //foreach( Revit.Element elem in windowSet )

      // First filter out to only Window elements

      Autodesk.Revit.DB.Document doc = m_revitApp.ActiveUIDocument.Document;

      ICollection<ElementId> windowSet = Utils.Selection.FilterToCategory( m_revitApp.ActiveUIDocument.Selection.GetElementIds(), Revit.BuiltInCategory.OST_Windows, false, doc );

      if( 0 == windowSet.Count )
      {
        MessageBox.Show( "No Window elements are currently selected" );
        return;
      }

      // go through each window and set the Sill parameter
      foreach(ElementId id  in windowSet )
      {
        Revit.Element elem = doc.GetElement( id );
        double sillHt = Utils.Convert.InchesToFeet( 18.0 );   // Revit wants things in Feet and Fractional Inches

        Revit.Parameter param = elem.get_Parameter( BuiltInParameter.INSTANCE_SILL_HEIGHT_PARAM );
        param.Set( sillHt );
      }
    }

    public void
    WindowCenterVertical()
    {
      //// first filter out to only Window elements
      //Revit.ElementSet windowSet = Utils.Selection.FilterToCategory( m_revitApp.ActiveUIDocument.Selection.Elements, Revit.BuiltInCategory.OST_Windows, false, m_revitApp.ActiveUIDocument.Document );

      //if( windowSet.IsEmpty )
      //{
      //  MessageBox.Show( "No Window elements are currently selected" );
      //  return;
      //}

      //// go through each window and set the Sill parameter
      //foreach( Revit.Element elem in windowSet )

      Autodesk.Revit.DB.Document doc = m_revitApp.ActiveUIDocument.Document;

      ICollection<ElementId> windowSet = Utils.Selection.FilterToCategory( m_revitApp.ActiveUIDocument.Selection.GetElementIds(), Revit.BuiltInCategory.OST_Windows, false, doc );

      if( 0 == windowSet.Count )
      {
        MessageBox.Show( "No Window elements are currently selected" );
        return;
      }

      // go through each window and set the Sill parameter
      foreach( ElementId id in windowSet )
      {
        Revit.Element elem = doc.GetElement( id );
        try
        {
          FamilyInstance window = (FamilyInstance) elem;
          HostObject host = (HostObject) window.Host;

          double wallHt = GetWallHeight( host );
          double windowHt = GetWindowHeight( window );
          double sillHt = ( wallHt / 2.0 ) - ( windowHt / 2.0 );

          Revit.Parameter param = elem.get_Parameter( BuiltInParameter.INSTANCE_SILL_HEIGHT_PARAM );
          param.Set( sillHt );
        }
        catch( System.Exception e )
        {	// we want to catch it so we can see the problem, otherwise it just silently bails out
          MessageBox.Show( e.Message );
          throw e;
        }
      }
    }

    private double
    GetWallHeight( HostObject hostObj )
    {
      return hostObj.get_Parameter( BuiltInParameter.WALL_USER_HEIGHT_PARAM ).AsDouble();
    }

    private double
    GetWindowHeight( FamilyInstance window )
    {
      return window.Document.GetElement( window.GetTypeId() ).get_Parameter( BuiltInParameter.WINDOW_HEIGHT ).AsDouble();
    }

    private void
    ParameterEnums()
    {
      ArrayList labelStrs = new ArrayList();
      ArrayList valueStrs = new ArrayList();

      foreach( BuiltInParameter paramEnum in System.Enum.GetValues( typeof( BuiltInParameter ) ) )
      {
        labelStrs.Add( paramEnum.ToString() );
        valueStrs.Add( string.Format( "{0:d}", (int) paramEnum ) );
      }

      RevitLookup.Snoop.Forms.ParamEnum dbox = new RevitLookup.Snoop.Forms.ParamEnum( labelStrs, valueStrs );
      dbox.ShowDialog();
    }

    private void
    ParameterEnumsNoDups()
    {
      ArrayList labelStrs = new ArrayList();
      ArrayList valueStrs = new ArrayList();

      string[] strs = System.Enum.GetNames( typeof( BuiltInParameter ) );
      foreach( string str in strs )
      {
        BuiltInParameter paramEnum = (BuiltInParameter) System.Enum.Parse( typeof( BuiltInParameter ), str );

        labelStrs.Add( str );
        valueStrs.Add( string.Format( "{0:d}", (int) paramEnum ) );
      }

      RevitLookup.Snoop.Forms.ParamEnum dbox = new RevitLookup.Snoop.Forms.ParamEnum( labelStrs, valueStrs );
      dbox.ShowDialog();
    }

    private void
    CategoryEnums()
    {
      Revit.Categories categories = m_revitApp.ActiveUIDocument.Document.Settings.Categories;
      ArrayList labelStrs = new ArrayList();
      ArrayList valueStrs = new ArrayList();

      foreach( Revit.BuiltInCategory catEnum in System.Enum.GetValues( typeof( Revit.BuiltInCategory ) ) )
      {
        labelStrs.Add( catEnum.ToString() );

        try
        {
          Revit.Category cat = categories.get_Item( catEnum );
          if( cat == null )
            valueStrs.Add( "<null>" );
          else
            valueStrs.Add( cat.Name );
        }
        catch( System.Exception )
        {
          Trace.WriteLine( catEnum.ToString() );
        }



      }

      RevitLookup.Snoop.Forms.ParamEnum dbox = new RevitLookup.Snoop.Forms.ParamEnum( labelStrs, valueStrs );
      dbox.ShowDialog();




    }

    /// <summary>
    /// 
    /// </summary>
    public void ClassRoom()
    {
      Revit.Document doc = m_revitApp.ActiveUIDocument.Document;

      // get the symbols in the beginning itself , so that 
      // if one is missing you can load it
      FamilySymbol columnSymbol = null;
      FamilySymbol chairSymbol = null;
      FamilySymbol deskSymbol = null;
      FamilySymbol chairMainSymbol = null;
      //FamilySymbol chairSym = null;
      FamilySymbol doorSymbol = null;
      Family columnfamily;
      Family deskfamily;
      Family doorfamily;

      FilteredElementCollector fec = new FilteredElementCollector( m_revitApp.ActiveUIDocument.Document );
      ElementClassFilter familiesAreWanted = new ElementClassFilter( typeof( Family ) );
      fec.WherePasses( familiesAreWanted );
      List<Element> elements = fec.ToElements() as List<Element>;

      foreach( Element element in elements )
      {
        Family fam = element as Family;
        if( fam != null )
        {
          if( fam.Name == "Rectangular Column" )
          {
            columnSymbol = Utils.FamilyUtil.GetFamilySymbol( fam, "24\" x 24\"" );
          }

          if( fam.Name == "Desk" )
          {
            deskSymbol = Utils.FamilyUtil.GetFamilySymbol( fam, "60\" x 30\"" );
          }

          if( fam.Name == "Single-Flush" )
          {
            doorSymbol = Utils.FamilyUtil.GetFamilySymbol( fam, "36\" x 84\"" );
          }

          if( fam.Name == "Chair-Tablet Arm" )
          {
            chairSymbol = Utils.FamilyUtil.GetFamilySymbol( fam, "Chair-Tablet" );
          }

          if( fam.Name == "Chair-Breuer" )
          {
            chairMainSymbol = Utils.FamilyUtil.GetFamilySymbol( fam, "Chair-Breuer" );
          }
        }
      }

      String familyNameToLoad = "";

      // check for required families
      if( columnSymbol == null )
      {
        MessageBox.Show( "Please load Rectangular Column 24\" x 24\" into project" );

        String famName = Utils.UserInput.GetFamilyNameFromUser( null, ref familyNameToLoad );

        m_revitApp.ActiveUIDocument.Document.LoadFamily( famName, out columnfamily );
        if( columnfamily != null )
        {
          columnSymbol = Utils.FamilyUtil.GetFamilySymbol( columnfamily, "24\" x 24\"" );
        }
      }

      if( deskSymbol == null )
      {
        MessageBox.Show( "Please load Desk 60\" x 30\" into project" );

        String famName = Utils.UserInput.GetFamilyNameFromUser( null, ref familyNameToLoad );

        m_revitApp.ActiveUIDocument.Document.LoadFamily( famName, out deskfamily );
        if( deskfamily != null )
        {
          deskSymbol = Utils.FamilyUtil.GetFamilySymbol( deskfamily, "36\" x 84\"" );
        }
      }

      if( doorSymbol == null )
      {
        MessageBox.Show( "Please load door Single Flush 36\" x 84\" into project" );

        String famName = Utils.UserInput.GetFamilyNameFromUser( null, ref familyNameToLoad );

        m_revitApp.ActiveUIDocument.Document.LoadFamily( famName, out doorfamily );
        if( doorfamily != null )
        {
          doorSymbol = Utils.FamilyUtil.GetFamilySymbol( doorfamily, "36\" x 84\"" );
        }
      }


      IDictionary<String, String> libsPaths = m_revitApp.Application.GetLibraryPaths();
      string pathName = string.Empty;
      foreach( KeyValuePair<String, String> libPath in libsPaths )
      {
        pathName = libPath.Value;
        break;
      }

      string fileName = string.Empty;
      bool success = false;

      fileName = pathName + @"\Furniture\Chair-Tablet Arm.rfa";

      //Boolean load = m_revitApp.ActiveUIDocument.Document.LoadFamilySymbol(fileName, "Chair-Tablet Arm", ref chairSym);

      Family chairFamily;
      // if not found in doc try to load automatically
      if( chairSymbol == null )
      {
        success = m_revitApp.ActiveUIDocument.Document.LoadFamily( fileName, out chairFamily );
        chairSymbol = Utils.FamilyUtil.GetFamilySymbol( chairFamily, "Chair-Tablet Arm" );
      }
      // last ditch effort by trying to load manually
      if( chairSymbol == null )
      {
        MessageBox.Show( "Please load Chair-Tablet Arm into project" );
        Utils.UserInput.LoadFamily( null, m_revitApp.ActiveUIDocument.Document );
      }

      fileName = pathName + @"\Furniture\Chair-Breuer.rfa";
      Family chairMainFamily;
      // if not found in doc try to load automatically
      if( chairMainSymbol == null )
      {
        success = m_revitApp.ActiveUIDocument.Document.LoadFamily( fileName, out chairMainFamily );
        chairMainSymbol = Utils.FamilyUtil.GetFamilySymbol( chairMainFamily, "Chair-Breuer" );
      }
      // last ditch effort by trying to load manually
      if( chairMainSymbol == null )
      {
        MessageBox.Show( "Please load Chair-Breuer into project" );
        Utils.UserInput.LoadFamily( null, m_revitApp.ActiveUIDocument.Document );
      }

      // get the level on which we want to build
      Level level1 = null;
      FilteredElementCollector levelFec = new FilteredElementCollector( doc );
      ElementClassFilter levelsAreWanted = new ElementClassFilter( typeof( Level ) );
      levelFec.WherePasses( levelsAreWanted );
      List<Element> levels = levelFec.ToElements() as List<Element>;

      foreach( Element element in levels )
      {
        Level levelTemp = element as Level;
        if( levelTemp != null )
        {
          if( levelTemp.Name == "Level 1" )
          {
            level1 = levelTemp;
            break;
          }
        }
      }

      // draw 4 lines
      XYZ startPt1 = new XYZ( 0.0, 0.0, 0.0 );
      XYZ endPt1 = new XYZ( 56.5, 0.0, 0.0 );
      Line line1 = Line.CreateBound( startPt1, endPt1 );

      XYZ startPt2 = endPt1;
      XYZ endPt2 = new XYZ( 56.5, 61.5, 0.0 );
      Line line2 = Line.CreateBound( startPt2, endPt2 );

      XYZ startPt3 = endPt2;
      XYZ endPt3 = new XYZ( 0.0, 61.5, 0.0 );
      Line line3 = Line.CreateBound( startPt3, endPt3 );

      XYZ startPt4 = endPt3;
      XYZ endPt4 = startPt1;
      Line line4 = Line.CreateBound( startPt4, endPt4 );

      // get the wall types we want
      WallType curtainWallType = null;
      WallType outerWallType = null;

      FilteredElementCollector wallTypeFec = new FilteredElementCollector( doc );
      ElementClassFilter wallTypesAreWanted = new ElementClassFilter( typeof( WallType ) );
      wallTypeFec.WherePasses( wallTypesAreWanted );
      List<Element> wallTypes = wallTypeFec.ToElements() as List<Element>;
      foreach( Element element in wallTypes )
      {
        WallType wallType = element as WallType;
        if( wallType != null )
        {
          if( wallType.Name == "Curtain Wall 1" )
          {
            curtainWallType = wallType;
          }
          if( wallType.Name == "Generic - 6\" Masonry" )
          {
            outerWallType = wallType;
          }
        }
      }

      // draw 4 walls
      Autodesk.Revit.DB.Document activeDoc = m_revitApp.ActiveUIDocument.Document;
      Wall wall1 = Wall.Create( activeDoc, line1, outerWallType.Id, level1.Id, 7.0, 0.0, false, false );
      Wall wall2 = Wall.Create( activeDoc, line2, curtainWallType.Id, level1.Id, 7.0, 0.0, false, false );
      Wall wall3 = Wall.Create( activeDoc, line3, curtainWallType.Id, level1.Id, 7.0, 0.0, false, false );
      Wall wall4 = Wall.Create( activeDoc, line4, curtainWallType.Id, level1.Id, 7.0, 0.0, false, false );


      FamilyInstance StructColumn1 = m_revitApp.ActiveUIDocument.Document.Create.NewFamilyInstance( startPt1, columnSymbol, wall1, level1, StructuralType.Column );
      FamilyInstance StructColumn2 = m_revitApp.ActiveUIDocument.Document.Create.NewFamilyInstance( startPt2, columnSymbol, wall2, level1, StructuralType.Column );
      FamilyInstance StructColumn3 = m_revitApp.ActiveUIDocument.Document.Create.NewFamilyInstance( startPt3, columnSymbol, wall3, level1, StructuralType.Column );
      FamilyInstance StructColumn4 = m_revitApp.ActiveUIDocument.Document.Create.NewFamilyInstance( startPt4, columnSymbol, wall4, level1, StructuralType.Column );

      int row = 5;
      int col = 8;

      double x = 5.0;
      double y = 11.0;
      double z = 0.0;

      XYZ refDirection = new XYZ( 1, 10, 0 );

      // place all the chairs
      for( int i = 0; i < row; i++ )
      {
        y = y + 8;
        for( int j = 0; j < col; j++ )
        {
          x = x + 5;
          XYZ location = new XYZ( x, y, z );
          FamilyInstance famInst = m_revitApp.ActiveUIDocument.Document.Create.NewFamilyInstance( location, chairSymbol, StructuralType.UnknownFraming );

          // flip the chairs 180 degrees
          XYZ pt = location;
          XYZ dir = GeomUtils.kZAxis;
          Line zAxis = Line.CreateUnbound( pt, dir );
          famInst.Location.Rotate( zAxis, GeomUtils.kRad180 );
        }
        x = x - ( col * 5 );
      }

      // place the desk
      XYZ deskPosition = new XYZ( 15.0, 8.0, 0.0 );
      m_revitApp.ActiveUIDocument.Document.Create.NewFamilyInstance( deskPosition, deskSymbol, StructuralType.UnknownFraming );

      // place the chair
      XYZ chairPosition = new XYZ( 18, 7.0, 0.0 );
      m_revitApp.ActiveUIDocument.Document.Create.NewFamilyInstance( chairPosition, chairMainSymbol, StructuralType.UnknownFraming );

      // place the door
      XYZ doorPosition = new XYZ( 46.0, 0.0, 0.0 );
      m_revitApp.ActiveUIDocument.Document.Create.NewFamilyInstance( doorPosition, doorSymbol, wall1, StructuralType.Brace );
    }

    /// <summary>
    /// Create a single wall at the specified location and level
    /// </summary>
    public void
    SimpleWall()
    {
      XYZ pt1 = new XYZ( 10.0, 10.0, 0.0 );
      XYZ pt2 = new XYZ( 30.0, 10.0, 0.0 );

      Line line = Line.CreateBound( pt1, pt2 );

      Autodesk.Revit.Creation.Document doc = m_revitApp.ActiveUIDocument.Document.Create;

      // Just for kicks we will place it at a newly created level, but on checking the 
      // properties of the wall we can note that it still exists on Level 1.
      Wall.Create( m_revitApp.ActiveUIDocument.Document, line, doc.NewLevel( 0.0 ).Id, true );
    }


    /// <summary>
    /// Create a floor for the selected set of walls.
    /// </summary>
    public void
    SimpleFloor()
    {
      Autodesk.Revit.DB.Document dbdoc = m_revitApp.ActiveUIDocument.Document;
      Autodesk.Revit.Creation.Document doc = dbdoc.Create;
      Autodesk.Revit.Creation.Application applic = m_revitApp.Application.Create;

      //// first filter out to only Wall elements
      //Revit.ElementSet wallSet = Utils.Selection.FilterToCategory( m_revitApp.ActiveUIDocument.Selection.Elements,
      //                                        Revit.BuiltInCategory.OST_Walls, false, m_revitApp.ActiveUIDocument.Document );

      //if( wallSet.IsEmpty )
      //{
      //  MessageBox.Show( "No wall elements are currently selected" );
      //  return;
      //}


      // First filter out to only Wall elements
      ICollection<ElementId> wallSet 
        = Utils.Selection.FilterToCategory(
          m_revitApp.ActiveUIDocument.Selection.GetElementIds(),
          Revit.BuiltInCategory.OST_Walls, false, dbdoc );

      if( 0 == wallSet.Count )
      {
        MessageBox.Show( "No wall elements are currently selected" );
        return;
      }
      
      // Get the wall profile needed for the floor
      CurveArray profile = applic.NewCurveArray();

      foreach( ElementId id in wallSet )
      {
        Wall w = dbdoc.GetElement( id ) as Wall;
        Revit.LocationCurve curve = w.Location as Revit.LocationCurve;
        profile.Append( curve.Curve );
      }

      // Obtain the required floor type
      FloorType floorType = null;

      try
      {
        FilteredElementCollector fec = new FilteredElementCollector( m_revitApp.ActiveUIDocument.Document );
        ElementClassFilter elementsAreWanted = new ElementClassFilter( typeof( FloorType ) );
        fec.WherePasses( elementsAreWanted );
        List<Element> elements = fec.ToElements() as List<Element>;

        foreach( Element element in elements )
        {
          FloorType f = element as FloorType;

          if( null == f )
          {
            continue;
          }

          if( f.Name == "Generic - 12\"" )
          {
            floorType = f;
          }
        }

      }
      catch( Exception e )
      {
        throw e;
      }

      // Set the stuctural value
      bool structural = true;

      // Create the floor instance
      try
      {
        Floor f = doc.NewFloor( profile, floorType, doc.NewLevel( 0.0 ), structural );
      }
      catch( Exception e )
      {
        throw e;
      }
    }



    /// <summary>
    /// Creates a simple shed consisting of a door, window, a few floors and walls
    /// </summary>
    public void
    SimpleShed()
    {
      FamilySymbol doorSymbol = null;
      FamilySymbol windowSymbol = null;
      Family doorFamily = null;
      Family windowFamily = null;
      Autodesk.Revit.Creation.Document doc = m_revitApp.ActiveUIDocument.Document.Create;
      Revit.Document docu = m_revitApp.ActiveUIDocument.Document;
      Autodesk.Revit.Creation.Application applic = m_revitApp.Application.Create;


      //The levels for the floors and the fake roof
      Level floorLevel = null;
      Level midLevel = null;

      Revit.ElementSet wallSet = new ElementSet();

      // Create a new CurveArray to provide the profile of the walls to the floor
      CurveArray curArray = applic.NewCurveArray();


      // iterate through all available levels...
      FilteredElementCollector fec = new FilteredElementCollector( m_revitApp.ActiveUIDocument.Document );
      ElementClassFilter elementsAreWanted = new ElementClassFilter( typeof( Level ) );
      fec.WherePasses( elementsAreWanted );
      List<Element> elements = fec.ToElements() as List<Element>;

      foreach( Element element in elements )
      {
        Level sysLevel = element as Level;
        if( sysLevel != null )
        {
          String name = sysLevel.Name;

          if( name == "Level 1" )
          {
            floorLevel = sysLevel;
          }

          if( name == "Level 2" )
          {
            midLevel = sysLevel;
          }
        }
      }

      // first create 4 walls            

      // wall1
      XYZ pt1 = new XYZ( 10.0, 10.0, 0.0 );
      XYZ pt2 = new XYZ( 30.0, 10.0, 0.0 );

      Line line = Line.CreateBound( pt1, pt2 );

      Wall windowHost = Wall.Create( docu, line, floorLevel.Id, false );

      curArray.Append( line );
      wallSet.Insert( windowHost );

      Revit.ElementId windowHostId = windowHost.Id;
      m_shedElements.Add( windowHostId );

      // wall2
      XYZ pt3 = new XYZ( 10.0, 10.0, 0.0 );
      XYZ pt4 = new XYZ( 10.0, 30.0, 0.0 );

      Line line1 = Line.CreateBound( pt3, pt4 );

      Wall wall2 = Wall.Create( docu, line1, floorLevel.Id, false );

      curArray.Append( line1 );
      wallSet.Insert( wall2 );

      Revit.ElementId wall2Id = wall2.Id;
      m_shedElements.Add( wall2Id );

      // wall3
      XYZ pt5 = new XYZ( 10.0, 30.0, 0.0 );
      XYZ pt6 = new XYZ( 30.0, 30.0, 0.0 );

      Line line2 = Line.CreateBound( pt5, pt6 );

      Wall doorHost = Wall.Create( docu, line2, floorLevel.Id, false );

      curArray.Append( line2 );
      wallSet.Insert( doorHost );

      Revit.ElementId doorHostId = doorHost.Id;
      m_shedElements.Add( doorHostId );

      // wall4
      XYZ pt7 = new XYZ( 30.0, 30.0, 0.0 );
      XYZ pt8 = new XYZ( 30.0, 10.0, 0.0 );

      Line line3 = Line.CreateBound( pt7, pt8 );

      Wall wall4 = Wall.Create( docu, line3, floorLevel.Id, false );

      curArray.Append( line3 );
      wallSet.Insert( wall4 );

      Revit.ElementId wall4Id = wall4.Id;
      m_shedElements.Add( wall4Id );

      if( curArray != null )
      {
        // add two floors to the walls created above
        Revit.ElementId floorId = SimpleFloor( curArray, floorLevel );
        SimpleFloor( curArray, midLevel );

        // set the top/bottom constraint and room bounding  params for the walls

        // Note: The WALL_TOP/BOTTOM_IS_ATTACHED parameter's value when set, does not seem to stick. This needs
        // to be looked into as well. arj 7/25/06

        // The WALL_TOP/BOTTOM_IS_ATTACHED parameters are Read Only and cannot be set. arj 01/17/07
        foreach( Wall wall in wallSet )
        {
          Revit.Parameter wallRoomBound = wall.get_Parameter( BuiltInParameter.WALL_ATTR_ROOM_BOUNDING );
          wallRoomBound.Set( 1 );

          Revit.Parameter wallBaseConstr = wall.get_Parameter( BuiltInParameter.WALL_BASE_CONSTRAINT );
          wallBaseConstr.Set( floorId );
        }
      }

      // set the location for elements such as doors, windows

      XYZ doorLocation = new XYZ( 20.0, 30.0, 0.0 );
      XYZ windowLocation = new XYZ( 20.0, 10.0, 10.0 );

      // check for required symbols
      FilteredElementCollector familyFec = new FilteredElementCollector( m_revitApp.ActiveUIDocument.Document );
      ElementClassFilter familiesAreWanted = new ElementClassFilter( typeof( Family ) );
      familyFec.WherePasses( familiesAreWanted );
      List<Element> families = familyFec.ToElements() as List<Element>;

      foreach( Element element in families )
      {
        Family fam = element as Family;
        if( fam != null )
        {
          if( fam.Name == "Single-Flush" )
          {
            doorSymbol = Utils.FamilyUtil.GetFamilySymbol( fam, "36\" x 84\"" );
          }

          if( fam.Name == "Fixed" )
          {
            windowSymbol = Utils.FamilyUtil.GetFamilySymbol( fam, "24\" x 48\"" );
          }
        }
      }

      // check for required symbols

      String familyNameToLoad = "";

      if( doorSymbol == null )
      {
        MessageBox.Show( "Please load Single-Flush 34\" x 84\" into project" );

        String famName = Utils.UserInput.GetFamilyNameFromUser( null, ref familyNameToLoad );

        m_revitApp.ActiveUIDocument.Document.LoadFamily( famName, out doorFamily );
        if( doorFamily != null )
        {
          doorSymbol = Utils.FamilyUtil.GetFamilySymbol( doorFamily, "36\" x 84\"" );
        }
      }

      if( windowSymbol == null )
      {
        MessageBox.Show( "Please load Fixed 24\" x 48\" into project" );

        String famName = Utils.UserInput.GetFamilyNameFromUser( null, ref familyNameToLoad );

        m_revitApp.ActiveUIDocument.Document.LoadFamily( famName, out windowFamily );
        if( windowFamily != null )
        {
          windowSymbol = Utils.FamilyUtil.GetFamilySymbol( windowFamily, "24\" x 48\"" );
        }
      }

      // Create the door instance            

      floorLevel.Elevation = 0.0;
      FamilyInstance doorInst = doc.NewFamilyInstance( doorLocation, doorSymbol, doorHost, floorLevel, StructuralType.Column );

      Revit.ElementId doorInstId = doorInst.Id;
      m_shedElements.Add( doorInstId );


      // Create the window instance                     

      floorLevel.Elevation = 0.0;
      FamilyInstance windowInst = doc.NewFamilyInstance( windowLocation, windowSymbol, windowHost, floorLevel, StructuralType.Column );

      // The sill height needs to be set to 4.0 or less for the window to be visible in Level 1 view.
      Revit.Parameter sillHeightParam = windowInst.get_Parameter( BuiltInParameter.INSTANCE_SILL_HEIGHT_PARAM );
      sillHeightParam.Set( 4.0 );

      Revit.ElementId windowInstId = windowInst.Id;
      m_shedElements.Add( windowInstId );
    }



    /// <summary>
    ///  Used by the SimpleShed to create its floors and the fake roof
    /// </summary>
    /// <param name="profile"></param>
    public Revit.ElementId
    SimpleFloor( CurveArray profile, Level level )
    {
      Autodesk.Revit.Creation.Document doc = m_revitApp.ActiveUIDocument.Document.Create;
      Autodesk.Revit.Creation.Application applic = m_revitApp.Application.Create;

      // Obtain the required floor type
      FloorType floorType = null;

      try
      {
        FilteredElementCollector fec = new FilteredElementCollector( m_revitApp.ActiveUIDocument.Document );
        ElementClassFilter elementsAreWanted = new ElementClassFilter( typeof( FloorType ) );
        fec.WherePasses( elementsAreWanted );
        List<Element> elements = fec.ToElements() as List<Element>;

        foreach( Element element in elements )
        {
          FloorType fType = element as FloorType;

          if( fType == null )
          {
            continue;
          }

          if( fType.Name == "Generic - 12\"" )
          {
            floorType = fType;
          }
        }
      }
      catch( Exception e )
      {
        throw e;
      }

      // Set the stuctural value
      bool structural = true;

      Revit.ElementId elemId = new ElementId( 0 );

      // Create the floor instance
      try
      {
        if( level.Name == "Level 2" )
        {
          level.Elevation = 10.0;

          Floor f = doc.NewFloor( profile, floorType, level, structural );

          Revit.ElementId fId = f.Id;
          m_shedElements.Add( fId );

          // This param need to be set for any level above Level 1 for the floor to move to the correct level
          Revit.Parameter midFloorparam = f.get_Parameter( BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM );
          midFloorparam.Set( 0.0 );

          return f.LevelId;
        }

        if( level.Name == "Level 1" )
        {

          Floor f = doc.NewFloor( profile, floorType, level, structural );

          Revit.ElementId fId = f.Id;
          m_shedElements.Add( fId );

          return f.LevelId;
        }

        // if none of the types match 
        return elemId;
      }
      catch( Exception e )
      {
        throw e;
      }
    }

    /// <summary>
    /// Swap any selected door with a hardwired double door.
    /// </summary>
    private void
    SimpleSwap()
    {
      FamilySymbol doubleDoorSymbol = null;

      Revit.Document docu = m_revitApp.ActiveUIDocument.Document;
      Autodesk.Revit.Creation.Document doc = docu.Create;
      Autodesk.Revit.UI.Selection.Selection sel = m_revitApp.ActiveUIDocument.Selection;
      Autodesk.Revit.Creation.Application applic = m_revitApp.Application.Create;

      // First filter out to only Door elements

      //Revit.ElementSet doorSet = Utils.Selection.FilterToCategory( m_revitApp.ActiveUIDocument.Selection.Elements,
      //                                        Revit.BuiltInCategory.OST_Doors, false, m_revitApp.ActiveUIDocument.Document );

      //if( doorSet.IsEmpty )
      //{
      //  MessageBox.Show( "No door element is currently selected" );
      //  return;
      //}

      //ICollection<ElementId> doorSet 
      //  = Utils.Selection.FilterToCategory( 
      //    m_revitApp.ActiveUIDocument.Selection.GetElementIds(),
      //    Revit.BuiltInCategory.OST_Doors, false, docu );

      //if( 0 == doorSet.Count )
      //{
      //  MessageBox.Show( "No door element is currently selected" );
      //  return;
      //}

      // Load the concerned door family        
      FilteredElementCollector fec = new FilteredElementCollector( docu );
      ElementClassFilter elementsAreWanted = new ElementClassFilter( typeof( Family ) );
      fec.WherePasses( elementsAreWanted );
      List<Element> elements = fec.ToElements() as List<Element>;

      foreach( Element element in elements )
      {
        Family fam = element as Family;
        if( fam != null )
        {
          if( fam.Name == "Double-Glass 1" )
          {
            doubleDoorSymbol = Utils.FamilyUtil.GetFamilySymbol( fam, "72\" x 78\"" );
          }
        }
      }

      string fileName = string.Empty;
      bool success = false;

      // Load the required family

      fileName = "../Data/Platform/Imperial/Library/Architectural/Doors/Double-Glass 1.rfa";
      Family doubleDoorFamily = null;

      if( doubleDoorSymbol == null )
      {
        success = docu.LoadFamily( fileName, out doubleDoorFamily );
        doubleDoorSymbol = Utils.FamilyUtil.GetFamilySymbol( doubleDoorFamily, "72\" x 78\"" );
      }

      if( doubleDoorSymbol == null )
      {
        MessageBox.Show( "Please load Double-Glass 1 into project" );
        Utils.UserInput.LoadFamily( null, docu );
      }

      // Perform the swap.

      //Revit.ElementSet elemSet = sel.Elements; // 2015, jeremy: 'Autodesk.Revit.UI.Selection.Selection.Elements' is obsolete: 'This property is deprecated in Revit 2015. Use GetElementIds() and SetElementIds instead.'
      //System.Collections.IEnumerator iters = elemSet.GetEnumerator();

      //while( iters.MoveNext() )
      //{
      //  FamilyInstance famInst = (FamilyInstance) iters.Current;
      //  famInst.Symbol = doubleDoorSymbol;
      //}

      ICollection<ElementId> elemSet = sel.GetElementIds(); // 2016, jeremy

      foreach(ElementId id in elemSet )
      {
        FamilyInstance famInst = docu.GetElement( id ) as FamilyInstance;
        famInst.Symbol = doubleDoorSymbol;
      }
    }


    /// <summary>
    /// Demonstrates the increase of floor to floor height by iterating over a set of available
    /// levels.
    /// </summary>
    public void
    SimpleLevelIteration()
    {
      // Clear all related elements if a shed has already been created to prevent overlapping elements.
      m_revitApp.ActiveUIDocument.Document.Delete( m_shedElements );

      // Use the simple shed sample to demonstrate the increase in floor to floor height.
      SimpleShed();

      DialogResult dlgRes = MessageBox.Show( "This will increase the floor to floor height of the Simple Shed by 10 Ft \n \t\t Hit Cancel to see original shed.", "Test Framework", MessageBoxButtons.OKCancel, MessageBoxIcon.Information );

      if( dlgRes == System.Windows.Forms.DialogResult.OK )
      {

        ArrayList levels = new ArrayList();
        ArrayList elevations = new ArrayList();
        Hashtable hash = new Hashtable();

        FilteredElementCollector fec = new FilteredElementCollector( m_revitApp.ActiveUIDocument.Document );
        ElementClassFilter elementsAreWanted = new ElementClassFilter( typeof( Level ) );
        fec.WherePasses( elementsAreWanted );
        List<Element> elements = fec.ToElements() as List<Element>;

        foreach( Element element in elements )
        {
          Level systemLevel = element as Level;
          if( null != systemLevel )
          {
            // collect all the levels
            levels.Add( systemLevel );
          }
        }

        // Polulates a hashtable with the [key. value] pair represented as [Elevation, Level]
        // Also populate a list of elevations.

        for( Int32 i = 0; i < levels.Count; i++ )
        {

          Level lev = (Level) levels[i];

          if( !hash.ContainsKey( lev.Elevation ) && !hash.ContainsValue( lev ) )
          {
            hash.Add( lev.Elevation, lev );
          }

          elevations.Add( lev.Elevation );
        }

        // Sort the elevations
        elevations.Sort();

        Double elevation = (Double) elevations[0];

        // Now that we have the elevations sorted, get the levels corresponding to them and bump 
        // them up by a desired value.

        for( Int32 z = 1; z < elevations.Count; z++ )
        {

          Double elev = (Double) elevations[z];

          if( elev != 0.0 )
          {
            Level level = (Level) hash[elev];

            elevation = elevation + 20.0;
            level.Elevation = elevation;
          }
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public void
    Flip()
    {
      //Revit.ElementSet elemSet = m_revitApp.ActiveUIDocument.Selection.Elements; // 2015, jeremy: 'Autodesk.Revit.UI.Selection.Selection.Elements' is obsolete: 'This property is deprecated in Revit 2015. Use GetElementIds() and SetElementIds instead.'
      //if( elemSet.IsEmpty )
      //{
      //  MessageBox.Show( "Please select element(s) to flip" );
      //  return;
      //}

      Autodesk.Revit.DB.Document doc = m_revitApp.ActiveUIDocument.Document;

      ICollection<ElementId> elemSet = m_revitApp.ActiveUIDocument.Selection.GetElementIds(); // 2016, jeremy

      if( 0 == elemSet.Count )
      {
        MessageBox.Show( "Please select elements to flip" );
        return;
      }

      XYZ zDir = GeomUtils.kZAxis;
      XYZ flipPoint;
      Line zAxis = null;
      Revit.LocationCurve locCurve;
      Revit.LocationPoint locPt;
      bool success;

      //Revit.ElementSetIterator elemSetIter = elemSet.ForwardIterator();
      //while( elemSetIter.MoveNext() )

      foreach( ElementId id in elemSet )
      {
        success = false;
        //Revit.Element elem = elemSetIter.Current as Revit.Element;
        Revit.Element elem = doc.GetElement( id );
        Revit.Location location = elem.Location;

        // for elements like walls
        locCurve = location as Revit.LocationCurve;
        if( locCurve != null )
        {
          XYZ startPt = locCurve.Curve.GetEndPoint( 0 );
          XYZ endPt = locCurve.Curve.GetEndPoint( 1 );
          double x = ( endPt.X + startPt.X ) / 2;
          double y = ( endPt.Y + startPt.Y ) / 2;
          flipPoint = new XYZ( x, y, 0 );
          zAxis = Line.CreateUnbound( flipPoint, zDir );
        }

        // for familyinstances
        locPt = location as Revit.LocationPoint;
        if( locPt != null )
        {
          flipPoint = locPt.Point;
          zAxis = Line.CreateUnbound( flipPoint, zDir );
        }

        success = elem.Location.Rotate( zAxis, GeomUtils.kRad180 );
      }
    }

    /// <summary>
    /// Mirrors a selected modscope
    /// </summary>
    public void
    Mirror()
    {
      //Revit.ElementSet elemSet = m_revitApp.ActiveUIDocument.Selection.Elements; // 2015, jeremy: 'Autodesk.Revit.UI.Selection.Selection.Elements' is obsolete: 'This property is deprecated in Revit 2015. Use GetElementIds() and SetElementIds instead.'

      //if( elemSet.IsEmpty )
      //{
      //  MessageBox.Show( "Please select elements to be mirrored" );
      //  return;
      //}

      Autodesk.Revit.DB.Document doc = m_revitApp.ActiveUIDocument.Document;

      ICollection<ElementId> elemSet = m_revitApp.ActiveUIDocument.Selection.GetElementIds(); // 2016, jeremy

      if( 0 == elemSet.Count )
      {
        MessageBox.Show( "Please select elements to be mirrored" );
        return;
      }

      /// a map to hold original elementids and their corresponding
      /// clone elements
      Hashtable origAndCopy = new Hashtable();

      HostObject hostObj;
      Revit.LocationCurve locationCurve = null;
      
      //Revit.ElementSetIterator elemSetIter = elemSet.ForwardIterator();
      //while( elemSetIter.MoveNext() )

      // first pass will create only host elements, because without them
      // any hosted elements cannot be created 

      foreach( ElementId id in elemSet )
      {
        //Revit.Element elem = elemSetIter.Current as Revit.Element;

        Revit.Element elem = doc.GetElement( id );

        /// there are only 4 host objects, of which only wall 
        /// and floor can be created for now
        hostObj = elem as HostObject;
        if( hostObj != null )
        {

          Floor floor = elem as Floor;
          if( floor != null )
          {
            // get geometry to figure out location of floor
            Options options = m_revitApp.Application.Create.NewGeometryOptions();
            options.DetailLevel = ViewDetailLevel.Coarse;
            GeometryElement geomElem = floor.get_Geometry( options );
            Solid solid = null;
            foreach( GeometryObject geoObject in geomElem )
            {
              if( geoObject is Solid )
              {
                solid = geoObject as Solid;
                break;
              }
            }
            double absoluteElev = ( (Level) floor.Document.GetElement( floor.LevelId ) ).Elevation + floor.get_Parameter( BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM ).AsDouble();
            CurveArray curveArray = Utils.Geometry.GetProfile( solid, absoluteElev, m_revitApp.Application );

            // mirror the location
            CurveArray mirroredCurveArray = m_revitApp.Application.Create.NewCurveArray();
            for( int i = 0; i < curveArray.Size; i++ )
            {
              mirroredCurveArray.Append( Utils.Geometry.Mirror( curveArray.get_Item( i ), GeomUtils.kXAxis, m_revitApp.Application ) );
            }

            // create the mirrored floor
            Floor floorClone = m_revitApp.ActiveUIDocument.Document.Create.NewFloor( mirroredCurveArray, floor.FloorType, (Level) floor.Document.GetElement( floor.LevelId ), false );

            // give mirrored floor same param values as original floor
            Utils.ParamUtil.SetParameters( floorClone.Parameters, floor.Parameters );

            // save the original to mirrored floor mapping
            origAndCopy.Add( floor.Id.IntegerValue, floorClone );
            continue;
          }

          Revit.Element elemClone = Utils.Elements.CloneElement( m_revitApp.Application, elem );
          Revit.Location elemLocation = elemClone.Location;

          if( elemLocation != null )
          {
            locationCurve = elemLocation as Revit.LocationCurve;
            if( locationCurve != null )
            {
              locationCurve.Curve = Utils.Geometry.Mirror( locationCurve.Curve, GeomUtils.kXAxis, m_revitApp.Application );
            }
          }

          // save the mapping
          origAndCopy.Add( elem.Id.IntegerValue, elemClone );
        }
      }

      //elemSetIter.Reset();


      Revit.LocationPoint locationPoint = null;

      // second pass will put in the family instances. If it is hosted on an element
      // extract that info from the map and use it
      //while( elemSetIter.MoveNext() )

      foreach( ElementId id in elemSet )
      {
        //Revit.Element elem = elemSetIter.Current as Revit.Element;

        Revit.Element elem = doc.GetElement( id );

        hostObj = elem as HostObject;
        if( hostObj == null )
        {

          FamilyInstance famInst = elem as FamilyInstance;
          if( famInst != null )
          {

            XYZ pt = new XYZ();

            // get location of familyinstance
            locationPoint = famInst.Location as Revit.LocationPoint;
            if( locationPoint != null )
            {
              // mirror the location
              pt = Utils.Geometry.Mirror( locationPoint.Point, GeomUtils.kXAxis );
            }
            locationCurve = famInst.Location as Revit.LocationCurve;
            if( locationCurve != null )
            {
              // mirror the location
              pt = Utils.Geometry.Mirror( locationCurve.Curve.GetEndPoint( 0 ), GeomUtils.kXAxis );
            }

            // if family instance is hosted get it from the map
            Revit.Element elemHost = null;
            if( famInst.Host != null )
            {
              elemHost = origAndCopy[famInst.Host.Id.IntegerValue] as Revit.Element;
            }

            // create mirrored family instance
            FamilyInstance famInstClone = m_revitApp.ActiveUIDocument.Document.Create.NewFamilyInstance( pt, famInst.Symbol, elemHost, (Level) famInst.Document.GetElement( famInst.LevelId ), famInst.StructuralType );

            // give mirrored family instance same param values as original family instance
            Utils.ParamUtil.SetParameters( famInstClone.Parameters, famInst.Parameters );
            continue;
          }

          Grid grid = elem as Grid;
          if( grid != null )
          {
            Curve curve = grid.Curve;

            curve = Utils.Geometry.Mirror( curve, GeomUtils.kXAxis, m_revitApp.Application );

            Line line = curve as Line;
            if( line != null )
            {
              m_revitApp.ActiveUIDocument.Document.Create.NewGrid( line );
            }

            Arc arc = curve as Arc;
            if( arc != null )
            {
              m_revitApp.ActiveUIDocument.Document.Create.NewGrid( arc );
            }
            continue;
          }

          Opening opening = elem as Opening;
          if( opening != null )
          {
            // get host from the map
            Revit.Element elemHost = null;
            if( opening.Host != null )
            {
              elemHost = origAndCopy[opening.Host.Id.IntegerValue] as Revit.Element;
            }

            CurveArray curveArray = opening.BoundaryCurves;
            // mirror the location
            CurveArray mirroredCurveArray = m_revitApp.Application.Create.NewCurveArray();
            for( int i = 0; i < curveArray.Size; i++ )
            {
              mirroredCurveArray.Append( Utils.Geometry.Mirror( curveArray.get_Item( i ), GeomUtils.kXAxis, m_revitApp.Application ) );
            }

            m_revitApp.ActiveUIDocument.Document.Create.NewOpening( elemHost, mirroredCurveArray, true );
            continue;
          }

          // TBD: Dimension.Curve always throws an exception
          //Dimension dimension = elem as Dimension;
          //if (dimension != null) {
          //    Curve curve = dimension.Curve;
          //    curve = Utils.Geometry.Mirror(curve, GeomUtils.kXAxis, m_revitApp);

          //    Line line = curve as Line;
          //    if (line != null) {
          //        m_revitApp.ActiveUIDocument.Document.Create.NewDimension(dimension.View, line, dimension.References, dimension.DimensionType);
          //    }
          //    continue;
          //}


          Revit.Element elementClone = Utils.Elements.CloneElement( m_revitApp.Application, elem );
          if( elementClone != null )
          {

            BeamSystem beamSystem = elementClone as BeamSystem;
            if( beamSystem != null )
            {
              CurveArray curveArray = beamSystem.Profile;
              // mirror the location
              CurveArray mirroredCurveArray = m_revitApp.Application.Create.NewCurveArray();
              for( int i = 0; i < curveArray.Size; i++ )
              {
                mirroredCurveArray.Append( Utils.Geometry.Mirror( curveArray.get_Item( i ), GeomUtils.kXAxis, m_revitApp.Application ) );
              }
              beamSystem.Profile = mirroredCurveArray;
            }

            ModelCurve modelCurve = elementClone as ModelCurve;
            if( modelCurve != null )
            {
              Curve curve = modelCurve.GeometryCurve;
              curve = Utils.Geometry.Mirror( curve, GeomUtils.kXAxis, m_revitApp.Application );
              modelCurve.GeometryCurve = curve;
            }

            Revit.Location elemLocation = elementClone.Location;
            if( elemLocation != null )
            {
              locationCurve = elemLocation as Revit.LocationCurve;
              if( locationCurve != null )
              {
                locationCurve.Curve = Utils.Geometry.Mirror( locationCurve.Curve, GeomUtils.kXAxis, m_revitApp.Application );
              }
              locationPoint = elemLocation as Revit.LocationPoint;
              if( locationPoint != null )
              {
                locationPoint.Point = Utils.Geometry.Mirror( locationPoint.Point, GeomUtils.kXAxis );
              }
            }

            continue;
          }
        }
      }
    }

    /// <summary>
    /// Creates a new annotation symbol
    /// </summary>
    public void
    AnnoSymbol()
    {
      Revit.Document doc = m_revitApp.ActiveUIDocument.Document;

      FamilyItemFactory famFact = doc.FamilyCreate;

      AnnotationSymbolTypeSet annoSymTypeSet = new FilteredElementCollector( doc ).OfClass( typeof( AnnotationSymbol ) ).Cast<AnnotationSymbol>() as AnnotationSymbolTypeSet;
      // TBD: why is the size only 2
      Int32 size = annoSymTypeSet.Size;

      AnnotationSymbolTypeSetIterator annoSymTypeSetIter = annoSymTypeSet.ForwardIterator();

      AnnotationSymbolType annoSymType = null;

      while( annoSymTypeSetIter.MoveNext() )
      {
        AnnotationSymbolType tempAnnoSymType = annoSymTypeSetIter.Current as AnnotationSymbolType;

        if( null != tempAnnoSymType )
        {
          if( tempAnnoSymType.Name == "North Arrow 2" )
          {
            annoSymType = tempAnnoSymType;
          }
        }
      }

      if( annoSymType == null )
        return;

      XYZ location = GeomUtils.kOrigin;
      Autodesk.Revit.DB.View view = m_revitApp.ActiveUIDocument.Document.ActiveView;

      doc.Create.NewFamilyInstance( location, annoSymType, view );
    }

    /// <summary>
    /// Creates a new Beam System
    /// </summary>
    public void
    BeamSystemHardWired()
    {
      List<Curve> profile = new List<Curve>();

      XYZ location1 = GeomUtils.kOrigin;
      XYZ location2 = new XYZ( 0.0, 10.0, 0.0 );
      XYZ location3 = new XYZ( 10.0, 10.0, 0.0 );
      XYZ location4 = new XYZ( 10.0, 0.0, 0.0 );

      profile.Add( Line.CreateBound( location1, location2 ) );
      profile.Add( Line.CreateBound( location2, location3 ) );
      profile.Add( Line.CreateBound( location3, location4 ) );
      profile.Add( Line.CreateBound( location4, location1 ) );

      XYZ normal = GeomUtils.kZAxis;
      XYZ origin = GeomUtils.kOrigin;

      Plane plane = m_revitApp.Application.Create.NewPlane( normal, origin );

      SketchPlane sketchPlane = SketchPlane.Create( m_revitApp.ActiveUIDocument.Document, plane );
      BeamSystem.Create( m_revitApp.ActiveUIDocument.Document, profile, sketchPlane, new XYZ( 1.0, 1.0, 0.0 ), false );
    }

    /// <summary>
    /// Creates new Detail Curves
    /// </summary>
    public void
    DetailCurveHardWired()
    {
      CurveArray curveArray = m_revitApp.Application.Create.NewCurveArray();

      //"J"
      XYZ location1 = GeomUtils.kOrigin;
      XYZ location2 = new XYZ( 0.0, -20.0, 0.0 );
      XYZ location3 = new XYZ( -10.0, -20.0, 0.0 );
      XYZ ptOnCurve = new XYZ( -5.0, -25.0, 0.0 );
      curveArray.Append( Line.CreateBound( location1, location2 ) );
      curveArray.Append( Arc.Create( location2, location3, ptOnCurve ) );

      // "a"
      XYZ center = new XYZ( 10.0, -17.5, 0.0 );
      XYZ xVec = GeomUtils.kXAxis;
      XYZ yVec = GeomUtils.kYAxis;
      Ellipse ellipse = Ellipse.Create( center, 5.0, 8.0, xVec, yVec, 0.0, ( GeomUtils.kRevitPi * 2.0 ) );
      // the curve should be bound otherwise NewDetailCurveArray will reject it
      ellipse.MakeBound( 0.0, GeomUtils.kRevitPi * 2.0 );
      curveArray.Append( ellipse );

      XYZ end0 = new XYZ( 15.0, -17.5, 0.0 );
      XYZ end1 = new XYZ( 18.0, -25.5, 0.0 );
      ptOnCurve = new XYZ( 15.5, -23.0, 0.0 );
      curveArray.Append( Arc.Create( end0, end1, ptOnCurve ) );

      // "i"
      XYZ start = new XYZ( 22.0, -12.5, 0.0 );
      XYZ end = new XYZ( 22.0, -25.0, 0.0 );
      curveArray.Append( Line.CreateBound( start, end ) );

      XYZ normal = GeomUtils.kZAxis;
      XYZ origin = GeomUtils.kOrigin;

      Plane plane = m_revitApp.Application.Create.NewPlane( normal, origin );
      SketchPlane sketchPlane = SketchPlane.Create( m_revitApp.ActiveUIDocument.Document, plane );

      m_revitApp.ActiveUIDocument.Document.Create.NewDetailCurveArray( m_revitApp.ActiveUIDocument.Document.ActiveView,
                                                          curveArray );
    }

    /// <summary>
    /// Creates 2 hardwired lines and a dimension to measure 
    /// the distance between them
    /// </summary>
    public void
    DimensionHardWired()
    {
      XYZ location1 = GeomUtils.kOrigin;
      XYZ location2 = new XYZ( 20.0, 0.0, 0.0 );
      XYZ location3 = new XYZ( 0.0, 20.0, 0.0 );
      XYZ location4 = new XYZ( 20.0, 20.0, 0.0 );

      Curve curve1 = Line.CreateBound( location1, location2 );
      Curve curve2 = Line.CreateBound( location3, location4 );

      DetailCurve dCurve1 = null;
      DetailCurve dCurve2 = null;
      using( Transaction trans = new Transaction( m_revitApp.ActiveUIDocument.Document, "DimensionHardWired" ) )
      {
        trans.Start();

        if( !m_revitApp.ActiveUIDocument.Document.IsFamilyDocument )
        {
          dCurve1 = m_revitApp.ActiveUIDocument.Document.Create.NewDetailCurve( m_revitApp.ActiveUIDocument.Document.ActiveView,
                                                          curve1 );
          dCurve2 = m_revitApp.ActiveUIDocument.Document.Create.NewDetailCurve( m_revitApp.ActiveUIDocument.Document.ActiveView,
                                                          curve2 );
        }
        else
        {
          if( null != m_revitApp.ActiveUIDocument.Document.OwnerFamily && null != m_revitApp.ActiveUIDocument.Document.OwnerFamily.FamilyCategory )
          {
            if( !m_revitApp.ActiveUIDocument.Document.OwnerFamily.FamilyCategory.Name.Contains( "Detail" ) )
            {
              MessageBox.Show( "Please make sure you open a detail based family template.", "RevitLookup", MessageBoxButtons.OK, MessageBoxIcon.Information );
              return;
            }
          }
          dCurve1 = m_revitApp.ActiveUIDocument.Document.FamilyCreate.NewDetailCurve( m_revitApp.ActiveUIDocument.Document.ActiveView,
                                                          curve1 );
          dCurve2 = m_revitApp.ActiveUIDocument.Document.FamilyCreate.NewDetailCurve( m_revitApp.ActiveUIDocument.Document.ActiveView,
                                                          curve2 );
        }

        Line line = Line.CreateBound( location2, location4 );

        ReferenceArray refArray = new ReferenceArray();
        /// TBD: all curves return a null Reference, what am I missing?
        /// 01/12/07
        refArray.Append( dCurve1.GeometryCurve.Reference );
        refArray.Append( dCurve2.GeometryCurve.Reference );

        if( !m_revitApp.ActiveUIDocument.Document.IsFamilyDocument )
        {
          m_revitApp.ActiveUIDocument.Document.Create.NewDimension( m_revitApp.ActiveUIDocument.Document.ActiveView,
              line, refArray );
        }
        else
        {
          m_revitApp.ActiveUIDocument.Document.FamilyCreate.NewDimension( m_revitApp.ActiveUIDocument.Document.ActiveView,
            line, refArray );
        }
        trans.Commit();
      }
    }

    /// <summary>
    /// Creates hardwired foundation slabs
    /// </summary>
    public void
    FoundationSlabHardWired()
    {
      CurveArray curveArray = m_revitApp.Application.Create.NewCurveArray();

      XYZ location1 = GeomUtils.kOrigin;
      XYZ location2 = new XYZ( 0.0, 20.0, 0.0 );
      XYZ location3 = new XYZ( 20.0, 20.0, 0.0 );
      XYZ location4 = new XYZ( 20.0, 0.0, 0.0 );

      curveArray.Append( Line.CreateBound( location1, location2 ) );
      curveArray.Append( Line.CreateBound( location2, location3 ) );
      curveArray.Append( Line.CreateBound( location3, location4 ) );
      curveArray.Append( Line.CreateBound( location4, location1 ) );

      FloorTypeSet floorTypeSet = new FilteredElementCollector( m_revitApp.ActiveUIDocument.Document ).OfClass( typeof( FloorType ) ).Cast<FloorType>() as FloorTypeSet;
      FloorTypeSetIterator floorTypeSetIter = floorTypeSet.ForwardIterator();
      FloorType floorType = null;

      while( floorTypeSetIter.MoveNext() )
      {
        FloorType floorTypeTemp = floorTypeSetIter.Current as FloorType;
        if( floorTypeTemp.Name == "6\" Foundation Slab" )
        {
          floorType = floorTypeTemp;
          break;
        }

      }

      Level level = m_revitApp.ActiveUIDocument.Document.ActiveView.GenLevel;
      XYZ normal = GeomUtils.kZAxis;

      /// create a slab
      m_revitApp.ActiveUIDocument.Document.Create.NewFoundationSlab( curveArray, floorType, level, false, normal );

      /// floor slab is below all levels and is not visible in floor plan view!
      if( m_revitApp.ActiveUIDocument.Document.ActiveView.ViewType != ViewType.ThreeD
          && m_revitApp.ActiveUIDocument.Document.ActiveView.ViewType != ViewType.Elevation )
        MessageBox.Show( "Foundation slab created. Go to 3D or Elevation views to view" );
    }

    /// <summary>
    /// Creates a hardwired text note with a straight leader
    /// </summary>
    public void
    TextNoteHardWired()
    {
      Autodesk.Revit.DB.View view = m_revitApp.ActiveUIDocument.Document.ActiveView;

      /// create a wall to point to
      XYZ location1 = new XYZ( 21.0, 10.0, 0.0 );
      XYZ location2 = new XYZ( 21.0, -10.0, 0.0 );

      Line line = Line.CreateBound( location1, location2 );
      Wall.Create( m_revitApp.ActiveUIDocument.Document, line, view.GenLevel.Id, true );

      /// align text middle and center
      TextAlignFlags align = TextAlignFlags.TEF_ALIGN_MIDDLE ^ TextAlignFlags.TEF_ALIGN_CENTER;
      TextNote txtNote = m_revitApp.ActiveUIDocument.Document.Create.NewTextNote( view, GeomUtils.kOrigin, GeomUtils.kXAxis,
                                                                      view.ViewDirection, .25,
                                                                      align, "Simple wall" );
      /// add a straight leader
      txtNote.AddLeader( TextNoteLeaderTypes.TNLT_STRAIGHT_R );
    }


    /// <summary>
    /// Creates a hardwired slab.
    /// </summary>
    public void
    SimpleSlab()
    {
      XYZ startPt = new XYZ( 20.0, 0.0, 0.0 );
      XYZ endPt = new XYZ( 20.0, 20.0, 0.0 );

      CurveArray curveArray = m_revitApp.Application.Create.NewCurveArray();

      XYZ location1 = GeomUtils.kOrigin;
      XYZ location2 = new XYZ( 0.0, 10.0, 0.0 );
      XYZ location3 = new XYZ( 10.0, 10.0, 0.0 );
      XYZ location4 = new XYZ( 10.0, 0.0, 0.0 );

      curveArray.Append( Line.CreateBound( location1, location2 ) );
      curveArray.Append( Line.CreateBound( location2, location3 ) );
      curveArray.Append( Line.CreateBound( location3, location4 ) );
      curveArray.Append( Line.CreateBound( location4, location1 ) );

      Level level = m_revitApp.ActiveUIDocument.Document.ActiveView.GenLevel;
      Line line = Line.CreateBound( startPt, endPt );

      m_revitApp.ActiveUIDocument.Document.Create.NewSlab( curveArray, level, line, 45.0, false );
    }


    /// <summary>
    /// 
    /// </summary>
    public void
    SimpleViewSection()
    {
      Revit.Document doc = m_revitApp.ActiveUIDocument.Document;

      Random rnd = new Random();
      String viewName = "RevitLookup_ViewSection" + rnd.Next().ToString();

      FilteredElementCollector collector = new FilteredElementCollector( doc );
      ViewFamilyType viewFamType = collector.OfClass( typeof( ViewFamilyType ) ).Cast<ViewFamilyType>().Where( c => c.ViewFamily == ViewFamily.Detail ).FirstOrDefault();
      ViewSection viewSec = ViewSection.CreateDetail( doc, viewFamType.Id, doc.ActiveView.CropBox );
      viewSec.ViewName = viewName;
    }

    /// <summary>
    ///  
    /// </summary>
    public void
    FloorViewPlan()
    {
      Autodesk.Revit.Creation.Document doc = m_revitApp.ActiveUIDocument.Document.Create;

      // Just appending a random number for uniqueness. An exception will be thrown if the view name is not unique.
      Random rnd = new Random();
      String viewName = "RevitLookup_Plan" + rnd.Next().ToString();

      FilteredElementCollector collector = new FilteredElementCollector( m_revitApp.ActiveUIDocument.Document );
      ViewFamilyType viewFamType = collector.OfClass( typeof( ViewFamilyType ) ).Cast<ViewFamilyType>().Where( c => c.ViewFamily == ViewFamily.FloorPlan ).FirstOrDefault();
      ViewPlan createdPlan = ViewPlan.Create( m_revitApp.ActiveUIDocument.Document, viewFamType.Id, doc.NewLevel( 10.0 ).Id );
      createdPlan.ViewName = viewName;
    }


    /// <summary>
    /// Add the Level 1 (type = Floor Plan) view to a newly created sheet.
    /// </summary>
    public void
    ViewToNewSheetHardwired()
    {
      Revit.Document doc = m_revitApp.ActiveUIDocument.Document;

      // Collect all available views
      ViewSet allViews = Utils.View.GetAllViews( doc );

      // Collection of views that have been added to existing sheets.
      ViewSet usedViews = new ViewSet();

      // Collect all the existing sheets. It would be nice to have some API to do this.
      Revit.ElementSet allSheets = Utils.View.GetAllSheets( doc );

      // Create a new sheet
      ViewSheet sheet = ViewSheet.Create( doc, new FilteredElementCollector( doc ).OfClass( typeof( FamilySymbol ) ).OfCategory( BuiltInCategory.OST_TitleBlocks ).Cast<FamilySymbol>().LastOrDefault().Id );

      Random rnd = new Random();
      String name = "RevitLookup_Sheet" + rnd.Next().ToString();
      sheet.Name = name;

      foreach( ViewSheet viewSheet in allSheets )
      {
        // jeremy migrated from Revit 2014 to 2015:
        // 'Autodesk.Revit.DB.ViewSheet.Views' is obsolete: 'This property is obsolete in Revit 2015.  Use GetAllPlacedViews() instead.'
        //foreach( Autodesk.Revit.DB.View usedView in viewSheet.Views )
        
        foreach( ElementId id in viewSheet.GetAllPlacedViews() )
        {
          Autodesk.Revit.DB.View usedView = doc.GetElement( id ) 
            as Autodesk.Revit.DB.View;

          usedViews.Insert( usedView );
        }
      }

      // Note: The application does not allow the addition of a view to a new sheet if that view has already been added to
      // an existing sheet. I realized this when a programmatic attempt to do so resulted in an exception being thrown.
      // So running this hardwired test a second time would result in an exception (As "Level 1 (FloorPlan) would have already been added)". 
      // The workaround that has been used here is that first all the available sheets and their added views are gathered.
      // Before adding a view it is compared with the used views. arj - 1/17/07
      //            
      foreach( Autodesk.Revit.DB.View view in allViews )
      {
        // Has been hardwired for the following view.
        if( ( view.ViewName == "Level 1" ) && ( view.ViewType == ViewType.FloorPlan ) )
        {
          UV point = view.Outline.Max;

          // Display notification message if view is already being used.
          foreach( Autodesk.Revit.DB.View usedView in usedViews )
          {
            if( ( usedView.ViewName == view.ViewName ) && ( usedView.ViewType == view.ViewType ) )
            {
              MessageBox.Show( "View already added to an existing sheet", "RevitLookup Test", MessageBoxButtons.OK, MessageBoxIcon.Information );
              return;
            }
          }
          Viewport.Create( view.Document, sheet.Id, view.Id, new XYZ( point.U, point.V, 0 ) );
        }
      }
    }


    /// <summary>
    /// Add a simple tag to selected elements.
    /// </summary>
    public void
    SimpleTag()
    {
      Revit.Document docu = m_revitApp.ActiveUIDocument.Document;

      //Autodesk.Revit.UI.Selection.SelElementSet elemSet = m_revitApp.ActiveUIDocument.Selection.Elements; // 2015, jeremy: 'Autodesk.Revit.UI.Selection.SelElementSet' is obsolete: 'This class is deprecated in Revit 2015. Use Selection.SetElementIds() and Selection.GetElementIds() instead.'
      ICollection<ElementId> ids = m_revitApp.ActiveUIDocument.Selection.GetElementIds();  // 2016, jeremy

      Autodesk.Revit.DB.View view = m_revitApp.ActiveUIDocument.Document.ActiveView;

      XYZ tagLocation = new XYZ();

      //Revit.ElementSetIterator elemIter = elemSet.ForwardIterator();
      //while( elemIter.MoveNext() )

      foreach(ElementId id in ids )
      {
        //Revit.Element elem = elemIter.Current as Revit.Element;
        Revit.Element elem = docu.GetElement( id );

        Revit.Location location = elem.Location;

        // for elements like walls
        Revit.LocationCurve locCurve = location as Revit.LocationCurve;
        if( null != locCurve )
        {
          XYZ startPt = locCurve.Curve.GetEndPoint( 0 );
          XYZ endPt = locCurve.Curve.GetEndPoint( 1 );
          double x = ( endPt.X + startPt.X ) / 2;
          double y = ( endPt.Y + startPt.Y ) / 2;
          tagLocation = new XYZ( x, y, 0 );
        }

        // for familyinstances
        Revit.LocationPoint locPt = location as Revit.LocationPoint;
        if( null != locPt )
        {
          tagLocation = locPt.Point;
        }

        // Add the tag.
        if( null != elem )
        {
          m_revitApp.ActiveUIDocument.Document.Create.NewTag( view, elem, true, TagMode.TM_ADDBY_CATEGORY, TagOrientation.Horizontal, tagLocation );
        }
      }
    }

    /// <summary>
    /// Modify the width of a wall by changing the layer thickness.
    /// </summary>
    public void
    ModifyWallWidth()
    {
      Revit.Document doc = m_revitApp.ActiveUIDocument.Document;
      WallType outerWallType = null;

      FilteredElementCollector fec = new FilteredElementCollector( m_revitApp.ActiveUIDocument.Document );
      ElementClassFilter elementsAreWanted = new ElementClassFilter( typeof( WallType ) );
      fec.WherePasses( elementsAreWanted );
      List<Element> elements = fec.ToElements() as List<Element>;

      foreach( Element element in elements )
      {
        WallType wallType = element as WallType;

        if( wallType != null )
        {
          // hardwired to affect a Generic - 8\" Wall Type 
          if( wallType.Name == "Generic - 8\"" )
          {
            outerWallType = wallType;
          }
        }
      }

      Boolean success = UpdateHostCompoudStructures( outerWallType );
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="symbol"></param>
    /// <returns></returns>
    private Boolean
    UpdateHostCompoudStructures( ElementType symbol )
    {
      HostObjAttributes host = symbol as HostObjAttributes;

      if( host == null ) return false;

      foreach( CompoundStructureLayer layer in host.GetCompoundStructure().GetLayers() )
      {
        layer.Width = 15.0; // this should change the width of the wall.
      }
      return true;
    }
  }
}
