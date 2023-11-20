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
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public class MepSystemDescriptor(MEPSystem mepSystem) : ElementDescriptor(mepSystem), IDescriptorResolver
{
    public new ResolveSet Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(MEPSystem.GetSectionByIndex) => ResolveMepSections(),
            nameof(MEPSystem.GetSectionByNumber) => ResolveMepSections(),
            _ => null
        };
        
        ResolveSet ResolveMepSections()
        {
            var capacity = mepSystem.SectionsCount;
            var resolveSummary = new ResolveSet(capacity);
            for (var i = 0; i < capacity; i++)
            {
                var section = mepSystem.GetSectionByIndex(i);
                resolveSummary.AppendVariant(section, $"Index: {i}, number: {section.Number}");
            }

            return resolveSummary;
        }
    }
}