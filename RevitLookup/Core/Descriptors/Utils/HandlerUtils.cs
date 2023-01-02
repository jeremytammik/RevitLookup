using System.Reflection;
using Autodesk.Revit.DB;
using RevitLookup.ViewModels.Objects;

namespace RevitLookup.Core.Descriptors.Utils;

public static class HandlerUtils
{
    public static IReadOnlyList<Descriptor> HandleMethods(Descriptor descriptor, object obj)
    {
        return HandleMethods(descriptor, null, obj);
    }

    public static IReadOnlyList<Descriptor> HandleMethods(Descriptor descriptor, Document context, object obj)
    {
        var type = obj.GetType();
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        var methodDescriptors = new List<Descriptor>();
        foreach (var methodInfo in methods)
        {
            if (methodInfo.IsSpecialName) continue;

            var args = methodInfo.GetParameters();
            var methodDescriptor = new ObjectDescriptor
            {
                Type = methodInfo.DeclaringType!.Name,
                Label = methodInfo.Name
            };

            try
            {
                object result;
                if (args.Length > 0)
                {
                    if (!descriptor.TryInvoke(methodInfo.Name, args, out result)) continue;
                }
                else
                {
                    result = methodInfo.Invoke(obj, null);
                }

                methodDescriptor.Child = new SnoopableObject(context, result);
            }
            catch (Exception exception)
            {
                methodDescriptor.Child = new SnoopableObject(context, exception);
            }

            methodDescriptors.Add(methodDescriptor);
        }

        methodDescriptors.Sort();
        return methodDescriptors;
    }
}