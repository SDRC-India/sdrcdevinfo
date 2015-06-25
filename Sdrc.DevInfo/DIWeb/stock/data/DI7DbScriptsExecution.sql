-- SET TIMEOUT
EXEC sp_configure 'remote query timeout', 0 
reconfigure with override 
EXEC sp_configure
--SPSEPARATOR--
UPDATE dbo.ut_data SET IUNId = Cast(Indicator_NId as varchar(25)) + '_' + Cast(Unit_NId as varchar(25))

--SPSEPARATOR--

EXEC sp_UpdateISMRD
--SPSEPARATOR--

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UT_Metadata_Category_XX]') AND type in (N'U'))
BEGIN
	IF NOT EXISTS (SELECT column_name FROM INFORMATION_SCHEMA.columns WHERE table_name = 'UT_Metadata_Category_XX' 
	AND COLUMN_NAME IN( N'ParentCategoryNId',N'CategoryGId',N'CategoryDescription',N'IsPresentational',N'IsMandatory'))
	BEGIN
	 ALTER TABLE UT_Metadata_Category_XX
		ADD ParentCategoryNId int NULL,
		 CategoryGId nvarchar(60) NULL,
		 CategoryDescription ntext,
		 IsPresentational bit,
		 IsMandatory bit;
	END 
END

--SPSEPARATOR--
--UPDATE ut_data SET MultipleSource = 1
--WHERE data_nid in 
--(
-- select data_nid from 
-- (Select  d1.data_nid   from

-- (SELECT   d2.IUSNID,d2.Area_NID,t2.timeperiod,t2.timeperiod_nid,d2.data_nid  from UT_Data d2, ut_timeperiod t2  where d2.timeperiod_nid=t2.timeperiod_nid )
--  as d1,
-- (
-- SELECT d.IUSNID,d.Area_NID, Max(t.timeperiod) as timeperiod   
-- from UT_Data d, ut_timeperiod t where d.timeperiod_nid=t.timeperiod_nid
-- group by  d.IUSNID,d.Area_NID  ) 
-- as MRDTable

-- where   d1.iusnid= mrdtable.iusnid and d1.area_nid=mrdtable.area_nid and d1.timeperiod=mrdtable.timeperiod ) as d4

-- where data_nid=d4.data_nid

--)

--EXEC sp_6to7_data

GO

--SPSEPARATOR--

--EXEC [dbo].[sp_6to7_ic_XX]

--SPSEPARATOR--

--EXEC [dbo].[sp_6to7_IUS_XX]

--SPSEPARATOR--

--EXEC [dbo].[sp_6to7_timeperiod]