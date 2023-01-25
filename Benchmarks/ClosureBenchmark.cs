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

using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[ShortRunJob]
[MemoryDiagnoser]
public class ClosureBenchmark
{
    [GlobalSetup]
    public void Setup()
    {
        Manager = new ExtensionManager("context");
    }
    
    [Params("Text", 12d)] public object Parameter { get; set; }
    public ExtensionManager Manager { get; set; }

    [Benchmark]
    public void ClosureMethod()
    {
        Manager.Register("Extension", () => Parameter.ToString());
    }

    [Benchmark]
    public void NonClosureMethod()
    {
        Manager.Register("Extension", Parameter, extension =>
        {
            extension.Result = extension.Value.ToString();
        });
    }
}

public sealed class ExtensionManager
{
    private readonly string _context;

    public ExtensionManager(string context)
    {
        _context = context;
    }

    public List<Descriptor> Descriptors { get; set; } = new();

    public void Register<T>(string name, T value, Action<DescriptorExtension<T>> extension)
    {
        var descriptorExtension = new DescriptorExtension<T>
        {
            Value = value,
            Context = _context
        };

        var descriptor = new ObjectDescriptor
        {
            Label = name
        };

        try
        {
            extension.Invoke(descriptorExtension);
            descriptor.Value = new SnoopableObject(_context, descriptorExtension.Result);
        }
        catch (Exception exception)
        {
            descriptor.Value = new SnoopableObject(_context, exception);
        }

        Descriptors.Add(descriptor);
    }

    public void Register(string name, Func<object> result)
    {
        var descriptor = new ObjectDescriptor
        {
            Label = name
        };

        try
        {
            descriptor.Value = new SnoopableObject(_context, result());
        }
        catch (Exception exception)
        {
            descriptor.Value = new SnoopableObject(_context, exception);
        }

        Descriptors.Add(descriptor);
    }
}

public sealed class DescriptorExtension<T>
{
    public string Context { get; set; }
    public T Value { get; set; }
    public object Result { get; set; }
}

public sealed class ObjectDescriptor : Descriptor
{
    public ObjectDescriptor()
    {
    }

    public ObjectDescriptor(object value)
    {
        Label = value.ToString();
    }
}

public abstract class Descriptor : IComparable<Descriptor>, IComparable
{
    public string Type { get; set; }
    public string Label { get; set; }
    public SnoopableObject Value { get; set; }

    public int CompareTo(object obj)
    {
        if (ReferenceEquals(null, obj)) return 1;
        if (ReferenceEquals(this, obj)) return 0;
        return obj is Descriptor other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(Descriptor)}");
    }

    public int CompareTo(Descriptor other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        var typeComparison = string.Compare(Type, other.Type, StringComparison.Ordinal);
        if (typeComparison != 0) return typeComparison;
        return string.Compare(Label, other.Label, StringComparison.Ordinal);
    }
}

public sealed class SnoopableObject
{
    public SnoopableObject(string context, object obj)
    {
        Object = obj;
        Context = context;
    }

    public object Object { get; }
    public string Context { get; }
}