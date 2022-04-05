// Copyright 2003-2022 by Autodesk, Inc. 
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
    private readonly object _element;

    public DataFactory(Document document, object element)
    {
        _document = document;
        _element = element;
    }

    public Data Create(MethodInfo methodInfo)
    {
        var info = methodInfo.ContainsGenericParameters
            ? _element.GetType().GetMethod(methodInfo.Name, methodInfo.GetParameters().Select(x => x.ParameterType).ToArray())
            : methodInfo;

        if (info is null) return null;

        var declaringType = info.DeclaringType;
        if (info.IsSpecialName || declaringType is null) return null;

        if (declaringType == typeof(Element) && info.Name == nameof(Element.GetDependentElements))
        {
            var element = (Element) _element;
            return DataTypeInfoHelper.CreateFrom(_document, info, element.GetDependentElements(null), element);
        }

        if (declaringType == typeof(Element) && info.Name == nameof(Element.GetPhaseStatus))
            return new ElementPhaseStatusesData(info.Name, (Element) _element);

        if (declaringType == typeof(Reference) && info.Name == nameof(Reference.ConvertToStableRepresentation))
        {
            var reference = (Reference) _element;
            return DataTypeInfoHelper.CreateFrom(_document, info, reference.ConvertToStableRepresentation(_document), reference);
        }

        if (declaringType == typeof(View) && info.Name == nameof(View.GetFilterOverrides))
            return new ViewFiltersOverrideGraphicSettingsData(info.Name, (View) _element);

        if (declaringType == typeof(View) && info.Name == nameof(View.GetFilterVisibility))
            return new ViewFiltersVisibilitySettingsData(info.Name, (View) _element);

        if (declaringType == typeof(View) && info.Name == nameof(View.GetNonControlledTemplateParameterIds))
            return new ViewGetNonControlledTemplateParameterIdsData(info.Name, (View) _element);

        if (declaringType == typeof(View) && info.Name == nameof(View.GetTemplateParameterIds))
            return new ViewGetTemplateParameterIdsData(info.Name, (View) _element);

        if (declaringType == typeof(ScheduleDefinition) && info.Name == nameof(ScheduleDefinition.GetField))
        {
            var parameters = info.GetParameters();
            if (parameters[0].ParameterType == typeof(int))
                return new ScheduleDefinitionGetFieldsData(info.Name, (ScheduleDefinition) _element);
        }

        if (declaringType == typeof(ViewCropRegionShapeManager) && info.Name == nameof(ViewCropRegionShapeManager.GetSplitRegionOffset))
            return new ViewCropRegionShapeManagerGetSplitRegionOffsetsData(info.Name, (ViewCropRegionShapeManager) _element);

        if (declaringType == typeof(Curve) && info.Name == nameof(Curve.GetEndPoint))
            return new CurveGetEndPointData(info.Name, (Curve) _element);

        if (declaringType == typeof(TableData) && info.Name == nameof(TableData.GetSectionData))
        {
            var parameters = info.GetParameters();
            if (parameters[0].ParameterType == typeof(SectionType))
                return new TableDataSectionData(info.Name, (TableData) _element);
        }

        if (declaringType == typeof(PlanViewRange) && info.Name == nameof(PlanViewRange.GetLevelId))
            return new PlanViewRangeGetLevelIdData(info.Name, (PlanViewRange) _element, _document);

        if (declaringType == typeof(PlanViewRange) && info.Name == nameof(PlanViewRange.GetOffset))
            return new PlanViewRangeGetOffsetData(info.Name, (PlanViewRange) _element);

        if (declaringType == typeof(Document) && info.Name == nameof(Document.Close))
            return null;

        if (info.GetParameters().Length > 0 || info.ReturnType == typeof(void))
            return null;

        if (declaringType == typeof(PrintManager) && info.Name == nameof(PrintManager.SubmitPrint))
            return new BoolData(nameof(PrintManager.SubmitPrint), false);

        var returnValue = info.Invoke(_element, Array.Empty<object>());
        return DataTypeInfoHelper.CreateFrom(_document, info, returnValue, _element);
    }
}