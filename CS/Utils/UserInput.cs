#region Header
//
// Copyright 2003-2014 by Autodesk, Inc. 
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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit.DB;

namespace RevitLookup.Utils {

    class UserInput {
    
        /// <summary>
        /// Load a family specified by the user
        /// </summary>
        /// <param name="dboxTitle">Title to use (or null or "" for default)</param>
        /// <param name="doc">Active Document object to load the Family into</param>
        /// <returns>true if successful, false otherwise</returns>
        
        public static bool
        LoadFamily(string dboxTitle, Document doc)
        {
            string familyNameToLoad = "";
            if (GetFamilyNameFromUser(dboxTitle, ref familyNameToLoad) != "")
                return doc.LoadFamily(familyNameToLoad);

            return false;
        }
        
        /// <summary>
        /// Display a dialog to get the Family name to load from the user
        /// </summary>
        /// <param name="dboxTitle">Title to use (or null or "" for default)</param>
        /// <param name="familyNameToLoad">path of the selected file</param>
        /// <returns>true if dialog result OK, false otherwise</returns>
        
        public static String
        GetFamilyNameFromUser(string dboxTitle, ref string familyNameToLoad)
        {
            System.Windows.Forms.OpenFileDialog dbox = new System.Windows.Forms.OpenFileDialog();
            dbox.CheckFileExists = true;
            dbox.AddExtension = true;
            dbox.DefaultExt = "rfa";
            dbox.Filter = "Revit Family Files (*.rfa)|*.rfa";
            dbox.Multiselect = false;
            if ((dboxTitle != null) && (dboxTitle != string.Empty))
                dbox.Title = dboxTitle;
            else
                dbox.Title = "Select a Revit Family file";

            if (dbox.ShowDialog() == DialogResult.OK) {
                familyNameToLoad = dbox.FileName;
                return familyNameToLoad;
            }
            
            return "";
        }


    }
}
