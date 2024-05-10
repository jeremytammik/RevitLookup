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
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class DocumentDescriptor : Descriptor, IDescriptorResolver
{
    private readonly Document _document;

    public DocumentDescriptor(Document document)
    {
        _document = document;
        Name = document.Title;
    }

    public IVariants Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(Document.Close) when parameters.Length == 0 => Variants.Single(false, "Method execution disabled"),
            nameof(Document.PlanTopologies) when parameters.Length == 0 => ResolvePlanTopologies(),
#if REVIT2024_OR_GREATER
            nameof(Document.GetUnusedElements) => Variants.Single(context.GetUnusedElements(new HashSet<ElementId>())),
            nameof(Document.GetAllUnusedElements) => Variants.Single(context.GetAllUnusedElements(new HashSet<ElementId>())),
#endif
            _ => null
        };

        IVariants ResolvePlanTopologies()
        {
            var transaction = new Transaction(_document);
            transaction.Start("Calculating plan topologies");
            var topologies = _document.PlanTopologies;
            transaction.Commit();

            return Variants.Single(topologies);
        }
    }
}