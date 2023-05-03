using Nuke.Common.IO;

partial class Build
{
    readonly AbsolutePath ArtifactsDirectory = RootDirectory / "output";
    readonly AbsolutePath ChangeLogPath = RootDirectory / "Changelog.md";

    protected override void OnBuildCreated()
    {
        Configurations = new[]
        {
            "Release*",
            "Installer*"
        };

        InstallersMap = new()
        {
            {Solution.Installer, Solution.RevitLookup}
        };

        VersionMap = new()
        {
            {"Release R21", "2021.2.5"},
            {"Release R22", "2022.2.5"},
            {"Release R23", "2023.2.5"},
            {"Release R24", "2024.0.5"}
        };
    }
}