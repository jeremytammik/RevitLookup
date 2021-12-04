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

using System.Xml;
using RevitLookup.Core.RevitTypes;

namespace RevitLookup.Core.Collectors
{
    /// <summary>
    ///     Collect data about an CollectorXmlNode.  Since everything we encounter
    ///     in XML will be restricted to XML and not Elements and such, keep these
    ///     all in their own Collector object so we don't waste time trying to traverse
    ///     a class hierarchy that we'll never be expected to deal with.
    /// </summary>
    public class CollectorXmlNode : Collector
    {
        public void Collect(XmlNode node)
        {
            DataObjects.Clear();
            Stream(node);

            // now that we've collected all the data that we know about,
            // fire an event to any registered Snoop Collector Extensions so
            // they can add their data
            //FireEvent_CollectExt(node);	// shouldn't be anyone else, we've taken care of it all
        }

        //  main branch for anything derived from System.Xml.XmlNode
        private void Stream(XmlNode node)
        {
            DataObjects.Add(new ClassSeparator(typeof(XmlNode)));
            DataObjects.Add(new String("Node type", node.NodeType.ToString()));
            DataObjects.Add(new String("Name", node.Name));
            DataObjects.Add(new String("Local name", node.LocalName));
            DataObjects.Add(new String("Value", node.Value));
            DataObjects.Add(new Bool("Has child nodes", node.HasChildNodes));
            DataObjects.Add(new String("Inner text", node.InnerText));
            DataObjects.Add(new Xml("Inner XML", node.InnerXml, false));
            DataObjects.Add(new Xml("Outer XML", node.OuterXml, false));
            DataObjects.Add(new Bool("Is read only", node.IsReadOnly));
            DataObjects.Add(new String("BaseURI", node.BaseURI));
            DataObjects.Add(new String("Namespace URI", node.NamespaceURI));
            DataObjects.Add(new String("Prefix", node.Prefix));

            switch (node)
            {
                case XmlAttribute att:
                    Stream(att);
                    return;
                case XmlDocument doc:
                    Stream(doc);
                    return;
                case XmlDocumentFragment docFrag:
                    Stream(docFrag);
                    return;
                case XmlEntity ent:
                    Stream(ent);
                    return;
                case XmlNotation notation:
                    Stream(notation);
                    return;
                case XmlLinkedNode lnkNode:
                    Stream(lnkNode);
                    return;
            }
        }

        private void Stream(XmlAttribute att)
        {
            DataObjects.Add(new ClassSeparator(typeof(XmlAttribute)));
            DataObjects.Add(new Bool("Specified", att.Specified));
        }

        private void Stream(XmlLinkedNode lnkNode)
        {
            DataObjects.Add(new ClassSeparator(typeof(XmlLinkedNode)));

            switch (lnkNode)
            {
                case XmlElement elem:
                    Stream(elem);
                    return;
                case XmlCharacterData charData:
                    Stream(charData);
                    return;
                case XmlDeclaration decl:
                    Stream(decl);
                    return;
                case XmlDocumentType dType:
                    Stream(dType);
                    return;
                case XmlEntityReference entRef:
                    Stream(entRef);
                    return;
                case XmlProcessingInstruction pi:
                    Stream(pi);
                    return;
            }
        }


        private void Stream(XmlElement elem)
        {
            DataObjects.Add(new ClassSeparator(typeof(XmlElement)));
            DataObjects.Add(new Bool("Has attributes", elem.HasAttributes));
            DataObjects.Add(new Bool("Is empty", elem.IsEmpty));
        }

        private void Stream(XmlCharacterData charData)
        {
            DataObjects.Add(new ClassSeparator(typeof(XmlCharacterData)));
            DataObjects.Add(new Int("Length", charData.Length));
            DataObjects.Add(new String("Data", charData.Data));

            switch (charData)
            {
                case XmlCDataSection cDataSection:
                    Stream(cDataSection);
                    return;
                case XmlComment comment:
                    Stream(comment);
                    return;
                case XmlSignificantWhitespace swSpace:
                    Stream(swSpace);
                    return;
                case XmlText txt:
                    Stream(txt);
                    return;
                case XmlWhitespace wSpace:
                    Stream(wSpace);
                    return;
            }
        }

        private void Stream(XmlCDataSection cDataSection)
        {
            DataObjects.Add(new ClassSeparator(typeof(XmlCDataSection)));
        }

        private void Stream(XmlComment comment)
        {
            DataObjects.Add(new ClassSeparator(typeof(XmlComment)));
        }

        private void Stream(XmlDeclaration decl)
        {
            DataObjects.Add(new ClassSeparator(typeof(XmlDeclaration)));
            DataObjects.Add(new String("Encoding", decl.Encoding));
            DataObjects.Add(new String("Standalone", decl.Standalone));
            DataObjects.Add(new String("Version", decl.Version));
        }

        private void Stream(XmlDocument doc)
        {
            DataObjects.Add(new ClassSeparator(typeof(XmlDocument)));
            DataObjects.Add(new Bool("Preserve whitespace", doc.PreserveWhitespace));
        }

        private void Stream(XmlDocumentFragment doc)
        {
            DataObjects.Add(new ClassSeparator(typeof(XmlDocumentFragment)));
        }

        private void Stream(XmlDocumentType dType)
        {
            DataObjects.Add(new ClassSeparator(typeof(XmlDocumentType)));
            DataObjects.Add(new String("Internal subset", dType.InternalSubset));
            DataObjects.Add(new String("Public ID", dType.PublicId));
            DataObjects.Add(new String("System ID", dType.SystemId));
        }

        private void Stream(XmlEntity ent)
        {
            DataObjects.Add(new ClassSeparator(typeof(XmlEntity)));
            DataObjects.Add(new String("Notation name", ent.NotationName));
            DataObjects.Add(new String("Public ID", ent.PublicId));
            DataObjects.Add(new String("System ID", ent.SystemId));
        }

        private void Stream(XmlEntityReference entRef)
        {
            DataObjects.Add(new ClassSeparator(typeof(XmlEntityReference)));
        }

        private void Stream(XmlNotation notation)
        {
            DataObjects.Add(new ClassSeparator(typeof(XmlNotation)));
            DataObjects.Add(new String("Public ID", notation.PublicId));
            DataObjects.Add(new String("System ID", notation.SystemId));
        }

        private void Stream(XmlProcessingInstruction pi)
        {
            DataObjects.Add(new ClassSeparator(typeof(XmlProcessingInstruction)));
            DataObjects.Add(new String("Target", pi.Target));
        }

        private void Stream(XmlSignificantWhitespace swSpace)
        {
            DataObjects.Add(new ClassSeparator(typeof(XmlSignificantWhitespace)));
        }

        private void Stream(XmlText text)
        {
            DataObjects.Add(new ClassSeparator(typeof(XmlText)));
        }

        private void Stream(XmlWhitespace wSpace)
        {
            DataObjects.Add(new ClassSeparator(typeof(XmlWhitespace)));
        }
    }
}