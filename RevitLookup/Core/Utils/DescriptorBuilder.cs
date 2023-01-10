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
using RevitLookup.Core.Comparers;
using RevitLookup.Core.ComponentModel.Descriptors;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Extensions;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.Utils;

public sealed class DescriptorBuilder : IBuilderConfigurator
{
    private readonly SnoopableObject _snoopableObject;
    private readonly List<Descriptor> _descriptors;
    [CanBeNull] private Descriptor _descriptor;
    private ExtensionManager _extensionManager;
    private Type _type;

    public DescriptorBuilder(SnoopableObject snoopableObject)
    {
        _snoopableObject = snoopableObject;
        _descriptors = new List<Descriptor>(8);
    }

    public ExtensionManager ExtensionManager => _extensionManager ??= new ExtensionManager(_descriptor, _snoopableObject.Context);

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
                Type = _descriptor is null ? member.DeclaringType!.Name : _descriptor.Type,
                Label = member.Name,
                Value = new SnoopableObject(_snoopableObject.Context, value)
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
                Type = _descriptor is null ? member.DeclaringType!.Name : _descriptor.Type,
                Label = member.Name,
                Value = new SnoopableObject(_snoopableObject.Context, value)
            };

            _descriptors.Add(descriptor);
        }
    }

    public void AddExtensions()
    {
        if (_descriptor is not IDescriptorExtension extension) return;

        extension.RegisterExtensions(ExtensionManager);
        if (_extensionManager.ClassExtensions is not null) _descriptors.AddRange(_extensionManager.ClassExtensions);
    }

    public IReadOnlyList<Descriptor> Build(Action<IBuilderConfigurator> configurator)
    {
        if (_snoopableObject.Object is null) return Array.Empty<Descriptor>();

        var type = _snoopableObject.Object.GetType();
        var types = new List<Type>();
        while (type.BaseType is not null)
        {
            types.Add(type);
            type = type.BaseType;
        }

        for (var i = types.Count - 1; i >= 0; i--)
        {
            _type = types[i];

            //Finding a descriptor to analyze IDescriptorResolver and IDescriptorExtension interfaces
            var descriptor = DescriptorUtils.FindSuitableDescriptor(_snoopableObject.Object, _type);
            //And creating an empty descriptor in case of mismatch of base types
            if (descriptor.Type == _snoopableObject.Descriptor.Type) _descriptor = descriptor;

            configurator(this);
        }

        //Adding object extensions to the end of the table
        if (_extensionManager?.ObjectExtensions is not null) _descriptors.AddRange(_extensionManager.ObjectExtensions);
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

        value = member.GetValue(_snoopableObject.Object);
#else
        value = args.Length > 0 ? new NotSupportedException("Unsupported property. Try implement IDescriptorResolver") : member.GetValue(_snoopableObject.Object);
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

        value = member.Invoke(_snoopableObject.Object, null);
#else
        value = args.Length > 0 ? new NotSupportedException("Unsupported property. Try implement IDescriptorResolver") : member.Invoke(_snoopableObject.Object, null);
#endif

        return true;
    }
}