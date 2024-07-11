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

using System.Reflection;
using Autodesk.Revit.DB.Lighting;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;
#if REVIT2023_OR_GREATER
using Autodesk.Revit.DB.Structure;
#endif

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class DocumentDescriptor : Descriptor, IDescriptorResolver, IDescriptorExtension
{
    private readonly Document _document;
    
    public DocumentDescriptor(Document document)
    {
        _document = document;
        Name = document.Title;
    }
    
    public Func<IVariants> Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(Document.Close) when parameters.Length == 0 => Variants.Disabled,
            nameof(Document.PlanTopologies) when parameters.Length == 0 => ResolvePlanTopologies,
#if REVIT2024_OR_GREATER
            nameof(Document.GetUnusedElements) => ResolveGetUnusedElements,
            nameof(Document.GetAllUnusedElements) => ResolveGetAllUnusedElements,
#endif
            _ => null
        };
        
        IVariants ResolvePlanTopologies()
        {
            if (_document.IsReadOnly) return Variants.Empty<PlanTopologySet>();
            
            var transaction = new Transaction(_document);
            transaction.Start("Calculating plan topologies");
            var topologies = _document.PlanTopologies;
            transaction.Commit();
            
            return Variants.Single(topologies);
        }
#if REVIT2024_OR_GREATER
        
        IVariants ResolveGetUnusedElements()
        {
            return Variants.Single(context.GetUnusedElements(new HashSet<ElementId>()));
        }
        
        IVariants ResolveGetAllUnusedElements()
        {
            return Variants.Single(context.GetAllUnusedElements(new HashSet<ElementId>()));
        }
#endif
    }
    
    public void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register(nameof(GlobalParametersManager.GetAllGlobalParameters), GlobalParametersManager.GetAllGlobalParameters);
        manager.Register(nameof(LightGroupManager.GetLightGroupManager), LightGroupManager.GetLightGroupManager);
        manager.Register(nameof(LightFamily.GetLightFamily), LightFamily.GetLightFamily);
#if REVIT2022_OR_GREATER
        manager.Register(nameof(TemporaryGraphicsManager.GetTemporaryGraphicsManager), TemporaryGraphicsManager.GetTemporaryGraphicsManager);
#endif
#if REVIT2023_OR_GREATER
        manager.Register(nameof(AnalyticalToPhysicalAssociationManager.GetAnalyticalToPhysicalAssociationManager),
            AnalyticalToPhysicalAssociationManager.GetAnalyticalToPhysicalAssociationManager);
#endif
        if (_document.IsFamilyDocument)
        {
            manager.Register(nameof(FamilySizeTableManager.GetFamilySizeTableManager), context =>
            {
                var familyTableId = new ElementId(BuiltInParameter.RBS_LOOKUP_TABLE_NAME);
                return FamilySizeTableManager.GetFamilySizeTableManager(context, familyTableId);
            });
        }
        
        // Disabled: slow performance.
        // manager.Register(nameof(WorksharingUtils.GetUserWorksetInfo), context =>
        // {
        //     var modelPath = context.IsModelInCloud ? context.GetCloudModelPath() : context.GetWorksharingCentralModelPath();
        //     return WorksharingUtils.GetUserWorksetInfo(modelPath);
        // });
    }
}