#region Header
//
// Copyright 2003-2013 by Autodesk, Inc. 
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
using System.Xml;
using System.IO;
using System.Collections;

using Autodesk.Revit;
using Autodesk.Revit.DB;

namespace RevitLookup.ExIm
{
    /// <summary>
    /// Keeps a record of all import errors
    /// </summary>
    public class ImportErrorLogger
    {
        /// <summary>
        /// 
        /// </summary>
        private XmlDocument m_dom;
        private string m_logFileName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logFileName"></param>
        public ImportErrorLogger (string logFileName)
        {
            m_logFileName = Path.ChangeExtension(logFileName, ".log.xml");
            m_dom = new XmlDocument();
            XmlElement root = m_dom.CreateElement("ImportErrors");
            m_dom.AppendChild(root);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        public void LogError (Parameter param, ArrayList paramsList)
        {
            XmlElement errorElem = m_dom.CreateElement("ImportError");
            
            XmlAttribute attrib1 = errorElem.OwnerDocument.CreateAttribute("Name");
            attrib1.Value = param.Definition.Name;
            errorElem.Attributes.Append(attrib1);

            XmlAttribute attrib2 = errorElem.OwnerDocument.CreateAttribute("Value");
            attrib2.Value = Utils.ParamUtil.ParamValueToStr(param);
            errorElem.Attributes.Append(attrib2);

            XmlAttribute attrib3 = errorElem.OwnerDocument.CreateAttribute("Mark");
            foreach (Parameter parameter in paramsList) {
                if (parameter.Definition.Name == "Mark") {
                    attrib3.Value = parameter.AsString();
                }
            }
            errorElem.Attributes.Append(attrib3);

            m_dom.DocumentElement.AppendChild(errorElem);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SaveErrorLog ()
        {
            m_dom.Save(m_logFileName);
        }
    }
}
