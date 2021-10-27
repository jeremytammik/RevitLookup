using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace RevitLookup.Snoop
{
    public enum Selector
    {
        SnoopDb = 0,
        SnoopCurrentSelection,
        SnoopPickFace,
        SnoopPickEdge,
        SnoopLinkedElement,
        SnoopDependentElements,
        SnoopActiveView,
        SnoopApplication
    }

    internal static class Selectors
    {
        public static (object, Document) Snoop(UIApplication app, Selector selector)
        {
            switch (selector)
            {
                case Selector.SnoopDb:
                    return SnoopDb(app);
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

        public static (IList<Element>, Document) SnoopDb(UIApplication app)
        {
            var doc = app.ActiveUIDocument.Document;
            var elemTypeCtor = new FilteredElementCollector(doc).WhereElementIsElementType();
            var notElemTypeCtor = new FilteredElementCollector(doc).WhereElementIsNotElementType();
            var allElementCtor = elemTypeCtor.UnionWith(notElemTypeCtor);
            var founds = allElementCtor.ToElements();

            Trace.WriteLine(founds.Count.ToString());

            return (founds, doc);
        }

        public static (IList<Element>, Document) SnoopCurrentSelection(UIApplication app)
        {
            var activeUiDocument = app.ActiveUIDocument;
            var document = activeUiDocument.Document;

            var ids = activeUiDocument.Selection.GetElementIds();
            if (ids.Any()) return (new FilteredElementCollector(document, ids).WhereElementIsNotElementType().ToElements(), document);

            return (new FilteredElementCollector(document, document.ActiveView.Id).WhereElementIsNotElementType().ToElements(), document);
        }

        public static (GeometryObject, Document) SnoopPickFace(UIApplication app)
        {
            try
            {
                var refElem = app.ActiveUIDocument.Selection.PickObject(ObjectType.Face);
                var geoObject = app.ActiveUIDocument.Document.GetElement(refElem).GetGeometryObjectFromReference(refElem);
                return (geoObject, app.ActiveUIDocument.Document);
            }
            catch
            {
            }

            return (null, null);
        }

        public static (GeometryObject, Document) SnoopPickEdge(UIApplication app)
        {
            try
            {
                var refElem = app.ActiveUIDocument.Selection.PickObject(ObjectType.Edge);
                var geoObject = app.ActiveUIDocument.Document.GetElement(refElem).GetGeometryObjectFromReference(refElem);
                return (geoObject, app.ActiveUIDocument.Document);
            }
            catch
            {
            }

            return (null, null);
        }

        public static (Element, Document) SnoopLinkedElement(UIApplication app)
        {
            var doc = app.ActiveUIDocument.Document;

            Reference refElem = null;
            try
            {
                refElem = app.ActiveUIDocument.Selection.PickObject(ObjectType.LinkedElement);
            }
            catch
            {
                return (null, null);
            }

            var stableReflink = refElem.ConvertToStableRepresentation(doc).Split(':')[0];
            var refLink = Reference.ParseFromStableRepresentation(doc, stableReflink);
            var rliReturn = doc.GetElement(refLink) as RevitLinkInstance;
            var mActiveDoc = rliReturn.GetLinkDocument();
            var e = mActiveDoc.GetElement(refElem.LinkedElementId);

            return (e, mActiveDoc);
        }

        public static (IList<Element>, Document) SnoopDependentElements(UIApplication app)
        {
            var uidoc = app.ActiveUIDocument;
            var idPickfirst = uidoc.Selection.GetElementIds();
            var doc = uidoc.Document;

            if (idPickfirst.Any())
            {
                var elemSet = new FilteredElementCollector(doc, idPickfirst).WhereElementIsNotElementType().ToElements();
                ICollection<ElementId> ids = elemSet.SelectMany(t => t.GetDependentElements(null)).ToList();
                var result = new FilteredElementCollector(doc, ids).WhereElementIsNotElementType().ToElements();

                return (result, doc);
            }

            return (new List<Element>(), doc);
        }

        public static (View, Document) SnoopActiveView(UIApplication app)
        {
            var doc = app.ActiveUIDocument.Document;
            if (doc.ActiveView == null)
            {
                TaskDialog.Show("RevitLookup", "The document must have an active view!");
                return (null, null);
            }

            return (doc.ActiveView, doc);
        }

        public static (Application, Document) SnoopApplication(UIApplication app)
        {
            return (app.Application, null);
        }
    }
}