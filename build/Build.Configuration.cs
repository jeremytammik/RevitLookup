sealed partial class Build
{
    readonly AbsolutePath ArtifactsDirectory = RootDirectory / "output";
    readonly AbsolutePath ChangeLogPath = RootDirectory / "Changelog.md";

    protected override void OnBuildInitialized()
    {
        Configurations =
        [
            "Release*",
            "Installer*"
        ];

        InstallersMap = new()
        {
            { Solution.Installer, Solution.RevitLookup }
        };

        VersionMap = new()
        {
            { "Release R21", "2021.3.6" },
            { "Release R22", "2022.3.6" },
            { "Release R23", "2023.3.6" },
            { "Release R24", "2024.1.6" },
            { "Release R25", "2025.0.6" }
        };
    }
}