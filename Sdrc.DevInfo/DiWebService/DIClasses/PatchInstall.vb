Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Data
Imports System.IO
Imports System.Xml
Imports System.Collections
Imports System.Xml.Linq
Imports DALConnection = DevInfo.Lib.DI_LibDAL.Connection
Imports System.Data.SqlClient

''' <summary>
''' methods for installing patch, updating language  xml, updating app setting file
''' </summary>
Public Class PatchInstall

#Region "--Internal--"
    Friend Shared StatusSucess As String = ReadKeysForPatch("StatusPassed").ToString()
    Friend Shared StatusFailed As String = ReadKeysForPatch("StatusFail").ToString()
#End Region

#Region "-- Public --"


    ''' <summary>
    ''' Install all patches from 7.0.0.3 to 7.0.0.9
    ''' If installation fails at any stop further execution of code
    ''' </summary>
    ''' <returns>true if patches installed from 7.0.0.3 to 7.0.0.9 sucessfully, else return false</returns>

    Public Function InstallPatch() As Boolean
        Dim RetVal As Boolean = False
        Dim LogMessage As String = String.Empty
        Dim DbConnectionstring As String = String.Empty

        Try
            DbConnectionstring = ConfigurationManager.AppSettings("DBConnString")
        Catch ex As Exception
            RetVal = False
            LogMessage = ReadKeysForPatch("ReadConnectionString").ToString()
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusSucess, ex.Message.ToString())
        End Try
        ' check if patch 7.0.0.3 installed successfully
        If Me.InstallPatch7_0_0_3() Then
            ' check if patch 7.0.0.4 installed successfully
            If Me.InstallPatch7_0_0_4(DbConnectionstring) Then
                ' check if patch 7.0.0.5 installed successfully
                If Me.InstallPatch7_0_0_5() Then
                    ' check if patch 7.0.0.6 installed successfully
                    If Me.InstallPatch7_0_0_6() Then
                        ' check if patch 7.0.0.7 installed successfully
                        If Me.InstallPatch7_0_0_7(DbConnectionstring) Then
                            ' check if patch 7.0.0.8 installed successfully
                            If Me.InstallPatch7_0_0_8() Then
                                ' check if patch 7.0.0.9 installed successfully
                                If Me.InstallPatch7_0_0_9(DbConnectionstring) Then
                                    RetVal = True
                                    Return RetVal
                                Else
                                    RetVal = False
                                    Return RetVal
                                End If
                            Else
                                RetVal = False
                                Return RetVal
                            End If
                        Else
                            RetVal = False
                            Return RetVal
                        End If
                    Else
                        RetVal = False
                        Return RetVal
                    End If
                Else
                    RetVal = False
                    Return RetVal
                End If
            Else
                RetVal = False
                Return RetVal
            End If
        Else
            RetVal = False
            Return RetVal
        End If


    End Function



#End Region

#Region "-- Private --"

#Region "-- InstallPatches --"

    ''' <summary>
    ''' Remove unwated tables from webservices database.
    ''' </summary>
    ''' <returns>True if all the unwanted tables removed successfully, else return false</returns>
    ''' <remarks></remarks>
    Private Function InstallPatch7_0_0_3() As Boolean
        Dim RetVal As Boolean = True
        Dim Table1 As String = String.Empty
        Dim LogMessage As String = String.Empty
        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Try
            ObjDIConnection = SharedFunctions.GetDbConnection()
        Catch ex As Exception
            LogMessage = String.Format(ReadKeysForPatch("GetDatabaseConnection").ToString(), ex.Message.ToString(), "7.0.0.3")
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusFailed, ex.Message.ToString())
        End Try

        Try
            Dim AllTablesToDrop As New List(Of String)()
            AllTablesToDrop.Add("mAdaptation")
            AllTablesToDrop.Add("mCountry")
            AllTablesToDrop.Add("mRegion")
            AllTablesToDrop.Add("mUserInfo")
            AllTablesToDrop.Add("rCountryAdaptation")
            AllTablesToDrop.Add("rUserAdaptation")
            AllTablesToDrop.Add("Sheet1$")
            Dim Query As String = String.Empty
            '--- Remeove all the tables that are not in use ---
            '1. Itterate througe loop, to Get table name
            '2. Check if table exists, drop table
            '2.1 If table removed successfull add entry in log file
            '2.2 else if error occured in removing table , add entry in log file
            '3. else if table exists  do nothing

            ' Loop over each element with For Each.
            For Each TableName As String In AllTablesToDrop

                Query = "IF EXISTS (SELECT * FROM dbo.sysobjects where id = object_id('dbo." + TableName + "'))" +
                                " drop table dbo." + TableName
                ObjDIConnection.ExecuteScalarSqlQuery(Query)
            Next
            ' If all the tables removed successfully
            ' Set return value to true
            ' Add entry in xls log file for patch, for success
            LogMessage = ReadKeysForPatch("RemoveExtraTables").ToString()
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, ReadKeysForPatch("StatusPassed").ToString(), String.Empty)
            RetVal = True

        Catch ex As Exception
            ' If exception occured in removing tables
            ' Set return value to false
            ' Add entry in xls log file for patch, for faliure
            LogMessage = ReadKeysForPatch("RemoveExtraTables").ToString()
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusFailed, ex.Message.ToString())
            RetVal = False
        End Try
        Return RetVal
    End Function

    Private Function InstallPatch7_0_0_4(DbConnectionstring As String) As Boolean
        Dim RetVal As Boolean = True
        Dim DbCon As SqlConnection = Nothing
        Dim SqlCmd As SqlCommand = Nothing
        Dim Query As String = String.Empty
        Dim tran As SqlTransaction = Nothing
        Dim LogMessage As String = String.Empty
        Dim PatchMessagesFile As String = String.Empty
        Try
            DbCon = New SqlConnection(DbConnectionstring)
            SqlCmd = New SqlCommand()
            DbCon.Open()
            ' Open database connection
            ' Begin Transaction
            tran = DbCon.BeginTransaction()
        Catch Ex As Exception
            RetVal = False
            LogMessage = String.Format(ReadKeysForPatch("OpenDatabaseConnection").ToString(), Ex.Message.ToString(), "7.0.0.4")
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusFailed, Ex.Message.ToString())
            Return RetVal
        End Try
        Try
            'Query to add GUID column in the adaptation table, if column does not exists
            Query = "IF NOT EXISTS(SELECT * FROM sys.columns where Name = N'GUID' and Object_ID = Object_ID(N'Adaptations')) Begin ALTER TABLE Adaptations DROP COLUMN Date_Created;ALTER TABLE Adaptations Drop CONSTRAINT [def_Visible_In_Catalog];ALTER TABLE Adaptations DROP COLUMN Visible_In_Catalog;ALTER TABLE Adaptations ADD GUID nvarchar(50);ALTER TABLE Adaptations ADD Date_Created nvarchar(100);ALTER TABLE Adaptations ADD Visible_In_Catalog nvarchar(10);ALTER TABLE Adaptations ADD CONSTRAINT [def_Visible_In_Catalog] DEFAULT ('False') FOR [Visible_In_Catalog];Update Adaptations SET Date_Created ='1/1/2013 00:00:00 AM' Update Adaptations SET Visible_In_Catalog = 'True' END"
            SqlCmd.CommandText = Query
            SqlCmd.Connection = DbCon
            SqlCmd.Transaction = tran
            SqlCmd.ExecuteNonQuery() ' Execute query to add GUID column in the adaptation table 
            LogMessage = ReadKeysForPatch("GUIDColCreation").ToString()
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusSucess, String.Empty)
            '----------------------------------------------------------------

            'Query to alter procedure SP_UPDATE_CATALOG_INFO, for changes regarding guid
            Query = "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_UPDATE_CATALOG_INFO]')) Begin EXEC dbo.sp_executesql @statement =N'CREATE PROC [dbo].[SP_UPDATE_CATALOG_INFO] @NAME NVARCHAR(4000), @DI_VERSION NVARCHAR(50), @ONLINE_URL NVARCHAR(4000), @LAST_MODIFIED NVARCHAR(50), @AREA_NID INT, @SUB_NATION NVARCHAR(4000), @DB_ADM_NAME NVARCHAR(4000), @DB_ADM_INSTITUTION NVARCHAR(4000), @DB_ADM_EMAIL NVARCHAR(4000), @UNICEF_REGION NVARCHAR(4000), @ADAPTATION_YEAR NVARCHAR(4), @Adapted_For NVARCHAR(4000), @Country NVARCHAR(4000), @AdaptationGUID NVARCHAR(100) AS BEGIN UPDATE ADAPTATIONS SET NAME = @NAME, LAST_MODIFIED = @LAST_MODIFIED, AREA_NID = @AREA_NID, SUB_NATION = @SUB_NATION, DB_ADM_NAME = @DB_ADM_NAME, DB_ADM_INSTITUTION = @DB_ADM_INSTITUTION, DB_ADM_EMAIL = @DB_ADM_EMAIL, UNICEF_REGION = @UNICEF_REGION, ADAPTATION_YEAR = @ADAPTATION_YEAR, Adapted_For=@Adapted_For, Country=@Country, ONLINE_URL = @ONLINE_URL WHERE [GUID] = @AdaptationGUID END' End ELSE BEGIN EXEC dbo.sp_executesql @statement =N'ALTER PROC [dbo].[SP_UPDATE_CATALOG_INFO] @NAME NVARCHAR(4000), @DI_VERSION NVARCHAR(50), @ONLINE_URL NVARCHAR(4000), @LAST_MODIFIED NVARCHAR(50), @AREA_NID INT, @SUB_NATION NVARCHAR(4000), @DB_ADM_NAME NVARCHAR(4000), @DB_ADM_INSTITUTION NVARCHAR(4000), @DB_ADM_EMAIL NVARCHAR(4000), @UNICEF_REGION NVARCHAR(4000), @ADAPTATION_YEAR NVARCHAR(4), @Adapted_For NVARCHAR(4000), @Country NVARCHAR(4000), @AdaptationGUID NVARCHAR(100) AS BEGIN UPDATE ADAPTATIONS SET NAME = @NAME, LAST_MODIFIED = @LAST_MODIFIED, AREA_NID = @AREA_NID, SUB_NATION = @SUB_NATION, DB_ADM_NAME = @DB_ADM_NAME, DB_ADM_INSTITUTION = @DB_ADM_INSTITUTION, DB_ADM_EMAIL = @DB_ADM_EMAIL, UNICEF_REGION = @UNICEF_REGION, ADAPTATION_YEAR = @ADAPTATION_YEAR, Adapted_For=@Adapted_For, Country=@Country, ONLINE_URL = @ONLINE_URL WHERE [GUID] = @AdaptationGUID END' END"
            SqlCmd.CommandText = Query
            SqlCmd.Connection = DbCon
            SqlCmd.Transaction = tran
            SqlCmd.ExecuteNonQuery() ' Execute query to alter procedure SP_UPDATE_CATALOG_INFO
            LogMessage = String.Format(ReadKeysForPatch("ChangeProcForGuid").ToString(), "SP_UPDATE_CATALOG_INFO")
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusSucess, String.Empty)
            '----------------------------------------------------------------

            'Query to alter procedure SP_INSERT_CATALOG, for changes regarding guid
            Query = "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_INSERT_CATALOG]')) Begin EXEC dbo.sp_executesql @statement =N'CREATE PROC [dbo].[SP_INSERT_CATALOG] @NAME NVARCHAR(4000), @DESCRIPTION NVARCHAR(4000), @DI_VERSION NVARCHAR(50), @IS_DESKTOP BIT, @IS_ONLINE BIT, @ONLINE_URL NVARCHAR(4000), @AREA_COUNT INT, @IUS_COUNT INT, @TIME_PERIODS_COUNT INT, @DATA_VALUES_COUNT INT, @START_YEAR NVARCHAR(50), @END_YEAR NVARCHAR(50), @LAST_MODIFIED NVARCHAR(50), @AREA_NID INT, @SUB_NATION NVARCHAR(4000), @THUMBNAIL_IMAGE_URL NVARCHAR(4000), @DB_ADM_NAME NVARCHAR(4000), @DB_ADM_INSTITUTION NVARCHAR(4000), @DB_ADM_EMAIL NVARCHAR(4000), @UNICEF_REGION NVARCHAR(4000), @ADAPTATION_YEAR NVARCHAR(4), @DB_LANGUAGES NVARCHAR(4000), @LangCode_CSVFiles NVARCHAR(4000), @Adapted_For NVARCHAR(4000), @Country NVARCHAR(4000), @AdaptationGUID NVARCHAR(100), @Date_Created NVARCHAR(4000) AS DECLARE @REC_COUNT INT BEGIN IF(@DI_VERSION = ''-1'') BEGIN SET @DI_VERSION = (SELECT MAX(VER_NID) FROM ADAPTATIONVERSION) END IF(@IS_DESKTOP=1) BEGIN INSERT INTO ADAPTATIONS ( NAME, DESCRIPTION, DI_VERSION, IS_DESKTOP, IS_ONLINE, ONLINE_URL, AREA_COUNT, IUS_COUNT, TIME_PERIODS_COUNT, DATA_VALUES_COUNT, START_YEAR, END_YEAR, LAST_MODIFIED, AREA_NID, SUB_NATION, THUMBNAIL_IMAGE_URL, DB_ADM_NAME, DB_ADM_INSTITUTION, DB_ADM_EMAIL, UNICEF_REGION, ADAPTATION_YEAR, DB_LANGUAGES, LangCode_CSVFiles, Adapted_For, Country, [GUID], Date_Created ) VALUES ( @NAME, @DESCRIPTION, @DI_VERSION, @IS_DESKTOP, @IS_ONLINE, @ONLINE_URL, @AREA_COUNT, @IUS_COUNT, @TIME_PERIODS_COUNT, @DATA_VALUES_COUNT, @START_YEAR, @END_YEAR, @LAST_MODIFIED, @AREA_NID, @SUB_NATION, @THUMBNAIL_IMAGE_URL, @DB_ADM_NAME, @DB_ADM_INSTITUTION, @DB_ADM_EMAIL, @UNICEF_REGION, @ADAPTATION_YEAR, @DB_LANGUAGES, @LangCode_CSVFiles, @Adapted_For, @Country, @AdaptationGUID, @Date_Created ) END IF(@IS_ONLINE=1) BEGIN SET @REC_COUNT = (SELECT COUNT(*) FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID) IF (@REC_COUNT = 0) BEGIN INSERT INTO ADAPTATIONS ( NAME, DESCRIPTION, DI_VERSION, IS_DESKTOP, IS_ONLINE, ONLINE_URL, AREA_COUNT, IUS_COUNT, TIME_PERIODS_COUNT, DATA_VALUES_COUNT, START_YEAR, END_YEAR, LAST_MODIFIED, AREA_NID, SUB_NATION, THUMBNAIL_IMAGE_URL, DB_ADM_NAME, DB_ADM_INSTITUTION, DB_ADM_EMAIL, UNICEF_REGION, ADAPTATION_YEAR, DB_LANGUAGES, LangCode_CSVFiles, Adapted_For, Country, [GUID], Date_Created ) VALUES ( @NAME, @DESCRIPTION, @DI_VERSION, @IS_DESKTOP, @IS_ONLINE, @ONLINE_URL, @AREA_COUNT, @IUS_COUNT, @TIME_PERIODS_COUNT, @DATA_VALUES_COUNT, @START_YEAR, @END_YEAR, @LAST_MODIFIED, @AREA_NID, @SUB_NATION, @THUMBNAIL_IMAGE_URL, @DB_ADM_NAME, @DB_ADM_INSTITUTION, @DB_ADM_EMAIL, @UNICEF_REGION, @ADAPTATION_YEAR, @DB_LANGUAGES, @LangCode_CSVFiles, @Adapted_For, @Country, @AdaptationGUID, @Date_Created ) END ELSE BEGIN UPDATE ADAPTATIONS SET NAME = @NAME, DESCRIPTION = @DESCRIPTION, DI_VERSION = @DI_VERSION, IS_DESKTOP = @IS_DESKTOP, IS_ONLINE = @IS_ONLINE, AREA_COUNT = @AREA_COUNT, IUS_COUNT = @IUS_COUNT, ONLINE_URL = @ONLINE_URL, TIME_PERIODS_COUNT = @TIME_PERIODS_COUNT, DATA_VALUES_COUNT = @DATA_VALUES_COUNT, START_YEAR = @START_YEAR, END_YEAR = @END_YEAR, LAST_MODIFIED = @LAST_MODIFIED, AREA_NID = @AREA_NID, SUB_NATION = @SUB_NATION, THUMBNAIL_IMAGE_URL = @THUMBNAIL_IMAGE_URL, DB_ADM_NAME = @DB_ADM_NAME, DB_ADM_INSTITUTION = @DB_ADM_INSTITUTION, DB_ADM_EMAIL = @DB_ADM_EMAIL, UNICEF_REGION = @UNICEF_REGION, ADAPTATION_YEAR = @ADAPTATION_YEAR, DB_LANGUAGES = @DB_LANGUAGES, LangCode_CSVFiles = @LangCode_CSVFiles, Adapted_For=@Adapted_For, Country=@Country WHERE [GUID] = @AdaptationGUID END END END' End ELSE BEGIN EXEC dbo.sp_executesql @statement =N'ALTER PROC [dbo].[SP_INSERT_CATALOG] @NAME NVARCHAR(4000), @DESCRIPTION NVARCHAR(4000), @DI_VERSION NVARCHAR(50), @IS_DESKTOP BIT, @IS_ONLINE BIT, @ONLINE_URL NVARCHAR(4000), @AREA_COUNT INT, @IUS_COUNT INT, @TIME_PERIODS_COUNT INT, @DATA_VALUES_COUNT INT, @START_YEAR NVARCHAR(50), @END_YEAR NVARCHAR(50), @LAST_MODIFIED NVARCHAR(50), @AREA_NID INT, @SUB_NATION NVARCHAR(4000), @THUMBNAIL_IMAGE_URL NVARCHAR(4000), @DB_ADM_NAME NVARCHAR(4000), @DB_ADM_INSTITUTION NVARCHAR(4000), @DB_ADM_EMAIL NVARCHAR(4000), @UNICEF_REGION NVARCHAR(4000), @ADAPTATION_YEAR NVARCHAR(4), @DB_LANGUAGES NVARCHAR(4000), @LangCode_CSVFiles NVARCHAR(4000), @Adapted_For NVARCHAR(4000), @Country NVARCHAR(4000), @AdaptationGUID NVARCHAR(100), @Date_Created NVARCHAR(4000) AS DECLARE @REC_COUNT INT BEGIN IF(@DI_VERSION = ''-1'') BEGIN SET @DI_VERSION = (SELECT MAX(VER_NID) FROM ADAPTATIONVERSION) END IF(@IS_DESKTOP=1) BEGIN INSERT INTO ADAPTATIONS ( NAME, DESCRIPTION, DI_VERSION, IS_DESKTOP, IS_ONLINE, ONLINE_URL, AREA_COUNT, IUS_COUNT, TIME_PERIODS_COUNT, DATA_VALUES_COUNT, START_YEAR, END_YEAR, LAST_MODIFIED, AREA_NID, SUB_NATION, THUMBNAIL_IMAGE_URL, DB_ADM_NAME, DB_ADM_INSTITUTION, DB_ADM_EMAIL, UNICEF_REGION, ADAPTATION_YEAR, DB_LANGUAGES, LangCode_CSVFiles, Adapted_For, Country, [GUID], Date_Created ) VALUES ( @NAME, @DESCRIPTION, @DI_VERSION, @IS_DESKTOP, @IS_ONLINE, @ONLINE_URL, @AREA_COUNT, @IUS_COUNT, @TIME_PERIODS_COUNT, @DATA_VALUES_COUNT, @START_YEAR, @END_YEAR, @LAST_MODIFIED, @AREA_NID, @SUB_NATION, @THUMBNAIL_IMAGE_URL, @DB_ADM_NAME, @DB_ADM_INSTITUTION, @DB_ADM_EMAIL, @UNICEF_REGION, @ADAPTATION_YEAR, @DB_LANGUAGES, @LangCode_CSVFiles, @Adapted_For, @Country, @AdaptationGUID, @Date_Created ) END IF(@IS_ONLINE=1) BEGIN SET @REC_COUNT = (SELECT COUNT(*) FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID) IF (@REC_COUNT = 0) BEGIN INSERT INTO ADAPTATIONS ( NAME, DESCRIPTION, DI_VERSION, IS_DESKTOP, IS_ONLINE, ONLINE_URL, AREA_COUNT, IUS_COUNT, TIME_PERIODS_COUNT, DATA_VALUES_COUNT, START_YEAR, END_YEAR, LAST_MODIFIED, AREA_NID, SUB_NATION, THUMBNAIL_IMAGE_URL, DB_ADM_NAME, DB_ADM_INSTITUTION, DB_ADM_EMAIL, UNICEF_REGION, ADAPTATION_YEAR, DB_LANGUAGES, LangCode_CSVFiles, Adapted_For, Country, [GUID], Date_Created ) VALUES ( @NAME, @DESCRIPTION, @DI_VERSION, @IS_DESKTOP, @IS_ONLINE, @ONLINE_URL, @AREA_COUNT, @IUS_COUNT, @TIME_PERIODS_COUNT, @DATA_VALUES_COUNT, @START_YEAR, @END_YEAR, @LAST_MODIFIED, @AREA_NID, @SUB_NATION, @THUMBNAIL_IMAGE_URL, @DB_ADM_NAME, @DB_ADM_INSTITUTION, @DB_ADM_EMAIL, @UNICEF_REGION, @ADAPTATION_YEAR, @DB_LANGUAGES, @LangCode_CSVFiles, @Adapted_For, @Country, @AdaptationGUID, @Date_Created ) END ELSE BEGIN UPDATE ADAPTATIONS SET NAME = @NAME, DESCRIPTION = @DESCRIPTION, DI_VERSION = @DI_VERSION, IS_DESKTOP = @IS_DESKTOP, IS_ONLINE = @IS_ONLINE, AREA_COUNT = @AREA_COUNT, IUS_COUNT = @IUS_COUNT, ONLINE_URL = @ONLINE_URL, TIME_PERIODS_COUNT = @TIME_PERIODS_COUNT, DATA_VALUES_COUNT = @DATA_VALUES_COUNT, START_YEAR = @START_YEAR, END_YEAR = @END_YEAR, LAST_MODIFIED = @LAST_MODIFIED, AREA_NID = @AREA_NID, SUB_NATION = @SUB_NATION, THUMBNAIL_IMAGE_URL = @THUMBNAIL_IMAGE_URL, DB_ADM_NAME = @DB_ADM_NAME, DB_ADM_INSTITUTION = @DB_ADM_INSTITUTION, DB_ADM_EMAIL = @DB_ADM_EMAIL, UNICEF_REGION = @UNICEF_REGION, ADAPTATION_YEAR = @ADAPTATION_YEAR, DB_LANGUAGES = @DB_LANGUAGES, LangCode_CSVFiles = @LangCode_CSVFiles, Adapted_For=@Adapted_For, Country=@Country WHERE [GUID] = @AdaptationGUID END END END' END"
            SqlCmd.CommandText = Query
            SqlCmd.Connection = DbCon
            SqlCmd.Transaction = tran
            SqlCmd.ExecuteNonQuery() ' Execute query to alter procedure SP_INSERT_CATALOG\
            LogMessage = String.Format(ReadKeysForPatch("ChangeProcForGuid").ToString(), "SP_INSERT_CATALOG")
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusSucess, String.Empty)
            '----------------------------------------------------------------

            'Query to alter procedure SP_GET_DATE_CREATED, for changes regarding guid
            Query = "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GET_DATE_CREATED]')) Begin EXEC dbo.sp_executesql @statement =N'CREATE PROC [dbo].[SP_GET_DATE_CREATED] @AdaptationURL NVARCHAR(4000) AS BEGIN SELECT Date_Created FROM ADAPTATIONS WHERE [GUID] = @AdaptationURL END' End ELSE BEGIN EXEC dbo.sp_executesql @statement =N'ALTER PROC [dbo].[SP_GET_DATE_CREATED] @AdaptationURL NVARCHAR(4000) AS BEGIN SELECT Date_Created FROM ADAPTATIONS WHERE [GUID] = @AdaptationURL END' END"
            SqlCmd.CommandText = Query
            SqlCmd.Connection = DbCon
            SqlCmd.Transaction = tran
            SqlCmd.ExecuteNonQuery() ' Execute query to alter procedure SP_GET_DATE_CREATED
            LogMessage = String.Format(ReadKeysForPatch("ChangeProcForGuid").ToString(), "SP_GET_DATE_CREATED")
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusSucess, String.Empty)
            '----------------------------------------------------------------

            'Query to alter procedure SP_GET_CATALOG, for changes regarding guid
            Query = "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GET_CATALOG]')) Begin EXEC dbo.sp_executesql @statement =N'CREATE PROC [dbo].[SP_GET_CATALOG] @AdaptationGUID NVARCHAR(4000) AS BEGIN DECLARE @ADPT_AREA_NID INT DECLARE @IS_DI7_ORG_SITE_Val BIT SET @ADPT_AREA_NID = (SELECT AREA_NID FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID) SET @IS_DI7_ORG_SITE_Val=(SELECT IS_DI7_ORG_SITE FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID) IF @ADPT_AREA_NID = -1 BEGIN IF @IS_DI7_ORG_SITE_Val = ''True'' BEGIN SELECT NId,Name,Description,Ver_Name [DI_Version],Is_Desktop,Is_Online,Online_URL,Area_Count,IUS_Count, Time_Periods_Count,Data_Values_Count,Start_Year,End_Year,Last_Modified,Area_NId,Sub_Nation, Thumbnail_Image_URL,Db_Adm_Name,Db_Adm_Institution,Db_Adm_Email,Unicef_Region,Adaptation_Year,Db_Languages,LangCode_CSVFiles FROM ADAPTATIONS, ADAPTATIONVERSION WHERE (AREA_NID IN(@ADPT_AREA_NID) OR AREA_NID IN( SELECT AREANID FROM AREAS WHERE PARENTNID IN( SELECT AREANID FROM AREAS WHERE PARENTNID=-1) )) AND DI_VERSION = VER_NID AND Visible_In_Catalog = ''True'' ORDER BY Name ASC END ELSE BEGIN SELECT NId,Name,Description,Ver_Name [DI_Version],Is_Desktop,Is_Online,Online_URL,Area_Count,IUS_Count, Time_Periods_Count,Data_Values_Count,Start_Year,End_Year,Last_Modified,Area_NId,Sub_Nation, Thumbnail_Image_URL,Db_Adm_Name,Db_Adm_Institution,Db_Adm_Email,Unicef_Region,Adaptation_Year,Db_Languages,LangCode_CSVFiles FROM ADAPTATIONS, ADAPTATIONVERSION WHERE( AREA_NID =''-1'' AND( IS_DI7_ORG_SITE <>''True'' OR IS_DI7_ORG_SITE IS NULL ))AND DI_VERSION = VER_NID AND Visible_In_Catalog = ''True'' ORDER BY Name ASC END END ELSE BEGIN SELECT NId,Name,Description,Ver_Name [DI_Version],Is_Desktop,Is_Online,Online_URL,Area_Count,IUS_Count, Time_Periods_Count,Data_Values_Count,Start_Year,End_Year,Last_Modified,Area_NId,Sub_Nation, Thumbnail_Image_URL,Db_Adm_Name,Db_Adm_Institution,Db_Adm_Email,Unicef_Region,Adaptation_Year,Db_Languages,LangCode_CSVFiles FROM ADAPTATIONS, ADAPTATIONVERSION WHERE (AREA_NID IN(@ADPT_AREA_NID)) AND DI_VERSION = VER_NID AND Visible_In_Catalog = ''True'' ORDER BY Name ASC END END' End ELSE BEGIN EXEC dbo.sp_executesql @statement =N'ALTER PROC [dbo].[SP_GET_CATALOG] @AdaptationGUID NVARCHAR(4000) AS BEGIN DECLARE @ADPT_AREA_NID INT DECLARE @IS_DI7_ORG_SITE_Val BIT SET @ADPT_AREA_NID = (SELECT AREA_NID FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID) SET @IS_DI7_ORG_SITE_Val=(SELECT IS_DI7_ORG_SITE FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID) IF @ADPT_AREA_NID = -1 BEGIN IF @IS_DI7_ORG_SITE_Val = ''True'' BEGIN SELECT NId,Name,Description,Ver_Name [DI_Version],Is_Desktop,Is_Online,Online_URL,Area_Count,IUS_Count, Time_Periods_Count,Data_Values_Count,Start_Year,End_Year,Last_Modified,Area_NId,Sub_Nation, Thumbnail_Image_URL,Db_Adm_Name,Db_Adm_Institution,Db_Adm_Email,Unicef_Region,Adaptation_Year,Db_Languages,LangCode_CSVFiles FROM ADAPTATIONS, ADAPTATIONVERSION WHERE (AREA_NID IN(@ADPT_AREA_NID) OR AREA_NID IN( SELECT AREANID FROM AREAS WHERE PARENTNID IN( SELECT AREANID FROM AREAS WHERE PARENTNID=-1) )) AND DI_VERSION = VER_NID AND Visible_In_Catalog = ''True'' ORDER BY Name ASC END ELSE BEGIN SELECT NId,Name,Description,Ver_Name [DI_Version],Is_Desktop,Is_Online,Online_URL,Area_Count,IUS_Count, Time_Periods_Count,Data_Values_Count,Start_Year,End_Year,Last_Modified,Area_NId,Sub_Nation, Thumbnail_Image_URL,Db_Adm_Name,Db_Adm_Institution,Db_Adm_Email,Unicef_Region,Adaptation_Year,Db_Languages,LangCode_CSVFiles FROM ADAPTATIONS, ADAPTATIONVERSION WHERE( AREA_NID =''-1'' AND( IS_DI7_ORG_SITE <>''True'' OR IS_DI7_ORG_SITE IS NULL ))AND DI_VERSION = VER_NID AND Visible_In_Catalog = ''True'' ORDER BY Name ASC END END ELSE BEGIN SELECT NId,Name,Description,Ver_Name [DI_Version],Is_Desktop,Is_Online,Online_URL,Area_Count,IUS_Count, Time_Periods_Count,Data_Values_Count,Start_Year,End_Year,Last_Modified,Area_NId,Sub_Nation, Thumbnail_Image_URL,Db_Adm_Name,Db_Adm_Institution,Db_Adm_Email,Unicef_Region,Adaptation_Year,Db_Languages,LangCode_CSVFiles FROM ADAPTATIONS, ADAPTATIONVERSION WHERE (AREA_NID IN(@ADPT_AREA_NID)) AND DI_VERSION = VER_NID AND Visible_In_Catalog = ''True'' ORDER BY Name ASC END END' END"
            SqlCmd.CommandText = Query
            SqlCmd.Connection = DbCon
            SqlCmd.Transaction = tran
            SqlCmd.ExecuteNonQuery() ' Execute query to alter procedure SP_GET_CATALOG
            LogMessage = String.Format(ReadKeysForPatch("ChangeProcForGuid").ToString(), "SP_GET_CATALOG")
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusSucess, String.Empty)
            '----------------------------------------------------------------

            'Query to alter procedure SP_GET_ADAPTATION_NID_AND_DELETE_INDEXED, for changes regarding guid
            Query = "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GET_ADAPTATION_NID_AND_DELETE_INDEXED]')) Begin EXEC dbo.sp_executesql @statement =N'CREATE PROC [dbo].[SP_GET_ADAPTATION_NID_AND_DELETE_INDEXED] @AdaptationGUID NVARCHAR(4000), @TABLE_TYPE NVARCHAR(20) AS BEGIN DECLARE @ADPT_NID INT SET @ADPT_NID = (SELECT NID FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID) IF(@TABLE_TYPE = ''AREA'') BEGIN DELETE FROM INDEXEDAREAS WHERE ADPT_NID = @ADPT_NID END IF(@TABLE_TYPE = ''INDICATOR'') BEGIN DELETE FROM INDEXEDINDICATORS WHERE ADPT_NID = @ADPT_NID END SELECT @ADPT_NID END' End ELSE BEGIN EXEC dbo.sp_executesql @statement =N'ALTER PROC [dbo].[SP_GET_ADAPTATION_NID_AND_DELETE_INDEXED] @AdaptationGUID NVARCHAR(4000), @TABLE_TYPE NVARCHAR(20) AS BEGIN DECLARE @ADPT_NID INT SET @ADPT_NID = (SELECT NID FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID) IF(@TABLE_TYPE = ''AREA'') BEGIN DELETE FROM INDEXEDAREAS WHERE ADPT_NID = @ADPT_NID END IF(@TABLE_TYPE = ''INDICATOR'') BEGIN DELETE FROM INDEXEDINDICATORS WHERE ADPT_NID = @ADPT_NID END SELECT @ADPT_NID END' END"
            SqlCmd.CommandText = Query
            SqlCmd.Connection = DbCon
            SqlCmd.Transaction = tran
            SqlCmd.ExecuteNonQuery() ' Execute query to alter procedure SP_GET_ADAPTATION_NID_AND_DELETE_INDEXED
            LogMessage = String.Format(ReadKeysForPatch("ChangeProcForGuid").ToString(), "SP_GET_ADAPTATION_NID_AND_DELETE_INDEXED")
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusSucess, String.Empty)
            '----------------------------------------------------------------
            'Query to alter procedure SP_CATALOG_EXISTS, for changes regarding guid
            Query = "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CATALOG_EXISTS]')) Begin EXEC dbo.sp_executesql @statement =N'CREATE PROC [dbo].[SP_CATALOG_EXISTS] @AdaptationGUID NVARCHAR(4000) AS BEGIN SELECT NId,Date_Created,Visible_in_Catalog FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID END' End ELSE BEGIN EXEC dbo.sp_executesql @statement =N'ALTER PROC [dbo].[SP_CATALOG_EXISTS] @AdaptationGUID NVARCHAR(4000) AS BEGIN SELECT NId,Date_Created,Visible_in_Catalog FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID END' END"
            SqlCmd.CommandText = Query
            SqlCmd.Connection = DbCon
            SqlCmd.Transaction = tran
            SqlCmd.ExecuteNonQuery() ' Execute query to alter procedure SP_CATALOG_EXISTS
            LogMessage = String.Format(ReadKeysForPatch("ChangeProcForGuid").ToString(), "SP_CATALOG_EXISTS")
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusSucess, String.Empty)
            '----------------------------------------------------------------

            'Query to alter procedure Set_User_AsAdmin, for changes regarding guid
            Query = "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Set_User_AsAdmin]')) Begin EXEC dbo.sp_executesql @statement =N'CREATE Procedure [dbo].[Set_User_AsAdmin] @User_NId INT, @AdaptationGUID as NVarchar(4000) AS BEGIN BEGIN TRY BEGIN TRANSACTION UPDATE ProviderDetails SET [User_Is_Provider] = ''False'', [User_Is_Admin] = ''False'' WHERE [AdaptationNid] in (SELECT NId FROM Adaptations where [GUID]=@AdaptationGUID) AND [User_Is_Admin] = ''True'' UPDATE ProviderDetails SET [User_Is_Provider] = ''True'', [User_Is_Admin] = ''True'' WHERE [AdaptationNid] in (SELECT NId FROM Adaptations where [GUID]=@AdaptationGUID) AND [UserNid] = @User_NId COMMIT END TRY BEGIN CATCH IF @@TRANCOUNT > 0 ROLLBACK END CATCH END' End ELSE BEGIN EXEC dbo.sp_executesql @statement =N'ALTER Procedure [dbo].[Set_User_AsAdmin] @User_NId INT, @AdaptationGUID as NVarchar(4000) AS BEGIN BEGIN TRY BEGIN TRANSACTION UPDATE ProviderDetails SET [User_Is_Provider] = ''False'', [User_Is_Admin] = ''False'' WHERE [AdaptationNid] in (SELECT NId FROM Adaptations where [GUID]=@AdaptationGUID) AND [User_Is_Admin] = ''True'' UPDATE ProviderDetails SET [User_Is_Provider] = ''True'', [User_Is_Admin] = ''True'' WHERE [AdaptationNid] in (SELECT NId FROM Adaptations where [GUID]=@AdaptationGUID) AND [UserNid] = @User_NId COMMIT END TRY BEGIN CATCH IF @@TRANCOUNT > 0 ROLLBACK END CATCH END' END"
            SqlCmd.CommandText = Query
            SqlCmd.Connection = DbCon
            SqlCmd.Transaction = tran
            SqlCmd.ExecuteNonQuery() ' Execute query to alter procedure Set_User_AsAdmin
            LogMessage = String.Format(ReadKeysForPatch("ChangeProcForGuid").ToString(), "Set_User_AsAdmin")
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusSucess, String.Empty)
            '----------------------------------------------------------------

            'Query to alter procedure Get_Users_ByAdaptationURL, for changes regarding guid
            Query = "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Get_Users_ByAdaptationURL]')) Begin EXEC dbo.sp_executesql @statement =  N'CREATE Procedure [dbo].[Get_Users_ByAdaptationURL] @AdaptationGUID as NVarchar(4000) AS BEGIN SELECT  U.[NId],U.[User_First_Name] AS ''UserName'', U.[User_Email_Id]  AS ''EmailId'', P.[User_Is_Provider] AS ''Is Provider'' ,P.[User_Is_Admin] AS ''Is Admin'',U.[IsMasterAccount] AS ''IsMasterAccount'' FROM ProviderDetails P JOIN Global_UserLogin U ON P.[UserNid] = U.[NId] JOIN Adaptations A ON P.[AdaptationNid] = A.[NId] WHERE A.[GUID] = @AdaptationGUID ORDER BY P.[User_Is_Admin] DESC END' End ELSE BEGIN EXEC dbo.sp_executesql @statement =N'ALTER Procedure [dbo].[Get_Users_ByAdaptationURL] @AdaptationGUID as NVarchar(4000) AS BEGIN SELECT U.[NId],U.[User_First_Name] AS  ''UserName'', U.[User_Email_Id] AS ''EmailId'', P.[User_Is_Provider] AS ''Is Provider'' ,P.[User_Is_Admin] AS ''Is Admin'',U.[IsMasterAccount] AS ''IsMasterAccount'' FROM ProviderDetails P JOIN Global_UserLogin U ON P.[UserNid] = U.[NId] JOIN Adaptations A ON P.[AdaptationNid] = A.[NId] WHERE A.[GUID] = @AdaptationGUID ORDER BY P.[User_Is_Admin] DESC END' END"
            SqlCmd.CommandText = Query
            SqlCmd.Connection = DbCon
            SqlCmd.Transaction = tran
            SqlCmd.ExecuteNonQuery() ' Execute query to alter procedure Get_Users_ByAdaptationURL
            LogMessage = String.Format(ReadKeysForPatch("ChangeProcForGuid").ToString(), "Get_Users_ByAdaptationURL")
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusSucess, String.Empty)
            '----------------------------------------------------------------

            ' If Guid column created and all the stored procedure altered successfully, comit transaction
            tran.Commit()
            RetVal = True

        Catch Ex As Exception
            ' Add entry in xls log file for patch, for faliure
            LogMessage = ReadKeysForPatch("GUIDProcChanges").ToString()
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusFailed, Ex.Message.ToString())
            ' if any error occured during 
            tran.Rollback()
            LogMessage = String.Format(ReadKeysForPatch("RollBack").ToString(), Ex.Message.ToString(), "7.0.0.4")
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusFailed, Ex.Message.ToString())
            RetVal = False
        Finally
            ' Dispose connection  Object
            DbCon.Close()
        End Try
        Return RetVal
    End Function

    Private Function InstallPatch7_0_0_5() As Boolean
        Dim RetVal As Boolean = True
        Return RetVal
    End Function

    Private Function InstallPatch7_0_0_6() As Boolean
        Dim RetVal As Boolean = True
        Return RetVal
    End Function

    ''' <summary>
    ''' Make all the necessary changes in database, that were done in patch 7.0.0.7
    ''' </summary>
    ''' <param name="DbConnectionstring">Database connection string</param>
    ''' <returns>Returns true, if patch installed successfully, else return false</returns>
    Private Function InstallPatch7_0_0_7(DbConnectionstring As String) As Boolean
        'The Catalog should display only those adaptations which have the same nature type.
        'Master Global will show all adaptations.        
        'Global sites will show only global adaptations.
        'National adaptions will show the adaptations with the same Nation and its sub-nations.
        Dim RetVal As Boolean = True
        Dim DbCon As SqlConnection = Nothing
        Dim SqlCmd As SqlCommand = Nothing
        Dim Query As String = String.Empty
        Dim tran As SqlTransaction = Nothing
        Dim LogMessage As String = String.Empty
        Dim PatchMessagesFile As String = String.Empty
        Try
            DbCon = New SqlConnection(DbConnectionstring)
            SqlCmd = New SqlCommand()
            DbCon.Open()
            ' Open database connection
            ' Begin Transaction
            tran = DbCon.BeginTransaction()
        Catch Ex As Exception
            RetVal = False
            LogMessage = String.Format(ReadKeysForPatch("OpenDatabaseConnection").ToString(), Ex.Message.ToString(), "7.0.0.7")
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusFailed, Ex.Message.ToString())
            Return RetVal
        End Try
        Try
            'Query to create procedure SP_GET_CATALOG if not exist, else alter
            Query = "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GET_CATALOG]') AND type in (N'P', N'PC')) BEGIN EXEC dbo.sp_executesql @statement = N' CREATE PROC [dbo].[SP_GET_CATALOG] @AdaptationGUID NVARCHAR(4000) AS BEGIN DECLARE @ADPT_AREA_NID INT DECLARE @IS_DI7_ORG_SITE_Val BIT SET @ADPT_AREA_NID = (SELECT AREA_NID FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID) SET @IS_DI7_ORG_SITE_Val=(SELECT IS_DI7_ORG_SITE FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID) IF @ADPT_AREA_NID = -1 BEGIN IF @IS_DI7_ORG_SITE_Val = ''True'' BEGIN SELECT NId,Name,Description,Ver_Name [DI_Version],Is_Desktop,Is_Online,Online_URL,Area_Count,IUS_Count, Time_Periods_Count,Data_Values_Count,Start_Year,End_Year,Last_Modified,Area_NId,Sub_Nation, Thumbnail_Image_URL,Db_Adm_Name,Db_Adm_Institution,Db_Adm_Email,Unicef_Region,Adaptation_Year,Db_Languages,LangCode_CSVFiles FROM ADAPTATIONS, ADAPTATIONVERSION WHERE (AREA_NID IN(@ADPT_AREA_NID) OR AREA_NID IN( SELECT AREANID FROM AREAS WHERE PARENTNID IN( SELECT AREANID FROM AREAS WHERE PARENTNID=-1) )) AND DI_VERSION = VER_NID AND Visible_In_Catalog = ''True'' ORDER BY Name ASC END ELSE BEGIN SELECT NId,Name,Description,Ver_Name [DI_Version],Is_Desktop,Is_Online,Online_URL,Area_Count,IUS_Count, Time_Periods_Count,Data_Values_Count,Start_Year,End_Year,Last_Modified,Area_NId,Sub_Nation, Thumbnail_Image_URL,Db_Adm_Name,Db_Adm_Institution,Db_Adm_Email,Unicef_Region,Adaptation_Year,Db_Languages,LangCode_CSVFiles FROM ADAPTATIONS, ADAPTATIONVERSION WHERE( AREA_NID =''-1'' AND( IS_DI7_ORG_SITE <>''True'' OR IS_DI7_ORG_SITE IS NULL ))AND DI_VERSION = VER_NID AND Visible_In_Catalog = ''True'' ORDER BY Name ASC END END ELSE BEGIN SELECT NId,Name,Description,Ver_Name [DI_Version],Is_Desktop,Is_Online,Online_URL,Area_Count,IUS_Count, Time_Periods_Count,Data_Values_Count,Start_Year,End_Year,Last_Modified,Area_NId,Sub_Nation, Thumbnail_Image_URL,Db_Adm_Name,Db_Adm_Institution,Db_Adm_Email,Unicef_Region,Adaptation_Year,Db_Languages,LangCode_CSVFiles FROM ADAPTATIONS, ADAPTATIONVERSION WHERE (AREA_NID IN(@ADPT_AREA_NID)) AND DI_VERSION = VER_NID AND Visible_In_Catalog = ''True'' ORDER BY Name ASC END END ' END ELSE BEGIN EXEC dbo.sp_executesql @statement = N' ALTER PROC [dbo].[SP_GET_CATALOG] @AdaptationGUID NVARCHAR(4000) AS BEGIN DECLARE @ADPT_AREA_NID INT DECLARE @IS_DI7_ORG_SITE_Val BIT SET @ADPT_AREA_NID = (SELECT AREA_NID FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID) SET @IS_DI7_ORG_SITE_Val=(SELECT IS_DI7_ORG_SITE FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID) IF @ADPT_AREA_NID = -1 BEGIN IF @IS_DI7_ORG_SITE_Val = ''True'' BEGIN SELECT NId,Name,Description,Ver_Name [DI_Version],Is_Desktop,Is_Online,Online_URL,Area_Count,IUS_Count, Time_Periods_Count,Data_Values_Count,Start_Year,End_Year,Last_Modified,Area_NId,Sub_Nation, Thumbnail_Image_URL,Db_Adm_Name,Db_Adm_Institution,Db_Adm_Email,Unicef_Region,Adaptation_Year,Db_Languages,LangCode_CSVFiles FROM ADAPTATIONS, ADAPTATIONVERSION WHERE (AREA_NID IN(@ADPT_AREA_NID) OR AREA_NID IN( SELECT AREANID FROM AREAS WHERE PARENTNID IN( SELECT AREANID FROM AREAS WHERE PARENTNID=-1) )) AND DI_VERSION = VER_NID AND Visible_In_Catalog = ''True'' ORDER BY Name ASC END ELSE BEGIN SELECT NId,Name,Description,Ver_Name [DI_Version],Is_Desktop,Is_Online,Online_URL,Area_Count,IUS_Count, Time_Periods_Count,Data_Values_Count,Start_Year,End_Year,Last_Modified,Area_NId,Sub_Nation, Thumbnail_Image_URL,Db_Adm_Name,Db_Adm_Institution,Db_Adm_Email,Unicef_Region,Adaptation_Year,Db_Languages,LangCode_CSVFiles FROM ADAPTATIONS, ADAPTATIONVERSION WHERE( AREA_NID =''-1'' AND( IS_DI7_ORG_SITE <>''True'' OR IS_DI7_ORG_SITE IS NULL ))AND DI_VERSION = VER_NID AND Visible_In_Catalog = ''True'' ORDER BY Name ASC END END ELSE BEGIN SELECT NId,Name,Description,Ver_Name [DI_Version],Is_Desktop,Is_Online,Online_URL,Area_Count,IUS_Count, Time_Periods_Count,Data_Values_Count,Start_Year,End_Year,Last_Modified,Area_NId,Sub_Nation, Thumbnail_Image_URL,Db_Adm_Name,Db_Adm_Institution,Db_Adm_Email,Unicef_Region,Adaptation_Year,Db_Languages,LangCode_CSVFiles FROM ADAPTATIONS, ADAPTATIONVERSION WHERE (AREA_NID IN(@ADPT_AREA_NID)) AND DI_VERSION = VER_NID AND Visible_In_Catalog = ''True'' ORDER BY Name ASC END END ' END"

            SqlCmd.CommandText = Query
            SqlCmd.Connection = DbCon
            SqlCmd.Transaction = tran
            SqlCmd.ExecuteNonQuery() ' Execute query to alter procedure SP_GET_CATALOG 

            ' If Guid column created and all the stored procedure altered successfully, comit transaction
            tran.Commit()
            RetVal = True
            ' Add entry in xls log file for patch, for Success
            LogMessage = ReadKeysForPatch("GetCatalog").ToString()
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusSucess, String.Empty)

        Catch Ex As Exception
            ' Add entry in xls log file for patch, for falure
            LogMessage = ReadKeysForPatch("GetCatalog").ToString()
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusSucess, Ex.Message.ToString())
            ' if any error occured during transaction
            tran.Rollback()
            LogMessage = String.Format(ReadKeysForPatch("RollBack").ToString(), Ex.Message.ToString(), "7.0.0.7")
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusFailed, Ex.Message.ToString())
            RetVal = False
        End Try
        Return RetVal
    End Function

    ''' <summary>
    ''' Make all the necessary changes in database, that were done in patch 7.0.0.8
    ''' </summary>
    ''' <returns>Returns true, if patch installed successfully, else return false</returns>
    Private Function InstallPatch7_0_0_8() As Boolean
        Dim RetVal As Boolean = True
        Return RetVal
    End Function

    ''' <summary>
    ''' Make all the necessary changes in database, that were done in patch 7.0.0.9
    ''' </summary>
    ''' <param name="DbConnectionstring">Database connection string</param>
    ''' <returns>Returns true, if patch installed successfully, else return false</returns>
    Private Function InstallPatch7_0_0_9(DbConnectionstring As String) As Boolean

        Dim RetVal As Boolean = True
        Dim DbCon As SqlConnection = Nothing
        Dim SqlCmd As SqlCommand = Nothing
        Dim Query As String = String.Empty
        Dim tran As SqlTransaction = Nothing
        Dim LogMessage As String = String.Empty
        Dim PatchMessagesFile As String = String.Empty
        Try
            DbCon = New SqlConnection(DbConnectionstring)
            SqlCmd = New SqlCommand()
            DbCon.Open()
            ' Open database connection
            ' Begin Transaction
            tran = DbCon.BeginTransaction()
        Catch Ex As Exception
            RetVal = False
            LogMessage = String.Format(ReadKeysForPatch("OpenDatabaseConnection").ToString(), Ex.Message.ToString(), "7.0.0.9")
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusFailed, Ex.Message.ToString())
            Return RetVal
        End Try
        Try
            'Query to add colume SortingOrder, if not exist, in table Areas
            Query = "IF NOT EXISTS(SELECT * FROM sys.columns where Name = N'SortingOrder' and Object_ID = Object_ID(N'Areas'))Begin ALTER TABLE Areas ADD SortingOrder int;EXEC ('UPDATE  Areas SET SortingOrder=1 WHERE AreaName=''Global'' ;UPDATE  Areas SET SortingOrder=2 WHERE AreaName=''Regional'';UPDATE  Areas SET SortingOrder=11 WHERE AreaName !=''Global'' AND AreaName!=''Regional'' ;');END"
            SqlCmd.CommandText = Query
            SqlCmd.Connection = DbCon
            SqlCmd.Transaction = tran
            SqlCmd.ExecuteNonQuery() ' Execute query 

            'Query to create procedure SP_GET_ADAPTATION_AREAS if not exist, else alter
            Query = "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GET_ADAPTATION_AREAS]') AND type in (N'P', N'PC')) BEGIN EXEC dbo.sp_executesql @statement = N' CREATE PROC [dbo].[SP_GET_ADAPTATION_AREAS] @AdaptationGUID NVARCHAR(4000) AS BEGIN DECLARE @ADPT_AREA_NID INT SET @ADPT_AREA_NID = (SELECT AREA_NID FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID) IF @ADPT_AREA_NID = -1 BEGIN SELECT * FROM ( SELECT TOP 10000 AREANID, AREANAME, AREAID, PARENTNID,SortingOrder FROM AREAS WHERE PARENTNID = -1 ORDER BY AREANAME) A UNION ALL SELECT * FROM ( SELECT TOP 10000 AREANID, AREANAME, AREAID, PARENTNID,SortingOrder FROM AREAS WHERE AREALEVEL = 2 AND AREANID IN (SELECT DISTINCT AREA_NID FROM ADAPTATIONS WHERE AREA_NID > 0) ORDER BY SortingOrder, AREANAME ) B END ELSE BEGIN SELECT AREANID, AREANAME, AREAID, PARENTNID,SortingOrder FROM AREAS WHERE AREANID = @ADPT_AREA_NID OR PARENTNID = @ADPT_AREA_NID ORDER BY SortingOrder,AREANAME END END ' END ELSE BEGIN EXEC dbo.sp_executesql @statement = N' ALTER PROC [dbo].[SP_GET_ADAPTATION_AREAS] @AdaptationGUID NVARCHAR(4000) AS BEGIN DECLARE @ADPT_AREA_NID INT SET @ADPT_AREA_NID = (SELECT AREA_NID FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID) IF @ADPT_AREA_NID = -1 BEGIN SELECT * FROM ( SELECT TOP 10000 AREANID, AREANAME, AREAID, PARENTNID,SortingOrder FROM AREAS WHERE PARENTNID = -1 ORDER BY AREANAME) A UNION ALL SELECT * FROM ( SELECT TOP 10000 AREANID, AREANAME, AREAID, PARENTNID,SortingOrder FROM AREAS WHERE AREALEVEL = 2 AND AREANID IN (SELECT DISTINCT AREA_NID FROM ADAPTATIONS WHERE AREA_NID > 0) ORDER BY SortingOrder, AREANAME ) B END ELSE BEGIN SELECT AREANID, AREANAME, AREAID, PARENTNID,SortingOrder FROM AREAS WHERE AREANID = @ADPT_AREA_NID OR PARENTNID = @ADPT_AREA_NID ORDER BY SortingOrder,AREANAME END END ' END"
            SqlCmd.CommandText = Query
            SqlCmd.Connection = DbCon
            SqlCmd.Transaction = tran
            SqlCmd.ExecuteNonQuery() ' Execute query to alter procedure SP_GET_ADAPTATION_AREAS 

            ' If stored procedure SP_GET_ADAPTATION_AREAS altered successfully, commit transaction
            tran.Commit()
            RetVal = True
            ' Add entry in xls log file for patch, for Success
            LogMessage = ReadKeysForPatch("GetAdaptationAreas").ToString()
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusSucess, String.Empty)

        Catch Ex As Exception
            ' Add entry in xls log file for patch, for falure
            LogMessage = ReadKeysForPatch("GetAdaptationAreas").ToString()
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusSucess, Ex.Message.ToString())
            ' if any error occured during transaction
            tran.Rollback()
            tran.Dispose()
            LogMessage = String.Format(ReadKeysForPatch("RollBack").ToString(), Ex.Message.ToString(), "7.0.0.9")
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusFailed, Ex.Message.ToString())
            RetVal = False

        End Try
        Return RetVal
    End Function


    Private Function CatalogMoveRegionaltoAfterGlobal() As String
        Dim RetVal As String
        Return RetVal
    End Function

#End Region



#Region "--Common Methods"
    ''' <summary>
    ''' Get value of keys by key name and file path
    ''' </summary>
    ''' <param name="keyName">Name of key, for which value is to retrieve</param>
    ''' <returns></returns>
    Public Shared Function ReadKeysForPatch(keyName As String) As String
        Dim RetVal As String = String.Empty
        Dim LanguageFileWithPath As String = String.Empty
        Dim PatchMessagesFile As String = String.Empty

        Dim XmlDoc As XmlDocument

        Try
            ' Path of file containing messages for patch installation
            PatchMessagesFile = PatchConstaints.PatchInstalMsgFile
            LanguageFileWithPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, PatchConstaints.PatchFolderPath, PatchMessagesFile)
            XmlDoc = New XmlDocument()
            XmlDoc.Load(LanguageFileWithPath)

            RetVal = XmlDoc.SelectSingleNode((("/" + PatchConstaints.Xml_Tags_Root & "/") + PatchConstaints.Xml_Tags_Row & "[@") + PatchConstaints.Xml_Tags_Key + "='" & keyName & "']").Attributes(PatchConstaints.Xml_Tags_Value).Value
        Catch ex As Exception
            RetVal = String.Empty
        End Try

        Return RetVal
    End Function

#End Region






#End Region
End Class



Public Class PatchConstaints
    Public Const PatchInstLogFileName = "DI7WS_PatchUpdate_LogFile"
    Public Const CSVLogPath = "Stock\\CSVLogs"
    Public Const Script_GetCatlog_Sorted = "Script_GetCatlog_Sorted.sql"
    Public Const Script_GetCatlog_ShowSpecific = "Script_GetCatlog_ShowSpecific.sql"
    Public Const PatchFolderPath = "Stock\\Patch"
    Public Const Script_GUID_Changes = "Script_GUID_Changes.sql"
    Public Const Script_GetAdapt_Areas = "Script_GetAdapt_Areas.sql"

    Public Const PatchInstalMsgFile = "PatchInstalMessages.xml"


    Public Const Xml_Tags_Root = "root"
    Public Const Xml_Tags_Row = "Row"
    Public Const Xml_Tags_Key = "key"
    Public Const Xml_Tags_Value = "value"


End Class
