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

namespace RevitLookup.Core.ComponentModel.Descriptors;

public sealed class PerformanceAdviserDescriptor(PerformanceAdviser adviser) : Descriptor, IDescriptorResolver
{
    public Func<IVariants> Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(PerformanceAdviser.ExecuteAllRules) when parameters.Length == 1 &&
                                                            parameters[0].ParameterType == typeof(Document) => ResolveExecuteAllRules,
            nameof(PerformanceAdviser.GetRuleDescription) when parameters.Length == 1 &&
                                                               parameters[0].ParameterType == typeof(int) => ResolveGetRuleDescription,
            nameof(PerformanceAdviser.GetRuleId) when parameters.Length == 1 &&
                                                      parameters[0].ParameterType == typeof(int) => ResolveGetRuleId,
            nameof(PerformanceAdviser.GetRuleName) when parameters.Length == 1 &&
                                                        parameters[0].ParameterType == typeof(int) => ResolveGetRuleName,
            nameof(PerformanceAdviser.IsRuleEnabled) when parameters.Length == 1 &&
                                                          parameters[0].ParameterType == typeof(int) => ResolveIsRuleEnabled,
            nameof(PerformanceAdviser.WillRuleCheckElements) when parameters.Length == 1 &&
                                                                  parameters[0].ParameterType == typeof(int) => ResolveWillRuleCheckElements,
            nameof(PerformanceAdviser.GetElementFilterFromRule) when parameters.Length == 2 &&
                                                                     parameters[0].ParameterType == typeof(int) => ResolveGetElementFilterFromRule,
            _ => null
        };

        IVariants ResolveGetElementFilterFromRule()
        {
            var rules = adviser.GetNumberOfRules();
            var variants = new Variants<KeyValuePair<int, ElementFilter>>(rules);
            for (var i = 0; i < rules; i++) variants.Add(new KeyValuePair<int, ElementFilter>(i, adviser.GetElementFilterFromRule(i, Context.ActiveDocument)));
            return variants;
        }

        IVariants ResolveWillRuleCheckElements()
        {
            var rules = adviser.GetNumberOfRules();
            var variants = new Variants<KeyValuePair<int, bool>>(rules);
            for (var i = 0; i < rules; i++) variants.Add(new KeyValuePair<int, bool>(i, adviser.WillRuleCheckElements(i)));
            return variants;
        }

        IVariants ResolveIsRuleEnabled()
        {
            var rules = adviser.GetNumberOfRules();
            var variants = new Variants<KeyValuePair<int, bool>>(rules);
            for (var i = 0; i < rules; i++) variants.Add(new KeyValuePair<int, bool>(i, adviser.IsRuleEnabled(i)));
            return variants;
        }

        IVariants ResolveGetRuleName()
        {
            var rules = adviser.GetNumberOfRules();
            var variants = new Variants<KeyValuePair<int, string>>(rules);
            for (var i = 0; i < rules; i++) variants.Add(new KeyValuePair<int, string>(i, adviser.GetRuleName(i)));
            return variants;
        }

        IVariants ResolveGetRuleId()
        {
            var rules = adviser.GetNumberOfRules();
            var variants = new Variants<KeyValuePair<int, PerformanceAdviserRuleId>>(rules);
            for (var i = 0; i < rules; i++) variants.Add(new KeyValuePair<int, PerformanceAdviserRuleId>(i, adviser.GetRuleId(i)));
            return variants;
        }

        IVariants ResolveGetRuleDescription()
        {
            var rules = adviser.GetNumberOfRules();
            var variants = new Variants<KeyValuePair<int, string>>(rules);
            for (var i = 0; i < rules; i++) variants.Add(new KeyValuePair<int, string>(i, adviser.GetRuleDescription(i)));
            return variants;
        }

        IVariants ResolveExecuteAllRules()
        {
            return Variants.Single(adviser.ExecuteAllRules(Context.ActiveDocument));
        }
    }
}