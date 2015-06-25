<!--
	- XSLT is a template based language to transform Xml documents
	It uses XPath to select specific nodes 
	for processing.
	
	- A XSLT file is a well formed Xml document
-->

<!-- every StyleSheet starts with this tag -->
<xsl:stylesheet
       xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
       xmlns:structure="http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure" 
       xmlns:common="http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common" 
       xmlns:message="http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message"
       xmlns:xml="http://www.w3.org/XML/1998/namespace"
      version="1.0">

  <!-- indicates what our output type is going to be -->
  <xsl:output method="html" />

  <!-- 
		Main template to kick off processing our Sample.xml
		From here on we use a simple XPath selection query to 
		get to our data.
	-->
  <xsl:template match="/">
                <div class="CodeListView">
                  <div id="reg_content_containers">
                    <caption>
                      <h2>
                         <xsl:value-of select="message:Structure/message:CodeLists/structure:CodeList/structure:Name [@xml:lang='fr']"/>
                        </h2>
                        <h5></h5>
                    </caption>

                      <div id="reg_wide_sec_ppup">
                        
                        <xsl:for-each select="message:Structure/message:CodeLists/structure:CodeList/structure:Code/structure:Description [@xml:lang='fr']">

                            <div class="reg_strct_ppup_clmn_frst">
                              <img src="../../stock/themes/default/images/Select_normal.png" alt="Select" />
                            </div>
                            <div class="reg_strct_ppup_clmn_scnd">
                              <xsl:value-of select="."/>
                            </div>
                            <div class="reg_strct_ppup_clmn_thrd">
                              <xsl:value-of select="../@value"/>
                            </div>
                          <div class="reg_strct_ppup_clmn_clear"></div>
                        </xsl:for-each>
                      </div>
                  </div>                    
                </div>
  </xsl:template>

</xsl:stylesheet>
