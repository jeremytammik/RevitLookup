﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;

// ReSharper disable once CheckNamespace
namespace System.Runtime.CompilerServices;

/// <summary>
///     Reserved to be used by the compiler for tracking metadata.
///     This class should not be used by developers in source code.
///     This dummy class is required to compile records when targeting .NET Standard
/// </summary>
[PublicAPI("C# 9 feature: init keyword")]
[EditorBrowsable(EditorBrowsableState.Never)]
public static class IsExternalInit
{
}