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

var version = GetAssemblyVersion(out var dllVersion);
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
    GUID = new Guid("2179ECCB-0ED3-4FFF-907D-01C9D57AD20D"),
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

MajorUpgrade.Default.AllowSameVersionUpgrades = true;
project.RemoveDialogsBetween(NativeDialogs.WelcomeDlg, NativeDialogs.InstallDirDlg);
project.BuildMsi();

WixEntity[] GenerateWixEntities()
{
    var versionRegex = new Regex(@"\d+");
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

string GetAssemblyVersion(out string originalVersion)
{
    foreach (var directory in args)
    {
        var assemblies = Directory.GetFiles(directory, @"RevitLookup.dll", SearchOption.AllDirectories);
        if (assemblies.Length == 0) continue;
        var fileVersionInfo = FileVersionInfo.GetVersionInfo(assemblies[0]);
        var versionGroups = fileVersionInfo.ProductVersion.Split('.');
        var majorVersion = versionGroups[0];
        if (int.Parse(majorVersion) > 255) versionGroups[0] = majorVersion.Substring(majorVersion.Length - 2);
        originalVersion = fileVersionInfo.ProductVersion;
        var wixVersion = string.Join(".", versionGroups);
        if (!originalVersion.Equals(wixVersion)) Console.WriteLine($"Installer version trimmed from '{originalVersion}' to '{wixVersion}'");
        return wixVersion;
    }

    throw new Exception("Cant find RevitLookup.dll file");
}