// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
//
// Description: Placeholder object, with a name that appears in the debugger
//

using System.Globalization;

namespace RevitLookup.UI.Win32;

/// <summary>
///     An instance of this class can be used wherever you might otherwise use
///     "new Object()".  The name will show up in the debugger, instead of
///     merely "{object}"
/// </summary>
internal class NamedObject
{
    private string _name;

    public NamedObject(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(name);

        _name = name;
    }

    public override string ToString()
    {
        if (_name[0] != '{')
            // lazily add {} around the name, to avoid allocating a string
            // until it's actually needed
            _name = string.Format(CultureInfo.InvariantCulture, "{{{0}}}", _name);

        return _name;
    }
}