using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WixSharp;
using WixSharp.CommonTasks;
using WixSharp.Controls;

const string installationDir = @"%AppDataFolder%\Autodesk\Revit\Addins\";
const string projectName = "RevitLookup";
const string outputName = "RevitLookup";
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
var fileName = new StringBuilder().Append(outputName).Append("-").Append(dllVersion);

var project = new Project
{
    Name = projectName,
    OutDir = outputDir,
    Platform = Platform.x64,
    UI = WUI.WixUI_InstallDir,
    Version = new Version(version),
    OutFileName = fileName.ToString(),
    InstallScope = InstallScope.perUser,
    MajorUpgrade = MajorUpgrade.Default,
    GUID = new Guid(guidMap[revitVersion]),
    BackgroundImage = @"Installer\Resources\Icons\BackgroundImage.png",
    BannerImage = @"Installer\Resources\Icons\BannerImage.png",
    ControlPanelInfo =
    {
        Manufacturer = "Autodesk",
        HelpLink = "https://github.com/jeremytammik/RevitLookup/issues",
        ProductIcon = @"Installer\Resources\Icons\ShellIcon.ico"
    },
    Dirs = new Dir[]
    {
        new InstallDir(installationDir, GenerateWixEntities())
    }
};

project.RemoveDialogsBetween(NativeDialogs.WelcomeDlg, NativeDialogs.InstallDirDlg);
project.BuildMsi();

WixEntity[] GenerateWixEntities()
{
    var versionRegex = new Regex(@"\d+.*$");
    var versionStorages = new Dictionary<string, List<WixEntity>>();

    foreach (var directory in args)
    {
        var directoryInfo = new DirectoryInfo(directory);
        var fileVersion = versionRegex.Match(directoryInfo.Name).Value;
        var files = new Files($@"{directory}\*.*");
        if (versionStorages.ContainsKey(fileVersion))
            versionStorages[fileVersion].Add(files);
        else
            versionStorages.Add(fileVersion, new List<WixEntity> {files});

        var assemblies = Directory.GetFiles(directory, "*", SearchOption.AllDirectories);
        Console.WriteLine($"Added '{fileVersion}' version files: ");
        foreach (var assembly in assemblies) Console.WriteLine($"'{assembly}'");
    }

    return versionStorages.Select(storage => new Dir(storage.Key, storage.Value.ToArray())).Cast<WixEntity>().ToArray();
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