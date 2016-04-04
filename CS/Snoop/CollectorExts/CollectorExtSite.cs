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
using System.Collections;
using System.Text;
using System.Diagnostics;

using RevitLookup.Snoop.Collectors;

using Autodesk.Revit.DB;


namespace RevitLookup.Snoop.CollectorExts {

    public class CollectorExtSite : CollectorExt {

        public
        CollectorExtSite()
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


                // see if it is a type we are responsible for
			City city = e.ObjToSnoop as City;
            if (city != null) {
                Stream(snoopCollector.Data(), city);
				return;
			}

            ProjectLocation projLoc = e.ObjToSnoop as ProjectLocation;
            if (projLoc != null) {
                Stream(snoopCollector.Data(), projLoc);
                return;
            }

            ProjectPosition projPos = e.ObjToSnoop as ProjectPosition;
            if (projPos != null) {
                Stream(snoopCollector.Data(), projPos);
                return;
            }

            SiteLocation siteLoc = e.ObjToSnoop as SiteLocation;
            if (siteLoc != null) {
                Stream(snoopCollector.Data(), siteLoc);
                return;
            }
        }
        
		private void
        Stream(ArrayList data, City city)
		{
            data.Add(new Snoop.Data.ClassSeparator(typeof(City)));

            data.Add(new Snoop.Data.String("Name", city.Name));
            data.Add(new Snoop.Data.Double("Latitude", city.Latitude));
            data.Add(new Snoop.Data.Double("Longitude", city.Longitude));
            data.Add(new Snoop.Data.Double("Time zone", city.TimeZone));
        }

        private void
        Stream(ArrayList data, ProjectLocation projLoc)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(ProjectLocation)));

            data.Add(new Snoop.Data.String("Name", projLoc.Name));
            data.Add(new Snoop.Data.Object("Site location", projLoc.SiteLocation));

            XYZ pt = new XYZ();
            data.Add(new Snoop.Data.Object("Project position", projLoc.get_ProjectPosition(pt)));
            data.Add(new Snoop.Data.Xyz("Project position (pt)", pt));
        }

        private void
        Stream(ArrayList data, ProjectPosition projPos)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(ProjectPosition)));

            data.Add(new Snoop.Data.Double("North/South", projPos.NorthSouth));
            data.Add(new Snoop.Data.Double("East/West", projPos.EastWest));
            data.Add(new Snoop.Data.Angle("Angle", projPos.Angle));
            data.Add(new Snoop.Data.Double("Elevation", projPos.Elevation));
        }

        private void
        Stream(ArrayList data, SiteLocation siteLoc)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(SiteLocation)));

            data.Add(new Snoop.Data.Double("Latitude", siteLoc.Latitude));
            data.Add(new Snoop.Data.Double("Longitude", siteLoc.Longitude));
            data.Add(new Snoop.Data.Double("Time zone", siteLoc.TimeZone));
        }
    }
}
