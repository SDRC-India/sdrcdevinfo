Imports System.IO
Imports System.Web.Services

Public Class PatchInstaller
    Inherits System.Web.UI.Page

    Public LogfilePath As String = String.Empty

    <WebMethod()> _
    Public Shared Function InstallPatch() As String
        Dim RetVal As String
        RetVal = False
        ' Dim jSearializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim objPatchas As New PatchInstall()
        Try
            If objPatchas.InstallPatch() Then
                RetVal = True
            Else
                RetVal = False
            End If
        Catch ex As Exception
            RetVal = False
        End Try
        Return RetVal
    End Function

    Private Sub PatchInstaller_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim LogFilePath As String = String.Empty
        DeleteNCreateNewLogFile()
        SetControlsText()
        LogFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, PatchConstaints.CSVLogPath.ToString(), PatchConstaints.PatchInstLogFileName.ToString() & ".xls")

        aLogFile.HRef = "../../" & (System.IO.Path.Combine(PatchConstaints.CSVLogPath.ToString(), PatchConstaints.PatchInstLogFileName.ToString()) & ".xls")


    End Sub

    Private Sub DeleteNCreateNewLogFile()
        Dim directryPath As String = String.Empty
        Dim FileName As String = String.Empty
        Dim FilePath As String = String.Empty
        'Get Directort Path
        directryPath = PatchConstaints.CSVLogPath
        ' Get file Name
        FileName = PatchConstaints.PatchInstLogFileName
        ' Get a FilePath
        FilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, directryPath, FileName) + ".xls"
        ' if file is already existing then delete existion file
        If File.Exists(FilePath) Then
            File.Delete(FilePath)
        End If
    End Sub

    Private Sub SetControlsText()
        If Not String.IsNullOrEmpty(PatchInstall.ReadKeysForPatch("LangLeftLnkInstPatch")) Then
            LangLeftLnkInstPatch.InnerText = PatchInstall.ReadKeysForPatch("LangLeftLnkInstPatch")
        End If

        If Not String.IsNullOrEmpty(PatchInstall.ReadKeysForPatch("lang_db_PatchMainHeading")) Then
            lang_db_PatchMainHeading.InnerText = PatchInstall.ReadKeysForPatch("lang_db_PatchMainHeading")
        End If

        If Not String.IsNullOrEmpty(PatchInstall.ReadKeysForPatch("lang_db_Patch_subHead")) Then
            lang_db_Patch_subHead.InnerText = PatchInstall.ReadKeysForPatch("lang_db_Patch_subHead")
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
    End Sub

    


End Class
