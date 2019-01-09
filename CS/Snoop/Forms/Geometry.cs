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
    Geometry( Autodesk.Revit.DB.Element elem, Autodesk.Revit.ApplicationServices.Application app )
    {
      this.Text = "Element Geometry";

      m_elem = elem;
      m_app = app;

      m_tvObjs.BeginUpdate();
      AddObjectsToTree( elem, m_tvObjs.Nodes );
      m_tvObjs.EndUpdate();
    }

    protected void
    AddObjectsToTree( Element elem, TreeNodeCollection curNodes )
    {
      Autodesk.Revit.DB.Options geomOp;

      TreeNode tmpNode;

      // add geometry with the View set to null.
      TreeNode rootNode1 = new TreeNode( "View = null" );
      curNodes.Add( rootNode1 );
      foreach( ViewDetailLevel viewDetailLevel in Enum.GetValues( typeof( ViewDetailLevel ) ) )
      {
        tmpNode = new TreeNode( "Detail Level = " + viewDetailLevel.ToString() );
        // IMPORTANT!!! Need to create options each time when you are 
        // getting geometry. In other case, all the geometry you got at the 
        // previous step will be owerriten according with the latest DetailLevel
        geomOp = m_app.Create.NewGeometryOptions();
        geomOp.ComputeReferences = true;
        geomOp.DetailLevel = viewDetailLevel;
        tmpNode.Tag = elem.get_Geometry( geomOp );
        rootNode1.Nodes.Add( tmpNode );
      }



      // add model geometry including geometry objects not set as Visible.
      TreeNode rootNode = new TreeNode( "View = null - Including geometry objects not set as Visible" );
      curNodes.Add( rootNode );
      foreach( ViewDetailLevel viewDetailLevel in Enum.GetValues( typeof( ViewDetailLevel ) ) )
      {
        tmpNode = new TreeNode( "Detail Level = " + viewDetailLevel.ToString() );
        // IMPORTANT!!! Need to create options each time when you are 
        // getting geometry. In other case, all the geometry you got at the 
        // previous step will be owerriten according with the latest DetailLevel
        geomOp = m_app.Create.NewGeometryOptions();
        geomOp.ComputeReferences = true;
        geomOp.IncludeNonVisibleObjects = true;
        geomOp.DetailLevel = viewDetailLevel;
        tmpNode.Tag = elem.get_Geometry( geomOp );
        rootNode.Nodes.Add( tmpNode );
      }

      // now add geometry with the View set to the current view
      if( elem.Document.ActiveView != null )
      {
        Options geomOp2 = m_app.Create.NewGeometryOptions();
        geomOp2.ComputeReferences = true;
        geomOp2.View = elem.Document.ActiveView;

        TreeNode rootNode2 = new TreeNode( "View = Document.ActiveView" );
        rootNode2.Tag = elem.get_Geometry( geomOp2 );
        curNodes.Add( rootNode2 );

        // SOFiSTiK FS
        // add model geometry including geometry objects not set as Visible.
        {
          Autodesk.Revit.DB.Options opts = m_app.Create.NewGeometryOptions();
          opts.ComputeReferences = true;
          opts.IncludeNonVisibleObjects = true;
          opts.View = elem.Document.ActiveView;

          rootNode = new TreeNode( "View = Document.ActiveView - Including geometry objects not set as Visible" );
          curNodes.Add( rootNode );

          rootNode.Tag = elem.get_Geometry( opts );
        }
      }
    }

    new private void InitializeComponent()
    {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Geometry));
			this.SuspendLayout();
			// 
			// m_tvObjs
			// 
			this.m_tvObjs.LineColor = System.Drawing.Color.Black;
			// 
			// Geometry
			// 
			this.ClientSize = new System.Drawing.Size(800, 478);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Geometry";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.ResumeLayout(false);
			this.PerformLayout();

    }
  }




  //SOFiSTiK FS

  public class OriginalGeometry : RevitLookup.Snoop.Forms.ObjTreeBase
  {
    protected Element m_elem = null;
    protected Autodesk.Revit.ApplicationServices.Application m_app = null;

    public
    OriginalGeometry( Autodesk.Revit.DB.FamilyInstance elem, Autodesk.Revit.ApplicationServices.Application app )
    {
      this.Text = "Element Original Geometry";

      m_elem = elem;
      m_app = app;

      m_tvObjs.BeginUpdate();
      AddObjectsToTree( elem, m_tvObjs.Nodes );
      m_tvObjs.EndUpdate();
    }

    protected void
    AddObjectsToTree( FamilyInstance elem, TreeNodeCollection curNodes )
    {
      Autodesk.Revit.DB.Options geomOp = m_app.Create.NewGeometryOptions();
      geomOp.ComputeReferences = false; // Not allowed for GetOriginalGeometry()!
      TreeNode tmpNode;

      // add geometry with the View set to null.
      TreeNode rootNode1 = new TreeNode( "View = null" );
      curNodes.Add( rootNode1 );

      tmpNode = new TreeNode( "Detail Level = Undefined" );
      geomOp.DetailLevel = ViewDetailLevel.Undefined;
      tmpNode.Tag = elem.GetOriginalGeometry( geomOp );
      rootNode1.Nodes.Add( tmpNode );

      tmpNode = new TreeNode( "Detail Level = Coarse" );
      geomOp.DetailLevel = ViewDetailLevel.Coarse;
      tmpNode.Tag = elem.GetOriginalGeometry( geomOp );
      rootNode1.Nodes.Add( tmpNode );

      tmpNode = new TreeNode( "Detail Level = Medium" );
      geomOp.DetailLevel = ViewDetailLevel.Medium;
      tmpNode.Tag = elem.GetOriginalGeometry( geomOp );
      rootNode1.Nodes.Add( tmpNode );

      tmpNode = new TreeNode( "Detail Level = Fine" );
      geomOp.DetailLevel = ViewDetailLevel.Fine;
      tmpNode.Tag = elem.GetOriginalGeometry( geomOp );
      rootNode1.Nodes.Add( tmpNode );

      // SOFiSTiK FS
      // add model geometry including geometry objects not set as Visible.
      {
        Autodesk.Revit.DB.Options opts = m_app.Create.NewGeometryOptions();
        opts.ComputeReferences = false; // Not allowed for GetOriginalGeometry()!;
        opts.IncludeNonVisibleObjects = true;

        TreeNode rootNode = new TreeNode( "View = null - Including geometry objects not set as Visible" );
        curNodes.Add( rootNode );

        tmpNode = new TreeNode( "Detail Level = Undefined" );
        opts.DetailLevel = ViewDetailLevel.Undefined;
        tmpNode.Tag = elem.GetOriginalGeometry( opts );
        rootNode.Nodes.Add( tmpNode );

        tmpNode = new TreeNode( "Detail Level = Coarse" );
        opts.DetailLevel = ViewDetailLevel.Coarse;
        tmpNode.Tag = elem.GetOriginalGeometry( opts );
        rootNode.Nodes.Add( tmpNode );

        tmpNode = new TreeNode( "Detail Level = Medium" );
        opts.DetailLevel = ViewDetailLevel.Medium;
        tmpNode.Tag = elem.GetOriginalGeometry( opts );
        rootNode.Nodes.Add( tmpNode );

        tmpNode = new TreeNode( "Detail Level = Fine" );
        opts.DetailLevel = ViewDetailLevel.Fine;
        tmpNode.Tag = elem.GetOriginalGeometry( opts );
        rootNode.Nodes.Add( tmpNode );
      }

      // now add geometry with the View set to the current view
      if( elem.Document.ActiveView != null )
      {
        Options geomOp2 = m_app.Create.NewGeometryOptions();
        geomOp2.ComputeReferences = false; // Not allowed for GetOriginalGeometry()!;
        geomOp2.View = elem.Document.ActiveView;

        TreeNode rootNode2 = new TreeNode( "View = Document.ActiveView" );
        rootNode2.Tag = elem.GetOriginalGeometry( geomOp2 );
        curNodes.Add( rootNode2 );

        // SOFiSTiK FS
        // add model geometry including geometry objects not set as Visible.
        {
          Autodesk.Revit.DB.Options opts = m_app.Create.NewGeometryOptions();
          opts.ComputeReferences = false; // Not allowed for GetOriginalGeometry()!;
          opts.IncludeNonVisibleObjects = true;
          opts.View = elem.Document.ActiveView;

          TreeNode rootNode = new TreeNode( "View = Document.ActiveView - Including geometry objects not set as Visible" );
          curNodes.Add( rootNode );

          rootNode.Tag = elem.GetOriginalGeometry( opts );
        }
      }
    }
  }
}
