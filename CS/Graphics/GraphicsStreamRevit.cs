//
// Copyright 2003-2010 by Autodesk, Inc. 
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

using System;
using System.Collections.Generic;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.DB;


namespace RevitLookup.Graphics {

    public class GraphicsStreamRevit : GraphicsStream {

        private SketchPlane m_sketchPlane = null;
        private Plane m_plane = null;

        public GraphicsStreamRevit(Autodesk.Revit.UI.UIApplication app)
        :   base(app)
        {
            SetSketchPlane();
        }

        /// <summary>
        /// Set the current SketchPlane to be the same as the UCS Xform currently on the stack.  This
        /// allows lines in that plane to be grip edited in the same plane as would be expected.
        /// </summary>
        
        private void SetSketchPlane()
        {
            if (HasXform)
                m_plane = m_app.Application.Create.NewPlane(CurrentXform.BasisZ, CurrentXform.Origin);
            else
               m_plane = m_app.Application.Create.NewPlane(XYZ.BasisZ, XYZ.Zero);      // standard World plane
     
        }

        public override void
        PushXform(Transform mat)
        {
            base.PushXform(mat);
            SetSketchPlane();   // set the SketchPlane based on the current Xform
        }

        public override void
        PopXform()
        {
            base.PopXform();
            SetSketchPlane();   // set the SketchPlane based on the current Xform
        }

        /// <summary>
        /// Only required override.  If we didn't override any of the other geometry signatures,
        /// everything would get broken down into line segments (even Ellipse and Arc)
        /// </summary>
        /// <param name="pt1">start point</param>
        /// <param name="pt2">end point</param>
        
        public override void
        StreamWcs(XYZ pt1, XYZ pt2)
        {
            Line line = Line.CreateBound(pt1, pt2);
            m_app.ActiveUIDocument.Document.Create.NewModelCurve(line, m_sketchPlane);
        }

        /// <summary>
        /// Override Arcs since we can make an optimal Arc Model Line before it
        /// gets broken into tesselated segments.
        /// </summary>
        /// <param name="arc">arc to create</param>
        
        public override void
        StreamWcs(Arc arc)
        {
            m_app.ActiveUIDocument.Document.Create.NewModelCurve(arc, m_sketchPlane);
        }

        /// <summary>
        /// Override Ellipses since we can make an optimal Ellipse Model Line before it
        /// gets broken into tesselated segments.
        /// </summary>
        /// <param name="ellipse">ellipse to create</param>
        
        public override void
        StreamWcs(Ellipse ellipse)
        {
            m_app.ActiveUIDocument.Document.Create.NewModelCurve(ellipse, m_sketchPlane);
        }

        /// <summary>
        /// Override Splines since we can make an optimal Spline Model Line before it
        /// gets broken into tesselated segments.
        /// </summary>
        /// <param name="nurbSpline">spline to create</param>
        
        public override void
        StreamWcs(NurbSpline nurbSpline)
        {
            m_app.ActiveUIDocument.Document.Create.NewModelCurve(nurbSpline, m_sketchPlane);
        }


    }
}

