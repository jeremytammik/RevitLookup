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

namespace RevitLookup.Snoop.Data
{
	/// <summary>
	/// Snoop.Data class to hold and format a ClassSeparator value.
	/// </summary>
	
	public class ClassSeparator : Data
	{
        protected System.Type    m_val;
	    
        public ClassSeparator(System.Type val)
        :   base("------- CLASS -------")
		{
		    m_val = val;
		}
		
        override public string
        StrValue()
        {
            return string.Format("--- {0} ---", m_val.Name);
        }
        
        public override bool
        IsSeparator
        {
            get { return true;  }
        }
        
        public override bool
        HasDrillDown
        {
            get { return true; }
        }
        
        public override void DrillDown(System.Windows.Forms.Form parent)
        {
			// DrillDown on a ClassType will just browse it using Reflection
            Snoop.Forms.GenericPropGrid pgForm = new Snoop.Forms.GenericPropGrid(m_val);
            pgForm.Text = string.Format("System.Type = {0}", m_val.FullName);
            pgForm.ShowDialog();
        }
    }
}
