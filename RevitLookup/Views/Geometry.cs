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
using System.Windows.Forms;
using Autodesk.Revit.DB;

namespace RevitLookup.Views
{
    /// <summary>
    ///     Summary description for BindingMap form.
    /// </summary>
    public class Geometry : ObjTreeBase
    {
        private readonly Autodesk.Revit.ApplicationServices.Application _app;

        public Geometry(Element elem, Autodesk.Revit.ApplicationServices.Application app)
        {
            Text = "Element Geometry";
            _app = app;
            TvObjs.BeginUpdate();
            AddObjectsToTree(elem, TvObjs.Nodes);
            TvObjs.EndUpdate();
        }

        private void AddObjectsToTree(Element elem, TreeNodeCollection curNodes)
        {
            Options geomOp;
            TreeNode tmpNode;

            // add geometry with the View set to null.
            var rootNode1 = new TreeNode("View = null");
            curNodes.Add(rootNode1);
            foreach (ViewDetailLevel viewDetailLevel in Enum.GetValues(typeof(ViewDetailLevel)))
            {
                tmpNode = new TreeNode($"Detail Level = {viewDetailLevel}");
                // IMPORTANT!!! Need to create options each time when you are 
                // getting geometry. In other case, all the geometry you got at the 
                // previous step will be owerriten according with the latest DetailLevel
                geomOp = _app.Create.NewGeometryOptions();
                geomOp.ComputeReferences = true;
                geomOp.DetailLevel = viewDetailLevel;
                tmpNode.Tag = elem.get_Geometry(geomOp);
                rootNode1.Nodes.Add(tmpNode);
            }


            // add model geometry including geometry objects not set as Visible.
            var rootNode = new TreeNode("View = null - Including geometry objects not set as Visible");
            curNodes.Add(rootNode);
            foreach (ViewDetailLevel viewDetailLevel in Enum.GetValues(typeof(ViewDetailLevel)))
            {
                tmpNode = new TreeNode($"Detail Level = {viewDetailLevel}");
                // IMPORTANT!!! Need to create options each time when you are 
                // getting geometry. In other case, all the geometry you got at the 
                // previous step will be owerriten according with the latest DetailLevel
                geomOp = _app.Create.NewGeometryOptions();
                geomOp.ComputeReferences = true;
                geomOp.IncludeNonVisibleObjects = true;
                geomOp.DetailLevel = viewDetailLevel;
                tmpNode.Tag = elem.get_Geometry(geomOp);
                rootNode.Nodes.Add(tmpNode);
            }

            // now add geometry with the View set to the current view
            if (elem.Document.ActiveView is not null)
            {
                var geomOp2 = _app.Create.NewGeometryOptions();
                geomOp2.ComputeReferences = true;
                geomOp2.View = elem.Document.ActiveView;

                var rootNode2 = new TreeNode("View = Document.ActiveView")
                {
                    Tag = elem.get_Geometry(geomOp2)
                };
                curNodes.Add(rootNode2);

                // SOFiSTiK FS
                // add model geometry including geometry objects not set as Visible.
                var opts = _app.Create.NewGeometryOptions();
                opts.ComputeReferences = true;
                opts.IncludeNonVisibleObjects = true;
                opts.View = elem.Document.ActiveView;

                rootNode = new TreeNode("View = Document.ActiveView - Including geometry objects not set as Visible");
                curNodes.Add(rootNode);
                rootNode.Tag = elem.get_Geometry(opts);
            }
        }
    }
}