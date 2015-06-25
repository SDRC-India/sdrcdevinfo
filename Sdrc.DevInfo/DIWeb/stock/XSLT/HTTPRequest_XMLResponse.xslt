<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="html" version="3.2" encoding="ISO-8859-1"/>
<xsl:param name="Title_Text"></xsl:param>
<xsl:param name="Footnote_Text"></xsl:param>
<xsl:param name="BaseUrl"></xsl:param>
<xsl:param name="Rest">/Search/en/S/Sector/</xsl:param>
<xsl:param name="Page" select="0" />
<xsl:param name="PageSize" select="10000" />  
<xsl:template name="results" match="/">

  <html>
  <body>
    <h1>
      <xsl:value-of select="$Title_Text"/>
    </h1>
    <table style="font:12px Helvetica, sans-serif; color:#404141;border:1px solid #c8c8c8;" cellpadding="0" cellspacing="0" width="100%">
      <tr style="padding:7px 5px; font-size:14px;background:#eee" align="left">
        <th width="30%" style="border:1px solid #c8c8c8;border-right:0px;border-left:0px;border-top:0px;">Indicator</th>
        <th width="17%" style="border:1px solid #c8c8c8;border-right:0px;border-top:0px;">Area</th>
        <th width="10%" style="border:1px solid #c8c8c8;border-right:0px;border-top:0px;">Data value</th>
        <th width="10%" style="border:1px solid #c8c8c8;border-right:0px;border-top:0px;">Time Period</th>
        <th style="border:1px solid #c8c8c8;border-top:0px;border-right:0px;">Source</th>
      </tr>
      
	<xsl:variable name="mycount" select="count(Root/Data/Observation)"/>
	<xsl:variable name="selectedRowCount" select="floor((number($mycount)-1) div $PageSize)+1"/>
	
	
	<!-- Loop through the Data -->
	<xsl:for-each select="Root/Data/Observation">

		<!-- Indicator Grouping -->
		<!--
		<xsl:if test="not(INDICATOR = preceding-sibling::Observation[1]/INDICATOR)">
			<tr style="padding:15px 0px 15px 2px;border:0px;margin-left:auto;margin-right:auto;"><td colspan="4">
			<font style="font:bold 16px"><xsl:value-of select="INDICATOR"/></font> (<xsl:value-of select="UNIT"/>), <xsl:value-of select="SUBGROUP"/>
			</td></tr>
		</xsl:if>
		-->
	
		<!-- Pagination -->
		<xsl:if test="position() &gt;= ($Page * $PageSize) + 1">
		<xsl:if test="position() &lt;= $PageSize + ($PageSize * $Page)">
		<!-- Display data -->
		<tr style="padding:5px 5px 13px;">

			<!-- INDICATOR -->
			<td style="border:1px solid #c8c8c8;border-right:0px;border-left:0px;border-top:0px;vertical-align:top"><xsl:value-of select="INDICATOR"/> (<xsl:value-of select="UNIT"/>), <xsl:value-of select="SUBGROUP"/></td>
			
			<!-- AREA -->
			<td style="border:1px solid #c8c8c8;border-right:0px;border-top:0px;vertical-align:top">
			<A target="_new">
        <xsl:attribute name="href">          
            <xsl:value-of select="$BaseUrl"></xsl:value-of><xsl:value-of select="$Rest"></xsl:value-of>
          <xsl:value-of select="INDICATOR"/>/<xsl:value-of select="AREA"/></xsl:attribute><xsl:value-of select="AREA"/></A></td>

			<!-- DATA VALUE and Footnotes -->
			<td style="border:1px solid #c8c8c8;border-right:0px;border-top:0px;vertical-align:top">
				<xsl:value-of select="OBS_VALUE"/>
				<xsl:if test="FOOTNOTES != ''">
					<br></br><br></br>
					<font style="color:#ababab;font-style: italic;font-size : 9px"><xsl:value-of select="FOOTNOTES"/></font>
				</xsl:if> 
			</td>
			<td style="border:1px solid #c8c8c8;border-right:0px;border-top:0px;vertical-align:top"><xsl:value-of select="TIME_PERIOD"/></td>
			<td style="border:1px solid #c8c8c8;border-right:0px;border-top:0px;vertical-align:top"><xsl:value-of select="SOURCE"/></td>
		</tr>


		
		
		</xsl:if>
		</xsl:if>

	</xsl:for-each>

	<!-- Powered by DevInfo -->
	<tr style="padding:15px 0px 15px 2px;border:0px;margin-left:auto;margin-right:auto;">
		<td align="center" colspan="5">
      <a target="_new" style="text-decoration:none;" title="Powered by DevInfo">
        <xsl:attribute name="href">
          <xsl:value-of select="$BaseUrl"></xsl:value-of>
        </xsl:attribute>
      <font style="color:#aaaaaa;font:normal 10px">   
      <xsl:value-of select="$Footnote_Text"/>
    </font></a></td></tr>

	
	<!-- Prev link for pagination -->
	<font style="font:11px Helvetica, sans-serif; color:#404141;">
	<xsl:choose>
		<xsl:when test="number($Page)-1 &gt;= 0">&#160;
		<A><xsl:attribute name="href">_dirresult?page=<xsl:value-of select="number($Page)-1"/>&amp;pagesize=<xsl:value-of 
select="$PageSize"/></xsl:attribute>&lt;&lt;Prev</A>
		</xsl:when>
		<xsl:otherwise>
		<!-- display something else -->
		</xsl:otherwise>
      </xsl:choose>
      
      <xsl:if test="$selectedRowCount &gt; 1">
       <b class="blacktext"><xsl:value-of select="number($Page)+1"/> of <xsl:value-of select="number($selectedRowCount)"/></b>
      </xsl:if>
               
      <!-- Next link for pagination -->
      <xsl:choose>
       <xsl:when test="number($Page)+1 &lt; number($selectedRowCount)">
        <A><xsl:attribute name="href">_dirresult?page=<xsl:value-of select="number($Page)+1"/>&amp;pagesize=<xsl:value-of 
select="$PageSize"/></xsl:attribute> Next&gt;&gt;</A>
       </xsl:when>
       <xsl:otherwise>
        <!-- display something else -->
       </xsl:otherwise>
      </xsl:choose>
      </font>




    </table>
  </body>
  </html>
</xsl:template>
</xsl:stylesheet>

