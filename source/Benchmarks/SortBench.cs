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
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MediumRunJob]
[MemoryDiagnoser(false)]
public class SortBench
{
    private MethodInfo[] _methodInfos;

    [GlobalSetup]
    public void Setup()
    {
        var obj = new Thread(() => { });
        _methodInfos = obj.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
    }

    [Benchmark]
    public void Linq()
    {
        var enumerable = _methodInfos.OrderBy(info => info.Name);
        foreach (var methodInfo in enumerable)
        {
            var parameterInfos = methodInfo.GetParameters();
        }
    }

    [Benchmark]
    public void SortComparer()
    {
        Array.Sort(_methodInfos, new MethodInfoComparer());
        foreach (var methodInfo in _methodInfos)
        {
            var parameterInfos = methodInfo.GetParameters();
        }
    }
    
    [Benchmark]
    public void SortComparison()
    {
        Array.Sort(_methodInfos, Comparison);
        foreach (var methodInfo in _methodInfos)
        {
            var parameterInfos = methodInfo.GetParameters();
        }
    }
    
    private int Comparison(MethodInfo x, MethodInfo y)
    {
        return x.Name == y.Name ? 0 : string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
    }
}

public class MethodInfoComparer : IComparer<MethodInfo>
{
    public int Compare(MethodInfo x, MethodInfo y)
    {
        return x.Name == y.Name ? 0 : string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
    }
}