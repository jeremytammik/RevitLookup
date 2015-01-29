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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using Revit = Autodesk.Revit;
using Autodesk.Revit.DB;
using RevitLookup.Graphics;


namespace RevitLookup.Test {

    class TestGraphicsStream : RevitLookupTestFuncs {

        public
        TestGraphicsStream(Autodesk.Revit.UI.UIApplication app)
            : base(app)
        {


            m_testFuncs.Add(new RevitLookupTestFuncInfo("Graphics Stream (Element, Xform)", "Select an Element and stream out its graphics as Model Lines, offset by an Xform", "Graphics Streams", new RevitLookupTestFuncInfo.TestFunc(GraphicsStreamElementXform), RevitLookupTestFuncInfo.TestType.Create));
            m_testFuncs.Add(new RevitLookupTestFuncInfo("Curtain System to Wireframe", "Convert a Curtain System to Wireframe", "Graphics Streams", new RevitLookupTestFuncInfo.TestFunc(CurtainSystemToWireframe), RevitLookupTestFuncInfo.TestType.Create));
        }



        /// <summary>
        /// Same as above, but offset them from the original location by a given xform
        /// </summary>
        
        public void
        GraphicsStreamElementXform()
        {
            if (m_revitApp.ActiveUIDocument.Selection.GetElementIds().Count == 0) {
                MessageBox.Show("Please select elements and re-run test.", "No Elements Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            GraphicsStreamRevit grStream = new GraphicsStreamRevit(m_revitApp);
            grStream.PushView(m_revitApp.ActiveUIDocument.Document.ActiveView);

            Options opts = m_revitApp.Application.Create.NewGeometryOptions();
            opts.ComputeReferences = true;
            opts.View = m_revitApp.ActiveUIDocument.Document.ActiveView;
            grStream.PushGeometryOptions(opts);

                // transform everything by 50 in both the X and Y axes
            Transform xform = new Transform(Transform.Identity);
            xform.Origin = new XYZ(50.0, 50.0, 0.0);
            grStream.PushXform(xform);
            var selElementIds = m_revitApp.ActiveUIDocument.Selection.GetElementIds();
           
            Document dbDoc = m_revitApp.ActiveUIDocument.Document;
            foreach (ElementId elemId in selElementIds) {
                grStream.Stream(dbDoc.GetElement(elemId));
            }
        }

        public void
        CurtainSystemToWireframe()
        {
           if (m_revitApp.ActiveUIDocument.Selection.GetElementIds().Count == 0)
            {
                MessageBox.Show("Please select elements and re-run test.", "No Elements Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            GraphicsStreamRevit grStream = new GraphicsStreamRevit(m_revitApp);
            grStream.PushView(m_revitApp.ActiveUIDocument.Document.ActiveView);

            Options opts = m_revitApp.Application.Create.NewGeometryOptions();
            opts.ComputeReferences = true;
            opts.View = m_revitApp.ActiveUIDocument.Document.ActiveView;
            grStream.PushGeometryOptions(opts);

            // transform everything by 50 in both the X and Y axes
            Transform xform = new Transform(Transform.Identity);
            xform.Origin = new XYZ(50.0, 50.0, 0.0);
            grStream.PushXform(xform);
            Document dbDoc = m_revitApp.ActiveUIDocument.Document;
            var selElementIds = m_revitApp.ActiveUIDocument.Selection.GetElementIds();
            foreach (ElementId elemId in selElementIds)
            {
               Element elem = dbDoc.GetElement(elemId);
                if (elem is Wall) {
                    Wall wall = elem as Wall;

                    if (wall.WallType.Kind == WallKind.Curtain) {
                        WriteCurtainCells(wall.CurtainGrid, grStream); // call same function for each
                    }
                }                
                else if (elem is ExtrusionRoof) {
                    ExtrusionRoof roof = elem as ExtrusionRoof;

                    foreach (CurtainGrid grid in roof.CurtainGrids) {
                        WriteCurtainCells(grid, grStream);
                    }
                }
                else if (elem is CurtainSystem) {
                    CurtainSystem curtSys = elem as CurtainSystem;

                    foreach (CurtainGrid grid in curtSys.CurtainGrids) {
                        WriteCurtainCells(grid, grStream);
                    }
                }
            }
        }


        private void
        WriteCurtainCells(CurtainGrid grid, GraphicsStream grStream)
        {
            foreach (CurtainCell cell in grid.GetCurtainCells()) {
                foreach (CurveArray crvArray in cell.CurveLoops) {
                    foreach (Curve crv in crvArray) {
                        grStream.Stream(crv);
                    }
                }
            }
        }
    }
}
