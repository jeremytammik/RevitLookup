# RevitLookup

<p align="center">
  <img src="https://img.shields.io/badge/Revit%20API-2022-blue.svg?style=for-the-badge">
  <a href="http://opensource.org/licenses/MIT"><img src="https://img.shields.io/github/license/jeremytammik/RevitLookup?style=for-the-badge"></a>
  <a href="https://github.com/jeremytammik/RevitLookup/releases/latest"><img src="https://img.shields.io/github/downloads/jeremytammik/RevitLookup/total?style=for-the-badge"></a>
  <a href="https://actions-badge.atrox.dev/jeremytammik/RevitLookup/goto?ref=master"><img src="https://img.shields.io/endpoint.svg?url=https%3A%2F%2Factions-badge.atrox.dev%2Fjeremytammik%2FRevitLookup%2Fbadge%3Fref%3Dmaster&style=for-the-badge"></a>
</p>

Interactive Revit `RFA` and `RVT` project database exploration tool to view and navigate BIM element parameters, properties and relationships.

Please refer to [The Building Coder](http://thebuildingcoder.typepad.com) for more information.

## Installation

- Go to the [**Releases**](https://github.com/jeremytammik/RevitLookup/releases/latest) section.
- Download and run MSI file.

The installer contains all the latest versions starting from the 2015 version of Revit.

## Build

Debugging:

- Run **Debug Profile** in Visual Studio or **Run Configuration** in JetBrains Rider. The required files have been added. All project files will be automatically copied to the
  Revit plugins folder.

Creating a package:

- Open the terminal of your IDE.
- Install Nuke global tools `dotnet tool install Nuke.GlobalTool --global`.
- Run `nuke` command.
- The generated package will be in the **output** folder.

For more information on building, see the [**RevitTemplates**](https://github.com/Nice3point/RevitTemplates) Wiki page.

The project currently supports the 2022 version of Revit. You can find the source code of previous versions at the links below:

- [latest release](https://github.com/jeremytammik/RevitLookup/releases/latest) for Revit 2022
- [2021.0.0.13](https://github.com/jeremytammik/RevitLookup/releases/tag/2021.0.0.13) for Revit 2021
- [2020.0.0.4](https://github.com/jeremytammik/RevitLookup/releases/tag/2020.0.0.4) for Revit 2020
- [2019.0.0.13](https://github.com/jeremytammik/RevitLookup/releases/tag/2019.0.0.13) for Revit 2019
- [2018.0.0.8](https://github.com/jeremytammik/RevitLookup/releases/tag/2018.0.0.8) for Revit 2018
- [2017.0.0.24](https://github.com/jeremytammik/RevitLookup/releases/tag/2017.0.0.24) for Revit 2017
- [2016.0.0.13](https://github.com/jeremytammik/RevitLookup/releases/tag/2016.0.0.13) for Revit 2016
- [2015.0.0.8](https://github.com/jeremytammik/RevitLookup/releases/tag/2015.0.0.8) for Revit 2015

Please refer to the [changelog](Doc/Changelog.md) for details.

## Caveat &ndash; RevitLookup Cannot Snoop Everything

This clarification was prompted by the [issue #35 &ndash; RevitLookup doesn't snoop all members](https://github.com/jeremytammik/RevitLookup/issues/35):

**Question:** I tried snooping a selected Structural Rebar element in the active view and found not all of the Rebar class members showed up in the Snoop Objects window. One of
many members that weren't there: `Rebar.GetFullGeometryForView` method.

Is this the expected behaviour? I was thinking I could get all object members just with RevitLookup and without the Revit API help file `RevitAPI.chm`.

**Answer:** RevitLookup cannot report **all** properties and methods on **all** elements.

For instance, in the case of `GetFullGeometryForView`, a view input argument is required. How is RevitLookup supposed to be able to guess what view you are interested in?

For methods requiring dynamic input that cannot be automatically determined, you can
[make use of more intimate interactive database exploration tools, e.g. RevitPythonShell](http://thebuildingcoder.typepad.com/blog/2013/11/intimate-revit-database-exploration-with-the-python-shell.html).

## Contributing

Contributions are more than welcome! Please work in the `dev` branch to do so:

- Create or update your own fork of RevitLookup under your GitHub account.
- Checkout to the `dev` branch.
- In the `dev` branch, implement and test you changes specific to the feature.
- Build the project and make sure everything works.
- Create well-documented commits of your changes.
- Submit a pull request to the `origin:dev` branch.

Please avoid:

- Lots of unrelated changes in one commit.
- Modifying files that are not directly related to the feature you implement.

## Author

Originally implemented by Jim Awe and the Revit API development team at Autodesk.

Maintained by Jeremy Tammik,
[The Building Coder](http://thebuildingcoder.typepad.com) and
[The 3D Web Coder](http://the3dwebcoder.typepad.com),
[Forge](http://forge.autodesk.com) [Platform](https://developer.autodesk.com) Development,
[ADN](http://www.autodesk.com/adn)
[Open](http://www.autodesk.com/adnopen),
[Autodesk Inc.](http://www.autodesk.com), with invaluable [contributions](https://github.com/jeremytammik/RevitLookup/graphs/contributors)
from the entire Revit add-in developer community.

Thank you, guys!

## License

This sample is licensed under the terms of the [MIT License](http://opensource.org/licenses/MIT). Please see the [License](License.md) file for full details.

Credit to [icons8.com](https://icons8.com) for the RevitLookup icons.

## Technology Sponsors

Thanks to [JetBrains](https://jetbrains.com) for providing licenses for [Rider](https://jetbrains.com/rider), which both make open-source development a real pleasure!

[<img src="https://user-images.githubusercontent.com/20504884/144722493-6fe08637-b5ef-4e71-aa19-faef9f45021f.png" height="120" al />](https://jetbrains.com/)