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
using System.Windows.Forms;

using Revit = Autodesk.Revit;
using Autodesk.Revit.DB;
using RevitLookup.Graphics;


namespace RevitLookup.Test {

    class TestGeometry : RevitLookupTestFuncs {

        public
        TestGeometry(Autodesk.Revit.UI.UIApplication app)
            : base(app)
        {
            m_testFuncs.Add(new RevitLookupTestFuncInfo("Ellipse", "Hardwire a simple Ellipse", typeof(ModelEllipse), new RevitLookupTestFuncInfo.TestFunc(EllipseHardwire), RevitLookupTestFuncInfo.TestType.Create));
            m_testFuncs.Add(new RevitLookupTestFuncInfo("Elliptical Arc", "Hardwire a simple Elliptical Arc", typeof(ModelEllipse), new RevitLookupTestFuncInfo.TestFunc(EllipticalArcHardwire), RevitLookupTestFuncInfo.TestType.Create));
            m_testFuncs.Add(new RevitLookupTestFuncInfo("Create Helix", "Create Helix with Model Curves", typeof(ModelCurve), new RevitLookupTestFuncInfo.TestFunc(CreateHelix), RevitLookupTestFuncInfo.TestType.Create));
            m_testFuncs.Add(new RevitLookupTestFuncInfo("Build Tower", "Build Tower with Model Curves", typeof(ModelCurve), new RevitLookupTestFuncInfo.TestFunc(BuildTower), RevitLookupTestFuncInfo.TestType.Create));
        }


        public void
        CreateHelix()
        {
            double increment = 0.1;
            double current = 0;
            XYZ startPt;
            XYZ endPt;
            XYZ zAxis = GeomUtils.kZAxis;
            XYZ origin = GeomUtils.kOrigin;
            Line line;
            Plane plane = m_revitApp.Application.Create.NewPlane(zAxis, origin);
            SketchPlane sketchPlane = SketchPlane.Create(m_revitApp.ActiveUIDocument.Document, plane);
            CurveArray curveArray = new CurveArray();

            startPt = new XYZ(Math.Cos(current), Math.Sin(current), current);
            current += increment;

            while (current <= GeomUtils.kTwoPi) {
                endPt = new XYZ(Math.Cos(current), Math.Sin(current), current);

                line = Line.CreateBound(startPt, endPt);
                curveArray.Append(line);

                startPt = endPt;
                current += increment;
            }

            m_revitApp.ActiveUIDocument.Document.Create.NewModelCurveArray(curveArray, sketchPlane);
        }

        public void BuildTower()
        {
           Transaction newTowerTransaction = new Transaction(m_revitApp.ActiveUIDocument.Document, "New Tower");
           newTowerTransaction.Start();
           Tower tower = new Tower(m_revitApp);
           tower.Build();
           newTowerTransaction.Commit();
        }

        public void
        EllipseHardwire()
        {
                // Revit PI   = 3.14159265358979
                // AutoCAD PI = 3.14159265358979323846;
                // Math.Pi    = 3.14159265358979323846;
            double revitPi = 3.14159265358979;  // TBD: have to use Revit's version of Pi or the Ellipse will fail!

                // make a simple Ellipse, oriented in the Y axis, at the origin.
            double radiusX = 5.0;
            double radiusY = 8.0;
            SketchPlane sketchPlane = Utils.Geometry.GetWorldPlane(m_revitApp);
                // TBD: in order to get a full ellipse, I have to know the exact full parameter range (see above).  Should just be two signatures.
            Ellipse ellipse = Ellipse.Create(GeomUtils.kOrigin, radiusX, radiusY, GeomUtils.kXAxis, GeomUtils.kYAxis, 0.0, (revitPi * 2.0));

            m_revitApp.ActiveUIDocument.Document.Create.NewModelCurve(ellipse, sketchPlane);
        }

        public void
        EllipticalArcHardwire()
        {
            double revitPi = 3.14159265358979;  // TBD: see above

                // make a simple Ellipse, oriented in the X axis, at the origin.
            double radiusX = 8.0;
            double radiusY = 5.0;
            SketchPlane sketchPlane = Utils.Geometry.GetWorldPlane(m_revitApp);
                // TBD: in order to get a full ellipse, I have to know the exact full parameter range (see above).  Should just be two signatures.
            Ellipse ellipse = Ellipse.Create(GeomUtils.kOrigin, radiusX, radiusY, GeomUtils.kXAxis, GeomUtils.kYAxis, 0.0, (revitPi*1.5));

            m_revitApp.ActiveUIDocument.Document.Create.NewModelCurve(ellipse, sketchPlane);
        }
    }
}
