
//////////////////////////////////////////////////////////////////////////
//
// Functions to help implement various reports
//
// Jim Awe
// Autodesk, Inc.
//
//////////////////////////////////////////////////////////////////////////


/*************************************************************************
**
**  createXmlDom
**      do the repetative work of loading a DOM tree with all the right
**  arguments.  I'm not currently sure these are all the right settings,
**  but it will do for now.
**
**  **jma
**
*********************************/

function createXmlDom()
{
	var xmlDom = new ActiveXObject("MSXML2.DOMDocument");
    if (xmlDom == null)
        return null;

	xmlDom.async = false;
	xmlDom.setProperty("SelectionLanguage", "XPath");

    return xmlDom;
}

/*************************************************************************
**
**  createFreeThreadedXmlDom
**      do the repetative work of loading a DOM tree with all the right
**  arguments.  I'm not currently sure these are all the right settings,
**  but it will do for now.
**
**  **jma
**
*********************************/

function createFreeThreadedXmlDom()
{
	var xmlDom = new ActiveXObject("MSXML2.FreeThreadedDOMDocument");
    if (xmlDom == null)
        return null;

	xmlDom.async = false;
	xmlDom.setProperty("SelectionLanguage", "XPath");

    return xmlDom;
}

/*************************************************************************
**
**  createXmlDomAndLoad
**      do the repetative work of loading a DOM tree with all the right
**  arguments.  I'm not currently sure these are all the right settings,
**  but it will do for now.
**
**  **jma
**
*********************************/

function createXmlDomAndLoad(url)
{
	var xmlDom = new ActiveXObject("MSXML2.DOMDocument");
    if (xmlDom == null)
        return null;

	xmlDom.async = false;
	xmlDom.setProperty("SelectionLanguage", "XPath");

    if (!xmlDom.load(url)) {
        var msg = "Could not load file: " + url + "\n" + xmlDom.parseError.reason;
        alert(msg);
        return null;
	}
    else
        return xmlDom;
}

/*************************************************************************
**
**  createFreeThreadedXmlDomAndLoad
**      do the repetative work of loading a DOM tree with all the right
**  arguments.  I'm not currently sure these are all the right settings,
**  but it will do for now.
**
**  **jma
**
*********************************/

function createFreeThreadedXmlDomAndLoad(url)
{
	var xmlDom = new ActiveXObject("MSXML2.FreeThreadedDOMDocument");
    if (xmlDom == null)
        return null;

	xmlDom.async = false;
	xmlDom.setProperty("SelectionLanguage", "XPath");

    if (!xmlDom.load(url)) {
        var msg = "Could not load file: " + url + "\n" + xmlDom.parseError.reason;
        alert(msg);
        return null;
	}
    else
        return xmlDom;
}

/*************************************************************************
**
**  createXslTemplateAndLoad
**
**  **jma
**
*********************************/

function createXslTemplateAndLoad(url)
{
    var template = new ActiveXObject("MSXML2.XSLTemplate.3.0");
    template.stylesheet = createFreeThreadedXmlDomAndLoad(url);
    return template;  
}

/*************************************************************************
**
**  xformByXsl
**
**  **jma
**
*********************************/

function xformByXsl(xmlDataSrcObj, xslUrl)
{
	var xslXform = createXmlDomAndLoad(xslUrl);
	if (xslXform == null) {
		window.status = "Could not load XSL transform!";
		return;
	}

	if (xmlDataSrcObj != null) {
		xslTarget.innerHTML = xmlDataSrcObj.transformNode(xslXform.documentElement);
	}
	else {
		alert("No XML data source is loaded.");
		document.write("No XML data source loaded.");
	}
}

/*************************************************************************
**
**  writeDomElement
**
**  **jma
**
*********************************/

function writeDomElement(xmlDoc, curNode, tagName)
{
    var newNode = xmlDoc.createElement(tagName);

    if (curNode == null) {
            // not sure why this has to be special cased, but I couldn't
            // get it to work any other way???
        var rootNode = xmlDoc.selectSingleNode("/");
        rootNode.appendChild(newNode);
    }
    else {
        curNode.appendChild(newNode);
    }
    
    return newNode;
}

/*************************************************************************
**
**  writeDomAttribute
**
**  **jma
**
*********************************/

function writeDomAttribute(curNode, attName, attValue)
{
    //alert("about to add att: " + attName);

    curNode.setAttribute(attName, attValue);
}

/*************************************************************************
**
**  arrayHas
**      see if this item exists in a string array already
**
**  **jma
**
*********************************/

function arrayHas(strArray, strVal)
{
	for (var i=0;i<strArray.length;i++) {
		if (strArray[i] == strVal)
			return true;
	}

	return false;
}

/*************************************************************************
**
**  arrayIndex
**      get the array index of a given string
**
**  **jma
**
*********************************/

function arrayIndex(strArray, strVal)
{
    for (var i=0;i<strArray.length;i++) {
        if (strArray[i] == strVal)
            return i;
    }

    return -1;
}

/*************************************************************************
**
**  getRawCountTotals
**
**  **jma
**
*********************************/

function getRawCountTotals(xmlDoc, classArray, dispNameArray, countArray)
{
    var nodes = xmlDoc.selectNodes("//Entities/RawCount/ObjectType");
    
    var tmpClassStr;

    for (var i=0; i<nodes.length; i++) {
        tmpClassStr = nodes.item(i).getAttribute("class");
        
        var index = arrayIndex(classArray, tmpClassStr);
        if (index != -1) {
            countArray[index] += parseInt(nodes.item(i).getAttribute("count"));
        }
        else {
            classArray.push(tmpClassStr);
            dispNameArray.push(nodes.item(i).getAttribute("displayName"));
            countArray.push(parseInt(nodes.item(i).getAttribute("count")));
        }
    }
}

/*************************************************************************
**
**  makeRawCountTotalDom
**
**  **jma
**
*********************************/

function makeRawCountTotalDom(xmlSourceDoc)
{
    if (xmlSourceDoc == null) {
		alert("No XML data source is loaded.");
		document.write("No XML data source loaded.");
	}

    var xmlIntrTblDoc = createXmlDom();
    if (xmlIntrTblDoc == null) {
        alert("ERROR: could not make project-wide transformation of source doc.");
        return null;
    }
        
    var classArray = new Array();
    var dispNameArray = new Array();
    var countArray = new Array();
    
    getRawCountTotals(xmlSourceDoc, classArray, dispNameArray, countArray);

    var rootNode = writeDomElement(xmlIntrTblDoc, null, "Drawings");
    var dwgsNode = writeDomElement(xmlIntrTblDoc, rootNode, "Drawing");
    writeDomAttribute(dwgsNode, "path", "** PROJECT TOTALS **");
    
    var entitiesNode = writeDomElement(xmlIntrTblDoc, dwgsNode, "Entities");
    var rawCountNode = writeDomElement(xmlIntrTblDoc, entitiesNode, "RawCount");

    var curNode = null;

    for (var i=0;i<classArray.length;i++) {
        curNode = writeDomElement(xmlIntrTblDoc, rawCountNode, "ObjectType");
        writeDomAttribute(curNode, "class", classArray[i]);
        writeDomAttribute(curNode, "displayName", dispNameArray[i]);
        writeDomAttribute(curNode, "count", countArray[i]);
    }

    return xmlIntrTblDoc;
}