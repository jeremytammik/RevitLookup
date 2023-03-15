# Changelog

- 2022-03-14 **2024.0.0** 

<div align="center">
<img alt="RevitLookup" width="600"    src="https://user-images.githubusercontent.com/20504884/218192495-19b13547-ce67-40e3-8fe8-e847f89bddb7.png"/>
</div>

In this release, the entire code base has been completely rewritten from scratch with a redesigned user interface. New tools added, OTA update, Windows 11 support included

# UI

* A brand-new user interface

    ![image](https://user-images.githubusercontent.com/20504884/225030639-f13c5432-84e0-4508-876e-f104794540f5.png)

* Themes support

    ![image](https://user-images.githubusercontent.com/93661926/225276975-44ff4149-3411-4d71-bc3d-8093e6378a0f.png)

* New logo
* Searchbar
* Context menu with new actions

    ![image](https://user-images.githubusercontent.com/20504884/225292648-fd485954-f469-4dd4-812d-8a4d8fdf8094.png)

* Tooltips

    ![image](https://user-images.githubusercontent.com/93661926/225278782-14624000-5b4a-43ce-9e72-e411fcb17527.png)

* The Snoop Selection button has been moved to the Modify tab

    ![image](https://user-images.githubusercontent.com/93661926/225279009-e440f9cd-b59f-4198-b058-e081cc4204c4.png)

* Smooth navigation. Enable acceleration in Revit settings if you are having trouble with this option

    ![image](https://user-images.githubusercontent.com/20504884/225108851-e098a8ae-ce91-445b-b683-5c82cf47f777.png)

* Windows 11 Mica background style support
* Windows 11 snap layouts support

    ![image](https://user-images.githubusercontent.com/93661926/225279198-c6985018-b1fc-435e-9fb4-f0c97f99ff8c.png)

# Engine

* A brand-new core
* Extension support. Support for some static methods from the API and some new ones

    ![image](https://user-images.githubusercontent.com/20504884/225107640-66d586fc-4610-4c44-8952-74ade1b88955.png)

* Generic names support

    | Before                                                                                                                  | Now                                                                                                                   |
    |-----------------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------|
    | ![image](https://user-images.githubusercontent.com/20504884/225105646-37f2b052-f3fc-4771-967b-0578a94f9b07.png) | ![image](https://user-images.githubusercontent.com/20504884/225105607-ce5b064b-e3b1-4fbe-ab7a-57fe285ab9aa.png) |

* Now you can see all the available methods that objects have, even if RevitLookup doesn't support them

    ![image](https://user-images.githubusercontent.com/20504884/225106462-a40c7dbf-4886-476e-bf99-6bbf9fd63166.png)

    ![image](https://user-images.githubusercontent.com/20504884/225106951-1c5f9e6d-f8df-4a7a-bf3b-c13926ec3b8b.png)

* Support for multiple results for methods with overloads

    ![image](https://user-images.githubusercontent.com/20504884/225129590-81304032-410e-4677-bb4e-cea22598e1c9.png)

* Extensible Storage has been removed from the element. Now the GetEntity() method is used for this, which is already in the API
* Simplified addition of new features by other developers. The developer guide can be found here: https://github.com/jeremytammik/RevitLookup/blob/dev/Contributing.md#architecture

# Features

* Snoop UI Application
* Snoop Point
* Snoop Sub-Element
* Component manager. Now you can explore it from the AdWindows.dll and learn how the ribbon and user interface in Revit are arranged

    ![image](https://user-images.githubusercontent.com/20504884/225289883-c686b3ce-3398-4768-80d3-8ddb229526ac.png)

* PerformanceAdviser. Explore performance issues in this document
* Registry research, schemas, services, updaters
* Explore BuiltIn and Forge units

    ![image](https://user-images.githubusercontent.com/20504884/225114192-5178be61-2fa1-4b57-8fbe-f6a85eae27d1.png)

* Search reworked

    Now you can search by several values. Use values with different delimiters on a new line.
    Support for search by name, id, uniqID and IfcGUID, Type IfcGUID

    ![image](https://user-images.githubusercontent.com/20504884/225115721-cd1d661c-f9f0-49fd-94e1-9e42aa185322.png)

    The Type IfcGUID search also finds all instances of that type

    ![image](https://user-images.githubusercontent.com/20504884/225117066-a8d80f50-8bb9-4ccf-84b4-ace798e52858.png)

    This is a more convenient way to search for items since it is accessible from the Revit properties panel and is always at your fingertips

    ![image](https://user-images.githubusercontent.com/20504884/225117536-daf8a0ad-ccfd-4632-b6f4-07654cdf6970.png)

* Event monitor. Now you can track all incoming events. Events from RevitApi and RevitAPIUI libraries are available

    ![image](https://user-images.githubusercontent.com/20504884/225290957-29a745c4-f121-4994-a430-7d85f3fa5e7c.png)

* Now you can visually search for elements in the project:

    ![image](https://user-images.githubusercontent.com/20504884/225122161-d61ff4d2-8263-45d8-8e0e-71ab9821b592.png)

    Select solid:

    ![image](https://user-images.githubusercontent.com/20504884/225123500-b354cde2-347b-4552-b04b-f7b215b70321.png)

    Select face:

    ![image](https://user-images.githubusercontent.com/20504884/225124175-82f9ae66-fc33-4f88-97fd-d594ea6f5233.png)

    Select edge:

    ![image](https://user-images.githubusercontent.com/20504884/225124568-1c53118f-3806-4b46-ab66-f498dde2b77c.png)

* OTA update. The RevitLookup update is now available directly from the plugin

And all other updates that are not described in the changelog you can check now.

Made with ❤️ by Nice3point
- 2022-06-17 **2023.1.0** In this update:
    - New: Hello World window changed to About
    - New: resorted commands on the Revit ribbon, frequently used moved to the top
    - New: added Snoop Active Document command
    - Fix: revert support search index from keyboard
    - Fix: removed label null if ElementID was -1
- 2022-04-05 **2023.0.0** Revit 2023 support
- 2022-03-02 **2022.0.4.1** Minor UI changes https://github.com/jeremytammik/RevitLookup/pull/135
- 2022-03-02 **2022.0.4.0** Minimize, maximize support https://github.com/jeremytammik/RevitLookup/pull/134. Fixed problem with sending a print job https://github.com/jeremytammik/RevitLookup/pull/133
- 2022-01-15 **2022.0.3.3** BindingMap support. https://github.com/jeremytammik/RevitLookup/issues/128 issue.
- 2022-01-03 **2022.0.3.2** This update fixes the display of labels:
    - Support for string empty label.
    - Support for string null label.
    - Support for double? null label.
    - Renamed "View = null" to "Undefined View". This was a misnomer because null throws an exception
    - Renamed "View = null - Including geometry objects not set as Visible" to "Undefined View, including non-visible objects"
    - Renamed "View = Document.ActiveView" to "Active View"
    - Renamed "View = Document.ActiveView - Including geometry objects not set as Visible" to "Active View, including non-visible objects"
- 2021-12-21 **2022.0.3.1** Fixed https://github.com/jeremytammik/RevitLookup/issues/117 issue. Reflection TargetException message replaced by InnerException message.
- 2021-12-04 **2022.0.3.0** This is patch release to fix a few items. This release will fix the issues below:
    - Removed unused code and resources, reduced application size
    - Optimized collections and arrays, updating the window just got a little faster
    - Fixed broken print and preview button
    - Reduced memory allocation
- 2021-11-30 **2022.0.2.6** mention RFA and RVT project in the project description in readme.md and repository summary
- 2021-11-13 **2022.0.2.5** Added automatic generation of a release for the master branch
- 2021-10-29 **2022.0.2.0** integrated pull request [#108](https://github.com/jeremytammik/RevitLookup/pull/108) by Roman @Nice3point to include previous versions in the installer
- 2021-10-28 **2022.0.1.6** integrated pull request [#107](https://github.com/jeremytammik/RevitLookup/pull/107) by Roman @Nice3point to rename Build, fix hello world version and move version number into csproj
- 2021-10-28 **2022.0.1.4** integrated pull request [#105](https://github.com/jeremytammik/RevitLookup/pull/105) by Roman @Nice3point to update badges, consolidate version number management, clean up builder and remove gitlab CI
- 2021-10-28 **2022.0.1.3** integrated pull request [#104](https://github.com/jeremytammik/RevitLookup/pull/104) by Roman @Nice3point to fix snoop db exception due to tag and enum mismatch
- 2021-10-28 **2022.0.1.2** integrated pull request [#102](https://github.com/jeremytammik/RevitLookup/pull/102) by Roman @Nice3point to add changelog and remove unused files
- 2021-10-28 **2022.0.1.1** integrated pull request [#101](https://github.com/jeremytammik/RevitLookup/pull/101) by Roman @Nice3point implementing code rerstructuring, cleanup, build system and installer
- 2021-10-24 **2022.0.1.0** integrated pull request [#99](https://github.com/jeremytammik/RevitLookup/pull/99) by @NeVeS to Eliminate warnings from [#98](https://github.com/jeremytammik/RevitLookup/pull/98)
- 2021-10-18 **2022.0.1.0** integrated pull request [#97](https://github.com/jeremytammik/RevitLookup/pull/97) by @NeVeS to restore ability to snoop plan topologies
- 2021-10-17 **2022.0.1.0** integrated pull request [#96](https://github.com/jeremytammik/RevitLookup/pull/96) by @NeVeS to fix crash on user cancel picking object in cmds: SnoopPickFace, SnoopPickEdge, SnoopLinkedElement
- 2021-10-17 **2022.0.1.0** integrated pull request [#95](https://github.com/jeremytammik/RevitLookup/pull/95) by @NeVeS to handle multiple open documents at the same time
- 2021-10-16 **2022.0.1.0** integrated pull request [#94](https://github.com/jeremytammik/RevitLookup/pull/94) by @NeVeS fixing problem with tranferring focus to Revit when using selectors from modeless window
- 2021-10-16 **2022.0.1.0** integrated pull request [#93](https://github.com/jeremytammik/RevitLookup/pull/93) by @NeVeS imlementing Modeless windows
- 2021-09-22 **2022.0.0.16** integrated pull request [#91](https://github.com/jeremytammik/RevitLookup/pull/91) by @mphelt to add PartUtilsStream
- 2021-07-01 **2022.0.0.13** integrated pull request [#86](https://github.com/jeremytammik/RevitLookup/pull/86) by Luiz Henrique Cassettari adding OnLoad to update width of snoop window value ListView last column
- 2021-06-30 **2022.0.0.12** integrated pull request [#85](https://github.com/jeremytammik/RevitLookup/pull/85) by Luiz Henrique Cassettari increasing width of snoop window value ListView column from 300 to 800
- 2021-06-07 **2022.0.0.11** integrated pull request [#84](https://github.com/jeremytammik/RevitLookup/pull/84) by @RevitArkitek adding PlanViewRange functionality to display view range level id and offset
- 2021-06-07 **2022.0.0.10** integrated pull request [#83](https://github.com/jeremytammik/RevitLookup/pull/83) by @RevitArkitek fixing error where element cannot be retrieved for an element id because SupportedColorFillCategoryIds returns category ids instead
- 2021-05-18 **2022.0.0.9** integrated pull request [#81](https://github.com/jeremytammik/RevitLookup/pull/81) by @CADBIMDeveloper enhancing `ElementId` and Revit 2022 extensible storage support
- 2021-05-14 **2022.0.0.8** integrated pull request [#80](https://github.com/jeremytammik/RevitLookup/pull/80) by @WspDev to remove deprecated `ParameterType` usage
- 2021-05-07 **2022.0.0.7** integrated pull request [#78](https://github.com/jeremytammik/RevitLookup/pull/78) by @RevitArkitek to handle `TableData.GetSectionData`
- 2021-05-07 **2022.0.0.6** integrated pull request [#77](https://github.com/jeremytammik/RevitLookup/pull/77) by @RevitArkitek to get end points for curves
- 2021-04-16 **2022.0.0.5** integrated pull request [#76](https://github.com/jeremytammik/RevitLookup/pull/76) by @peterhirn to fix CI for new VS 2019 Revit 2022 dotnet-core csproj
- 2021-04-15 **2022.0.0.4** upgraded to Visual Studio 2019 (from 2017) and adopted @peterhirn project and solution files
- 2021-04-15 **2022.0.0.3** reset Revit API assembly DLL references to Copy Local false
- 2021-04-15 **2022.0.0.3** integrated pull request [#73](https://github.com/jeremytammik/RevitLookup/pull/73) by @mphelt to wrap snoop in temporary transaction allowing to snoop PlanTopologies
- 2021-04-15 **2022.0.0.2** integrated pull request [#75](https://github.com/jeremytammik/RevitLookup/pull/75) by @peterhirn to fix CI for Revit 2022 and non-dotnet-core project file
- 2021-04-15 **2022.0.0.1** integrated pull request [#74](https://github.com/jeremytammik/RevitLookup/pull/74) by @peterhirn setting up CI to Revit 2022
- 2021-04-15 **2022.0.0.0** flat migration to Revit 2022
- 2021-02-09 **2021.0.0.13** integrated pull request [#71](https://github.com/jeremytammik/RevitLookup/pull/71) by @RevitArkitek adding handler for ScheduleDefinition.GetField to address issue [#70](https://github.com/jeremytammik/RevitLookup/issues/70)
- 2021-02-01 **2021.0.0.12** integrated pull request [#69](https://github.com/jeremytammik/RevitLookup/pull/69) by @RevitArkitek adding handler for the GetSplitRegionOffsets method to address issue [#68](https://github.com/jeremytammik/RevitLookup/issues/68) Split Region Offsets (2021)
- 2021-01-12 **2021.0.0.11** integrated pull request [#67](https://github.com/jeremytammik/RevitLookup/pull/67) by @peterhirn to update timestamp server from Verisign to digicert
- 2021-01-11 **2021.0.0.10** increment copyright year
- 2020-12-06 **2021.0.0.9** locally disable warning CS0618 `DisplayUnitType` is obsolete for one specific use case
- 2020-12-04 **2021.0.0.8** integrated pull request [#66](https://github.com/jeremytammik/RevitLookup/pull/66) by @RevitArkitek adding handlers for View GetTemplateParameterIds and GetNonControlledTemplateParameterIds
- 2020-11-09 **2021.0.0.7** integrated pull request [#64](https://github.com/jeremytammik/RevitLookup/pull/64) by @peterhirn to update CI for Revit 2021
- 2020-10-20 **2021.0.0.6** eliminated deprecated unit api usage
- 2020-10-20 **2021.0.0.5** integrated pull request [#63](https://github.com/jeremytammik/RevitLookup/pull/63) by @swfaust to update command registration and remove obsolete test framework command
- 2020-04-14 **2021.0.0.1** integrated pull request [#58](https://github.com/jeremytammik/RevitLookup/pull/58) by @harrymattison with solution changes for multi-release building
- 2020-04-12 **2021.0.0.0** flat migration to Revit 2021
- 2020-02-11 **2020.0.0.4** incremented copyright year
- 2019-08-20 **2020.0.0.3** integrated pull request [#56](https://github.com/jeremytammik/RevitLookup/pull/56) by @nonoesp - fix two small typos in readme
- 2019-06-03 **2020.0.0.2** integrated pull request [#53](https://github.com/jeremytammik/RevitLookup/pull/53) by @CADBIMDeveloper - list available values for ParameterType.FamilyType and FamilyParameters titles
- 2019-04-26 **2020.0.0.1** integrated pull request [#52](https://github.com/jeremytammik/RevitLookup/pull/52) by @CADBIMDeveloper
- 2019-04-18 **2020.0.0.0** flat migration to Revit 2020
- 2019-04-18 **2019.0.0.13** added MSI installer for Revit 2017-2020 by Harry Mattison
- 2019-03-27 **2019.0.0.12** added MSI installer for 2018.0.0.0 submitted by @VBScab in issue [#51](https://github.com/jeremytammik/RevitLookup/issues/51)
- 2019-03-26 **2019.0.0.11** integrated pull request [#50](https://github.com/jeremytammik/RevitLookup/pull/50) by Victor Chekalin to handle DoubleArray4d values
- 2019-03-25 **2019.0.0.10** integrated pull requests [#48](https://github.com/jeremytammik/RevitLookup/pull/48) and [#49](https://github.com/jeremytammik/RevitLookup/pull/49) by Victor Chekalin to snoop rendering AssetProperty via Material-AppearanceAssetId-GetRenderingAssset
- 2019-03-18 **2019.0.0.9** added CmdSnoopModScopeDependents
- 2019-03-18 **2019.0.0.8** cleanup before adding CmdSnoopModScopeDependents
- 2019-01-21 **2019.0.0.7** fixed typo in variable name reported by @yk35 in pull request [#47](https://github.com/jeremytammik/RevitLookup/pull/47)
- 2019-01-17 **2019.0.0.6** added new commands by H�vard Leding: pick surface, edge, linked element
- 2019-01-09 **2019.0.0.5** incremented copyright year to 2019
- 2018-12-13 **2019.0.0.4** merged issue [#45](https://github.com/jeremytammik/RevitLookup/issues/45) and pull request [#46](https://github.com/jeremytammik/RevitLookup/pull/46) from @TheKidMSX to center parent for forms
- 2018-05-29 **2019.0.0.2** merged pull request [#43](https://github.com/jeremytammik/RevitLookup/pull/43) from Levente Koncz @palver123 to use ProgramW6432 variable in csproj to locate Revit API assembly DLLs
- 2018-04-15 **2019.0.0.0** flat migration to Revit 2019
- 2018-03-12 **2018.0.0.8** merged pull request [#42](https://github.com/jeremytammik/RevitLookup/pull/42) from @Modis Pekshev: Add "Search by and snoop" command
- 2018-03-02 **2018.0.0.7** merged pull request [#41](https://github.com/jeremytammik/RevitLookup/pull/41) from @Modis Pekshev: Add ConvertToStableRepresentation method for References
- 2018-01-05 **2018.0.0.6** incremented copyright year to 2018
- 2018-01-05 **2018.0.0.5** readme enhancements: badges, installer and updated link to MSI installer
- 2017-08-28 **2018.0.0.3** merged pull request [#36](https://github.com/jeremytammik/RevitLookup/pull/36) from @Andrey-Bushman: switch target platform to.Net 4.6 and replace Revit 2017 NuGet package by Revit 2018.1 Nuget package
- 2017-06-05 **2018.0.0.1** merged pull request [#34](https://github.com/jeremytammik/RevitLookup/pull/34) from @CADBIMDeveloper: annotative family instance geometry, element enumerations instead of ids, parameter names and byte property values
- 2017-04-21 **2018.0.0.0** flat migration to Revit 2018
- 2017-04-07 **2017.0.0.24** merged pull request [#33](https://github.com/jeremytammik/RevitLookup/pull/33) by @peterhirn added build status badge
- 2017-03-27 **2017.0.0.23** dummy modification to trigger build for https://lookupbuilds.com cf. https://forums.autodesk.com/t5/revit-api-forum/ci-for-revit-lookup/m-p/6947111
- 2017-03-17 **2017.0.0.22** added 'new' keyword to avoid warning and override inherited methods
- 2017-03-17 **2017.0.0.21** merged pull request [#31](https://github.com/jeremytammik/RevitLookup/pull/31) from @CADBIMDeveloper removing try-catch handler
- 2017-03-16 **2017.0.0.20** merged pull request [#30](https://github.com/jeremytammik/RevitLookup/pull/30) from @eirannejad adding icon and exception handling
- 2017-03-15 **2017.0.0.19** merged pull request [#29](https://github.com/jeremytammik/RevitLookup/pull/29) from @CADBIMDeveloper fixing bugs initialising type and opening background documents
- 2017-03-02 **2017.0.0.18** merged pull request [#27](https://github.com/jeremytammik/RevitLookup/pull/27) from @CADBIMDeveloper to display category BuiltInCategory, nullable double properties and empty lists
- 2017-02-21 **2017.0.0.17** merged pull request [#26](https://github.com/jeremytammik/RevitLookup/pull/26) from Alexander Ignatovich to restore ability to see extensible storage content
- 2017-02-17 **2017.0.0.16** merged pull request [#25](https://github.com/jeremytammik/RevitLookup/pull/25) from chekalin-v: fix old bugs, significant improvements to the new reflection approach
- 2017-02-06 **2017.0.0.15** merged pull request [#23](https://github.com/jeremytammik/RevitLookup/pull/23) from awmcc90 to catch specific reflection invocation exceptions, not all
- 2017-02-06 **2017.0.0.14** merged pull request [#22](https://github.com/jeremytammik/RevitLookup/pull/22) from awmcc90 drastic changes implementing object inspection via reflection and cross-version compatibility
- 2017-02-02 **2017.0.0.13** whitespace
- 2017-02-02 **2017.0.0.12** merged pull [#21](https://github.com/jeremytammik/RevitLookup/pull/21) from @eibre adding UnitType property on the parameter Definition class
- 2017-01-06 **2017.0.0.11** fixed issue [#19](https://github.com/jeremytammik/RevitLookup/issues/19) raised by LeeJaeYoung spot dimension position and text position error
- 2017-01-06 **2017.0.0.10** merged pull [#20](https://github.com/jeremytammik/RevitLookup/pull/20) from @luftbanana supporting close-with-ESC to all forms by assigning the cancel button
- 2017-01-03 **2017.0.0.9** incremented copyright year
- 2016-12-20 **2017.0.0.8** added version number to Hello World message box
- 2016-12-20 **2017.0.0.7** merged pull request [#18](https://github.com/jeremytammik/RevitLookup/pull/18) by @Andrey-Bushman to use NuGet Revit API package
- 2016-08-05 **2017.0.0.6** merged pull request [#16](https://github.com/jeremytammik/RevitLookup/pull/16) by @arif-hanif to add post build event to project file to copy addin manifest and dll to addins folder
- 2016-06-23 **2017.0.0.5** merged pull request [#14](https://github.com/jeremytammik/RevitLookup/pull/14) by Shayneham to handle exceptions snooping flex pipe and duct lacking levels etc.
- 2016-06-04 **2017.0.0.4** merged pull request [#13](https://github.com/jeremytammik/RevitLookup/pull/13) by awmcc90 to skip mepSys.Elements for OST_ElectricalInternalCircuits category
- 2016-06-04 **2017.0.0.3** before merging pull request [#13](https://github.com/jeremytammik/RevitLookup/pull/13) by awmcc90 to skip mepSys.Elements
- 2016-04-19 **2017.0.0.2** ready for publication
- 2016-04-15 **2017.0.0.1** microscopic cleanup
- 2016-04-15 **2017.0.0.0** migration to Revit 2017 Manuel of Sofistik
- 2016-04-04 **2016.0.0.13** incremented copyright year from 2015 to 2016
- 2015-10-22 **2016.0.0.12** readme cleanup
- 2015-09-15 **2016.0.0.11** implemented support for Element bounding box
- 2015-09-01 **2016.0.0.10** handle null floor.GetAnalyticalModel returned in RAC and RME
- 2015-05-21 **2016.0.0.9** display all the display names of the BuiltInParameter enumeration value
- 2015-05-15 **2016.0.0.8** updated Revit API assembly paths for final release of Revit 2016
- 2015-04-23 **2016.0.0.7** updated post-build event target path to Revit 2016 add-ins folder
- 2015-04-21 **2016.0.0.6** set Copy Local false on Revit API assemblies
- 2015-04-21 **2016.0.0.5** initial migration to Revit 2016 - first public release
- 2015-04-20 **2016.0.0.4** integrated changes from previous Revit 2016 version into elaine's one
- 2015-04-20 **2016.0.0.3** initial migration to Revit 2016 by @ElaineJieyanZheng
- 2015-04-19 **2015.0.0.8** integrated pull request #6 by yzraeu, additinal try catch for Level Offset and MEP System
- 2015-01-30 **2015.0.0.7** removed all statements 'using' the empty root namespace Autodesk.Revit
- 2015-01-29 **2015.0.0.6** incremented copyright message year from 2014 to 2015
- 2015-01-13 **2015.0.0.5** added CategoryType to the CategoryCollector
- 2014-11-24 **2015.0.0.4** encapsulate transaction in using statement
- 2014-11-24 **2015.0.0.3** merged fix by Tom Pesman @tompesman to catch exception thrown by doc.PrintManager
- 2014-10-06 **2015.0.0.2** removed obsolete Revit API usage to compile with zero errors and zero warnings
- 2014-04-17 **2015.0.0.1** recompiled for Revit 2015 UR1
- 2014-04-02 **2015.0.0.0** initial migration to Revit 2015 Meridian prerelease PR10
- 2014-01-28 **2014.0.1.0** double checked that this version corresponds with florian's
- 2014-01-27 **2014.0.0.7** merged pull request from FlorianSchmid of SOFiSTiK: extended (added) snooping of geometry, FormatOptions and RevitLinkInstances plus some fixes of compiler errors/warnings; bumped copyright year from 2013 to 2014
- 2014-01-11 **2014.0.0.6** joespiff adjusted the Anchor property of the "Built-in Enums Map..." button on the Parameters form so that it behaves well when stretching the form
- 2013-10-24 **2014.0.0.5** merged pull request from Prasadgalle