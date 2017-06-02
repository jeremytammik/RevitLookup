#region Header
//
// Copyright 2003-2017 by Autodesk, Inc. 
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
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.Data
{
	/// <summary>
	/// Snoop.Data class to hold and format an Object value.
	/// </summary>
	
	public class ElementGeometry : Data
	{
	    protected Element m_val;
        protected Autodesk.Revit.ApplicationServices.Application m_app;
        protected bool m_hasGeometry;
	    
		public
		ElementGeometry(string label, Element val, Autodesk.Revit.ApplicationServices.Application app)
		:   base(label)
		{
		    m_val = val;
            m_app = app;

            m_hasGeometry = false;

		    if (m_val != null && m_app != null)
		        m_hasGeometry = HasModelGeometry() || HasViewSpecificGeometry();
		}
		
        public override string
        StrValue()
        {
			return "<Geometry.Element>";
        }
        
        public override bool
        HasDrillDown
        {
            get {
                return m_hasGeometry;
            }
        }
        
        public override void
        DrillDown()
        {
            if (m_hasGeometry) {
				Snoop.Forms.Geometry form = new Snoop.Forms.Geometry(m_val, m_app);
				form.ShowDialog();
			}
        }

	    private bool HasModelGeometry()
	    {
	        return Enum
	            .GetValues(typeof (ViewDetailLevel))
	            .Cast<ViewDetailLevel>()
	            .Select(x => new Options
	                {
	                    DetailLevel = x
	                })
	            .Any(x => m_val.get_Geometry(x) != null);
	    }

	    private bool HasViewSpecificGeometry()
	    {
	        var view = m_val.Document.ActiveView;

	        if (view == null)
	            return false;

	        var options = new Options
	            {
	                View = view,
	                IncludeNonVisibleObjects = true
	            };

	        return m_val.get_Geometry(options) != null;
	    }
	}



   // SOFiSTiK FS
   public class OriginalInstanceGeometry : Data
   {
      protected FamilyInstance m_val;
      protected Autodesk.Revit.ApplicationServices.Application m_app;
      protected bool m_hasGeometry;

      public
      OriginalInstanceGeometry(string label, FamilyInstance val, Autodesk.Revit.ApplicationServices.Application app)
         : base(label)
      {
         m_val = val;
         m_app = app;

         m_hasGeometry = false;

         if (m_val != null && m_app != null)
         {
            Autodesk.Revit.DB.Options geomOp = m_app.Create.NewGeometryOptions();
            geomOp.DetailLevel = ViewDetailLevel.Undefined;
            if (m_val.GetOriginalGeometry(geomOp) != null)
               m_hasGeometry = true;
         }
      }

      public override string
      StrValue()
      {
         return "<Geometry.Element>";
      }

      public override bool
      HasDrillDown
      {
         get
         {
            if (m_hasGeometry)
               return true;
            else
               return false;
         }
      }

      public override void
      DrillDown()
      {
         if (m_hasGeometry)
         {
            Snoop.Forms.OriginalGeometry form = new Snoop.Forms.OriginalGeometry(m_val, m_app);
            form.ShowDialog();
         }
      }
   }
}
