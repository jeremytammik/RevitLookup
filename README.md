# RevitLookup

![Revit API](https://img.shields.io/badge/Revit%20API-2020-blue.svg)
![Platform](https://img.shields.io/badge/platform-Windows-lightgray.svg)
![.NET](https://img.shields.io/badge/.NET-4.7-blue.svg)
[![License](http://img.shields.io/:license-mit-blue.svg)](http://opensource.org/licenses/MIT)
[![Build Status](https://s3-eu-west-1.amazonaws.com/lookup-builds/extra/build_status.svg)](https://lookupbuilds.com)

Interactive Revit BIM database exploration tool to view and navigate element properties and relationships.

Please refer to [The Building Coder](http://thebuildingcoder.typepad.com) for more information.


## <a name="versions"></a> Versions

The most up-to-date version provided here is for Revit 2020.

If you are interested in an earlier release of Revit, please grab the latest appropriate one from the
[release list](https://github.com/jeremytammik/RevitLookup/releases), e.g.:

- [2019.0.0.13](https://github.com/jeremytammik/RevitLookup/releases/tag/2019.0.0.13) for Revit 2019
- [2018.0.0.8](https://github.com/jeremytammik/RevitLookup/releases/tag/2018.0.0.8) for Revit 2018
- [2017.0.0.24](https://github.com/jeremytammik/RevitLookup/releases/tag/2017.0.0.24) for Revit 2017
- [2016.0.0.13](https://github.com/jeremytammik/RevitLookup/releases/tag/2016.0.0.13) for Revit 2016
- [2015.0.0.8](https://github.com/jeremytammik/RevitLookup/releases/tag/2015.0.0.8) for Revit 2015


## <a name="builds"></a> Builds

Peter Hirn of [Build Informed GmbH](https://www.buildinformed.com) very kindly set up a
public [CI](https://en.wikipedia.org/wiki/Continuous_integration) for RevitLookup
at [lookupbuilds.com](https://lookupbuilds.com)
using [Jenkins](https://jenkins.io/index.html) in
a multi-branch project configuration to build all branches and tags from the GitHub repository.
The output is dual-signed with the Build Informed certificate, zipped and published to an Amazon S3 bucket.
For more information, please refer to 
the [Revit API discussion forum](http://forums.autodesk.com/t5/revit-api-forum/bd-p/160) thread
on [CI for RevitLookup](https://forums.autodesk.com/t5/revit-api-forum/ci-for-revit-lookup/m-p/6947111).

Peter also added the build status badge at the top of this page.

Thank you very much, Peter!


## Installation

You install RevitLookup just like any other Revit add-in,
by [copying the add-in manifest and the assembly DLL to the Revit Add-Ins folder](http://help.autodesk.com/view/RVT/2019/ENU/?guid=Revit_API_Revit_API_Developers_Guide_Introduction_Add_In_Integration_Add_in_Registration_html).

<!----
by [copying the add-in manifest and the assembly DLL to the Revit Add-Ins folder](http://help.autodesk.com/view/RVT/2018/ENU/?guid=GUID-4FFDB03E-6936-417C-9772-8FC258A261F7).
---->


If you specify the full DLL pathname in the add-in manifest, it can also be located elsewhere.

For more information on installing Revit add-ins in general, please refer to
the [Revit API getting started material](http://thebuildingcoder.typepad.com/blog/about-the-author.html#2).

Harry Mattison of [Boost your BIM](https://boostyourbim.wordpress.com) very kindly provides
a ready-built [RevitLookup 2018 installer](https://boostyourbim.wordpress.com/2017/04/28/revit-lookup-2018-install):

> If you don’t want to deal with source code and just want to use the tool, here
is [Revit Lookup 2018.msi](https://drive.google.com/open?id=182W00Mk5Hj1FMHAo-xVnoFYlJ_s2Swrw),
an installer for the compiled and signed DLL ready, courtesy of Boost Your BIM.

Harry's installer was cleaned up
by [@VBScab](https://github.com/VBScab) and submitted to this repository
in [issue #51 &ndash; the MSI in the project is flawed](https://github.com/jeremytammik/RevitLookup/issues/51).
Here is the [cleaned-up RevitLookup 2018.0.0.0 installer](installer/revit_lookup_2018.0.0.0.msi).
However, please note that other, more recent builds exist for Revit 2018.
As explained above, they can be downloaded sans installer
from [lookupbuilds.com](https://lookupbuilds.com).

Harry shared a new installer in his post
on [RevitLookup for Revit 2020](https://boostyourbim.wordpress.com/2019/04/16/revit-lookup-for-revit-2020),
which I also added to the [installer folder](installer)
in  [RevitLookup2017-2020.msi](installer/RevitLookup2017-2020.msi).
Says he:

> I’ve added some conditional compilation and multiple configurations so that the single solution can be used to build against any version of Revit 2017-2020.

<a name="caveat"></a>
## Caveat &ndash; RevitLookup Cannot Snoop Everything

This clarification was prompted by 
the [issue #35 &ndash; RevitLookup doesn't snoop all members](https://github.com/jeremytammik/RevitLookup/issues/35):

**Question:** I tried snooping a selected Structual Rebar element in the active view and found not all of the Rebar class members showed up in the Snoop Objects window. One of many members that weren't there: `Rebar.GetFullGeometryForView` method.

Is this the expected behaviour? I was thinking I could get all object members just with  RevitLookup and without the Revit API help file `RevitAPI.chm`.

**Answer:** RevitLookup cannot report **all** properties and methods on **all** elements.

For instance, in the case of GetFullGeometryForView, a view input argument is required. How is RevitLookup supposed to be able to guess what view you are interested in?

For methods requiring dynamic input that cannot be automatically determined, you will have to [make use of more intimate interactive database exploration tools, e.g. RevitPythonShell](http://thebuildingcoder.typepad.com/blog/2013/11/intimate-revit-database-exploration-with-the-python-shell.html).


## Author

Implemented by Jim Awe and the Revit API developement team at Autodesk.

Maintained by Jeremy Tammik,
[The Building Coder](http://thebuildingcoder.typepad.com) and
[The 3D Web Coder](http://the3dwebcoder.typepad.com),
[Forge](http://forge.autodesk.com) [Platform](https://developer.autodesk.com) Development,
[ADN](http://www.autodesk.com/adn)
[Open](http://www.autodesk.com/adnopen),
[Autodesk Inc.](http://www.autodesk.com),
with invaluable contributions from the entire Revit add-in developer community.

Thank you, guys!


## License

This sample is licensed under the terms of the [MIT License](http://opensource.org/licenses/MIT).
Please see the [LICENSE](LICENSE) file for full details.

Credit to [icons8.com](https://icons8.com) for the RevitLookup icons.

