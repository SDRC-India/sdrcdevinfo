Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports DevInfo.Lib.DI_LibDAL
Imports DevInfo.Lib.DI_LibDAL.Queries
Imports DevInfo.Lib.DI_LibDAL.Queries.DIColumns
Imports System.Xml

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class DIIndexUtility
    Inherits System.Web.Services.WebService

    Private Const diIndex_Subgroup_TARGET_GID As String = "TARGET"
    Private Const diIndex_Subgroup_ACTUAL_GID As String = "ACTUAL"
    Private Const diIndex_Subgroup_WEIGHT_GID As String = "WEIGHT"
    Private Const diIndex_Subgroup_INDEX_GID As String = "INDEX"
    Dim MainIndicatorMAXWeightDataValue As Decimal = 0
    Dim IndexValueForArea As Decimal = 0


#Region " WEb Methods"

    <WebMethod()> _
    Public Function UpdateIUSIndexData(ByVal xmlData As String) As Boolean

        Dim RetVal As Boolean = False
        Dim IndicatorGID As String = String.Empty
        Dim UnitGID As String = String.Empty
        Dim SubgroupValGID As String = String.Empty
        Dim TimePeriod As String = String.Empty
        Dim AreaId As String = String.Empty
        Dim Source As String = String.Empty
        Dim DataValue As String = String.Empty

        Try
            '- Load XML string into XMLDoc object
            Dim XmlDoc As New XmlDocument()
            XmlDoc.LoadXml(xmlData)


            '- Loop each <IC> element
            '- Get GID Details of Indicator, Unit, SubgroupVal 
            ' and Datavalue with Area, TimePeriod & Sources

            '-- Get all XmlNode <IC> in  xml document.

            Dim ICElements As XmlNodeList = XmlDoc.DocumentElement.SelectNodes("//IC")

            If ICElements IsNot Nothing AndAlso ICElements.Count > 0 Then

                For count As Integer = 0 To ICElements.Count - 1
                    Dim Ic_Nid As String = ICElements(count).Attributes("IC_GID").Value
                    If ICElements(count).HasChildNodes Then
                        Try
                            '-- Get all XmlNode <Data> in  xml document.
                            'Dim DataElements As XmlNodeList = XmlDoc.DocumentElement.SelectNodes("//Data")
                            Dim DataElements As XmlNodeList = ICElements(count).ChildNodes ' SelectNodes("//KPI")
                            If DataElements IsNot Nothing AndAlso DataElements.Count > 0 Then
                                For i As Integer = 0 To DataElements.Count - 1
                                    If DataElements(i).HasChildNodes Then
                                        Try
                                            IndicatorGID = DataElements(i).Item(DIColumns.Indicator.IndicatorGId).InnerText
                                            UnitGID = DataElements(i).Item(DIColumns.Unit.UnitGId).InnerText
                                            SubgroupValGID = DataElements(i).Item(DIColumns.SubgroupVals.SubgroupValGId).InnerText
                                            TimePeriod = DataElements(i).Item("TimePeriod").InnerText
                                            AreaId = DataElements(i).Item(DIColumns.Area.AreaID).InnerText
                                            Source = DataElements(i).Item(DIColumns.IndicatorClassifications.ICGId).InnerText
                                            DataValue = DataElements(i).Item(DIColumns.Data.DataValue).InnerText
                                            RetVal = Me.UpdateIUSData(IndicatorGID, UnitGID, SubgroupValGID, DataValue, TimePeriod, AreaId, Source)

                                        Catch e As Exception
                                            'If XMl text is unreadable, then display message on interface. But Do not abort the process.
                                            'TODO: handling of displaying message.
                                        End Try
                                    End If
                                Next
                            End If
                        Catch ex As Exception

                        End Try
                    End If
                    ' TODO calculate index value
                    Me.UpdateIndexValue(Me.GetDBConnection(), Ic_Nid, AreaId)
                Next
                'To do --- Insert the Area's overall Score 
                'Get the percent value of Area's OverallScore Value    
                'Dim AreaOverallScore As Decimal = CDec((MainIndicatorMAXWeightDataValue - IndexValueForArea) * 100)
                'Me.UpdateIUSData(Me.GetDBConnection(), Ic_Nid, AreaId)
            End If

        Catch ex As Exception

        End Try

        Return RetVal

    End Function

    ''' <summary>
    ''' Updates the Data Value in database for given Indicator, unit, Subgroup, Area and TimePeriod
    ''' </summary>
    ''' <param name="indicator_Gid"></param>
    ''' <param name="unit_GId"></param>
    ''' <param name="subgroupVal_GId"></param>
    ''' <param name="dataValue"></param>
    ''' <param name="timePeriod"></param>
    ''' <param name="areaId"></param>
    ''' <param name="source"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateIUSData(ByVal indicator_Gid As String, ByVal unit_GId As String, ByVal subgroupVal_GId As String, ByVal dataValue As String, ByVal timePeriod As String, ByVal areaId As String, ByVal source As String) As Boolean
        Dim retVal As Boolean = False

        If Me.IsDataExists(indicator_Gid, unit_GId, subgroupVal_GId, dataValue, timePeriod, areaId, source) Then
            '-- Update DataValue for given I,U,S Selections
            retVal = Me.UpdateDataValue(indicator_Gid, unit_GId, subgroupVal_GId, dataValue, timePeriod, areaId, source)
        Else
            '- Insert new Record (DataValue) for given I, U, S, A , T
            retVal = Me.InsertIUSData(dataValue, indicator_Gid, unit_GId, subgroupVal_GId, timePeriod, areaId, source)
        End If

        Return retVal
    End Function

    Private Function UpdateDataValue(ByVal indicator_Gid As String, ByVal unit_GId As String, ByVal subgroupVal_GId As String, ByVal dataValue As String, ByVal timePeriod As String, ByVal areaId As String, ByVal source As String) As Boolean
        Dim retVal As Boolean = False
        Dim dbConnection As Connection.DIConnection = Me.GetDBConnection()
        Dim dbQueries As DIQueries = Me.GetDBQueries(dbConnection)

        Try
            Dim SqlUpdate As String = "UPDATE " & dbQueries.TablesName.Area & " AS A INNER JOIN (" & dbQueries.TablesName.Unit & " AS U INNER JOIN (" & dbQueries.TablesName.TimePeriod & " AS T INNER JOIN (" & dbQueries.TablesName.SubgroupVals & " AS S " & _
               " INNER JOIN ((" & dbQueries.TablesName.Indicator & " AS I INNER JOIN " & dbQueries.TablesName.IndicatorUnitSubgroup & " AS IUS ON I.Indicator_NId = IUS.Indicator_NId) INNER JOIN UT_Data AS D ON IUS.IUSNId = D.IUSNId) ON S.Subgroup_Val_NId = IUS.Subgroup_Val_NId) ON T.TimePeriod_NId = D.TimePeriod_NId) ON U.Unit_NId = IUS.Unit_NId) ON A.Area_NId = D.Area_NId " & _
               " SET D.Data_Value = '" & dataValue & "'" & _
               " WHERE (((I.Indicator_GId)='" & indicator_Gid & "') AND ((U.Unit_GId)='" & unit_GId & "') AND ((S.Subgroup_Val_GId)='" & subgroupVal_GId & "')" & _
               "  AND ((T.TimePeriod)='" & timePeriod & "') AND ((A.Area_ID)='" & areaId & "'));"

            dbConnection.ExecuteNonQuery(SqlUpdate)

            retVal = True

        Catch ex As Exception

        Finally
            If Not IsNothing(dbConnection) Then
                dbConnection.Dispose()
            End If
        End Try

        Return retVal
    End Function

    Private Function InsertIUSData(ByVal dataValue As String, ByVal indicator_Gid As String, ByVal unit_GId As String, ByVal subgroup_Val_GId As String, ByVal timePeriod As String, ByVal areaId As String, ByVal source As String) As Boolean
        Dim RetVal As Boolean = False

        Dim dbConnection As Connection.DIConnection = Me.GetDBConnection()
        Dim dbQueries As DIQueries = Me.GetDBQueries(dbConnection)

        Try
            Dim SqlUpdate As String = "Insert INTO UT_Data(IUSNId,TimePeriod_NId, Area_Nid,Data_Value, Indicator_NId,Unit_NId, Subgroup_Val_NId , Source_NId) " & _
" SELECT " & dbQueries.TablesName.IndicatorUnitSubgroup & ".IUSNId, T.TimePeriod_NId,A.Area_NId," & dataValue & ", I.Indicator_NId,  U.Unit_NId,S.Subgroup_Val_NId,IC.IC_NId" & _
 " FROM " & dbQueries.TablesName.IndicatorClassifications & " AS IC, " & dbQueries.TablesName.TimePeriod & " AS T, " & dbQueries.TablesName.Area & " AS A, (" & dbQueries.TablesName.SubgroupVals & " AS S INNER JOIN (" & dbQueries.TablesName.Unit & " AS U INNER JOIN " & dbQueries.TablesName.IndicatorUnitSubgroup & " ON U.Unit_NId = UT_Indicator_Unit_Subgroup.Unit_NId) ON S.Subgroup_Val_NId = UT_Indicator_Unit_Subgroup.Subgroup_Val_NId) INNER JOIN " & dbQueries.TablesName.Indicator & " AS I ON UT_Indicator_Unit_Subgroup.Indicator_NId = I.Indicator_NId " & _
" WHERE (((I.Indicator_GId)='" & indicator_Gid & "') AND ((U.Unit_GId)='" & unit_GId & "') AND ((S.Subgroup_Val_GId)='" & subgroup_Val_GId & "')" & _
" AND ((T.TimePeriod)='" & timePeriod & "') AND ((A.Area_ID)='" & areaId & "') AND ((IC.IC_GId)='" & source & "'))"


            dbConnection.ExecuteNonQuery(SqlUpdate)

            RetVal = True
        Catch ex As Exception

        Finally
            If Not IsNothing(dbConnection) Then
                dbConnection.Dispose()
            End If
        End Try

        Return RetVal
    End Function

    Private Function UpdateIndexValue(ByVal dbConnection As Connection.DIConnection, ByVal ICSector_GID As String, ByVal area_GId As String) As Boolean
        Dim RetVal As Boolean = False
        Dim dbQueries As DIQueries = Me.GetDBQueries(dbConnection)
        Dim DT_ICData As DataTable
        Dim DT_Indicators As DataTable
        Dim SumOfDataValues As Decimal = 0
        Dim AllDataFound As Boolean = False
        Dim RAWIndexesSum As Decimal = 0
        Dim RAWIndex As Decimal = 0
        Dim ActualDataValue As Decimal = 0
        Dim TargetDataValue As Decimal = 0
        Dim WeightDataValue As Decimal = 0
        Dim FinalIndexvalue As Decimal = 0
        Dim SumOfWeightDataValue As Decimal = 0
        Dim DT_TargetVal As DataRow()
        Dim SBQuery As New StringBuilder()
        '- ALGORITHM
        ' Get All Indicators (KPI's) under given IC Sector Name
        ' For Each Indicators (KPI), do the following:-
        '-- Get dataValues for each subgroupVals:- Actual, Tagert, Weight
        '-- and calculate RAW Index Value for each subgroups :- RAWIndex = Weight_Value * (Actual value / Target Value)
        '-- SUM RAWIndex values 
        '-- then calculate INDEX value of Main IC indicator :- WEIGHT * (SUM of RAWIndex) 
        '-- then calculate the score of provided area
        'SBQuery.Append("SELECT D.*,I." & Indicator.IndicatorGId &"I."&Indicator.IndicatorNId &" AS "&  & DIColumns.Indicator.IndicatorNId & "I.Indicator_Type, I."& Indicator.IndicatorName &",IC."& IndicatorClassifications.ICGId &",A." & Area.AreaName &",A."& Area.AreaNId &",S."&SubgroupVals.SubgroupValGId &",S."&Unit.UnitGId & ",T." & Timeperiods.TimePeriod )
        'SBQuery.Append(" FROM (((" & dbQueries.TablesName.Area & " AS A INNER JOIN (((" & dbQueries.TablesName.IndicatorUnitSubgroup & " AS IUS INNER JOIN " & dbQueries.TablesName.Indicator & " AS I ON IUS."& IUS.Indicator_NId = I.Indicator_NId) INNER JOIN " & dbQueries.TablesName.Data & " AS D ON IUS.IUSNId = D.IUSNId) INNER JOIN " & dbQueries.TablesName.Unit & " AS U ON IUS.Unit_NId = U.Unit_Nid) ON A.Area_NId = D.Area_NId) INNER JOIN (" & dbQueries.TablesName.IndicatorClassifications & " AS IC INNER JOIN " & dbQueries.TablesName.IndicatorClassificationsIUS & " AS IC_IUS ON IC.IC_NId = IC_IUS.IC_NId) ON IUS.IUSNId = IC_IUS.IUSNId) INNER JOIN " & dbQueries.TablesName.SubgroupVals & " AS S ON IUS.Subgroup_Val_NId = S.Subgroup_Val_NId ) INNER JOIN " & dbQueries.TablesName.TimePeriod & " AS T ON D.TimePeriod_NId = T.TimePeriod_NId " & _)



        Dim sSql As String = "SELECT D.*,I.Indicator_GID, I.Indicator_NId AS " & DIColumns.Indicator.IndicatorNId & ",I.Indicator_Type" & ", I.Indicator_Name, IC.IC_Name, IC.IC_GID, A.Area_Name, A.Area_ID, S.Subgroup_Val, S.Subgroup_Val_GId, U.Unit_GId, T.TimePeriod" & _
        " FROM (((" & dbQueries.TablesName.Area & " AS A INNER JOIN (((" & dbQueries.TablesName.IndicatorUnitSubgroup & " AS IUS INNER JOIN " & dbQueries.TablesName.Indicator & " AS I ON IUS.Indicator_NId = I.Indicator_NId) INNER JOIN " & dbQueries.TablesName.Data & " AS D ON IUS.IUSNId = D.IUSNId) INNER JOIN " & dbQueries.TablesName.Unit & " AS U ON IUS.Unit_NId = U.Unit_Nid) ON A.Area_NId = D.Area_NId) INNER JOIN (" & dbQueries.TablesName.IndicatorClassifications & " AS IC INNER JOIN " & dbQueries.TablesName.IndicatorClassificationsIUS & " AS IC_IUS ON IC.IC_NId = IC_IUS.IC_NId) ON IUS.IUSNId = IC_IUS.IUSNId) INNER JOIN " & dbQueries.TablesName.SubgroupVals & " AS S ON IUS.Subgroup_Val_NId = S.Subgroup_Val_NId ) INNER JOIN " & dbQueries.TablesName.TimePeriod & " AS T ON D.TimePeriod_NId = T.TimePeriod_NId " & _
        " WHERE IC.IC_GID ='" & ICSector_GID & "' AND A.Area_GID = '" & area_GId & "'"


        DT_ICData = dbConnection.ExecuteDataTable(sSql)

        If DT_ICData.Rows.Count > 0 Then
            '- Get all distinct Indicators 
            ' DT_Indicators = DT_ICData.DefaultView.ToTable(True, DIColumns.Indicator.IndicatorNId, DIColumns.Indicator.IndicatorName, DIColumns.Indicator.IndicatorGId)
            DT_Indicators = DT_ICData.DefaultView.ToTable(True, DIColumns.Indicator.IndicatorNId, DIColumns.Indicator.IndicatorName, DIColumns.Indicator.IndicatorGId)
            DT_TargetVal = DT_ICData.Select(DIColumns.SubgroupVals.SubgroupValNId & "=1", "") '--Get the target values of Indicator

            For Each DRIndicator As DataRow In DT_Indicators.Rows
                SumOfDataValues = 0
                'RAWIndexesSum = 0
                If DRIndicator(DIColumns.Indicator.IndicatorGId) <> ICSector_GID Then
                    AllDataFound = False
                    '-- Get dataValues for each subgroupVals:- Actual, Tagert, Weight
                    ' Get "ACTUAL" DataValue
                    Dim tempValue As String = Me.GetDataValue(DT_ICData, DRIndicator(DIColumns.Indicator.IndicatorGId).ToString(), DIIndexUtility.diIndex_Subgroup_ACTUAL_GID)
                    'Dim tempTargetValue As String = Me.GetDataValue(DT_ICData, DRIndicator(DIColumns.Indicator.IndicatorGId).ToString(), DIIndexUtility.diIndex_Subgroup_TARGET_GID)
                    If Not String.IsNullOrEmpty(tempValue) Then
                        ActualDataValue = tempValue
                        SumOfDataValues += CDec(ActualDataValue)

                        ' Get "TARGET" DataValue
                        tempValue = Me.GetDataValue(DT_ICData, DRIndicator(DIColumns.Indicator.IndicatorGId).ToString(), DIIndexUtility.diIndex_Subgroup_TARGET_GID)
                        If Not String.IsNullOrEmpty(tempValue) Then
                            TargetDataValue = tempValue
                            SumOfDataValues += CDec(TargetDataValue)

                            ' Get "WEIGHT" DataValue
                            tempValue = Me.GetDataValue(DT_ICData, DRIndicator(DIColumns.Indicator.IndicatorGId).ToString(), DIIndexUtility.diIndex_Subgroup_WEIGHT_GID)

                            If Not String.IsNullOrEmpty(tempValue) Then
                                WeightDataValue = tempValue
                                SumOfWeightDataValue += CDec(WeightDataValue)
                                SumOfDataValues += CDec(WeightDataValue)

                                '- Get RAW Index value of this indicator (KPI)
                                RAWIndex = (ActualDataValue / TargetDataValue) * WeightDataValue
                                'RAWIndex = (TargetDataValue / TargetDataValue) * WeightDataValue
                                '- Sum RAW Index Values
                                RAWIndexesSum += RAWIndex
                                AllDataFound = True '-- Indicates that WEIGHT value can be calculated
                            End If
                        End If
                    End If
                End If

            Next

            If AllDataFound Then
                ' Get "WEIGHT" DataValue of Main Indicator
                Dim DRows() As DataRow = DT_ICData.Select(DIColumns.Indicator.IndicatorGId & "='" & ICSector_GID & "' AND " & DIColumns.SubgroupVals.SubgroupVal & "='" & DIIndexUtility.diIndex_Subgroup_WEIGHT_GID & "'")

                If DRows.Length > 0 Then
                    WeightDataValue = Convert.ToString(DRows(0)(DIColumns.Data.DataValue))

                    '- Calculate FINAL INDEX value
                    FinalIndexvalue = RAWIndexesSum * CDec(WeightDataValue)
                    IndexValueFOrArea += FinalIndexvalue
                    '- Calculate Maximum INDEX value of main Indicator
                    MainIndicatorMAXWeightDataValue += CDec(SumOfWeightDataValue * CDec(WeightDataValue))
                    '---- Implementing the concept of Indicator Type 'Good/Bad'
                    'If DRows("Indicator_Type").ToString = "B" Then
                    '    FinalIndexvalue = 100 - FinalIndexvalue
                    'End If

                    '- Update Final INdex Value
                    RetVal = Me.UpdateIUSData(DRows(0)(DIColumns.Indicator.IndicatorGId), DRows(0)(DIColumns.Unit.UnitGId), DIIndexUtility.diIndex_Subgroup_INDEX_GID, FinalIndexvalue, DRows(0)(DIColumns.Timeperiods.TimePeriod), area_GId, DRows(0)(DIColumns.IndicatorClassifications.ICGId))

                End If
            End If
        End If

    End Function

    Private Function IsDataExists(ByVal indicator_Gid As String, ByVal unit_GId As String, ByVal subgroupVal_GId As String, ByVal dataValue As String, ByVal timePeriod As String, ByVal areaId As String, ByVal source As String) As Boolean
        ''- return true, if Data found for given Indicator, unit, Subgroupval, TimePeriod and AreaID

        Dim RetVal As Boolean = False
        Dim dbConnection As Connection.DIConnection = Me.GetDBConnection()
        Dim dbQueries As DIQueries = Me.GetDBQueries(dbConnection)

        Try
            Dim sSql As String = "Select Count(D.Data_NId) FROM " & dbQueries.TablesName.Unit & " AS U INNER JOIN (" & dbQueries.TablesName.Indicator & " AS I INNER JOIN ((" & dbQueries.TablesName.TimePeriod & " AS T INNER JOIN (" & dbQueries.TablesName.Area & " AS A INNER JOIN " & dbQueries.TablesName.Data & " AS D ON A.Area_NId = D.Area_NId) ON T.TimePeriod_NId = D.TimePeriod_NId) INNER JOIN " & dbQueries.TablesName.SubgroupVals & " AS S ON D.Subgroup_Val_NId = S.Subgroup_Val_NId) ON I.Indicator_NId = D.Indicator_NId) ON U.Unit_NId = D.Unit_NId " & _
               " WHERE (((I.Indicator_GId)='" & indicator_Gid & "') AND ((U.Unit_GId)='" & unit_GId & "') AND ((S.Subgroup_Val_GId)='" & subgroupVal_GId & "')" & _
               "  AND ((T.TimePeriod)='" & timePeriod & "') AND ((A.Area_ID)='" & areaId & "'));"

            Dim ResultCount As Int64 = dbConnection.ExecuteScalarSqlQuery(sSql)


            If ResultCount > 0 Then
                RetVal = True
            End If
        Catch ex As Exception

        Finally
            If Not IsNothing(dbConnection) Then
                dbConnection.Dispose()
            End If
        End Try

        Return RetVal
    End Function

    Private Function GetDBConnection() As Connection.DIConnection
        '- Get diIndex Database Name form Web.config settings
        Dim DBFilepath As String = System.Configuration.ConfigurationManager.AppSettings("DIIndexDatabase")
        'Dim DBFilepath As String = Server.MapPath("..\" & System.Configuration.ConfigurationManager.AppSettings("DIIndexDatabase"))

        Return New DevInfo.Lib.DI_LibDAL.Connection.DIConnection(Connection.DIServerType.MsAccess, "", "", DBFilepath, "", "")

    End Function

    Private Function GetDBQueries(ByVal dbConnection As Connection.DIConnection) As DIQueries
        Dim retVal As DIQueries = Nothing

        If Not IsNothing(dbConnection) Then
            retVal = New DIQueries(dbConnection.DIDataSetDefault(), dbConnection.DILanguageCodeDefault(dbConnection.DIDataSetDefault()))
        End If

        Return retVal
    End Function

    Private Function GetDataValue(ByVal DT_IUSData As DataTable, ByVal indicatorGID As String, ByVal subgroupValGID As String) As String
        Dim RetVal As String = String.Empty
        Dim strQuery As String = DIColumns.Indicator.IndicatorGId & "='" & indicatorGID & "' AND " & DIColumns.SubgroupVals.SubgroupValGId & "='" & subgroupValGID & "'"

        Dim DRows() As DataRow = DT_IUSData.Select(strQuery)

        If DRows.Length > 0 Then
            RetVal = Convert.ToString(DRows(0)(DIColumns.Data.DataValue))
        End If

        Return RetVal
    End Function

#End Region

#Region " Private"

#End Region

End Class