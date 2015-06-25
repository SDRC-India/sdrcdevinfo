Imports di_Worldwide.DIDBConstants
Imports DevInfo.Lib.DI_LibDAL.Queries
Public Class SelectQuery
    ''' <summary>
    ''' Query to check if the database is already registered in the Index Server
    ''' </summary>
    ''' <param name="DBName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function IsDatabaseAlreadyRegistered(ByVal DBName As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select count(*) from " & DBDetailsTableName & " where " & DatabaseName & " = '" & DBName & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to get the inserted NID
    ''' </summary>
    ''' <returns>returns the latest inserted NID</returns>
    ''' <remarks></remarks>
    Friend Shared Function GetLatestNID() As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select @@identity"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to check if the Adaptation already exists in the database
    ''' </summary>
    ''' <param name="AdaptationName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function CheckAdaptationExists(ByVal AdaptationName As String)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "Select AdaptationNID from Adaptations where AdaptationName = '" & AdaptationName & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function

    Friend Shared Function GetAdaptationAssociationCount(ByVal AdaptationNID As String)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select count(*) from " & DIDBConstants.DBDetailsTableName & " where " & DIDBConstants.DBAdaptationNID & "='" & AdaptationNID & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function

    ''' <summary>
    ''' Query to Check the given Indicator and it's GID combination existence in the Table
    ''' </summary>
    ''' <param name="IndName">Indicator Name</param>
    ''' <param name="IndGid">Indicator GID</param>
    ''' <returns>returns the query to check the indicator and gid combination existence</returns>
    ''' <remarks></remarks>
    Friend Shared Function CheckIndicatorandGIDCombination(ByVal IndName As String, ByVal IndGid As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select " & DSIndexNid & " from indicator where " & DIColumns.Indicator.IndicatorName & "='" & IndName & "' and " & DIColumns.Indicator.IndicatorGId & "='" & IndGid & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to check the Area and it's GID combination existence in the Table
    ''' </summary>
    ''' <param name="AreaName">Area Name</param>
    ''' <param name="AreaGid">Area GID</param>
    ''' <returns>returns the query to check the area and gid combination existence</returns>
    ''' <remarks></remarks>
    Friend Shared Function CheckAreaandGIDCombination(ByVal AreaName As String, ByVal AreaGid As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select " & DSIndexNid & " from " & Area & " where " & DIColumns.Area.AreaName & "='" & AreaName & "' and " & DIColumns.Area.AreaGId & "='" & AreaGid & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to check the Unit and it's GID combination existence in the Table
    ''' </summary>
    ''' <param name="UnitName">Unit Name</param>
    ''' <param name="UnitGid">Unit GID</param>
    ''' <returns>returns the query to check the unit and gid combination existence</returns>
    ''' <remarks></remarks>
    Friend Shared Function CheckUnitandGIDCombination(ByVal UnitName As String, ByVal UnitGid As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select " & DSIndexNid & " from " & Unit & " where " & DIColumns.Unit.UnitName & "='" & UnitName & "' and " & DIColumns.Unit.UnitGId & "='" & UnitGid & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to check the Source and it's GID combination existence in the Table
    ''' </summary>
    ''' <param name="SourceName">Source Name</param>
    ''' <param name="SourceGid">Source GID</param>
    ''' <returns>returns the query to check the source and gid combination existence</returns>
    ''' <remarks></remarks>
    Friend Shared Function CheckSourceandGIDCombination(ByVal SourceName As String, ByVal SourceGid As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select " & DSIndexNid & " from " & Source & " where " & DIColumns.IndicatorClassifications.ICName & "='" & SourceName & "' and " & DIColumns.IndicatorClassifications.ICGId & "='" & SourceGid & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to check the Subgroup and it's GID combination existence in the Table
    ''' </summary>
    ''' <param name="SubgroupName">Subgroup Name</param>
    ''' <param name="SubgroupGid">Subgroup GID</param>
    ''' <returns>returns the query to check the subgroup and gid combination existence</returns>
    ''' <remarks></remarks>
    Friend Shared Function CheckSubgroupandGIDCombination(ByVal SubgroupName As String, ByVal SubgroupGid As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select " & DSIndexNid & " from " & Subgroup & " where " & DIColumns.SubgroupVals.SubgroupVal & "='" & SubgroupName & "' and " & DIColumns.SubgroupVals.SubgroupValGId & "='" & SubgroupGid & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to check the TimePeriod existence in the Table
    ''' </summary>
    ''' <param name="timPeriod">Time Period</param>
    ''' <returns>returns the query to check the timeperiod existence</returns>
    ''' <remarks></remarks>
    Friend Shared Function CheckTimePeriod(ByVal timPeriod As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select " & DSIndexNid & " from " & TimePeriod & " where " & DIColumns.Timeperiods.TimePeriod & "='" & timPeriod & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to search the keyword in the Indicator table
    ''' </summary>
    ''' <param name="searchString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function IndicatorSearch(ByVal searchString As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select " & DSIndexNid & " from " & DIDBConstants.Indicator & " where " & DIColumns.Indicator.IndicatorName & " like '%" & searchString & "%'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to search the keyword in the Area table
    ''' </summary>
    ''' <param name="searchString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function AreaSearch(ByVal searchString As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select " & DSIndexNid & " from " & DIDBConstants.Area & " where " & DIColumns.Area.AreaName & " like '%" & searchString & "%'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to search the keyword in the Unit table
    ''' </summary>
    ''' <param name="searchString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function UnitSearch(ByVal searchString As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select " & DSIndexNid & " from " & DIDBConstants.Unit & " where " & DIColumns.Unit.UnitName & " like '%" & searchString & "%'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to search the keyword in the Source table
    ''' </summary>
    ''' <param name="searchString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function SourceSearch(ByVal searchString As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select " & DSIndexNid & " from " & DIDBConstants.Source & " where " & DIColumns.IndicatorClassifications.ICName & " like '%" & searchString & "%'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to search the keyword in the Subgroup table
    ''' </summary>
    ''' <param name="searchString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function SubgroupSearch(ByVal searchString As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select " & DSIndexNid & " from " & DIDBConstants.Subgroup & " where " & DIColumns.SubgroupVals.SubgroupVal & " like '%" & searchString & "%'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to search the keyword in the Time Period table
    ''' </summary>
    ''' <param name="searchString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function TimePeriodSearch(ByVal searchString As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select " & DSIndexNid & " from " & DIDBConstants.TimePeriod & " where " & DIColumns.Timeperiods.TimePeriod & " like '%" & searchString & "%'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to get the Database NID from the Dataset Nid
    ''' </summary>
    ''' <param name="dsIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function GetDSDBIndex(ByVal dsIndex As Integer)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select " & DBNID & " from " & DBRegisteredDatasets & " where " & DSIndexNid & "=" & dsIndex
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to get the Adaptation name by it's ID
    ''' </summary>
    ''' <param name="adaptationNID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function GetAdaptationNameById(ByVal adaptationNID As Integer)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select " & DIDBConstants.DBAdaptation & ",AdaptationVersion from Adaptations where " & DIDBConstants.DBAdaptationNID & "=" & adaptationNID & ""
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to get the Adaptation version by it's ID
    ''' </summary>
    ''' <param name="adaptationNID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function GetAdaptationVersion(ByVal adaptationNID As Integer)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select AdaptationVersion from Adaptations where " & DIDBConstants.DBAdaptationNID & "=" & adaptationNID & ""
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to get the Adaptation NID by the Database NID
    ''' </summary>
    ''' <param name="dbIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function GetAdaptationNidByDBIndex(ByVal dbIndex As String)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select " & DIDBConstants.DBAdaptationNID & " from " & DIDBConstants.DBDetailsTableName & " where " & DIDBConstants.DBNID & "=" & dbIndex
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to get the Database details by it's ID
    ''' </summary>
    ''' <param name="dbIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function GetDatabaseDetails(ByVal dbIndex As Integer)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select " & DIDBConstants.DBServerName & "," & DIDBConstants.DBType & "," & DIDBConstants.DBAdaptationNID & "," & DIDBConstants.DatabaseName & "," & DIDBConstants.DBUsername & "," & DIDBConstants.DBPassword & "," & DIDBConstants.DBPort & " from " & DIDBConstants.DBDetailsTableName & " where " & DIDBConstants.DBNID & "=" & dbIndex
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to get the Dataset info.
    ''' </summary>
    ''' <param name="dbIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function GetDatasetsOfDatabase(ByVal dbIndex As Integer, ByVal langCode As String)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select DSIndex,DSPrefix,DSName,DBMtd_Desc,DBMtd_IndCnt,DBMtd_AreaCnt,DBMtd_IUSCnt,DBMtd_TimeCnt,DBMtd_SrcCnt,DBMtd_DataCnt from " & DIDBConstants.DBRegisteredDatasets & " where " & DIDBConstants.DBNID & "=" & dbIndex & " and LanguageCode = '" & langCode & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    Friend Shared Function GetDatasetsOfDatabase(ByVal dbIndex As Integer)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select DSIndex,DSPrefix,DSName,DBMtd_Desc,DBMtd_IndCnt,DBMtd_AreaCnt,DBMtd_IUSCnt,DBMtd_TimeCnt,DBMtd_SrcCnt,DBMtd_DataCnt from " & DIDBConstants.DBRegisteredDatasets & " where " & DIDBConstants.DBNID & "=" & dbIndex & ""
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Check if the searched keyword already exists in the SearchedRecords table
    ''' </summary>
    ''' <param name="keyword"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function CheckExistingKeywords(ByVal keyword As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select count(*) from " & DIDBConstants.DBSearchResults & " where keywords = '" & keyword & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to retrieve the existing results for a given keyword from the database
    ''' </summary>
    ''' <param name="keyword"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function GetResultsXML(ByVal keyword As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select XMLFile from SearchResults where keywords = '" & keyword & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to retrieve Database Index on the basis of Database name
    ''' </summary>
    ''' <param name="dbName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function GetDBIndexByDBName(ByVal dbName As String)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select " & DIDBConstants.DBNID & " from " & DIDBConstants.DBDetailsTableName & " where " & DIDBConstants.DatabaseName & "='" & dbName & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to get the table from the database from the table name as mentioned in the parameter
    ''' </summary>
    ''' <param name="tblType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function GetTable(ByVal tblType As String)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select * from " & tblType
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to get all the datasets in the Registered Datasets table
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function GetAllDatabases()
        Dim RetVal As String = String.Empty
        Try
            RetVal = "select " & DIDBConstants.DSIndexNid & " from " & DIDBConstants.DBRegisteredDatasets
        Catch ex As Exception

        End Try
        Return RetVal
    End Function

End Class
