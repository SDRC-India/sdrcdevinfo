Imports di_Worldwide.DIDBConstants
Imports DevInfo.Lib.DI_LibDAL.Queries
Public Class DeleteQuery
    ''' <summary>
    ''' Query to Truncate all Information in the Database Index Server
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function TruncateAll()
        Dim RetVal As String = String.Empty
        Try
            RetVal = "truncate table " & DIDBConstants.DBDetailsTableName & ";truncate table " & DBRegisteredDatasets & ";truncate table " & DBAdaptations & ";"
            RetVal &= "truncate table " & DIDBConstants.Indicator & ";truncate table " & DIDBConstants.Area & ";truncate table " & DIDBConstants.Unit & ";"
            RetVal &= "truncate table " & DIDBConstants.Subgroup & ";truncate table " & DIDBConstants.Source & ";truncate table " & DIDBConstants.TimePeriod & ";"
            RetVal &= "truncate table " & DIDBConstants.DBSearchResults & ";"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function

    ''' <summary>
    ''' Query to remove the Adaptation from the Registered Adaptations
    ''' </summary>
    ''' <param name="AdaptationNid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function RemoveAdaptation(ByVal AdaptationNid As String)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "delete from " & DIDBConstants.DBAdaptations & " where " & DIDBConstants.DBAdaptationNID & "=" & AdaptationNid & ""
        Catch ex As Exception

        End Try
        Return RetVal
    End Function

    ''' <summary>
    ''' Query to Remove the Database name from the Registered Databases
    ''' </summary>
    ''' <param name="DBName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function RemoveDatabase(ByVal DBName As String)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "delete from " & DIDBConstants.DBDetailsTableName & " where " & DIDBConstants.DatabaseName & "='" & DBName & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function

    ''' <summary>
    ''' Query to remove Dataset from the registered datasets
    ''' </summary>
    ''' <param name="dbIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function RemoveDataset(ByVal dbIndex As String)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "delete from " & DIDBConstants.DBRegisteredDatasets & " where " & DIDBConstants.DBNID & "=" & dbIndex & ""
        Catch ex As Exception

        End Try
        Return RetVal
    End Function

    ''' <summary>
    ''' Query to remove Indicator from Indicator table
    ''' </summary>
    ''' <param name="dsIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function RemoveIndicator(ByVal dsIndex As String)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "delete from " & DIDBConstants.Indicator & " where " & DIDBConstants.DSIndexNid & "='" & dsIndex & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function

    ''' <summary>
    ''' Query to remove Area from Area table
    ''' </summary>
    ''' <param name="dsIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function RemoveArea(ByVal dsIndex As String)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "delete from " & DIDBConstants.Area & " where " & DIDBConstants.DSIndexNid & "='" & dsIndex & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function

    ''' <summary>
    ''' Query to remove Unit from Unit table
    ''' </summary>
    ''' <param name="dsIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function RemoveUnit(ByVal dsIndex As String)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "delete from " & DIDBConstants.Unit & " where " & DIDBConstants.DSIndexNid & "='" & dsIndex & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function

    ''' <summary>
    ''' Query to remove Source from Source table
    ''' </summary>
    ''' <param name="dsIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function RemoveSource(ByVal dsIndex As String)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "delete from " & DIDBConstants.Source & " where " & DIDBConstants.DSIndexNid & "='" & dsIndex & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function

    ''' <summary>
    ''' Query to remove Subgroup from Subgroup table
    ''' </summary>
    ''' <param name="dsIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function RemoveSubgroup(ByVal dsIndex As String)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "delete from " & DIDBConstants.Subgroup & " where " & DIDBConstants.DSIndexNid & "='" & dsIndex & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function

    ''' <summary>
    ''' Query to remove Time Period from TimePeriod table
    ''' </summary>
    ''' <param name="dsIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function RemoveTimePeriod(ByVal dsIndex As String)
        Dim RetVal As String = String.Empty
        Try
            RetVal = "delete from " & DIDBConstants.TimePeriod & " where " & DIDBConstants.DSIndexNid & "='" & dsIndex & "'"
        Catch ex As Exception

        End Try
        Return RetVal
    End Function

End Class
