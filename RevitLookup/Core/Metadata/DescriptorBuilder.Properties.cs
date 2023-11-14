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
using RevitLookup.Core.Contracts;

namespace RevitLookup.Core.Metadata;

public sealed partial class DescriptorBuilder
{
    private void AddProperties(BindingFlags bindingFlags)
    {
        var members = _type.GetProperties(bindingFlags);
        foreach (var member in members)
        {
            if (member.IsSpecialName) continue;

            object value;
            ParameterInfo[] parameters = null;
            _tracker.Start();
            try
            {
                if (!TryEvaluate(member, out value, out parameters))
                {
                    _tracker.Reset();
                    continue;
                }
            }
            catch (Exception exception)
            {
                value = exception;
            }
            finally
            {
                _tracker.Stop();
            }

            WriteDescriptor(member, value, parameters);
        }
    }

    private bool TryEvaluate(PropertyInfo member, out object value, out ParameterInfo[] parameters)
    {
        value = null;
        parameters = null;

        if (!member.CanRead)
        {
            value = new NotSupportedException("Property does not have a get accessor, it cannot be read");
            return true;
        }

        parameters = member.GetMethod.GetParameters();
        if (_currentDescriptor is IDescriptorResolver resolver)
        {
            value = resolver.Resolve(Context, member.Name, parameters);
            if (value is not null) return true;
        }

        if (parameters.Length > 0)
        {
            if (!_settings.IsUnsupportedAllowed) return false;

            value = new NotSupportedException("Unsupported property overload");
            return true;
        }

        value = member.GetValue(_obj);
        return true;
    }
}