﻿// Copyright 2003-2024 by Autodesk, Inc.
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
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class DocumentDescriptor : Descriptor, IDescriptorResolver, IDescriptorExtension
{
    private readonly Document _document;
    
    public DocumentDescriptor(Document document)
    {
        _document = document;
        Name = document.Title;
    }
    
    public ResolveSet Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(Document.Close) when parameters.Length == 0 => ResolveSet.Append(false, "Method execution disabled"),
            nameof(Document.PlanTopologies) when parameters.Length == 0 => ResolvePlanTopologies(),
#if REVIT2024_OR_GREATER
            nameof(Document.GetUnusedElements) => ResolveSet.Append(context.GetUnusedElements(new HashSet<ElementId>())),
            nameof(Document.GetAllUnusedElements) => ResolveSet.Append(context.GetAllUnusedElements(new HashSet<ElementId>())),
#endif
            _ => null
        };
        
        ResolveSet ResolvePlanTopologies()
        {
            if (_document.IsReadOnly) return ResolveSet.Append(null);
            
            var transaction = new Transaction(_document);
            transaction.Start("Calculating plan topologies");
            var topologies = _document.PlanTopologies;
            transaction.Commit();
            return ResolveSet.Append(topologies);
        }
    }
    
    public void RegisterExtensions(IExtensionManager manager)
    {
        if (!_document.IsFamilyDocument)
            manager.Register(nameof(FamilySizeTableManager.GetFamilySizeTableManager), _ =>
            {
                var families = _document.EnumerateInstances<Family>().ToArray();
                var resolveSummary = new ResolveSet(families.Length);
                foreach (var family in families)
                {
                    var result = FamilySizeTableManager.GetFamilySizeTableManager(_document, family.Id);
                    if (result is not null && result.NumberOfSizeTables > 0)
                        resolveSummary.AppendVariant(result, $"{ElementDescriptor.CreateName(family)}");
                }
                
                return resolveSummary;
            });
    }
}