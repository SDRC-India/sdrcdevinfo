Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Public Class Queries

#Region "Private Variables"
    Private dt As DataTable
    Private ds As DataSet
    Private con As New DIConnection
#End Region

#Region "Queries"


#Region "--Methods For Queries--"


#Region "--Select Query--"

    Public Shared Function GetRegionIdFromName(ByVal countryName As String) As String
        Dim RetVal As String = String.Empty
        RetVal = "Select Country_NId from mCountry where Country_Name='" & countryName & "' "
        Return RetVal
    End Function

   
    Public Shared Function GetQueryforSupportGroupForUser() As String
        Dim RetVal As String = String.Empty
        RetVal = ((("Select " & Constants.SupportGroupId & " from ") + Constants.SupportGroups & " where ") + Constants.SupportName & "='") + Constants.SupportMember & "'"
        Return RetVal
    End Function

    Public Shared Function GetQueryforUserIdBasisOnUserName(ByVal name As String) As String
        Dim RetVal As String = String.Empty
        RetVal = ((("Select " & Constants.SupportUserId & " from ") + Constants.User & " where ") + Constants.SupportName & "='") + name & "'"
        Return RetVal
    End Function

    Public Shared Function GetQueryForForumGroupOfUser() As String
        Dim RetVal As String = String.Empty
        RetVal = ("Select " & Constants.ForumUserGroupId & " from ") + Constants.ForumUserGroup
        RetVal = ((RetVal & " where ") + Constants.ForumTitle & "='") + Constants.ForumRegistrationTitle & "'"
        Return RetVal
    End Function


    Public Shared Function GetQueryForTitleAndTitleIdOfForumUser() As String
        Dim RetVal As String = String.Empty
        RetVal = (("Select " & Constants.ForumUserTitleId & " , ") + Constants.ForumTitle & " from ") + Constants.ForumUserTitle
        RetVal = ((RetVal & " where ") + Constants.ForumMinPosts & "='") + 0 & "'"
        Return RetVal
    End Function

#Region "--User Validation--"
    Public Shared Function GetQueryforCheckDesktopUser(ByVal emailId As String) As String
        Dim RetVal As String = String.Empty
        RetVal = (("Select count(*) from " & Constants.mUserInfo & " where ") + Constants.Email & "='") + emailId & "'"

        Return RetVal
    End Function

    Public Shared Function GetQueryForForumUserValidation(ByVal email As String) As String
        Dim RetVal As String = String.Empty
        RetVal = "Select count(*) from " & Constants.User
        RetVal = ((RetVal & " where ") + Constants.ForumEmail & "='") + email & "'"

        Return RetVal
    End Function

    Public Shared Function GetQueryForSupportValidation(ByVal email As String) As String
        Dim RetVal As String = String.Empty
        RetVal = "Select count(*) from " & Constants.User
        RetVal = ((RetVal & " where ") + Constants.SupportName & "='") + email & "'"
        Return RetVal
    End Function

    Public Shared Function GetQueryForWIKIUserValidation(ByVal email As String) As String
        Dim RetVal As String = String.Empty
        RetVal = "Select count(*) from " & Constants.User
        RetVal = ((RetVal & " where ") + Constants.SupportName & "='") + email & "'"
        Return RetVal
    End Function


#End Region
#End Region

#Region "--Insert/Update Query--"
    Public Shared Function GetQueryForInsertUserInDesktop(ByVal Firstname As String, ByVal LastName As String, ByVal Login As String, ByVal Email As String, ByVal ContactDetails As String, ByVal GUID As String, _
    ByVal Password As String, ByVal Organization As String, ByVal Country_NId As String) As String
        Dim RetVal As String = String.Empty
        RetVal = ((((((((("Insert into " & Constants.mUserInfo & " (") + Constants.Firstname & ", ") + Constants.LastName & ", ") + Constants.Login & ", ") + Constants.Email & ", ") + Constants.ContactDetails & " , ") + Constants.GUID & " ,") + Constants.Password & " , ") + Constants.Organization & " ,") + Constants.CountryNId
        RetVal = (((((((((RetVal & " ) values ('") + Firstname & "' ,'") + LastName & "','") + Login & "','") + Email & "','") + ContactDetails & "','") + GUID & "','") + Password & "','") + Organization & "','") + Country_NId & "')"

        Return RetVal
    End Function

    Public Shared Function GetQueryForInsertUserInSupport(ByVal userName As String, ByVal email As String, ByVal encryptedPassword As String) As String
        Dim RetVal As String = String.Empty
        RetVal = "Insert into " & Constants.User
        RetVal = ((RetVal & " (") + Constants.SupportName & ", ") + Constants.SupportPassword
        RetVal = ((RetVal & " ) values ('") + email & "','") + encryptedPassword & "')"
        Return RetVal
    End Function

    Public Shared Function GetQueryForInsertUserInForum(ByVal userName As String, ByVal email As String, ByVal encryptedPassword As String, ByVal salt As String) As String
        Dim RetVal As String = String.Empty
        RetVal = "Insert into " & Constants.User
        RetVal = ((((RetVal & " (") + Constants.ForumUserName & ", ") + Constants.ForumEmail & ", ") + Constants.ForumPassword & " ,") + Constants.ForumSalt


        RetVal = ((((RetVal & " ) values ('") + userName & "','") + email & "','") + encryptedPassword & "','") + salt & "')"

        Return RetVal
    End Function

    Public Shared Function GetQueryForUpdateUserInForum(ByVal ForumUserGroupId As String, ByVal ForumShowVbCode As String, ByVal ForumShowBirthday As String, ByVal ForumUserTitle As String, ByVal userName As String) As String
        Dim RetVal As String = String.Empty
        RetVal = "Update " & Constants.User
        RetVal = ((RetVal & " Set ") + Constants.ForumUserGroupId & "=") + ForumUserGroupId
        ' RetVal = RetVal + ", " + Constants.ForumDisplayGroupId + "=" + ForumDisplayGroupId; 
        RetVal = (RetVal & " , ") + Constants.ForumPasswordDate & "= DATE(CURRENT_TIMESTAMP) "
        ' +ForumPasswordDate; 
        ' RetVal = RetVal + " ," + Constants.ForumStyleId + "=" + ForumStyleId; 
        RetVal = ((RetVal & " , ") + Constants.ForumShowVbCode & "=") + ForumShowVbCode
        RetVal = ((RetVal & " , ") + Constants.ForumShowBirthday & "=") + ForumShowBirthday
        RetVal = ((RetVal & " , ") + Constants.ForumUserTitle & "='") + ForumUserTitle & "'"
        RetVal = (RetVal & " , ") + Constants.ForumJoinDate & "= UNIX_TIMESTAMP(NOW()) "
        ' +ForumJoinDate; 
        RetVal = (RetVal & " , ") + Constants.ForumLastActvity & "= UNIX_TIMESTAMP(NOW()) "
        ' +ForumLastActvity; 
        RetVal = (RetVal & " , ") + Constants.ForumLastVisit & "= UNIX_TIMESTAMP(NOW()) "
        ' +ForumLastVisit; 
        RetVal = ((RetVal & " where ") + Constants.ForumUserName & "='") + userName & "'"

        Return RetVal
    End Function

    Public Shared Function GetQueryForInsertUserInWiki(ByVal userName As String, ByVal email As String, ByVal encryptedPassword As String) As String
        Dim RetVal As String = String.Empty
        RetVal = "Insert into " & Constants.User
        RetVal = (((RetVal & " (") + Constants.WikiUserName & ", ") + Constants.WikiUserEmail & ", ") + Constants.WikiUserPassword
        RetVal = (((RetVal & " ) values ('") + userName & "','") + email & "','") + encryptedPassword & "')"
        Return RetVal
    End Function

    Public Shared Function GetQueryforInsertSupportTicket(ByVal userId As String, ByVal added As String, ByVal subject As String, ByVal body As String) As String
        Dim RetVal As String = String.Empty
        RetVal = "Insert into " & Constants.SupportTickets
        RetVal = ((((RetVal & " (") + Constants.SupportUserId & ",") + Constants.SupportAdded & ", ") + Constants.SupportSubject & ", ") + Constants.SupportBody
        RetVal = ((((RetVal & " ) values ('") + userId & "','") + added & "','") + subject & "','") + body & "')"
        Return RetVal
    End Function

    Public Shared Function GetQueryforInsertSupportUserInGroup(ByVal userId As String, ByVal groupId As String) As String
        Dim RetVal As String = String.Empty
        RetVal = "Insert into " & Constants.SupportUserGroup
        RetVal = ((RetVal & " (") + Constants.SupportUserId & ", ") + Constants.SupportGroupId
        RetVal = ((RetVal & " ) values ('") + userId & "','") + groupId & "')"
        Return RetVal
    End Function


    ''' <summary> 
    ''' Query for Updatting FactInfo based on countryName 
    ''' </summary> 
    ''' <param name="countryName"></param> 
    ''' <param name="FocalPoint"></param> 
    ''' <param name="FocalPointEmail"></param> 
    ''' <param name="ImplementingAgency"></param> 
    ''' <param name="HomepageURL"></param> 
    ''' <param name="Databases"></param> 
    ''' <param name="Website"></param> 
    ''' <param name="LastUpdated"></param> 
    ''' <returns></returns> 
    Public Shared Function GetQueryforUpdateCountryFactInfo(ByVal countryName As String, ByVal FocalPoint As String, ByVal FocalPointEmail As String, ByVal ImplementingAgency As String, ByVal HomepageURL As String, ByVal Databases As String, _
    ByVal Website As String, ByVal LastUpdated As String) As String
        Dim RetVal As String = String.Empty

        RetVal = (("Update " & Constants.rCountryAdaptation & " Set ") + Constants.FocalPoints & "='") + FocalPoint & "' , "
        RetVal += (Constants.FocalPointEmails & "='") + FocalPointEmail & "' , "
        RetVal += (Constants.ImplementingAgency & "='") + ImplementingAgency & "' , "
        RetVal += Constants.HomepageURL + "='" + HomepageURL + "' , "
        RetVal += (Constants.Databases & "='") + Databases & "' , "
        RetVal += (Constants.Website & "='") + Website & "' , "
        RetVal += (Constants.LastUpdated & "='") + LastUpdated & "' "
        RetVal += ((" Where " & Constants.CountryNId & " in (Select ") + Constants.CountryNId & " from ") + Constants.mCountry
        RetVal += (" Where " & Constants.CountryName & "='") + countryName & "')"

        Return RetVal
    End Function

    ''' <summary>
    ''' Update FactsheetURL
    ''' </summary>
    ''' <param name="countryName"></param>
    ''' <param name="countryFactSheetURL"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetQueryforUpdateFactSheet(countryName,countryFactSheetURL ) As String
        Dim RetVal As String = String.Empty

        RetVal = "Update " & Constants.mCountry & " Set " & Constants.countryFactSheetURL & "='" & countryFactSheetURL & "'  "
        RetVal += " Where " & Constants.CountryName & "='" & countryName & "'"

        Return RetVal
    End Function

    ''' <summary> 
    ''' 
    ''' </summary> 
    ''' <param name="countryName"></param> 
    ''' <returns></returns> 
    Public Shared Function GetQueryForIsValidCountry(ByVal countryName As String) As String
        Dim RetVal As String = String.Empty

        RetVal = (("Select count(*) from " & Constants.mCountry & " where ") + Constants.CountryName & "='") + countryName & "'"


        Return RetVal
    End Function


#End Region

#End Region

    'Function To Get Adaptations Information
    Public Function GenerateKMLForAdaptations() As DataTable
        Try
            Dim dr As SqlDataReader
            Dim objdt As New DataTable
            objdt.TableName = "m_Success"
            dr = con.ReaderData("Exec dw_Adaptations")
            objdt.Load(dr)
            Return objdt
        Catch ex As Exception
            Dim dt As DataTable = Returndt()
            Return dt
        End Try
    End Function

    'Function To Get Registered Users Information
    Public Function GenerateKMLForRegisteredUsers() As DataTable
        Try
            Dim dr As SqlDataReader
            Dim objdt As New DataTable
            objdt.TableName = "m_Success"
            dr = con.ReaderData("Exec dw_Activations")
            objdt.Load(dr)
            Return objdt
        Catch ex As Exception
            Dim dt As DataTable = Returndt()
            Return dt
        End Try
    End Function

    'Function To Get OnlineUsers Information
    Public Function GenerateKMLForOnlineUsers() As DataTable
        Try
            Dim dr As SqlDataReader
            Dim objdt As New DataTable
            objdt.TableName = "m_Success"
            dr = con.ReaderData("Exec dw_OnlineUsers")
            objdt.Load(dr)
            Return objdt
        Catch ex As Exception
            Dim dt As DataTable = Returndt()
            Return dt
        End Try
    End Function

    'Function To Get Countryid And RegionID. INPUT--- CountryName , RegionName
    Public Function GetCounrtyAndRegionId(ByVal Country As String) As DataTable
        Try
            Dim Query As String = String.Empty
            'Query = "Select Country_NId,(Select Region_NId from mRegion where Region_Name='" & Region & "') as Region_NId from mCountry where Country_Name='" & Country & "' "
            Query = "Select Country_NId from mCountry where Country_Name='" & Country & "' "
            dt = con.FillDataTable(Query)
            Return dt
        Catch ex As Exception
            Throw New System.Exception("Error Source: Queries " + ex.Message)

        End Try
    End Function

    'Function To Set User Details into User Master Table
    Public Function SetUserDetails(ByVal Firstname As String, ByVal LastName As String, ByVal Login As String, ByVal Email As String, ByVal ContactDetails As String, ByVal GUID As String, ByVal Password As String, ByVal Organization As String, ByVal Country_NId As Integer) As Boolean
        Try
            Dim Query As String = String.Empty
            Query = "Insert into mUserInfo (Firstname,LastName,Login,Email,ContactDetails,GUID,Password,Organization,Country_NId) values ('" & Firstname & "' ,'" & LastName & "','" & Login & "','" & Email & "','" & ContactDetails & "','" & GUID & "','" & Password & "','" & Organization & "','" & Country_NId & "')"
            con.Insert(Query)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    'Function To Get UserId. INPUT--  WorldwideUserGId,Adaptation_Name,Adaptation_Version
    Public Function GetUserNId(ByVal WorldwideUserGId As String, ByVal Adaptation_Name As String, ByVal Adaptation_Ver As String) As DataTable
        Try
            Dim Query As String = String.Empty
            Query = "Select mUserInfo.User_NId,(Select Adaptation_NId from mAdaptation where Adaptation_Name='" & Adaptation_Name & "' AND Adaptation_Ver='" & Adaptation_Ver & "') as Adaptation_NId from mUserInfo where GUID='" & WorldwideUserGId & "'"
            'dt = con.FillDataTable("Exec strpc_GetUser_AdapIds '" & WorldwideUserGId & "','" & Adaptation_Name & "'")
            dt = con.FillDataTable(Query)
            Return dt
        Catch ex As Exception
            Throw New System.Exception("Error Source: Queries " + ex.Message)
        End Try
    End Function

    'Function To Set User Adaptation
    Public Function SetUserAdaptation(ByVal User_NId, ByVal Adaptation_NId) As Boolean
        Try
            Dim Query As String = String.Empty
            Query = "Insert into rUserAdaptation(User_Nid,Adaptation_NId) values ('" & User_NId & "','" & Adaptation_NId & "')"
            con.Insert(Query)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    'Function To Update User Status(Online or Offline)
    Public Function SetUserStatus(ByVal Online As Boolean, ByVal WorldwideUserGId As String) As Boolean
        Try
            If Online = True Then
                Dim Query As String = String.Empty
                Query = "Update mUserInfo set IsOnline='" & IIf(Online, "1", "0") & "',LastLoggedIn='" & Now & "' where GUID='" & WorldwideUserGId & "'"
                con.Insert(Query)
                Return True
            ElseIf Online = False Then
                Dim Query As String = String.Empty
                Query = "Update mUserInfo set IsOnline='" & IIf(Online, "1", "0") & "' where GUID='" & WorldwideUserGId & "'"
                con.Insert(Query)
                Return True
            End If

        Catch ex As Exception
            Return False
        End Try
    End Function

    'Function To Get User Status(Online or Offline)
    Public Function GetUserStatus(ByVal WorldwideUserGId As String) As DataTable
        Try
            Dim Query As String = String.Empty
            Query = "Select Isonline from mUserInfo where GUID='" & WorldwideUserGId & "'"
            dt = con.FillDataTable(Query)
            Return dt
        Catch ex As Exception
            Throw New System.Exception("Error Source: Queries " + ex.Message)
            'Dim dt As DataTable = Returndt()
            'Return dt
        End Try
    End Function

    'Function To Get Country Lists
    Public Function GetCountryLists() As DataSet
        Try
            Dim Query As String = String.Empty
            Query = "Select Country_ID,Country_Name from mCountry"
            ds = con.FillDataSet(Query)
            Return ds
        Catch ex As Exception
            Return GetErrorDataSet()
        End Try
    End Function

    'Function To Get RegionLists
    Public Function GetRegionLists() As DataSet
        Try
            Dim Query As String = String.Empty
            Query = "Select Region_ID,Region_Name from mRegion"
            ds = con.FillDataSet(Query)
            Return ds
        Catch ex As Exception
            Return GetErrorDataSet()
        End Try
    End Function

    'Function To Set New Country 
    Public Function CreateNewCountry(ByVal Country_Name, ByVal Country_ID, ByVal Longitude, ByVal Latitude, ByVal CountryLogo_URL, ByVal CountryFactSheet_URL) As Boolean
        Try
            Dim Query As String = String.Empty
            Query = "Insert Into mCountry (Country_Name,Country_ID,Longitude,Latitude,CountryFlag_URL,CountryFactSheet_URL) values('" & Country_Name & "','" & Country_ID & "','" & Longitude & "','" & Latitude & "','" & CountryLogo_URL & "','" & CountryFactSheet_URL & "')"
            con.Insert(Query)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    'Function To Set New Adaptation 
    Public Function CreateNewAdaptation(ByVal Adaptation_Name As String, ByVal Adaptation_Ver As String) As Boolean
        Try
            Dim Query As String = String.Empty
            Query = "Insert Into mAdaptation (Adaptation_Name,Adaptation_Ver) values('" & Adaptation_Name & "','" & Adaptation_Ver & "')"
            con.Insert(Query)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    'Function To Check Email Aready Exists
    Public Function CheckEmailExistence(ByVal Email As String) As SqlDataReader
        Dim dr As SqlDataReader
        Try
            Dim Query As String = String.Empty

            Query = "Select GUID from mUserInfo where Email='" & Email & "'"
            dr = con.ReaderData(Query)
        Catch ex As Exception
            Throw New System.Exception("Error Source: Queries " + ex.Message)
        End Try
        Return dr
    End Function

    Public Function GetAdaptationID(ByVal AdaptationName As String, ByVal AdaptationVersion As String) As DataTable

        Try
            Dim Query As String = String.Empty
            Query = "Select Adaptation_Nid from mAdaptation where Adaptation_Name='" & AdaptationName & "' and Adaptation_Ver='" & AdaptationVersion & "'"
            dt = con.FillDataTable(Query)
            Return dt
        Catch ex As Exception
            Throw New System.Exception("Error Source: Queries " + ex.Message)
        End Try
    End Function

    'Function To Set New Adaptation 
    Public Function CreateCountryAdaptation(ByVal CountryID As Integer, ByVal AdaptationID As Integer, ByVal FocalPoint As String, ByVal FocalPointEmail As String, ByVal ImplementingAgency As String, ByVal HomepageURL As String, ByVal Databases As String, ByVal Website As String, ByVal LastUpdated As String) As Boolean
        Try
            Dim Query As String = String.Empty
            Query = "Insert Into rCountryAdaptation (Country_Nid,Adaptation_Nid,FocalPoints,FocalPoint_Emails,Implementing_Agency,Homepage_URL,Databases,Website,LastUpdated) values('" & CountryID & "','" & AdaptationID & "','" & FocalPoint & "','" & FocalPointEmail & "','" & ImplementingAgency & "','" & HomepageURL & "','" & Databases & "','" & Website & "','" & LastUpdated & "')"
            con.Insert(Query)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    'Function To Update the User Information
    Public Function UpdateUserInformation(ByVal GUID As String, ByVal FirstName As String, ByVal LastName As String, ByVal Email As String, ByVal Organization As String, ByVal CountryId As String) As Boolean
        Try
            Dim Query As String = String.Empty
            Query = "Update muserinfo Set Firstname='" & FirstName & "',Lastname='" & LastName & "',Email='" & Email & "',Organization='" & Organization & "',Country_NID='" & CountryId & "' where GUID='" & GUID & "'"
            con.Insert(Query)
            Return True

        Catch ex As Exception
            Return False
        End Try
    End Function

    'Function To retrieve User Information
    Public Function GetUserInfo(ByVal GUID As String) As DataSet
        Try
            Dim Query As String = String.Empty
            Query = "Select A.FirstName,A.LastName,A.Email,A.Organization,B.Country_Name from mUserInfo A"
            Query += " inner join mCountry B on A.Country_Nid=B.Country_Nid where GUID='" & GUID & "'"
            ds = con.FillDataSet(Query)
            Return ds
        Catch ex As Exception
            Throw New System.Exception("Error Source: Queries " + ex.Message)
        End Try
    End Function

    'Function To Update user Information
    Public Function UpdateUserOnlinestatus()
        Try
            Dim Query As String = String.Empty
            Query = "Update mUserInfo Set IsOnline='0' where User_NId in(Select User_NId from muserinfo where DATEDIFF(Minute, LastLoggedIn, Getdate()) > 480)"
            con.Insert(Query)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    'Function For Checking that UserNid and AdaptationNid Already exists In Relationship table
    Public Function CheckUserNidAndAdaptationNid(ByVal User_NId As Integer, ByVal Adaptation_NId As Integer) As SqlDataReader
        Try
            Dim Query As String = String.Empty
            Dim dr As SqlDataReader
            Query = "Select * from ruseradaptation where User_NId='" & User_NId & "' and Adaptation_NId='" & Adaptation_NId & "'"
            dr = con.ReaderData(Query)
            Return dr
        Catch ex As Exception
            Throw New System.Exception("Error Source: Queries " + ex.Message)
        End Try
    End Function

    'Function to return DataTable If Error Occurs
    Private Function Returndt()
        Dim DS As New DataSet
        Dim DT As DataTable
        Dim DR1 As DataRow
        DT = New DataTable("ExceptionTable")
        Dim DC As DataColumn
        DC = New DataColumn("Values")
        DC.DataType = System.Type.GetType("System.Boolean")
        DT.Columns.Add(DC)
        DR1 = DT.NewRow()
        DR1.Item("Values") = False
        DT.Rows.Add(DR1)
        Return DT
    End Function

    'Function To Return Dataset If Error Occurs
    Private Function GetErrorDataSet() As DataSet
        Try
            Dim oDT As New DataTable("Errors")
            oDT.Columns.Add("Message", System.Type.GetType("System.Boolean"))
            Dim oDR As DataRow = oDT.NewRow()
            oDR("Message") = False
            oDT.Rows.Add(oDR)
            Dim oDS As New DataSet("Error")
            oDS.Tables.Add(oDT)
            Return oDS
        Catch
            Return Nothing
        End Try

    End Function

    'Public Function GetAdaptationCounts() As DataSet
    '    Try
    '        Dim Query As String = String.Empty
    '        Query = "Select count(*) as AdapationCount from rCountryAdaptation A inner join mCountry B on A.Country_NId=B.Country_Nid inner join mAdaptation C on C.Adaptation_NId=A.Adaptation_NId"
    '        ds = con.FillDataSet(Query)
    '        Return ds
    '    Catch ex As Exception
    '        Throw New System.Exception("Error Source: Queries " + ex.Message)
    '    End Try
    'End Function
    'Public Function GetOnlineUserCounts() As DataSet

    '    Try
    '        Dim Query As String = String.Empty
    '        Query = "Select count(*) as OnlineUserCount from mUserInfo inner join mCountry on mCountry.Country_Nid=mUserInfo.Country_NId where IsOnline=1"
    '        ds = con.FillDataSet(Query)
    '        Return ds
    '    Catch ex As Exception
    '        Throw New System.Exception("Error Source: Queries " + ex.Message)
    '    End Try
    'End Function
    'Public Function GetActivationCounts() As DataSet
    '    Try
    '        Dim Query As String = String.Empty
    '        Query = "select count(*) as ActivationCount from mUserInfo inner join mCountry on mCountry.Country_NId=mUserInfo.Country_NId inner join ruseradaptation  on ruseradaptation.User_Nid=mUserInfo.User_Nid  inner join mAdaptation  on ruseradaptation.Adaptation_Nid=mAdaptation.Adaptation_Nid"
    '        ds = con.FillDataSet(Query)
    '        Return ds
    '    Catch ex As Exception
    '        Throw New System.Exception("Error Source: Queries " + ex.Message)
    '    End Try
    'End Function


#End Region
End Class
