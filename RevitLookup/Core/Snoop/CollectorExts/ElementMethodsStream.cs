using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autodesk.Revit.DB;
using RevitLookup.Core.Snoop.Data;
using Exception = RevitLookup.Core.Snoop.Data.Exception;

namespace RevitLookup.Core.Snoop.CollectorExts
{
    public class ElementMethodsStream : IElementStream
    {
        private readonly ArrayList _data;
        private readonly DataFactory _methodDataFactory;
        private readonly List<string> _seenMethods = new();

        public ElementMethodsStream(Document document, ArrayList data, object elem)
        {
            _data = data;

            _methodDataFactory = new DataFactory(document, elem);
        }

        public void Stream(Type type)
        {
            var methods = type
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .OrderBy(x => x.Name)
                .ToList();

            if (methods.Count > 0) _data.Add(new MemberSeparatorWithOffset("Methods"));

            var currentTypeMethods = new List<string>();

            foreach (var methodInfo in methods)
            {
                if (_seenMethods.Contains(methodInfo.Name)) continue;

                currentTypeMethods.Add(methodInfo.Name);

                var methodData = GetMethodData(methodInfo);

                if (methodData != null)
                    _data.Add(methodData);
            }

            _seenMethods.AddRange(currentTypeMethods);
        }

        private Data.Data GetMethodData(MethodInfo methodInfo)
        {
            try
            {
                return _methodDataFactory.Create(methodInfo);
            }
            catch (TargetException ex)
            {
                return new Exception(methodInfo.Name, ex);
            }
            catch (TargetInvocationException ex)
            {
                return new Exception(methodInfo.Name, ex);
            }
            catch (TargetParameterCountException ex)
            {
                return new Exception(methodInfo.Name, ex);
            }
        }
    }
}