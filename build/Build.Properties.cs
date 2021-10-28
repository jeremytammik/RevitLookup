partial class Build
{
    readonly string[] Projects =
    {
        "RevitLookup"
    };
    
    const string InstallerProject = "Installer";

    public const string BuildConfiguration = "Release";
    public const string InstallerConfiguration = "Installer";

    const string AddInBinPrefix = "AddIn";
    const string ArtifactsFolder = "output";

    //The libraries below use the AfterBuild target
    //Change the version here if it is different from the one specified in the .csproj file
    const string WixTargetPath = @"%USERPROFILE%\.nuget\packages\wixsharp\1.18.1\build\WixSharp.targets";

    //Specify the path to the MSBuild.exe file here if you are not using VisualStudio
    const string CustomMsBuildPath = @"C:\Program Files\JetBrains\JetBrains Rider\tools\MSBuild\Current\Bin\MSBuild.exe";

    //The Wix installer is created in a different thread
    //Increase this value if your installer does not have time to build
    readonly int InstallerCreationTime = IsServerBuild ? 15 : 7;
}