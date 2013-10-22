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
