// Copyright 2003-2024 by Autodesk, Inc.
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

using System.Collections;
using RevitLookup.Core.Contracts;

namespace RevitLookup.Core.Objects;

/// <summary>
///     Provides methods for creating variant collections
/// </summary>
/// <remarks>Variants are provided for IDescriptorResolver</remarks>
public static class Variants
{
    /// <summary>
    ///     Creates a variant collection with a single value
    /// </summary>
    /// <returns>A variant collection containing the specified value</returns>
    public static IVariants Single<T>(T value)
    {
        return new Variants<T>(1).Add(value);
    }
    
    /// <summary>
    ///     Creates a variant collection with a single value and description
    /// </summary>
    /// <returns>A variant collection containing the specified value</returns>
    public static IVariants Single<T>(T value, string description)
    {
        return new Variants<T>(1).Add(value, description);
    }
    
    /// <summary>
    ///     Creates an empty variant collection
    /// </summary>
    /// <returns>An empty variant collection</returns>
    /// <remarks>An empty collection is returned when there are no solutions for a member</remarks>
    public static IVariants Empty<T>()
    {
        return new Variants<T>(0);
    }
    
    /// <summary>
    ///     A variant that disables the member calculation
    /// </summary>
    public static Func<IVariants> Disabled { get; } = () => new Variants<InvalidOperationException>(1).Add(new InvalidOperationException("Method execution disabled"));
}

/// <summary>
///     Represents a collection of variants
/// </summary>
/// <typeparam name="T">The type of the variants</typeparam>
/// <param name="capacity">The initial variants capacity. Required for atomic performance optimizations</param>
public sealed class Variants<T>(int capacity) : IVariants<T>
{
    private readonly Queue<Variant> _items = new(capacity);
    
    /// <summary>
    ///     Gets the number of variants
    /// </summary>
    public int Count => _items.Count;
    
    /// <summary>
    ///     Adds a new variant
    /// </summary>
    /// <returns>The variant collection with a new value</returns>
    public Variants<T> Add(T result)
    {
        if (result is null) return this;
        if (result is ICollection {Count: 0}) return this;
        
        _items.Enqueue(new Variant
        {
            Object = result
        });
        
        return this;
    }
    
    /// <summary>
    ///     Adds a new variant with description
    /// </summary>
    /// <returns>The variant collection with a new value</returns>
    public Variants<T> Add(T result, string description)
    {
        if (result is null) return this;
        if (result is ICollection {Count: 0}) return this;
        
        _items.Enqueue(new Variant
        {
            Object = result,
            Description = description
        });
        
        return this;
    }
    
    /// <summary>
    ///     Returns a single value if exists
    /// </summary>
    /// <returns>Single variant</returns>
    /// <remarks>Used for internal purposes only and to display a single result instead of a list</remarks>
    public Variant Single()
    {
        if (_items.Count != 1)
        {
            throw new IndexOutOfRangeException("Variants contains more than one element or variants is empty");
        }
        
        return _items.Peek();
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    IEnumerator<Variant> IEnumerable<Variant>.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    private Queue<Variant>.Enumerator GetEnumerator()
    {
        return _items.GetEnumerator();
    }
}