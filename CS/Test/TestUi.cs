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
using System.Collections.Generic;
using System.Text;

using Autodesk.Revit.DB;

namespace RevitLookup.Test {

    class TestUi : RevitLookupTestFuncs {

        public
        TestUi(Autodesk.Revit.UI.UIApplication app)
            : base(app)
        {
            //m_testFuncs.Add(new RevitLookupTestFuncInfo("Add Menu", "Add a menu to the menubar", "UI", new RevitLookupTestFuncInfo.TestFunc(AddMenu), RevitLookupTestFuncInfo.TestType.Modify));
            //m_testFuncs.Add(new RevitLookupTestFuncInfo("Add Toolbar", "Add a toolbar item", "UI", new RevitLookupTestFuncInfo.TestFunc(AddToolbar), RevitLookupTestFuncInfo.TestType.Modify));
            m_testFuncs.Add(new RevitLookupTestFuncInfo("Select Element", "Select a single element", "UI", new RevitLookupTestFuncInfo.TestFunc(SelectElement), RevitLookupTestFuncInfo.TestType.Query));
        }

        // TBD: as of 04/10/07 - only works on App startup, not dynamically from another command (jma)
        /*public void
        AddMenu()
        {
            MenuItem rootMenu = m_revitApp.CreateTopMenu("RevitLookup Test Menu Item");

            bool success = rootMenu.AddToExternalTools();
            if (success) {
                MenuItem subItem = rootMenu.Append(MenuItem.MenuType.BasicMenu, "Pick me to call back into RevitLookup", "RevitLookup.dll", "CmdSampleMenuItemCallback");
                System.Windows.Forms.MessageBox.Show("Successfully added new menu to the External Tools menu.  Pick the item to demonstrate callback.");
            }
            else
                System.Windows.Forms.MessageBox.Show("Could not add new menu!");
        }

        public void
        AddToolbar()
        {
            Toolbar toolBar = m_revitApp.CreateToolbar();
            toolBar.Name = "Jimbo";

            if (toolBar != null) {
                ToolbarItem tbItem = toolBar.AddItem("RevitLookup.dll", "CmdSampleMenuItemCallback");
                System.Windows.Forms.MessageBox.Show("Successfully added new toolbar.  Pick the item to demonstrate callback.");
            }
            else
                System.Windows.Forms.MessageBox.Show("Could not add new toolbar!");
        }*/

        public void SelectElement()
        {
            Autodesk.Revit.UI.Selection.Selection selSet = m_revitApp.ActiveUIDocument.Selection;

            try
            {
                Autodesk.Revit.DB.Element elem = m_revitApp.ActiveUIDocument.Document.GetElement(selSet.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element).ElementId);
                Autodesk.Revit.DB.ElementSet elemSet = m_revitApp.Application.Create.NewElementSet();
                elemSet.Insert(elem);
                Snoop.Forms.Objects form = new Snoop.Forms.Objects(elemSet);
                form.ShowDialog();
            }
            catch(System.Exception)
            {
                System.Windows.Forms.MessageBox.Show("Didn't pick one!");
            }
        }
    }
}

