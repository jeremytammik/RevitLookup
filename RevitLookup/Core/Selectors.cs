using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using View = Autodesk.Revit.DB.View;

namespace RevitLookup.Core;

public enum Selector
{
    SnoopDb,
    SnoopCurrentSelection,
    SnoopPickFace,
    SnoopPickEdge,
    SnoopLinkedElement,
    SnoopDependentElements,
    SnoopActiveView,
    SnoopActiveDocument,
    SnoopApplication
}

public static class Selectors
{
    public static IList<Element> SnoopDatabase()
    {
        var document = RevitApi.UiApplication.ActiveUIDocument.Document;
        var elementTypes = new FilteredElementCollector(document).WhereElementIsElementType();
        var elementInstances = new FilteredElementCollector(document).WhereElementIsNotElementType();
        var elementsCollector = elementTypes.UnionWith(elementInstances);
        var elements = elementsCollector.ToElements();
        return elements;
    }

    public static IList<Element> SnoopCurrentSelection()
    {
        var document = RevitApi.Document;
        var selectedIds = RevitApi.UiApplication.ActiveUIDocument.Selection.GetElementIds();

        if (selectedIds.Count > 0)
            return new FilteredElementCollector(document, selectedIds)
                .WherePasses(new LogicalOrFilter(new ElementIsElementTypeFilter(false), new ElementIsElementTypeFilter(true)))
                .ToElements();

        return new FilteredElementCollector(document, document.ActiveView.Id)
            .WhereElementIsNotElementType()
            .ToElements();
    }

    [CanBeNull]
    public static GeometryObject SnoopPickFace()
    {
        try
        {
            var refElem = RevitApi.UiApplication.ActiveUIDocument.Selection.PickObject(ObjectType.Face);
            var geoObject = RevitApi.UiApplication.ActiveUIDocument.Document.GetElement(refElem).GetGeometryObjectFromReference(refElem);
            return geoObject;
        }
        catch
        {
            // ignored
        }

        return null;
    }

    [CanBeNull]
    public static GeometryObject SnoopPickEdge()
    {
        try
        {
            var refElem = RevitApi.UiApplication.ActiveUIDocument.Selection.PickObject(ObjectType.Edge);
            var geoObject = RevitApi.UiApplication.ActiveUIDocument.Document.GetElement(refElem).GetGeometryObjectFromReference(refElem);
            return geoObject;
        }
        catch
        {
            // ignored
        }

        return null;
    }

    [CanBeNull]
    public static Element SnoopLinkedElement()
    {
        var document = RevitApi.Document;

        Reference refElem;
        try
        {
            refElem = RevitApi.UiApplication.ActiveUIDocument.Selection.PickObject(ObjectType.LinkedElement);
        }
        catch
        {
            return null;
        }

        var representation = refElem.ConvertToStableRepresentation(document).Split(':')[0];
        var reference = Reference.ParseFromStableRepresentation(document, representation);
        var revitLinkInstance = (RevitLinkInstance) document.GetElement(reference);
        var activeDoc = revitLinkInstance.GetLinkDocument();
        var element = activeDoc.GetElement(refElem.LinkedElementId);

        return element;
    }

    public static IList<Element> SnoopDependentElements()
    {
        var uiDocument = RevitApi.UiApplication.ActiveUIDocument;
        var document = RevitApi.Document;
        var selectedIds = uiDocument.Selection.GetElementIds();
        if (selectedIds.Count == 0) return new List<Element>();

        var elemSet = new FilteredElementCollector(document, selectedIds).WhereElementIsNotElementType().ToElements();
        ICollection<ElementId> ids = elemSet.SelectMany(t => t.GetDependentElements(null)).ToList();
        var result = new FilteredElementCollector(document, ids).WhereElementIsNotElementType().ToElements();
        return result;
    }

    [CanBeNull]
    public static View SnoopActiveView()
    {
        return RevitApi.Document.ActiveView;
    }

    public static Document SnoopActiveDocument()
    {
        return RevitApi.Document;
    }

    public static Autodesk.Revit.ApplicationServices.Application SnoopApplication()
    {
        return RevitApi.UiApplication.Application;
    }
}