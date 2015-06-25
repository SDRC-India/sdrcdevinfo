Public Partial Class UpdateUserOnlineStatus
    Inherits System.Web.UI.Page
    Dim objKML As New KMLGen
    Dim objDAL As New Queries
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Success As Boolean = True
        Success = objDAL.UpdateUserOnlinestatus()
        If Success = True Then
            'objKML.GenerateKML(objKML.KML_OnlineUsers)
        End If
    End Sub

End Class