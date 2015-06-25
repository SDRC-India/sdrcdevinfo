IF EXISTS 
(
	SELECT 1 FROM sys.Indexes IND INNER JOIN sys.tables TBL 
	ON IND.object_id = TBL.object_id
	WHERE TBL.name = 'UT_Data' AND IND.name = 'Index_UT_Data_ON_Indicator_NId' 
)
BEGIN
	DROP INDEX UT_Data.Index_UT_Data_ON_Indicator_NId
END

IF EXISTS 
(
	SELECT 1 FROM sys.Indexes IND INNER JOIN sys.tables TBL 
	ON IND.object_id = TBL.object_id
	WHERE TBL.name = 'UT_Data' AND IND.name = 'Index_UT_Data_ON_Unit_NId' 
)
BEGIN
	DROP INDEX UT_Data.Index_UT_Data_ON_Unit_NId
END

IF EXISTS 
(
	SELECT 1 FROM sys.Indexes IND INNER JOIN sys.tables TBL 
	ON IND.object_id = TBL.object_id
	WHERE TBL.name = 'UT_Data' AND IND.name = 'Index_UT_Data_ON_Subgroup_Val_NId' 
)
BEGIN
	DROP INDEX UT_Data.Index_UT_Data_ON_Subgroup_Val_NId
END

IF EXISTS 
(
	SELECT 1 FROM sys.Indexes IND INNER JOIN sys.tables TBL 
	ON IND.object_id = TBL.object_id
	WHERE TBL.name = 'UT_Data' AND IND.name = 'Index_UT_Data_ON_Area_NId' 
)
BEGIN
	DROP INDEX UT_Data.Index_UT_Data_ON_Area_NId
END

IF EXISTS 
(
	SELECT 1 FROM sys.Indexes IND INNER JOIN sys.tables TBL 
	ON IND.object_id = TBL.object_id
	WHERE TBL.name = 'UT_Data' AND IND.name = 'Index_UT_Data_ON_TimePeriod_NId' 
)
BEGIN
	DROP INDEX UT_Data.Index_UT_Data_ON_TimePeriod_NId
END

IF EXISTS 
(
	SELECT 1 FROM sys.Indexes IND INNER JOIN sys.tables TBL 
	ON IND.object_id = TBL.object_id
	WHERE TBL.name = 'UT_Indicator_Unit_Subgroup' AND IND.name = 'Index_UT_Indicator_Unit_Subgroup_ON_Indicator_NId' 
)
BEGIN
	DROP INDEX UT_Indicator_Unit_Subgroup.Index_UT_Indicator_Unit_Subgroup_ON_Indicator_NId
END

IF EXISTS 
(
	SELECT 1 FROM sys.Indexes IND INNER JOIN sys.tables TBL 
	ON IND.object_id = TBL.object_id
	WHERE TBL.name = 'UT_Indicator_Unit_Subgroup' AND IND.name = 'Index_UT_Indicator_Unit_Subgroup_ON_Unit_NId' 
)
BEGIN
	DROP INDEX UT_Indicator_Unit_Subgroup.Index_UT_Indicator_Unit_Subgroup_ON_Unit_NId
END

IF EXISTS 
(
	SELECT 1 FROM sys.Indexes IND INNER JOIN sys.tables TBL 
	ON IND.object_id = TBL.object_id
	WHERE TBL.name = 'UT_Indicator_Unit_Subgroup' AND IND.name = 'Index_UT_Indicator_Unit_Subgroup_ON_Subgroup_Val_NId' 
)
BEGIN
	DROP INDEX UT_Indicator_Unit_Subgroup.Index_UT_Indicator_Unit_Subgroup_ON_Subgroup_Val_NId
END

IF EXISTS 
(
	SELECT 1 FROM sys.Indexes IND INNER JOIN sys.tables TBL 
	ON IND.object_id = TBL.object_id
	WHERE TBL.name = 'UT_ic_IUS' AND IND.name = 'Index_UT_Indicator_Classifications_IUS_ON_IC_NId' 
)
BEGIN
	DROP INDEX UT_ic_IUS.Index_UT_Indicator_Classifications_IUS_ON_IC_NId
END

IF EXISTS 
(
	SELECT 1 FROM sys.Indexes IND INNER JOIN sys.tables TBL 
	ON IND.object_id = TBL.object_id
	WHERE TBL.name = 'UT_ic_IUS' AND IND.name = 'Index_UT_Indicator_Classifications_IUS_ON_IUSNId' 
)
BEGIN
	DROP INDEX UT_ic_IUS.Index_UT_Indicator_Classifications_IUS_ON_IUSNId
END

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GET_SEARCH_RESULTS]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop proc dbo.GET_SEARCH_RESULTS

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_6to7_data]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop proc dbo.sp_6to7_data

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_6to7_ic]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop proc dbo.sp_6to7_ic

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_6to7_IUS]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop proc dbo.sp_6to7_IUS

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_6to7_timeperiod]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop proc dbo.sp_6to7_timeperiod

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_get_area_wheredataexists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop proc dbo.sp_get_area_wheredataexists

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[SP_GET_AREAS_FROM_NIDS]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop proc dbo.SP_GET_AREAS_FROM_NIDS

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[SP_GET_AS_RESULTS_IU]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop proc dbo.SP_GET_AS_RESULTS_IU

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[SP_GET_AS_RESULTS_IUA]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop proc dbo.SP_GET_AS_RESULTS_IUA

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_get_dataview]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop proc dbo.sp_get_dataview

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_get_dataview_datanid]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop proc dbo.sp_get_dataview_datanid

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_get_dataview3]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop proc dbo.sp_get_dataview3

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_get_ind_wheredataexists]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop proc dbo.sp_get_ind_wheredataexists

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[SP_GET_SEARCH_RESULTS_IU]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop proc dbo.SP_GET_SEARCH_RESULTS_IU

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[SP_GET_SEARCH_RESULTS_IUA]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop proc dbo.SP_GET_SEARCH_RESULTS_IUA

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[SP_GET_SGS_FROM_NIDS]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop proc dbo.SP_GET_SGS_FROM_NIDS

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[SP_GET_SOURCES_FROM_NIDS]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop proc dbo.SP_GET_SOURCES_FROM_NIDS

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[SP_GET_TPS_FROM_NIDS]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
drop proc dbo.SP_GET_TPS_FROM_NIDS

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_6to7_IUS_ISDefaultSubgroup]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [sp_6to7_IUS_ISDefaultSubgroup]


IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Split]') AND xtype in (N'FN', N'IF', N'TF'))
drop function dbo.Split

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FN_GET_SPLITTED_LIST]') AND xtype in (N'FN', N'IF', N'TF'))
drop function dbo.FN_GET_SPLITTED_LIST

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FN_CALCULATE_SCORE]') AND xtype in (N'FN', N'IF', N'TF'))
drop function dbo.FN_CALCULATE_SCORE

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[fn_get_child_areas]') AND xtype in (N'FN', N'IF', N'TF'))
drop function dbo.fn_get_child_areas

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[fn_get_EndDate]') AND xtype in (N'FN', N'IF', N'TF'))
drop function dbo.fn_get_EndDate

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[fn_get_normalized_area_nids]') AND xtype in (N'FN', N'IF', N'TF'))
drop function dbo.fn_get_normalized_area_nids

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[fn_get_Periodicity]') AND xtype in (N'FN', N'IF', N'TF'))
drop function dbo.fn_get_Periodicity

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[fn_get_qs_area_nids]') AND xtype in (N'FN', N'IF', N'TF'))
drop function dbo.fn_get_qs_area_nids

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[fn_get_StartDate]') AND xtype in (N'FN', N'IF', N'TF'))
drop function dbo.fn_get_StartDate

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[getAreaLevel]') AND xtype in (N'FN', N'IF', N'TF'))
drop function dbo.getAreaLevel

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[getAreaNid]') AND xtype in (N'FN', N'IF', N'TF'))
drop function dbo.getAreaNid

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[isReallyNumeric]') AND xtype in (N'FN', N'IF', N'TF'))
drop function dbo.isReallyNumeric

IF NOT EXISTS (SELECT column_name FROM INFORMATION_SCHEMA.columns WHERE table_name = 'ut_data' AND column_name = 'IUNId')
BEGIN
	ALTER TABLE ut_data ADD IUNId varchar(50);
END
GO

IF NOT EXISTS (SELECT column_name FROM INFORMATION_SCHEMA.columns WHERE table_name = 'ut_data' AND column_name = 'isMRD')
BEGIN
	ALTER TABLE ut_data ADD isMRD bit;
END	
GO

-- Add Column MultipleSource
IF NOT EXISTS (SELECT column_name FROM INFORMATION_SCHEMA.columns WHERE table_name = 'ut_data' AND column_name = 'MultipleSource')
BEGIN
 ALTER TABLE ut_data ADD MultipleSource bit;
END 
GO

if(NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
				WHERE TABLE_NAME = N'ut_data' AND  (COLUMN_NAME = N'IsPlannedValue')))
	begin
		-- Add Columns if not exists
		ALTER TABLE ut_data
			ADD IsPlannedValue  bit;
		--	"False = Actual, True = Planned. 		
	end


IF NOT EXISTS (SELECT column_name FROM INFORMATION_SCHEMA.columns WHERE table_name = 'ut_data' AND column_name = 'ConfidenceIntervalUpper')
BEGIN
 ALTER TABLE ut_data ADD ConfidenceIntervalUpper decimal(18, 2);
END 
GO


IF NOT EXISTS (SELECT column_name FROM INFORMATION_SCHEMA.columns WHERE table_name = 'ut_data' AND column_name = 'ConfidenceIntervalLower')
BEGIN
 ALTER TABLE ut_data ADD ConfidenceIntervalLower decimal(18, 2);
END 
GO

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE name = 'DI_SEARCH_RESULTS' )
BEGIN
	DROP TABLE DI_SEARCH_RESULTS
END

IF NOT EXISTS( SELECT 1 FROM dbo.sysobjects WHERE name = 'DI_SEARCH_RESULTS' )
BEGIN
	CREATE TABLE [dbo].[DI_SEARCH_RESULTS](
	[NId] [int] IDENTITY(1,1) NOT NULL,
	[SearchLanguage] [nvarchar](max) NOT NULL CONSTRAINT [DF__DI_SEARCH__Searc__2EDAF651]  DEFAULT (''),
	[IndicatorNId] [int] NULL CONSTRAINT [DF__DI_SEARCH__Indic__2FCF1A8A]  DEFAULT ((0)),
	[UnitNId] [int] NULL CONSTRAINT [DF__DI_SEARCH__UnitN__30C33EC3]  DEFAULT ((0)),
	[AreaNId] [nvarchar](max) NULL CONSTRAINT [DF__DI_SEARCH__AreaN__31B762FC]  DEFAULT ((0)),
	[IsAreaNumeric] [bit] NOT NULL CONSTRAINT [DF_DI_SEARCH_RESULTS_IsAreaNumeric]  DEFAULT ((0)),
	[Indicator] [nvarchar](max) NOT NULL CONSTRAINT [DF__DI_SEARCH__Indic__32AB8735]  DEFAULT (''),
	[Unit] [nvarchar](max) NOT NULL CONSTRAINT [DF__DI_SEARCH___Unit__3493CFA7]  DEFAULT (''),
	[Area] [nvarchar](max) NULL CONSTRAINT [DF__DI_SEARCH___Area__3587F3E0]  DEFAULT (''),
	[DefaultSG] [nvarchar](max) NULL CONSTRAINT [DF__DI_SEARCH__Defau__367C1819]  DEFAULT (''),
	[MRDTP] [nvarchar](max) NULL CONSTRAINT [DF__DI_SEARCH__MRDTP__37703C52]  DEFAULT (''),
	[MRD] [nvarchar](max) NULL CONSTRAINT [DF__DI_SEARCH_R__MRD__3864608B]  DEFAULT (''),
	[AreaCount] [int] NULL CONSTRAINT [DF__DI_SEARCH__AreaC__395884C4]  DEFAULT ((0)),
	[SGCount] [int] NULL CONSTRAINT [DF__DI_SEARCH__SGCou__3A4CA8FD]  DEFAULT ((0)),
	[SourceCount] [int] NULL CONSTRAINT [DF__DI_SEARCH__Sourc__3B40CD36]  DEFAULT ((0)),
	[TPCount] [int] NULL CONSTRAINT [DF__DI_SEARCH__TPCou__3C34F16F]  DEFAULT ((0)),
	[DVCount] [int] NULL CONSTRAINT [DF__DI_SEARCH__DVCou__3D2915A8]  DEFAULT ((0)),
	[AreaNIds] [varchar](max) NULL CONSTRAINT [DF__DI_SEARCH__AreaN__3E1D39E1]  DEFAULT (''),
	[SGNIds] [varchar](max) NULL CONSTRAINT [DF__DI_SEARCH__SGNId__3F115E1A]  DEFAULT (''),
	[SourceNIds] [varchar](max) NULL CONSTRAINT [DF__DI_SEARCH__Sourc__40058253]  DEFAULT (''),
	[TPNIds] [varchar](max) NULL CONSTRAINT [DF__DI_SEARCH__TPNId__40F9A68C]  DEFAULT (''),
	[DVNIds] [varchar](max) NULL CONSTRAINT [DF__DI_SEARCH__DVNId__41EDCAC5]  DEFAULT (''),
	[DVSeries] [nvarchar](max) NULL CONSTRAINT [DF__DI_SEARCH__DVSer__42E1EEFE]  DEFAULT (''),
	[Dimensions] [nvarchar](max) NULL,
	[BlockAreaParentNId] [int] NULL,
 CONSTRAINT [PK__DI_SEARCH_RESULT__2B0A656D] PRIMARY KEY CLUSTERED 
(
	[NId] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[isLangExisting]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[isLangExisting] 
 @DestLngCode varchar(2)
AS
BEGIN
 
 SET NOCOUNT ON;
 
 Select count(*) from ut_language where language_code = @DestLngCode
                
END
' 
END


IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'ut_indicator_classifications_ius'))
BEGIN
    exec sp_rename 'ut_indicator_classifications_ius','ut_ic_ius'
END


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UT_SDMXUser]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UT_SDMXUser](
	[Sender_NId] [int] IDENTITY(1,1) NOT NULL,
	[ID] [nvarchar](255) NULL,
	[Name] [nvarchar](255) NULL,
	[ContactName] [nvarchar](255) NULL,
	[Department] [nvarchar](255) NULL,
	[Role] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NULL,
	[Telephone] [nvarchar](50) NULL,
	[Fax] [nvarchar](50) NULL
) ON [PRIMARY]
END