using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autodesk.Revit.UI;

namespace RevitLookup.Snoop.CollectorExts
{
    public class ElementMethodsStream : IElementStream
    {
        private readonly UIApplication application;
        private readonly ArrayList data;
        private readonly object elem;
        private readonly List<string> seenMethods = new List<string>();

        public ElementMethodsStream(UIApplication application, ArrayList data, object elem)
        {
            this.application = application;
            this.data = data;
            this.elem = elem;
        }

        public void Stream(Type type)
        {
            var methods = GetElementMethods(type);

            if (methods.Length > 0) data.Add(new Snoop.Data.MemberSeparatorWithOffset("Methods"));

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
            MethodInfo[] mInfo = type
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(x => !x.GetParameters().Any()
                            && x.ReturnType != typeof(void)
                            && !x.IsSpecialName)
                .OrderBy(x => x.Name)
                .ToArray();

            return mInfo;
        }

        private void AddMethodToData(MethodInfo mi)
        {
            Type methodType = mi.ReturnType;

            try
            {
                var returnValue = mi.Invoke(elem, new object[0]);
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