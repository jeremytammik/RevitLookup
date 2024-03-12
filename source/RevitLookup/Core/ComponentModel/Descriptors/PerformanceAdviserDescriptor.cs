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
using Autodesk.Revit.DB;
using RevitLookup.Core.Contracts;
using RevitLookup.Core.Objects;

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class PerformanceAdviserDescriptor(PerformanceAdviser adviser) : Descriptor, IDescriptorResolver
{
    public ResolveSet Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        if (parameters.Length == 0) return null;
        var rules = adviser.GetNumberOfRules();
        var resolveSet = new ResolveSet(rules);

        switch (parameters.Length)
        {
            case 1 when parameters[0].ParameterType == typeof(Document):
                switch (target)
                {
                    case nameof(PerformanceAdviser.ExecuteAllRules):
                    {
                        resolveSet.AppendVariant(adviser.ExecuteAllRules(RevitShell.Document));
                        break;
                    }
                    default:
                        return null;
                }

                break;
            case 1 when parameters[0].ParameterType == typeof(int):
                switch (target)
                {
                    case nameof(PerformanceAdviser.GetRuleDescription):
                    {
                        for (var i = 0; i < rules; i++) resolveSet.AppendVariant(new KeyValuePair<int, string>(i, adviser.GetRuleDescription(i)));
                        break;
                    }
                    case nameof(PerformanceAdviser.GetRuleId):
                    {
                        for (var i = 0; i < rules; i++) resolveSet.AppendVariant(new KeyValuePair<int, PerformanceAdviserRuleId>(i, adviser.GetRuleId(i)));
                        break;
                    }
                    case nameof(PerformanceAdviser.GetRuleName):
                    {
                        for (var i = 0; i < rules; i++) resolveSet.AppendVariant(new KeyValuePair<int, string>(i, adviser.GetRuleName(i)));
                        break;
                    }
                    case nameof(PerformanceAdviser.IsRuleEnabled):
                    {
                        for (var i = 0; i < rules; i++) resolveSet.AppendVariant(new KeyValuePair<int, bool>(i, adviser.IsRuleEnabled(i)));
                        break;
                    }
                    case nameof(PerformanceAdviser.WillRuleCheckElements):
                    {
                        for (var i = 0; i < rules; i++) resolveSet.AppendVariant(new KeyValuePair<int, bool>(i, adviser.WillRuleCheckElements(i)));
                        break;
                    }
                    default:
                        return null;
                }

                break;
            case 2 when parameters[0].ParameterType == typeof(int):
                switch (target)
                {
                    case nameof(PerformanceAdviser.GetElementFilterFromRule):
                    {
                        for (var i = 0; i < rules; i++)
                            resolveSet.AppendVariant(new KeyValuePair<int, ElementFilter>(i, adviser.GetElementFilterFromRule(i, RevitShell.Document)));
                        break;
                    }
                    default:
                        return null;
                }

                break;
            default:
                return null;
        }

        return resolveSet;
    }
}