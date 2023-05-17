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
    public Version InstallerVersion { get; set; }
    public Version AssemblyVersion { get; set; }
    public int RevitVersion { get; set; }
}

public static class Tools
{
    public static Versions ComputeVersions(string[] args)
    {
        foreach (var directory in args)
        {
            var assemblies = Directory.GetFiles(directory, @"RevitLookup.dll", SearchOption.AllDirectories);
            if (assemblies.Length == 0) continue;

            var version = new Version(FileVersionInfo.GetVersionInfo(assemblies[0]).ProductVersion);
            return new Versions
            {
                AssemblyVersion = version,
                RevitVersion = version.Major,
                InstallerVersion = version.Major > 255 ? new Version(version.Major % 100, version.Minor, version.Build) : version
            };
        }

        throw new Exception("RevitLookup.dll file could not be found");
    }
}