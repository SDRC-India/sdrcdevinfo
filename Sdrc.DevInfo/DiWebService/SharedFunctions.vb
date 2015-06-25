Imports System.IO
Imports System.Xml
Imports DALConnection = DevInfo.Lib.DI_LibDAL.Connection

Public Class SharedFunctions

    Public Shared Function SplitString(ByVal valueString As String, ByVal delimiter As String) As String()
        Dim RetVal As String()
        Dim Index As Integer = 0
        Dim Value As String
        Dim SplittedList As New List(Of String)()

        While True
            Index = valueString.IndexOf(delimiter)
            If Index = -1 Then
                If Not String.IsNullOrEmpty(valueString) Then
                    SplittedList.Add(valueString)
                End If
                Exit While
            Else
                Value = valueString.Substring(0, Index)
                valueString = valueString.Substring(Index + delimiter.Length)

                SplittedList.Add(Value)

            End If
        End While

        RetVal = SplittedList.ToArray()

        Return RetVal
    End Function

    Public Shared Function GetDbConnection() As DALConnection.DIConnection

        Dim ObjDIConnection As DALConnection.DIConnection = Nothing
        Dim ConnectionStr As String = String.Empty
        Dim ConnDetailsArr(5) As String
        Dim ServerName As String = String.Empty
        Dim DatabaseName As String = String.Empty
        Dim UserName As String = String.Empty
        Dim Password As String = String.Empty

        Try
            ConnectionStr = ConfigurationManager.AppSettings("DBConnString")

            ConnDetailsArr = SplitString(ConnectionStr, ";")

            ServerName = ConnDetailsArr(0).Substring(ConnDetailsArr(0).IndexOf("=") + 1)
            DatabaseName = ConnDetailsArr(1).Substring(ConnDetailsArr(1).IndexOf("=") + 1)
            UserName = ConnDetailsArr(2).Substring(ConnDetailsArr(2).IndexOf("=") + 1)
            Password = ConnDetailsArr(3).Substring(ConnDetailsArr(3).IndexOf("=") + 1)

            ObjDIConnection = New DALConnection.DIConnection(DALConnection.DIServerType.SqlServer, ServerName, "", DatabaseName, UserName, Password)

        Catch ex As Exception
        End Try

        Return ObjDIConnection

    End Function

    Public Shared Function GetJSONString(ByVal Dt As DataTable) As String
        Dim RetVal As String = String.Empty
        Dim StrDc As String() = New String(Dt.Columns.Count - 1) {}
        Dim HeadStr As String = String.Empty
        Dim Sb As New StringBuilder()
        Dim TempStr As String = String.Empty
        Dim i As Integer
        Dim j As Integer

        Try
            For i = 0 To Dt.Columns.Count - 1
                StrDc(i) = Dt.Columns(i).Caption
                HeadStr += """" & StrDc(i) & """ : """ & StrDc(i) & i.ToString() & "¾" & ""","
            Next

            HeadStr = HeadStr.Substring(0, HeadStr.Length - 1)

            Sb.Append("{""" & Convert.ToString(Dt.TableName) & """ : [")

            For i = 0 To Dt.Rows.Count - 1
                TempStr = HeadStr
                Sb.Append("{")

                For j = 0 To Dt.Columns.Count - 1
                    TempStr = TempStr.Replace(Dt.Columns(j).ToString() + j.ToString() + "¾", Dt.Rows(i)(j).ToString())
                Next

                Sb.Append(TempStr & "},")
            Next

            Sb = New StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1))
            Sb.Append("]}")

            RetVal = Sb.ToString()
        Catch
        End Try

        Return RetVal
    End Function
End Class
