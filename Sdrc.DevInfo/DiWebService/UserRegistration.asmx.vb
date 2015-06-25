Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data.SqlClient
Imports System.Xml
Imports System.Data

<WebService(Namespace:="http://DIworldwide/DIWWS/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class UserRegistration
    Inherits System.Web.Services.WebService

#Region "Private Variables"
    Private con As New DIConnection
    Private objKML As New KMLGen
    Private objDAL As New Queries
    Private objutility As New Utility
#End Region

#Region "Methods"
    'Function To Set User Details While Registration
    <WebMethod()> _
    Public Function RegisterUser(ByVal FirstName As String, ByVal LastName As String, ByVal Email As String, ByVal Password As String, ByVal Organization As String, ByVal Country As String)
        Try
            Dim CR_Query As String = String.Empty
            Dim Country_ID As Integer
            'Dim Region_ID As Integer
            'Dim Login As String = FirstName + " " + LastName
            Dim GUID As String = String.Empty
            Dim ContactDetails As String = ""
            Dim dt As DataTable

            '-------- Get Country Name on the basis of Country ID ----------
            dt = objDAL.GetCounrtyAndRegionId(Country)
            For i As Integer = 0 To dt.Rows.Count - 1
                Country_ID = dt.Rows(i)(objKML.Country_NId)
                'Region_ID = dt.Rows(i)(objKML.Region_Nid)
            Next

            '-------- Check Email Aready Exists ----------
            Dim dr As SqlDataReader
            Dim Success As Boolean = False
            dr = objDAL.CheckEmailExistence(Email)

            If dr.HasRows Then
                '-------- If  Email Exists Return GUID ----------
                While dr.Read
                    GUID = dr.Item(0).ToString
                End While
                dr.Close()

                Success = objDAL.UpdateUserInformation(GUID, ReplaceCharacter(FirstName), ReplaceCharacter(LastName), Email, ReplaceCharacter(Organization), Country_ID)
                If Success = True Then
                    Return GUID
                ElseIf Success = False Then
                    Return 0
                End If
            Else
                '-------- If  Email Not Exists ----------
                dr.Close()
                Dim _FirstName As String = ReplaceCharacter(FirstName)
                Dim _LastName As String = ReplaceCharacter(LastName)
                Dim _Login As String = _FirstName + " " + _LastName
                Dim _Email As String = ReplaceCharacter(Email)
                Dim _ContactDetails As String = ReplaceCharacter(ContactDetails)
                Dim _Password As String = ReplaceCharacter(Password)
                Dim _Organization As String = ReplaceCharacter(Organization)
                GUID = Trim(FirstName.Replace("'", "")) + "_" + GenerateGUID()

                '------ Insert User Information In User Master table ---------

                Success = objDAL.SetUserDetails(_FirstName, _LastName, _Login, _Email, _ContactDetails, GUID, _Password, _Organization, Country_ID)
                If Success = True Then
                    Return GUID
                ElseIf Success = False Then
                    Return 0
                End If
            End If
        Catch ex As Exception
            Return 0
        End Try
    End Function

    'Function To Set User Adaptation.'INPUTS - - (WorldwideUserGId,Adaptation Name,Adaptation version)
    <WebMethod()> _
    Public Function SetUserAdaptationInfo(ByVal WorldwideUserGId As String, ByVal Adaptation_Name As String, ByVal Adaptation_Version As String)
        Try
            Dim User_NId As Integer
            Dim Adaptation_NId As Integer
            Dim dt As DataTable
            Dim Success As Boolean = False
            Dim dr As SqlDataReader

            '--------- For retrieving UserNid and AdaptationNid ------ 
            dt = objDAL.GetUserNId(WorldwideUserGId, Adaptation_Name, Adaptation_Version)

            For i As Integer = 0 To dt.Rows.Count - 1
                User_NId = dt.Rows(i)(objKML.User_NId)
                Adaptation_NId = Convert.ToInt32(dt.Rows(i)(objKML.Adaptation_NId))
            Next


            dr = objDAL.CheckUserNidAndAdaptationNid(User_NId, Adaptation_NId)
            If dr.HasRows Then
                dr.Close()
                '--------- Generate KML on the basis of KML Type ---------
                objKML.GenerateKML(objKML.KML_RegisteredUsers)

                '--------- Generate StatsHTML.html For showing Different Counts----------

                objutility.GenerateStatsHTML()
            Else
                dr.Close()
                '--------- Set User Adaptation --------------------------
                Success = objDAL.SetUserAdaptation(User_NId, Adaptation_NId)
                '--------- Generate KML on the basis of KML Type ---------
                objKML.GenerateKML(objKML.KML_RegisteredUsers)

                '--------- Generate StatsHTML.html For showing Different Counts----------

                objutility.GenerateStatsHTML()

            End If

        Catch ex As Exception
            Return 0
        End Try
    End Function

    'Function To Get User Information on the Basis of GUID
    <WebMethod()> _
    Public Function GetUserInformation(ByVal GUID As String) As DataTable
        Dim objdt As DataTable = Nothing
        Try
            Dim objDS As DataSet
            objDS = objDAL.GetUserInfo(GUID)
            objdt = objDS.Tables("Table")
            Return objdt
        Catch ex As Exception
            Return objdt
        End Try
       End Function

    'Function To Update User Information on The Basis of GUID
    <WebMethod()> _
    Public Function UpdateUserInfo(ByVal GUID As String, ByVal FirstName As String, ByVal LastName As String, ByVal Email As String, ByVal Organization As String, ByVal Country As String)
        Try
            '-------- Get Country Name on the basis of Country ID ----------
            Dim dt As DataTable
            Dim Country_ID As Integer
            dt = objDAL.GetCounrtyAndRegionId(Country)
            Country_ID = dt.Rows(0)(objKML.Country_NId)

            '------------ Update User information -------------------------
            Dim Success As Boolean = False
            Success = objDAL.UpdateUserInformation(GUID, ReplaceCharacter(FirstName), ReplaceCharacter(LastName), Email, ReplaceCharacter(Organization), Country_ID)
            If Success = True Then
                Return -1
            ElseIf Success = False Then
                Return 0
            End If
        Catch ex As Exception
            Return 0
        End Try
    End Function

    'Function To Generate GUID
    Private Function GenerateGUID()
        Dim sGUID As String = String.Empty
        sGUID = System.Guid.NewGuid.ToString()
        Return sGUID
    End Function

    'Function To Replace Single Quotes With Double Quotes
    Private Function ReplaceCharacter(ByVal Str As String)
        Dim _Str As String = Str.Replace("'", "''")
        Return _Str
    End Function

#End Region

   End Class
