using Nuke.Common.IO;

partial class Build
{
    readonly AbsolutePath ChangeLogPath = RootDirectory / "Changelog.md";
    readonly AbsolutePath ArtifactsDirectory = RootDirectory / "output";

    readonly string[] Configurations =
    {
        "Release*",
        "Installer"
    };

    readonly Dictionary<string, string> VersionMap = new()
    {
        {"Release R21", "2021.2.4"},
        {"Release R22", "2022.2.4"},
        {"Release R23", "2023.2.4"},
        {"Release R24", "2024.0.4"}
    };
}