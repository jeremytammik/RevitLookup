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
	public class Categories : ObjTreeBase
	{
		public
		Categories( CategoryNameMap map )
		{
			Text = "Snoop Categories";

			MTvObjs.BeginUpdate();
			AddObjectsToTree( map, MTvObjs.Nodes );
			MTvObjs.EndUpdate();
		}

		protected void
		AddObjectsToTree( CategoryNameMap map, TreeNodeCollection curNodes )
		{
			MTvObjs.Sorted = true;

			if( map.IsEmpty )
				return;   // nothing to add

			// iterate over the map and add items to the tree
			var iter = map.ForwardIterator();
			while( iter.MoveNext() )
			{
				var tmpNode = new TreeNode( iter.Key );
				tmpNode.Tag = iter.Current;
				curNodes.Add( tmpNode );

				// recursively add sub-nodes (if any)
				var curCat = (Category) iter.Current;
				AddObjectsToTree( curCat.SubCategories, tmpNode.Nodes );
			}
		}

		new private void InitializeComponent()
		{
			var resources = new ComponentResourceManager(typeof(Categories));
			SuspendLayout();
			// 
			// m_tvObjs
			// 
			MTvObjs.LineColor = System.Drawing.Color.Black;
			// 
			// Categories
			// 
			ClientSize = new Size(800, 478);
			Icon = ((Icon)(resources.GetObject("$this.Icon")));
			Name = "Categories";
			StartPosition = FormStartPosition.CenterParent;
			ResumeLayout(false);
			PerformLayout();

		}
	}
}
