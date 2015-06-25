Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data
Imports System.Data.SqlClient
Imports System.xml
Imports System.Text
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Web.UI
Imports System.IO
Imports System.net
Imports System.Text.RegularExpressions
Imports DevInfo.Lib.DI_LibBAL.UI.Presentations.Map
Imports DevInfo.Lib.DI_LibDAL.Queries
Imports System.Drawing.Drawing2D


<WebService(Namespace:="http://DIworldwide/DIWWS/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class Utility
    Inherits System.Web.Services.WebService

#Region "Private Variables"

    Private con As New DIConnection
    Private objKML As New KMLGen
    Private objDAL As New Queries
    Private dt As DataTable
    'Private strFTPInfo As String = System.Configuration.ConfigurationManager.AppSettings("DILU_FTP")
    'Private strCPDFTPInfo As String = System.Configuration.ConfigurationManager.AppSettings("CPDLU_FTP")
    Private LogFilePath As String = System.Configuration.ConfigurationManager.AppSettings("LogFilePath")

#End Region

#Region "Methods"
    'Function to Get Country List 
    <WebMethod()> _
        Public Function GetCountryListValues() As DataSet
        Try
            Dim objDS As DataSet
            ' --------------- To Get All The Country List ----------------------
            objDS = objDAL.GetCountryLists()

            Return objDS
        Catch ex As Exception
            Throw New System.Exception(ex.Message)
        End Try
    End Function

    'Function to Get Region List
    '<WebMethod()> _
    '    Public Function GetRegionList() As DataSet
    '    Dim objDS As DataSet
    '    Try
    '        Dim Query As String = String.Empty
    '        objDS = objDAL.GetRegionLists()

    '        Return objDS
    '    Catch ex As Exception
    '        Throw New System.Exception(ex.Message)
    '    End Try

    'End Function

    'Function To RegisterUser 
    <WebMethod()> _
    Public Function RegisterAllUser(ByVal userName As String, ByVal email As String, ByVal password As String, ByVal organization As String, ByVal country As String, ByVal firstName As String, _
    ByVal lastName As String, ByVal contactDetails As String, ByVal type As String) As Boolean
        Dim RetVal As Boolean = False
        Try
            Dim Util As DIUtility = New DIUtility()
            RetVal = Util.RegisterUser(userName, password, email, organization, country, firstName, _
            lastName, contactDetails, type)
        Catch generatedExceptionName As Exception

            RetVal = False
        End Try


        Return RetVal
    End Function

    'Function To Update Country fact Info 
    <WebMethod()> _
    Public Function UpdateCountryFactInfo(ByVal countryName As String, ByVal FocalPoint As String, ByVal FocalPointEmail As String, ByVal ImplementingAgency As String, ByVal HomepageURL As String, ByVal Databases As String, _
    ByVal Website As String, ByVal LastUpdated As String, ByVal countryFactSheetURL As String) As Boolean
        Dim RetVal As Boolean = False

        Try
            '' DIUtility Util=new DIUtility();
            Dim Util As DIUtility = New DIUtility()


            RetVal = Util.UpdateCountryFactInfo(countryName, FocalPoint, FocalPointEmail, ImplementingAgency, HomepageURL, Databases, _
        Website, LastUpdated, countryFactSheetURL)
            If RetVal Then
                Me.GenerateKML("ADAPTATIONS")
            End If
        Catch generatedExceptionName As Exception
            RetVal = False
        End Try

        Return RetVal
    End Function


    'Function To Generate KML on the Basis of KML Type
    'Input - - KMLType(Adaptations,RegisteredUsers,OnlineUsers,All)
    <WebMethod()> _
    Public Function GenerateKML(ByVal KMLType As String)
        Try
            '------- Generate KML On The Basis Of KML Type (1)Adaptations 2)OnlineUsers 3)Activations 4)All ---------------
            objKML.GenerateKML(KMLType)
            objKML.GenerateJS()            '-- Generate JS File
            Return -1
        Catch ex As Exception
            Return 0
        End Try

    End Function

    'Function To Add New Country
    <WebMethod()> _
    Public Function CreateCountry(ByVal Country_Name As String, ByVal Country_ID As String, ByVal Longitude As String, ByVal Latitude As String, ByVal CountryFlag_URL As String, ByVal CountryFactSheet_URL As String)
        Try
            Dim Success As Boolean = False
            '--------------- To Add New Country Information -------------------
            Success = objDAL.CreateNewCountry(Country_Name, Country_ID, Longitude, Latitude, CountryFlag_URL, CountryFactSheet_URL)
            If Success = True Then
                Return -1
            ElseIf Success = False Then
                Return 0
            End If
        Catch ex As Exception
            Return 0
        End Try
        Return -1
    End Function

    'Function To Create Country Adaptation
    <WebMethod()> _
   Public Function CreateCountryAdaptation(ByVal CountryName As String, ByVal AdaptationName As String, ByVal AdaptationVersion As String, ByVal FocalPoint As String, ByVal FocalPointEmail As String, ByVal ImplementingAgency As String, ByVal HomepageURL As String, ByVal Databases As String, ByVal Website As String, ByVal LastUpdated As String)

        Try
            Dim AdaptationID As Integer
            Dim CountryID As Integer
            Dim Success As Boolean = False

            '------------ To Get The AdaptationID On The Basis of Adaptation Name and Adaptation Version ----------------
            dt = objDAL.GetAdaptationID(AdaptationName, AdaptationVersion)
            For i As Integer = 0 To dt.Rows.Count - 1
                AdaptationID = dt.Rows(i)(objKML.Adaptation_NId)
            Next

            '------------ To Get CountryID On The Basis of Country Name -------------------
            dt = objDAL.GetCounrtyAndRegionId(CountryName)
            For i As Integer = 0 To dt.Rows.Count - 1
                CountryID = dt.Rows(i)(objKML.Country_NId)
            Next


            '------------ To Set Country Adaptation ---------------------
            Success = objDAL.CreateCountryAdaptation(CountryID, AdaptationID, FocalPoint, FocalPointEmail, ImplementingAgency, HomepageURL, Databases, Website, LastUpdated)
            If Success = True Then
                Return -1
            Else
                Return 0
            End If
        Catch ex As Exception
            Return 0
        End Try
    End Function

    'Function To Add New Adaptation
    <WebMethod()> _
    Public Function CreateAdaptation(ByVal Adaptation_Name As String, ByVal Adaptation_Ver As String)
        Try
            Dim Success As Boolean = False
            'Dim Country_ID As Integer
            ''Dim Region_ID As Integer

            'dt = objDAL.GetCounrtyAndRegionId(Country_Name)
            'For i As Integer = 0 To dt.Rows.Count - 1
            '    Country_ID = dt.Rows(i)("Country_NId")
            '    'Region_ID = dt.Rows(i)("Region_NId")
            'Next

            ' -----------------To Set New Adaptation Information -----------------------
            Success = objDAL.CreateNewAdaptation(Adaptation_Name, Adaptation_Ver)
            If Success = True Then
                Return -1
            Else
                Return 0
            End If

        Catch ex As Exception
            Return 0
        End Try
    End Function

    'Get Adaptation Count
    <WebMethod()> _
   Public Function GetAdaptationCount() As Integer
        Try
            'Dim objDS As DataSet
            'objDS = objDAL.GetAdaptationCounts()
            dt = objDAL.GenerateKMLForAdaptations()
            Dim AdaptationsCount As Integer = dt.Rows.Count
            Return AdaptationsCount
        Catch ex As Exception
            Throw New System.Exception(ex.Message)
        End Try

    End Function

    'Get Online User Count
    <WebMethod()> _
   Public Function GetOnlineUserCount() As Integer
        Try
            'Dim objDS As DataSet
            'objDS = objDAL.GetOnlineUserCounts()
            dt = objDAL.GenerateKMLForOnlineUsers()
            Dim OnlineUserCount As Integer = dt.Rows.Count
            Return OnlineUserCount
        Catch ex As Exception
            Throw New System.Exception(ex.Message)
        End Try

    End Function

    'Get Online Activation Count
    <WebMethod()> _
   Public Function GetActivationCount() As Integer
        Try
            'Dim objDS As DataSet
            'objDS = objDAL.GetActivationCounts()
            dt = objDAL.GenerateKMLForRegisteredUsers()
            Dim ActivationCount As Integer = dt.Rows.Count
            Return ActivationCount
        Catch ex As Exception
            Throw New System.Exception(ex.Message)
        End Try

    End Function

    'Genearte StatsCount.html To show AdaptationCount,OnlineuserCount,ActivationsCount
    <WebMethod()> _
   Public Function GenerateStatsHTML()
        Try

            'Dim dt As New DataTable
            'Dim objDS As New DataSet
            'objDS = GetAdaptationCount()
            'For Each dr As DataRow In objDS.Tables("Table").Rows
            '    objKML.AdaptationsCount = dr("AdapationCount")
            'Next
            'objDS.Dispose()


            'objDS = GetOnlineUserCount()
            'For Each dr As DataRow In objDS.Tables("Table").Rows
            '    objKML.OnlineUsersCount = dr("OnlineUserCount")
            'Next
            'objDS.Dispose()

            'objDS = GetActivationCount()
            'For Each dr As DataRow In objDS.Tables("Table").Rows
            '    objKML.ActivationsCount = dr("ActivationCount")
            'Next
            'objDS.Dispose()

            ''Generate StatsHTML.html
            'objKML.CreateHTMLTable_StatsCounts()
            objKML.AdaptationsCount = GetAdaptationCount()          '-----Get Adaptation Count---------
            objKML.OnlineUsersCount = GetOnlineUserCount()          '-----Get OnlineUsers Count---------
            objKML.ActivationsCount = GetActivationCount()          '-----Get Activations Count---------
            objKML.GenerateJS()                                '-----Generate Stats.js for showing all the counts---------

            'Dim Content As String = objKML.CreateHTMLTable_StatsCounts()                    '-----Generate StatsHTML.html for showing all the counts---------

            Return -1
        Catch ex As Exception
            Return 0
        End Try

    End Function
    'Function to Get DI FTP Information
    '  <WebMethod()> _
    'Public Function GetDIFTPInfo(ByVal FTPType As String) As FTPReturnObject
    '      Dim objFTP As New FTPReturnObject
    '      Dim _FTPType As String = FTPType.ToUpper
    '      Select Case _FTPType

    '          Case "DILU"
    '              objFTP.FTPHost = strFTPInfo.Split(";")(0).Split("=")(1)
    '              objFTP.FTPUserName = strFTPInfo.Split(";")(1).Split("=")(1)
    '              objFTP.FTPPassword = strFTPInfo.Split(";")(2).Split("=")(1)
    '              objFTP.FTPDirectory = strFTPInfo.Split(";")(3).Split("=")(1)

    '              'FTP Connection details added for CPD, added on Nov. 19 by Kapil
    '          Case "CPDLU"
    '              objFTP.FTPHost = strCPDFTPInfo.Split(";")(0).Split("=")(1)
    '              objFTP.FTPUserName = strCPDFTPInfo.Split(";")(1).Split("=")(1)
    '              objFTP.FTPPassword = strCPDFTPInfo.Split(";")(2).Split("=")(1)
    '              objFTP.FTPDirectory = strCPDFTPInfo.Split(";")(3).Split("=")(1)

    '      End Select
    '      Return objFTP
    '  End Function

    <WebMethod()> _
    Public Function GetDIFTPInfo(ByVal FTPType As String) As FTPReturnObject
        Dim objFTP As New FTPReturnObject
        Dim _FTPType As String = FTPType.ToUpper
        Dim strFTPInfo As String = System.Configuration.ConfigurationManager.AppSettings(_FTPType)

        If String.IsNullOrEmpty(strFTPInfo) = False Then

            If _FTPType.ToUpper() = "DI7SDMXREGISTRY" Then


            Else
                objFTP.FTPHost = strFTPInfo.Split(";")(0).Split("=")(1)
                objFTP.FTPUserName = strFTPInfo.Split(";")(1).Split("=")(1)
                objFTP.FTPPassword = strFTPInfo.Split(";")(2).Split("=")(1)
                objFTP.FTPDirectory = strFTPInfo.Split(";")(3).Split("=")(1)

            End If

        End If

        Return objFTP
    End Function

    <WebMethod()> _
    Public Function GetSDMXRegistryURL() As String
        Dim RetVal As String = String.Empty
        Dim SDMXURL As String = String.Empty
        Dim SDMXFilename As String = String.Empty
        Dim DBFileURL As String = String.Empty
        Dim XmlDoc As Xml.XmlDocument
        Dim Node As Xml.XmlNode

        Try
            '-- Get sdmx url from web.config
            SDMXURL = System.Configuration.ConfigurationManager.AppSettings("DI7SDMXREGISTRY")
            SDMXFilename = System.Configuration.ConfigurationManager.AppSettings("DI7SDMXFILENAME")

            If (Not String.IsNullOrEmpty(SDMXURL) AndAlso Not String.IsNullOrEmpty(SDMXFilename)) Then
                '-- create dbfile  url
                DBFileURL = SDMXURL & "/db.xml"

                '-- Get database id from db file
                XmlDoc = New XmlDocument()
                XmlDoc.Load(DBFileURL)
                Node = XmlDoc.SelectSingleNode("dbinfo[1]")

                '-- Create SDMX complete xml url using database id
                RetVal = Path.Combine(SDMXURL, "data/" + Node.Attributes("def").Value + "/sdmx/" + SDMXFilename)

            End If
        Catch ex As Exception
            RetVal = String.Empty
        Finally
            XmlDoc = Nothing
        End Try


        Return RetVal
    End Function

    <WebMethod()> _
   Public Function GetDatabaseDetails(ByVal DBType As String) As DbDetailsReturnObject
        Dim objDB As New DbDetailsReturnObject
        Dim _DBType As String = DBType.ToUpper
        Dim strDatabaseDetails As String = System.Configuration.ConfigurationManager.AppSettings(_DBType)

        If String.IsNullOrEmpty(strDatabaseDetails) = False Then

            objDB.ServerType = strDatabaseDetails.Split(";")(0).Split("=")(1)
            objDB.PortNumber = strDatabaseDetails.Split(";")(1).Split("=")(1)
            objDB.DatabaseName = strDatabaseDetails.Split(";")(2).Split("=")(1)
            objDB.UserId = strDatabaseDetails.Split(";")(3).Split("=")(1)
            objDB.Password = strDatabaseDetails.Split(";")(4).Split("=")(1)

        End If

        Return objDB
    End Function

    <WebMethod()> _
    Public Function GetValueByKey(ByVal key As String) As List(Of String)
        Dim RetVal As New List(Of String)
        Dim ConfigValue As String = String.Empty
        '-- <add key="MapRegistryDatabase" value="ServerType = DIServerType.SqlServer;Server=61.12.1.180;DatabaseName=DI6_MapRegistry;UserID=sa;Password:weblis;"/>

        '-- get value for the given key and split value string using ";" character
        Try
            ConfigValue = System.Configuration.ConfigurationManager.AppSettings(key.ToUpper())
            If String.IsNullOrEmpty(ConfigValue) = False Then

                For Each Val As String In DevInfo.Lib.DI_LibBAL.Utility.DICommon.SplitString(ConfigValue, ";")

                    RetVal.Add(Val.Split("=")(1))
                Next
            End If

        Catch ex As Exception
        End Try

        Return RetVal
    End Function


#Region "Map Extraction"

    ''' <summary>
    ''' It extracts Shape File's information of all layers present in the database, in XML format file.
    ''' It generates one XML file for each Layer.
    ''' </summary>
    ''' <param name="databaseServerPath">Database file Server Relative Path.</param>
    ''' <param name="outputServerPath">Output Server folder path where XML file will be generated</param>
    <WebMethod()> _
    Public Function ExtractShapeInfoXML(ByVal databaseServerPath As String, ByVal outputServerPath As String, ByVal tempServerFolder As String) As Boolean
        Dim RetVal As Boolean = False
        Dim ShapeInfoFilepath As String = String.Empty

        Dim dIConnection As DevInfo.Lib.DI_LibDAL.Connection.DIConnection = Nothing
        If String.IsNullOrEmpty(databaseServerPath) = False Then
            Try
                dIConnection = New DevInfo.Lib.DI_LibDAL.Connection.DIConnection(DevInfo.Lib.DI_LibDAL.Connection.DIServerType.MsAccess, String.Empty, String.Empty, Server.MapPath(databaseServerPath), String.Empty, String.Empty)

                ' Get Distinct LayersNIds from UT_AreaMap
                ' Extract shapefiles from UT_Area_Map_Layer and place them into  folder
                ' Map Class Static function - public static bool ExtractShapeFileByLayerId(string layerNId, string spatialMapFolder, DIConnection DIConnection, DIQueries DIQueries)

                Dim _map As Map = New Map()
                _map.Width = 600 ''Keeping 600,600 Hardcoded, this could be passed in parameted in future if required
                _map.Height = 600

                '-- Extracts Shape files 
                Dim dbQueries As New DIQueries(dIConnection.DIDataSetDefault(), dIConnection.DILanguageCodeDefault(dIConnection.DIDataSetDefault()))
                Map.ExtractAllShapeFiles(dIConnection, dbQueries, Server.MapPath(tempServerFolder))

                Dim di As New DirectoryInfo(Server.MapPath(tempServerFolder))
                Dim rgFiles() As FileInfo = di.GetFiles("*.shp")

                '-- Add Shape Files in Layers Collection that are extracted in TempFolder
                For Each fi As FileInfo In rgFiles
                    Try
                        _map.Layers.AddShapeFile(Server.MapPath(tempServerFolder), System.IO.Path.GetFileNameWithoutExtension(fi.FullName))
                    Catch ex As Exception

                    End Try
                Next

                _map.SetFullExtent()
                Dim ImgStream As Stream = _map.GetMapStream()

                '- Transform map coordinates
                Dim Matrixpoint As Single() = _map.TransMatrix
                Dim Transmatrix As New Matrix(Matrixpoint(0), Matrixpoint(1), Matrixpoint(2), Matrixpoint(3), Matrixpoint(4), Matrixpoint(5))

                Dim TopLeft As PointF() = New PointF(0) {}
                TopLeft(0) = New PointF(_map.FullExtent.Left, _map.FullExtent.Top)
                Transmatrix.TransformPoints(TopLeft)
                Dim BottomRight As PointF() = New PointF(0) {}
                BottomRight(0) = New PointF(_map.FullExtent.Right, _map.FullExtent.Bottom)
                Transmatrix.TransformPoints(BottomRight)
                Dim ExtentWidth As Single = Math.Abs(BottomRight(0).X - TopLeft(0).X)
                Dim ExtentHeight As Single = Math.Abs(TopLeft(0).Y - BottomRight(0).Y)
                Dim LayerName_en As String = String.Empty
                Dim LayerName_fr As String = String.Empty
                Dim BorderColor As String = "0x000000"
                Dim BorderWidth As Double = 0.5
                Dim BorderAlpha As Double = 0.5

                ' Iterate for each shape file in map folder and generate transformation xml
                'Map transform matrix is updated only after GetMapStream is called
                For Each oLayer As Layer In _map.Layers
                    Try
                        Dim sb As New StringBuilder()
                        sb.Append("<mapdata>")
                        sb.Append(((("<map width=""" & _map.Width & """ height=""") & _map.Width & """ extentwidth=""") & ExtentWidth & """ extentheight=""") & ExtentHeight & """ >")

                        TopLeft(0) = New PointF(oLayer.Extent.Left, oLayer.Extent.Top)
                        Transmatrix.TransformPoints(TopLeft)
                        BottomRight(0) = New PointF(oLayer.Extent.Right, oLayer.Extent.Bottom)

                        Transmatrix.TransformPoints(BottomRight)
                        ExtentWidth = Math.Abs(BottomRight(0).X - TopLeft(0).X)
                        ExtentHeight = Math.Abs(TopLeft(0).Y - BottomRight(0).Y)
                        sb.Append((((((("<layer id=""" & oLayer.LayerName & """ type=""") & Convert.ToInt32(oLayer.LayerType) & """ extentwidth=""") & ExtentWidth & """ extentheight=""") & ExtentHeight & """ BorderColor=""") & BorderColor & """ BorderWidth=""") & BorderWidth & """ BorderAlpha=""") & BorderAlpha & """ >")
                        sb.Append(("<name en=""" & oLayer.LayerName & """ fr=""") & oLayer.LayerName & """ >")
                        sb.Append("</name>")
                        Dim ht As System.Collections.Hashtable = oLayer.GetRecords((oLayer.LayerPath & "\") & oLayer.ID)
                        Me.GenerateTransformedXMl(ht, Transmatrix, sb, oLayer.LayerType)
                        sb.Append("</layer>")

                        sb.Append("</map>")
                        sb.Append("</mapdata>")

                        ShapeInfoFilepath = Path.Combine(Server.MapPath(outputServerPath), oLayer.LayerName & ".xml")
                        Dim sw As New System.IO.StreamWriter(ShapeInfoFilepath)
                        sw.Write(sb.ToString())

                        sw.Close()

                        ''- Zip File and save it as [LayerName].zip
                        Me.ZipFile(ShapeInfoFilepath)
                        RetVal = True

                    Catch ex As Exception
                    End Try
                Next
            Catch ex As Exception

            Finally
                If Not IsNothing(dIConnection) Then
                    dIConnection.Dispose()
                End If
            End Try
        End If
        Return RetVal
    End Function


    Private Sub GenerateTransformedXMl(ByVal ht As System.Collections.Hashtable, ByVal Transmatrix As Matrix, ByVal sb As StringBuilder, ByVal shapeType__1 As ShapeType)
        Dim dicEnumerator As System.Collections.IDictionaryEnumerator = ht.GetEnumerator()
        Dim _Shape As Shape
        Dim AreaNId As Integer = 1
        Dim CentroidF As PointF() = New PointF(0) {}
        Dim TopLeft As PointF() = New PointF(0) {}
        Dim BottomRight As PointF() = New PointF(0) {}
        Dim ExtentWidth As Single = 0
        Dim ExtentHeight As Single = 0
        Dim IsHighResolution As Boolean = True
        While dicEnumerator.MoveNext()
            _Shape = DirectCast(dicEnumerator.Value, Shape)

            If shapeType__1 = ShapeType.Point OrElse shapeType__1 = ShapeType.PointCustom Then
                CentroidF(0) = DirectCast(_Shape.Parts(0), PointF)
            Else
                'TODO centroid for line type
                CentroidF(0) = _Shape.Centroid

                TopLeft(0) = New PointF(_Shape.Extent.Left, _Shape.Extent.Top)
                Transmatrix.TransformPoints(TopLeft)
                BottomRight = New PointF(0) {}
                BottomRight(0) = New PointF(_Shape.Extent.Right, _Shape.Extent.Bottom)
                Transmatrix.TransformPoints(BottomRight)
                ExtentWidth = Math.Abs(BottomRight(0).X - TopLeft(0).X)


                ExtentHeight = Math.Abs(TopLeft(0).Y - BottomRight(0).Y)
            End If
            Transmatrix.TransformPoints(CentroidF)
            'sb.Append("<area>");
            '((float)nudHeight.Value - Math.Round(TopLeft[0].Y))
            sb.Append(((("<area x=""" & Math.Round(TopLeft(0).X, 4) & """ y=""") & Math.Round(BottomRight(0).Y) & """ extentwidth=""") & ExtentWidth & """ extentheight=""") & ExtentHeight & """ >")
            sb.Append("<nid>" & AreaNId & "</nid>")
            sb.Append("<id>" & _Shape.AreaId & "</id>")
            sb.Append("<name>" & _Shape.AreaName.Replace("&", "&") & "</name>")
            ' // &=& '='   >=>  <=<   "="

            If shapeType__1 = ShapeType.Point OrElse shapeType__1 = ShapeType.PointCustom Then
                ' Dont truncate in case of point layer
                sb.Append(("<centroid>" & CentroidF(0).X & " ") & CentroidF(0).Y & "</centroid>")
            Else
                Dim Centroid As Point = Point.Truncate(CentroidF(0))
                sb.Append(("<centroid>" & Centroid.X & " ") & Centroid.Y & "</centroid>")
            End If
            sb.Append(" <map coordinates=""")
            For i As Integer = 0 To _Shape.Parts.Count - 1
                Dim Pts As PointF() = Nothing
                If shapeType__1 = ShapeType.Point OrElse shapeType__1 = ShapeType.PointCustom Then
                    Pts = New PointF(0) {}
                    Pts(0) = DirectCast(_Shape.Parts(i), PointF)
                Else

                    Pts = DirectCast(_Shape.Parts(i), PointF())
                End If
                Transmatrix.TransformPoints(Pts)
                For j As Integer = 0 To Pts.Length - 1
                    '-- Int point lower precision lighter file
                    If IsHighResolution Then
                        '-- Float Point for prcision but will make file bulky
                        sb.Append((Pts(j).X.ToString() & " ") + Pts(j).Y.ToString() & " ")
                    Else
                        Dim Pt As Point = Point.Truncate(Pts(j))
                        sb.Append((Pt.X.ToString() & " ") + Pt.Y.ToString() & " ")
                    End If
                Next
                sb.Append("-- ")
            Next
            sb.Append(" ""/>")
            sb.Append("</area>")
            AreaNId += 1

        End While
    End Sub

    Private Function ZipFile(ByVal fileNameWPath As String) As Boolean
        Dim RetVal As Boolean = False

        ' ***NOTE***
        ' If a fileName is Pref.xml, then zipped fileName should be "Pref.zip".

        Try
            ' Zip file path
            Dim sZipFilePath As String = (Path.GetDirectoryName(fileNameWPath) & "\") + Path.GetFileNameWithoutExtension(fileNameWPath) & ".zip"

            ' Create a Zip file in the stock folder and then provide a download link
            Dim zipOut As New ICSharpCode.SharpZipLib.Zip.ZipOutputStream(File.Create(sZipFilePath))

            ' add to the zip file.
            Dim fi As New System.IO.FileInfo(fileNameWPath)

            Dim entry As New ICSharpCode.SharpZipLib.Zip.ZipEntry(fi.Name)
            Dim sReader As FileStream = File.OpenRead(fileNameWPath)
            Dim buff As Byte() = New Byte(Convert.ToInt32(sReader.Length) - 1) {}
            sReader.Read(buff, 0, CInt(sReader.Length))
            entry.DateTime = fi.LastWriteTime
            entry.Size = sReader.Length
            sReader.Close()
            zipOut.PutNextEntry(entry)
            zipOut.Write(buff, 0, buff.Length)
            zipOut.Finish()
            zipOut.Close()

            RetVal = True
        Catch ex As Exception
        End Try

        Return RetVal
    End Function

#End Region

#Region "-- Map server --"

    <WebMethod()> _
    Public Function GetMapServerConnection() As String
        Dim RetVal As String
        RetVal = ConfigurationManager.AppSettings(Constants.MapServerConnectionString)
        Return RetVal
    End Function

    <WebMethod()> _
    Public Function GetMapServerDirectory() As String
        Dim MapServerPath As String
        MapServerPath = ConfigurationManager.AppSettings(Constants.MapServerDirectoryPath)
        Return MapServerPath
    End Function

    <WebMethod()> _
    Public Function GetMapServerURL() As String
        Dim MapServerPath As String
        MapServerPath = ConfigurationManager.AppSettings(Constants.MapServerURL)
        Return MapServerPath
    End Function

#End Region

#End Region

End Class
Public Class FTPReturnObject

    Public FTPHost As String = String.Empty
    Public FTPUserName As String = String.Empty
    Public FTPPassword As String = String.Empty
    Public FTPDirectory As String = String.Empty

End Class

Public Class DbDetailsReturnObject

    Public ServerType As String = String.Empty
    Public PortNumber As String = String.Empty
    Public DatabaseName As String = String.Empty
    Public UserId As String = String.Empty
    Public Password As String = String.Empty

End Class

