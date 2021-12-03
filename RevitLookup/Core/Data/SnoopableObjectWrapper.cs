using System;
using RevitLookup.Core.Data.PlaceHolders;

namespace RevitLookup.Core.Data
{
    public class SnoopableObjectWrapper
    {
        public SnoopableObjectWrapper(string title, object obj)
        {
            Title = title;

            Object = obj;
        }

        public string Title { get; }

        public object Object { get; }

        public static SnoopableObjectWrapper Create(object obj)
        {
            return new SnoopableObjectWrapper(Utils.ObjToLabelStr(obj), obj);
        }

        public Type GetUnderlyingType()
        {
            if (Object is IObjectToSnoopPlaceholder placeholder) return placeholder.GetUnderlyingType();

            return Object.GetType();
        }
    }
}