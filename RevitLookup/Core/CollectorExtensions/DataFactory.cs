// Copyright 2003-2021 by Autodesk, Inc. 
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
using RevitLookup.Core.RevitTypes;
using View = Autodesk.Revit.DB.View;

namespace RevitLookup.Core.CollectorExtensions;

public class DataFactory
{
    private readonly Document _document;
    private readonly object _elem;

    public DataFactory(Document document, object elem)
    {
        _document = document;
        _elem = elem;
    }

    public Data Create(MethodInfo mi)
    {
        var methodInfo = mi.ContainsGenericParameters
            ? _elem.GetType().GetMethod(mi.Name, mi.GetParameters().Select(x => x.ParameterType).ToArray())
            : mi;

        if (methodInfo is null) return null;

        var declaringType = methodInfo.DeclaringType;
        if (methodInfo.IsSpecialName || declaringType is null) return null;

        if (declaringType == typeof(Element) && methodInfo.Name == nameof(Element.GetDependentElements))
        {
            var element = (Element) _elem;
            return DataTypeInfoHelper.CreateFrom(_document, methodInfo, element.GetDependentElements(null), element);
        }

        if (declaringType == typeof(Element) && methodInfo.Name == nameof(Element.GetPhaseStatus))
            return new ElementPhaseStatuses(methodInfo.Name, (Element) _elem);

        if (declaringType == typeof(Reference) && methodInfo.Name == nameof(Reference.ConvertToStableRepresentation))
        {
            var reference = (Reference) _elem;
            return DataTypeInfoHelper.CreateFrom(_document, methodInfo, reference.ConvertToStableRepresentation(_document), reference);
        }

        if (declaringType == typeof(View) && methodInfo.Name == nameof(View.GetFilterOverrides))
            return new ViewFiltersOverrideGraphicSettings(methodInfo.Name, (View) _elem);

        if (declaringType == typeof(View) && methodInfo.Name == nameof(View.GetFilterVisibility))
            return new ViewFiltersVisibilitySettings(methodInfo.Name, (View) _elem);

        if (declaringType == typeof(View) && methodInfo.Name == nameof(View.GetNonControlledTemplateParameterIds))
            return new ViewGetNonControlledTemplateParameterIds(methodInfo.Name, (View) _elem);

        if (declaringType == typeof(View) && methodInfo.Name == nameof(View.GetTemplateParameterIds))
            return new ViewGetTemplateParameterIds(methodInfo.Name, (View) _elem);

        if (declaringType == typeof(ScheduleDefinition) && methodInfo.Name == nameof(ScheduleDefinition.GetField))
        {
            var parameters = methodInfo.GetParameters();
            if (parameters[0].ParameterType == typeof(int))
                return new ScheduleDefinitionGetFields(methodInfo.Name, (ScheduleDefinition) _elem);
        }

        if (declaringType == typeof(ViewCropRegionShapeManager) && methodInfo.Name == nameof(ViewCropRegionShapeManager.GetSplitRegionOffset))
            return new ViewCropRegionShapeManagerGetSplitRegionOffsets(methodInfo.Name, (ViewCropRegionShapeManager) _elem);

        if (declaringType == typeof(Curve) && methodInfo.Name == nameof(Curve.GetEndPoint))
            return new CurveGetEndPoint(methodInfo.Name, (Curve) _elem);

        if (declaringType == typeof(TableData) && methodInfo.Name == nameof(TableData.GetSectionData))
        {
            var parameters = methodInfo.GetParameters();
            if (parameters[0].ParameterType == typeof(SectionType))
                return new TableDataSectionData(methodInfo.Name, (TableData) _elem);
        }

        if (declaringType == typeof(PlanViewRange) && methodInfo.Name == nameof(PlanViewRange.GetLevelId))
            return new PlanViewRangeGetLevelId(methodInfo.Name, (PlanViewRange) _elem, _document);

        if (declaringType == typeof(PlanViewRange) && methodInfo.Name == nameof(PlanViewRange.GetOffset))
            return new PlanViewRangeGetOffset(methodInfo.Name, (PlanViewRange) _elem);

        if (declaringType == typeof(Document) && methodInfo.Name == nameof(Document.Close))
            return null;

        if (methodInfo.GetParameters().Length > 0 || methodInfo.ReturnType == typeof(void))
            return null;

        var returnValue = methodInfo.Invoke(_elem, Array.Empty<object>());
        return DataTypeInfoHelper.CreateFrom(_document, methodInfo, returnValue, _elem);
    }
}