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

using RevitLookup.Core.Contracts;

namespace RevitLookup.Core.Engine;

public sealed partial class DescriptorBuilder : IExtensionManager
{
    private void AddExtensions()
    {
        if (!_settings.IncludeExtensions) return;
        if (_currentDescriptor is not IDescriptorExtension extension) return;
        
        extension.RegisterExtensions(this);
    }
    
    public void Register(string methodName, Func<Document, object> handler)
    {
        try
        {
            var result = Evaluate(handler);
            WriteDescriptor(methodName, result);
        }
        catch (Exception exception)
        {
            WriteDescriptor(methodName, exception);
        }
    }
    
    private object Evaluate(Func<Document, object> handler)
    {
        try
        {
            _clockDiagnoser.Start();
            _memoryDiagnoser.Start();
            return handler.Invoke(Context);
        }
        finally
        {
            _memoryDiagnoser.Stop();
            _clockDiagnoser.Stop();
        }
    }
}