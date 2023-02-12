// Copyright 2003-2022 by Autodesk, Inc.
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
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI.Selection;
using Autodesk.Windows;
using RevitLookup.Core.Objects;
using RevitLookup.Services.Enums;

namespace RevitLookup.Core;

public static class Selector
{
    public static IReadOnlyList<SnoopableObject> Snoop(SnoopableType type)
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
            SnoopableType.Schemas => SnoopSchemas(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    private static IReadOnlyList<SnoopableObject> SnoopView()
    {
        return new SnoopableObject[] {new(RevitApi.Document, RevitApi.ActiveView)};
    }

    private static IReadOnlyList<SnoopableObject> SnoopDocument()
    {
        return new SnoopableObject[] {new(RevitApi.Document, RevitApi.Document)};
    }

    private static IReadOnlyList<SnoopableObject> SnoopApplication()
    {
        return new SnoopableObject[] {new(RevitApi.Document, RevitApi.Application)};
    }

    private static IReadOnlyList<SnoopableObject> SnoopUiApplication()
    {
        return new SnoopableObject[] {new(RevitApi.Document, RevitApi.UiApplication)};
    }

    private static IReadOnlyList<SnoopableObject> SnoopEdge()
    {
        return new[] {SelectObject(ObjectType.Edge)};
    }

    private static IReadOnlyList<SnoopableObject> SnoopFace()
    {
        return new[] {SelectObject(ObjectType.Face)};
    }

    private static IReadOnlyList<SnoopableObject> SnoopSubElement()
    {
        return new[] {SelectObject(ObjectType.Subelement)};
    }

    private static IReadOnlyList<SnoopableObject> SnoopPoint()
    {
        return new[] {SelectObject(ObjectType.PointOnElement)};
    }

    private static IReadOnlyList<SnoopableObject> SnoopLinkedElement()
    {
        return new[] {SelectObject(ObjectType.LinkedElement)};
    }

    private static IReadOnlyList<SnoopableObject> SnoopSelection()
    {
        var selectedIds = RevitApi.UiApplication.ActiveUIDocument.Selection.GetElementIds();
        if (selectedIds.Count > 0)
            return RevitApi.Document
                .GetElements(selectedIds)
                .WherePasses(new ElementIdSetFilter(selectedIds))
                .Select(element => new SnoopableObject(RevitApi.Document, element))
                .ToList();

        return RevitApi.Document
            .GetElements(RevitApi.ActiveView.Id)
            .Select(element => new SnoopableObject(RevitApi.Document, element))
            .ToList();
    }

    private static IReadOnlyList<SnoopableObject> SnoopDatabase()
    {
        var elementTypes = RevitApi.Document.GetElements().WhereElementIsElementType();
        var elementInstances = RevitApi.Document.GetElements().WhereElementIsNotElementType();
        return elementTypes
            .UnionWith(elementInstances)
            .Select(element => new SnoopableObject(RevitApi.Document, element))
            .ToList();
    }

    private static IReadOnlyList<SnoopableObject> SnoopDependentElements()
    {
        var selectedIds = RevitApi.UiDocument.Selection.GetElementIds();
        if (selectedIds.Count == 0) return Array.Empty<SnoopableObject>();

        var elements = new List<ElementId>();
        var selectedElements = RevitApi.Document.GetElements(selectedIds).WhereElementIsNotElementType();

        foreach (var selectedElement in selectedElements)
        {
            var dependentElements = selectedElement.GetDependentElements(null);
            foreach (var dependentElement in dependentElements) elements.Add(dependentElement);
        }

        return RevitApi.Document.GetElements()
            .WherePasses(new ElementIdSetFilter(elements))
            .Select(element => new SnoopableObject(RevitApi.Document, element))
            .ToList();
    }

    private static IReadOnlyList<SnoopableObject> SnoopComponentManager()
    {
        return new[] {new SnoopableObject(typeof(ComponentManager))};
    }

    private static IReadOnlyList<SnoopableObject> SnoopPerformanceAdviser()
    {
        return new[] {new SnoopableObject(RevitApi.Document, PerformanceAdviser.GetPerformanceAdviser())};
    }

    private static IReadOnlyList<SnoopableObject> SnoopUpdaterRegistry()
    {
        return UpdaterRegistry.GetRegisteredUpdaterInfos().Select(schema => new SnoopableObject(RevitApi.Document, schema)).ToArray();
    }

    private static IReadOnlyList<SnoopableObject> SnoopSchemas()
    {
        return Schema.ListSchemas().Select(schema => new SnoopableObject(RevitApi.Document, schema)).ToArray();
    }

    private static SnoopableObject SelectObject(ObjectType objectType)
    {
        var reference = RevitApi.UiDocument.Selection.PickObject(objectType);

        object element;
        Document document;
        switch (objectType)
        {
            case ObjectType.Edge:
            case ObjectType.Face:
                document = RevitApi.Document;
                element = document.GetElement(reference).GetGeometryObjectFromReference(reference);
                break;
            case ObjectType.Element:
            case ObjectType.Subelement:
                document = RevitApi.Document;
                element = document.GetElement(reference);
                break;
            case ObjectType.PointOnElement:
                document = RevitApi.Document;
                element = reference.GlobalPoint;
                break;
            case ObjectType.LinkedElement:
                var revitLinkInstance = reference.ElementId.ToElement<RevitLinkInstance>(RevitApi.Document);
                document = revitLinkInstance.GetLinkDocument();
                element = document.GetElement(reference.LinkedElementId);
                break;
            case ObjectType.Nothing:
            default:
                throw new NotSupportedException();
        }

        return new SnoopableObject(document, element);
    }
}