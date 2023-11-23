// Copyright 2003-$File.CreatedYear by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;
using ArgumentException = Autodesk.Revit.Exceptions.ArgumentException;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public class MepSectionDescriptor : Descriptor, IDescriptorResolver
{
    private MEPSection _mepSection;
    public MepSectionDescriptor(MEPSection mepSection)
    {
        _mepSection = mepSection;
    }
    public ResolveSet Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(MEPSection.GetElementIds) => ResolveSectionIds(),
            nameof(MEPSection.GetCoefficient) => ResolveCoefficient(),
            nameof(MEPSection.GetPressureDrop) => ResolvePressureDrop(),
            nameof(MEPSection.GetSegmentLength) => ResolveSegmentLength(),
            _ => null
        };
        
        ResolveSet ResolveSectionIds()
        {
            var elementIds = _mepSection.GetElementIds();
            var resolveSummary = new ResolveSet(elementIds.Count);
            foreach (var id in elementIds)
            {
                resolveSummary.AppendVariant(id);
            }
            return resolveSummary;
        }
        ResolveSet ResolveCoefficient()
        {
            var elementIds = _mepSection.GetElementIds();
            var resolveSummary = new ResolveSet(elementIds.Count);
            foreach (var id in elementIds)
            {
                resolveSummary.AppendVariant(_mepSection.GetCoefficient(id), $"{id}");
            }
            return resolveSummary;
        }
        
        ResolveSet ResolvePressureDrop()
        {
            var elementIds = _mepSection.GetElementIds();
            var resolveSummary = new ResolveSet(elementIds.Count);
            foreach (var id in elementIds)
            {
                resolveSummary.AppendVariant(_mepSection.GetPressureDrop(id), $"{id}");
            }
            return resolveSummary;
        }
        
        ResolveSet ResolveSegmentLength()
        {
            var elementIds = _mepSection.GetElementIds();
            var resolveSummary = new ResolveSet(elementIds.Count);
            foreach (var id in elementIds)
            {
                var length = 0.0d;
                try
                {
                    length = _mepSection.GetSegmentLength(id);
                }
                catch (ArgumentException)
                {
                    // ignored
                }
                if (length != 0.0d) resolveSummary.AppendVariant(length, $"{id}");
            }
            return resolveSummary;
        }
    }

    
}