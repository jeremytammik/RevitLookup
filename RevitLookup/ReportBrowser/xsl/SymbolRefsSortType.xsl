<?xml version="1.0"?>

<!--
//  Jim Awe
//  Autodesk, Inc.
-->

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
    <xsl:output method="html"/>

    <xsl:template match="/">
        <html>
            <head>
            	<link rel='stylesheet' type='text/css' href='../css/Reports.css'/>
            </head>
            <body>
                <xsl:apply-templates select="//Project" />
            </body>
        </html>
    </xsl:template>


    <xsl:template match="Project">
        <hr/>
        <table>
            <tr class="drawing">
                <td class="label">Document:</td><td width="1000"><xsl:value-of select="@path"/></td>
            </tr>
        </table>
        <hr/>

        <xsl:apply-templates select="Symbols" />
    </xsl:template>


    <xsl:template match="Symbols">
        <table class="small" border="3" cellpadding="5">
            <caption>Symbol Reference Count</caption>
            <thead>
                <tr>
                    <th>Symbol Name</th>
                    <th>Symbol Type</th>
                    <th>References</th>
                </tr>
            </thead>
            <tbody>
                <xsl:for-each select="Symbol">
                    <xsl:sort select="@symbolType" />
                    <tr>
                        <td><xsl:value-of select="@name"/></td>
                        <td><xsl:value-of select="@symbolType"/></td>
                        <td align="right"><xsl:value-of select="@refCount"/></td>
                    </tr>
                </xsl:for-each>
            </tbody>
        </table>
        <br/>
    </xsl:template>

</xsl:stylesheet>
