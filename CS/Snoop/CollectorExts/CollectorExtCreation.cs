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

using Autodesk.Revit.Creation;

using RevitLookup.Snoop.Collectors;

namespace RevitLookup.Snoop.CollectorExts
{
    /// <summary>
    /// Provide Snoop.Data for any classes related to Geometry.
    /// </summary>
    ///

    public class CollectorExtCreation : CollectorExt
    {
        public CollectorExtCreation()
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

            Application app = e.ObjToSnoop as Application;
            if (app != null) {
                Stream(snoopCollector.Data(), app);
                return;
            }

            FamilyInstanceCreationData famInstData = e.ObjToSnoop as FamilyInstanceCreationData;
            if (famInstData != null) {
                Stream(snoopCollector.Data(), famInstData);
                return;
            }

            ItemFactoryBase itemFactBase = e.ObjToSnoop as ItemFactoryBase;
            if (itemFactBase != null) {
                Stream(snoopCollector.Data(), itemFactBase);
                return;
            }
        }

        private void
        Stream(ArrayList data, Application app)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(Application)));

        }

        private void
        Stream(ArrayList data, FamilyInstanceCreationData famInstData)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(FamilyInstanceCreationData)));

            data.Add(new Snoop.Data.Object("Axis", famInstData.Axis));
            data.Add(new Snoop.Data.Double("Rotate angle", famInstData.RotateAngle));
        }

        private void
        Stream(ArrayList data, ItemFactoryBase itemFactBase)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(ItemFactoryBase)));

            // No data at this level yet!

            FamilyItemFactory famItem = itemFactBase as FamilyItemFactory;
            if (famItem != null) {
                Stream(data, famItem);
                return;
            }
        }

        private void
        Stream(ArrayList data, FamilyItemFactory famItem)
        {
            data.Add(new Snoop.Data.ClassSeparator(typeof(FamilyItemFactory)));           

            // No data at this level yet!
        }
    }
}
