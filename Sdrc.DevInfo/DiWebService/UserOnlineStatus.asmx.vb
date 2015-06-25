Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data
Imports System.Data.SqlClient
Imports System.Xml


<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class UserOnlineStatus
    Inherits System.Web.Services.WebService

#Region "Private Variables"
    Private con As New DIConnection
    Private objKML As New KMLGen
    Private objDAL As New Queries
    Private objutility As New Utility
    Private dt As DataTable

#End Region

#Region "Methods"
    'Function To Set The Status of the User        
    <WebMethod()> _
    Public Function SetStatus(ByVal Online As Boolean, ByVal WorldwideUserGId As String)
        Try

            Dim Success As Boolean = False

            '----------- To Set the User Status (ONLINE    OR  OFFLINE) -----------------
            Success = objDAL.SetUserStatus(Online, WorldwideUserGId)
            If Success = True Then
                '----------- Generate KML For Online Users -----------------
                objKML.GenerateKML(objKML.KML_OnlineUsers)

                '----------- Generate KML For Registered Users -----------------
                objKML.GenerateKML(objKML.KML_RegisteredUsers)

                '----------- Generate StatsHTML.html For showing Different Counts----------
                objutility.GenerateStatsHTML()

                Return -1
            ElseIf Success = False Then
                Return 0
            End If

        Catch ex As Exception
            Return 0
        End Try

    End Function

    'Function To Get The Status(Online or offline) of the User       
    <WebMethod()> _
    Public Function GetStatus(ByVal WorldwideUserGId As String)
        Try

            Dim UserStatus As Boolean = False

            ' ------------To Get The UserStatus (ONLINE OR OFFLINE)--------------------
            dt = objDAL.GetUserStatus(WorldwideUserGId)
            For i As Integer = 0 To dt.Rows.Count - 1
                UserStatus = dt.Rows(i)(objKML.Isonline)
            Next

            Return UserStatus
            'Dim UserStatus As Boolean = False
            'dt = objDAL.GetUserStatus(WorldwideUserGId)

            'Dim TableName As String = dt.TableName
            'If TableName = "ExceptionTable" Then
            '    Return 0
            'Else

            '    ' ------------To Get The UserStatus (ONLINE OR OFFLINE)--------------------
            '    For i As Integer = 0 To dt.Rows.Count - 1
            '        UserStatus = dt.Rows(i)(objKML.Isonline)
            '    Next

            '    Return UserStatus
            'End If



        Catch ex As Exception
            Return -1
        End Try

    End Function

#End Region
   

End Class
