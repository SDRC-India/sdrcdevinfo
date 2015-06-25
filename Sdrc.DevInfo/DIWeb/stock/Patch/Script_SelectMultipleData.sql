SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SelectMultipleData_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_SelectMultipleData_XX]
AS

BEGIN

select Indicator_Name as Indicator,Unit_Name as Unit,
Subgroup_Val as Subgroup,Area_Name as Area,ut_data.area_nid as ''Area ID'',
TimePeriod as ''Time Period'',IC_Name as Source,
Data_Value as ''Data Value'',FootNote as Footnotes

from ut_indicator_XX,ut_unit_XX,ut_subgroup_vals_XX,ut_area_XX,
ut_timeperiod,ut_indicator_classifications_XX,ut_data,ut_footnote_XX

where
ut_data.indicator_nid=ut_indicator_XX.indicator_nid and
ut_data.Unit_Nid=ut_unit_XX.Unit_Nid and
ut_data.subgroup_val_nid=ut_subgroup_vals_XX.subgroup_val_nid and
ut_data.area_nid=ut_area_XX.area_nid and
ut_data.timeperiod_nid=ut_timeperiod.timeperiod_nid and
ut_data.footnote_nid=ut_footnote_XX.footnote_nid and
ut_data.source_nid=ut_indicator_classifications_XX.ic_nid and ut_indicator_classifications_XX.ic_type=''sr''


END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'ALTER PROCEDURE [dbo].[sp_SelectMultipleData_XX]
AS

BEGIN

select Indicator_Name as Indicator,Unit_Name as Unit,
Subgroup_Val as Subgroup,Area_Name as Area,ut_data.area_nid as ''Area ID'',
TimePeriod as ''Time Period'',IC_Name as Source,
Data_Value as ''Data Value'',FootNote as Footnotes

from ut_indicator_XX,ut_unit_XX,ut_subgroup_vals_XX,ut_area_XX,
ut_timeperiod,ut_indicator_classifications_XX,ut_data,ut_footnote_XX

where
ut_data.indicator_nid=ut_indicator_XX.indicator_nid and
ut_data.Unit_Nid=ut_unit_XX.Unit_Nid and
ut_data.subgroup_val_nid=ut_subgroup_vals_XX.subgroup_val_nid and
ut_data.area_nid=ut_area_XX.area_nid and
ut_data.timeperiod_nid=ut_timeperiod.timeperiod_nid and
ut_data.footnote_nid=ut_footnote_XX.footnote_nid and
ut_data.source_nid=ut_indicator_classifications_XX.ic_nid and ut_indicator_classifications_XX.ic_type=''sr''


END
' 
END

GO