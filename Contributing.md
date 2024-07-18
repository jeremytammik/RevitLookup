## Table of contents

<!-- TOC -->
  * [Fork, Clone, Branch and Create your PR](#fork-clone-branch-and-create-your-pr)
  * [Rules](#rules)
  * [Compiling RevitLookup](#compiling-revitlookup)
    * [Prerequisites for Compiling RevitLookup](#prerequisites-for-compiling-revitlookup)
    * [Install .Net versions](#install-net-versions)
    * [Compiling Source Code](#compiling-source-code)
    * [Creating MSI installer on a local machine](#creating-msi-installer-on-a-local-machine)
    * [Creating a new release on GitHub](#creating-a-new-release-on-github)
  * [Solution structure](#solution-structure)
  * [Project structure](#project-structure)
  * [Architecture](#architecture)
    * [IDescriptorCollector](#idescriptorcollector)
    * [IDescriptorResolver](#idescriptorresolver)
      * [Resolution with only one variant](#resolution-with-only-one-variant)
      * [Resolution with multiple values](#resolution-with-multiple-values)
      * [Resolution without variants](#resolution-without-variants)
      * [Disabling methods](#disabling-methods)
    * [IDescriptorExtension](#idescriptorextension)
    * [IDescriptorRedirection](#idescriptorredirection)
    * [IDescriptorConnector](#idescriptorconnector)
    * [Styles](#styles)
<!-- TOC -->

## Fork, Clone, Branch and Create your PR

1. Fork the repo if you haven't already
2. Clone your fork locally
3. Create & push a feature branch
4. Create a [Draft Pull Request (PR)](https://github.blog/2019-02-14-introducing-draft-pull-requests/)
5. Work on your changes

## Rules

- **Follow the pattern of what you already see in the code**
- When adding new classes/methods/changing existing code: run the debugger and make sure everything works
- The naming should be descriptive and direct, giving a clear idea of the functionality and usefulness in the future

## Compiling RevitLookup

### Prerequisites for Compiling RevitLookup

- Windows 10 April 2018 Update (version 1803) or newer
- Visual Studio 2022 / JetBrains Rider 2023.3 or newer
- A local clone of the RevitLookup repository

### Install .Net versions

Before you can build this project, you will need to install .NET, depending upon the solution file you are building. 
If you haven't already installed these frameworks, you can do so by visiting the following:

* [.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48)
* [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet)

### Compiling Source Code

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

### Creating MSI installer on a local machine

To build the RevitLookup for all versions and create the installer, use [NUKE](https://github.com/nuke-build/nuke)

To execute your NUKE build locally, you can follow these steps:

1. **Install NUKE as a global tool**. First, make sure you have NUKE installed as a global tool. You can install it using dotnet CLI:

    ```powershell
    dotnet tool install Nuke.GlobalTool --global
    ```

   You only need to do this once on your machine.

2. **Navigate to the solution directory**. Open a terminal / command prompt and navigate to RevitLookup root directory.
3. **Run the build**. Once you have navigated to your solution directory, you can run the NUKE build by calling:

   Compile:
   ```powershell
   nuke
   ```

   Create an installer:
   ```powershell
   nuke createinstaller
   ```
   
   This command will execute the NUKE build, defined in the RevitLookup project.

### Creating a new release on GitHub

Publishing the release, generating the installer, is performed automatically on GitHub.

To execute NUKE build on GitHub, you can follow these steps:

1. Merge all your commits into the `main` / `master` branch.
2. Navigate to the `Build/Build.Configuration.cs` file.
3. Increase the `Version` value.
4. Make a commit.
5. Push your changes to GitHub, everything will happen automatically, and you can follow the progress in the Actions section of the repository page.

## Solution structure

| Folder   | Description                                                                |
|----------|----------------------------------------------------------------------------|
| build    | Nuke build system. Used to automate project builds                         |
| install  | Add-in installer, called implicitly by the Nuke build                      |
| source   | Project source code folder. Contains all solution projects                 |
| output   | Folder of generated files by the build system, such as bundles, installers |
| branding | Source files for logo, banner, installer background                        |
| doc      | Museum, storage of original RevitLookup documentation                      |

## Project structure

| Folder     | Description                                                                                                                                                                                          |
|------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Commands   | External commands invoked from the Revit ribbon                                                                                                                                                      |
| Config     | Application settings, configurations files                                                                                                                                                           |
| Core       | Contains a main application logic                                                                                                                                                                    |
| Services   | Services and components providing the application functionality                                                                                                                                      |
| Models     | Classes that encapsulate the app's data, include data transfer objects (DTOs). More [details](https://learn.microsoft.com/en-us/dotnet/architecture/maui/mvvm).                                      |
| ViewModels | Classes that implement properties and commands to which the view can bind data. More [details](https://learn.microsoft.com/en-us/dotnet/architecture/maui/mvvm).                                     |
| Views      | Classes that are responsible for defining the structure, layout and appearance of what the user sees on the screen. More [details](https://learn.microsoft.com/en-us/dotnet/architecture/maui/mvvm). |
| Resources  | Images, localisation files, etc.                                                                                                                                                                     |
| Utils      | Utilities, extensions, helpers used across the application                                                                                                                                           |

## Architecture

Descriptors and interfaces are used to extend functionality in the project. They are located in the `RevitLookup/Core/ComponentModel` path.

The Descriptors directory contains descriptors that describe exactly how the program should handle types and what data to show the user.

The DescriptorMap file is responsible for mapping a descriptor to a type. The map is searched both roughly, for displaying to the user, and precisely by type, for the work of
adding extensions and additional functionality to a particular type.

To add descriptors for new classes, you must add a new file and update the DescriptorMap.

Interfaces are responsible for extending functionality:

### IDescriptorCollector

Indicates that the descriptor can retrieve object members by reflection.
If you add this interface, the user can click on the object and analyze its members.

```c#
public sealed class ApplicationDescriptor : Descriptor, IDescriptorCollector
{
    public ApplicationDescriptor(Autodesk.Revit.ApplicationServices.Application application)
    {
        Name = application.VersionName;
    }
}
```

### IDescriptorResolver

Indicates that the descriptor can decide to call methods/properties with parameters or override their values.

#### Resolution with only one variant

To resolve member with only one variant, or you want to disable some method, use the `Variants.Single()`:

```c#
public sealed class DocumentDescriptor : Descriptor, IDescriptorResolver
{
    public Func<IVariants> Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(Document.PlanTopologies) => ResolvePlanTopologies,
            _ => null
        };
        
        IVariants ResolvePlanTopologies()
        {
            return Variants.Single(_document.PlanTopologies);
        }
    }
}
```

#### Resolution with multiple values

To resolve member with different input parameters, create a new Variants collection and specify variant count `new Variants<double>(count)`:

```c#
public sealed class PlanViewRangeDescriptor : Descriptor, IDescriptorResolver
{
    public ResolveSet Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(PlanViewRange.GetOffset) => ResolveGetOffset,
            nameof(PlanViewRange.GetLevelId) => ResolveGetLevelId,
            _ => null
        };
        
        IVariants ResolveGetOffset()
        {
            return new Variants<double>(2)
                .Add(viewRange.GetOffset(PlanViewPlane.TopClipPlane), "Top clip plane")
                .Add(viewRange.GetOffset(PlanViewPlane.CutPlane), "Cut plane")
        }
        
        IVariants ResolveGetLevelId()
        {
            return new Variants<ElementId>(2)
                .Add(viewRange.GetLevelId(PlanViewPlane.TopClipPlane), "Top clip plane")
                .Add(viewRange.GetLevelId(PlanViewPlane.CutPlane), "Cut plane")
        }
    }
}
```

#### Resolution without variants

If your member is not resolved, use the `Variants.Empty()` method. For example, you want to disable Enumerator call but want to display this member:

```c#
public sealed class UiElementDescriptor : Descriptor, IDescriptorResolver
{
    public Func<IVariants> Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(UIElement.GetLocalValueEnumerator) => ResolveGetLocalValueEnumerator,
            _ => null
        };
        
        IVariants ResolveGetLocalValueEnumerator()
        {
            return Variants.Empty<LocalValueEnumerator>();
        }
    }
}
```

In another situation you have nothing to return by the condition, use the `Variants.Empty()` as well:

```c#
public sealed class DocumentDescriptor : Descriptor, IDescriptorResolver
{
    public Func<IVariants> Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(Document.PlanTopologies) when parameters.Length == 0 => ResolvePlanTopologies,
            _ => null
        };
        
        IVariants ResolvePlanTopologies()
        {
            if (_document.IsReadOnly) return Variants.Empty<PlanTopologySet>();
            
            return Variants.Single(_document.PlanTopologies);
        }
    }
}
```

#### Disabling methods

If you want to disable some method, use `Variants.Disabled` property:

```c#
public class UiElementDescriptor : Descriptor, IDescriptorResolver
{
    public ResolveSet Resolve(Document context, string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(UIElement.Focus) => Variants.Disabled,
            _ => null
        };
    }
}
```

### IDescriptorExtension

Indicates that additional members can be added to the descriptor.

Adding a new `HEX()` method for the `Color` class:

```c#
public void RegisterExtensions(IExtensionManager manager)
{
    manager.Register("HEX", context => ColorRepresentationUtils.ColorToHex(_color.GetDrawingColor()));
    manager.Register("RGB", context => ColorRepresentationUtils.ColorToRgb(_color.GetDrawingColor()));
    manager.Register("HSL", context => ColorRepresentationUtils.ColorToHsl(_color.GetDrawingColor()));
    manager.Register("HSV", context => ColorRepresentationUtils.ColorToHsv(_color.GetDrawingColor()));
    manager.Register("CMYK", context => ColorRepresentationUtils.ColorToCmyk(_color.GetDrawingColor()));
}
```

### IDescriptorRedirection

Indicates that the object can be redirected to another.

Redirect from `ElementId` to `Element` if Element itself exists:

```c#
public sealed class ElementIdDescriptor : Descriptor, IDescriptorRedirection
{
    public bool TryRedirect(Document context, string target, out object output)
    {
        output = _elementId.ToElement(context);
        if (element is null) return false;

        return true;
    }
}
```

### IDescriptorConnector

Indicates that the descriptor can interact with the UI and execute commands.

Adding an option for the context menu:

```c#
public sealed class ElementDescriptor : Descriptor, IDescriptorConnector
{
    public void RegisterMenu(ContextMenu contextMenu)
    {
        contextMenu.AddMenuItem()
            .SetHeader("Show element")
            .SetAvailability(_element is not ElementType)
            .SetCommand(_element, element =>
            {
                Context.UiDocument.ShowElements(element);
                Context.UiDocument.Selection.SetElementIds(new List<ElementId>(1) {element.Id});
            })
            .AddShortcut(ModifierKeys.Alt, Key.F7);
    }
}
```

### Styles

The application UI is divided into templates, where each template can be customized for different types of data.
There are several different rules for customizing TreeView, DataGrid row, DataGrid cell and they are all located in the
file `RevitLookup/Views/Pages/Abstraction/SnoopViewBase.Styles.cs`.

Use the DataTemplate `x:Key` property to search for `templateName` inside the switch block:

```C#
public override DataTemplate SelectTemplate(object item, DependencyObject container)
{
    if (item is null) return null;

    var descriptor = (Descriptor) item;
    var presenter = (FrameworkElement) container;
    var templateName = descriptor.Value.Descriptor switch
    {
        ColorDescriptor => "DataGridColorCellTemplate",
        ColorMediaDescriptor => "DataGridColorCellTemplate",
        _ => "DefaultLookupDataGridCellTemplate"
    };

    return (DataTemplate) presenter.FindResource(templateName);
}
```

The templates themselves are located in the `RevitLookup/Views/Controls` folder.
For example, in the `RevitLookup/Views/Controls/DataGrid/DataGridCellTemplate.xaml` file there is a cell template that displays the text:

```xaml
<DataTemplate
    x:Key="DefaultLookupDataGridCellTemplate">
    <TextBlock
        d:DataContext="{d:DesignInstance objects:Descriptor}"
        Text="{Binding Value.Descriptor,
            Converter={converters:CombinedDescriptorConverter},
            Mode=OneTime}" />
</DataTemplate>
```

References to additional files must be registered in `RevitLookup/Views/Resources/RevitLookup.Ui.xaml`.