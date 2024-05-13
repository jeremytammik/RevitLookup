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

using System.Collections;
using System.Reflection;
using Autodesk.Revit.DB.Electrical;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public class EnumerableDescriptor : Descriptor, IDescriptorEnumerator, IDescriptorResolver
{
    public EnumerableDescriptor(IEnumerable value)
    {
        // SnoopUtils.ParseEnumerable dispose this Enumerator;
        // ReSharper disable once GenericEnumeratorNotDisposed
        Enumerator = value.GetEnumerator();
        
        //Checking types to reduce memory allocation when creating an iterator and increase performance
        IsEmpty = value switch
        {
            string => true,
            IVariants enumerable => enumerable.Count == 0,
            ICollection enumerable => enumerable.Count == 0,
            ParameterSet enumerable => enumerable.IsEmpty,
            ParameterMap enumerable => enumerable.IsEmpty,
            DefinitionBindingMap enumerable => enumerable.IsEmpty,
            CategoryNameMap enumerable => enumerable.IsEmpty,
            DefinitionGroups enumerable => enumerable.IsEmpty,
            HashSet<ElementId> enumerable => enumerable.Count == 0,
            HashSet<ElectricalSystem> enumerable => enumerable.Count == 0,
            DocumentSet enumerable => enumerable.IsEmpty,
            PhaseArray enumerable => enumerable.IsEmpty,
            ProjectLocationSet enumerable => enumerable.IsEmpty,
            PlanTopologySet enumerable => enumerable.IsEmpty,
            CitySet enumerable => enumerable.IsEmpty,
            WireTypeSet enumerable => enumerable.IsEmpty,
            PanelTypeSet enumerable => enumerable.IsEmpty,
            FamilyTypeSet enumerable => enumerable.IsEmpty,
            MullionTypeSet enumerable => enumerable.IsEmpty,
            VoltageTypeSet enumerable => enumerable.IsEmpty,
            InsulationTypeSet enumerable => enumerable.IsEmpty,
            TemperatureRatingTypeSet enumerable => enumerable.IsEmpty,
            _ => !Enumerator.MoveNext()
        };
    }
    
    public IEnumerator Enumerator { get; }
    public bool IsEmpty { get; }
    
    public Func<IVariants> Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(IEnumerable.GetEnumerator) => ResolveGetEnumerator,
            _ => null
        };
        
        IVariants ResolveGetEnumerator()
        {
            return Variants.Empty<IEnumerator>();
        }
    }
}