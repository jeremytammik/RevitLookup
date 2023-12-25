// Copyright 2003-2023 by Autodesk, Inc.
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
using CommunityToolkit.Mvvm.ComponentModel;
using RevitLookup.Core;

namespace RevitLookup.ViewModels.Dialogs;

public sealed partial class SearchElementsViewModel : ObservableObject
{
    [ObservableProperty] private string _searchText = string.Empty;

    [Pure]
    public List<Element> SearchElements()
    {
        var rows = SearchText.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
        var items = ParseRawRequest(rows);
        var results = new List<Element>(items.Count);

        foreach (var rawId in items)
        {
#if R24_OR_GREATER
            if (long.TryParse(rawId, out var id))
            {
                var element = RevitApi.Document.GetElement(new ElementId(id));
#else
            if (int.TryParse(rawId, out var id))
            {
                var element = RevitApi.Document.GetElement(new ElementId(id));
#endif
                if (element is not null) results.Add(element);
            }
            else if (rawId.Length == 45 && rawId.Count(c => c == '-') == 5)
            {
                var element = RevitApi.Document.GetElement(rawId);
                if (element is not null) results.Add(element);
            }
            else if (rawId.Length == 22 && rawId.Count(c => c == ' ') == 0)
            {
                var elements = SearchByIfcGuid(rawId);
                results.AddRange(elements);
            }
            else
            {
                var elements = SearchByName(rawId);
                results.AddRange(elements);
            }
        }

        return results;
    }

    private static IEnumerable<Element> SearchByName(string rawId)
    {
        var elementTypes = RevitApi.Document.GetElements().WhereElementIsElementType();
        var elementInstances = RevitApi.Document.GetElements().WhereElementIsNotElementType();
        return elementTypes
            .UnionWith(elementInstances)
            .Where(element => element.Name.Contains(rawId, StringComparison.OrdinalIgnoreCase));
    }

    private static IList<Element> SearchByIfcGuid(string rawId)
    {
        var guidProvider = new ParameterValueProvider(new ElementId(BuiltInParameter.IFC_GUID));
        var typeGuidProvider = new ParameterValueProvider(new ElementId(BuiltInParameter.IFC_TYPE_GUID));
#if R22_OR_GREATER
        var filterRule = new FilterStringRule(guidProvider, new FilterStringEquals(), rawId);
        var typeFilterRule = new FilterStringRule(typeGuidProvider, new FilterStringEquals(), rawId);
#else
        var filterRule = new FilterStringRule(guidProvider, new FilterStringEquals(), rawId, true);
        var typeFilterRule = new FilterStringRule(typeGuidProvider, new FilterStringEquals(), rawId, true);
#endif
        var elementFilter = new ElementParameterFilter(filterRule);
        var typeElementFilter = new ElementParameterFilter(typeFilterRule);

        var typeGuidsCollector = RevitApi.Document
            .GetElements()
            .WherePasses(typeElementFilter);

        return RevitApi.Document
            .GetElements()
            .WherePasses(elementFilter)
            .UnionWith(typeGuidsCollector)
            .ToElements();
    }

    private static List<string> ParseRawRequest(string[] rows)
    {
        var items = new List<string>(rows.Length);
        var delimiters = new[] {'\t', ';', ',', ' '};
        foreach (var row in rows)
        {
            for (var i = 0; i < delimiters.Length; i++)
            {
                var delimiter = delimiters[i];
                var split = row.Split([delimiter], StringSplitOptions.RemoveEmptyEntries);
                if (split.Length > 1 || i == delimiters.Length - 1 || split.Length == 1 && split[0] != row)
                {
                    items.AddRange(split);
                    break;
                }
            }
        }

        return items;
    }
}