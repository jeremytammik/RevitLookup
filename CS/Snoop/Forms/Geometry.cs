#region Header
//
// Copyright 2003-2013 by Autodesk, Inc. 
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
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.Forms
{
   /// <summary>
   /// Summary description for BindingMap form.
   /// </summary>

   public class Geometry : RevitLookup.Snoop.Forms.ObjTreeBase
   {
      protected Element m_elem = null;
      protected Autodesk.Revit.ApplicationServices.Application m_app = null;

      public
      Geometry(Autodesk.Revit.DB.Element elem, Autodesk.Revit.ApplicationServices.Application app)
      {
         this.Text = "Element Geometry";

         m_elem = elem;
         m_app = app;

         m_tvObjs.BeginUpdate();
         AddObjectsToTree(elem, m_tvObjs.Nodes);
         m_tvObjs.EndUpdate();
      }

      protected void
      AddObjectsToTree(Element elem, TreeNodeCollection curNodes)
      {
         Autodesk.Revit.DB.Options geomOp = m_app.Create.NewGeometryOptions();
         TreeNode tmpNode;

         // add geometry with the View set to null.
         TreeNode rootNode1 = new TreeNode("View = null");
         curNodes.Add(rootNode1);

         tmpNode = new TreeNode("Detail Level = Undefined");
         geomOp.DetailLevel = ViewDetailLevel.Undefined;
         tmpNode.Tag = elem.get_Geometry(geomOp);
         rootNode1.Nodes.Add(tmpNode);

         tmpNode = new TreeNode("Detail Level = Coarse");
         geomOp.DetailLevel = ViewDetailLevel.Coarse;
         tmpNode.Tag = elem.get_Geometry(geomOp);
         rootNode1.Nodes.Add(tmpNode);

         tmpNode = new TreeNode("Detail Level = Medium");
         geomOp.DetailLevel = ViewDetailLevel.Medium;
         tmpNode.Tag = elem.get_Geometry(geomOp);
         rootNode1.Nodes.Add(tmpNode);

         tmpNode = new TreeNode("Detail Level = Fine");
         geomOp.DetailLevel = ViewDetailLevel.Fine;
         tmpNode.Tag = elem.get_Geometry(geomOp);
         rootNode1.Nodes.Add(tmpNode);


         // now add geometry with the View set to the current view
         if (elem.Document.ActiveView != null)
         {
            Options geomOp2 = m_app.Create.NewGeometryOptions();
            geomOp2.View = elem.Document.ActiveView;

            TreeNode rootNode2 = new TreeNode("View = Document.ActiveView");
            rootNode2.Tag = elem.get_Geometry(geomOp2);
            curNodes.Add(rootNode2);
         }
      }

   }
}

