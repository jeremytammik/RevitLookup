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

namespace Installer
{
    /// <summary>
    /// Documentation:
    /// https://github.com/Nice3point/RevitTemplates/wiki
    /// </summary>
    public static class Installer
    {
        private const string InstallationDir = @"%AppDataFolder%\Autodesk\Revit\Addins\";
        private const string ProjectName = "RevitLookup";
        private const string OutputName = "RevitLookup";
        private const string OutputDir = "output";

        public static void Main(string[] args)
        {
            var version = GetAssemblyVersion(args);
            var outFileNameBuilder = new StringBuilder().Append(OutputName).Append("-").Append("20").Append(version);
            //Additional suffixes for unique configurations add here
            var outFileName = outFileNameBuilder.ToString();

            var project = new Project
            {
                Name = ProjectName,
                OutDir = OutputDir,
                OutFileName = outFileName,
                Platform = Platform.x64,
                Version = new Version(version),
                InstallScope = InstallScope.perUser,
                MajorUpgrade = MajorUpgrade.Default,
                UI = WUI.WixUI_InstallDir,
                GUID = new Guid("2179ECCB-0ED3-4FFF-907D-01C9D57AD20D"),
                BackgroundImage = @"Installer\Resources\Icons\BackgroundImage.png",
                BannerImage = @"Installer\Resources\Icons\BannerImage.png",
                ControlPanelInfo =
                {
                    Manufacturer = "Jeremy Tammik",
                    ProductIcon = @"Installer\Resources\Icons\ShellIcon.ico"
                },
                Dirs = new Dir[]
                {
                    new InstallDir(InstallationDir, GetOutputFolders(args))
                }
            };

            MajorUpgrade.Default.AllowSameVersionUpgrades = true;
            project.RemoveDialogsBetween(NativeDialogs.WelcomeDlg, NativeDialogs.InstallDirDlg);
            project.BuildMsi();
        }

        private static WixEntity[] GetOutputFolders(string[] directories)
        {
            var versionRegex = new Regex(@"\d+");
            var versionStorages = new Dictionary<string, List<WixEntity>>();

            foreach (var directory in directories)
            {
                var directoryInfo = new DirectoryInfo(directory);
                var version = versionRegex.Match(directoryInfo.Name).Value;
                var files = new Files($@"{directory}\*.*");
                if (versionStorages.ContainsKey(version))
                    versionStorages[version].Add(files);
                else
                    versionStorages.Add(version, new List<WixEntity> {files});

                var assemblies = Directory.GetFiles(directory, "*", SearchOption.AllDirectories);
                foreach (var assembly in assemblies)
                {
                    Console.WriteLine($"Added {version} version file: {assembly}");
                }
            }

            return versionStorages.Select(storage => new Dir(storage.Key, storage.Value.ToArray())).Cast<WixEntity>().ToArray();
        }

        private static string GetAssemblyVersion(string[] directories)
        {
            foreach (var directory in directories)
            {
                var assemblies = Directory.GetFiles(directory, @"RevitLookup.dll", SearchOption.AllDirectories);
                if (assemblies.Length == 0) continue;
                var fileVersionInfo = FileVersionInfo.GetVersionInfo(assemblies[0]);
                return fileVersionInfo.ProductVersion;
            }

            throw new Exception("Cant find RevitLookup.dll file");
        }
    }
}