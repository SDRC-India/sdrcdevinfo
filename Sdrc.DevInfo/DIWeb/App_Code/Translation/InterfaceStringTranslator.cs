using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;

/// <summary>
/// Summary description for InterfaceStringTranslator
/// </summary>
public class InterfaceStringTranslator
{
    #region "-- Private --"

    #region "-- Methods --"

    private DataTable CreateLanguageTable()
    {
        DataTable DataTbl = new DataTable();

        DataColumn dataColumn;

        dataColumn = new DataColumn();
        dataColumn.DataType = Type.GetType("System.String");
        dataColumn.ColumnName = "Key";
        DataTbl.Columns.Add(dataColumn);

        dataColumn = new DataColumn();
        dataColumn.DataType = Type.GetType("System.String");
        dataColumn.ColumnName = "SourceValue";
        DataTbl.Columns.Add(dataColumn);

        dataColumn = new DataColumn();
        dataColumn.DataType = Type.GetType("System.String");
        dataColumn.ColumnName = "TargetValue";
        DataTbl.Columns.Add(dataColumn);

        return DataTbl;
    }

    private void InsertDataIntoLanguageTbl(string key, string sourceValue, string targetValue, DataTable languageTbl)
    {
        DataRow row;

        row = languageTbl.NewRow();

        row["Key"] = key;
        row["SourceValue"] = sourceValue;
        row["TargetValue"] = targetValue;

        languageTbl.Rows.Add(row);
    }

    #endregion

    #endregion

    #region "-- Public --"

    #region "-- Methods --"

    public InterfaceStringTranslator()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public DataTable GetTranslationTable(string languageFilePath, string sourceLanguage, string targetLanguage)
    {
        DataTable LanguageTbl = new DataTable();
        string SrcLanguageFileName = string.Empty;
        string TrgLanguagefileName = string.Empty;
        string Key = string.Empty;
        string SourceValue = string.Empty;
        string TargetValue = string.Empty;

        LanguageTbl = CreateLanguageTable();

        SrcLanguageFileName = languageFilePath + sourceLanguage + ".xml";
        TrgLanguagefileName = languageFilePath + targetLanguage + ".xml";

        XmlDocument xmlDocSource = new XmlDocument();
        xmlDocSource.Load(SrcLanguageFileName);
        XmlNodeList xmlNodeList = xmlDocSource.SelectNodes("root/Row");

        XmlDocument xmlDocTarget = new XmlDocument();
        xmlDocTarget.Load(TrgLanguagefileName);
        XmlElement Element;        

        foreach (XmlNode xmlNode in xmlNodeList)
        {
            Key = xmlNode.Attributes[0].Value;
            SourceValue = xmlNode.Attributes[1].Value;

            //Find key value in target language file.
            Element = xmlDocTarget.GetElementById(Key);

            try
            {
                TargetValue = Element.Attributes["value"].Value;
            }
            catch (Exception ex)
            {
                TargetValue = "";
                Global.CreateExceptionString(ex, null);
            }

            InsertDataIntoLanguageTbl(Key, SourceValue, TargetValue, LanguageTbl);
        }

        return LanguageTbl;
    }

    #endregion

    #endregion
}
