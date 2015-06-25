Imports di_Worldwide.DIDBConstants
Imports DevInfo.Lib.DI_LibDAL.Queries
Public Class UpdateQuery
    ''' <summary>
    ''' Query to update the Dataset Index of an already existing Indicator and it's GID combination
    ''' </summary>
    ''' <param name="IndName">Indicator Name</param>
    ''' <param name="IndGid">Indicator GID</param>
    ''' <param name="DSIndex">Updated Dataset Index</param>
    ''' <returns>returns the query to update the Dataset Index</returns>
    ''' <remarks></remarks>
    Friend Shared Function UpdateIndicator(ByVal IndName As String, ByVal IndGid As String, ByVal DSIndex As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "Update " & Indicator & " set " & DSIndexNid & "='" & DSIndex & "' where " & DIColumns.Indicator.IndicatorName & "='" & IndName & "' and " & DIColumns.Indicator.IndicatorGId & "='" & IndGid & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to update the Dataset Index of an Area
    ''' </summary>
    ''' <param name="AreaName">Area Name</param>
    ''' <param name="AreaGid">Area GID</param>
    ''' <param name="DSIndex">updated Dataset Index</param>
    ''' <returns>returns the query to update the Dataset Index</returns>
    ''' <remarks></remarks>
    Friend Shared Function UpdateArea(ByVal AreaName As String, ByVal AreaGid As String, ByVal DSIndex As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "Update " & Area & " set " & DSIndexNid & "='" & DSIndex & "' where " & DIColumns.Area.AreaName & "='" & AreaName & "' and " & DIColumns.Area.AreaGId & "='" & AreaGid & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to update the Dataset Index of a Source
    ''' </summary>
    ''' <param name="SourceName">Source Name</param>
    ''' <param name="SourceGid">Source GID</param>
    ''' <param name="DSIndex">updated Dataset Index</param>
    ''' <returns>returns the query to update the Dataset Index</returns>
    ''' <remarks></remarks>
    Friend Shared Function UpdateSource(ByVal SourceName As String, ByVal SourceGid As String, ByVal DSIndex As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "Update " & Source & " set " & DSIndexNid & "='" & DSIndex & "' where " & DIColumns.IndicatorClassifications.ICName & "='" & SourceName & "' and " & DIColumns.IndicatorClassifications.ICGId & "='" & SourceGid & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to update the Dataset Index of a Subgroup
    ''' </summary>
    ''' <param name="SubgroupName">Subgroup Name</param>
    ''' <param name="SubgroupGid">Subgroup GID</param>
    ''' <param name="DSIndex">updated Dataset Index</param>
    ''' <returns>returns the query to update the Dataset Index</returns>
    ''' <remarks></remarks>
    Friend Shared Function UpdateSubgroup(ByVal SubgroupName As String, ByVal SubgroupGid As String, ByVal DSIndex As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "Update " & Subgroup & " set " & DSIndexNid & "='" & DSIndex & "' where " & DIColumns.SubgroupVals.SubgroupVal & "='" & SubgroupName & "' and " & DIColumns.SubgroupVals.SubgroupValGId & "='" & SubgroupGid & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to update the Dataset index of a Unit
    ''' </summary>
    ''' <param name="UnitName">Unit Name</param>
    ''' <param name="UnitGid">Unit GID</param>
    ''' <param name="DSIndex">updated Dataset Index</param>
    ''' <returns>returns the query to update the Dataset Index</returns>
    ''' <remarks></remarks>
    Friend Shared Function UpdateUnit(ByVal UnitName As String, ByVal UnitGid As String, ByVal DSIndex As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "Update " & Unit & " set " & DSIndexNid & "='" & DSIndex & "' where " & DIColumns.Unit.UnitName & "='" & UnitName & "' and " & DIColumns.Unit.UnitGId & "='" & UnitGid & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to update the Dataset Index of a TimePeriod
    ''' </summary>
    ''' <param name="TimPeriod">Time Period</param>
    ''' <param name="DSIndex">updated Dataset Index</param>
    ''' <returns>returns the query to update the Dataset Index</returns>
    ''' <remarks></remarks>
    Friend Shared Function UpdateTimePeriod(ByVal TimPeriod As String, ByVal DSIndex As String) As String
        Dim RetVal As String = String.Empty
        Try
            RetVal = "Update " & TimePeriod & " set " & DSIndexNid & "='" & DSIndex & "' where " & DIColumns.Timeperiods.TimePeriod & "='" & TimPeriod & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Query to update the Adaptation version of an existing Adaptation
    ''' </summary>
    ''' <param name="AdaptationName"></param>
    ''' <param name="AdaptationVersion"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function UpdateAdaptationVersion(ByVal AdaptationName As String, ByVal AdaptationVersion As String)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "Update Adaptations set AdaptationVersion = '" & AdaptationVersion & "' where AdaptationName = '" & AdaptationName & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function
End Class
