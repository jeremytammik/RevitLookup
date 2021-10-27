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

  public class Geometry : ObjTreeBase
  {
    protected Element MElem;
    protected Autodesk.Revit.ApplicationServices.Application MApp;

    public
    Geometry( Element elem, Autodesk.Revit.ApplicationServices.Application app )
    {
      Text = "Element Geometry";

      MElem = elem;
      MApp = app;

      MTvObjs.BeginUpdate();
      AddObjectsToTree( elem, MTvObjs.Nodes );
      MTvObjs.EndUpdate();
    }

    protected void
    AddObjectsToTree( Element elem, TreeNodeCollection curNodes )
    {
      Options geomOp;

      TreeNode tmpNode;

      // add geometry with the View set to null.
      var rootNode1 = new TreeNode( "View = null" );
      curNodes.Add( rootNode1 );
      foreach( ViewDetailLevel viewDetailLevel in Enum.GetValues( typeof( ViewDetailLevel ) ) )
      {
        tmpNode = new TreeNode( "Detail Level = " + viewDetailLevel.ToString() );
        // IMPORTANT!!! Need to create options each time when you are 
        // getting geometry. In other case, all the geometry you got at the 
        // previous step will be owerriten according with the latest DetailLevel
        geomOp = MApp.Create.NewGeometryOptions();
        geomOp.ComputeReferences = true;
        geomOp.DetailLevel = viewDetailLevel;
        tmpNode.Tag = elem.get_Geometry( geomOp );
        rootNode1.Nodes.Add( tmpNode );
      }



      // add model geometry including geometry objects not set as Visible.
      var rootNode = new TreeNode( "View = null - Including geometry objects not set as Visible" );
      curNodes.Add( rootNode );
      foreach( ViewDetailLevel viewDetailLevel in Enum.GetValues( typeof( ViewDetailLevel ) ) )
      {
        tmpNode = new TreeNode( "Detail Level = " + viewDetailLevel.ToString() );
        // IMPORTANT!!! Need to create options each time when you are 
        // getting geometry. In other case, all the geometry you got at the 
        // previous step will be owerriten according with the latest DetailLevel
        geomOp = MApp.Create.NewGeometryOptions();
        geomOp.ComputeReferences = true;
        geomOp.IncludeNonVisibleObjects = true;
        geomOp.DetailLevel = viewDetailLevel;
        tmpNode.Tag = elem.get_Geometry( geomOp );
        rootNode.Nodes.Add( tmpNode );
      }

      // now add geometry with the View set to the current view
      if( elem.Document.ActiveView != null )
      {
        var geomOp2 = MApp.Create.NewGeometryOptions();
        geomOp2.ComputeReferences = true;
        geomOp2.View = elem.Document.ActiveView;

        var rootNode2 = new TreeNode( "View = Document.ActiveView" )
        {
          Tag = elem.get_Geometry( geomOp2 )
        };
        curNodes.Add( rootNode2 );

        // SOFiSTiK FS
        // add model geometry including geometry objects not set as Visible.
        {
          var opts = MApp.Create.NewGeometryOptions();
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
			var resources = new ComponentResourceManager(typeof(Geometry));
			SuspendLayout();
			// 
			// m_tvObjs
			// 
			MTvObjs.LineColor = System.Drawing.Color.Black;
			// 
			// Geometry
			// 
			ClientSize = new Size(800, 478);
			Icon = ((Icon)(resources.GetObject("$this.Icon")));
			Name = "Geometry";
			StartPosition = FormStartPosition.CenterParent;
			ResumeLayout(false);
			PerformLayout();

    }
  }




  //SOFiSTiK FS

  public class OriginalGeometry : ObjTreeBase
  {
    protected Element MElem;
    protected Autodesk.Revit.ApplicationServices.Application MApp;

    public
    OriginalGeometry( FamilyInstance elem, Autodesk.Revit.ApplicationServices.Application app )
    {
      Text = "Element Original Geometry";

      MElem = elem;
      MApp = app;

      MTvObjs.BeginUpdate();
      AddObjectsToTree( elem, MTvObjs.Nodes );
      MTvObjs.EndUpdate();
    }

    protected void
    AddObjectsToTree( FamilyInstance elem, TreeNodeCollection curNodes )
    {
      var geomOp = MApp.Create.NewGeometryOptions();
      geomOp.ComputeReferences = false; // Not allowed for GetOriginalGeometry()!
      TreeNode tmpNode;

      // add geometry with the View set to null.
      var rootNode1 = new TreeNode( "View = null" );
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
        var opts = MApp.Create.NewGeometryOptions();
        opts.ComputeReferences = false; // Not allowed for GetOriginalGeometry()!;
        opts.IncludeNonVisibleObjects = true;

        var rootNode = new TreeNode( "View = null - Including geometry objects not set as Visible" );
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
        var geomOp2 = MApp.Create.NewGeometryOptions();
        geomOp2.ComputeReferences = false; // Not allowed for GetOriginalGeometry()!;
        geomOp2.View = elem.Document.ActiveView;

        var rootNode2 = new TreeNode( "View = Document.ActiveView" )
        {
          Tag = elem.GetOriginalGeometry( geomOp2 )
        };
        curNodes.Add( rootNode2 );

        // SOFiSTiK FS
        // add model geometry including geometry objects not set as Visible.
        {
          var opts = MApp.Create.NewGeometryOptions();
          opts.ComputeReferences = false; // Not allowed for GetOriginalGeometry()!;
          opts.IncludeNonVisibleObjects = true;
          opts.View = elem.Document.ActiveView;

          var rootNode = new TreeNode( "View = Document.ActiveView - Including geometry objects not set as Visible" );
          curNodes.Add( rootNode );

          rootNode.Tag = elem.GetOriginalGeometry( opts );
        }
      }
    }
  }
}
