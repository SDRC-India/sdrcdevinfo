Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports DALConnection = DevInfo.Lib.DI_LibDAL.Connection
Imports System.IO

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class AdaptationUsers
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function GetAllAdaptations() As String

        Dim RetVal As String = String.Empty

        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DbParams As New List(Of System.Data.Common.DbParameter)()
        Dim DtAdpt As DataTable = Nothing
        Try

            ObjDIConnection = SharedFunctions.GetDbConnection()

            If Not IsNothing(ObjDIConnection) Then

                DtAdpt = ObjDIConnection.ExecuteDataTable("Get_AdaptationName", CommandType.StoredProcedure, DbParams)
                Return SharedFunctions.GetJSONString(DtAdpt)

            End If

        Catch Ex As Exception

        Finally

            ObjDIConnection.Dispose()

        End Try

        Return RetVal


    End Function


    <WebMethod()> _
    Public Function GetUsersByAdaptationURL(ByVal AdaptationGUID As String, ByVal ShowMasterAccount As Boolean) As DataSet
        Dim RetVal As New DataSet()

        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DbParams As New List(Of System.Data.Common.DbParameter)()
        Dim DtAdpt As DataTable = Nothing
        Try

            ObjDIConnection = SharedFunctions.GetDbConnection()

            If Not IsNothing(ObjDIConnection) Then

                Dim ParamAdaptationURL As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                ParamAdaptationURL.ParameterName = "AdaptationGUID"
                ParamAdaptationURL.DbType = DbType.[String]
                ParamAdaptationURL.Value = AdaptationGUID
                DbParams.Add(ParamAdaptationURL)

                DtAdpt = ObjDIConnection.ExecuteDataTable("Get_Users_ByAdaptationURL", CommandType.StoredProcedure, DbParams)
                RetVal.Tables.Add(DtAdpt)

            End If

        Catch Ex As Exception

        Finally

            ObjDIConnection.Dispose()

        End Try

        Return RetVal


    End Function

    <WebMethod()> _
    Public Function SetUserAsAdmin(ByVal UserNId As Integer, ByVal AdaptationGUID As String) As Boolean

        Dim RetVal As Boolean = False

        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DbParams As New List(Of System.Data.Common.DbParameter)()
        Dim DtAdpt As DataTable = Nothing
        Try

            ObjDIConnection = SharedFunctions.GetDbConnection()

            If Not IsNothing(ObjDIConnection) Then

                Dim ParamUserNId As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                ParamUserNId.ParameterName = "User_NId"
                ParamUserNId.DbType = DbType.Int32
                ParamUserNId.Value = UserNId
                DbParams.Add(ParamUserNId)

                Dim ParamAdaptationURL As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                ParamAdaptationURL.ParameterName = "AdaptationGUID"
                ParamAdaptationURL.DbType = DbType.[String]
                ParamAdaptationURL.Value = AdaptationGUID
                DbParams.Add(ParamAdaptationURL)

                DtAdpt = ObjDIConnection.ExecuteDataTable("Set_User_AsAdmin", CommandType.StoredProcedure, DbParams)

                Return True

            End If

        Catch Ex As Exception

        Finally
            ObjDIConnection.Dispose()
        End Try

        Return RetVal


    End Function


    <WebMethod()> _
    Public Function GetGlobalUsers(ByVal Adaptation_NId As Integer) As DataSet

        Dim RetVal As New DataSet()
        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DbParams As New List(Of System.Data.Common.DbParameter)()
        Dim DtAdpt As DataTable = Nothing
        Try

            ObjDIConnection = SharedFunctions.GetDbConnection()

            If Not IsNothing(ObjDIConnection) Then

                Dim ParamAdaptationNId As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                ParamAdaptationNId.ParameterName = "AdaptationNId"
                ParamAdaptationNId.DbType = DbType.Int32
                ParamAdaptationNId.Value = Adaptation_NId
                DbParams.Add(ParamAdaptationNId)

                DtAdpt = ObjDIConnection.ExecuteDataTable("Get_GlobalUsers", CommandType.StoredProcedure, DbParams)
                RetVal.Tables.Add(DtAdpt)

                Return RetVal
            End If

        Catch Ex As Exception

        Finally
            ObjDIConnection.Dispose()

        End Try

        Return RetVal


    End Function



    <WebMethod()> _
    Public Function UpdateUserPassword(ByVal UserNId As Integer, ByVal CryptedPasswod As String) As Boolean

        Dim RetVal As Boolean = False

        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DbParams As New List(Of System.Data.Common.DbParameter)()
        Dim DtAdpt As DataTable = Nothing
        Try

            ObjDIConnection = SharedFunctions.GetDbConnection()

            If Not IsNothing(ObjDIConnection) Then

                Dim ParamUserNId As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                ParamUserNId.ParameterName = "User_NId"
                ParamUserNId.DbType = DbType.Int32
                ParamUserNId.Value = UserNId
                DbParams.Add(ParamUserNId)

                Dim ParamCryptedPassword As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                ParamCryptedPassword.ParameterName = "Password"
                ParamCryptedPassword.DbType = DbType.[String]
                ParamCryptedPassword.Value = CryptedPasswod
                DbParams.Add(ParamCryptedPassword)

                DtAdpt = ObjDIConnection.ExecuteDataTable("Update_Password", CommandType.StoredProcedure, DbParams)

                Return True

            End If

        Catch Ex As Exception

        Finally
            ObjDIConnection.Dispose()
        End Try

        Return RetVal


    End Function

    <WebMethod()> _
    Public Function GetAreaFromAreaNId(ByVal Area_NId As Integer) As DataSet

        Dim RetVal As New DataSet()
        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DbParams As New List(Of System.Data.Common.DbParameter)()
        Dim DtAdpt As DataTable = Nothing
        Try

            ObjDIConnection = SharedFunctions.GetDbConnection()

            If Not IsNothing(ObjDIConnection) Then

                Dim ParamAreaNId As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                ParamAreaNId.ParameterName = "AreaNId"
                ParamAreaNId.DbType = DbType.Int32
                ParamAreaNId.Value = Area_NId
                DbParams.Add(ParamAreaNId)

                DtAdpt = ObjDIConnection.ExecuteDataTable("Get_AreaFromAreaNId", CommandType.StoredProcedure, DbParams)
                RetVal.Tables.Add(DtAdpt)

                Return RetVal
            End If

        Catch Ex As Exception
        Finally
            ObjDIConnection.Dispose()
        End Try

        Return RetVal


    End Function

    <WebMethod()> _
    Public Function GetAreaForUser(ByVal User_NId As Integer) As DataSet

        Dim RetVal As New DataSet()
        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DbParams As New List(Of System.Data.Common.DbParameter)()
        Dim DtAdpt As DataTable = Nothing
        Try

            ObjDIConnection = SharedFunctions.GetDbConnection()

            If Not IsNothing(ObjDIConnection) Then

                Dim ParamUserNId As System.Data.Common.DbParameter = ObjDIConnection.CreateDBParameter()
                ParamUserNId.ParameterName = "UserNId"
                ParamUserNId.DbType = DbType.Int32
                ParamUserNId.Value = User_NId
                DbParams.Add(ParamUserNId)

                DtAdpt = ObjDIConnection.ExecuteDataTable("Get_AreaForUser", CommandType.StoredProcedure, DbParams)
                RetVal.Tables.Add(DtAdpt)

                Return RetVal
            End If

        Catch Ex As Exception
        Finally
            ObjDIConnection.Dispose()
        End Try

        Return RetVal


    End Function
End Class