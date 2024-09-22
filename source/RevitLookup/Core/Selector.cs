// Copyright 2003-2024 by Autodesk, Inc.
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

using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.ExternalService;
using Autodesk.Revit.UI.Selection;
using Autodesk.Windows;
using RevitLookup.Services.Enums;

namespace RevitLookup.Core;

public static class Selector
{
    public static IList<SnoopableObject> Snoop(SnoopableType type)
    {
        return type switch
        {
            SnoopableType.View => SnoopView(),
            SnoopableType.Document => SnoopDocument(),
            SnoopableType.Application => SnoopApplication(),
            SnoopableType.UiApplication => SnoopUiApplication(),
            SnoopableType.Database => SnoopDatabase(),
            SnoopableType.DependentElements => SnoopDependentElements(),
            SnoopableType.Selection => SnoopSelection(),
            SnoopableType.Face => SnoopFace(),
            SnoopableType.Edge => SnoopEdge(),
            SnoopableType.SubElement => SnoopSubElement(),
            SnoopableType.Point => SnoopPoint(),
            SnoopableType.LinkedElement => SnoopLinkedElement(),
            SnoopableType.ComponentManager => SnoopComponentManager(),
            SnoopableType.PerformanceAdviser => SnoopPerformanceAdviser(),
            SnoopableType.UpdaterRegistry => SnoopUpdaterRegistry(),
            SnoopableType.Services => SnoopServices(),
            SnoopableType.Schemas => SnoopSchemas(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    private static IList<SnoopableObject> SnoopView()
    {
        return [new SnoopableObject(Context.ActiveView)];
    }

    private static IList<SnoopableObject> SnoopDocument()
    {
        return [new SnoopableObject(Context.ActiveDocument)];
    }

    private static IList<SnoopableObject> SnoopApplication()
    {
        return [new SnoopableObject(Context.Application)];
    }

    private static IList<SnoopableObject> SnoopUiApplication()
    {
        return [new SnoopableObject(Context.UiApplication)];
    }

    private static IList<SnoopableObject> SnoopEdge()
    {
        return [SelectObject(ObjectType.Edge)];
    }

    private static IList<SnoopableObject> SnoopFace()
    {
        return [SelectObject(ObjectType.Face)];
    }

    private static IList<SnoopableObject> SnoopSubElement()
    {
        return [SelectObject(ObjectType.Subelement)];
    }

    private static IList<SnoopableObject> SnoopPoint()
    {
        return [SelectObject(ObjectType.PointOnElement)];
    }

    private static IList<SnoopableObject> SnoopLinkedElement()
    {
        return [SelectObject(ObjectType.LinkedElement)];
    }

    private static IList<SnoopableObject> SnoopSelection()
    {
        var selectedIds = Context.UiApplication.ActiveUIDocument.Selection.GetElementIds();
        if (selectedIds.Count > 0)
            return Context.ActiveDocument
                .GetElements(selectedIds)
                .WherePasses(new ElementIdSetFilter(selectedIds))
                .Select(element => new SnoopableObject(element))
                .ToArray();

        return Context.ActiveDocument
            .GetElements(Context.ActiveView!.Id)
            .Select(element => new SnoopableObject(element))
            .ToArray();
    }

    private static IList<SnoopableObject> SnoopDatabase()
    {
        var elementTypes = Context.ActiveDocument.GetElements().WhereElementIsElementType();
        var elementInstances = Context.ActiveDocument.GetElements().WhereElementIsNotElementType();
        return elementTypes
            .UnionWith(elementInstances)
            .Select(element => new SnoopableObject(element))
            .ToArray();
    }

    private static IList<SnoopableObject> SnoopDependentElements()
    {
        var selectedIds = Context.ActiveUiDocument!.Selection.GetElementIds();
        if (selectedIds.Count == 0) return Array.Empty<SnoopableObject>();

        var elements = new List<ElementId>();
        var selectedElements = Context.ActiveDocument.GetElements(selectedIds).WhereElementIsNotElementType();

        foreach (var selectedElement in selectedElements)
        {
            var dependentElements = selectedElement.GetDependentElements(null);
            foreach (var dependentElement in dependentElements) elements.Add(dependentElement);
        }

        return Context.ActiveDocument.GetElements()
            .WherePasses(new ElementIdSetFilter(elements))
            .Select(element => new SnoopableObject(element))
            .ToArray();
    }

    private static IList<SnoopableObject> SnoopComponentManager()
    {
        return [new SnoopableObject(typeof(ComponentManager))];
    }

    private static IList<SnoopableObject> SnoopPerformanceAdviser()
    {
        return [new SnoopableObject(PerformanceAdviser.GetPerformanceAdviser())];
    }

    private static IList<SnoopableObject> SnoopUpdaterRegistry()
    {
        return UpdaterRegistry.GetRegisteredUpdaterInfos().Select(schema => new SnoopableObject(schema)).ToArray();
    }

    private static IList<SnoopableObject> SnoopSchemas()
    {
        return Schema.ListSchemas().Select(schema => new SnoopableObject(schema)).ToArray();
    }

    private static IList<SnoopableObject> SnoopServices()
    {
        return ExternalServiceRegistry.GetServices().Select(service => new SnoopableObject(service)).ToArray();
    }

    private static SnoopableObject SelectObject(ObjectType objectType)
    {
        var reference = Context.ActiveUiDocument!.Selection.PickObject(objectType);

        object element;
        switch (objectType)
        {
            case ObjectType.Edge:
            case ObjectType.Face:
                element = Context.ActiveDocument.GetElement(reference).GetGeometryObjectFromReference(reference);
                break;
            case ObjectType.Element:
            case ObjectType.Subelement:
                element = Context.ActiveDocument.GetElement(reference);
                break;
            case ObjectType.PointOnElement:
                element = reference.GlobalPoint;
                break;
            case ObjectType.LinkedElement:
                var revitLinkInstance = reference.ElementId.ToElement<RevitLinkInstance>(Context.ActiveDocument);
                element = revitLinkInstance.GetLinkDocument().GetElement(reference.LinkedElementId);
                break;
            case ObjectType.Nothing:
            default:
                throw new NotSupportedException();
        }

        return new SnoopableObject(element);
    }
}