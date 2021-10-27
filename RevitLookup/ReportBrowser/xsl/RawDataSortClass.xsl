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

        <xsl:apply-templates select="RawCounts" />
    </xsl:template>



    <xsl:template match="RawCounts">
        <table class="small" border="3" cellpadding="5">
            <caption>Raw Count</caption>
            <thead>
                <tr>
                    <th>Class</th>
                    <th>Full Name</th>
                    <th>Count</th>
                </tr>
            </thead>
            <tbody>
                <xsl:for-each select="ClassType">
                    <xsl:sort select="@name" />
                    <tr>
                        <td><xsl:value-of select="@name"/></td>
                        <td><xsl:value-of select="@fullName"/></td>
                        <td align="right"><xsl:value-of select="@count"/></td>
                    </tr>
                </xsl:for-each>
            </tbody>
        </table>
        <br/>
    </xsl:template>

</xsl:stylesheet>
