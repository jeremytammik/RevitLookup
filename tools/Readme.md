# Revit tools

Contains utilities for Revit:

- **AssembliesReport** - tool designed for Revit, creates a report on the dependencies used. Aimed at finding conflicts between plugins. 
    To run it, go to Revit → Add-ins tab → External tools → AssembliesReport

- **DependenciesReport** - tool for static analysis of dependency conflicts between plugins.
    Displays a conflict summary and optionally updates dependencies to the latest version. 
    It works without starting Revit.

### Technologies Used

* C# 13
* .NET Framework 4.8
* .NET 8
* Desktop Development for C++ workload

### Getting Started

Before you can build this project, you will need to install .NET, depending upon the solution file you are building. If you haven't already installed these
frameworks, you can do so by visiting the following:

* [.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48)
* [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet)

After installing the necessary frameworks, clone this repository to your local machine and navigate to the project directory.

### Building

We recommend JetBrains Rider as preferred IDE, since it has outstanding .NET support. If you don't have Rider installed, you can download it
from [here](https://www.jetbrains.com/rider/).

1. Open JetBrains Rider
2. Click on `File -> Open` and choose the RevitLookup.sln file to open.
3. In the `Solutions Configuration` drop-down menu, select `Release R25` or `Debug R25`. Suffix `R25` means compiling for the Revit 2025.
4. After the solution loads, you can build it by clicking on `Build -> Build Solution`.

Also, you can use Visual Studio. If you don't have Visual Studio installed, download it from [here](https://visualstudio.microsoft.com/downloads/).

1. Open Visual Studio
2. Click on `File -> Open -> Project/Solution` and locate your solution file to open.
3. In the `Solutions Configuration` drop-down menu, select `Release R25` or `Debug R25`. Suffix `R25` means compiling for the Revit 2025.
4. After the solution loads, you can build it by clicking on `Build -> Build Solution`.

### Publish on the local machine

1. **Install NUKE as a global tool**. First, make sure you have NUKE installed as a global tool. You can install it using dotnet CLI:

    ```powershell
    dotnet tool install Nuke.GlobalTool --global
    ```

   You only need to do this once on your machine.

2. **Navigate to your project directory**. Open a terminal / command prompt and navigate to your project's root directory.
3. **Run the build**. Once you have navigated to your project's root directory, you can run the NUKE build by calling:

   Create an installer for Revit:
   ```powershell
   nuke createinstaller
   ```

   Create executable tools:
   ```powershell
   nuke publish
   ```

   This command will execute the NUKE build defined in your project.

### Solution structure

| Folder  | Description                                                       |
|---------|-------------------------------------------------------------------|
| build   | Nuke build system. Used to automate project builds                |
| install | Add-in installer, called implicitly by the Nuke build             |
| source  | Project source code folder. Contains all solution projects        |
| output  | Folder of generated files by the build system, such as installers |