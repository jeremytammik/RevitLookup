using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;

namespace RevitLookup.ExIm
{
    /// <summary>
    /// To add an xml map to a worksheet there needs to be an xml schema,
    /// this class builds it.
    /// (TBD: Could extract a schema by building a temp dataSet(dataset.GetXmlSchema()),
    /// which would do away with a lot of hard code stuff, however the schema returned
    /// by a dataset doesnt seem to reflect what was involved in
    /// building the dataset in the first place)
    /// </summary>
    public class XmlSchemaBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        private XmlDocument m_dom;
        private XmlElement m_complexType;
        private string m_nameSpaceUri = "http://www.w3.org/2001/XMLSchema";

        /// <summary>
        /// 
        /// </summary>
        public XmlSchemaBuilder ()
        {
            m_dom = new XmlDocument();
            CreateGenericStructure();
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateGenericStructure ()
        {
            XmlElement root = m_dom.CreateElement("schema", m_nameSpaceUri);
            root.Prefix = "xs";
            XmlAttribute attrib1 = m_dom.CreateAttribute("attributeFormDefault");
            attrib1.Value = "unqualified";
            XmlAttribute attrib2 = m_dom.CreateAttribute("elementFormDefault");
            attrib2.Value = "qualified";
            XmlAttribute attrib3 = m_dom.CreateAttribute("xmlns:xs");
            attrib3.Value = m_nameSpaceUri;
            root.Attributes.Append(attrib1);
            root.Attributes.Append(attrib2);
            root.Attributes.Append(attrib3);
            m_dom.AppendChild(root);

            XmlElement elemRecs = m_dom.CreateElement("element", m_nameSpaceUri);
            elemRecs.Prefix = "xs";
            XmlAttribute attrib4 = m_dom.CreateAttribute("name");
            attrib4.Value = "Records";
            elemRecs.Attributes.Append(attrib4);

            root.AppendChild(elemRecs);

            XmlElement elemComplex = m_dom.CreateElement("complexType", m_nameSpaceUri);
            elemComplex.Prefix = "xs";
            elemRecs.AppendChild(elemComplex);

            XmlElement elemSeq = m_dom.CreateElement("xs", "sequence", m_nameSpaceUri);
            elemSeq.Prefix = "xs";
            elemComplex.AppendChild(elemSeq);

            XmlElement elemRec = m_dom.CreateElement("element", m_nameSpaceUri);
            elemRec.Prefix = "xs";
            XmlAttribute attrib5 = m_dom.CreateAttribute("maxOccurs");
            attrib5.Value = "unbounded";
            XmlAttribute attrib6 = m_dom.CreateAttribute("name");
            attrib6.Value = "Record";
            elemRec.Attributes.Append(attrib5);
            elemRec.Attributes.Append(attrib6);
            elemSeq.AppendChild(elemRec);

            m_complexType = m_dom.CreateElement("complexType", m_nameSpaceUri);
            m_complexType.Prefix = "xs";
            elemRec.AppendChild(m_complexType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void AddAttribute (string name)
        {
            XmlElement elemAttrib = m_dom.CreateElement("attribute", m_nameSpaceUri);
            elemAttrib.Prefix = "xs";
            XmlAttribute attrib1 = m_dom.CreateAttribute("name");
            attrib1.Value = name;
            XmlAttribute attrib2 = m_dom.CreateAttribute("type");
            attrib2.Value = "xs:string";
            XmlAttribute attrib3 = m_dom.CreateAttribute("use");
            attrib3.Value = "required";
            elemAttrib.Attributes.Append(attrib1);
            elemAttrib.Attributes.Append(attrib2);
            elemAttrib.Attributes.Append(attrib3);
            m_complexType.AppendChild(elemAttrib);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetXmlSchema ()
        {
            string schema = "<?xml version=\"1.0\" encoding=\"Windows-1252\" ?>" + m_dom.OuterXml;
            return schema;
        }
    }
}
