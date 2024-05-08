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

namespace RevitLookup.Core.Engine;

public sealed partial class DescriptorBuilder
{
    private void AddMethods(BindingFlags bindingFlags)
    {
        var members = _type.GetMethods(bindingFlags);
        foreach (var member in members)
        {
            if (member.IsSpecialName) continue;
            
            object value;
            var parameters = member.GetParameters();
            
            try
            {
                if (!TryResolve(member, parameters, out value))
                {
                    if (!IsMethodSupported(member, parameters, out value)) continue;
                    
                    if (value is null)
                    {
                        Evaluate(member, out value);
                    }
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
            
            WriteDescriptor(member, value, parameters);
        }
    }
    
    private bool TryResolve(MethodInfo member, ParameterInfo[] parameters, out object value)
    {
        value = null;
        if (_currentDescriptor is not IDescriptorResolver resolver) return false;
        
        try
        {
            _clockDiagnoser.Start();
            _memoryDiagnoser.Start();
            value = resolver.Resolve(Context, member.Name, parameters);
        }
        finally
        {
            _memoryDiagnoser.Stop();
            _clockDiagnoser.Stop();
        }
        
        return value is not null;
    }
    
    private void Evaluate(MethodInfo member, out object value)
    {
        try
        {
            _clockDiagnoser.Start();
            _memoryDiagnoser.Start();
            value = member.Invoke(_obj, null);
        }
        finally
        {
            _memoryDiagnoser.Stop();
            _clockDiagnoser.Stop();
        }
    }
    
    private bool IsMethodSupported(MethodInfo member, ParameterInfo[] parameters, out object value)
    {
        value = null;
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
        
        return true;
    }
}