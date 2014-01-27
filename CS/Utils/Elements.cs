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
using System.Text;
using System.Diagnostics;

using Revit = Autodesk.Revit.DB;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB.Architecture;


namespace RevitLookup.Utils {
    /// <summary>
    /// 
    /// </summary>
    public class Elements {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Revit.Element CloneElement(Application app, Revit.Element element)
        {
            Opening opening = element as Opening;
            if (opening != null) {
                return CloneElement(app, opening);
            }

            BoundaryConditions boundaryConditions = element as BoundaryConditions;
            if (boundaryConditions != null) {
                return CloneElement(app, boundaryConditions);
            }

            AreaLoad areaLoad = element as AreaLoad;
            if (areaLoad != null) {
                return CloneElement(app, areaLoad);
            }

            AreaReinforcement areaReinforcement = element as AreaReinforcement;
            if (areaReinforcement != null) {
                return CloneElement(app, areaReinforcement);
            }

            BeamSystem beamSystem = element as BeamSystem;
            if (beamSystem != null) {
                return CloneElement(app, beamSystem);
            }

            Dimension dimension = element as Dimension;
            if (dimension != null) {
                return CloneElement(app, dimension);
            }

            FamilyInstance familyInstance = element as FamilyInstance;
            if (familyInstance != null) {
                return CloneElement(app, familyInstance);
            }

            Floor floor = element as Floor;
            if (floor != null) {
                return CloneElement(app, floor);
            }

            Grid grid = element as Grid;
            if (grid != null) {
                return CloneElement(app, grid);
            }

            Group group = element as Group;
            if (group != null) {
                return CloneElement(app, group);
            }

            Level level = element as Level;
            if (floor != null) {
                return CloneElement(app, floor);
            }

            LineLoad lineLoad = element as LineLoad;
            if (lineLoad != null) {
                return CloneElement(app, lineLoad);
            }

            LoadCase loadCase = element as LoadCase;
            if (loadCase != null) {
                return CloneElement(app, loadCase);
            }

            LoadCombination loadCombination = element as LoadCombination;
            if (loadCombination != null) {
                return CloneElement(app, loadCombination);
            }

            LoadNature loadNature = element as LoadNature;
            if (loadNature != null) {
                return CloneElement(app, loadNature);
            }

            LoadUsage loadUsage = element as LoadUsage;
            if (loadUsage != null) {
                return CloneElement(app, loadUsage);
            }

            ModelCurve modelCurve = element as ModelCurve;
            if (modelCurve != null) {
                return CloneElement(app, modelCurve);
            }

            PointLoad pointLoad = element as PointLoad;
            if (pointLoad != null) {
                return CloneElement(app, pointLoad);
            }

            Rebar rebar = element as Rebar;
            if (rebar != null) {
                return CloneElement(app, rebar);
            }

            ReferencePlane referencePlane = element as ReferencePlane;
            if (referencePlane != null) {
                return CloneElement(app, referencePlane);
            }

            Room room = element as Room;
            if (room != null) {
                return CloneElement(app, room);
            }

            RoomTag roomTag = element as RoomTag;
            if (roomTag != null) {
                return CloneElement(app, roomTag);
            }

            SketchPlane sketchPlane = element as SketchPlane;
            if (sketchPlane != null) {
                return CloneElement(app, sketchPlane);
            }

            View3D view3D = element as View3D;
            if (view3D != null) {
                return CloneElement(app, view3D);
            }

            ViewDrafting viewDrafting = element as ViewDrafting;
            if (viewDrafting != null) {
                return CloneElement(app, viewDrafting);
            }

            ViewSection viewSection = element as ViewSection;
            if (viewSection != null) {
                return CloneElement(app, viewSection);
            }

            ViewSheet viewSheet = element as ViewSheet;
            if (viewSheet != null) {
                return CloneElement(app, viewSheet);
            }

            Wall wall = element as Wall;
            if (wall != null) {
                return CloneElement(app, wall);
            }

            // this element has not yet been exposed in the Creation Document class
            //Debug.Assert(false);

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="opening"></param>
        /// <returns></returns>
        private static Revit.Element CloneElement(Autodesk.Revit.UI.UIApplication app, Opening opening)
        {
           Opening openingClone = app.ActiveUIDocument.Document.Create.NewOpening(opening.Host, opening.BoundaryCurves, true);
            Utils.ParamUtil.SetParameters(openingClone.Parameters, opening.Parameters);
            return openingClone;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="beamSystem"></param>
        /// <returns></returns>
        private static Revit.Element CloneElement(Autodesk.Revit.UI.UIApplication app, BeamSystem beamSystem)
        {
            List<Curve> profile = new List<Curve>();
            foreach (Curve curve in beamSystem.Profile)
            {
                profile.Add(curve);
            }
            BeamSystem beamSystemClone = BeamSystem.Create(app.ActiveUIDocument.Document, profile, beamSystem.Level, 0, false);
            Utils.ParamUtil.SetParameters(beamSystemClone.Parameters, beamSystem.Parameters);
            return beamSystemClone;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="wall"></param>
        /// <returns></returns>
        private static Revit.Element CloneElement(Autodesk.Revit.UI.UIApplication app, Wall wall)
        {
            Revit.LocationCurve locCurve = wall.Location as Revit.LocationCurve;
            bool isStructural = (wall.StructuralUsage == StructuralWallUsage.NonBearing) ? false : true;
            Wall wallClone = Wall.Create(app.ActiveUIDocument.Document, locCurve.Curve, wall.LevelId, isStructural);
            Utils.ParamUtil.SetParameters(wallClone.Parameters, wall.Parameters);
            return wallClone;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="familyInstance"></param>
        /// <returns></returns>
        private static Revit.Element CloneElement(Autodesk.Revit.UI.UIApplication app, FamilyInstance familyInstance)
        {
            XYZ location = new XYZ();

            // special case for something like a beam system which has a curve
            Revit.LocationCurve locationCurve = familyInstance.Location as Revit.LocationCurve;
            if (locationCurve != null) {
                location = locationCurve.Curve.GetEndPoint(0);
            }
            Revit.LocationPoint locationPoint = familyInstance.Location as Revit.LocationPoint;
            if (locationPoint != null) {
                location = locationPoint.Point;
            }

            FamilyInstance familyInstanceClone = app.ActiveUIDocument.Document.Create.NewFamilyInstance(location, familyInstance.Symbol, familyInstance.StructuralType);
            Utils.ParamUtil.SetParameters(familyInstanceClone.Parameters, familyInstance.Parameters);
            return familyInstanceClone;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="floor"></param>
        /// <returns></returns>
        private static Revit.Element CloneElement(Autodesk.Revit.UI.UIApplication app, Floor floor)
        {
            //get geometry to figure out location of floor
            Options options = app.Application.Create.NewGeometryOptions();
            options.DetailLevel = ViewDetailLevel.Coarse;
            GeometryElement geomElem = floor.get_Geometry(options);
            Solid solid = null;
            foreach(GeometryObject geoObject in geomElem)
            {
                if(geoObject is Solid)
                {
                    solid = geoObject as Solid;
                    break;
                }
            }
            Level level = floor.Document.GetElement(floor.LevelId) as Level;
            double absoluteElev = level.Elevation + floor.get_Parameter(BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM).AsDouble();
            CurveArray curveArray = Utils.Geometry.GetProfile(solid, absoluteElev, app.Application);

            Floor floorClone = app.ActiveUIDocument.Document.Create.NewFloor(curveArray, floor.FloorType, level, false);
            Utils.ParamUtil.SetParameters(floorClone.Parameters, floor.Parameters);
            return floorClone;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="dimension"></param>
        /// <returns></returns>
        private static Revit.Element CloneElement(Autodesk.Revit.UI.UIApplication app, Dimension dimension)
        {
            XYZ startPt = dimension.Curve.GetEndPoint(0);
            XYZ endPt = dimension.Curve.GetEndPoint(1);
            Line line = Line.CreateBound(startPt, endPt);
            Dimension dimensionClone = app.ActiveUIDocument.Document.Create.NewDimension(dimension.View, line, dimension.References);
            Utils.ParamUtil.SetParameters(dimensionClone.Parameters, dimension.Parameters);
            return dimensionClone;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="grid"></param>
        /// <returns></returns>
        private static Revit.Element CloneElement(Autodesk.Revit.UI.UIApplication app, Grid grid)
        {
            Grid gridClone = null;
            Curve curve = grid.Curve;

            Line line = grid.Curve as Line;
            if (line != null) {
               gridClone = app.ActiveUIDocument.Document.Create.NewGrid(line);
            }

            Arc arc = grid.Curve as Arc;
            if (arc != null) {
               gridClone = app.ActiveUIDocument.Document.Create.NewGrid(arc);
            }
            //Utils.ParamUtil.SetParameters(gridClone.Parameters, grid.Parameters);
            return gridClone;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        private static Revit.Element CloneElement(Autodesk.Revit.UI.UIApplication app, Group group)
        {
            IList<ElementId> elemList = group.GetMemberIds();
            Group groupClone = app.ActiveUIDocument.Document.Create.NewGroup(elemList);
            Utils.ParamUtil.SetParameters(groupClone.Parameters, group.Parameters);
            return groupClone;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        private static Revit.Element CloneElement(Autodesk.Revit.UI.UIApplication app, Level level)
        {
            Level levelClone = app.ActiveUIDocument.Document.Create.NewLevel(level.Elevation);
            Utils.ParamUtil.SetParameters(levelClone.Parameters, level.Parameters);
            return levelClone;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="modelCurve"></param>
        /// <returns></returns>
        private static Revit.Element CloneElement(Autodesk.Revit.UI.UIApplication app, ModelCurve modelCurve)
        {
            ModelCurve modelCurveClone = app.ActiveUIDocument.Document.Create.NewModelCurve(modelCurve.GeometryCurve, modelCurve.SketchPlane);
            Utils.ParamUtil.SetParameters(modelCurveClone.Parameters, modelCurve.Parameters);
            return modelCurveClone;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="referencePlane"></param>
        /// <returns></returns>
        private static Revit.Element CloneElement(Autodesk.Revit.UI.UIApplication app, ReferencePlane referencePlane)
        {
            ReferencePlane referencePlaneClone = app.ActiveUIDocument.Document.Create.NewReferencePlane(referencePlane.BubbleEnd, referencePlane.FreeEnd, referencePlane.Normal, app.ActiveUIDocument.Document.ActiveView);   // TBD: ReferencePlane.View dissappeared (jma - 12/05/06)
            Utils.ParamUtil.SetParameters(referencePlaneClone.Parameters, referencePlane.Parameters);
            return referencePlaneClone;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="room"></param>
        /// <returns></returns>
        private static Revit.Element CloneElement(Autodesk.Revit.UI.UIApplication app, Autodesk.Revit.DB.Architecture.Room room)
        {
            Revit.LocationPoint locationPoint = room.Location as Revit.LocationPoint;
            UV point = new UV(locationPoint.Point.X,locationPoint.Point.Y);
            
            Room roomClone = app.ActiveUIDocument.Document.Create.NewRoom((Level)room.Document.GetElement(room.LevelId), point);
            Utils.ParamUtil.SetParameters(roomClone.Parameters, room.Parameters);
            return roomClone;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="roomTag"></param>
        /// <returns></returns>
        private static Revit.Element CloneElement(Autodesk.Revit.UI.UIApplication app, Autodesk.Revit.DB.Architecture.RoomTag roomTag)
        {
            Revit.LocationPoint locationPoint = roomTag.Location as Revit.LocationPoint;
            UV point = new UV(locationPoint.Point.X, locationPoint.Point.Y);
            RoomTag roomTagClone = null;//app.ActiveUIDocument.Document.Create.NewRoomTag(roomTag.Room, ref point, );
            Utils.ParamUtil.SetParameters(roomTagClone.Parameters, roomTag.Parameters);
            return roomTagClone;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="sketchPlane"></param>
        /// <returns></returns>
        private static Revit.Element CloneElement(Autodesk.Revit.UI.UIApplication app, SketchPlane sketchPlane)
        {
            SketchPlane sketchPlaneClone = SketchPlane.Create(app.ActiveUIDocument.Document, sketchPlane.GetPlane());
            Utils.ParamUtil.SetParameters(sketchPlaneClone.Parameters, sketchPlane.Parameters);
            return sketchPlaneClone;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="view3D"></param>
        /// <returns></returns>
        private static Revit.Element CloneElement(Autodesk.Revit.UI.UIApplication app, View3D view3D)
        {
            XYZ viewDir = view3D.ViewDirection;
            View3D view3DClone = View3D.CreateIsometric(app.ActiveUIDocument.Document, view3D.GetTypeId());
            Utils.ParamUtil.SetParameters(view3DClone.Parameters, view3D.Parameters);
            return view3DClone;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="viewDrafting"></param>
        /// <returns></returns>
        private static Revit.Element CloneElement(Autodesk.Revit.UI.UIApplication app, ViewDrafting viewDrafting)
        {
            ViewDrafting viewDraftingClone = app.ActiveUIDocument.Document.Create.NewViewDrafting();
            Utils.ParamUtil.SetParameters(viewDraftingClone.Parameters, viewDrafting.Parameters);
            return viewDraftingClone;
        }
    }
}
