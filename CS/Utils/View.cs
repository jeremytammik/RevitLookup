using System;
using System.Collections.Generic;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.DB;


namespace RevitLookup.Utils
{
    class View
    {      
        /// <summary>
        /// Finds all the views in the active document.
        /// </summary>
        /// <param name="doc">the active document</param>
        public static ViewSet GetAllViews (Document doc)
        {
            ViewSet allViews = new ViewSet();

            FilteredElementCollector fec = new FilteredElementCollector(doc);
            ElementClassFilter elementsAreWanted = new ElementClassFilter(typeof(FloorType));
            fec.WherePasses(elementsAreWanted);
            List<Element> elements = fec.ToElements() as List<Element>;

            foreach (Element element in elements)
            {
               Autodesk.Revit.DB.View view = element as Autodesk.Revit.DB.View;

               if (null == view)
               {
                  continue;
               }
               else
               {
                  
                  ElementType objType = doc.GetElement(view.GetTypeId()) as ElementType;
                  if (null == objType || objType.Name.Equals("Drawing Sheet"))
                  {
                     continue;
                  }
                  else
                  {
                     allViews.Insert(view);
                  }
               }
            }

            return allViews;
        }


        /// <summary>
        /// Gather up all the available sheets.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public static ElementSet
        GetAllSheets (Document doc)
        {
            ElementSet allSheets = new ElementSet();
            FilteredElementCollector fec = new FilteredElementCollector(doc);
            ElementClassFilter elementsAreWanted = new ElementClassFilter(typeof(ViewSheet));
            fec.WherePasses(elementsAreWanted);
            List<Element> elements = fec.ToElements() as List<Element>;

            foreach (Element element in elements)
            {
               ViewSheet viewSheet = element as ViewSheet;

               if (null == viewSheet)
               {
                  continue;
               }
               else
               {
                  ElementId objId = viewSheet.GetTypeId();
                  if (ElementId.InvalidElementId == objId)
                  {
                     continue;
                  }
                  else
                  {
                     allSheets.Insert(viewSheet);
                  }
               }
            }

            return allSheets;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static ViewSet
        GetAvailableViewsToExport (Document doc)
        {
            ViewSet viewSet = new ViewSet();

            // TBD: using a filter iterator will only give you the base class View, not derived classes
            // like View3D or ViewDrafting.
            /*ElementFilterIterator viewIter = m_revitApp.ActiveUIDocument.Document.GetElements(typeof(Autodesk.Revit.DB.View));
            while (viewIter.MoveNext()) {
                Autodesk.Revit.DB.View tmpView = (Autodesk.Revit.DB.View)viewIter.Current;
                if (tmpView.CanBePrinted)
                    viewSet.Insert((Autodesk.Revit.DB.View)viewIter.Current);
            }*/

            FilteredElementCollector fec = new FilteredElementCollector(doc);
            ElementClassFilter elementsAreWanted = new ElementClassFilter(typeof(View));
            fec.WherePasses(elementsAreWanted);
            List<Element> elements = fec.ToElements() as List<Element>;

            foreach (Element element in elements)
            {
               Autodesk.Revit.DB.View tmpView = element as Autodesk.Revit.DB.View;

               if ((tmpView != null) && tmpView.CanBePrinted)
               {
                  viewSet.Insert(tmpView);
               }
            }

            return viewSet;
        }
    }
}
