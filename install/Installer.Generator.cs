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
using System.Collections.Generic;
using System.IO;
using WixSharp;
using WixSharp.CommonTasks;
using File = WixSharp.File;

namespace Installer;

public static class Generator
{
    public static WixEntity[] GenerateWixEntities(string[] args, Version version)
    {
        var entities = new List<WixEntity>();
        foreach (var directory in args)
        {
            Console.WriteLine($"Installer files for version '{version}':");
            ProcessDirectory(directory, entities);
        }

        return entities.ToArray();
    }

    private static void ProcessDirectory(string directory, List<WixEntity> entities)
    {
        foreach (var file in Directory.GetFiles(directory))
        {
            Console.WriteLine($"'{file}'");
            entities.Add(new File(file));
        }

        foreach (var folder in Directory.GetDirectories(directory))
        {
            var folderName = Path.GetFileName(folder);
            var entity = new Dir(folderName);
            entities.Add(entity);

            ProcessDirectoryFiles(folder, entity);
        }
    }

    private static void ProcessDirectoryFiles(string directory, Dir parent)
    {
        foreach (var file in Directory.GetFiles(directory))
        {
            Console.WriteLine($"'{file}'");
            parent.AddFile(new File(file));
        }

        foreach (var subfolder in Directory.GetDirectories(directory))
        {
            var folderName = Path.GetFileName(subfolder);
            var entity = new Dir(folderName);
            parent.AddDir(entity);

            ProcessDirectoryFiles(subfolder, entity);
        }
    }
}