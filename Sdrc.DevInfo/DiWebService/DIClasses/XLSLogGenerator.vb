Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Text
Imports System.IO
Imports SpreadsheetGear

''' <summary>
''' Summary description for XLSLogGenerator
''' </summary>
Public NotInheritable Class XLSLogGenerator



    ''' <summary>
    ''' Write log into XLS file
    ''' </summary>
    ''' <param name="FilePath"></param>
    ''' <param name="CurrentDateTime"></param>
    ''' <param name="UserName"></param>
    ''' <param name="Module"></param>
    ''' <param name="Details"></param>
    Private Shared Sub WriteToCsvFile(FilePath As String, CurrentDateTime As String, UserName As String, [Module] As String, Details As String, UserEmailId As String)
        Dim StrBld As New StringBuilder()
        Try
            StrBld.Append(vbCr & vbLf)
            ' Implement special handling for values that contain comma or quote
            ' Enclose in quotes and double up any double quotes
            If UserName.IndexOfAny(New Char() {""""c, ","c}) <> -1 Then
                StrBld.AppendFormat("""{0}""", UserName.Replace("""", """"""))
            Else
                StrBld.Append(UserName)
            End If
            StrBld.Append(",")
            StrBld.Append([Module])
            StrBld.Append(",")
            StrBld.Append(CurrentDateTime)
            StrBld.Append(",")
            If Details.IndexOfAny(New Char() {""""c, ","c}) <> -1 Then
                StrBld.AppendFormat("""{0}""", Details.Replace("""", """"""))
            Else
                StrBld.Append(Details)
            End If
            StrBld.Append(",")
            StrBld.Append(UserEmailId)

            File.AppendAllText(FilePath, StrBld.ToString())
        Catch Ex As Exception

        End Try
    End Sub

    ' main calling method for writing log of patch installation
    Public Shared Sub WriteLogForPatchInstallation(Message As String, Status As String, ExceptionMsg As String)

        Dim FileName As String = String.Empty
        Dim FilePath As String = String.Empty
        Dim CurrentDateTime As String = String.Empty
        Dim FileFullName As String = String.Empty
        Dim directryPath As String = String.Empty
        Try
            CurrentDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")

            'Get Directort Path
            directryPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, PatchConstaints.CSVLogPath)
            ' Get file Name
            FileName = PatchConstaints.PatchInstLogFileName

            ' Get a FilePath
            FilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, directryPath, FileName) & ".xls"

            ' create directory if not exists
            If Not Directory.Exists(directryPath) Then
                Directory.CreateDirectory(directryPath)
            End If
            ' If file not exist create file with header
            If Not File.Exists(FilePath) Then
                Dim ExcelSheet As IWorksheet = DirectCast(Factory.GetWorkbook().Sheets(0), IWorksheet)
                ' Create Header of XLS file  
                CreatePatchInstFileHeader(ExcelSheet, FileName)
                'ref IWorksheet ExcelSheet, string FilePath, string UserName,string Date, string Module, string Details, string UserEmailId
                WriteLogsForInstallation(ExcelSheet, FilePath, CurrentDateTime, Message, Status, ExceptionMsg)
            Else
                ' If file exist directally write on file
                Dim ExcelSheet1 As IWorksheet = DirectCast(Factory.GetWorkbook(FilePath).Sheets(0), IWorksheet)
                WriteLogsForInstallation(ExcelSheet1, FilePath, CurrentDateTime, Message, Status, ExceptionMsg)
            End If
        Catch Ex As Exception

        End Try
    End Sub

    Private Shared Sub CreatePatchInstFileHeader(ByRef excelSheet As IWorksheet, FileName As String)
        Try
            Dim RowIndex As Integer = 0
            Dim ColumnIndex As Integer = 0
            Dim ListHeaderName As New List(Of String)()
            ' ListHeaderName.Add("Name");
            ListHeaderName.Add("Message")
            ListHeaderName.Add("Date")
            ListHeaderName.Add("Status")
            ListHeaderName.Add("ExceptionMsg")
            For Each StrHeaderName As String In ListHeaderName
                excelSheet.Cells(RowIndex, ColumnIndex).Value = StrHeaderName
                excelSheet.Cells(RowIndex, ColumnIndex).Borders.Color = System.Drawing.Color.Black
                ' Sets The Border Color For Header
                excelSheet.Cells(RowIndex, ColumnIndex).Interior.Color = System.Drawing.Color.Gray
                ' Sets The Background Color For Header
                excelSheet.Cells(RowIndex, ColumnIndex).Font.Bold = True
                ' Sets Header Font To Bold
                excelSheet.Cells(RowIndex, ColumnIndex).Font.Color = System.Drawing.Color.White
                ' Sets Header Font Color To White
                excelSheet.Cells(RowIndex, ColumnIndex).Font.Size = 12
                excelSheet.Cells(RowIndex, ColumnIndex).VerticalAlignment = VAlign.Top

                Dim Range As SpreadsheetGear.IRange = excelSheet.Range(RowIndex, ColumnIndex, 1, ColumnIndex + 1)
                If StrHeaderName = "ExceptionMsg" Then
                    Range.ColumnWidth = 80.0
                Else
                    Range.ColumnWidth = 23.0
                End If
                ColumnIndex += 1
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Shared Sub WriteLogsForInstallation(ByRef ExcelSheet As IWorksheet, FilePath As String, [Date] As String, Message As String, Status As String, ExceptionMsg As String)
        Try

            Dim RowIndex As Integer = Convert.ToInt32(ExcelSheet.UsedRange.RowCount)
            ExcelSheet.Cells(RowIndex, 0).Value = Message
            ExcelSheet.Cells(RowIndex, 1).Value = [Date]
            ExcelSheet.Cells(RowIndex, 2).Value = Status
            ExcelSheet.Cells(RowIndex, 3).Value = ExceptionMsg
            ExcelSheet.Cells(RowIndex, 0).Borders.Color = System.Drawing.Color.Black
            ExcelSheet.Cells(RowIndex, 1).Borders.Color = System.Drawing.Color.Black
            ExcelSheet.Cells(RowIndex, 2).Borders.Color = System.Drawing.Color.Black
            ExcelSheet.Cells(RowIndex, 3).Borders.Color = System.Drawing.Color.Black
            ExcelSheet.Name = PatchConstaints.PatchInstLogFileName
            ExcelSheet.SaveAs(FilePath, SpreadsheetGear.FileFormat.Excel8)
        Catch ex As Exception

        End Try
    End Sub

End Class




