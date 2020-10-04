#region Header
//
// Copyright 2003-2020 by Autodesk, Inc. 
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
using System.Windows.Forms;

namespace RevitLookup.Snoop.Data
{
	/// <summary>
	/// Snoop.Data class to hold and format an Enumerable value.  This class can be used
	/// for any object that supports the IEnumerable interface.  However, some classes,
	/// such as a Map, are better seen visually in Key/Value pairs vs a straight list of 
	/// enumerated objects.  Use this when it works well, but write your own Snoop.Data object
	/// for Enumerable cases that need better feedback to the user.
	/// </summary>
	
	public class Enumerable : Data
	{
	    protected IEnumerable	m_val;
		protected ArrayList		m_objs = new ArrayList();
	    
		public
		Enumerable(string label, IEnumerable val)
		:   base(label)
		{
		    m_val = val;

				// iterate over the collection and put them in an ArrayList so we can pass on
				// to our Form
			if (m_val != null) {
				IEnumerator iter = m_val.GetEnumerator();
				while (iter.MoveNext())
					m_objs.Add(iter.Current);
			}
		}

        public
        Enumerable(string label, IEnumerable val, Autodesk.Revit.DB.Document doc)
            : base(label)
        {
            m_val = val;

            // iterate over the collection and put them in an ArrayList so we can pass on
            // to our Form
            if (m_val != null)
            {
                IEnumerator iter = m_val.GetEnumerator();
                while (iter.MoveNext())
                {
                    var elementId = iter.Current as Autodesk.Revit.DB.ElementId;

                    if (elementId != null && doc != null)
                        m_objs.Add(doc.GetElement(elementId)); // it's more useful for user to view element rather than element id.
                    else
                        m_objs.Add(iter.Current);
                }
            }
        }

		
        public override string
        StrValue()
        {
			return Utils.ObjToLabelStr(m_val);
        }
        
        public override bool
        HasDrillDown
        {
            get {
                if ((m_val == null) || (m_objs.Count == 0))
                    return false;
                else
                    return true;
            }
        }
        
        public override void
        DrillDown()
        {
			if ((m_val != null) && (m_objs.Count != 0)) {
				Snoop.Forms.Objects form = new Snoop.Forms.Objects(m_objs);
				form.ShowDialog();
			}
        }
	}
}
