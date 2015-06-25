Imports di_Worldwide.DIDBConstants
Public Class InsertQuery
    ''' <summary>
    ''' Query to insert the Database details into Index Server table
    ''' </summary>
    ''' <param name="DBName">Name of the Database</param>
    ''' <param name="Uname">Username</param>
    ''' <param name="Pwd">Password</param>
    ''' <param name="PortNo">Port No.</param>
    ''' <param name="DatabaseType">Database type</param>
    ''' <returns>Returns the query to insert the Database details into Index Server table</returns>
    ''' <remarks></remarks>
    Friend Shared Function InsertDetails(ByVal ServerName As String, ByVal DBName As String, ByVal Uname As String, ByVal Pwd As String, ByVal PortNo As String, ByVal DatabaseType As String, ByVal AdaptationNID As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "Insert into " & DBDetailsTableName & " (" & DBServerName & "," & DatabaseName & "," & DBUsername & "," & DBPassword & "," & DBPort & "," & DBType & ",AdaptationNID) values ('" & ServerName & "','" & DBName & "','" & Uname & "','" & Pwd & "','" & PortNo & "','" & DatabaseType & "','" & AdaptationNID & "')"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to insert the Adaptation details into the database
    ''' </summary>
    ''' <param name="AdaptationName"></param>
    ''' <param name="AdaptationVersion"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function InsertAdaptation(ByVal AdaptationName As String, ByVal AdaptationVersion As String)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "Insert into Adaptations (AdaptationName,AdaptationVersion) values ('" & AdaptationName & "','" & AdaptationVersion & "')"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to insert the Dataset details
    ''' </summary>
    ''' <param name="DSName">Dataset Name</param>
    ''' <param name="DbIndex">Database Index</param>
    ''' <param name="metadata_desc">Database Description</param>
    ''' <param name="pub_date">Published Date</param>
    ''' <param name="ind_Count">Indicator Count</param>
    ''' <param name="area_Count">Area Count</param>
    ''' <param name="timeperiod_Count">Timeperiod Count</param>
    ''' <param name="ius_Count">IUS Count</param>
    ''' <param name="source_Count">Source Count</param>
    ''' <param name="data_Count">Data Count</param>
    ''' <returns>Returns the query to insert the Dataset details</returns>
    ''' <remarks></remarks>
    Friend Shared Function InsertDSDetails(ByVal dsPrefix As String, ByVal DSName As String, ByVal langCode As String, ByVal DbIndex As Integer, ByVal metadata_desc As String, ByVal pub_date As String, ByVal ind_Count As Decimal, ByVal area_Count As Decimal, ByVal timeperiod_Count As Decimal, ByVal ius_Count As Decimal, ByVal source_Count As Decimal, ByVal data_Count As Decimal) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "Insert into " & DBRegisteredDatasets & " (" & DataSetPrefix & "," & DataSetName & ",LanguageCode," & DBNID & "," & Metadata_Description & "," & Metadata_PubDate & "," & AreaCount & "," & IndCount & "," & IUSCount & "," & TPCount & "," & SourceCount & "," & DataCount & ")"
            RetVal &= " values ('" & dsPrefix & "','" & DSName & "','" & langCode & "'," & DbIndex & ",'" & metadata_desc & "','" & pub_date & "'," & area_Count & "," & ind_Count & "," & ius_Count & "," & timeperiod_Count & "," & source_Count & "," & data_Count & ")"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to insert the Indicator details
    ''' </summary>
    ''' <param name="IndName">Indicator Name</param>
    ''' <param name="IndGid">Indicator Gid</param>
    ''' <param name="DSIndex">Dataset Index</param>
    ''' <returns>Returns the query to insert the Indicator details</returns>
    ''' <remarks></remarks>
    Friend Shared Function InsertIndicator(ByVal IndName As String, ByVal IndGid As String, ByVal DSIndex As String)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "Insert into " & Indicator & " values ('" & IndName & "','" & IndGid & "','" & DSIndex & "')"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to insert the Area details
    ''' </summary>
    ''' <param name="AreaName">Area Name</param>
    ''' <param name="AreaGid">Area Gid</param>
    ''' <param name="DSIndex">Dataset Index</param>
    ''' <returns>Returns the query to insert the Area details</returns>
    ''' <remarks></remarks>
    Friend Shared Function InsertArea(ByVal AreaName As String, ByVal AreaGid As String, ByVal DSIndex As String)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "Insert into " & Area & " values ('" & AreaName & "','" & AreaGid & "','" & DSIndex & "')"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to insert the Unit details
    ''' </summary>
    ''' <param name="UnitName">Unit Name</param>
    ''' <param name="UnitGid">Unit Gid</param>
    ''' <param name="DSIndex">Dataset Index</param>
    ''' <returns>Returns the query to insert the Unit details</returns>
    ''' <remarks></remarks>
    Friend Shared Function InsertUnit(ByVal UnitName As String, ByVal UnitGid As String, ByVal DSIndex As String)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "Insert into " & Unit & " values ('" & UnitName & "','" & UnitGid & "','" & DSIndex & "')"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to insert the Source details
    ''' </summary>
    ''' <param name="SourceName">Source Name</param>
    ''' <param name="SourceGid">Source Gid</param>
    ''' <param name="DSIndex">Dataset Index</param>
    ''' <returns>Returns the query to insert the Source details</returns>
    ''' <remarks></remarks>
    Friend Shared Function InsertSource(ByVal SourceName As String, ByVal SourceGid As String, ByVal DSIndex As String)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "Insert into " & Source & " values ('" & SourceName & "','" & SourceGid & "','" & DSIndex & "')"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to insert the Subgroup details
    ''' </summary>
    ''' <param name="SubgroupName">Subgroup Name</param>
    ''' <param name="SubgroupGid">Subgroup Gid</param>
    ''' <param name="DSIndex">Dataset Index</param>
    ''' <returns>Returns the query to insert the Subgroup details</returns>
    ''' <remarks></remarks>
    Friend Shared Function InsertSubgroup(ByVal SubgroupName As String, ByVal SubgroupGid As String, ByVal DSIndex As String)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "Insert into " & Subgroup & " values ('" & SubgroupName & "','" & SubgroupGid & "','" & DSIndex & "')"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to insert the TimePeriod details
    ''' </summary>
    ''' <param name="TimPeriod">Time Period</param>
    ''' <param name="DSIndex">Dataset Index</param>
    ''' <returns>Returns the query to insert the TimePeriod details</returns>
    ''' <remarks></remarks>
    Friend Shared Function InsertTimePeriod(ByVal TimPeriod As String, ByVal DSIndex As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "Insert into " & TimePeriod & " values ('" & TimPeriod & "','" & DSIndex & "')"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to insert the search results into the database
    ''' </summary>
    ''' <param name="keywords"></param>
    ''' <param name="XMLGenerated"></param>
    ''' <param name="DSIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function InsertSearchResults(ByVal keywords As String, ByVal XMLGenerated As String, ByVal DSIndex As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "insert into " & DBSearchResults & " (Keywords,XMLFile,DSIndex) values ('" & keywords & "','" & XMLGenerated & "','" & DSIndex & "')"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function

End Class