using System.Diagnostics;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using View = Autodesk.Revit.DB.View;

namespace RevitLookup.Core;

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
        return selector switch
        {
            Selector.SnoopDb => SnoopDb(app),
            Selector.SnoopCurrentSelection => SnoopCurrentSelection(app),
            Selector.SnoopPickFace => SnoopPickFace(app),
            Selector.SnoopPickEdge => SnoopPickEdge(app),
            Selector.SnoopLinkedElement => SnoopLinkedElement(app),
            Selector.SnoopDependentElements => SnoopDependentElements(app),
            Selector.SnoopActiveView => SnoopActiveView(app),
            Selector.SnoopApplication => SnoopApplication(app),
            _ => throw new NotImplementedException()
        };
    }

    private static (IList<Element>, Document) SnoopDb(UIApplication app)
    {
        var doc = app.ActiveUIDocument.Document;
        var elemTypeCtor = new FilteredElementCollector(doc).WhereElementIsElementType();
        var notElemTypeCtor = new FilteredElementCollector(doc).WhereElementIsNotElementType();
        var allElementCtor = elemTypeCtor.UnionWith(notElemTypeCtor);
        var founds = allElementCtor.ToElements();

        Trace.WriteLine(founds.Count.ToString());

        return (founds, doc);
    }

    private static (IList<Element>, Document) SnoopCurrentSelection(UIApplication app)
    {
        var activeUiDocument = app.ActiveUIDocument;
        var document = activeUiDocument.Document;

        var ids = activeUiDocument.Selection.GetElementIds();

        if (ids.Count > 0)
            return (new FilteredElementCollector(document, ids)
                .WherePasses(new LogicalOrFilter(new ElementIsElementTypeFilter(false), new ElementIsElementTypeFilter(true)))
                .ToElements(), document);

        return (new FilteredElementCollector(document, document.ActiveView.Id)
            .WhereElementIsNotElementType()
            .ToElements(), document);
    }

    private static (GeometryObject, Document) SnoopPickFace(UIApplication app)
    {
        try
        {
            var refElem = app.ActiveUIDocument.Selection.PickObject(ObjectType.Face);
            var geoObject = app.ActiveUIDocument.Document.GetElement(refElem).GetGeometryObjectFromReference(refElem);
            return (geoObject, app.ActiveUIDocument.Document);
        }
        catch
        {
            // ignored
        }

        return (null, null);
    }

    private static (GeometryObject, Document) SnoopPickEdge(UIApplication app)
    {
        try
        {
            var refElem = app.ActiveUIDocument.Selection.PickObject(ObjectType.Edge);
            var geoObject = app.ActiveUIDocument.Document.GetElement(refElem).GetGeometryObjectFromReference(refElem);
            return (geoObject, app.ActiveUIDocument.Document);
        }
        catch
        {
            // ignored
        }

        return (null, null);
    }

    private static (Element, Document) SnoopLinkedElement(UIApplication app)
    {
        var document = app.ActiveUIDocument.Document;

        Reference refElem;
        try
        {
            refElem = app.ActiveUIDocument.Selection.PickObject(ObjectType.LinkedElement);
        }
        catch
        {
            return (null, null);
        }

        var representation = refElem.ConvertToStableRepresentation(document).Split(':')[0];
        var reference = Reference.ParseFromStableRepresentation(document, representation);
        var revitLinkInstance = (RevitLinkInstance) document.GetElement(reference);
        var activeDoc = revitLinkInstance.GetLinkDocument();
        var element = activeDoc.GetElement(refElem.LinkedElementId);

        return (element, activeDoc);
    }

    private static (IList<Element>, Document) SnoopDependentElements(UIApplication app)
    {
        var uiDocument = app.ActiveUIDocument;
        var document = uiDocument.Document;
        var selectedIds = uiDocument.Selection.GetElementIds();
        if (selectedIds.Count == 0) return (new List<Element>(), document);

        var elemSet = new FilteredElementCollector(document, selectedIds).WhereElementIsNotElementType().ToElements();
        ICollection<ElementId> ids = elemSet.SelectMany(t => t.GetDependentElements(null)).ToList();
        var result = new FilteredElementCollector(document, ids).WhereElementIsNotElementType().ToElements();
        return (result, document);
    }

    private static (View, Document) SnoopActiveView(UIApplication app)
    {
        var doc = app.ActiveUIDocument.Document;
        if (doc.ActiveView is not null) return (doc.ActiveView, doc);

        TaskDialog.Show("RevitLookup", "The document must have an active view!");
        return (null, null);
    }

    private static (Autodesk.Revit.ApplicationServices.Application, Document) SnoopApplication(UIApplication app)
    {
        return (app.Application, null);
    }
}