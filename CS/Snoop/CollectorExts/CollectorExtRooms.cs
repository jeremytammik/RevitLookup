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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using Autodesk.Revit.DB;

using RevitLookup.Snoop.Collectors;

namespace RevitLookup.Snoop.CollectorExts
{
    /// <summary>
    /// Provide Snoop.Data for any classes related to Geometry.
    /// </summary>
    /// 

    public class CollectorExtRooms : CollectorExt
    {
        public CollectorExtRooms()
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

            BoundarySegment bndSeg = e.ObjToSnoop as BoundarySegment;
            if (bndSeg != null) {
                Stream(snoopCollector.Data(), bndSeg);
                return;
            }

            List<BoundarySegment> segArray = e.ObjToSnoop as List<BoundarySegment>;    // NOTE: this is needed because BoundarySegmentArrayArray will display enumerable Snoop items
            if( segArray != null )
            {
              Stream( snoopCollector.Data(), segArray );
              return;
            }
        }

        private void
        Stream(ArrayList data, BoundarySegment seg)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(BoundarySegment)));

            data.Add(new Snoop.Data.Object("Curve", seg.GetCurve()));
            data.Add(new Snoop.Data.Object("ElementId", seg.ElementId));
        }

        private void
        Stream( ArrayList data, List<BoundarySegment> segArray )
        {
          data.Add( new Snoop.Data.ClassSeparator( typeof( List<BoundarySegment> ) ) );

          IEnumerator iter = segArray.GetEnumerator();
          int i = 0;
          while( iter.MoveNext() )
          {
            data.Add( new Snoop.Data.Object( string.Format( "Boundary segment {0:d}", i++ ), iter.Current ) );
          }
        }
    }
}
