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

using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Color = System.Drawing.Color;

namespace RevitLookup.Forms
{
    /// <summary>
    ///     Summary description for BindingMap form.
    /// </summary>
    public class BindingMap : ObjTreeBase
    {
        protected Autodesk.Revit.DB.BindingMap MMap = null;

        public BindingMap(Autodesk.Revit.DB.BindingMap map)
        {
            Text = "Snoop Binding Map";

            MTvObjs.BeginUpdate();
            AddObjectsToTree(map, MTvObjs.Nodes);
            MTvObjs.EndUpdate();
        }

        protected void AddObjectsToTree(Autodesk.Revit.DB.BindingMap map, TreeNodeCollection curNodes)
        {
            if (map.IsEmpty)
                return; // nothing to add

            // iterate over the map and add items to the tree
            var iter = map.ForwardIterator();
            while (iter.MoveNext())
            {
                var def = iter.Key;
                var elemBind = (ElementBinding) iter.Current;

                // TBD:  not sure if this map is implemented correctly... doesn't seem to be
                // find out if this one already exists
                var defNode = curNodes
                    .Cast<TreeNode>()
                    .FirstOrDefault(tmpNode => tmpNode.Text == def.Name);

                // this one doesn't exist in the tree yet, add it.
                if (defNode == null)
                {
                    defNode = new TreeNode(def.Name)
                    {
                        Tag = iter.Current
                    };
                    curNodes.Add(defNode);
                }

                if (elemBind != null)
                {
                    var cats = elemBind.Categories;
                    foreach (Category cat in cats)
                    {
                        var tmpNode = new TreeNode(cat.Name)
                        {
                            Tag = cat
                        };
                        defNode.Nodes.Add(tmpNode);
                    }
                }
            }
        }

        private new void InitializeComponent()
        {
            var resources = new ComponentResourceManager(typeof(BindingMap));
            SuspendLayout();
            // 
            // m_tvObjs
            // 
            MTvObjs.LineColor = Color.Black;
            // 
            // BindingMap
            // 
            ClientSize = new Size(800, 478);
            Icon = (Icon) resources.GetObject("$this.Icon");
            Name = "BindingMap";
            StartPosition = FormStartPosition.CenterParent;
            ResumeLayout(false);
            PerformLayout();
        }
    }
}