#region Header

//
// Copyright 2003-2021 by Autodesk, Inc. 
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
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using RevitLookup.Snoop.Forms;
using Form = System.Windows.Forms.Form;

namespace RevitLookup.Snoop.Data
{
	/// <summary>
	///     Snoop.Data class to hold and format an Object value.
	/// </summary>
	public class ElementGeometry : Data
    {
        protected Application MApp;
        protected bool MHasGeometry;
        protected Element MVal;

        public
            ElementGeometry(string label, Element val, Application app)
            : base(label)
        {
            MVal = val;
            MApp = app;

            MHasGeometry = false;

            if (MVal != null && MApp != null)
                MHasGeometry = HasModelGeometry() || HasViewSpecificGeometry();
        }

        public override bool
            HasDrillDown =>
            MHasGeometry;

        public override string
            StrValue()
        {
            return "<Geometry.Element>";
        }

        public override Form DrillDown()
        {
            if (MHasGeometry)
            {
                var form = new Geometry(MVal, MApp);
                return form;
            }

            return null;
        }

        private bool HasModelGeometry()
        {
            return Enum
                .GetValues(typeof(ViewDetailLevel))
                .Cast<ViewDetailLevel>()
                .Select(x => new Options
                {
                    DetailLevel = x
                })
                .Any(x => MVal.get_Geometry(x) != null);
        }

        private bool HasViewSpecificGeometry()
        {
            var view = MVal.Document.ActiveView;

            if (view == null)
                return false;

            var options = new Options
            {
                View = view,
                IncludeNonVisibleObjects = true
            };

            return MVal.get_Geometry(options) != null;
        }
    }


    // SOFiSTiK FS
    public class OriginalInstanceGeometry : Data
    {
        protected Application MApp;
        protected bool MHasGeometry;
        protected FamilyInstance MVal;

        public
            OriginalInstanceGeometry(string label, FamilyInstance val, Application app)
            : base(label)
        {
            MVal = val;
            MApp = app;

            MHasGeometry = false;

            if (MVal != null && MApp != null)
            {
                var geomOp = MApp.Create.NewGeometryOptions();
                geomOp.DetailLevel = ViewDetailLevel.Undefined;
                if (MVal.GetOriginalGeometry(geomOp) != null)
                    MHasGeometry = true;
            }
        }

        public override bool
            HasDrillDown
        {
            get
            {
                if (MHasGeometry)
                    return true;
                return false;
            }
        }

        public override string
            StrValue()
        {
            return "<Geometry.Element>";
        }

        public override Form DrillDown()
        {
            if (MHasGeometry)
            {
                var form = new OriginalGeometry(MVal, MApp);
                return form;
            }

            return null;
        }
    }
}