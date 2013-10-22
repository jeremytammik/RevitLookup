using System;
using System.Collections.Generic;
using System.Text;

using Revit = Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;

namespace RevitLookup.Utils {
    /// <summary>
    /// 
    /// </summary>
    public class Geometry {
        /// <summary>
        /// Given a solid, returns a curve array containing the edges
        /// which are visible in plan view
        /// </summary>
        /// <param name="solid">solid whose plan view edges are needed </param>
        /// <param name="offset">offset from plane</param>
        /// <param name="app">revit application</param>
        /// <returns>curve array of plan view curves</returns>
        public static CurveArray
        GetProfile(Solid solid, double offset, Revit.ApplicationServices.Application app)
        {
            CurveArray curveArray = app.Create.NewCurveArray();
            EdgeArray edgeArray = GetEdgesOnPlaneAtOffset(solid.Edges, GeomUtils.kZAxis, offset);
            curveArray = ToCurveArray(edgeArray, curveArray);
            return curveArray;
        }

        /// <summary>
        /// Given an array of edges, weeds out any edges
        /// not present on the desired plane
        /// </summary>
        /// <param name="xyzArray">the array of edges </param>
        /// <param name="normal">normal to the desired plane</param>
        /// <returns>edges on the desired plane</returns>
        public static EdgeArray
        GetEdgesOnPlane(EdgeArray edgeArray, XYZ normal)
        {
            EdgeArray edgesOnPlane = new EdgeArray();
            for (int i = 0; i < edgeArray.Size; i++) {
                IList<XYZ> xyzArray = edgeArray.get_Item(i).Tessellate();
                if (normal.Equals(GeomUtils.kXAxis)) {
                    if (xyzArray[0].X == xyzArray[1].X) {
                        edgesOnPlane.Append(edgeArray.get_Item(i));
                    }
                }
                if (normal.Equals(GeomUtils.kYAxis)) {
                    if (xyzArray[0].Y == xyzArray[1].Y) {
                        edgesOnPlane.Append(edgeArray.get_Item(i));
                    }
                }
                if (normal.Equals(GeomUtils.kZAxis)) {
                    if (xyzArray[0].Z == xyzArray[1].Z) {
                        edgesOnPlane.Append(edgeArray.get_Item(i));
                    }
                }
            }
            return edgesOnPlane;
        }

        /// <summary>
        /// Given an array of edges, weeds out any edges
        /// not present at the given offset
        /// </summary>
        /// <param name="edgeArray">the array of edges </param>
        /// <param name="normal">normal to the desired plane</param>
        /// <param name="offset">offset from the plane</param>
        /// <returns>edges on a plane at given offset</returns>
        public static EdgeArray
        GetEdgesOnPlaneAtOffset(EdgeArray edgeArray, XYZ normal, double offset)
        {
            EdgeArray edgesAtOffset = new EdgeArray();
            edgeArray = GetEdgesOnPlane(edgeArray, normal);
            for (int i = 0; i < edgeArray.Size; i++) {
                IList<XYZ> xyzArray = edgeArray.get_Item(i).Tessellate();
                if (normal.Equals(GeomUtils.kXAxis)) {
                    if ((xyzArray[0].X == offset)) {
                        edgesAtOffset.Append(edgeArray.get_Item(i));
                    }
                }
                if (normal.Equals(GeomUtils.kYAxis)) {
                    if (xyzArray[0].Y == offset) {
                        edgesAtOffset.Append(edgeArray.get_Item(i));
                    }
                }
                if (normal.Equals(GeomUtils.kZAxis)) {
                    if (xyzArray[0].Z == offset) {
                        edgesAtOffset.Append(edgeArray.get_Item(i));
                    }
                }
            }
            return edgesAtOffset;
        }


        /// <summary>
        /// Given an edge Array converts it to a curveArray
        /// </summary>
        /// <param name="edgeArray">edgeArray to convert</param>
        /// <param name="curveArray">curveArray to fill</param>
        /// <returns>a curveArray</returns>
        public static CurveArray
        ToCurveArray(EdgeArray edgeArray, CurveArray curveArray)
        {
            EdgeArrayIterator edgeArrayIter = edgeArray.ForwardIterator();
            while (edgeArrayIter.MoveNext()) {
                Edge edge = edgeArrayIter.Current as Edge;
                XYZ startPt = edge.Tessellate()[0];
                XYZ endPt = edge.Tessellate()[1];
                Line curve = Line.CreateBound(startPt, endPt);
                curveArray.Append(curve);
            }

            return curveArray;
        }

        /// <summary>
        /// Given a curve, mirrors it along the given axis
        /// </summary>
        /// <param name="curve">curve to mirror</param>
        /// <param name="axis">axis to mirror along</param>
        /// <param name="app">revit application</param>
        /// <returns>a mirrored curve</returns>
        public static Curve
        Mirror(Curve curve, XYZ axis, Application app)
        {
            XYZ startPt = curve.GetEndPoint(0);
            XYZ endPt = curve.GetEndPoint(1);
            XYZ p = new XYZ();

            XYZ newStart = null;
            XYZ newEnd = null;

            if (axis.Equals(GeomUtils.kXAxis)) {
                newStart = new XYZ(startPt.X, -startPt.Y, startPt.Z);
                newEnd = new XYZ(endPt.X, -endPt.Y, endPt.Z);
            }
            if (axis.Equals(GeomUtils.kYAxis)) {
                newStart = new XYZ(-startPt.X, startPt.Y, startPt.Z);
                newEnd = new XYZ(-endPt.X, -endPt.Y, endPt.Z);
            }
            if (axis.Equals(GeomUtils.kZAxis)) {
                newStart = new XYZ(startPt.X, startPt.Y, -startPt.Z);
                newEnd = new XYZ(endPt.X, endPt.Y, -endPt.Z);
            }
            return Line.CreateBound(startPt, endPt);
        }

        /// <summary>
        /// Given a point, mirrors it along the given axis
        /// </summary>
        /// <param name="point">point to mirror</param>
        /// <param name="axiz">axis to mirror along</param>
        /// <returns>a mirrored point</returns>
        public static XYZ
        Mirror(XYZ point, XYZ axis)
        {
            XYZ Point = null;
            if (axis.Equals(GeomUtils.kXAxis)) {
                Point = new XYZ(point.X, -point.Y, point.Z);                
            }
            if (axis.Equals(GeomUtils.kYAxis)) {
                Point = new XYZ(-point.X, point.Y, point.Z);                
        
            }
            if (axis.Equals(GeomUtils.kZAxis)) {
                Point = new XYZ(point.X, point.Y, -point.Z);                
           
            }
            return Point;
        }

        /// <summary>
        /// Get the "standard" plane (equiv. to WCS in AutoCAD terms)
        /// </summary>
        /// <param name="app"></param>
        /// <returns>A new sketch plane</returns>
        
        public static SketchPlane
        GetWorldPlane(Autodesk.Revit.UI.UIApplication app)
        {
            Plane plane = app.Application.Create.NewPlane(GeomUtils.kZAxis, GeomUtils.kOrigin);
            SketchPlane sketchPlane = SketchPlane.Create(app.ActiveUIDocument.Document, plane);

            return sketchPlane;
        }
    }
}
