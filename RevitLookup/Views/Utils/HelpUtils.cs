﻿// Copyright 2003-2023 by Autodesk, Inc.
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

using System.Diagnostics;

namespace RevitLookup.Views.Utils;

public static class HelpUtils
{
    public static void ShowHelp(string query)
    {
        string uri;

        if (query.Contains(' '))
        {
            uri = $"https://duckduckgo.com/?q={query}";
        }
        else if (query.StartsWith("System"))
        {
            query = query.Replace('`', '-');
            uri = $"https://docs.microsoft.com/en-us/dotnet/api/{query}";
        }
        else
        {
            uri = $"https://duckduckgo.com/?q={query}";
        }

        Process.Start(uri);
    }

    public static void ShowHelp(string query, string parameter)
    {
        if (query.StartsWith("System"))
        {
            ShowHelp($"{query}.{parameter}");
        }
        else ShowHelp($"{query} {parameter}");
    }
}