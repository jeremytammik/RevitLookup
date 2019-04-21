#region Header
//
// Copyright 2003-2019 by Autodesk, Inc. 
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

namespace RevitLookup.Snoop.Data
{
	/// <summary>
	/// Snoop.Data class to hold and format a chunk of XML.
	/// </summary>

	public class Xml : Data
	{
	    protected string    m_val;
	    protected bool      m_isFileName = false;
	    
		public
		Xml(string label, string val, bool isFileName)
		:   base(label)
		{
		    m_val = val;
		    m_isFileName = isFileName;
		}
		
        public override string
        StrValue()
        {
            return m_val;
        }
        
        public override bool
        HasDrillDown
        {
            get {
                if (m_val == string.Empty)
                    return false;
                else
                    return true;
            }
        }
        
        public override void
        DrillDown()
        {
            try {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                if (m_isFileName)
                    xmlDoc.Load(m_val);
                else
                    xmlDoc.LoadXml(m_val);
            
                RevitLookup.Xml.Forms.Dom form = new RevitLookup.Xml.Forms.Dom(xmlDoc);
                form.ShowDialog();
            }
            catch (System.Xml.XmlException e) {
                MessageBox.Show(e.Message, "XML Exception");
            }
        }
	}
}
