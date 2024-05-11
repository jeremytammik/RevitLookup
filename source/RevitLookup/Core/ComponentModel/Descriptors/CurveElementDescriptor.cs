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

public sealed class CurveElementDescriptor : Descriptor, IDescriptorResolver
{
    private readonly CurveElement _curveElement;
    
    public CurveElementDescriptor(CurveElement curveElement)
    {
        _curveElement = curveElement;
        Name = ElementDescriptor.CreateName(curveElement);
    }
    
    public Func<IVariants> Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(CurveElement.GetAdjoinedCurveElements) => ResolveAdjoinedCurveElements,
            nameof(CurveElement.HasTangentLocks) => ResolveHasTangentLocks,
            nameof(CurveElement.GetTangentLock) => ResolveTangentLock,
            nameof(CurveElement.HasTangentJoin) => ResolveTangentJoin,
            nameof(CurveElement.IsAdjoinedCurveElement) => ResolveIsAdjoinedCurveElement,
            _ => null
        };
        
        IVariants ResolveAdjoinedCurveElements()
        {
            var startCurveElements = _curveElement.GetAdjoinedCurveElements(0);
            var endCurveElements = _curveElement.GetAdjoinedCurveElements(1);
            
            return new Variants<ISet<ElementId>>(2)
                .Add(startCurveElements, "Point 0")
                .Add(endCurveElements, "Point 1");
        }
        
        IVariants ResolveHasTangentLocks()
        {
            var startHasTangentLocks = _curveElement.HasTangentLocks(0);
            var endHasTangentLocks = _curveElement.HasTangentLocks(1);
            
            return new Variants<bool>(2)
                .Add(startHasTangentLocks, $"Point 0: {startHasTangentLocks}")
                .Add(endHasTangentLocks, $"Point 1: {endHasTangentLocks}");
        }
        
        IVariants ResolveTangentLock()
        {
            var startCurveElements = _curveElement.GetAdjoinedCurveElements(0);
            var endCurveElements = _curveElement.GetAdjoinedCurveElements(1);
            var variants = new Variants<bool>(startCurveElements.Count + endCurveElements.Count);
            
            foreach (var id in startCurveElements)
            {
                if (!_curveElement.HasTangentJoin(0, id)) continue;
                
                var result = _curveElement.GetTangentLock(0, id);
                variants.Add(result, $"Point 0, {id}: {result}");
            }
            
            foreach (var id in endCurveElements)
            {
                if (!_curveElement.HasTangentJoin(1, id)) continue;
                
                var result = _curveElement.GetTangentLock(1, id);
                variants.Add(result, $"Point 1, {id}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveTangentJoin()
        {
            var startCurveElements = _curveElement.GetAdjoinedCurveElements(0);
            var endCurveElements = _curveElement.GetAdjoinedCurveElements(1);
            var variants = new Variants<bool>(startCurveElements.Count + endCurveElements.Count);
            
            foreach (var id in startCurveElements)
            {
                var result = _curveElement.HasTangentJoin(0, id);
                variants.Add(result, $"Point 0, {id}: {result}");
            }
            
            foreach (var id in endCurveElements)
            {
                var result = _curveElement.HasTangentJoin(1, id);
                variants.Add(result, $"Point 1, {id}: {result}");
            }
            
            return variants;
        }
        
        IVariants ResolveIsAdjoinedCurveElement()
        {
            var startCurveElements = _curveElement.GetAdjoinedCurveElements(0);
            var endCurveElements = _curveElement.GetAdjoinedCurveElements(1);
            var variants = new Variants<bool>(startCurveElements.Count + endCurveElements.Count);
            
            foreach (var id in startCurveElements)
            {
                var result = _curveElement.IsAdjoinedCurveElement(0, id);
                variants.Add(result, $"Point 0, {id}: {result}");
            }
            
            foreach (var id in endCurveElements)
            {
                var result = _curveElement.IsAdjoinedCurveElement(1, id);
                variants.Add(result, $"Point 1, {id}: {result}");
            }
            
            return variants;
        }
    }
}