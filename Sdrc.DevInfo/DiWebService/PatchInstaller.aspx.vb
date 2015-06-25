Imports System.IO
Imports System.Web.Services

Public Class PatchInstaller
    Inherits System.Web.UI.Page

    Public LogFilePath As String = String.Empty
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
        '  Dim LogFilePath As String = String.Empty
        DeleteExistingLogFile()
        SetControlsText()
        LogFilePath = Path.Combine(PatchConstaints.CSVLogPath, PatchConstaints.PatchInstLogFileName).Replace("\\", "/").Replace("\", "/") & ".xls"
        If Request.QueryString.Count > 0 Then
            If Not IsNothing(Request.QueryString("Src")) And Request.QueryString("Src") = "1" Then
                ClientScript.RegisterStartupScript(Page.GetType(), "OnLoad", "InstallPatch('\" + LogFilePath + "');", True)
                BtnInstall_Patch.Visible = False
                System.IO.File.WriteAllText(Server.MapPath("ws.txt"), "text")
                'Dim objPatchas As New PatchInstall()
                'objPatchas.InstallPatch()
                'Dim close As String = "<script type='text/javascript'>window.returnValue = true;window.close();</script>"
                'Response.Write(close)
            End If
            If Not IsNothing(Request.QueryString("Src")) And Request.QueryString("Src") = "2" Then
                BtnInstall_Patch.Visible = False
            End If
        End If
    End Sub

    Private Sub DeleteExistingLogFile()
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
        aLogFile.HRef = "javascript:alert('" + PatchInstall.ReadKeysForPatch("LogFileMsg") + "');"
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

        If Not String.IsNullOrEmpty(PatchInstall.ReadKeysForPatch("aLogFile")) Then
            aLogFile.InnerText = PatchInstall.ReadKeysForPatch("aLogFile")
        End If

    End Sub


End Class
