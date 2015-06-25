Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Public Class DIConnection
    

#Region "Variables"
    Private oConn As SqlConnection
    Private oCommand As New SqlCommand
    Private oReader As SqlDataReader
    Private oDataAdapder As New SqlDataAdapter
    Private inCommand As New SqlCommand
    Private connstring As String = Nothing
#End Region

#Region "Methods"
    'Open Connection
    Public Sub openconnection()
        Try
            If connstring Is Nothing Then
                connstring = ReadConnectionString()
            End If
            oConn = New SqlConnection(connstring)
            If oConn.State = ConnectionState.Open Then
                oConn.Close()
            End If
            oConn.Open()
        Catch Ex As Exception
            Throw New System.Exception("Error Source: Connection " + Ex.Message)
        End Try
    End Sub

    ' Close Connection
    Private Sub closeconnection()
        Try
            If oConn.State = ConnectionState.Open Then
                oConn.Close()
                oConn = Nothing
            End If
        Catch Ex As Exception
            Throw New System.Exception("Error Source: Connection " + Ex.Message)
        End Try
    End Sub

    'Read Connection String
    Private Function ReadConnectionString() As String
        'connstring = "Data Source=127.0.0.1;Initial Catalog=DIWorldWide;User Id=sa;Password=sa;"
        'connstring = "Data Source=dgps;Initial Catalog=DIWorldwide;User Id=sa;"
        connstring = System.Configuration.ConfigurationManager.AppSettings("DBConnString")
        Return connstring
    End Function

    'ReaderData
    Public Function ReaderData(ByVal mySelectQuery As String) As SqlDataReader
        Try
            Dim objcon As Object = oConn
            If objcon Is Nothing Then
                openconnection()
            End If

            oCommand = New SqlCommand(mySelectQuery, oConn)
            oCommand.CommandTimeout = 90
            oReader = oCommand.ExecuteReader

            Return oReader
        Catch ex As Exception
            Throw New System.Exception("Error Source: Connection " + ex.Message + mySelectQuery)
        Finally
            oCommand = Nothing
        End Try
    End Function

    'Fill DataSet
    Public Function FillDataSet(ByVal MySelectQuery As String) As DataSet
        Try
            Dim objcon As Object = oConn

            If objcon Is Nothing Then
                openconnection()
            End If

            oCommand = New SqlCommand(MySelectQuery, oConn)
            Dim MyDataSet As New DataSet
            oDataAdapder = New SqlDataAdapter(oCommand)
            oCommand.CommandTimeout = 300
            oCommand.CommandType = CommandType.Text
            oDataAdapder.Fill(MyDataSet, "Table")
            Return MyDataSet
        Catch ex As Exception
            Throw New System.Exception(ex.Message)
        Finally
            oCommand = Nothing
            closeconnection()
        End Try
    End Function

    'Fill DataTable
    Public Function FillDataTable(ByVal MySelectQuery As String) As DataTable
        Try
            Dim objcon As Object = oConn
            Dim MyDataTable As New DataTable
            If objcon Is Nothing Then
                openconnection()
            End If

            oCommand = New SqlCommand(MySelectQuery, oConn)
            oCommand.CommandTimeout = 300
            oCommand.CommandType = CommandType.Text
            oCommand.ExecuteNonQuery()
            oDataAdapder = New SqlDataAdapter(oCommand)
            oDataAdapder.Fill(MyDataTable)
            Return MyDataTable
        Catch ex As Exception
            Throw New System.Exception(ex.Message & MySelectQuery)
        Finally
            oCommand = Nothing
            closeconnection()
        End Try
    End Function

    'Insert
    Public Function Insert(ByVal myInsertQuery As String) As Object
        Try
            Dim objcon As Object = oConn

            If objcon Is Nothing Then
                openconnection()
            End If

            oCommand = New SqlCommand(myInsertQuery, oConn)
            oCommand.CommandTimeout = 90
            oCommand.ExecuteNonQuery()
            Return "0"
        Catch ex As Exception
            'For Debugging
            Throw New System.Exception("Error Source: Connection " + ex.Message + myInsertQuery)
        Finally
            oCommand = Nothing
            oConn = Nothing
        End Try 'myReader = null; 
    End Function

#End Region
 
End Class
