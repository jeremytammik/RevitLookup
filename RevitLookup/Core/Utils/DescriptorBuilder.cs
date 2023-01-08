// Copyright 2003-2023 by Autodesk, Inc.
// 
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
// 
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
// 
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.

using System.Reflection;
using Autodesk.Revit.DB;
using RevitLookup.Core.ComponentModel.Descriptors;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Extensions;

namespace RevitLookup.Core.Utils;

public class DescriptorBuilder : IBuilderConfigurator
{
    private readonly object _obj;
    private readonly Document _context;
    private readonly List<Descriptor> _descriptors;
    private Type _type;
    private Descriptor _descriptor;

    public DescriptorBuilder(object obj, Document context)
    {
        _obj = obj;
        _context = context;
        _descriptors = new List<Descriptor>(0);
    }

    public void AddProperties()
    {
        var members = _type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Array.Sort(members, new PropertyInfoComparer());

        foreach (var member in members)
        {
            if (member.IsSpecialName) continue;

            object value;
            try
            {
                if (!TryEvaluate(member, out value)) continue;
            }
            catch (Exception exception)
            {
                value = exception;
            }

            var descriptor = new ObjectDescriptor
            {
                Type = member.DeclaringType!.Name,
                Label = member.Name,
                Value = new SnoopableObject(_context, value)
            };

            _descriptors.Add(descriptor);
        }
    }

    public void AddMethods()
    {
        var members = _type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Array.Sort(members, new MethodInfoComparer());

        foreach (var member in members)
        {
            if (member.IsSpecialName) continue;
            if (member.ReturnType.Name == "Void") continue;

            object value;
            try
            {
                if (!TryEvaluate(member, out value)) continue;
            }
            catch (Exception exception)
            {
                value = exception;
            }

            var descriptor = new ObjectDescriptor
            {
                Type = member.DeclaringType!.Name,
                Label = member.Name,
                Value = new SnoopableObject(_context, value)
            };

            _descriptors.Add(descriptor);
        }
    }

    public void AddClassExtensions()
    {
        if (_descriptor is not IDescriptorExtension extension) return;
        extension.RegisterExtensions(new ExtensionManager(_descriptor, _context, _descriptors));
    }

    public void AddGroupExtensions()
    {
    }

    public IReadOnlyList<Descriptor> Build(Action<IBuilderConfigurator> configurator)
    {
        if (_obj is null) return Array.Empty<Descriptor>();

        var type = _obj.GetType();
        var types = new List<Type>();
        while (type.BaseType is not null)
        {
            types.Add(type);
            type = type.BaseType;
        }

        for (var i = types.Count - 1; i >= 0; i--)
        {
            _type = types[i];
            _descriptor = DescriptorUtils.FindSuitableDescriptor(_obj, _type);
            configurator(this);
        }

        return _descriptors;
    }

    private bool TryEvaluate(PropertyInfo member, out object value)
    {
        var args = member.GetMethod.GetParameters();
        if (_descriptor is IDescriptorResolver resolver)
        {
            var manager = new ResolverManager(member.Name, args);
            resolver.RegisterResolvers(manager);
            if (manager.IsResolved)
            {
                value = manager.Result;
                return true;
            }
        }
        //TODO add settings
#if RELEASE
        if (args.Length > 0)
        {
            value = null;
            return false;
        }

        value = member.GetValue(_obj);
#else
        value = args.Length > 0 ? new NotSupportedException("Unsupported property. Try implement IDescriptorResolver") : member.GetValue(_obj);
#endif

        return true;
    }

    private bool TryEvaluate(MethodInfo member, out object value)
    {
        var args = member.GetParameters();
        if (_descriptor is IDescriptorResolver resolver)
        {
            var manager = new ResolverManager(member.Name, args);
            resolver.RegisterResolvers(manager);
            if (manager.IsResolved)
            {
                value = manager.Result;
                return true;
            }
        }
        //TODO add settings
#if RELEASE
        if (args.Length > 0)
        {
            value = null;
            return false;
        }

        value = member.Invoke(_obj, null);
#else
        value = args.Length > 0 ? new NotSupportedException("Unsupported property. Try implement IDescriptorResolver") : member.Invoke(_obj, null);
#endif

        return true;
    }
}