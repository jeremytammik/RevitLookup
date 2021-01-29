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
	/// Summary description for Categories form.
	/// </summary>
	public class Categories : RevitLookup.Snoop.Forms.ObjTreeBase
	{
		public
		Categories( CategoryNameMap map )
		{
			this.Text = "Snoop Categories";

			m_tvObjs.BeginUpdate();
			AddObjectsToTree( map, m_tvObjs.Nodes );
			m_tvObjs.EndUpdate();
		}

		protected void
		AddObjectsToTree( CategoryNameMap map, TreeNodeCollection curNodes )
		{
			m_tvObjs.Sorted = true;

			if( map.IsEmpty )
				return;   // nothing to add

			// iterate over the map and add items to the tree
			CategoryNameMapIterator iter = map.ForwardIterator();
			while( iter.MoveNext() )
			{
				TreeNode tmpNode = new TreeNode( iter.Key );
				tmpNode.Tag = iter.Current;
				curNodes.Add( tmpNode );

				// recursively add sub-nodes (if any)
				Category curCat = (Category) iter.Current;
				AddObjectsToTree( curCat.SubCategories, tmpNode.Nodes );
			}
		}

		new private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Categories));
			this.SuspendLayout();
			// 
			// m_tvObjs
			// 
			this.m_tvObjs.LineColor = System.Drawing.Color.Black;
			// 
			// Categories
			// 
			this.ClientSize = new System.Drawing.Size(800, 478);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Categories";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
