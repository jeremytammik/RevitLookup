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

using System.Windows.Forms;
using System.Xml;
using RevitLookup.Views;

namespace RevitLookup.Core.Snoop.Data
{
    /// <summary>
    ///     Snoop.Data class to hold and format a chunk of XML.
    /// </summary>
    public class Xml : Data
    {
        protected bool MIsFileName;
        protected string MVal;

        public
            Xml(string label, string val, bool isFileName)
            : base(label)
        {
            MVal = val;
            MIsFileName = isFileName;
        }

        public override bool
            HasDrillDown
        {
            get
            {
                if (MVal == string.Empty)
                    return false;
                return true;
            }
        }

        public override string
            StrValue()
        {
            return MVal;
        }

        public override Form DrillDown()
        {
            try
            {
                var xmlDoc = new XmlDocument();
                if (MIsFileName)
                    xmlDoc.Load(MVal);
                else
                    xmlDoc.LoadXml(MVal);

                var form = new Dom(xmlDoc);
                form.ShowDialog();
            }
            catch (XmlException e)
            {
                MessageBox.Show(e.Message, "XML Exception");
            }

            return null;
        }
    }
}