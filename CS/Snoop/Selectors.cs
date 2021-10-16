using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitLookup.Snoop
{
    enum Selector
    {
        SnoopDB = 0,
        SnoopCurrentSelection,
        SnoopPickFace,
        SnoopPickEdge,
        SnoopLinkedElement,
        SnoopDependentElements,
        SnoopActiveView,
        SnoopApplication
    }

    static class Selectors
    {

        public static object Snoop(UIApplication app, Selector selector)
        {
            switch (selector)
            {
                case Selector.SnoopDB:
                    return SnoopDB(app);
                case Selector.SnoopCurrentSelection:
                    return SnoopCurrentSelection(app);
                case Selector.SnoopPickFace:
                    return SnoopPickFace(app);
                case Selector.SnoopPickEdge:
                    return SnoopPickEdge(app);
                case Selector.SnoopLinkedElement:
                    return SnoopLinkedElement(app);
                case Selector.SnoopDependentElements:
                    return SnoopDependentElements(app);
                case Selector.SnoopActiveView:
                    return SnoopActiveView(app);
                case Selector.SnoopApplication:
                    return SnoopApplication(app);
                default:
                    throw new NotImplementedException();
            }
        }

        public static IList<Element> SnoopDB(UIApplication app)
        {            
            Autodesk.Revit.DB.Document doc = app.ActiveUIDocument.Document;
            FilteredElementCollector elemTypeCtor = (new FilteredElementCollector(doc)).WhereElementIsElementType();
            FilteredElementCollector notElemTypeCtor = (new FilteredElementCollector(doc)).WhereElementIsNotElementType();
            FilteredElementCollector allElementCtor = elemTypeCtor.UnionWith(notElemTypeCtor);
            IList<Element> founds = allElementCtor.ToElements();          

            System.Diagnostics.Trace.WriteLine(founds.Count.ToString());

            return founds;
        }
        public static IList<Element> SnoopCurrentSelection(UIApplication app)
        {
            var activeUIDocument = app.ActiveUIDocument;
            var document = activeUIDocument.Document;

            ICollection<ElementId> ids = activeUIDocument.Selection.GetElementIds();
            if (ids.Any())
            {
                return new FilteredElementCollector(document, ids).WhereElementIsNotElementType().ToElements();
            }

            return new FilteredElementCollector(document, document.ActiveView.Id).WhereElementIsNotElementType().ToElements();
        }
        public static object SnoopPickFace(UIApplication app)
        {           
            try
            {
                Reference refElem = app.ActiveUIDocument.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Face);

                GeometryObject geoObject = app.ActiveUIDocument.Document.GetElement(refElem).GetGeometryObjectFromReference(refElem);
                return geoObject;
            }
            catch
            {
                
            }

            return null;
        }
        public static object SnoopPickEdge(UIApplication app)
        {            
            try
            {
                Reference refElem = app.ActiveUIDocument.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Edge);
                GeometryObject geoObject = app.ActiveUIDocument.Document.GetElement(refElem).GetGeometryObjectFromReference(refElem);
                return geoObject;
            }
            catch
            {
               
            }

            return null;
        }
        public static Element SnoopLinkedElement(UIApplication app)
        {
            Document doc = app.ActiveUIDocument.Document;

            Reference refElem = null;
            try
            {
                refElem = app.ActiveUIDocument.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.LinkedElement);
            }
            catch
            {
                return null;
            }

            string stableReflink = refElem.ConvertToStableRepresentation(doc).Split(':')[0];
            Reference refLink = Reference.ParseFromStableRepresentation(doc, stableReflink);
            RevitLinkInstance rli_return = doc.GetElement(refLink) as RevitLinkInstance;
            var m_activeDoc = rli_return.GetLinkDocument();
            Element e = m_activeDoc.GetElement(refElem.LinkedElementId);

            return e;
        }
        public static IList<Element> SnoopDependentElements(UIApplication app)
        {
            UIDocument uidoc = app.ActiveUIDocument;
            ICollection<ElementId> idPickfirst = uidoc.Selection.GetElementIds();
            Document doc = uidoc.Document;

            if (idPickfirst.Any())
            {
                IList<Element> elemSet = new FilteredElementCollector(doc, idPickfirst).WhereElementIsNotElementType().ToElements();
                ICollection<ElementId> ids = elemSet.SelectMany(t => t.GetDependentElements(null)).ToList();
                IList<Element> result = new FilteredElementCollector(doc, ids).WhereElementIsNotElementType().ToElements();

                return result;
            }

            return new List<Element>();        
        }
        public static View SnoopActiveView(UIApplication app)
        {
            Autodesk.Revit.DB.Document doc = app.ActiveUIDocument.Document;
            if (doc.ActiveView == null)
            {
                TaskDialog.Show("RevitLookup", "The document must have an active view!");
                return null;
            }
            return doc.ActiveView;
        }
        public static object SnoopApplication(UIApplication app)
        {
            return app.Application;
        }
    }
}