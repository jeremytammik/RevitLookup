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

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Installer;

[StructLayout(LayoutKind.Auto)]
public struct Versions
{
    public string InstallerVersion { get; set; }
    public string AssemblyVersion { get; set; }
    public string RevitVersion { get; set; }
}

public static class Tools
{
    public static Versions ComputeVersions(string[] args)
    {
        var versions = new Versions();
        foreach (var directory in args)
        {
            var assemblies = Directory.GetFiles(directory, @"RevitLookup.dll", SearchOption.AllDirectories);
            if (assemblies.Length == 0) continue;
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assemblies[0]);
            var versionGroups = fileVersionInfo.ProductVersion.Split('.');
            if (versionGroups.Length > 3) Array.Resize(ref versionGroups, 3);
            var majorVersion = versionGroups[0];
            if (int.Parse(majorVersion) > 255) versionGroups[0] = majorVersion.Substring(majorVersion.Length - 2);

            versions.RevitVersion = majorVersion;
            versions.InstallerVersion = string.Join(".", versionGroups);
            versions.AssemblyVersion = fileVersionInfo.ProductVersion;
            return versions;
        }

        throw new Exception("RevitLookup.dll file could not be found");
    }
}