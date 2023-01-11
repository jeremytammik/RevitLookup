using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using WixSharp;
using WixSharp.CommonTasks;
using WixSharp.Controls;

const string projectName = "RevitLookup";
const string outputDir = "output";

var guidMap = new Dictionary<string, string>
{
    {"2015", "1C877362-19E8-4E10-A4B0-802BA88C1F3E"},
    {"2016", "230933BA-3865-41C8-80E1-CFA0EB08B44B"},
    {"2017", "A2402F72-90B0-4803-B783-06487D6BFBEB"},
    {"2018", "2997B545-391C-41B2-A90A-E5C6BB39087A"},
    {"2019", "DBF3C66B-B624-4E0B-B86D-44857B19CD0C"},
    {"2020", "36D21BA1-C945-4D40-83B9-4C2518FC40EA"},
    {"2021", "9B7FD05D-C782-4538-A5D3-04B64AE81FE4"},
    {"2022", "207733B1-1BEA-4603-99EA-EA1E87077F60"},
    {"2023", "2179ECCB-0ED3-4FFF-907D-01C9D57AD20D"}
};

var version = GetAssemblyVersion(out var dllVersion, out var revitVersion);
var fileName = $"{projectName}-{dllVersion}";

var wixEntities = GenerateWixEntities();
var project = new Project
{
    Name = $"{projectName} {dllVersion}",
    OutDir = outputDir,
    Platform = Platform.x64,
    UI = WUI.WixUI_InstallDir,
    Version = new Version(version),
    MajorUpgrade = MajorUpgrade.Default,
    GUID = new Guid(guidMap[revitVersion]),
    BackgroundImage = @"Installer\Resources\Icons\BackgroundImage.png",
    BannerImage = @"Installer\Resources\Icons\BannerImage.png",
    ControlPanelInfo =
    {
        Manufacturer = "Autodesk",
        HelpLink = "https://github.com/jeremytammik/RevitLookup/issues",
        ProductIcon = @"Installer\Resources\Icons\ShellIcon.ico"
    }
};

project.RemoveDialogsBetween(NativeDialogs.WelcomeDlg, NativeDialogs.InstallDirDlg);
BuildSingleUserMsi();
BuildMultiUserUserMsi();

// User Addins:
// %appdata%\Autodesk\Revit\Addins\
// %appdata%\Autodesk\ApplicationPlugins\
//
// Machine Addins (for all users of the machine):
// C:\ProgramData\Autodesk\Revit\Addins\
//
// Addins packaged for the Autodesk Exchange store:
// C:\ProgramData\Autodesk\ApplicationPlugins\
//
// Autodesk servers and services:
// C:\Program Files\Autodesk\Revit 2023\AddIns\

void BuildSingleUserMsi()
{
    project.InstallScope = InstallScope.perUser;
    project.OutFileName = $"{fileName}-SingleUser";
    project.Dirs = new Dir[]
    {
        new InstallDir($@"%AppDataFolder%\Autodesk\Revit\Addins\{revitVersion}", wixEntities)
    };
    project.BuildMsi();
}

void BuildMultiUserUserMsi()
{
    project.InstallScope = InstallScope.perMachine;
    project.OutFileName = $"{fileName}-MultiUser";
    project.Dirs = new Dir[]
    {
        new InstallDir($@"%CommonAppDataFolder%\Autodesk\Revit\Addins\{revitVersion}", wixEntities)
    };
    project.BuildMsi();
}

WixEntity[] GenerateWixEntities()
{
    var entities = new List<WixEntity>();
    var versionRegex = new Regex(@"\d+.*$");
    foreach (var directory in args)
    {
        var queue = new Queue<string>();
        var fileVersion = versionRegex.Match(Path.GetFileName(directory)).Value;
        queue.Enqueue(directory);

        Console.WriteLine($"Added '{fileVersion}' version files: ");
        while (queue.Count > 0)
        {
            var currentPath = queue.Dequeue();
            if (currentPath == directory)
            {
                foreach (var file in Directory.GetFiles(currentPath))
                    entities.Add(new WixSharp.File(file));
            }
            else
            {
                var currentFolder = Path.GetFileName(currentPath);
                var currentDir = new Dir(currentFolder);
                entities.Add(currentDir);

                foreach (var file in Directory.GetFiles(currentPath))
                    currentDir.AddFile(new WixSharp.File(file));
            }
            foreach (var subfolder in Directory.GetDirectories(currentPath))
                queue.Enqueue(subfolder);
        }
    }

    return entities.ToArray();
}

string GetAssemblyVersion(out string originalVersion, out string majorVersion)
{
    foreach (var directory in args)
    {
        var assemblies = Directory.GetFiles(directory, @"RevitLookup.dll", SearchOption.AllDirectories);
        if (assemblies.Length == 0) continue;
        var fileVersionInfo = FileVersionInfo.GetVersionInfo(assemblies[0]);
        var versionGroups = fileVersionInfo.ProductVersion.Split('.');
        if (versionGroups.Length > 3) Array.Resize(ref versionGroups, 3);
        majorVersion = versionGroups[0];
        if (int.Parse(majorVersion) > 255) versionGroups[0] = majorVersion.Substring(majorVersion.Length - 2);
        originalVersion = fileVersionInfo.ProductVersion;
        var wixVersion = string.Join(".", versionGroups);
        if (!originalVersion.Equals(wixVersion)) Console.WriteLine($"Installer version trimmed from '{originalVersion}' to '{wixVersion}'");
        return wixVersion;
    }

    throw new Exception("Cant find RevitLookup.dll file");
}