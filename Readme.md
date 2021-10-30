# RevitLookup

<p align="center">
  <img src="https://img.shields.io/badge/Revit%20API-2022-blue.svg?style=for-the-badge">
  <img src="https://img.shields.io/badge/platform-Windows-lightgray.svg?style=for-the-badge">
  <img src="https://img.shields.io/badge/.NET-4.8-blue.svg?style=for-the-badge">
  <a href="http://opensource.org/licenses/MIT"><img src="https://img.shields.io/github/license/jeremytammik/RevitLookup?style=for-the-badge"></a>
  <a href="https://actions-badge.atrox.dev/jeremytammik/RevitLookup/goto"><img src="https://img.shields.io/endpoint.svg?url=https%3A%2F%2Factions-badge.atrox.dev%2Fjeremytammik%2FRevitLookup%2Fbadge&style=for-the-badge"></a>
</p>

Interactive Revit BIM database exploration tool to view and navigate BIM element parameters, properties and relationships.

Please refer to [The Building Coder](http://thebuildingcoder.typepad.com) for more information.


## Versions

The project currently supports the 2022 version of Revit.
You can find the source code of previous versions at the links below:
- [latest release](https://github.com/jeremytammik/RevitLookup/releases/latest) for Revit 2022
- [2021.0.0.13](https://github.com/jeremytammik/RevitLookup/releases/tag/2021.0.0.13) for Revit 2021
- [2020.0.0.4](https://github.com/jeremytammik/RevitLookup/releases/tag/2020.0.0.4) for Revit 2020
- [2019.0.0.13](https://github.com/jeremytammik/RevitLookup/releases/tag/2019.0.0.13) for Revit 2019
- [2018.0.0.8](https://github.com/jeremytammik/RevitLookup/releases/tag/2018.0.0.8) for Revit 2018
- [2017.0.0.24](https://github.com/jeremytammik/RevitLookup/releases/tag/2017.0.0.24) for Revit 2017
- [2016.0.0.13](https://github.com/jeremytammik/RevitLookup/releases/tag/2016.0.0.13) for Revit 2016
- [2015.0.0.8](https://github.com/jeremytammik/RevitLookup/releases/tag/2015.0.0.8) for Revit 2015

Please refer to the [changelog](Doc/Changelog.md) for details.

## Installation

* Go to the [**Git Actions**](https://github.com/jeremytammik/RevitLookup/actions/workflows/CreatePackage.yml) section on the project page.
* Open the most recent successful workflow.
* Download and open **RevitLookup.zip** from the **Artifacts** section.
* Run the MSI package.

The installer contains all the latest versions starting from the 2015 version of Revit.

## Build

* To build the package on your local computer, open the terminal of your IDE.
* Install Nuke global tools `dotnet tool install Nuke.GlobalTool --global`.
* Run `nuke` command.
* The generated package will be in the **output** folder.
* For debugging, run the build or Run configuration of your IDE. For Visual Studio and JetBrains Rider, the required configurations have been added. All required files will be automatically copied to the Revit plugins folder.

For more information on building, see the [**RevitTemplates**](https://github.com/Nice3point/RevitTemplates) Wiki page.

## Caveat &ndash; RevitLookup Cannot Snoop Everything

This clarification was prompted by
the [issue #35 &ndash; RevitLookup doesn't snoop all members](https://github.com/jeremytammik/RevitLookup/issues/35):

**Question:** I tried snooping a selected Structural Rebar element in the active view and found not all of the Rebar class members showed up in the Snoop Objects window. One of many members that weren't there: `Rebar.GetFullGeometryForView` method.

Is this the expected behaviour? I was thinking I could get all object members just with  RevitLookup and without the Revit API help file `RevitAPI.chm`.

**Answer:** RevitLookup cannot report **all** properties and methods on **all** elements.

For instance, in the case of `GetFullGeometryForView`, a view input argument is required.
How is RevitLookup supposed to be able to guess what view you are interested in?

For methods requiring dynamic input that cannot be automatically determined, you can [make use of more intimate interactive database exploration tools, e.g. RevitPythonShell](http://thebuildingcoder.typepad.com/blog/2013/11/intimate-revit-database-exploration-with-the-python-shell.html).


## Author

Implemented by Jim Awe and the Revit API development team at Autodesk.

Maintained by Jeremy Tammik,
[The Building Coder](http://thebuildingcoder.typepad.com) and
[The 3D Web Coder](http://the3dwebcoder.typepad.com),
[Forge](http://forge.autodesk.com) [Platform](https://developer.autodesk.com) Development,
[ADN](http://www.autodesk.com/adn)
[Open](http://www.autodesk.com/adnopen),
[Autodesk Inc.](http://www.autodesk.com),
with invaluable [contributions](https://github.com/jeremytammik/RevitLookup/graphs/contributors) from
the entire Revit add-in developer community.

Thank you, guys!


## License

This sample is licensed under the terms of the [MIT License](http://opensource.org/licenses/MIT).
Please see the [LICENSE](License.md) file for full details.

Credit to [icons8.com](https://icons8.com) for the RevitLookup icons.

