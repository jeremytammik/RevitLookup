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
public class GeometryView : ObjTreeBaseView
{
    public GeometryView(Element elem)
    {
        Text = "Element Geometry";
        TvObjs.BeginUpdate();
        AddObjectsToTree(elem, TvObjs.Nodes);
        TvObjs.EndUpdate();
    }

    private void AddObjectsToTree(Element elem, TreeNodeCollection nodes)
    {
        var rootNode1 = new TreeNode("Undefined View");
        nodes.Add(rootNode1);
        foreach (ViewDetailLevel detailLevel in Enum.GetValues(typeof(ViewDetailLevel)))
        {
            var treeNode = new TreeNode($"Detail Level: {detailLevel}");
            // IMPORTANT!!! Need to create options each time when you are 
            // getting geometry. In other case, all the geometry you got at the 
            // previous step will be overridden according with the latest DetailLevel
            var options1 = new Options
            {
                DetailLevel = detailLevel,
                ComputeReferences = true
            };
            treeNode.Tag = elem.get_Geometry(options1);
            rootNode1.Nodes.Add(treeNode);
        }

        var rootNode2 = new TreeNode("Undefined View, including non-visible objects");
        nodes.Add(rootNode2);
        foreach (ViewDetailLevel detailLevel in Enum.GetValues(typeof(ViewDetailLevel)))
        {
            var treeNode = new TreeNode($"Detail Level: {detailLevel}");
            var options2 = new Options
            {
                DetailLevel = detailLevel,
                ComputeReferences = true,
                IncludeNonVisibleObjects = true
            };
            treeNode.Tag = elem.get_Geometry(options2);
            rootNode2.Nodes.Add(treeNode);
        }

        if (elem.Document.ActiveView is null) return;

        var options3 = new Options
        {
            View = elem.Document.ActiveView,
            ComputeReferences = true
        };

        var rootNode3 = new TreeNode("Active View")
        {
            Tag = elem.get_Geometry(options3)
        };
        nodes.Add(rootNode3);

        var options4 = new Options
        {
            View = elem.Document.ActiveView,
            ComputeReferences = true,
            IncludeNonVisibleObjects = true
        };

        var rootNode4 = new TreeNode("Active View, including non-visible objects");
        nodes.Add(rootNode4);
        rootNode4.Tag = elem.get_Geometry(options4);
    }
}