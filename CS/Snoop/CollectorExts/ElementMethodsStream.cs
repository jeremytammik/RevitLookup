using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitLookup.Snoop.CollectorExts
{
    public class ElementMethodsStream : IElementStream
    {
        private readonly UIApplication application;
        private readonly ArrayList data;
        private readonly object elem;
        private readonly List<string> seenMethods = new List<string>();
        private readonly IEnumerable<string> specialMethods;
        private readonly IEnumerable<string> forbiddenMethods;

        public ElementMethodsStream(UIApplication application, ArrayList data, object elem)
        {
            this.application = application;
            this.data = data;
            this.elem = elem;

            specialMethods = new[]
                {
                    $"{typeof(Reference).FullName}.{nameof(Reference.ConvertToStableRepresentation)}",
                    $"{typeof(Element).FullName}.{nameof(Element.GetDependentElements)}"
                };

            forbiddenMethods = new[]
                {
                    $"{typeof(Document).FullName}.{nameof(Document.Close)}"
                };
        }

        public void Stream(Type type)
        {
            var methods = GetElementMethods(type).ToList();

            if (methods.Count > 0) data.Add(new Snoop.Data.MemberSeparatorWithOffset("Methods"));

            var currentTypeMethods = new List<string>();

            foreach (MethodInfo mi in methods)
            {
                if (seenMethods.Contains(mi.Name)) continue;

                currentTypeMethods.Add(mi.Name);
                AddMethodToData(mi);
            }

            seenMethods.AddRange(currentTypeMethods);
        }

        private MethodInfo[] GetElementMethods(Type type)
        {
            return type
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(IsValidMethod)
                .OrderBy(x => x.Name)
                .ToArray();
        }

        private bool IsValidMethod(MethodInfo methodInfo)
        {
            var name = $"{methodInfo.DeclaringType?.FullName}.{methodInfo.Name}";

            if (methodInfo.IsSpecialName || forbiddenMethods.Contains(name))
                return false;
            
            return specialMethods.Contains(name) || (!methodInfo.GetParameters().Any() && methodInfo.ReturnType != typeof(void));
        }

        private void AddMethodToData(MethodInfo mi)
        {
            Type methodType = mi.ReturnType;

            try
            {
                object returnValue;
                if (mi.Name == nameof(Reference.ConvertToStableRepresentation))
                    returnValue = mi.Invoke(elem, new object[] {application.ActiveUIDocument.Document});
                else if (mi.Name == nameof(Element.GetDependentElements))
                    returnValue = mi.Invoke(elem, new object[] {null});
                else
                    returnValue = mi.Invoke(elem, new object[0]);
                DataTypeInfoHelper.AddDataFromTypeInfo(application, mi, methodType, returnValue, elem, data);
                
            }
            catch (TargetException ex)
            {
                data.Add(new Snoop.Data.Exception(mi.Name, ex));
            }
            catch (TargetInvocationException ex)
            {
                data.Add(new Snoop.Data.Exception(mi.Name, ex));
            }
            catch (TargetParameterCountException ex)
            {
                data.Add(new Snoop.Data.Exception(mi.Name, ex));
            }
        }
    }
}