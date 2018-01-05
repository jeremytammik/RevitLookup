#region Header
//
// Copyright 2003-2018 by Autodesk, Inc. 
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

  public class BindingMap : RevitLookup.Snoop.Forms.ObjTreeBase
  {
    protected Autodesk.Revit.DB.BindingMap m_map = null;

    public
    BindingMap( Autodesk.Revit.DB.BindingMap map )
    {
      this.Text = "Snoop Binding Map";

      m_tvObjs.BeginUpdate();
      AddObjectsToTree( map, m_tvObjs.Nodes );
      m_tvObjs.EndUpdate();
    }

    protected void
    AddObjectsToTree( Autodesk.Revit.DB.BindingMap map, TreeNodeCollection curNodes )
    {
      if( map.IsEmpty )
        return;   // nothing to add

      // iterate over the map and add items to the tree
      Autodesk.Revit.DB.DefinitionBindingMapIterator iter = map.ForwardIterator();
      while( iter.MoveNext() )
      {
        Definition def = iter.Key;
        ElementBinding elemBind = (ElementBinding) iter.Current;

        // TBD:  not sure if this map is implemented correctly... doesn't seem to be
        // find out if this one already exists
        TreeNode defNode = null;
        foreach( TreeNode tmpNode in curNodes )
        {
          if( tmpNode.Text == def.Name )
          {
            defNode = tmpNode;
            break;
          }
        }
        // this one doesn't exist in the tree yet, add it.
        if( defNode == null )
        {
          defNode = new TreeNode( def.Name );
          defNode.Tag = iter.Current;
          curNodes.Add( defNode );
        }

        if( elemBind != null )
        {
          CategorySet cats = elemBind.Categories;
          foreach( Category cat in cats )
          {
            TreeNode tmpNode = new TreeNode( cat.Name );
            tmpNode.Tag = cat;
            defNode.Nodes.Add( tmpNode );
          }
        }
      }
    }

    new private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( BindingMap ) );
      this.SuspendLayout();
      // 
      // m_tvObjs
      // 
      this.m_tvObjs.LineColor = System.Drawing.Color.Black;
      // 
      // BindingMap
      // 
      this.ClientSize = new System.Drawing.Size( 800, 478 );
      this.Icon = ( (System.Drawing.Icon) ( resources.GetObject( "$this.Icon" ) ) );
      this.Name = "BindingMap";
      this.ResumeLayout( false );
      this.PerformLayout();

    }
  }
}
