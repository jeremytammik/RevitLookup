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
using RevitLookup.Snoop.Data;

namespace RevitLookup.Snoop.Collectors
{
    /// <summary>
    ///     Collect data about an CollectorXmlNode.  Since everything we encounter
    ///     in XML will be restricted to XML and not Elements and such, keep these
    ///     all in their own Collector object so we don't waste time trying to traverse
    ///     a class hierarchy that we'll never be expected to deal with.
    /// </summary>
    public class CollectorXmlNode : Collector
    {
        public void
            Collect(XmlNode node)
        {
            MDataObjs.Clear();
            Stream(node);

            // now that we've collected all the data that we know about,
            // fire an event to any registered Snoop Collector Extensions so
            // they can add their data
            //FireEvent_CollectExt(node);	// shouldn't be anyone else, we've taken care of it all
        }

        //  main branch for anything derived from System.Xml.XmlNode
        private void
            Stream(XmlNode node)
        {
            MDataObjs.Add(new ClassSeparator(typeof(XmlNode)));

            MDataObjs.Add(new String("Node type", node.NodeType.ToString()));
            MDataObjs.Add(new String("Name", node.Name));
            MDataObjs.Add(new String("Local name", node.LocalName));
            MDataObjs.Add(new String("Value", node.Value));
            MDataObjs.Add(new Bool("Has child nodes", node.HasChildNodes));
            MDataObjs.Add(new String("Inner text", node.InnerText));
            MDataObjs.Add(new Data.Xml("Inner XML", node.InnerXml, false));
            MDataObjs.Add(new Data.Xml("Outer XML", node.OuterXml, false));
            MDataObjs.Add(new Bool("Is read only", node.IsReadOnly));
            MDataObjs.Add(new String("BaseURI", node.BaseURI));
            MDataObjs.Add(new String("Namespace URI", node.NamespaceURI));
            MDataObjs.Add(new String("Prefix", node.Prefix));

            switch (node)
            {
                // branch to all known major sub-classes
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
                    break;
            }
        }

        private void
            Stream(XmlAttribute att)
        {
            MDataObjs.Add(new ClassSeparator(typeof(XmlAttribute)));

            MDataObjs.Add(new Bool("Specified", att.Specified));
        }

        private void Stream(XmlLinkedNode lnkNode)
        {
            MDataObjs.Add(new ClassSeparator(typeof(XmlLinkedNode)));

            // No data to show at this level, but we want to explicitly
            // show that there is an intermediate class.

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
                    break;
            }
        }


        private void Stream(XmlElement elem)
        {
            MDataObjs.Add(new ClassSeparator(typeof(XmlElement)));

            MDataObjs.Add(new Bool("Has attributes", elem.HasAttributes));
            MDataObjs.Add(new Bool("Is empty", elem.IsEmpty));
        }

        private void Stream(XmlCharacterData charData)
        {
            MDataObjs.Add(new ClassSeparator(typeof(XmlCharacterData)));

            MDataObjs.Add(new Int("Length", charData.Length));
            MDataObjs.Add(new String("Data", charData.Data));

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
                    break;
            }
        }

        private void Stream(XmlCDataSection cDataSection)
        {
            MDataObjs.Add(new ClassSeparator(typeof(XmlCDataSection)));

            // do data to display at this level
        }

        private void Stream(XmlComment comment)
        {
            MDataObjs.Add(new ClassSeparator(typeof(XmlComment)));

            // no data to display at this level
        }

        private void Stream(XmlDeclaration decl)
        {
            MDataObjs.Add(new ClassSeparator(typeof(XmlDeclaration)));

            MDataObjs.Add(new String("Encoding", decl.Encoding));
            MDataObjs.Add(new String("Standalone", decl.Standalone));
            MDataObjs.Add(new String("Version", decl.Version));
        }

        private void Stream(XmlDocument doc)
        {
            MDataObjs.Add(new ClassSeparator(typeof(XmlDocument)));

            MDataObjs.Add(new Bool("Preserve whitespace", doc.PreserveWhitespace));
        }

        private void Stream(XmlDocumentFragment doc)
        {
            MDataObjs.Add(new ClassSeparator(typeof(XmlDocumentFragment)));

            // no data to display at this level
        }

        private void Stream(XmlDocumentType dType)
        {
            MDataObjs.Add(new ClassSeparator(typeof(XmlDocumentType)));

            MDataObjs.Add(new String("Internal subset", dType.InternalSubset));
            MDataObjs.Add(new String("Public ID", dType.PublicId));
            MDataObjs.Add(new String("System ID", dType.SystemId));
        }

        private void Stream(XmlEntity ent)
        {
            MDataObjs.Add(new ClassSeparator(typeof(XmlEntity)));

            MDataObjs.Add(new String("Notation name", ent.NotationName));
            MDataObjs.Add(new String("Public ID", ent.PublicId));
            MDataObjs.Add(new String("System ID", ent.SystemId));
        }

        private void Stream(XmlEntityReference entRef)
        {
            MDataObjs.Add(new ClassSeparator(typeof(XmlEntityReference)));

            // no data to display at this level
        }

        private void Stream(XmlNotation notation)
        {
            MDataObjs.Add(new ClassSeparator(typeof(XmlNotation)));

            MDataObjs.Add(new String("Public ID", notation.PublicId));
            MDataObjs.Add(new String("System ID", notation.SystemId));
        }

        private void Stream(XmlProcessingInstruction pi)
        {
            MDataObjs.Add(new ClassSeparator(typeof(XmlProcessingInstruction)));

            MDataObjs.Add(new String("Target", pi.Target));
        }

        private void Stream(XmlSignificantWhitespace swSpace)
        {
            MDataObjs.Add(new ClassSeparator(typeof(XmlSignificantWhitespace)));

            // no data to display at this level
        }

        private void Stream(XmlText text)
        {
            MDataObjs.Add(new ClassSeparator(typeof(XmlText)));

            // no data to display at this level
        }

        private void Stream(XmlWhitespace wSpace)
        {
            MDataObjs.Add(new ClassSeparator(typeof(XmlWhitespace)));

            // no data to display at this level
        }
    }
}