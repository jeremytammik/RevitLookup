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

using RevitLookup.Core.Objects;
using RevitLookup.ViewModels.Contracts;
using RevitLookup.ViewModels.Enums;

namespace RevitLookup.ViewModels.Utils;

public static class SearchEngine
{
    public static void SearchAsync(ISnoopViewModel model, SearchOption option)
    {
        switch (option)
        {
            case SearchOption.Objects:
            {
                var filteredObjects = Search(model.SearchText, model.SnoopableObjects);
                var filteredData = Search(model.SearchText, model.SnoopableData);
                var isObjectSelected = false;

                if (model.SelectedObject is not null)
                {
                    //Add other types to selection if no objects are found
                    if (filteredObjects.Count == 0 && filteredData.Count > 0)
                        filteredObjects = Search(model.SelectedObject, model.SnoopableObjects);

                    //Add selected object to results
                    if (filteredData.Count > 0)
                    {
                        foreach (var item in filteredObjects)
                            if (item == model.SelectedObject)
                            {
                                isObjectSelected = true;
                                break;
                            }

                        if (!isObjectSelected) filteredObjects.Add(model.SelectedObject);
                    }
                }

                //Add data of the selected object if no others are found
                if (filteredObjects.Count > 0 && filteredData.Count == 0)
                {
                    model.FilteredSnoopableObjects = filteredObjects;
                    model.FilteredSnoopableData = isObjectSelected ? model.SnoopableData : filteredData;
                }
                //Output as is
                else
                {
                    model.FilteredSnoopableObjects = filteredObjects;
                    model.FilteredSnoopableData = filteredData;
                }

                break;
            }
            case SearchOption.Selection:
            {
                //Filter data for new tree selection
                var filteredData = Search(model.SearchText, model.SnoopableData);
                model.FilteredSnoopableData = filteredData.Count == 0 ? model.SnoopableData : filteredData;
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(option), option, null);
        }
    }

    /// <summary>
    ///     Search all types for selected object
    /// </summary>
    private static List<SnoopableObject> Search(SnoopableObject query, IEnumerable<SnoopableObject> data)
    {
        var filteredSnoopableObjects = new List<SnoopableObject>();
        foreach (var item in data)
            if (item.Descriptor.Type == query.Descriptor.Type)
                filteredSnoopableObjects.Add(item);

        return filteredSnoopableObjects;
    }

    /// <summary>
    ///     Objects filtering
    /// </summary>
    private static List<SnoopableObject> Search(string query, IEnumerable<SnoopableObject> data)
    {
        var filteredSnoopableObjects = new List<SnoopableObject>();
        foreach (var item in data)
            if (item.Descriptor.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                filteredSnoopableObjects.Add(item);
            else if (item.Descriptor.Type.Contains(query, StringComparison.OrdinalIgnoreCase)) filteredSnoopableObjects.Add(item);

        return filteredSnoopableObjects;
    }

    /// <summary>
    ///     Data filtering
    /// </summary>
    private static List<Descriptor> Search(string query, IEnumerable<Descriptor> data)
    {
        var filteredSnoopableData = new List<Descriptor>();
        foreach (var item in data)
            if (item.Name.Contains(query, StringComparison.OrdinalIgnoreCase)) filteredSnoopableData.Add(item);
            else if (item.Value.Descriptor.Name.Contains(query, StringComparison.OrdinalIgnoreCase)) filteredSnoopableData.Add(item);

        return filteredSnoopableData;
    }
}