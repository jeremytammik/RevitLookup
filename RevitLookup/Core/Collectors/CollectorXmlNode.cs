// Copyright 2003-2022 by Autodesk, Inc. 
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
        Data.Add(new ClassSeparatorData(typeof(XmlNode)));
        Data.Add(new StringData("Node type", node.NodeType.ToString()));
        Data.Add(new StringData("Name", node.Name));
        Data.Add(new StringData("Local name", node.LocalName));
        Data.Add(new StringData("Value", node.Value));
        Data.Add(new BoolData("Has child nodes", node.HasChildNodes));
        Data.Add(new StringData("Inner text", node.InnerText));
        Data.Add(new XmlData("Inner XML", node.InnerXml, false));
        Data.Add(new XmlData("Outer XML", node.OuterXml, false));
        Data.Add(new BoolData("Is read only", node.IsReadOnly));
        Data.Add(new StringData("BaseURI", node.BaseURI));
        Data.Add(new StringData("Namespace URI", node.NamespaceURI));
        Data.Add(new StringData("Prefix", node.Prefix));

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

    private void Stream(XmlAttribute attribute)
    {
        Data.Add(new ClassSeparatorData(typeof(XmlAttribute)));
        Data.Add(new BoolData("Specified", attribute.Specified));
    }

    private void Stream(XmlLinkedNode linkedNode)
    {
        Data.Add(new ClassSeparatorData(typeof(XmlLinkedNode)));

        switch (linkedNode)
        {
            case XmlElement element:
                Stream(element);
                return;
            case XmlCharacterData characterData:
                Stream(characterData);
                return;
            case XmlDeclaration declaration:
                Stream(declaration);
                return;
            case XmlDocumentType documentType:
                Stream(documentType);
                return;
            case XmlEntityReference entityReference:
                Stream(entityReference);
                return;
            case XmlProcessingInstruction processingInstruction:
                Stream(processingInstruction);
                return;
        }
    }


    private void Stream(XmlElement element)
    {
        Data.Add(new ClassSeparatorData(typeof(XmlElement)));
        Data.Add(new BoolData("Has attributes", element.HasAttributes));
        Data.Add(new BoolData("Is empty", element.IsEmpty));
    }

    private void Stream(XmlCharacterData characterData)
    {
        Data.Add(new ClassSeparatorData(typeof(XmlCharacterData)));
        Data.Add(new IntData("Length", characterData.Length));
        Data.Add(new StringData("Data", characterData.Data));

        switch (characterData)
        {
            case XmlCDataSection dataSection:
                Stream(dataSection);
                return;
            case XmlComment comment:
                Stream(comment);
                return;
            case XmlSignificantWhitespace significantWhitespace:
                Stream(significantWhitespace);
                return;
            case XmlText text:
                Stream(text);
                return;
            case XmlWhitespace whitespace:
                Stream(whitespace);
                return;
        }
    }

    private void Stream(XmlCDataSection dataSection)
    {
        Data.Add(new ClassSeparatorData(typeof(XmlCDataSection)));
    }

    private void Stream(XmlComment comment)
    {
        Data.Add(new ClassSeparatorData(typeof(XmlComment)));
    }

    private void Stream(XmlDeclaration declaration)
    {
        Data.Add(new ClassSeparatorData(typeof(XmlDeclaration)));
        Data.Add(new StringData("Encoding", declaration.Encoding));
        Data.Add(new StringData("Standalone", declaration.Standalone));
        Data.Add(new StringData("Version", declaration.Version));
    }

    private void Stream(XmlDocument document)
    {
        Data.Add(new ClassSeparatorData(typeof(XmlDocument)));
        Data.Add(new BoolData("Preserve whitespace", document.PreserveWhitespace));
    }

    private void Stream(XmlDocumentFragment documentFragment)
    {
        Data.Add(new ClassSeparatorData(typeof(XmlDocumentFragment)));
    }

    private void Stream(XmlDocumentType documentType)
    {
        Data.Add(new ClassSeparatorData(typeof(XmlDocumentType)));
        Data.Add(new StringData("Internal subset", documentType.InternalSubset));
        Data.Add(new StringData("Public ID", documentType.PublicId));
        Data.Add(new StringData("System ID", documentType.SystemId));
    }

    private void Stream(XmlEntity entity)
    {
        Data.Add(new ClassSeparatorData(typeof(XmlEntity)));
        Data.Add(new StringData("Notation name", entity.NotationName));
        Data.Add(new StringData("Public ID", entity.PublicId));
        Data.Add(new StringData("System ID", entity.SystemId));
    }

    private void Stream(XmlEntityReference entityReference)
    {
        Data.Add(new ClassSeparatorData(typeof(XmlEntityReference)));
    }

    private void Stream(XmlNotation notation)
    {
        Data.Add(new ClassSeparatorData(typeof(XmlNotation)));
        Data.Add(new StringData("Public ID", notation.PublicId));
        Data.Add(new StringData("System ID", notation.SystemId));
    }

    private void Stream(XmlProcessingInstruction processingInstruction)
    {
        Data.Add(new ClassSeparatorData(typeof(XmlProcessingInstruction)));
        Data.Add(new StringData("Target", processingInstruction.Target));
    }

    private void Stream(XmlSignificantWhitespace significantWhitespace)
    {
        Data.Add(new ClassSeparatorData(typeof(XmlSignificantWhitespace)));
    }

    private void Stream(XmlText text)
    {
        Data.Add(new ClassSeparatorData(typeof(XmlText)));
    }

    private void Stream(XmlWhitespace whitespace)
    {
        Data.Add(new ClassSeparatorData(typeof(XmlWhitespace)));
    }
}