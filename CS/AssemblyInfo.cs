#region Header
//
// Copyright 2003-2017 by Autodesk, Inc. 
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//
#endregion // Header

using System.Reflection;
using System.Runtime.CompilerServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle( "RevitLookup" )]
[assembly: AssemblyDescription( "Revit add-in interactive BIM database exploration tool to view and navigate element properties and relationships." )]
[assembly: AssemblyConfiguration( "" )]
[assembly: AssemblyCompany( "Autodesk Inc." )]
[assembly: AssemblyProduct( "RevitLookup" )]
[assembly: AssemblyCopyright( "Copyright 2003-2017 (C) Autodesk Inc." )]
[assembly: AssemblyTrademark( "" )]
[assembly: AssemblyCulture( "" )]

//
// In order to sign your assembly you must specify a key to use. Refer to the
// Microsoft .NET Framework documentation for more information on assembly signing.
//
// Use the attributes below to control which key is used for signing.
//
// Notes:
//   (*) If no key is specified, the assembly is not signed.
//   (*) KeyName refers to a key that has been installed in the Crypto Service
//       Provider (CSP) on your machine. KeyFile refers to a file which contains
//       a key.
//   (*) If the KeyFile and the KeyName values are both specified, the
//       following processing occurs:
//       (1) If the KeyName can be found in the CSP, that key is used.
//       (2) If the KeyName does not exist and the KeyFile does exist, the key
//           in the KeyFile is installed into the CSP and used.
//   (*) In order to create a KeyFile, you can use the sn.exe (Strong Name) utility.
//       When specifying the KeyFile, the location of the KeyFile should be
//       relative to the project output directory which is
//       %Project Directory%\obj\<configuration>. For example, if your KeyFile is
//       located in the project directory, you would specify the AssemblyKeyFile
//       attribute as [assembly: AssemblyKeyFile("..\\..\\mykey.snk")]
//   (*) Delay Signing is an advanced option - see the Microsoft .NET Framework
//       documentation for more information on this.
//
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("")]
[assembly: AssemblyKeyName("")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
// 2013-10-24 - 2014.0.0.5 - merged pull request from Prasadgalle
// 2014-01-11 - 2014.0.0.6 - joespiff adjusted the Anchor property of the "Built-in Enums Map..." button on the Parameters form so that it behaves well when stretching the form
// 2014-01-27 - 2014.0.0.7 - merged pull request from FlorianSchmid of SOFiSTiK: extended (added) snooping of geometry, FormatOptions and RevitLinkInstances plus some fixes of compiler errors/warnings; bumped copyright year from 2013 to 2014
// 2014-01-28 - 2014.0.1.0 - double checked that this version corresponds with florian's
// 2014-04-02 - 2015.0.0.0 - initial migration to Revit 2015 Meridian prerelease PR10
// 2014-04-17 - 2015.0.0.1 - recompiled for Revit 2015 UR1
// 2014-10-06 - 2015.0.0.2 - removed obsolete Revit API usage to compile with zero errors and zero warnings
// 2014-11-24 - 2015.0.0.3 - merged fix by Tom Pesman @tompesman to catch exception thrown by doc.PrintManager
// 2014-11-24 - 2015.0.0.4 - encapsulate transaction in using statement
// 2015-01-13 - 2015.0.0.5 - added CategoryType to the CategoryCollector
// 2015-01-29 - 2015.0.0.6 - incremented copyright message year from 2014 to 2015
// 2015-01-30 - 2015.0.0.7 - removed all statements 'using' the empty root namespace Autodesk.Revit
// 2015-04-19 - 2015.0.0.8 - integrated pull request #6 by yzraeu, additinal try catch for Level Offset and MEP System
// 2015-04-20 - 2016.0.0.3 - initial migration to Revit 2016 by @ElaineJieyanZheng
// 2015-04-20 - 2016.0.0.4 - integrated changes from previous Revit 2016 version into elaine's one
// 2015-04-21 - 2016.0.0.5 - initial migration to Revit 2016 - first public release
// 2015-04-21 - 2016.0.0.6 - set Copy Local false on Revit API assemblies
// 2015-04-23 - 2016.0.0.7 - updated post-build event target path to Revit 2016 add-ins folder
// 2015-05-15 - 2016.0.0.8 - updated Revit API assembly paths for final release of Revit 2016
// 2015-05-21 - 2016.0.0.9 - display all the display names of the BuiltInParameter enumeration value
// 2015-09-01 - 2016.0.0.10 - handle null floor.GetAnalyticalModel returned in RAC and RME 
// 2015-09-15 - 2016.0.0.11 - implemented support for Element bounding box
// 2015-10-22 - 2016.0.0.12 - readme cleanup
// 2016-04-04 - 2016.0.0.13 - incremented copyright year from 2015 to 2016
// 2016-04-15 - 2017.0.0.0 - migration to Revit 2017 by manuel of sofistik
// 2016-04-15 - 2017.0.0.1 - microscopic cleanup
// 2016-04-19 - 2017.0.0.2 - ready for publication
// 2016-06-04 - 2017.0.0.3 - before merging pull request #13 by awmcc90 to skip mepSys.Elements
// 2016-06-04 - 2017.0.0.4 - merged pull request #13 by awmcc90 to skip mepSys.Elements for OST_ElectricalInternalCircuits category
// 2016-06-23 - 2017.0.0.5 - merged pull request #14 by Shayneham to handle exceptions snooping flex pipe and duct lacking levels etc.
// 2016-08-05 - 2017.0.0.6 - merged pull request #16 by @arif-hanif to add post build event to project file to copy addin manifest and dll to addins folder
// 2016-12-20 - 2017.0.0.7 - merged pull request #18 by @Andrey-Bushman to use NuGet Revit API package
// 2016-12-20 - 2017.0.0.8 - added version number to Hello World message box
// 2017-01-03 - 2017.0.0.9 - incremented copyright year
//
[assembly: AssemblyVersion( "2017.0.0.9" )]
[assembly: AssemblyFileVersion( "2017.0.0.9" )]
