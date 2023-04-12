partial class Build
{
    const string InstallerProject = "Installer";

    public const string BuildConfiguration = "Release";
    public const string InstallerConfiguration = "Installer";

    const string AddInBinPrefix = "AddIn";
    const string ArtifactsFolder = "output";

    //Specify the path to the MSBuild.exe file here if you are not using VisualStudio
    const string CustomMsBuildPath = @"C:\Program Files\JetBrains\JetBrains Rider\tools\MSBuild\Current\Bin\MSBuild.exe";

    readonly string[] Projects =
    {
        "RevitLookup"
    };
    
    readonly Dictionary<string, string> VersionMap = new()
    {
        {"Release R22", "2022.1.0"},
        {"Release R23", "2023.1.0"}
    };
}