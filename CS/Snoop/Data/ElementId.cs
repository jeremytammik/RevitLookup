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
using System.Windows.Forms;
using System.Collections;

using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.Data
{
	/// <summary>
	/// Snoop.Data class to hold and format an ElementId value.
	/// </summary>
	
	public class ElementId : Data
	{
	    protected Autodesk.Revit.DB.ElementId	m_val;
		protected Element	m_elem = null;
	    
		public
		ElementId(string label, Autodesk.Revit.DB.ElementId val, Document doc)
		:   base(label)
		{
		   m_val = val;
         try
         {
            if (val != Autodesk.Revit.DB.ElementId.InvalidElementId)
               m_elem = doc.GetElement(val);	// TBD: strange signature!
         }
         catch (System.Exception)
         {
            m_elem = null;
         }
		}
		
        public override string
        StrValue()
        {
			return Utils.ObjToTypeStr(m_elem);
        }
        
        public override bool
        HasDrillDown
        {
            get {
                if (m_elem == null)
                    return false;
                else
                    return true;
            }
        }
        
        public override void
        DrillDown()
        {
            if (m_elem != null) {
				Snoop.Forms.Objects form = new Snoop.Forms.Objects(m_elem);
				form.ShowDialog();
			}
        }
	}
}
