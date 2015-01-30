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
using System.Diagnostics;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;


namespace RevitLookup.Graphics {

    public abstract class GraphicsStream {

        protected Autodesk.Revit.UI.UIApplication      m_app;
        protected Stack<Transform> m_xformStack;
        protected Stack<Options>   m_geomOptionsStack;
        protected Stack<View>      m_viewStack;

        public
        GraphicsStream(UIApplication app)
        {
            m_app = app;
            m_xformStack = new Stack<Transform>();
            m_geomOptionsStack = new Stack<Options>();
            m_viewStack = new Stack<View>();
        }

        public UIApplication Application
        {
            get { return m_app; }
        }

        #region Transformation Stack

        public virtual void
        PushXform(Transform mat)
        {
            if (m_xformStack.Count > 0) {
                m_xformStack.Push(m_xformStack.Peek() * mat);
            }
            else {
                m_xformStack.Push(mat);
            }
        }

        public virtual void
        PopXform()
        {
            m_xformStack.Pop();
        }

        public virtual Transform
        CurrentXform
        {
            get { return m_xformStack.Peek(); }
        }

        public Boolean
        HasXform
        {
            get { return (m_xformStack.Count == 0) ? false : true; }
        }

        #endregion

        #region Geometry Options Stack

        public void
        PushGeometryOptions(Options opts)
        {
            m_geomOptionsStack.Push(opts);
        }

        public void
        PopGeometryOptions()
        {
            m_geomOptionsStack.Pop();
        }

        public Options
        CurrentGeometryOptions
        {
            get { return m_geomOptionsStack.Peek(); }
        }

        #endregion

        #region View Stack

        public void
        PushView(View view)
        {
            m_viewStack.Push(view);
        }

        public void
        PopView()
        {
            m_viewStack.Pop();
        }

        public View
        CurrentView
        {
            get { return m_viewStack.Peek(); }
        }

        #endregion

        #region Geometric Primitives

        public virtual Double
        DeviationForCurves()
        {
            return 0.5;
        }

        public virtual Double
        DeviationForCurves(XYZ samplePt)
        {
            return 0.5;
        }

        #endregion

        #region Low-Level geometric primitives

        public abstract void
        StreamWcs(XYZ pt1, XYZ pt2);       // only function you absolutely have to override (to get Vector graphics)


        public virtual void
        StreamWcs(IList<XYZ> pts, bool closed)
        {
            if (pts.Count < 2) {
                Debug.Assert(false); // have to have at least 2 points!
                return;
            }

            XYZ pt1 = new XYZ();
            XYZ pt2 = new XYZ();

            int len = pts.Count;
            for (int i=0; i <(len - 1); i++) {
                pt1 = pts[i];
                pt2 = pts[i + 1];

                StreamWcs(pt1, pt2);    // pts are already in WCS, no Xform
            }

            if (closed)
                StreamWcs(pts[len-1], pts[0]);
        }

        public virtual void
        StreamWcs(Line line)
        {
            if (line.IsBound == false) {
                Debug.Assert(false);
                return;
            }

            StreamWcs(line.GetEndPoint(0), line.GetEndPoint(1));
        }

        public virtual void
        StreamWcs(Arc arc)
        {
            StreamCurveAsTesselatedPointsWcs(arc);
        }

        public virtual void
        StreamWcs(Ellipse ellipse)
        {
            StreamCurveAsTesselatedPointsWcs(ellipse);
        }

        public virtual void
        StreamWcs(NurbSpline spline)
        {
            StreamCurveAsTesselatedPointsWcs(spline);
        }

        public virtual void
        StreamWcs(Curve curve)
        {
            if (curve is Line)
                StreamWcs(curve as Line);
            else if (curve is Arc)
                StreamWcs(curve as Arc);
            else if (curve is Ellipse)
                StreamWcs(curve as Ellipse);
            else if (curve is NurbSpline)
                StreamWcs(curve as NurbSpline);
            else
                StreamCurveAsTesselatedPointsWcs(curve);
        }

        #endregion // Low-level geometric primitives

        public virtual void
        Stream(XYZ pt1, XYZ pt2)
        {
            if (HasXform)
                StreamWcs(CurrentXform.OfPoint(pt1), CurrentXform.OfPoint(pt2));
            else
                StreamWcs(pt1, pt2);
        }

        public virtual void
        Stream(IList<XYZ> pts, bool closed)
        {
            if (pts.Count < 2) {
                Debug.Assert(false); // have to have at least 2 points!
                return;
            }

            XYZ pt1, pt2;

            int len = pts.Count;
            for (int i=0; i <(len - 1); i++) {
                pt1 = pts[i];
                pt2 = pts[i + 1];

                if (HasXform)
                    StreamWcs(CurrentXform.OfPoint(pt1), CurrentXform.OfPoint(pt2));
                else
                    StreamWcs(pt1, pt2);
            }

            if (closed) {
                if (HasXform)
                    StreamWcs(CurrentXform.OfPoint(pts[len-1]), CurrentXform.OfPoint(pts[0]));
                else
                    StreamWcs(pts[len-1], pts[0]);
            }
        }

        public virtual void
        Stream(Line line)
        {
            if (line.IsBound == false) {
                Debug.Assert(false);
                return;
            }

            Stream(line.GetEndPoint(0), line.GetEndPoint(1));
        }

        public virtual void
        Stream(Arc arc)
        {
            if (HasXform)
                StreamWcs(arc.CreateTransformed(CurrentXform));
            else
                StreamWcs(arc);
        }

        public virtual void
        Stream(Ellipse ellipse)
        {
            if (HasXform)
                StreamWcs(ellipse.CreateTransformed(CurrentXform));
            else
                StreamWcs(ellipse);
        }

        public virtual void
        Stream(NurbSpline spline)
        {
            if (HasXform)
                StreamWcs(spline.CreateTransformed(CurrentXform));
            else
                StreamWcs(spline);
        }

        public virtual void
        Stream(Curve curve)
        {
            if (HasXform)
                StreamWcs(curve.CreateTransformed(CurrentXform));
            else
                StreamWcs(curve);
        }


        /// <summary>
        /// By default, everything goes out as tesselated vectors.  This function allows all the
        /// base class functions to easily tesselate.  But, if derived classes override individual
        /// curve types, they can intercept before they are tesselated.
        /// </summary>
        /// <param name="crv">Curve to tesselate into vectors</param>
        
        private void
        StreamCurveAsTesselatedPointsWcs(Curve crv)
        {
            StreamWcs(crv.Tessellate(), false);   // stream out as array of points
        }

        private void
        StreamCurveAsTesselatedPoints(Curve crv)
        {
            Stream(crv.Tessellate(), false);   // stream out as array of points
        }

        #region High-Level Object Stream functions

        public virtual void
        Stream(Element elem)
        {
            if ((m_viewStack.Count == 0) || (m_geomOptionsStack.Count == 0)) {
                throw new System.ArgumentException("View stack or Geometry Options stack is empty.");
            }

            GeometryElement geom = elem.get_Geometry(this.CurrentGeometryOptions);
            if (geom != null) {
                Stream(geom);
            }
        }

        public virtual void
        Stream(GeometryObject obj)
        {
            if (obj is Curve) {
                Stream(obj as Curve);
            }
            else if (obj is Edge) {
                Stream(obj as Edge);
            }
            else if (obj is GeometryElement) {
                Stream(obj as GeometryElement);
            }
            else if (obj is ConicalFace) {
                Stream(obj as ConicalFace);
            }
            else if (obj is CylindricalFace) {
                Stream(obj as CylindricalFace);
            }
            else if (obj is HermiteFace) {
                Stream(obj as HermiteFace);
            }
            else if (obj is PlanarFace) {
                Stream(obj as PlanarFace);
            }
            else if (obj is RevolvedFace) {
                Stream(obj as RevolvedFace);
            }
            else if (obj is RuledFace) {
                Stream(obj as RuledFace);
            }
            else if (obj is Face) {
                Stream(obj as Face);
            }
            else if (obj is GeometryInstance) {
                Stream(obj as GeometryInstance);
            }
            else if (obj is Mesh) {
                Stream(obj as Mesh);
            }
            else if (obj is Profile) {
                Stream(obj as Profile);
            }
            else if (obj is Solid) {
                Stream(obj as Solid);
            }
        }

        public virtual void
        Stream(Edge edge)
        {
            IList<XYZ> ptArray = edge.Tessellate();

            int len = ptArray.Count;
            for (int i=0; i < (len - 1); i++) {
                Stream(ptArray[i], ptArray[i + 1]);
            }
        }

        public virtual void
        Stream(EdgeArray edgeArray)
        {
            foreach (Edge edge in edgeArray) {
                Stream(edge);
            }
        }

        public virtual void
        Stream(GeometryElement elem)
        {
            foreach (GeometryObject geom in elem)
            {
                Stream(geom);
            }
        }

        // All of these types of Faces get their geometry from the base class Face.  We
        // want a specific virtual for each type though so that derived streams can pick
        // up the geometry at the optimal level (if they want).

        /// <summary>
        /// Do the common work of streaming data out for all Face types
        /// </summary>
        /// <param name="face"></param>
        
        private void
        StreamFaceGeoometry(Face face)
        {
            foreach (EdgeArray edgeArray in face.EdgeLoops)
                Stream(edgeArray);
        }

        public virtual void
        Stream(ConicalFace face)
        {
            StreamFaceGeoometry(face);
        }

        public virtual void
        Stream(CylindricalFace face)
        {
            StreamFaceGeoometry(face);
        }

        public virtual void
        Stream(HermiteFace face)
        {
            StreamFaceGeoometry(face);
        }

        public virtual void
        Stream(PlanarFace face)
        {
            StreamFaceGeoometry(face);
        }

        public virtual void
        Stream(RevolvedFace face)
        {
            StreamFaceGeoometry(face);
        }

        public virtual void
        Stream(RuledFace face)
        {
            StreamFaceGeoometry(face);
        }

        public virtual void
        Stream(Face face)
        {
            StreamFaceGeoometry(face);
        }


        public virtual void
        Stream(Autodesk.Revit.DB.GeometryInstance inst)
        {
            PushXform(inst.Transform);
            Stream(inst.SymbolGeometry);
            PopXform();
        }

        public virtual void
        Stream(Mesh mesh)
        {
            for (int i=0; i<mesh.NumTriangles; i++) {
                MeshTriangle mt = mesh.get_Triangle(i);

                Stream(mt.get_Vertex(0), mt.get_Vertex(1));
                Stream(mt.get_Vertex(1), mt.get_Vertex(2));
                Stream(mt.get_Vertex(2), mt.get_Vertex(0));               
            }
        }

        public virtual void
        Stream(Profile prof)
        {
            foreach (Curve curve in prof.Curves) {
                Stream(curve);
            }
        }

        public virtual void
        Stream(Solid solid)
        {
            foreach (Face face in solid.Faces) {
                Stream(face);
            }

            //These edges will appear when streaming the faces
            //
            ////StreamEdgeArray(solid.Edges);
        }

        #endregion
    }
}
