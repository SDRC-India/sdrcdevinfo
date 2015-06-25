using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.IO;
using System.Configuration;
using System.Data;

/// <summary>
/// Migrate html content to database
/// </summary>
public class HtmlContentMigrator
{

    /// Create CMS database for adaptation If Database Not exise
    /// </summary>
    /// <param name="CMSDatabaseName">Name of the created Cms Database</param>
    /// <returns>True if database created or already exist</returns>
    public bool MigratorHtmlContentToDb(List<DataContent> InputListDataContent, out string ContentDbImportMessage)
    {
        bool RetVal = false;
        DIConnection ObjDIConnection = null;
        CMSHelper ObjCmsHelper = new CMSHelper();
        string DbScripts = string.Empty;
        string LngDbScripts = string.Empty;
        string ConnectionMessage = string.Empty;
        string QryStr = string.Empty;
        DataTable DTCounts;

        List<System.Data.Common.DbParameter> DbParams = new List<System.Data.Common.DbParameter>();
        try
        {

            ObjDIConnection = ObjCmsHelper.GetConnectionObject(out ConnectionMessage);
            if (ObjDIConnection == null)
            {
                ContentDbImportMessage = ConnectionMessage;
                RetVal = false;
                return RetVal;
            }
            else
            {
                foreach (DataContent DContent in InputListDataContent)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(DContent.Title) || !string.IsNullOrEmpty(DContent.Summary.ToString()) || !string.IsNullOrEmpty(DContent.Thumbnail.ToString())
                            || !string.IsNullOrEmpty(DContent.Description.ToString()) || !string.IsNullOrEmpty(DContent.PDFUpload.ToString()))
                        {
                            QryStr = "select * from DataContent where Title='" + DContent.Title.Replace("'", "''") + "'" + "and Date='" + DContent.Date + "' and Summary='" + DContent.Summary.Replace("'", "''") + "'";

                            DTCounts = ObjDIConnection.ExecuteDataTable(QryStr);


                            if (DTCounts.Rows.Count == 0)
                            {

                                QryStr = "INSERT into DataContent (MenuCategory, Title, Date, Thumbnail,  Summary, Description, PDFUpload, DateAdded,DateModified, Archived,  ArticleTagId, UserNameEmail, URL, LngCode, IsDeleted,IsHidden) Values (" + "'" + DContent.MenuCategory + "','" + DContent.Title.Replace("'", "''") + "','" + DContent.Date + "','" + DContent.Thumbnail + "','" + DContent.Summary.Replace("'", "''") + "','" + DContent.Description.Replace("'", "''") + "','" + DContent.PDFUpload + "','" + DContent.DateAdded + "','" + DContent.DateModified + "','" + DContent.Archived + "','" + DContent.ArticleTagID + "','" + DContent.UserNameEmail.Replace("'", "''") + "','" + DContent.URL + "','" + DContent.LngCode + "','" + DContent.IsDeleted + "','" + DContent.IsHidden + "')";

                                ObjDIConnection.ExecuteNonQuery(QryStr);
                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        string ExceptionMgs = "Exception occured in method ImportHtmlContentToDb while insering records in database,";
                        ExceptionMgs = " Exception message is" + Ex.InnerException.ToString() + "Stack trace is" + Ex.StackTrace.ToString();
                        Global.WriteErrorsInLogFolder(ExceptionMgs);
                        ContentDbImportMessage = "Error occured while importing datavalues to database";
                        RetVal = false;
                    }
                }
            }


            // Set Retval to true
            RetVal = true;
            // Dispose connection object
            ObjDIConnection.Dispose();
        }
        catch (Exception ex)
        {
            ContentDbImportMessage = "Error occured while importing datavalues to database";
            RetVal = false;
            Global.CreateExceptionString(ex, null);
        }

        finally
        { // dispose connection object
            ObjDIConnection = null;
        }
        ContentDbImportMessage = string.Empty;
        return RetVal;
    }





}