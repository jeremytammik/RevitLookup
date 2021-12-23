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

using System.Xml;
using RevitLookup.Core.RevitTypes;
using String = RevitLookup.Core.RevitTypes.String;

namespace RevitLookup.Core.Collectors;

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
        Data.Clear();
        Stream(node);

        // now that we've collected all the data that we know about,
        // fire an event to any registered Snoop Collector Extensions so
        // they can add their data
        //FireEvent_CollectExt(node);	// shouldn't be anyone else, we've taken care of it all
    }

    //  main branch for anything derived from System.Xml.XmlNode
    private void Stream(XmlNode node)
    {
        Data.Add(new ClassSeparator(typeof(XmlNode)));
        Data.Add(new String("Node type", node.NodeType.ToString()));
        Data.Add(new String("Name", node.Name));
        Data.Add(new String("Local name", node.LocalName));
        Data.Add(new String("Value", node.Value));
        Data.Add(new Bool("Has child nodes", node.HasChildNodes));
        Data.Add(new String("Inner text", node.InnerText));
        Data.Add(new Xml("Inner XML", node.InnerXml, false));
        Data.Add(new Xml("Outer XML", node.OuterXml, false));
        Data.Add(new Bool("Is read only", node.IsReadOnly));
        Data.Add(new String("BaseURI", node.BaseURI));
        Data.Add(new String("Namespace URI", node.NamespaceURI));
        Data.Add(new String("Prefix", node.Prefix));

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
        Data.Add(new ClassSeparator(typeof(XmlAttribute)));
        Data.Add(new Bool("Specified", att.Specified));
    }

    private void Stream(XmlLinkedNode lnkNode)
    {
        Data.Add(new ClassSeparator(typeof(XmlLinkedNode)));

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
        Data.Add(new ClassSeparator(typeof(XmlElement)));
        Data.Add(new Bool("Has attributes", elem.HasAttributes));
        Data.Add(new Bool("Is empty", elem.IsEmpty));
    }

    private void Stream(XmlCharacterData charData)
    {
        Data.Add(new ClassSeparator(typeof(XmlCharacterData)));
        Data.Add(new Int("Length", charData.Length));
        Data.Add(new String("Data", charData.Data));

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
        Data.Add(new ClassSeparator(typeof(XmlCDataSection)));
    }

    private void Stream(XmlComment comment)
    {
        Data.Add(new ClassSeparator(typeof(XmlComment)));
    }

    private void Stream(XmlDeclaration decl)
    {
        Data.Add(new ClassSeparator(typeof(XmlDeclaration)));
        Data.Add(new String("Encoding", decl.Encoding));
        Data.Add(new String("Standalone", decl.Standalone));
        Data.Add(new String("Version", decl.Version));
    }

    private void Stream(XmlDocument doc)
    {
        Data.Add(new ClassSeparator(typeof(XmlDocument)));
        Data.Add(new Bool("Preserve whitespace", doc.PreserveWhitespace));
    }

    private void Stream(XmlDocumentFragment doc)
    {
        Data.Add(new ClassSeparator(typeof(XmlDocumentFragment)));
    }

    private void Stream(XmlDocumentType dType)
    {
        Data.Add(new ClassSeparator(typeof(XmlDocumentType)));
        Data.Add(new String("Internal subset", dType.InternalSubset));
        Data.Add(new String("Public ID", dType.PublicId));
        Data.Add(new String("System ID", dType.SystemId));
    }

    private void Stream(XmlEntity ent)
    {
        Data.Add(new ClassSeparator(typeof(XmlEntity)));
        Data.Add(new String("Notation name", ent.NotationName));
        Data.Add(new String("Public ID", ent.PublicId));
        Data.Add(new String("System ID", ent.SystemId));
    }

    private void Stream(XmlEntityReference entRef)
    {
        Data.Add(new ClassSeparator(typeof(XmlEntityReference)));
    }

    private void Stream(XmlNotation notation)
    {
        Data.Add(new ClassSeparator(typeof(XmlNotation)));
        Data.Add(new String("Public ID", notation.PublicId));
        Data.Add(new String("System ID", notation.SystemId));
    }

    private void Stream(XmlProcessingInstruction pi)
    {
        Data.Add(new ClassSeparator(typeof(XmlProcessingInstruction)));
        Data.Add(new String("Target", pi.Target));
    }

    private void Stream(XmlSignificantWhitespace swSpace)
    {
        Data.Add(new ClassSeparator(typeof(XmlSignificantWhitespace)));
    }

    private void Stream(XmlText text)
    {
        Data.Add(new ClassSeparator(typeof(XmlText)));
    }

    private void Stream(XmlWhitespace wSpace)
    {
        Data.Add(new ClassSeparator(typeof(XmlWhitespace)));
    }
}