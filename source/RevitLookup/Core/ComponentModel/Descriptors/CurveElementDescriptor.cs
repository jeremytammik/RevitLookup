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

public class CurveElementDescriptor(CurveElement curveElement) : Descriptor, IDescriptorResolver
{
    public ResolveSet Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(CurveElement.GetAdjoinedCurveElements) => ResolveSet
                .Append(curveElement.GetAdjoinedCurveElements(0), "Point 0")
                .AppendVariant(curveElement.GetAdjoinedCurveElements(1), "Point 1"),
            nameof(CurveElement.HasTangentLocks) => ResolveSet
                .Append(curveElement.HasTangentLocks(0), "Point 0")
                .AppendVariant(curveElement.HasTangentLocks(1), "Point 1"),
            nameof(CurveElement.GetTangentLock) => ResolveTangentLock(),
            nameof(CurveElement.HasTangentJoin) => ResolveTangentJoin(),
            nameof(CurveElement.IsAdjoinedCurveElement) => ResolveIsAdjoinedCurveElement(),
            _ => null
        };
        
        ResolveSet ResolveTangentLock()
        {
            var set0 = curveElement.GetAdjoinedCurveElements(0);
            var set1 = curveElement.GetAdjoinedCurveElements(1);
            var resolveSummary = new ResolveSet(set0.Count + set1.Count);
            
            foreach (var id in set0)
            {
                if (!curveElement.HasTangentJoin(0, id)) continue;
                
                var result = curveElement.GetTangentLock(0, id);
                var element = id.ToElement(context);
                resolveSummary.AppendVariant(result, $"Point 0, {element!.Name}: {result}");
            }
            
            foreach (var id in set1)
            {
                if (!curveElement.HasTangentJoin(1, id)) continue;
                
                var result = curveElement.GetTangentLock(1, id);
                var element = id.ToElement(context);
                resolveSummary.AppendVariant(result, $"Point 1, {element!.Name}: {result}");
            }
            return resolveSummary;
        }
        
        ResolveSet ResolveTangentJoin()
        {
            var set0 = curveElement.GetAdjoinedCurveElements(0);
            var set1 = curveElement.GetAdjoinedCurveElements(1);
            var resolveSummary = new ResolveSet(set0.Count + set1.Count);
            
            foreach (var id in set0)
            {
                var result = curveElement.HasTangentJoin(0, id);
                resolveSummary.AppendVariant(result, $"Point 0, {id}: {result}");
            }
            
            foreach (var id in set1)
            {
                var result = curveElement.HasTangentJoin(1, id);
                resolveSummary.AppendVariant(result, $"Point 1, {id}: {result}");
            }
            return resolveSummary;
        }
        
        ResolveSet ResolveIsAdjoinedCurveElement()
        {
            var set0 = curveElement.GetAdjoinedCurveElements(0);
            var set1 = curveElement.GetAdjoinedCurveElements(1);
            var resolveSummary = new ResolveSet(set0.Count + set1.Count);
            
            foreach (var id in set0)
            {
                var result = curveElement.IsAdjoinedCurveElement(0, id);
                resolveSummary.AppendVariant(result, $"Point 0, {id}: {result}");
            }
            
            foreach (var id in set1)
            {
                var result = curveElement.IsAdjoinedCurveElement(1, id);
                resolveSummary.AppendVariant(result, $"Point 1, {id}: {result}");
            }
            return resolveSummary;
        }
    }
}