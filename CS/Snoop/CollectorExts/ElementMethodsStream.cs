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
        private readonly ArrayList data;
        private readonly List<string> seenMethods = new List<string>();
        private readonly DataFactory methodDataFactory;

        public ElementMethodsStream(UIApplication application, ArrayList data, object elem)
        {
            this.data = data;

            methodDataFactory = new DataFactory(application, elem);
        }

        public void Stream(Type type)
        {
            var methods = type
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .OrderBy(x => x.Name)
                .ToList();

            if (methods.Count > 0) data.Add(new Data.MemberSeparatorWithOffset("Methods"));

            var currentTypeMethods = new List<string>();

            foreach (var methodInfo in methods)
            {
                if (seenMethods.Contains(methodInfo.Name)) continue;

                currentTypeMethods.Add(methodInfo.Name);

                var methodData = GetMethodData(methodInfo);

                if (methodData != null)
                    data.Add(methodData);
            }

            seenMethods.AddRange(currentTypeMethods);
        }

        private Data.Data GetMethodData(MethodInfo methodInfo)
        {
            try
            {
                return methodDataFactory.Create(methodInfo);
            }
            catch (TargetException ex)
            {
                return new Data.Exception(methodInfo.Name, ex);
            }
            catch (TargetInvocationException ex)
            {
                return new Data.Exception(methodInfo.Name, ex);
            }
            catch (TargetParameterCountException ex)
            {
                return new Data.Exception(methodInfo.Name, ex);
            }
        }
    }
}