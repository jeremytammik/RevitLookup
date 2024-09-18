sealed partial class Build
{
    const string Version = "1.0.1";
    readonly AbsolutePath ArtifactsDirectory = RootDirectory / "output";

    protected override void OnBuildInitialized()
    {
        Configurations =
        [
            "Release*",
            "Installer*"
        ];

        InstallersMap = new()
        {
            { Solution.Build.Installer, Solution.AssembliesReport }
        };
    }
}