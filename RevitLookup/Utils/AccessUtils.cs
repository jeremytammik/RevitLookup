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

using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace RevitLookup.Utils;

public static class AccessUtils
{
    public static bool CheckWriteAccess(string path)
    {
        var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        var accessControl = new DirectoryInfo(path).GetAccessControl();
        var accessRules = accessControl.GetAccessRules(true, true, typeof(NTAccount));
        var writeAccess = false;
        foreach (FileSystemAccessRule rule in accessRules)
        {
            if (principal.IsInRole(rule.IdentityReference.Value) && (rule.FileSystemRights & FileSystemRights.WriteData) == FileSystemRights.WriteData)
            {
                writeAccess = true;
                break;
            }
        }

        return writeAccess;
    }
}