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
using RevitLookup.ViewModels.Objects;

namespace RevitLookup.ViewModels.Dialogs;

public partial class UnitsViewModel : ObservableObject
{
    private readonly List<UnitInfo> _units;
    [ObservableProperty] private List<UnitInfo> _filteredUnits;
    [ObservableProperty] private string _searchText;

    public UnitsViewModel(Type unitType)
    {
        _units = GetUnits(unitType);
        _filteredUnits = _units;
    }

    private List<UnitInfo> GetUnits(Type unitType)
    {
        // return new List<UnitInfo>();
        if (unitType == typeof(BuiltInParameter))
        {
            var parameters = Enum.GetValues(unitType).Cast<BuiltInParameter>();
            var list = new List<UnitInfo>();
            foreach (var builtInParameter in parameters)
                try
                {
                    list.Add(new UnitInfo(builtInParameter.ToString(), builtInParameter.ToLabel()));
                }
                catch
                {
                    // ignored
                    // Some parameters don't have a label
                }

            return list;
        }

        if (unitType == typeof(BuiltInCategory))
        {
            var categories = Enum.GetValues(unitType).Cast<BuiltInCategory>();
            var list = new List<UnitInfo>();
            foreach (var category in categories)
                try
                {
                    list.Add(new UnitInfo(category.ToString(), category.ToLabel()));
                }
                catch
                {
                    // ignored
                    // Some categories don't have a label
                }

            return list;
        }

        if (unitType == typeof(ForgeTypeId))
            return UnitUtils.GetAllUnits()
                .Concat(UnitUtils.GetAllDisciplines())
                .Concat(UnitUtils.GetAllMeasurableSpecs())
                .Concat(SpecUtils.GetAllSpecs())
                .Concat(ParameterUtils.GetAllBuiltInGroups())
                .Concat(ParameterUtils.GetAllBuiltInParameters())
                .Select(typeId => new UnitInfo(typeId.TypeId, typeId.ToLabel()))
                .ToList();

        throw new NotSupportedException();
    }

    async partial void OnSearchTextChanged(string value)
    {
        if (string.IsNullOrEmpty(SearchText))
        {
            FilteredUnits = _units;
            return;
        }

        FilteredUnits = await Task.Run(() =>
        {
            var formattedText = value.ToLower().Trim();
            var searchResults = new List<UnitInfo>();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var family in _units)
                if (family.Label.ToLower().Contains(formattedText) || family.Unit.ToLower().Contains(formattedText))
                    searchResults.Add(family);

            return searchResults;
        });
    }
}