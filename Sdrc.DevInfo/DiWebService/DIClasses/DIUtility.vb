Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports DevInfo.Lib.DI_LibDAL.Connection
Imports System.Globalization
Imports System.Net.Mail
Imports System.IO

''' <summary> 
''' Summary description for Utility 
''' </summary> 
Public Class DIUtility

#Region "--Public--"

#Region "--New/Dispose--"

    Public Sub New()
        ' 
        ' TODO: Add constructor logic here 
        ' 


    End Sub
#End Region

#Region "--Methods--"




    ''' <summary> 
    ''' Get RegionId Basis on CountryName 
    ''' </summary> 
    ''' <param name="countryName"></param> 
    ''' <returns></returns> 
    Public Function GetRegionIdByCountryName(ByVal countryName As String) As String
        Dim RetVal As String = String.Empty
        Dim Query As String = String.Empty
        Dim RegionTable As DataTable = Nothing
        Try

            Query = Queries.GetRegionIdFromName(countryName)
            RegionTable = DBConn.ExecuteDataTable(Query)

            If RegionTable.Rows.Count <> 0 Then
                For Each Row As DataRow In RegionTable.Rows

                    RetVal = Row(0).ToString()

                Next



            End If
        Catch ex As Exception

            Throw New System.Exception("Error Source: Queries " & ex.Message)
        End Try


        Return RetVal
    End Function




    ''' <summary> 
    ''' Get Encrypted Password basis on type and password 
    ''' </summary> 
    ''' <param name="type"></param> 
    ''' <param name="password"></param> 
    ''' <returns></returns> 
    Public Function EncryptionOfPassword(ByVal type As String, ByVal password As String) As String
        Dim RetVal As String = String.Empty

        Select Case type

            Case Constants.DIDESKTOP

                'Generate password 
                If password = String.Empty Then
                    Me.Password = Me.RandomNumberGenerator(6)
                End If
                'Generate UserName 
                If UserName = String.Empty Then
                    Me.UserName = (Me.FirstName & "_") + Me.LastName



                End If
                RetVal = password

                Exit Select
            Case Constants.DIFORUM
                RetVal = Me.PasswordGeneratorBasedOnMD5Format(password)
                Exit Select
            Case Constants.DISUPPORT
                RetVal = password
                Exit Select
            Case Constants.DIWIKI
                RetVal = Me.PasswordGenerateForWiki(password)
                Exit Select

        End Select


        Return RetVal
    End Function


    ''' <summary> 
    ''' Register User For All types 
    ''' </summary> 
    ''' <param name="userName"></param> 
    ''' <param name="password"></param> 
    ''' <param name="email"></param> 
    ''' <param name="organization"></param> 
    ''' <param name="country"></param> 
    ''' <param name="firstName"></param> 
    ''' <param name="lastName"></param> 
    ''' <param name="contactDetails"></param> 
    ''' <returns></returns> 
    Public Function RegisterUser(ByVal userName As String, ByVal password As String, ByVal email As String, ByVal organization As String, ByVal country As String, ByVal firstName As String, _
    ByVal lastName As String, ByVal contactDetails As String, ByVal type As String) As Boolean
        Dim RetVal As Boolean = True

        Dim ProjectTypes As String() = Me.GetProjectTypes()
        Dim ValuesForSubject As String = String.Empty

        'Set Fields For Registration 
        Me.SetFieldForUser(userName, password, email, organization, country, firstName, _
        lastName, contactDetails)

        'Register User 
        ' RegisterUser(Constants.DIDESKTOP); 
        ' RegisterUser(Constants.DISUPPORT); 
        ' RegisterUser(Constants.DIFORUM); 
        ' RegisterUser(Constants.DIWIKI); 

        For count As Integer = 0 To ProjectTypes.Length - 1
            If ProjectTypes(count).ToUpper() <> type.ToUpper() Then
                RegisterUser(ProjectTypes(count))
                ValuesForSubject += ProjectTypes(count).ToString() & ","

            End If
        Next

        Me.GenerateMailSubject(ValuesForSubject)


        ' this.SendMail(this.Email, string.Empty, this.MailSubject, this.MailBody); 


        Return RetVal
    End Function

    ''' <summary> 
    ''' Update CountryFactInfo 
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
    Public Function UpdateCountryFactInfo(ByVal countryName As String, ByVal FocalPoint As String, ByVal FocalPointEmail As String, ByVal ImplementingAgency As String, ByVal HomepageURL As String, ByVal Databases As String, _
    ByVal Website As String, ByVal LastUpdated As String, ByVal countryFactSheetURL As String) As Boolean

        Dim RetVal As Boolean = False
        Dim Query As String = String.Empty
        Dim query1 As String = String.Empty
        Dim query2 As String = String.Empty
        'string LastUpdated = System.DateTime.Now.ToShortDateString(); 

        Try
            countryName = Me.ReplaceCharacter(countryName)
            FocalPoint = Me.ReplaceCharacter(FocalPoint)
            FocalPointEmail = Me.ReplaceCharacter(FocalPointEmail)
            ImplementingAgency = Me.ReplaceCharacter(ImplementingAgency)
            Databases = Me.ReplaceCharacter(Databases)
            Website = Me.ReplaceCharacter(Website)
            LastUpdated = Me.ReplaceCharacter(LastUpdated)
            countryFactSheetURL = Me.ReplaceCharacter(countryFactSheetURL)
            HomepageURL = Me.ReplaceCharacter(HomepageURL)





            'Create Log file 
            Me.CreateLogFile(countryName, FocalPoint, FocalPointEmail, ImplementingAgency, HomepageURL, Databases, Website, LastUpdated, countryFactSheetURL, False, Query)

            'If Website is not available 
            If Convert.ToString(Website) = String.Empty Then
                Website = "Not Yet Available"
            End If


            'If Website is not available 
            If Convert.ToString(HomepageURL) = String.Empty Then
                HomepageURL = "Not Yet Available"
            End If

            'Create Connection 
            Me.ConnectionOpen(Constants.DIDESKTOP)

            Query = Queries.GetQueryForIsValidCountry(countryName)

            query1 = Query

            'Update FactInfo 
            Dim dt As DataTable = Me.DBConn.ExecuteDataTable(Query)

            If dt.Rows.Count <> 0 Then
                'if (Convert.ToString(HomepageURL) = string.Empty) 
                '{ 

                '} 
                'Get Query for Update factINfo 
                Query = Queries.GetQueryforUpdateCountryFactInfo(countryName, FocalPoint, FocalPointEmail, ImplementingAgency, HomepageURL, Databases, _
                Website, LastUpdated)

                query2 = Query
                'Update FactInfo 
                Me.DBConn.ExecuteNonQuery(Query)

                'Get Query for Update FactSheetURL
                Query = Queries.GetQueryforUpdateFactSheet(countryName, countryFactSheetURL)

                'Update FactInfo 
                Me.DBConn.ExecuteNonQuery(Query)

                RetVal = True

            End If
        Catch generatedExceptionName As Exception
            RetVal = False
            'Create Log file 
            Me.CreateLogFile(countryName, generatedExceptionName.Message, query1, query2, HomepageURL, Databases, Website, LastUpdated, countryFactSheetURL, RetVal, Query)


        End Try
        Me.CreateLogFile(countryName, String.Empty, query1, query2, HomepageURL, Databases, Website, LastUpdated, countryFactSheetURL, RetVal, Query)

       
        Return RetVal
    End Function




#End Region

#End Region

#Region "--Private--"

#Region "--Variables--"
    Private DBConn As DevInfo.Lib.DI_LibDAL.Connection.DIConnection = Nothing
    Private ConnectionString As String = String.Empty
    Private ServerType As DIServerType
    Private DBQueries As Queries = Nothing



    Private Password As String = String.Empty
    Private UserName As String = String.Empty
    Private Email As String = String.Empty
    Private Organization As String = String.Empty
    Private Country As String = String.Empty
    Private LastName As String = String.Empty
    Private FirstName As String = String.Empty
    Private ContactDetails As String = String.Empty
    Private Salt As String = String.Empty

    Private EncryptedPassword As String = String.Empty

    Private Subject As String = String.Empty
    Private Body As String = String.Empty

    Private MailSubject As String = String.Empty
    Private MailBody As String = String.Empty

    Private ForumUserGroupId As Integer = 2
    ' private int ForumDisplayGroupId = 0; 
    ' private DateTime ForumPasswordDate = System.DateTime.Now; 
    ' private int ForumStyleId = 0; 
    ' private int ForumShowVbCode = 1; 
    ' private int ForumShowBirthday = 0; 
    ' private string ForumUserTitle = "Junior Member"; 
    ' private int ForumCustomTitle = 0; 
    ' private int ForumReputation = 10; 
    ' private int ForumReputationLevelId = 5; 
    ' private int ForumOptions = 11537495; 

#End Region

#Region "--Methods--"

    ''' <summary>
    ''' Function To Replace Single Quotes With Double Quotes
    ''' </summary>
    ''' <param name="Str"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ReplaceCharacter(ByVal Str As String)
        Dim _Str As String = Str.Replace("'", "''")
        Return _Str
    End Function

    ''' <summary> 
    ''' Validate User For regsitration 
    ''' </summary> 
    ''' <param name="type"></param> 
    ''' <param name="email"></param> 
    ''' <returns></returns> 
    Private Function ValidateUser(ByVal type As String, ByVal email As String) As Boolean
        Dim Query As String = String.Empty
        Dim Record As DataTable = Nothing
        Dim RetVal As Boolean = False

        Select Case type

            Case Constants.DIDESKTOP

                Query = Queries.GetQueryforCheckDesktopUser(email)
                Record = DBConn.ExecuteDataTable(Query)
                If Record.Rows.Count <> 0 Then
                    RetVal = True
                End If

                Exit Select

            Case Constants.DIFORUM
                Query = Queries.GetQueryForForumUserValidation(email)
                Record = DBConn.ExecuteDataTable(Query)
                If Record.Rows.Count <> 0 Then
                    RetVal = True
                End If

                Exit Select

            Case Constants.DISUPPORT
                Query = Queries.GetQueryForSupportValidation(email)
                Record = DBConn.ExecuteDataTable(Query)
                If Record.Rows.Count <> 0 Then
                    RetVal = True
                End If

                Exit Select

            Case Constants.DIWIKI
                Query = Queries.GetQueryForWIKIUserValidation(email)
                Record = DBConn.ExecuteDataTable(Query)
                If Record.Rows.Count <> 0 Then
                    RetVal = True
                End If

                Exit Select

        End Select

        Return RetVal
    End Function


    ''' <summary> 
    ''' Generate Forum Password Basis on MD5 Format basis on password 
    ''' </summary> 
    ''' <param name="password"></param> 
    ''' <returns></returns> 
    Private Function PasswordGeneratorBasedOnMD5Format(ByVal password As String) As String
        Dim RetVal As String = String.Empty
        Dim Encryption As String = String.Empty

        Encryption = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, Constants.MD5)
        Me.Salt = RandomNumberGenerator(3)

        Encryption = Encryption.ToLower() + Me.Salt
        RetVal = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Encryption, Constants.MD5)

        Return RetVal.ToLower()
    End Function

    ''' <summary> 
    ''' Generate Password for WIKI basis on password 
    ''' </summary> 
    ''' <param name="password"></param> 
    ''' <returns></returns> 
    Private Function PasswordGenerateForWiki(ByVal password As String) As String
        Dim RetVal As String = String.Empty
        Dim Encryption As String = ":B:"
        'string Salt= string.Empty; 
        ' password = "bajaj"; 
        ' RetVal = RandomNumberGenerator(3); 
        ' Encryption = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, Constants.MD5); 
        Me.Salt = "4989d0d9"
        ' RandomNumberGenerator(8).ToLower(); 
        ' this.Salt = RandomNumberGenerator(8).ToLower(); 
        Encryption = Encryption + Salt.ToLower() & ":"
        RetVal = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, Constants.MD5)
        RetVal = RetVal.ToLower()
        RetVal = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile((Salt.ToLower() & "-") + RetVal, Constants.MD5)
        RetVal = Encryption + RetVal.ToLower()



        Return RetVal
    End Function

    ''' <summary> 
    ''' Connection Close 
    ''' </summary> 
    Private Sub ConnectionClose()


        Me.DBConn.Dispose()
    End Sub

    ''' <summary> 
    ''' Create Connection Basis on type 
    ''' </summary> 
    ''' <param name="type"></param> 
    Private Sub ConnectionOpen(ByVal type As String)

        'Getting Connection String 
        Me.ConnectionString = GetConnectionString(type)

        'Setting Database Type 
        If type = Constants.DIDESKTOP Then

            Me.ServerType = DIServerType.SqlServer
        Else

            Me.ServerType = DIServerType.MySql
        End If

        'Close Connection If it exists 

        ' this.ConnectionClose(); 


        'Create Connection 




        Me.DBConn = New DevInfo.Lib.DI_LibDAL.Connection.DIConnection(Me.ConnectionString, ServerType)
    End Sub

    ''' <summary> 
    ''' Create ConnectionString Basis on type 
    ''' </summary> 
    ''' <param name="type"></param> 
    ''' <returns></returns> 
    Private Function GetConnectionString(ByVal type As String) As String
        Dim RetVal As String = String.Empty
        RetVal = System.Configuration.ConfigurationManager.AppSettings(type)
        Return RetVal
    End Function







    ''' <summary> 
    ''' Generate random Number basis on length 
    ''' </summary> 
    ''' <param name="length"></param> 
    ''' <returns></returns> 
    Private Function RandomNumberGenerator(ByVal length As Integer) As String
        Dim RetVal As String = String.Empty
        Dim rng As System.Security.Cryptography.RandomNumberGenerator = System.Security.Cryptography.RandomNumberGenerator.Create()
        Dim chars As Char() = New Char(length - 1) {}

        'based on your requirment you can take only alphabets or number 
        Dim validChars As String = "abcdefghijklmnopqrstuvwxyzABCEDFGHIJKLMNOPQRSTUVWXYZ1234567890"

        For i As Integer = 0 To length - 1
            Dim bytes As Byte() = New Byte(0) {}
            rng.GetBytes(bytes)
            Dim rnd As New Random(bytes(0))
            chars(i) = validChars(rnd.[Next](0, 61))
        Next
        RetVal = New String(chars)
        Return RetVal
    End Function





    ''' <summary> 
    ''' Set All Field for Register User 
    ''' </summary> 
    ''' <param name="userName"></param> 
    ''' <param name="password"></param> 
    ''' <param name="email"></param> 
    ''' <param name="organization"></param> 
    ''' <param name="country"></param> 
    ''' <param name="firstName"></param> 
    ''' <param name="lastName"></param> 
    ''' <param name="contactDetails"></param> 
    Private Sub SetFieldForUser(ByVal userName As String, ByVal password As String, ByVal email As String, ByVal organization As String, ByVal country As String, ByVal firstName As String, _
    ByVal lastName As String, ByVal contactDetails As String)


        'Set Fields For User Details 
        Me.UserName = userName
        Me.Email = email
        Me.Organization = organization
        Me.Country = country
        Me.LastName = lastName
        Me.FirstName = firstName
        Me.ContactDetails = contactDetails
        Me.Password = password


        'If FirstName of User is Empty then Get FirstName from Email 
        If Me.FirstName = String.Empty Then
            Dim EndIndex As Integer = email.IndexOf("@")

            Me.FirstName = Me.Email.Substring(0, EndIndex)
        End If
        'If Country is Empty then Set Default INDIA as a Country 
        If Me.Country = String.Empty Then

            Me.Country = "India"
        End If
        'If UserName is Empty then set Email as a Username 
        If Me.UserName = String.Empty Then

            Me.UserName = Me.Email
        End If
        'If Body Is empty give UserDetails in Body 
        If Me.Body = String.Empty Then
            Me.Body = " User Created with following Details"
            Me.Body += " UserName: " & Me.UserName
            Me.Body += " Email: " & Me.Email


            Me.Body += " Password: " & Me.Password
        End If
        'If Subject is empty set subject as User Created With date 
        If Me.Subject = String.Empty Then

            Me.Subject = "User Created on " & System.DateTime.Now.ToString()

        End If
    End Sub




    ''' <summary> 
    ''' Register User Basis on Type 
    ''' </summary> 
    ''' <param name="type"></param> 
    ''' <returns></returns> 
    Private Function RegisterUser(ByVal type As String) As Boolean
        Dim Query As String = String.Empty
        Dim CountryId As String = String.Empty
        Dim GUID As String = String.Empty
        Dim RetVal As Boolean = True

        Dim ForumTitleId As String = String.Empty
        Dim ForumUserTitle As String = String.Empty
        Dim ForumUserGroupId As String = String.Empty
        Dim ForumShowVbCode As String = String.Empty
        Dim IsUserExists As Boolean = False



        Dim Records As DataTable

        Try
            'Get Encrypted Password 
            EncryptedPassword = Me.EncryptionOfPassword(type, Me.Password)

            'Get GUID 
            GUID = Me.GenerateGUIDBasedOnName(Me.FirstName)

            'Create Connection 
            Me.ConnectionOpen(type)


            'Check User Already Exists 
            IsUserExists = ValidateUser(type, Me.Email)

            If Not IsUserExists Then
                Select Case type

                    Case Constants.DIDESKTOP


                        ' this.MailBody += type + System.Environment.NewLine; 

                        'Get CountryId 
                        CountryId = Me.GetRegionIdByCountryName(Me.Country)

                        'Insert User in DIDesktop 
                        Query = Queries.GetQueryForInsertUserInDesktop(Me.FirstName, Me.LastName, Me.UserName, Me.Email, Me.ContactDetails, GUID, _
                        EncryptedPassword, Me.Organization, CountryId)
                        DBConn.ExecuteNonQuery(Query)

                        GenerateMailBody(type, Me.UserName)

                        Exit Select

                    Case Constants.DIFORUM

                        'Insert User In forum table 
                        Query = Queries.GetQueryForInsertUserInForum(Me.UserName, Me.Email, EncryptedPassword, Me.Salt)
                        DBConn.ExecuteNonQuery(Query)

                        'Get UserGroup Of Forum Where minposts=0 
                        Query = Queries.GetQueryForForumGroupOfUser()
                        Records = DBConn.ExecuteDataTable(Query)
                        ForumUserGroupId = Me.GetIdFromDatatable(Records)



                        'Get UserTitle and UserTitleId of Forum User 
                        Query = Queries.GetQueryForTitleAndTitleIdOfForumUser()
                        Records = DBConn.ExecuteDataTable(Query)

                        For Each dr As DataRow In Records.Rows
                            ForumShowVbCode = dr(0).ToString()
                            ForumUserTitle = dr(1).ToString()
                        Next



                        'Update User In forum table 
                        Query = Queries.GetQueryForUpdateUserInForum(ForumUserGroupId, ForumShowVbCode, Constants.ForumShowBirthdayCode, ForumUserTitle, Me.UserName)
                        DBConn.ExecuteNonQuery(Query)

                        GenerateMailBody(type, Me.UserName)

                        Exit Select

                    Case Constants.DISUPPORT
                        Query = Queries.GetQueryForInsertUserInSupport(Me.UserName, Me.Email, EncryptedPassword)
                        DBConn.ExecuteNonQuery(Query)


                        Query = Queries.GetQueryforUserIdBasisOnUserName(Me.Email)
                        Dim UserId As String = Me.GetIdFromDatatable(DBConn.ExecuteDataTable(Query))



                        Query = Queries.GetQueryforInsertSupportTicket(UserId, System.DateTime.Now.ToString(), Me.Subject, Me.Body)
                        DBConn.ExecuteNonQuery(Query)

                        Query = Queries.GetQueryforSupportGroupForUser()
                        Dim GroupId As String = Me.GetIdFromDatatable(DBConn.ExecuteDataTable(Query))


                        Query = Queries.GetQueryforInsertSupportUserInGroup(UserId, GroupId)
                        DBConn.ExecuteNonQuery(Query)


                        GenerateMailBody(type, Me.Email)
                        Exit Select

                    Case Constants.DIWIKI
                        Dim Name As String = New CultureInfo("en").TextInfo.ToTitleCase(Me.UserName)
                        Query = Queries.GetQueryForInsertUserInWiki(Name, Me.Email, EncryptedPassword)
                        DBConn.ExecuteNonQuery(Query)

                        GenerateMailBody(type, Name)

                        Exit Select


                End Select
            End If




        Catch generatedExceptionName As Exception

            Throw
        End Try


        Me.ConnectionClose()

        Return RetVal
    End Function


    ''' <summary> 
    ''' Get Id from datatable passed Datatable in it 
    ''' </summary> 
    ''' <param name="Record"></param> 
    ''' <returns></returns> 
    Private Function GetIdFromDatatable(ByVal Record As DataTable) As String
        Dim RetVal As String = String.Empty
        Try

            For Each dr As DataRow In Record.Rows
                RetVal = dr(0).ToString()

            Next
        Catch ex As Exception
            'throw ex.Message; 

        End Try

        Return RetVal
    End Function



    ''' <summary> 
    ''' Generate GUID based on Name for DIDESKTOP 
    ''' </summary> 
    ''' <param name="name"></param> 
    ''' <returns></returns> 
    Private Function GenerateGUIDBasedOnName(ByVal name As String) As String
        Dim RetVal As String = String.Empty
        RetVal = Me.ReplaceSingleCodesWithEmpty(name)
        RetVal = (RetVal.Trim() & "_") + System.Guid.NewGuid().ToString()

        Return RetVal
    End Function




    ''' <summary> 
    ''' Replace Single Quote with empty 
    ''' </summary> 
    ''' <param name="name"></param> 
    ''' <returns></returns> 
    Private Function ReplaceSingleCodesWithEmpty(ByVal name As String) As String
        Dim RetVal As String = String.Empty
        RetVal = name.Replace("'", String.Empty)

        Return RetVal
    End Function

    ''' <summary> 
    ''' Get Unix Time Stamp 
    ''' </summary> 
    ''' <returns></returns> 
    Private Function GetUnixTimeStamp() As Double
        Dim UnixTime As TimeSpan = (System.DateTime.UtcNow - New DateTime(1970, 1, 1, 0, 0, 0))
        Return Math.Floor(UnixTime.TotalSeconds)
    End Function

    ''' <summary> 
    ''' Array of all types 
    ''' </summary> 
    ''' <returns></returns> 
    Private Function GetProjectTypes() As String()
        Dim ProjectTypes As String() = New String(2) {}
        ' ProjectTypes[0]=Constants.DIDESKTOP; 
        ProjectTypes(0) = Constants.DISUPPORT
        ProjectTypes(1) = Constants.DIFORUM
        ProjectTypes(2) = Constants.DIWIKI

        Return ProjectTypes
    End Function


    ''' <summary> 
    ''' Send Mail 
    ''' </summary> 
    ''' <param name="sendTo"></param> 
    ''' <param name="sendFrom"></param> 
    ''' <param name="subject"></param> 
    ''' <param name="body"></param> 
    Private Sub SendMail(ByVal sendTo As String, ByVal sendFrom As String, ByVal subject As String, ByVal body As String)
        Dim mail As New MailMessage()
        mail.[To].Add(sendTo)
        mail.From = New MailAddress("From@gmail.com")
        mail.Subject = subject
        mail.Body = body
        mail.IsBodyHtml = True




        Dim smtp As New SmtpClient()
        smtp.Host = ConfigurationManager.AppSettings(Constants.SMTP)


        smtp.Send(mail)
    End Sub



    ''' <summary> 
    ''' Generate mail Body part 
    ''' </summary> 
    ''' <param name="type"></param> 
    ''' <param name="userName"></param> 
    Private Sub GenerateMailBody(ByVal type As String, ByVal userName As String)
        Me.MailBody += type + System.Environment.NewLine
        Me.MailBody += (" User :" & userName) + System.Environment.NewLine
        Me.MailBody += (" Email :" & Me.Email) + System.Environment.NewLine
        Me.MailBody += (" Password :" & Me.Password) + System.Environment.NewLine + System.Environment.NewLine
    End Sub


    ''' <summary> 
    ''' Generate mail subject part 
    ''' </summary> 
    ''' <param name="type"></param> 
    ''' <param name="userName"></param> 
    Private Sub GenerateMailSubject(ByVal subject As String)
        Me.MailSubject = " User created in " & subject
    End Sub
    ''' <summary>
    ''' Create Log file for getting FactsheetContents
    ''' </summary>
    ''' <param name="countryName"></param>
    ''' <param name="FocalPoint"></param>
    ''' <param name="FocalPointEmail"></param>
    ''' <param name="ImplementingAgency"></param>
    ''' <param name="HomepageURL"></param>
    ''' <param name="Databases"></param>
    ''' <param name="Website"></param>
    ''' <param name="LastUpdated"></param>
    ''' <param name="countryFactSheetURL"></param>
    ''' <remarks></remarks>
    Private Sub CreateLogFile(ByVal countryName As String, ByVal FocalPoint As String, ByVal FocalPointEmail As String, ByVal ImplementingAgency As String, ByVal HomepageURL As String, ByVal Databases As String, _
    ByVal Website As String, ByVal LastUpdated As String, ByVal countryFactSheetURL As String, ByVal IsUpdated As Boolean, ByVal query As String)

        Dim txtwriter As TextWriter

        Dim LogFilePath As String = System.Configuration.ConfigurationManager.AppSettings("LogFilePath")


        txtwriter = New StreamWriter(LogFilePath)
        txtwriter.WriteLine("countryName: " & countryName)
        txtwriter.WriteLine("FocalPoint: " & FocalPoint)
        txtwriter.WriteLine("FocalPointEmail: " & FocalPointEmail)
        txtwriter.WriteLine("ImplementingAgency: " & ImplementingAgency)
        txtwriter.WriteLine("HomepageURL: " & HomepageURL)
        txtwriter.WriteLine("Databases: " & Databases)
        txtwriter.WriteLine("Website: " & Website)
        txtwriter.WriteLine("LastUpdated: " & LastUpdated)
        txtwriter.WriteLine("countryFactSheetURl: " & countryFactSheetURL)
        txtwriter.WriteLine("file Created: " & IsUpdated)
        txtwriter.WriteLine("Query: " & query)

        txtwriter.Close()


    End Sub

#End Region

#End Region



End Class