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

using System.Collections;
using System.Windows.Forms;
using RevitLookup.Views;

namespace RevitLookup.Core.Data
{
    /// <summary>
    ///     Snoop.Data class to hold and format an Object value.
    /// </summary>
    public class Object : Data
    {
        private readonly string _stringValue;

        private readonly object _value;

        public Object(string label, object val) : base(label)
        {
            _value = val;
            _stringValue = Utils.ObjToLabelStr(_value);
        }

        public override bool HasDrillDown => _value != null;

        public override string StrValue()
        {
            return _stringValue;
        }

        public override Form DrillDown()
        {
            if (_value != null)
            {
                var objects = new ArrayList {_value};
                var form = new Objects(objects);
                return form;
            }

            return null;
        }
    }
}