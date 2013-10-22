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
using System.Collections;
using System.Diagnostics;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.DB;

using RevitLookup.Snoop.Collectors;

namespace RevitLookup.Snoop.CollectorExts
{
    public class CollectorExtEditor : CollectorExt
    {
        public CollectorExtEditor()
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
            SlabShapeCrease slabCrease = e.ObjToSnoop as SlabShapeCrease ;
            if (slabCrease != null) {
                Stream(snoopCollector.Data(), slabCrease);
                return;
            }

            SlabShapeEditor slabEditor = e.ObjToSnoop as SlabShapeEditor;
            if (slabEditor != null) {
                Stream(snoopCollector.Data(), slabEditor);
                return;
            }

            SlabShapeVertex slabVertex = e.ObjToSnoop as SlabShapeVertex;
            if (slabVertex != null) {
                Stream(snoopCollector.Data(), slabVertex);
                return;
            }
        }

        private void
        Stream(ArrayList data, SlabShapeCrease slabCrease)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(SlabShapeCrease)));

            data.Add(new Snoop.Data.String("Crease type", slabCrease.CreaseType.ToString()));
            data.Add(new Snoop.Data.Object("Curve", slabCrease.Curve));
            data.Add(new Snoop.Data.Enumerable("End points", slabCrease.EndPoints));          
           
        }

        private void
        Stream(ArrayList data, SlabShapeEditor slabEditor)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(SlabShapeEditor)));

            data.Add(new Snoop.Data.Bool("Is enabled", slabEditor.IsEnabled));
            data.Add(new Snoop.Data.Enumerable("Slab shape creases", slabEditor.SlabShapeCreases));
            data.Add(new Snoop.Data.Enumerable("Slab shape vertices", slabEditor.SlabShapeVertices));
        }

        private void
        Stream(ArrayList data, SlabShapeVertex slabVertex)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(SlabShapeVertex)));

            data.Add(new Snoop.Data.Xyz("Position", slabVertex.Position));
            data.Add(new Snoop.Data.String("Vertex type", slabVertex.VertexType.ToString()));            
        }
    }
}
