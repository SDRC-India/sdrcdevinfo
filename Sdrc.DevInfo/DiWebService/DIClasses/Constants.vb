Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls

''' <summary> 
''' Summary description for Constants 
''' </summary> 

Friend NotInheritable Class Constants

#Region "--New/Dispose--"
    ''' <summary> 
    ''' Private constructor 
    ''' </summary> 
    ''' <remarks></remarks> 
    Private Sub New()
        'DO Nothing 
    End Sub
#End Region

#Region "--Constants Region--"


#Region "--Common For All Regions--"

    Friend Const MD5 As String = "MD5"

    Friend Const User As String = "user"
    Friend Const SMTP As String = "SMTP"
#End Region


#Region "--Desktop Region--"

    Friend Const DIDESKTOP As String = "DBConnString"

    'Constant Table Names 
    Friend Const mUserInfo As String = "mUserInfo"

    'Constant Field Names 
    Friend Const Firstname As String = "Firstname"
    Friend Const LastName As String = "LastName"
    Friend Const Login As String = "Login"
    Friend Const Email As String = "Email"
    Friend Const ContactDetails As String = "ContactDetails"
    Friend Const GUID As String = "GUID"
    Friend Const Password As String = "Password"
    Friend Const Organization As String = "Organization"
    Friend Const CountryNId As String = "Country_NId"
    Friend Const mCountry As String = "mCountry"
    Friend Const CountryName As String = "Country_Name"


#End Region


#Region "--Support Region--"

    Friend Const DISUPPORT As String = "DISUPPORT"
    Friend Const SupportName As String = "name"
    Friend Const SupportPassword As String = "password"
    Friend Const SupportEmail As String = "email"


    Friend Const SupportGroupId As String = "group_id"
    Friend Const SupportGroups As String = "groups"
    Friend Const SupportMember As String = "Members"

    Friend Const SupportUserId As String = "user_id"
    Friend Const SupportAdded As String = "added"
    Friend Const SupportSubject As String = "subject"
    Friend Const SupportBody As String = "body"

    Friend Const SupportUserGroup As String = "user_group"
    Friend Const SupportTickets As String = "tickets"

#End Region


#Region "--WIKI Region--"

    Friend Const DIWIKI As String = "DIWIKI"

    Friend Const WikiUserName As String = "user_name"
    Friend Const WikiUserPassword As String = "user_password"
    Friend Const WikiUserEmail As String = "user_email"
    Friend Const WikiSalt As String = "4989d0d9"

#End Region


#Region "--FORUM Region--"

    Friend Const DIFORUM As String = "DIFORUM"


    Friend Const ForumUserName As String = "username"
    Friend Const ForumPassword As String = "password"
    Friend Const ForumEmail As String = "email"
    Friend Const ForumSalt As String = "salt"

    Friend Const ForumUserGroupId As String = "usergroupid"
    Friend Const ForumDisplayGroupId As String = "displaygroupid"
    Friend Const ForumPasswordDate As String = "passworddate"
    Friend Const ForumStyleId As String = "styleid"
    Friend Const ForumShowVbCode As String = "showvbcode"
    Friend Const ForumShowBirthday As String = "showbirthday"
    Friend Const ForumUserTitle As String = "usertitle"
    Friend Const ForumCustomTitle As String = "customtitle"
    Friend Const ForumReputation As String = "reputation"
    Friend Const ForumReputationLevelId As String = "reputationlevelid"
    Friend Const ForumOptions As String = "options"
    Friend Const ForumUserTitleId As String = "usertitleid"

    Friend Const ForumUserGroup As String = "usergroup"

    Friend Const ForumRegistrationTitle As String = "Registered Users"
    Friend Const ForumTitle As String = "title"
    Friend Const ForumMinPosts As String = "minposts"


    Friend Const ForumJoinDate As String = "joindate"
    Friend Const ForumLastVisit As String = "lastvisit"
    Friend Const ForumLastActvity As String = "lastactivity"
    Friend Const ForumShowBirthdayCode As String = "0"
#End Region

#Region "--FactInfo Region--"

    Friend Const FocalPoints As String = "FocalPoints"
    Friend Const FocalPointEmails As String = "FocalPoint_Emails"
    Friend Const ImplementingAgency As String = "Implementing_Agency"
    Friend Const HomepageURL As String = "Homepage_URL"
    Friend Const Databases As String = "Databases"
    Friend Const Website As String = "Website"
    Friend Const LastUpdated As String = "LastUpdated"
    Friend Const rCountryAdaptation As String = "rCountryAdaptation"
    Friend Const countryFactSheetURL As String = "CountryFactSheet_URL"

#End Region

#Region "-- App Settings --"

    Friend Const DI7SDMXREGISTRY As String = "DI7SDMXREGISTRY"

    Friend Class XmlFile
        Friend Class AppSettings
            Friend Class Tags
                Friend Const Root As String = "appsettings"
                Friend Const Item As String = "item"

                Friend Class ItemAttributes
                    Friend Const Name As String = "n"
                    Friend Const Value As String = "v"
                End Class
            End Class
        End Class
    End Class

    Friend Class AppSettingKeys

        Friend Const Disclaimer As String = "Disclaimer"
        Friend Const MasterAdaptationUrl As String = "Master_Adaptation_Url"

    End Class


#End Region

#Region "--Delimiters--"

    Friend Const PARAM_DELIMITER As String = "[****]"

#End Region


    Friend Class FileNames
        Friend Const AppSettingFileName = "Stock\AppSettings.xml"
    End Class

#End Region

#Region "Map Server"

    Public Const MapServerConnectionString As String = "MapServerConnectionString"
    Public Const MapServerDirectoryPath As String = "MapServerDirectoryPath"
    Public Const MapServerURL As String = "MapServerURL"

#End Region

End Class