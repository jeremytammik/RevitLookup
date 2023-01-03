using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using RevitLookup.Core.Descriptors.Contracts;
using RevitLookup.Core.Descriptors.Extensions;
using RevitLookup.ViewModels.Objects;

namespace RevitLookup.Core.Descriptors.Utils;

public static class ReflectionUtils
{
    public static void CollectMethods(Descriptor descriptor, Document context, List<Descriptor> members, object obj)
    {
        var type = obj.GetType();
        var methods = type
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .OrderBy(info => info.Name);

        foreach (var method in methods)
        {
            if (method.IsSpecialName) continue;
            if (method.ReturnType.Name == "Void") continue;

            var item = new ObjectDescriptor
            {
                Type = method.DeclaringType!.Name,
                Label = method.Name
            };

            try
            {
                object result;
                var args = method.GetParameters();
                if (args.Length > 0)
                {
                    if (descriptor is IDescriptorResolver resolver)
                    {
                        var manager = new ResolverManager(method.Name, args);
                        resolver.RegisterResolvers(manager);
                        if (!manager.IsResolved) continue;
                        result = manager.Result;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    result = method.Invoke(obj, null);
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

    public static void CollectProperties(Descriptor descriptor, Document context, List<Descriptor> members, object obj)
    {
        var type = obj.GetType();
        var properties = type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .OrderBy(info => info.Name);

        foreach (var property in properties)
        {
            if (property.IsSpecialName) continue;

            var item = new ObjectDescriptor
            {
                Type = property.DeclaringType!.Name,
                Label = property.Name
            };

            try
            {
                object result;
                var args = property.GetMethod.GetParameters();
                if (args.Length > 0)
                {
                    if (descriptor is IDescriptorResolver resolver)
                    {
                        var manager = new ResolverManager(property.Name, args);
                        resolver.RegisterResolvers(manager);
                        if (!manager.IsResolved) continue;
                        result = manager.Result;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    result = property.GetValue(obj);
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
}