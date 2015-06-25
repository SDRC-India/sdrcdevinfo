Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data
Imports System.IO
Imports System.Xml
Imports DALConnection = DevInfo.Lib.DI_LibDAL.Connection

<WebService(Namespace:="http://DIworldwide/DIWWS/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class Catalog
    Inherits System.Web.Services.WebService

#Region "-- Private --"

#Region "-- Variables --"
    Private JsonAdaptationsTxtFileName As String = "jsonAdaptations.txt"
    Private JsonCountriesTxtFileName As String = "jsonM49Countries.txt"
#End Region

#Region "-- Method --"

#Region "-- Catalog --"

    Private Function GetDbConnection() As DALConnection.DIConnection

        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim ConnectionStr As String = String.Empty
        Dim ConnDetailsArr(5) As String
        Dim ServerName As String = String.Empty
        Dim DatabaseName As String = String.Empty
        Dim UserName As String = String.Empty
        Dim Password As String = String.Empty

        Try
            ConnectionStr = ConfigurationManager.AppSettings("DBConnString")

            ConnDetailsArr = SplitString(ConnectionStr, ";")

            ServerName = ConnDetailsArr(0).Substring(ConnDetailsArr(0).IndexOf("=") + 1)
            DatabaseName = ConnDetailsArr(1).Substring(ConnDetailsArr(1).IndexOf("=") + 1)
            UserName = ConnDetailsArr(2).Substring(ConnDetailsArr(2).IndexOf("=") + 1)
            Password = ConnDetailsArr(3).Substring(ConnDetailsArr(3).IndexOf("=") + 1)

            ObjDIConnection = New DALConnection.DIConnection(DALConnection.DIServerType.SqlServer, ServerName, "", DatabaseName, UserName, Password)

        Catch ex As Exception
        End Try

        Return ObjDIConnection

    End Function

    Private Function GetJSONString(ByVal Dt As DataTable) As String
        Dim RetVal As String = String.Empty
        Dim StrDc As String() = New String(Dt.Columns.Count - 1) {}
        Dim HeadStr As String = String.Empty
        Dim Sb As New StringBuilder()
        Dim TempStr As String = String.Empty
        Dim i As Integer
        Dim j As Integer

        Try
            For i = 0 To Dt.Columns.Count - 1
                StrDc(i) = Dt.Columns(i).Caption
                HeadStr += """" & StrDc(i) & """ : """ & StrDc(i) & i.ToString() & "¾" & ""","
            Next

            HeadStr = HeadStr.Substring(0, HeadStr.Length - 1)

            Sb.Append("{""" & Convert.ToString(Dt.TableName) & """ : [")

            For i = 0 To Dt.Rows.Count - 1
                TempStr = HeadStr
                Sb.Append("{")

                For j = 0 To Dt.Columns.Count - 1
                    TempStr = TempStr.Replace(Dt.Columns(j).ToString() + j.ToString() + "¾", Dt.Rows(i)(j).ToString())
                Next

                Sb.Append(TempStr & "},")
            Next

            Sb = New StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1))
            Sb.Append("]}")

            RetVal = Sb.ToString()
        Catch
        End Try

        Return RetVal
    End Function

    Private Function SplitString(ByVal valueString As String, ByVal delimiter As String) As String()
        Dim RetVal As String()
        Dim Index As Integer = 0
        Dim Value As String
        Dim SplittedList As New List(Of String)()

        While True
            Index = valueString.IndexOf(delimiter)
            If Index = -1 Then
                If Not String.IsNullOrEmpty(valueString) Then
                    SplittedList.Add(valueString)
                End If
                Exit While
            Else
                Value = valueString.Substring(0, Index)
                valueString = valueString.Substring(Index + delimiter.Length)

                SplittedList.Add(Value)

            End If
        End While

        RetVal = SplittedList.ToArray()

        Return RetVal
    End Function

    Private Sub CreateFile(ByVal fileNameWithPath As String, ByVal data As String)
        Dim SW As StreamWriter

        Try
            If File.Exists(fileNameWithPath) Then
                File.Delete(fileNameWithPath)
            End If

            SW = New StreamWriter(fileNameWithPath)
            SW.WriteLine(data)
            SW.Close()
        Catch ex As Exception
        End Try
    End Sub

    Private Function GetAdaptationAreas(ByVal gUID As String)

        Dim RetVal As String = String.Empty
        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DbParams As New List(Of System.Data.Common.DbParameter)()
        Dim DtCatalog As DataTable = Nothing

        Try
            ObjDIConnection = GetDbConnection()

            If Not IsNothing(ObjDIConnection) Then
                Dim Param1 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param1.ParameterName = "AdaptationGUID"
                Param1.DbType = DbType.[String]
                Param1.Value = gUID
                DbParams.Add(Param1)

                DtCatalog = ObjDIConnection.ExecuteDataTable("[SP_GET_ADAPTATION_AREAS]", CommandType.StoredProcedure, DbParams)
                DtCatalog.TableName = "Areas"

                RetVal = GetJSONString(DtCatalog)
            End If

        Catch ex As Exception
        Finally
            If Not IsNothing(ObjDIConnection) Then
                ObjDIConnection.Dispose()
            End If
        End Try

        Return RetVal

    End Function

    Private Function GetAdaptationsJsonData(ByVal gUID As String) As String
        Dim RetVal As String = String.Empty
        Dim AreaJsonData As String = String.Empty
        Dim AdapdationJsonData As String = String.Empty

        Try
            AreaJsonData = GetAdaptationAreas(gUID)
            AreaJsonData = AreaJsonData.Remove(AreaJsonData.Length - 1, 1)

            AdapdationJsonData = GetCatalog(gUID)
            AdapdationJsonData = AdapdationJsonData.Remove(0, 1)

            RetVal = AreaJsonData + " , " + AdapdationJsonData
        Catch ex As Exception
        End Try

        Return RetVal

    End Function

    'Private Sub WriteAdaptationsJsonDataIntoTxtFile()
    '    Dim JsonFileNameWithPath As String = String.Empty

    '    Dim JsonData As String = String.Empty

    '    Try
    '        JsonFileNameWithPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock\") + JsonAdaptationsTxtFileName

    '        JsonData = GetAdaptationsJsonData()

    '        CreateFile(JsonFileNameWithPath, JsonData)

    '    Catch ex As Exception
    '    End Try

    'End Sub

    Private Sub WriteM49CountriesJsonDataIntoTxtFile()
        Dim JsonFileNameWithPath As String = String.Empty

        Dim JsonData As String = String.Empty

        Try
            JsonFileNameWithPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock\") + JsonCountriesTxtFileName

            JsonData = GetM49Countries()

            CreateFile(JsonFileNameWithPath, JsonData)

        Catch ex As Exception
        End Try

    End Sub



    Private Function GetMatchedAreas(ByVal SearchedAreas As String, ByVal ObjDIConnection As DALConnection.DIConnection, ByVal langCodeDb As String) As DataTable

        Dim Results As DataTable = New DataTable()

        Dim DbParams As New List(Of System.Data.Common.DbParameter)()

        Try

            If Not IsNothing(ObjDIConnection) Then
                Dim ParamAreas As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                ParamAreas.ParameterName = "Search_Areas"
                ParamAreas.DbType = DbType.[String]
                ParamAreas.Value = SearchedAreas
                DbParams.Add(ParamAreas)

                Dim ParamLangCodeDb As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                ParamLangCodeDb.ParameterName = "LANGUAGE_CODE"
                ParamLangCodeDb.DbType = DbType.[String]
                ParamLangCodeDb.Value = langCodeDb
                DbParams.Add(ParamLangCodeDb)

                Results = ObjDIConnection.ExecuteDataTable("SP_GET_MATCHED_INDEXED_AREAS", CommandType.StoredProcedure, DbParams)
                Results.TableName = "MatchedAreas"

            End If
        Catch ex As Exception
        Finally
        End Try

        Return Results

    End Function

    Private Function GetMatchedIndicators(ByVal SearchedIndicators As String, ByVal ObjDIConnection As DALConnection.DIConnection, ByVal langCodeDb As String) As DataTable

        Dim Results As DataTable = New DataTable()

        Dim DbParams As New List(Of System.Data.Common.DbParameter)()

        Try

            If Not IsNothing(ObjDIConnection) Then
                Dim ParamAreas As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                ParamAreas.ParameterName = "Search_Indicators"
                ParamAreas.DbType = DbType.[String]
                ParamAreas.Value = SearchedIndicators
                DbParams.Add(ParamAreas)

                Dim ParamLangCodeDb As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                ParamLangCodeDb.ParameterName = "LANGUAGE_CODE"
                ParamLangCodeDb.DbType = DbType.[String]
                ParamLangCodeDb.Value = langCodeDb
                DbParams.Add(ParamLangCodeDb)

                Results = ObjDIConnection.ExecuteDataTable("SP_GET_MATCHED_INDEXED_INDICATORS", CommandType.StoredProcedure, DbParams)
                Results.TableName = "MatchedIndicators"

            End If
        Catch ex As Exception
        Finally
        End Try

        Return Results

    End Function

    Private Function getDistinctColValues(ByVal dtSource As DataTable, ByVal ColumnName As String) As List(Of String)

        Dim Result As List(Of String) = New List(Of String)

        For Each dr As DataRow In dtSource.Rows
            Dim nowVal As String = dr(ColumnName)

            If Not Result.Contains(nowVal) Then
                Result.Add(nowVal)
            End If
        Next

        Return Result

    End Function

    Private Function getColValuesOfAdaptation(ByVal dtSource As DataTable, ByVal ColumnName As String, ByVal AdaptationNId As String) As List(Of String)

        Dim Result As List(Of String) = New List(Of String)

        For Each dr As DataRow In dtSource.Rows
            Dim nowVal As String = dr(ColumnName)
            Dim nowAdaptationNId As String = dr("Adpt_NId")

            If (Not Result.Contains(nowVal)) And nowAdaptationNId = AdaptationNId Then
                Result.Add(nowVal)
            End If
        Next

        Return Result

    End Function

    Private Function GetGlobalMasterUrlRecord(ByVal param As String) As DataTable

        Dim RetVal As DataTable = Nothing
        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DbParams As New List(Of System.Data.Common.DbParameter)()
        Try
            ObjDIConnection = GetDbConnection()
            If Not IsNothing(ObjDIConnection) Then
                Dim Query As String = String.Empty
                Query = "SELECT * FROM Adaptations"
                If param <> String.Empty Then
                    Query = Query + " WHERE GUID='" + param + "'"
                End If
                RetVal = ObjDIConnection.ExecuteDataTable(Query, CommandType.Text, DbParams) ' [GetMasterSiteURL]
            End If
        Catch ex As Exception
        Finally
            If Not IsNothing(ObjDIConnection) Then
                ObjDIConnection.Dispose()
            End If
        End Try
        Return RetVal
    End Function

#End Region

#Region "-- App Settings --"

    Private Function GetNodeValue(ByVal xmlDoc As XmlDocument, ByVal keyName As String) As String
        Dim RetVal As String = String.Empty

        Try
            RetVal = xmlDoc.SelectSingleNode((("/" + Constants.XmlFile.AppSettings.Tags.Root & "/") + Constants.XmlFile.AppSettings.Tags.Item & "[@") + Constants.XmlFile.AppSettings.Tags.ItemAttributes.Name & "='" & keyName & "']").Attributes(Constants.XmlFile.AppSettings.Tags.ItemAttributes.Value).Value

        Catch ex As Exception
        End Try

        Return RetVal
    End Function

#End Region

#End Region

#End Region

#Region "-- Public --"

#Region "-- Method --"

#Region "-- Catalog --"

    <WebMethod()> _
    Public Function GetM49Countries() As String
        Dim RetVal As String = String.Empty
        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DtAreas As DataTable = Nothing

        Try
            ObjDIConnection = GetDbConnection()

            If Not IsNothing(ObjDIConnection) Then
                DtAreas = ObjDIConnection.ExecuteDataTable("SP_GET_M49_COUNTRIES", CommandType.StoredProcedure, Nothing)
                DtAreas.TableName = "Areas"
                RetVal = GetJSONString(DtAreas)
            End If

        Catch ex As Exception
        Finally
            If Not IsNothing(ObjDIConnection) Then
                ObjDIConnection.Dispose()
            End If
        End Try

        Return RetVal
    End Function

    <WebMethod()> _
    Public Function SetCatalog(ByVal adaptationName As String, ByVal description As String, ByVal version As String, ByVal isDesktop As Boolean, ByVal isWeb As Boolean, ByVal webURL As String, ByVal areaCount As Integer, ByVal iusCount As Integer, ByVal timePeriodsCount As Integer, ByVal dataValuesCount As Integer, ByVal startYear As String, ByVal endYear As String, ByVal lastModifiedOn As String, ByVal areaNId As Integer, ByVal subNation As String, ByVal catalogImageURL As String, ByVal dbAdmName As String, ByVal dbAdmInstitution As String, ByVal dbAdmEmail As String, ByVal unicefRegion As String, ByVal adaptationYear As String, ByVal dbLanguages As String, ByVal langCode_CSVFiles As String, ByVal Adapted_For As String, ByVal Country As String, ByVal Date_Created As String, ByVal gUID As String) As Boolean

        Dim RetVal As Boolean = False
        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DbParams As New List(Of System.Data.Common.DbParameter)()
        Dim DtCatalog As DataTable = Nothing

        Try
            ObjDIConnection = GetDbConnection()

            If Not IsNothing(ObjDIConnection) Then

                Dim Param2 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param2.ParameterName = "NAME"
                Param2.DbType = DbType.[String]
                Param2.Value = adaptationName
                DbParams.Add(Param2)

                Dim Param3 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param3.ParameterName = "DESCRIPTION"
                Param3.DbType = DbType.[String]
                Param3.Value = description
                DbParams.Add(Param3)

                Dim Param4 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param4.ParameterName = "DI_VERSION"
                Param4.DbType = DbType.[String]
                Param4.Value = version
                DbParams.Add(Param4)

                Dim Param5 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param5.ParameterName = "IS_DESKTOP"
                Param5.DbType = DbType.[Boolean]
                Param5.Value = isDesktop
                DbParams.Add(Param5)

                Dim Param6 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param6.ParameterName = "IS_ONLINE"
                Param6.DbType = DbType.[Boolean]
                Param6.Value = isWeb
                DbParams.Add(Param6)

                Dim Param7 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param7.ParameterName = "ONLINE_URL"
                Param7.DbType = DbType.[String]
                Param7.Value = webURL
                DbParams.Add(Param7)

                Dim Param8 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param8.ParameterName = "AREA_COUNT"
                Param8.DbType = DbType.[Int32]
                Param8.Value = areaCount
                DbParams.Add(Param8)

                Dim Param9 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param9.ParameterName = "IUS_COUNT"
                Param9.DbType = DbType.[Int32]
                Param9.Value = iusCount
                DbParams.Add(Param9)

                Dim Param10 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param10.ParameterName = "TIME_PERIODS_COUNT"
                Param10.DbType = DbType.[Int32]
                Param10.Value = timePeriodsCount
                DbParams.Add(Param10)

                Dim Param11 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param11.ParameterName = "DATA_VALUES_COUNT"
                Param11.DbType = DbType.[Int32]
                Param11.Value = dataValuesCount
                DbParams.Add(Param11)

                Dim Param12 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param12.ParameterName = "START_YEAR"
                Param12.DbType = DbType.[String]
                Param12.Value = startYear
                DbParams.Add(Param12)

                Dim Param13 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param13.ParameterName = "END_YEAR"
                Param13.DbType = DbType.[String]
                Param13.Value = endYear
                DbParams.Add(Param13)

                Dim Param14 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param14.ParameterName = "LAST_MODIFIED"
                Param14.DbType = DbType.[String]
                Param14.Value = lastModifiedOn
                DbParams.Add(Param14)

                Dim Param15 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param15.ParameterName = "AREA_NID"
                Param15.DbType = DbType.[Int32]
                Param15.Value = areaNId
                DbParams.Add(Param15)

                Dim Param16 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param16.ParameterName = "SUB_NATION"
                Param16.DbType = DbType.[String]
                Param16.Value = subNation
                DbParams.Add(Param16)

                Dim Param17 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param17.ParameterName = "THUMBNAIL_IMAGE_URL"
                Param17.DbType = DbType.[String]
                Param17.Value = catalogImageURL
                DbParams.Add(Param17)

                Dim Param18 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param18.ParameterName = "DB_ADM_NAME"
                Param18.DbType = DbType.[String]
                Param18.Value = dbAdmName
                DbParams.Add(Param18)

                Dim Param19 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param19.ParameterName = "DB_ADM_INSTITUTION"
                Param19.DbType = DbType.[String]
                Param19.Value = dbAdmInstitution
                DbParams.Add(Param19)

                Dim Param20 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param20.ParameterName = "DB_ADM_EMAIL"
                Param20.DbType = DbType.[String]
                Param20.Value = dbAdmEmail
                DbParams.Add(Param20)

                Dim Param21 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param21.ParameterName = "UNICEF_REGION"
                Param21.DbType = DbType.[String]
                Param21.Value = unicefRegion
                DbParams.Add(Param21)

                Dim Param22 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param22.ParameterName = "ADAPTATION_YEAR"
                Param22.DbType = DbType.[String]
                Param22.Value = adaptationYear
                DbParams.Add(Param22)

                Dim Param23 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param23.ParameterName = "DB_LANGUAGES"
                Param23.DbType = DbType.[String]
                Param23.Value = dbLanguages
                DbParams.Add(Param23)

                Dim Param24 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param24.ParameterName = "LangCode_CSVFiles"
                Param24.DbType = DbType.[String]
                Param24.Value = langCode_CSVFiles
                DbParams.Add(Param24)

                Dim Param25 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param25.ParameterName = "Adapted_For"
                Param25.DbType = DbType.[String]
                Param25.Value = Adapted_For
                DbParams.Add(Param25)

                Dim Param26 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param26.ParameterName = "Country"
                Param26.DbType = DbType.[String]
                Param26.Value = Country
                DbParams.Add(Param26)

                Dim Param27 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param27.ParameterName = "AdaptationGUID"
                Param27.DbType = DbType.[String]
                Param27.Value = gUID
                DbParams.Add(Param27)

                Dim Param28 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param28.ParameterName = "Date_Created"
                Param28.DbType = DbType.[String]
                Param28.Value = Date_Created
                DbParams.Add(Param28)



                DtCatalog = ObjDIConnection.ExecuteDataTable("SP_INSERT_CATALOG", CommandType.StoredProcedure, DbParams)

                'WriteAdaptationsJsonDataIntoTxtFile()

                RetVal = True

            End If
        Catch ex As Exception
        Finally
            If Not IsNothing(ObjDIConnection) Then
                ObjDIConnection.Dispose()
            End If
        End Try

        Return RetVal
    End Function

    <WebMethod()> _
    Public Function GetCatalog(ByVal gUID As String) As String
        Dim RetVal As String = String.Empty
        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DbParams As New List(Of System.Data.Common.DbParameter)()
        Dim DtCatalog As DataTable = Nothing

        Try
            ObjDIConnection = GetDbConnection()

            If Not IsNothing(ObjDIConnection) Then
                Dim Param1 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param1.ParameterName = "AdaptationGUID"
                Param1.DbType = DbType.[String]
                Param1.Value = gUID
                DbParams.Add(Param1)

                DtCatalog = ObjDIConnection.ExecuteDataTable("SP_GET_CATALOG", CommandType.StoredProcedure, DbParams)
                DtCatalog.TableName = "Adaptations"

                RetVal = GetJSONString(DtCatalog)
            End If

        Catch ex As Exception
        Finally
            If Not IsNothing(ObjDIConnection) Then
                ObjDIConnection.Dispose()
            End If
        End Try

        Return RetVal
    End Function
    <WebMethod()> _
    Public Function CatalogExists(ByVal gUID As String) As DataSet
        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DbParams As New List(Of System.Data.Common.DbParameter)()
        Dim DtCatalog As DataTable = Nothing
        Dim RetVal As New DataSet()
        Try
            ObjDIConnection = GetDbConnection()

            If Not IsNothing(ObjDIConnection) Then
                Dim Param1 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param1.ParameterName = "AdaptationGUID"
                Param1.DbType = DbType.[String]
                Param1.Value = gUID
                DbParams.Add(Param1)

                DtCatalog = ObjDIConnection.ExecuteDataTable("SP_CATALOG_EXISTS", CommandType.StoredProcedure, DbParams)
                RetVal.Tables.Add(DtCatalog)
                Return RetVal
            End If

        Catch ex As Exception
        Finally
            If Not IsNothing(ObjDIConnection) Then
                ObjDIConnection.Dispose()
            End If
        End Try

        Return RetVal
    End Function


    <WebMethod()> _
    Public Function DeleteCatalog(ByVal adaptationNid As Integer) As Boolean
        Dim RetVal As Boolean = False
        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DbParams As New List(Of System.Data.Common.DbParameter)()

        Try
            ObjDIConnection = GetDbConnection()

            If Not IsNothing(ObjDIConnection) Then
                Dim Param1 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param1.ParameterName = "ADAPTATION_NID"
                Param1.DbType = DbType.Int32
                Param1.Value = adaptationNid
                DbParams.Add(Param1)

                ObjDIConnection.ExecuteDataTable("SP_DELETE_CATALOG", CommandType.StoredProcedure, DbParams)

                RetVal = True
            End If

        Catch ex As Exception
        Finally
            If Not IsNothing(ObjDIConnection) Then
                ObjDIConnection.Dispose()
            End If
        End Try

        Return RetVal
    End Function

    <WebMethod()> _
    Public Function UpdateCatalog(ByVal adaptationNid As Integer, ByVal adaptationName As String, ByVal description As String, ByVal version As String, ByVal isDesktop As Boolean, ByVal isWeb As Boolean, ByVal webURL As String, ByVal areaCount As Integer, ByVal iusCount As Integer, ByVal timePeriodsCount As Integer, ByVal dataValuesCount As Integer, ByVal startYear As String, ByVal endYear As String, ByVal lastModifiedOn As String, ByVal areaNId As Integer, ByVal subNation As String, ByVal catalogImageURL As String, ByVal dbAdmName As String, ByVal dbAdmInstitution As String, ByVal dbAdmEmail As String, ByVal unicefRegion As String, ByVal adaptationYear As String, ByVal dbLanguages As String) As Boolean

        Dim RetVal As Boolean = False
        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DbParams As New List(Of System.Data.Common.DbParameter)()

        Try
            ObjDIConnection = GetDbConnection()

            If Not IsNothing(ObjDIConnection) Then

                Dim Param1 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param1.ParameterName = "ADAPTATION_NID"
                Param1.DbType = DbType.[Int32]
                Param1.Value = adaptationNid
                DbParams.Add(Param1)

                Dim Param2 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param2.ParameterName = "NAME"
                Param2.DbType = DbType.[String]
                Param2.Value = adaptationName
                DbParams.Add(Param2)

                Dim Param3 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param3.ParameterName = "DESCRIPTION"
                Param3.DbType = DbType.[String]
                Param3.Value = description
                DbParams.Add(Param3)

                Dim Param4 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param4.ParameterName = "DI_VERSION"
                Param4.DbType = DbType.[String]
                Param4.Value = version
                DbParams.Add(Param4)

                Dim Param5 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param5.ParameterName = "IS_DESKTOP"
                Param5.DbType = DbType.[Boolean]
                Param5.Value = isDesktop
                DbParams.Add(Param5)

                Dim Param6 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param6.ParameterName = "IS_ONLINE"
                Param6.DbType = DbType.[Boolean]
                Param6.Value = isWeb
                DbParams.Add(Param6)

                Dim Param7 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param7.ParameterName = "ONLINE_URL"
                Param7.DbType = DbType.[String]
                Param7.Value = webURL
                DbParams.Add(Param7)

                Dim Param8 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param8.ParameterName = "AREA_COUNT"
                Param8.DbType = DbType.[Int32]
                Param8.Value = areaCount
                DbParams.Add(Param8)

                Dim Param9 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param9.ParameterName = "IUS_COUNT"
                Param9.DbType = DbType.[Int32]
                Param9.Value = iusCount
                DbParams.Add(Param9)

                Dim Param10 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param10.ParameterName = "TIME_PERIODS_COUNT"
                Param10.DbType = DbType.[Int32]
                Param10.Value = timePeriodsCount
                DbParams.Add(Param10)

                Dim Param11 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param11.ParameterName = "DATA_VALUES_COUNT"
                Param11.DbType = DbType.[Int32]
                Param11.Value = dataValuesCount
                DbParams.Add(Param11)

                Dim Param12 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param12.ParameterName = "START_YEAR"
                Param12.DbType = DbType.[String]
                Param12.Value = startYear
                DbParams.Add(Param12)

                Dim Param13 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param13.ParameterName = "END_YEAR"
                Param13.DbType = DbType.[String]
                Param13.Value = endYear
                DbParams.Add(Param13)

                Dim Param14 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param14.ParameterName = "LAST_MODIFIED"
                Param14.DbType = DbType.[String]
                Param14.Value = lastModifiedOn
                DbParams.Add(Param14)

                Dim Param15 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param15.ParameterName = "AREA_NID"
                Param15.DbType = DbType.[Int32]
                Param15.Value = areaNId
                DbParams.Add(Param15)

                Dim Param16 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param16.ParameterName = "SUB_NATION"
                Param16.DbType = DbType.[String]
                Param16.Value = subNation
                DbParams.Add(Param16)

                Dim Param17 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param17.ParameterName = "THUMBNAIL_IMAGE_URL"
                Param17.DbType = DbType.[String]
                Param17.Value = catalogImageURL
                DbParams.Add(Param17)

                Dim Param18 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param18.ParameterName = "DB_ADM_NAME"
                Param18.DbType = DbType.[String]
                Param18.Value = dbAdmName
                DbParams.Add(Param18)

                Dim Param19 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param19.ParameterName = "DB_ADM_INSTITUTION"
                Param19.DbType = DbType.[String]
                Param19.Value = dbAdmInstitution
                DbParams.Add(Param19)

                Dim Param20 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param20.ParameterName = "DB_ADM_EMAIL"
                Param20.DbType = DbType.[String]
                Param20.Value = dbAdmEmail
                DbParams.Add(Param20)

                Dim Param21 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param21.ParameterName = "UNICEF_REGION"
                Param21.DbType = DbType.[String]
                Param21.Value = unicefRegion
                DbParams.Add(Param21)

                Dim Param22 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param22.ParameterName = "ADAPTATION_YEAR"
                Param22.DbType = DbType.[String]
                Param22.Value = adaptationYear
                DbParams.Add(Param22)

                Dim Param23 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param23.ParameterName = "DB_LANGUAGES"
                Param23.DbType = DbType.[String]
                Param23.Value = dbLanguages
                DbParams.Add(Param23)

                ObjDIConnection.ExecuteDataTable("SP_UPDATE_CATALOG", CommandType.StoredProcedure, DbParams)

                RetVal = True

            End If
        Catch ex As Exception
        Finally
            If Not IsNothing(ObjDIConnection) Then
                ObjDIConnection.Dispose()
            End If
        End Try

        Return RetVal
    End Function
    <WebMethod()> _
    Public Function UpdateCatalogInfo(ByVal adaptationName As String, ByVal version As String, ByVal webURL As String, ByVal lastModifiedOn As String, ByVal areaNId As Integer, ByVal subNation As String, ByVal dbAdmName As String, ByVal dbAdmInstitution As String, ByVal dbAdmEmail As String, ByVal unicefRegion As String, ByVal adaptationYear As String, ByVal AdaptedFor As String, ByVal Country As String, ByVal gUID As String) As Boolean

        Dim RetVal As Boolean = False
        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DbParams As New List(Of System.Data.Common.DbParameter)()

        Try
            ObjDIConnection = GetDbConnection()

            If Not IsNothing(ObjDIConnection) Then


                Dim Param1 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param1.ParameterName = "NAME"
                Param1.DbType = DbType.[String]
                Param1.Value = adaptationName
                DbParams.Add(Param1)

                Dim Param2 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param2.ParameterName = "DI_VERSION"
                Param2.DbType = DbType.[String]
                Param2.Value = version
                DbParams.Add(Param2)

                Dim Param3 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param3.ParameterName = "ONLINE_URL"
                Param3.DbType = DbType.[String]
                Param3.Value = webURL
                DbParams.Add(Param3)

                Dim Param4 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param4.ParameterName = "LAST_MODIFIED"
                Param4.DbType = DbType.[String]
                Param4.Value = lastModifiedOn
                DbParams.Add(Param4)

                Dim Param5 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param5.ParameterName = "AREA_NID"
                Param5.DbType = DbType.[Int32]
                Param5.Value = areaNId
                DbParams.Add(Param5)

                Dim Param6 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param6.ParameterName = "SUB_NATION"
                Param6.DbType = DbType.[String]
                Param6.Value = subNation
                DbParams.Add(Param6)

                Dim Param7 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param7.ParameterName = "DB_ADM_NAME"
                Param7.DbType = DbType.[String]
                Param7.Value = dbAdmName
                DbParams.Add(Param7)

                Dim Param8 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param8.ParameterName = "DB_ADM_INSTITUTION"
                Param8.DbType = DbType.[String]
                Param8.Value = dbAdmInstitution
                DbParams.Add(Param8)

                Dim Param9 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param9.ParameterName = "DB_ADM_EMAIL"
                Param9.DbType = DbType.[String]
                Param9.Value = dbAdmEmail
                DbParams.Add(Param9)

                Dim Param10 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param10.ParameterName = "UNICEF_REGION"
                Param10.DbType = DbType.[String]
                Param10.Value = unicefRegion
                DbParams.Add(Param10)

                Dim Param11 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param11.ParameterName = "ADAPTATION_YEAR"
                Param11.DbType = DbType.[String]
                Param11.Value = adaptationYear
                DbParams.Add(Param11)

                Dim Param12 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param12.ParameterName = "Adapted_For"
                Param12.DbType = DbType.[String]
                Param12.Value = AdaptedFor
                DbParams.Add(Param12)

                Dim Param13 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param13.ParameterName = "Country"
                Param13.DbType = DbType.[String]
                Param13.Value = Country
                DbParams.Add(Param13)

                Dim Param14 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param14.ParameterName = "AdaptationGUID"
                Param14.DbType = DbType.[String]
                Param14.Value = gUID
                DbParams.Add(Param14)

                ObjDIConnection.ExecuteDataTable("SP_UPDATE_CATALOG_INFO", CommandType.StoredProcedure, DbParams)

                RetVal = True

            End If
        Catch ex As Exception
        Finally
            If Not IsNothing(ObjDIConnection) Then
                ObjDIConnection.Dispose()
            End If
        End Try

        Return RetVal
    End Function

    <WebMethod()> _
    Public Function Update_LangCode_CSVFiles_Catalog(ByVal gUID As String, ByVal LangCode_CSVFiles As String) As Boolean

        Dim RetVal As Boolean = False
        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DbParams As New List(Of System.Data.Common.DbParameter)()

        Try
            ObjDIConnection = GetDbConnection()

            If Not IsNothing(ObjDIConnection) Then

                Dim Param1 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param1.ParameterName = "AdaptationGUID"
                Param1.DbType = DbType.[String]
                Param1.Value = gUID
                DbParams.Add(Param1)

                Dim Param2 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param2.ParameterName = "LangCode_CSVFiles"
                Param2.DbType = DbType.[String]
                Param2.Value = LangCode_CSVFiles
                DbParams.Add(Param2)

                ObjDIConnection.ExecuteDataTable("SP_UPDATE_LangCode_CSVFiles_CATALOG", CommandType.StoredProcedure, DbParams)

                RetVal = True

            End If
        Catch ex As Exception
        Finally
            If Not IsNothing(ObjDIConnection) Then
                ObjDIConnection.Dispose()
            End If
        End Try

        Return RetVal
    End Function

    <WebMethod()> _
    Public Function GetJsonAdaptations(ByVal gUID As String) As String
        Dim RetVal As String = String.Empty

        Try
            'Dim JsonFileNameWithPath As String = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock\") + JsonAdaptationsTxtFileName
            'If Not File.Exists(JsonFileNameWithPath) Then
            '    WriteAdaptationsJsonDataIntoTxtFile()
            'End If

            'RetVal = System.IO.File.ReadAllText(JsonFileNameWithPath)

            RetVal = GetAdaptationsJsonData(gUID)

        Catch ex As Exception
            RetVal = "ERROR: " + ex.Message
        End Try

        Return RetVal

    End Function

    <WebMethod()> _
    Public Function GetJsonM49CountriesFile() As String
        Dim RetVal As String = String.Empty

        Try
            Dim JsonFileNameWithPath As String = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock\") + JsonCountriesTxtFileName
            If Not File.Exists(JsonFileNameWithPath) Then
                WriteM49CountriesJsonDataIntoTxtFile()
            End If
            RetVal = System.IO.File.ReadAllText(JsonFileNameWithPath)
        Catch ex As Exception
        End Try

        Return RetVal

    End Function

    <WebMethod()> _
    Public Function UpdateIndexedAreas(ByVal objDataSet As DataSet, ByVal adaptationGUID As String) As Boolean


        Dim RetVal As Boolean = False
        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DbParams As New List(Of System.Data.Common.DbParameter)()
        Dim DtAdpt As DataTable = Nothing
        Dim AdptNId As String = String.Empty
        Dim LanguageCode As String = String.Empty
        Dim TableName As String = String.Empty

        Try

            ObjDIConnection = GetDbConnection()

            If Not IsNothing(ObjDIConnection) Then
                Dim Param1 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param1.ParameterName = "AdaptationGUID"
                Param1.DbType = DbType.[String]
                Param1.Value = adaptationGUID
                DbParams.Add(Param1)

                Dim Param2 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param2.ParameterName = "TABLE_TYPE"
                Param2.DbType = DbType.[String]
                Param2.Value = "Area"
                DbParams.Add(Param2)

                DtAdpt = ObjDIConnection.ExecuteDataTable("SP_GET_ADAPTATION_NID_AND_DELETE_INDEXED", CommandType.StoredProcedure, DbParams)

                AdptNId = Convert.ToString(DtAdpt.Rows(0)(0))

                If (DtAdpt.Rows.Count > 0) Then

                    For Each ObjTable As DataTable In objDataSet.Tables

                        TableName = ObjTable.TableName

                        LanguageCode = TableName.Substring(TableName.Length - 2)

                        ObjTable.Columns.Add("Adpt_NId", GetType(Integer))
                        ObjTable.Columns.Add("Language_Code", GetType(String))

                        For Each dr As DataRow In ObjTable.Rows

                            dr("Adpt_NId") = CInt(AdptNId)
                            dr("Language_Code") = LanguageCode

                        Next

                        Dim sqlBulkTableCopier As New SqlClient.SqlBulkCopy( _
                        ObjDIConnection.ConnectionStringParameters.GetConnectionString())

                        sqlBulkTableCopier.DestinationTableName = "IndexedAreas"
                        sqlBulkTableCopier.WriteToServer(ObjTable)

                    Next
                End If
            End If
            RetVal = True
        Catch ex As Exception
            RetVal = False

        End Try

        Return RetVal

    End Function

    <WebMethod()> _
    Public Function UpdateIndexedIndicators(ByVal objDataSet As DataSet, ByVal adaptationGUID As String) As Boolean
        Dim RetVal As Boolean = False

        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DbParams As New List(Of System.Data.Common.DbParameter)()
        Dim DtAdpt As DataTable = Nothing
        Dim AdptNId As String = String.Empty
        Dim LanguageCode As String = String.Empty
        Dim TableName As String = String.Empty

        Try

            ObjDIConnection = GetDbConnection()

            If Not IsNothing(ObjDIConnection) Then
                Dim Param1 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param1.ParameterName = "AdaptationGUID"
                Param1.DbType = DbType.[String]
                Param1.Value = adaptationGUID
                DbParams.Add(Param1)

                Dim Param2 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param2.ParameterName = "TABLE_TYPE"
                Param2.DbType = DbType.[String]
                Param2.Value = "Indicator"
                DbParams.Add(Param2)

                DtAdpt = ObjDIConnection.ExecuteDataTable("SP_GET_ADAPTATION_NID_AND_DELETE_INDEXED", CommandType.StoredProcedure, DbParams)

                If (DtAdpt.Rows.Count > 0) Then

                    AdptNId = Convert.ToString(DtAdpt.Rows(0)(0))

                    For Each ObjTable As DataTable In objDataSet.Tables

                        TableName = ObjTable.TableName
                        LanguageCode = TableName.Substring(TableName.Length - 2)

                        ObjTable.Columns.Add("Adpt_NId", GetType(Integer))
                        ObjTable.Columns.Add("Language_Code", GetType(String))

                        For Each dr As DataRow In ObjTable.Rows

                            dr("Adpt_NId") = CInt(AdptNId)
                            dr("Language_Code") = LanguageCode

                        Next

                        Dim sqlBulkTableCopier As New SqlClient.SqlBulkCopy( _
                        ObjDIConnection.ConnectionStringParameters.GetConnectionString())

                        sqlBulkTableCopier.DestinationTableName = "IndexedIndicators"
                        sqlBulkTableCopier.WriteToServer(ObjTable)

                    Next

                End If
            End If

            RetVal = True

        Catch ex As Exception

            RetVal = False

        End Try

        Return RetVal

    End Function

    <WebMethod()> _
    Public Function GetCatalogCacheResults(ByVal searchAreas As String, ByVal searchIndicators As String) As String

        Dim RetVal As String = String.Empty
        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DbParams As New List(Of System.Data.Common.DbParameter)()
        Dim DtCacheResult As DataTable = Nothing


        Try
            ObjDIConnection = GetDbConnection()

            If Not IsNothing(ObjDIConnection) Then
                Dim Param2 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param2.ParameterName = "SearchAreas"
                Param2.DbType = DbType.[String]
                Param2.Value = searchAreas
                DbParams.Add(Param2)

                Dim Param3 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param3.ParameterName = "SearchIndicators"
                Param3.DbType = DbType.[String]
                Param3.Value = searchIndicators
                DbParams.Add(Param3)

                DtCacheResult = ObjDIConnection.ExecuteDataTable("SP_GET_CATALOG_CACHE_RESULTS_EN", CommandType.StoredProcedure, DbParams)
                DtCacheResult.TableName = "CatalogCache"

                DtCacheResult = DtCacheResult.DefaultView.ToTable(True, "ADPT_NID")

                If DtCacheResult.Rows.Count > 0 Then
                    For Each Row As DataRow In DtCacheResult.Rows
                        RetVal += "," + Convert.ToString(Row("ADPT_NID"))
                    Next
                    RetVal = RetVal.Substring(1)
                End If
            End If
        Catch ex As Exception
        Finally
            If Not IsNothing(ObjDIConnection) Then
                ObjDIConnection.Dispose()
            End If
        End Try

        Return RetVal
    End Function

    Private Function RemoveDelimiter(ByVal InputString As String) As String
        Dim RetVal As String = String.Empty

        If String.IsNullOrEmpty(InputString) = False Then
            RetVal = InputString.Substring(Constants.PARAM_DELIMITER.Length)
        End If
        Return RetVal


    End Function

    <WebMethod()> _
    Public Function GetCatalogMatchedResults(ByVal searchAreas As String, ByVal searchIndicators As String, ByVal langCodeDb As String) As String

        Dim RetVal As String = String.Empty
        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DbParams As New List(Of System.Data.Common.DbParameter)()
        Dim DtCacheResult As DataTable = Nothing

        Dim Delimiter As String = Constants.PARAM_DELIMITER

        Try
            ObjDIConnection = GetDbConnection()

            If Not IsNothing(ObjDIConnection) Then

                ' Get matched areas
                Dim MatchedAreas As DataTable = GetMatchedAreas(searchAreas, ObjDIConnection, langCodeDb)
                ' Get matched Indicators
                Dim MatchedIndicators As DataTable = GetMatchedIndicators(searchIndicators, ObjDIConnection, langCodeDb)
                ' Create final Adaptation table with Intersection of Areas and Indicators
                Dim MatchedAdaptationsAreaIndicators As DataTable = New DataTable("MatchedResults")
                MatchedAdaptationsAreaIndicators.Columns.Add("Adaptation_NId", GetType(String))
                MatchedAdaptationsAreaIndicators.Columns.Add("Indicator_Names", GetType(String))
                MatchedAdaptationsAreaIndicators.Columns.Add("Indicator_NIds", GetType(String))
                MatchedAdaptationsAreaIndicators.Columns.Add("Area_Names", GetType(String))
                MatchedAdaptationsAreaIndicators.Columns.Add("Area_NIds", GetType(String))

                ' Only areas are found
                If MatchedIndicators.Rows.Count = 0 Then

                    ' Get distinct adaptation nids
                    ' Traverse each area of each adaptation nid
                    ' Concatinate area names & area nids grouped by adaptation nids

                    Dim distinctAdaptations As List(Of String) = getDistinctColValues(MatchedAreas, "Adpt_NId")

                    For Each adaptation_nid As String In distinctAdaptations
                        Dim nowAdptAreaNames As List(Of String) = getColValuesOfAdaptation(MatchedAreas, "Area_Name", adaptation_nid)
                        Dim nowAdptAreaNIds As List(Of String) = getColValuesOfAdaptation(MatchedAreas, "Area_NId", adaptation_nid)

                        ' Now concatinate each area name & area nid found for current adaptation

                        Dim tmpAreaNames As String = String.Empty
                        For Each area_name As String In nowAdptAreaNames
                            tmpAreaNames &= Delimiter & area_name
                        Next

                        Dim tmpAreaNIds As String = String.Empty
                        For Each area_nid As String In nowAdptAreaNIds
                            tmpAreaNIds &= Delimiter & area_nid
                        Next

                        tmpAreaNames = RemoveDelimiter(tmpAreaNames)
                        tmpAreaNIds = RemoveDelimiter(tmpAreaNIds)
                        ' Save them for each corresponding adaptation

                        Dim dr As DataRow = MatchedAdaptationsAreaIndicators.NewRow()
                        dr("Adaptation_NId") = adaptation_nid
                        dr("Area_Names") = tmpAreaNames
                        dr("Area_NIds") = tmpAreaNIds

                        MatchedAdaptationsAreaIndicators.Rows.Add(dr)

                    Next

                End If


                ' Only Indicators are found
                If MatchedAreas.Rows.Count = 0 Then
                    ' Get distinct adaptation nids
                    ' Traverse each indicator of each adaptation nid
                    ' Concatinate indicator names & indicator nids grouped by adaptation nids

                    Dim distinctAdaptations As List(Of String) = getDistinctColValues(MatchedIndicators, "Adpt_NId")

                    For Each adaptation_nid As String In distinctAdaptations
                        Dim nowAdptIndicatorNames As List(Of String) = getColValuesOfAdaptation(MatchedIndicators, "Indicator_Name", adaptation_nid)
                        Dim nowAdptIndicatorNIds As List(Of String) = getColValuesOfAdaptation(MatchedIndicators, "Indicator_NId", adaptation_nid)

                        ' Now concatinate each Indicator name & Indicator nid found for current adaptation

                        Dim tmpIndicatorNames As String = String.Empty
                        For Each Indicator_name As String In nowAdptIndicatorNames
                            tmpIndicatorNames &= Delimiter & Indicator_name
                        Next

                        Dim tmpIndicatorNIds As String = String.Empty
                        For Each Indicator_nid As String In nowAdptIndicatorNIds
                            tmpIndicatorNIds &= Delimiter & Indicator_nid
                        Next

                        tmpIndicatorNames = RemoveDelimiter(tmpIndicatorNames)
                        tmpIndicatorNIds = RemoveDelimiter(tmpIndicatorNIds)

                        ' Save them for each corresponding adaptation

                        Dim dr As DataRow = MatchedAdaptationsAreaIndicators.NewRow()
                        dr("Adaptation_NId") = adaptation_nid
                        dr("Indicator_Names") = tmpIndicatorNames
                        dr("Indicator_NIds") = tmpIndicatorNIds

                        MatchedAdaptationsAreaIndicators.Rows.Add(dr)

                    Next
                End If

                ' Both Indicators & Areas are found
                If MatchedAreas.Rows.Count > 0 And MatchedIndicators.Rows.Count > 0 Then
                    ' Firstly, find those adaptations which are found in both results of indicators search & areas search
                    ' Now traverse all areas, indicators info for each adaptation found common

                    Dim distinctIndicatorAdaptations As List(Of String) = getDistinctColValues(MatchedIndicators, "Adpt_NId")
                    Dim distinctAreaAdaptations As List(Of String) = getDistinctColValues(MatchedAreas, "Adpt_NId")

                    Dim distinctAdaptations As List(Of String) = New List(Of String)

                    For Each indicator_adaptation As String In distinctIndicatorAdaptations
                        If distinctAreaAdaptations.Contains(indicator_adaptation) Then
                            distinctAdaptations.Add(indicator_adaptation)
                        End If
                    Next


                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    For Each adaptation_nid As String In distinctAdaptations
                        Dim nowAdptIndicatorNames As List(Of String) = getColValuesOfAdaptation(MatchedIndicators, "Indicator_Name", adaptation_nid)
                        Dim nowAdptIndicatorNIds As List(Of String) = getColValuesOfAdaptation(MatchedIndicators, "Indicator_NId", adaptation_nid)
                        Dim nowAdptAreaNames As List(Of String) = getColValuesOfAdaptation(MatchedAreas, "Area_Name", adaptation_nid)
                        Dim nowAdptAreaNIds As List(Of String) = getColValuesOfAdaptation(MatchedAreas, "Area_NId", adaptation_nid)

                        ' Now concatinate each Indicator name & Indicator nid found for current adaptation

                        Dim tmpIndicatorNames As String = String.Empty
                        For Each Indicator_name As String In nowAdptIndicatorNames
                            tmpIndicatorNames &= Delimiter & Indicator_name
                        Next

                        Dim tmpIndicatorNIds As String = String.Empty
                        For Each Indicator_nid As String In nowAdptIndicatorNIds
                            tmpIndicatorNIds &= Delimiter & Indicator_nid
                        Next

                        ' Now concatinate each area name & area nid found for current adaptation

                        Dim tmpAreaNames As String = String.Empty
                        For Each area_name As String In nowAdptAreaNames
                            tmpAreaNames &= Delimiter & area_name
                        Next

                        Dim tmpAreaNIds As String = String.Empty
                        For Each area_nid As String In nowAdptAreaNIds
                            tmpAreaNIds &= Delimiter & area_nid
                        Next

                        ' Save them for each corresponding adaptation

                        Dim dr As DataRow = MatchedAdaptationsAreaIndicators.NewRow()
                        dr("Adaptation_NId") = adaptation_nid
                        dr("Indicator_Names") = RemoveDelimiter(tmpIndicatorNames)
                        dr("Indicator_NIds") = RemoveDelimiter(tmpIndicatorNIds)
                        dr("Area_Names") = RemoveDelimiter(tmpAreaNames)
                        dr("Area_NIds") = RemoveDelimiter(tmpAreaNIds)

                        MatchedAdaptationsAreaIndicators.Rows.Add(dr)

                    Next

                End If

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                RetVal = GetJSONString(MatchedAdaptationsAreaIndicators)


            End If
        Catch ex As Exception
        Finally
            If Not IsNothing(ObjDIConnection) Then
                ObjDIConnection.Dispose()
            End If
        End Try

        Return RetVal
    End Function

    <WebMethod()> _
    Public Function GetAdaptationVersions() As String
        Dim RetVal As String = String.Empty
        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DtAdptVersion As DataTable = Nothing

        Try
            ObjDIConnection = GetDbConnection()

            If Not IsNothing(ObjDIConnection) Then
                DtAdptVersion = ObjDIConnection.ExecuteDataTable("SP_GET_ADAPTATION_VERSIONS", CommandType.StoredProcedure, Nothing)
                DtAdptVersion.TableName = "AdaptationVersion"

                RetVal = GetJSONString(DtAdptVersion)
            End If

        Catch ex As Exception
        Finally
            If Not IsNothing(ObjDIConnection) Then
                ObjDIConnection.Dispose()
            End If
        End Try

        Return RetVal
    End Function

#End Region

#Region "-- App Settings --"

    <WebMethod()> _
    Public Function GetDisclaimerText() As String
        Dim RetVal As String = String.Empty
        Dim AppSettingFile As String = String.Empty
        Dim XmlDoc As XmlDocument

        Try
            AppSettingFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FileNames.AppSettingFileName)
            XmlDoc = New XmlDocument()
            XmlDoc.Load(AppSettingFile)

            RetVal = GetNodeValue(XmlDoc, Constants.AppSettingKeys.Disclaimer)

        Catch ex As Exception
        End Try

        Return RetVal
    End Function

    <WebMethod()> _
    Public Function CheckIsGlobalAdaptation(ByVal gUID As String) As Boolean
        Dim RetVal As Boolean = False
        Dim DT As DataTable = Nothing
        Dim Rows() As DataRow = Nothing
        Try
            DT = GetGlobalMasterUrlRecord(String.Empty)
            Rows = DT.Select("Area_NId=-1")
            For Each dr As DataRow In Rows
                If (dr("GUID").ToString().ToLower() = gUID) Then
                    RetVal = True
                End If
            Next
        Catch ex As Exception
        End Try
        Return RetVal
    End Function

    ''' <summary>
    ''' Get Adaptation URL by GUID of Adaptation.
    ''' </summary>
    ''' <param name="gUID">GUID of the Adaptation</param>
    ''' <returns>Adaptation URL | GUID | IsMasterAdaptation</returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function GetAdaptationURL(ByVal gUID As String) As XmlDocument
        Dim DT As DataTable = Nothing
        Dim Rows() As DataRow = Nothing
        Dim RetVal As New XmlDocument()
        Try
            RetVal = GetAdaptationInfo(gUID)
        Catch ex As Exception
        End Try
        Return RetVal
    End Function

    ''' <summary>
    ''' Get Adaptation URL by GUID of Adaptation.
    ''' </summary>
    ''' <param name="gUID">GUID of the Adaptation</param>
    ''' <returns>Adaptation URL | GUID | IsMasterAdaptation</returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function GetAdaptationInfo(ByVal gUID As String) As XmlDocument
        Dim DT As DataTable = Nothing
        Dim Rows() As DataRow = Nothing
        Dim RetVal As New XmlDocument()
        Dim InnerRow As DataRow
        Try

            Dim declaration As XmlNode = RetVal.CreateNode(XmlNodeType.XmlDeclaration, Nothing, Nothing)
            RetVal.AppendChild(declaration)
            Dim Adaptations As XmlElement = RetVal.CreateElement("Adaptations")
            RetVal.AppendChild(Adaptations)
            If Not String.IsNullOrEmpty(gUID.ToLower()) Then
                DT = GetGlobalMasterUrlRecord(gUID.ToLower())
                If DT.Rows.Count > 0 Then
                    InnerRow = DT.Rows(0)
                    If (InnerRow("GUID").ToString().ToLower() = gUID.ToLower()) Then
                        Dim Adaptation As XmlElement = RetVal.CreateElement("Adaptation")
                        Adaptation.InnerText = InnerRow("Online_URL").ToString()
                        Adaptations.AppendChild(Adaptation)

                        Dim GUIDVal As XmlAttribute = RetVal.CreateAttribute("GUID")
                        GUIDVal.Value = InnerRow("GUID").ToString()
                        Adaptation.Attributes.Append(GUIDVal)

                        Dim MasterAdaptation As XmlAttribute = RetVal.CreateAttribute("MasterAdaptation")
                        If Not IsDBNull(InnerRow("IS_DI7_ORG_SITE")) Then
                            MasterAdaptation.Value = "1"
                        Else
                            MasterAdaptation.Value = "0"
                        End If
                        Adaptation.Attributes.Append(MasterAdaptation)
                        Dim AdaptationName As XmlAttribute = RetVal.CreateAttribute("Name")
                        AdaptationName.Value = InnerRow("Name").ToString()
                        Adaptation.Attributes.Append(AdaptationName)
                        Dim Country As XmlAttribute = RetVal.CreateAttribute("Country")
                        Country.Value = InnerRow("Country").ToString()
                        Adaptation.Attributes.Append(Country)
                    End If
                End If
            Else
                DT = GetGlobalMasterUrlRecord(String.Empty)
                For Each dr As DataRow In DT.Rows
                    Dim Adaptation As XmlElement = RetVal.CreateElement("Adaptation")
                    Adaptation.InnerText = dr("Online_URL").ToString()
                    Adaptations.AppendChild(Adaptation)
                    Dim GUIDVal As XmlAttribute = RetVal.CreateAttribute("GUID")
                    GUIDVal.Value = dr("GUID").ToString()
                    Adaptation.Attributes.Append(GUIDVal)
                    Dim MasterAdaptation As XmlAttribute = RetVal.CreateAttribute("MasterAdaptation")
                    If Not IsDBNull(dr("IS_DI7_ORG_SITE")) Then
                        If dr("IS_DI7_ORG_SITE") = True Then
                            MasterAdaptation.Value = "1"
                        Else
                            MasterAdaptation.Value = "0"
                        End If
                    Else
                        MasterAdaptation.Value = "0"
                    End If
                    Adaptation.Attributes.Append(MasterAdaptation)
                    Dim AdaptationName As XmlAttribute = RetVal.CreateAttribute("Name")
                    AdaptationName.Value = dr("Name").ToString()
                    Adaptation.Attributes.Append(AdaptationName)
                    Dim Country As XmlAttribute = RetVal.CreateAttribute("Country")
                    Country.Value = dr("Country").ToString()
                    Adaptation.Attributes.Append(Country)

                Next
            End If
        Catch ex As Exception
        End Try
        Return RetVal
    End Function

    ''' <summary>
    ''' Get Database Metadata information for GUID
    ''' </summary>
    ''' <param name="gUID">Unique ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function GetDBMetaData(ByVal gUID As String) As XmlDocument
        Dim RetVal As New XmlDocument()
        Dim DTMetadata As DataTable = Nothing
        Dim InnerRow As DataRow
        Try
            DTMetadata = GetGlobalMasterUrlRecord(gUID.ToLower())
            Dim declaration As XmlNode = RetVal.CreateNode(XmlNodeType.XmlDeclaration, Nothing, Nothing)
            RetVal.AppendChild(declaration)
            Dim DBDetail As XmlElement = RetVal.CreateElement("DBDetail")
            RetVal.AppendChild(DBDetail)
            If gUID <> String.Empty And DTMetadata.Rows.Count > 0 Then
                InnerRow = DTMetadata.Rows(0)
                'Description
                Dim ElemantName As XmlElement = RetVal.CreateElement("Desc")
                Dim cdata As XmlNode = RetVal.CreateCDataSection(InnerRow("Description").ToString())
                ElemantName.AppendChild(cdata)
                DBDetail.AppendChild(ElemantName)

                'Area Count
                ElemantName = RetVal.CreateElement("AreaCount")
                ElemantName.InnerText = InnerRow("Area_Count").ToString()
                DBDetail.AppendChild(ElemantName)

                'IUS Count
                ElemantName = RetVal.CreateElement("IUSCount")
                ElemantName.InnerText = InnerRow("IUS_Count").ToString()
                DBDetail.AppendChild(ElemantName)

                'Time Periods Count
                ElemantName = RetVal.CreateElement("TPCount")
                ElemantName.InnerText = InnerRow("Time_Periods_Count").ToString()
                DBDetail.AppendChild(ElemantName)

                'Data Values Count
                ElemantName = RetVal.CreateElement("DVCount")
                ElemantName.InnerText = InnerRow("Data_Values_Count").ToString()
                DBDetail.AppendChild(ElemantName)

                'Start Year
                ElemantName = RetVal.CreateElement("StartYr")
                ElemantName.InnerText = InnerRow("Start_Year").ToString()
                DBDetail.AppendChild(ElemantName)

                'End Year
                ElemantName = RetVal.CreateElement("EndYr")
                ElemantName.InnerText = InnerRow("End_Year").ToString()
                DBDetail.AppendChild(ElemantName)

                'Last Modified
                ElemantName = RetVal.CreateElement("LstModified")
                cdata = RetVal.CreateCDataSection(InnerRow("Last_Modified").ToString())
                ElemantName.AppendChild(cdata)
                DBDetail.AppendChild(ElemantName)

                'Thumbnail Image URL
                ElemantName = RetVal.CreateElement("ThmbURL")
                cdata = RetVal.CreateCDataSection(InnerRow("Thumbnail_Image_URL").ToString())
                ElemantName.AppendChild(cdata)
                DBDetail.AppendChild(ElemantName)

                'DB Admin Name
                ElemantName = RetVal.CreateElement("DBADMName")
                ElemantName.InnerText = InnerRow("Db_Adm_Name").ToString()
                DBDetail.AppendChild(ElemantName)

                'DB Admin Institution
                ElemantName = RetVal.CreateElement("DBADMInst")
                cdata = RetVal.CreateCDataSection(InnerRow("Db_Adm_Institution").ToString())
                ElemantName.AppendChild(cdata)
                DBDetail.AppendChild(ElemantName)

                'DB Admin Email
                ElemantName = RetVal.CreateElement("DBADMEmail")
                cdata = RetVal.CreateCDataSection(InnerRow("Db_Adm_Email").ToString())
                ElemantName.AppendChild(cdata)
                DBDetail.AppendChild(ElemantName)
            End If
        Catch ex As Exception
        End Try
        Return RetVal
    End Function

    <WebMethod()> _
    Public Function CheckIsDI7ORGAdaptation(ByVal gUID As String) As Boolean
        Dim RetVal As Boolean = False
        Dim DT As DataTable = Nothing
        Dim Rows() As DataRow = Nothing
        Try
            DT = GetGlobalMasterUrlRecord(String.Empty)
            'Rows = DT.Select("Area_NId=-1")
            For Each dr As DataRow In DT.Rows
                If (dr("GUID").ToString().ToLower() = gUID) Then
                    If Not IsDBNull(dr("IS_DI7_ORG_SITE")) Then
                        If dr("IS_DI7_ORG_SITE") = True Then
                            RetVal = True
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
        End Try

        Return RetVal
    End Function

    <WebMethod()> _
    Public Function GetGlobalMasterWebUrl() As String
        Dim RetVal As String = String.Empty
        Dim DT As DataTable = Nothing
        Dim Rows() As DataRow = Nothing
        Try
            DT = GetGlobalMasterUrlRecord(String.Empty)
            Rows = DT.Select("is_di7_org_site=True")
            If (Rows.Length = 1) Then
                RetVal = Rows(0)("online_url").ToString()
            End If
        Catch ex As Exception
        End Try
        Return RetVal
    End Function

    <WebMethod()> _
    Public Function GetDateCreated(ByVal gUID As String) As DataSet
        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DbParams As New List(Of System.Data.Common.DbParameter)()
        Dim DtCatalog As DataTable = Nothing
        Dim RetVal As New DataSet()
        Try
            ObjDIConnection = GetDbConnection()

            If Not IsNothing(ObjDIConnection) Then
                Dim Param1 As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                Param1.ParameterName = "AdaptationURL"
                Param1.DbType = DbType.[String]
                Param1.Value = gUID
                DbParams.Add(Param1)

                DtCatalog = ObjDIConnection.ExecuteDataTable("SP_GET_DATE_CREATED", CommandType.StoredProcedure, DbParams)
                RetVal.Tables.Add(DtCatalog)
                Return RetVal
            End If

        Catch ex As Exception
        Finally
            If Not IsNothing(ObjDIConnection) Then
                ObjDIConnection.Dispose()
            End If
        End Try

        Return RetVal
    End Function

#End Region

#End Region

#End Region

End Class