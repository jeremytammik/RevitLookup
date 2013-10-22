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
