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
using RevitLookup.Views;
using Form = System.Windows.Forms.Form;

namespace RevitLookup.Core.Data
{
    /// <summary>
    ///     Snoop.Data class to hold and format an ElementId value.
    /// </summary>
    public class ElementId : Data
    {
        protected Element MElem;
        protected Autodesk.Revit.DB.ElementId MVal;

        public ElementId(string label, Autodesk.Revit.DB.ElementId val, Document doc) : base(label)
        {
            MVal = val;

            MElem = doc.GetElement(val);
        }

        public override bool HasDrillDown => MElem != null;

        public override string StrValue()
        {
            if (MElem != null)
                return Utils.ObjToLabelStr(MElem);

            return MVal != Autodesk.Revit.DB.ElementId.InvalidElementId ? MVal.ToString() : Utils.ObjToLabelStr(null);
        }

        public override Form DrillDown()
        {
            if (MElem == null)
                return null;

            var form = new Objects(MElem);
            return form;
        }
    }
}