Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports DALConnection = DevInfo.Lib.DI_LibDAL.Connection
Imports System.Security.Cryptography
Imports System.IO


<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class UserLoginInformation
    Inherits System.Web.Services.WebService


#Region "-- Private --"

#Region "-- Variables --"
    Private EncryptionKey As String = "<""}#$7#%"
    Private ParamDelimiter As String = "[****]"
#End Region

#Region "-- Method --"
#Region "Getting Database connection"
    ''' <summary>
    ''' Get database connection
    ''' </summary>
    ''' <returns>DIConnection object</returns>
    ''' <remarks></remarks>
    Private Function GetDbConnection() As DALConnection.DIConnection

        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim ConnectionStr As String = String.Empty
        Dim ConnDetailsArr(5) As String
        Dim ServerName As String = String.Empty
        Dim DatabaseName As String = String.Empty
        Dim UserName As String = String.Empty
        Dim Password As String = String.Empty

        'Try
        ConnectionStr = ConfigurationManager.AppSettings("DBConnString")

        ConnDetailsArr = SplitString(ConnectionStr, ";")

        ServerName = ConnDetailsArr(0).Substring(ConnDetailsArr(0).IndexOf("=") + 1)
        DatabaseName = ConnDetailsArr(1).Substring(ConnDetailsArr(1).IndexOf("=") + 1)
        UserName = ConnDetailsArr(2).Substring(ConnDetailsArr(2).IndexOf("=") + 1)
        Password = ConnDetailsArr(3).Substring(ConnDetailsArr(3).IndexOf("=") + 1)

        ObjDIConnection = New DALConnection.DIConnection(DALConnection.DIServerType.SqlServer, ServerName, "", DatabaseName, UserName, Password)

        'Catch ex As Exception
        'End Try

        Return ObjDIConnection

    End Function
#End Region

#Region "Split String by delimeter"
    ''' <summary>
    ''' Split String on the basis of given delimiter.
    ''' </summary>
    ''' <param name="valueString">String which is going to be split</param>
    ''' <param name="delimiter">Delimiter</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
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
#End Region

#Region "-- Encryption / Decryption --"
    ''' <summary>
    ''' To Encrypt the text
    ''' </summary>
    ''' <param name="text">Text need to be encrypted</param>
    ''' <returns>Encrypted Text</returns>
    ''' <remarks></remarks>
    Private Function EncryptString(ByVal text As String) As String
        Dim RetVal As String
        Dim CryptoProvider As DESCryptoServiceProvider
        Dim MemoryStream As MemoryStream
        Dim CryptoStream As CryptoStream
        Dim Writer As StreamWriter
        Dim Bytes As Byte()

        RetVal = String.Empty
        If (String.IsNullOrEmpty(text) = False) Then
            Bytes = ASCIIEncoding.ASCII.GetBytes(EncryptionKey)
            CryptoProvider = New DESCryptoServiceProvider()
            MemoryStream = New MemoryStream(Bytes.Length)
            CryptoStream = New CryptoStream(MemoryStream, CryptoProvider.CreateEncryptor(Bytes, Bytes), CryptoStreamMode.Write)
            Writer = New StreamWriter(CryptoStream)
            Writer.Write(text)
            Writer.Flush()
            CryptoStream.FlushFinalBlock()
            Writer.Flush()
            RetVal = Convert.ToBase64String(MemoryStream.GetBuffer(), 0, CInt(MemoryStream.Length))
        End If
        Return RetVal
    End Function

    ''' <summary>
    ''' To decrypt the text
    ''' </summary>
    ''' <param name="text">test is need to be decrypted</param>
    ''' <returns>decrypted text</returns>
    ''' <remarks></remarks>
    Private Function DecryptString(ByVal text As String) As String
        Dim RetVal As String
        Dim CryptoProvider As DESCryptoServiceProvider
        Dim MemoryStream As MemoryStream
        Dim CryptoStream As CryptoStream
        Dim Reader As StreamReader
        Dim Bytes As Byte()

        RetVal = String.Empty

        If String.IsNullOrEmpty(text) = False Then
            Bytes = ASCIIEncoding.ASCII.GetBytes(EncryptionKey)
            CryptoProvider = New DESCryptoServiceProvider()
            MemoryStream = New MemoryStream(Convert.FromBase64String(text))
            CryptoStream = New CryptoStream(MemoryStream, CryptoProvider.CreateDecryptor(Bytes, Bytes), CryptoStreamMode.Read)
            Reader = New StreamReader(CryptoStream)
            RetVal = Reader.ReadToEnd()
        End If
        Return RetVal
    End Function

#End Region

#Region "Getting user information by 3 ways"

#Region "First way to get user information"
    ''' <summary>
    ''' Getting user information
    ''' </summary>
    ''' <param name="EmailId">EmailId</param>
    ''' <param name="Password">Password</param>
    ''' <param name="isAdmin">true if user is admin else false</param>
    ''' <returns>Datatable</returns>
    ''' <remarks></remarks>    
    Private Function Get_User(ByVal EmailId As String, ByVal Password As String, ByVal isAdmin As Boolean, ByVal AdaptationGUID As String) As DataTable
        Dim RetVal As DataTable
        Dim DIConnection As DALConnection.DIConnection
        Dim Query As String
        Dim AdapNid As String
        RetVal = Nothing
        DIConnection = Nothing
        Try
            DIConnection = GetDbConnection()
            Query = "SELECT * FROM Global_UserLogin, Areas, ProviderDetails WHERE Global_UserLogin.User_Email_Id = '" + EmailId + "' AND Global_UserLogin.User_Password='" + Password + "' AND Global_UserLogin.User_AreaNid = Areas.AreaNid AND Global_UserLogin.Nid = ProviderDetails.UserNid"
            If Not (String.IsNullOrEmpty(AdaptationGUID)) Then
                AdapNid = GetAdaptationNid(AdaptationGUID)
                Query += " AND ProviderDetails.AdaptationNid =" + AdapNid
            End If

            'Query = "SELECT * FROM Global_UserLogin WHERE User_Email_Id = '" + EmailId + "' AND User_Password = '" + Password + "'"
            If isAdmin = True Then
                Query += " AND ProviderDetails.User_Is_Admin = 'True'"
            End If
            RetVal = DIConnection.ExecuteDataTable(Query)
        Catch Ex As Exception
            Throw Ex
        Finally
            If Not (DIConnection Is Nothing) Then
                DIConnection.Dispose()
            End If
        End Try
        Return RetVal
    End Function
#End Region

#Region "Second way to get user information"
    ''' <summary>
    ''' Getting user information
    ''' </summary>
    ''' <param name="EmailId">EmailId</param>
    ''' <returns>Datatable</returns>
    ''' <remarks></remarks>
    Private Function Get_User(ByVal EmailId As String, ByVal AdaptationGUID As String) As DataTable
        Dim RetVal As DataTable
        Dim DIConnection As DALConnection.DIConnection
        Dim Query As String
        RetVal = Nothing
        DIConnection = Nothing
        Dim AdapNid As String
        Try
            DIConnection = GetDbConnection()
            AdapNid = GetAdaptationNid(AdaptationGUID)
            Query = "SELECT * FROM Global_UserLogin, Areas, ProviderDetails WHERE Global_UserLogin.NId = ProviderDetails.UserNid AND Global_UserLogin.User_Email_Id = '" + EmailId + "' AND Global_UserLogin.User_AreaNid = Areas.AreaNid AND ProviderDetails.AdaptationNid =" + AdapNid
            'Query = "SELECT * FROM Global_UserLogin WHERE User_Email_Id = '" + EmailId + "' and AdaptationNid"
            RetVal = DIConnection.ExecuteDataTable(Query)
        Catch Ex As Exception
            Throw Ex
        Finally
            If Not (DIConnection Is Nothing) Then
                DIConnection.Dispose()
            End If
        End Try
        Return RetVal
    End Function
#End Region

#Region "Third way to get user information"
    ''' <summary>
    ''' Getting user information
    ''' </summary>
    ''' <param name="UserNId">user nid</param>
    ''' <returns>Datatable</returns>
    ''' <remarks></remarks>
    Private Function Get_User(ByVal UserNId As Integer, ByVal AdaptationGUID As String) As DataTable
        Dim RetVal As DataTable
        Dim DIConnection As DALConnection.DIConnection
        Dim Query As String
        Dim AdapNid As String

        RetVal = Nothing
        DIConnection = Nothing

        Try
            DIConnection = GetDbConnection()
            Query = "SELECT * FROM Global_UserLogin,ProviderDetails,Areas WHERE Global_UserLogin.NId = ProviderDetails.UserNid AND Global_UserLogin.NId = " + UserNId.ToString() + " AND Global_UserLogin.User_AreaNid = Areas.AreaNid"
            If Not (String.IsNullOrEmpty(AdaptationGUID)) Then
                AdapNid = GetAdaptationNid(AdaptationGUID)
                Query += " AND ProviderDetails.AdaptationNid =" + AdapNid
            End If
            RetVal = DIConnection.ExecuteDataTable(Query)
        Catch Ex As Exception
            Throw Ex
        Finally
            If Not (DIConnection Is Nothing) Then
                DIConnection.Dispose()
            End If
        End Try
        Return RetVal
    End Function
#End Region
#End Region

#Region "Delete user from the table"
    ''' <summary>
    ''' Delete user information from the database
    ''' </summary>
    ''' <param name="UserNId">User nid</param>
    ''' <returns>true if successfully deleted else false</returns>
    ''' <remarks></remarks>
    Public Function Delete_In_User_Table(ByVal UserNId As Integer) As Boolean
        Dim RetVal As Boolean = False
        Dim DeleteQuery As String
        Dim DIConnection As DALConnection.DIConnection
        DeleteQuery = String.Empty
        DIConnection = Nothing
        Try
            DIConnection = GetDbConnection()
            DeleteQuery = "DELETE FROM Global_UserLogin WHERE NId = " + UserNId.ToString()
            DIConnection.ExecuteDataTable(DeleteQuery)
            DeleteQuery = "DELETE FROM ProviderDetails WHERE UserNid = " + UserNId.ToString()
            DIConnection.ExecuteDataTable(DeleteQuery)
            RetVal = True
        Catch Ex As Exception
            Throw Ex
        Finally
            If Not (DIConnection Is Nothing) Then
                DIConnection.Dispose()
            End If
        End Try
        Return RetVal
    End Function
#End Region

#Region "To get adaptation nid"
    Private Function GetAdaptationNid(ByVal gUID As String) As String
        Dim AdaptationNid As String = String.Empty
        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DtCatalog As DataTable = Nothing
        Dim Query As String = String.Empty

        Try
            ObjDIConnection = GetDbConnection()
            Query = "Select NId from Adaptations where [GUID]='" + gUID + "'"
            DtCatalog = ObjDIConnection.ExecuteDataTable(Query)
            AdaptationNid = DtCatalog.Rows(0)(0).ToString()
        Catch ex As Exception
        Finally
            If Not IsNothing(ObjDIConnection) Then
                ObjDIConnection.Dispose()
            End If
        End Try
        Return AdaptationNid
    End Function
#End Region

#Region "To all existing adaptation nid"
    Private Function GetAllAdaptationNid() As DataTable
        Dim DtAdaNid As DataTable = Nothing
        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim Query As String = String.Empty

        Try
            ObjDIConnection = GetDbConnection()
            Query = "Select NId from Adaptations"
            DtAdaNid = ObjDIConnection.ExecuteDataTable(Query)
        Catch ex As Exception
        Finally
            If Not IsNothing(ObjDIConnection) Then
                ObjDIConnection.Dispose()
            End If
        End Try
        Return DtAdaNid
    End Function
#End Region


#End Region
#End Region
#Region "----Public----"
#Region "Save user's record into table"
    ''' <summary>
    ''' Register new user into global_userlogin
    ''' </summary>
    ''' <param name="UserEmailId">Email Id</param>
    ''' <param name="UserPassword">Encrypted Password</param>
    ''' <param name="UserFirstName">User Firstname</param>
    ''' <param name="UserLastName">User Lastname</param>
    ''' <param name="UserCountryNid">Area Nid</param>
    ''' <param name="IsUserProvider">true if user is provider else false</param>
    ''' <param name="IsUserAdmin">true if user is admin else false</param>    
    ''' <param name="AdaptationGUID">Adaptation GUID</param>    
    ''' <returns>true if record is inserted successfully else false</returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function Save_In_User_Table(ByVal UserEmailId As String, ByVal UserPassword As String, ByVal UserFirstName As String, ByVal UserLastName As String, ByVal UserCountryNid As String, ByVal IsUserProvider As Boolean, ByVal IsUserAdmin As Boolean, ByVal Send_Updates As Boolean, ByVal AdaptationGUID As String) As Boolean
        Dim DbConnection As DALConnection.DIConnection = Nothing
        Dim RetVal As Boolean = False
        Dim InsertQuery, AdaptationNid As String
        Dim DtAllAdapNid As DataTable = Nothing
        Dim UserNid As String = String.Empty
        Dim DtAllUserNid As DataTable = Nothing
        Dim UpdateQuery As String
        UpdateQuery = String.Empty

        Try
            AdaptationNid = GetAdaptationNid(AdaptationGUID)
            ' Get database connection object
            DbConnection = GetDbConnection()
            ' Insert record into global_userlogin table
            InsertQuery = "INSERT INTO Global_UserLogin (User_Email_Id, User_Password, User_First_Name, User_Last_Name, User_AreaNid) VALUES('" + UserEmailId + "','" + UserPassword + "','" + UserFirstName + "','" + UserLastName + "'," + UserCountryNid.ToString() + ");"
            DbConnection.ExecuteDataTable(InsertQuery)
            ' Get usernid which is recently created.             
            UserNid = DbConnection.ExecuteDataTable("Select Nid from Global_UserLogin where user_email_id='" + UserEmailId + "'").Rows(0)(0).ToString()
            InsertQuery = "INSERT INTO ProviderDetails (UserNid, AdaptationNid, User_Is_Provider, User_Is_Admin,Send_Updates) VALUES(" + UserNid + "," + AdaptationNid + ",'" + IsUserProvider.ToString() + "','" + IsUserAdmin.ToString() + "','" + Send_Updates.ToString() + "')"
            DbConnection.ExecuteDataTable(InsertQuery)
            If IsUserAdmin Then
                UpdateQuery = "UPDATE ProviderDetails SET User_Is_Provider ='False', User_Is_Admin = 'False' WHERE AdaptationNid in (SELECT NId FROM Adaptations WHERE [GUID] = '" + AdaptationGUID + "') AND User_Is_Admin = 'True' AND UserNid != " + UserNid + ""
                DbConnection.ExecuteDataTable(UpdateQuery)
            End If

            DtAllAdapNid = GetAllAdaptationNid()
            For Each AdapNidRow As DataRow In DtAllAdapNid.Rows
                If AdapNidRow(0).ToString() <> AdaptationNid Then
                    InsertQuery = "INSERT INTO ProviderDetails (UserNid, AdaptationNid, User_Is_Provider, User_Is_Admin, Send_Updates) VALUES(" + UserNid + "," + AdapNidRow(0).ToString() + ",'False','False','" + Send_Updates.ToString() + "')"
                    DbConnection.ExecuteDataTable(InsertQuery)
                End If
            Next
            RetVal = True
        Catch ex As Exception
        End Try
        Return RetVal
    End Function
#End Region

#Region "Login user"
    ''' <summary>
    ''' To login user
    ''' </summary>
    ''' <param name="EmailId">Email Id</param>
    ''' <param name="Password">Password</param>
    ''' <param name="IsAdmin">true if user is admin</param>    
    ''' <returns>true[****]user details if successfully login else false[****]Invalid Credentials! / Error message</returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function LoginUser(ByVal EmailId As String, ByVal Password As String, ByVal IsAdmin As Boolean, ByVal AdaptationGUID As String) As String
        Dim RetVal As String
        Dim DtUser As DataTable
        Try
            DtUser = Get_User(EmailId, Password, IsAdmin, AdaptationGUID)

            If (Not (DtUser Is Nothing)) And DtUser.Rows.Count > 0 Then

                Dim AdminUserRows() As DataRow = DtUser.Select("User_Is_Admin='True'")

                ' Check if any of selected users is admin for given adaptation, 
                ' if yes then return UserNId of that Admin user only
                ' otherwise, return first user nid from selected users (default case)

                Dim UserNid As String = String.Empty

                If AdminUserRows.Length > 0 Then
                    RetVal = "true" + ParamDelimiter + "1" + ParamDelimiter + AdminUserRows(0)("NId").ToString() + "|" + AdminUserRows(0)("User_Is_Provider").ToString() + ParamDelimiter + AdminUserRows(0)("User_First_Name").ToString()
                Else
                    RetVal = "true" + ParamDelimiter + "1" + ParamDelimiter + DtUser.Rows(0)("NId").ToString() + "|" + DtUser.Rows(0)("User_Is_Provider").ToString() + ParamDelimiter + DtUser.Rows(0)("User_First_Name").ToString()
                End If

            Else
                RetVal = "false" + ParamDelimiter + "3"
            End If
        Catch Ex As Exception
            RetVal = "false" + ParamDelimiter + Ex.Message
        End Try
        Return RetVal
    End Function
#End Region

#Region "Does user exist or not on the bases of email id"
    ''' <summary>
    ''' Does user exist into database
    ''' </summary>
    ''' <param name="EmailId"> Email Id</param>
    ''' <returns>true if user exists else false</returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function Is_Existing_User(ByVal EmailId As String) As Boolean
        Dim RetVal As Boolean
        Dim DIConnection As DALConnection.DIConnection
        Dim Query As String
        Dim DtUser As DataTable

        RetVal = False
        DIConnection = Nothing
        Try
            DIConnection = GetDbConnection()
            Query = "SELECT * FROM Global_UserLogin WHERE User_Email_Id = '" + EmailId + "'"
            DtUser = DIConnection.ExecuteDataTable(Query)

            If (Not (DtUser Is Nothing) And DtUser.Rows.Count > 0) Then
                RetVal = True
            Else
                RetVal = False
            End If
        Catch Ex As Exception
            Throw Ex
        Finally
            If Not (DIConnection Is Nothing) Then
                DIConnection.Dispose()
            End If
        End Try
        Return RetVal
    End Function

#End Region

#Region "To get user information"
    ''' <summary>
    ''' To get user's details
    ''' </summary>
    ''' <param name="UserNId">User nid</param>
    ''' <returns>user's details, seperated by [****]</returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function GetUserDetails(ByVal UserNId As String, ByVal AdapGUID As String) As String
        Dim RetVal As String
        Dim DtUser As DataTable
        Try
            RetVal = String.Empty
            DtUser = Get_User(Convert.ToInt32(UserNId), AdapGUID)
            If (Not (DtUser Is Nothing) And DtUser.Rows.Count > 0) Then
                RetVal = DtUser.Rows(0)("User_Email_id").ToString() + ParamDelimiter + DtUser.Rows(0)("User_Password").ToString() + ParamDelimiter + DtUser.Rows(0)("User_First_Name").ToString() + ParamDelimiter + DtUser.Rows(0)("User_Last_Name").ToString() + ParamDelimiter + DtUser.Rows(0)("User_AreaNid").ToString() + ParamDelimiter + DtUser.Rows(0)("User_Is_Provider").ToString() + ParamDelimiter + DtUser.Rows(0)("User_Is_Admin").ToString() + ParamDelimiter + DtUser.Rows(0)("Send_Updates").ToString()
            End If
        Catch Ex As Exception
            RetVal = Ex.Message
        End Try
        Return RetVal
    End Function
#End Region

#Region "Get user name"
    ''' <summary>
    ''' To get user name
    ''' </summary>
    ''' <param name="EmailId">Email Id</param>
    ''' <returns>true[****]userdetails if user is valid otherwise false[****]Error message</returns>
    ''' <remarks>This function is used while retriving the password from database</remarks>
    <WebMethod()> _
    Public Function Get_UserName(ByVal EmailId As String, ByVal AdapGUID As String) As String
        Dim RetVal As String = String.Empty
        Dim FirstName As String
        Dim DtUser As DataTable
        Dim Delemeter As String = "|"
        Try
            DtUser = Get_User(EmailId, AdapGUID)
            If (Not (DtUser Is Nothing) And DtUser.Rows.Count > 0) Then
                FirstName = DtUser.Rows(0)("User_First_Name").ToString()
                RetVal = FirstName
            End If
        Catch Ex As Exception
        End Try
        Return RetVal
    End Function
#End Region

#Region "Get all user information"
    ''' <summary>
    ''' Get all user information
    ''' </summary>
    ''' <returns>Datatable object</returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function GetAllUsersGridHTML(ByVal AdaptationGUID As String) As DataTable
        Dim DtUsers As DataTable
        Dim AdaptationNid = String.Empty
        Dim DIConnection As DALConnection.DIConnection = Nothing
        Dim Query As String
        Try
            AdaptationNid = GetAdaptationNid(AdaptationGUID)
            DIConnection = GetDbConnection()
            Query = "SELECT DISTINCT NId, User_Email_Id as Email, User_First_Name as [First Name], User_Last_Name as [Last Name], User_AreaNid as [User Country], User_Is_Admin as [Admin User],User_Is_Provider as [Provider User], AdaptationNid, Send_Updates FROM Global_UserLogin, ProviderDetails where ProviderDetails.UserNid = Global_UserLogin.Nid And AdaptationNid =" + AdaptationNid
            DtUsers = DIConnection.ExecuteDataTable(Query)
            Dim DataRows() As DataRow = DtUsers.Select("[Admin User] ='True'")
            DtUsers.Rows.Remove(DataRows(0))
            DtUsers.TableName = "user"
            DtUsers.AcceptChanges()
            Return DtUsers
        Catch Ex As Exception
            Throw Ex
        Finally
            If Not (DIConnection Is Nothing) Then
                DIConnection.Dispose()
            End If
        End Try
        Return DtUsers
    End Function
#End Region

#Region "Is user DataProvider"
    ''' <summary>
    ''' Is user dataprovider
    ''' </summary>
    ''' <param name="UserNid">usernid</param>
    ''' <returns>true if user is dataprovider otherwise false</returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function IsUserDataProvider(ByVal UserNid As String, ByVal AdapGUID As String) As Boolean
        Dim IsDataProvider As Boolean
        Dim DtTable As DataTable
        IsDataProvider = False
        Try
            DtTable = Get_User(Convert.ToInt32(UserNid), AdapGUID)
            If (Not (DtTable Is Nothing) And DtTable.Rows.Count > 0) Then
                IsDataProvider = Convert.ToBoolean(DtTable.Rows(0)("User_Is_Provider").ToString())
            End If
        Catch ex As Exception
        End Try
        Return IsDataProvider
    End Function
#End Region

#Region "Is user Admin"
    ''' <summary>
    ''' Is user admin
    ''' </summary>
    ''' <param name="UserNid">usernid</param>
    ''' <returns>true if user is admin otherwise false</returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function IsUserAdmin(ByVal UserNid As String, ByVal AdapGUID As String) As Boolean
        Dim IsAdmin As Boolean
        Dim DtTable As DataTable
        IsAdmin = False
        Try
            DtTable = Get_User(Convert.ToInt32(UserNid), AdapGUID)
            If (Not (DtTable Is Nothing) And DtTable.Rows.Count > 0) Then
                IsAdmin = Convert.ToBoolean(DtTable.Rows(0)("User_Is_Admin").ToString())
            End If
        Catch ex As Exception
        End Try
        Return IsAdmin
    End Function
#End Region



#Region "Is MasterAccount"
    ''' <summary>
    ''' Is MasterAccount
    ''' </summary>
    ''' <param name="UserNid">usernid</param>
    ''' <returns>true if MasterAccount otherwise false</returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function IsMasterAccount(ByVal UserNid As String) As Boolean

        Dim RetVal As Boolean

        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim DbParams As New List(Of System.Data.Common.DbParameter)()
        Dim DtTable As DataTable = Nothing
        Try

            ObjDIConnection = SharedFunctions.GetDbConnection()

            If Not IsNothing(ObjDIConnection) Then

                DtTable = ObjDIConnection.ExecuteDataTable("SP_GET_MASTER_ACCOUNT", CommandType.StoredProcedure, DbParams)
                If (UserNid = DtTable.Rows(0)(0).ToString()) Then
                    RetVal = True
                Else
                    RetVal = False

                End If

            End If

        Catch Ex As Exception

        Finally

            ObjDIConnection.Dispose()

        End Try

        Return RetVal


    End Function
#End Region
#Region "To get user nid"
    ''' <summary>
    ''' To get user nid
    ''' </summary>
    ''' <param name="EmailId">Email Id</param>
    ''' <param name="Password">Password</param>
    ''' <param name="IsAdmin">true if user is admin otherwise false</param>
    ''' <returns>user nid</returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function GetUserNid(ByVal EmailId As String, ByVal Password As String, ByVal IsAdmin As Boolean) As String
        Dim UserNid As String = String.Empty
        Dim DtUser As DataTable = Nothing
        DtUser = Get_User(EmailId, Password, IsAdmin, "")
        If (Not (DtUser Is Nothing) And DtUser.Rows.Count > 0) Then
            UserNid = DtUser.Rows(0)("NId").ToString()
        End If
        Return UserNid
    End Function
#End Region

#Region "Update user information"
    ''' <summary>
    ''' Update user details
    ''' </summary>
    ''' <param name="UserNId">User Id</param>
    ''' <param name="EmailId">Email Id</param>
    ''' <param name="FirstName">Firstname</param>
    ''' <param name="LastName">Lastname</param>
    ''' <param name="Country">Country</param>
    ''' <param name="Password">Password</param>
    ''' <param name="IsDataProvider">true if d</param>
    ''' <param name="IsAdmin"></param>    
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function Update_In_User_Table(ByVal UserNId As String, ByVal EmailId As String, ByVal Password As String, ByVal FirstName As String, ByVal LastName As String, ByVal Country As String, ByVal IsDataProvider As Boolean, ByVal IsAdmin As Boolean, ByVal Send_Updates As Boolean, ByVal AdapGUID As String) As Boolean
        Dim RetVal As Boolean = False
        Dim UpdateQuery As String
        Dim DIConnection As DALConnection.DIConnection
        Dim AdapNid As String
        UpdateQuery = String.Empty
        DIConnection = Nothing
        Try
            DIConnection = GetDbConnection()
            AdapNid = GetAdaptationNid(AdapGUID)
            If (String.IsNullOrEmpty(Password) = False) Then
                UpdateQuery = "UPDATE Global_UserLogin SET User_Email_Id = '" + EmailId + "', User_Password = '" + Password + "', User_First_Name = '" + FirstName + "', User_Last_Name = '" + LastName + "', User_AreaNid = '" + Country + "' WHERE NId = " + UserNId
            Else
                UpdateQuery = "UPDATE Global_UserLogin SET User_Email_Id = '" + EmailId + "', User_First_Name = '" + FirstName + "', User_Last_Name = '" + LastName + "', User_AreaNid = '" + Country + "' WHERE NId = " + UserNId
            End If

            DIConnection.ExecuteDataTable(UpdateQuery)
            UpdateQuery = "UPDATE ProviderDetails SET User_Is_Provider='" + IsDataProvider.ToString() + "', User_Is_Admin ='" + IsAdmin.ToString() + "', Send_Updates ='" + Send_Updates.ToString() + "' WHERE UserNid = " + UserNId + " AND AdaptationNid = " + AdapNid

            DIConnection.ExecuteDataTable(UpdateQuery)
            RetVal = True
        Catch Ex As Exception
            RetVal = False
            Throw Ex
        Finally
            If Not (DIConnection Is Nothing) Then
                DIConnection.Dispose()
            End If
        End Try
        Return RetVal
    End Function
#End Region

#Region "To get user email id"
    ''' <summary>
    ''' To get user email id
    ''' </summary>
    ''' <param name="UserNid">User Nid</param>        
    ''' <returns>user email id</returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function GetUserEmailId(ByVal UserNid As String) As String
        Dim UserEmailId As String = String.Empty
        Dim DtUser As DataTable = Nothing
        Try
            DtUser = Get_User(Convert.ToInt32(UserNid), "")
            If (Not (DtUser Is Nothing) And DtUser.Rows.Count > 0) Then
                UserEmailId = DtUser.Rows(0)("User_Email_Id").ToString()
            End If
        Catch ex As Exception
        End Try
        Return UserEmailId
    End Function
#End Region

#Region "Does user exist or not on the bases of user nid"
    ''' <summary>
    ''' Does user exist into database
    ''' </summary>
    ''' <param name="UserNid"> User Nid</param>
    ''' <returns>true if user exists else false</returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function IsValidUser(ByVal UserNid As String) As Boolean
        Dim RetVal As Boolean
        Dim DIConnection As DALConnection.DIConnection
        Dim Query As String
        Dim DtUser As DataTable

        RetVal = False
        DIConnection = Nothing
        Try
            DIConnection = GetDbConnection()
            Query = "SELECT * FROM Global_UserLogin WHERE Nid = " + UserNid
            DtUser = DIConnection.ExecuteDataTable(Query)

            If (Not (DtUser Is Nothing) And DtUser.Rows.Count > 0) Then
                RetVal = True
            Else
                RetVal = False
            End If
        Catch Ex As Exception
            Throw Ex
        Finally
            If Not (DIConnection Is Nothing) Then
                DIConnection.Dispose()
            End If
        End Try
        Return RetVal
    End Function

#End Region

#Region "Is Admin registered"
    ''' <summary>
    ''' Is Admin registered
    ''' </summary>
    ''' <param name="AdaptaionUrl">AdaptaionUrl</param>
    ''' <returns>true if user is admin otherwise false</returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function IsAdminRegistered(ByVal adaptaionUrl As String, ByVal AdaptationName As String, ByVal DbAdmName As String, ByVal DbAdmInstitution As String, ByVal DbAdmEmail As String, ByVal AdaptedFor As String, ByVal Country As String, ByVal Subnational As String, ByVal AreaNId As String, ByVal CatalogImage As String, ByVal Version As String, ByVal gUID As String) As String
        Dim IsAdmin As String
        Dim DtTable, DtAdap As DataTable
        Dim AdaptationNid As String
        Dim DIConnection As DALConnection.DIConnection = Nothing
        Dim Query As String
        Dim DtAllUserNid As DataTable = Nothing
        Dim MasterUser As String
        IsAdmin = "False"
        Try
            DIConnection = GetDbConnection()
            Query = "SELECT * From Adaptations where [GUID]='" + gUID + "'"
            DtAdap = DIConnection.ExecuteDataTable(Query)
            If (Not (DtAdap Is Nothing) And DtAdap.Rows.Count = 0) Then
                Dim CatalogService As Catalog = New Catalog()
                CatalogService.SetCatalog(AdaptationName, "", Version, False, True, adaptaionUrl, 0, 0, 0, 0, "", "", Date.Now.ToString(), AreaNId, Subnational, CatalogImage, DbAdmName, DbAdmInstitution, DbAdmEmail, "", "", "", "", AdaptedFor, Country, Date.Now.ToString(), gUID)
                AdaptationNid = GetAdaptationNid(gUID)
                Query = "SELECT Nid From Global_UserLogin"
                DtAllUserNid = DIConnection.ExecuteDataTable(Query)
                For Each UserNidRow As DataRow In DtAllUserNid.Rows
                    Query = "INSERT INTO ProviderDetails (UserNid, AdaptationNid, User_Is_Provider,User_Is_Admin) VALUES(" + UserNidRow(0).ToString() + "," + AdaptationNid + ",'False','False')"
                    DIConnection.ExecuteDataTable(Query)
                Next
            End If
            AdaptationNid = GetAdaptationNid(gUID)
            Query = "SELECT * FROM Global_UserLogin, ProviderDetails WHERE Global_UserLogin.Nid = ProviderDetails.UserNid And AdaptationNid = " + AdaptationNid + " AND USER_IS_ADMIN='True'"
            DtTable = DIConnection.ExecuteDataTable(Query)
            If (Not (DtTable Is Nothing) And DtTable.Rows.Count > 0) Then
                IsAdmin = "True"

            Else
                Query = "SELECT Nid From Global_UserLogin WHERE [IsMasterAccount] = 'True'"
                MasterUser = DIConnection.ExecuteScalarSqlQuery(Query)
                Query = "UPDATE ProviderDetails SET User_Is_Provider = 'True', User_Is_Admin = 'True' WHERE UserNid = " + MasterUser + " And AdaptationNid = " + AdaptationNid + ""
                DIConnection.ExecuteDataTable(Query)
                IsAdmin = "TrueMA"
            End If
        Catch ex As Exception
        End Try
        Return IsAdmin
    End Function
#End Region

#Region "Get Master Account Detail"
    ''' <summary>
    ''' Is Admin registered
    ''' </summary>
    ''' <param name="AdaptaionUrl">AdaptaionUrl</param>
    ''' <returns>true if user is admin otherwise false</returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function GetMasterAccountDetail(ByVal AdaptaionGUID As String) As DataSet
        Dim RetVal As New DataSet()
        Dim DtTable As DataTable = Nothing

        Dim DIConnection As DALConnection.DIConnection = Nothing
        Dim Query As String
        Try
            DIConnection = GetDbConnection()
            Query = "SELECT NId,User_Email_Id,User_First_Name From Global_UserLogin WHERE [IsMasterAccount] = 'True'"
            DtTable = DIConnection.ExecuteDataTable(Query)
            RetVal.Tables.Add(DtTable)
        Catch ex As Exception
        End Try
        Return RetVal
    End Function
#End Region

#Region "Get area names"
    ''' <summary>
    ''' To get Area Name
    ''' </summary>
    ''' <param name="AreaNids">AreaNids comma seperated</param>
    ''' <returns>AreasName comma seperated</returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function GetAreasName(ByVal AreaNids As String) As String
        Dim RetVal As String = String.Empty
        Dim DIConnection As DALConnection.DIConnection = Nothing
        Dim DtTable As DataTable = Nothing
        Dim Seperator As String = ","
        Dim AreaNidList As String() = Nothing
        Dim Query As String = "Select AreaName from Areas where AreaNid = "
        DIConnection = GetDbConnection()
        If (Not (String.IsNullOrEmpty(AreaNids))) Then
            AreaNidList = AreaNids.Split(New String() {Seperator}, StringSplitOptions.None)
            For Each Areanid As String In AreaNidList
                If Not (String.IsNullOrEmpty(Areanid)) Then
                    Query = "Select AreaName from Areas where AreaNid = " + Areanid
                    DtTable = DIConnection.ExecuteDataTable(Query)
                    If (Not (DtTable Is Nothing) And DtTable.Rows.Count > 0) Then
                        RetVal += Seperator + DtTable.Rows(0)(0).ToString()
                    End If
                End If
            Next
            If RetVal.Length > 0 Then
                RetVal = RetVal.Substring(1)
            End If
        End If
        Return RetVal
    End Function
#End Region

#Region "Admin Details"
    ''' <summary>
    ''' To get admin's nid
    ''' </summary>
    ''' <param name="AdapGUID">Adaptation GUID</param>
    ''' <returns>Nid</returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function GetAdminNid(ByVal AdapGUID As String) As String
        Dim Nid As String = String.Empty
        Dim AdapNid = GetAdaptationNid(AdapGUID)
        Dim Query As String = "SELECT NID From Global_UserLogin,ProviderDetails where Global_UserLogin.Nid = ProviderDetails.UserNid AND User_Is_Admin = 'True' AND AdaptationNid =" + AdapNid
        Dim DIConnection As DALConnection.DIConnection = Nothing

        Try

            Dim DTUser As DataTable
            DIConnection = GetDbConnection()
            DTUser = DIConnection.ExecuteDataTable(Query)
            If (Not (DTUser Is Nothing)) And DTUser.Rows.Count > 0 Then
                Nid = DTUser.Rows(0)(0).ToString()
            End If


        Catch ex As Exception

            Nid = "Message: " + ex.Message
            Nid += ", <br /> Stack Trace: " + ex.StackTrace
            Nid += ", <br /> Source: " + ex.Source
            Nid += ", <br /> Help Link: " + ex.HelpLink


        Finally

            If DIConnection IsNot Nothing Then
                DIConnection.Dispose()
            End If


        End Try

        Return Nid
    End Function
#End Region

#Region "Get all Data Providers"
    ''' <summary>
    ''' Get all user information
    ''' </summary>
    ''' <returns>Datatable object</returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function GetAllDataProviders(ByVal AdaptationGUID As String) As DataTable
        Dim DtUsers As DataTable
        Dim AdaptationNid = String.Empty
        Dim DIConnection As DALConnection.DIConnection = Nothing
        Dim Query As String
        Try
            AdaptationNid = GetAdaptationNid(AdaptationGUID)
            DIConnection = GetDbConnection()
            Query = "SELECT DISTINCT NId, User_First_Name, User_Last_Name, User_AreaNid as [User Country], User_Email_Id FROM Global_UserLogin, ProviderDetails where Global_UserLogin.Nid= ProviderDetails.UserNid And AdaptationNid =" + AdaptationNid + " AND User_Is_Provider='True'"
            DtUsers = DIConnection.ExecuteDataTable(Query)
            'Dim DataRows() As DataRow = DtUsers.Select("[Admin User] ='True'")
            'DtUsers.Rows.Remove(DataRows(0))
            DtUsers.TableName = "user"
            'DtUsers.AcceptChanges()
            Return DtUsers
        Catch Ex As Exception
            Throw Ex
        Finally
            If Not (DIConnection Is Nothing) Then
                DIConnection.Dispose()
            End If
        End Try
        Return DtUsers
    End Function
#End Region


#Region "Update Password"
    ''' <summary>
    ''' Update Password
    ''' </summary>    
    ''' <param name="EmailId">Email Id</param>
    ''' <param name="Password">Password</param>        
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function Update_Password(ByVal EmailId As String, ByVal Password As String) As Boolean
        Dim RetVal As Boolean = False
        Dim UpdateQuery As String
        Dim DIConnection As DALConnection.DIConnection
        UpdateQuery = String.Empty
        DIConnection = Nothing
        Try
            DIConnection = GetDbConnection()
            UpdateQuery = "UPDATE Global_UserLogin SET User_Password = '" + Password + "' WHERE User_Email_Id = '" + EmailId + "'"
            DIConnection.ExecuteDataTable(UpdateQuery)
            RetVal = True
        Catch Ex As Exception
            RetVal = False
            Throw Ex
        Finally
            If Not (DIConnection Is Nothing) Then
                DIConnection.Dispose()
            End If
        End Try
        Return RetVal
    End Function
#End Region

#Region "Saving in TokenInformation table"
    <WebMethod()> _
    Public Function Save_In_TokenInformation(ByVal UserNid As String, ByVal IsRegistration As Boolean, ByVal isDataview As Boolean) As String
        Dim Query As String
        Dim DIConnection As DALConnection.DIConnection
        Query = String.Empty
        DIConnection = Nothing
        Dim TokenKey As String = String.Empty
        Dim TimeStamp As String = String.Empty
        Dim TokeInfoTable As DataTable = Nothing
        Try
            DIConnection = GetDbConnection()
            Query = "Select * from TokenInformation where User_Nid = " + UserNid + " and IsRegistration = '" + IsRegistration.ToString() + "' and IsDataviewFlow='" + isDataview.ToString() + "'"
            TokeInfoTable = DIConnection.ExecuteDataTable(Query)
            If (Not (TokeInfoTable Is Nothing) And TokeInfoTable.Rows.Count > 0) Then
                Query = "delete from TokenInformation where User_Nid = " + UserNid + " and IsRegistration = '" + IsRegistration.ToString() + "' and IsDataviewFlow='" + isDataview.ToString() + "'"
                DIConnection.ExecuteDataTable(Query)
            End If
            If (Not (TokeInfoTable Is Nothing)) Then
                TokenKey = Guid.NewGuid().ToString()
                TimeStamp = DateTime.Now.ToString()
                Query = "INSERT INTO TokenInformation (User_Nid, CreatedTime, TokenKey, IsRegistration, IsDataviewFlow) VALUES(" + UserNid + ",'" + TimeStamp + "','" + TokenKey + "','" + IsRegistration.ToString() + "','" + isDataview.ToString() + "')"
                DIConnection.ExecuteDataTable(Query)
            End If
        Catch Ex As Exception
            Throw Ex
        Finally
            If Not (DIConnection Is Nothing) Then
                DIConnection.Dispose()
            End If
        End Try
        Return TokenKey
    End Function
#End Region
#Region "Is Account active"
    <WebMethod()> _
    Public Function IsAccountActive(ByVal UserNid As String) As String
        Dim TokenInfoDT As DataTable = Nothing
        Dim DIConnection As DALConnection.DIConnection
        Dim Query As String
        Dim RetVal As String = String.Empty
        DIConnection = Nothing
        Dim ParamDelimiter As String = "[****]"
        Dim TimeStamp As Date
        Dim TimeSpan As TimeSpan
        Dim ExpireTime As Double = 24

        Try
            DIConnection = GetDbConnection()
            Query = "SELECT * FROM TokenInformation WHERE User_Nid = " + UserNid + " and isDataviewFlow='false'"
            TokenInfoDT = DIConnection.ExecuteDataTable(Query)
            If (Not (TokenInfoDT Is Nothing) And TokenInfoDT.Rows.Count > 0) Then
                Dim TS As String = TokenInfoDT.Rows(0)("createdtime").ToString()
                TimeStamp = Convert.ToDateTime(TS)
                TimeSpan = DateTime.Now.Subtract(TimeStamp)
                If (TimeSpan.TotalHours <= ExpireTime) Then
                    RetVal = "False" + ParamDelimiter + "0"
                Else
                    RetVal = "False" + ParamDelimiter + "2"
                End If
            Else
                RetVal = "True" + ParamDelimiter + "1"
                Query = "SELECT * FROM TokenInformation WHERE User_Nid = " + UserNid + " and isDataviewFlow='true'"
                TokenInfoDT = DIConnection.ExecuteDataTable(Query)
                If (Not (TokenInfoDT Is Nothing) And TokenInfoDT.Rows.Count = 0) Then
                    RetVal = "True" + ParamDelimiter + "1"
                Else
                    Dim TS As String = TokenInfoDT.Rows(0)("createdtime").ToString()
                    TimeStamp = Convert.ToDateTime(TS)
                    TimeSpan = DateTime.Now.Subtract(TimeStamp)
                    If (TimeSpan.TotalHours <= ExpireTime) Then
                        RetVal = "True" + ParamDelimiter + "1"
                    Else
                        RetVal = "False" + ParamDelimiter + "2"
                    End If
                End If
            End If
        Catch Ex As Exception
            Throw Ex
        Finally
            If Not (DIConnection Is Nothing) Then
                DIConnection.Dispose()
            End If
        End Try
        Return RetVal
    End Function
#End Region
    <WebMethod()> _
    Public Function GetDeactivatedAccountInfo(ByVal UserNid As String) As String
        Dim TokenInfoDT As DataTable = Nothing
        Dim DIConnection As DALConnection.DIConnection
        Dim Query As String
        Dim RetVal As String = String.Empty
        DIConnection = Nothing

        Try
            DIConnection = GetDbConnection()
            Query = "SELECT * FROM TokenInformation WHERE User_Nid = " + UserNid.ToString()
            TokenInfoDT = DIConnection.ExecuteDataTable(Query)
            If Not (TokenInfoDT Is Nothing) And TokenInfoDT.Rows.Count = 1 Then
                RetVal = "Your account has not been activated yet."
            ElseIf Not (TokenInfoDT Is Nothing) And TokenInfoDT.Rows.Count = 2 Then
                RetVal = "You have changed your password but it's not activated."
            End If
        Catch Ex As Exception
            Throw Ex
        Finally
            If Not (DIConnection Is Nothing) Then
                DIConnection.Dispose()
            End If
        End Try

        Return RetVal
    End Function

    <WebMethod()> _
    Public Function ActivateAccount(ByVal EmailId As String, ByVal TokenKey As String, ByVal isRegistration As Boolean, ByVal AdapGUID As String) As String
        Dim UserDT As DataTable = Nothing
        Dim TokenInfoDT As DataTable = Nothing
        Dim ParamDelimiter As String = "[****]"
        Dim UserNid As String
        Dim Name As String
        Dim Query As String
        Dim DIConnection As DALConnection.DIConnection = Nothing
        Dim RetVal As String = String.Empty
        Dim TimeSpan As TimeSpan
        Dim TimeStamp As Date
        Dim ExpireTime As Double = 72
        Try
            UserDT = Get_User(EmailId, AdapGUID)
            If Not (UserDT Is Nothing) And UserDT.Rows.Count > 0 Then
                UserNid = UserDT.Rows(0)("nid").ToString()
                Name = UserDT.Rows(0)("user_first_name").ToString()
                DIConnection = GetDbConnection()
                Query = "SELECT * FROM TokenInformation WHERE TokenKey = '" + TokenKey + "' and IsRegistration='" + isRegistration.ToString() + "'"
                TokenInfoDT = DIConnection.ExecuteDataTable(Query)
                If Not (TokenInfoDT Is Nothing) And TokenInfoDT.Rows.Count > 0 Then
                    Dim TS As String = TokenInfoDT.Rows(0)("createdtime").ToString()
                    TimeStamp = Convert.ToDateTime(TS)
                    TimeSpan = DateTime.Now.Subtract(TimeStamp)
                    If (TimeSpan.TotalHours <= ExpireTime) Then
                        RetVal = "false" + ParamDelimiter + "0" + ParamDelimiter + UserNid

                        ' Don't delete token in case of Forgot Password flow.
                        If isRegistration Then
                            ' Remove this token once account is activated 
                            Query = "DELETE FROM TokenInformation WHERE User_Nid = " + UserNid + _
                                "And IsRegistration = '" + isRegistration.ToString() + "'"
                        End If

                        DIConnection.ExecuteDataTable(Query)
                    Else
                        RetVal = "false" + ParamDelimiter + "1"
                    End If
                Else

                    RetVal = "true" + ParamDelimiter + "2"
                End If
            Else
                RetVal = "false" + ParamDelimiter + "3"
            End If
        Catch Ex As Exception
            Throw Ex
        Finally
            If Not (DIConnection Is Nothing) Then
                DIConnection.Dispose()
            End If
        End Try
        Return RetVal
    End Function

    <WebMethod()> _
    Public Function ChangePassword(ByVal UserNid As String, ByVal EncryptedPassword As String, ByVal IsRegistration As Boolean, ByVal AdapGUID As String) As String
        Dim DtUser, DtTokenInfo As DataTable
        Dim ParamDelimiter As String = "[****]"
        Dim DIConnection As DALConnection.DIConnection = Nothing
        Dim Query As String
        Dim RetVal As String
        Try
            DtUser = Get_User(Convert.ToInt32(UserNid), AdapGUID)
            If Not (DtUser Is Nothing) And DtUser.Rows.Count > 0 Then
                DIConnection = GetDbConnection()
                Query = "Update Global_UserLogin set user_password = '" + EncryptedPassword + "' where nid = " + UserNid
                DIConnection.ExecuteDataTable(Query)
                If (String.IsNullOrEmpty(IsRegistration) = False) Then
                    Query = "SELECT * FROM TokenInformation WHERE User_Nid = " + UserNid + "And IsRegistration = '" + IsRegistration.ToString() + "'"
                    DtTokenInfo = DIConnection.ExecuteDataTable(Query)
                    If (Not (DtTokenInfo Is Nothing) And DtTokenInfo.Rows.Count > 0) Then
                        Query = "DELETE FROM TokenInformation WHERE User_Nid = " + UserNid + "And IsRegistration = '" + IsRegistration.ToString() + "'"
                        DIConnection.ExecuteDataTable(Query)
                    End If
                End If
                RetVal = "true" + ParamDelimiter + "1" + ParamDelimiter + DtUser.Rows(0)("nid").ToString() + ParamDelimiter + DtUser.Rows(0)("User_First_Name").ToString() + ParamDelimiter + DtUser.Rows(0)("User_Is_Admin").ToString()
            Else
                RetVal = "false" + ParamDelimiter + "2"
            End If
        Catch Ex As Exception
            Throw Ex
        Finally
            If Not (DIConnection Is Nothing) Then
                DIConnection.Dispose()
            End If
        End Try
        Return RetVal
    End Function

#Region "Get user Nid"
    ''' <summary>
    ''' To get user Nid
    ''' </summary>
    ''' <param name="EmailId">Email Id</param>
    ''' <returns>true[****]userdetails if user is valid otherwise false[****]Error message</returns>
    ''' <remarks>This function is used while retriving the password from database</remarks>
    <WebMethod()> _
    Public Function Get_UserNid(ByVal EmailId As String, ByVal AdapGUID As String) As String
        Dim RetVal As String = String.Empty
        Dim UserNid As String
        Dim DtUser As DataTable
        Try
            DtUser = Get_User(EmailId, AdapGUID)
            If (Not (DtUser Is Nothing) And DtUser.Rows.Count > 0) Then

                Dim AdminUserRows() As DataRow = DtUser.Select("User_Is_Admin='True'")

                ' Check if any of selected users is admin for given adaptation, 
                ' if yes then return UserNId of that Admin user only
                ' otherwise, return first user nid from selected users (default case)

                If AdminUserRows.Length > 0 Then
                    UserNid = AdminUserRows(0)("Nid").ToString()
                Else
                    UserNid = DtUser.Rows(0)("Nid").ToString()
                End If

                RetVal = UserNid
            End If
        Catch Ex As Exception
        End Try
        Return RetVal
    End Function
#End Region
    <WebMethod()> _
    Public Function IsMatchOldPassword(ByVal UserNid As String, ByVal EncrytOldPwd As String, ByVal AdapGUID As String) As Boolean
        Dim DtUser As DataTable
        Dim RetVal As Boolean = False
        DtUser = Get_User(Convert.ToInt32(UserNid), AdapGUID)
        If (Not (DtUser Is Nothing) And DtUser.Rows.Count > 0) Then
            If (EncrytOldPwd.Equals(DtUser.Rows(0)("User_Password").ToString())) Then
                RetVal = True
            End If
        End If
        Return RetVal
    End Function

#Region "Verify Master Account"
    <WebMethod()> _
    Public Function GetMasterAccountEmailPassword() As DataSet
        Dim RetVal As New DataSet()
        Dim DtTable As DataTable = Nothing

        Dim DIConnection As DALConnection.DIConnection = Nothing
        Dim Query As String
        Try
            DIConnection = GetDbConnection()
            Query = "SELECT User_Email_Id,User_Password From Global_UserLogin WHERE [IsMasterAccount] = 'True'"
            DtTable = DIConnection.ExecuteDataTable(Query)
            RetVal.Tables.Add(DtTable)
        Catch ex As Exception
        End Try
        Return RetVal
    End Function
#End Region
#End Region


End Class