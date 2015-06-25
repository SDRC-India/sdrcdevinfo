Imports System.IO
Imports System.Web.Services

Public Class PatchInstaller
    Inherits System.Web.UI.Page

    Public LogfilePath As String = String.Empty


    'Private Sub BtnInstall_Patch_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnInstall_Patch.ServerClick

    '    Dim objPatchas As New PatchInstall()
    '    objPatchas.InstallPatch()

    'End Sub

    Private Sub PatchInstaller_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim LogFilePath As String = String.Empty

        If Not String.IsNullOrEmpty(PatchInstall.ReadKeysForPatch("LangLeftLnkInstPatch")) Then
            LangLeftLnkInstPatch.InnerText = PatchInstall.ReadKeysForPatch("LangLeftLnkInstPatch")
        End If

        If Not String.IsNullOrEmpty(PatchInstall.ReadKeysForPatch("lang_db_PatchMainHeading")) Then
            lang_db_PatchMainHeading.InnerText = PatchInstall.ReadKeysForPatch("lang_db_PatchMainHeading")
        End If

        If Not String.IsNullOrEmpty(PatchInstall.ReadKeysForPatch("lang_db_Patch_subHeadFileName")) Then
            lang_db_Patch_subHead.InnerText = PatchInstall.ReadKeysForPatch("lang_db_Patch_subHead")
        End If

        If Not String.IsNullOrEmpty(PatchInstall.ReadKeysForPatch("langInstallingPatchHeadFileName")) Then
            langInstallingPatchHead.InnerText = PatchInstall.ReadKeysForPatch("langInstallingPatchHead")
        End If

        If Not String.IsNullOrEmpty(PatchInstall.ReadKeysForPatch("langInstallingPatch")) Then
            langInstallingPatch.InnerText = PatchInstall.ReadKeysForPatch("langInstallingPatch")
        End If


        If Not String.IsNullOrEmpty(PatchInstall.ReadKeysForPatch("BtnInstall_Patch")) Then
            BtnInstall_Patch.Value = PatchInstall.ReadKeysForPatch("BtnInstall_Patch")
        End If


        If Not String.IsNullOrEmpty(PatchInstall.ReadKeysForPatch("LangPatchInstSuccess")) Then
            LangPatchInstSuccess.Value = PatchInstall.ReadKeysForPatch("LangPatchInstSuccess")
        End If

        If Not String.IsNullOrEmpty(PatchInstall.ReadKeysForPatch("LangPatchInstFailed")) Then
            LangPatchInstFailed.Value = PatchInstall.ReadKeysForPatch("LangPatchInstFailed")
        End If



        'If Not String.IsNullOrEmpty(ReadKeysForPatch.GetLanguageKeyValue("Lang_aLogFile")) Then
        '    aLogFile.InnerText = ReadKeysForPatch.GetLanguageKeyValue("Lang_aLogFile")
        'End If

        LogFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, PatchConstaints.CSVLogPath.ToString(), PatchConstaints.PatchInstLogFileName.ToString() & ".xls")
        'if (File.Exists(LogFilePath))
        '{
        aLogFile.HRef = "../../" & (System.IO.Path.Combine(PatchConstaints.CSVLogPath.ToString(), PatchConstaints.PatchInstLogFileName.ToString()) & ".xls")
        ' }

    End Sub


    <WebMethod()> _
    Public Shared Function InstallPatch() As String
        Dim RetVal As String
        RetVal = False
        Dim objPatchas As New PatchInstall()
        Try
            If objPatchas.InstallPatch() Then
                RetVal = True
            Else
                RetVal = False
            End If

        Catch ex As Exception

        End Try
        Return RetVal
    End Function

End Class
