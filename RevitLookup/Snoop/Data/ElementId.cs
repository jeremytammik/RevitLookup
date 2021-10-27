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

using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.Data
{
    /// <summary>
    /// Snoop.Data class to hold and format an ElementId value.
    /// </summary>

    public class ElementId : Data
    {
        protected Autodesk.Revit.DB.ElementId m_val;
        protected Element m_elem;

        public ElementId(string label, Autodesk.Revit.DB.ElementId val, Document doc) : base(label)
        {
            m_val = val;
            
            m_elem = doc.GetElement(val);
        }

        public override string StrValue()
        {
            if (m_elem != null)
                return Utils.ObjToLabelStr(m_elem);

            return m_val != Autodesk.Revit.DB.ElementId.InvalidElementId ? m_val.ToString() : Utils.ObjToLabelStr(null);
        }

        public override bool HasDrillDown => m_elem != null;

        public override System.Windows.Forms.Form DrillDown()
        {
            if (m_elem == null) 
                return null;
            
            var form = new Forms.Objects(m_elem);
            return form;
        }
    }
}
