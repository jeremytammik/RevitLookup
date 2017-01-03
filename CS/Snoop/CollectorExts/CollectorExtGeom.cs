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
using System.Collections;
using System.Diagnostics;

using Autodesk.Revit.DB;

using RevitLookup.Snoop.Collectors;

namespace RevitLookup.Snoop.CollectorExts
{
    /// <summary>
    /// Provide Snoop.Data for any classes related to Geometry.
    /// </summary>

    public class CollectorExtGeom : CollectorExt
    {
        public CollectorExtGeom()
        {
        }

        protected override void
        CollectEvent(object sender, CollectorEventArgs e)
        {
            // cast the sender object to the SnoopCollector we are expecting
            Collector snoopCollector = sender as Collector;
            if (snoopCollector == null)
            {
                Debug.Assert(false);    // why did someone else send us the message?
                return;
            }

            // see if it is a type we are responsible for
            Location loc = e.ObjToSnoop as Location;
            if (loc != null)
            {
                Stream(snoopCollector.Data(), loc);
                return;
            }

            GeometryObject geomObj = e.ObjToSnoop as GeometryObject;
            if (geomObj != null)
            {
                Stream(snoopCollector.Data(), geomObj);
                return;
            }

            Options opts = e.ObjToSnoop as Options;
            if (opts != null)
            {
                Stream(snoopCollector.Data(), opts);
                return;
            }

            Transform trf = e.ObjToSnoop as Transform;
            if (trf != null)
            {
                Stream(snoopCollector.Data(), trf);
                return;
            }

            BoundingBoxXYZ bndBoxXyz = e.ObjToSnoop as BoundingBoxXYZ;
            if (bndBoxXyz != null)
            {
                Stream(snoopCollector.Data(), bndBoxXyz);
                return;
            }

            MeshTriangle meshTri = e.ObjToSnoop as MeshTriangle;
            if (meshTri != null)
            {
                Stream(snoopCollector.Data(), meshTri);
                return;
            }

            Reference reference = e.ObjToSnoop as Reference;
            if (reference != null)
            {
                Stream(snoopCollector.Data(), reference);
                return;
            }

            EdgeArray edgeArray = e.ObjToSnoop as EdgeArray;    // NOTE: this is needed because EdgeArrayArray will display enumerable Snoop items
            if (edgeArray != null)
            {
                Stream(snoopCollector.Data(), edgeArray);
                return;
            }

            CurveArray curveArray = e.ObjToSnoop as CurveArray;    // NOTE: this is needed because CurveArrayArray will display enumerable Snoop items
            if (curveArray != null)
            {
                Stream(snoopCollector.Data(), curveArray);
                return;
            }

            Plane plane = e.ObjToSnoop as Plane;
            if (plane != null)
            {
                Stream(snoopCollector.Data(), plane);
                return;
            }

            IntersectionResult intrResult = e.ObjToSnoop as IntersectionResult;
            if (intrResult != null)
            {
                Stream(snoopCollector.Data(), intrResult);
                return;
            }

            BoundingBoxUV bboxUV = e.ObjToSnoop as BoundingBoxUV;
            if (bboxUV != null)
            {
                Stream(snoopCollector.Data(), bboxUV);
                return;
            }

            SweepProfile sweepProf = e.ObjToSnoop as SweepProfile;
            if (sweepProf != null)
            {
                Stream(snoopCollector.Data(), sweepProf);
                return;
            }

            DimensionSegment dimSeg = e.ObjToSnoop as DimensionSegment;
            if (dimSeg != null)
            {
                Stream(snoopCollector.Data(), dimSeg);
                return;
            }

            UV uv = e.ObjToSnoop as UV;
            if (uv != null)
            {
                Stream(snoopCollector.Data(), uv);
                return;
            }
        }

        private void
        Stream(ArrayList data, Location loc)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(Location)));

            // Nothing at this level yet!

            LocationPoint locPt = loc as LocationPoint;
            if (locPt != null)
            {
                Stream(data, locPt);
                return;
            }

            LocationCurve locCrv = loc as LocationCurve;
            if (locCrv != null)
            {
                Stream(data, locCrv);
                return;
            }
        }

        private void
        Stream(ArrayList data, LocationPoint locPt)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(LocationPoint)));

            data.Add(new Snoop.Data.Xyz("Point", locPt.Point));

            // some point location instance do not support the rotation property and throw exception here.
            try
            {
                data.Add(new Snoop.Data.Angle("Rotation", locPt.Rotation));
            }
            catch (System.Exception ex)
            {
                data.Add(new Snoop.Data.String("Rotation", ex.Message));
            }
        }

        private void
        Stream(ArrayList data, LocationCurve locCrv)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(LocationCurve)));

            data.Add(new Snoop.Data.Object("Curve", locCrv.Curve));
        }

        private void
        Stream(ArrayList data, GeometryObject geomObj)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(GeometryObject)));

            data.Add(new Snoop.Data.Bool("IsElementGeometry", geomObj.IsElementGeometry));

            data.Add(new Snoop.Data.String("Visibility", geomObj.Visibility.ToString()));
            data.Add(new Snoop.Data.ElementId("GraphicStyleId", geomObj.GraphicsStyleId, m_activeDoc));

            data.Add(new Snoop.Data.String("Type", geomObj.GetType().Name));

            Curve crv = geomObj as Curve;
            if (crv != null)
            {
                Stream(data, crv);
                return;
            }

            Edge edg = geomObj as Edge;
            if (edg != null)
            {
                Stream(data, edg);
                return;
            }

            GeometryElement elem = geomObj as GeometryElement;
            if (elem != null)
            {
                Stream(data, elem);
                return;
            }

            Face face = geomObj as Face;
            if (face != null)
            {
                Stream(data, face);
                return;
            }

            GeometryInstance inst = geomObj as GeometryInstance;
            if (inst != null)
            {
                Stream(data, inst);
                return;
            }

            Mesh mesh = geomObj as Mesh;
            if (mesh != null)
            {
                Stream(data, mesh);
                return;
            }

            Profile prof = geomObj as Profile;
            if (prof != null)
            {
                Stream(data, prof);
                return;
            }

            Solid solid = geomObj as Solid;
            if (solid != null)
            {
                Stream(data, solid);
                return;
            }

            Point point = geomObj as Point;
            if (point != null)
            {
                Stream(data, point);
                return;
            }
        }

        public enum DataType
        {
            Angle = 0,
            BindingMap = 1,
            Bool = 2,
            CategoryNameMap = 3,
            Double = 4,
            Int = 5,
            Object = 6,
            String = 7,
            Uv = 8,
            Xyz = 9,
        }

        public void DataAdd(ArrayList data, string propertyName, DataType dataType, object value)
        {
            try
            {
                switch (dataType)
                {
                    case DataType.Angle:
                        data.Add(new Snoop.Data.Angle(propertyName, (double)value));
                        break;
                    case DataType.BindingMap:
                        data.Add(new Snoop.Data.BindingMap(propertyName, (Autodesk.Revit.DB.BindingMap)value));
                        break;
                    case DataType.Bool:
                        data.Add(new Snoop.Data.Bool(propertyName, (bool)value));
                        break;
                    case DataType.CategoryNameMap:
                        data.Add(new Snoop.Data.CategoryNameMap(propertyName, (Autodesk.Revit.DB.CategoryNameMap)value));
                        break;
                    case DataType.Double:
                        data.Add(new Snoop.Data.Double(propertyName, (double)value));
                        break;
                    case DataType.Int:
                        data.Add(new Snoop.Data.Int(propertyName, (int)value));
                        break;
                    case DataType.Object:
                        data.Add(new Snoop.Data.Object(propertyName, value));
                        break;
                    case DataType.String:
                        data.Add(new Snoop.Data.String(propertyName, (String)value));
                        break;
                    case DataType.Uv:
                        data.Add(new Snoop.Data.Uv(propertyName, (Autodesk.Revit.DB.UV)value));
                        break;
                    case DataType.Xyz:
                        data.Add(new Snoop.Data.Xyz(propertyName, (Autodesk.Revit.DB.XYZ)value));
                        break;
                }
            }
            catch (System.Exception ex)
            {
                data.Add(new Snoop.Data.Exception(propertyName, ex));
            }
        }

        private void
        Stream(ArrayList data, Curve crv)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(Curve)));


            try
            {
                data.Add(new Snoop.Data.Double("Approximate length", crv.ApproximateLength));
            }
            catch (System.Exception ex)
            {
                data.Add(new Snoop.Data.Exception("Approximate length", ex));
            }

            try
            {
                data.Add(new Snoop.Data.Double("Length", crv.Length));
            }
            catch (System.Exception ex)
            {
                data.Add(new Snoop.Data.Exception("Length", ex));
            }

            try
            {
                data.Add(new Snoop.Data.Double("Period", crv.Period));
            }
            catch (System.Exception ex)
            {
                data.Add(new Snoop.Data.Exception("Period", ex));
            }

            try
            {
                data.Add(new Snoop.Data.Bool("Is bound", crv.IsBound));
            }
            catch (System.Exception ex)
            {
                data.Add(new Snoop.Data.Exception("Is bound", ex));
            }

            try
            {
                data.Add(new Snoop.Data.Bool("Is cyclic", crv.IsCyclic));
            }
            catch (System.Exception ex)
            {
                data.Add(new Snoop.Data.Exception("Is cyclic", ex));
            }

            try
            {
                data.Add(new Snoop.Data.Xyz("Start point", crv.GetEndPoint(0)));
                data.Add(new Snoop.Data.Xyz("End point", crv.GetEndPoint(1)));
            }
            catch (System.Exception)
            {
                // if the curve is unbound, those don't mean anything
                data.Add(new Snoop.Data.String("Start point", "N/A"));
                data.Add(new Snoop.Data.String("End point", "N/A"));
            }

            try
            {
                data.Add(new Snoop.Data.Double("Start parameter", crv.GetEndParameter(0)));
                data.Add(new Snoop.Data.Double("End parameter", crv.GetEndParameter(1)));
            }
            catch (System.Exception)
            {
                // if the curve is unbound, those don't mean anything
                data.Add(new Snoop.Data.String("Start parameter", "N/A"));
                data.Add(new Snoop.Data.String("End parameter", "N/A"));
            }

            try
            {
                data.Add(new Snoop.Data.Object("Start point reference", crv.GetEndPointReference(0)));
                data.Add(new Snoop.Data.Object("End point reference", crv.GetEndPointReference(1)));
            }
            catch (System.Exception)
            {
                // if the curve is unbound, those don't mean anything
                data.Add(new Snoop.Data.String("Start point reference", "N/A"));
                data.Add(new Snoop.Data.String("End point reference", "N/A"));
            }

         try
         {
            data.Add(new Snoop.Data.Object("Reference", crv.Reference));
         }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("Reference", ex));
         }

         try
         {
            data.Add(new Snoop.Data.Object("Derivative at Start", crv.ComputeDerivatives(0.0, normalized: true)));
         }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("Derivative at Start", ex));
         }

         try
         {
            data.Add(new Snoop.Data.Object("Derivative at Center", crv.ComputeDerivatives(0.5, normalized: true)));
         }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("Derivative at Center", ex));
         }

         try
         {
            data.Add(new Snoop.Data.Object("Derivative at End", crv.ComputeDerivatives(1.0, normalized: true)));
         }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("Derivative at End", ex));
         }

            try
            {
                data.Add(new Snoop.Data.CategorySeparator("Tesselated Points"));
            }
            catch
            {
                data.Add(new Snoop.Data.String("Tesselated Points", "N/A"));
            }

            try
            {
                System.Collections.Generic.IList<XYZ> pts = crv.Tessellate();
                int i = 0;
                foreach (XYZ pt in pts)
                {
                    data.Add(new Snoop.Data.Xyz(string.Format("PT [{0:d}]", i++), pt));
                }
            }
            catch (System.Exception ex)
            {
                data.Add(new Snoop.Data.Exception("Xyz", ex));
            }

            Line line = crv as Line;
            if (line != null)
            {
                Stream(data, line);
                return;
            }

            Arc arc = crv as Arc;
            if (arc != null)
            {
                Stream(data, arc);
                return;
            }

            Ellipse ellipse = crv as Ellipse;
            if (ellipse != null)
            {
                Stream(data, ellipse);
                return;
            }

            NurbSpline nurbSpline = crv as NurbSpline;
            if (nurbSpline != null)
            {
                Stream(data, nurbSpline);
                return;
            }

            HermiteSpline hermiteSpline = crv as HermiteSpline;
            if (hermiteSpline != null)
            {
                Stream(data, hermiteSpline);
                return;
            }
        }

        private void
        Stream(ArrayList data, Line line)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(Line)));

         data.Add(new Snoop.Data.Xyz("Origin", line.Origin));
         data.Add(new Snoop.Data.Xyz("Direction", line.Direction));
      }

        private void
        Stream(ArrayList data, Arc arc)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(Arc)));

            data.Add(new Snoop.Data.Xyz("Center", arc.Center));
            data.Add(new Snoop.Data.Xyz("Normal", arc.Normal));
            data.Add(new Snoop.Data.Double("Radius", arc.Radius));
        }

        private void
        Stream(ArrayList data, Ellipse ellipse)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(Ellipse)));

            data.Add(new Snoop.Data.Xyz("Center", ellipse.Center));
            data.Add(new Snoop.Data.Xyz("Normal", ellipse.Normal));
            data.Add(new Snoop.Data.Double("Radius X", ellipse.RadiusX));
            data.Add(new Snoop.Data.Double("Radius Y", ellipse.RadiusY));
        }

        private void
        Stream(ArrayList data, NurbSpline nurbSpline)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(NurbSpline)));

            data.Add(new Snoop.Data.Enumerable("Control points", nurbSpline.CtrlPoints));
            data.Add(new Snoop.Data.Int("Degree", nurbSpline.Degree));
            data.Add(new Snoop.Data.Bool("Is closed", nurbSpline.isClosed));    // TBD: should be upper-case Is
            data.Add(new Snoop.Data.Bool("Is rational", nurbSpline.isRational));    // TBD: should be upper-case Is
            data.Add(new Snoop.Data.Enumerable("Knots", nurbSpline.Knots));
            data.Add(new Snoop.Data.Enumerable("Weights", nurbSpline.Weights));
        }


        private void
        Stream(ArrayList data, HermiteSpline hermiteSpline)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(HermiteSpline)));

            data.Add(new Snoop.Data.Enumerable("Control points", hermiteSpline.ControlPoints));
            data.Add(new Snoop.Data.Bool("Is periodic", hermiteSpline.IsPeriodic));
            data.Add(new Snoop.Data.Enumerable("Parameters", hermiteSpline.Parameters));
            data.Add(new Snoop.Data.Enumerable("Tangents", hermiteSpline.Tangents));
        }

        private void
        Stream(ArrayList data, Edge edge)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(Edge)));

         // Curve Type
         {
            string curveType = "None";
            if (edge.AsCurve() != null)
            {
               Curve crv = edge.AsCurve();
               if (crv is Arc)
                  curveType = "Arc";
               else if (crv is CylindricalHelix)
                  curveType = "CylindricalHelix";
               else if (crv is Ellipse)
                  curveType = "Ellipse";
               else if (crv is HermiteSpline)
                  curveType = "HermiteSpline";
               else if (crv is Line)
                  curveType = "Line";
               else if (crv is NurbSpline)
                  curveType = "NurbSpline";
            }
            data.Add(new Snoop.Data.String("Curve Type", curveType));
         }

         try
         {
            data.Add(new Snoop.Data.Object("Curve", edge.AsCurve()));
         }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("Curve", ex));
         }

         try
         {
           data.Add(new Snoop.Data.Object("Start point reference", edge.GetEndPointReference(0)));
         }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("Start point reference", ex));
         }

            try
            {
                data.Add(new Snoop.Data.Object("End point reference", edge.GetEndPointReference(1)));
            }
            catch (System.Exception ex)
            {
                data.Add(new Snoop.Data.Exception("End point reference", ex));
            }

            data.Add(new Snoop.Data.Object("Reference", edge.Reference));
            data.Add(new Snoop.Data.Double("Approximate length", edge.ApproximateLength));
            data.Add(new Snoop.Data.Object("Face [0]", edge.GetFace(0)));
            data.Add(new Snoop.Data.Object("Face [1]", edge.GetFace(1)));

            data.Add(new Snoop.Data.CategorySeparator("Tesselated Points"));

            System.Collections.Generic.IList<XYZ> pts = edge.Tessellate();
            int i = 0;
            foreach (XYZ pt in pts)
            {
                data.Add(new Snoop.Data.Xyz(string.Format("PT [{0:d}]", i++), pt));
            }

            // TBD: not sure how to use these yet...
            // TesselateOnFace ??
        }

        private void
        Stream(ArrayList data, Face face)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(Face)));

            data.Add(new Snoop.Data.Double("Area", face.Area));
            ///TODO: Restore: data.Add(new Snoop.Data.Object("Material element", face.MaterialElement));
            data.Add(new Snoop.Data.Bool("Is two-sided", face.IsTwoSided));
            data.Add(new Snoop.Data.Enumerable("Edge loops", face.EdgeLoops));
            data.Add(new Snoop.Data.Object("Triangulate", face.Triangulate()));
            data.Add(new Snoop.Data.Object("Reference", face.Reference));

            ConicalFace conicalFace = face as ConicalFace;
            if (conicalFace != null)
            {
                Stream(data, conicalFace);
                return;
            }

            CylindricalFace cylFace = face as CylindricalFace;
            if (cylFace != null)
            {
                Stream(data, cylFace);
                return;
            }

            HermiteFace hermiteFace = face as HermiteFace;
            if (hermiteFace != null)
            {
                Stream(data, hermiteFace);
                return;
            }

            PlanarFace planarFace = face as PlanarFace;
            if (planarFace != null)
            {
                Stream(data, planarFace);
                return;
            }

            RevolvedFace revlFace = face as RevolvedFace;
            if (revlFace != null)
            {
                Stream(data, revlFace);
                return;
            }

            RuledFace ruledFace = face as RuledFace;
            if (ruledFace != null)
            {
                Stream(data, ruledFace);
                return;
            }
        }

        private void
        Stream(ArrayList data, ConicalFace face)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(ConicalFace)));

            data.Add(new Snoop.Data.Xyz("Origin", face.Origin));
            data.Add(new Snoop.Data.Xyz("Axis", face.Axis));
            data.Add(new Snoop.Data.Angle("Half angle", face.HalfAngle));
        }

        private void
        Stream(ArrayList data, CylindricalFace face)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(CylindricalFace)));

            data.Add(new Snoop.Data.Xyz("Origin", face.Origin));
            data.Add(new Snoop.Data.Xyz("Axis", face.Axis));
        }

        private void
        Stream(ArrayList data, HermiteFace face)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(HermiteFace)));

            data.Add(new Snoop.Data.Enumerable("Mixed derivatives", face.MixedDerivs));
            data.Add(new Snoop.Data.Enumerable("Points", face.Points));
        }

        private void
        Stream(ArrayList data, PlanarFace face)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(PlanarFace)));

            data.Add(new Snoop.Data.Xyz("Origin", face.Origin));
            data.Add(new Snoop.Data.Xyz("Normal", face.ComputeNormal(UV.Zero)));
        }

        private void
        Stream(ArrayList data, RevolvedFace face)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(RevolvedFace)));

            data.Add(new Snoop.Data.Xyz("Origin", face.Origin));
            data.Add(new Snoop.Data.Xyz("Axis", face.Axis));
            data.Add(new Snoop.Data.Object("Curve", face.Curve));
        }

        private void
        Stream(ArrayList data, RuledFace face)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(RuledFace)));

            // TBD: get_Curve(int), get_Point(int) ???
        }

        private void
        Stream(ArrayList data, GeometryElement elem)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(GeometryElement)));

            ///TODO: Restore: data.Add(new Snoop.Data.Object("Material element", elem.MaterialElement));
            data.Add(new Snoop.Data.Enumerable("Geometry Objects", elem));
        }

        private void
        Stream(ArrayList data, GeometryInstance inst)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(GeometryInstance)));

            data.Add(new Snoop.Data.Object("Symbol", inst.Symbol));
            data.Add(new Snoop.Data.Object("Symbol geometry", inst.SymbolGeometry));
            data.Add(new Snoop.Data.Object("Transform", inst.Transform));
        }

        private void
        Stream(ArrayList data, Mesh mesh)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(Mesh)));

            // TBD: need a MeshTriangleIterator like the other classes, and then we could use Snoop.Data.Enumerable
            data.Add(new Snoop.Data.CategorySeparator("Triangles"));
            data.Add(new Snoop.Data.Int("Number of triangles", mesh.NumTriangles));
            for (int i = 0; i < mesh.NumTriangles; i++)
            {
                data.Add(new Snoop.Data.Object(string.Format("Triangle [{0:d}]", i), mesh.get_Triangle(i)));
            }

            data.Add(new Snoop.Data.CategorySeparator("Vertices"));
            System.Collections.Generic.IList<XYZ> pts = mesh.Vertices;
            int j = 0;
            foreach (XYZ pt in pts)
            {
                data.Add(new Snoop.Data.Xyz(string.Format("PT [{0:d}]", j++), pt));
            }
        }

        private void
        Stream(ArrayList data, Profile prof)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(Profile)));

            data.Add(new Snoop.Data.Enumerable("Curves", prof.Curves));
            data.Add(new Snoop.Data.Bool("Filled", prof.Filled));
        }

        private void
        Stream(ArrayList data, Solid solid)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(Solid)));

            data.Add(new Snoop.Data.Enumerable("Edges", solid.Edges));
            data.Add(new Snoop.Data.Enumerable("Faces", solid.Faces));
            data.Add(new Snoop.Data.Double("Surface area", solid.SurfaceArea));
            data.Add(new Snoop.Data.Double("Volume", solid.Volume));
        }

        private void
        Stream(ArrayList data, Point pt)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(Point)));

            data.Add(new Snoop.Data.Xyz("Coord", pt.Coord));
        }

        private void
        Stream(ArrayList data, Options opts)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(Options)));

            data.Add(new Snoop.Data.Bool("Compute references", opts.ComputeReferences));
            data.Add(new Snoop.Data.String("Detail level", opts.DetailLevel.ToString()));
            data.Add(new Snoop.Data.Object("View", opts.View));
        }

        private void
        Stream(ArrayList data, Transform trf)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(Transform)));

         data.Add(new Snoop.Data.Xyz("Origin", trf.Origin));
         data.Add(new Snoop.Data.Xyz("X axis", trf.BasisX));
         data.Add(new Snoop.Data.Xyz("Y axis", trf.BasisY));
         data.Add(new Snoop.Data.Xyz("Z axis", trf.BasisZ));
         data.Add(new Snoop.Data.Double("Determinant", trf.Determinant));
         try
         {
            data.Add(new Snoop.Data.Bool("Has reflection", trf.HasReflection));
         }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("Has reflection", ex));         	
         }
         try
         {
            data.Add(new Snoop.Data.Object("Inverse", trf.Inverse));            
         }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("Inverse", ex));
         }
         data.Add(new Snoop.Data.Bool("Is conformal", trf.IsConformal));
         data.Add(new Snoop.Data.Bool("Is identity", trf.IsIdentity));
         data.Add(new Snoop.Data.Bool("Is translation", trf.IsTranslation));
         try
         {
            data.Add(new Snoop.Data.Double("Scale", trf.Scale));
         }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("Scale", ex));
         }
         data.Add(new Snoop.Data.Object("Identity", Transform.Identity));
      }

        private void
        Stream(ArrayList data, BoundingBoxUV bndBox)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(BoundingBoxUV)));

            data.Add(new Snoop.Data.Uv("Min", bndBox.Min));
            data.Add(new Snoop.Data.Uv("Max", bndBox.Max));
        }


        private void
        Stream(ArrayList data, BoundingBoxXYZ bndBox)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(BoundingBoxXYZ)));

            data.Add(new Snoop.Data.Bool("Enabled", bndBox.Enabled));
            data.Add(new Snoop.Data.Xyz("Min", bndBox.Min));
            data.Add(new Snoop.Data.Xyz("Max", bndBox.Max));
            data.Add(new Snoop.Data.Object("Transform", bndBox.Transform));
        }

        private void
        Stream(ArrayList data, MeshTriangle meshTri)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(MeshTriangle)));

            // TBD: not sure what get_index() is all about??

            data.Add(new Snoop.Data.Xyz("Vertex [0]", meshTri.get_Vertex(0)));
            data.Add(new Snoop.Data.Xyz("Vertex [1]", meshTri.get_Vertex(1)));
            data.Add(new Snoop.Data.Xyz("Vertex [2]", meshTri.get_Vertex(2)));
        }

        private void
        Stream(ArrayList data, Plane plane)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(Plane)));

            data.Add(new Snoop.Data.Xyz("Origin", plane.Origin));
            data.Add(new Snoop.Data.Xyz("Normal", plane.Normal));
            data.Add(new Snoop.Data.Xyz("X vec", plane.XVec));
            data.Add(new Snoop.Data.Xyz("Y vec", plane.YVec));
        }

        private void
        Stream(ArrayList data, EdgeArray edgeArray)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(EdgeArray)));

            IEnumerator iter = edgeArray.GetEnumerator();
            int i = 0;
            while (iter.MoveNext())
            {
                data.Add(new Snoop.Data.Object(string.Format("Edge {0:d}", i++), iter.Current));
            }
        }

        private void
        Stream(ArrayList data, CurveArray curveArray)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(CurveArray)));

            IEnumerator iter = curveArray.GetEnumerator();
            int i = 0;
            while (iter.MoveNext())
            {
                data.Add(new Snoop.Data.Object(string.Format("Curve {0:d}", i++), iter.Current));
            }
        }

        private void
        Stream(ArrayList data, Reference reference)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(Reference)));

         Element elem = null;
         try
         {
            elem = m_activeDoc.GetElement(reference);
            data.Add(new Snoop.Data.Object("Element", elem));
         }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("Element", ex));
         }

         try
         {
            if (elem != null)
               data.Add(new Snoop.Data.Object("GeometryObject", elem.GetGeometryObjectFromReference(reference)));
         }
         catch (System.Exception ex)
         {
            data.Add(new Snoop.Data.Exception("GeometryObject", ex));
         }

         data.Add(new Snoop.Data.Object("ElementReferenceType", reference.ElementReferenceType));

        try
        {
          data.Add(new Snoop.Data.Xyz("GlobalPoint", reference.GlobalPoint));
        }
        catch (System.Exception ex) 
        {
          data.Add(new Snoop.Data.Exception("GlobalPoint", ex));
        }            
              
            data.Add(new Snoop.Data.ElementId("LinkedElementId", reference.LinkedElementId, m_activeDoc));
            try
            {
              data.Add(new Snoop.Data.Uv("UVPoint", reference.UVPoint));
            }
            catch (System.Exception ex)
            {
              data.Add(new Snoop.Data.Exception("UVPoint", ex));
            }
          // no data at this level
        }

        private void
        Stream(ArrayList data, IntersectionResult intrResult)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(IntersectionResult)));

            data.Add(new Snoop.Data.Double("Distance", intrResult.Distance));
            data.Add(new Snoop.Data.Object("Edge object", intrResult.EdgeObject));
            data.Add(new Snoop.Data.Double("Edge parameter", intrResult.EdgeParameter));
            data.Add(new Snoop.Data.Double("Parameter", intrResult.Parameter));
            data.Add(new Snoop.Data.Uv("UV Point", intrResult.UVPoint));
            data.Add(new Snoop.Data.Xyz("XYZ Point", intrResult.XYZPoint));
        }

        private void
        Stream(ArrayList data, SweepProfile sweepProf)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(SweepProfile)));

            // Nothing at this level yet!

            CurveLoopsProfile curLoopsProf = sweepProf as CurveLoopsProfile;
            if (curLoopsProf != null)
            {
                Stream(data, curLoopsProf);
                return;
            }

            FamilySymbolProfile famSymProf = sweepProf as FamilySymbolProfile;
            if (famSymProf != null)
            {
                Stream(data, famSymProf);
                return;
            }
        }

        private void
        Stream(ArrayList data, CurveLoopsProfile curLoopsProf)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(CurveLoopsProfile)));

            data.Add(new Snoop.Data.Enumerable("Profile", curLoopsProf.Profile));
        }

        private void
        Stream(ArrayList data, FamilySymbolProfile famSymProf)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(FamilySymbolProfile)));

            data.Add(new Snoop.Data.Double("Angle", famSymProf.Angle));
            data.Add(new Snoop.Data.Bool("Is flipped", famSymProf.IsFlipped));
            data.Add(new Snoop.Data.Object("Profile", famSymProf.Profile));
            data.Add(new Snoop.Data.Double("X offset", famSymProf.XOffset));
            data.Add(new Snoop.Data.Double("Y offset", famSymProf.YOffset));
        }

        private void
        Stream(ArrayList data, DimensionSegment dimSeg)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(DimensionSegment)));

            data.Add(new Snoop.Data.Bool("Is locked", dimSeg.IsLocked));
            data.Add(new Snoop.Data.Xyz("Origin", dimSeg.Origin));
            data.Add(new Snoop.Data.Double("Value", dimSeg.Value.Value));
            data.Add(new Snoop.Data.String("Value string", dimSeg.ValueString));
        }

        private void
        Stream(ArrayList data, UV UV)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(UV)));

            data.Add(new Snoop.Data.Uv("Basis U", UV.BasisU));
            data.Add(new Snoop.Data.Uv("Basis V", UV.BasisV));
            //data.Add(new Snoop.Data.Bool("Is normalized", UV.IsNormalized));
            //data.Add(new Snoop.Data.Bool("Is zero", UV.IsZero));
            //data.Add(new Snoop.Data.Double("Length", UV.));
            data.Add(new Snoop.Data.Uv("Normalized", UV.Normalize()));
            data.Add(new Snoop.Data.Double("U", UV.U));
            data.Add(new Snoop.Data.Double("V", UV.V));
            data.Add(new Snoop.Data.Uv("Zero", UV.Zero));
        }
    }
}
