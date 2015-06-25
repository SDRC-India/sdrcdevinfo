Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports di_Worldwide.DIDBSqlOperations
Imports DevInfo.Lib.DI_LibDAL.Connection


<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class DIOnlineDatabases
    Inherits System.Web.Services.WebService
    Dim objDIDBIndex As New DIDBSqlOperations

    ''' <summary>
    ''' Function to Register the database details into the Database Index Server
    ''' </summary>
    ''' <param name="dbType"></param>
    ''' <param name="AdaptationName"></param>
    ''' <param name="AdaptationVersion"></param>
    ''' <param name="ServerName"></param>
    ''' <param name="DBName"></param>
    ''' <param name="Username"></param>
    ''' <param name="Password"></param>
    ''' <param name="PortNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()> _
   Public Function Register(ByVal dbType As DIServerType, ByVal AdaptationName As String, ByVal AdaptationVersion As String, ByVal ServerName As String, ByVal DBName As String, ByVal Username As String, ByVal Password As String, ByVal PortNo As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = objDIDBIndex.RegisterDatabase(dbType, AdaptationName, AdaptationVersion, ServerName, DBName, Username, Password, PortNo)
        Catch ex As Exception
            RetVal = "Error"
        End Try
        Return RetVal
    End Function

    ''' <summary>
    ''' Function to search the keywords in the Database Index Server
    ''' </summary>
    ''' <param name="keywords"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function Search(ByVal keywords As String, ByVal langCode As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = objDIDBIndex.SimpleSearch(keywords, langCode)
        Catch ex As Exception

        End Try
        Return RetVal
    End Function

    ''' <summary>
    ''' Function to Truncate all the Info. in the DI Database Index Server
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function TruncateAll()
        Dim RetVal As String = String.Empty
        Try
            RetVal = objDIDBIndex.TruncateAllTables()
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Function to Unregister a database from the DI Database Index Server
    ''' </summary>
    ''' <param name="dbType"></param>
    ''' <param name="AdaptationName"></param>
    ''' <param name="AdaptationVersion"></param>
    ''' <param name="ServerName"></param>
    ''' <param name="DBName"></param>
    ''' <param name="Username"></param>
    ''' <param name="Password"></param>
    ''' <param name="PortNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function Unregister(ByVal dbType As DIServerType, ByVal AdaptationName As String, ByVal AdaptationVersion As String, ByVal ServerName As String, ByVal DBName As String, ByVal Username As String, ByVal Password As String, ByVal PortNo As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = objDIDBIndex.UnregisterDatabase(dbType, AdaptationName, AdaptationVersion, ServerName, DBName, Username, Password, PortNo)
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Function to check if a given database is already registered in the DI Database Index Server or not
    ''' </summary>
    ''' <param name="dbType"></param>
    ''' <param name="AdaptationName"></param>
    ''' <param name="AdaptationVersion"></param>
    ''' <param name="ServerName"></param>
    ''' <param name="DBName"></param>
    ''' <param name="Username"></param>
    ''' <param name="Password"></param>
    ''' <param name="PortNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function IsDatabaseRegistered(ByVal dbType As DIServerType, ByVal AdaptationName As String, ByVal AdaptationVersion As String, ByVal ServerName As String, ByVal DBName As String, ByVal Username As String, ByVal Password As String, ByVal PortNo As String) As Boolean
        Dim RetVal As Boolean = False
        Try
            If objDIDBIndex.DatabaseExists(dbType, AdaptationName, AdaptationVersion, ServerName, DBName, Username, Password, PortNo) Then
                RetVal = True
            End If
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Function to get all the details of all the Databases registed in the DI Database Index Server
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function GetAllDatabases(ByVal langCode As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = objDIDBIndex.GetAllDatabasesXML(langCode)
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
End Class