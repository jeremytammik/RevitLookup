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
using Autodesk.Revit.DB.ExternalService;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class ExternalServiceDescriptor : Descriptor, IDescriptorResolver
{
    private readonly ExternalService _service;
    
    public ExternalServiceDescriptor(ExternalService service)
    {
        _service = service;
        Name = service.Name;
    }
    
    public Func<IVariants> Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(ExternalService.GetServer) => ResolveGetServer,
            _ => null
        };
        
        IVariants ResolveGetServer()
        {
            var serverIds = _service.GetRegisteredServerIds();
            var resolveSet = new Variants<IExternalServer>(_service.NumberOfServers);
            foreach (var serverId in serverIds)
            {
                resolveSet.Add(_service.GetServer(serverId));
            }
            
            return resolveSet;
        }
    }
}