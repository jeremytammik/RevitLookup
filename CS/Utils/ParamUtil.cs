#region Header
//
// Copyright 2003-2014 by Autodesk, Inc. 
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
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Revit = Autodesk.Revit.DB;

namespace RevitLookup.Utils
{
    public class ParamUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
       public static string
       ParamValueToStr (Revit.Parameter param)
        {
           
            if (param.StorageType == Revit.StorageType.Double)
                return param.AsDouble().ToString();
            else if (param.StorageType == Revit.StorageType.ElementId)
                return param.AsElementId().ToString();
            else if (param.StorageType == Revit.StorageType.Integer)
                return param.AsInteger().ToString();
            else if (param.StorageType == Revit.StorageType.String)
                return param.AsString();
            else {
                Debug.Assert(false);
                return string.Empty;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterSetToSet"></param>
        /// <param name="parameterSet"></param>
        public static void
        SetParameters (Revit.ParameterSet parameterSetToSet, Revit.ParameterSet parameterSet)
        {
            foreach (Revit.Parameter param in parameterSetToSet) {
                SetParameter(param, parameterSet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterToSet"></param>
        /// <param name="parameterSet"></param>
        public static void
        SetParameter (Revit.Parameter parameterToSet, Revit.ParameterSet parameterSet)
        {
            foreach (Revit.Parameter param in parameterSet) {
                if (param.Definition.Name == parameterToSet.Definition.Name) {
                    switch (param.StorageType) {
                    case Revit.StorageType.Double:
                        parameterToSet.Set(param.AsDouble());
                        break;
                    case Revit.StorageType.ElementId:
                        Revit.ElementId elemId = param.AsElementId();
                        parameterToSet.Set(elemId);
                        break;
                    case Revit.StorageType.Integer:
                        parameterToSet.Set(param.AsInteger());
                        break;
                    case Revit.StorageType.String:
                        parameterToSet.Set(param.AsString());
                        break;
                    }
                    return;
                }
            }
        }
    }
}
