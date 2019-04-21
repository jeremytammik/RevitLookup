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
            var methods = GetElementMethods(type)
                .Where(x => IsValidMethod(type, x))
                .ToList();

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
            List<MethodInfo> mInfo = new List<MethodInfo>();

            if(type.Name == "Reference")
                mInfo.Add(type.GetMethod("ConvertToStableRepresentation"));

            mInfo.AddRange(type
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(x => !x.GetParameters().Any()
                            && x.ReturnType != typeof(void)
                            && !x.IsSpecialName)
                .OrderBy(x => x.Name));

            return mInfo.ToArray();
        }

        private bool IsValidMethod(Type type, MethodInfo methodInfo)
        {
            var forbiddenMethods = new[]
                {
                    "Autodesk.Revit.DB.Document.Close"
                };

            var name = string.Format("{0}.{1}", type.FullName, methodInfo.Name);

            return !forbiddenMethods.Contains(name);
        }

        private void AddMethodToData(MethodInfo mi)
        {
            Type methodType = mi.ReturnType;

            try
            {
                object returnValue;
                if (mi.Name == "ConvertToStableRepresentation")
                {
                    returnValue = mi.Invoke(elem, new object[] {application.ActiveUIDocument.Document});
                }
                else
                {
                    returnValue = mi.Invoke(elem, new object[0]);
                }
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