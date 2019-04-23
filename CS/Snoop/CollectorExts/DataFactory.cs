using System.Linq;
using System.Reflection;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

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

            if (methodInfo.Name == nameof(Element.GetDependentElements))
            {
                var element = (Element) elem;

                return DataTypeInfoHelper.CreateFrom(application, methodInfo, element.GetDependentElements(null), element);
            }

            if (methodInfo.Name == nameof(Reference.ConvertToStableRepresentation))
            {
                var reference = (Reference)elem;

                return DataTypeInfoHelper.CreateFrom(application, methodInfo, reference.ConvertToStableRepresentation(application.ActiveUIDocument.Document), reference);
            }

            if (declaringType == typeof (Document) && methodInfo.Name == nameof(Document.Close))
                return null;

            if (methodInfo.GetParameters().Any() || methodInfo.ReturnType == typeof (void))
                return null;

            var returnValue = methodInfo.Invoke(elem, new object[0]);

            return DataTypeInfoHelper.CreateFrom(application, methodInfo, returnValue, elem);
        }
    }
}