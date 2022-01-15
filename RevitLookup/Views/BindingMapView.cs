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

using Autodesk.Revit.DB;

namespace RevitLookup.Views;

/// <summary>
///     Summary description for BindingMap form.
/// </summary>
public class BindingMapView : ObjTreeBaseView
{
    public BindingMapView(BindingMap map)
    {
        Text = "Snoop Binding Map";
        TvObjs.BeginUpdate();
        AddObjectsToTree(map, TvObjs.Nodes);
        TvObjs.EndUpdate();
    }

    private void AddObjectsToTree(BindingMap map, TreeNodeCollection nodeCollection)
    {
        var iterator = map.ForwardIterator();
        while (iterator.MoveNext())
        {
            var definition = iterator.Key;
            var node = new TreeNode(definition.Name)
            {
                Tag = iterator.Current
            };

            nodeCollection.Add(node);

            if (iterator.Current is not ElementBinding elementBinding) continue;

            var categories = elementBinding.Categories;
            foreach (Category category in categories)
            {
                var treeNode = new TreeNode(category.Name)
                {
                    Tag = category
                };
                node.Nodes.Add(treeNode);
            }
        }
    }
}