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
    private void AddProperties(BindingFlags bindingFlags)
    {
        var members = _type.GetProperties(bindingFlags);
        foreach (var member in members)
        {
            if (member.IsSpecialName) continue;
            
            object value;
            var parameters = member.CanRead ? member.GetMethod!.GetParameters() : null;
            
            try
            {
                if (!TryResolve(member, parameters, out value))
                {
                    if (!IsPropertySupported(member, parameters, out value)) continue;
                    
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
    
    private bool TryResolve(PropertyInfo member, ParameterInfo[] parameters, out object value)
    {
        value = null;
        if (_currentDescriptor is not IDescriptorResolver resolver) return false;
        
        var handler = resolver.Resolve(Context, member.Name, parameters);
        if (handler is null) return false;
        
        try
        {
            _clockDiagnoser.Start();
            _memoryDiagnoser.Start();
            value = handler.Invoke();
        }
        finally
        {
            _memoryDiagnoser.Stop();
            _clockDiagnoser.Stop();
        }
        
        return true;
    }
    
    private void Evaluate(PropertyInfo member, out object value)
    {
        try
        {
            _clockDiagnoser.Start();
            _memoryDiagnoser.Start();
            value = member.GetValue(_obj);
        }
        finally
        {
            _memoryDiagnoser.Stop();
            _clockDiagnoser.Stop();
        }
    }
    
    private bool IsPropertySupported(PropertyInfo member, ParameterInfo[] parameters, out object value)
    {
        value = null;
        
        if (!member.CanRead)
        {
            value = new NotSupportedException("Property does not have a get accessor, it cannot be read");
            return true;
        }
        
        if (parameters.Length > 0)
        {
            if (!_settings.IncludeUnsupported) return false;
            
            value = new NotSupportedException("Unsupported property overload");
            return true;
        }
        
        return true;
    }
}