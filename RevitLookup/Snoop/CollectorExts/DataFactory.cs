using System.Linq;
using System.Reflection;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using RevitLookup.Snoop.Data;

namespace RevitLookup.Snoop.CollectorExts
{
    public class DataFactory
    {
        private readonly Document _document;
        private readonly object _elem;

        public DataFactory(Document document, object elem)
        {
            this._document = document;
            this._elem = elem;
        }

        public Data.Data Create(MethodInfo mi)
        {
            var methodInfo = mi.ContainsGenericParameters ? _elem.GetType().GetMethod(mi.Name, mi.GetParameters().Select(x => x.ParameterType).ToArray()) : mi;

            if (methodInfo == null)
                return null;
            
            var declaringType = methodInfo.DeclaringType;

            if (methodInfo.IsSpecialName || declaringType == null)
                return null;

            if (declaringType == typeof(Element) && methodInfo.Name == nameof(Element.GetDependentElements))
            {
                var element = (Element) _elem;

                return DataTypeInfoHelper.CreateFrom(_document, methodInfo, element.GetDependentElements(null), element);
            }

            if (declaringType == typeof (Element) && methodInfo.Name == nameof(Element.GetPhaseStatus))
                return new ElementPhaseStatuses(methodInfo.Name, (Element) _elem);

            if (declaringType == typeof(Reference) && methodInfo.Name == nameof(Reference.ConvertToStableRepresentation))
            {
                var reference = (Reference)_elem;

                return DataTypeInfoHelper.CreateFrom(_document, methodInfo, reference.ConvertToStableRepresentation(_document), reference);
            }

            if (declaringType == typeof (View) && methodInfo.Name == nameof(View.GetFilterOverrides))
               return new ViewFiltersOverrideGraphicSettings(methodInfo.Name, (View) _elem);

            if (declaringType == typeof(View) && methodInfo.Name == nameof(View.GetFilterVisibility))
                return new ViewFiltersVisibilitySettings(methodInfo.Name, (View) _elem);

            if (declaringType == typeof(View) && methodInfo.Name == nameof(View.GetNonControlledTemplateParameterIds))
                return new ViewGetNonControlledTemplateParameterIds(methodInfo.Name, (View)_elem);

            if (declaringType == typeof(View) && methodInfo.Name == nameof(View.GetTemplateParameterIds))
                return new ViewGetTemplateParameterIds(methodInfo.Name, (View)_elem);

            if (declaringType == typeof(ScheduleDefinition) && methodInfo.Name == nameof(ScheduleDefinition.GetField))
            {
                var parameters = methodInfo.GetParameters();
                if (parameters[0].ParameterType == typeof(int))
                    return new ScheduleDefinitionGetFields(methodInfo.Name, (ScheduleDefinition)_elem);
            }

            if (declaringType == typeof(ViewCropRegionShapeManager) && methodInfo.Name == nameof(ViewCropRegionShapeManager.GetSplitRegionOffset))
                return new ViewCropRegionShapeManagerGetSplitRegionOffsets(methodInfo.Name, (ViewCropRegionShapeManager)_elem);

            if (declaringType == typeof(Curve) && methodInfo.Name == nameof(Curve.GetEndPoint))
                return new CurveGetEndPoint(methodInfo.Name, (Curve)_elem);

            if (declaringType == typeof(TableData) && methodInfo.Name == nameof(TableData.GetSectionData))
            {
                var parameters = methodInfo.GetParameters();
                if (parameters[0].ParameterType == typeof(SectionType))
                    return new TableDataSectionData(methodInfo.Name, (TableData)_elem);
            }

            if (declaringType == typeof(PlanViewRange) && methodInfo.Name == nameof(PlanViewRange.GetLevelId))
                return new PlanViewRangeGetLevelId(methodInfo.Name, (PlanViewRange) _elem, _document);

            if (declaringType == typeof(PlanViewRange) && methodInfo.Name == nameof(PlanViewRange.GetOffset))
                return new PlanViewRangeGetOffset(methodInfo.Name, (PlanViewRange)_elem);

            if (declaringType == typeof (Document) && methodInfo.Name == nameof(Document.Close))
                return null;

            if (methodInfo.GetParameters().Any() || methodInfo.ReturnType == typeof (void))
                return null;

            var returnValue = methodInfo.Invoke(_elem, new object[0]);

            return DataTypeInfoHelper.CreateFrom(_document, methodInfo, returnValue, _elem);
        }
    }
}