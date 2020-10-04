#region Header
//
// Copyright 2003-2020 by Autodesk, Inc. 
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
        public const double kPi       = 3.14159265358979323846;
        // TBD: have to use Revit's version of Pi for Ellipse else it will fail!
        public const double kRevitPi   = 3.14159265358979;  
        public const double kHalfPi   = 3.14159265358979323846 / 2.0;
        public const double kTwoPi	  = 3.14159265358979323846 * 2.0;

        public const double kRad0     = 0.0;
        public const double kRad45    = 3.14159265358979323846 / 4.0;
        public const double kRad90    = 3.14159265358979323846 / 2.0;
        public const double kRad135   = (3.14159265358979323846 * 3.0) / 4.0;
        public const double kRad180   = 3.14159265358979323846;
        public const double kRad270   = 3.14159265358979323846 * 1.5;
        public const double kRad360   = 3.14159265358979323846 * 2.0;
        
            // predefined values for common Points and Vectors
        public static readonly XYZ kOrigin = new XYZ(0.0, 0.0, 0.0);
        public static readonly XYZ kXAxis  = new XYZ(1.0, 0.0, 0.0);
        public static readonly XYZ kYAxis  = new XYZ(0.0, 1.0, 0.0);
        public static readonly XYZ kZAxis  = new XYZ(0.0, 0.0, 1.0);

		public GeomUtils()
		{
		}
		
        public static double
        RadiansToDegrees(double rads)
        {
            return rads * (180.0 / kPi);
        }
        
        public static double
        DegreesToRadians(double degrees)
        {
            return degrees * (kPi / 180.0);
        }
        
        public static XYZ
        Midpoint(XYZ pt1, XYZ pt2)
        {
            XYZ newPt = new XYZ(((pt1.X + pt2.X) / 2.0),
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
            XYZ pt = new XYZ();

            if(axis.Equals(kXAxis)){
                if (pt1.X > pt2.X)
                    pt = pt1;
                else
                    pt = pt2;
            }
            if (axis.Equals(kYAxis)) {
                if (pt1.Y > pt2.Y)
                    pt = pt1;
                else
                    pt = pt2;
            }
            if (axis.Equals(kZAxis)) {
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
            
            XYZ closestPt = new XYZ();
            Double closestDist = 0.0;

            foreach( XYZ ptTemp in pts )
            {
                /// don't consider the pt itself
                if (pt.Equals(ptTemp))
                    continue;

                Double dist = Math.Sqrt(Math.Pow((pt.X - ptTemp.X), 2.0) +
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
