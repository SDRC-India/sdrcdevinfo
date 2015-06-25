using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Map;
using System.Xml;
using DevInfo.Lib.DI_LibDAL;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Drawing;

namespace DevInfo.Lib.DI_LibBAL.Controls.MetadataBAL
{
    /// <summary>
    /// Class contains functions having Bussiness Logic for displaying Metadata Info .
    /// </summary>
    public class Metadata
    {
        #region " -- Private -- "
        #region " -- Methods --"

        /// <summary>
        /// Get the base layer (most recent) associated with an area
        /// </summary>
        /// <param name="AreaNId"></param>
        /// <param name="DIConnection"></param>
        /// <param name="DIQueries"></param>
        /// <returns>Layer NId</returns>
        private static int GetLayerNId(string AreaNId, DIConnection DIConnection, DIQueries DIQueries)
        {
            int RetVal = -1;
            string sSql = DIQueries.Area.GetAreaMapByAreaNIds(AreaNId, false); //Consider base layers only
            DataTable dtAreaMap = DIConnection.ExecuteDataTable(sSql);
            if (dtAreaMap != null && dtAreaMap.Rows.Count > 0)
            {
                //OPT If multiple base layer are associated with an area select latest map available
                RetVal = (int)dtAreaMap.Rows[0][Area_Map.LayerNId];
            }
            dtAreaMap.Dispose();
            return RetVal;
        }

        /// <summary>
        /// Get layer name based on layer nid
        /// </summary>
        /// <param name="LayerNId">Layer NId</param>
        /// <param name="DIConnection">DIConnection</param>
        /// <param name="DIQueries">DIQueries</param>
        /// <returns>Layer Name</returns>
        private static string GetLayerName(string layerNId, DIConnection DIConnection, DIQueries DIQueries)
        {
            string RetVal = string.Empty;
            string sSql = DIQueries.Area.GetAreaMapLayer(DevInfo.Lib.DI_LibDAL.Queries.Area.Select.MapFilterFieldType.LayerNId, layerNId.ToString(), FieldSelection.Light);
            DataTable dtLayer = DIConnection.ExecuteDataTable(sSql);
            if (dtLayer != null & dtLayer.Rows.Count > 0)
            {
                RetVal = dtLayer.Rows[0][Area_Map_Metadata.LayerName].ToString();
            }
            dtLayer.Dispose();
            return RetVal;
        }

        /// <summary>
        /// This will write text content to the file specified
        /// </summary>
        /// <param name="textToWrite">Text Content  to be written in the file</param> 
        /// <param name="fileToWrite"> File to be written</param>
        private static void WriteToFile(string textToWrite, string fileToWrite)
        {
            System.IO.StreamWriter stWriter = new System.IO.StreamWriter(fileToWrite);
            stWriter.Write(textToWrite);
            stWriter.Close();
            stWriter.Dispose();

        }

        /// <summary>
        /// Function to extract the images from the database and copy then to the temp location   Get icons referred inside metadata
        /// </summary>
        private static void ExtractImage(int elementNId, IconElementType IconElementType, DIConnection DIConnection, DIQueries DIQueries, String OutPutPath)
        {
            string sSql = DIQueries.Icon.GetIcon(elementNId.ToString(), IconElementType);
            DataTable dtIcon = DIConnection.ExecuteDataTable(sSql);
            try
            {
                if (dtIcon != null)
                {
                    for (int index = 0; index < dtIcon.Rows.Count; index++)
                    {
                        Byte[] Buffer;
                        Buffer = (Byte[])dtIcon.Rows[index][Icons.ElementIcon];
                        //create memory stream from this array of bytes 
                        System.IO.MemoryStream str = new System.IO.MemoryStream(Buffer);
                        System.Drawing.Image.FromStream(str).Save(System.IO.Path.Combine(OutPutPath, dtIcon.Rows[index][Icons.IconNId].ToString() + "." + dtIcon.Rows[index][Icons.IconType].ToString()));
                        str.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                //throw ex;
            }

        }

        /// <summary>
        /// write a Html file at the specified path
        /// </summary>
        /// <param name="outPutPath"> directory Path where Html file will be created</param>
        /// <param name="outputFileName"> Name of the file created (without Extention) </param>
        /// <remarks>
        /// This Html File is used for combining Xml file content with Xsl style sheet.
        /// </remarks>
        private static void WriteHtmlFile(string outPutPath, String outputFileName)
        {
            System.IO.StreamWriter stWriter;
            stWriter = new System.IO.StreamWriter(System.IO.Path.Combine(outPutPath, outputFileName + ".html"));
            stWriter.WriteLine("<html>");
            stWriter.WriteLine("<body><script type=\"text/javascript\">");
            stWriter.WriteLine("var xml = new ActiveXObject(\"Microsoft.XMLDOM\")");
            stWriter.WriteLine("xml.async = false");
            stWriter.WriteLine("xml.load(\"" + outputFileName + ".xml" + "\")");
            stWriter.WriteLine("var xsl = new ActiveXObject(\"Microsoft.XMLDOM\")");
            stWriter.WriteLine("xsl.async = false");
            stWriter.WriteLine("xsl.load(\"" + outputFileName + ".xsl" + "\")");
            stWriter.WriteLine("document.write(xml.transformNode(xsl))</script></body>");
            stWriter.WriteLine("</html>");
            stWriter.Close();
            stWriter.Dispose();

        }


        #endregion
        #endregion

        #region " -- Public -- "

        private static int _LayerNId = -1;
        /// <summary>
        /// Set layer NId in case Area metadata assocaiated to a specific map is desired
        /// </summary>
        public static int LayerNId
        {
            set { _LayerNId = value; }
        }


        /// <summary>
        ///  Main Method for Displaying Metadata
        /// </summary>       
        /// <param name="elementType"> Element Type.<example>ElementType.All, ElementType.Indicator </example> <seealso cref="DI_MetaInfo.ElementType"/></param>
        /// <param name="elementNid">ID of selected Data.IND - IndicatorNId, Area - AreaNId, Source - ICNId</param>
        /// <param name="DI_QueryBase"> This contain Connection Details</param>
        /// <param name="outPutPath">This contains the path where  output files were created</param>
        /// <param name="outputFileName"> 
        /// this will be  the name of outputFile without extention
        /// which will be created at the output path to display metadata
        /// <remarks> extension like ".xml" ".xsl" and ".html" are added to this outputFile name 
        /// while creating files </remarks>
        /// </param>
        /// <returns>
        /// </returns>
        public static bool ExtractXmlMetadata(MetadataElementType metadataElementType, int elementNId, DIConnection DIConnection, DIQueries DIQueries, string outputFolder, string outputFileName, ref string infoTitle)
        {
            bool RetVal = false;
            infoTitle = string.Empty;   //Will return the Element name against  ElementNId
            IconElementType IconElementType = IconElementType.MetadataIndicator;
            XmlDocument XMLDoc = new XmlDocument();

            DataTable DTMetaDataInfo = null;
            string MetadataInfo = string.Empty;
            String sSql = "";
            int LayerNId = -1;
            string TargetObjectID = string.Empty;
            Lib.DI_LibSDMX.MetadataTypes SDMXMetadataType = DevInfo.Lib.DI_LibSDMX.MetadataTypes.Indicator;

            switch (metadataElementType)
            {
                case MetadataElementType.Area:
                    IconElementType = IconElementType.MetadataArea;
                    SDMXMetadataType = DevInfo.Lib.DI_LibSDMX.MetadataTypes.Area;

                    //-- Get LayerNId associated with AreaNId
                    if (_LayerNId == -1)
                    {
                        LayerNId = GetLayerNId(elementNId.ToString(), DIConnection, DIQueries);
                    }
                    else
                    {
                        LayerNId = _LayerNId;
                    }
                    //if (LayerNId != -1)
                    //{
                    //    //-- Get Layer Metadata Info
                    //    sSql = DIQueries.Area.GetAreaMapMetadata(LayerNId.ToString());
                    //    DTMetaDataInfo = DIConnection.ExecuteDataTable(sSql);
                    //    if (DTMetaDataInfo != null & DTMetaDataInfo.Rows.Count > 0)
                    //    {
                    //        MetadataInfo = DTMetaDataInfo.Rows[0][Area_Map_Metadata.MetadataText].ToString();
                    //    }
                    //}
                    // Set Meta Info title.
                    sSql = DIQueries.Area.GetArea(FilterFieldType.NId, elementNId.ToString());
                    DTMetaDataInfo = DIConnection.ExecuteDataTable(sSql);
                    if (DTMetaDataInfo != null & DTMetaDataInfo.Rows.Count > 0)
                    {
                        infoTitle = DTMetaDataInfo.Rows[0][Area.AreaName].ToString() + " (" + DTMetaDataInfo.Rows[0][Area.AreaID].ToString() + ")";
                        TargetObjectID = Convert.ToString(DTMetaDataInfo.Rows[0][Area.AreaID]);
                    }
                    break;
                case MetadataElementType.Indicator:
                    IconElementType = IconElementType.MetadataIndicator;
                    SDMXMetadataType = DevInfo.Lib.DI_LibSDMX.MetadataTypes.Indicator;

                    sSql = DIQueries.Indicators.GetIndicator(FilterFieldType.NId, elementNId.ToString(), FieldSelection.Heavy);
                    DTMetaDataInfo = DIConnection.ExecuteDataTable(sSql);
                    if (DTMetaDataInfo != null & DTMetaDataInfo.Rows.Count > 0)
                    {
                        infoTitle = DTMetaDataInfo.Rows[0][Indicator.IndicatorName].ToString();
                        TargetObjectID = Convert.ToString(DTMetaDataInfo.Rows[0][Indicator.IndicatorGId]);
                    }
                    break;

                case MetadataElementType.Source:
                    IconElementType = IconElementType.MetadataSource;
                    SDMXMetadataType = DevInfo.Lib.DI_LibSDMX.MetadataTypes.Source;

                    sSql = DIQueries.IndicatorClassification.GetIC(FilterFieldType.NId, elementNId.ToString(), ICType.Source, FieldSelection.Heavy);
                    DTMetaDataInfo = DIConnection.ExecuteDataTable(sSql);
                    if (DTMetaDataInfo != null & DTMetaDataInfo.Rows.Count > 0)
                    {
                        infoTitle = DTMetaDataInfo.Rows[0][IndicatorClassifications.ICName].ToString();
                        TargetObjectID = Convert.ToString(DTMetaDataInfo.Rows[0][IndicatorClassifications.ICGId]);
                    }
                    break;
            }


            try
            {
                // Get metadata xml file using SDMX library

                MetadataInfo = Lib.DI_LibSDMX.SDMXUtility.Get_MetadataReport(DevInfo.Lib.DI_LibSDMX.SDMXSchemaType.Two_One, TargetObjectID, SDMXMetadataType, "MDAgency", DIQueries.LanguageCode.Replace("_", ""), DIConnection, DIQueries).InnerXml;
                
                DevInfo.Lib.DI_LibBAL.DA.DML.DI7MetaDataBuilder MetadataBuilder = new DevInfo.Lib.DI_LibBAL.DA.DML.DI7MetaDataBuilder(DIConnection,DIQueries);
                
                // Get metadata xml file with category name..
                XMLDoc = MetadataBuilder.GetMetadataReportWCategoryName(MetadataInfo,metadataElementType);

                MetadataInfo= XMLDoc.InnerXml;
                
                //Write Metadata text to physical file
                // --Change on 18-03-08 .blank xml with root element will be written in case of no metadata info found in database
                // If no metadata info create a blank xml file. with root element.
                if (string.IsNullOrEmpty(MetadataInfo) || MetadataInfo.Trim().Length == 0)
                {
                    WriteToFile("<?xml version='1.0'?><root ></root >", System.IO.Path.Combine(outputFolder, outputFileName + ".xml"));
                }
                else
                {
                    WriteToFile(MetadataInfo, System.IO.Path.Combine(outputFolder, outputFileName + ".xml"));
                }
                // Commented on 18-march -2008. shifted in if else part above.
                ////WriteToFile(MetadataInfo, System.IO.Path.Combine(outputFolder, outputFileName + ".xml"));
            }
            catch (Exception ex)
            {
                RetVal = false;
            }

            if (MetadataInfo.Trim().Length > 0)
            {

                RetVal = true;

                //Write XLS and HTMLFile
                try
                {
                    switch (metadataElementType)
                    {
                        case MetadataElementType.Area:
                            //In Case Area Get Xslt associated with LayerNId (not with AreaNId)
                            sSql = DIQueries.Xslt.GetXSLT(LayerNId.ToString(), metadataElementType);
                            break;
                        case MetadataElementType.Indicator:
                        case MetadataElementType.Source:
                            sSql = DIQueries.Xslt.GetXSLT(elementNId.ToString(), metadataElementType);
                            break;
                        default:
                            break;
                    }
                    DataTable XSLTInfo = (DIConnection.ExecuteDataTable(sSql));

                    // If  XSLT info was Found Write write Xsl and Html file
                    if (XSLTInfo != null & XSLTInfo.Rows.Count > 0)
                    {
                        // Creating Xsl File                           
                        String xslString = XSLTInfo.Rows[0]["XSLT_Text"].ToString();
                        WriteToFile(xslString, System.IO.Path.Combine(outputFolder, outputFileName + ".xsl"));

                        //Write HTml file
                        WriteHtmlFile(outputFolder, outputFileName);
                    }

                }
                catch (Exception)
                {
                }

                //Extract Images associated with metadata
                try
                {
                    if (metadataElementType == MetadataElementType.Area)
                    {
                        ExtractImage(LayerNId, IconElementType, DIConnection, DIQueries, outputFolder);
                    }
                    else
                    {
                        ExtractImage(elementNId, IconElementType, DIConnection, DIQueries, outputFolder);
                    }

                }
                catch (Exception)
                {

                    //Old database may not have Icon table
                }

            }

            infoTitle = infoTitle.Replace("&", "&&"); //label control do not display & character
            return RetVal;
        }


        /// <summary>
        ///  Get path for selected area starting from root area
        /// </summary>                      
        /// <param name="ElementNid">ID of selected Data</param>
        /// <param name="DI_QueryBase">This contains Connection Details</param>
        /// <returns>String containg the path of area node starting from root node</returns>
        public static string GetAreaChain(int AreaNId, DIConnection DIConnection, DIQueries DIQueries)
        {
            string RetVal = string.Empty;
            string sSql = string.Empty;
            int AreaLevel = 1;

            // Get Current Area Level
            sSql = DIQueries.Area.GetArea(FilterFieldType.NId, AreaNId.ToString());
            DataTable dtArea = DIConnection.ExecuteDataTable(sSql);
            if (dtArea != null & dtArea.Rows.Count > 0)
            {
                AreaLevel = (int)dtArea.Rows[0][Area.AreaLevel];
            }

            //Get concatinated Area names
            sSql = DIQueries.Area.GetAreaChain(AreaNId, AreaLevel, DIConnection.ConnectionStringParameters.ServerType);
            RetVal = DIConnection.ExecuteScalarSqlQuery(sSql).ToString();

            return RetVal;
        }

        /// <summary>
        /// Get Image for the selected area
        /// </summary>
        /// <param name="AreaNId"> Area Nid of selected area</param>
        /// <param name="OutPath"> Location where metadata related files are created</param>
        /// <param name="DI_QueryBase"> This contain Conncetion Details</param>
        /// <param name="PicHeight"> Height of the map </param>
        /// <param name="PicWidth">width of the map</param>
        /// <returns></returns>
        public static Image GetAreaMapImage(DIConnection DIConnection, DIQueries DIQueries, int AreaNId, string TempFolder, int PicHeight, int PicWidth)
        {
            Image RetVal = null;
            int LayerNId = -1;
            try
            {
                //-- If any specific layer has not been defined then get the first base layer associated with the area
                if (_LayerNId == -1)
                {
                    LayerNId = GetLayerNId(AreaNId.ToString(), DIConnection, DIQueries);
                }
                else
                {
                    LayerNId = _LayerNId;
                }


                if (LayerNId != -1)
                {

                    //Get Layer Name
                    string LayerName = GetLayerName(LayerNId.ToString(), DIConnection, DIQueries);

                    //Extarct shape file for associated layer
                    Map.ExtractShapeFileByLayerId(LayerNId.ToString(), TempFolder, DIConnection, DIQueries);

                    Map map = new Map();
                    map.CanvasColor = Color.White;

                    // Add layer to map layer collection
                    map.Layers.Clear();
                    map.Width = PicWidth;
                    map.Height = PicHeight;
                    map.Layers.AddShapeFile(TempFolder, LayerName);
                    map.Layers[0].FillColor = System.Drawing.Color.White;
                    map.SetFullExtent();

                    //Extract map image
                    RetVal = Image.FromStream(map.GetMapStream());

                    map.Dispose();
                    map = null;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return RetVal;
        }

        /// <summary>
        /// Getting MetaData and writing them to Rtf file.
        /// </summary>
        public static string ExtractRtfMetadata(ICType ICType, int ICNId, DIConnection DIConnection, DIQueries DIQueries, String outputFolder, String outputFileName)
        {
            string RetVal = string.Empty;
            string RtfFilePath = System.IO.Path.Combine(outputFolder, outputFileName + ".rtf");
            DataTable RtfDataTable = new DataTable();
            try
            {
                if (System.IO.File.Exists(RtfFilePath))
                {
                    System.IO.File.Delete(RtfFilePath);
                }
                // getting metadata informationn from Database into DataTable RtfDataTable
                string sSql = DIQueries.IndicatorClassification.GetIC(FilterFieldType.NId, ICNId.ToString(), ICType, FieldSelection.Heavy);
                RtfDataTable = DIConnection.ExecuteDataTable(sSql);
                if (RtfDataTable != null & RtfDataTable.Rows.Count > 0)
                {
                    RetVal = RtfDataTable.Rows[0][IndicatorClassifications.ICName].ToString();
                    if (!Convert.IsDBNull(RtfDataTable.Rows[0][IndicatorClassifications.ICInfo]))
                    {
                        string RtfString = RtfDataTable.Rows[0][IndicatorClassifications.ICInfo].ToString();

                        // Writng  metadata into  a rtf file in output directory
                        if (RtfString.Trim().Length > 0)
                        {
                            WriteToFile(RtfString, RtfFilePath);

                        }
                    }
                }
            }
            catch (Exception)
            {
                //throw ex;
            }

            return RetVal;
        }


        #endregion

    }
}
