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
using System.Text;

using Revit = Autodesk.Revit.DB;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;

namespace RevitLookup.Test {

        // This shape generation code originally provided by Neil Katz at SOM (Skidmore, Owings, and Merrill).
        // It was originally written in AutoLISP and is a formula to generate a building skin that, in this case,
        // transforms from a square at the base to a circular top.  This code creates one "patch" of the skin
        // that is then stitched together for the overall building shape.
    class Tower {
        private Autodesk.Revit.UI.UIApplication m_app;
        private SketchPlane m_sketchPlane = null;
        private XYZ m_origin;
        private XYZ m_unfoldOriginPoint;
        private double m_baseSquareHalfLength;
        private double m_topCircleRadius;
        private double m_buildingHeight;
        private List<double> m_verticalDivisionList;
        private BuildingType m_buildingType;
        private int m_verticalDivisions;
        private int m_structuralDivisions;
        private XYZ[,] m_pointArray;

        private enum BuildingType {
            SharpCorner = 0,
            RoundCorner = 1
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public Tower(Autodesk.Revit.UI.UIApplication app)
        {
            m_app = app;
            m_origin = XYZ.Zero;
            m_baseSquareHalfLength = 40.0;
            m_topCircleRadius = 25.0;
            m_buildingHeight = 500.0;
            m_buildingType = BuildingType.SharpCorner;
            m_unfoldOriginPoint = new XYZ(6.0 * m_baseSquareHalfLength, 0, 0);
            m_verticalDivisionList = new List<double>(new double[] { 50.0, 47.5, 45.0, 42.5, 40.0, 37.5, 35.0, 32.5, 30.0, 27.5, 25.0, 22.5, 20.0, 17.5, 15.0, 12.5 });
            m_verticalDivisions = 12;
            m_structuralDivisions = 9;
        }

        /// <summary>
        /// 
        /// </summary>
        public void
        Build()
        {
            if ((m_verticalDivisionList != null) && (m_verticalDivisionList.Count > 0)) {
                double total = 0;

                foreach (double d in m_verticalDivisionList) {
                    total += d;
                }

                m_buildingHeight = total;
                m_verticalDivisions = m_verticalDivisionList.Count;
            }
            else {
                m_verticalDivisions = 12;
            }

            XYZ bottomCenterPoint = new XYZ(m_origin.X, m_origin.Y, 0);
            XYZ topCenterPoint = new XYZ(m_origin.X, m_origin.Y, m_buildingHeight);

            if (m_buildingType == BuildingType.SharpCorner) {
                //double          panelSubdivisions = 2;
                XYZ pointABase = new XYZ(-m_baseSquareHalfLength, -m_baseSquareHalfLength, 0);
                XYZ pointBBase = new XYZ(m_baseSquareHalfLength, -m_baseSquareHalfLength, 0);
                XYZ pointATop = Polar(GeomUtils.DegreesToRadians(225.0), topCenterPoint, m_topCircleRadius);
                XYZ pointBTop = Polar(GeomUtils.DegreesToRadians(315.0), topCenterPoint, m_topCircleRadius);

                //For now let's leave the array's as 1 based for LISP
                //
                m_pointArray = new XYZ[m_verticalDivisions + 2, m_structuralDivisions + 1];
                //m_pointArray = new XYZ[m_verticalDivisions + 1, m_structuralDivisions];

                //if UNFOLD - goes here

                int iVertical = 0;
                double currentHeight = 0;
                int iV = 0;

                while (iVertical <= m_verticalDivisions) { //<= ?
                    if (m_verticalDivisionList.Count > 0) {
                        while (iV < iVertical) {
                            currentHeight += m_verticalDivisionList[iV];
                            iV++;
                        }

                        System.Diagnostics.Debug.WriteLine(String.Format("CURRENT HEIGHT: {0}", currentHeight));
                    }
                    else {
                        currentHeight = (iVertical - 1) * (m_buildingHeight / m_verticalDivisions);
                    }

                    int iStructural = 1;

                    while (iStructural <= m_structuralDivisions) { // <= ?
                        XYZ bottomPoint = new XYZ(Slider(1, m_structuralDivisions, iStructural, -m_baseSquareHalfLength, m_baseSquareHalfLength), -m_baseSquareHalfLength, 0);
                        XYZ topPoint = Polar(GeomUtils.DegreesToRadians(Slider(1, m_structuralDivisions, iStructural, 225.0, 315.0)), topCenterPoint, m_topCircleRadius);
                        XYZ currentPoint = new XYZ(Slider(0, m_buildingHeight, currentHeight, bottomPoint.X, topPoint.X),
                                                                     Slider(0, m_buildingHeight, currentHeight, bottomPoint.Y, topPoint.Y),
                                                                     Slider(0, m_buildingHeight, currentHeight, bottomPoint.Z, topPoint.Z));

                        m_pointArray[iVertical + 1, iStructural] = currentPoint;

                        ////(if UNFOLD

                        iStructural++;
                    }

                    iVertical++;
                }

                CreateHorizontals();
                CreateDiagrids();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void
        CreateHorizontals()
        {
            int iVertical = 1;
            List<XYZ> pointList = new List<XYZ>();

            while (iVertical <= (m_verticalDivisions + 1)) {
                int iStructural = 1;

                pointList.Clear();

                while (iStructural <= m_structuralDivisions) {
                    pointList.Add(m_pointArray[iVertical, iStructural]);
                    iStructural++;
                }

                CreatePolyline(pointList);

                iVertical++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void
        CreateDiagrids()
        {
            int iVertical = 2;
            List<XYZ> pointList1 = new List<XYZ>();
            List<XYZ> pointList2 = new List<XYZ>();

            while (iVertical < (m_verticalDivisions + 1)) {
                int iStructural = 1;

                pointList1.Clear();
                pointList2.Clear();

                while (iStructural <= m_structuralDivisions) {
                    pointList1.Add(m_pointArray[iVertical - 1, iStructural]);
                    if (iStructural < m_structuralDivisions) {
                        pointList1.Add(m_pointArray[iVertical, iStructural + 1]);
                    }

                    pointList2.Add(m_pointArray[iVertical + 1, iStructural]);
                    if (iStructural < m_structuralDivisions) {
                        pointList2.Add(m_pointArray[iVertical, iStructural + 1]);
                    }

                    iStructural += 2;
                }
                CreatePolyline(pointList1);
                CreatePolyline(pointList2);

                iVertical += 2;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pts"></param>
        private void CreatePolyline(IList<XYZ> pts)
        {
            if (m_sketchPlane == null) {
                XYZ zAxis = GeomUtils.kZAxis;
                XYZ origin = GeomUtils.kOrigin;
                Plane plane = m_app.Application.Create.NewPlane(zAxis, origin);

                m_sketchPlane = SketchPlane.Create(m_app.ActiveUIDocument.Document, plane);
            }

            Line line;
            XYZ startPt;
            XYZ endPt;
            CurveArray curveArray = new CurveArray();

            for (int i = 0; i < (pts.Count - 1); ++i)
            {
                startPt = pts[i];
                endPt   = pts[i + 1];

                line = Line.CreateBound(startPt, endPt);
                curveArray.Append(line);
            }

            m_app.ActiveUIDocument.Document.Create.NewModelCurveArray(curveArray, m_sketchPlane);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="radians"></param>
        /// <param name="pt"></param>
        /// <param name="dist"></param>
        /// <returns></returns>
        
        private XYZ
        Polar(double radians, XYZ pt, double dist)
        {
            //AcGe.Vector3d v = Utils.Dwg.kXAxis.RotateBy(radians, Utils.Dwg.kZAxis);
            XYZ       origin = XYZ.Zero;
            XYZ       zaxis  = XYZ.BasisZ;
            XYZ       xaxis  = XYZ.BasisX;
            Transform trf = Transform.CreateRotationAtPoint(zaxis, radians, origin);
            XYZ       v      = trf.OfVector(xaxis);
            
            v = v.Normalize() * dist;

            XYZ newPt = pt + v;

            return newPt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="minA"></param>
        /// <param name="maxA"></param>
        /// <param name="curA"></param>
        /// <param name="minB"></param>
        /// <param name="maxB"></param>
        /// <returns></returns>
        
        private double
        Slider(double minA, double maxA, double curA, double minB, double maxB)
        {
            ////; given minA maxA curA minB maxB, find curB
            ////;
            ////; minA            curA       maxA
            ////;   |               V          |
            ////;   +--------------------------+
            ////;   |               ^          |
            ////; minB            curB       maxB
            ////;
            double curAPercent = 0;

            if (minA != maxA) {
                curAPercent = (curA - minA) / (maxA - minA);
            }

            return (minB + (curAPercent * (maxB - minB)));  //curB
        }
    }
}
