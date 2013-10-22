#region Header
//
// Copyright 2003-2013 by Autodesk, Inc. 
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
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using Autodesk.Revit;
using Autodesk.Revit.DB;



namespace RevitLookup.Test.SDKSamples.CreateSheet {

    public class Views {

        private ViewSet         m_allViews = new ViewSet();
        private ViewSet         m_selectedViews = new ViewSet();
        private FamilySymbol    m_titleBlock = null;
        private FamilySymbolSet m_allTitleBlocks = new FamilySymbolSet();
        private ArrayList       m_titleBlockNames = new ArrayList();
        private string          m_sheetName = null;
        private double          m_rows = 0;

        private double TITLEBAR         = 0.2;
        private double GOLDENSECTION    = 0.618;

        /// <summary>
        /// Constructor of views object.
        /// </summary>
        /// <param name="doc">the active document</param>
        
        public
        Views(Document doc)
        {
            m_allViews = Utils.View.GetAllViews(doc);            
            GetTitleBlocks(doc);
        }

        /// <summary>
        /// Tree node store all views' names.
        /// </summary>
        
        public ViewSet
        AllViews
        {
            get { return m_allViews; }
        }

        /// <summary>
        /// List of all title blocks' names.
        /// </summary>
        
        public ArrayList
        TitleBlockNames
        {
            get { return m_titleBlockNames; }
        }

        /// <summary>
        /// The selected sheet's name.
        /// </summary>
        
        public string
        SheetName
        {
            get { return m_sheetName;  }
            set { m_sheetName = value; }
        }       

        /// <summary>
        /// Set the views to generate from a list of ViewNames.
        /// </summary>
        
        public void
        SetSelectViewsFromNames(ArrayList viewNames)
        {
            foreach (Autodesk.Revit.DB.View v in m_allViews) {
                foreach (string s in viewNames) {
                    if (s.Equals(v.Name)) {
                        m_selectedViews.Insert(v);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Generate sheet in active document.
        /// </summary>
        /// <param name="doc">the currently active document</param>
        
        public void
        GenerateSheet(Document doc)
        {
            if (0 == m_selectedViews.Size) {
                MessageBox.Show("No view was selected, sheet generation cancelled.");
                return;
            }
            
            ViewSheet sheet = ViewSheet.Create(doc, m_titleBlock.Id);
            sheet.Name = m_sheetName;
            PlaceViews(m_selectedViews, sheet);
        }

        /// <summary>
        /// Retrieve the title block to be generate by its name.
        /// </summary>
        /// <param name="name">The title block's name</param>
        
        public void
        SetTitleBlock(string name)
        {
            foreach (FamilySymbol f in m_allTitleBlocks) {
                if (name.Equals(f.Name)) {
                    m_titleBlock = f;
                    return;
                }
            }
        }

        /// <summary>
        /// Retrieve all available title blocks in the currently active document.
        /// </summary>
        /// <param name="doc">the currently active document</param>
        
        private void
        GetTitleBlocks(Document doc)
        {
            m_allTitleBlocks = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_TitleBlocks).Cast<FamilySymbol>() as FamilySymbolSet;
            if (0 == m_allTitleBlocks.Size) {
                throw new Exception("There is no title block to generate sheet.");
            }

            foreach (FamilySymbol f in m_allTitleBlocks) {
                m_titleBlockNames.Add(f.Name);

                if (null == m_titleBlock) {
                    m_titleBlock = f;
                }
            }
        }

        /// <summary>
        /// Place all selected views on this sheet's appropriate location.
        /// </summary>
        /// <param name="views">all selected views</param>
        /// <param name="sheet">all views located sheet</param>
        
        private void
        PlaceViews(ViewSet views, ViewSheet sheet)
        {
            double xDistance = 0;
            double yDistance = 0;
            CalculateDistance(sheet.Outline, views.Size, ref xDistance, ref yDistance);

            UV origin = GetOffset(sheet.Outline, xDistance, yDistance);
            UV temp = new UV(origin.U, origin.V);

            int n = 1;
            foreach (Autodesk.Revit.DB.View v in views) {
                UV location = new UV(temp.U, temp.V);
                Autodesk.Revit.DB.View view = v;
                Rescale(view, xDistance, yDistance);
                Viewport.Create(view.Document, sheet.Id, view.Id, new XYZ(location.U, location.V, 0));

                if (0 != n++ % m_rows) {
                    temp = new UV( temp.U + xDistance * (1 - TITLEBAR), temp.V );                    
                }
                else {
                    temp = new UV( origin.U,temp.V + yDistance);
                }
            }
        }

        /// <summary>
        /// Retrieve the appropriate origin.
        /// </summary>
        /// <param name="bBox">The 2D outline of the sheet</param>
        /// <returns>The appropriate origin</returns>
        
        private UV
        GetOffset(BoundingBoxUV bBox, double x, double y)
        {
            return new UV(bBox.Min.U + x * GOLDENSECTION, bBox.Min.V + y * GOLDENSECTION);
        }

        /// <summary>
        /// Calculate the appropriate distance between the views lay on the sheet.
        /// </summary>
        /// <param name="bBox">The outline of sheet.</param>
        /// <param name="amount">Amount of views.</param>
        /// <param name="x">Distance in x axis between each view</param>
        /// <param name="y">Distance in y axis between each view</param>
       
        private void
        CalculateDistance(BoundingBoxUV bBox, int amount, ref double x, ref double y)
        {
            double xLength = (bBox.Max.U - bBox.Min.U) * (1 - TITLEBAR);
            double yLength = (bBox.Max.V - bBox.Min.V);

            //calculate appropriate rows numbers.
            double result = Math.Sqrt(amount);

            while (0 < (result - (int)result)) {
                amount = amount + 1;
                result = Math.Sqrt(amount);
            }
            m_rows = result;
            double area = xLength * yLength / amount;

            //calculate appropriate distance between the views.
            if (bBox.Max.U > bBox.Max.V) {
                x = Math.Sqrt(area / GOLDENSECTION);
                y = GOLDENSECTION * x;
            }
            else {
                y = Math.Sqrt(area / GOLDENSECTION);
                x = GOLDENSECTION * y;
            }
        }

        /// <summary>
        /// Rescale the view's Scale value for suitable.
        /// </summary>
        /// <param name="view">The view to be located on sheet.</param>
        /// <param name="x">Distance in x axis between each view</param>
        /// <param name="y">Distance in y axis between each view</param>
        
        private void
        Rescale(Autodesk.Revit.DB.View view, double x, double y)
        {
            double Rescale = 2;
            UV outline = new UV(view.Outline.Max.U - view.Outline.Min.U,
                view.Outline.Max.V - view.Outline.Min.V);

            if (outline.U > outline.V) {
                Rescale = outline.U / x * Rescale;
            }
            else {
                Rescale = outline.V / y * Rescale;
            }

            if (1 != view.Scale) {
                view.Scale = (int)(view.Scale * Rescale);
            }
        }
    }
}
