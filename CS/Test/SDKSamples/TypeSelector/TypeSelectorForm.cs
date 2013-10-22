
//
// Copyright 2003-2010 by Autodesk, Inc.
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

using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using Autodesk.Revit;
using Autodesk.Revit.DB;


namespace RevitLookup.Test.SDKSamples.TypeSelector
{

   public partial class TypeSelectorForm : System.Windows.Forms.Form
   {

      protected System.Object m_curObj;
      protected ArrayList m_treeTypeNodes = new ArrayList();
      protected ArrayList m_types = new ArrayList();

      protected Autodesk.Revit.UI.UIApplication m_app = null;
      protected List<Element> m_symbolSet;
      protected int m_curSymbolId = 0;

      public
        TypeSelectorForm(Autodesk.Revit.UI.UIApplication revitApp, ElementSet elemSet)
      {
         m_app = revitApp;
         m_symbolSet = new List<Element>();

         InitializeComponent();
         CommonInit(elemSet);
      }

      protected void
      CommonInit(IEnumerable objs)
      {
         m_tvObjs.BeginUpdate();

         AddObjectsToTree(objs);

         // if the tree isn't well populated, expand it and select the first item
         // so its not a pain for the user when there is only one relevant item in the tree
         if (m_tvObjs.Nodes.Count == 1)
         {
            m_tvObjs.Nodes[0].Expand();
            if (m_tvObjs.Nodes[0].Nodes.Count == 0)
               m_tvObjs.SelectedNode = m_tvObjs.Nodes[0];
            else
               m_tvObjs.SelectedNode = m_tvObjs.Nodes[0].Nodes[0];
         }

         m_tvObjs.EndUpdate();
      }

      protected void
      AddObjectsToTree(IEnumerable objs)
      {
         m_tvObjs.Sorted = true;

         // initialize the tree control
         foreach (Object tmpObj in objs)
         {
            // hook this up to the correct spot in the tree based on the object's type
            TreeNode parentNode = GetExistingNodeForType(tmpObj.GetType());
            if (parentNode == null)
            {
               parentNode = new TreeNode(tmpObj.GetType().Name);
               m_tvObjs.Nodes.Add(parentNode);

               // record that we've seen this one
               m_treeTypeNodes.Add(parentNode);
               m_types.Add(tmpObj.GetType());
            }


            // add the new node for this element
            TreeNode tmpNode = new TreeNode(Snoop.Utils.ObjToLabelStr(tmpObj));
            tmpNode.Tag = tmpObj;
            parentNode.Nodes.Add(tmpNode);
         }
      }

      /// <summary>
      /// If we've already seen this type before, return the existing TreeNode object
      /// </summary>
      /// <param name="objType">System.Type we're looking to find</param>
      /// <returns>The existing TreeNode or NULL</returns> 

      protected TreeNode
      GetExistingNodeForType(System.Type objType)
      {
         int len = m_types.Count;
         for (int i = 0; i < len; i++)
         {
            if ((System.Type)m_types[i] == objType)
               return (TreeNode)m_treeTypeNodes[i];
         }

         return null;
      }

      /// <summary>
      /// User selected a node of the tree.  Just change what is displayed in
      /// the ListBox for the current node.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>

      private void
      OnTreeNodeSelect(object sender, TreeViewEventArgs e)
      {
         m_curObj = e.Node.Tag;

         GetAvailableSymbols(m_curObj);
         DisplayAvailableSymbols();
      }

      /// <summary>
      /// For a given Element, get all available Symbols that it could be set to.
      /// NOTE: since there is no virtual function to get and set Symbols for an
      /// element, we have to have a switch-like statement and treat each known type
      /// manually.
      /// </summary>
      /// <param name="curObj">The Element that was selected in the Tree</param>

      protected void
      GetAvailableSymbols(System.Object curObj)
      {
         m_symbolSet.Clear();    // clear out any old set
         m_curSymbolId = 0;

         Element element = curObj as Element;
         if (element == null)
            return;     // Could have been a ClassType node, and therefore null

         if (element is FamilyInstance)
         {
            FamilyInstance component = (FamilyInstance)element;
            m_curSymbolId = component.Symbol.Id.IntegerValue;

            foreach (FamilySymbol familySymbol in component.Symbol.Family.Symbols)
            {
               m_symbolSet.Add(familySymbol);
            }
         }
         else if (element is Wall)
         {
            Wall wall = (Wall)element;
            m_curSymbolId = wall.WallType.Id.IntegerValue;
            foreach (WallType wallType in new FilteredElementCollector(element.Document).OfClass(typeof(WallType)).Cast<WallType>())
            {
               m_symbolSet.Add(wallType);
            }
         }
      }

      /// <summary>
      /// The list of available Symbols.  Need to just walk the list and 
      /// print out a human read-able label
      /// </summary>

      protected void
      DisplayAvailableSymbols()
      {
         m_lbSymbols.Items.Clear();
         if (0 == m_symbolSet.Count)
         {
            m_lbSymbols.Enabled = false;
            return;
         }
         else
            m_lbSymbols.Enabled = true;

         int curIndex = -1;
         int i = -1;
         string labelStr;

         foreach (ElementType sym in m_symbolSet)
         {
            Category cat = sym.Category;
            if (cat != null)    // TBD: some WallTypes have a null Category (seems like a bug?)
               labelStr = string.Format("{0} : {1}", sym.Category.Name, sym.Name);
            else
               labelStr = sym.Name;

            m_lbSymbols.Items.Add(new Utils.StrElementIdPair(labelStr, sym.Id));
            i++;
            if (sym.Id.IntegerValue == m_curSymbolId)
               curIndex = i;
         }

         m_lbSymbols.SelectedIndex = curIndex;
      }

      /// <summary>
      /// User selected a different item in the ListBox, update the underlying Element to point
      /// to the new Symbol selected.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>

      private void
      OnTypeChanged(object sender, EventArgs e)
      {
         Utils.StrElementIdPair selItem = (Utils.StrElementIdPair)m_lbSymbols.Items[m_lbSymbols.SelectedIndex];

         if (m_curObj is FamilyInstance)
         {
            FamilyInstance component = (FamilyInstance)m_curObj;

            ElementId tmpId = selItem.Id;
            FamilySymbol familySymbol = (FamilySymbol)m_app.ActiveUIDocument.Document.GetElement(tmpId);  // TBD: strange signature!

            component.Symbol = familySymbol;
         }
         else if (m_curObj is Wall)
         {
            Wall wall = (Wall)m_curObj;

            ElementId tmpId = selItem.Id;
            WallType wallType = (WallType)m_app.ActiveUIDocument.Document.GetElement(tmpId);  // TBD: strange signature!

            wall.WallType = wallType;
         }
      }

   }
}