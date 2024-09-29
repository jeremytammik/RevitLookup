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
using RevitLookup.Views.Dialogs;

namespace RevitLookup.ViewModels.Dialogs.ExtensibleStorage;

public partial class SelectEntityViewModel : ObservableObject
{
    private readonly Element _element;
    [ObservableProperty] private EntityDto _selectedEntity;
    public List<EntityDto> ExistedEntities { get; } = [];
    public SelectEntityViewModel(Element element)
    {
        _element = element;
        var schemas = Schema.ListSchemas();
        foreach (var schema in schemas)
        {
            if (!schema.ReadAccessGranted()) return;
            var entity = element.GetEntity(schema);
            if (entity is not null && entity.Schema is not null)
            {
                ExistedEntities.Add(new EntityDto
                {
                    EntityName = schema.SchemaName,
                    Guid = entity.SchemaGUID
                });
            }
        }
    }
    
    public async Task ShowEditEntityDialogAsync(IServiceProvider serviceProvider)
    {
        var schema = Schema.Lookup(SelectedEntity.Guid);
        var entity = _element.GetEntity(schema);
        var dialog = new EditEntityValuesDialog(serviceProvider, _element, entity);
        await dialog.ShowAsync();
    }
}