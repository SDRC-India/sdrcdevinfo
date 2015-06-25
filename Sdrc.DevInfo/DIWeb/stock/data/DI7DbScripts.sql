-- SET TIMEOUT
EXEC sp_configure 'remote query timeout', 0 
reconfigure with override 
EXEC sp_configure

UPDATE dbo.ut_data SET IUNId = Cast(Indicator_NId as varchar(25)) + '_' + Cast(Unit_NId as varchar(25))


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_UpdateISMRD]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE Procedure [dbo].[sp_UpdateISMRD]
AS

-- SET TIMEOUT
EXEC SP_CONFIGURE ''remote query timeout'', 0
reconfigure
EXEC sp_configure

SET NOCOUNT ON
UPDATE ut_data SET ismrd = 0


UPDATE ut_data SET ismrd = 1
WHERE data_nid in 
(
  select data_nid from 

 (Select  d1.IUSNID,d1.Area_NID,d1.timeperiod_nid,Max(d1.data_nid) as data_nid   from

		 (SELECT   d2.IUSNID,d2.Area_NID,t2.timeperiod,t2.timeperiod_nid,d2.data_nid  from UT_Data d2, ut_timeperiod t2 
		where d2.timeperiod_nid = t2.timeperiod_nid ) as d1,

		 ( SELECT d.IUSNID,d.Area_NID, Max(t.timeperiod) as timeperiod   
		 from UT_Data d, ut_timeperiod t where d.timeperiod_nid=t.timeperiod_nid
		 group by  d.IUSNID,d.Area_NID  ) as MRDTable

			where   d1.iusnid= mrdtable.iusnid 
					and d1.area_nid=mrdtable.area_nid 
					and d1.timeperiod=mrdtable.timeperiod 

		group by  d1.IUSNID,d1.Area_NID,d1.timeperiod_nid

) as d4

)

' 
END

EXEC sp_UpdateISMRD

--SPSEPARATOR--

----------------------------------------------------------- By Pankaj   ------------------------------------

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


UPDATE ut_data SET MultipleSource = 1
WHERE data_nid in 
(
 select data_nid from 
 (Select  d1.data_nid   from

 (SELECT   d2.IUSNID,d2.Area_NID,t2.timeperiod,t2.timeperiod_nid,d2.data_nid  from UT_Data d2, ut_timeperiod t2  where d2.timeperiod_nid=t2.timeperiod_nid )
  as d1,
 (
 SELECT d.IUSNID,d.Area_NID, Max(t.timeperiod) as timeperiod   
 from UT_Data d, ut_timeperiod t where d.timeperiod_nid=t.timeperiod_nid
 group by  d.IUSNID,d.Area_NID  ) 
 as MRDTable

 where   d1.iusnid= mrdtable.iusnid and d1.area_nid=mrdtable.area_nid and d1.timeperiod=mrdtable.timeperiod ) as d4

 where data_nid=d4.data_nid

)


--SPSEPARATOR--
-- FUNCTION [CustomCast]

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[CustomCast]') AND xtype in (N'FN', N'IF', N'TF'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[CustomCast]  
(  
    @Input FLOAT  
)  
RETURNS NVARCHAR(MAX) 
BEGIN 
DECLARE @RetVal	NVARCHAR(MAX)
DECLARE @IntPart BIGINT
DECLARE @DecimalPart FLOAT


SET @RetVal = ''''

IF (@Input >= 0)
BEGIN
	SET @IntPart = FLOOR(@Input)
	SET @DecimalPart = @Input - @IntPart
	
	IF (@Input - @IntPart = 0)
	BEGIN
		SET @RetVal =  @IntPart
	END
	ELSE
	BEGIN
		IF (LEN(CAST(@IntPart AS VARCHAR(MAX)) + ''.'' + REPLACE(CAST(@DecimalPart AS VARCHAR(MAX)), ''0.'', '''')) > 16)
		BEGIN
			SET @RetVal =  CAST(@IntPart AS VARCHAR(MAX))
		END
		ELSE
		BEGIN
			SET @RetVal =  CAST(@IntPart AS VARCHAR(MAX)) + ''.'' + REPLACE(CAST(@DecimalPart AS VARCHAR(MAX)), ''0.'', '''')	
		END
	END
END
ELSE
BEGIN
	SET @IntPart = CEILING(@Input)
	SET @DecimalPart = @Input - @IntPart
	
	IF (@Input - @IntPart = 0)
	BEGIN
		SET @RetVal =  @IntPart
	END
	ELSE
	BEGIN
		IF ((LEN(CAST(@IntPart AS VARCHAR(MAX)) + ''.'' + REPLACE(CAST(@DecimalPart AS VARCHAR(MAX)), ''0.'', '''')) > 16))
		BEGIN
			SET @RetVal =  CAST(@IntPart AS VARCHAR(MAX))
		END
		ELSE
		BEGIN
			SET @RetVal =  CAST(@IntPart AS VARCHAR(MAX)) + ''.'' + REPLACE(CAST(ABS(@DecimalPart) AS VARCHAR(MAX)), ''0.'', '''')
		END
	END
END
	RETURN @RetVal

END 
	
' 
END

-- FUNCTION [isReallyNumeric]

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[isReallyNumeric]') AND xtype in (N'FN', N'IF', N'TF'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[isReallyNumeric]  
(  
    @num VARCHAR(64)  
)  
RETURNS BIT  
BEGIN  
    IF LEFT(@num, 1) = ''-''  
        SET @num = SUBSTRING(@num, 2, LEN(@num))  
 
    DECLARE @pos TINYINT  
 
    SET @pos = 1 + LEN(@num) - CHARINDEX(''.'', REVERSE(@num))  
 
    RETURN CASE  
    WHEN PATINDEX(''%[^0-9.-]%'', @num) = 0  
        AND @num NOT IN (''.'', ''-'', ''+'', ''^'') 
        AND LEN(@num)>0  
        AND @num NOT LIKE ''%-%'' 
        AND  
        (  
            ((@pos = LEN(@num)+1)  
            OR @pos = CHARINDEX(''.'', @num))  
        )  
    THEN  
        1  
    ELSE  
    0  
    END  
END  
' 
END


-- FUNCTION [fn_get_StartDate]

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[fn_get_StartDate]') AND xtype in (N'FN', N'IF', N'TF'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_get_StartDate]
(
	@timeperiod nvarchar(30)
)
RETURNS DateTime
AS
BEGIN
	Declare @Result DateTime

	-- YYYY	OR YYYY.MM
	if(LEN(@timeperiod) =  4)
		SET @Result =  CONVERT(DateTime,@timeperiod,20);
	ELSE if(LEN(@timeperiod) =  7 OR LEN(@timeperiod) =  6) 
		SET @Result =  CONVERT(DateTime,@timeperiod+''.01'',20);
	-- 0000.00.00      
	ELSE if(LEN(@timeperiod) =  10 OR LEN(@timeperiod) =  8  )
		SET @Result = CONVERT(DateTime,@timeperiod,20);
    -- 0000-0000
	ELSE if(LEN(@timeperiod) =  9  )
			SET @Result =  CONVERT(DateTime,LEFT(@timeperiod,4),20);
	-- 0000.00-0000.00
	ELSE if(LEN(@timeperiod) =  15 OR LEN(@timeperiod) =  13  )
		BEGIN				
		if(substring(@timeperiod,8,1)=''-'')
			SET @Result = CONVERT(DateTime,LEFT(@timeperiod,7)+''.01'',20);
		else if(substring(@timeperiod,7,1)=''-'')
			SET @Result = CONVERT(DateTime,LEFT(@timeperiod,6)+''.01'',20);
		END
       -- 0000.00.00-0000.00.00 or 0000.0.0-0000.0.0 or 0000.0.00-0000.00.00
     ELSE if(LEN(@timeperiod) =  19 OR LEN(@timeperiod) =  21 OR LEN(@timeperiod) =  17 OR LEN(@timeperiod) =  18 OR LEN(@timeperiod) =  20 )
		BEGIN				
			if(substring(@timeperiod,11,1)=''-'')
				SET @Result = CONVERT(DateTime,LEFT(@timeperiod,10),20);
			else if(substring(@timeperiod,10,1)=''-'')
				SET @Result = CONVERT(DateTime,LEFT(@timeperiod,9),20);
			else if(substring(@timeperiod,9,1)=''-'')
				SET @Result = CONVERT(DateTime,LEFT(@timeperiod,8),20);		
		END
	

	RETURN @Result

END
' 
END


-- FUNCTION [fn_get_EndDate]

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[fn_get_EndDate]') AND xtype in (N'FN', N'IF', N'TF'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_get_EndDate]
(
	@timeperiod nvarchar(30)
)
RETURNS DateTime
AS
BEGIN
	Declare @Result DateTime,
			@TempDate DateTime

	
	if(LEN(@timeperiod) =  4)	-- YYYY
		SET @Result =  CONVERT(DateTime,@timeperiod+''.12.31'',20);

	ELSE if(LEN(@timeperiod) =  7 OR LEN(@timeperiod) =  6) -- YYYY.MM
		
		-- Set Last day of Month
		SET @Result =  CONVERT(DateTime,DATEADD(day,-1,DATEADD(month,1,@timeperiod+''.01'')),20)

	ELSE if(LEN(@timeperiod) =  10 OR LEN(@timeperiod) =  8  )-- yyyy.mm.dd / yyyy.m.d
		
		SET @Result = CONVERT(DateTime,@timeperiod,20);
   
	ELSE if(LEN(@timeperiod) =  9  ) -- yyyy-yyyy
			-- SET last day of year
			SET @Result =  CONVERT(DateTime,RIGHT(@timeperiod,4)+''.12.31'',20);
	
	ELSE if(LEN(@timeperiod) =  15 OR LEN(@timeperiod) =  13  )-- yyyy.mm-yyyy.mm
		BEGIN				
			if(substring(@timeperiod,8,1)=''-'')
				begin
					-- Set set 1st day of month
					SET @Result = CONVERT(DateTime,RIGHT(@timeperiod,LEN(@timeperiod)-8)+''.01'',20);
					-- Get last day of month
					SET @Result =DATEADD(day,-1,DATEADD(month,1,CONVERT(DateTime,  @Result  ,20)))
				end
			else if(substring(@timeperiod,7,1)=''-'')
			begin
				-- Set set 1st day of month
				SET @Result = CONVERT(DateTime,RIGHT(@timeperiod,LEN(@timeperiod)-7)+''.01'',20);
				-- Get last day of month
				SET @Result =DATEADD(day,-1,DATEADD(month,1,CONVERT(DateTime,  @Result  ,20)))
			end
		END
       -- 0000.00.00-0000.00.00 or 0000.0.0-0000.0.0 or 0000.0.00-0000.00.00
     ELSE if(LEN(@timeperiod) =  19 OR LEN(@timeperiod) =  21 OR LEN(@timeperiod) =  17 OR LEN(@timeperiod) =  18 OR LEN(@timeperiod) =  20 )
		BEGIN				
			if(substring(@timeperiod,11,1)=''-'')
				SET @Result = CONVERT(DateTime,RIGHT(@timeperiod,LEN(@timeperiod)-11),20);
				
			else if(substring(@timeperiod,10,1)=''-'')
				SET @Result = CONVERT(DateTime,RIGHT(@timeperiod,LEN(@timeperiod)-10),20);
			else if(substring(@timeperiod,9,1)=''-'')
				SET @Result = CONVERT(DateTime,RIGHT(@timeperiod,LEN(@timeperiod)-9),20);		
		END
	

	RETURN @Result

END' 
END


-- FUNCTION [fn_get_Periodicity]

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[fn_get_Periodicity]') AND xtype in (N'FN', N'IF', N'TF'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_get_Periodicity]
(
	@timeperiod nvarchar(30)
)
RETURNS int
AS
BEGIN
	Declare @Result int
			
		-- 0 = Yearly, 1 - Quarterly 2 - Monthly, 3- Daily
		if(LEN(@timeperiod) =  4 OR LEN(@timeperiod) =  9)	-- YYYY/yyyy-yyyy
			SET @Result =  0;		-- 0 = Yearly

		ELSE if(LEN(@timeperiod) =  7 OR LEN(@timeperiod) =  6 or LEN(@timeperiod) =  15 OR LEN(@timeperiod) =  13) -- YYYY.MM		
			-- 2 - Monthly
			SET @Result =  2

		ELSE if(LEN(@timeperiod) =  10 OR LEN(@timeperiod) =  8 OR LEN(@timeperiod) =  19 -- yyyy.mm.dd / yyyy.m.d
			OR LEN(@timeperiod) =  21 OR LEN(@timeperiod) =  17 OR LEN(@timeperiod) =  18 OR LEN(@timeperiod) =  20 ) 
			-- 3- Daily
			SET @Result = 3
		ELSE
			SET @Result =  0;
		

	RETURN @Result

END' 
END

--SPSEPARATOR--

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_6to7_ic_XX]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_6to7_ic_XX]
AS
	-- Check Column exists or not into datatable	
	if(NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
				WHERE TABLE_NAME = N''ut_indicator_classifications_XX'' AND COLUMN_NAME IN( N''ISBN'',N''Nature'')))
	begin
		-- Add Columns if not exists
		ALTER TABLE ut_indicator_classifications_XX
			ADD ISBN  ntext,
				Nature  ntext

		--	TODO : "False = Actual, True = Planned. Seprate rows for planned and actual data"
	end

' 
END


IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_6to7_IUS_ISDefaultSubgroup_XX]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_6to7_IUS_ISDefaultSubgroup_XX]
AS

DECLARE @sql nvarchar(MAX)
--Update DefaultSubgroup IUS where Subgroup is ''Total''
SET @sql= N''UPDATE UT_Indicator_Unit_Subgroup 
				SET IsDefaultSubgroup = 1
				WHERE Subgroup_Val_NId IN 
					( SELECT IUS.Subgroup_Val_NId FROM UT_Subgroup_Vals_XX AS S
						INNER JOIN UT_Indicator_Unit_Subgroup AS IUS 
						ON S.Subgroup_Val_NId = IUS.Subgroup_Val_NId 
						 WHERE S.Subgroup_Val =''''Total'''')''

	EXEC sp_executesql @sql;

--Update DefaultSubgroup IUS where Subgroup is not Total
SET @sql= N''Update ut_indicator_unit_subgroup
SET  IsDefaultSubgroup=1
where IUSNID in
(
	Select IUSNID from 
	(
			Select MIN( IUS1.IUSNID) as IUSNID,  IUS1.Indicator_NId, IUS1.Unit_NId 
			from ut_indicator_unit_subgroup as IUS1,
			(
				SELECT distinct IUS.Indicator_NId ,IUS.Unit_NId
				FROM  ut_indicator_unit_subgroup AS IUS INNER JOIN
                      ut_indicator_XX AS I ON IUS.Indicator_NId = I.Indicator_NId INNER JOIN
                      ut_unit_XX AS U ON IUS.Unit_NId = U.Unit_NId INNER JOIN
                      ut_subgroup_vals_XX AS S ON IUS.Subgroup_Val_NId = S.Subgroup_Val_NId
				where NOT exists(
				SELECT distinct IUS1.Indicator_NId ,IUS1.Unit_NId
				FROM  ut_indicator_unit_subgroup AS IUS1 INNER JOIN
                      ut_indicator_XX AS I1 
					ON IUS1.Indicator_NId = I1.Indicator_NId INNER JOIN
                      ut_unit_XX AS U1 
						ON IUS1.Unit_NId = U1.Unit_NId INNER JOIN
                      ut_subgroup_vals_XX AS S1
					ON IUS1.Subgroup_Val_NId = S1.Subgroup_Val_NId
				WHERE S1.Subgroup_Val =''''Total'''' and IUS1.Indicator_NId = IUS.Indicator_NId 
				AND IUS1.Unit_NId = IUS.Unit_NId)
				 
				) as T1
			WHERE T1.Indicator_Nid=IUS1.Indicator_NId and T1.Unit_NId=IUS1.Unit_NId
			GROUP BY IUS1.Indicator_NId, IUS1.Unit_NId 
)as T2 )''

	EXEC sp_executesql @sql;

' 
END


IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_6to7_IUS_XX]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_6to7_IUS_XX]
AS
	-- Check Column exists or not into datatable	
	if(NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
				WHERE TABLE_NAME = N''UT_Indicator_Unit_Subgroup'' 
				AND  COLUMN_NAME IN( N''IsDefaultSubgroup'', N''AvlMinDataValue'',
					N''AvlMaxDataValue'',N''AvlMinTimePeriod'' ,N''AvlMaxTimePeriod'')))
	begin
		-- Add Columns if not exists
		ALTER TABLE ut_indicator_unit_subgroup
			ADD IsDefaultSubgroup bit default 0,
			AvlMinDataValue	Decimal(18,2),
			AvlMaxDataValue	Decimal(18,2),
			AvlMinTimePeriod text,
			AvlMaxTimePeriod text
	end

	DECLARE @sql nvarchar(MAX),
			@TotalValue nvarchar(10)
	-- UPDATE DEFAULT SUBGROUP Column Status
	SET @sql= N''UPDATE UT_Indicator_Unit_Subgroup SET IsDefaultSubgroup = 0''
	EXEC sp_executesql @sql;

	-- Set Default Subgroup
	--EXEC sp_6to7_IUS_ISDefaultSubgroup_XX;

	--UPDATE AvlMinDataValue
	SET @sql= N''UPDATE UT_Indicator_Unit_Subgroup 
	SET AvlMinDataValue =(SELECT MIN(D.Data_Value) FROM UT_Data AS D
							where UT_Indicator_Unit_Subgroup.IUSNID=D.IUSNID AND dbo.isReallyNumeric(D.Data_Value) = 1)''
	EXEC sp_executesql @sql;

	--UPDATE AvlMaxDataValue
	SET @sql= N''UPDATE UT_Indicator_Unit_Subgroup 
	SET AvlMaxDataValue =(SELECT MAX(D.Data_Value) FROM UT_Data AS D
							where UT_Indicator_Unit_Subgroup.IUSNID=D.IUSNID AND dbo.isReallyNumeric(D.Data_Value) = 1)''
	EXEC sp_executesql @sql;

	--UPDATE AvlMinTimePeriod
	SET @sql= N''UPDATE ut_Indicator_Unit_Subgroup 
	SET AvlMinTimePeriod =
			(SELECT Min(T.Timeperiod) FROM ut_Data AS D
							INNER JOIN ut_timeperiod as T 
				on T.timeperiod_nid=D.timeperiod_nid 
				and ut_Indicator_Unit_Subgroup.IUSNID=D.IUSNID)''
	EXEC sp_executesql @sql;
	
	-- UPDATE AvlMaxTimePeriod
	SET @sql= N''UPDATE ut_Indicator_Unit_Subgroup 
	SET AvlMaxTimePeriod =
			(SELECT Max(T.Timeperiod) FROM ut_Data AS D
							INNER JOIN ut_timeperiod as T 
				on T.timeperiod_nid=D.timeperiod_nid 
				and ut_Indicator_Unit_Subgroup.IUSNID=D.IUSNID)''
	EXEC sp_executesql @sql;
' 
END



IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_6to7_timeperiod]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_6to7_timeperiod]
AS
	-- Check Column exists or not into datatable	
	if(NOT EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
				WHERE TABLE_NAME = N''ut_timeperiod'' AND  
				(COLUMN_NAME IN( N''StartDate'',N''EndDate'',N''Periodicity''))))
	begin
		-- Add Columns if not exists
		ALTER TABLE ut_timeperiod
			ADD StartDate  DateTime,
				EndDate  DateTime,
				Periodicity  int			
	end
	
	DECLARE @sql nvarchar(2000)
	-- UPDATE STARTDATE, Last =date and Periodicity-- 0 = Yearly, 1 - Quarterly 2 - Monthly, 3- Daily
	SET @sql=N''UPDATE ut_timeperiod
		SET StartDate=dbo.fn_get_StartDate(Timeperiod),
		EndDate = dbo.fn_get_EndDate(Timeperiod),
		Periodicity=dbo.fn_get_Periodicity(Timeperiod)''
	EXEC sp_executesql @sql;
 
' 
END

--SPSEPARATOR--

SET ANSI_NULLS ON


GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_6to7_data]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
create PROCEDURE [dbo].[sp_6to7_data]
AS
DECLARE @tbl nvarchar(50),
        @sql nvarchar(4000)
    
 
IF NOT EXISTS (SELECT column_name FROM INFORMATION_SCHEMA.columns WHERE table_name = ''ut_data'' AND column_name = ''Textual_Data_Value'')
BEGIN
 ALTER TABLE ut_data ADD Textual_Data_Value nvarchar(4000);



IF NOT EXISTS (SELECT column_name FROM INFORMATION_SCHEMA.columns WHERE table_name = ''ut_data'' AND column_name = ''IsTextualData'')
BEGIN
 ALTER TABLE ut_data ADD IsTextualData bit DEFAULT 0;
END 


-- Add Columns if not exists
IF NOT EXISTS (SELECT column_name FROM INFORMATION_SCHEMA.columns WHERE table_name = ''ut_data'' AND column_name = ''Data_Value_Temp'')
BEGIN
ALTER TABLE ut_data
	ADD Data_Value_Temp  decimal(18, 2)
END	

set @tbl = N''ut_data''
--Update Textual value false if data value is Numeric
SET @sql=N''UPDATE '' + @tbl + N'' SET IsTextualData = 0 where dbo.isReallyNumeric(data_value)=1''
EXEC sp_executesql @sql;

--Update Textual value true if data value is not Numeric
SET @sql=N''UPDATE '' + @tbl + N'' SET IsTextualData = 1 where dbo.isReallyNumeric(data_value)=0''
EXEC sp_executesql @sql;

--Update Numeric values into Temp DataColumn 
SET @sql=N''UPDATE '' + @tbl + N''	Set Data_Value_Temp = Data_Value WHERE IsTextualData = 0''
EXEC sp_executesql @sql;

--Update Textual value into Textual_Data_Value column
SET @sql=N''UPDATE '' + @tbl + N'' Set Textual_Data_Value = Data_Value WHERE IsTextualData = 1''
EXEC sp_executesql @sql;

-- Drop Original Data_value column and rename new Numeric column
ALTER TABLE ut_data DROP COLUMN Data_Value;

-- Rename Temp DataValue Column into Original Data_Value Column
EXEC sp_rename ''ut_data.Data_Value_Temp'', ''Data_Value'', ''COLUMN''

END 

' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[sp_6to7_data]
AS
DECLARE @tbl nvarchar(50),
        @sql nvarchar(4000)
    
 
IF NOT EXISTS (SELECT column_name FROM INFORMATION_SCHEMA.columns WHERE table_name = ''ut_data'' AND column_name = ''Textual_Data_Value'')
BEGIN
 ALTER TABLE ut_data ADD Textual_Data_Value nvarchar(4000);



IF NOT EXISTS (SELECT column_name FROM INFORMATION_SCHEMA.columns WHERE table_name = ''ut_data'' AND column_name = ''IsTextualData'')
BEGIN
 ALTER TABLE ut_data ADD IsTextualData bit DEFAULT 0;
END 


-- Add Columns if not exists
IF NOT EXISTS (SELECT column_name FROM INFORMATION_SCHEMA.columns WHERE table_name = ''ut_data'' AND column_name = ''Data_Value_Temp'')
BEGIN
ALTER TABLE ut_data
	ADD Data_Value_Temp  decimal(18, 2)
END	

set @tbl = N''ut_data''
--Update Textual value false if data value is Numeric
SET @sql=N''UPDATE '' + @tbl + N'' SET IsTextualData = 0 where dbo.isReallyNumeric(data_value)=1''
EXEC sp_executesql @sql;

--Update Textual value true if data value is not Numeric
SET @sql=N''UPDATE '' + @tbl + N'' SET IsTextualData = 1 where dbo.isReallyNumeric(data_value)=0''
EXEC sp_executesql @sql;

--Update Numeric values into Temp DataColumn 
SET @sql=N''UPDATE '' + @tbl + N''	Set Data_Value_Temp = Data_Value WHERE IsTextualData = 0''
EXEC sp_executesql @sql;

--Update Textual value into Textual_Data_Value column
SET @sql=N''UPDATE '' + @tbl + N'' Set Textual_Data_Value = Data_Value WHERE IsTextualData = 1''
EXEC sp_executesql @sql;

-- Drop Original Data_value column and rename new Numeric column
ALTER TABLE ut_data DROP COLUMN Data_Value;

-- Rename Temp DataValue Column into Original Data_Value Column
EXEC sp_rename ''ut_data.Data_Value_Temp'', ''Data_Value'', ''COLUMN''

END 

' 
END

GO

--EXEC sp_6to7_data

GO

--SPSEPARATOR--

--EXEC [dbo].[sp_6to7_ic_XX]

--SPSEPARATOR--

--EXEC [dbo].[sp_6to7_IUS_XX]

--SPSEPARATOR--

--EXEC [dbo].[sp_6to7_timeperiod]
   

--SPSEPARATOR--

--------------------------------------SCRIPTS FOR DATAVIEW & QDS----------------------------------------------


IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Split]') AND xtype in (N'FN', N'IF', N'TF'))
BEGIN
execute dbo.sp_executesql @statement = N'    CREATE FUNCTION [dbo].[Split](@String varchar(MAX), @Delimiter char(1))       
    returns @temptable TABLE (items varchar(MAX))       
    as       
    begin       
        declare @idx int       
        declare @slice varchar(MAX)       
          
        select @idx = 1       
            if len(@String)<1 or @String is null  return       
          
        while @idx!= 0       
        begin       
            set @idx = charindex(@Delimiter,@String)       
            if @idx!=0       
                set @slice = left(@String,@idx - 1)       
            else       
                set @slice = @String       
              
            if(len(@slice)>0)  
                insert into @temptable(Items) values(@slice)       
      
            set @String = right(@String,len(@String) - @idx)       
            if len(@String) = 0 break       
        end   
    return       
    end  ' 
END

--SPSEPARATOR-

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FN_GET_SPLITTED_LIST]') AND xtype in (N'FN', N'IF', N'TF'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[FN_GET_SPLITTED_LIST]
(
	@String							VARCHAR(MAX),
	@Delimiter						VARCHAR(MAX)
)
RETURNS
@SplittedTable TABLE 
(
	Value			VARCHAR(MAX)
)
AS
BEGIN
	DECLARE @CurrentIndex	INT;
	DECLARE @Slice			VARCHAR(MAX);
	
	SET @CurrentIndex = 1

	WHILE (@CurrentIndex <> 0 AND @String IS NOT NULL AND @String <> '''' AND DATALENGTH(@String) > 0)
	BEGIN	
		SELECT @CurrentIndex = CHARINDEX(@Delimiter, @String)
		IF (@CurrentIndex <> 0)
		BEGIN
			SELECT @Slice = LEFT(@String, @CurrentIndex - 1)
		END
		ELSE
		BEGIN
			SELECT @Slice = @String
		END
		
		IF (DATALENGTH(@Slice) > 0)
		BEGIN
			INSERT INTO @SplittedTable VALUES(@Slice)
		END

		SELECT @String = RIGHT(@String, DATALENGTH(@String) - (@CurrentIndex + DATALENGTH(@Delimiter) - 1))
	END

	RETURN
END
' 
END

--SPSEPARATOR-

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[fn_get_dimensions_IU_XX]') AND xtype in (N'FN', N'IF', N'TF'))
BEGIN
	execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[fn_get_dimensions_IU_XX]
(
	@Indicator_NId int,
	@Unit_NId int
)
RETURNS nvarchar(max)
AS
BEGIN
	
DECLARE @RESULT NVARCHAR(MAX)

SELECT @RESULT = COALESCE(@RESULT + '','', '''') + DIMENSION 
FROM
(
	SELECT DISTINCT   
		(
			SELECT TOP 1 SUBGROUP_TYPE_NAME FROM DBO.UT_SUBGROUP_TYPE_XX 
			WHERE SUBGROUP_TYPE_NID = SUBGROUP_TYPE
		) AS DIMENSION
	FROM DBO.UT_SUBGROUP_XX 
	WHERE SUBGROUP_NID IN 
		(
			SELECT SUBGROUP_NID FROM DBO.UT_SUBGROUP_VALS_SUBGROUP WHERE SUBGROUP_VAL_NID IN 
			(			
				SELECT IUS.SUBGROUP_VAL_NID
				FROM UT_INDICATOR_UNIT_SUBGROUP IUS
				WHERE IUS.INDICATOR_NID = @Indicator_NId and ius.Unit_NId = @Unit_NId
			)
		)
) DISTNICT_DIMENSIONS

RETURN @RESULT

END
' 
END
ELSE
BEGIN
execute dbo.sp_executesql @statement = N'
ALTER FUNCTION [dbo].[fn_get_dimensions_IU_XX]
(
	@Indicator_NId int,
	@Unit_NId int
)
RETURNS nvarchar(max)
AS
BEGIN
	
DECLARE @RESULT NVARCHAR(MAX)

SELECT @RESULT = COALESCE(@RESULT + '','', '''') + DIMENSION 
FROM
(
	SELECT DISTINCT   
		(
			SELECT TOP 1 SUBGROUP_TYPE_NAME FROM DBO.UT_SUBGROUP_TYPE_XX 
			WHERE SUBGROUP_TYPE_NID = SUBGROUP_TYPE
		) AS DIMENSION
	FROM DBO.UT_SUBGROUP_XX 
	WHERE SUBGROUP_NID IN 
		(
			SELECT SUBGROUP_NID FROM DBO.UT_SUBGROUP_VALS_SUBGROUP WHERE SUBGROUP_VAL_NID IN 
			(			
				SELECT IUS.SUBGROUP_VAL_NID
				FROM UT_INDICATOR_UNIT_SUBGROUP IUS
				WHERE IUS.INDICATOR_NID = @Indicator_NId and ius.Unit_NId = @Unit_NId
			)
		)
) DISTNICT_DIMENSIONS

RETURN @RESULT

END
' 
END


--SPSEPARATOR-

GO

--Get Generic Area NIDs
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FN_GET_AreaNIdGeneric_XX]') AND xtype in (N'FN', N'IF', N'TF'))
BEGIN
	execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[FN_GET_AreaNIdGeneric_XX]
(
	@AREA_ID NVARCHAR(MAX)
)

RETURNS INT
AS
BEGIN
	DECLARE @tempArea_ID NVARCHAR(MAX)
	DECLARE @RESULT INT
	SET @RESULT = 0
	
	IF(@AREA_ID) LIKE ''QS_%''
		BEGIN
			SET @tempArea_ID=PARSENAME(REPLACE(@AREA_ID,''_'', ''.''),2)
			SET @RESULT = (SELECT (CAST(AREA_NID AS NVARCHAR)) FROM UT_AREA_XX WHERE [Area_ID] = @tempArea_ID)
			RETURN @RESULT
		END
	ELSE IF (@AREA_ID) LIKE ''BL_%''
		BEGIN
			SET @tempArea_ID=PARSENAME(REPLACE(@AREA_ID,''_'', ''.''),1)
			SET @RESULT=CAST(@tempArea_ID AS INT)--@tempArea_ID
			RETURN @RESULT
		END
	ELSE 
			BEGIN
			SET @tempArea_ID=@AREA_ID
			SET @RESULT=CAST(@tempArea_ID AS INT)--@tempArea_ID
			RETURN @RESULT
			END
RETURN @RESULT
END
'
END
ELSE
BEGIN
execute dbo.sp_executesql @statement = N'
ALTER FUNCTION [dbo].[FN_GET_AreaNIdGeneric_XX]
(
	@AREA_ID NVARCHAR(MAX)
)

RETURNS INT
AS
BEGIN
	DECLARE @tempArea_ID NVARCHAR(MAX)
	DECLARE @RESULT INT
	SET @RESULT = 0
	
	IF(@AREA_ID) LIKE ''QS_%''
		BEGIN
			SET @tempArea_ID=PARSENAME(REPLACE(@AREA_ID,''_'', ''.''),2)
			SET @RESULT = (SELECT (CAST(AREA_NID AS NVARCHAR)) FROM UT_AREA_XX WHERE [Area_ID] = @tempArea_ID)
			RETURN @RESULT
		END
	ELSE IF (@AREA_ID) LIKE ''BL_%''
		BEGIN
			SET @tempArea_ID=PARSENAME(REPLACE(@AREA_ID,''_'', ''.''),1)
			SET @RESULT=CAST(@tempArea_ID AS INT)--@tempArea_ID
			RETURN @RESULT
		END
	ELSE 
			BEGIN
			SET @tempArea_ID=@AREA_ID
			SET @RESULT=CAST(@tempArea_ID AS INT)--@tempArea_ID
			RETURN @RESULT
			END
RETURN @RESULT
END
' 
END

--Get Generic Area NIDs

GO

--SPSEPARATOR-

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[fn_get_child_areas_XX]') AND xtype in (N'FN', N'IF', N'TF'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[fn_get_child_areas_XX]
(
	@ParentNid varchar(MAX)
)
RETURNS varchar(MAX)
AS
BEGIN
	DECLARE @Result varchar(MAX)
	
	SELECT @Result = COALESCE(@Result + '','', '''') + 
		CAST(Area_NId AS varchar(50))
		from dbo.ut_area_XX 
		where Area_Parent_NId in 
			(
			select items from dbo.split(@ParentNid,'','')
			)

	RETURN @Result

END


' 
END
ELSE
BEGIN
execute dbo.sp_executesql @statement = N'
ALTER FUNCTION [dbo].[fn_get_child_areas_XX]
(
	@ParentNid varchar(MAX)
)
RETURNS varchar(MAX)
AS
BEGIN
	DECLARE @Result varchar(MAX)
		
	SELECT @Result = COALESCE(@Result + '','', '''') + 
		CAST(Area_NId AS varchar(50))
		from dbo.ut_area_XX 
		where Area_Parent_NId in 
			(
			select items from dbo.split(@ParentNid,'','')
			)

	RETURN @Result

END


' 
END

--SPSEPARATOR-



IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[getAreaNid_XX]') AND xtype in (N'FN', N'IF', N'TF'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[getAreaNid_XX] 
(
	@AreaId varchar(50)
)
RETURNS int
AS
BEGIN
	Declare @Result int
	Set @Result = (select top 1 Area_NId from dbo.ut_area_XX 
					where Area_ID = @AreaId)

	return @Result

END
' 
END

--SPSEPARATOR-

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[getAreaLevel_XX]') AND xtype in (N'FN', N'IF', N'TF'))
BEGIN
execute dbo.sp_executesql @statement = N'
Create FUNCTION [dbo].[getAreaLevel_XX] 
(
	@AreaNid varchar(50)
)
RETURNS int
AS
BEGIN
	Declare @Result int
	Set @Result = (select top 1 Area_Level from dbo.ut_area_XX 
					where Area_NId = @AreaNid)

	return @Result

END

' 
END

--SPSEPARATOR-

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[fn_get_normalized_area_nids_XX]') AND xtype in (N'FN', N'IF', N'TF'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[fn_get_normalized_area_nids_XX]
(
	@AllAreaIds varchar(MAX)
)
RETURNS varchar(MAX)

AS

BEGIN
	Declare @Result varchar(MAX)
	Declare @tmpHolder varchar(MAX)
	Set @tmpHolder = ''''
	SELECT @tmpHolder = COALESCE(@tmpHolder + '','', '''') + 
		CAST(items AS varchar(MAX))
		from dbo.split(@AllAreaIds,'','')
		where dbo.isReallyNumeric(items) = 1

	SELECT @tmpHolder = COALESCE(@tmpHolder + '','', '''') + 
		CAST(dbo.fn_get_qs_area_nids_XX(items) AS varchar(MAX))
		from dbo.split(@AllAreaIds,'','')
		where dbo.isReallyNumeric(items) = 0

	DECLARE @MyTableVar table(
    resultField int NOT NULL)

	Insert into @MyTableVar
	Select distinct items from dbo.split(@tmpHolder,'','')

	SELECT @Result = COALESCE(@Result + '','', '''') + 
		CAST(resultField AS varchar(MAX))
		from @MyTableVar ORDER BY resultField

	RETURN @Result

END





' 
END

--SPSEPARATOR-

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FN_CALCULATE_SCORE]') AND xtype in (N'FN', N'IF', N'TF'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[FN_CALCULATE_SCORE]
(
	@Searchee					VARCHAR(MAX),
	@Searcher					VARCHAR(MAX)
)
RETURNS DECIMAL
AS
BEGIN
	DECLARE @Score					FLOAT
	DECLARE @NumTotalCharacters		FLOAT
	DECLARE @Count					INT
	DECLARE @Max					INT
	DECLARE @TempSearch				VARCHAR(MAX)
	DECLARE @QuoteSearch			VARCHAR(MAX)

	DECLARE @SearcheeTable TABLE 
	(
		Id			INT IDENTITY(1,1) ,
		Value		VARCHAR(MAX)
	)

	DECLARE @SearcherTable TABLE 
	(
		Id			INT IDENTITY(1,1) ,
		Value		VARCHAR(MAX)
	)

	SET @Score = 0.0
	SELECT @NumTotalCharacters = LEN(REPLACE(@Searchee, '' '', ''''))

	INSERT INTO @SearcheeTable SELECT Value FROM dbo.FN_GET_SPLITTED_LIST(@Searchee, '' '') WHERE DataLength(Value) >= 3
	INSERT INTO @SearcherTable SELECT Value FROM dbo.FN_GET_SPLITTED_LIST(@Searcher, '' '') WHERE DataLength(Value) >= 3

	SELECT @TempSearch = '''', @Count = 0, @Max = COUNT(*) FROM @SearcherTable
	WHILE (@Count < @Max)
	BEGIN
		SET @Count = @Count + 1
		SELECT @TempSearch = Value FROM @SearcherTable WHERE Id = @Count

		IF (CHARINDEX(''[__]'', @TempSearch) = 0)
		BEGIN
			IF EXISTS(SELECT 1 FROM @SearcheeTable WHERE Value = @TempSearch)
			BEGIN
				SET @Score = @Score + (LEN(@TempSearch)/@NumTotalCharacters)
			END
		END
		ELSE
		BEGIN
			SELECT @QuoteSearch = REPLACE(@TempSearch, ''[__]'', '' '')
			
			IF (CHARINDEX(@QuoteSearch, @Searchee) <> 0)
			BEGIN
				SET @Score = @Score + (LEN(REPLACE(@QuoteSearch, '' '', ''''))/@NumTotalCharacters)
			END
		END
	END

	SET @Score = @Score * 100

	RETURN @Score
END


' 
END

--SPSEPARATOR-

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[fn_get_qs_area_nids_XX]') AND xtype in (N'FN', N'IF', N'TF'))
BEGIN
execute dbo.sp_executesql @statement = N'


CREATE FUNCTION [dbo].[fn_get_qs_area_nids_XX]
(
	@QS_AreaID_Level varchar(50)
)
RETURNS varchar(MAX)
AS
BEGIN

DECLARE @LevelNeededAreaNIds varchar(MAX)
DECLARE @strQS_AreaID_Level nvarchar(50)
	
IF (SUBSTRING(@QS_AreaID_Level, 1, 2) = ''QS'')
	BEGIN	

		Declare @ParentAreaID varchar(50)
		Declare @LevelNeeded int
		Declare @IndexOfLevel int
		
		Set @strQS_AreaID_Level = @QS_AreaID_Level
		Set @strQS_AreaID_Level = Replace(@strQS_AreaID_Level, ''QS_'', '''')
		Set @IndexOfLevel =  CHARINDEX(''_'', @strQS_AreaID_Level)
		Set @ParentAreaID = SubString(@strQS_AreaID_Level, 1, @IndexOfLevel - 1)
		Set @LevelNeeded = Cast(SubString(@strQS_AreaID_Level, @IndexOfLevel + 2, len(@strQS_AreaID_Level)) as int)

		Declare @ParenNid varchar(MAX)
		Declare @ParentLevel  int
		Declare @LevelCounter int
		
		Set @ParenNid = dbo.getAreaNid_XX(@ParentAreaID)
		Set @ParentLevel = dbo.getAreaLevel_XX(@ParenNid)
		Set @LevelCounter = @ParentLevel

		WHILE(1=1)
		BEGIN
			if (@LevelCounter = @LevelNeeded)
			BEGIN
				BREAK
			END
			-- Get the AreaNids of the next level of the parent
			set @LevelNeededAreaNIds = dbo.fn_get_child_areas_XX(@ParenNid)
			set @ParenNid = @LevelNeededAreaNIds
			Set @LevelCounter =@LevelCounter+1
		END

	END -- IF (SUBSTRING(@QS_AreaID_Level, 1, 2) == ''QS'')	


ELSE -- For BLOCK Areas, get all area nids for that block

	BEGIN

		
		Set @strQS_AreaID_Level = @QS_AreaID_Level
		Set @strQS_AreaID_Level = Replace(@strQS_AreaID_Level, ''BL_'', '''')
		
		SET @LevelNeededAreaNIds = (	
										SELECT TOP 1 Area_Block FROM dbo.ut_area_XX
										WHERE Area_NId = CAST(@strQS_AreaID_Level AS INT)
									)


	END -- ELSE of IF (SUBSTRING(@QS_AreaID_Level, 1, 2) == ''QS'')	 -- For BLOCK Areas, get all area nids for that block

--
	
RETURN @LevelNeededAreaNIds

END


' 
END
ELSE
BEGIN
execute dbo.sp_executesql @statement = N'
ALTER FUNCTION [dbo].[fn_get_qs_area_nids_XX]
(
	@QS_AreaID_Level varchar(50)
)
RETURNS varchar(MAX)
AS
BEGIN

DECLARE @LevelNeededAreaNIds varchar(MAX)
DECLARE @strQS_AreaID_Level nvarchar(50)
	
IF (SUBSTRING(@QS_AreaID_Level, 1, 2) = ''QS'')
	BEGIN	

		Declare @ParentAreaID varchar(50)
		Declare @LevelNeeded int
		Declare @IndexOfLevel int
		
		Set @strQS_AreaID_Level = @QS_AreaID_Level
		Set @strQS_AreaID_Level = Replace(@strQS_AreaID_Level, ''QS_'', '''')
		Set @IndexOfLevel =  CHARINDEX(''_'', @strQS_AreaID_Level)
		Set @ParentAreaID = SubString(@strQS_AreaID_Level, 1, @IndexOfLevel - 1)
		Set @LevelNeeded = Cast(SubString(@strQS_AreaID_Level, @IndexOfLevel + 2, len(@strQS_AreaID_Level)) as int)

		Declare @ParenNid varchar(MAX)
		Declare @ParentLevel  int
		Declare @LevelCounter int
		
		Set @ParenNid = dbo.getAreaNid_XX(@ParentAreaID)
		Set @ParentLevel = dbo.getAreaLevel_XX(@ParenNid)
		Set @LevelCounter = @ParentLevel

		WHILE(1=1)
		BEGIN
			if (@LevelCounter = @LevelNeeded)
			BEGIN
				BREAK
			END
			-- Get the AreaNids of the next level of the parent
			set @LevelNeededAreaNIds = dbo.fn_get_child_areas_XX(@ParenNid)
			set @ParenNid = @LevelNeededAreaNIds
			Set @LevelCounter =@LevelCounter+1
		END

	END -- IF (SUBSTRING(@QS_AreaID_Level, 1, 2) == ''QS'')	


ELSE -- For BLOCK Areas, get all area nids for that block

	BEGIN

		
		Set @strQS_AreaID_Level = @QS_AreaID_Level
		Set @strQS_AreaID_Level = Replace(@strQS_AreaID_Level, ''BL_'', '''')
		
		SET @LevelNeededAreaNIds = (	
										SELECT TOP 1 Area_Block FROM dbo.ut_area_XX
										WHERE Area_NId = CAST(@strQS_AreaID_Level AS INT)
									)


	END -- ELSE of IF (SUBSTRING(@QS_AreaID_Level, 1, 2) == ''QS'')	 -- For BLOCK Areas, get all area nids for that block

--
	
RETURN @LevelNeededAreaNIds

END

' 
END


--SPSEPARATOR--


GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_get_dataview_datanid_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_get_dataview_datanid_XX]
	@strAllData_Nids  nvarchar(MAX),
	@strSourceNids nvarchar(500) = Null,
	@TimePeriodNids nvarchar(1000) = Null,
	@isMRD int = 0,
	@dvStart nvarchar(10) = Null,
	@dvEnd nvarchar(10) = Null
AS
BEGIN

Declare @strSQL nvarchar(MAX)
Declare @strAllSearchNIds nvarchar(MAX)

SET @STRALLSEARCHNIDS = @STRALLDATA_NIDS
SET @STRALLDATA_NIDS = ''''

SELECT @STRALLDATA_NIDS =	COALESCE(@STRALLDATA_NIDS + '','', '''') + 
							CAST(DVNIDS AS VARCHAR(MAX))
							FROM DI_SEARCH_RESULTS 
							WHERE NID IN
								(
									SELECT DISTINCT ITEMS FROM DBO.SPLIT(@STRALLSEARCHNIDS,'','') 
								)

SELECT	DT.IUNID, 
		ISNULL(DBO.CustomCast(DT.Data_Value), DT.Textual_Data_Value) as ''Data_Value'',
		DT.IUSNID, DT.AREA_NID,
		DT.SUBGROUP_VAL_NID, DT.SOURCE_NID  AS IC_NID, 
		DT.FOOTNOTE_NID, 
		(SELECT TOP 1 TIMEPERIOD FROM DBO.UT_TIMEPERIOD WHERE TIMEPERIOD_NID = DT.TIMEPERIOD_NID)  AS TIMEPERIOD,
		(SELECT TOP 1 AREA_ID FROM DBO.UT_AREA_XX WHERE AREA_NID = DT.AREA_NID)  AS AREA_ID,
		isMRD

FROM UT_DATA AS DT
WHERE DT.DATA_NID in
(
	SELECT DISTINCT ITEMS FROM DBO.SPLIT(@STRALLDATA_NIDS,'','') 
)

END


' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[sp_get_dataview_datanid_XX]
	@strAllData_Nids  nvarchar(MAX),
	@strSourceNids nvarchar(500) = Null,
	@TimePeriodNids nvarchar(1000) = Null,
	@isMRD int = 0,
	@dvStart nvarchar(10) = Null,
	@dvEnd nvarchar(10) = Null
AS
BEGIN

Declare @strSQL nvarchar(MAX)
Declare @strAllSearchNIds nvarchar(MAX)

SET @STRALLSEARCHNIDS = @STRALLDATA_NIDS
SET @STRALLDATA_NIDS = ''''

SELECT @STRALLDATA_NIDS =	COALESCE(@STRALLDATA_NIDS + '','', '''') + 
							CAST(DVNIDS AS VARCHAR(MAX))
							FROM DI_SEARCH_RESULTS 
							WHERE NID IN
								(
									SELECT DISTINCT ITEMS FROM DBO.SPLIT(@STRALLSEARCHNIDS,'','') 
								)

SELECT	DT.IUNID, 
		ISNULL(DBO.CustomCast(DT.Data_Value), DT.Textual_Data_Value) as ''Data_Value'',
		DT.IUSNID, DT.AREA_NID,
		DT.SUBGROUP_VAL_NID, DT.SOURCE_NID  AS IC_NID, 
		DT.FOOTNOTE_NID, 
		(SELECT TOP 1 TIMEPERIOD FROM DBO.UT_TIMEPERIOD WHERE TIMEPERIOD_NID = DT.TIMEPERIOD_NID)  AS TIMEPERIOD,
		(SELECT TOP 1 AREA_ID FROM DBO.UT_AREA_XX WHERE AREA_NID = DT.AREA_NID)  AS AREA_ID,
		isMRD

FROM UT_DATA AS DT
WHERE DT.DATA_NID in
(
	SELECT DISTINCT ITEMS FROM DBO.SPLIT(@STRALLDATA_NIDS,'','') 
)

END


' 
END
GO

--SPSEPARATOR--



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_get_dataview_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_get_dataview_XX]
	@STRALLAREAQSIDS_NIDS  NVARCHAR(MAX),
	@STRIUS_NIDS NVARCHAR(1500),
	@STRSOURCENIDS NVARCHAR(500) = NULL,
	@TIMEPERIODNIDS NVARCHAR(1000) = NULL,
	@ISMRD INT = 0,
	@DVSTART NVARCHAR(10) = NULL,
	@DVEND NVARCHAR(10) = NULL
AS
BEGIN

DECLARE @STRAREANIDS VARCHAR(MAX)
SET @STRAREANIDS = DBO.FN_GET_NORMALIZED_AREA_NIDS_XX(@STRALLAREAQSIDS_NIDS)

SELECT	DT.IUNID, 
		ISNULL(DBO.CustomCast(DT.Data_Value), DT.Textual_Data_Value) as ''Data_Value'',
		DT.IUSNID, DT.AREA_NID,
		DT.SUBGROUP_VAL_NID, DT.SOURCE_NID  AS IC_NID, 
		DT.FOOTNOTE_NID, 
		(SELECT TOP 1 TIMEPERIOD FROM DBO.UT_TIMEPERIOD WHERE TIMEPERIOD_NID = DT.TIMEPERIOD_NID)  AS TIMEPERIOD,
		(SELECT TOP 1 AREA_ID FROM DBO.UT_AREA_XX WHERE AREA_NID = DT.AREA_NID)  AS AREA_ID,
		isMRD

FROM UT_DATA AS DT

WHERE DT.AREA_NID IN
(
	SELECT ITEMS FROM DBO.SPLIT(@STRAREANIDS, '','')
)
AND
IUSNID IN 
(
	SELECT ITEMS FROM DBO.SPLIT(@STRIUS_NIDS, '','')
)

END









' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[sp_get_dataview_XX]
	@STRALLAREAQSIDS_NIDS  NVARCHAR(MAX),
	@STRIUS_NIDS NVARCHAR(1500),
	@STRSOURCENIDS NVARCHAR(500) = NULL,
	@TIMEPERIODNIDS NVARCHAR(1000) = NULL,
	@ISMRD INT = 0,
	@DVSTART NVARCHAR(10) = NULL,
	@DVEND NVARCHAR(10) = NULL
AS
BEGIN

DECLARE @STRAREANIDS VARCHAR(MAX)
SET @STRAREANIDS = DBO.FN_GET_NORMALIZED_AREA_NIDS_XX(@STRALLAREAQSIDS_NIDS)

SELECT	DT.IUNID, 
		ISNULL(DBO.CustomCast(DT.Data_Value), DT.Textual_Data_Value) as ''Data_Value'',
		DT.IUSNID, DT.AREA_NID,
		DT.SUBGROUP_VAL_NID, DT.SOURCE_NID  AS IC_NID, 
		DT.FOOTNOTE_NID, 
		(SELECT TOP 1 TIMEPERIOD FROM DBO.UT_TIMEPERIOD WHERE TIMEPERIOD_NID = DT.TIMEPERIOD_NID)  AS TIMEPERIOD,
		(SELECT TOP 1 AREA_ID FROM DBO.UT_AREA_XX WHERE AREA_NID = DT.AREA_NID)  AS AREA_ID,
		isMRD

FROM UT_DATA AS DT

WHERE DT.AREA_NID IN
(
	SELECT ITEMS FROM DBO.SPLIT(@STRAREANIDS, '','')
)
AND
IUSNID IN 
(
	SELECT ITEMS FROM DBO.SPLIT(@STRIUS_NIDS, '','')
)

END
' 
END

--SPSEPARATOR--

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_get_ind_wheredataexists_XX]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_get_ind_wheredataexists_XX]
	@strAllAreaQSIds_Nids  nvarchar(MAX),
	@strIU_Nids nvarchar(MAX)
AS
BEGIN	
	SET NOCOUNT ON;

	Declare @strAreaNids varchar(MAX)
	Set @strAreaNids = dbo.fn_get_normalized_area_nids_XX(@strAllAreaQSIds_Nids)
		
SELECT Distinct IUNid FROM dbo.UT_Data --Replace(,''_'',''~'')
where IUNid in 
(
select distinct items from dbo.split(@strIU_Nids,'','') 
)
and 
Area_NId in 
(
select distinct items from dbo.split(@strAreaNids,'','') 
) 


END




' 
END


IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[SP_GET_AREAS_FROM_NIDS_XX]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[SP_GET_AREAS_FROM_NIDS_XX]
	@AreaNIds		VARCHAR(MAX)
AS
BEGIN
	SELECT * FROM UT_Area_XX Area
	JOIN dbo.FN_GET_SPLITTED_LIST(@AreaNIds, '','') List ON Area.Area_NId = List.Value
END
' 
END


IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[SP_GET_SGS_FROM_NIDS_XX]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[SP_GET_SGS_FROM_NIDS_XX]
	@SGNIds			VARCHAR(MAX)
AS
BEGIN
	SELECT * FROM UT_Subgroup_Vals_XX SGV
	JOIN dbo.FN_GET_SPLITTED_LIST(@SGNIds, '','') List ON SGV.Subgroup_Val_NId = List.Value
END
' 
END


IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[SP_GET_SOURCES_FROM_NIDS_XX]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[SP_GET_SOURCES_FROM_NIDS_XX]
	@SourceNIds		VARCHAR(MAX)
AS
BEGIN
	SELECT * FROM UT_Indicator_Classifications_XX IC 
	JOIN dbo.FN_GET_SPLITTED_LIST(@SourceNIds, '','') List ON IC.IC_NId = List.Value
	WHERE IC.IC_Type = ''SR''
END
' 
END


IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[SP_GET_TPS_FROM_NIDS]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[SP_GET_TPS_FROM_NIDS]
	@TPNIds		VARCHAR(MAX)
AS
BEGIN
	SELECT * FROM UT_TimePeriod TP JOIN dbo.FN_GET_SPLITTED_LIST(@TPNIds, '','') List ON TP.TimePeriod_NId = List.Value
	ORDER BY TP.TimePeriod
END


' 
END


--SPSEPARATOR--


IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[SP_GET_AS_RESULTS_IUA_XX]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[SP_GET_AS_RESULTS_IUA_XX]
	@SearchIndicatorICs		VARCHAR(MAX),
	@SearchAreas			VARCHAR(MAX)
AS
BEGIN
	DECLARE @SQLSearch					NVARCHAR(MAX);

	DECLARE @Max						INT;
	DECLARE @Count						INT;
	DECLARE @TempIndicatorNId			INT;
	DECLARE @TempUnitNId				INT;
	DECLARE @TempAreaNId				INT;
	DECLARE @TempDefaultSGNId			INT;
	DECLARE @TempSGCount				INT;
	DECLARE @TempSourceCount			INT;
	DECLARE @TempTPCount				INT;
	DECLARE @TempDVCount				INT;
	DECLARE @TempSGNIds					VARCHAR(MAX);
	DECLARE @TempSourceNIds				VARCHAR(MAX);
	DECLARE @TempTPNIds					VARCHAR(MAX);
	DECLARE @TempDVNIds					VARCHAR(MAX);
	DECLARE @TempDVSeries				VARCHAR(MAX);
	DECLARE @TempTPSeries				VARCHAR(MAX);
	DECLARE @SearchLanguage				VARCHAR(MAX);

	SET @SearchLanguage = ''_XX'';
	SELECT @SearchLanguage = SUBSTRING(@SearchLanguage, 2, LEN(@SearchLanguage) - 1)

	CREATE TABLE #ResultSet
	(
		Id							INT IDENTITY(1,1),
		IndicatorNId				INT DEFAULT 0,
		UnitNId						INT DEFAULT 0,
		AreaNId						INT DEFAULT 0,
		Indicator					VARCHAR(MAX) DEFAULT '''' NOT NULL,
		ICName						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		Unit						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		Area						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		DefaultSGNId				INT DEFAULT 0,
		DefaultSG					VARCHAR(MAX) DEFAULT '''' NOT NULL,
		MRDTP						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		MRD							VARCHAR(MAX) DEFAULT '''' NOT NULL,
		AreaCount					INT DEFAULT 0,
		SGCount						INT DEFAULT 0,
		SourceCount					INT DEFAULT 0,
		TPCount						INT DEFAULT 0,
		DVCount						INT DEFAULT 0,
		AreaNIds					VARCHAR(MAX) DEFAULT '''' NOT NULL,
		SGNIds						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		SourceNIds					VARCHAR(MAX) DEFAULT '''' NOT NULL,
		TPNIds						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		DVNIds						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		DVSeries					VARCHAR(MAX) DEFAULT '''' NOT NULL,
		Score						FLOAT DEFAULT 0.0
	)
	
	CREATE TABLE #SearchAreas 
	(
		Id							INT IDENTITY(1,1) ,
		AreaNId						INT DEFAULT 0,
		Area						VARCHAR(MAX) DEFAULT '''' NOT NULL
	)

	CREATE TABLE #SearchIndicators 
	(
		Id							INT IDENTITY(1,1) ,
		IndicatorNId				INT DEFAULT 0,
		Indicator					VARCHAR(MAX) DEFAULT '''' NOT NULL,
		ICName						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		Score						FLOAT DEFAULT 0.0
	)

	CREATE TABLE #SearchICs 
	(
		Id							INT IDENTITY(1,1) ,
		ICNId						INT DEFAULT 0,
		ICName						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		Score						FLOAT DEFAULT 0.0
	)

	CREATE TABLE #ICIndicators
	(
		Id							INT IDENTITY(1,1) ,
		IndicatorNId				INT DEFAULT 0,
		Indicator					VARCHAR(MAX) DEFAULT '''' NOT NULL,
		ICName						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		Score						FLOAT DEFAULT 0.0
	)
	
--***************************************************************************************************************************************************
-- Filling Area Table
--***************************************************************************************************************************************************
	INSERT INTO #SearchAreas (AreaNId, Area)   
	SELECT Area.Area_NId, Area.Area_Name FROM UT_Area_XX Area 
	JOIN dbo.FN_GET_SPLITTED_LIST(@SearchAreas, ''||'') SearchAreas 
	ON Area.Area_Name LIKE ''%'' + SearchAreas.Value + ''%''
	
--***************************************************************************************************************************************************
-- Filling Indicator Table
--***************************************************************************************************************************************************
	INSERT INTO #SearchIndicators (IndicatorNId, Indicator, Score)
	SELECT Indicator.Indicator_NId, Indicator.Indicator_Name, MAX(dbo.FN_CALCULATE_SCORE(Indicator.Indicator_Name, SearchIndicators.Value)) AS Score
	FROM UT_Indicator_XX Indicator JOIN dbo.FN_GET_SPLITTED_LIST(@SearchIndicatorICs, ''||'') SearchIndicators 
	ON dbo.FN_CALCULATE_SCORE(Indicator.Indicator_Name, SearchIndicators.Value) > 0
	GROUP BY Indicator.Indicator_NId, Indicator.Indicator_Name

	IF EXISTS(SELECT 1 FROM #SearchIndicators WHERE Score = 100)
	BEGIN
		DELETE FROM #SearchIndicators WHERE Score <> 100
	END
	ELSE
	BEGIN
		--***************************************************************************************************************************************************
		-- Filling IC Table
		--***************************************************************************************************************************************************
		INSERT INTO #SearchICs (ICNId, ICName, Score)  
		SELECT IC.IC_NId, IC.IC_Name, MAX(dbo.FN_CALCULATE_SCORE(IC.IC_Name, SearchICs.Value)) AS Score
		FROM UT_Indicator_Classifications_XX IC JOIN dbo.FN_GET_SPLITTED_LIST(@SearchIndicatorICs, ''||'') SearchICs 
		ON dbo.FN_CALCULATE_SCORE(IC.IC_Name, SearchICs.Value) > 0
		GROUP BY IC.IC_NId, IC.IC_Name

		INSERT INTO #ICIndicators (IndicatorNId, Indicator, ICName, Score)
		SELECT DISTINCT Indicator.Indicator_NId, Indicator.Indicator_Name, ICs.ICName, ICs.Score FROM #SearchICs ICs
		JOIN UT_ic_IUS IC_IUS ON ICs.ICNId = IC_IUS.IC_NId
		JOIN UT_Indicator_Unit_Subgroup IUS ON IC_IUS.IUSNId = IUS.IUSNId
		JOIN UT_Indicator_XX Indicator ON IUS.Indicator_NId = Indicator.Indicator_NId

		INSERT INTO #SearchIndicators (IndicatorNId, Indicator, ICName, Score)
		SELECT IndicatorNId, Indicator, ICName, Score FROM #ICIndicators
		WHERE IndicatorNId NOT IN (SELECT IndicatorNId FROM #SearchIndicators)
	END

	IF (@SearchIndicatorICs IS NULL OR @SearchIndicatorICs = '''')
	BEGIN
		INSERT INTO #SearchIndicators (IndicatorNId, Indicator)
		SELECT Indicator.Indicator_NId, Indicator.Indicator_Name FROM UT_Indicator_XX Indicator 
	END

	INSERT INTO #ResultSet (IndicatorNId, UnitNId, AreaNId, Indicator, ICName, Unit, Area, Score)
	SELECT DISTINCT Data.Indicator_NId, Data.Unit_NId, Data.Area_NId, Data.Indicator, Data.ICName, Unit.Unit_Name, Data.Area, Data.Score FROM 
	(
		SELECT DISTINCT Indicator_NId, Unit_NId, Area_NId, #SearchIndicators.Indicator, #SearchIndicators.ICName, #SearchAreas.Area, #SearchIndicators.Score FROM UT_Data 
		JOIN #SearchIndicators ON UT_Data.Indicator_NId = #SearchIndicators.IndicatorNId 
		JOIN #SearchAreas ON UT_Data.Area_NId = #SearchAreas.AreaNId 
	) Data 
	JOIN UT_Unit_XX Unit ON Data.Unit_NId = Unit.Unit_NId 
	ORDER BY Data.Score DESC, Data.Indicator_NId, Data.Unit_NId, Data.Area_NId

	UPDATE #ResultSet SET DefaultSGNId = IUS.Subgroup_Val_NId , DefaultSG = SGV.Subgroup_Val
	FROM UT_Indicator_Unit_Subgroup IUS, UT_Subgroup_Vals_XX SGV 
	WHERE IndicatorNId = IUS.Indicator_NId AND UnitNId = IUS.Unit_NId 
	AND IUS.IsDefaultSubgroup = 1 AND IUS.Subgroup_Val_NId = SGV.Subgroup_Val_NId

	UPDATE #ResultSet 
	SET MRDTP = TP.TimePeriod, MRD = Data.Data_Value
	FROM UT_Data Data, UT_TimePeriod TP 
	WHERE IndicatorNId = Data.Indicator_NId AND UnitNId = Data.Unit_NId AND DefaultSGNId = Data.Subgroup_Val_NId 
	AND AreaNId = Data.Area_NId AND Data.isMRD = 1 AND Data.TimePeriod_NId = TP.TimePeriod_NId
		
	SELECT @Count = 0, @Max = COUNT(*) FROM #ResultSet
	WHILE(@Count < @Max)
	BEGIN
		SET @Count = @Count + 1;
		
		SELECT @TempIndicatorNId = IndicatorNId, @TempUnitNId = UnitNId, @TempAreaNId = AreaNId, @TempDefaultSGNId = DefaultSGNId,
			   @TempSGCount = 0, @TempSourceCount = 0, @TempTPCount = 0, @TempDVCount = 0,
			   @TempSGNIds = '','', @TempSourceNIds = '','', @TempTPNIds = '','', @TempDVNIds = '','', @TempDVSeries = '','', @TempTPSeries = '',''
		FROM #ResultSet WHERE Id = @Count

		SELECT 
			@TempSGNIds = 
			CASE 
				WHEN (CHARINDEX( '','' + CAST(Subgroup_Val_NId AS VARCHAR(MAX))+ '','', @TempSGNIds) = 0) 
				THEN 
					@TempSGNIds + CAST(Subgroup_Val_NId AS VARCHAR(MAX)) + '',''
				ELSE
					@TempSGNIds
			END ,
			@TempSourceNIds = 
			CASE 
				WHEN (CHARINDEX( '','' + CAST(Source_NId AS VARCHAR(MAX))+ '','', @TempSourceNIds) = 0) 
				THEN 
					@TempSourceNIds + CAST(Source_NId AS VARCHAR(MAX)) + '',''
				ELSE
					@TempSourceNIds
			END ,
			@TempTPNIds = 
			CASE 
				WHEN (CHARINDEX( '','' + CAST(TimePeriod_NId AS VARCHAR(MAX))+ '','', @TempTPNIds) = 0) 
				THEN 
					@TempTPNIds + CAST(TimePeriod_NId AS VARCHAR(MAX)) + '',''
				ELSE
					@TempTPNIds
			END ,
			@TempDVNIds = 
			CASE 
				WHEN (CHARINDEX( '','' + CAST(Data_NId AS VARCHAR(MAX))+ '','', @TempDVNIds) = 0) 
				THEN 
					@TempDVNIds + CAST(Data_NId AS VARCHAR(MAX)) + '',''
				ELSE
					@TempDVNIds
			END 
		FROM UT_DATA WHERE Indicator_NId = @TempIndicatorNId AND Unit_NId = @TempUnitNId AND Area_NId = @TempAreaNId
		
		SELECT @TempDVSeries = @TempDVSeries + CAST(Tbl.Data_Value AS VARCHAR(MAX)) + '','', @TempTPSeries = @TempTPSeries + Tbl.TimePeriod + '',''
		FROM 
		(
		SELECT TOP 2147483647 Data.Data_Value, TP.TimePeriod_NId, TP.TimePeriod FROM UT_DATA Data JOIN UT_TimePeriod TP ON Data.TimePeriod_NId = TP.TimePeriod_NId 
		WHERE Indicator_NId = @TempIndicatorNId AND Unit_NId = @TempUnitNId AND Area_NId = @TempAreaNId AND Subgroup_Val_NId = @TempDefaultSGNId 
		ORDER BY TimePeriod
		) Tbl

		IF (LEN(@TempSGNIds) > 1)
		BEGIN
			SELECT @TempSGNIds = SUBSTRING(@TempSGNIds, 2, LEN(@TempSGNIds) - 2)
			SET @TempSGCount = LEN(@TempSGNIds) - LEN(REPLACE(@TempSGNIds, '','', '''')) + 1
		END
		ELSE IF (LEN(@TempSGNIds) = 1)
		BEGIN
			SELECT @TempSGNIds = ''''
			SET @TempSGCount = 0
		END	

		IF (LEN(@TempSourceNIds) > 1)
		BEGIN
			SELECT @TempSourceNIds = SUBSTRING(@TempSourceNIds, 2, LEN(@TempSourceNIds) - 2)
			SET @TempSourceCount = LEN(@TempSourceNIds) - LEN(REPLACE(@TempSourceNIds, '','', '''')) + 1
		END
		ELSE IF (LEN(@TempSourceNIds) = 1)
		BEGIN
			SELECT @TempSourceNIds = ''''
			SET @TempSourceCount = 0
		END	

		IF (LEN(@TempTPNIds) > 1)
		BEGIN
			SELECT @TempTPNIds = SUBSTRING(@TempTPNIds, 2, LEN(@TempTPNIds) - 2)
			SET @TempTPCount = LEN(@TempTPNIds) - LEN(REPLACE(@TempTPNIds, '','', '''')) + 1
		END
		ELSE IF (LEN(@TempTPNIds) = 1)
		BEGIN
			SELECT @TempTPNIds = ''''
			SET @TempTPCount = 0
		END	

		IF (LEN(@TempDVNIds) > 1)
		BEGIN
			SELECT @TempDVNIds = SUBSTRING(@TempDVNIds, 2, LEN(@TempDVNIds) - 2)
			SET @TempDVCount = LEN(@TempDVNIds) - LEN(REPLACE(@TempDVNIds, '','', '''')) + 1
		END
		ELSE IF (LEN(@TempDVNIds) = 1)
		BEGIN
			SELECT @TempDVNIds = ''''
			SET @TempDVCount = 0
		END	

		IF (LEN(@TempDVSeries) > 1)
		BEGIN
			SELECT @TempDVSeries = SUBSTRING(@TempDVSeries, 2, LEN(@TempDVSeries) - 2)
		END
		ELSE IF (LEN(@TempDVSeries) = 1)
		BEGIN
			SELECT @TempDVSeries = ''''
		END

		IF (LEN(@TempTPSeries) > 1)
		BEGIN
			SELECT @TempTPSeries = SUBSTRING(@TempTPSeries, 2, LEN(@TempTPSeries) - 2)
		END
		ELSE IF (LEN(@TempTPSeries) = 1)
		BEGIN
			SELECT @TempTPSeries = ''''
		END	

		UPDATE #ResultSet
		SET	SGCount = @TempSGCount, SourceCount = @TempSourceCount, TPCount = @TempTPCount, DVCount = @TempDVCount,
			SGNIds = @TempSGNIds, SourceNIds = @TempSourceNIds, TPNIds = @TempTPNIds, DVNIds = @TempDVNIds, DVSeries = @TempTPSeries + '':'' + @TempDVSeries
		WHERE IndicatorNId = @TempIndicatorNId AND UnitNId = @TempUnitNId AND AreaNId = @TempAreaNId
	END

	SELECT * FROM #ResultSet AS ResultSet ORDER BY Score DESC, IndicatorNId, UnitNId, AreaNId

	DROP TABLE #ResultSet
	DROP TABLE #SearchAreas
	DROP TABLE #SearchIndicators
	DROP TABLE #SearchICs
	DROP TABLE #ICIndicators
END




' 
END


--SPSEPARATOR--

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GET_SEARCH_RESULTS]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GET_SEARCH_RESULTS]
	@SearchIndicators		VARCHAR(MAX),
	@SearchAreas			VARCHAR(MAX),
	@Language				VARCHAR(MAX)
AS
BEGIN
	DECLARE @SQLSearch					NVARCHAR(MAX);
	DECLARE @Max						INT
	DECLARE @Count						INT
	DECLARE @TempIndicatorNId			INT
	DECLARE @TempUnitNId				INT
	DECLARE @TempAreaNId				INT
	DECLARE @TempScore					FLOAT
	DECLARE @TempIndicatorName			VARCHAR(MAX)
	DECLARE @TempUnit					VARCHAR(MAX)
	DECLARE @TempArea					VARCHAR(MAX)
	DECLARE @TempSGCount				VARCHAR(MAX)
	DECLARE @TempTPCount				VARCHAR(MAX)
	DECLARE @TempSourceCount			VARCHAR(MAX)
	DECLARE @TempDVCount				VARCHAR(MAX)
	DECLARE @TempSGNIds					VARCHAR(MAX)
	DECLARE @TempTPNIds					VARCHAR(MAX)
	DECLARE @TempSourceNIds				VARCHAR(MAX)
	DECLARE @TempDVNIds					VARCHAR(MAX)

	
	CREATE TABLE #ResultSet
	(
		Id							INT IDENTITY(1,1) ,
		IndicatorName				VARCHAR(MAX) DEFAULT '''' NOT NULL,
		Unit						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		Area						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		SGCount						INT DEFAULT -1,
		TPCount						INT DEFAULT -1,
		SourceCount					INT DEFAULT -1,
		DVCount						INT DEFAULT -1,
		SGNIds						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		TPNIds						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		SourceNIds					VARCHAR(MAX) DEFAULT '''' NOT NULL,
		DVNIds						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		Score						DECIMAL
	)
	
	CREATE TABLE #BasicResultSet
	(
		Id				INT IDENTITY(1,1) ,
		IndicatorNId	INT,
		UnitNId			INT,
		AreaNId			INT,
		IndicatorName	VARCHAR(MAX),
		AreaName		VARCHAR(MAX),
		Score			FLOAT
	)
	
	CREATE TABLE #IndicatorsTable  		 
	(
		Id				INT IDENTITY(1,1) ,
		IndicatorNId	INT,
		IndicatorName	VARCHAR(MAX),
		Score			FLOAT
	)
	
	CREATE TABLE #AreasTable
	(
		Id				INT IDENTITY(1,1) ,
		AreaNId			INT DEFAULT -1,
		AreaName		VARCHAR(MAX),
	)

	
--***************************************************************************************************************************************************
-- Filling Area Table
--***************************************************************************************************************************************************

	SET @SQLSearch = ''INSERT INTO #AreasTable (AreaNId, AreaName)'' +  
					 '' SELECT Area.Area_NId, Area.Area_Name FROM UT_Area_'' + @Language + '' Area JOIN dbo.GET_SPLITTED_LIST('''''' + @SearchAreas + 
					 '''''', '''','''') SearchAreas ON Area.Area_Name LIKE ''''%'''' + SearchAreas.Value + ''''%''''''
	PRINT @SQLSearch
	EXEC(@SQLSearch)

--***************************************************************************************************************************************************
-- Filling Indicator Table
--***************************************************************************************************************************************************
	SET @SQLSearch = ''INSERT INTO #IndicatorsTable (IndicatorNId, IndicatorName, Score)'' +  
					 '' SELECT Indicator.Indicator_NId, Indicator.Indicator_Name, SUM(dbo.CALCULATE_SCORE(Indicator.Indicator_Name, IndicatorSearch.Value)) AS Score'' +
					 '' FROM UT_Indicator_'' + @Language + '' Indicator JOIN dbo.GET_SPLITTED_LIST('''''' + @SearchIndicators + 
					 '''''', '''','''') IndicatorSearch ON dbo.CALCULATE_SCORE(Indicator.Indicator_Name, IndicatorSearch.Value) > 0'' + 
					 '' GROUP BY Indicator.Indicator_NId, Indicator.Indicator_Name ORDER BY Score DESC''
	EXEC(@SQLSearch)

	INSERT INTO #BasicResultSet
	SELECT DISTINCT Indicators.IndicatorNId, Data.Unit_NId, Areas.AreaNId, Indicators.IndicatorName, Areas.AreaName, 
	Indicators.Score FROM UT_DATA Data 
	JOIN #AreasTable Areas ON Data.Area_NId = Areas.AreaNId 
	JOIN #IndicatorsTable Indicators ON Data.Indicator_NId = Indicators.IndicatorNId
	
	SELECT @Count = 0, @Max = COUNT(*) FROM #BasicResultSet
	WHILE (@Count < @Max)
	BEGIN
		SET @Count = @Count + 1
		SELECT @TempIndicatorNId = IndicatorNId, @TempUnitNId = UnitNId, @TempAreaNId = AreaNId, 
		@TempIndicatorName = IndicatorName, @TempArea = AreaName, @TempScore = Score, 
		@TempSGCount = 0, @TempTPCount = 0, @TempSourceCount = 0, @TempDVCount = 0,
		@TempSGNIds = '','', @TempTPNIds = '','', @TempSourceNIds = '','', @TempDVNIds = '',''
		FROM #BasicResultSet WHERE Id = @Count

		SET @SQLSearch = ''SELECT @TempUnit = Unit_Name FROM UT_Unit_'' + @Language + '' WHERE Unit_NId = '' + CAST(@TempUnitNId AS VARCHAR(MAX))
		EXEC sp_executesql @SQLSearch, N''@TempUnit VARCHAR(MAX) OUTPUT'', @TempUnit OUTPUT 

		SELECT 
			@TempSGNIds = 
			CASE 
				WHEN (CHARINDEX( '','' + CAST(Subgroup_Val_NId AS VARCHAR(MAX))+ '','', @TempSGNIds) = 0) 
				THEN 
					@TempSGNIds + CAST(Subgroup_Val_NId AS VARCHAR(MAX)) + '',''
				ELSE
					@TempSGNIds
			END ,
			@TempTPNIds = 
			CASE 
				WHEN (CHARINDEX( '','' + CAST(TimePeriod_NId AS VARCHAR(MAX))+ '','', @TempTPNIds) = 0) 
				THEN 
					@TempTPNIds + CAST(TimePeriod_NId AS VARCHAR(MAX)) + '',''
				ELSE
					@TempTPNIds
			END ,
			@TempSourceNIds = 
			CASE 
				WHEN (CHARINDEX( '','' + CAST(Source_NId AS VARCHAR(MAX))+ '','', @TempSourceNIds) = 0) 
				THEN 
					@TempSourceNIds + CAST(Source_NId AS VARCHAR(MAX)) + '',''
				ELSE
					@TempSourceNIds
			END ,
			@TempDVNIds = 
			CASE 
				WHEN (CHARINDEX( '','' + CAST(Data_NId AS VARCHAR(MAX))+ '','', @TempDVNIds) = 0) 
				THEN 
					@TempDVNIds + CAST(Data_NId AS VARCHAR(MAX)) + '',''
				ELSE
					@TempDVNIds
			END 
		FROM UT_DATA WHERE Indicator_NId = @TempIndicatorNId AND Unit_NId = @TempUnitNId AND Area_NId = @TempAreaNId
		
		IF (LEN(@TempSGNIds) > 1)
		BEGIN
			SELECT @TempSGNIds = SUBSTRING(@TempSGNIds, 2, LEN(@TempSGNIds) - 2)
		END
		
		IF (LEN(@TempTPNIds) > 1)
		BEGIN
			SELECT @TempTPNIds = SUBSTRING(@TempTPNIds, 2, LEN(@TempTPNIds) - 2)
		END

		IF (LEN(@TempSourceNIds) > 1)
		BEGIN
			SELECT @TempSourceNIds = SUBSTRING(@TempSourceNIds, 2, LEN(@TempSourceNIds) - 2)
		END

		IF (LEN(@TempDVNIds) > 1)
		BEGIN
			SELECT @TempDVNIds = SUBSTRING(@TempDVNIds, 2, LEN(@TempDVNIds) - 2)
		END


		SET @TempSGCount = LEN(@TempSGNIds) - LEN(REPLACE(@TempSGNIds, '','', '''')) + 1
		SET @TempTPCount = LEN(@TempTPNIds) - LEN(REPLACE(@TempTPNIds, '','', '''')) + 1
		SET @TempSourceCount = LEN(@TempSourceNIds) - LEN(REPLACE(@TempSourceNIds, '','', '''')) + 1
		SET @TempDVCount = LEN(@TempDVNIds) - LEN(REPLACE(@TempDVNIds, '','', '''')) + 1
		
		INSERT INTO #ResultSet
		SELECT @TempIndicatorName, @TempUnit, @TempArea, @TempSGCount, @TempTPCount, @TempSourceCount, @TempDVCount, @TempSGNIds, @TempTPNIds,
		@TempSourceNIds, @TempDVNIds, @TempScore
	END
	
	SELECT * FROM #ResultSet AS ResultSet ORDER BY Score DESC FOR XML AUTO, ELEMENTS

	DROP TABLE #BasicResultSet
	DROP TABLE #ResultSet
	DROP TABLE #IndicatorsTable
	DROP TABLE #AreasTable
END

-- DROP PROCEDURE GET_SEARCH_RESULTS


' 
END


--SPSEPARATOR--

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_get_AreaNames_XX]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_get_AreaNames_XX]
	@AreaNids nvarchar(MAX)
AS
BEGIN
	SET NOCOUNT ON;

Declare @Normalized_Area_Nids nvarchar(MAX)
Set @Normalized_Area_Nids = dbo.[fn_get_normalized_area_nids_XX](@AreaNids)

select Area_Name from dbo.ut_area_XX
where area_nid in 
(
Select items from dbo.split(@Normalized_Area_Nids,'','') 
)
order by Area_Name asc

END

' 
END


IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[sp_get_area_wheredataexists_XX]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_get_area_wheredataexists_XX]
	@strAllAreaQSIds_Nids  nvarchar(MAX),
	@strIUS_Nids nvarchar(MAX)
AS
BEGIN	
	SET NOCOUNT ON;

	
	IF (SUBSTRING(@strAllAreaQSIds_Nids,1,2) != ''L_'')
		BEGIN
			Declare @strAreaNids varchar(MAX)
			Set @strAreaNids = dbo.fn_get_child_areas_XX(@strAllAreaQSIds_Nids) --dbo.sp_get_child_areas(dbo.getAreaNid_XX(@strAllAreaQSIds_Nids)) --dbo.fn_get_normalized_area_nids_XX(@strAllAreaQSIds_Nids)
				
			SELECT Distinct Area_NId FROM dbo.UT_Data
			where IUSNId in 
			(
			select distinct items from dbo.split(@strIUS_Nids,'','') 
			)
			and 
			Area_NId in 
			(
			select distinct items from dbo.split(@strAreaNids,'','') 
			) 
		END
	
	ELSE
		BEGIN
			DECLARE @intLevels nvarchar(MAX)

			SET @intLevels = REPLACE(@strAllAreaQSIds_Nids,''L_'','''')
			
			SELECT Distinct Area_NId FROM dbo.UT_Data
			where IUSNId in 
			(
			select distinct items from dbo.split(@strIUS_Nids,'','') 
			)
			and 
			Area_NId in 
			(
			select distinct area_nid from dbo.ut_area_XX 
			where area_level in 
				(
					select distinct CAST(items as int) from dbo.split(@intLevels,'','') 
				)
			) 

		END

END

' 
END

--SPSEPARATOR--

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[SP_GET_AS_RESULTS_IU_XX]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[SP_GET_AS_RESULTS_IU_XX]
	@SearchIndicatorICs		VARCHAR(MAX),
	@SearchAreas			VARCHAR(MAX)
AS
BEGIN
	DECLARE @SQLSearch					NVARCHAR(MAX);
	DECLARE @FinalSearchAreas			VARCHAR(MAX);

	DECLARE @Max						INT;
	DECLARE @Count						INT;
	DECLARE @TempIndicatorNId			INT;
	DECLARE @TempUnitNId				INT;
	DECLARE @TempDefaultSGNId			INT;
	DECLARE @TempAreaCount				INT;
	DECLARE @TempSGCount				INT;
	DECLARE @TempSourceCount			INT;
	DECLARE @TempTPCount				INT;
	DECLARE @TempDVCount				INT;
	DECLARE @TempAreaNIds				VARCHAR(MAX);
	DECLARE @TempSGNIds					VARCHAR(MAX);
	DECLARE @TempSourceNIds				VARCHAR(MAX);
	DECLARE @TempTPNIds					VARCHAR(MAX);
	DECLARE @TempDVNIds					VARCHAR(MAX);
	DECLARE @TempDVSeries				VARCHAR(MAX);
	DECLARE @TempAreaSeries				VARCHAR(MAX);
	DECLARE @SearchLanguage				VARCHAR(MAX);

	SET @SearchLanguage = ''_XX'';
	SELECT @SearchLanguage = SUBSTRING(@SearchLanguage, 2, LEN(@SearchLanguage) - 1)

	CREATE TABLE #ResultSet
	(
		Id							INT IDENTITY(1,1),
		IndicatorNId				INT DEFAULT 0,
		UnitNId						INT DEFAULT 0,
		AreaNId						INT DEFAULT 0,
		Indicator					VARCHAR(MAX) DEFAULT '''' NOT NULL,
		ICName						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		Unit						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		Area						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		DefaultSGNId				INT DEFAULT 0,
		DefaultSG					VARCHAR(MAX) DEFAULT '''' NOT NULL,
		MRDTP						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		MRD							VARCHAR(MAX) DEFAULT '''' NOT NULL,
		AreaCount					INT DEFAULT 0,
		SGCount						INT DEFAULT 0,
		SourceCount					INT DEFAULT 0,
		TPCount						INT DEFAULT 0,
		DVCount						INT DEFAULT 0,
		AreaNIds					VARCHAR(MAX) DEFAULT '''' NOT NULL,
		SGNIds						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		SourceNIds					VARCHAR(MAX) DEFAULT '''' NOT NULL,
		TPNIds						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		DVNIds						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		DVSeries					VARCHAR(MAX) DEFAULT '''' NOT NULL,
		Score						FLOAT DEFAULT 0.0
	)
	
	CREATE TABLE #SearchAreas 
	(
		Id							INT IDENTITY(1,1) ,
		AreaNId						INT DEFAULT 0,
		Area						VARCHAR(MAX) DEFAULT '''' NOT NULL
	)

	CREATE TABLE #SearchIndicators 
	(
		Id							INT IDENTITY(1,1) ,
		IndicatorNId				INT DEFAULT 0,
		Indicator					VARCHAR(MAX) DEFAULT '''' NOT NULL,
		ICName						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		Score						FLOAT DEFAULT 0.0
	)

	CREATE TABLE #SearchICs 
	(
		Id							INT IDENTITY(1,1) ,
		ICNId						INT DEFAULT 0,
		ICName						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		Score						FLOAT DEFAULT 0.0
	)

	CREATE TABLE #ICIndicators
	(
		Id							INT IDENTITY(1,1) ,
		IndicatorNId				INT DEFAULT 0,
		Indicator					VARCHAR(MAX) DEFAULT '''' NOT NULL,
		ICName						VARCHAR(MAX) DEFAULT '''' NOT NULL,
		Score						FLOAT DEFAULT 0.0
	)

	SELECT @FinalSearchAreas = dbo.fn_get_normalized_area_nids_XX(@SearchAreas)
	
--***************************************************************************************************************************************************
-- Filling Area Table
--***************************************************************************************************************************************************

	INSERT INTO #SearchAreas (AreaNId, Area)   
	SELECT Area.Area_NId, Area.Area_Name FROM UT_Area_XX Area 
	JOIN dbo.FN_GET_SPLITTED_LIST(@FinalSearchAreas, '','') List 
	ON Area.Area_NId = List.Value

--***************************************************************************************************************************************************
-- Filling Indicator Table
--***************************************************************************************************************************************************
	INSERT INTO #SearchIndicators (IndicatorNId, Indicator, Score)
	SELECT Indicator.Indicator_NId, Indicator.Indicator_Name, MAX(dbo.FN_CALCULATE_SCORE(Indicator.Indicator_Name, SearchIndicators.Value)) AS Score
	FROM UT_Indicator_XX Indicator JOIN dbo.FN_GET_SPLITTED_LIST(@SearchIndicatorICs, ''||'') SearchIndicators 
	ON dbo.FN_CALCULATE_SCORE(Indicator.Indicator_Name, SearchIndicators.Value) > 0
	GROUP BY Indicator.Indicator_NId, Indicator.Indicator_Name

	IF EXISTS(SELECT 1 FROM #SearchIndicators WHERE Score = 100)
	BEGIN
		DELETE FROM #SearchIndicators WHERE Score <> 100
	END
	ELSE
	BEGIN
		--***************************************************************************************************************************************************
		-- Filling IC Table
		--***************************************************************************************************************************************************
			INSERT INTO #SearchICs (ICNId, ICName, Score)  
			SELECT IC.IC_NId, IC.IC_Name, MAX(dbo.FN_CALCULATE_SCORE(IC.IC_Name, SearchICs.Value)) AS Score
			FROM UT_Indicator_Classifications_XX IC JOIN dbo.FN_GET_SPLITTED_LIST(@SearchIndicatorICs, ''||'') SearchICs 
			ON dbo.FN_CALCULATE_SCORE(IC.IC_Name, SearchICs.Value) > 0
			GROUP BY IC.IC_NId, IC.IC_Name

			INSERT INTO #ICIndicators (IndicatorNId, Indicator, ICName, Score)
			SELECT DISTINCT Indicator.Indicator_NId, Indicator.Indicator_Name, ICs.ICName, ICs.Score FROM #SearchICs ICs
			JOIN UT_ic_IUS IC_IUS ON ICs.ICNId = IC_IUS.IC_NId
			JOIN UT_Indicator_Unit_Subgroup IUS ON IC_IUS.IUSNId = IUS.IUSNId
			JOIN UT_Indicator_XX Indicator ON IUS.Indicator_NId = Indicator.Indicator_NId

			INSERT INTO #SearchIndicators (IndicatorNId, Indicator, ICName, Score)
			SELECT IndicatorNId, Indicator, ICName, Score FROM #ICIndicators
			WHERE IndicatorNId NOT IN (SELECT IndicatorNId FROM #SearchIndicators)
	END

	INSERT INTO #ResultSet (IndicatorNId, UnitNId, Indicator, ICName, Unit, Score)
	SELECT DISTINCT Data.Indicator_NId, Data.Unit_NId, Data.Indicator, Data.ICName, Unit.Unit_Name, Data.Score FROM 
	(
		SELECT DISTINCT Indicator_NId, Unit_NId, #SearchIndicators.Indicator, #SearchIndicators.ICName, #SearchIndicators.Score FROM UT_Data 
		JOIN #SearchIndicators ON UT_Data.Indicator_NId = #SearchIndicators.IndicatorNId 
		JOIN #SearchAreas ON UT_Data.Area_NId = #SearchAreas.AreaNId 
	) Data 
	JOIN UT_Unit_XX Unit ON Data.Unit_NId = Unit.Unit_NId 
	ORDER BY Data.Score DESC, Data.Indicator_NId, Data.Unit_NId

	UPDATE #ResultSet SET DefaultSGNId = IUS.Subgroup_Val_NId , DefaultSG = SGV.Subgroup_Val
	FROM UT_Indicator_Unit_Subgroup IUS, UT_Subgroup_Vals_XX SGV 
	WHERE IndicatorNId = IUS.Indicator_NId AND UnitNId = IUS.Unit_NId 
	AND IUS.IsDefaultSubgroup = 1 AND IUS.Subgroup_Val_NId = SGV.Subgroup_Val_NId

	SELECT @Count = 0, @Max = COUNT(*) FROM #ResultSet
	WHILE(@Count < @Max)
	BEGIN
		SET @Count = @Count + 1;
			
			SELECT @TempIndicatorNId = IndicatorNId, @TempUnitNId = UnitNId, @TempDefaultSGNId = DefaultSGNId, 
			@TempAreaCount = 0, @TempSGCount = 0, @TempSourceCount = 0, @TempTPCount = 0, @TempDVCount = 0,
			@TempAreaNIds = '','', @TempSGNIds = '','', @TempSourceNIds = '','', @TempTPNIds = '','', @TempDVNIds = '','', 
			@TempDVSeries = '','', @TempAreaSeries = '',''
			FROM #ResultSet WHERE Id = @Count

			SELECT 
				@TempAreaNIds = 
				CASE 
					WHEN (CHARINDEX( '','' + CAST(Data.Area_NId AS VARCHAR(MAX))+ '','', @TempAreaNIds) = 0) 
					THEN 
						@TempAreaNIds + CAST(Data.Area_NId AS VARCHAR(MAX)) + '',''
					ELSE
						@TempAreaNIds
				END ,
				@TempSGNIds = 
				CASE 
					WHEN (CHARINDEX( '','' + CAST(Data.Subgroup_Val_NId AS VARCHAR(MAX))+ '','', @TempSGNIds) = 0) 
					THEN 
						@TempSGNIds + CAST(Data.Subgroup_Val_NId AS VARCHAR(MAX)) + '',''
					ELSE
						@TempSGNIds
				END ,
				@TempSourceNIds = 
				CASE 
					WHEN (CHARINDEX( '','' + CAST(Data.Source_NId AS VARCHAR(MAX))+ '','', @TempSourceNIds) = 0) 
					THEN 
						@TempSourceNIds + CAST(Data.Source_NId AS VARCHAR(MAX)) + '',''
					ELSE
						@TempSourceNIds
				END ,
				@TempTPNIds = 
				CASE 
					WHEN (CHARINDEX( '','' + CAST(Data.TimePeriod_NId AS VARCHAR(MAX))+ '','', @TempTPNIds) = 0) 
					THEN 
						@TempTPNIds + CAST(Data.TimePeriod_NId AS VARCHAR(MAX)) + '',''
					ELSE
						@TempTPNIds
				END ,
				@TempDVNIds = 
				CASE 
					WHEN (CHARINDEX( '','' + CAST(Data.Data_NId AS VARCHAR(MAX))+ '','', @TempDVNIds) = 0) 
					THEN 
						@TempDVNIds + CAST(Data.Data_NId AS VARCHAR(MAX)) + '',''
					ELSE
						@TempDVNIds
				END 
			FROM UT_DATA Data JOIN dbo.FN_GET_SPLITTED_LIST(@FinalSearchAreas, '','') Areas ON Data.Area_NId = Areas.Value
			WHERE Data.Indicator_NId = @TempIndicatorNId AND Data.Unit_NId = @TempUnitNId 
			
			SELECT @TempDVSeries = @TempDVSeries + CAST(Tbl.Data_Value AS VARCHAR(MAX)) + '','', @TempAreaSeries = @TempAreaSeries + Tbl.Area + '',''
			FROM 
			(
				SELECT TOP 2147483647 Data.Data_Value, Areas.Area FROM UT_DATA Data JOIN #SearchAreas Areas ON Data.Area_NId = Areas.AreaNId
				WHERE Data.Indicator_NId = @TempIndicatorNId AND Data.Unit_NId = @TempUnitNId AND Data.Subgroup_Val_NId = @TempDefaultSGNId AND 
					  Data.isMRD = 1 
				ORDER BY CAST(Data.Data_Value AS NUMERIC)
			) Tbl

			IF (LEN(@TempAreaNIds) > 1)
			BEGIN
				SELECT @TempAreaNIds = SUBSTRING(@TempAreaNIds, 2, LEN(@TempAreaNIds) - 2)
				SET @TempAreaCount = LEN(@TempAreaNIds) - LEN(REPLACE(@TempAreaNIds, '','', '''')) + 1
			END	
			ELSE IF (LEN(@TempAreaNIds) = 1)
			BEGIN
				SELECT @TempAreaNIds = ''''
				SET @TempAreaCount = 0
			END	
		
			IF (LEN(@TempSGNIds) > 1)
			BEGIN
				SELECT @TempSGNIds = SUBSTRING(@TempSGNIds, 2, LEN(@TempSGNIds) - 2)
				SET @TempSGCount = LEN(@TempSGNIds) - LEN(REPLACE(@TempSGNIds, '','', '''')) + 1
			END
			ELSE IF (LEN(@TempSGNIds) = 1)
			BEGIN
				SELECT @TempSGNIds = ''''
				SET @TempSGCount = 0
			END	

			IF (LEN(@TempSourceNIds) > 1)
			BEGIN
				SELECT @TempSourceNIds = SUBSTRING(@TempSourceNIds, 2, LEN(@TempSourceNIds) - 2)
				SET @TempSourceCount = LEN(@TempSourceNIds) - LEN(REPLACE(@TempSourceNIds, '','', '''')) + 1
			END
			ELSE IF (LEN(@TempSourceNIds) = 1)
			BEGIN
				SELECT @TempSourceNIds = ''''
				SET @TempSourceCount = 0
			END	

			IF (LEN(@TempTPNIds) > 1)
			BEGIN
				SELECT @TempTPNIds = SUBSTRING(@TempTPNIds, 2, LEN(@TempTPNIds) - 2)
				SET @TempTPCount = LEN(@TempTPNIds) - LEN(REPLACE(@TempTPNIds, '','', '''')) + 1
			END
			ELSE IF (LEN(@TempTPNIds) = 1)
			BEGIN
				SELECT @TempTPNIds = ''''
				SET @TempTPCount = 0
			END	

			IF (LEN(@TempDVNIds) > 1)
			BEGIN
				SELECT @TempDVNIds = SUBSTRING(@TempDVNIds, 2, LEN(@TempDVNIds) - 2)
				SET @TempDVCount = LEN(@TempDVNIds) - LEN(REPLACE(@TempDVNIds, '','', '''')) + 1
			END
			ELSE IF (LEN(@TempDVNIds) = 1)
			BEGIN
				SELECT @TempDVNIds = ''''
				SET @TempDVCount = 0
			END	

			IF (LEN(@TempDVSeries) > 1)
			BEGIN
				SELECT @TempDVSeries = SUBSTRING(@TempDVSeries, 2, LEN(@TempDVSeries) - 2)
			END
			ELSE IF (LEN(@TempDVSeries) = 1)
			BEGIN
				SELECT @TempDVSeries = ''''
			END

			IF (LEN(@TempAreaSeries) > 1)
			BEGIN
				SELECT @TempAreaSeries = SUBSTRING(@TempAreaSeries, 2, LEN(@TempAreaSeries) - 2)
			END
			ELSE IF (LEN(@TempAreaSeries) = 1)
			BEGIN
				SELECT @TempAreaSeries = ''''
			END	

			UPDATE #ResultSet
			SET	AreaCount = @TempAreaCount, SGCount = @TempSGCount, SourceCount = @TempSourceCount, TPCount = @TempTPCount, DVCount = @TempDVCount,
				AreaNIds = @TempAreaNIds, SGNIds = @TempSGNIds, SourceNIds = @TempSourceNIds, TPNIds = @TempTPNIds, DVNIds = @TempDVNIds, 
				DVSeries = @TempAreaSeries + '':'' + @TempDVSeries
			WHERE IndicatorNId = @TempIndicatorNId AND UnitNId = @TempUnitNId
	END

	SELECT * FROM #ResultSet AS ResultSet ORDER BY Score DESC, IndicatorNId, UnitNId

	DROP TABLE #ResultSet
	DROP TABLE #SearchAreas
	DROP TABLE #SearchIndicators
	DROP TABLE #SearchICs
	DROP TABLE #ICIndicators
END





' 
END


IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[SP_GET_AREA_DVCLOUD_DATA_XX]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[SP_GET_AREA_DVCLOUD_DATA_XX]
	@IndicatorNId		INT,
	@UnitNId			INT,
	@Area				VARCHAR(MAX)
AS
BEGIN
	DECLARE @SQL					NVARCHAR(MAX);
	DECLARE @Language				VARCHAR(MAX)
	DECLARE @DefaultSGNId			INT;
	

	SET @SQL = '''';
	SET @DefaultSGNId = 0;
	SELECT @Area = dbo.fn_get_normalized_area_nids_XX(@Area)

	SELECT @DefaultSGNId = IUS.Subgroup_Val_NId FROM UT_Indicator_Unit_Subgroup IUS 
	WHERE Indicator_NId = @IndicatorNId AND Unit_NId = @UnitNId AND IUS.IsDefaultSubgroup = 1

	IF ( @DefaultSGNId <> 0)
	BEGIN
		SELECT DISTINCT Area.Area_Name, Data.Data_Value, Data.Data_NId FROM
		(
			SELECT DISTINCT Data.Area_NId, Data.Data_Value, Data.Data_NId FROM UT_Data Data
			JOIN dbo.FN_GET_SPLITTED_LIST(@Area, '','') List ON Data.Area_NId = List.Value
			WHERE Data.Indicator_NId = @IndicatorNId AND Data.Unit_NId = @UnitNId AND Data.Subgroup_Val_NId = @DefaultSGNId AND Data.isMRD = 1 
		) Data JOIN UT_Area_XX Area ON Data.Area_NId = Area.Area_NId
	END
END
' 
END


--SPSEPARATOR--

---------------------------------- Priyanka ------------------------


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_get_iusinfo_from_nids_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_get_iusinfo_from_nids_XX]  
 @iusNIds  VARCHAR(MAX)  
AS  
BEGIN  
	SELECT ius.iusnid,ius.indicator_nid,ius.unit_nid,ius.Subgroup_Val_Nid,
	i.indicator_Name,u.Unit_Name,s.Subgroup_Val 
	FROM ut_Indicator_Unit_Subgroup ius
		inner join ut_Indicator_XX i on ius.Indicator_Nid=i.Indicator_Nid
		inner join ut_Unit_XX u on ius.Unit_Nid=u.Unit_Nid
		inner join ut_Subgroup_vals_XX s on ius.Subgroup_Val_Nid=s.Subgroup_Val_Nid
	where iusnid in(select items from dbo.split(@iusNIds,'',''))
END  

--sp_get_iusinfo_from_nids_XX ''612,646''' 
END


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_get_areaMapLayer_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_get_areaMapLayer_XX]
	@strAllAreaQSIds_Nids  nvarchar(MAX)
AS
BEGIN

Declare @strAreaNids varchar(MAX)
Declare @strSQL nvarchar(MAX)
Set @strAreaNids = dbo.fn_get_normalized_area_nids_XX(@strAllAreaQSIds_Nids)

select amm.layer_name,aml.layer_nid,aml.layer_size,aml.layer_type,aml.minx,aml.miny,aml.maxx,aml.maxy,aml.start_date,aml.end_date,aml.update_timestamp,a.area_level, a.area_nid, a.area_id, a.area_block , A.Area_Name 
from ut_area_map_layer as aml,ut_area_map_metadata_XX as amm,ut_area_map as am,ut_area_XX as a 
where  
aml.layer_nid = amm.layer_nid and aml.layer_nid = am.layer_nid and am.area_nid = a.area_nid 
and a.area_nid in (select items from dbo.split(@strAreaNids, '','')) order by aml.layer_type desc,a.area_level 


exec sp_executesql @strSQL

END' 
END



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_get_areaMapLayer_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'ALTER PROCEDURE [dbo].[sp_get_areaMapLayer_XX]
	@strAllAreaQSIds_Nids  nvarchar(MAX)
AS
BEGIN

Declare @strAreaNids varchar(MAX)
Declare @strSQL nvarchar(MAX)
Set @strAreaNids = dbo.fn_get_normalized_area_nids_XX(@strAllAreaQSIds_Nids)

select amm.layer_name,aml.layer_nid,aml.layer_size,aml.layer_type,aml.minx,aml.miny,aml.maxx,aml.maxy,aml.start_date,aml.end_date,aml.update_timestamp,a.area_level, a.area_nid, a.area_id, a.area_block , A.Area_Name 
from ut_area_map_layer as aml,ut_area_map_metadata_XX as amm,ut_area_map as am,ut_area_XX as a 
where  
aml.layer_nid = amm.layer_nid and aml.layer_nid = am.layer_nid and am.area_nid = a.area_nid 
and a.area_nid in (select items from dbo.split(@strAreaNids, '','')) order by aml.layer_type desc,a.area_level 


exec sp_executesql @strSQL

END' 
END



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_get_all_indicators_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
create PROCEDURE [dbo].[sp_get_all_indicators_XX]
AS
BEGIN

SELECT  Indicator_NId FROM	dbo.ut_indicator_XX
			
END

' 
END



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_get_all_blockAreas_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
create PROCEDURE [dbo].[sp_get_all_blockAreas_XX]
AS
BEGIN

SELECT ''BL_'' + CAST(AREA_NID AS NVARCHAR) AS Area_NId
FROM DBO.UT_AREA_XX
WHERE AREA_BLOCK LIKE ''%,%'' 
			
END

' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
alter PROCEDURE [dbo].[sp_get_all_blockAreas_XX]
AS
BEGIN

SELECT ''BL_'' + CAST(AREA_NID AS NVARCHAR) AS Area_NId
FROM DBO.UT_AREA_XX
WHERE AREA_BLOCK LIKE ''%,%'' 
			
END
' 
END


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_get_all_areas_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_get_all_areas_XX]
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Area_NId FROM dbo.ut_area_XX
END
' 
END




IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GET_SEARCH_RESULTS_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[SP_GET_SEARCH_RESULTS_XX]
	@SearchIndicators		VARCHAR(MAX),
	@SearchICs				VARCHAR(MAX),
	@SearchAreas			VARCHAR(MAX),
	@SearchBlocks			BIT
AS
BEGIN
	DECLARE @SQLSearch					NVARCHAR(MAX);
	DECLARE @FinalSearchAreas			NVARCHAR(MAX);

	DECLARE @Max						INT;
	DECLARE @Count						INT;
	DECLARE @TempIndicatorNId			INT;
	DECLARE @TempUnitNId				INT;
	DECLARE @TempAreaNId				INT;
	DECLARE @TempDefaultSGNId			INT;
	DECLARE @TempSGCount				INT;
	DECLARE @TempSourceCount			INT;
	DECLARE @TempTPCount				INT;
	DECLARE @TempDVCount				INT;
	DECLARE @TempSGNIds					VARCHAR(MAX);
	DECLARE @TempSourceNIds				VARCHAR(MAX);
	DECLARE @TempTPNIds					VARCHAR(MAX);
	DECLARE @TempDVNIds					VARCHAR(MAX);
	DECLARE @TempDVSeries				VARCHAR(MAX);
	DECLARE @TempTPSeries				VARCHAR(MAX);
	DECLARE @SearchLanguage				VARCHAR(MAX);
	

		CREATE TABLE #SearchAreas 
		(
			AreaNId						VARCHAR(MAX)
		)

		CREATE TABLE #SearchIndicators 
		(
			IndicatorNId				INT DEFAULT 0
		)

		CREATE TABLE #SearchICs 
		(
			ICNId						INT DEFAULT 0
		)
		
	IF (@SearchAreas IS NOT NULL AND @SearchAreas <> '''')
		BEGIN
			--***************************************************************************************************************************************************
			-- Filling Area Table
			--***************************************************************************************************************************************************
			--SELECT @FinalSearchAreas = dbo.fn_get_normalized_area_nids_XX(@SearchAreas);
			INSERT INTO #SearchAreas   --SELECT Area.Area_NId FROM UT_Area_XX Area 			
			SELECT List.Value 
			FROM dbo.FN_GET_SPLITTED_LIST(@SearchAreas, '','') List 
		END


	IF (@SearchICs IS NOT NULL AND @SearchICs <> '''')
		BEGIN
			INSERT INTO #SearchICs  
			SELECT IC.IC_NId FROM UT_Indicator_Classifications_XX IC 
			JOIN dbo.FN_GET_SPLITTED_LIST(@SearchICs, '','') List 
			ON IC.IC_NId = List.Value

			INSERT INTO #SearchIndicators
			SELECT DISTINCT Indicator.Indicator_NId
			FROM UT_ic_IUS IC_IUS 
			JOIN UT_Indicator_Unit_Subgroup IUS ON IC_IUS.IUSNId = IUS.IUSNId
			JOIN UT_Indicator_XX Indicator ON IUS.Indicator_NId = Indicator.Indicator_NId
			WHERE IC_IUS.IC_NId in (Select SrcICs.ICNId from #SearchICs SrcICs)
		
		END
	IF(@SearchIndicators IS NOT NULL AND @SearchIndicators <> '''')
		BEGIN
			--***************************************************************************************************************************************************
			-- Filling Indicator Table
			--***************************************************************************************************************************************************
			INSERT INTO #SearchIndicators
			SELECT Indicator.Indicator_NId FROM UT_Indicator_XX Indicator 
			JOIN dbo.FN_GET_SPLITTED_LIST(@SearchIndicators, '','') List 
			ON Indicator.Indicator_NId = List.Value
		END
	
	SET @SearchLanguage = ''_XX'';
	SELECT @SearchLanguage = SUBSTRING(@SearchLanguage, 2, LEN(@SearchLanguage) - 1)
	
	IF(@SearchBlocks = 1)
	BEGIN
	
		INSERT INTO #SEARCHAREAS
		SELECT DBO.FN_GET_QS_AREA_FOR_BLOCKS_XX(A.AREANID, I.INDICATORNID)
		FROM #SEARCHAREAS A, #SEARCHINDICATORS I
		
	END

	SELECT DISTINCT NId,	
	SearchLanguage,
	C.IndicatorNId,
	UnitNId,
	C.AreaNId,
	IsAreaNumeric,
	Indicator,
	Unit,
	Area,
	DefaultSG,
	MRDTP,
	MRD,
	AreaCount,
	SGCount,
	SourceCount,
	TPCount,
	DVCount,
	AreaNIds,
	SGNIds,
	SourceNIds,
	TPNIds,
	DVSeries,
	Dimensions,
	[BlockAreaParentNId]
	FROM dbo.DI_SEARCH_RESULTS C -- C = Cache Table
	JOIN #SearchAreas A ON C.AreaNId = A.AreaNId
	JOIN #SearchIndicators I ON C.IndicatorNId = I.IndicatorNId
	WHERE @SearchLanguage = C.SearchLanguage
	ORDER BY C.Indicator, C.Area ASC

	BEGIN
		DROP TABLE #SearchAreas
		DROP TABLE #SearchIndicators
		DROP TABLE #SearchICs
	END


END



' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[SP_GET_SEARCH_RESULTS_XX]
	@SearchIndicators		VARCHAR(MAX),
	@SearchICs				VARCHAR(MAX),
	@SearchAreas			VARCHAR(MAX),
	@SearchBlocks			BIT
AS
BEGIN
	DECLARE @SQLSearch					NVARCHAR(MAX);
	DECLARE @FinalSearchAreas			NVARCHAR(MAX);

	DECLARE @Max						INT;
	DECLARE @Count						INT;
	DECLARE @TempIndicatorNId			INT;
	DECLARE @TempUnitNId				INT;
	DECLARE @TempAreaNId				INT;
	DECLARE @TempDefaultSGNId			INT;
	DECLARE @TempSGCount				INT;
	DECLARE @TempSourceCount			INT;
	DECLARE @TempTPCount				INT;
	DECLARE @TempDVCount				INT;
	DECLARE @TempSGNIds					VARCHAR(MAX);
	DECLARE @TempSourceNIds				VARCHAR(MAX);
	DECLARE @TempTPNIds					VARCHAR(MAX);
	DECLARE @TempDVNIds					VARCHAR(MAX);
	DECLARE @TempDVSeries				VARCHAR(MAX);
	DECLARE @TempTPSeries				VARCHAR(MAX);
	DECLARE @SearchLanguage				VARCHAR(MAX);
	

		CREATE TABLE #SearchAreas 
		(
			AreaNId						VARCHAR(MAX)
		)

		CREATE TABLE #SearchIndicators 
		(
			IndicatorNId				INT DEFAULT 0
		)

		CREATE TABLE #SearchICs 
		(
			ICNId						INT DEFAULT 0
		)
		
	IF (@SearchAreas IS NOT NULL AND @SearchAreas <> '''')
		BEGIN
			--***************************************************************************************************************************************************
			-- Filling Area Table
			--***************************************************************************************************************************************************
			--SELECT @FinalSearchAreas = dbo.fn_get_normalized_area_nids_XX(@SearchAreas);
			INSERT INTO #SearchAreas   --SELECT Area.Area_NId FROM UT_Area_XX Area 			
			SELECT List.Value 
			FROM dbo.FN_GET_SPLITTED_LIST(@SearchAreas, '','') List 
		END


	IF (@SearchICs IS NOT NULL AND @SearchICs <> '''')
		BEGIN
			INSERT INTO #SearchICs  
			SELECT IC.IC_NId FROM UT_Indicator_Classifications_XX IC 
			JOIN dbo.FN_GET_SPLITTED_LIST(@SearchICs, '','') List 
			ON IC.IC_NId = List.Value

			INSERT INTO #SearchIndicators
			SELECT DISTINCT Indicator.Indicator_NId
			FROM UT_ic_IUS IC_IUS 
			JOIN UT_Indicator_Unit_Subgroup IUS ON IC_IUS.IUSNId = IUS.IUSNId
			JOIN UT_Indicator_XX Indicator ON IUS.Indicator_NId = Indicator.Indicator_NId
			WHERE IC_IUS.IC_NId in (Select SrcICs.ICNId from #SearchICs SrcICs)
		
		END
	IF(@SearchIndicators IS NOT NULL AND @SearchIndicators <> '''')
		BEGIN
			--***************************************************************************************************************************************************
			-- Filling Indicator Table
			--***************************************************************************************************************************************************
			INSERT INTO #SearchIndicators
			SELECT Indicator.Indicator_NId FROM UT_Indicator_XX Indicator 
			JOIN dbo.FN_GET_SPLITTED_LIST(@SearchIndicators, '','') List 
			ON Indicator.Indicator_NId = List.Value
		END
	
	SET @SearchLanguage = ''_XX'';
	SELECT @SearchLanguage = SUBSTRING(@SearchLanguage, 2, LEN(@SearchLanguage) - 1)

	IF(@SearchBlocks = 1)
	BEGIN
	
		INSERT INTO #SEARCHAREAS
		SELECT DBO.FN_GET_QS_AREA_FOR_BLOCKS_XX(A.AREANID, I.INDICATORNID)
		FROM #SEARCHAREAS A, #SEARCHINDICATORS I
		
	END
	
	SELECT DISTINCT NId,	
	SearchLanguage,
	C.IndicatorNId,
	UnitNId,
	C.AreaNId,
	IsAreaNumeric,
	Indicator,
	Unit,
	Area,
	DefaultSG,
	MRDTP,
	MRD,
	AreaCount,
	SGCount,
	SourceCount,
	TPCount,
	DVCount,
	AreaNIds,
	SGNIds,
	SourceNIds,
	TPNIds,
	DVSeries,
	Dimensions,
	[BlockAreaParentNId]
	FROM dbo.DI_SEARCH_RESULTS C -- C = Cache Table
	JOIN #SearchAreas A ON C.AreaNId = A.AreaNId
	JOIN #SearchIndicators I ON C.IndicatorNId = I.IndicatorNId
	WHERE @SearchLanguage = C.SearchLanguage
	ORDER BY C.Indicator, C.Area ASC

	BEGIN
		DROP TABLE #SearchAreas
		DROP TABLE #SearchIndicators
		DROP TABLE #SearchICs
	END


END


' 
END


--SPSEPARATOR--


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_get_matcharealist_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_get_matcharealist_XX] (
@type nvarchar(5),
@arealist nvarchar(MAX))
AS
Begin
	Declare @strSQL nvarchar(MAX)
	/*SET NOCOUNT ON*/	
	if (@type = ''aid'')
	Begin
	set @strSQL = ''Select Area_id, Area_name from ut_area_XX where area_id in (''+@arealist+'')''
	end
	if (@type = ''aname'')
	Begin
	set @strSQL = ''Select Area_name, Area_id from ut_area_XX where area_name in ('' + @arealist + '')''	
	end
	exec sp_executesql @strSQL
END' 
END

ELSE

BEGIN

EXEC dbo.sp_executesql @statement = N'ALTER PROCEDURE [dbo].[sp_get_matcharealist_XX] (
@type nvarchar(5),
@arealist nvarchar(MAX))
AS
Begin
	Declare @strSQL nvarchar(MAX)
	/*SET NOCOUNT ON*/	
	if (@type = ''aid'')
	Begin
	set @strSQL = ''Select Area_id, Area_name from ut_area_XX where area_id in (''+@arealist+'')''
	end
	if (@type = ''aname'')
	Begin
	set @strSQL = ''Select Area_name, Area_id from ut_area_XX where area_name in ('' + @arealist + '')''	
	end
	exec sp_executesql @strSQL
END'  
END


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CREATE_CACHE_RESULT_IUA_GENERAL_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[SP_CREATE_CACHE_RESULT_IUA_GENERAL_XX]
	@SearchIndicator		INT
AS
BEGIN
	DECLARE @SQLSearch					NVARCHAR(MAX);
	DECLARE @FinalSearchAreas			NVARCHAR(MAX);

	DECLARE @Max						INT;
	DECLARE @Count						INT;
	DECLARE @TempIndicatorNId			INT;
	DECLARE @TempUnitNId				INT;
	DECLARE @TempAreaNId				INT;
	DECLARE @TempDefaultSGNId			INT;
	DECLARE @TempSGCount				INT;
	DECLARE @TempSourceCount			INT;
	DECLARE @TempTPCount				INT;
	DECLARE @TempDVCount				INT;
	DECLARE @TempSGNIds					NVARCHAR(MAX);
	DECLARE @TempSourceNIds				NVARCHAR(MAX);
	DECLARE @TempTPNIds					NVARCHAR(MAX);
	DECLARE @TempDVNIds					NVARCHAR(MAX);
	DECLARE @TempDVSeries				NVARCHAR(MAX);
	DECLARE @TempDimensions				NVARCHAR(MAX);
	DECLARE @TempTPSeries				NVARCHAR(MAX);
	DECLARE @SearchLanguage				NVARCHAR(MAX);
	
	SET @SearchLanguage = ''_XX'';
	SELECT @SearchLanguage = SUBSTRING(@SearchLanguage, 2, LEN(@SearchLanguage) - 1)

	
		CREATE TABLE #ResultSet
		(
			Id							INT IDENTITY(1,1) PRIMARY KEY,
			IndicatorNId				INT DEFAULT 0,
			UnitNId						INT DEFAULT 0,
			AreaNId						INT DEFAULT 0,
			Indicator					NVARCHAR(MAX) DEFAULT '''',
			ICName						NVARCHAR(MAX) DEFAULT '''',
			Unit						NVARCHAR(MAX) DEFAULT '''',
			Area						NVARCHAR(MAX) DEFAULT '''',
			DefaultSGNId				INT DEFAULT 0,
			DefaultSG					NVARCHAR(MAX) DEFAULT '''',
			MRDTP						NVARCHAR(MAX) DEFAULT '''',
			MRD							NVARCHAR(MAX) DEFAULT '''',
			SGCount						INT DEFAULT 0,
			SourceCount					INT DEFAULT 0,
			TPCount						INT DEFAULT 0,
			DVCount						INT DEFAULT 0,
			SGNIds						NVARCHAR(MAX) DEFAULT '''',
			SourceNIds					NVARCHAR(MAX) DEFAULT '''' ,
			TPNIds						NVARCHAR(MAX) DEFAULT ''''  ,
			DVNIds						NVARCHAR(MAX) DEFAULT ''''  ,
			DVSeries					NVARCHAR(MAX) DEFAULT ''''  ,
			Dimensions					NVARCHAR(MAX) DEFAULT '''',
			BlockAreaParentNId			INT DEFAULT 0
		)

		CREATE TABLE #SearchAreas 
		(
			Id							INT IDENTITY(1,1) PRIMARY KEY,
			AreaNId						INT DEFAULT 0,
			Area						NVARCHAR(MAX) DEFAULT '''' NOT NULL
		)

		CREATE TABLE #SearchIndicators 
		(
			Id							INT IDENTITY(1,1) PRIMARY KEY,
			IndicatorNId				INT DEFAULT 0,
			Indicator					NVARCHAR(MAX) DEFAULT '''' NOT NULL,
			ICName						NVARCHAR(MAX) DEFAULT '''' NOT NULL
		)

		CREATE TABLE #SearchICs 
		(
			Id							INT IDENTITY(1,1) PRIMARY KEY,
			ICNId						INT DEFAULT 0,
			ICName						NVARCHAR(MAX) DEFAULT '''' NOT NULL
		)

		CREATE TABLE #ICIndicators
		(
			Id							INT IDENTITY(1,1) PRIMARY KEY,
			IndicatorNId				INT DEFAULT 0,
			Indicator					NVARCHAR(MAX) DEFAULT '''' NOT NULL,
			ICName						NVARCHAR(MAX) DEFAULT '''' NOT NULL
		)

		
			--***************************************************************************************************************************************************
			-- Filling Area Table
			--***************************************************************************************************************************************************
			INSERT INTO #SearchAreas (AreaNId, Area)   
			SELECT Area.Area_NId, Area.Area_Name FROM UT_Area_XX Area 
			

		IF (@SearchIndicator IS NOT NULL AND @SearchIndicator <> '''')
		BEGIN
			--***************************************************************************************************************************************************
			-- Filling Indicator Table
			--***************************************************************************************************************************************************
			INSERT INTO #SearchIndicators (IndicatorNId, Indicator)
			SELECT Indicator.Indicator_NId, Indicator.Indicator_Name FROM UT_Indicator_XX Indicator
			WHERE  Indicator.Indicator_NId = @SearchIndicator
			--JOIN dbo.FN_GET_SPLITTED_LIST(@SearchIndicator, '','') List 
			--ON Indicator.Indicator_NId = List.Value
		END

		INSERT INTO #ResultSet (IndicatorNId, UnitNId, AreaNId, Indicator, ICName, Unit, Area)
		SELECT DISTINCT Data.Indicator_NId, Data.Unit_NId, Data.Area_NId, #SearchIndicators.Indicator, #SearchIndicators.ICName, Unit.Unit_Name, #SearchAreas.Area 
		FROM UT_Data Data
		JOIN #SearchIndicators ON Data.Indicator_NId = #SearchIndicators.IndicatorNId
		JOIN UT_Unit_XX Unit ON Data.Unit_NId = Unit.Unit_NId 
		JOIN #SearchAreas ON Data.Area_NId = #SearchAreas.AreaNId 

		UPDATE #ResultSet SET DefaultSGNId = IUS.Subgroup_Val_NId , DefaultSG = SGV.Subgroup_Val
		FROM UT_Indicator_Unit_Subgroup IUS, UT_Subgroup_Vals_XX SGV 
		WHERE IndicatorNId = IUS.Indicator_NId AND UnitNId = IUS.Unit_NId AND IUS.IsDefaultSubgroup = 1 
		AND IUS.Subgroup_Val_NId = SGV.Subgroup_Val_NId

		UPDATE #ResultSet 
		SET MRDTP = TP.TimePeriod, MRD = ISNULL(dbo.CustomCast(Data.Data_Value), Data.Textual_Data_Value)
		FROM UT_Data Data, UT_TimePeriod TP 
		WHERE IndicatorNId = Data.Indicator_NId AND UnitNId = Data.Unit_NId AND DefaultSGNId = Data.Subgroup_Val_NId 
			  AND AreaNId = Data.Area_NId AND Data.isMRD = 1 AND Data.TimePeriod_NId = TP.TimePeriod_NId
		

		SELECT @Count = 0, @Max = COUNT(*) FROM #ResultSet
		WHILE(@Count < @Max)
		BEGIN
			SET @Count = @Count + 1;
			
			SELECT @TempIndicatorNId = IndicatorNId, @TempUnitNId = UnitNId, @TempAreaNId = AreaNId, @TempDefaultSGNId = DefaultSGNId, 
				   @TempSGCount = 0, @TempSourceCount = 0, @TempTPCount = 0, @TempDVCount = 0,
				   @TempSGNIds = '','', @TempSourceNIds = '','', @TempTPNIds = '','', @TempDVNIds = '','', @TempDVSeries = '','', @TempTPSeries = '',''
			FROM #ResultSet WHERE Id = @Count

			SELECT 
				@TempSGNIds = 
				CASE 
					WHEN (CHARINDEX( '','' + CAST(Tbl.Subgroup_Val_NId AS NVARCHAR(MAX))+ '','', @TempSGNIds) = 0) 
					THEN 
						@TempSGNIds + CAST(Tbl.Subgroup_Val_NId AS NVARCHAR(MAX)) + '',''
					ELSE
						@TempSGNIds
				END ,
				@TempSourceNIds = 
				CASE 
					WHEN (CHARINDEX( '','' + CAST(Tbl.Source_NId AS NVARCHAR(MAX))+ '','', @TempSourceNIds) = 0) 
					THEN 
						@TempSourceNIds + CAST(Tbl.Source_NId AS NVARCHAR(MAX)) + '',''
					ELSE
						@TempSourceNIds
				END ,
				@TempTPNIds = 
				CASE 
					WHEN (CHARINDEX( '','' + CAST(Tbl.TimePeriod_NId AS VARCHAR(MAX))+ '','', @TempTPNIds) = 0) 
					THEN 
						@TempTPNIds + CAST(Tbl.TimePeriod_NId AS NVARCHAR(MAX)) + '',''
					ELSE
						@TempTPNIds
				END ,
				@TempDVNIds = 
				CASE 
					WHEN (CHARINDEX( '','' + CAST(Tbl.Data_NId AS NVARCHAR(MAX))+ '','', @TempDVNIds) = 0) 
					THEN 
						@TempDVNIds + CAST(Tbl.Data_NId AS NVARCHAR(MAX)) + '',''
					ELSE
						@TempDVNIds
				END ,
				@TempDVSeries = 
				CASE 
					WHEN (Tbl.Subgroup_Val_NId = @TempDefaultSGNId) 
					THEN 
						@TempDVSeries + ISNULL(dbo.CustomCast(Tbl.Data_Value), Tbl.Textual_Data_Value) + ''|''
					ELSE
						@TempDVSeries
				END ,
				@TempTPSeries = 
				CASE 
					WHEN (Tbl.Subgroup_Val_NId = @TempDefaultSGNId) 
					THEN 
						@TempTPSeries + Tbl.TimePeriod + ''|''
					ELSE
						@TempTPSeries
				END 
			FROM 
			(
				SELECT DISTINCT TOP 2147483647 Data.Data_NId, Data.Data_Value, Data.Textual_Data_Value, Data.Subgroup_Val_NId, Data.Source_NId, Data.TimePeriod_NId, TP.TimePeriod
				FROM UT_DATA Data JOIN UT_TimePeriod TP ON Data.TimePeriod_NId = TP.TimePeriod_NId 
				WHERE Data.Indicator_NId = @TempIndicatorNId AND Data.Unit_NId = @TempUnitNId AND Data.Area_NId = @TempAreaNId
				ORDER BY TP.TimePeriod
			) Tbl
			
			IF (LEN(@TempSGNIds) > 1)
			BEGIN
				SELECT @TempSGNIds = SUBSTRING(@TempSGNIds, 2, LEN(@TempSGNIds) - 2)
				SET @TempSGCount = LEN(@TempSGNIds) - LEN(REPLACE(@TempSGNIds, '','', '''')) + 1
			END
			ELSE IF (LEN(@TempSGNIds) = 1)
			BEGIN
				SELECT @TempSGNIds = ''''
				SET @TempSGCount = 0
			END	

			IF (LEN(@TempSourceNIds) > 1)
			BEGIN
				SELECT @TempSourceNIds = SUBSTRING(@TempSourceNIds, 2, LEN(@TempSourceNIds) - 2)
				SET @TempSourceCount = LEN(@TempSourceNIds) - LEN(REPLACE(@TempSourceNIds, '','', '''')) + 1
			END
			ELSE IF (LEN(@TempSourceNIds) = 1)
			BEGIN
				SELECT @TempSourceNIds = ''''
				SET @TempSourceCount = 0
			END	

			IF (LEN(@TempTPNIds) > 1)
			BEGIN
				SELECT @TempTPNIds = SUBSTRING(@TempTPNIds, 2, LEN(@TempTPNIds) - 2)
				SET @TempTPCount = LEN(@TempTPNIds) - LEN(REPLACE(@TempTPNIds, '','', '''')) + 1
			END
			ELSE IF (LEN(@TempTPNIds) = 1)
			BEGIN
				SELECT @TempTPNIds = ''''
				SET @TempTPCount = 0
			END	

			IF (LEN(@TempDVNIds) > 1)
			BEGIN
				SELECT @TempDVNIds = SUBSTRING(@TempDVNIds, 2, LEN(@TempDVNIds) - 2)
				SET @TempDVCount = LEN(@TempDVNIds) - LEN(REPLACE(@TempDVNIds, '','', '''')) + 1
			END
			ELSE IF (LEN(@TempDVNIds) = 1)
			BEGIN
				SELECT @TempDVNIds = ''''
				SET @TempDVCount = 0
			END	

			IF (LEN(@TempDVSeries) > 1)
			BEGIN
				SELECT @TempDVSeries = SUBSTRING(@TempDVSeries, 2, LEN(@TempDVSeries) - 2)
			END
			ELSE IF (LEN(@TempDVSeries) = 1)
			BEGIN
				SELECT @TempDVSeries = ''''
			END

			IF (LEN(@TempTPSeries) > 1)
			BEGIN
				SELECT @TempTPSeries = SUBSTRING(@TempTPSeries, 2, LEN(@TempTPSeries) - 2)
			END
			ELSE IF (LEN(@TempTPSeries) = 1)
			BEGIN
				SELECT @TempTPSeries = ''''
			END	

			SET @TempDimensions = [dbo].[fn_get_dimensions_IU_XX](@TempIndicatorNId, @TempUnitNId)

			UPDATE #ResultSet
			SET	SGCount = @TempSGCount, SourceCount = @TempSourceCount, TPCount = @TempTPCount, DVCount = @TempDVCount,
				SGNIds = @TempSGNIds, SourceNIds = @TempSourceNIds, TPNIds = @TempTPNIds, DVNIds = @TempDVNIds, DVSeries = @TempTPSeries + '':'' + @TempDVSeries, Dimensions = @TempDimensions,BlockAreaParentNId=NULL
			WHERE IndicatorNId = @TempIndicatorNId AND UnitNId = @TempUnitNId AND AreaNId = @TempAreaNId
		END
		
		INSERT INTO DI_SEARCH_RESULTS
			SELECT @SearchLanguage,
				   IndicatorNid, UnitNId, AreaNId, 1, 
				   Indicator, Unit, Area, 
				   CAST(DefaultSGNId AS NVARCHAR) + ''[@@@@]'' + DefaultSG, MRDTP, MRD, 
				   0, SGCount, SourceCount, TPCount, DVCount, 
				   '''', SGNIds, SourceNIds, TPNIds, DVNIds, DVSeries , Dimensions,BlockAreaParentNId
			FROM #ResultSet R WHERE MRDTP LIKE ''%_%'' AND MRD LIKE ''%_%'' AND
			NOT EXISTS (	SELECT TOP 1 NId FROM DI_SEARCH_RESULTS C 
								WHERE C.IndicatorNId = R.IndicatorNid
								AND C.AreaNId = CAST(R.AreaNId AS NVARCHAR(50)) AND SearchLanguage = @SearchLanguage)
			AND DVCount > 0

		DROP TABLE #ResultSet
		DROP TABLE #SearchAreas
		DROP TABLE #SearchIndicators
		DROP TABLE #ICIndicators
	

END




' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'

ALTER PROCEDURE [dbo].[SP_CREATE_CACHE_RESULT_IUA_GENERAL_XX]
	@SearchIndicator		INT
AS
BEGIN
	DECLARE @SQLSearch					NVARCHAR(MAX);
	DECLARE @FinalSearchAreas			NVARCHAR(MAX);

	DECLARE @Max						INT;
	DECLARE @Count						INT;
	DECLARE @TempIndicatorNId			INT;
	DECLARE @TempUnitNId				INT;
	DECLARE @TempAreaNId				INT;
	DECLARE @TempDefaultSGNId			INT;
	DECLARE @TempSGCount				INT;
	DECLARE @TempSourceCount			INT;
	DECLARE @TempTPCount				INT;
	DECLARE @TempDVCount				INT;
	DECLARE @TempSGNIds					NVARCHAR(MAX);
	DECLARE @TempSourceNIds				NVARCHAR(MAX);
	DECLARE @TempTPNIds					NVARCHAR(MAX);
	DECLARE @TempDVNIds					NVARCHAR(MAX);
	DECLARE @TempDVSeries				NVARCHAR(MAX);
	DECLARE @TempDimensions				NVARCHAR(MAX);
	DECLARE @TempTPSeries				NVARCHAR(MAX);
	DECLARE @SearchLanguage				NVARCHAR(MAX);
	
	SET @SearchLanguage = ''_XX'';
	SELECT @SearchLanguage = SUBSTRING(@SearchLanguage, 2, LEN(@SearchLanguage) - 1)

	
		CREATE TABLE #ResultSet
		(
			Id							INT IDENTITY(1,1) PRIMARY KEY,
			IndicatorNId				INT DEFAULT 0,
			UnitNId						INT DEFAULT 0,
			AreaNId						INT DEFAULT 0,
			Indicator					NVARCHAR(MAX) DEFAULT '''',
			ICName						NVARCHAR(MAX) DEFAULT '''',
			Unit						NVARCHAR(MAX) DEFAULT '''',
			Area						NVARCHAR(MAX) DEFAULT '''',
			DefaultSGNId				INT DEFAULT 0,
			DefaultSG					NVARCHAR(MAX) DEFAULT '''',
			MRDTP						NVARCHAR(MAX) DEFAULT '''',
			MRD							NVARCHAR(MAX) DEFAULT '''',
			SGCount						INT DEFAULT 0,
			SourceCount					INT DEFAULT 0,
			TPCount						INT DEFAULT 0,
			DVCount						INT DEFAULT 0,
			SGNIds						NVARCHAR(MAX) DEFAULT '''',
			SourceNIds					NVARCHAR(MAX) DEFAULT '''' ,
			TPNIds						NVARCHAR(MAX) DEFAULT ''''  ,
			DVNIds						NVARCHAR(MAX) DEFAULT ''''  ,
			DVSeries					NVARCHAR(MAX) DEFAULT ''''  ,
			Dimensions					NVARCHAR(MAX) DEFAULT '''',
			BlockAreaParentNId			INT DEFAULT 0
		)

		CREATE TABLE #SearchAreas 
		(
			Id							INT IDENTITY(1,1) PRIMARY KEY,
			AreaNId						INT DEFAULT 0,
			Area						NVARCHAR(MAX) DEFAULT '''' NOT NULL
		)

		CREATE TABLE #SearchIndicators 
		(
			Id							INT IDENTITY(1,1) PRIMARY KEY,
			IndicatorNId				INT DEFAULT 0,
			Indicator					NVARCHAR(MAX) DEFAULT '''' NOT NULL,
			ICName						NVARCHAR(MAX) DEFAULT '''' NOT NULL
		)

		CREATE TABLE #SearchICs 
		(
			Id							INT IDENTITY(1,1) PRIMARY KEY,
			ICNId						INT DEFAULT 0,
			ICName						NVARCHAR(MAX) DEFAULT '''' NOT NULL
		)

		CREATE TABLE #ICIndicators
		(
			Id							INT IDENTITY(1,1) PRIMARY KEY,
			IndicatorNId				INT DEFAULT 0,
			Indicator					NVARCHAR(MAX) DEFAULT '''' NOT NULL,
			ICName						NVARCHAR(MAX) DEFAULT '''' NOT NULL
		)

		
			--***************************************************************************************************************************************************
			-- Filling Area Table
			--***************************************************************************************************************************************************
			INSERT INTO #SearchAreas (AreaNId, Area)   
			SELECT Area.Area_NId, Area.Area_Name FROM UT_Area_XX Area 
			

		IF (@SearchIndicator IS NOT NULL AND @SearchIndicator <> '''')
		BEGIN
			--***************************************************************************************************************************************************
			-- Filling Indicator Table
			--***************************************************************************************************************************************************
			INSERT INTO #SearchIndicators (IndicatorNId, Indicator)
			SELECT Indicator.Indicator_NId, Indicator.Indicator_Name FROM UT_Indicator_XX Indicator
			WHERE  Indicator.Indicator_NId = @SearchIndicator
			--JOIN dbo.FN_GET_SPLITTED_LIST(@SearchIndicator, '','') List 
			--ON Indicator.Indicator_NId = List.Value
		END

		INSERT INTO #ResultSet (IndicatorNId, UnitNId, AreaNId, Indicator, ICName, Unit, Area)
		SELECT DISTINCT Data.Indicator_NId, Data.Unit_NId, Data.Area_NId, #SearchIndicators.Indicator, #SearchIndicators.ICName, Unit.Unit_Name, #SearchAreas.Area 
		FROM UT_Data Data
		JOIN #SearchIndicators ON Data.Indicator_NId = #SearchIndicators.IndicatorNId
		JOIN UT_Unit_XX Unit ON Data.Unit_NId = Unit.Unit_NId 
		JOIN #SearchAreas ON Data.Area_NId = #SearchAreas.AreaNId 

		UPDATE #ResultSet SET DefaultSGNId = IUS.Subgroup_Val_NId , DefaultSG = SGV.Subgroup_Val
		FROM UT_Indicator_Unit_Subgroup IUS, UT_Subgroup_Vals_XX SGV 
		WHERE IndicatorNId = IUS.Indicator_NId AND UnitNId = IUS.Unit_NId AND IUS.IsDefaultSubgroup = 1 
		AND IUS.Subgroup_Val_NId = SGV.Subgroup_Val_NId

		UPDATE #ResultSet 
		SET MRDTP = TP.TimePeriod, MRD = ISNULL(dbo.CustomCast(Data.Data_Value), Data.Textual_Data_Value)
		FROM UT_Data Data, UT_TimePeriod TP 
		WHERE IndicatorNId = Data.Indicator_NId AND UnitNId = Data.Unit_NId AND DefaultSGNId = Data.Subgroup_Val_NId 
			  AND AreaNId = Data.Area_NId AND Data.isMRD = 1 AND Data.TimePeriod_NId = TP.TimePeriod_NId
		

		SELECT @Count = 0, @Max = COUNT(*) FROM #ResultSet
		WHILE(@Count < @Max)
		BEGIN
			SET @Count = @Count + 1;
			
			SELECT @TempIndicatorNId = IndicatorNId, @TempUnitNId = UnitNId, @TempAreaNId = AreaNId, @TempDefaultSGNId = DefaultSGNId, 
				   @TempSGCount = 0, @TempSourceCount = 0, @TempTPCount = 0, @TempDVCount = 0,
				   @TempSGNIds = '','', @TempSourceNIds = '','', @TempTPNIds = '','', @TempDVNIds = '','', @TempDVSeries = '','', @TempTPSeries = '',''
			FROM #ResultSet WHERE Id = @Count

			SELECT 
				@TempSGNIds = 
				CASE 
					WHEN (CHARINDEX( '','' + CAST(Tbl.Subgroup_Val_NId AS NVARCHAR(MAX))+ '','', @TempSGNIds) = 0) 
					THEN 
						@TempSGNIds + CAST(Tbl.Subgroup_Val_NId AS NVARCHAR(MAX)) + '',''
					ELSE
						@TempSGNIds
				END ,
				@TempSourceNIds = 
				CASE 
					WHEN (CHARINDEX( '','' + CAST(Tbl.Source_NId AS NVARCHAR(MAX))+ '','', @TempSourceNIds) = 0) 
					THEN 
						@TempSourceNIds + CAST(Tbl.Source_NId AS NVARCHAR(MAX)) + '',''
					ELSE
						@TempSourceNIds
				END ,
				@TempTPNIds = 
				CASE 
					WHEN (CHARINDEX( '','' + CAST(Tbl.TimePeriod_NId AS VARCHAR(MAX))+ '','', @TempTPNIds) = 0) 
					THEN 
						@TempTPNIds + CAST(Tbl.TimePeriod_NId AS NVARCHAR(MAX)) + '',''
					ELSE
						@TempTPNIds
				END ,
				@TempDVNIds = 
				CASE 
					WHEN (CHARINDEX( '','' + CAST(Tbl.Data_NId AS NVARCHAR(MAX))+ '','', @TempDVNIds) = 0) 
					THEN 
						@TempDVNIds + CAST(Tbl.Data_NId AS NVARCHAR(MAX)) + '',''
					ELSE
						@TempDVNIds
				END ,
				@TempDVSeries = 
				CASE 
					WHEN (Tbl.Subgroup_Val_NId = @TempDefaultSGNId) 
					THEN 
						@TempDVSeries + ISNULL(dbo.CustomCast(Tbl.Data_Value), Tbl.Textual_Data_Value) + ''|''
					ELSE
						@TempDVSeries
				END ,
				@TempTPSeries = 
				CASE 
					WHEN (Tbl.Subgroup_Val_NId = @TempDefaultSGNId) 
					THEN 
						@TempTPSeries + Tbl.TimePeriod + ''|''
					ELSE
						@TempTPSeries
				END 
			FROM 
			(
				SELECT DISTINCT TOP 2147483647 Data.Data_NId, Data.Data_Value, Data.Textual_Data_Value, Data.Subgroup_Val_NId, Data.Source_NId, Data.TimePeriod_NId, TP.TimePeriod
				FROM UT_DATA Data JOIN UT_TimePeriod TP ON Data.TimePeriod_NId = TP.TimePeriod_NId 
				WHERE Data.Indicator_NId = @TempIndicatorNId AND Data.Unit_NId = @TempUnitNId AND Data.Area_NId = @TempAreaNId
				ORDER BY TP.TimePeriod
			) Tbl
			
			IF (LEN(@TempSGNIds) > 1)
			BEGIN
				SELECT @TempSGNIds = SUBSTRING(@TempSGNIds, 2, LEN(@TempSGNIds) - 2)
				SET @TempSGCount = LEN(@TempSGNIds) - LEN(REPLACE(@TempSGNIds, '','', '''')) + 1
			END
			ELSE IF (LEN(@TempSGNIds) = 1)
			BEGIN
				SELECT @TempSGNIds = ''''
				SET @TempSGCount = 0
			END	

			IF (LEN(@TempSourceNIds) > 1)
			BEGIN
				SELECT @TempSourceNIds = SUBSTRING(@TempSourceNIds, 2, LEN(@TempSourceNIds) - 2)
				SET @TempSourceCount = LEN(@TempSourceNIds) - LEN(REPLACE(@TempSourceNIds, '','', '''')) + 1
			END
			ELSE IF (LEN(@TempSourceNIds) = 1)
			BEGIN
				SELECT @TempSourceNIds = ''''
				SET @TempSourceCount = 0
			END	

			IF (LEN(@TempTPNIds) > 1)
			BEGIN
				SELECT @TempTPNIds = SUBSTRING(@TempTPNIds, 2, LEN(@TempTPNIds) - 2)
				SET @TempTPCount = LEN(@TempTPNIds) - LEN(REPLACE(@TempTPNIds, '','', '''')) + 1
			END
			ELSE IF (LEN(@TempTPNIds) = 1)
			BEGIN
				SELECT @TempTPNIds = ''''
				SET @TempTPCount = 0
			END	

			IF (LEN(@TempDVNIds) > 1)
			BEGIN
				SELECT @TempDVNIds = SUBSTRING(@TempDVNIds, 2, LEN(@TempDVNIds) - 2)
				SET @TempDVCount = LEN(@TempDVNIds) - LEN(REPLACE(@TempDVNIds, '','', '''')) + 1
			END
			ELSE IF (LEN(@TempDVNIds) = 1)
			BEGIN
				SELECT @TempDVNIds = ''''
				SET @TempDVCount = 0
			END	

			IF (LEN(@TempDVSeries) > 1)
			BEGIN
				SELECT @TempDVSeries = SUBSTRING(@TempDVSeries, 2, LEN(@TempDVSeries) - 2)
			END
			ELSE IF (LEN(@TempDVSeries) = 1)
			BEGIN
				SELECT @TempDVSeries = ''''
			END

			IF (LEN(@TempTPSeries) > 1)
			BEGIN
				SELECT @TempTPSeries = SUBSTRING(@TempTPSeries, 2, LEN(@TempTPSeries) - 2)
			END
			ELSE IF (LEN(@TempTPSeries) = 1)
			BEGIN
				SELECT @TempTPSeries = ''''
			END	

			SET @TempDimensions = [dbo].[fn_get_dimensions_IU_XX](@TempIndicatorNId, @TempUnitNId)

			UPDATE #ResultSet
			SET	SGCount = @TempSGCount, SourceCount = @TempSourceCount, TPCount = @TempTPCount, DVCount = @TempDVCount,
				SGNIds = @TempSGNIds, SourceNIds = @TempSourceNIds, TPNIds = @TempTPNIds, DVNIds = @TempDVNIds, DVSeries = @TempTPSeries + '':'' + @TempDVSeries, Dimensions = @TempDimensions,BlockAreaParentNId=NULL
			WHERE IndicatorNId = @TempIndicatorNId AND UnitNId = @TempUnitNId AND AreaNId = @TempAreaNId
		END
		
		INSERT INTO DI_SEARCH_RESULTS
			SELECT @SearchLanguage,
				   IndicatorNid, UnitNId, AreaNId, 1, 
				   Indicator, Unit, Area, 
				   CAST(DefaultSGNId AS NVARCHAR) + ''[@@@@]'' + DefaultSG, MRDTP, MRD, 
				   0, SGCount, SourceCount, TPCount, DVCount, 
				   '''', SGNIds, SourceNIds, TPNIds, DVNIds, DVSeries , Dimensions,BlockAreaParentNId
			FROM #ResultSet R WHERE MRDTP LIKE ''%_%'' and MRD LIKE ''%_%'' AND
			NOT EXISTS (	SELECT TOP 1 NId FROM DI_SEARCH_RESULTS C 
								WHERE C.IndicatorNId = R.IndicatorNid
								AND C.AreaNId = CAST(R.AreaNId AS NVARCHAR(50)) AND SearchLanguage = @SearchLanguage)
			AND DVCount > 0

		DROP TABLE #ResultSet
		DROP TABLE #SearchAreas
		DROP TABLE #SearchIndicators
		DROP TABLE #ICIndicators
	

END




' 
END

--SPSEPARATOR--
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CREATE_CACHE_RESULT_IUA_SPECIFIC_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[SP_CREATE_CACHE_RESULT_IUA_SPECIFIC_XX]
	@SearchIndicator		INT
AS
BEGIN
	DECLARE @SQLSearch					NVARCHAR(MAX);
	DECLARE @FinalSearchAreas			NVARCHAR(MAX);

	DECLARE @Max						INT;
	DECLARE @Count						INT;
	DECLARE @TempIndicatorNId			INT;
	DECLARE @TempUnitNId				INT;
	DECLARE @TempAreaNId				INT;
	DECLARE @TempDefaultSGNId			INT;
	DECLARE @TempSGCount				INT;
	DECLARE @TempSourceCount			INT;
	DECLARE @TempTPCount				INT;
	DECLARE @TempDVCount				INT;
	DECLARE @TempSGNIds					NVARCHAR(MAX);
	DECLARE @TempSourceNIds				NVARCHAR(MAX);
	DECLARE @TempTPNIds					NVARCHAR(MAX);
	DECLARE @TempDVNIds					NVARCHAR(MAX);
	DECLARE @TempDVSeries				NVARCHAR(MAX);
	DECLARE @TempDimensions				NVARCHAR(MAX);
	DECLARE @TempTPSeries				NVARCHAR(MAX);
	DECLARE @SearchLanguage				NVARCHAR(MAX);
	
	SET @SearchLanguage = ''_XX'';
	SELECT @SearchLanguage = SUBSTRING(@SearchLanguage, 2, LEN(@SearchLanguage) - 1)

	
		CREATE TABLE #ResultSet
		(
			Id							INT IDENTITY(1,1) PRIMARY KEY,
			IndicatorNId				INT DEFAULT 0,
			UnitNId						INT DEFAULT 0,
			AreaNId						INT DEFAULT 0,
			Indicator					NVARCHAR(MAX) DEFAULT '''',
			ICName						NVARCHAR(MAX) DEFAULT '''',
			Unit						NVARCHAR(MAX) DEFAULT '''',
			Area						NVARCHAR(MAX) DEFAULT '''',
			DefaultSGNId				INT DEFAULT 0,
			DefaultSG					NVARCHAR(MAX) DEFAULT '''',
			MRDTP						NVARCHAR(MAX) DEFAULT '''',
			MRD							NVARCHAR(MAX) DEFAULT '''',
			SGCount						INT DEFAULT 0,
			SourceCount					INT DEFAULT 0,
			TPCount						INT DEFAULT 0,
			DVCount						INT DEFAULT 0,
			SGNIds						NVARCHAR(MAX) DEFAULT '''',
			SourceNIds					NVARCHAR(MAX) DEFAULT '''' ,
			TPNIds						NVARCHAR(MAX) DEFAULT ''''  ,
			DVNIds						NVARCHAR(MAX) DEFAULT ''''  ,
			DVSeries					NVARCHAR(MAX) DEFAULT ''''  ,
			Dimensions					NVARCHAR(MAX) DEFAULT '''',
			BlockAreaParentNId			INT DEFAULT 0
		)

		CREATE TABLE #SearchAreas 
		(
			Id							INT IDENTITY(1,1) PRIMARY KEY,
			AreaNId						INT DEFAULT 0,
			Area						NVARCHAR(MAX) DEFAULT '''' NOT NULL
		)

		CREATE TABLE #SearchIndicators 
		(
			Id							INT IDENTITY(1,1) PRIMARY KEY,
			IndicatorNId				INT DEFAULT 0,
			Indicator					NVARCHAR(MAX) DEFAULT '''' NOT NULL,
			ICName						NVARCHAR(MAX) DEFAULT '''' NOT NULL
		)

		CREATE TABLE #SearchICs 
		(
			Id							INT IDENTITY(1,1) PRIMARY KEY,
			ICNId						INT DEFAULT 0,
			ICName						NVARCHAR(MAX) DEFAULT '''' NOT NULL
		)

		CREATE TABLE #ICIndicators
		(
			Id							INT IDENTITY(1,1) PRIMARY KEY,
			IndicatorNId				INT DEFAULT 0,
			Indicator					NVARCHAR(MAX) DEFAULT '''' NOT NULL,
			ICName						NVARCHAR(MAX) DEFAULT '''' NOT NULL
		)

		
			--***************************************************************************************************************************************************
			-- Filling Area Table
			--***************************************************************************************************************************************************
			INSERT INTO #SearchAreas (AreaNId, Area)   
			SELECT Area.Area_NId, Area.Area_Name FROM UT_Area_XX Area 
			

		IF (@SearchIndicator IS NOT NULL AND @SearchIndicator <> '''')
		BEGIN
			--***************************************************************************************************************************************************
			-- Filling Indicator Table
			--***************************************************************************************************************************************************
			INSERT INTO #SearchIndicators (IndicatorNId, Indicator)
			SELECT Indicator.Indicator_NId, Indicator.Indicator_Name FROM UT_Indicator_XX Indicator
			WHERE  Indicator.Indicator_NId = @SearchIndicator
			--JOIN dbo.FN_GET_SPLITTED_LIST(@SearchIndicator, '','') List 
			--ON Indicator.Indicator_NId = List.Value
		END

		INSERT INTO #ResultSet (IndicatorNId, UnitNId, AreaNId, Indicator, ICName, Unit, Area)
		SELECT DISTINCT Data.Indicator_NId, Data.Unit_NId, Data.Area_NId, #SearchIndicators.Indicator, #SearchIndicators.ICName, Unit.Unit_Name, #SearchAreas.Area 
		FROM UT_Data Data
		JOIN #SearchIndicators ON Data.Indicator_NId = #SearchIndicators.IndicatorNId
		JOIN UT_Unit_en Unit ON Data.Unit_NId = Unit.Unit_NId 
		JOIN #SearchAreas ON Data.Area_NId = #SearchAreas.AreaNId 
		JOIN dbo.ut_indicator_unit_subgroup ut_IUS ON Data.Indicator_NId = ut_IUS.Indicator_NId AND Data.Unit_NId = ut_IUS.Unit_NId
		WHERE ut_IUS.IsDefaultSubgroup=1

		UPDATE #ResultSet SET DefaultSGNId = IUS.Subgroup_Val_NId , DefaultSG = SGV.Subgroup_Val
		FROM UT_Indicator_Unit_Subgroup IUS, UT_Subgroup_Vals_XX SGV 
		WHERE IndicatorNId = IUS.Indicator_NId AND UnitNId = IUS.Unit_NId AND IUS.IsDefaultSubgroup = 1 
		AND IUS.Subgroup_Val_NId = SGV.Subgroup_Val_NId

		UPDATE #ResultSet 
		SET MRDTP = TP.TimePeriod, MRD = ISNULL(dbo.CustomCast(Data.Data_Value), Data.Textual_Data_Value)
		FROM UT_Data Data, UT_TimePeriod TP 
		WHERE IndicatorNId = Data.Indicator_NId AND UnitNId = Data.Unit_NId AND DefaultSGNId = Data.Subgroup_Val_NId 
			  AND AreaNId = Data.Area_NId AND Data.isMRD = 1 AND Data.TimePeriod_NId = TP.TimePeriod_NId
		

		SELECT @Count = 0, @Max = COUNT(*) FROM #ResultSet
		WHILE(@Count < @Max)
		BEGIN
			SET @Count = @Count + 1;
			
			SELECT @TempIndicatorNId = IndicatorNId, @TempUnitNId = UnitNId, @TempAreaNId = AreaNId, @TempDefaultSGNId = DefaultSGNId, 
				   @TempSGCount = 0, @TempSourceCount = 0, @TempTPCount = 0, @TempDVCount = 0,
				   @TempSGNIds = '','', @TempSourceNIds = '','', @TempTPNIds = '','', @TempDVNIds = '','', @TempDVSeries = '','', @TempTPSeries = '',''
			FROM #ResultSet WHERE Id = @Count

			SELECT 
				@TempSGNIds = 
				CASE 
					WHEN (CHARINDEX( '','' + CAST(Tbl.Subgroup_Val_NId AS NVARCHAR(MAX))+ '','', @TempSGNIds) = 0) 
					THEN 
						@TempSGNIds + CAST(Tbl.Subgroup_Val_NId AS NVARCHAR(MAX)) + '',''
					ELSE
						@TempSGNIds
				END ,
				@TempSourceNIds = 
				CASE 
					WHEN (CHARINDEX( '','' + CAST(Tbl.Source_NId AS NVARCHAR(MAX))+ '','', @TempSourceNIds) = 0) 
					THEN 
						@TempSourceNIds + CAST(Tbl.Source_NId AS NVARCHAR(MAX)) + '',''
					ELSE
						@TempSourceNIds
				END ,
				@TempTPNIds = 
				CASE 
					WHEN (CHARINDEX( '','' + CAST(Tbl.TimePeriod_NId AS VARCHAR(MAX))+ '','', @TempTPNIds) = 0) 
					THEN 
						@TempTPNIds + CAST(Tbl.TimePeriod_NId AS NVARCHAR(MAX)) + '',''
					ELSE
						@TempTPNIds
				END ,
				@TempDVNIds = 
				CASE 
					WHEN (CHARINDEX( '','' + CAST(Tbl.Data_NId AS NVARCHAR(MAX))+ '','', @TempDVNIds) = 0) 
					THEN 
						@TempDVNIds + CAST(Tbl.Data_NId AS NVARCHAR(MAX)) + '',''
					ELSE
						@TempDVNIds
				END ,
				@TempDVSeries = 
				CASE 
					WHEN (Tbl.Subgroup_Val_NId = @TempDefaultSGNId) 
					THEN 
						@TempDVSeries + ISNULL(dbo.CustomCast(Tbl.Data_Value), Tbl.Textual_Data_Value) + ''|''
					ELSE
						@TempDVSeries
				END ,
				@TempTPSeries = 
				CASE 
					WHEN (Tbl.Subgroup_Val_NId = @TempDefaultSGNId) 
					THEN 
						@TempTPSeries + Tbl.TimePeriod + ''|''
					ELSE
						@TempTPSeries
				END 
			FROM 
			(
				SELECT DISTINCT TOP 2147483647 Data.Data_NId, Data.Data_Value, Data.Textual_Data_Value, Data.Subgroup_Val_NId, Data.Source_NId, Data.TimePeriod_NId, TP.TimePeriod
				FROM UT_DATA Data JOIN UT_TimePeriod TP ON Data.TimePeriod_NId = TP.TimePeriod_NId 
				WHERE Data.Indicator_NId = @TempIndicatorNId AND Data.Unit_NId = @TempUnitNId AND Data.Area_NId = @TempAreaNId
				AND Data.Subgroup_Val_NId = @TempDefaultSGNId
				ORDER BY TP.TimePeriod
			) Tbl
			
			IF (LEN(@TempSGNIds) > 1)
			BEGIN
				SELECT @TempSGNIds = SUBSTRING(@TempSGNIds, 2, LEN(@TempSGNIds) - 2)
				SET @TempSGCount = LEN(@TempSGNIds) - LEN(REPLACE(@TempSGNIds, '','', '''')) + 1
			END
			ELSE IF (LEN(@TempSGNIds) = 1)
			BEGIN
				SELECT @TempSGNIds = ''''
				SET @TempSGCount = 0
			END	

			IF (LEN(@TempSourceNIds) > 1)
			BEGIN
				SELECT @TempSourceNIds = SUBSTRING(@TempSourceNIds, 2, LEN(@TempSourceNIds) - 2)
				SET @TempSourceCount = LEN(@TempSourceNIds) - LEN(REPLACE(@TempSourceNIds, '','', '''')) + 1
			END
			ELSE IF (LEN(@TempSourceNIds) = 1)
			BEGIN
				SELECT @TempSourceNIds = ''''
				SET @TempSourceCount = 0
			END	

			IF (LEN(@TempTPNIds) > 1)
			BEGIN
				SELECT @TempTPNIds = SUBSTRING(@TempTPNIds, 2, LEN(@TempTPNIds) - 2)
				SET @TempTPCount = LEN(@TempTPNIds) - LEN(REPLACE(@TempTPNIds, '','', '''')) + 1
			END
			ELSE IF (LEN(@TempTPNIds) = 1)
			BEGIN
				SELECT @TempTPNIds = ''''
				SET @TempTPCount = 0
			END	

			IF (LEN(@TempDVNIds) > 1)
			BEGIN
				SELECT @TempDVNIds = SUBSTRING(@TempDVNIds, 2, LEN(@TempDVNIds) - 2)
				SET @TempDVCount = LEN(@TempDVNIds) - LEN(REPLACE(@TempDVNIds, '','', '''')) + 1
			END
			ELSE IF (LEN(@TempDVNIds) = 1)
			BEGIN
				SELECT @TempDVNIds = ''''
				SET @TempDVCount = 0
			END	

			IF (LEN(@TempDVSeries) > 1)
			BEGIN
				SELECT @TempDVSeries = SUBSTRING(@TempDVSeries, 2, LEN(@TempDVSeries) - 2)
			END
			ELSE IF (LEN(@TempDVSeries) = 1)
			BEGIN
				SELECT @TempDVSeries = ''''
			END

			IF (LEN(@TempTPSeries) > 1)
			BEGIN
				SELECT @TempTPSeries = SUBSTRING(@TempTPSeries, 2, LEN(@TempTPSeries) - 2)
			END
			ELSE IF (LEN(@TempTPSeries) = 1)
			BEGIN
				SELECT @TempTPSeries = ''''
			END	

			SET @TempDimensions = [dbo].[fn_get_dimensions_IU_XX](@TempIndicatorNId, @TempUnitNId)

			UPDATE #ResultSet
			SET	SGCount = @TempSGCount, SourceCount = @TempSourceCount, TPCount = @TempTPCount, DVCount = @TempDVCount,
				SGNIds = @TempSGNIds, SourceNIds = @TempSourceNIds, TPNIds = @TempTPNIds, DVNIds = @TempDVNIds, DVSeries = @TempTPSeries + '':'' + @TempDVSeries, Dimensions = @TempDimensions,BlockAreaParentNId=NULL
			WHERE IndicatorNId = @TempIndicatorNId AND UnitNId = @TempUnitNId AND AreaNId = @TempAreaNId
		END
		
		INSERT INTO DI_SEARCH_RESULTS
			SELECT @SearchLanguage,
				   IndicatorNid, UnitNId, AreaNId, 1, 
				   Indicator, Unit, Area, 
				   CAST(DefaultSGNId AS NVARCHAR) + ''[@@@@]'' + DefaultSG, MRDTP, MRD, 
				   0, SGCount, SourceCount, TPCount, DVCount, 
				   '''', SGNIds, SourceNIds, TPNIds, DVNIds, DVSeries , Dimensions,BlockAreaParentNId
			FROM #ResultSet R WHERE MRDTP LIKE ''%_%'' AND MRD LIKE ''%_%'' AND
			NOT EXISTS (	SELECT TOP 1 NId FROM DI_SEARCH_RESULTS C 
								WHERE C.IndicatorNId = R.IndicatorNid
								AND C.AreaNId = CAST(R.AreaNId AS NVARCHAR(50)) AND SearchLanguage = @SearchLanguage)
			AND DVCount > 0

		DROP TABLE #ResultSet
		DROP TABLE #SearchAreas
		DROP TABLE #SearchIndicators
		DROP TABLE #ICIndicators
	

END




' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'

ALTER PROCEDURE [dbo].[SP_CREATE_CACHE_RESULT_IUA_SPECIFIC_XX]
	@SearchIndicator		INT
AS
BEGIN
	DECLARE @SQLSearch					NVARCHAR(MAX);
	DECLARE @FinalSearchAreas			NVARCHAR(MAX);

	DECLARE @Max						INT;
	DECLARE @Count						INT;
	DECLARE @TempIndicatorNId			INT;
	DECLARE @TempUnitNId				INT;
	DECLARE @TempAreaNId				INT;
	DECLARE @TempDefaultSGNId			INT;
	DECLARE @TempSGCount				INT;
	DECLARE @TempSourceCount			INT;
	DECLARE @TempTPCount				INT;
	DECLARE @TempDVCount				INT;
	DECLARE @TempSGNIds					NVARCHAR(MAX);
	DECLARE @TempSourceNIds				NVARCHAR(MAX);
	DECLARE @TempTPNIds					NVARCHAR(MAX);
	DECLARE @TempDVNIds					NVARCHAR(MAX);
	DECLARE @TempDVSeries				NVARCHAR(MAX);
	DECLARE @TempDimensions				NVARCHAR(MAX);
	DECLARE @TempTPSeries				NVARCHAR(MAX);
	DECLARE @SearchLanguage				NVARCHAR(MAX);
	
	SET @SearchLanguage = ''_XX'';
	SELECT @SearchLanguage = SUBSTRING(@SearchLanguage, 2, LEN(@SearchLanguage) - 1)

	
		CREATE TABLE #ResultSet
		(
			Id							INT IDENTITY(1,1) PRIMARY KEY,
			IndicatorNId				INT DEFAULT 0,
			UnitNId						INT DEFAULT 0,
			AreaNId						INT DEFAULT 0,
			Indicator					NVARCHAR(MAX) DEFAULT '''',
			ICName						NVARCHAR(MAX) DEFAULT '''',
			Unit						NVARCHAR(MAX) DEFAULT '''',
			Area						NVARCHAR(MAX) DEFAULT '''',
			DefaultSGNId				INT DEFAULT 0,
			DefaultSG					NVARCHAR(MAX) DEFAULT '''',
			MRDTP						NVARCHAR(MAX) DEFAULT '''',
			MRD							NVARCHAR(MAX) DEFAULT '''',
			SGCount						INT DEFAULT 0,
			SourceCount					INT DEFAULT 0,
			TPCount						INT DEFAULT 0,
			DVCount						INT DEFAULT 0,
			SGNIds						NVARCHAR(MAX) DEFAULT '''',
			SourceNIds					NVARCHAR(MAX) DEFAULT '''' ,
			TPNIds						NVARCHAR(MAX) DEFAULT ''''  ,
			DVNIds						NVARCHAR(MAX) DEFAULT ''''  ,
			DVSeries					NVARCHAR(MAX) DEFAULT ''''  ,
			Dimensions					NVARCHAR(MAX) DEFAULT '''',
			BlockAreaParentNId			INT DEFAULT 0
		)

		CREATE TABLE #SearchAreas 
		(
			Id							INT IDENTITY(1,1) PRIMARY KEY,
			AreaNId						INT DEFAULT 0,
			Area						NVARCHAR(MAX) DEFAULT '''' NOT NULL
		)

		CREATE TABLE #SearchIndicators 
		(
			Id							INT IDENTITY(1,1) PRIMARY KEY,
			IndicatorNId				INT DEFAULT 0,
			Indicator					NVARCHAR(MAX) DEFAULT '''' NOT NULL,
			ICName						NVARCHAR(MAX) DEFAULT '''' NOT NULL
		)

		CREATE TABLE #SearchICs 
		(
			Id							INT IDENTITY(1,1) PRIMARY KEY,
			ICNId						INT DEFAULT 0,
			ICName						NVARCHAR(MAX) DEFAULT '''' NOT NULL
		)

		CREATE TABLE #ICIndicators
		(
			Id							INT IDENTITY(1,1) PRIMARY KEY,
			IndicatorNId				INT DEFAULT 0,
			Indicator					NVARCHAR(MAX) DEFAULT '''' NOT NULL,
			ICName						NVARCHAR(MAX) DEFAULT '''' NOT NULL
		)

		
			--***************************************************************************************************************************************************
			-- Filling Area Table
			--***************************************************************************************************************************************************
			INSERT INTO #SearchAreas (AreaNId, Area)   
			SELECT Area.Area_NId, Area.Area_Name FROM UT_Area_XX Area 
			

		IF (@SearchIndicator IS NOT NULL AND @SearchIndicator <> '''')
		BEGIN
			--***************************************************************************************************************************************************
			-- Filling Indicator Table
			--***************************************************************************************************************************************************
			INSERT INTO #SearchIndicators (IndicatorNId, Indicator)
			SELECT Indicator.Indicator_NId, Indicator.Indicator_Name FROM UT_Indicator_XX Indicator
			WHERE  Indicator.Indicator_NId = @SearchIndicator
			--JOIN dbo.FN_GET_SPLITTED_LIST(@SearchIndicator, '','') List 
			--ON Indicator.Indicator_NId = List.Value
		END

		INSERT INTO #ResultSet (IndicatorNId, UnitNId, AreaNId, Indicator, ICName, Unit, Area)
		SELECT DISTINCT Data.Indicator_NId, Data.Unit_NId, Data.Area_NId, #SearchIndicators.Indicator, #SearchIndicators.ICName, Unit.Unit_Name, #SearchAreas.Area 
		FROM UT_Data Data
		JOIN #SearchIndicators ON Data.Indicator_NId = #SearchIndicators.IndicatorNId
		JOIN UT_Unit_en Unit ON Data.Unit_NId = Unit.Unit_NId 
		JOIN #SearchAreas ON Data.Area_NId = #SearchAreas.AreaNId 
		JOIN dbo.ut_indicator_unit_subgroup ut_IUS ON Data.Indicator_NId = ut_IUS.Indicator_NId AND Data.Unit_NId = ut_IUS.Unit_NId
		WHERE ut_IUS.IsDefaultSubgroup=1

		UPDATE #ResultSet SET DefaultSGNId = IUS.Subgroup_Val_NId , DefaultSG = SGV.Subgroup_Val
		FROM UT_Indicator_Unit_Subgroup IUS, UT_Subgroup_Vals_XX SGV 
		WHERE IndicatorNId = IUS.Indicator_NId AND UnitNId = IUS.Unit_NId AND IUS.IsDefaultSubgroup = 1 
		AND IUS.Subgroup_Val_NId = SGV.Subgroup_Val_NId

		UPDATE #ResultSet 
		SET MRDTP = TP.TimePeriod, MRD = ISNULL(dbo.CustomCast(Data.Data_Value), Data.Textual_Data_Value)
		FROM UT_Data Data, UT_TimePeriod TP 
		WHERE IndicatorNId = Data.Indicator_NId AND UnitNId = Data.Unit_NId AND DefaultSGNId = Data.Subgroup_Val_NId 
			  AND AreaNId = Data.Area_NId AND Data.isMRD = 1 AND Data.TimePeriod_NId = TP.TimePeriod_NId
		

		SELECT @Count = 0, @Max = COUNT(*) FROM #ResultSet
		WHILE(@Count < @Max)
		BEGIN
			SET @Count = @Count + 1;
			
			SELECT @TempIndicatorNId = IndicatorNId, @TempUnitNId = UnitNId, @TempAreaNId = AreaNId, @TempDefaultSGNId = DefaultSGNId, 
				   @TempSGCount = 0, @TempSourceCount = 0, @TempTPCount = 0, @TempDVCount = 0,
				   @TempSGNIds = '','', @TempSourceNIds = '','', @TempTPNIds = '','', @TempDVNIds = '','', @TempDVSeries = '','', @TempTPSeries = '',''
			FROM #ResultSet WHERE Id = @Count

			SELECT 
				@TempSGNIds = 
				CASE 
					WHEN (CHARINDEX( '','' + CAST(Tbl.Subgroup_Val_NId AS NVARCHAR(MAX))+ '','', @TempSGNIds) = 0) 
					THEN 
						@TempSGNIds + CAST(Tbl.Subgroup_Val_NId AS NVARCHAR(MAX)) + '',''
					ELSE
						@TempSGNIds
				END ,
				@TempSourceNIds = 
				CASE 
					WHEN (CHARINDEX( '','' + CAST(Tbl.Source_NId AS NVARCHAR(MAX))+ '','', @TempSourceNIds) = 0) 
					THEN 
						@TempSourceNIds + CAST(Tbl.Source_NId AS NVARCHAR(MAX)) + '',''
					ELSE
						@TempSourceNIds
				END ,
				@TempTPNIds = 
				CASE 
					WHEN (CHARINDEX( '','' + CAST(Tbl.TimePeriod_NId AS VARCHAR(MAX))+ '','', @TempTPNIds) = 0) 
					THEN 
						@TempTPNIds + CAST(Tbl.TimePeriod_NId AS NVARCHAR(MAX)) + '',''
					ELSE
						@TempTPNIds
				END ,
				@TempDVNIds = 
				CASE 
					WHEN (CHARINDEX( '','' + CAST(Tbl.Data_NId AS NVARCHAR(MAX))+ '','', @TempDVNIds) = 0) 
					THEN 
						@TempDVNIds + CAST(Tbl.Data_NId AS NVARCHAR(MAX)) + '',''
					ELSE
						@TempDVNIds
				END ,
				@TempDVSeries = 
				CASE 
					WHEN (Tbl.Subgroup_Val_NId = @TempDefaultSGNId) 
					THEN 
						@TempDVSeries + ISNULL(dbo.CustomCast(Tbl.Data_Value), Tbl.Textual_Data_Value) + ''|''
					ELSE
						@TempDVSeries
				END ,
				@TempTPSeries = 
				CASE 
					WHEN (Tbl.Subgroup_Val_NId = @TempDefaultSGNId) 
					THEN 
						@TempTPSeries + Tbl.TimePeriod + ''|''
					ELSE
						@TempTPSeries
				END 
			FROM 
			(
				SELECT DISTINCT TOP 2147483647 Data.Data_NId, Data.Data_Value, Data.Textual_Data_Value, Data.Subgroup_Val_NId, Data.Source_NId, Data.TimePeriod_NId, TP.TimePeriod
				FROM UT_DATA Data JOIN UT_TimePeriod TP ON Data.TimePeriod_NId = TP.TimePeriod_NId 
				WHERE Data.Indicator_NId = @TempIndicatorNId AND Data.Unit_NId = @TempUnitNId AND Data.Area_NId = @TempAreaNId
				AND Data.Subgroup_Val_NId = @TempDefaultSGNId
				ORDER BY TP.TimePeriod
			) Tbl
			
			IF (LEN(@TempSGNIds) > 1)
			BEGIN
				SELECT @TempSGNIds = SUBSTRING(@TempSGNIds, 2, LEN(@TempSGNIds) - 2)
				SET @TempSGCount = LEN(@TempSGNIds) - LEN(REPLACE(@TempSGNIds, '','', '''')) + 1
			END
			ELSE IF (LEN(@TempSGNIds) = 1)
			BEGIN
				SELECT @TempSGNIds = ''''
				SET @TempSGCount = 0
			END	

			IF (LEN(@TempSourceNIds) > 1)
			BEGIN
				SELECT @TempSourceNIds = SUBSTRING(@TempSourceNIds, 2, LEN(@TempSourceNIds) - 2)
				SET @TempSourceCount = LEN(@TempSourceNIds) - LEN(REPLACE(@TempSourceNIds, '','', '''')) + 1
			END
			ELSE IF (LEN(@TempSourceNIds) = 1)
			BEGIN
				SELECT @TempSourceNIds = ''''
				SET @TempSourceCount = 0
			END	

			IF (LEN(@TempTPNIds) > 1)
			BEGIN
				SELECT @TempTPNIds = SUBSTRING(@TempTPNIds, 2, LEN(@TempTPNIds) - 2)
				SET @TempTPCount = LEN(@TempTPNIds) - LEN(REPLACE(@TempTPNIds, '','', '''')) + 1
			END
			ELSE IF (LEN(@TempTPNIds) = 1)
			BEGIN
				SELECT @TempTPNIds = ''''
				SET @TempTPCount = 0
			END	

			IF (LEN(@TempDVNIds) > 1)
			BEGIN
				SELECT @TempDVNIds = SUBSTRING(@TempDVNIds, 2, LEN(@TempDVNIds) - 2)
				SET @TempDVCount = LEN(@TempDVNIds) - LEN(REPLACE(@TempDVNIds, '','', '''')) + 1
			END
			ELSE IF (LEN(@TempDVNIds) = 1)
			BEGIN
				SELECT @TempDVNIds = ''''
				SET @TempDVCount = 0
			END	

			IF (LEN(@TempDVSeries) > 1)
			BEGIN
				SELECT @TempDVSeries = SUBSTRING(@TempDVSeries, 2, LEN(@TempDVSeries) - 2)
			END
			ELSE IF (LEN(@TempDVSeries) = 1)
			BEGIN
				SELECT @TempDVSeries = ''''
			END

			IF (LEN(@TempTPSeries) > 1)
			BEGIN
				SELECT @TempTPSeries = SUBSTRING(@TempTPSeries, 2, LEN(@TempTPSeries) - 2)
			END
			ELSE IF (LEN(@TempTPSeries) = 1)
			BEGIN
				SELECT @TempTPSeries = ''''
			END	

			SET @TempDimensions = [dbo].[fn_get_dimensions_IU_XX](@TempIndicatorNId, @TempUnitNId)

			UPDATE #ResultSet
			SET	SGCount = @TempSGCount, SourceCount = @TempSourceCount, TPCount = @TempTPCount, DVCount = @TempDVCount,
				SGNIds = @TempSGNIds, SourceNIds = @TempSourceNIds, TPNIds = @TempTPNIds, DVNIds = @TempDVNIds, DVSeries = @TempTPSeries + '':'' + @TempDVSeries, Dimensions = @TempDimensions,BlockAreaParentNId=NULL
			WHERE IndicatorNId = @TempIndicatorNId AND UnitNId = @TempUnitNId AND AreaNId = @TempAreaNId
		END
		
		INSERT INTO DI_SEARCH_RESULTS
			SELECT @SearchLanguage,
				   IndicatorNid, UnitNId, AreaNId, 1, 
				   Indicator, Unit, Area, 
				   CAST(DefaultSGNId AS NVARCHAR) + ''[@@@@]'' + DefaultSG, MRDTP, MRD, 
				   0, SGCount, SourceCount, TPCount, DVCount, 
				   '''', SGNIds, SourceNIds, TPNIds, DVNIds, DVSeries , Dimensions,BlockAreaParentNId
			FROM #ResultSet R WHERE MRDTP LIKE ''%_%'' and MRD LIKE ''%_%'' AND
			NOT EXISTS (	SELECT TOP 1 NId FROM DI_SEARCH_RESULTS C 
								WHERE C.IndicatorNId = R.IndicatorNid
								AND C.AreaNId = CAST(R.AreaNId AS NVARCHAR(50)) AND SearchLanguage = @SearchLanguage)
			AND DVCount > 0

		DROP TABLE #ResultSet
		DROP TABLE #SearchAreas
		DROP TABLE #SearchIndicators
		DROP TABLE #ICIndicators
	

END




' 
END

--SPSEPARATOR--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CREATE_CACHE_RESULT_IUA_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[SP_CREATE_CACHE_RESULT_IUA_XX]
	@SearchIndicator		INT,
	@AdditionalParams		VARCHAR(100)
AS
BEGIN
IF(@AdditionalParams = ''GENERAL'')
	BEGIN
		EXEC SP_CREATE_CACHE_RESULT_IUA_GENERAL_XX @SearchIndicator
	END
	ELSE IF(@AdditionalParams = ''SPECIFIC'')
	BEGIN
		EXEC SP_CREATE_CACHE_RESULT_IUA_SPECIFIC_XX @SearchIndicator
	END
END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'

ALTER PROCEDURE [dbo].[SP_CREATE_CACHE_RESULT_IUA_XX]
		@SearchIndicator		INT,
	@AdditionalParams		VARCHAR(100)
AS
BEGIN
IF(@AdditionalParams = ''GENERAL'')
	BEGIN
		EXEC SP_CREATE_CACHE_RESULT_IUA_GENERAL_XX @SearchIndicator
	END
	ELSE IF(@AdditionalParams = ''SPECIFIC'')
	BEGIN
		EXEC SP_CREATE_CACHE_RESULT_IUA_SPECIFIC_XX @SearchIndicator
	END
END
' 
END

--SPSEPARATOR--

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CREATE_CACHE_RESULT_IU_GENERAL_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[SP_CREATE_CACHE_RESULT_IU_GENERAL_XX]
	@SearchArea			NVARCHAR(MAX)
AS
BEGIN
	DECLARE @SQLSearch					NVARCHAR(MAX);

	DECLARE @Max						INT;
	DECLARE @Count						INT;
	DECLARE @TempIndicatorNId			INT;
	DECLARE @TempUnitNId				INT;
	DECLARE @TempDefaultSGNId			INT;
	DECLARE @TempAreaCount				INT;
	DECLARE @TempSGCount				INT;
	DECLARE @TempSourceCount			INT;
	DECLARE @TempTPCount				INT;
	DECLARE @TempDVCount				INT;
	DECLARE @TempAreaNIds				NVARCHAR(MAX);
	DECLARE @TempSGNIds					NVARCHAR(MAX);
	DECLARE @TempSourceNIds				NVARCHAR(MAX);
	DECLARE @TempTPNIds					NVARCHAR(MAX);
	DECLARE @TempDVNIds					NVARCHAR(MAX);
	DECLARE @TempDVSeries				NVARCHAR(MAX);
	DECLARE @TempDimensions				NVARCHAR(MAX);
	DECLARE @TempAreaSeries				NVARCHAR(MAX);
	DECLARE @SearchLanguage				NVARCHAR(MAX);
	DECLARE @SearchAreasCopy			NVARCHAR(MAX);
	DECLARE @BlockAreaParentNID			INT ;
	DECLARE @IsAreaNumeric				bit;


	SET @SearchLanguage = ''_XX'';
	SELECT @SearchLanguage = SUBSTRING(@SearchLanguage, 2, LEN(@SearchLanguage) - 1)
		

		SET @SearchAreasCopy = @SearchArea
		SET @IsAreaNumeric = 0
		SELECT @SearchArea = dbo.fn_get_normalized_area_nids_XX(@SearchArea)
		SELECT @BlockAreaParentNID = dbo.FN_GET_AreaNIdGeneric_XX(@SearchAreasCopy)
				CREATE TABLE #ResultSet
				(
					Id							INT IDENTITY(1,1) PRIMARY KEY,
					IndicatorNId				INT DEFAULT 0,
					UnitNId						INT DEFAULT 0,
					Indicator					NVARCHAR(MAX) DEFAULT ''''  ,
					ICName						NVARCHAR(MAX) DEFAULT ''''  ,
					Unit						NVARCHAR(MAX) DEFAULT ''''  ,
					DefaultSGNId				INT DEFAULT 0,
					DefaultSG					NVARCHAR(MAX) DEFAULT ''''  ,
					AreaCount					INT DEFAULT 0,
					SGCount						INT DEFAULT 0,
					SourceCount					INT DEFAULT 0,
					TPCount						INT DEFAULT 0,
					DVCount						INT DEFAULT 0,
					AreaNIds					NVARCHAR(MAX) DEFAULT ''''  ,
					SGNIds						NVARCHAR(MAX) DEFAULT ''''  ,
					SourceNIds					NVARCHAR(MAX) DEFAULT ''''  ,
					TPNIds						NVARCHAR(MAX) DEFAULT ''''  ,
					DVNIds						NVARCHAR(MAX) DEFAULT ''''  ,
					DVSeries					NVARCHAR(MAX) DEFAULT ''''	,
					Dimensions					NVARCHAR(MAX) DEFAULT ''''	,
					BlockAreaParentNId			INT DEFAULT 0
				)

				CREATE TABLE #SearchAreas 
				(
					Id							INT IDENTITY(1,1) PRIMARY KEY,
					AreaNId						INT DEFAULT 0,
					Area						NVARCHAR(MAX) DEFAULT '''' NOT NULL
				)

				CREATE TABLE #SearchIndicators 
				(
					Id							INT IDENTITY(1,1) PRIMARY KEY,
					IndicatorNId				INT DEFAULT 0,
					Indicator					NVARCHAR(MAX) DEFAULT '''' NOT NULL,
					ICName						NVARCHAR(MAX) DEFAULT '''' NOT NULL
				)

				IF (@SearchArea IS NOT NULL AND @SearchArea <> '''')
				BEGIN
					--***************************************************************************************************************************************************
					-- Filling Area Table
					--***************************************************************************************************************************************************
					INSERT INTO #SearchAreas (AreaNId, Area)   
					SELECT Area.Area_NId, Area.Area_Name FROM UT_Area_XX Area 
					JOIN dbo.FN_GET_SPLITTED_LIST(@SearchArea, '','') List 
					ON Area.Area_NId = List.Value
				END

				
					--***************************************************************************************************************************************************
					-- Filling Indicator Table
					--***************************************************************************************************************************************************
				INSERT INTO #SearchIndicators (IndicatorNId, Indicator)
				SELECT Indicator.Indicator_NId, Indicator.Indicator_Name FROM UT_Indicator_XX Indicator 
						

				INSERT INTO #ResultSet (IndicatorNId, UnitNId, Indicator, ICName, Unit)
				SELECT DISTINCT Data.Indicator_NId, Data.Unit_NId, #SearchIndicators.Indicator, #SearchIndicators.ICName, Unit.Unit_Name 
				FROM UT_Data Data
				JOIN #SearchIndicators ON Data.Indicator_NId = #SearchIndicators.IndicatorNId 
				JOIN UT_Unit_XX Unit ON Data.Unit_NId = Unit.Unit_NId 
				JOIN #SearchAreas ON Data.Area_NId = #SearchAreas.AreaNId 
				ORDER BY Data.Indicator_NId, Data.Unit_NId

				UPDATE #ResultSet SET DefaultSGNId = IUS.Subgroup_Val_NId , DefaultSG = SGV.Subgroup_Val
				FROM UT_Indicator_Unit_Subgroup IUS, UT_Subgroup_Vals_XX SGV 
				WHERE IndicatorNId = IUS.Indicator_NId AND UnitNId = IUS.Unit_NId 
				AND IUS.IsDefaultSubgroup = 1 AND IUS.Subgroup_Val_NId = SGV.Subgroup_Val_NId

				SELECT @Count = 0, @Max = COUNT(*) FROM #ResultSet			
			
				WHILE(@Count < @Max)
				BEGIN
					SET @Count = @Count + 1;
					
					SELECT @TempIndicatorNId = IndicatorNId, @TempUnitNId = UnitNId, @TempDefaultSGNId = DefaultSGNId,  
					@TempAreaCount = 0, @TempSGCount = 0, @TempSourceCount = 0, @TempTPCount = 0, @TempDVCount = 0,
					@TempAreaNIds = '','', @TempSGNIds = '','', @TempSourceNIds = '','', @TempTPNIds = '','', @TempDVNIds = '','', 
					@TempDVSeries = '','', @TempAreaSeries = '',''
					FROM #ResultSet WHERE Id = @Count

					SELECT 
						@TempAreaNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Area_NId AS NVARCHAR(10))+ '','', @TempAreaNIds) = 0) 
							THEN 
								@TempAreaNIds + CAST(Tbl.Area_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempAreaNIds
						END ,
						@TempSGNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Subgroup_Val_NId AS NVARCHAR(10))+ '','', @TempSGNIds) = 0) 
							THEN 
								@TempSGNIds + CAST(Tbl.Subgroup_Val_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempSGNIds
						END ,
						@TempSourceNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Source_NId AS NVARCHAR(10))+ '','', @TempSourceNIds) = 0) 
							THEN 
								@TempSourceNIds + CAST(Tbl.Source_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempSourceNIds
						END ,
						@TempTPNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.TimePeriod_NId AS NVARCHAR(10))+ '','', @TempTPNIds) = 0) 
							THEN 
								@TempTPNIds + CAST(Tbl.TimePeriod_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempTPNIds
						END ,
						@TempDVNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Data_NId AS NVARCHAR(10))+ '','', @TempDVNIds) = 0) 
							THEN 
								@TempDVNIds + CAST(Tbl.Data_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempDVNIds
						END 
						,
						@TempDVSeries = 
						CASE 
							WHEN (Tbl.Subgroup_Val_NId = @TempDefaultSGNId AND Tbl.isMRD = 1) 
							THEN 
								@TempDVSeries + ISNULL(dbo.CustomCast(Tbl.Data_Value), Tbl.Textual_Data_Value) + ''|''
							ELSE
								@TempDVSeries
						END ,
						@TempAreaSeries = 
						CASE 
							WHEN (Tbl.Subgroup_Val_NId = @TempDefaultSGNId AND Tbl.isMRD = 1) 
							THEN 
								@TempAreaSeries + Tbl.Area + ''|''
							ELSE
								@TempAreaSeries
						END 
					FROM 
					(
						SELECT TOP 2147483647 Data.Data_NId, Data.Textual_Data_Value, Data.Data_Value, Data.Subgroup_Val_NId, Data.Source_NId, Data.TimePeriod_NId, Data.Area_NId, Data.isMRD, Areas.Area 
						FROM UT_DATA Data JOIN #SearchAreas Areas ON Data.Area_NId = Areas.AreaNId
						WHERE Data.Indicator_NId = @TempIndicatorNId AND Data.Unit_NId = @TempUnitNId 						
					) Tbl 
					
					IF (LEN(@TempAreaNIds) > 1)
					BEGIN
						SELECT @TempAreaNIds = SUBSTRING(@TempAreaNIds, 2, LEN(@TempAreaNIds) - 2)
						SET @TempAreaCount = LEN(@TempAreaNIds) - LEN(REPLACE(@TempAreaNIds, '','', '''')) + 1
					END	
					ELSE IF (LEN(@TempAreaNIds) = 1)
					BEGIN
						SELECT @TempAreaNIds = ''''
						SET @TempAreaCount = 0
					END	
				
					IF (LEN(@TempSGNIds) > 1)
					BEGIN
						SELECT @TempSGNIds = SUBSTRING(@TempSGNIds, 2, LEN(@TempSGNIds) - 2)
						SET @TempSGCount = LEN(@TempSGNIds) - LEN(REPLACE(@TempSGNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempSGNIds) = 1)
					BEGIN
						SELECT @TempSGNIds = ''''
						SET @TempSGCount = 0
					END	

					IF (LEN(@TempSourceNIds) > 1)
					BEGIN
						SELECT @TempSourceNIds = SUBSTRING(@TempSourceNIds, 2, LEN(@TempSourceNIds) - 2)
						SET @TempSourceCount = LEN(@TempSourceNIds) - LEN(REPLACE(@TempSourceNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempSourceNIds) = 1)
					BEGIN
						SELECT @TempSourceNIds = ''''
						SET @TempSourceCount = 0
					END	

					IF (LEN(@TempTPNIds) > 1)
					BEGIN
						SELECT @TempTPNIds = SUBSTRING(@TempTPNIds, 2, LEN(@TempTPNIds) - 2)
						SET @TempTPCount = LEN(@TempTPNIds) - LEN(REPLACE(@TempTPNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempTPNIds) = 1)
					BEGIN
						SELECT @TempTPNIds = ''''
						SET @TempTPCount = 0
					END	

					IF (LEN(@TempDVNIds) > 1)
					BEGIN
						SELECT @TempDVNIds = SUBSTRING(@TempDVNIds, 2, LEN(@TempDVNIds) - 2)
						SET @TempDVCount = LEN(@TempDVNIds) - LEN(REPLACE(@TempDVNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempDVNIds) = 1)
					BEGIN
						SELECT @TempDVNIds = ''''
						SET @TempDVCount = 0
					END	

					IF (LEN(@TempDVSeries) > 1)
					BEGIN
						SELECT @TempDVSeries = SUBSTRING(@TempDVSeries, 2, LEN(@TempDVSeries) - 2)
					END
					ELSE IF (LEN(@TempDVSeries) = 1)
					BEGIN
						SELECT @TempDVSeries = ''''
					END

					IF (LEN(@TempAreaSeries) > 1)
					BEGIN
						SELECT @TempAreaSeries = SUBSTRING(@TempAreaSeries, 2, LEN(@TempAreaSeries) - 2)
					END
					ELSE IF (LEN(@TempAreaSeries) = 1)
					BEGIN
						SELECT @TempAreaSeries = ''''
					END	

					SET @TempDimensions = [dbo].[fn_get_dimensions_IU_XX](@TempIndicatorNId, @TempUnitNId)

					UPDATE #ResultSet
					SET	AreaCount = @TempAreaCount, SGCount = @TempSGCount, SourceCount = @TempSourceCount, TPCount = @TempTPCount, DVCount = @TempDVCount,
						AreaNIds = @TempAreaNIds, SGNIds = @TempSGNIds, SourceNIds = @TempSourceNIds, TPNIds = @TempTPNIds, DVNIds = @TempDVNIds, 
						DVSeries = @TempAreaSeries + '':'' + @TempDVSeries, Dimensions = @TempDimensions,BlockAreaParentNId=@BlockAreaParentNID
					WHERE IndicatorNId = @TempIndicatorNId AND UnitNId = @TempUnitNId
				END
				
				INSERT INTO DI_SEARCH_RESULTS
					SELECT @SearchLanguage, 
					   IndicatorNid, UnitNId, @SearchAreasCopy, @IsAreaNumeric,  
					   Indicator, Unit, '''', 
					   CAST(DefaultSGNId AS NVARCHAR) + ''[@@@@]'' + DefaultSG, '''', '''',
					   AreaCount, SGCount, SourceCount, TPCount, DVCount, 
					   AreaNIds, SGNIds, SourceNIds, TPNIds, DVNIds, DVSeries, Dimensions,BlockAreaParentNId
					FROM #ResultSet R
					WHERE  NOT EXISTS (	SELECT TOP 1 NId FROM DI_SEARCH_RESULTS C 
								WHERE C.IndicatorNId = R.IndicatorNid
								AND C.AreaNId = @SearchAreasCopy AND SearchLanguage = @SearchLanguage)
					AND DVCount > 0

				DROP TABLE #ResultSet
				DROP TABLE #SearchAreas
				DROP TABLE #SearchIndicators		
	
END





' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'

ALTER PROCEDURE [dbo].[SP_CREATE_CACHE_RESULT_IU_GENERAL_XX]
	@SearchArea			NVARCHAR(MAX)
AS
BEGIN
	DECLARE @SQLSearch					NVARCHAR(MAX);

	DECLARE @Max						INT;
	DECLARE @Count						INT;
	DECLARE @TempIndicatorNId			INT;
	DECLARE @TempUnitNId				INT;
	DECLARE @TempDefaultSGNId			INT;
	DECLARE @TempAreaCount				INT;
	DECLARE @TempSGCount				INT;
	DECLARE @TempSourceCount			INT;
	DECLARE @TempTPCount				INT;
	DECLARE @TempDVCount				INT;
	DECLARE @TempAreaNIds				NVARCHAR(MAX);
	DECLARE @TempSGNIds					NVARCHAR(MAX);
	DECLARE @TempSourceNIds				NVARCHAR(MAX);
	DECLARE @TempTPNIds					NVARCHAR(MAX);
	DECLARE @TempDVNIds					NVARCHAR(MAX);
	DECLARE @TempDVSeries				NVARCHAR(MAX);
	DECLARE @TempDimensions				NVARCHAR(MAX);
	DECLARE @TempAreaSeries				NVARCHAR(MAX);
	DECLARE @SearchLanguage				NVARCHAR(MAX);
	DECLARE @SearchAreasCopy			NVARCHAR(MAX);
	DECLARE @BlockAreaParentNID			INT ;
	DECLARE @IsAreaNumeric				bit;


	SET @SearchLanguage = ''_XX'';
	SELECT @SearchLanguage = SUBSTRING(@SearchLanguage, 2, LEN(@SearchLanguage) - 1)
		

		SET @SearchAreasCopy = @SearchArea
		SET @IsAreaNumeric = 0
		SELECT @SearchArea = dbo.fn_get_normalized_area_nids_XX(@SearchArea)
		SELECT @BlockAreaParentNID = dbo.FN_GET_AreaNIdGeneric_XX(@SearchAreasCopy)

				CREATE TABLE #ResultSet
				(
					Id							INT IDENTITY(1,1) PRIMARY KEY,
					IndicatorNId				INT DEFAULT 0,
					UnitNId						INT DEFAULT 0,
					Indicator					NVARCHAR(MAX) DEFAULT ''''  ,
					ICName						NVARCHAR(MAX) DEFAULT ''''  ,
					Unit						NVARCHAR(MAX) DEFAULT ''''  ,
					DefaultSGNId				INT DEFAULT 0,
					DefaultSG					NVARCHAR(MAX) DEFAULT ''''  ,
					AreaCount					INT DEFAULT 0,
					SGCount						INT DEFAULT 0,
					SourceCount					INT DEFAULT 0,
					TPCount						INT DEFAULT 0,
					DVCount						INT DEFAULT 0,
					AreaNIds					NVARCHAR(MAX) DEFAULT ''''  ,
					SGNIds						NVARCHAR(MAX) DEFAULT ''''  ,
					SourceNIds					NVARCHAR(MAX) DEFAULT ''''  ,
					TPNIds						NVARCHAR(MAX) DEFAULT ''''  ,
					DVNIds						NVARCHAR(MAX) DEFAULT ''''  ,
					DVSeries					NVARCHAR(MAX) DEFAULT ''''	,
					Dimensions					NVARCHAR(MAX) DEFAULT ''''	,
					BlockAreaParentNId			INT DEFAULT 0
				)

				CREATE TABLE #SearchAreas 
				(
					Id							INT IDENTITY(1,1) PRIMARY KEY,
					AreaNId						INT DEFAULT 0,
					Area						NVARCHAR(MAX) DEFAULT '''' NOT NULL
				)

				CREATE TABLE #SearchIndicators 
				(
					Id							INT IDENTITY(1,1) PRIMARY KEY,
					IndicatorNId				INT DEFAULT 0,
					Indicator					NVARCHAR(MAX) DEFAULT '''' NOT NULL,
					ICName						NVARCHAR(MAX) DEFAULT '''' NOT NULL
				)

				IF (@SearchArea IS NOT NULL AND @SearchArea <> '''')
				BEGIN
					--***************************************************************************************************************************************************
					-- Filling Area Table
					--***************************************************************************************************************************************************
					INSERT INTO #SearchAreas (AreaNId, Area)   
					SELECT Area.Area_NId, Area.Area_Name FROM UT_Area_XX Area 
					JOIN dbo.FN_GET_SPLITTED_LIST(@SearchArea, '','') List 
					ON Area.Area_NId = List.Value
				END

				
					--***************************************************************************************************************************************************
					-- Filling Indicator Table
					--***************************************************************************************************************************************************
				INSERT INTO #SearchIndicators (IndicatorNId, Indicator)
				SELECT Indicator.Indicator_NId, Indicator.Indicator_Name FROM UT_Indicator_XX Indicator 
						

				INSERT INTO #ResultSet (IndicatorNId, UnitNId, Indicator, ICName, Unit)
				SELECT DISTINCT Data.Indicator_NId, Data.Unit_NId, #SearchIndicators.Indicator, #SearchIndicators.ICName, Unit.Unit_Name 
				FROM UT_Data Data
				JOIN #SearchIndicators ON Data.Indicator_NId = #SearchIndicators.IndicatorNId 
				JOIN UT_Unit_XX Unit ON Data.Unit_NId = Unit.Unit_NId 
				JOIN #SearchAreas ON Data.Area_NId = #SearchAreas.AreaNId 
				ORDER BY Data.Indicator_NId, Data.Unit_NId

				UPDATE #ResultSet SET DefaultSGNId = IUS.Subgroup_Val_NId , DefaultSG = SGV.Subgroup_Val
				FROM UT_Indicator_Unit_Subgroup IUS, UT_Subgroup_Vals_XX SGV 
				WHERE IndicatorNId = IUS.Indicator_NId AND UnitNId = IUS.Unit_NId 
				AND IUS.IsDefaultSubgroup = 1 AND IUS.Subgroup_Val_NId = SGV.Subgroup_Val_NId

				SELECT @Count = 0, @Max = COUNT(*) FROM #ResultSet			
			
				WHILE(@Count < @Max)
				BEGIN
					SET @Count = @Count + 1;
					
					SELECT @TempIndicatorNId = IndicatorNId, @TempUnitNId = UnitNId, @TempDefaultSGNId = DefaultSGNId,  
					@TempAreaCount = 0, @TempSGCount = 0, @TempSourceCount = 0, @TempTPCount = 0, @TempDVCount = 0,
					@TempAreaNIds = '','', @TempSGNIds = '','', @TempSourceNIds = '','', @TempTPNIds = '','', @TempDVNIds = '','', 
					@TempDVSeries = '','', @TempAreaSeries = '',''
					FROM #ResultSet WHERE Id = @Count

					SELECT 
						@TempAreaNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Area_NId AS NVARCHAR(10))+ '','', @TempAreaNIds) = 0) 
							THEN 
								@TempAreaNIds + CAST(Tbl.Area_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempAreaNIds
						END ,
						@TempSGNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Subgroup_Val_NId AS NVARCHAR(10))+ '','', @TempSGNIds) = 0) 
							THEN 
								@TempSGNIds + CAST(Tbl.Subgroup_Val_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempSGNIds
						END ,
						@TempSourceNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Source_NId AS NVARCHAR(10))+ '','', @TempSourceNIds) = 0) 
							THEN 
								@TempSourceNIds + CAST(Tbl.Source_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempSourceNIds
						END ,
						@TempTPNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.TimePeriod_NId AS NVARCHAR(10))+ '','', @TempTPNIds) = 0) 
							THEN 
								@TempTPNIds + CAST(Tbl.TimePeriod_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempTPNIds
						END ,
						@TempDVNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Data_NId AS NVARCHAR(10))+ '','', @TempDVNIds) = 0) 
							THEN 
								@TempDVNIds + CAST(Tbl.Data_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempDVNIds
						END 
						,
						@TempDVSeries = 
						CASE 
							WHEN (Tbl.Subgroup_Val_NId = @TempDefaultSGNId AND Tbl.isMRD = 1) 
							THEN 
								@TempDVSeries + ISNULL(dbo.CustomCast(Tbl.Data_Value), Tbl.Textual_Data_Value) + ''|''
							ELSE
								@TempDVSeries
						END ,
						@TempAreaSeries = 
						CASE 
							WHEN (Tbl.Subgroup_Val_NId = @TempDefaultSGNId AND Tbl.isMRD = 1) 
							THEN 
								@TempAreaSeries + Tbl.Area + ''|''
							ELSE
								@TempAreaSeries
						END 
					FROM 
					(
						SELECT TOP 2147483647 Data.Data_NId, Data.Textual_Data_Value, Data.Data_Value, Data.Subgroup_Val_NId, Data.Source_NId, Data.TimePeriod_NId, Data.Area_NId, Data.isMRD, Areas.Area 
						FROM UT_DATA Data JOIN #SearchAreas Areas ON Data.Area_NId = Areas.AreaNId
						WHERE Data.Indicator_NId = @TempIndicatorNId AND Data.Unit_NId = @TempUnitNId 						
					) Tbl 
					
					IF (LEN(@TempAreaNIds) > 1)
					BEGIN
						SELECT @TempAreaNIds = SUBSTRING(@TempAreaNIds, 2, LEN(@TempAreaNIds) - 2)
						SET @TempAreaCount = LEN(@TempAreaNIds) - LEN(REPLACE(@TempAreaNIds, '','', '''')) + 1
					END	
					ELSE IF (LEN(@TempAreaNIds) = 1)
					BEGIN
						SELECT @TempAreaNIds = ''''
						SET @TempAreaCount = 0
					END	
				
					IF (LEN(@TempSGNIds) > 1)
					BEGIN
						SELECT @TempSGNIds = SUBSTRING(@TempSGNIds, 2, LEN(@TempSGNIds) - 2)
						SET @TempSGCount = LEN(@TempSGNIds) - LEN(REPLACE(@TempSGNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempSGNIds) = 1)
					BEGIN
						SELECT @TempSGNIds = ''''
						SET @TempSGCount = 0
					END	

					IF (LEN(@TempSourceNIds) > 1)
					BEGIN
						SELECT @TempSourceNIds = SUBSTRING(@TempSourceNIds, 2, LEN(@TempSourceNIds) - 2)
						SET @TempSourceCount = LEN(@TempSourceNIds) - LEN(REPLACE(@TempSourceNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempSourceNIds) = 1)
					BEGIN
						SELECT @TempSourceNIds = ''''
						SET @TempSourceCount = 0
					END	

					IF (LEN(@TempTPNIds) > 1)
					BEGIN
						SELECT @TempTPNIds = SUBSTRING(@TempTPNIds, 2, LEN(@TempTPNIds) - 2)
						SET @TempTPCount = LEN(@TempTPNIds) - LEN(REPLACE(@TempTPNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempTPNIds) = 1)
					BEGIN
						SELECT @TempTPNIds = ''''
						SET @TempTPCount = 0
					END	

					IF (LEN(@TempDVNIds) > 1)
					BEGIN
						SELECT @TempDVNIds = SUBSTRING(@TempDVNIds, 2, LEN(@TempDVNIds) - 2)
						SET @TempDVCount = LEN(@TempDVNIds) - LEN(REPLACE(@TempDVNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempDVNIds) = 1)
					BEGIN
						SELECT @TempDVNIds = ''''
						SET @TempDVCount = 0
					END	

					IF (LEN(@TempDVSeries) > 1)
					BEGIN
						SELECT @TempDVSeries = SUBSTRING(@TempDVSeries, 2, LEN(@TempDVSeries) - 2)
					END
					ELSE IF (LEN(@TempDVSeries) = 1)
					BEGIN
						SELECT @TempDVSeries = ''''
					END

					IF (LEN(@TempAreaSeries) > 1)
					BEGIN
						SELECT @TempAreaSeries = SUBSTRING(@TempAreaSeries, 2, LEN(@TempAreaSeries) - 2)
					END
					ELSE IF (LEN(@TempAreaSeries) = 1)
					BEGIN
						SELECT @TempAreaSeries = ''''
					END	

					SET @TempDimensions = [dbo].[fn_get_dimensions_IU_XX](@TempIndicatorNId, @TempUnitNId)

					UPDATE #ResultSet
					SET	AreaCount = @TempAreaCount, SGCount = @TempSGCount, SourceCount = @TempSourceCount, TPCount = @TempTPCount, DVCount = @TempDVCount,
						AreaNIds = @TempAreaNIds, SGNIds = @TempSGNIds, SourceNIds = @TempSourceNIds, TPNIds = @TempTPNIds, DVNIds = @TempDVNIds, 
						DVSeries = @TempAreaSeries + '':'' + @TempDVSeries, Dimensions = @TempDimensions,BlockAreaParentNId=@BlockAreaParentNID
					WHERE IndicatorNId = @TempIndicatorNId AND UnitNId = @TempUnitNId
				END
				
				INSERT INTO DI_SEARCH_RESULTS
					SELECT @SearchLanguage, 
					   IndicatorNid, UnitNId, @SearchAreasCopy, @IsAreaNumeric,  
					   Indicator, Unit, '''', 
					   CAST(DefaultSGNId AS NVARCHAR) + ''[@@@@]'' + DefaultSG, '''', '''',
					   AreaCount, SGCount, SourceCount, TPCount, DVCount, 
					   AreaNIds, SGNIds, SourceNIds, TPNIds, DVNIds, DVSeries, Dimensions,BlockAreaParentNId
					FROM #ResultSet R
					WHERE  NOT EXISTS (	SELECT TOP 1 NId FROM DI_SEARCH_RESULTS C 
								WHERE C.IndicatorNId = R.IndicatorNid
								AND C.AreaNId = @SearchAreasCopy AND SearchLanguage = @SearchLanguage)
					AND DVCount > 0

				DROP TABLE #ResultSet
				DROP TABLE #SearchAreas
				DROP TABLE #SearchIndicators		
	
END





' 
END

--SPSEPARATOR--

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CREATE_CACHE_RESULT_IU_SPECIFIC_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[SP_CREATE_CACHE_RESULT_IU_SPECIFIC_XX]
	@SearchArea			NVARCHAR(MAX)
AS
BEGIN
	DECLARE @SQLSearch					NVARCHAR(MAX);

	DECLARE @Max						INT;
	DECLARE @Count						INT;
	DECLARE @TempIndicatorNId			INT;
	DECLARE @TempUnitNId				INT;
	DECLARE @TempDefaultSGNId			INT;
	DECLARE @TempAreaCount				INT;
	DECLARE @TempSGCount				INT;
	DECLARE @TempSourceCount			INT;
	DECLARE @TempTPCount				INT;
	DECLARE @TempDVCount				INT;
	DECLARE @TempAreaNIds				NVARCHAR(MAX);
	DECLARE @TempSGNIds					NVARCHAR(MAX);
	DECLARE @TempSourceNIds				NVARCHAR(MAX);
	DECLARE @TempTPNIds					NVARCHAR(MAX);
	DECLARE @TempDVNIds					NVARCHAR(MAX);
	DECLARE @TempDVSeries				NVARCHAR(MAX);
	DECLARE @TempDimensions				NVARCHAR(MAX);
	DECLARE @TempAreaSeries				NVARCHAR(MAX);
	DECLARE @SearchLanguage				NVARCHAR(MAX);
	DECLARE @SearchAreasCopy			NVARCHAR(MAX);
	DECLARE @BlockAreaParentNID			INT ;
	DECLARE @IsAreaNumeric				bit;


	SET @SearchLanguage = ''_XX'';
	SELECT @SearchLanguage = SUBSTRING(@SearchLanguage, 2, LEN(@SearchLanguage) - 1)
		

		SET @SearchAreasCopy = @SearchArea
		SET @IsAreaNumeric = 0
		SELECT @SearchArea = dbo.fn_get_normalized_area_nids_XX(@SearchArea)
		SELECT @BlockAreaParentNID = dbo.FN_GET_AreaNIdGeneric_XX(@SearchAreasCopy)
				CREATE TABLE #ResultSet
				(
					Id							INT IDENTITY(1,1) PRIMARY KEY,
					IndicatorNId				INT DEFAULT 0,
					UnitNId						INT DEFAULT 0,
					Indicator					NVARCHAR(MAX) DEFAULT ''''  ,
					ICName						NVARCHAR(MAX) DEFAULT ''''  ,
					Unit						NVARCHAR(MAX) DEFAULT ''''  ,
					DefaultSGNId				INT DEFAULT 0,
					DefaultSG					NVARCHAR(MAX) DEFAULT ''''  ,
					AreaCount					INT DEFAULT 0,
					SGCount						INT DEFAULT 0,
					SourceCount					INT DEFAULT 0,
					TPCount						INT DEFAULT 0,
					DVCount						INT DEFAULT 0,
					AreaNIds					NVARCHAR(MAX) DEFAULT ''''  ,
					SGNIds						NVARCHAR(MAX) DEFAULT ''''  ,
					SourceNIds					NVARCHAR(MAX) DEFAULT ''''  ,
					TPNIds						NVARCHAR(MAX) DEFAULT ''''  ,
					DVNIds						NVARCHAR(MAX) DEFAULT ''''  ,
					DVSeries					NVARCHAR(MAX) DEFAULT ''''	,
					Dimensions					NVARCHAR(MAX) DEFAULT ''''	,
					BlockAreaParentNId			INT DEFAULT 0
				)

				CREATE TABLE #SearchAreas 
				(
					Id							INT IDENTITY(1,1) PRIMARY KEY,
					AreaNId						INT DEFAULT 0,
					Area						NVARCHAR(MAX) DEFAULT '''' NOT NULL
				)

				CREATE TABLE #SearchIndicators 
				(
					Id							INT IDENTITY(1,1) PRIMARY KEY,
					IndicatorNId				INT DEFAULT 0,
					Indicator					NVARCHAR(MAX) DEFAULT '''' NOT NULL,
					ICName						NVARCHAR(MAX) DEFAULT '''' NOT NULL
				)

				IF (@SearchArea IS NOT NULL AND @SearchArea <> '''')
				BEGIN
					--***************************************************************************************************************************************************
					-- Filling Area Table
					--***************************************************************************************************************************************************
					INSERT INTO #SearchAreas (AreaNId, Area)   
					SELECT Area.Area_NId, Area.Area_Name FROM UT_Area_XX Area 
					JOIN dbo.FN_GET_SPLITTED_LIST(@SearchArea, '','') List 
					ON Area.Area_NId = List.Value
				END

				
					--***************************************************************************************************************************************************
					-- Filling Indicator Table
					--***************************************************************************************************************************************************
				INSERT INTO #SearchIndicators (IndicatorNId, Indicator)
				SELECT Indicator.Indicator_NId, Indicator.Indicator_Name FROM UT_Indicator_XX Indicator 
						

				INSERT INTO #ResultSet (IndicatorNId, UnitNId, Indicator, ICName, Unit)
				SELECT DISTINCT Data.Indicator_NId, Data.Unit_NId, #SearchIndicators.Indicator, #SearchIndicators.ICName, Unit.Unit_Name 
				FROM UT_Data Data
				JOIN #SearchIndicators ON Data.Indicator_NId = #SearchIndicators.IndicatorNId 
				JOIN UT_Unit_XX Unit ON Data.Unit_NId = Unit.Unit_NId 
				JOIN #SearchAreas ON Data.Area_NId = #SearchAreas.AreaNId 
				ORDER BY Data.Indicator_NId, Data.Unit_NId

				UPDATE #ResultSet SET DefaultSGNId = IUS.Subgroup_Val_NId , DefaultSG = SGV.Subgroup_Val
				FROM UT_Indicator_Unit_Subgroup IUS, UT_Subgroup_Vals_XX SGV 
				WHERE IndicatorNId = IUS.Indicator_NId AND UnitNId = IUS.Unit_NId 
				AND IUS.IsDefaultSubgroup = 1 AND IUS.Subgroup_Val_NId = SGV.Subgroup_Val_NId

				SELECT @Count = 0, @Max = COUNT(*) FROM #ResultSet			
			
				WHILE(@Count < @Max)
				BEGIN
					SET @Count = @Count + 1;
					
					SELECT @TempIndicatorNId = IndicatorNId, @TempUnitNId = UnitNId, @TempDefaultSGNId = DefaultSGNId,  
					@TempAreaCount = 0, @TempSGCount = 0, @TempSourceCount = 0, @TempTPCount = 0, @TempDVCount = 0,
					@TempAreaNIds = '','', @TempSGNIds = '','', @TempSourceNIds = '','', @TempTPNIds = '','', @TempDVNIds = '','', 
					@TempDVSeries = '','', @TempAreaSeries = '',''
					FROM #ResultSet WHERE Id = @Count

					SELECT 
						@TempAreaNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Area_NId AS NVARCHAR(10))+ '','', @TempAreaNIds) = 0) 
							THEN 
								@TempAreaNIds + CAST(Tbl.Area_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempAreaNIds
						END ,
						@TempSGNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Subgroup_Val_NId AS NVARCHAR(10))+ '','', @TempSGNIds) = 0) 
							THEN 
								@TempSGNIds + CAST(Tbl.Subgroup_Val_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempSGNIds
						END ,
						@TempSourceNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Source_NId AS NVARCHAR(10))+ '','', @TempSourceNIds) = 0) 
							THEN 
								@TempSourceNIds + CAST(Tbl.Source_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempSourceNIds
						END ,
						@TempTPNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.TimePeriod_NId AS NVARCHAR(10))+ '','', @TempTPNIds) = 0) 
							THEN 
								@TempTPNIds + CAST(Tbl.TimePeriod_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempTPNIds
						END ,
						@TempDVNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Data_NId AS NVARCHAR(10))+ '','', @TempDVNIds) = 0) 
							THEN 
								@TempDVNIds + CAST(Tbl.Data_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempDVNIds
						END 
						,
						@TempDVSeries = 
						CASE 
							WHEN (Tbl.Subgroup_Val_NId = @TempDefaultSGNId AND Tbl.isMRD = 1) 
							THEN 
								@TempDVSeries + ISNULL(dbo.CustomCast(Tbl.Data_Value), Tbl.Textual_Data_Value) + ''|''
							ELSE
								@TempDVSeries
						END ,
						@TempAreaSeries = 
						CASE 
							WHEN (Tbl.Subgroup_Val_NId = @TempDefaultSGNId AND Tbl.isMRD = 1) 
							THEN 
								@TempAreaSeries + Tbl.Area + ''|''
							ELSE
								@TempAreaSeries
						END 
					FROM 
					(
						SELECT TOP 2147483647 Data.Data_NId, Data.Textual_Data_Value, Data.Data_Value, Data.Subgroup_Val_NId, Data.Source_NId, Data.TimePeriod_NId, Data.Area_NId, Data.isMRD, Areas.Area 
						FROM UT_DATA Data JOIN #SearchAreas Areas ON Data.Area_NId = Areas.AreaNId
						WHERE Data.Indicator_NId = @TempIndicatorNId AND Data.Unit_NId = @TempUnitNId
						AND DATA.Subgroup_Val_NId = @TempDefaultSGNId 						
					) Tbl 
					
					IF (LEN(@TempAreaNIds) > 1)
					BEGIN
						SELECT @TempAreaNIds = SUBSTRING(@TempAreaNIds, 2, LEN(@TempAreaNIds) - 2)
						SET @TempAreaCount = LEN(@TempAreaNIds) - LEN(REPLACE(@TempAreaNIds, '','', '''')) + 1
					END	
					ELSE IF (LEN(@TempAreaNIds) = 1)
					BEGIN
						SELECT @TempAreaNIds = ''''
						SET @TempAreaCount = 0
					END	
				
					IF (LEN(@TempSGNIds) > 1)
					BEGIN
						SELECT @TempSGNIds = SUBSTRING(@TempSGNIds, 2, LEN(@TempSGNIds) - 2)
						SET @TempSGCount = LEN(@TempSGNIds) - LEN(REPLACE(@TempSGNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempSGNIds) = 1)
					BEGIN
						SELECT @TempSGNIds = ''''
						SET @TempSGCount = 0
					END	

					IF (LEN(@TempSourceNIds) > 1)
					BEGIN
						SELECT @TempSourceNIds = SUBSTRING(@TempSourceNIds, 2, LEN(@TempSourceNIds) - 2)
						SET @TempSourceCount = LEN(@TempSourceNIds) - LEN(REPLACE(@TempSourceNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempSourceNIds) = 1)
					BEGIN
						SELECT @TempSourceNIds = ''''
						SET @TempSourceCount = 0
					END	

					IF (LEN(@TempTPNIds) > 1)
					BEGIN
						SELECT @TempTPNIds = SUBSTRING(@TempTPNIds, 2, LEN(@TempTPNIds) - 2)
						SET @TempTPCount = LEN(@TempTPNIds) - LEN(REPLACE(@TempTPNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempTPNIds) = 1)
					BEGIN
						SELECT @TempTPNIds = ''''
						SET @TempTPCount = 0
					END	

					IF (LEN(@TempDVNIds) > 1)
					BEGIN
						SELECT @TempDVNIds = SUBSTRING(@TempDVNIds, 2, LEN(@TempDVNIds) - 2)
						SET @TempDVCount = LEN(@TempDVNIds) - LEN(REPLACE(@TempDVNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempDVNIds) = 1)
					BEGIN
						SELECT @TempDVNIds = ''''
						SET @TempDVCount = 0
					END	

					IF (LEN(@TempDVSeries) > 1)
					BEGIN
						SELECT @TempDVSeries = SUBSTRING(@TempDVSeries, 2, LEN(@TempDVSeries) - 2)
					END
					ELSE IF (LEN(@TempDVSeries) = 1)
					BEGIN
						SELECT @TempDVSeries = ''''
					END

					IF (LEN(@TempAreaSeries) > 1)
					BEGIN
						SELECT @TempAreaSeries = SUBSTRING(@TempAreaSeries, 2, LEN(@TempAreaSeries) - 2)
					END
					ELSE IF (LEN(@TempAreaSeries) = 1)
					BEGIN
						SELECT @TempAreaSeries = ''''
					END	

					SET @TempDimensions = [dbo].[fn_get_dimensions_IU_XX](@TempIndicatorNId, @TempUnitNId)

					UPDATE #ResultSet
					SET	AreaCount = @TempAreaCount, SGCount = @TempSGCount, SourceCount = @TempSourceCount, TPCount = @TempTPCount, DVCount = @TempDVCount,
						AreaNIds = @TempAreaNIds, SGNIds = @TempSGNIds, SourceNIds = @TempSourceNIds, TPNIds = @TempTPNIds, DVNIds = @TempDVNIds, 
						DVSeries = @TempAreaSeries + '':'' + @TempDVSeries, Dimensions = @TempDimensions,BlockAreaParentNId=@BlockAreaParentNID
					WHERE IndicatorNId = @TempIndicatorNId AND UnitNId = @TempUnitNId
				END
				
				INSERT INTO DI_SEARCH_RESULTS
					SELECT @SearchLanguage, 
					   IndicatorNid, UnitNId, @SearchAreasCopy, @IsAreaNumeric,  
					   Indicator, Unit, '''', 
					   CAST(DefaultSGNId AS NVARCHAR) + ''[@@@@]'' + DefaultSG, '''', '''',
					   AreaCount, SGCount, SourceCount, TPCount, DVCount, 
					   AreaNIds, SGNIds, SourceNIds, TPNIds, DVNIds, DVSeries, Dimensions,BlockAreaParentNId
					FROM #ResultSet R
					WHERE  NOT EXISTS (	SELECT TOP 1 NId FROM DI_SEARCH_RESULTS C 
								WHERE C.IndicatorNId = R.IndicatorNid
								AND C.AreaNId = @SearchAreasCopy AND SearchLanguage = @SearchLanguage)
					AND DVCount > 0

				DROP TABLE #ResultSet
				DROP TABLE #SearchAreas
				DROP TABLE #SearchIndicators		
	
END





' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'

ALTER PROCEDURE [dbo].[SP_CREATE_CACHE_RESULT_IU_SPECIFIC_XX]
	@SearchArea			NVARCHAR(MAX)
AS
BEGIN
	DECLARE @SQLSearch					NVARCHAR(MAX);

	DECLARE @Max						INT;
	DECLARE @Count						INT;
	DECLARE @TempIndicatorNId			INT;
	DECLARE @TempUnitNId				INT;
	DECLARE @TempDefaultSGNId			INT;
	DECLARE @TempAreaCount				INT;
	DECLARE @TempSGCount				INT;
	DECLARE @TempSourceCount			INT;
	DECLARE @TempTPCount				INT;
	DECLARE @TempDVCount				INT;
	DECLARE @TempAreaNIds				NVARCHAR(MAX);
	DECLARE @TempSGNIds					NVARCHAR(MAX);
	DECLARE @TempSourceNIds				NVARCHAR(MAX);
	DECLARE @TempTPNIds					NVARCHAR(MAX);
	DECLARE @TempDVNIds					NVARCHAR(MAX);
	DECLARE @TempDVSeries				NVARCHAR(MAX);
	DECLARE @TempDimensions				NVARCHAR(MAX);
	DECLARE @TempAreaSeries				NVARCHAR(MAX);
	DECLARE @SearchLanguage				NVARCHAR(MAX);
	DECLARE @SearchAreasCopy			NVARCHAR(MAX);
	DECLARE @BlockAreaParentNID			INT ;
	DECLARE @IsAreaNumeric				bit;


	SET @SearchLanguage = ''_XX'';
	SELECT @SearchLanguage = SUBSTRING(@SearchLanguage, 2, LEN(@SearchLanguage) - 1)
		

		SET @SearchAreasCopy = @SearchArea
		SET @IsAreaNumeric = 0
		SELECT @SearchArea = dbo.fn_get_normalized_area_nids_XX(@SearchArea)
		SELECT @BlockAreaParentNID = dbo.FN_GET_AreaNIdGeneric_XX(@SearchAreasCopy)

				CREATE TABLE #ResultSet
				(
					Id							INT IDENTITY(1,1) PRIMARY KEY,
					IndicatorNId				INT DEFAULT 0,
					UnitNId						INT DEFAULT 0,
					Indicator					NVARCHAR(MAX) DEFAULT ''''  ,
					ICName						NVARCHAR(MAX) DEFAULT ''''  ,
					Unit						NVARCHAR(MAX) DEFAULT ''''  ,
					DefaultSGNId				INT DEFAULT 0,
					DefaultSG					NVARCHAR(MAX) DEFAULT ''''  ,
					AreaCount					INT DEFAULT 0,
					SGCount						INT DEFAULT 0,
					SourceCount					INT DEFAULT 0,
					TPCount						INT DEFAULT 0,
					DVCount						INT DEFAULT 0,
					AreaNIds					NVARCHAR(MAX) DEFAULT ''''  ,
					SGNIds						NVARCHAR(MAX) DEFAULT ''''  ,
					SourceNIds					NVARCHAR(MAX) DEFAULT ''''  ,
					TPNIds						NVARCHAR(MAX) DEFAULT ''''  ,
					DVNIds						NVARCHAR(MAX) DEFAULT ''''  ,
					DVSeries					NVARCHAR(MAX) DEFAULT ''''	,
					Dimensions					NVARCHAR(MAX) DEFAULT ''''	,
					BlockAreaParentNId			INT DEFAULT 0
				)

				CREATE TABLE #SearchAreas 
				(
					Id							INT IDENTITY(1,1) PRIMARY KEY,
					AreaNId						INT DEFAULT 0,
					Area						NVARCHAR(MAX) DEFAULT '''' NOT NULL
				)

				CREATE TABLE #SearchIndicators 
				(
					Id							INT IDENTITY(1,1) PRIMARY KEY,
					IndicatorNId				INT DEFAULT 0,
					Indicator					NVARCHAR(MAX) DEFAULT '''' NOT NULL,
					ICName						NVARCHAR(MAX) DEFAULT '''' NOT NULL
				)

				IF (@SearchArea IS NOT NULL AND @SearchArea <> '''')
				BEGIN
					--***************************************************************************************************************************************************
					-- Filling Area Table
					--***************************************************************************************************************************************************
					INSERT INTO #SearchAreas (AreaNId, Area)   
					SELECT Area.Area_NId, Area.Area_Name FROM UT_Area_XX Area 
					JOIN dbo.FN_GET_SPLITTED_LIST(@SearchArea, '','') List 
					ON Area.Area_NId = List.Value
				END

				
					--***************************************************************************************************************************************************
					-- Filling Indicator Table
					--***************************************************************************************************************************************************
				INSERT INTO #SearchIndicators (IndicatorNId, Indicator)
				SELECT Indicator.Indicator_NId, Indicator.Indicator_Name FROM UT_Indicator_XX Indicator 
						

				INSERT INTO #ResultSet (IndicatorNId, UnitNId, Indicator, ICName, Unit)
				SELECT DISTINCT Data.Indicator_NId, Data.Unit_NId, #SearchIndicators.Indicator, #SearchIndicators.ICName, Unit.Unit_Name 
				FROM UT_Data Data
				JOIN #SearchIndicators ON Data.Indicator_NId = #SearchIndicators.IndicatorNId 
				JOIN UT_Unit_XX Unit ON Data.Unit_NId = Unit.Unit_NId 
				JOIN #SearchAreas ON Data.Area_NId = #SearchAreas.AreaNId 
				ORDER BY Data.Indicator_NId, Data.Unit_NId

				UPDATE #ResultSet SET DefaultSGNId = IUS.Subgroup_Val_NId , DefaultSG = SGV.Subgroup_Val
				FROM UT_Indicator_Unit_Subgroup IUS, UT_Subgroup_Vals_XX SGV 
				WHERE IndicatorNId = IUS.Indicator_NId AND UnitNId = IUS.Unit_NId 
				AND IUS.IsDefaultSubgroup = 1 AND IUS.Subgroup_Val_NId = SGV.Subgroup_Val_NId

				SELECT @Count = 0, @Max = COUNT(*) FROM #ResultSet			
			
				WHILE(@Count < @Max)
				BEGIN
					SET @Count = @Count + 1;
					
					SELECT @TempIndicatorNId = IndicatorNId, @TempUnitNId = UnitNId, @TempDefaultSGNId = DefaultSGNId,  
					@TempAreaCount = 0, @TempSGCount = 0, @TempSourceCount = 0, @TempTPCount = 0, @TempDVCount = 0,
					@TempAreaNIds = '','', @TempSGNIds = '','', @TempSourceNIds = '','', @TempTPNIds = '','', @TempDVNIds = '','', 
					@TempDVSeries = '','', @TempAreaSeries = '',''
					FROM #ResultSet WHERE Id = @Count

					SELECT 
						@TempAreaNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Area_NId AS NVARCHAR(10))+ '','', @TempAreaNIds) = 0) 
							THEN 
								@TempAreaNIds + CAST(Tbl.Area_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempAreaNIds
						END ,
						@TempSGNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Subgroup_Val_NId AS NVARCHAR(10))+ '','', @TempSGNIds) = 0) 
							THEN 
								@TempSGNIds + CAST(Tbl.Subgroup_Val_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempSGNIds
						END ,
						@TempSourceNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Source_NId AS NVARCHAR(10))+ '','', @TempSourceNIds) = 0) 
							THEN 
								@TempSourceNIds + CAST(Tbl.Source_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempSourceNIds
						END ,
						@TempTPNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.TimePeriod_NId AS NVARCHAR(10))+ '','', @TempTPNIds) = 0) 
							THEN 
								@TempTPNIds + CAST(Tbl.TimePeriod_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempTPNIds
						END ,
						@TempDVNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Data_NId AS NVARCHAR(10))+ '','', @TempDVNIds) = 0) 
							THEN 
								@TempDVNIds + CAST(Tbl.Data_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempDVNIds
						END 
						,
						@TempDVSeries = 
						CASE 
							WHEN (Tbl.Subgroup_Val_NId = @TempDefaultSGNId AND Tbl.isMRD = 1) 
							THEN 
								@TempDVSeries + ISNULL(dbo.CustomCast(Tbl.Data_Value), Tbl.Textual_Data_Value) + ''|''
							ELSE
								@TempDVSeries
						END ,
						@TempAreaSeries = 
						CASE 
							WHEN (Tbl.Subgroup_Val_NId = @TempDefaultSGNId AND Tbl.isMRD = 1) 
							THEN 
								@TempAreaSeries + Tbl.Area + ''|''
							ELSE
								@TempAreaSeries
						END 
					FROM 
					(
						SELECT TOP 2147483647 Data.Data_NId, Data.Textual_Data_Value, Data.Data_Value, Data.Subgroup_Val_NId, Data.Source_NId, Data.TimePeriod_NId, Data.Area_NId, Data.isMRD, Areas.Area 
						FROM UT_DATA Data JOIN #SearchAreas Areas ON Data.Area_NId = Areas.AreaNId
						WHERE Data.Indicator_NId = @TempIndicatorNId AND Data.Unit_NId = @TempUnitNId 
						AND DATA.Subgroup_Val_NId = @TempDefaultSGNId						
					) Tbl 
					
					IF (LEN(@TempAreaNIds) > 1)
					BEGIN
						SELECT @TempAreaNIds = SUBSTRING(@TempAreaNIds, 2, LEN(@TempAreaNIds) - 2)
						SET @TempAreaCount = LEN(@TempAreaNIds) - LEN(REPLACE(@TempAreaNIds, '','', '''')) + 1
					END	
					ELSE IF (LEN(@TempAreaNIds) = 1)
					BEGIN
						SELECT @TempAreaNIds = ''''
						SET @TempAreaCount = 0
					END	
				
					IF (LEN(@TempSGNIds) > 1)
					BEGIN
						SELECT @TempSGNIds = SUBSTRING(@TempSGNIds, 2, LEN(@TempSGNIds) - 2)
						SET @TempSGCount = LEN(@TempSGNIds) - LEN(REPLACE(@TempSGNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempSGNIds) = 1)
					BEGIN
						SELECT @TempSGNIds = ''''
						SET @TempSGCount = 0
					END	

					IF (LEN(@TempSourceNIds) > 1)
					BEGIN
						SELECT @TempSourceNIds = SUBSTRING(@TempSourceNIds, 2, LEN(@TempSourceNIds) - 2)
						SET @TempSourceCount = LEN(@TempSourceNIds) - LEN(REPLACE(@TempSourceNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempSourceNIds) = 1)
					BEGIN
						SELECT @TempSourceNIds = ''''
						SET @TempSourceCount = 0
					END	

					IF (LEN(@TempTPNIds) > 1)
					BEGIN
						SELECT @TempTPNIds = SUBSTRING(@TempTPNIds, 2, LEN(@TempTPNIds) - 2)
						SET @TempTPCount = LEN(@TempTPNIds) - LEN(REPLACE(@TempTPNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempTPNIds) = 1)
					BEGIN
						SELECT @TempTPNIds = ''''
						SET @TempTPCount = 0
					END	

					IF (LEN(@TempDVNIds) > 1)
					BEGIN
						SELECT @TempDVNIds = SUBSTRING(@TempDVNIds, 2, LEN(@TempDVNIds) - 2)
						SET @TempDVCount = LEN(@TempDVNIds) - LEN(REPLACE(@TempDVNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempDVNIds) = 1)
					BEGIN
						SELECT @TempDVNIds = ''''
						SET @TempDVCount = 0
					END	

					IF (LEN(@TempDVSeries) > 1)
					BEGIN
						SELECT @TempDVSeries = SUBSTRING(@TempDVSeries, 2, LEN(@TempDVSeries) - 2)
					END
					ELSE IF (LEN(@TempDVSeries) = 1)
					BEGIN
						SELECT @TempDVSeries = ''''
					END

					IF (LEN(@TempAreaSeries) > 1)
					BEGIN
						SELECT @TempAreaSeries = SUBSTRING(@TempAreaSeries, 2, LEN(@TempAreaSeries) - 2)
					END
					ELSE IF (LEN(@TempAreaSeries) = 1)
					BEGIN
						SELECT @TempAreaSeries = ''''
					END	

					SET @TempDimensions = [dbo].[fn_get_dimensions_IU_XX](@TempIndicatorNId, @TempUnitNId)

					UPDATE #ResultSet
					SET	AreaCount = @TempAreaCount, SGCount = @TempSGCount, SourceCount = @TempSourceCount, TPCount = @TempTPCount, DVCount = @TempDVCount,
						AreaNIds = @TempAreaNIds, SGNIds = @TempSGNIds, SourceNIds = @TempSourceNIds, TPNIds = @TempTPNIds, DVNIds = @TempDVNIds, 
						DVSeries = @TempAreaSeries + '':'' + @TempDVSeries, Dimensions = @TempDimensions,BlockAreaParentNId=@BlockAreaParentNID
					WHERE IndicatorNId = @TempIndicatorNId AND UnitNId = @TempUnitNId
				END
				
				INSERT INTO DI_SEARCH_RESULTS
					SELECT @SearchLanguage, 
					   IndicatorNid, UnitNId, @SearchAreasCopy, @IsAreaNumeric,  
					   Indicator, Unit, '''', 
					   CAST(DefaultSGNId AS NVARCHAR) + ''[@@@@]'' + DefaultSG, '''', '''',
					   AreaCount, SGCount, SourceCount, TPCount, DVCount, 
					   AreaNIds, SGNIds, SourceNIds, TPNIds, DVNIds, DVSeries, Dimensions,BlockAreaParentNId
					FROM #ResultSet R
					WHERE  NOT EXISTS (	SELECT TOP 1 NId FROM DI_SEARCH_RESULTS C 
								WHERE C.IndicatorNId = R.IndicatorNid
								AND C.AreaNId = @SearchAreasCopy AND SearchLanguage = @SearchLanguage)
					AND DVCount > 0

				DROP TABLE #ResultSet
				DROP TABLE #SearchAreas
				DROP TABLE #SearchIndicators		
	
END





' 
END

--SPSEPARATOR--

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CREATE_CACHE_RESULT_IU_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[SP_CREATE_CACHE_RESULT_IU_XX]
	@SearchArea			NVARCHAR(MAX),
	@AdditionalParams		VARCHAR(100)
AS
BEGIN
	IF(@AdditionalParams = ''GENERAL'')
	BEGIN
		EXEC SP_CREATE_CACHE_RESULT_IU_GENERAL_XX @SearchArea
	END
	ELSE IF(@AdditionalParams = ''SPECIFIC'')
	BEGIN
		EXEC SP_CREATE_CACHE_RESULT_IU_SPECIFIC_XX @SearchArea
	END
	
END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'

ALTER PROCEDURE [dbo].[SP_CREATE_CACHE_RESULT_IU_XX]
		@SearchArea			NVARCHAR(MAX),
	@AdditionalParams		VARCHAR(100)
AS
BEGIN
	IF(@AdditionalParams = ''GENERAL'')
	BEGIN
		EXEC SP_CREATE_CACHE_RESULT_IU_GENERAL_XX @SearchArea
	END
	ELSE IF(@AdditionalParams = ''SPECIFIC'')
	BEGIN
		EXEC SP_CREATE_CACHE_RESULT_IU_SPECIFIC_XX @SearchArea
	END
	
END
' 
END

--SPSEPARATOR--



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GET_DEFAULT_AREAS_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROC [dbo].[SP_GET_DEFAULT_AREAS_XX]

AS
BEGIN
		SELECT	Area_NId,
				Area_ID, 
				Area_Name,
				(SELECT COUNT(Area_NId) FROM dbo.ut_area_XX WHERE Area_Parent_NId = A.Area_NId) AS ''Children''
		FROM dbo.ut_area_XX A
		WHERE Area_Level = 1
END' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'ALTER PROC [dbo].[SP_GET_DEFAULT_AREAS_XX]

AS
BEGIN
		SELECT	Area_NId,
				Area_ID, 
				Area_Name,
				(SELECT COUNT(Area_NId) FROM dbo.ut_area_XX WHERE Area_Parent_NId = A.Area_NId) AS ''Children''
		FROM dbo.ut_area_XX A
		WHERE Area_Level = 1
END' 
END


--SPSEPARATOR--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GET_DEFAULT_INDICATORS_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROC [dbo].[SP_GET_DEFAULT_INDICATORS_XX]

AS

BEGIN

	SELECT IUS.IUSNId, IUS.Indicator_NId, IUS.Unit_NId, I.Indicator_Name, U.Unit_Name  FROM dbo.ut_indicator_unit_subgroup IUS
	JOIN dbo.ut_indicator_XX I ON IUS.Indicator_NId = I.Indicator_NId
	JOIN dbo.ut_unit_XX U ON IUS.Unit_NId = U.Unit_NId
	WHERE IUS.IUSNId IN
	(
		SELECT DISTINCT IUSNId FROM dbo.ut_ic_ius
		WHERE IC_NId IN
		(
			SELECT TOP 1 IC_NID FROM DBO.UT_INDICATOR_CLASSIFICATIONS_XX
			WHERE IC_PARENT_NID = -1 AND IC_ORDER IS NOT NULL
			ORDER BY IC_ORDER ASC
		)
	)

END' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROC [dbo].[SP_GET_DEFAULT_INDICATORS_XX]

AS

BEGIN


	SELECT IUS.IUSNId, IUS.Indicator_NId, IUS.Unit_NId, I.Indicator_Name, U.Unit_Name  FROM dbo.ut_indicator_unit_subgroup IUS
	JOIN dbo.ut_indicator_XX I ON IUS.Indicator_NId = I.Indicator_NId
	JOIN dbo.ut_unit_XX U ON IUS.Unit_NId = U.Unit_NId
	WHERE IUS.IUSNId IN
	(
		SELECT DISTINCT IUSNId FROM dbo.ut_ic_ius
		WHERE IC_NId IN
		(
			SELECT TOP 1 IC_NID FROM DBO.UT_INDICATOR_CLASSIFICATIONS_XX
			WHERE IC_PARENT_NID = -1 AND IC_ORDER IS NOT NULL
			ORDER BY IC_ORDER ASC
		)
	)

END' 
END



--SPSEPARATOR--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GET_CACHE_COUNT]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROC [dbo].[SP_GET_CACHE_COUNT]

AS

BEGIN

	SELECT COUNT(*) AS ROWS_COUNT FROM DBO.DI_SEARCH_RESULTS

END' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROC [dbo].[SP_GET_CACHE_COUNT]

AS

BEGIN

	SELECT COUNT(*) AS ROWS_COUNT FROM DBO.DI_SEARCH_RESULTS

END' 
END

--SPSEPARATOR--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_getDimensions_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROC sp_getDimensions_XX
(
	@DistinctSubgroup_Val_NIds VARCHAR(MAX)
)
AS

BEGIN

SELECT SUBGROUP_TYPE_NAME FROM DBO.UT_SUBGROUP_TYPE_XX 
WHERE SUBGROUP_TYPE_NID IN 
(
	SELECT SUBGROUP_TYPE FROM DBO.UT_SUBGROUP_XX WHERE SUBGROUP_NID IN
	(
		SELECT SUBGROUP_NID FROM DBO.UT_SUBGROUP_VALS_SUBGROUP WHERE SUBGROUP_VAL_NID IN 
		( 
			SELECT ITEMS FROM DBO.SPLIT(@DistinctSubgroup_Val_NIds, '','')
		)
	)
)
END' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROC sp_getDimensions_XX
(
	@DistinctSubgroup_Val_NIds VARCHAR(MAX)
)
AS

BEGIN

SELECT SUBGROUP_TYPE_NAME FROM DBO.UT_SUBGROUP_TYPE_XX 
WHERE SUBGROUP_TYPE_NID IN 
(
	SELECT SUBGROUP_TYPE FROM DBO.UT_SUBGROUP_XX WHERE SUBGROUP_NID IN
	(
		SELECT SUBGROUP_NID FROM DBO.UT_SUBGROUP_VALS_SUBGROUP WHERE SUBGROUP_VAL_NID IN 
		( 
			SELECT ITEMS FROM DBO.SPLIT(@DistinctSubgroup_Val_NIds, '','')
		)
	)
)
END' 
END


--SPSEPARATOR--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_getIUNidName_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROC sp_getIUNidName_XX
(
	@IndicatorNId int,
	@UnitNId int
)
AS

BEGIN

SELECT 
	(
		SELECT TOP 1 INDICATOR_NAME FROM DBO.UT_INDICATOR_XX 
		WHERE INDICATOR_NID = @IndicatorNId
	) 
	+ 
	'' - ''
	+ 
	(
		SELECT TOP 1 UNIT_NAME FROM DBO.UT_UNIT_XX 
		WHERE UNIT_NID = @UnitNId
	)
END' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'

ALTER PROC sp_getIUNidName_XX
(
	@IndicatorNId int,
	@UnitNId int
)
AS

BEGIN

SELECT 
	(
		SELECT TOP 1 INDICATOR_NAME FROM DBO.UT_INDICATOR_XX 
		WHERE INDICATOR_NID = @IndicatorNId
	) 
	+ 
	'' - '' 
	+ 
	(
		SELECT TOP 1 UNIT_NAME FROM DBO.UT_UNIT_XX 
		WHERE UNIT_NID = @UnitNId
	)
END' 
END



--SPSEPARATOR--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_getSGName_DimensionPair_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROC sp_getSGName_DimensionPair_XX
(
	@distinctSGNids VARCHAR(MAX)
)
AS

BEGIN
SELECT SUBGROUP_NAME, 
	(
		SELECT TOP 1 SUBGROUP_TYPE_NAME FROM DBO.UT_SUBGROUP_TYPE_XX 
		WHERE SUBGROUP_TYPE_NID = SUBGROUP_TYPE
	) AS DIMENSION
FROM DBO.UT_SUBGROUP_XX 
WHERE SUBGROUP_NID IN 
	(
		SELECT SUBGROUP_NID FROM DBO.UT_SUBGROUP_VALS_SUBGROUP WHERE SUBGROUP_VAL_NID IN 
		(
			SELECT ITEMS FROM DBO.SPLIT(@distinctSGNids, '','')
		)
	)

END' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'


ALTER PROC sp_getSGName_DimensionPair_XX
(
	@distinctSGNids VARCHAR(MAX)
)
AS

BEGIN
SELECT SUBGROUP_NAME, 
	(
		SELECT TOP 1 SUBGROUP_TYPE_NAME FROM DBO.UT_SUBGROUP_TYPE_XX 
		WHERE SUBGROUP_TYPE_NID = SUBGROUP_TYPE
	) AS DIMENSION
FROM DBO.UT_SUBGROUP_XX 
WHERE SUBGROUP_NID IN 
	(
		SELECT SUBGROUP_NID FROM DBO.UT_SUBGROUP_VALS_SUBGROUP WHERE SUBGROUP_VAL_NID IN 
		(
			SELECT ITEMS FROM DBO.SPLIT(@distinctSGNids, '','')
		)
	)

END' 
END

--SPSEPARATOR--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_getSubgroup_Name_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROC sp_getSubgroup_Name_XX
(
	@Subgroup_Val_NId int
)
AS

BEGIN
SELECT 
CAST(
		(
			SELECT TOP 1 SUBGROUP_NAME FROM DBO.UT_SUBGROUP_XX 
			WHERE SUBGROUP_NID = SVS.SUBGROUP_NID
		)
		AS NVARCHAR(50)
	) AS SubGroup_Name 
FROM DBO.UT_SUBGROUP_VALS_SUBGROUP AS SVS 
	
WHERE SUBGROUP_VAL_NID = @Subgroup_Val_NId

END' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROC sp_getSubgroup_Name_XX
(
	@Subgroup_Val_NId int
)
AS

BEGIN
SELECT 
CAST(
		(
			SELECT TOP 1 SUBGROUP_NAME FROM DBO.UT_SUBGROUP_XX 
			WHERE SUBGROUP_NID = SVS.SUBGROUP_NID
		)
		AS NVARCHAR(50)
	) AS SubGroup_Name 
FROM DBO.UT_SUBGROUP_VALS_SUBGROUP AS SVS 
	
WHERE SUBGROUP_VAL_NID = @Subgroup_Val_NId

END' 
END



--SPSEPARATOR--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_getIUSNames_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROC sp_getIUSNames_XX
(
	@DistinctNids VARCHAR(MAX)
)
AS

BEGIN

SELECT	IUSNId, 
		(
			SELECT TOP 1 INDICATOR_NAME FROM DBO.UT_INDICATOR_XX 
			WHERE INDICATOR_NID = DTIUS.INDICATOR_NID
		) 
		+ 
		'', '' 
		+ 
		(
			SELECT TOP 1 UNIT_NAME FROM DBO.UT_UNIT_XX 
			WHERE UNIT_NID = DTIUS.UNIT_NID
		) 
		+ 
		'', '' 
		+ 
		(
			SELECT TOP 1 SUBGROUP_VAL FROM DBO.UT_SUBGROUP_VALS_XX 
			WHERE SUBGROUP_VAL_NID = DTIUS.SUBGROUP_VAL_NID
		) AS IUSName 

FROM DBO.UT_INDICATOR_UNIT_SUBGROUP AS DTIUS 
WHERE IUSNID IN 
(
	SELECT ITEMS FROM DBO.SPLIT(@DistinctNids,'','')
)

END' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROC sp_getIUSNames_XX
(
	@DistinctNids VARCHAR(MAX)
)
AS

BEGIN

SELECT	IUSNId, 
		(
			SELECT TOP 1 INDICATOR_NAME FROM DBO.UT_INDICATOR_XX 
			WHERE INDICATOR_NID = DTIUS.INDICATOR_NID
		) 
		+ 
		'', ''
		+ 
		(
			SELECT TOP 1 UNIT_NAME FROM DBO.UT_UNIT_XX 
			WHERE UNIT_NID = DTIUS.UNIT_NID
		) 
		+ 
		'', '' 
		+ 
		(
			SELECT TOP 1 SUBGROUP_VAL FROM DBO.UT_SUBGROUP_VALS_XX 
			WHERE SUBGROUP_VAL_NID = DTIUS.SUBGROUP_VAL_NID
		) AS IUSName 

FROM DBO.UT_INDICATOR_UNIT_SUBGROUP AS DTIUS 
WHERE IUSNID IN 
(
	SELECT ITEMS FROM DBO.SPLIT(@DistinctNids,'','')
)

END' 
END



--SPSEPARATOR--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetAreaMapLayerInfoToConvertIntoM49_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_GetAreaMapLayerInfoToConvertIntoM49_XX]	
AS
BEGIN
	SET NOCOUNT ON;
    Select AM.Area_Nid, A.Area_ID, A.Area_Level, AML.Layer_NId, AM.Feature_Layer, AM.Feature_Type_NId , AML.Layer_Size, AML.Layer_Type, AML.MinX, AML.MinY, AML.MaxX, AML.MaxY, AML.Start_Date, AML.End_Date, AML.Update_Timestamp, AMM.Layer_Name, AMM.Metadata_Text, AML.Layer_shp,AML.Layer_shx, AML.Layer_dbf 
	FROM ut_area_map_layer AML
	inner join ut_area_map_metadata_XX AMM on AMM.Layer_NId = AML.Layer_NId
	inner join ut_area_map AM on AM.Layer_NId = AML.Layer_NId
	inner join ut_area_XX A on A.Area_Nid = AM.Area_Nid
	where (len(A.area_id)>=3 and (
	A.area_id like ''AFR%''  OR
	A.area_id like ''ASI%'' OR
	A.area_id like ''EUR%'' OR
	A.area_id like ''LAC%'' OR
	A.area_id like ''NAM%'' OR
	A.area_id like ''OCN%'')) or (A.area_id like ''NAM%'')
END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'ALTER PROCEDURE [dbo].[sp_GetAreaMapLayerInfoToConvertIntoM49_XX]	
AS
BEGIN
	SET NOCOUNT ON;
    Select AM.Area_Nid, A.Area_ID, A.Area_Level, AML.Layer_NId, AM.Feature_Layer, AM.Feature_Type_NId , AML.Layer_Size, AML.Layer_Type, AML.MinX, AML.MinY, AML.MaxX, AML.MaxY, AML.Start_Date, AML.End_Date, AML.Update_Timestamp, AMM.Layer_Name, AMM.Metadata_Text, AML.Layer_shp,AML.Layer_shx, AML.Layer_dbf 
	FROM ut_area_map_layer AML
	inner join ut_area_map_metadata_XX AMM on AMM.Layer_NId = AML.Layer_NId
	inner join ut_area_map AM on AM.Layer_NId = AML.Layer_NId
	inner join ut_area_XX A on A.Area_Nid = AM.Area_Nid
	where (len(A.area_id)>=3 and (
	A.area_id like ''AFR%''  OR
	A.area_id like ''ASI%'' OR
	A.area_id like ''EUR%'' OR
	A.area_id like ''LAC%'' OR
	A.area_id like ''NAM%'' OR
	A.area_id like ''OCN%'')) or (A.area_id like ''NAM%'')
END
' 
END


--SPSEPARATOR--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_UpdateAreaIntoM49Standard]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_UpdateAreaIntoM49Standard]	
AS
BEGIN
	declare @sqlText nvarchar(max)
	declare @CurrentLanguageCode varchar(10)	

	declare @LanguageTable TABLE (RowNumber int identity(1,1), Language_Code varchar(10))
	declare @TotalRowCount int
	declare @Counter int

	insert into @LanguageTable (Language_Code) select Language_Code from UT_Language   
	select @Counter = 1, @TotalRowCount = count(*) from @LanguageTable
	set @sqlText=''''

	SET NOCOUNT ON;

	begin try
		while(@Counter <= @TotalRowCount)
		begin				
			select @CurrentLanguageCode = Language_Code from @LanguageTable where RowNumber = @Counter

			set @sqlText = @sqlText + '' update ut_area_'' + @CurrentLanguageCode + '' set area_id=substring( area_id,4,len(area_id)) where len(area_id)>3 and (area_id like ''''AFR%'''' OR area_id like ''''ASI%'''' OR area_id like ''''EUR%'''' OR area_id like ''''LAC%'''' OR area_id like ''''NAM%'''' OR area_id like ''''NAC%''''  OR area_id like ''''OCN%''''); ''
						
			set @Counter = @Counter + 1
		end	
	execute sp_executesql @sqlText
	end try
	begin catch
		if (@@error <> 0)
		begin
			set nocount off  
			/* Raise an error with the details of the exception*/
			declare @ErrMsg nvarchar(4000), @ErrSeverity int
			select @ErrMsg = ERROR_MESSAGE(),@ErrSeverity = ERROR_SEVERITY()
			raiserror(@ErrMsg, @ErrSeverity, 1)
			return(@@ERROR)		
		end
	end catch  	    
	set nocount off  

END' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'ALTER PROCEDURE [dbo].[sp_UpdateAreaIntoM49Standard]	
AS
BEGIN
	declare @sqlText nvarchar(max)
	declare @CurrentLanguageCode varchar(10)	

	declare @LanguageTable TABLE (RowNumber int identity(1,1), Language_Code varchar(10))
	declare @TotalRowCount int
	declare @Counter int

	insert into @LanguageTable (Language_Code) select Language_Code from UT_Language   
	select @Counter = 1, @TotalRowCount = count(*) from @LanguageTable
	set @sqlText=''''

	SET NOCOUNT ON;

	begin try
		while(@Counter <= @TotalRowCount)
		begin				
			select @CurrentLanguageCode = Language_Code from @LanguageTable where RowNumber = @Counter

			set @sqlText = @sqlText + '' update ut_area_'' + @CurrentLanguageCode + '' set area_id=substring( area_id,4,len(area_id)) where len(area_id)>3 and (area_id like ''''AFR%'''' OR area_id like ''''ASI%'''' OR area_id like ''''EUR%'''' OR area_id like ''''LAC%'''' OR area_id like ''''NAM%'''' OR area_id like ''''NAC%''''  OR area_id like ''''OCN%''''); ''
						
			set @Counter = @Counter + 1
		end	
	execute sp_executesql @sqlText
	end try
	begin catch
		if (@@error <> 0)
		begin
			set nocount off  
			/* Raise an error with the details of the exception*/
			declare @ErrMsg nvarchar(4000), @ErrSeverity int
			select @ErrMsg = ERROR_MESSAGE(),@ErrSeverity = ERROR_SEVERITY()
			raiserror(@ErrMsg, @ErrSeverity, 1)
			return(@@ERROR)		
		end
	end catch  	    
	set nocount off  

END' 
END


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GET_ALL_LANGUAGE_CODES]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE SP_GET_ALL_LANGUAGE_CODES
AS
BEGIN
	SELECT LANGUAGE_CODE FROM UT_LANGUAGE
END

' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'

ALTER PROCEDURE SP_GET_ALL_LANGUAGE_CODES
AS
BEGIN
	SELECT LANGUAGE_CODE FROM UT_LANGUAGE
END

' 
END


--SPSEPARATOR--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetAreaMapLayer_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_GetAreaMapLayer_XX]
	@FilterFieldType nvarchar(25),
	@FilterText nvarchar(MAX)
AS
BEGIN
	Declare @strAreaNids varchar(MAX)
	Declare @strSQL nvarchar(MAX)
	DECLARE @ParamDefinition AS NVarchar(2000)  

	Set @strAreaNids=''''
	if(lower(@FilterFieldType)=''area_nid'')
	begin
		print @strAreaNids
		Set @strAreaNids = dbo.fn_get_normalized_area_nids_XX(@FilterText)		
	end
	else
		Set @strAreaNids = @FilterText

	set @strSQL= ''select amm.layer_name,aml.layer_nid,aml.layer_size,aml.layer_type,aml.minx,aml.miny,aml.maxx,aml.maxy,aml.start_date,aml.end_date,aml.update_timestamp,a.area_level, a.area_nid, a.area_id, a.area_block , A.Area_Name 
		from ut_area_map_layer as aml,ut_area_map_metadata_XX as amm,ut_area_map as am,ut_area_XX as a 
		where  
		aml.layer_nid = amm.layer_nid and aml.layer_nid = am.layer_nid and am.area_nid = a.area_nid 
		and a.''+@FilterFieldType+'' in (select items from dbo.split(@strAreaNids, '''','''')) order by aml.layer_type desc,a.area_level ''
	print @strSQL
	
	Set @ParamDefinition = '' @FilterFieldType nvarchar(25), @FilterText nvarchar(MAX), @strAreaNids varchar(MAX)''
    Execute sp_Executesql @strSQL, @ParamDefinition, @FilterFieldType, @FilterText,@strAreaNids
END' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'ALTER PROCEDURE [dbo].[sp_GetAreaMapLayer_XX]
	@FilterFieldType nvarchar(25),
	@FilterText nvarchar(MAX)
AS
BEGIN
	Declare @strAreaNids varchar(MAX)
	Declare @strSQL nvarchar(MAX)
	DECLARE @ParamDefinition AS NVarchar(2000)  

	Set @strAreaNids=''''
	if(lower(@FilterFieldType)=''area_nid'')
	begin
		print @strAreaNids
		Set @strAreaNids = dbo.fn_get_normalized_area_nids_XX(@FilterText)		
	end
	else
		Set @strAreaNids = @FilterText

	set @strSQL= ''select amm.layer_name,aml.layer_nid,aml.layer_size,aml.layer_type,aml.minx,aml.miny,aml.maxx,aml.maxy,aml.start_date,aml.end_date,aml.update_timestamp,a.area_level, a.area_nid, a.area_id, a.area_block , A.Area_Name 
		from ut_area_map_layer as aml,ut_area_map_metadata_XX as amm,ut_area_map as am,ut_area_XX as a 
		where  
		aml.layer_nid = amm.layer_nid and aml.layer_nid = am.layer_nid and am.area_nid = a.area_nid 
		and a.''+@FilterFieldType+'' in (select items from dbo.split(@strAreaNids, '''','''')) order by aml.layer_type desc,a.area_level ''
	print @strSQL
	
	Set @ParamDefinition = '' @FilterFieldType nvarchar(25), @FilterText nvarchar(MAX), @strAreaNids varchar(MAX)''
    Execute sp_Executesql @strSQL, @ParamDefinition, @FilterFieldType, @FilterText,@strAreaNids
END' 
END


--SPSEPARATOR--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_get_areaMapLayer_By_Level_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_get_areaMapLayer_By_Level_XX]	
	@AreaLevel  int,
	@AreaParentNId  int
AS
BEGIN

select amm.layer_name,aml.layer_nid,aml.layer_size,aml.layer_type,aml.minx,aml.miny,aml.maxx,aml.maxy,aml.start_date,aml.end_date,aml.update_timestamp,a.area_level, a.area_nid, a.area_id, a.area_Parent_NId, a.area_block , a.Area_Name 
from ut_area_map_layer as aml,ut_area_map_metadata_XX as amm,ut_area_map as am,ut_area_XX as a 
where  
aml.layer_nid = amm.layer_nid and aml.layer_nid = am.layer_nid and am.area_nid = a.area_nid 
and (a.area_level =@AreaLevel and a.Area_Parent_NId=@AreaParentNId)order by aml.layer_type desc,a.area_level 

END' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'ALTER PROCEDURE [dbo].[sp_get_areaMapLayer_By_Level_XX]	
	@AreaLevel  int,
	@AreaParentNId  int
AS
BEGIN

select amm.layer_name,aml.layer_nid,aml.layer_size,aml.layer_type,aml.minx,aml.miny,aml.maxx,aml.maxy,aml.start_date,aml.end_date,aml.update_timestamp,a.area_level, a.area_nid, a.area_id, a.area_Parent_NId, a.area_block , a.Area_Name 
from ut_area_map_layer as aml,ut_area_map_metadata_XX as amm,ut_area_map as am,ut_area_XX as a 
where  
aml.layer_nid = amm.layer_nid and aml.layer_nid = am.layer_nid and am.area_nid = a.area_nid 
and (a.area_level =@AreaLevel and a.Area_Parent_NId=@AreaParentNId)order by aml.layer_type desc,a.area_level 

END' 
END

--SPSEPARATOR--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CREATE_CACHE_RESULT_IU_QSA_GENERAL_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[SP_CREATE_CACHE_RESULT_IU_QSA_GENERAL_XX]
	@SearchArea			NVARCHAR(MAX),
	@IndicatorNId		INT			
AS
BEGIN
	DECLARE @SQLSearch					NVARCHAR(MAX);

	DECLARE @Max						INT;
	DECLARE @Count						INT;
	DECLARE @TempIndicatorNId			INT;
	DECLARE @TempUnitNId				INT;
	DECLARE @TempDefaultSGNId			INT;
	DECLARE @TempAreaCount				INT;
	DECLARE @TempSGCount				INT;
	DECLARE @TempSourceCount			INT;
	DECLARE @TempTPCount				INT;
	DECLARE @TempDVCount				INT;
	DECLARE @TempAreaNIds				NVARCHAR(MAX);
	DECLARE @TempSGNIds					NVARCHAR(MAX);
	DECLARE @TempSourceNIds				NVARCHAR(MAX);
	DECLARE @TempTPNIds					NVARCHAR(MAX);
	DECLARE @TempDVNIds					NVARCHAR(MAX);
	DECLARE @TempDVSeries				NVARCHAR(MAX);
	DECLARE @TempDimensions				NVARCHAR(MAX);
	DECLARE @TempAreaSeries				NVARCHAR(MAX);
	DECLARE @SearchLanguage				NVARCHAR(MAX);
	DECLARE @SearchAreasCopy			NVARCHAR(MAX);
	DECLARE @BlockAreaParentNID			INT;
	DECLARE @IsAreaNumeric				bit;


	SET @SearchLanguage = ''_XX'';
	SELECT @SearchLanguage = SUBSTRING(@SearchLanguage, 2, LEN(@SearchLanguage) - 1)
		

		SET @SearchAreasCopy = @SearchArea
		SET @IsAreaNumeric = 0
		SELECT @SearchArea = dbo.fn_get_normalized_area_nids_XX(@SearchArea)
		SELECT @BlockAreaParentNID = dbo.FN_GET_AreaNIdGeneric_XX(@SearchAreasCopy)
				CREATE TABLE #ResultSet
				(
					Id							INT IDENTITY(1,1) PRIMARY KEY,
					IndicatorNId				INT DEFAULT 0,
					UnitNId						INT DEFAULT 0,
					Indicator					NVARCHAR(MAX) DEFAULT ''''  ,
					ICName						NVARCHAR(MAX) DEFAULT ''''  ,
					Unit						NVARCHAR(MAX) DEFAULT ''''  ,
					DefaultSGNId				INT DEFAULT 0,
					DefaultSG					NVARCHAR(MAX) DEFAULT ''''  ,
					AreaCount					INT DEFAULT 0,
					SGCount						INT DEFAULT 0,
					SourceCount					INT DEFAULT 0,
					TPCount						INT DEFAULT 0,
					DVCount						INT DEFAULT 0,
					AreaNIds					NVARCHAR(MAX) DEFAULT ''''  ,
					SGNIds						NVARCHAR(MAX) DEFAULT ''''  ,
					SourceNIds					NVARCHAR(MAX) DEFAULT ''''  ,
					TPNIds						NVARCHAR(MAX) DEFAULT ''''  ,
					DVNIds						NVARCHAR(MAX) DEFAULT ''''  ,
					DVSeries					NVARCHAR(MAX) DEFAULT ''''  ,
					Dimensions					NVARCHAR(MAX) DEFAULT '''',
					BlockAreaParentNId			INT DEFAULT 0
				)

				CREATE TABLE #SearchAreas 
				(
					Id							INT IDENTITY(1,1) PRIMARY KEY,
					AreaNId						INT DEFAULT 0,
					Area						NVARCHAR(MAX) DEFAULT '''' NOT NULL
				)

				CREATE TABLE #SearchIndicators 
				(
					Id							INT IDENTITY(1,1) PRIMARY KEY,
					IndicatorNId				INT DEFAULT 0,
					Indicator					NVARCHAR(MAX) DEFAULT '''' NOT NULL,
					ICName						NVARCHAR(MAX) DEFAULT '''' NOT NULL
				)

				IF (@SearchArea IS NOT NULL AND @SearchArea <> '''')
				BEGIN
					--***************************************************************************************************************************************************
					-- Filling Area Table
					--***************************************************************************************************************************************************
					INSERT INTO #SearchAreas (AreaNId, Area)   
					SELECT Area.Area_NId, Area.Area_Name FROM UT_Area_XX Area 
					JOIN dbo.FN_GET_SPLITTED_LIST(@SearchArea, '','') List 
					ON Area.Area_NId = List.Value
				END

				
					--***************************************************************************************************************************************************
					-- Filling Indicator Table
					--***************************************************************************************************************************************************
				INSERT INTO #SearchIndicators (IndicatorNId, Indicator)
				SELECT Indicator.Indicator_NId, Indicator.Indicator_Name FROM UT_Indicator_XX Indicator 
				WHERE Indicator.Indicator_NId = @IndicatorNId
	
		

				INSERT INTO #ResultSet (IndicatorNId, UnitNId, Indicator, ICName, Unit)
				SELECT DISTINCT Data.Indicator_NId, Data.Unit_NId, #SearchIndicators.Indicator, #SearchIndicators.ICName, Unit.Unit_Name 
				FROM UT_Data Data
				JOIN #SearchIndicators ON Data.Indicator_NId = #SearchIndicators.IndicatorNId 
				JOIN UT_Unit_XX Unit ON Data.Unit_NId = Unit.Unit_NId 
				JOIN #SearchAreas ON Data.Area_NId = #SearchAreas.AreaNId 
				ORDER BY Data.Indicator_NId, Data.Unit_NId

				UPDATE #ResultSet SET DefaultSGNId = IUS.Subgroup_Val_NId , DefaultSG = SGV.Subgroup_Val
				FROM UT_Indicator_Unit_Subgroup IUS, UT_Subgroup_Vals_XX SGV 
				WHERE IndicatorNId = IUS.Indicator_NId AND UnitNId = IUS.Unit_NId 
				AND IUS.IsDefaultSubgroup = 1 AND IUS.Subgroup_Val_NId = SGV.Subgroup_Val_NId

				SELECT @Count = 0, @Max = COUNT(*) FROM #ResultSet			
			
				WHILE(@Count < @Max)
				BEGIN
					SET @Count = @Count + 1;
					
					SELECT @TempIndicatorNId = IndicatorNId, @TempUnitNId = UnitNId, @TempDefaultSGNId = DefaultSGNId,  
					@TempAreaCount = 0, @TempSGCount = 0, @TempSourceCount = 0, @TempTPCount = 0, @TempDVCount = 0,
					@TempAreaNIds = '','', @TempSGNIds = '','', @TempSourceNIds = '','', @TempTPNIds = '','', @TempDVNIds = '','', 
					@TempDVSeries = '','', @TempAreaSeries = '',''
					FROM #ResultSet WHERE Id = @Count

					SELECT 
						@TempAreaNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Area_NId AS NVARCHAR(10))+ '','', @TempAreaNIds) = 0) 
							THEN 
								@TempAreaNIds + CAST(Tbl.Area_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempAreaNIds
						END ,
						@TempSGNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Subgroup_Val_NId AS NVARCHAR(10))+ '','', @TempSGNIds) = 0) 
							THEN 
								@TempSGNIds + CAST(Tbl.Subgroup_Val_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempSGNIds
						END ,
						@TempSourceNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Source_NId AS NVARCHAR(10))+ '','', @TempSourceNIds) = 0) 
							THEN 
								@TempSourceNIds + CAST(Tbl.Source_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempSourceNIds
						END ,
						@TempTPNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.TimePeriod_NId AS NVARCHAR(10))+ '','', @TempTPNIds) = 0) 
							THEN 
								@TempTPNIds + CAST(Tbl.TimePeriod_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempTPNIds
						END ,
						@TempDVNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Data_NId AS NVARCHAR(10))+ '','', @TempDVNIds) = 0) 
							THEN 
								@TempDVNIds + CAST(Tbl.Data_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempDVNIds
						END 
						,
						@TempDVSeries = 
						CASE 
							WHEN (Tbl.Subgroup_Val_NId = @TempDefaultSGNId AND Tbl.isMRD = 1) 
							THEN 
								@TempDVSeries + ISNULL(dbo.CustomCast(Tbl.Data_Value), Tbl.Textual_Data_Value) + ''|''
							ELSE
								@TempDVSeries
						END ,
						@TempAreaSeries = 
						CASE 
							WHEN (Tbl.Subgroup_Val_NId = @TempDefaultSGNId AND Tbl.isMRD = 1) 
							THEN 
								@TempAreaSeries + Tbl.Area + ''|''
							ELSE
								@TempAreaSeries
						END 
					FROM 
					(
						SELECT TOP 2147483647 Data.Data_NId, Data.Textual_Data_Value, Data.Data_Value, Data.Subgroup_Val_NId, Data.Source_NId, Data.TimePeriod_NId, Data.Area_NId, Data.isMRD, Areas.Area 
						FROM UT_DATA Data JOIN #SearchAreas Areas ON Data.Area_NId = Areas.AreaNId
						WHERE Data.Indicator_NId = @TempIndicatorNId AND Data.Unit_NId = @TempUnitNId 						
					) Tbl 
					
					IF (LEN(@TempAreaNIds) > 1)
					BEGIN
						SELECT @TempAreaNIds = SUBSTRING(@TempAreaNIds, 2, LEN(@TempAreaNIds) - 2)
						SET @TempAreaCount = LEN(@TempAreaNIds) - LEN(REPLACE(@TempAreaNIds, '','', '''')) + 1
					END	
					ELSE IF (LEN(@TempAreaNIds) = 1)
					BEGIN
						SELECT @TempAreaNIds = ''''
						SET @TempAreaCount = 0
					END	
				
					IF (LEN(@TempSGNIds) > 1)
					BEGIN
						SELECT @TempSGNIds = SUBSTRING(@TempSGNIds, 2, LEN(@TempSGNIds) - 2)
						SET @TempSGCount = LEN(@TempSGNIds) - LEN(REPLACE(@TempSGNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempSGNIds) = 1)
					BEGIN
						SELECT @TempSGNIds = ''''
						SET @TempSGCount = 0
					END	

					IF (LEN(@TempSourceNIds) > 1)
					BEGIN
						SELECT @TempSourceNIds = SUBSTRING(@TempSourceNIds, 2, LEN(@TempSourceNIds) - 2)
						SET @TempSourceCount = LEN(@TempSourceNIds) - LEN(REPLACE(@TempSourceNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempSourceNIds) = 1)
					BEGIN
						SELECT @TempSourceNIds = ''''
						SET @TempSourceCount = 0
					END	

					IF (LEN(@TempTPNIds) > 1)
					BEGIN
						SELECT @TempTPNIds = SUBSTRING(@TempTPNIds, 2, LEN(@TempTPNIds) - 2)
						SET @TempTPCount = LEN(@TempTPNIds) - LEN(REPLACE(@TempTPNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempTPNIds) = 1)
					BEGIN
						SELECT @TempTPNIds = ''''
						SET @TempTPCount = 0
					END	

					IF (LEN(@TempDVNIds) > 1)
					BEGIN
						SELECT @TempDVNIds = SUBSTRING(@TempDVNIds, 2, LEN(@TempDVNIds) - 2)
						SET @TempDVCount = LEN(@TempDVNIds) - LEN(REPLACE(@TempDVNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempDVNIds) = 1)
					BEGIN
						SELECT @TempDVNIds = ''''
						SET @TempDVCount = 0
					END	

					IF (LEN(@TempDVSeries) > 1)
					BEGIN
						SELECT @TempDVSeries = SUBSTRING(@TempDVSeries, 2, LEN(@TempDVSeries) - 2)
					END
					ELSE IF (LEN(@TempDVSeries) = 1)
					BEGIN
						SELECT @TempDVSeries = ''''
					END

					IF (LEN(@TempAreaSeries) > 1)
					BEGIN
						SELECT @TempAreaSeries = SUBSTRING(@TempAreaSeries, 2, LEN(@TempAreaSeries) - 2)
					END
					ELSE IF (LEN(@TempAreaSeries) = 1)
					BEGIN
						SELECT @TempAreaSeries = ''''
					END	

					SET @TempDimensions = [dbo].[fn_get_dimensions_IU_XX](@TempIndicatorNId, @TempUnitNId)

					UPDATE #ResultSet
					SET	AreaCount = @TempAreaCount, SGCount = @TempSGCount, SourceCount = @TempSourceCount, TPCount = @TempTPCount, DVCount = @TempDVCount,
						AreaNIds = @TempAreaNIds, SGNIds = @TempSGNIds, SourceNIds = @TempSourceNIds, TPNIds = @TempTPNIds, DVNIds = @TempDVNIds, 
						DVSeries = @TempAreaSeries + '':'' + @TempDVSeries , Dimensions = @TempDimensions,BlockAreaParentNId=@BlockAreaParentNID
					WHERE IndicatorNId = @TempIndicatorNId AND UnitNId = @TempUnitNId
				END
				
				INSERT INTO DI_SEARCH_RESULTS
					SELECT @SearchLanguage, 
					   IndicatorNid, UnitNId, @SearchAreasCopy, @IsAreaNumeric,  
					   Indicator, Unit, '''', 
					   CAST(DefaultSGNId AS NVARCHAR) + ''[@@@@]'' + DefaultSG, '''', '''',
					   AreaCount, SGCount, SourceCount, TPCount, DVCount, 
					   AreaNIds, SGNIds, SourceNIds, TPNIds, DVNIds, DVSeries, Dimensions,BlockAreaParentNId
					FROM #ResultSet R
					WHERE  NOT EXISTS (	SELECT TOP 1 NId FROM DI_SEARCH_RESULTS C 
								WHERE C.IndicatorNId = R.IndicatorNid
								AND C.AreaNId = @SearchAreasCopy AND SearchLanguage = @SearchLanguage)
					AND DVCount > 0

				DROP TABLE #ResultSet
				DROP TABLE #SearchAreas
				DROP TABLE #SearchIndicators
		
	
END

' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'

ALTER PROCEDURE [dbo].[SP_CREATE_CACHE_RESULT_IU_QSA_GENERAL_XX]
	@SearchArea			NVARCHAR(MAX),
	@IndicatorNId		INT			
AS
BEGIN
	DECLARE @SQLSearch					NVARCHAR(MAX);

	DECLARE @Max						INT;
	DECLARE @Count						INT;
	DECLARE @TempIndicatorNId			INT;
	DECLARE @TempUnitNId				INT;
	DECLARE @TempDefaultSGNId			INT;
	DECLARE @TempAreaCount				INT;
	DECLARE @TempSGCount				INT;
	DECLARE @TempSourceCount			INT;
	DECLARE @TempTPCount				INT;
	DECLARE @TempDVCount				INT;
	DECLARE @TempAreaNIds				NVARCHAR(MAX);
	DECLARE @TempSGNIds					NVARCHAR(MAX);
	DECLARE @TempSourceNIds				NVARCHAR(MAX);
	DECLARE @TempTPNIds					NVARCHAR(MAX);
	DECLARE @TempDVNIds					NVARCHAR(MAX);
	DECLARE @TempDVSeries				NVARCHAR(MAX);
	DECLARE @TempDimensions				NVARCHAR(MAX);
	DECLARE @TempAreaSeries				NVARCHAR(MAX);
	DECLARE @SearchLanguage				NVARCHAR(MAX);
	DECLARE @SearchAreasCopy			NVARCHAR(MAX);
	DECLARE @BlockAreaParentNID			INT;
	DECLARE @IsAreaNumeric				bit;


	SET @SearchLanguage = ''_XX'';
	SELECT @SearchLanguage = SUBSTRING(@SearchLanguage, 2, LEN(@SearchLanguage) - 1)
		

		SET @SearchAreasCopy = @SearchArea
		SET @IsAreaNumeric = 0
		SELECT @SearchArea = dbo.fn_get_normalized_area_nids_XX(@SearchArea)
		SELECT @BlockAreaParentNID = dbo.FN_GET_AreaNIdGeneric_XX(@SearchAreasCopy)
				CREATE TABLE #ResultSet
				(
					Id							INT IDENTITY(1,1) PRIMARY KEY,
					IndicatorNId				INT DEFAULT 0,
					UnitNId						INT DEFAULT 0,
					Indicator					NVARCHAR(MAX) DEFAULT ''''  ,
					ICName						NVARCHAR(MAX) DEFAULT ''''  ,
					Unit						NVARCHAR(MAX) DEFAULT ''''  ,
					DefaultSGNId				INT DEFAULT 0,
					DefaultSG					NVARCHAR(MAX) DEFAULT ''''  ,
					AreaCount					INT DEFAULT 0,
					SGCount						INT DEFAULT 0,
					SourceCount					INT DEFAULT 0,
					TPCount						INT DEFAULT 0,
					DVCount						INT DEFAULT 0,
					AreaNIds					NVARCHAR(MAX) DEFAULT ''''  ,
					SGNIds						NVARCHAR(MAX) DEFAULT ''''  ,
					SourceNIds					NVARCHAR(MAX) DEFAULT ''''  ,
					TPNIds						NVARCHAR(MAX) DEFAULT ''''  ,
					DVNIds						NVARCHAR(MAX) DEFAULT ''''  ,
					DVSeries					NVARCHAR(MAX) DEFAULT ''''  ,
					Dimensions					NVARCHAR(MAX) DEFAULT '''',
					BlockAreaParentNId			INT DEFAULT 0
				)

				CREATE TABLE #SearchAreas 
				(
					Id							INT IDENTITY(1,1) PRIMARY KEY,
					AreaNId						INT DEFAULT 0,
					Area						NVARCHAR(MAX) DEFAULT '''' NOT NULL
				)

				CREATE TABLE #SearchIndicators 
				(
					Id							INT IDENTITY(1,1) PRIMARY KEY,
					IndicatorNId				INT DEFAULT 0,
					Indicator					NVARCHAR(MAX) DEFAULT '''' NOT NULL,
					ICName						NVARCHAR(MAX) DEFAULT '''' NOT NULL
				)

				IF (@SearchArea IS NOT NULL AND @SearchArea <> '''')
				BEGIN
					--***************************************************************************************************************************************************
					-- Filling Area Table
					--***************************************************************************************************************************************************
					INSERT INTO #SearchAreas (AreaNId, Area)   
					SELECT Area.Area_NId, Area.Area_Name FROM UT_Area_XX Area 
					JOIN dbo.FN_GET_SPLITTED_LIST(@SearchArea, '','') List 
					ON Area.Area_NId = List.Value
				END

				
					--***************************************************************************************************************************************************
					-- Filling Indicator Table
					--***************************************************************************************************************************************************
				INSERT INTO #SearchIndicators (IndicatorNId, Indicator)
				SELECT Indicator.Indicator_NId, Indicator.Indicator_Name FROM UT_Indicator_XX Indicator 
				WHERE Indicator.Indicator_NId = @IndicatorNId
	
		

				INSERT INTO #ResultSet (IndicatorNId, UnitNId, Indicator, ICName, Unit)
				SELECT DISTINCT Data.Indicator_NId, Data.Unit_NId, #SearchIndicators.Indicator, #SearchIndicators.ICName, Unit.Unit_Name 
				FROM UT_Data Data
				JOIN #SearchIndicators ON Data.Indicator_NId = #SearchIndicators.IndicatorNId 
				JOIN UT_Unit_XX Unit ON Data.Unit_NId = Unit.Unit_NId 
				JOIN #SearchAreas ON Data.Area_NId = #SearchAreas.AreaNId 
				ORDER BY Data.Indicator_NId, Data.Unit_NId

				UPDATE #ResultSet SET DefaultSGNId = IUS.Subgroup_Val_NId , DefaultSG = SGV.Subgroup_Val
				FROM UT_Indicator_Unit_Subgroup IUS, UT_Subgroup_Vals_XX SGV 
				WHERE IndicatorNId = IUS.Indicator_NId AND UnitNId = IUS.Unit_NId 
				AND IUS.IsDefaultSubgroup = 1 AND IUS.Subgroup_Val_NId = SGV.Subgroup_Val_NId

				SELECT @Count = 0, @Max = COUNT(*) FROM #ResultSet			
			
				WHILE(@Count < @Max)
				BEGIN
					SET @Count = @Count + 1;
					
					SELECT @TempIndicatorNId = IndicatorNId, @TempUnitNId = UnitNId, @TempDefaultSGNId = DefaultSGNId,  
					@TempAreaCount = 0, @TempSGCount = 0, @TempSourceCount = 0, @TempTPCount = 0, @TempDVCount = 0,
					@TempAreaNIds = '','', @TempSGNIds = '','', @TempSourceNIds = '','', @TempTPNIds = '','', @TempDVNIds = '','', 
					@TempDVSeries = '','', @TempAreaSeries = '',''
					FROM #ResultSet WHERE Id = @Count

					SELECT 
						@TempAreaNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Area_NId AS NVARCHAR(10))+ '','', @TempAreaNIds) = 0) 
							THEN 
								@TempAreaNIds + CAST(Tbl.Area_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempAreaNIds
						END ,
						@TempSGNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Subgroup_Val_NId AS NVARCHAR(10))+ '','', @TempSGNIds) = 0) 
							THEN 
								@TempSGNIds + CAST(Tbl.Subgroup_Val_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempSGNIds
						END ,
						@TempSourceNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Source_NId AS NVARCHAR(10))+ '','', @TempSourceNIds) = 0) 
							THEN 
								@TempSourceNIds + CAST(Tbl.Source_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempSourceNIds
						END ,
						@TempTPNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.TimePeriod_NId AS NVARCHAR(10))+ '','', @TempTPNIds) = 0) 
							THEN 
								@TempTPNIds + CAST(Tbl.TimePeriod_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempTPNIds
						END ,
						@TempDVNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Data_NId AS NVARCHAR(10))+ '','', @TempDVNIds) = 0) 
							THEN 
								@TempDVNIds + CAST(Tbl.Data_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempDVNIds
						END 
						,
						@TempDVSeries = 
						CASE 
							WHEN (Tbl.Subgroup_Val_NId = @TempDefaultSGNId AND Tbl.isMRD = 1) 
							THEN 
								@TempDVSeries + ISNULL(dbo.CustomCast(Tbl.Data_Value), Tbl.Textual_Data_Value) + ''|''
							ELSE
								@TempDVSeries
						END ,
						@TempAreaSeries = 
						CASE 
							WHEN (Tbl.Subgroup_Val_NId = @TempDefaultSGNId AND Tbl.isMRD = 1) 
							THEN 
								@TempAreaSeries + Tbl.Area + ''|''
							ELSE
								@TempAreaSeries
						END 
					FROM 
					(
						SELECT TOP 2147483647 Data.Data_NId, Data.Textual_Data_Value, Data.Data_Value, Data.Subgroup_Val_NId, Data.Source_NId, Data.TimePeriod_NId, Data.Area_NId, Data.isMRD, Areas.Area 
						FROM UT_DATA Data JOIN #SearchAreas Areas ON Data.Area_NId = Areas.AreaNId
						WHERE Data.Indicator_NId = @TempIndicatorNId AND Data.Unit_NId = @TempUnitNId 						
					) Tbl 
					
					IF (LEN(@TempAreaNIds) > 1)
					BEGIN
						SELECT @TempAreaNIds = SUBSTRING(@TempAreaNIds, 2, LEN(@TempAreaNIds) - 2)
						SET @TempAreaCount = LEN(@TempAreaNIds) - LEN(REPLACE(@TempAreaNIds, '','', '''')) + 1
					END	
					ELSE IF (LEN(@TempAreaNIds) = 1)
					BEGIN
						SELECT @TempAreaNIds = ''''
						SET @TempAreaCount = 0
					END	
				
					IF (LEN(@TempSGNIds) > 1)
					BEGIN
						SELECT @TempSGNIds = SUBSTRING(@TempSGNIds, 2, LEN(@TempSGNIds) - 2)
						SET @TempSGCount = LEN(@TempSGNIds) - LEN(REPLACE(@TempSGNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempSGNIds) = 1)
					BEGIN
						SELECT @TempSGNIds = ''''
						SET @TempSGCount = 0
					END	

					IF (LEN(@TempSourceNIds) > 1)
					BEGIN
						SELECT @TempSourceNIds = SUBSTRING(@TempSourceNIds, 2, LEN(@TempSourceNIds) - 2)
						SET @TempSourceCount = LEN(@TempSourceNIds) - LEN(REPLACE(@TempSourceNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempSourceNIds) = 1)
					BEGIN
						SELECT @TempSourceNIds = ''''
						SET @TempSourceCount = 0
					END	

					IF (LEN(@TempTPNIds) > 1)
					BEGIN
						SELECT @TempTPNIds = SUBSTRING(@TempTPNIds, 2, LEN(@TempTPNIds) - 2)
						SET @TempTPCount = LEN(@TempTPNIds) - LEN(REPLACE(@TempTPNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempTPNIds) = 1)
					BEGIN
						SELECT @TempTPNIds = ''''
						SET @TempTPCount = 0
					END	

					IF (LEN(@TempDVNIds) > 1)
					BEGIN
						SELECT @TempDVNIds = SUBSTRING(@TempDVNIds, 2, LEN(@TempDVNIds) - 2)
						SET @TempDVCount = LEN(@TempDVNIds) - LEN(REPLACE(@TempDVNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempDVNIds) = 1)
					BEGIN
						SELECT @TempDVNIds = ''''
						SET @TempDVCount = 0
					END	

					IF (LEN(@TempDVSeries) > 1)
					BEGIN
						SELECT @TempDVSeries = SUBSTRING(@TempDVSeries, 2, LEN(@TempDVSeries) - 2)
					END
					ELSE IF (LEN(@TempDVSeries) = 1)
					BEGIN
						SELECT @TempDVSeries = ''''
					END

					IF (LEN(@TempAreaSeries) > 1)
					BEGIN
						SELECT @TempAreaSeries = SUBSTRING(@TempAreaSeries, 2, LEN(@TempAreaSeries) - 2)
					END
					ELSE IF (LEN(@TempAreaSeries) = 1)
					BEGIN
						SELECT @TempAreaSeries = ''''
					END	

					SET @TempDimensions = [dbo].[fn_get_dimensions_IU_XX](@TempIndicatorNId, @TempUnitNId)

					UPDATE #ResultSet
					SET	AreaCount = @TempAreaCount, SGCount = @TempSGCount, SourceCount = @TempSourceCount, TPCount = @TempTPCount, DVCount = @TempDVCount,
						AreaNIds = @TempAreaNIds, SGNIds = @TempSGNIds, SourceNIds = @TempSourceNIds, TPNIds = @TempTPNIds, DVNIds = @TempDVNIds, 
						DVSeries = @TempAreaSeries + '':'' + @TempDVSeries , Dimensions = @TempDimensions,BlockAreaParentNId=@BlockAreaParentNID
					WHERE IndicatorNId = @TempIndicatorNId AND UnitNId = @TempUnitNId
				END
				
				INSERT INTO DI_SEARCH_RESULTS
					SELECT @SearchLanguage, 
					   IndicatorNid, UnitNId, @SearchAreasCopy, @IsAreaNumeric,  
					   Indicator, Unit, '''', 
					   CAST(DefaultSGNId AS NVARCHAR) + ''[@@@@]'' + DefaultSG, '''', '''',
					   AreaCount, SGCount, SourceCount, TPCount, DVCount, 
					   AreaNIds, SGNIds, SourceNIds, TPNIds, DVNIds, DVSeries, Dimensions,BlockAreaParentNId
					FROM #ResultSet R
					WHERE  NOT EXISTS (	SELECT TOP 1 NId FROM DI_SEARCH_RESULTS C 
								WHERE C.IndicatorNId = R.IndicatorNid
								AND C.AreaNId = @SearchAreasCopy AND SearchLanguage = @SearchLanguage)
					AND DVCount > 0

				DROP TABLE #ResultSet
				DROP TABLE #SearchAreas
				DROP TABLE #SearchIndicators
		
	
END

' 
END

--SPSeparator--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CREATE_CACHE_RESULT_IU_QSA_SPECIFIC_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[SP_CREATE_CACHE_RESULT_IU_QSA_SPECIFIC_XX]
	@SearchArea			NVARCHAR(MAX),
	@IndicatorNId		INT			
AS
BEGIN
	DECLARE @SQLSearch					NVARCHAR(MAX);

	DECLARE @Max						INT;
	DECLARE @Count						INT;
	DECLARE @TempIndicatorNId			INT;
	DECLARE @TempUnitNId				INT;
	DECLARE @TempDefaultSGNId			INT;
	DECLARE @TempAreaCount				INT;
	DECLARE @TempSGCount				INT;
	DECLARE @TempSourceCount			INT;
	DECLARE @TempTPCount				INT;
	DECLARE @TempDVCount				INT;
	DECLARE @TempAreaNIds				NVARCHAR(MAX);
	DECLARE @TempSGNIds					NVARCHAR(MAX);
	DECLARE @TempSourceNIds				NVARCHAR(MAX);
	DECLARE @TempTPNIds					NVARCHAR(MAX);
	DECLARE @TempDVNIds					NVARCHAR(MAX);
	DECLARE @TempDVSeries				NVARCHAR(MAX);
	DECLARE @TempDimensions				NVARCHAR(MAX);
	DECLARE @TempAreaSeries				NVARCHAR(MAX);
	DECLARE @SearchLanguage				NVARCHAR(MAX);
	DECLARE @SearchAreasCopy			NVARCHAR(MAX);
	DECLARE @BlockAreaParentNID			INT;
	DECLARE @IsAreaNumeric				bit;


	SET @SearchLanguage = ''_XX'';
	SELECT @SearchLanguage = SUBSTRING(@SearchLanguage, 2, LEN(@SearchLanguage) - 1)
		

		SET @SearchAreasCopy = @SearchArea
		SET @IsAreaNumeric = 0
		SELECT @SearchArea = dbo.fn_get_normalized_area_nids_XX(@SearchArea)
		SELECT @BlockAreaParentNID = dbo.FN_GET_AreaNIdGeneric_XX(@SearchAreasCopy)
				CREATE TABLE #ResultSet
				(
					Id							INT IDENTITY(1,1) PRIMARY KEY,
					IndicatorNId				INT DEFAULT 0,
					UnitNId						INT DEFAULT 0,
					Indicator					NVARCHAR(MAX) DEFAULT ''''  ,
					ICName						NVARCHAR(MAX) DEFAULT ''''  ,
					Unit						NVARCHAR(MAX) DEFAULT ''''  ,
					DefaultSGNId				INT DEFAULT 0,
					DefaultSG					NVARCHAR(MAX) DEFAULT ''''  ,
					AreaCount					INT DEFAULT 0,
					SGCount						INT DEFAULT 0,
					SourceCount					INT DEFAULT 0,
					TPCount						INT DEFAULT 0,
					DVCount						INT DEFAULT 0,
					AreaNIds					NVARCHAR(MAX) DEFAULT ''''  ,
					SGNIds						NVARCHAR(MAX) DEFAULT ''''  ,
					SourceNIds					NVARCHAR(MAX) DEFAULT ''''  ,
					TPNIds						NVARCHAR(MAX) DEFAULT ''''  ,
					DVNIds						NVARCHAR(MAX) DEFAULT ''''  ,
					DVSeries					NVARCHAR(MAX) DEFAULT ''''  ,
					Dimensions					NVARCHAR(MAX) DEFAULT '''',
					BlockAreaParentNId			INT DEFAULT 0
				)

				CREATE TABLE #SearchAreas 
				(
					Id							INT IDENTITY(1,1) PRIMARY KEY,
					AreaNId						INT DEFAULT 0,
					Area						NVARCHAR(MAX) DEFAULT '''' NOT NULL
				)

				CREATE TABLE #SearchIndicators 
				(
					Id							INT IDENTITY(1,1) PRIMARY KEY,
					IndicatorNId				INT DEFAULT 0,
					Indicator					NVARCHAR(MAX) DEFAULT '''' NOT NULL,
					ICName						NVARCHAR(MAX) DEFAULT '''' NOT NULL
				)

				IF (@SearchArea IS NOT NULL AND @SearchArea <> '''')
				BEGIN
					--***************************************************************************************************************************************************
					-- Filling Area Table
					--***************************************************************************************************************************************************
					INSERT INTO #SearchAreas (AreaNId, Area)   
					SELECT Area.Area_NId, Area.Area_Name FROM UT_Area_XX Area 
					JOIN dbo.FN_GET_SPLITTED_LIST(@SearchArea, '','') List 
					ON Area.Area_NId = List.Value
				END

				
					--***************************************************************************************************************************************************
					-- Filling Indicator Table
					--***************************************************************************************************************************************************
				INSERT INTO #SearchIndicators (IndicatorNId, Indicator)
				SELECT Indicator.Indicator_NId, Indicator.Indicator_Name FROM UT_Indicator_XX Indicator 
				WHERE Indicator.Indicator_NId = @IndicatorNId
	
		

				INSERT INTO #ResultSet (IndicatorNId, UnitNId, Indicator, ICName, Unit)
				SELECT DISTINCT Data.Indicator_NId, Data.Unit_NId, #SearchIndicators.Indicator, #SearchIndicators.ICName, Unit.Unit_Name 
				FROM UT_Data Data
				JOIN #SearchIndicators ON Data.Indicator_NId = #SearchIndicators.IndicatorNId 
				JOIN UT_Unit_XX Unit ON Data.Unit_NId = Unit.Unit_NId 
				JOIN #SearchAreas ON Data.Area_NId = #SearchAreas.AreaNId 
				ORDER BY Data.Indicator_NId, Data.Unit_NId

				UPDATE #ResultSet SET DefaultSGNId = IUS.Subgroup_Val_NId , DefaultSG = SGV.Subgroup_Val
				FROM UT_Indicator_Unit_Subgroup IUS, UT_Subgroup_Vals_XX SGV 
				WHERE IndicatorNId = IUS.Indicator_NId AND UnitNId = IUS.Unit_NId 
				AND IUS.IsDefaultSubgroup = 1 AND IUS.Subgroup_Val_NId = SGV.Subgroup_Val_NId

				SELECT @Count = 0, @Max = COUNT(*) FROM #ResultSet			
			
				WHILE(@Count < @Max)
				BEGIN
					SET @Count = @Count + 1;
					
					SELECT @TempIndicatorNId = IndicatorNId, @TempUnitNId = UnitNId, @TempDefaultSGNId = DefaultSGNId,  
					@TempAreaCount = 0, @TempSGCount = 0, @TempSourceCount = 0, @TempTPCount = 0, @TempDVCount = 0,
					@TempAreaNIds = '','', @TempSGNIds = '','', @TempSourceNIds = '','', @TempTPNIds = '','', @TempDVNIds = '','', 
					@TempDVSeries = '','', @TempAreaSeries = '',''
					FROM #ResultSet WHERE Id = @Count

					SELECT 
						@TempAreaNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Area_NId AS NVARCHAR(10))+ '','', @TempAreaNIds) = 0) 
							THEN 
								@TempAreaNIds + CAST(Tbl.Area_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempAreaNIds
						END ,
						@TempSGNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Subgroup_Val_NId AS NVARCHAR(10))+ '','', @TempSGNIds) = 0) 
							THEN 
								@TempSGNIds + CAST(Tbl.Subgroup_Val_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempSGNIds
						END ,
						@TempSourceNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Source_NId AS NVARCHAR(10))+ '','', @TempSourceNIds) = 0) 
							THEN 
								@TempSourceNIds + CAST(Tbl.Source_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempSourceNIds
						END ,
						@TempTPNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.TimePeriod_NId AS NVARCHAR(10))+ '','', @TempTPNIds) = 0) 
							THEN 
								@TempTPNIds + CAST(Tbl.TimePeriod_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempTPNIds
						END ,
						@TempDVNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Data_NId AS NVARCHAR(10))+ '','', @TempDVNIds) = 0) 
							THEN 
								@TempDVNIds + CAST(Tbl.Data_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempDVNIds
						END 
						,
						@TempDVSeries = 
						CASE 
							WHEN (Tbl.Subgroup_Val_NId = @TempDefaultSGNId AND Tbl.isMRD = 1) 
							THEN 
								@TempDVSeries + ISNULL(dbo.CustomCast(Tbl.Data_Value), Tbl.Textual_Data_Value) + ''|''
							ELSE
								@TempDVSeries
						END ,
						@TempAreaSeries = 
						CASE 
							WHEN (Tbl.Subgroup_Val_NId = @TempDefaultSGNId AND Tbl.isMRD = 1) 
							THEN 
								@TempAreaSeries + Tbl.Area + ''|''
							ELSE
								@TempAreaSeries
						END 
					FROM 
					(
						SELECT TOP 2147483647 Data.Data_NId, Data.Textual_Data_Value, Data.Data_Value, Data.Subgroup_Val_NId, Data.Source_NId, Data.TimePeriod_NId, Data.Area_NId, Data.isMRD, Areas.Area 
						FROM UT_DATA Data JOIN #SearchAreas Areas ON Data.Area_NId = Areas.AreaNId
						WHERE Data.Indicator_NId = @TempIndicatorNId AND Data.Unit_NId = @TempUnitNId
						AND Data.Subgroup_Val_NId = @TempDefaultSGNId 						
					) Tbl 
					
					IF (LEN(@TempAreaNIds) > 1)
					BEGIN
						SELECT @TempAreaNIds = SUBSTRING(@TempAreaNIds, 2, LEN(@TempAreaNIds) - 2)
						SET @TempAreaCount = LEN(@TempAreaNIds) - LEN(REPLACE(@TempAreaNIds, '','', '''')) + 1
					END	
					ELSE IF (LEN(@TempAreaNIds) = 1)
					BEGIN
						SELECT @TempAreaNIds = ''''
						SET @TempAreaCount = 0
					END	
				
					IF (LEN(@TempSGNIds) > 1)
					BEGIN
						SELECT @TempSGNIds = SUBSTRING(@TempSGNIds, 2, LEN(@TempSGNIds) - 2)
						SET @TempSGCount = LEN(@TempSGNIds) - LEN(REPLACE(@TempSGNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempSGNIds) = 1)
					BEGIN
						SELECT @TempSGNIds = ''''
						SET @TempSGCount = 0
					END	

					IF (LEN(@TempSourceNIds) > 1)
					BEGIN
						SELECT @TempSourceNIds = SUBSTRING(@TempSourceNIds, 2, LEN(@TempSourceNIds) - 2)
						SET @TempSourceCount = LEN(@TempSourceNIds) - LEN(REPLACE(@TempSourceNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempSourceNIds) = 1)
					BEGIN
						SELECT @TempSourceNIds = ''''
						SET @TempSourceCount = 0
					END	

					IF (LEN(@TempTPNIds) > 1)
					BEGIN
						SELECT @TempTPNIds = SUBSTRING(@TempTPNIds, 2, LEN(@TempTPNIds) - 2)
						SET @TempTPCount = LEN(@TempTPNIds) - LEN(REPLACE(@TempTPNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempTPNIds) = 1)
					BEGIN
						SELECT @TempTPNIds = ''''
						SET @TempTPCount = 0
					END	

					IF (LEN(@TempDVNIds) > 1)
					BEGIN
						SELECT @TempDVNIds = SUBSTRING(@TempDVNIds, 2, LEN(@TempDVNIds) - 2)
						SET @TempDVCount = LEN(@TempDVNIds) - LEN(REPLACE(@TempDVNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempDVNIds) = 1)
					BEGIN
						SELECT @TempDVNIds = ''''
						SET @TempDVCount = 0
					END	

					IF (LEN(@TempDVSeries) > 1)
					BEGIN
						SELECT @TempDVSeries = SUBSTRING(@TempDVSeries, 2, LEN(@TempDVSeries) - 2)
					END
					ELSE IF (LEN(@TempDVSeries) = 1)
					BEGIN
						SELECT @TempDVSeries = ''''
					END

					IF (LEN(@TempAreaSeries) > 1)
					BEGIN
						SELECT @TempAreaSeries = SUBSTRING(@TempAreaSeries, 2, LEN(@TempAreaSeries) - 2)
					END
					ELSE IF (LEN(@TempAreaSeries) = 1)
					BEGIN
						SELECT @TempAreaSeries = ''''
					END	

					SET @TempDimensions = [dbo].[fn_get_dimensions_IU_XX](@TempIndicatorNId, @TempUnitNId)

					UPDATE #ResultSet
					SET	AreaCount = @TempAreaCount, SGCount = @TempSGCount, SourceCount = @TempSourceCount, TPCount = @TempTPCount, DVCount = @TempDVCount,
						AreaNIds = @TempAreaNIds, SGNIds = @TempSGNIds, SourceNIds = @TempSourceNIds, TPNIds = @TempTPNIds, DVNIds = @TempDVNIds, 
						DVSeries = @TempAreaSeries + '':'' + @TempDVSeries , Dimensions = @TempDimensions,BlockAreaParentNId=@BlockAreaParentNID
					WHERE IndicatorNId = @TempIndicatorNId AND UnitNId = @TempUnitNId
				END
				
				INSERT INTO DI_SEARCH_RESULTS
					SELECT @SearchLanguage, 
					   IndicatorNid, UnitNId, @SearchAreasCopy, @IsAreaNumeric,  
					   Indicator, Unit, '''', 
					   CAST(DefaultSGNId AS NVARCHAR) + ''[@@@@]'' + DefaultSG, '''', '''',
					   AreaCount, SGCount, SourceCount, TPCount, DVCount, 
					   AreaNIds, SGNIds, SourceNIds, TPNIds, DVNIds, DVSeries, Dimensions,BlockAreaParentNId
					FROM #ResultSet R
					WHERE  NOT EXISTS (	SELECT TOP 1 NId FROM DI_SEARCH_RESULTS C 
								WHERE C.IndicatorNId = R.IndicatorNid
								AND C.AreaNId = @SearchAreasCopy AND SearchLanguage = @SearchLanguage)
					AND DVCount > 0

				DROP TABLE #ResultSet
				DROP TABLE #SearchAreas
				DROP TABLE #SearchIndicators
		
	
END






' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'

ALTER PROCEDURE [dbo].[SP_CREATE_CACHE_RESULT_IU_QSA_SPECIFIC_XX]
	@SearchArea			NVARCHAR(MAX),
	@IndicatorNId		INT			
AS
BEGIN
	DECLARE @SQLSearch					NVARCHAR(MAX);

	DECLARE @Max						INT;
	DECLARE @Count						INT;
	DECLARE @TempIndicatorNId			INT;
	DECLARE @TempUnitNId				INT;
	DECLARE @TempDefaultSGNId			INT;
	DECLARE @TempAreaCount				INT;
	DECLARE @TempSGCount				INT;
	DECLARE @TempSourceCount			INT;
	DECLARE @TempTPCount				INT;
	DECLARE @TempDVCount				INT;
	DECLARE @TempAreaNIds				NVARCHAR(MAX);
	DECLARE @TempSGNIds					NVARCHAR(MAX);
	DECLARE @TempSourceNIds				NVARCHAR(MAX);
	DECLARE @TempTPNIds					NVARCHAR(MAX);
	DECLARE @TempDVNIds					NVARCHAR(MAX);
	DECLARE @TempDVSeries				NVARCHAR(MAX);
	DECLARE @TempDimensions				NVARCHAR(MAX);
	DECLARE @TempAreaSeries				NVARCHAR(MAX);
	DECLARE @SearchLanguage				NVARCHAR(MAX);
	DECLARE @SearchAreasCopy			NVARCHAR(MAX);
	DECLARE @BlockAreaParentNID			INT;
	DECLARE @IsAreaNumeric				bit;


	SET @SearchLanguage = ''_XX'';
	SELECT @SearchLanguage = SUBSTRING(@SearchLanguage, 2, LEN(@SearchLanguage) - 1)
		

		SET @SearchAreasCopy = @SearchArea
		SET @IsAreaNumeric = 0
		SELECT @SearchArea = dbo.fn_get_normalized_area_nids_XX(@SearchArea)
		SELECT @BlockAreaParentNID = dbo.FN_GET_AreaNIdGeneric_XX(@SearchAreasCopy)
				CREATE TABLE #ResultSet
				(
					Id							INT IDENTITY(1,1) PRIMARY KEY,
					IndicatorNId				INT DEFAULT 0,
					UnitNId						INT DEFAULT 0,
					Indicator					NVARCHAR(MAX) DEFAULT ''''  ,
					ICName						NVARCHAR(MAX) DEFAULT ''''  ,
					Unit						NVARCHAR(MAX) DEFAULT ''''  ,
					DefaultSGNId				INT DEFAULT 0,
					DefaultSG					NVARCHAR(MAX) DEFAULT ''''  ,
					AreaCount					INT DEFAULT 0,
					SGCount						INT DEFAULT 0,
					SourceCount					INT DEFAULT 0,
					TPCount						INT DEFAULT 0,
					DVCount						INT DEFAULT 0,
					AreaNIds					NVARCHAR(MAX) DEFAULT ''''  ,
					SGNIds						NVARCHAR(MAX) DEFAULT ''''  ,
					SourceNIds					NVARCHAR(MAX) DEFAULT ''''  ,
					TPNIds						NVARCHAR(MAX) DEFAULT ''''  ,
					DVNIds						NVARCHAR(MAX) DEFAULT ''''  ,
					DVSeries					NVARCHAR(MAX) DEFAULT ''''  ,
					Dimensions					NVARCHAR(MAX) DEFAULT '''',
					BlockAreaParentNId			INT DEFAULT 0
				)

				CREATE TABLE #SearchAreas 
				(
					Id							INT IDENTITY(1,1) PRIMARY KEY,
					AreaNId						INT DEFAULT 0,
					Area						NVARCHAR(MAX) DEFAULT '''' NOT NULL
				)

				CREATE TABLE #SearchIndicators 
				(
					Id							INT IDENTITY(1,1) PRIMARY KEY,
					IndicatorNId				INT DEFAULT 0,
					Indicator					NVARCHAR(MAX) DEFAULT '''' NOT NULL,
					ICName						NVARCHAR(MAX) DEFAULT '''' NOT NULL
				)

				IF (@SearchArea IS NOT NULL AND @SearchArea <> '''')
				BEGIN
					--***************************************************************************************************************************************************
					-- Filling Area Table
					--***************************************************************************************************************************************************
					INSERT INTO #SearchAreas (AreaNId, Area)   
					SELECT Area.Area_NId, Area.Area_Name FROM UT_Area_XX Area 
					JOIN dbo.FN_GET_SPLITTED_LIST(@SearchArea, '','') List 
					ON Area.Area_NId = List.Value
				END

				
					--***************************************************************************************************************************************************
					-- Filling Indicator Table
					--***************************************************************************************************************************************************
				INSERT INTO #SearchIndicators (IndicatorNId, Indicator)
				SELECT Indicator.Indicator_NId, Indicator.Indicator_Name FROM UT_Indicator_XX Indicator 
				WHERE Indicator.Indicator_NId = @IndicatorNId
	
		

				INSERT INTO #ResultSet (IndicatorNId, UnitNId, Indicator, ICName, Unit)
				SELECT DISTINCT Data.Indicator_NId, Data.Unit_NId, #SearchIndicators.Indicator, #SearchIndicators.ICName, Unit.Unit_Name 
				FROM UT_Data Data
				JOIN #SearchIndicators ON Data.Indicator_NId = #SearchIndicators.IndicatorNId 
				JOIN UT_Unit_XX Unit ON Data.Unit_NId = Unit.Unit_NId 
				JOIN #SearchAreas ON Data.Area_NId = #SearchAreas.AreaNId 
				ORDER BY Data.Indicator_NId, Data.Unit_NId

				UPDATE #ResultSet SET DefaultSGNId = IUS.Subgroup_Val_NId , DefaultSG = SGV.Subgroup_Val
				FROM UT_Indicator_Unit_Subgroup IUS, UT_Subgroup_Vals_XX SGV 
				WHERE IndicatorNId = IUS.Indicator_NId AND UnitNId = IUS.Unit_NId 
				AND IUS.IsDefaultSubgroup = 1 AND IUS.Subgroup_Val_NId = SGV.Subgroup_Val_NId

				SELECT @Count = 0, @Max = COUNT(*) FROM #ResultSet			
			
				WHILE(@Count < @Max)
				BEGIN
					SET @Count = @Count + 1;
					
					SELECT @TempIndicatorNId = IndicatorNId, @TempUnitNId = UnitNId, @TempDefaultSGNId = DefaultSGNId,  
					@TempAreaCount = 0, @TempSGCount = 0, @TempSourceCount = 0, @TempTPCount = 0, @TempDVCount = 0,
					@TempAreaNIds = '','', @TempSGNIds = '','', @TempSourceNIds = '','', @TempTPNIds = '','', @TempDVNIds = '','', 
					@TempDVSeries = '','', @TempAreaSeries = '',''
					FROM #ResultSet WHERE Id = @Count

					SELECT 
						@TempAreaNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Area_NId AS NVARCHAR(10))+ '','', @TempAreaNIds) = 0) 
							THEN 
								@TempAreaNIds + CAST(Tbl.Area_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempAreaNIds
						END ,
						@TempSGNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Subgroup_Val_NId AS NVARCHAR(10))+ '','', @TempSGNIds) = 0) 
							THEN 
								@TempSGNIds + CAST(Tbl.Subgroup_Val_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempSGNIds
						END ,
						@TempSourceNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Source_NId AS NVARCHAR(10))+ '','', @TempSourceNIds) = 0) 
							THEN 
								@TempSourceNIds + CAST(Tbl.Source_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempSourceNIds
						END ,
						@TempTPNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.TimePeriod_NId AS NVARCHAR(10))+ '','', @TempTPNIds) = 0) 
							THEN 
								@TempTPNIds + CAST(Tbl.TimePeriod_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempTPNIds
						END ,
						@TempDVNIds = 
						CASE 
							WHEN (CHARINDEX( '','' + CAST(Tbl.Data_NId AS NVARCHAR(10))+ '','', @TempDVNIds) = 0) 
							THEN 
								@TempDVNIds + CAST(Tbl.Data_NId AS NVARCHAR(10)) + '',''
							ELSE
								@TempDVNIds
						END 
						,
						@TempDVSeries = 
						CASE 
							WHEN (Tbl.Subgroup_Val_NId = @TempDefaultSGNId AND Tbl.isMRD = 1) 
							THEN 
								@TempDVSeries + ISNULL(dbo.CustomCast(Tbl.Data_Value), Tbl.Textual_Data_Value) + ''|''
							ELSE
								@TempDVSeries
						END ,
						@TempAreaSeries = 
						CASE 
							WHEN (Tbl.Subgroup_Val_NId = @TempDefaultSGNId AND Tbl.isMRD = 1) 
							THEN 
								@TempAreaSeries + Tbl.Area + ''|''
							ELSE
								@TempAreaSeries
						END 
					FROM 
					(
						SELECT TOP 2147483647 Data.Data_NId, Data.Textual_Data_Value, Data.Data_Value, Data.Subgroup_Val_NId, Data.Source_NId, Data.TimePeriod_NId, Data.Area_NId, Data.isMRD, Areas.Area 
						FROM UT_DATA Data JOIN #SearchAreas Areas ON Data.Area_NId = Areas.AreaNId
						WHERE Data.Indicator_NId = @TempIndicatorNId AND Data.Unit_NId = @TempUnitNId
						AND Data.Subgroup_Val_NId = @TempDefaultSGNId 						
					) Tbl 
					
					IF (LEN(@TempAreaNIds) > 1)
					BEGIN
						SELECT @TempAreaNIds = SUBSTRING(@TempAreaNIds, 2, LEN(@TempAreaNIds) - 2)
						SET @TempAreaCount = LEN(@TempAreaNIds) - LEN(REPLACE(@TempAreaNIds, '','', '''')) + 1
					END	
					ELSE IF (LEN(@TempAreaNIds) = 1)
					BEGIN
						SELECT @TempAreaNIds = ''''
						SET @TempAreaCount = 0
					END	
				
					IF (LEN(@TempSGNIds) > 1)
					BEGIN
						SELECT @TempSGNIds = SUBSTRING(@TempSGNIds, 2, LEN(@TempSGNIds) - 2)
						SET @TempSGCount = LEN(@TempSGNIds) - LEN(REPLACE(@TempSGNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempSGNIds) = 1)
					BEGIN
						SELECT @TempSGNIds = ''''
						SET @TempSGCount = 0
					END	

					IF (LEN(@TempSourceNIds) > 1)
					BEGIN
						SELECT @TempSourceNIds = SUBSTRING(@TempSourceNIds, 2, LEN(@TempSourceNIds) - 2)
						SET @TempSourceCount = LEN(@TempSourceNIds) - LEN(REPLACE(@TempSourceNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempSourceNIds) = 1)
					BEGIN
						SELECT @TempSourceNIds = ''''
						SET @TempSourceCount = 0
					END	

					IF (LEN(@TempTPNIds) > 1)
					BEGIN
						SELECT @TempTPNIds = SUBSTRING(@TempTPNIds, 2, LEN(@TempTPNIds) - 2)
						SET @TempTPCount = LEN(@TempTPNIds) - LEN(REPLACE(@TempTPNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempTPNIds) = 1)
					BEGIN
						SELECT @TempTPNIds = ''''
						SET @TempTPCount = 0
					END	

					IF (LEN(@TempDVNIds) > 1)
					BEGIN
						SELECT @TempDVNIds = SUBSTRING(@TempDVNIds, 2, LEN(@TempDVNIds) - 2)
						SET @TempDVCount = LEN(@TempDVNIds) - LEN(REPLACE(@TempDVNIds, '','', '''')) + 1
					END
					ELSE IF (LEN(@TempDVNIds) = 1)
					BEGIN
						SELECT @TempDVNIds = ''''
						SET @TempDVCount = 0
					END	

					IF (LEN(@TempDVSeries) > 1)
					BEGIN
						SELECT @TempDVSeries = SUBSTRING(@TempDVSeries, 2, LEN(@TempDVSeries) - 2)
					END
					ELSE IF (LEN(@TempDVSeries) = 1)
					BEGIN
						SELECT @TempDVSeries = ''''
					END

					IF (LEN(@TempAreaSeries) > 1)
					BEGIN
						SELECT @TempAreaSeries = SUBSTRING(@TempAreaSeries, 2, LEN(@TempAreaSeries) - 2)
					END
					ELSE IF (LEN(@TempAreaSeries) = 1)
					BEGIN
						SELECT @TempAreaSeries = ''''
					END	

					SET @TempDimensions = [dbo].[fn_get_dimensions_IU_XX](@TempIndicatorNId, @TempUnitNId)

					UPDATE #ResultSet
					SET	AreaCount = @TempAreaCount, SGCount = @TempSGCount, SourceCount = @TempSourceCount, TPCount = @TempTPCount, DVCount = @TempDVCount,
						AreaNIds = @TempAreaNIds, SGNIds = @TempSGNIds, SourceNIds = @TempSourceNIds, TPNIds = @TempTPNIds, DVNIds = @TempDVNIds, 
						DVSeries = @TempAreaSeries + '':'' + @TempDVSeries , Dimensions = @TempDimensions,BlockAreaParentNId=@BlockAreaParentNID
					WHERE IndicatorNId = @TempIndicatorNId AND UnitNId = @TempUnitNId
				END
				
				INSERT INTO DI_SEARCH_RESULTS
					SELECT @SearchLanguage, 
					   IndicatorNid, UnitNId, @SearchAreasCopy, @IsAreaNumeric,  
					   Indicator, Unit, '''', 
					   CAST(DefaultSGNId AS NVARCHAR) + ''[@@@@]'' + DefaultSG, '''', '''',
					   AreaCount, SGCount, SourceCount, TPCount, DVCount, 
					   AreaNIds, SGNIds, SourceNIds, TPNIds, DVNIds, DVSeries, Dimensions,BlockAreaParentNId
					FROM #ResultSet R
					WHERE  NOT EXISTS (	SELECT TOP 1 NId FROM DI_SEARCH_RESULTS C 
								WHERE C.IndicatorNId = R.IndicatorNid
								AND C.AreaNId = @SearchAreasCopy AND SearchLanguage = @SearchLanguage)
					AND DVCount > 0

				DROP TABLE #ResultSet
				DROP TABLE #SearchAreas
				DROP TABLE #SearchIndicators
		
	
END


' 
END

--SPSeparator--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CREATE_CACHE_RESULT_IU_QSA_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[SP_CREATE_CACHE_RESULT_IU_QSA_XX]
	@SearchArea			NVARCHAR(MAX),
	@IndicatorNId		INT,
	@AdditionalParams		VARCHAR(100)	
AS
BEGIN
	IF(@AdditionalParams = ''GENERAL'')
	BEGIN
		EXEC SP_CREATE_CACHE_RESULT_IU_QSA_GENERAL_XX @SearchArea,@IndicatorNId
	END
	ELSE IF(@AdditionalParams = ''SPECIFIC'')
	BEGIN
		EXEC SP_CREATE_CACHE_RESULT_IU_QSA_SPECIFIC_XX  @SearchArea,@IndicatorNId
	END
END

' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'

ALTER PROCEDURE [dbo].[SP_CREATE_CACHE_RESULT_IU_QSA_XX]
	@SearchArea			NVARCHAR(MAX),
	@IndicatorNId		INT,
	@AdditionalParams		VARCHAR(100)	
AS
BEGIN
	IF(@AdditionalParams = ''GENERAL'')
	BEGIN
		EXEC SP_CREATE_CACHE_RESULT_IU_QSA_GENERAL_XX @SearchArea,@IndicatorNId
	END
	ELSE IF(@AdditionalParams = ''SPECIFIC'')
	BEGIN
		EXEC SP_CREATE_CACHE_RESULT_IU_QSA_SPECIFIC_XX  @SearchArea,@IndicatorNId
	END
END
' 
END

--SpSeparator--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CREATE_CACHE_RESULT_BL_GENERAL_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[SP_CREATE_CACHE_RESULT_BL_GENERAL_XX]
@SearchArea			NVARCHAR(MAX)
AS
BEGIN

	DECLARE @SearchLanguage				VARCHAR(MAX);

	SET @SearchLanguage = ''_XX'';
	SELECT @SearchLanguage = SUBSTRING(@SearchLanguage, 2, LEN(@SearchLanguage) - 1)

	-- nText datatype doesn''t support comparision operators like ''='' or ''is'' or ''not''
	-- So we used comma(,) for identifying whether area is a block or not. 
	-- FACT : Each block contains more than one area nids which are comma separated.

		EXEC SP_CREATE_CACHE_RESULT_IU_GENERAL_XX @SearchArea	

		DECLARE @TMP_AREA_NID INT
		DECLARE @TMP_AREA_NAME NVARCHAR(MAX)

		SET @TMP_AREA_NID = CAST(REPLACE(@SearchArea	, ''BL_'', '''') AS INT)
		
		SET @TMP_AREA_NAME = (SELECT TOP 1 AREA_NAME FROM UT_AREA_XX WHERE AREA_NID = @TMP_AREA_NID)

		UPDATE DI_SEARCH_RESULTS 
		SET AREA = @TMP_AREA_NAME
		WHERE AREANID = @SearchArea	 AND [SearchLanguage] = @SearchLanguage

END

' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[SP_CREATE_CACHE_RESULT_BL_GENERAL_XX]
@SearchArea			NVARCHAR(MAX)
AS
BEGIN

	DECLARE @SearchLanguage				VARCHAR(MAX);

	SET @SearchLanguage = ''_XX'';
	SELECT @SearchLanguage = SUBSTRING(@SearchLanguage, 2, LEN(@SearchLanguage) - 1)

	-- nText datatype doesn''t support comparision operators like ''='' or ''is'' or ''not''
	-- So we used comma(,) for identifying whether area is a block or not. 
	-- FACT : Each block contains more than one area nids which are comma separated.

		EXEC SP_CREATE_CACHE_RESULT_IU_GENERAL_XX @SearchArea	

		DECLARE @TMP_AREA_NID INT
		DECLARE @TMP_AREA_NAME NVARCHAR(MAX)

		SET @TMP_AREA_NID = CAST(REPLACE(@SearchArea	, ''BL_'', '''') AS INT)
		
		SET @TMP_AREA_NAME = (SELECT TOP 1 AREA_NAME FROM UT_AREA_XX WHERE AREA_NID = @TMP_AREA_NID)

		UPDATE DI_SEARCH_RESULTS 
		SET AREA = @TMP_AREA_NAME
		WHERE AREANID = @SearchArea	 AND [SearchLanguage] = @SearchLanguage

END


' 
END


--SPSEPARATOR--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CREATE_CACHE_RESULT_BL_SPECIFIC_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[SP_CREATE_CACHE_RESULT_BL_SPECIFIC_XX]
@SearchArea			NVARCHAR(MAX)
AS
BEGIN

	
	DECLARE @SearchLanguage				VARCHAR(MAX);

	SET @SearchLanguage = ''_XX'';
	SELECT @SearchLanguage = SUBSTRING(@SearchLanguage, 2, LEN(@SearchLanguage) - 1)
	

	-- nText datatype doesn''t support comparision operators like ''='' or ''is'' or ''not''
	-- So we used comma(,) for identifying whether area is a block or not. 
	-- FACT : Each block contains more than one area nids which are comma separated.

		EXEC SP_CREATE_CACHE_RESULT_IU_SPECIFIC_XX @SearchArea	

		DECLARE @TMP_AREA_NID INT
		DECLARE @TMP_AREA_NAME NVARCHAR(MAX)

		SET @TMP_AREA_NID = CAST(REPLACE(@SearchArea, ''BL_'', '''') AS INT)
		
		SET @TMP_AREA_NAME = (SELECT TOP 1 AREA_NAME FROM UT_AREA_XX WHERE AREA_NID = @TMP_AREA_NID)

		UPDATE DI_SEARCH_RESULTS 
		SET AREA = @TMP_AREA_NAME
		WHERE AREANID = @SearchArea AND [SearchLanguage] = @SearchLanguage

END

' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[SP_CREATE_CACHE_RESULT_BL_SPECIFIC_XX]
@SearchArea			NVARCHAR(MAX)
AS
BEGIN

	
	DECLARE @SearchLanguage				VARCHAR(MAX);

	SET @SearchLanguage = ''_XX'';
	SELECT @SearchLanguage = SUBSTRING(@SearchLanguage, 2, LEN(@SearchLanguage) - 1)
	

	-- nText datatype doesn''t support comparision operators like ''='' or ''is'' or ''not''
	-- So we used comma(,) for identifying whether area is a block or not. 
	-- FACT : Each block contains more than one area nids which are comma separated.

		EXEC SP_CREATE_CACHE_RESULT_IU_SPECIFIC_XX @SearchArea	

		DECLARE @TMP_AREA_NID INT
		DECLARE @TMP_AREA_NAME NVARCHAR(MAX)

		SET @TMP_AREA_NID = CAST(REPLACE(@SearchArea, ''BL_'', '''') AS INT)
		
		SET @TMP_AREA_NAME = (SELECT TOP 1 AREA_NAME FROM UT_AREA_XX WHERE AREA_NID = @TMP_AREA_NID)

		UPDATE DI_SEARCH_RESULTS 
		SET AREA = @TMP_AREA_NAME
		WHERE AREANID = @SearchArea AND [SearchLanguage] = @SearchLanguage

END

' 
END


--SPSEPARATOR--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CREATE_CACHE_RESULT_BL_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[SP_CREATE_CACHE_RESULT_BL_XX]
@SearchArea			NVARCHAR(MAX),
@AdditionalParams		VARCHAR(100)
AS
BEGIN

	IF(@AdditionalParams = ''GENERAL'')
	BEGIN
		EXEC SP_CREATE_CACHE_RESULT_BL_GENERAL_XX @SearchArea
	END
	ELSE IF(@AdditionalParams = ''SPECIFIC'')
	BEGIN
		EXEC SP_CREATE_CACHE_RESULT_BL_SPECIFIC_XX @SearchArea
	END
	

END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[SP_CREATE_CACHE_RESULT_BL_XX]
@SearchArea			NVARCHAR(MAX),
@AdditionalParams		VARCHAR(100)
AS
BEGIN

	IF(@AdditionalParams = ''GENERAL'')
	BEGIN
		EXEC SP_CREATE_CACHE_RESULT_BL_GENERAL_XX @SearchArea
	END
	ELSE IF(@AdditionalParams = ''SPECIFIC'')
	BEGIN
		EXEC SP_CREATE_CACHE_RESULT_BL_SPECIFIC_XX @SearchArea
	END
END
' 
END


--SPSEPARATOR--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GET_START_END_YEARS]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'create PROC [dbo].[SP_GET_START_END_YEARS]
AS
BEGIN
	SELECT YEAR(MIN(STARTDATE)), YEAR(MAX(ENDDATE)) FROM UT_TIMEPERIOD
END
' 
END


--SPSEPARATOR--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GET_TABLE_DATA]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROC [dbo].[SP_GET_TABLE_DATA]
	@TAB_NAME NVARCHAR(200)
AS	
BEGIN
	DECLARE @SQL NVARCHAR(4000)
	
	SELECT @SQL = ''SELECT * FROM '' + @TAB_NAME

	EXEC (@SQL)
END
' 
END


--SPSEPARATOR--

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GET_CATALOG_CACHE_RESULTS_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROC [dbo].[SP_GET_CATALOG_CACHE_RESULTS_XX]
(
@AREAS NVARCHAR(MAX),
@INDICATORS NVARCHAR(MAX),
@AREA_NIDS NVARCHAR(MAX),
@IUS_NIDS NVARCHAR(MAX)
)
AS
BEGIN

	DECLARE @I INT
	DECLARE @MAX INT

	SET @I = 0

	CREATE TABLE #SEARCHED_AREA_INDICATOR_COMBINATIONS
	(
		ID INT IDENTITY(0,1),
		AREA NVARCHAR(MAX),
		INDICATOR NVARCHAR(MAX)
	)
	CREATE TABLE #CACHE_AREA_INDICATOR_NIDS
	(
		ID INT IDENTITY(0,1),
		AREA_NID NVARCHAR(MAX),
		INDICATOR_NID NVARCHAR(MAX)
	)
	
	
	-- Case when areas are not provided from free text, only topics are searched by free text
	-- In such case, use AreaNIds(also QS) for getting default Areas
	IF(@AREAS = '''')
	BEGIN	
	
		INSERT INTO #SEARCHED_AREA_INDICATOR_COMBINATIONS(AREA, INDICATOR)
		SELECT A.ITEMS AS ''AREA'', I.ITEMS AS ''INDICATORS''
		FROM DBO.SPLIT(@area_nids, '','') A, DBO.SPLIT(@INDICATORS, '','') I
		
		SELECT DISTINCT NId,	
		SearchLanguage,
		C.IndicatorNId,
		UnitNId,
		C.AreaNId,
		IsAreaNumeric,
		C.Indicator,
		Unit,
		C.Area,
		DefaultSG,
		MRDTP,
		MRD,
		AreaCount,
		SGCount,
		SourceCount,
		TPCount,
		DVCount,
		AreaNIds,
		SGNIds,
		SourceNIds,
		TPNIds,
		DVSeries,
		Dimensions
		FROM DBO.DI_SEARCH_RESULTS C, -- C = CACHE TABLE
		#SEARCHED_AREA_INDICATOR_COMBINATIONS IA 
		WHERE C.INDICATOR like ''%'' + LTRIM(RTRIM(IA.INDICATOR)) + ''%''
		AND C.AREANID = IA.AREA
		ORDER BY C.AREANID DESC
	
	END -- END of IF(@AREAS = '''')
	
	-- Case when topics are not provided from free text, only areas are searched by free text	
	-- In such case, use IUSNIds for getting default IndicatorNIds
	IF(@INDICATORS = '''')
	BEGIN
	
		INSERT INTO #SEARCHED_AREA_INDICATOR_COMBINATIONS(AREA, INDICATOR)
		SELECT A.ITEMS AS ''AREA'', IUS.Indicator_NId AS ''INDICATORS''
		FROM DBO.SPLIT(@AREAS, '','') A, ut_indicator_unit_subgroup IUS
		WHERE IUS.IUSNId in 
			(SELECT I.ITEMS FROM DBO.SPLIT(@IUS_NIDS, '','') I)
		
		SELECT DISTINCT NId,	
		SearchLanguage,
		C.IndicatorNId,
		UnitNId,
		C.AreaNId,
		IsAreaNumeric,
		C.Indicator,
		Unit,
		C.Area,
		DefaultSG,
		MRDTP,
		MRD,
		AreaCount,
		SGCount,
		SourceCount,
		TPCount,
		DVCount,
		AreaNIds,
		SGNIds,
		SourceNIds,
		TPNIds,
		DVSeries,
		Dimensions
		FROM DBO.DI_SEARCH_RESULTS C, -- C = CACHE TABLE
		#SEARCHED_AREA_INDICATOR_COMBINATIONS IA 
		WHERE C.INDICATORNID = CAST(IA.INDICATOR AS INT)
		AND C.AREA like ''%'' + LTRIM(RTRIM(IA.AREA)) + ''%''
		ORDER BY C.AREANID DESC
	
	END -- END of IF(@INDICATORS = '''')
	

	-- Case when both areas & topics are provided from free text search
	IF(@AREAS != '''' AND @INDICATORS != '''')
	BEGIN

		INSERT INTO #SEARCHED_AREA_INDICATOR_COMBINATIONS(AREA, INDICATOR)
		SELECT A.ITEMS AS ''AREA'', I.ITEMS AS ''INDICATORS''
		FROM DBO.SPLIT(@AREAS, '','') A, DBO.SPLIT(@INDICATORS, '','') I
				
		SET @MAX = (SELECT COUNT(*) FROM #SEARCHED_AREA_INDICATOR_COMBINATIONS)

		WHILE(@I < @MAX)
		BEGIN
			
			DECLARE @TMP_AREA NVARCHAR(MAX)
			DECLARE @TMP_INDICATOR NVARCHAR(MAX)

			SET @TMP_AREA = (SELECT TOP 1 AREA FROM #SEARCHED_AREA_INDICATOR_COMBINATIONS WHERE ID = @I)
			SET @TMP_INDICATOR = (SELECT TOP 1 INDICATOR FROM #SEARCHED_AREA_INDICATOR_COMBINATIONS WHERE ID = @I)

			INSERT INTO #CACHE_AREA_INDICATOR_NIDS(AREA_NID, INDICATOR_NID)
			SELECT A.AREA_NID, I.INDICATOR_NID
			FROM DBO.UT_AREA_XX A, DBO.UT_INDICATOR_XX I
			WHERE A.AREA_NAME LIKE ''%'' + LTRIM(RTRIM(@TMP_AREA)) + ''%''
			AND I.INDICATOR_NAME LIKE ''%'' + LTRIM(RTRIM(@TMP_INDICATOR)) + ''%''
			
			SET @I = @I + 1
		END
		
		SELECT DISTINCT NId,	
		SearchLanguage,
		C.IndicatorNId,
		UnitNId,
		C.AreaNId,
		IsAreaNumeric,
		Indicator,
		Unit,
		Area,
		DefaultSG,
		MRDTP,
		MRD,
		AreaCount,
		SGCount,
		SourceCount,
		TPCount,
		DVCount,
		AreaNIds,
		SGNIds,
		SourceNIds,
		TPNIds,
		DVSeries,
		Dimensions
		FROM DBO.DI_SEARCH_RESULTS C, -- C = CACHE TABLE
		#CACHE_AREA_INDICATOR_NIDS IA 
		WHERE C.INDICATORNID = IA.INDICATOR_NID
		AND C.AREANID = IA.AREA_NID
		ORDER BY C.AREANID DESC
		
		END -- END of IF(@AREAS != '''' AND @INDICATORS != '''')

	DROP TABLE #SEARCHED_AREA_INDICATOR_COMBINATIONS
	DROP TABLE #CACHE_AREA_INDICATOR_NIDS

END



'
END

ELSE

BEGIN
EXEC dbo.sp_executesql @statement = N'

ALTER PROC [dbo].[SP_GET_CATALOG_CACHE_RESULTS_XX]
(
@AREAS NVARCHAR(MAX),
@INDICATORS NVARCHAR(MAX),
@AREA_NIDS NVARCHAR(MAX),
@IUS_NIDS NVARCHAR(MAX)
)
AS
BEGIN

	DECLARE @I INT
	DECLARE @MAX INT

	SET @I = 0

	CREATE TABLE #SEARCHED_AREA_INDICATOR_COMBINATIONS
	(
		ID INT IDENTITY(0,1),
		AREA NVARCHAR(MAX),
		INDICATOR NVARCHAR(MAX)
	)
	CREATE TABLE #CACHE_AREA_INDICATOR_NIDS
	(
		ID INT IDENTITY(0,1),
		AREA_NID NVARCHAR(MAX),
		INDICATOR_NID NVARCHAR(MAX)
	)
	
	
	-- Case when areas are not provided from free text, only topics are searched by free text
	-- In such case, use AreaNIds(also QS) for getting default Areas
	IF(@AREAS = '''')
	BEGIN	
	
		INSERT INTO #SEARCHED_AREA_INDICATOR_COMBINATIONS(AREA, INDICATOR)
		SELECT A.ITEMS AS ''AREA'', I.ITEMS AS ''INDICATORS''
		FROM DBO.SPLIT(@area_nids, '','') A, DBO.SPLIT(@INDICATORS, '','') I
		
		SELECT DISTINCT NId,	
		SearchLanguage,
		C.IndicatorNId,
		UnitNId,
		C.AreaNId,
		IsAreaNumeric,
		C.Indicator,
		Unit,
		C.Area,
		DefaultSG,
		MRDTP,
		MRD,
		AreaCount,
		SGCount,
		SourceCount,
		TPCount,
		DVCount,
		AreaNIds,
		SGNIds,
		SourceNIds,
		TPNIds,
		DVSeries,
		Dimensions
		FROM DBO.DI_SEARCH_RESULTS C, -- C = CACHE TABLE
		#SEARCHED_AREA_INDICATOR_COMBINATIONS IA 
		WHERE C.INDICATOR like ''%'' + LTRIM(RTRIM(IA.INDICATOR)) + ''%''
		AND C.AREANID = IA.AREA
		ORDER BY C.AREANID DESC
	
	END -- END of IF(@AREAS = '''')
	
	-- Case when topics are not provided from free text, only areas are searched by free text	
	-- In such case, use IUSNIds for getting default IndicatorNIds
	IF(@INDICATORS = '''')
	BEGIN
	
		INSERT INTO #SEARCHED_AREA_INDICATOR_COMBINATIONS(AREA, INDICATOR)
		SELECT A.ITEMS AS ''AREA'', IUS.Indicator_NId AS ''INDICATORS''
		FROM DBO.SPLIT(@AREAS, '','') A, ut_indicator_unit_subgroup IUS
		WHERE IUS.IUSNId in 
			(SELECT I.ITEMS FROM DBO.SPLIT(@IUS_NIDS, '','') I)
		
		SELECT DISTINCT NId,	
		SearchLanguage,
		C.IndicatorNId,
		UnitNId,
		C.AreaNId,
		IsAreaNumeric,
		C.Indicator,
		Unit,
		C.Area,
		DefaultSG,
		MRDTP,
		MRD,
		AreaCount,
		SGCount,
		SourceCount,
		TPCount,
		DVCount,
		AreaNIds,
		SGNIds,
		SourceNIds,
		TPNIds,
		DVSeries,
		Dimensions
		FROM DBO.DI_SEARCH_RESULTS C, -- C = CACHE TABLE
		#SEARCHED_AREA_INDICATOR_COMBINATIONS IA 
		WHERE C.INDICATORNID = CAST(IA.INDICATOR AS INT)
		AND C.AREA like ''%'' + LTRIM(RTRIM(IA.AREA)) + ''%''
		ORDER BY C.AREANID DESC
	
	END -- END of IF(@INDICATORS = '''')
	

	-- Case when both areas & topics are provided from free text search
	IF(@AREAS != '''' AND @INDICATORS != '''')
	BEGIN

		INSERT INTO #SEARCHED_AREA_INDICATOR_COMBINATIONS(AREA, INDICATOR)
		SELECT A.ITEMS AS ''AREA'', I.ITEMS AS ''INDICATORS''
		FROM DBO.SPLIT(@AREAS, '','') A, DBO.SPLIT(@INDICATORS, '','') I
				
		SET @MAX = (SELECT COUNT(*) FROM #SEARCHED_AREA_INDICATOR_COMBINATIONS)

		WHILE(@I < @MAX)
		BEGIN
			
			DECLARE @TMP_AREA NVARCHAR(MAX)
			DECLARE @TMP_INDICATOR NVARCHAR(MAX)

			SET @TMP_AREA = (SELECT TOP 1 AREA FROM #SEARCHED_AREA_INDICATOR_COMBINATIONS WHERE ID = @I)
			SET @TMP_INDICATOR = (SELECT TOP 1 INDICATOR FROM #SEARCHED_AREA_INDICATOR_COMBINATIONS WHERE ID = @I)

			INSERT INTO #CACHE_AREA_INDICATOR_NIDS(AREA_NID, INDICATOR_NID)
			SELECT A.AREA_NID, I.INDICATOR_NID
			FROM DBO.UT_AREA_XX A, DBO.UT_INDICATOR_XX I
			WHERE A.AREA_NAME LIKE ''%'' + LTRIM(RTRIM(@TMP_AREA)) + ''%''
			AND I.INDICATOR_NAME LIKE ''%'' + LTRIM(RTRIM(@TMP_INDICATOR)) + ''%''
			
			SET @I = @I + 1
		END
		
		SELECT DISTINCT NId,	
		SearchLanguage,
		C.IndicatorNId,
		UnitNId,
		C.AreaNId,
		IsAreaNumeric,
		Indicator,
		Unit,
		Area,
		DefaultSG,
		MRDTP,
		MRD,
		AreaCount,
		SGCount,
		SourceCount,
		TPCount,
		DVCount,
		AreaNIds,
		SGNIds,
		SourceNIds,
		TPNIds,
		DVSeries,
		Dimensions
		FROM DBO.DI_SEARCH_RESULTS C, -- C = CACHE TABLE
		#CACHE_AREA_INDICATOR_NIDS IA 
		WHERE C.INDICATORNID = IA.INDICATOR_NID
		AND C.AREANID = IA.AREA_NID
		ORDER BY C.AREANID DESC
		
		END -- END of IF(@AREAS != '''' AND @INDICATORS != '''')

	DROP TABLE #SEARCHED_AREA_INDICATOR_COMBINATIONS
	DROP TABLE #CACHE_AREA_INDICATOR_NIDS

END

' 
END

--SPSEPARATOR--

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_getAreaListByNidOrName_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_getAreaListByNidOrName_XX] (
@UserKey  nvarchar(max),
@type nvarchar(5))
AS
Begin
	/*SET NOCOUNT ON*/	
	if (@type = ''aid'')
	Begin
	SELECT area_id,area_name from ut_area_XX where (SOUNDEX(area_id) = SOUNDEX('' + @UserKey + ''))
	end
	else if (@type = ''aname'')
	Begin
	SELECT area_id,area_name from ut_area_XX where (SOUNDEX(area_name) = SOUNDEX('' + @UserKey + ''))
	end	
END' 
END

ELSE

BEGIN

EXEC dbo.sp_executesql @statement = N'ALTER PROCEDURE [dbo].[sp_getAreaListByNidOrName_XX] (
@type nvarchar(5),
@UserKey  nvarchar(max))
AS
Begin	
	/*SET NOCOUNT ON*/	
	if (@type = ''aid'')
	Begin
	SELECT area_id,area_name from ut_area_XX where (SOUNDEX(area_id) = SOUNDEX('' + @UserKey + ''))
	end
	else if (@type = ''aname'')
	Begin
	SELECT area_id,area_name from ut_area_XX where (SOUNDEX(area_name) = SOUNDEX('' + @UserKey + ''))
	end	
END'  
END



--SPSEPARATOR--

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[FN_GET_QS_AREA_FOR_BLOCKS_XX]') AND xtype in (N'FN', N'IF', N'TF'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE FUNCTION FN_GET_QS_AREA_FOR_BLOCKS_XX
(
	@AREA_NID NVARCHAR(MAX),
	@INDICATOR_NID INT
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @RESULT NVARCHAR(MAX)
	
	DECLARE @CACHE_COUNT INT
	DECLARE @IS_BLOCK INT
	DECLARE @TMP_AREA_NID NVARCHAR(MAX)
	
	DECLARE @LANG NVARCHAR(3)
	
	SET @RESULT = ''0''
	SET @CACHE_COUNT = 0
	SET @LANG = ''_XX''
	SELECT @LANG = SUBSTRING(@LANG, 2, LEN(@LANG) - 1)
	
	-- 0. IF ITS QS AREA, THEN RETURN QS_AREA FROM HERE AND DON''T GO FURTHER
	-- 1. CHECK FOR AREA AS BLOCK
	-- 2. IF ITS A BLOCK THEN CONCATINATE AREANID WITH ''BL_''
	-- 3. NOW SEARCH WITH ABOVE BLOCK  (BL_AREANID) IN CACHE RESULT
	-- 4. IF ANY RESULT IS FOUND THEN RETURN ''BL_AREANID'' FROM HERE
	-- 5. IF NO RESULT IS FOUND IN STEP 4, THEN GET AREAID & NEXT IMMEDIATE LEVEL OF THAT NUMERIC AREANID
	-- 6. NOW CONCATENATE ABOVE INFO IN FORM OF QS_IND_L2 AND RETURN IT

	IF(SUBSTRING(@AREA_NID,1,3) = ''QS_'')
	BEGIN
		RETURN @AREA_NID
	END

	SET @IS_BLOCK = (SELECT LEN(CAST(AREA_BLOCK AS NVARCHAR)) FROM UT_AREA_XX WHERE AREA_NID = @AREA_NID)
	
	IF(@IS_BLOCK != 0)
	BEGIN
		SET @TMP_AREA_NID = ''BL_'' + @AREA_NID 
		SET @CACHE_COUNT = (SELECT COUNT(*) FROM DBO.DI_SEARCH_RESULTS WHERE AREANID = @TMP_AREA_NID AND INDICATORNID = @INDICATOR_NID AND SEARCHLANGUAGE = @LANG)
		IF(@CACHE_COUNT > 0)
		BEGIN
			SET @RESULT = @TMP_AREA_NID
			RETURN @RESULT
		END
			
	END
	ELSE
	BEGIN
		SET @TMP_AREA_NID = (SELECT ''QS_'' + AREA_ID + ''_L'' + CAST((AREA_LEVEL + 1) AS NVARCHAR) FROM UT_AREA_XX WHERE AREA_NID = CAST(@AREA_NID AS INT)) 
		SET @CACHE_COUNT = (SELECT COUNT(*) FROM DBO.DI_SEARCH_RESULTS WHERE AREANID = @TMP_AREA_NID AND INDICATORNID = @INDICATOR_NID AND SEARCHLANGUAGE = @LANG)
		IF(@CACHE_COUNT > 0)
		BEGIN
			SET @RESULT = @TMP_AREA_NID
			RETURN @RESULT
		END
	END	
	
	RETURN @RESULT

END


' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'

ALTER FUNCTION FN_GET_QS_AREA_FOR_BLOCKS_XX
(
	@AREA_NID NVARCHAR(MAX),
	@INDICATOR_NID INT
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @RESULT NVARCHAR(MAX)
	
	DECLARE @CACHE_COUNT INT
	DECLARE @IS_BLOCK INT
	DECLARE @TMP_AREA_NID NVARCHAR(MAX)
	
	DECLARE @LANG NVARCHAR(3)
	
	SET @RESULT = ''0''
	SET @CACHE_COUNT = 0
	SET @LANG = ''_XX''
	SELECT @LANG = SUBSTRING(@LANG, 2, LEN(@LANG) - 1)
	
	-- 0. IF ITS QS AREA, THEN RETURN QS_AREA FROM HERE AND DON''T GO FURTHER
	-- 1. CHECK FOR AREA AS BLOCK
	-- 2. IF ITS A BLOCK THEN CONCATINATE AREANID WITH ''BL_''
	-- 3. NOW SEARCH WITH ABOVE BLOCK  (BL_AREANID) IN CACHE RESULT
	-- 4. IF ANY RESULT IS FOUND THEN RETURN ''BL_AREANID'' FROM HERE
	-- 5. IF NO RESULT IS FOUND IN STEP 4, THEN GET AREAID & NEXT IMMEDIATE LEVEL OF THAT NUMERIC AREANID
	-- 6. NOW CONCATENATE ABOVE INFO IN FORM OF QS_IND_L2 AND RETURN IT

	IF(SUBSTRING(@AREA_NID,1,3) = ''QS_'')
	BEGIN
		RETURN @AREA_NID
	END

	SET @IS_BLOCK = (SELECT LEN(CAST(AREA_BLOCK AS NVARCHAR)) FROM UT_AREA_XX WHERE AREA_NID = @AREA_NID)
	
	IF(@IS_BLOCK != 0)
	BEGIN
		SET @TMP_AREA_NID = ''BL_'' + @AREA_NID 
		SET @CACHE_COUNT = (SELECT COUNT(*) FROM DBO.DI_SEARCH_RESULTS WHERE AREANID = @TMP_AREA_NID AND INDICATORNID = @INDICATOR_NID AND SEARCHLANGUAGE = @LANG)
		IF(@CACHE_COUNT > 0)
		BEGIN
			SET @RESULT = @TMP_AREA_NID
			RETURN @RESULT
		END
			
	END
	ELSE
	BEGIN
		SET @TMP_AREA_NID = (SELECT ''QS_'' + AREA_ID + ''_L'' + CAST((AREA_LEVEL + 1) AS NVARCHAR) FROM UT_AREA_XX WHERE AREA_NID = CAST(@AREA_NID AS INT)) 
		SET @CACHE_COUNT = (SELECT COUNT(*) FROM DBO.DI_SEARCH_RESULTS WHERE AREANID = @TMP_AREA_NID AND INDICATORNID = @INDICATOR_NID AND SEARCHLANGUAGE = @LANG)
		IF(@CACHE_COUNT > 0)
		BEGIN
			SET @RESULT = @TMP_AREA_NID
			RETURN @RESULT
		END
	END	
	
	RETURN @RESULT

END


' 
END


--SPSEPARATOR--

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetAllAreaNidName_XX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_GetAllAreaNidName_XX] 
AS
Begin
	select area_id,area_name from ut_area_XX
END' 
END

ELSE

BEGIN

EXEC dbo.sp_executesql @statement = N'ALTER PROCEDURE [dbo].[sp_GetAllAreaNidName_XX] 
AS
Begin	
	select area_id,area_name from ut_area_XX
END'  
END

--SPSEPARATOR--

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

