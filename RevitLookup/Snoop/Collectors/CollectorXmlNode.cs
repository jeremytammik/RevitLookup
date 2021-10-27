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

using System;
using System.Collections;


namespace RevitLookup.Snoop.Collectors
{
	/// <summary>
    /// Collect data about an CollectorXmlNode.  Since everything we encounter
    /// in XML will be restricted to XML and not Elements and such, keep these
    /// all in their own Collector object so we don't waste time trying to traverse
    /// a class hierarchy that we'll never be expected to deal with.
	/// </summary>
	
	public class CollectorXmlNode : Collector
	{
	    
		public
		CollectorXmlNode()
		{
		}
		
		public void
		Collect(System.Xml.XmlNode node)
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
		Stream(System.Xml.XmlNode node)
		{
		    MDataObjs.Add(new Data.ClassSeparator(typeof(System.Xml.XmlNode)));
		    
            MDataObjs.Add(new Snoop.Data.String("Node type", node.NodeType.ToString()));
            MDataObjs.Add(new Snoop.Data.String("Name", node.Name));
            MDataObjs.Add(new Snoop.Data.String("Local name", node.LocalName));
            MDataObjs.Add(new Snoop.Data.String("Value", node.Value));
            MDataObjs.Add(new Data.Bool("Has child nodes", node.HasChildNodes));
            MDataObjs.Add(new Snoop.Data.String("Inner text", node.InnerText));
            MDataObjs.Add(new Snoop.Data.Xml("Inner XML", node.InnerXml, false));
            MDataObjs.Add(new Snoop.Data.Xml("Outer XML", node.OuterXml, false));
            MDataObjs.Add(new Data.Bool("Is read only", node.IsReadOnly));
            MDataObjs.Add(new Snoop.Data.String("BaseURI", node.BaseURI));
            MDataObjs.Add(new Snoop.Data.String("Namespace URI", node.NamespaceURI));
            MDataObjs.Add(new Snoop.Data.String("Prefix", node.Prefix));
                        
                // branch to all known major sub-classes
            System.Xml.XmlAttribute att = node as System.Xml.XmlAttribute;
            if (att != null) {
                Stream(att);
                return;
            }
            
            System.Xml.XmlDocument doc = node as System.Xml.XmlDocument;
            if (doc != null) {
                Stream(doc);
                return;
            }
            
            System.Xml.XmlDocumentFragment docFrag = node as System.Xml.XmlDocumentFragment;
            if (docFrag != null) {
                Stream(docFrag);
                return;
            }
            
            System.Xml.XmlEntity ent = node as System.Xml.XmlEntity;
            if (ent != null) {
                Stream(ent);
                return;
            }
            
            System.Xml.XmlNotation notation = node as System.Xml.XmlNotation;
            if (notation != null) {
                Stream(notation);
                return;
            }
            
            System.Xml.XmlLinkedNode lnkNode = node as System.Xml.XmlLinkedNode;
            if (lnkNode != null) {
                Stream(lnkNode);
                return;
            }
        }
        
        private void
        Stream(System.Xml.XmlAttribute att)
        {
            MDataObjs.Add(new Data.ClassSeparator(typeof(System.Xml.XmlAttribute)));

            MDataObjs.Add(new Data.Bool("Specified", att.Specified));
        }
        
        private void
        Stream(System.Xml.XmlLinkedNode lnkNode)
        {
            MDataObjs.Add(new Data.ClassSeparator(typeof(System.Xml.XmlLinkedNode)));

                // No data to show at this level, but we want to explicitly
                // show that there is an intermediate class.
                
            System.Xml.XmlElement elem = lnkNode as System.Xml.XmlElement;
            if (elem != null) {
                Stream(elem);
                return;
            }
            
            System.Xml.XmlCharacterData charData = lnkNode as System.Xml.XmlCharacterData;
            if (charData != null) {
                Stream(charData);
                return;
            }
            
            System.Xml.XmlDeclaration decl = lnkNode as System.Xml.XmlDeclaration;
            if (decl != null) {
                Stream(decl);
                return;
            }
            
            System.Xml.XmlDocumentType dType = lnkNode as System.Xml.XmlDocumentType;
            if (dType != null) {
                Stream(dType);
                return;
            }
            
            System.Xml.XmlEntityReference entRef = lnkNode as System.Xml.XmlEntityReference;
            if (entRef != null) {
                Stream(entRef);
                return;
            }
            
            System.Xml.XmlProcessingInstruction pi = lnkNode as System.Xml.XmlProcessingInstruction;
            if (pi != null) {
                Stream(pi);
                return;
            }
        }

        
        private void
        Stream(System.Xml.XmlElement elem)
        {
            MDataObjs.Add(new Data.ClassSeparator(typeof(System.Xml.XmlElement)));

            MDataObjs.Add(new Data.Bool("Has attributes", elem.HasAttributes));
            MDataObjs.Add(new Data.Bool("Is empty", elem.IsEmpty));
        }
        
        private void
        Stream(System.Xml.XmlCharacterData charData)
        {
            MDataObjs.Add(new Data.ClassSeparator(typeof(System.Xml.XmlCharacterData)));

            MDataObjs.Add(new Data.Int("Length", charData.Length));
            MDataObjs.Add(new Snoop.Data.String("Data", charData.Data));
            
            System.Xml.XmlCDataSection cDataSection = charData as System.Xml.XmlCDataSection;
            if (cDataSection != null) {
                Stream(cDataSection);
                return;
            }
            
            System.Xml.XmlComment comment = charData as System.Xml.XmlComment;
            if (comment != null) {
                Stream(comment);
                return;
            }
            
            System.Xml.XmlSignificantWhitespace swSpace = charData as System.Xml.XmlSignificantWhitespace;
            if (swSpace != null) {
                Stream(swSpace);
                return;
            }
            
            System.Xml.XmlText txt = charData as System.Xml.XmlText;
            if (txt != null) {
                Stream(txt);
                return;
            }
            
            System.Xml.XmlWhitespace wSpace = charData as System.Xml.XmlWhitespace;
            if (wSpace != null) {
                Stream(wSpace);
                return;
            }
        }
        
        private void
        Stream(System.Xml.XmlCDataSection cDataSection)
        {
            MDataObjs.Add(new Data.ClassSeparator(typeof(System.Xml.XmlCDataSection)));

			// do data to display at this level
        }

        private void
        Stream(System.Xml.XmlComment comment)
        {
            MDataObjs.Add(new Data.ClassSeparator(typeof(System.Xml.XmlComment)));

            // no data to display at this level
        }
        
        private void
        Stream(System.Xml.XmlDeclaration decl)
        {
            MDataObjs.Add(new Data.ClassSeparator(typeof(System.Xml.XmlDeclaration)));

            MDataObjs.Add(new Snoop.Data.String("Encoding", decl.Encoding));
            MDataObjs.Add(new Snoop.Data.String("Standalone", decl.Standalone));
            MDataObjs.Add(new Snoop.Data.String("Version", decl.Version));
        }

        private void
        Stream(System.Xml.XmlDocument doc)
        {
            MDataObjs.Add(new Data.ClassSeparator(typeof(System.Xml.XmlDocument)));

            MDataObjs.Add(new Data.Bool("Preserve whitespace", doc.PreserveWhitespace));
        }

        private void
        Stream(System.Xml.XmlDocumentFragment doc)
        {
            MDataObjs.Add(new Data.ClassSeparator(typeof(System.Xml.XmlDocumentFragment)));

            // no data to display at this level
        }

        private void
        Stream(System.Xml.XmlDocumentType dType)
        {
            MDataObjs.Add(new Data.ClassSeparator(typeof(System.Xml.XmlDocumentType)));

            MDataObjs.Add(new Snoop.Data.String("Internal subset", dType.InternalSubset));
            MDataObjs.Add(new Snoop.Data.String("Public ID", dType.PublicId));
            MDataObjs.Add(new Snoop.Data.String("System ID", dType.SystemId));
        }

        private void
        Stream(System.Xml.XmlEntity ent)
        {
            MDataObjs.Add(new Data.ClassSeparator(typeof(System.Xml.XmlEntity)));

            MDataObjs.Add(new Snoop.Data.String("Notation name", ent.NotationName));
            MDataObjs.Add(new Snoop.Data.String("Public ID", ent.PublicId));
            MDataObjs.Add(new Snoop.Data.String("System ID", ent.SystemId));
        }
        
        private void
        Stream(System.Xml.XmlEntityReference entRef)
        {
            MDataObjs.Add(new Data.ClassSeparator(typeof(System.Xml.XmlEntityReference)));

            // no data to display at this level
        }
        
        private void
        Stream(System.Xml.XmlNotation notation)
        {
            MDataObjs.Add(new Data.ClassSeparator(typeof(System.Xml.XmlNotation)));

            MDataObjs.Add(new Snoop.Data.String("Public ID", notation.PublicId));
            MDataObjs.Add(new Snoop.Data.String("System ID", notation.SystemId));
        }
        
        private void
        Stream(System.Xml.XmlProcessingInstruction pi)
        {
            MDataObjs.Add(new Data.ClassSeparator(typeof(System.Xml.XmlProcessingInstruction)));

            MDataObjs.Add(new Snoop.Data.String("Target", pi.Target));
        }
        
        private void
        Stream(System.Xml.XmlSignificantWhitespace swSpace)
        {
            MDataObjs.Add(new Data.ClassSeparator(typeof(System.Xml.XmlSignificantWhitespace)));

            // no data to display at this level
        }
        
        private void
        Stream(System.Xml.XmlText text)
        {
            MDataObjs.Add(new Data.ClassSeparator(typeof(System.Xml.XmlText)));

            // no data to display at this level
        }
        
        private void
        Stream(System.Xml.XmlWhitespace wSpace)
        {
            MDataObjs.Add(new Data.ClassSeparator(typeof(System.Xml.XmlWhitespace)));

            // no data to display at this level
        }
    }
}

        
       