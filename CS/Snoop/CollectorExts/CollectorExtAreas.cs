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
using System.Collections;
using System.Diagnostics;

using Autodesk.Revit;
using Autodesk.Revit.DB;

using RevitLookup.Snoop.Collectors;

namespace RevitLookup.Snoop.CollectorExts
{
    /// <summary>
    /// Provide Snoop.Data for any classes related to Geometry.
    /// </summary>
    /// 

    public class CollectorExtAreas : CollectorExt
    {
        public CollectorExtAreas()
        {
        }

        protected override void
        CollectEvent(object sender, CollectorEventArgs e)
        {
            // cast the sender object to the SnoopCollector we are expecting
            Collector snoopCollector = sender as Collector;
            if (snoopCollector == null) {
                Debug.Assert(false);    // why did someone else send us the message?
                return;
            }            

            BoundarySegment boundSeg = e.ObjToSnoop as BoundarySegment;
            if (boundSeg != null) {
                Stream(snoopCollector.Data(), boundSeg);
                return;
            }

            BoundarySegmentArray boundSegArray = e.ObjToSnoop as BoundarySegmentArray;
            if (boundSegArray != null) {
                Stream(snoopCollector.Data(), boundSegArray);
                return;
            }
        }        

        private void
        Stream(ArrayList data, BoundarySegment boundSeg)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(BoundarySegment)));

            data.Add(new Snoop.Data.Object("Curve", boundSeg.Curve));
            data.Add(new Snoop.Data.Object("Document", boundSeg.Document));
            data.Add(new Snoop.Data.Object("Element", boundSeg.Element));            
        }

        private void
        Stream(ArrayList data, BoundarySegmentArray boundSegArray)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(BoundarySegmentArray)));

            IEnumerator iter = boundSegArray.GetEnumerator();
            int i = 0;
            while (iter.MoveNext()) {
                data.Add(new Snoop.Data.Object(string.Format("Boundary segment {0:d}", i++), iter.Current));
            }
        }
    }
}
