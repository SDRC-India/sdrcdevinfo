Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Web.UI
Imports System.IO
Imports System.Data.SqlClient
Imports System.xml


Public Class KMLGen


#Region " -- Private -- "

#Region " -- Variables -- "
    Private Writer As HtmlTextWriter
    Private StringWriter As StreamWriter
    Private LogFilePath As String = String.Empty
    Private DefaultStartDateTime As String = String.Empty
    Private StartDateTime As String = String.Empty
    Private EndDateTime As String = String.Empty
    Private KMLpath As String = String.Empty

    'Path for storing KMLs
    'Dim strBaseDirectory As String = System.Web.HttpContext.Current.Server.MapPath("Data\KMLs")
    Private strBaseDirectory_Adaptation As String = System.Configuration.ConfigurationManager.AppSettings("KMLAdaptationFile")
    Private strBaseDirectory_OnlineUsers As String = System.Configuration.ConfigurationManager.AppSettings("KMLOnlineUsersFile")
    Private strBaseDirectory_Activation As String = System.Configuration.ConfigurationManager.AppSettings("KMLActivationFile")
    Private strBaseDirectory_UserIcon As String = System.Configuration.ConfigurationManager.AppSettings("KMLOnlineUsersIconURL")
    Private strBaseDirectory_ActivationIcon As String = System.Configuration.ConfigurationManager.AppSettings("KMLActivationIconURL")
    'Dim strBaseDirectory_StatsHTML As String = System.Configuration.ConfigurationManager.AppSettings("StatsCountHTMLFile")
    Private strBaseDirectory_Statsjs As String = System.Configuration.ConfigurationManager.AppSettings("StatsCountjsFile")

    'KML Types
    Dim _KMLAdaptation As String = "ADAPTATIONS"
    Dim _KMLOnlineUsers As String = "ONLINEUSERS"
    Dim _KMLRegisteredUsers As String = "ACTIVATIONS"
    Dim _KMLAll As String = "ALL"

    Dim _UserName As String = String.Empty
    Dim _Organization As String = String.Empty
    Dim _OrganizationType As String = String.Empty
    Dim _Email As String = String.Empty
    Dim _CountryName As String = String.Empty
    Dim _CountryId As String = String.Empty
    Dim _AdaptationName As String = String.Empty
    Dim _AdaptationLogo As String = String.Empty
    Dim _ImplementingAgency As String = String.Empty
    Dim _Homepage As String = String.Empty
    Dim _Databases As String = String.Empty
    Dim _Website As String = String.Empty
    Dim _CountryFactSheetlink As String = String.Empty
    Dim _Latitude As String = String.Empty
    Dim _LastUpdated As String = String.Empty
    Dim _AdaptationsCount As Integer
    Dim _OnlineUsersCount As Integer
    Dim _ActivationsCount As Integer

#End Region

#Region " -- Methods -- "

#End Region

#End Region

#Region " -- Public -- "

#Region " -- Constructor -- "

#End Region

#Region " -- Properties -- "

    Public Property Isonline()
        Get
            Return C_Isonline
        End Get
        Set(ByVal value)
            C_Isonline = value
        End Set
    End Property

    Public Property User_NId()
        Get
            Return C_User_NId
        End Get
        Set(ByVal value)
            C_User_NId = value
        End Set
    End Property

    Public Property Adaptation_NId()
        Get
            Return C_Adaptation_NId
        End Get
        Set(ByVal value)
            C_Adaptation_NId = value
        End Set
    End Property

    Public Property Country_NId()
        Get
            Return C_Country_NId
        End Get
        Set(ByVal value)
            C_Country_NId = value
        End Set
    End Property

    Public Property Region_Nid()
        Get
            Return C_RegionNid
        End Get
        Set(ByVal value)
            C_RegionNid = value
        End Set
    End Property


    Public Property Username()
        Get
            Return _UserName
        End Get
        Set(ByVal value)
            _UserName = value
        End Set
    End Property


    Public Property Organization()
        Get
            Return _Organization
        End Get
        Set(ByVal value)
            _Organization = value
        End Set
    End Property


    Public Property OrganizationType()
        Get
            Return _OrganizationType
        End Get
        Set(ByVal value)
            _OrganizationType = value
        End Set
    End Property

    Public Property Email()
        Get
            Return _Email
        End Get
        Set(ByVal value)
            _Email = value
        End Set
    End Property


    Public Property CountryName()
        Get
            Return _CountryName
        End Get
        Set(ByVal value)
            _CountryName = value
        End Set
    End Property


    Public Property CountryId()
        Get
            Return _CountryId
        End Get
        Set(ByVal value)
            _CountryId = value
        End Set
    End Property


    Public Property AdaptationName()
        Get
            Return _AdaptationName
        End Get
        Set(ByVal value)
            _AdaptationName = value
        End Set
    End Property


    Public Property AdaptationLogo()
        Get
            Return _AdaptationLogo
        End Get
        Set(ByVal value)
            _AdaptationLogo = value
        End Set
    End Property


    Public Property ImplementingAgency()
        Get
            Return _ImplementingAgency
        End Get
        Set(ByVal value)
            _ImplementingAgency = value
        End Set
    End Property


    Public Property Homepage()
        Get
            Return _Homepage
        End Get
        Set(ByVal value)
            _Homepage = value
        End Set
    End Property

    Public Property Databases()
        Get
            Return _Databases
        End Get
        Set(ByVal value)

            _Databases = value
        End Set
    End Property

    Public Property Website()
        Get
            Return _Website
        End Get
        Set(ByVal value)
            _Website = value
        End Set
    End Property

    Public Property CountryFactSheetlink()
        Get
            Return _CountryFactSheetlink
        End Get
        Set(ByVal value)
            _CountryFactSheetlink = value
        End Set
    End Property


    Public Property LastUpdated()
        Get
            Return _LastUpdated
        End Get
        Set(ByVal value)
            _LastUpdated = value
        End Set
    End Property

    Public Property Latitude()
        Get
            Return _Latitude
        End Get
        Set(ByVal value)
            _Latitude = value
        End Set
    End Property
    Dim _Longitude As String = String.Empty

    Public Property Longitude()
        Get
            Return _Longitude
        End Get
        Set(ByVal value)
            _Longitude = value
        End Set
    End Property
    Dim _FocalPointEmail As String = String.Empty

    Public Property FocalPointEmail()
        Get
            Return _FocalPointEmail
        End Get
        Set(ByVal value)
            _FocalPointEmail = value
        End Set
    End Property
    Dim _FocalPoint As String = String.Empty

    Public Property FocalPoint()
        Get
            Return _FocalPoint
        End Get
        Set(ByVal value)
            _FocalPoint = value
        End Set
    End Property

    Public Property KML_Adaptations()
        Get
            Return _KMLAdaptation
        End Get
        Set(ByVal value)
            _KMLAdaptation = value
        End Set
    End Property
    Public Property KML_OnlineUsers()
        Get
            Return _KMLOnlineUsers
        End Get
        Set(ByVal value)
            _KMLOnlineUsers = value
        End Set
    End Property
    Public Property KML_RegisteredUsers()
        Get
            Return _KMLRegisteredUsers
        End Get
        Set(ByVal value)
            _KMLRegisteredUsers = value
        End Set
    End Property
    Public Property KML_All()
        Get
            Return _KMLAll
        End Get
        Set(ByVal value)
            _KMLAll = value
        End Set
    End Property

    Public Property AdaptationsCount()
        Get
            Return _AdaptationsCount
        End Get
        Set(ByVal value)
            _AdaptationsCount = value
        End Set
    End Property

    Public Property OnlineUsersCount()
        Get
            Return _OnlineUsersCount
        End Get
        Set(ByVal value)
            _OnlineUsersCount = value
        End Set
    End Property

    Public Property ActivationsCount()
        Get
            Return _ActivationsCount
        End Get
        Set(ByVal value)
            _ActivationsCount = value
        End Set
    End Property
#End Region

#Region " -- Methods -- "

    'Function to Generate KML.INPUT - - - KML Type
    Public Function GenerateKML(ByVal KMLType As String)
        Try
            Dim _KMLType As String = KMLType.ToUpper
            Dim dt As New DataTable
            Dim con As New DIConnection
            Dim objDAL As New Queries
            Dim Table_name As String
            Select Case _KMLType
                Case Me.KML_Adaptations
                    Try
                        '-------- To Get Information About all The Adaptations -----------------------
                        dt = objDAL.GenerateKMLForAdaptations()
                        Table_name = dt.TableName
                        If Table_name = "m_Success" Then
                            '-------For Generating Style For KML File For Adapatations-----
                            GenerateStyle(dt, _KMLType)

                            Return -1
                        Else
                            Return 0
                        End If

                        Exit Select
                    Catch ex As Exception
                        Return 0
                    End Try

                Case Me.KML_RegisteredUsers
                    Try
                        '-------- To Get Information About The Registered Users -----------------------
                        dt = objDAL.GenerateKMLForRegisteredUsers()
                        Table_name = dt.TableName
                        If Table_name = "m_Success" Then
                            '------For Generating Style For KML File For Activations-------
                            GenerateStyle(dt, _KMLType)

                            Return -1
                        Else
                            Return 0
                        End If


                        Exit Select
                    Catch ex As Exception
                        Return 0
                    End Try

                Case Me.KML_OnlineUsers
                    Try
                        '-------- To Get Information About The OnLine Users -----------------------
                        dt = objDAL.GenerateKMLForOnlineUsers()
                        Table_name = dt.TableName
                        If Table_name = "m_Success" Then
                            '-------For Generating Style For KML File For OnLineUsers-------
                            GenerateStyle(dt, _KMLType)
                            Return -1
                        Else
                            Return 0
                        End If

                        Exit Select
                    Catch ex As Exception
                        Return 0
                    End Try

                Case Me.KML_All
                    Try
                        '-------For Generating All the KML Files-------- 
                        GenerateKML(Me.KML_Adaptations)
                        GenerateKML(Me.KML_RegisteredUsers)
                        GenerateKML(Me.KML_OnlineUsers)
                        Return -1
                        Exit Select
                    Catch ex As Exception
                        Return 0
                    End Try

            End Select
        Catch ex As Exception
            Return 0
        End Try
        Return -1
    End Function

    'Function To Generate Styles for KML files 
    Public Function GenerateStyle(ByVal Data As DataTable, ByVal KMLType As String)
        Dim _CountryName As String = String.Empty
        Dim _AdaptationName As String = String.Empty
        Dim _AdaptationLogo As String = String.Empty
        Dim _ImplementingAgency As String = String.Empty
        Dim _Homepage As String = String.Empty
        Dim _Databases As String = String.Empty
        Dim _Website As String = String.Empty
        Dim _CountryFactSheetlink As String = String.Empty
        Dim _LastUpdated As String = String.Empty
        Dim _Latitude As String = String.Empty
        Dim _Longitude As String = String.Empty
        Dim dt As DataTable = Data
        Dim doc As XmlDocument = New XmlDocument
        Dim root As XmlElement = doc.CreateElement("kml")
        root.SetAttribute("xmlns", "http://earth.google.com/kml/2.1")
        doc.AppendChild(root)
        Dim dnode As XmlElement = doc.CreateElement("Document")
        root.AppendChild(dnode)

        Dim LookAt As XmlElement = doc.CreateElement("LookAt")
        Dim lgt As XmlElement = doc.CreateElement("longitude")
        lgt.AppendChild(doc.CreateTextNode("20"))
        Dim lat As XmlElement = doc.CreateElement("latitude")
        lat.AppendChild(doc.CreateTextNode("6.5"))
        Dim alt As XmlElement = doc.CreateElement("altitude")
        alt.AppendChild(doc.CreateTextNode("4500000"))
        Dim altmode As XmlElement = doc.CreateElement("altitudeMode")
        altmode.AppendChild(doc.CreateTextNode("relativeToGround"))

        LookAt.AppendChild(lgt)
        LookAt.AppendChild(lat)
        LookAt.AppendChild(alt)
        LookAt.AppendChild(altmode)
        dnode.AppendChild(LookAt)

        Dim Count As Integer = dt.Rows.Count
        Dim valcount As Integer = 0

        Select Case KMLType
            Case Me.KML_Adaptations

                While Count

                    '------Start Creating Styles in KML File For Adaptations.kml-----------

                    Dim rstyle As XmlElement = doc.CreateElement("Style")
                    rstyle.SetAttribute("id", Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Country_Name)), String.Empty, dt.Rows(valcount)(C_Country_Name))))
                    Dim ristyle As XmlElement = doc.CreateElement("IconStyle")
                    ristyle.SetAttribute("id", "UserIcon")
                    Dim ricon As XmlElement = doc.CreateElement("Icon")
                    Dim riconhref As XmlElement = doc.CreateElement("href")

                    riconhref.AppendChild(doc.CreateTextNode(Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Adaptation_Logo)), String.Empty, dt.Rows(valcount)(C_Adaptation_Logo)))))
                    rstyle.AppendChild(ristyle)
                    ricon.AppendChild(riconhref)
                    ristyle.AppendChild(ricon)
                    dnode.AppendChild(rstyle)
                    Count = Count - 1
                    valcount = valcount + 1
                End While


                '------End of Creating Styles in KML File For Adaptations.kml------------

                Exit Select

            Case Me.KML_OnlineUsers

                '-----Start Creating Styles in KML File For OnlineUsers.kml---------

                Dim rstyle As XmlElement = doc.CreateElement("Style")
                rstyle.SetAttribute("id", "User")
                Dim ristyle As XmlElement = doc.CreateElement("IconStyle")
                ristyle.SetAttribute("id", "UserIcon")
                Dim ricon As XmlElement = doc.CreateElement("Icon")
                Dim riconhref As XmlElement = doc.CreateElement("href")
                riconhref.AppendChild(doc.CreateTextNode(strBaseDirectory_UserIcon))
                rstyle.AppendChild(ristyle)
                ricon.AppendChild(riconhref)
                ristyle.AppendChild(ricon)
                dnode.AppendChild(rstyle)

                Exit Select

                '-----End of Creating Styles in KML File For OnlineUsers.kml--------

            Case Me.KML_RegisteredUsers

                '-----Start Creating Styles in KML File For Activations.kml---------

                Dim rstyle As XmlElement = doc.CreateElement("Style")
                rstyle.SetAttribute("id", "User")
                Dim ristyle As XmlElement = doc.CreateElement("IconStyle")
                ristyle.SetAttribute("id", "UserIcon")
                Dim ricon As XmlElement = doc.CreateElement("Icon")
                Dim riconhref As XmlElement = doc.CreateElement("href")
                riconhref.AppendChild(doc.CreateTextNode(strBaseDirectory_ActivationIcon))
                rstyle.AppendChild(ristyle)
                ricon.AppendChild(riconhref)
                ristyle.AppendChild(ricon)
                dnode.AppendChild(rstyle)

                Exit Select
                '-----End of Creating Styles in KML File For Activations.kml------
        End Select
        valcount = 0
        Count = dt.Rows.Count
        Dim KMLString As String

        Select Case KMLType

            Case Me.KML_Adaptations                 '-----Setting Properties For ADAPTATIONS
                KMLpath = strBaseDirectory_Adaptation
                While Count                         '-----Start Creating Table and Placemark in Adaptation.kml File
                    CountryId = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Country_Id)), String.Empty, dt.Rows(valcount)(C_Country_Id)))
                    CountryName = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Country_Name)), String.Empty, dt.Rows(valcount)(C_Country_Name)))
                    Longitude = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Longitude)), String.Empty, dt.Rows(valcount)(C_Longitude)))
                    Latitude = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Latitude)), String.Empty, dt.Rows(valcount)(C_Latitude)))
                    AdaptationName = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Adaptation_Name)), String.Empty, dt.Rows(valcount)(C_Adaptation_Name)))
                    AdaptationLogo = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Adaptation_Logo)), String.Empty, dt.Rows(valcount)(C_Adaptation_Logo)))
                    ImplementingAgency = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Implementing_Agency)), String.Empty, dt.Rows(valcount)(C_Implementing_Agency)))
                    Homepage = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Homepage)), String.Empty, dt.Rows(valcount)(C_Homepage)))
                    Databases = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Databases)), String.Empty, dt.Rows(valcount)(C_Databases)))
                    Website = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Website)), String.Empty, dt.Rows(valcount)(C_Website)))
                    CountryFactSheetlink = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_countryFactSheetlink)), String.Empty, dt.Rows(valcount)(C_countryFactSheetlink)))
                    LastUpdated = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_LastUpdated)), String.Empty, dt.Rows(valcount)(C_LastUpdated)))
                    FocalPointEmail = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Focalpoint_Emails)), String.Empty, dt.Rows(valcount)(C_Focalpoint_Emails)))
                    FocalPoint = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Focalpoint)), String.Empty, dt.Rows(valcount)(C_Focalpoint)))
                    KMLString = CreateTable(KMLType)                          '----Create table in KML File-------
                    Me.CreatePlacemark(doc, KMLString, dnode, KMLType)        '----Create Placemark in KML File---

                    Count = Count - 1
                    valcount = valcount + 1

                End While                          '-------End of Creating Table and Placemark in Adaptation.kml File------
                doc.Save(KMLpath)
                Exit Select
            Case Me.KML_OnlineUsers                '-------Setting Properties FOR ONLINEUSERS
                KMLpath = strBaseDirectory_OnlineUsers
                While Count                        '-------Start Creating Table and Placemark in RegisteredUsers.kml and OnLineUsers.kml File-------
                    Username = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_login)), String.Empty, dt.Rows(valcount)(C_login)))
                    Organization = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Organization)), String.Empty, dt.Rows(valcount)(C_Organization)))
                    'OrganizationType = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_OrganizationType)), String.Empty, dt.Rows(valcount)(C_OrganizationType)))
                    CountryName = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Country_Name)), String.Empty, dt.Rows(valcount)(C_Country_Name)))
                    Longitude = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Longitude)), String.Empty, dt.Rows(valcount)(C_Longitude)))
                    Latitude = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Latitude)), String.Empty, dt.Rows(valcount)(C_Latitude)))
                    Email = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Email)), String.Empty, dt.Rows(valcount)(C_Email)))
                    KMLString = CreateTable(KMLType)                          '-----Create table in KML File-------
                    Me.CreatePlacemark(doc, KMLString, dnode, KMLType)        '-----Create Placemark in KML File---
                    Count = Count - 1
                    valcount = valcount + 1
                End While                          '-------End of Creating Table and Placemark in RegisteredUsers.kml and OnLineUsers.kml File-------
                doc.Save(KMLpath)
                Exit Select
            Case Me.KML_RegisteredUsers            '-------Setting Properties FOR REGISTERED USERS-------
                KMLpath = strBaseDirectory_Activation

                While Count                        '-------Start Creating Table and Placemark in RegisteredUsers.kml and OnLineUsers.kml File--------
                    Username = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_login)), String.Empty, dt.Rows(valcount)(C_login)))
                    Organization = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Organization)), String.Empty, dt.Rows(valcount)(C_Organization)))
                    CountryName = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Country_Name)), String.Empty, dt.Rows(valcount)(C_Country_Name)))
                    Longitude = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Longitude)), String.Empty, dt.Rows(valcount)(C_Longitude)))
                    Latitude = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Latitude)), String.Empty, dt.Rows(valcount)(C_Latitude)))
                    Email = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Email)), String.Empty, dt.Rows(valcount)(C_Email)))
                    AdaptationName = Trim(IIf(IsDBNull(dt.Rows(valcount)(C_Adaptation_Name)), String.Empty, dt.Rows(valcount)(C_Adaptation_Name)))
                    Isonline = Trim(IIf(IsDBNull(dt.Rows(valcount)("Isonline")), String.Empty, dt.Rows(valcount)("Isonline")))
                    KMLString = CreateTable(KMLType)                          '------Create table in KML File----------
                    Me.CreatePlacemark(doc, KMLString, dnode, KMLType)        '------Create Placemark in KML File-------

                    Count = Count - 1
                    valcount = valcount + 1

                End While                          '-------End of Creating Table and Placemark in RegisteredUsers.kml and OnLineUsers.kml File-------

                doc.Save(KMLpath)
                Exit Select
        End Select
        Return ""
    End Function

    'Generate Placemark into KML File
    Public Sub CreatePlacemark(ByRef doc As XmlDocument, ByVal kmlstring As String, ByRef dnode As XmlElement, ByVal KMLType As String)

        Select Case KMLType
            Case Me.KML_Adaptations                     '----Generate Placemark into Adapatations.KML File----------
                Dim placemark As XmlElement = doc.CreateElement("Placemark")
                dnode.AppendChild(placemark)
                Dim name1 As XmlElement = doc.CreateElement("name")
                name1.AppendChild(doc.CreateTextNode(AdaptationName))
                placemark.AppendChild(name1)
                Dim descrip As XmlElement = doc.CreateElement("description")
                descrip.AppendChild(doc.CreateCDataSection(kmlstring))
                'descrip.AppendChild(doc.CreateTextNode(CountryName))
                placemark.AppendChild(descrip)
                Dim styleUrl As XmlElement = doc.CreateElement("styleUrl")
                styleUrl.AppendChild(doc.CreateTextNode("#" + CountryName))
                placemark.AppendChild(styleUrl)
                Dim point As XmlElement = doc.CreateElement("Point")
                Dim coordinates As XmlElement = doc.CreateElement("coordinates")
                coordinates.AppendChild(doc.CreateTextNode((Longitude & ",") + Latitude))
                point.AppendChild(coordinates)
                placemark.AppendChild(point)

                Exit Select

            Case Me.KML_OnlineUsers, Me.KML_RegisteredUsers     '----Generate Placemark into OnlineUsers.kml and Activations.KML File----------
                Dim placemark As XmlElement = doc.CreateElement("Placemark")
                dnode.AppendChild(placemark)
                Dim name1 As XmlElement = doc.CreateElement("name")
                name1.AppendChild(doc.CreateTextNode(Me.Username))
                placemark.AppendChild(name1)
                Dim descrip As XmlElement = doc.CreateElement("description")
                descrip.AppendChild(doc.CreateCDataSection(kmlstring))
                'descrip.AppendChild(doc.CreateTextNode(CountryName))
                placemark.AppendChild(descrip)
                Dim styleUrl As XmlElement = doc.CreateElement("styleUrl")
                styleUrl.AppendChild(doc.CreateTextNode("#" + "User"))
                placemark.AppendChild(styleUrl)
                Dim point As XmlElement = doc.CreateElement("Point")
                Dim coordinates As XmlElement = doc.CreateElement("coordinates")
                coordinates.AppendChild(doc.CreateTextNode((Longitude & ",") + Latitude))
                point.AppendChild(coordinates)
                placemark.AppendChild(point)

                Exit Select

        End Select

    End Sub

    'Generating Table into KML file For all Adaptations
    Public Function CreateTable(ByVal KMLType As String) As String

        Dim sFile As String
        sFile = Path.GetTempPath()
        Dim LogFile As FileInfo
        Me.LogFilePath = sFile + "GenerateTable.html"
        LogFile = New FileInfo(Me.LogFilePath)
        Me.StringWriter = LogFile.CreateText()
        Dim content As String = String.Empty
        Select Case KMLType
            Case Me.KML_Adaptations                         '-------FOR ADAPTATIONS--------
                Me.Writer = New HtmlTextWriter(StringWriter)
                Me.InsertStartTableTag()
                Me.InsertStartRowTag()
                Me.InsertStartCellTag()
                Me.InsertStartCellTagWBodyText()
                Me.InsertStartDiv()

                '1) Last updated on
                Me.InsertSpan("Last updated on " + Me.LastUpdated, "source")
                Me.InsertBreakTag()
                Me.InsertEndDiv()
                Me.InsertEndCellTag()
                Me.InsertEndRowTag()

                Me.InsertStartRowTag()
                Me.InsertBreakTag()
                Me.InsertStartCellTagWBodyText()
                Me.InsertStrongTag()

                '2) Country Logo
                Me.InsertSpanWImage("Header", Me.AdaptationLogo)

                Me.InsertSpan("&nbsp;&nbsp;" + Me.CountryName, "Header")
                Me.InsertBreakTag()
                Me.InsertEndStrongTag()
                Me.InsertEndCellTag()
                Me.InsertEndRowTag()

                '3) Implementing Agency
                Me.InsertRowWithHeader("Implementing Agency ", Me.ImplementingAgency)

                '4) Homepage
                Dim HomePage_data As String = Me.Homepage
                Dim HomePage_Result As Boolean = False

                If (String.IsNullOrEmpty(HomePage_data) Or HomePage_data.StartsWith("Not")) Then
                    HomePage_Result = True
                End If

                If HomePage_Result = True Then
                    Me.InsertRowWithHeader("HomePage", Me.Homepage)
                Else
                    Me.InsertBreakTag()
                    Me.InsertStartRowTag()
                    Me.InsertStartCellTagWBodyText()
                    Me.InsertStrongTag()
                    Me.Writer.Write("Home Page")
                    Me.InsertEndStrongTag()
                    Me.InsertEndCellTag()
                    Me.InsertStartCellTagWBodyText()
                    Me.InsertImageWWidthandHeight(Me.Homepage, "150", "81")
                    Me.InsertEndCellTag()
                    Me.InsertEndRowTag()
                    Me.InsertBreakTag()
                End If

                '5) DATABASES
                Me.InsertRowWithHeader("Databases", Me.Databases)

                '6) Web-Site
                Me.InsertRowWithHeader("Web-Site", Me.Website)

                '7) Focal Point
                Me.InsertRowWithHeader("Focal Point ", Me.FocalPoint)

                '8) Focal Point Email
                Me.InsertRowWithA("E-mail ", "mailto:" + FocalPointEmail, " " + FocalPointEmail)

                '9) Country FactSheet

                'Me.InsertRowWithA("Country FactSheet ", "http://www.devinfo.org/factsheets/" + Me.CountryId + "/" + Me.CountryFactSheetlink + ".htm?IDX=3", " " + Me.CountryFactSheetlink)
                Me.InsertRowWithA("Country FactSheet ", Me.CountryFactSheetlink, " " + Me.CountryFactSheetlink)
                Me.InsertBreakTag()

                'Me.InsertEndTableTag()
                Me.InsertEndCellTag()
                Me.InsertEndRowTag()
                Me.InsertEndTableTag()
                Me.Writer.Close()

                Dim sr As New StreamReader(Me.LogFilePath)
                content = sr.ReadToEnd()
                sr.Close()

                Exit Select

            Case Me.KML_RegisteredUsers                 '------ Create Table FOR REGISTERED USERS -------
                content = CreateUserTableForRegUsers()

                Exit Select

            Case Me.KML_OnlineUsers                     '------ Create Table FOR ONLINE USERS -----------
                content = CreateUserTable()

                Exit Select

        End Select

        LogFile.Delete()
        Return content
    End Function

    'Create Table into KML file For Online users
    Public Function CreateUserTable()
        Dim content As String
        Me.Writer = New HtmlTextWriter(StringWriter)
        Me.InsertStartTableTag()

        '1) Participant
        Me.InsertUserRow("Participant ", Me.Username)

        '2) Organization
        Me.InsertUserRow("Organization ", Me.Organization)

        '3) Area
        Me.InsertUserRow("Country ", Me.CountryName)

        '4) Organization Type
        'Me.InsertUserRow("Organization Type ", "")

        '5) E-Mail of User
        Me.InsertRowWithA("E-Mail ", "mailto: " + Me.Email, " " + Me.Email)
        Me.InsertEndTableTag()

        Me.Writer.Close()
        Dim sr As New StreamReader(Me.LogFilePath)
        content = sr.ReadToEnd()
        sr.Close()
        Return content
    End Function

    'Create Table into KML file For Reg. Users 
    Public Function CreateUserTableForRegUsers()
        Dim content As String
        Dim Status As String
        Me.Writer = New HtmlTextWriter(StringWriter)
        Me.InsertStartTableTag()

        '1) Participant
        Me.InsertUserRow("Participant ", Me.Username)

        '2) Organization
        Me.InsertUserRow("Organization ", Me.Organization)

        '3) Area
        Me.InsertUserRow("Country ", Me.CountryName)

        '4) Organization Type
        'Me.InsertUserRow("Organization Type ", "")

        '5) E-Mail of User
        Me.InsertRowWithA("E-Mail ", "mailto: " + Me.Email, " " + Me.Email)

        '6) Online Status
        If Me.Isonline = True Then
            Status = "Available"
        Else
            Status = "Not Available"
        End If
        Me.InsertUserRow("Online Status ", Status)

        '7) Adaptation
        Me.InsertUserRow("Adaptation ", Me._AdaptationName)
        Me.InsertEndTableTag()

        Me.Writer.Close()
        Dim sr As New StreamReader(Me.LogFilePath)
        content = sr.ReadToEnd()
        sr.Close()
        Return content
    End Function

    Public Function CreateHTMLTable_StatsCounts()
        'Dim sFile As String = strBaseDirectory_StatsHTML
        'Dim LogFile As FileInfo
        'Dim LogPath As String = sFile
        'LogFile = New FileInfo(LogPath)
        Dim sFile As String
        sFile = Path.GetTempPath()
        Dim LogFile As FileInfo
        Me.LogFilePath = sFile + "HTMLTable.html"
        LogFile = New FileInfo(Me.LogFilePath)

        Me.StringWriter = LogFile.CreateText()
        Dim content As String = String.Empty
        Me.Writer = New HtmlTextWriter(StringWriter)
        Dim CSSPath As String = HttpContext.Current.Request.MapPath("Stock\css\StatsHTML.css")
        GenerateStyleCSS(CSSPath)
        Me.StartTableTag_StatsHTML()
        Me.InsertStartRowTag()
        Me.StartTDTagWClass("StatCaption")      'Start td
        Me.Writer.Write("DevInfo Adaptations")
        Me.InsertEndCellTag()                   'End TD
        Me.StartTDTagWClass("StatValue")        'Start td
        Me.Writer.Write(Me.AdaptationsCount)
        Me.InsertEndCellTag()                   'End TD
        Me.InsertEndRowTag()


        Me.InsertStartRowTag()                  'Start tr
        Me.StartTDTagWClass("StatCaption")      'Start td
        Me.Writer.Write("DevInfo Online Users")
        Me.InsertEndCellTag()                   'End TD
        Me.StartTDTagWClass("StatValue")        'Start td
        Me.Writer.Write(Me.OnlineUsersCount)
        Me.InsertEndCellTag()                   'End TD
        Me.InsertEndRowTag()

        Me.InsertStartRowTag()                  'Start tr
        Me.StartTDTagWClass("StatCaption")      'Start td
        Me.Writer.Write("DevInfo Activations")
        Me.InsertEndCellTag()                   'End TD
        Me.StartTDTagWClass("StatValue")        'Start td
        Me.Writer.Write(Me.ActivationsCount)
        Me.InsertEndCellTag()                   'End TD
        Me.InsertEndRowTag()

        Me.InsertEndTableTag()
        Me.Writer.Close()
        Dim _Content As String = String.Empty
        'Dim sr As New StreamReader(LogPath)
        Dim sr As New StreamReader(LogFilePath)
        _Content = sr.ReadToEnd()
        sr.Close()
        Return _Content
    End Function

#End Region

#End Region


#Region "Set Column Names"
    'Set Column Names
    Dim C_Country_NId As String = "Country_NId"
    Dim C_Country_Id As String = "Country_Id"
    Dim C_RegionNid As String = "Region_NId"
    Dim C_User_NId As String = "User_NId"
    Dim C_Adaptation_NId As String = "Adaptation_NId"
    Dim C_Country_Name As String = "Country_Name"
    Dim C_Longitude As String = "Longitude"
    Dim C_Latitude As String = "Latitude"
    Dim C_Adaptation_Name As String = "Adaptation_Name"
    Dim C_Adaptation_Logo As String = "CountryFlag_URL"
    Dim C_Implementing_Agency As String = "Implementing_Agency"
    Dim C_Homepage As String = "Homepage_URL"
    Dim C_Databases As String = "Databases"
    Dim C_Website As String = "Website"
    Dim C_countryFactSheetlink As String = "countryFactSheet_URL"
    Dim C_LastUpdated As String = "LastUpdated"
    Dim C_Focalpoint_Emails As String = "Focalpoint_Emails"
    Dim C_Focalpoint As String = "Focalpoints"
    Dim C_login As String = "login"
    Dim C_Organization As String = "Organization"
    Dim C_OrganizationType As String = "Organization_Type"
    Dim C_Email As String = "Email"
    Dim C_Isonline As String = "Isonline"
#End Region


    Private Sub InsertUserRow(ByVal Header1 As String, ByVal Header2 As String)
        Me.InsertStartRowTag()
        Me.InsertStartCellTagWBodyText()
        Me.InsertStrongTag()
        Me.Writer.Write(Header1)
        Me.InsertEndStrongTag()
        Me.InsertEndCellTag()

        Me.InsertStartCellTagWBodyText()
        Me.Writer.Write(Header2)
        Me.InsertEndCellTag()
        Me.InsertEndRowTag()
    End Sub

    Private Sub InsertBreakTag()
        ' Me.Writer.WriteBreak()
    End Sub

    'Start HREf Tag
    Private Sub InsertHREfATAg(ByVal HrefSource As String, ByVal HeaderValue As String)
        Me.Writer.WriteBeginTag("a ")
        Me.Writer.WriteAttribute("href", HrefSource)
        Me.Writer.WriteAttribute("target", "_blank")

        Me.Writer.Write(HtmlTextWriter.TagRightChar)
        Me.Writer.Write(HeaderValue)
        Me.Writer.WriteEndTag("a")
    End Sub

    Private Sub InsertRowWithA(ByVal Header1 As String, ByVal HrefSource As String, ByVal HeaderValue As String)
        Me.InsertStartRowTag()
        Me.InsertStartCellTagWBodyText()
        Me.InsertStrongTag()
        Me.Writer.Write(Header1)
        Me.InsertEndStrongTag()
        Me.InsertEndCellTag()
        Me.InsertStartCellTagWBodyText()
        Me.InsertHREfATAg(HrefSource, HeaderValue)
        'Me.Writer.Write(HeaderValue)
        Me.InsertEndCellTag()
        Me.InsertEndRowTag()
    End Sub


    Private Sub InsertRowWithHeader(ByVal Header1 As String, ByVal Header2 As String)
        Me.InsertStartRowTag()
        Me.InsertStartCellTagWBodyText()
        Me.InsertStrongTag()
        Me.Writer.Write(Header1)
        Me.InsertEndStrongTag()
        Me.InsertEndCellTag()
        Me.InsertStartCellTagWBodyText()
        Me.Writer.Write(Header2)
        Me.InsertEndCellTag()
        Me.InsertEndRowTag()
    End Sub


    Private Sub InsertEndDiv()
        Me.Writer.WriteEndTag("DIV")
    End Sub

    'Start TABLE Tag
    Private Sub InsertStartTableTag()
        Me.Writer.WriteBeginTag("TABLE")
        Me.Writer.WriteAttribute("Width", "100%")
        Me.Writer.WriteAttribute("cellspacing", "0")
        Me.Writer.WriteAttribute("cellpadding", "5")
        Me.Writer.Write(HtmlTextWriter.TagRightChar)
    End Sub

    'End TABLE Tag
    Private Sub InsertEndTableTag()
        Me.Writer.WriteEndTag("TABLE")
    End Sub

    'Start TR Tag
    Private Sub InsertStartRowTag()
        Me.Writer.WriteBeginTag("TR")
        Me.Writer.Write(HtmlTextWriter.TagRightChar)
    End Sub

    'End TABLE Tag
    Private Sub InsertEndRowTag()
        Me.Writer.WriteEndTag("TR")
    End Sub

    'Start TD Tag
    Private Sub InsertStartCellTag()
        Me.Writer.WriteBeginTag("TD")
        Me.Writer.WriteAttribute("valign", "top")
        Me.Writer.Write(HtmlTextWriter.TagRightChar)
    End Sub

    'Start TD Tag with Class
    Private Sub InsertStartCellTagWBodyText()
        Me.Writer.WriteBeginTag("TD")
        Me.Writer.WriteAttribute("class", "bodytext")
        Me.Writer.Write(HtmlTextWriter.TagRightChar)
    End Sub

    'End TD Tag
    Private Sub InsertEndCellTag()
        Me.Writer.WriteEndTag("TD")
    End Sub

    'Start Span Tag
    Private Sub InsertSpan(ByVal Header As String, ByVal ClassName As String)
        Me.Writer.WriteBeginTag("Span")
        Me.Writer.WriteAttribute("class", ClassName)
        Me.Writer.Write(HtmlTextWriter.TagRightChar)
        Me.Writer.Write(Header)
        Me.Writer.WriteEndTag("Span")

    End Sub

    'Start Div Tag
    Private Sub InsertStartDiv()
        Me.Writer.WriteBeginTag("Div")
        Me.Writer.WriteAttribute("align", "right")
        Me.Writer.Write(HtmlTextWriter.TagRightChar)
    End Sub

    'Start Span Tag with Image
    Private Sub InsertSpanWImage(ByVal ClassName As String, ByVal ImageSource As String)
        Me.Writer.WriteBeginTag("Span ")
        Me.Writer.WriteAttribute("class", ClassName)
        Me.Writer.Write(HtmlTextWriter.TagRightChar)
        Me.InsertImage(ImageSource)
        Me.Writer.WriteEndTag("Span")


    End Sub

    'Start Strong Tag
    Private Sub InsertStrongTag()
        Me.Writer.WriteBeginTag("Strong")
        Me.Writer.Write(HtmlTextWriter.TagRightChar)
    End Sub

    'End Strong Tag
    Private Sub InsertEndStrongTag()
        Me.Writer.WriteEndTag("Strong")
    End Sub

    'Start Image Tag
    Private Sub InsertImage(ByVal source As String)
        Me.Writer.WriteBeginTag("img")
        Me.Writer.WriteAttribute("src", source)
        Me.Writer.Write(HtmlTextWriter.TagRightChar)
        Me.Writer.WriteEndTag("img")
    End Sub

    'Start Image Tag with Width and Height
    Private Sub InsertImageWWidthandHeight(ByVal source As String, ByVal width As String, ByVal height As String)
        Me.Writer.WriteBeginTag("img")
        Me.Writer.WriteAttribute("src", source)
        Me.Writer.WriteAttribute("width", width)
        Me.Writer.WriteAttribute("height", height)
        Me.Writer.Write(HtmlTextWriter.TagRightChar)
        Me.Writer.WriteEndTag("img")
    End Sub


    Private Sub StartTableTag_StatsHTML()
        Me.Writer.WriteBeginTag("Table")
        Me.Writer.Write(HtmlTextWriter.TagRightChar)
    End Sub

    Private Sub StartTDTagWClass(ByVal ClassName As String)
        Me.Writer.WriteBeginTag("TD")
        Me.Writer.WriteAttribute("class", ClassName)
        Me.Writer.Write(HtmlTextWriter.TagRightChar)
    End Sub

    Private Sub GenerateStyleCSS(ByVal path As String)
        Dim content As String = String.Empty
        Me.Writer.WriteBeginTag("STYLE")
        Me.Writer.WriteAttribute("TYPE", "'" & "text/css" & "'")
        Using reader As TextReader = New StreamReader(path)
            'Now read the entire file at once into our variable
            content = reader.ReadToEnd()
        End Using

        Me.Writer.Write(HtmlTextWriter.TagRightChar)
        Me.Writer.Write(content)

        Me.Writer.WriteEndTag("STYLE")

    End Sub

    Private Sub EndStyleTag()
        Me.Writer.WriteEndTag("STYLE")
    End Sub

    'To generate JS file of Adaptation count
    Public Sub GenerateJS()
        Dim sFile As String = strBaseDirectory_Statsjs
        Dim _Content As String = CreateHTMLTable_StatsCounts()
        _Content = _Content.Replace("""", "")
        Dim LogFile As FileInfo
        Dim LogPath As String = sFile
        LogFile = New FileInfo(LogPath)
        Me.StringWriter = LogFile.CreateText()
        Me.Writer = New HtmlTextWriter(StringWriter)
        Me.Writer.Write("document.write(""")
        Me.Writer.Write(_Content)
        Me.Writer.Write(""")")
        Me.Writer.Close()
    End Sub
End Class




