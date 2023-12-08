﻿using Nuke.Common.IO;

sealed partial class Build
{
    readonly AbsolutePath ArtifactsDirectory = RootDirectory / "output";
    readonly AbsolutePath ChangeLogPath = RootDirectory / "Changelog.md";

    protected override void OnBuildInitialized()
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
            {"Release R21", "2021.2.10"},
            {"Release R22", "2022.2.10"},
            {"Release R23", "2023.2.10"},
            {"Release R24", "2024.0.10"}
        };
    }
}