# Changelog

# 2023-12-01 **2024.0.10**

## General

* Introducing a brand new feature: Restore window size! Now, effortlessly revert back to your preferred window dimensions with a simple click

## Improvements

* Add `MEPSystem.GetSectionByIndex` support by @SergeyNefyodov in https://github.com/jeremytammik/RevitLookup/pull/189
* Add `MEPSystem.GetSectionByNumber` support by @SergeyNefyodov in https://github.com/jeremytammik/RevitLookup/pull/189
* Add `MEPSection.GetElementIds` support by @SergeyNefyodov in https://github.com/jeremytammik/RevitLookup/pull/192
* Add `MEPSection.GetCoefficient` support by @SergeyNefyodov in https://github.com/jeremytammik/RevitLookup/pull/192
* Add `MEPSection.GetPressureDrop` support by @SergeyNefyodov in https://github.com/jeremytammik/RevitLookup/pull/192
* Add `MEPSection.GetSegmentLength` support by @SergeyNefyodov in https://github.com/jeremytammik/RevitLookup/pull/192
* Add `MEPSection.IsMain` support
* Add show `System.Object` option (named Root hierarchy) by @SergeyNefyodov in https://github.com/jeremytammik/RevitLookup/pull/193
* Add generic types support for the help button
* Minor tooltip changes

## Bugs

* Fixed search that worked in the main thread

# 2023-11-19 **2024.0.9**

## User interface

* **Settings Accessibility:** Most settings have been relocated to the grid context menu for a more intuitive and accessible user experience. Now, users can conveniently access and modify
  settings directly from the grid context menu

  ![image](https://github.com/jeremytammik/RevitLookup/assets/20504884/77fc172a-48d3-4439-a5d6-9d8d80ee0efc)

## Improvements

* Added ExtensibleStorage `Array` and `Map` support https://github.com/jeremytammik/RevitLookup/issues/184
* Added `Room.GetBoundarySegments()` support by [@SergeyNefyodov](https://github.com/SergeyNefyodov) in https://github.com/jeremytammik/RevitLookup/pull/187
* Added `BoundarySegment` support by [@SergeyNefyodov](https://github.com/SergeyNefyodov) in https://github.com/jeremytammik/RevitLookup/pull/187
* Added changing theme in runtime
* Optimized rendering performance for both tree and grid views, resulting in smoother and faster visual representation
* Added shortcuts for close current and all RevitLookup instances https://github.com/jeremytammik/RevitLookup/issues/172

    * `Esc` now closes the current window.

    * `Shift + Esc` closes all RevitLookup instances

    * Explore all available shortcuts [here](https://github.com/jeremytammik/RevitLookup/wiki/Context-actions)

## Bugs

* Fixed some crashes https://github.com/jeremytammik/RevitLookup/issues/180

## Breaking changes

* Disabled the last selection restoration during search to improve performance, especially on larger lists.
  This change optimizes search functionality by preventing potential slowdowns previously experienced with extensive lists

## Miscellaneous Updates

* Upgraded the UI library to the latest version, ensuring compatibility and incorporating potential improvements in functionality and design
* Project updated to .Net 8 and C# 12

## New Contributors

* [@SergeyNefyodov](https://github.com/SergeyNefyodov)  made their first contribution in https://github.com/jeremytammik/RevitLookup/pull/187

Full changelog: https://github.com/jeremytammik/RevitLookup/compare/2024.0.8...2024.0.9

# 2023-06-22 **2024.0.8**

## Features

### Core

* Computing Time Tracking

  This feature includes the ability to monitor the computing time taken to invoke a member, such as methods or properties.
  By tracking the execution time, you can identify and analyze slow-performing methods or properties, gaining insights into their overall performance.
  The computing time is displayed in a separate column and a tooltip, providing you with detailed information. This feature is optional and disabled by default

  ![image](https://github.com/jeremytammik/RevitLookup/assets/20504884/9f9c816f-2f49-49a0-9757-6f0bb0000113)

### User interface

* Context Menu

  A convenient context menu has been added to the table, providing you with additional options to manage columns and update contents.
  This menu enables you to customize your table view and effortlessly perform actions to enhance your experience.

  ![image](https://github.com/jeremytammik/RevitLookup/assets/20504884/25491ace-1d16-46cc-9dc3-3e5016b266a0)

* Enhanced Visualization

  Icons have been added to the context menu, making it more visually appealing and intuitive for users to navigate and interact with the available options.

  ![image](https://github.com/jeremytammik/RevitLookup/assets/20504884/afe44cbf-6e5b-4508-beda-b5a4e7babcf2)

## Improvements

* Added async support for unit dialogs
* Added API for external programs https://github.com/jeremytammik/RevitLookup/issues/171
* Added FamilyParameter support by @CADBIMDeveloper in https://github.com/jeremytammik/RevitLookup/pull/174
* Added FamilyManager.GetAssociatedFamilyParameter extension by @CADBIMDeveloper in https://github.com/jeremytammik/RevitLookup/pull/175

## Bugs

* Fixed shortcuts reloading leading to incorrect ribbon update https://github.com/jeremytammik/RevitLookup/issues/177

Full changelog: https://github.com/jeremytammik/RevitLookup/compare/2024.0.7...2024.0.8

# 2023-06-03 **2024.0.7**

Corrective update to the last major release [RevitLookup 2024.0.6](https://github.com/jeremytammik/RevitLookup/releases/tag/2024.0.6)

## Hotfix

* Fixed cases where the return value type was displayed instead of the value itself for methods that support overloads

## Improvements

* Added the `BuiltInCategory` extension of the `Category` class for Revit versions 2021-2022 where this property is not present in the official
  API https://github.com/jeremytammik/RevitLookup/issues/162

# 2023-06-01 **2024.0.6**

## Features

### User interface

* Icons

  Introducing a collection of new icons for properties, methods, fields, and events, ensuring a visually appealing representation
  ![image](https://github.com/jeremytammik/RevitLookup/assets/20504884/ffbba475-e240-4928-bf02-68d8f75cbc4c)

* Enhanced Performance with Separate UI Thread

  The RevitLookup user interface now operates in a dedicated thread, independent of Revit's workload. This architectural improvement significantly enhances smoothness and responsiveness

* New Additional Setting Options

  Introducing a range of new setting options that expand customization capabilities and provide users with greater control over the tool's behavior

### Core

* Class fields

  Introducing support for displaying class fields, enabling a comprehensive understanding of the class structure
  ![image](https://github.com/jeremytammik/RevitLookup/assets/20504884/a4304fd4-4537-4bd2-8d90-88f46137a55a)

* Class events

  Introducing support for displaying class events, facilitating better comprehension of event-driven programming within the class
  ![image](https://github.com/jeremytammik/RevitLookup/assets/20504884/3b7ae347-e7bc-4642-89a0-99cd089f0abe)

* Class private members

  Empowering developers with the ability to visualize and access class private fields, properties, methods, and events, providing a complete overview of the class implementation
  ![image](https://github.com/jeremytammik/RevitLookup/assets/20504884/4c6e4459-cf2f-4d35-9b03-fe0b259b3c9a)

## Improvements

* ElementId Redirection to Category

  Implemented a helpful feature that automatically redirects ElementId to Category, whenever applicable. This simplifies navigation and enhances the user experience
* Content Virtualization

  Applied content virtualization to the dashboard and settings page, optimizing performance by efficiently managing large amounts of data and dynamically loading content as needed. This
  results in a smoother and more efficient user interaction

# 2023-05-17 **2024.0.5**

## Features

* Static members support

  For example, RevitLookup now supports the display of these and other properties and methods:
    ```c#
    Category.GetCategory();
    Document.GetDocumentVersion()
    UIDocument.GetRevitUIFamilyLoadOptions()
    Application.MinimumThickness
    ```

  ![image](https://github.com/jeremytammik/RevitLookup/assets/20504884/1712cf55-2451-4ed8-8487-20c5ab973278)

* Ribbon update

  SplitButton replaced by PullDownButton. Thank for voting https://github.com/jeremytammik/RevitLookup/discussions/159

  ![image](https://github.com/jeremytammik/RevitLookup/assets/20504884/b6819f3b-5670-45ad-9353-1dabf9d3b512)

## Improvements

* Added DefinitionGroup support
* Added Element.GetMaterialArea support
* Added Element.GetMaterialVolume support
* Added FamilyInstance.get_Room support
* Added FamilyInstance.get_ToRoom support
* Added FamilyInstance.get_FromRoom support
* "Show element" is no longer available for ElementType

## Bugs

* Fixed issue when GetMaterialIds method didn't return nonPaint materials https://github.com/jeremytammik/RevitLookup/issues/163

# 2023-04-27 **2024.0.4**

Critical security patch

# 2023-04-24 **2024.0.3**

## Improvements

* Added Workset support
* Added WorksetTable support
* Added Document.GetUnusedElements support

## Bugs

* Fixed Dashboard window startup location

# 2023-04-18 **2024.0.2**

## Bugs

* Fixed Fatal Error on Windows 10 https://github.com/jeremytammik/RevitLookup/issues/153

  Accent colour sync with OS now only available in Windows 11 and above. Many thanks to [Aleksey Negus](https://t.me/a_negus) for testing builds

# 2023-04-14 **2024.0.1**

* Added option to enable hardware acceleration (experimental)

  The user interface is now more responsive. Revit uses software acceleration by default. Contact us if you encounter problems with your graphics cards

  Known issue: rendering performance drops on selection. This is especially evident on
  roofs. https://forums.autodesk.com/t5/revit-api-forum/revit-2024-rendering-performance-drops-on-selection/td-p/11878396
* Added button to enable RevitLookup panel on Modify tab by @ricaun in https://github.com/jeremytammik/RevitLookup/pull/152

  Disabled by default. Thanks vor voting https://github.com/jeremytammik/RevitLookup/discussions/151

* Opening RevitLookup window only when the Revit runtime context is available https://github.com/jeremytammik/RevitLookup/issues/155

## Improvements

* Added shortcuts support for the Modify tab https://github.com/jeremytammik/RevitLookup/issues/150
* Added EvaluatedParameter support
* Added Category.get_Visible support
* Added Category.get_AllowsVisibilityControl support
* Added Category.GetLineWeight support
* Added Category.GetLinePatternId support
* Added Category.GetElements extension
* Added Reference.ConvertToStableRepresentation support

## Bugs

* Fixed rare crashes in EventMonitor on large models
* Fixed Curve.Evaluate resolver using EndParameter as values

## Other

* Added installers for previous RevitLookup versions https://github.com/jeremytammik/RevitLookup/wiki/Versions

# 2023-04-04 **2024.0.0**

<div align="center">
<img alt="RevitLookup" width="600" src="https://user-images.githubusercontent.com/20504884/218192495-19b13547-ce67-40e3-8fe8-e847f89bddb7.png"/>
</div>

In this release, the entire code base has been completely rewritten from scratch with a redesigned user interface. New tools, OTA update, Windows 11 support

# UI

* A brand-new user interface

  ![image](https://user-images.githubusercontent.com/20504884/225871636-21c17658-d02e-411c-93cd-34e4d2121933.png)

* Themes

  ![image](https://user-images.githubusercontent.com/20504884/225851738-9db4dde1-fa05-4115-bd45-5e3af870ad36.png)

* Extended context menu

  ![image](https://user-images.githubusercontent.com/20504884/225880995-e6a20465-26c0-494d-8f35-3abaacdb9525.png)

  Wiki page: https://github.com/jeremytammik/RevitLookup/wiki/Context-actions

* Tooltips

  ![image](https://user-images.githubusercontent.com/20504884/225851987-7cc5ad2f-1a3b-4c4c-8744-6f3372e0f2ff.png)

* The Snoop Selection button has been moved to the Modify tab

  ![image](https://user-images.githubusercontent.com/93661926/225279009-e440f9cd-b59f-4198-b058-e081cc4204c4.png)

* Smooth navigation. Enable acceleration in Revit settings if you are having trouble with this option

  ![image](https://user-images.githubusercontent.com/20504884/225870803-785b5082-edd2-44cf-a384-8633052740d7.png)

* Windows 11 Mica effect support
* Windows 11 Snap Layouts support

  ![image](https://user-images.githubusercontent.com/93661926/225279198-c6985018-b1fc-435e-9fb4-f0c97f99ff8c.png)

* Accent colour synced with OS

  ![image](https://user-images.githubusercontent.com/20504884/225880312-4e9ee066-97ba-4e72-b89f-966269b385ec.png)

* New logo
* Searchbar. Focus is triggered by pressing any key on the keyboard

# Engine

* A brand-new core
* Extensions. Support new methods from the API and other libraries

  ![image](https://user-images.githubusercontent.com/20504884/225852056-9bb523c6-85dd-44d8-b900-ef3ca1eefaa6.png)

  Available extensions: https://github.com/jeremytammik/RevitLookup/wiki/Extensions

* Displaying all methods that objects have, even if RevitLookup does not support them

  ![image](https://user-images.githubusercontent.com/20504884/225852714-3255ece8-1c6c-464e-8949-5693b7ed7223.png)

  ![image](https://user-images.githubusercontent.com/20504884/225853186-ef6ce65e-6ee3-4a4d-a213-49fb8dfc7e75.png)

* Generic names support

  | Before                                                                                                          | Now                                                                                                             |
                    |-----------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------|
  | ![image](https://user-images.githubusercontent.com/20504884/225105646-37f2b052-f3fc-4771-967b-0578a94f9b07.png) | ![image](https://user-images.githubusercontent.com/20504884/225852403-4023c704-1932-471e-9f9f-84f8433013d7.png) |

* Multiple results for methods with overloads

  ![image](https://user-images.githubusercontent.com/20504884/225853785-8b5beacd-c8f1-401e-a51e-3e45f6aa8cba.png)

* Extensible storage moved to the `GetEntity()` method
* Adding new features and extending the functionality of RevitLookup just got easier. Developer's
  guide: https://github.com/jeremytammik/RevitLookup/blob/dev/Contributing.md#architecture

# New features

* Snoop Point
* Snoop Sub-Element
* Snoop UI Application
* Component manager. Explore AdWindows.dll and learn how the ribbon and user interface in Revit are arranged

  ![image](https://user-images.githubusercontent.com/20504884/225854089-04c8448f-34f9-419c-b859-c51b2a2375b6.png)

* PerformanceAdviser. Explore document performance issues
* Registry research: schemas, services, updaters
* Explore BuiltIn and Forge units

  ![image](https://user-images.githubusercontent.com/20504884/225869710-3c651c4a-0b35-4dd4-8180-27370f657cd8.png)

* Event monitor. Track all incoming events. Events from the RevitAPI.dll and RevitAPIUI.dll libraries are available. The search bar is used to filter results

  ![image](https://user-images.githubusercontent.com/20504884/225856874-46c14b80-5c7d-444c-999a-10e8e4809ad2.png)

* Reworked search. Now you can search for multiple values by `Name`, `Id`, `UniqueId`, `IfcGUID` and `Type IfcGUID` parameters

  ![image](https://user-images.githubusercontent.com/20504884/225869706-7d5e2e4a-1f03-416e-9ad6-a96184b07836.png)

  Wiki page: https://github.com/jeremytammik/RevitLookup/wiki/Search-elements

* Visual search in a project.

  Showing elements:

  ![image](https://user-images.githubusercontent.com/20504884/225874545-aa0f7829-5215-412d-8c50-31ede8705ca8.png)

  Showing faces (Revit 2023 or higher):

  ![image](https://user-images.githubusercontent.com/20504884/225866963-d9a3c2e4-1569-40c4-9072-1736906dce6b.png)

  Showing solids (Revit 2023 or higher):

  ![image](https://user-images.githubusercontent.com/20504884/225867976-ccb417a0-85c6-4996-bcd8-fdff8a504152.png)

  Showing edges (Revit 2023 or higher):

  ![image](https://user-images.githubusercontent.com/20504884/225867460-282751ad-0782-4cb7-bb96-7465be556c6f.png)

* OTA update. The RevitLookup update is now available directly from the plugin

  ![image](https://user-images.githubusercontent.com/20504884/225875561-bda637d0-d170-411d-83ac-97b17342904a.png)

Designed & Developed by [Nice3point](https://github.com/Nice3point) 🕊

# 2022-06-17 **2023.1.0**

In this update:

- New: Hello World window changed to About
- New: resorted commands on the Revit ribbon, frequently used moved to the top
- New: added Snoop Active Document command
- Fix: revert support search index from keyboard
- Fix: removed label null if ElementID was -1

# 2022-04-05 **2023.0.0**

Revit 2023 support

# 2022-03-02 **2022.0.4.1**

Minor UI changes https://github.com/jeremytammik/RevitLookup/pull/135

# 2022-03-02 **2022.0.4.0**

Minimize, maximize support https://github.com/jeremytammik/RevitLookup/pull/134.
Fixed problem with sending a print job https://github.com/jeremytammik/RevitLookup/pull/133

# 2022-01-15 **2022.0.3.3**

BindingMap support. https://github.com/jeremytammik/RevitLookup/issues/128 issue.

# 2022-01-03 **2022.0.3.2**

This update fixes the display of labels:

- Support for string empty label.
- Support for string null label.
- Support for double? null label.
- Renamed "View = null" to "Undefined View". This was a misnomer because null throws an exception
- Renamed "View = null - Including geometry objects not set as Visible" to "Undefined View, including non-visible objects"
- Renamed "View = Document.ActiveView" to "Active View"
- Renamed "View = Document.ActiveView - Including geometry objects not set as Visible" to "Active View, including non-visible objects"

# 2021-12-21 **2022.0.3.1**

Fixed https://github.com/jeremytammik/RevitLookup/issues/117 issue. Reflection TargetException message replaced by InnerException message.

# 2021-12-04 **2022.0.3.0**

This is patch release to fix a few items. This release will fix the issues below:

- Removed unused code and resources, reduced application size
- Optimized collections and arrays, updating the window just got a little faster
- Fixed broken print and preview button
- Reduced memory allocation

# 2021-11-30 **2022.0.2.6**

mention RFA and RVT project in the project description in readme.md and repository summary

# 2021-11-13 **2022.0.2.5**

Added automatic generation of a release for the master branch

# 2021-10-29 **2022.0.2.0**

integrated pull request [#108](https://github.com/jeremytammik/RevitLookup/pull/108) by Roman @Nice3point
to include previous versions in the installer

# 2021-10-28 **2022.0.1.6**

integrated pull request [#107](https://github.com/jeremytammik/RevitLookup/pull/107) by Roman @Nice3point
to rename Build, fix hello world version and move version number into csproj

# 2021-10-28 **2022.0.1.4**

integrated pull request [#105](https://github.com/jeremytammik/RevitLookup/pull/105) by Roman @Nice3point
to update badges, consolidate version number management, clean up builder and remove gitlab CI

# 2021-10-28 **2022.0.1.3**

integrated pull request [#104](https://github.com/jeremytammik/RevitLookup/pull/104) by Roman @Nice3point
to fix snoop db exception due to tag and enum mismatch

# 2021-10-28 **2022.0.1.2**

integrated pull request [#102](https://github.com/jeremytammik/RevitLookup/pull/102) by Roman @Nice3point
to add changelog and remove unused files

# 2021-10-28 **2022.0.1.1**

integrated pull request [#101](https://github.com/jeremytammik/RevitLookup/pull/101) by Roman @Nice3point
implementing code rerstructuring, cleanup, build system and installer

# 2021-10-24 **2022.0.1.0**

integrated pull request [#99](https://github.com/jeremytammik/RevitLookup/pull/99) by @NeVeS
to Eliminate warnings from [#98](https://github.com/jeremytammik/RevitLookup/pull/98)

# 2021-10-18 **2022.0.1.0**

integrated pull request [#97](https://github.com/jeremytammik/RevitLookup/pull/97) by @NeVeS
to restore ability to snoop plan topologies

# 2021-10-17 **2022.0.1.0**

integrated pull request [#96](https://github.com/jeremytammik/RevitLookup/pull/96) by @NeVeS
to fix crash on user cancel picking object in cmds: SnoopPickFace, SnoopPickEdge, SnoopLinkedElement

# 2021-10-17 **2022.0.1.0**

integrated pull request [#95](https://github.com/jeremytammik/RevitLookup/pull/95) by @NeVeS
to handle multiple open documents at the same time

# 2021-10-16 **2022.0.1.0**

integrated pull request [#94](https://github.com/jeremytammik/RevitLookup/pull/94) by @NeVeS
fixing problem with tranferring focus to Revit when using selectors from modeless window

# 2021-10-16 **2022.0.1.0**

integrated pull request [#93](https://github.com/jeremytammik/RevitLookup/pull/93) by @NeVeS
imlementing Modeless windows

# 2021-09-22 **2022.0.0.16**

integrated pull request [#91](https://github.com/jeremytammik/RevitLookup/pull/91) by @mphelt
to add PartUtilsStream

# 2021-07-01 **2022.0.0.13**

integrated pull request [#86](https://github.com/jeremytammik/RevitLookup/pull/86) by Luiz Henrique Cassettari
adding OnLoad to update width of snoop window value ListView last column

# 2021-06-30 **2022.0.0.12**

integrated pull request [#85](https://github.com/jeremytammik/RevitLookup/pull/85) by Luiz Henrique Cassettari
increasing width of snoop window value ListView column from 300 to 800

# 2021-06-07 **2022.0.0.11**

integrated pull request [#84](https://github.com/jeremytammik/RevitLookup/pull/84) by @RevitArkitek
adding PlanViewRange functionality to display view range level id and offset

# 2021-06-07 **2022.0.0.10**

integrated pull request [#83](https://github.com/jeremytammik/RevitLookup/pull/83) by @RevitArkitek
fixing error where element cannot be retrieved for an element id because SupportedColorFillCategoryIds returns category ids instead

# 2021-05-18 **2022.0.0.9**

integrated pull request [#81](https://github.com/jeremytammik/RevitLookup/pull/81) by @CADBIMDeveloper
enhancing `ElementId` and Revit 2022 extensible storage support

# 2021-05-14 **2022.0.0.8**

integrated pull request [#80](https://github.com/jeremytammik/RevitLookup/pull/80) by @WspDev
to remove deprecated `ParameterType` usage

# 2021-05-07 **2022.0.0.7**

integrated pull request [#78](https://github.com/jeremytammik/RevitLookup/pull/78) by @RevitArkitek
to handle `TableData.GetSectionData`

# 2021-05-07 **2022.0.0.6**

integrated pull request [#77](https://github.com/jeremytammik/RevitLookup/pull/77) by @RevitArkitek
to get end points for curves

# 2021-04-16 **2022.0.0.5**

integrated pull request [#76](https://github.com/jeremytammik/RevitLookup/pull/76) by @peterhirn
to fix CI for new VS 2019 Revit 2022 dotnet-core csproj

# 2021-04-15 **2022.0.0.4**

upgraded to Visual Studio 2019 (from 2017) and adopted @peterhirn project and solution files

# 2021-04-15 **2022.0.0.3**

reset Revit API assembly DLL references to Copy Local false

# 2021-04-15 **2022.0.0.3**

integrated pull request [#73](https://github.com/jeremytammik/RevitLookup/pull/73) by @mphelt
to wrap snoop in temporary transaction allowing to snoop PlanTopologies

# 2021-04-15 **2022.0.0.2**

integrated pull request [#75](https://github.com/jeremytammik/RevitLookup/pull/75) by @peterhirn
to fix CI for Revit 2022 and non-dotnet-core project file

# 2021-04-15 **2022.0.0.1**

integrated pull request [#74](https://github.com/jeremytammik/RevitLookup/pull/74) by @peterhirn
setting up CI to Revit 2022

# 2021-04-15 **2022.0.0.0**

flat migration to Revit 2022

# 2021-02-09 **2021.0.0.13**

integrated pull request https://github.com/jeremytammik/RevitLookup/pull/71 by @RevitArkitek
adding handler for ScheduleDefinition.GetField to address issue https://github.com/jeremytammik/RevitLookup/issues/70

# 2021-02-01 **2021.0.0.12**

integrated pull request [#69](https://github.com/jeremytammik/RevitLookup/pull/69) by @RevitArkitek
adding handler for the GetSplitRegionOffsets method to address issue https://github.com/jeremytammik/RevitLookup/issues/68 Split Region Offsets (2021)

# 2021-01-12 **2021.0.0.11**

integrated pull request [#67](https://github.com/jeremytammik/RevitLookup/pull/67) by @peterhirn
to update timestamp server from Verisign to digicert

# 2021-01-11 **2021.0.0.10**

increment copyright year

# 2020-12-06 **2021.0.0.9**

locally disable warning CS0618 `DisplayUnitType` is obsolete for one specific use case

# 2020-12-04 **2021.0.0.8**

integrated pull request [#66](https://github.com/jeremytammik/RevitLookup/pull/66) by @RevitArkitek
adding handlers for View GetTemplateParameterIds and GetNonControlledTemplateParameterIds

# 2020-11-09 **2021.0.0.7**

integrated pull request [#64](https://github.com/jeremytammik/RevitLookup/pull/64) by @peterhirn
to update CI for Revit 2021

# 2020-10-20 **2021.0.0.6**

eliminated deprecated unit api usage

# 2020-10-20 **2021.0.0.5**

integrated pull request [#63](https://github.com/jeremytammik/RevitLookup/pull/63) by @swfaust
to update command registration and remove obsolete test framework command

# 2020-04-14 **2021.0.0.1**

integrated pull request [#58](https://github.com/jeremytammik/RevitLookup/pull/58) by @harrymattison with
solution changes for multi-release building

# 2020-04-12 **2021.0.0.0**

flat migration to Revit 2021

# 2020-02-11 **2020.0.0.4**

incremented copyright year

# 2019-08-20 **2020.0.0.3**

integrated pull request [#56](https://github.com/jeremytammik/RevitLookup/pull/56) by @nonoesp

- fix two small typos in readme

# 2019-06-03 **2020.0.0.2**

integrated pull request [#53](https://github.com/jeremytammik/RevitLookup/pull/53) by @CADBIMDeveloper

- list available values for ParameterType.FamilyType and FamilyParameters titles

# 2019-04-26 **2020.0.0.1**

integrated pull request [#52](https://github.com/jeremytammik/RevitLookup/pull/52) by @CADBIMDeveloper

# 2019-04-18 **2020.0.0.0**

flat migration to Revit 2020

# 2019-04-18 **2019.0.0.13**

added MSI installer for Revit 2017-2020 by Harry Mattison

# 2019-03-27 **2019.0.0.12**

added MSI installer for 2018.0.0.0 submitted by @VBScab in issue [#51](https://github.com/jeremytammik/RevitLookup/issues/51)

# 2019-03-26 **2019.0.0.11**

integrated pull request [#50](https://github.com/jeremytammik/RevitLookup/pull/50) by Victor Chekalin
to handle DoubleArray4d values

# 2019-03-25 **2019.0.0.10**

integrated pull requests [#48](https://github.com/jeremytammik/RevitLookup/pull/48) and [#49](https://github.com/jeremytammik/RevitLookup/pull/49) by Victor Chekalin
to snoop rendering AssetProperty via Material-AppearanceAssetId-GetRenderingAssset

# 2019-03-18 **2019.0.0.9**

added CmdSnoopModScopeDependents

# 2019-03-18 **2019.0.0.8**

cleanup before adding CmdSnoopModScopeDependents

# 2019-01-21 **2019.0.0.7**

fixed typo in variable name reported by @yk35 in pull request [#47](https://github.com/jeremytammik/RevitLookup/pull/47)

# 2019-01-17 **2019.0.0.6**

added new commands by H�vard Leding: pick surface, edge, linked element

# 2019-01-09 **2019.0.0.5**

incremented copyright year to 2019

# 2018-12-13 **2019.0.0.4**

merged issue [#45](https://github.com/jeremytammik/RevitLookup/issues/45) and pull request [#46](https://github.com/jeremytammik/RevitLookup/pull/46)from @TheKidMSX
to center parent for forms

# 2018-05-29 **2019.0.0.2**

merged pull request [#43](https://github.com/jeremytammik/RevitLookup/pull/43) from Levente Koncz @palver123
to use ProgramW6432 variable in csproj to locate Revit API assembly DLLs

# 2018-04-15 **2019.0.0.0**

flat migration to Revit 2019

# 2018-03-12 **2018.0.0.8**

merged pull request [#42](https://github.com/jeremytammik/RevitLookup/pull/42) from @Modis Pekshev:
Add "Search by and snoop" command

# 2018-03-02 **2018.0.0.7**

merged pull request [#41](https://github.com/jeremytammik/RevitLookup/pull/41) from @Modis Pekshev:
Add ConvertToStableRepresentation method for References

# 2018-01-05 **2018.0.0.6**

incremented copyright year to 2018

# 2018-01-05 **2018.0.0.5**

readme enhancements: badges, installer and updated link to MSI installer

# 2017-08-28 **2018.0.0.3**

merged pull request [#36](https://github.com/jeremytammik/RevitLookup/pull/36) from @Andrey-Bushman:
switch target platform to.Net 4.6 and replace Revit 2017 NuGet package by Revit 2018.1 Nuget package

# 2017-06-05 **2018.0.0.1**

merged pull request [#34](https://github.com/jeremytammik/RevitLookup/pull/34) from @CADBIMDeveloper:
annotative family instance geometry, element enumerations instead of ids, parameter names and byte property values

# 2017-04-21 **2018.0.0.0**

flat migration to Revit 2018

# 2017-04-07 **2017.0.0.24**

merged pull request [#33](https://github.com/jeremytammik/RevitLookup/pull/33) by @peterhirn
added build status badge

# 2017-03-27 **2017.0.0.23**

dummy modification to trigger build for https://lookupbuilds.com
cf.https://forums.autodesk.com/t5/revit-api-forum/ci-for-revit-lookup/m-p/6947111

# 2017-03-17 **2017.0.0.22**

added 'new' keyword to avoid warning and override inherited methods

# 2017-03-17 **2017.0.0.21**

merged pull request [#31](https://github.com/jeremytammik/RevitLookup/pull/31) from @CADBIMDeveloper
removing try-catch handler

# 2017-03-16 **2017.0.0.20**

merged pull request [#30](https://github.com/jeremytammik/RevitLookup/pull/30) from @eirannejad
adding icon and exception handling

# 2017-03-15 **2017.0.0.19**

merged pull request [#29](https://github.com/jeremytammik/RevitLookup/pull/29) from @CADBIMDeveloper
fixing bugs initialising type and opening background documents

# 2017-03-02 **2017.0.0.18**

merged pull request [#27](https://github.com/jeremytammik/RevitLookup/pull/27) from @CADBIMDeveloper
to display category BuiltInCategory, nullable double properties and empty lists

# 2017-02-21 **2017.0.0.17**

merged pull request [#26](https://github.com/jeremytammik/RevitLookup/pull/26) from Alexander Ignatovich
to restore ability to see extensible storage content

# 2017-02-17 **2017.0.0.16**

merged pull request [#25](https://github.com/jeremytammik/RevitLookup/pull/25) from chekalin-v:
fix old bugs, significant improvements to the new reflection approach

# 2017-02-06 **2017.0.0.15**

merged pull request [#23](https://github.com/jeremytammik/RevitLookup/pull/23) from awmcc90
to catch specific reflection invocation exceptions, not all

# 2017-02-06 **2017.0.0.14**

merged pull request [#22](https://github.com/jeremytammik/RevitLookup/pull/22) from awmcc90
drastic changes implementing object inspection via reflection and cross-version compatibility

# 2017-02-02 **2017.0.0.13**

whitespace

# 2017-02-02 **2017.0.0.12**

merged pull [#21](https://github.com/jeremytammik/RevitLookup/pull/21) from @eibre adding UnitType property on the parameter Definition class

# 2017-01-06 **2017.0.0.11**

fixed issue [#19](https://github.com/jeremytammik/RevitLookup/issues/19) raised by LeeJaeYoung spot dimension position and text position error

# 2017-01-06 **2017.0.0.10**

merged pull [#20](https://github.com/jeremytammik/RevitLookup/pull/20) from @luftbanana supporting close-with-ESC
to all forms by assigning the cancel button

# 2017-01-03 **2017.0.0.9**

incremented copyright year

# 2016-12-20 **2017.0.0.8**

added version number to Hello World message box

# 2016-12-20 **2017.0.0.7**

merged pull request [#18](https://github.com/jeremytammik/RevitLookup/pull/18) by @Andrey-Bushman
to use NuGet Revit API package

# 2016-08-05 **2017.0.0.6**

merged pull request [#16](https://github.com/jeremytammik/RevitLookup/pull/16) by @arif-hanif
to add post build event to project file to copy addin manifest and dll to addins folder

# 2016-06-23 **2017.0.0.5**

merged pull request [#14](https://github.com/jeremytammik/RevitLookup/pull/14) by Shayneham
to handle exceptions snooping flex pipe and duct lacking levels etc.

# 2016-06-04 **2017.0.0.4**

merged pull request [#13](https://github.com/jeremytammik/RevitLookup/pull/13) by awmcc90
to skip mepSys.Elements for OST_ElectricalInternalCircuits category

# 2016-06-04 **2017.0.0.3**

before merging pull request [#13](https://github.com/jeremytammik/RevitLookup/pull/13) by awmcc90 to skip mepSys.Elements

# 2016-04-19 **2017.0.0.2**

ready for publication

# 2016-04-15 **2017.0.0.1**

microscopic cleanup

# 2016-04-15 **2017.0.0.0**

migration to Revit 2017 Manuel of Sofistik

# 2016-04-04 **2016.0.0.13**

incremented copyright year from 2015 to 2016

# 2015-10-22 **2016.0.0.12**

readme cleanup

# 2015-09-15 **2016.0.0.11**

implemented support for Element bounding box

# 2015-09-01 **2016.0.0.10**

handle null floor.GetAnalyticalModel returned in RAC and RME

# 2015-05-21 **2016.0.0.9**

display all the display names of the BuiltInParameter enumeration value

# 2015-05-15 **2016.0.0.8**

updated Revit API assembly paths for final release of Revit 2016

# 2015-04-23 **2016.0.0.7**

updated post-build event target path to Revit 2016 add-ins folder

# 2015-04-21 **2016.0.0.6**

set Copy Local false on Revit API assemblies

# 2015-04-21 **2016.0.0.5**

initial migration to Revit 2016 - first public release

# 2015-04-20 **2016.0.0.4**

integrated changes from previous Revit 2016 version into elaine's one

# 2015-04-20 **2016.0.0.3**

initial migration to Revit 2016 by @ElaineJieyanZheng

# 2015-04-19 **2015.0.0.8**

integrated pull request #6 by yzraeu, additinal try catch for Level Offset and MEP System

# 2015-01-30 **2015.0.0.7**

removed all statements 'using' the empty root namespace Autodesk.Revit

# 2015-01-29 **2015.0.0.6**

incremented copyright message year from 2014 to 2015

# 2015-01-13 **2015.0.0.5**

added CategoryType to the CategoryCollector

# 2014-11-24 **2015.0.0.4**

encapsulate transaction in using statement

# 2014-11-24 **2015.0.0.3**

merged fix by Tom Pesman @tompesman to catch exception thrown by doc.PrintManager

# 2014-10-06 **2015.0.0.2**

removed obsolete Revit API usage to compile with zero errors and zero warnings

# 2014-04-17 **2015.0.0.1**

recompiled for Revit 2015 UR1

# 2014-04-02 **2015.0.0.0**

initial migration to Revit 2015 Meridian prerelease PR10

# 2014-01-28 **2014.0.1.0**

double checked that this version corresponds with florian's

# 2014-01-27 **2014.0.0.7**

merged pull request from FlorianSchmid of SOFiSTiK:
extended (added) snooping of geometry, FormatOptions and RevitLinkInstances plus some fixes of compiler errors/warnings; bumped copyright year from 2013 to 2014

# 2014-01-11 **2014.0.0.6**

joespiff adjusted the Anchor property of the "Built-in Enums Map..." button on the Parameters form so that it behaves well when stretching the form

# 2013-10-24 **2014.0.0.5**

merged pull request from Prasadgalle