#region Header
//
// Copyright 2003-2021 by Autodesk, Inc. 
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

using System;
using System.Diagnostics;
using Autodesk.Revit.DB;


namespace RevitLookup
{
	/// <summary>
	/// Summary description for GeomUtils.
	/// </summary>
	
	public class GeomUtils
	{
// Revit PI   = 3.14159265358979
// AutoCAD PI = 3.14159265358979323846;
// Math.Pi    = 3.14159265358979323846;

            // predefined constants for common angles
        public const double Pi       = 3.14159265358979323846;
        // TBD: have to use Revit's version of Pi for Ellipse else it will fail!
        public const double RevitPi   = 3.14159265358979;  
        public const double HalfPi   = 3.14159265358979323846 / 2.0;
        public const double TwoPi	  = 3.14159265358979323846 * 2.0;

        public const double Rad0     = 0.0;
        public const double Rad45    = 3.14159265358979323846 / 4.0;
        public const double Rad90    = 3.14159265358979323846 / 2.0;
        public const double Rad135   = (3.14159265358979323846 * 3.0) / 4.0;
        public const double Rad180   = 3.14159265358979323846;
        public const double Rad270   = 3.14159265358979323846 * 1.5;
        public const double Rad360   = 3.14159265358979323846 * 2.0;
        
            // predefined values for common Points and Vectors
        public static readonly XYZ Origin = new XYZ(0.0, 0.0, 0.0);
        public static readonly XYZ XAxis  = new XYZ(1.0, 0.0, 0.0);
        public static readonly XYZ YAxis  = new XYZ(0.0, 1.0, 0.0);
        public static readonly XYZ ZAxis  = new XYZ(0.0, 0.0, 1.0);

		public GeomUtils()
		{
		}
		
        public static double
        RadiansToDegrees(double rads)
        {
            return rads * (180.0 / Pi);
        }
        
        public static double
        DegreesToRadians(double degrees)
        {
            return degrees * (Pi / 180.0);
        }
        
        public static XYZ
        Midpoint(XYZ pt1, XYZ pt2)
        {
            var newPt = new XYZ(((pt1.X + pt2.X) / 2.0),
                                        ((pt1.Y + pt2.Y) / 2.0),
                                        ((pt1.Z + pt2.Z) / 2.0));

            return newPt;
        }

        /// <summary>
        /// Given two points and an axis, returns the 
        /// point with the greater value along the axis
        /// </summary>
        /// <param name="pt1">point 1 to compare</param>
        /// <param name="pt2">point 2 to compare</param>
        /// <param name="axis">axis to compare along</param>
        /// <returns></returns>
        public static XYZ
        Greater (XYZ pt1, XYZ pt2, XYZ axis)
        {
            var pt = new XYZ();

            if(axis.Equals(XAxis)){
                if (pt1.X > pt2.X)
                    pt = pt1;
                else
                    pt = pt2;
            }
            if (axis.Equals(YAxis)) {
                if (pt1.Y > pt2.Y)
                    pt = pt1;
                else
                    pt = pt2;
            }
            if (axis.Equals(ZAxis)) {
                if (pt1.Z > pt2.Z)
                    pt = pt1;
                else
                    pt = pt2;
            }

            return pt;
        }

        /// <summary>
        /// given an array of pts, find the closest
        /// pt to a given pt
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static XYZ
        GetClosestPt (XYZ pt, System.Collections.Generic.IList<XYZ> pts)
        {
            
            var closestPt = new XYZ();
            var closestDist = 0.0;

            foreach( var ptTemp in pts )
            {
                /// don't consider the pt itself
                if (pt.Equals(ptTemp))
                    continue;

                var dist = Math.Sqrt(Math.Pow((pt.X - ptTemp.X), 2.0) +
                                     Math.Pow((pt.Y - ptTemp.Y), 2.0) +
                                     Math.Pow((pt.Z - ptTemp.Z), 2.0));

                if (closestPt.IsZeroLength()) {
                    closestDist = dist;
                    closestPt = ptTemp;
                }
                else {
                    if (dist < closestDist) {
                        closestDist = dist;
                        closestPt = ptTemp;
                    }
                }
            }
            return closestPt;
        }
	}
}
