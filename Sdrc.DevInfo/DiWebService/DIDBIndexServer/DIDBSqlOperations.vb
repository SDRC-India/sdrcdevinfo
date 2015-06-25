Imports System.Xml
Imports System.Xml.Serialization
Imports System.IO

Imports di_Worldwide.InsertQuery
Imports di_Worldwide.Enums

Imports DevInfo.Lib.DI_LibDAL.Connection
Imports DevInfo.Lib.DI_LibDAL.Queries
Imports DevInfo.Lib.DI_LibDAL.Queries.DIColumns


Public Class DIDBSqlOperations

    Private _IndexDBName As String = ConfigurationManager.AppSettings.Item("DIIndexDatabaseName")
    Public Property IndexDBName() As String
        Get
            Return _IndexDBName
        End Get
        Set(ByVal value As String)
            _IndexDBName = value
        End Set
    End Property


    Private _IndexDBUsername As String = ConfigurationManager.AppSettings.Item("DIIndexServerUsername")
    Public Property IndexDBUsername() As String
        Get
            Return _IndexDBUsername
        End Get
        Set(ByVal value As String)
            _IndexDBUsername = value
        End Set
    End Property


    Private _IndexDBPassword As String = ConfigurationManager.AppSettings.Item("DIIndexServerPassword")
    Public Property IndexDBPassword() As String
        Get
            Return _IndexDBPassword
        End Get
        Set(ByVal value As String)
            _IndexDBPassword = value
        End Set
    End Property


    Private _IndexDBServerName As String = ConfigurationManager.AppSettings.Item("DIIndexServerName")
    Public Property IndexDBServerName() As String
        Get
            Return _IndexDBServerName
        End Get
        Set(ByVal value As String)
            _IndexDBServerName = value
        End Set
    End Property


    Private _IndexDBServerType As DIServerType
    Public Property IndexDBServerType() As DIServerType
        Get
            Return _IndexDBServerType
        End Get
        Set(ByVal value As DIServerType)
            _IndexDBServerType = value
        End Set
    End Property


    Private _IndexDBServerPortNo As String = ConfigurationManager.AppSettings.Item("DIIndexServerPort")
    Public Property IndexDBServerPortNo() As String
        Get
            Return _IndexDBServerPortNo
        End Get
        Set(ByVal value As String)
            _IndexDBServerPortNo = value
        End Set
    End Property

    Private _SearchText As String = String.Empty
    Public Property SearchText() As String
        Get
            Return _SearchText
        End Get
        Set(ByVal value As String)
            _SearchText = value
        End Set
    End Property

    Private DIConnection As DIConnection
    Private DIQueries As DIQueries
    Const WordCount As String = "WordCount"
    Const SearchKeyword As String = "SearchKeyword"
    Private KeywordDatasetPair As New Dictionary(Of String, String)


    Private _SearchedDSIndexes As String = String.Empty
    Public Property SearchedDSIndexes() As String
        Get
            Return _SearchedDSIndexes
        End Get
        Set(ByVal value As String)
            _SearchedDSIndexes = value
        End Set
    End Property


#Region "-- Public --"

    ''Public Sub Save(ByVal fileName As String)
    ''    Try
    ''        Dim SqlOperationsSerialize As New XmlSerializer(GetType(DIDBSqlOperations))
    ''        Dim SqlOperationsWriter As New StreamWriter(fileName)
    ''        SqlOperationsSerialize.Serialize(SqlOperationsWriter, Me)
    ''        SqlOperationsWriter.Close()
    ''    Catch ex As Exception
    ''    End Try
    ''End Sub

    ''Public Shared Function Load(ByVal fileName As String, ByVal languageFolder As String) As DIDBSqlOperations
    ''    Dim Retval As DIDBSqlOperations
    ''    Dim SqlOperationsReader As StreamReader = Nothing
    ''    Try
    ''        Dim SqlOperations As New XmlSerializer(GetType(DIDBSqlOperations))
    ''        SqlOperationsReader = New StreamReader(fileName)
    ''        Retval = DirectCast(SqlOperations.Deserialize(SqlOperationsReader), DIDBSqlOperations)

    ''        SqlOperationsReader.Close()
    ''        SqlOperationsReader.Dispose()
    ''    Catch ex As Exception
    ''        Retval = Nothing
    ''        If SqlOperationsReader IsNot Nothing Then
    ''            SqlOperationsReader.Dispose()
    ''        End If
    ''    End Try
    ''    Return (Retval)
    ''End Function


    ''' <summary>
    ''' Set the Database details in the Database Index Server
    ''' </summary>
    ''' <param name="ServerName">Name of the Database Server</param>
    ''' <param name="DBName">Name of the Database</param>
    ''' <param name="Username">Username to connect to the Database</param>
    ''' <param name="Password">Password to connect to the Database</param>
    ''' <param name="PortNo">Port No. to connect to the Database</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RegisterDatabase(ByVal dbType As DIServerType, ByVal AdaptationName As String, ByVal AdaptationVersion As String, ByVal ServerName As String, ByVal DBName As String, ByVal Username As String, ByVal Password As String, ByVal PortNo As String) As String
        Dim RetVal As String = String.Empty
        Try
            Dim diCon As New DevInfo.Lib.DI_LibDAL.Connection.DIConnection(DIServerType.SqlServer, IndexDBServerName, IndexDBServerPortNo, IndexDBName, IndexDBUsername, IndexDBPassword)
            Dim isAlreadyRegd As Integer = diCon.ExecuteScalarSqlQuery(SelectQuery.IsDatabaseAlreadyRegistered(DBName))
            If isAlreadyRegd = 0 Then

                Dim conn As New DevInfo.Lib.DI_LibDAL.Connection.DIConnection(dbType, ServerName, PortNo, DBName, Username, Password)

                Dim AdaptationNID As String = Convert.ToInt32(diCon.ExecuteScalarSqlQuery(SelectQuery.CheckAdaptationExists(AdaptationName)))
                If AdaptationNID = "0" Then
                    diCon.ExecuteNonQuery(InsertAdaptation(AdaptationName, AdaptationVersion))
                    AdaptationNID = diCon.ExecuteScalarSqlQuery(SelectQuery.GetLatestNID())
                Else
                    diCon.ExecuteNonQuery(UpdateQuery.UpdateAdaptationVersion(AdaptationName, AdaptationVersion))
                End If

                diCon.ExecuteNonQuery(InsertDetails(ServerName, DBName, Username, Password, PortNo, dbType.ToString(), AdaptationNID))

                Dim InsertedDBIndex As String = diCon.ExecuteScalarSqlQuery(SelectQuery.GetLatestNID())
                Dim InsertedDSIndex As String = String.Empty

                Dim objQuery As DIQueries = Nothing
                Dim dtDIDataset As DataTable = conn.DIDataSets()

                For Each dsRow As DataRow In dtDIDataset.Rows
                    If Not IsNothing(objQuery) Then
                        objQuery.Dispose()
                    End If
                    Dim dtLanguage As DataTable = conn.DILanguages(dsRow(DBAvailableDatabases.AvlDBPrefix).ToString() & "_")

                    For Each LanguageRow As DataRow In dtLanguage.Rows
                        objQuery = New DIQueries(dsRow(DBAvailableDatabases.AvlDBPrefix).ToString() & "_", LanguageRow(Language.LanguageCode).ToString())
                        Dim langCode As String = LanguageRow(Language.LanguageCode).ToString()
                        '' Insert Metadata
                        Dim dtMetadata As DataTable = conn.ExecuteDataTable(objQuery.DBMetadata.GetDBMetadata())
                        If dtMetadata.Rows.Count > 0 Then
                            For Each dRow As DataRow In dtMetadata.Rows
                                If Not IsDBNull(dRow(DBMetaData.PublisherDate)) Then
                                    diCon.ExecuteNonQuery(InsertQuery.InsertDSDetails(dsRow(DBAvailableDatabases.AvlDBPrefix), dsRow(DBAvailableDatabases.AvlDBName).ToString().Replace("'", "''"), langCode, InsertedDBIndex, dRow(DBMetaData.Description).ToString().Replace("'", "''"), dRow(DBMetaData.PublisherDate), Convert.ToDecimal(dRow(DBMetaData.IndicatorCount)), Convert.ToDecimal(dRow(DBMetaData.AreaCount)), Convert.ToDecimal(dRow(DBMetaData.TimeperiodCount)), Convert.ToDecimal(dRow(DBMetaData.IUSCount)), Convert.ToDecimal(dRow(DBMetaData.SourceCount)), Convert.ToDecimal(dRow(DBMetaData.DataCount))))
                                Else
                                    diCon.ExecuteNonQuery(InsertQuery.InsertDSDetails(dsRow(DBAvailableDatabases.AvlDBPrefix), dsRow(DBAvailableDatabases.AvlDBName).ToString().Replace("'", "''"), langCode, InsertedDBIndex, dRow(DBMetaData.Description).ToString().Replace("'", "''"), "", Convert.ToDecimal(dRow(DBMetaData.IndicatorCount)), Convert.ToDecimal(dRow(DBMetaData.AreaCount)), Convert.ToDecimal(dRow(DBMetaData.TimeperiodCount)), Convert.ToDecimal(dRow(DBMetaData.IUSCount)), Convert.ToDecimal(dRow(DBMetaData.SourceCount)), Convert.ToDecimal(dRow(DBMetaData.DataCount))))
                                End If
                                InsertedDSIndex = diCon.ExecuteScalarSqlQuery(SelectQuery.GetLatestNID())
                            Next
                        End If

                        '' Insert Indicators
                        Dim dtInd As DataTable = conn.ExecuteDataTable(objQuery.Indicators.GetIndicator(FilterFieldType.None, String.Empty, FieldSelection.Light))
                        If dtInd.Rows.Count > 0 Then
                            For Each dRow As DataRow In dtInd.Rows
                                Dim IndName As String = dRow(Indicator.IndicatorName).ToString.Replace("'", "''")
                                Dim IndGid As String = dRow(Indicator.IndicatorGId).ToString()

                                Dim IndExists As String = Convert.ToString(diCon.ExecuteScalarSqlQuery(SelectQuery.CheckIndicatorandGIDCombination(IndName, IndGid)))
                                If String.IsNullOrEmpty(IndExists) Then
                                    diCon.ExecuteNonQuery(InsertQuery.InsertIndicator(IndName, IndGid, InsertedDSIndex))
                                Else
                                    Dim NewDSIndex As String = IndExists & "," & InsertedDSIndex
                                    diCon.ExecuteNonQuery(UpdateQuery.UpdateIndicator(IndName, IndGid, NewDSIndex))
                                End If
                            Next
                        End If

                        '' Insert Areas
                        Dim dtArea As DataTable = conn.ExecuteDataTable(objQuery.Area.GetArea(FilterFieldType.None, String.Empty, DevInfo.Lib.DI_LibDAL.Queries.Area.Select.OrderBy.AreaNId))
                        If dtArea.Rows.Count > 0 Then
                            For Each dRow As DataRow In dtArea.Rows
                                Dim AreaName As String = dRow(Area.AreaName).ToString.Replace("'", "''")
                                Dim AreaGid As String = dRow(Area.AreaGId).ToString()
                                Dim AreaExists As String = Convert.ToString(diCon.ExecuteScalarSqlQuery(SelectQuery.CheckAreaandGIDCombination(AreaName, AreaGid)))
                                If String.IsNullOrEmpty(AreaExists) Then
                                    diCon.ExecuteNonQuery(InsertQuery.InsertArea(AreaName, AreaGid, InsertedDSIndex))
                                Else
                                    Dim NewDSIndex As String = AreaExists & "," & InsertedDSIndex
                                    diCon.ExecuteNonQuery(UpdateQuery.UpdateArea(AreaName, AreaGid, NewDSIndex))
                                End If

                            Next
                        End If

                        '' Insert Units
                        Dim dtUnit As DataTable = conn.ExecuteDataTable(objQuery.Unit.GetUnit(FilterFieldType.None, String.Empty))
                        If dtUnit.Rows.Count > 0 Then
                            For Each dRow As DataRow In dtUnit.Rows
                                Dim UnitName As String = dRow(Unit.UnitName).ToString.Replace("'", "''")
                                Dim UnitGid As String = dRow(Unit.UnitGId).ToString()
                                Dim UnitExists As String = Convert.ToString(diCon.ExecuteScalarSqlQuery(SelectQuery.CheckUnitandGIDCombination(UnitName, UnitGid)))
                                If String.IsNullOrEmpty(UnitExists) Then
                                    diCon.ExecuteNonQuery(InsertQuery.InsertUnit(UnitName, UnitGid, InsertedDSIndex))
                                Else
                                    Dim NewDSIndex As String = UnitExists & "," & InsertedDSIndex
                                    diCon.ExecuteNonQuery(UpdateQuery.UpdateUnit(UnitName, UnitGid, NewDSIndex))
                                End If
                            Next
                        End If

                        '' Insert Subgroups
                        Dim dtSubgroup As DataTable = conn.ExecuteDataTable(objQuery.SubgroupVals.GetSubgroupVals())
                        If dtSubgroup.Rows.Count > 0 Then
                            For Each dRow As DataRow In dtSubgroup.Rows
                                Dim SubgroupVals As String = dRow(DIColumns.SubgroupVals.SubgroupVal).ToString.Replace("'", "''")
                                Dim SubgroupVals_Gid As String = dRow(DIColumns.SubgroupVals.SubgroupValGId).ToString()
                                Dim SubgroupExists As String = Convert.ToString(diCon.ExecuteScalarSqlQuery(SelectQuery.CheckSubgroupandGIDCombination(SubgroupVals, SubgroupVals_Gid)))
                                If String.IsNullOrEmpty(SubgroupExists) Then
                                    diCon.ExecuteNonQuery(InsertQuery.InsertSubgroup(SubgroupVals, SubgroupVals_Gid, InsertedDSIndex))
                                Else
                                    Dim NewDSIndex As String = SubgroupExists & "," & InsertedDSIndex
                                    diCon.ExecuteNonQuery(UpdateQuery.UpdateSubgroup(SubgroupVals, SubgroupVals_Gid, NewDSIndex))
                                End If
                            Next
                        End If

                        '' Insert Source
                        Dim dtSource As DataTable = conn.ExecuteDataTable(objQuery.Source.GetSource(FilterFieldType.None, String.Empty, FieldSelection.Light, False))
                        If dtSource.Rows.Count > 0 Then
                            For Each dRow As DataRow In dtSource.Rows
                                Dim Sources As String = dRow(DIColumns.IndicatorClassifications.ICName).ToString.Replace("'", "''")
                                Dim SourceGid As String = dRow(DIColumns.IndicatorClassifications.ICGId).ToString()
                                Dim SourceExists As String = Convert.ToString(diCon.ExecuteScalarSqlQuery(SelectQuery.CheckSourceandGIDCombination(Sources, SourceGid)))
                                If String.IsNullOrEmpty(SourceExists) Then
                                    diCon.ExecuteNonQuery(InsertQuery.InsertSource(Sources, SourceGid, InsertedDSIndex))
                                Else
                                    Dim NewDSIndex As String = SourceExists & "," & InsertedDSIndex
                                    diCon.ExecuteNonQuery(UpdateQuery.UpdateSource(Sources, SourceGid, NewDSIndex))
                                End If

                            Next
                        End If

                        '' Insert TimePeriods
                        Dim dtTimePeriod As DataTable = conn.ExecuteDataTable(objQuery.Timeperiod.GetAvailableTimePeriod())
                        If dtTimePeriod.Rows.Count > 0 Then
                            For Each dRow As DataRow In dtTimePeriod.Rows
                                Dim TimePeriod As String = dRow(DIColumns.Timeperiods.TimePeriod).ToString()
                                Dim TimePeriodExists As String = Convert.ToString(diCon.ExecuteScalarSqlQuery(SelectQuery.CheckTimePeriod(TimePeriod)))
                                If String.IsNullOrEmpty(TimePeriodExists) Then
                                    diCon.ExecuteNonQuery(InsertQuery.InsertTimePeriod(TimePeriod, InsertedDSIndex))
                                Else
                                    Dim NewDSIndex As String = TimePeriodExists & "," & InsertedDSIndex
                                    diCon.ExecuteNonQuery(UpdateQuery.UpdateTimePeriod(TimePeriod, NewDSIndex))
                                End If
                            Next
                        End If
                    Next
                Next
                RetVal = "Success"
            Else
                RetVal = "AlreadyRegd"
            End If
        Catch ex As Exception
            RetVal = "Error occured while registering Database. Details of Error: " & ex.Message
        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Function to Unregister a database from the Index server and delete all its information from the tables
    ''' </summary>
    ''' <param name="dbType"></param>
    ''' <param name="AdaptationName"></param>
    ''' <param name="AdaptationVersion"></param>
    ''' <param name="ServerName"></param>
    ''' <param name="DBName"></param>
    ''' <param name="Username"></param>
    ''' <param name="Password"></param>
    ''' <param name="PortNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UnregisterDatabase(ByVal dbType As DIServerType, ByVal AdaptationName As String, ByVal AdaptationVersion As String, ByVal ServerName As String, ByVal DBName As String, ByVal Username As String, ByVal Password As String, ByVal PortNo As String) As String
        Dim RetVal As String = String.Empty
        Try
            Dim diCon As New DevInfo.Lib.DI_LibDAL.Connection.DIConnection(DIServerType.SqlServer, IndexDBServerName, IndexDBServerPortNo, IndexDBName, IndexDBUsername, IndexDBPassword)

            '' Select DBIndex from the regd. databases table
            Dim dbIndex As String = diCon.ExecuteScalarSqlQuery(SelectQuery.GetDBIndexByDBName(DBName))

            '' Remove Adaptation from Adaptations table on the basis of DBIndex
            Dim adaptationNId As String = diCon.ExecuteScalarSqlQuery(SelectQuery.GetAdaptationNidByDBIndex(dbIndex))
            Dim adaptationCount As Integer = Convert.ToInt32(diCon.ExecuteScalarSqlQuery(SelectQuery.GetAdaptationAssociationCount(adaptationNId)))
            If adaptationCount = 1 Then
                diCon.ExecuteNonQuery(DeleteQuery.RemoveAdaptation(adaptationNId))
            End If

            '' Get all the Datasets for the DBIndex from regd. datasets table
            Dim dtDatasets As DataTable = diCon.ExecuteDataTable(SelectQuery.GetDatasetsOfDatabase(dbIndex))

            For Each dRow As DataRow In dtDatasets.Rows
                Dim currentDSIndex As String = dRow(DIDBConstants.DSIndexNid).ToString()
                '' Remove all the Indicators on the basis of the Dataset Indexes for the given Database
                diCon.ExecuteNonQuery(DeleteQuery.RemoveIndicator(currentDSIndex))
                Dim dtIndDsIndexes As DataTable = diCon.ExecuteDataTable(SelectQuery.GetTable(DIDBConstants.Indicator))
                '' Remove the datasets clubbed with other datasets for given indicator
                For Each dsRow As DataRow In dtIndDsIndexes.Rows
                    Dim curInd As String = dsRow(Indicator.IndicatorName).ToString()
                    Dim curIndGid As String = dsRow(Indicator.IndicatorGId).ToString()
                    Dim dsIndexes() As String = dsRow(DIDBConstants.DSIndexNid).ToString().Split(",")
                    Dim dsIndex As String = dsRow(DIDBConstants.DSIndexNid).ToString()
                    If IsValueInArray(currentDSIndex, dsIndexes) Then
                        Dim newIndexes As String = RemoveValueFromArray(currentDSIndex, dsIndexes)
                        diCon.ExecuteNonQuery(UpdateQuery.UpdateIndicator(curInd.Replace("'", "''"), curIndGid, newIndexes))
                    End If
                Next

                '' Remove all the Areas on the basis of the Dataset Indexes for the given Database
                diCon.ExecuteNonQuery(DeleteQuery.RemoveArea(currentDSIndex))
                Dim dtAreaDsIndexes As DataTable = diCon.ExecuteDataTable(SelectQuery.GetTable(DIDBConstants.Area))
                '' Remove the datasets clubbed with other datasets for given area
                For Each dsRow As DataRow In dtAreaDsIndexes.Rows
                    Dim curArea As String = dsRow(Area.AreaName).ToString()
                    Dim curAreaGid As String = dsRow(Area.AreaGId).ToString()
                    Dim dsIndexes() As String = dsRow(DIDBConstants.DSIndexNid).ToString().Split(",")
                    Dim dsIndex As String = dsRow(DIDBConstants.DSIndexNid).ToString()
                    If IsValueInArray(currentDSIndex, dsIndexes) Then
                        Dim newIndexes As String = RemoveValueFromArray(currentDSIndex, dsIndexes)
                        diCon.ExecuteNonQuery(UpdateQuery.UpdateArea(curArea.Replace("'", "''"), curAreaGid, newIndexes))
                    End If
                Next

                '' Remove all the Units on the basis of the Dataset Indexes for the given Database
                diCon.ExecuteNonQuery(DeleteQuery.RemoveUnit(currentDSIndex))
                Dim dtUnitDsIndexes As DataTable = diCon.ExecuteDataTable(SelectQuery.GetTable(DIDBConstants.Unit))
                '' Remove the datasets clubbed with other datasets for given unit
                For Each dsRow As DataRow In dtUnitDsIndexes.Rows
                    Dim curUnit As String = dsRow(Unit.UnitName).ToString()
                    Dim curUnitGid As String = dsRow(Unit.UnitGId).ToString()
                    Dim dsIndexes() As String = dsRow(DIDBConstants.DSIndexNid).ToString().Split(",")
                    Dim dsIndex As String = dsRow(DIDBConstants.DSIndexNid).ToString()
                    If IsValueInArray(currentDSIndex, dsIndexes) Then
                        Dim newIndexes As String = RemoveValueFromArray(currentDSIndex, dsIndexes)
                        diCon.ExecuteNonQuery(UpdateQuery.UpdateUnit(curUnit.Replace("'", "''"), curUnitGid, newIndexes))
                    End If
                Next

                '' Remove all the Sources on the basis of the Dataset Indexes for the given Database
                diCon.ExecuteNonQuery(DeleteQuery.RemoveSource(currentDSIndex))
                Dim dtSourceDsIndexes As DataTable = diCon.ExecuteDataTable(SelectQuery.GetTable(DIDBConstants.Source))
                '' Remove the datasets clubbed with other datasets for given source
                For Each dsRow As DataRow In dtSourceDsIndexes.Rows
                    Dim curSource As String = dsRow(IndicatorClassifications.ICName).ToString()
                    Dim curSourceGid As String = dsRow(IndicatorClassifications.ICGId).ToString()
                    Dim dsIndexes() As String = dsRow(DIDBConstants.DSIndexNid).ToString().Split(",")
                    Dim dsIndex As String = dsRow(DIDBConstants.DSIndexNid).ToString()
                    If IsValueInArray(currentDSIndex, dsIndexes) Then
                        Dim newIndexes As String = RemoveValueFromArray(currentDSIndex, dsIndexes)
                        diCon.ExecuteNonQuery(UpdateQuery.UpdateSource(curSource.Replace("'", "''"), curSourceGid, newIndexes))
                    End If
                Next

                '' Remove all the Subgroups on the basis of the Dataset Indexes for the given Database
                diCon.ExecuteNonQuery(DeleteQuery.RemoveSubgroup(currentDSIndex))
                Dim dtSGDsIndexes As DataTable = diCon.ExecuteDataTable(SelectQuery.GetTable(DIDBConstants.Subgroup))
                '' Remove the datasets clubbed with other datasets for given subgroup
                For Each dsRow As DataRow In dtSGDsIndexes.Rows
                    Dim curSG As String = dsRow(SubgroupVals.SubgroupVal).ToString()
                    Dim curSGGid As String = dsRow(SubgroupVals.SubgroupValGId).ToString()
                    Dim dsIndexes() As String = dsRow(DIDBConstants.DSIndexNid).ToString().Split(",")
                    Dim dsIndex As String = dsRow(DIDBConstants.DSIndexNid).ToString()
                    If IsValueInArray(currentDSIndex, dsIndexes) Then
                        Dim newIndexes As String = RemoveValueFromArray(currentDSIndex, dsIndexes)
                        diCon.ExecuteNonQuery(UpdateQuery.UpdateSubgroup(curSG.Replace("'", "''"), curSGGid, newIndexes))
                    End If
                Next

                '' Remove all the TimePeriods on the basis of the Dataset Indexes for the given Database
                diCon.ExecuteNonQuery(DeleteQuery.RemoveTimePeriod(currentDSIndex))
                Dim dtTPDsIndexes As DataTable = diCon.ExecuteDataTable(SelectQuery.GetTable(DIDBConstants.TimePeriod))
                '' Remove the datasets clubbed with other datasets for given timeperiod
                For Each dsRow As DataRow In dtTPDsIndexes.Rows
                    Dim curTP As String = dsRow(Timeperiods.TimePeriod).ToString()
                    Dim dsIndexes() As String = dsRow(DIDBConstants.DSIndexNid).ToString().Split(",")
                    Dim dsIndex As String = dsRow(DIDBConstants.DSIndexNid).ToString()
                    If IsValueInArray(currentDSIndex, dsIndexes) Then
                        Dim newIndexes As String = RemoveValueFromArray(currentDSIndex, dsIndexes)
                        diCon.ExecuteNonQuery(UpdateQuery.UpdateTimePeriod(curTP, newIndexes))
                    End If
                Next
            Next

            '' Remove Datasets
            diCon.ExecuteNonQuery(DeleteQuery.RemoveDataset(dbIndex))
            '' Remove Database
            diCon.ExecuteNonQuery(DeleteQuery.RemoveDatabase(DBName))

            RetVal = "Success"
        Catch ex As Exception
            RetVal = "Error"
        End Try
        Return RetVal
    End Function

    ''' <summary>
    ''' Function to check if the given values exists in the array
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="arr"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsValueInArray(ByVal value As String, ByVal arr() As String)
        Dim RetVal As Boolean = False
        Try
            For Each Item As String In arr
                If Item = value Then
                    RetVal = True
                    Exit For
                End If
            Next
        Catch ex As Exception

        End Try
        Return RetVal
    End Function

    ''' <summary>
    ''' Function to remove a given value from the array
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="arr"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RemoveValueFromArray(ByVal value As String, ByVal arr() As String)
        Dim RetVal As String = String.Empty
        Try
            Dim al As New ArrayList
            For Each Item As String In arr
                al.Add(Item)
            Next
            If al.Contains(value) Then
                al.Remove(value)
            End If
            Dim i As Integer = 0
            For i = 0 To al.Count - 1
                RetVal = RetVal & "," & al.Item(i)
            Next
            If RetVal.Substring(0, 1) = "," Then
                RetVal = RetVal.Substring(1)
            End If
        Catch ex As Exception

        End Try
        Return RetVal
    End Function

    ''' <summary>
    ''' Function to Truncate all the tables in the DI Database Index Server
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TruncateAllTables()
        Dim RetVal As String = String.Empty
        Dim diCon As New DevInfo.Lib.DI_LibDAL.Connection.DIConnection(DIServerType.SqlServer, IndexDBServerName, IndexDBServerPortNo, IndexDBName, IndexDBUsername, IndexDBPassword)
        diCon.ExecuteNonQuery(DeleteQuery.TruncateAll())
        RetVal = "Success"
        Try

        Catch ex As Exception
            RetVal = "Error"
        End Try
        Return RetVal
    End Function

    '''''-- Free Text Search 
    Public Function SimpleSearch(ByVal keyword As String, ByVal languageCode As String) As String
        Dim RetVal As String = String.Empty
        keyword = keyword.Replace("'", "''")
        Me._SearchText = keyword
        'Dim mainKeyword As String = keyword
        Try
            RetVal = SimpleRecursiveSearch(keyword)
            ''If Not String.IsNullOrEmpty(retVal) Then
            ''    Dim arrayDSIndex() As String = retVal.Split(",")
            ''    arrayDSIndex = GetDistinctValues(arrayDSIndex)
            ''End If

            For Each Item As KeyValuePair(Of String, String) In Me.KeywordDatasetPair
                Dim arrayDSIndex() As String = Item.Value.Split(",")
                arrayDSIndex = GetDistinctValues(arrayDSIndex)
                Dim xmlString As String = ReturnXML(arrayDSIndex, Item.Key, languageCode)
            Next
            Dim arrayDSIndex1() As String = RetVal.Split(",")
            arrayDSIndex1 = GetDistinctValues(arrayDSIndex1)
            RetVal = ReturnXML(arrayDSIndex1, keyword, languageCode)

        Catch ex As Exception
            Throw ex

        End Try
        Return RetVal
    End Function

    ''' <summary>
    ''' Function to return XML based on the searched keyword(s)
    ''' </summary>
    ''' <param name="dsArray"></param>
    ''' <param name="keyword"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ReturnXML(ByVal dsArray() As String, ByVal keyword As String, ByVal langCode As String) As String
        Dim diCon As New DevInfo.Lib.DI_LibDAL.Connection.DIConnection(DIServerType.SqlServer, IndexDBServerName, IndexDBServerPortNo, IndexDBName, IndexDBUsername, IndexDBPassword)
        Dim RetVal As String = String.Empty
        Try
            'If diCon.ExecuteScalarSqlQuery(SelectQuery.CheckExistingKeywords(keyword)) = 0 Then
            RetVal = CreateXML(dsArray, keyword, langCode)
            'Else
            'RetVal = GetXML(keyword)
            'End If

        Catch ex As Exception

        End Try
        Return RetVal
    End Function

    ''' <summary>
    ''' Function to create XML for the searched keyword
    ''' </summary>
    ''' <param name="dsArray"></param>
    ''' <param name="keyword"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateXML(ByVal dsArray() As String, ByVal keyword As String, ByVal langCode As String)
        Dim RetVal As String = String.Empty
        Try
            Dim diCon As New DevInfo.Lib.DI_LibDAL.Connection.DIConnection(DIServerType.SqlServer, IndexDBServerName, IndexDBServerPortNo, IndexDBName, IndexDBUsername, IndexDBPassword)
            Dim _XmlDoc As System.Xml.XmlDocument
            _XmlDoc = GetXMLDoc("ROOT")
            Dim _XmlNode As System.Xml.XmlNode
            Dim _Attribute As System.Xml.XmlAttribute

            Dim dtDB As New DataTable("DBDetails")
            Dim dtRow As DataRow = dtDB.NewRow()
            Dim dtCol1 As New DataColumn("DatabaseIndex")
            Dim dtCol2 As New DataColumn("DatasetIndex")
            dtDB.Columns.Add(dtCol1)
            dtDB.Columns.Add(dtCol2)
            For Each arrItem As String In dsArray
                Dim dsIndex As Integer = Convert.ToInt32(arrItem)
                Dim dbIndex As Integer = diCon.ExecuteScalarSqlQuery(SelectQuery.GetDSDBIndex(dsIndex))

                If dtDB.Select("DatabaseIndex = " & dbIndex & "").Length > 0 Then
                    Dim dsRows() As DataRow = dtDB.Select("DatabaseIndex = " & dbIndex & "")
                    Dim updatedDSIndex As String = dsRows(0).Item(1) & "," & dsIndex
                    dsRows(0).Item(1) = updatedDSIndex
                Else
                    dtDB.Rows.Add(dbIndex, dsIndex)
                End If
            Next

            Dim dbFound As Integer = 0
            Dim updatedDBIndex As String = String.Empty
            For Each dRow As DataRow In dtDB.Rows
                dbFound += 1
                Dim dbIndx As String = dRow("DatabaseIndex")
                updatedDBIndex = updatedDBIndex & "," & dbIndx
                Dim dtDBDetails As DataTable = diCon.ExecuteDataTable(SelectQuery.GetDatabaseDetails(dbIndx))
                _XmlNode = _XmlDoc.CreateNode(XmlNodeType.Element, "row", "")
                _XmlDoc.DocumentElement.AppendChild(_XmlNode)
                For Each row As DataRow In dtDBDetails.Rows
                    _Attribute = _XmlDoc.CreateAttribute("ServerName")
                    _Attribute.Value = row(DIDBConstants.DBServerName).ToString
                    _XmlNode.Attributes.Append(_Attribute)

                    _Attribute = _XmlDoc.CreateAttribute("Adaptation")
                    Dim AdaptationName As String = diCon.ExecuteScalarSqlQuery(SelectQuery.GetAdaptationNameById(row(DIDBConstants.DBAdaptationNID).ToString))
                    Dim AdaptationVersion As String = diCon.ExecuteScalarSqlQuery(SelectQuery.GetAdaptationVersion(row(DIDBConstants.DBAdaptationNID).ToString))
                    _Attribute.Value = AdaptationName & " " & AdaptationVersion
                    _XmlNode.Attributes.Append(_Attribute)

                    _Attribute = _XmlDoc.CreateAttribute("DatabaseName")
                    _Attribute.Value = row(DIDBConstants.DatabaseName).ToString
                    _XmlNode.Attributes.Append(_Attribute)

                    _Attribute = _XmlDoc.CreateAttribute("DatabaseType")
                    _Attribute.Value = row(DIDBConstants.DBType).ToString
                    _XmlNode.Attributes.Append(_Attribute)

                    _Attribute = _XmlDoc.CreateAttribute("Username")
                    _Attribute.Value = row(DIDBConstants.DBUsername).ToString
                    _XmlNode.Attributes.Append(_Attribute)

                    _Attribute = _XmlDoc.CreateAttribute("Password")
                    _Attribute.Value = row(DIDBConstants.DBPassword).ToString
                    _XmlNode.Attributes.Append(_Attribute)

                    _Attribute = _XmlDoc.CreateAttribute("PortNo")
                    _Attribute.Value = row(DIDBConstants.DBPort).ToString
                    _XmlNode.Attributes.Append(_Attribute)

                Next

                Dim dtDSDetails As DataTable = diCon.ExecuteDataTable(SelectQuery.GetDatasetsOfDatabase(dbIndx, langCode))
                Dim IsDataSetFoundForSearchResults As Boolean = False
                For Each row As DataRow In dtDSDetails.Rows
                    For Each Item As String In dsArray
                        If row("DSIndex") = Item Then
                            IsDataSetFoundForSearchResults = True
                            Exit For
                        Else
                            IsDataSetFoundForSearchResults = False
                        End If
                    Next

                    If IsDataSetFoundForSearchResults = True Then
                        Dim xeDS As System.Xml.XmlNode
                        xeDS = _XmlDoc.CreateNode(XmlNodeType.Element, "Dataset", "")

                        _Attribute = _XmlDoc.CreateAttribute("Prefix")
                        _Attribute.Value = row("DSPrefix")
                        xeDS.Attributes.Append(_Attribute)

                        _Attribute = _XmlDoc.CreateAttribute("Name")
                        _Attribute.Value = row("DSName")
                        xeDS.Attributes.Append(_Attribute)

                        _Attribute = _XmlDoc.CreateAttribute("Description")
                        _Attribute.Value = row("DBMtd_Desc")
                        xeDS.Attributes.Append(_Attribute)

                        _Attribute = _XmlDoc.CreateAttribute("IndCount")
                        _Attribute.Value = row("DBMtd_IndCnt")
                        xeDS.Attributes.Append(_Attribute)

                        _Attribute = _XmlDoc.CreateAttribute("AreaCount")
                        _Attribute.Value = row("DBMtd_AreaCnt")
                        xeDS.Attributes.Append(_Attribute)

                        _Attribute = _XmlDoc.CreateAttribute("IUSCount")
                        _Attribute.Value = row("DBMtd_IUSCnt")
                        xeDS.Attributes.Append(_Attribute)

                        _Attribute = _XmlDoc.CreateAttribute("TimeCount")
                        _Attribute.Value = row("DBMtd_TimeCnt")
                        xeDS.Attributes.Append(_Attribute)

                        _Attribute = _XmlDoc.CreateAttribute("SrcCount")
                        _Attribute.Value = row("DBMtd_SrcCnt")
                        xeDS.Attributes.Append(_Attribute)

                        _Attribute = _XmlDoc.CreateAttribute("DataCount")
                        _Attribute.Value = row("DBMtd_DataCnt")
                        xeDS.Attributes.Append(_Attribute)

                        _XmlNode.AppendChild(xeDS)
                    Else

                    End If
                Next
                If dbFound = dtDB.Rows.Count Then
                    If updatedDBIndex.Substring(0, 1) = "," Then
                        updatedDBIndex = updatedDBIndex.Substring(1)
                    End If
                    Dim arrayDSIndex() As String = dsArray
                    arrayDSIndex = GetDistinctValues(arrayDSIndex)
                    Dim arrayOfDatasets As String = ReturnStringFromArray(arrayDSIndex)
                    Dim xmlString As String = _XmlDoc.InnerXml.ToString().Replace("'", "&apos;")
                    'diCon.ExecuteNonQuery(InsertQuery.InsertSearchResults(keyword, xmlString, arrayOfDatasets))
                End If
            Next
            RetVal = _XmlDoc.InnerXml
        Catch ex As Exception

        End Try
        Return RetVal
    End Function

    Public Function GetAllDatabasesXML(ByVal langCode As String) As String

        Dim RetVal As String = String.Empty
        Try
            Dim diCon As New DevInfo.Lib.DI_LibDAL.Connection.DIConnection(DIServerType.SqlServer, IndexDBServerName, IndexDBServerPortNo, IndexDBName, IndexDBUsername, IndexDBPassword)
            Dim allDSDt As DataTable = diCon.ExecuteDataTable(SelectQuery.GetAllDatabases())
            Dim allDatasets As String = String.Empty
            For Each allDsRows As DataRow In allDSDt.Rows
                allDatasets = allDatasets & "," & allDsRows(0)
            Next
            allDatasets = allDatasets.Substring(1)
            Dim dsArray() As String = allDatasets.Split(",")
            Dim _XmlDoc As System.Xml.XmlDocument
            _XmlDoc = GetXMLDoc("ROOT")
            Dim _XmlNode As System.Xml.XmlNode
            Dim _Attribute As System.Xml.XmlAttribute

            Dim dtDB As New DataTable("DBDetails")
            Dim dtRow As DataRow = dtDB.NewRow()
            Dim dtCol1 As New DataColumn("DatabaseIndex")
            Dim dtCol2 As New DataColumn("DatasetIndex")
            dtDB.Columns.Add(dtCol1)
            dtDB.Columns.Add(dtCol2)
            For Each arrItem As String In dsArray
                Dim dsIndex As Integer = Convert.ToInt32(arrItem)
                Dim dbIndex As Integer = diCon.ExecuteScalarSqlQuery(SelectQuery.GetDSDBIndex(dsIndex))

                If dtDB.Select("DatabaseIndex = " & dbIndex & "").Length > 0 Then
                    Dim dsRows() As DataRow = dtDB.Select("DatabaseIndex = " & dbIndex & "")
                    Dim updatedDSIndex As String = dsRows(0).Item(1) & "," & dsIndex
                    dsRows(0).Item(1) = updatedDSIndex
                Else
                    dtDB.Rows.Add(dbIndex, dsIndex)
                End If
            Next

            Dim dbFound As Integer = 0
            Dim updatedDBIndex As String = String.Empty
            For Each dRow As DataRow In dtDB.Rows
                dbFound += 1
                Dim dbIndx As String = dRow("DatabaseIndex")
                updatedDBIndex = updatedDBIndex & "," & dbIndx
                Dim dtDBDetails As DataTable = diCon.ExecuteDataTable(SelectQuery.GetDatabaseDetails(dbIndx))
                _XmlNode = _XmlDoc.CreateNode(XmlNodeType.Element, "row", "")
                _XmlDoc.DocumentElement.AppendChild(_XmlNode)
                For Each row As DataRow In dtDBDetails.Rows
                    _Attribute = _XmlDoc.CreateAttribute("ServerName")
                    _Attribute.Value = row(DIDBConstants.DBServerName).ToString
                    _XmlNode.Attributes.Append(_Attribute)

                    _Attribute = _XmlDoc.CreateAttribute("Adaptation")
                    Dim AdaptationName As String = diCon.ExecuteScalarSqlQuery(SelectQuery.GetAdaptationNameById(row(DIDBConstants.DBAdaptationNID).ToString))
                    Dim AdaptationVersion As String = diCon.ExecuteScalarSqlQuery(SelectQuery.GetAdaptationVersion(row(DIDBConstants.DBAdaptationNID).ToString))
                    _Attribute.Value = AdaptationName & " " & AdaptationVersion
                    _XmlNode.Attributes.Append(_Attribute)

                    _Attribute = _XmlDoc.CreateAttribute("DatabaseName")
                    _Attribute.Value = row(DIDBConstants.DatabaseName).ToString
                    _XmlNode.Attributes.Append(_Attribute)

                    _Attribute = _XmlDoc.CreateAttribute("DatabaseType")
                    _Attribute.Value = row(DIDBConstants.DBType).ToString
                    _XmlNode.Attributes.Append(_Attribute)

                    _Attribute = _XmlDoc.CreateAttribute("Username")
                    _Attribute.Value = row(DIDBConstants.DBUsername).ToString
                    _XmlNode.Attributes.Append(_Attribute)

                    _Attribute = _XmlDoc.CreateAttribute("Password")
                    _Attribute.Value = row(DIDBConstants.DBPassword).ToString
                    _XmlNode.Attributes.Append(_Attribute)

                    _Attribute = _XmlDoc.CreateAttribute("PortNo")
                    _Attribute.Value = row(DIDBConstants.DBPort).ToString
                    _XmlNode.Attributes.Append(_Attribute)

                Next

                Dim dtDSDetails As DataTable = diCon.ExecuteDataTable(SelectQuery.GetDatasetsOfDatabase(dbIndx, langCode))
                Dim IsDataSetFoundForSearchResults As Boolean = False
                For Each row As DataRow In dtDSDetails.Rows
                    For Each Item As String In dsArray
                        If row("DSIndex") = Item Then
                            IsDataSetFoundForSearchResults = True
                            Exit For
                        Else
                            IsDataSetFoundForSearchResults = False
                        End If
                    Next

                    If IsDataSetFoundForSearchResults = True Then
                        Dim xeDS As System.Xml.XmlNode
                        xeDS = _XmlDoc.CreateNode(XmlNodeType.Element, "Dataset", "")

                        _Attribute = _XmlDoc.CreateAttribute("Prefix")
                        _Attribute.Value = row("DSPrefix")
                        xeDS.Attributes.Append(_Attribute)

                        _Attribute = _XmlDoc.CreateAttribute("Name")
                        _Attribute.Value = row("DSName")
                        xeDS.Attributes.Append(_Attribute)

                        _Attribute = _XmlDoc.CreateAttribute("Description")
                        _Attribute.Value = row("DBMtd_Desc")
                        xeDS.Attributes.Append(_Attribute)

                        _Attribute = _XmlDoc.CreateAttribute("IndCount")
                        _Attribute.Value = row("DBMtd_IndCnt")
                        xeDS.Attributes.Append(_Attribute)

                        _Attribute = _XmlDoc.CreateAttribute("AreaCount")
                        _Attribute.Value = row("DBMtd_AreaCnt")
                        xeDS.Attributes.Append(_Attribute)

                        _Attribute = _XmlDoc.CreateAttribute("IUSCount")
                        _Attribute.Value = row("DBMtd_IUSCnt")
                        xeDS.Attributes.Append(_Attribute)

                        _Attribute = _XmlDoc.CreateAttribute("TimeCount")
                        _Attribute.Value = row("DBMtd_TimeCnt")
                        xeDS.Attributes.Append(_Attribute)

                        _Attribute = _XmlDoc.CreateAttribute("SrcCount")
                        _Attribute.Value = row("DBMtd_SrcCnt")
                        xeDS.Attributes.Append(_Attribute)

                        _Attribute = _XmlDoc.CreateAttribute("DataCount")
                        _Attribute.Value = row("DBMtd_DataCnt")
                        xeDS.Attributes.Append(_Attribute)

                        _XmlNode.AppendChild(xeDS)
                    Else

                    End If
                Next
                If dbFound = dtDB.Rows.Count Then
                    If updatedDBIndex.Substring(0, 1) = "," Then
                        updatedDBIndex = updatedDBIndex.Substring(1)
                    End If
                    Dim arrayDSIndex() As String = dsArray
                    arrayDSIndex = GetDistinctValues(arrayDSIndex)
                    Dim arrayOfDatasets As String = ReturnStringFromArray(arrayDSIndex)
                    Dim xmlString As String = _XmlDoc.InnerXml.ToString().Replace("'", "&apos;")
                    'diCon.ExecuteNonQuery(InsertQuery.InsertSearchResults(keyword, xmlString, arrayOfDatasets))
                End If
            Next
            RetVal = _XmlDoc.InnerXml
        Catch ex As Exception

        End Try
        Return RetVal
    End Function

    ''' <summary>
    ''' Function to fetch the pre-stored XML for the searched keywords
    ''' </summary>
    ''' <param name="keyword"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetXML(ByVal keyword As String) As String
        Dim RetVal As String = String.Empty
        Try
            Dim diCon As New DevInfo.Lib.DI_LibDAL.Connection.DIConnection(DIServerType.SqlServer, IndexDBServerName, IndexDBServerPortNo, IndexDBName, IndexDBUsername, IndexDBPassword)
            Dim dtXmlContents As DataTable = diCon.ExecuteDataTable(SelectQuery.GetResultsXML(keyword))
            RetVal = dtXmlContents.Rows(0)("XMLFile")
        Catch ex As Exception

        End Try
        Return RetVal
    End Function

    Friend Function GetXMLDoc(ByVal p_ROOTNAME As String) As System.Xml.XmlDocument
        Dim XMLDoc As New System.Xml.XmlDocument
        XMLDoc.LoadXml("<?xml version='1.0' encoding='utf-8' ?><" & p_ROOTNAME & "></" & p_ROOTNAME & ">")
        Return XMLDoc
    End Function

    ''' <summary>
    ''' Function to return an array in the form of string
    ''' </summary>
    ''' <param name="strArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ReturnStringFromArray(ByVal strArray() As String)
        Dim RetVal As String = String.Empty
        Try
            For Each arrItem As String In strArray
                RetVal = RetVal & "," & arrItem
            Next
        Catch ex As Exception

        End Try
        If RetVal.Substring(0, 1) = "," Then
            RetVal = RetVal.Substring(1)
        End If
        Return RetVal
    End Function

    Private Function SimpleRecursiveSearch(ByVal keyword As String) As String
        ' Replace Double space to single space
        keyword = RemoveDoubleSpace(keyword)
        Dim keywords As String() = Nothing
        Dim iWordCount As Int32 = 0
        '*****************************************Search Logic****************************
        ' STEP 1: Get All Possible search combinations
        ' will return the DataTable containing the Keywords and the wod count in separate columns
        ' string[,] PossibleKeywordCombinations = GetAllSearchCombinations(keywords);
        ' STEP 2: Sort the Data Table by Word count Descending

        ' STEP 3: For each item in the Data table, perform the search
        ' STEP 3.1 If search succeeded and iWordCount=WordCount of this row
        ' THEN do searching
        ' ELSE CONTINUE

        ' STEP 3.2 If search succeeded and iWordCount<>WordCount of this row
        ' THEN TRIM mainSearh string and do RECURSSION
        ' ELSE CONTINUE
        '*****************************************Search Logic****************************

        Try

            ' Break keyword typed into words and get Word count
            keywords = GetAbsoluteKeyword(keyword)
            iWordCount = keywords.Length

            ' STEP 1 & 2
            'Get All Possible search combinations. Sort the Data Table by Word count Descending
            Dim PossibleKeywordCombinationsDT As DataTable = GetAllSearchCombinationsDataTable(keywords)

            ' STEP 3: For each item in the Data table, perform the search
            For Each DRow As DataRow In PossibleKeywordCombinationsDT.Rows

                Dim CurrentSearchString As String = DRow(SearchKeyword).ToString()
                Dim dsIndexes As String = Me.PerformSimpleSearch(CurrentSearchString)
                ' Chk For Sucessful Search
                'If Not String.IsNullOrEmpty(TempIndicatorIUSNIds) OrElse Not String.IsNullOrEmpty(TempAreaNIds) OrElse Not String.IsNullOrEmpty(TempTimePeriodNIds) Then
                If Not String.IsNullOrEmpty(dsIndexes) Then
                    SearchedDSIndexes = SearchedDSIndexes & "," & dsIndexes
                    KeywordDatasetPair.Add(CurrentSearchString, dsIndexes)
                    ' STEP 3.1 If search succeeded and iWordCount=WordCount of this row
                    If Convert.ToInt32(DRow(WordCount)) = iWordCount Then
                        ''Me._Indicator_IUS_NId = CheckAndUpdateNewNIds(Me._Indicator_IUS_NId, TempIndicatorIUSNIds)
                        ''Me._AreaNId = CheckAndUpdateNewNIds(Me._AreaNId, TempAreaNIds)
                        ''Me._TimePeriodNId = CheckAndUpdateNewNIds(Me._TimePeriodNId, TempTimePeriodNIds)
                        Exit For
                    Else
                        'STEP 3.2 If search succeeded and iWordCount<>WordCount of this row

                        'Update I A T NIds
                        ''Me._Indicator_IUS_NId = CheckAndUpdateNewNIds(Me._Indicator_IUS_NId, TempIndicatorIUSNIds)
                        ''Me._AreaNId = CheckAndUpdateNewNIds(Me._AreaNId, TempAreaNIds)
                        ''Me._TimePeriodNId = CheckAndUpdateNewNIds(Me._TimePeriodNId, TempTimePeriodNIds)

                        ' STEP 3.3 : Trim current keyword from SearchString
                        Dim Tempkeyword As String = keyword.Replace(DRow(SearchKeyword).ToString().Trim(), "").Trim()

                        ' Recursive Call
                        Me.SimpleRecursiveSearch(Tempkeyword)
                        Exit For
                    End If
                End If
            Next
        Catch ex As Exception
        End Try
        If SearchedDSIndexes.Substring(0, 1) = "," Then
            SearchedDSIndexes = SearchedDSIndexes.Substring(1)
        End If
        Return SearchedDSIndexes
    End Function

    Private Function PerformSimpleSearch(ByVal searchkeyword As String) As String
        Dim RetVal As String = String.Empty
        Try
            Dim diCon As New DevInfo.Lib.DI_LibDAL.Connection.DIConnection(DIServerType.SqlServer, IndexDBServerName, IndexDBServerPortNo, IndexDBName, IndexDBUsername, IndexDBPassword)
            Dim dtDSIndex As DataTable = diCon.ExecuteDataTable(SelectQuery.IndicatorSearch(searchkeyword))
            Dim DatasetIndexes As String = String.Empty
            If dtDSIndex.Rows.Count > 0 Then                '' If found in Indicator table
                For Each row As DataRow In dtDSIndex.Rows
                    DatasetIndexes = DatasetIndexes & "," & row(DIDBConstants.DSIndexNid)
                Next
                If Not String.IsNullOrEmpty(DatasetIndexes) Then
                    DatasetIndexes = DatasetIndexes.Substring(1)
                End If
            Else    '' Else search in Area table
                dtDSIndex = diCon.ExecuteDataTable(SelectQuery.AreaSearch(searchkeyword))
                If dtDSIndex.Rows.Count > 0 Then        '' If found in Area table
                    For Each row As DataRow In dtDSIndex.Rows
                        DatasetIndexes = DatasetIndexes & "," & row(DIDBConstants.DSIndexNid)
                    Next
                    If Not String.IsNullOrEmpty(DatasetIndexes) Then
                        DatasetIndexes = DatasetIndexes.Substring(1)
                    End If
                Else    '' Else search in Unit table
                    dtDSIndex = diCon.ExecuteDataTable(SelectQuery.UnitSearch(searchkeyword))
                    If dtDSIndex.Rows.Count > 0 Then        '' If found in Unit table
                        For Each row As DataRow In dtDSIndex.Rows
                            DatasetIndexes = DatasetIndexes & "," & row(DIDBConstants.DSIndexNid)
                        Next
                        If Not String.IsNullOrEmpty(DatasetIndexes) Then
                            DatasetIndexes = DatasetIndexes.Substring(1)
                        End If
                    Else    '' Else search in Source table
                        dtDSIndex = diCon.ExecuteDataTable(SelectQuery.SourceSearch(searchkeyword))
                        If dtDSIndex.Rows.Count > 0 Then        '' If found in Source table
                            For Each row As DataRow In dtDSIndex.Rows
                                DatasetIndexes = DatasetIndexes & "," & row(DIDBConstants.DSIndexNid)
                            Next
                            If Not String.IsNullOrEmpty(DatasetIndexes) Then
                                DatasetIndexes = DatasetIndexes.Substring(1)
                            End If
                        Else    '' Else search in Subgroup table
                            dtDSIndex = diCon.ExecuteDataTable(SelectQuery.SubgroupSearch(searchkeyword))
                            If dtDSIndex.Rows.Count > 0 Then    '' If found in Subgroup table
                                For Each row As DataRow In dtDSIndex.Rows
                                    DatasetIndexes = DatasetIndexes & "," & row(DIDBConstants.DSIndexNid)
                                Next
                                If Not String.IsNullOrEmpty(DatasetIndexes) Then
                                    DatasetIndexes = DatasetIndexes.Substring(1)
                                End If
                            Else    '' Else search in TimePeriod table
                                dtDSIndex = diCon.ExecuteDataTable(SelectQuery.TimePeriodSearch(searchkeyword))
                                If dtDSIndex.Rows.Count > 0 Then
                                    For Each row As DataRow In dtDSIndex.Rows
                                        DatasetIndexes = DatasetIndexes & "," & row(DIDBConstants.DSIndexNid)
                                    Next
                                    If Not String.IsNullOrEmpty(DatasetIndexes) Then
                                        DatasetIndexes = DatasetIndexes.Substring(1)
                                    End If
                                End If
                            End If
                        End If
                    End If

                End If
            End If
            RetVal = DatasetIndexes
        Catch ex As Exception

        End Try
        Return RetVal
    End Function

    ''' <summary>
    ''' Function to return distinct values from an array
    ''' </summary>
    ''' <param name="array"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDistinctValues(ByVal array As String()) As String()
        Dim list As New List(Of String)()
        For i As Integer = 0 To array.Length - 1
            If list.Contains(array(i)) Then
                Continue For
            End If
            list.Add(array(i))
        Next
        Return (list.ToArray())
    End Function

    Private Function GetAbsoluteKeyword(ByVal keyword As String) As String()
        Dim RetVal As String()
        Dim TempSearchPattern As String()

        ' -- Split keywords at quotes and get it in SearchPattern"
        ' Empty entries are required as part of logic
        Dim SearchPattern As String() = keyword.Split("""".ToCharArray())
        ' string[] SearchPattern = keyword.Split("\"".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
        If IsEvenNumber(SearchPattern.Length) Then
            '--- Quotes to be ignored
            'SearchPattern = keyword.Split(' ');
            SearchPattern = keyword.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
        Else
            '--- Quotes to be considered
            Dim TempSearch As String()
            ' string[] TempStorage;
            Dim TempStorage As New List(Of String)()
            For i As Integer = 0 To SearchPattern.Length - 1
                If SearchPattern(i).Length > 0 Then
                    If IsEvenNumber(i) Then
                        '--- is string that is not withing ""
                        'TempSearch = SearchPattern[i].Split(' ');
                        TempSearch = SearchPattern(i).Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)

                        For j As Integer = 0 To TempSearch.Length - 1
                            If TempSearch(j).Length > 0 Then
                                TempStorage.Add(TempSearch(j))
                            End If
                        Next
                    Else
                        If SearchPattern(i).Length > 0 Then
                            ' values inside quotes will be matched exactly
                            'TempStorage.Add(SearchPattern[i]);
                            '-- end of Change on 18-03-08 . values inside quotes will be matched exactly

                            TempStorage.Add("'" & SearchPattern(i) & "'")

                        End If
                    End If
                End If
            Next
            TempSearchPattern = TempStorage.ToArray()
            SearchPattern = New String(TempSearchPattern.Length - 1) {}
            TempSearchPattern.CopyTo(SearchPattern, 0)
        End If
        RetVal = SearchPattern
        Return RetVal
    End Function

    ''' <summary>
    ''' Function to return a datatable with all possible search string combinations
    ''' </summary>
    ''' <param name="keywords"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetAllSearchCombinationsDataTable(ByVal keywords As String()) As DataTable
        Dim RetVal As New DataTable()
        ' Add Word cout and SearchString columns
        RetVal.Columns.Add(WordCount)
        RetVal.Columns.Add(SearchKeyword)

        Dim PrevWord As String = String.Empty

        Dim index As Integer = 0
        For i As Integer = 0 To keywords.Length - 1
            PrevWord = String.Empty
            Dim wordCount__1 As Integer = 0
            For j As Integer = i To keywords.Length - 1
                Dim TempRow As DataRow = RetVal.NewRow()
                If i = j Then
                    TempRow(SearchKeyword) = keywords(j).ToString()
                Else
                    TempRow(SearchKeyword) = (PrevWord & " ") + keywords(j).ToString()
                End If
                PrevWord = TempRow(SearchKeyword).ToString()
                wordCount__1 += 1
                TempRow(WordCount) = wordCount__1.ToString()
                index += 1
                RetVal.Rows.Add(TempRow)
            Next
        Next
        ' Sort Result Table
        If RetVal IsNot Nothing AndAlso RetVal.Rows.Count > 0 Then
            Dim Dv As DataView = RetVal.DefaultView
            Dv.Sort = WordCount & " DESC"
            RetVal = Dv.ToTable()
        End If
        Return RetVal
    End Function

    ''' <summary>
    ''' Function to remove double space from a string value
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function RemoveDoubleSpace(ByVal value As String) As String
        Dim RetVal As String = String.Empty
        RetVal = value.Replace("  ", " ")
        If RetVal.Contains("  ") Then
            RetVal = RemoveDoubleSpace(RetVal)
        End If

        Return RetVal
    End Function

    Public Shared Function EscapeWildcardChar(ByVal value As String) As String
        value = value.Replace("*", "[*]")
        value = value.Replace("%", "[%]")
        Return value
    End Function

    Public Shared Function RemoveQuotes(ByVal value As String) As String
        Dim RetVal As String = String.Empty
        RetVal = value.Replace("'", "''")
        Return RetVal
    End Function

    Private Function IsEvenNumber(ByVal number As Int32) As [Boolean]
        Dim result As Integer = 0
        Math.DivRem(number, 2, result)
        If result = 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function DatabaseExists(ByVal dbType As DIServerType, ByVal AdaptationName As String, ByVal AdaptationVersion As String, ByVal ServerName As String, ByVal DBName As String, ByVal Username As String, ByVal Password As String, ByVal PortNo As String) As Boolean
        Dim RetVal As Boolean = False
        Try
            Dim diCon As New DevInfo.Lib.DI_LibDAL.Connection.DIConnection(DIServerType.SqlServer, IndexDBServerName, IndexDBServerPortNo, IndexDBName, IndexDBUsername, IndexDBPassword)
            Dim isAlreadyRegd As Integer = diCon.ExecuteScalarSqlQuery(SelectQuery.IsDatabaseAlreadyRegistered(DBName))
            If isAlreadyRegd = 0 Then
                RetVal = False
            Else
                RetVal = True
            End If
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
#End Region

End Class
