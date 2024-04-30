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

using System.Reflection;
using RevitLookup.Core.Contracts;

namespace RevitLookup.Core.Metadata;

public sealed partial class DescriptorBuilder
{
    private void AddMethods(BindingFlags bindingFlags)
    {
        var members = _type.GetMethods(bindingFlags);
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
            catch (TargetInvocationException exception)
            {
                value = exception.InnerException;
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

    private bool TryEvaluate(MethodInfo member, out object value, out ParameterInfo[] parameters)
    {
        value = null;
        parameters = member.GetParameters();

        if (_currentDescriptor is IDescriptorResolver resolver)
        {
            value = resolver.Resolve(Context, member.Name, parameters);
            if (value is not null) return true;
        }
        
        if (member.ReturnType.Name == "Void")
        {
            if (!_settings.IncludeUnsupported) return false;

            value = new NotSupportedException("Method doesn't return a value");
            return true;
        }   

        if (parameters.Length > 0)
        {
            if (!_settings.IncludeUnsupported) return false;

            value = new NotSupportedException("Unsupported method overload");
            return true;
        }

        value = member.Invoke(_obj, null);
        return true;
    }
}