## Fork, Clone, Branch and Create your PR

1. Fork the repo if you haven't already
2. Clone your fork locally
3. Create & push a feature branch
4. Create a [Draft Pull Request (PR)](https://github.blog/2019-02-14-introducing-draft-pull-requests/)
5. Work on your changes

Please avoid:

- Lots of unrelated changes in one commit
- Modifying files that are not directly related to the feature you implement

## Install addin from source

- Clone this repository
  - Or, fork and clone your fork if you plan to contribute
- Ensure your development environment meets the
  [prerequisites](#prerequisites-for-compiling-revitlookup) from the
  contributing guidelines
- Run `dotnet build -c '{configuration}'`
  - Where `{configuration}` is a string defined in [`RevitLookup.csproj`][rl-proj] at the XPath `/Project/PropertyGroup/Configurations`
  - E.g. `dotnet build -c 'Release R23'`

This will build install the project into your local Revit addins directory for
the indicated year.

[rl-proj]: source/RevitLookup/RevitLookup.csproj

## Rules

- Follow the pattern of what you already see in the code
- When adding new classes/methods/changing existing code: run the debugger and make sure everything works

## Naming of features and functionality

The naming should be descriptive and direct, giving a clear idea of the functionality and usefulness in the future

## Prerequisites for Compiling RevitLookup

- .Net 8 SDK or newer
- Visual Studio 2022 / JetBrains Rider 2023.3 or newer

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

## Styles

The application UI is divided into templates, where each template can be customized for different types of data.
There are several different rules for customizing TreeView, DataGrid row, DataGrid cell and they are all located in the
file `RevitLookup/Views/Pages/Abstraction/SnoopViewBase.Styles.cs`.

Suggested methods search for a style/template by `x:Key`:

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
