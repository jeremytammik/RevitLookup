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

using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.DB;

namespace RevitLookup.Views
{
    /// <summary>
    ///     Summary description for BindingMap form.
    /// </summary>
    public class BindingMap : ObjTreeBase
    {
        public BindingMap(Autodesk.Revit.DB.BindingMap map)
        {
            Text = "Snoop Binding Map";
            TvObjs.BeginUpdate();
            AddObjectsToTree(map, TvObjs.Nodes);
            TvObjs.EndUpdate();
        }

        private void AddObjectsToTree(Autodesk.Revit.DB.BindingMap map, TreeNodeCollection curNodes)
        {
            if (map.IsEmpty) return;

            var iterator = map.ForwardIterator();
            while (iterator.MoveNext())
            {
                var def = iterator.Key;
                var elemBind = (ElementBinding) iterator.Current;

                // TBD:  not sure if this map is implemented correctly... doesn't seem to be
                // find out if this one already exists
                var defNode = curNodes
                    .Cast<TreeNode>()
                    .FirstOrDefault(tmpNode => tmpNode.Text == def.Name);

                // this one doesn't exist in the tree yet, add it.
                if (defNode is null)
                {
                    defNode = new TreeNode(def.Name)
                    {
                        Tag = iterator.Current
                    };
                    curNodes.Add(defNode);
                }

                if (elemBind is not null)
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
    }
}