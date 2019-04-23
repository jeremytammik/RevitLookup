using System.Linq;
using System.Reflection;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using RevitLookup.Snoop.Data;

namespace RevitLookup.Snoop.CollectorExts
{
    public class DataFactory
    {
        private readonly UIApplication application;
        private readonly object elem;

        public DataFactory(UIApplication application, object elem)
        {
            this.application = application;
            this.elem = elem;
        }

        public Data.Data Create(MethodInfo methodInfo)
        {
            var declaringType = methodInfo.DeclaringType;

            if (methodInfo.IsSpecialName || declaringType == null)
                return null;

            if (declaringType == typeof(Element) && methodInfo.Name == nameof(Element.GetDependentElements))
            {
                var element = (Element) elem;

                return DataTypeInfoHelper.CreateFrom(application, methodInfo, element.GetDependentElements(null), element);
            }

            if (declaringType == typeof (Element) && methodInfo.Name == nameof(Element.GetPhaseStatus))
                return new ElementPhaseStatuses(methodInfo.Name, (Element) elem);

            if (declaringType == typeof(Reference) && methodInfo.Name == nameof(Reference.ConvertToStableRepresentation))
            {
                var reference = (Reference)elem;

                return DataTypeInfoHelper.CreateFrom(application, methodInfo, reference.ConvertToStableRepresentation(application.ActiveUIDocument.Document), reference);
            }

            if (declaringType == typeof (View) && methodInfo.Name == nameof(View.GetFilterOverrides))
               return new ViewFilterOverrideGraphicSettings(methodInfo.Name, (View) elem);

            if (declaringType == typeof(View) && methodInfo.Name == nameof(View.GetFilterVisibility))
                return new ViewFiltersVisibilitySettings(methodInfo.Name, (View) elem);

            if (declaringType == typeof (Document) && methodInfo.Name == nameof(Document.Close))
                return null;

            if (methodInfo.GetParameters().Any() || methodInfo.ReturnType == typeof (void))
                return null;

            var returnValue = methodInfo.Invoke(elem, new object[0]);

            return DataTypeInfoHelper.CreateFrom(application, methodInfo, returnValue, elem);
        }
    }
}