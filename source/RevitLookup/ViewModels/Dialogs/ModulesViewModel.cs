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

using CommunityToolkit.Mvvm.ComponentModel;
using RevitLookup.ViewModels.Objects;

namespace RevitLookup.ViewModels.Dialogs;

public partial class ModulesViewModel : ObservableObject
{
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private List<ModuleInfo> _filteredModules;
    [ObservableProperty] private List<ModuleInfo> _modules;

    public ModulesViewModel()
    {
        Initialize();
    }

    private void Initialize()
    {
        var domain = AppDomain.CurrentDomain;
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Modules = new List<ModuleInfo>(assemblies.Length);

        for (var i = 0; i < assemblies.Length; i++)
        {
            var assembly = assemblies[i];
            var assemblyName = assembly.GetName();
            var module = new ModuleInfo
            {
                Name = assemblyName.Name,
                Path = assembly.IsDynamic ? string.Empty : assembly.Location,
                Order = i + 1,
                Version = assemblyName.Version.ToString(),
                Domain = domain.FriendlyName
            };

            Modules.Add(module);
        }
    }

    async partial void OnSearchTextChanged(string value)
    {
        if (string.IsNullOrEmpty(SearchText))
        {
            FilteredModules = Modules;
            return;
        }

        FilteredModules = await Task.Run(() =>
        {
            var formattedText = value.ToLower().Trim();
            var searchResults = new List<ModuleInfo>();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var module in Modules)
            {
                if (module.Name.ToLower().Contains(formattedText) ||
                    module.Path.ToLower().Contains(formattedText) ||
                    module.Version.ToLower().Contains(formattedText))
                {
                    searchResults.Add(module);
                }
            }

            return searchResults;
        });
    }

    partial void OnModulesChanged(List<ModuleInfo> value)
    {
        FilteredModules = value;
    }
}