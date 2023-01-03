using System.Reflection;
using Autodesk.Revit.DB;
using RevitLookup.Core.Descriptors.Contracts;
using RevitLookup.ViewModels.Objects;

namespace RevitLookup.Core.Descriptors.Utils;

public static class ReflectionUtils
{
    public static void HandleMethods(Descriptor descriptor, Document context, List<Descriptor> members, object obj)
    {
        var type = obj.GetType();
        var methods = type
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .OrderBy(info => info.Name);

        foreach (var methodInfo in methods)
        {
            if (methodInfo.IsSpecialName) continue;

            var args = methodInfo.GetParameters();
            var item = new ObjectDescriptor
            {
                Type = methodInfo.DeclaringType!.Name,
                Label = methodInfo.Name
            };

            try
            {
                object result;
                if (args.Length > 0)
                {
                    if (descriptor is IInvokedDescriptor invoker)
                    {
                        if (!invoker.TryInvoke(methodInfo.Name, args, out result))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    result = methodInfo.Invoke(obj, null);
                }

                item.Value = new SnoopableObject(context, result);
            }
            catch (Exception exception)
            {
                item.Value = new SnoopableObject(context, exception);
            }

            members.Add(item);
        }
    }

    public static void HandleProperties(Descriptor descriptor, Document context, List<Descriptor> members, object obj)
    {
        var type = obj.GetType();
        var properties = type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .OrderBy(info => info.Name);

        foreach (var propertyInfo in properties)
        {
            if (propertyInfo.IsSpecialName) continue;
            if (propertyInfo.GetMethod.IsSpecialName) continue;

            var item = new ObjectDescriptor
            {
                Type = propertyInfo.DeclaringType!.Name,
                Label = propertyInfo.Name,
                Value = new SnoopableObject(context, propertyInfo.GetValue(obj))
            };

            members.Add(item);
        }
    }
}