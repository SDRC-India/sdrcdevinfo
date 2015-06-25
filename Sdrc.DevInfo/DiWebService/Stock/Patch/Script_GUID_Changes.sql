SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---Add GUID Column in the Adaptation table
IF NOT EXISTS(SELECT * FROM sys.columns where Name = N'GUID' and Object_ID = Object_ID(N'Adaptations'))
Begin 
ALTER TABLE Adaptations DROP COLUMN Date_Created;
ALTER TABLE Adaptations Drop CONSTRAINT [DF_Adaptations_Visible_In_Catalog];  
ALTER TABLE Adaptations DROP COLUMN Visible_In_Catalog;
ALTER TABLE Adaptations ADD GUID nvarchar(50);
ALTER TABLE Adaptations ADD Date_Created nvarchar(100);
ALTER TABLE Adaptations ADD Visible_In_Catalog nvarchar(10);
ALTER TABLE Adaptations ADD  CONSTRAINT [DF_Adaptations_Visible_In_Catalog]  DEFAULT ('False') FOR [Visible_In_Catalog]
Update Adaptations SET Date_Created ='1/1/2013 00:00:00 AM'
Update Adaptations SET Visible_In_Catalog = 'True'
END

--Execute Stored Procedures Changes for GUID Column Changes
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_UPDATE_CATALOG_INFO]'))
Begin
EXEC dbo.sp_executesql @statement =N'CREATE PROC [dbo].[SP_UPDATE_CATALOG_INFO]	
	@NAME NVARCHAR(4000),
	@DI_VERSION NVARCHAR(50),
	@ONLINE_URL NVARCHAR(4000),
	@LAST_MODIFIED NVARCHAR(50),
	@AREA_NID INT,
	@SUB_NATION NVARCHAR(4000),
	@DB_ADM_NAME NVARCHAR(4000),
	@DB_ADM_INSTITUTION NVARCHAR(4000),
	@DB_ADM_EMAIL NVARCHAR(4000),
	@UNICEF_REGION NVARCHAR(4000),
	@ADAPTATION_YEAR NVARCHAR(4),
	@Adapted_For NVARCHAR(4000),
	@Country NVARCHAR(4000),
	@AdaptationGUID NVARCHAR(100)
AS
BEGIN
	UPDATE ADAPTATIONS SET
		NAME = @NAME,
		LAST_MODIFIED = @LAST_MODIFIED,
		AREA_NID = @AREA_NID,
		SUB_NATION = @SUB_NATION,
		DB_ADM_NAME = @DB_ADM_NAME,
		DB_ADM_INSTITUTION = @DB_ADM_INSTITUTION,
		DB_ADM_EMAIL = @DB_ADM_EMAIL,
		UNICEF_REGION = @UNICEF_REGION,
		ADAPTATION_YEAR = @ADAPTATION_YEAR,
		Adapted_For=@Adapted_For,
		Country=@Country,
		ONLINE_URL = @ONLINE_URL
		WHERE [GUID] = @AdaptationGUID
END'
End
ELSE
BEGIN
EXEC dbo.sp_executesql @statement =N'ALTER PROC [dbo].[SP_UPDATE_CATALOG_INFO]	
	@NAME NVARCHAR(4000),
	@DI_VERSION NVARCHAR(50),
	@ONLINE_URL NVARCHAR(4000),
	@LAST_MODIFIED NVARCHAR(50),
	@AREA_NID INT,
	@SUB_NATION NVARCHAR(4000),
	@DB_ADM_NAME NVARCHAR(4000),
	@DB_ADM_INSTITUTION NVARCHAR(4000),
	@DB_ADM_EMAIL NVARCHAR(4000),
	@UNICEF_REGION NVARCHAR(4000),
	@ADAPTATION_YEAR NVARCHAR(4),
	@Adapted_For NVARCHAR(4000),
	@Country NVARCHAR(4000),
	@AdaptationGUID NVARCHAR(100)
AS
BEGIN
	UPDATE ADAPTATIONS SET
		NAME = @NAME,
		LAST_MODIFIED = @LAST_MODIFIED,
		AREA_NID = @AREA_NID,
		SUB_NATION = @SUB_NATION,
		DB_ADM_NAME = @DB_ADM_NAME,
		DB_ADM_INSTITUTION = @DB_ADM_INSTITUTION,
		DB_ADM_EMAIL = @DB_ADM_EMAIL,
		UNICEF_REGION = @UNICEF_REGION,
		ADAPTATION_YEAR = @ADAPTATION_YEAR,
		Adapted_For=@Adapted_For,
		Country=@Country,
		ONLINE_URL = @ONLINE_URL
		WHERE [GUID] = @AdaptationGUID
END'
END
GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_INSERT_CATALOG]'))
Begin
EXEC dbo.sp_executesql @statement =N'CREATE PROC [dbo].[SP_INSERT_CATALOG]	
	@NAME NVARCHAR(4000),
	@DESCRIPTION NVARCHAR(4000),
	@DI_VERSION NVARCHAR(50),
	@IS_DESKTOP BIT,
	@IS_ONLINE BIT,
	@ONLINE_URL NVARCHAR(4000),
	@AREA_COUNT INT,
	@IUS_COUNT INT,
	@TIME_PERIODS_COUNT INT,
	@DATA_VALUES_COUNT INT,	
	@START_YEAR NVARCHAR(50),
	@END_YEAR NVARCHAR(50),	
	@LAST_MODIFIED NVARCHAR(50),
	@AREA_NID INT,
	@SUB_NATION NVARCHAR(4000),
	@THUMBNAIL_IMAGE_URL NVARCHAR(4000),
	@DB_ADM_NAME NVARCHAR(4000),
	@DB_ADM_INSTITUTION NVARCHAR(4000),
	@DB_ADM_EMAIL NVARCHAR(4000),
	@UNICEF_REGION NVARCHAR(4000),
	@ADAPTATION_YEAR NVARCHAR(4),
	@DB_LANGUAGES NVARCHAR(4000),
	@LangCode_CSVFiles NVARCHAR(4000),
	@Adapted_For NVARCHAR(4000),
	@Country NVARCHAR(4000),
	@AdaptationGUID NVARCHAR(100),
	@Date_Created NVARCHAR(4000)	
	
AS
	DECLARE @REC_COUNT INT

BEGIN
	IF(@DI_VERSION = ''-1'')
	BEGIN
		SET @DI_VERSION = (SELECT MAX(VER_NID) FROM ADAPTATIONVERSION)
	END

	IF(@IS_DESKTOP=1)
		BEGIN
			INSERT INTO ADAPTATIONS
			(
				NAME,
				DESCRIPTION,
				DI_VERSION, 
				IS_DESKTOP,
				IS_ONLINE,
				ONLINE_URL, 
				AREA_COUNT,
				IUS_COUNT,
				TIME_PERIODS_COUNT,
				DATA_VALUES_COUNT,		
				START_YEAR,
				END_YEAR,		
				LAST_MODIFIED,
				AREA_NID,
				SUB_NATION,
				THUMBNAIL_IMAGE_URL,
				DB_ADM_NAME,
				DB_ADM_INSTITUTION,
				DB_ADM_EMAIL,
				UNICEF_REGION,
				ADAPTATION_YEAR,
				DB_LANGUAGES,
				LangCode_CSVFiles,
				Adapted_For,
				Country,
				[GUID],
				Date_Created				
				
			)
			VALUES
			(
				@NAME,
				@DESCRIPTION,
				@DI_VERSION, 
				@IS_DESKTOP,
				@IS_ONLINE,
				@ONLINE_URL, 
				@AREA_COUNT,
				@IUS_COUNT,
				@TIME_PERIODS_COUNT,
				@DATA_VALUES_COUNT,		
				@START_YEAR,
				@END_YEAR,		
				@LAST_MODIFIED,
				@AREA_NID,
				@SUB_NATION,
				@THUMBNAIL_IMAGE_URL,
				@DB_ADM_NAME,
				@DB_ADM_INSTITUTION,
				@DB_ADM_EMAIL,
				@UNICEF_REGION,
				@ADAPTATION_YEAR,
				@DB_LANGUAGES,
				@LangCode_CSVFiles,
				@Adapted_For,
				@Country,
				@AdaptationGUID,
				@Date_Created
				
			)	
		END
	
	IF(@IS_ONLINE=1)
		BEGIN
			SET @REC_COUNT = (SELECT COUNT(*) FROM ADAPTATIONS 
				WHERE [GUID] = @AdaptationGUID)

			IF (@REC_COUNT = 0)
				BEGIN
					INSERT INTO ADAPTATIONS
					(
						NAME,
						DESCRIPTION,
						DI_VERSION, 
						IS_DESKTOP,
						IS_ONLINE,
						ONLINE_URL, 
						AREA_COUNT,
						IUS_COUNT,
						TIME_PERIODS_COUNT,
						DATA_VALUES_COUNT,		
						START_YEAR,
						END_YEAR,		
						LAST_MODIFIED,
						AREA_NID,
						SUB_NATION,
						THUMBNAIL_IMAGE_URL,
						DB_ADM_NAME,
						DB_ADM_INSTITUTION,
						DB_ADM_EMAIL,
						UNICEF_REGION,
						ADAPTATION_YEAR,
						DB_LANGUAGES,
						LangCode_CSVFiles,
						Adapted_For,
						Country,
						[GUID],
						Date_Created
						
					)
					VALUES
					(
						@NAME,
						@DESCRIPTION,
						@DI_VERSION, 
						@IS_DESKTOP,
						@IS_ONLINE,
						@ONLINE_URL, 
						@AREA_COUNT,
						@IUS_COUNT,
						@TIME_PERIODS_COUNT,
						@DATA_VALUES_COUNT,		
						@START_YEAR,
						@END_YEAR,		
						@LAST_MODIFIED,
						@AREA_NID,
						@SUB_NATION,
						@THUMBNAIL_IMAGE_URL,
						@DB_ADM_NAME,
						@DB_ADM_INSTITUTION,
						@DB_ADM_EMAIL,
						@UNICEF_REGION,
						@ADAPTATION_YEAR,
						@DB_LANGUAGES,
						@LangCode_CSVFiles,
						@Adapted_For,
						@Country,
						@AdaptationGUID,
						@Date_Created
						
					)	
				END
			ELSE
				BEGIN
					UPDATE ADAPTATIONS SET
						NAME = @NAME,
						DESCRIPTION = @DESCRIPTION,
						DI_VERSION = @DI_VERSION, 
						IS_DESKTOP = @IS_DESKTOP,
						IS_ONLINE = @IS_ONLINE,				
						AREA_COUNT = @AREA_COUNT,
						IUS_COUNT = @IUS_COUNT,
						ONLINE_URL = @ONLINE_URL,
						TIME_PERIODS_COUNT = @TIME_PERIODS_COUNT,
						DATA_VALUES_COUNT = @DATA_VALUES_COUNT,		
						START_YEAR = @START_YEAR,
						END_YEAR = @END_YEAR,
						LAST_MODIFIED = @LAST_MODIFIED,
						AREA_NID = @AREA_NID,
						SUB_NATION = @SUB_NATION,
						THUMBNAIL_IMAGE_URL = @THUMBNAIL_IMAGE_URL,
						DB_ADM_NAME = @DB_ADM_NAME,
						DB_ADM_INSTITUTION = @DB_ADM_INSTITUTION,
						DB_ADM_EMAIL = @DB_ADM_EMAIL,
						UNICEF_REGION = @UNICEF_REGION,
						ADAPTATION_YEAR = @ADAPTATION_YEAR,
						DB_LANGUAGES = @DB_LANGUAGES,
						LangCode_CSVFiles = @LangCode_CSVFiles,
						Adapted_For=@Adapted_For,
						Country=@Country
						WHERE [GUID] = @AdaptationGUID
				END
			END
END'
End
ELSE
BEGIN
EXEC dbo.sp_executesql @statement =N'ALTER PROC [dbo].[SP_INSERT_CATALOG]	
	@NAME NVARCHAR(4000),
	@DESCRIPTION NVARCHAR(4000),
	@DI_VERSION NVARCHAR(50),
	@IS_DESKTOP BIT,
	@IS_ONLINE BIT,
	@ONLINE_URL NVARCHAR(4000),
	@AREA_COUNT INT,
	@IUS_COUNT INT,
	@TIME_PERIODS_COUNT INT,
	@DATA_VALUES_COUNT INT,	
	@START_YEAR NVARCHAR(50),
	@END_YEAR NVARCHAR(50),	
	@LAST_MODIFIED NVARCHAR(50),
	@AREA_NID INT,
	@SUB_NATION NVARCHAR(4000),
	@THUMBNAIL_IMAGE_URL NVARCHAR(4000),
	@DB_ADM_NAME NVARCHAR(4000),
	@DB_ADM_INSTITUTION NVARCHAR(4000),
	@DB_ADM_EMAIL NVARCHAR(4000),
	@UNICEF_REGION NVARCHAR(4000),
	@ADAPTATION_YEAR NVARCHAR(4),
	@DB_LANGUAGES NVARCHAR(4000),
	@LangCode_CSVFiles NVARCHAR(4000),
	@Adapted_For NVARCHAR(4000),
	@Country NVARCHAR(4000),
	@AdaptationGUID NVARCHAR(100),
	@Date_Created NVARCHAR(4000)	
	
AS
	DECLARE @REC_COUNT INT

BEGIN
	IF(@DI_VERSION = ''-1'')
	BEGIN
		SET @DI_VERSION = (SELECT MAX(VER_NID) FROM ADAPTATIONVERSION)
	END

	IF(@IS_DESKTOP=1)
		BEGIN
			INSERT INTO ADAPTATIONS
			(
				NAME,
				DESCRIPTION,
				DI_VERSION, 
				IS_DESKTOP,
				IS_ONLINE,
				ONLINE_URL, 
				AREA_COUNT,
				IUS_COUNT,
				TIME_PERIODS_COUNT,
				DATA_VALUES_COUNT,		
				START_YEAR,
				END_YEAR,		
				LAST_MODIFIED,
				AREA_NID,
				SUB_NATION,
				THUMBNAIL_IMAGE_URL,
				DB_ADM_NAME,
				DB_ADM_INSTITUTION,
				DB_ADM_EMAIL,
				UNICEF_REGION,
				ADAPTATION_YEAR,
				DB_LANGUAGES,
				LangCode_CSVFiles,
				Adapted_For,
				Country,
				[GUID],
				Date_Created				
				
			)
			VALUES
			(
				@NAME,
				@DESCRIPTION,
				@DI_VERSION, 
				@IS_DESKTOP,
				@IS_ONLINE,
				@ONLINE_URL, 
				@AREA_COUNT,
				@IUS_COUNT,
				@TIME_PERIODS_COUNT,
				@DATA_VALUES_COUNT,		
				@START_YEAR,
				@END_YEAR,		
				@LAST_MODIFIED,
				@AREA_NID,
				@SUB_NATION,
				@THUMBNAIL_IMAGE_URL,
				@DB_ADM_NAME,
				@DB_ADM_INSTITUTION,
				@DB_ADM_EMAIL,
				@UNICEF_REGION,
				@ADAPTATION_YEAR,
				@DB_LANGUAGES,
				@LangCode_CSVFiles,
				@Adapted_For,
				@Country,
				@AdaptationGUID,
				@Date_Created
				
			)	
		END
	
	IF(@IS_ONLINE=1)
		BEGIN
			SET @REC_COUNT = (SELECT COUNT(*) FROM ADAPTATIONS 
				WHERE [GUID] = @AdaptationGUID)

			IF (@REC_COUNT = 0)
				BEGIN
					INSERT INTO ADAPTATIONS
					(
						NAME,
						DESCRIPTION,
						DI_VERSION, 
						IS_DESKTOP,
						IS_ONLINE,
						ONLINE_URL, 
						AREA_COUNT,
						IUS_COUNT,
						TIME_PERIODS_COUNT,
						DATA_VALUES_COUNT,		
						START_YEAR,
						END_YEAR,		
						LAST_MODIFIED,
						AREA_NID,
						SUB_NATION,
						THUMBNAIL_IMAGE_URL,
						DB_ADM_NAME,
						DB_ADM_INSTITUTION,
						DB_ADM_EMAIL,
						UNICEF_REGION,
						ADAPTATION_YEAR,
						DB_LANGUAGES,
						LangCode_CSVFiles,
						Adapted_For,
						Country,
						[GUID],
						Date_Created
						
					)
					VALUES
					(
						@NAME,
						@DESCRIPTION,
						@DI_VERSION, 
						@IS_DESKTOP,
						@IS_ONLINE,
						@ONLINE_URL, 
						@AREA_COUNT,
						@IUS_COUNT,
						@TIME_PERIODS_COUNT,
						@DATA_VALUES_COUNT,		
						@START_YEAR,
						@END_YEAR,		
						@LAST_MODIFIED,
						@AREA_NID,
						@SUB_NATION,
						@THUMBNAIL_IMAGE_URL,
						@DB_ADM_NAME,
						@DB_ADM_INSTITUTION,
						@DB_ADM_EMAIL,
						@UNICEF_REGION,
						@ADAPTATION_YEAR,
						@DB_LANGUAGES,
						@LangCode_CSVFiles,
						@Adapted_For,
						@Country,
						@AdaptationGUID,
						@Date_Created
						
					)	
				END
			ELSE
				BEGIN
					UPDATE ADAPTATIONS SET
						NAME = @NAME,
						DESCRIPTION = @DESCRIPTION,
						DI_VERSION = @DI_VERSION, 
						IS_DESKTOP = @IS_DESKTOP,
						IS_ONLINE = @IS_ONLINE,				
						AREA_COUNT = @AREA_COUNT,
						IUS_COUNT = @IUS_COUNT,
						ONLINE_URL = @ONLINE_URL,
						TIME_PERIODS_COUNT = @TIME_PERIODS_COUNT,
						DATA_VALUES_COUNT = @DATA_VALUES_COUNT,		
						START_YEAR = @START_YEAR,
						END_YEAR = @END_YEAR,
						LAST_MODIFIED = @LAST_MODIFIED,
						AREA_NID = @AREA_NID,
						SUB_NATION = @SUB_NATION,
						THUMBNAIL_IMAGE_URL = @THUMBNAIL_IMAGE_URL,
						DB_ADM_NAME = @DB_ADM_NAME,
						DB_ADM_INSTITUTION = @DB_ADM_INSTITUTION,
						DB_ADM_EMAIL = @DB_ADM_EMAIL,
						UNICEF_REGION = @UNICEF_REGION,
						ADAPTATION_YEAR = @ADAPTATION_YEAR,
						DB_LANGUAGES = @DB_LANGUAGES,
						LangCode_CSVFiles = @LangCode_CSVFiles,
						Adapted_For=@Adapted_For,
						Country=@Country
						WHERE [GUID] = @AdaptationGUID
				END
			END
END'
END
GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GET_DATE_CREATED]'))
Begin
EXEC dbo.sp_executesql @statement =N'CREATE PROC [dbo].[SP_GET_DATE_CREATED]
	@AdaptationURL NVARCHAR(4000)
AS
BEGIN
	SELECT Date_Created FROM ADAPTATIONS WHERE [GUID] = @AdaptationURL
END'
End
ELSE
BEGIN
EXEC dbo.sp_executesql @statement =N'ALTER PROC [dbo].[SP_GET_DATE_CREATED]
	@AdaptationURL NVARCHAR(4000)
AS
BEGIN
	SELECT Date_Created FROM ADAPTATIONS WHERE [GUID] = @AdaptationURL
END'
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GET_CATALOG]'))
Begin
EXEC dbo.sp_executesql @statement =N'CREATE PROC [dbo].[SP_GET_CATALOG]
	@AdaptationGUID NVARCHAR(4000)
AS
BEGIN

	DECLARE @ADPT_AREA_NID INT	
	DECLARE @IS_DI7_ORG_SITE_Val BIT

	SET @ADPT_AREA_NID = (SELECT AREA_NID FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID)
	SET @IS_DI7_ORG_SITE_Val=(SELECT IS_DI7_ORG_SITE FROM ADAPTATIONS WHERE [GUID] =  @AdaptationGUID)

	IF @ADPT_AREA_NID = -1 
		BEGIN
		IF @IS_DI7_ORG_SITE_Val = ''True'' 
		BEGIN
			SELECT
			NId,Name,Description,Ver_Name [DI_Version],Is_Desktop,Is_Online,Online_URL,Area_Count,IUS_Count,
			Time_Periods_Count,Data_Values_Count,Start_Year,End_Year,Last_Modified,Area_NId,Sub_Nation,
			Thumbnail_Image_URL,Db_Adm_Name,Db_Adm_Institution,Db_Adm_Email,Unicef_Region,Adaptation_Year,Db_Languages,LangCode_CSVFiles
			FROM ADAPTATIONS, ADAPTATIONVERSION
			WHERE (AREA_NID IN(@ADPT_AREA_NID) OR
			AREA_NID IN(
			SELECT AREANID FROM AREAS WHERE PARENTNID IN(
				SELECT AREANID FROM AREAS WHERE PARENTNID=-1)
			)) AND
			DI_VERSION = VER_NID AND  Visible_In_Catalog = ''True'' ORDER BY  Name ASC
		END
		ELSE
		BEGIN
			SELECT
			NId,Name,Description,Ver_Name [DI_Version],Is_Desktop,Is_Online,Online_URL,Area_Count,IUS_Count,
			Time_Periods_Count,Data_Values_Count,Start_Year,End_Year,Last_Modified,Area_NId,Sub_Nation,
			Thumbnail_Image_URL,Db_Adm_Name,Db_Adm_Institution,Db_Adm_Email,Unicef_Region,Adaptation_Year,Db_Languages,LangCode_CSVFiles
			FROM ADAPTATIONS, ADAPTATIONVERSION
			WHERE( AREA_NID =''-1'' AND( IS_DI7_ORG_SITE <>''True'' OR IS_DI7_ORG_SITE IS NULL ))AND
		DI_VERSION = VER_NID AND  Visible_In_Catalog = ''True'' ORDER BY  Name ASC
		END

		END
	ELSE
		BEGIN
			SELECT 
			NId,Name,Description,Ver_Name [DI_Version],Is_Desktop,Is_Online,Online_URL,Area_Count,IUS_Count,
			Time_Periods_Count,Data_Values_Count,Start_Year,End_Year,Last_Modified,Area_NId,Sub_Nation,
			Thumbnail_Image_URL,Db_Adm_Name,Db_Adm_Institution,Db_Adm_Email,Unicef_Region,Adaptation_Year,Db_Languages,LangCode_CSVFiles
			FROM ADAPTATIONS, ADAPTATIONVERSION
			WHERE (AREA_NID IN(@ADPT_AREA_NID)) AND
			DI_VERSION = VER_NID  AND Visible_In_Catalog = ''True'' ORDER BY Name ASC		
		END
END'
End
ELSE
BEGIN
EXEC dbo.sp_executesql @statement =N'ALTER PROC [dbo].[SP_GET_CATALOG]
	@AdaptationGUID NVARCHAR(4000)
AS
BEGIN

	DECLARE @ADPT_AREA_NID INT	
	DECLARE @IS_DI7_ORG_SITE_Val BIT

	SET @ADPT_AREA_NID = (SELECT AREA_NID FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID)
	SET @IS_DI7_ORG_SITE_Val=(SELECT IS_DI7_ORG_SITE FROM ADAPTATIONS WHERE [GUID] =  @AdaptationGUID)

	IF @ADPT_AREA_NID = -1 
		BEGIN
		IF @IS_DI7_ORG_SITE_Val = ''True'' 
		BEGIN
			SELECT
			NId,Name,Description,Ver_Name [DI_Version],Is_Desktop,Is_Online,Online_URL,Area_Count,IUS_Count,
			Time_Periods_Count,Data_Values_Count,Start_Year,End_Year,Last_Modified,Area_NId,Sub_Nation,
			Thumbnail_Image_URL,Db_Adm_Name,Db_Adm_Institution,Db_Adm_Email,Unicef_Region,Adaptation_Year,Db_Languages,LangCode_CSVFiles
			FROM ADAPTATIONS, ADAPTATIONVERSION
			WHERE (AREA_NID IN(@ADPT_AREA_NID) OR
			AREA_NID IN(
			SELECT AREANID FROM AREAS WHERE PARENTNID IN(
				SELECT AREANID FROM AREAS WHERE PARENTNID=-1)
			)) AND
			DI_VERSION = VER_NID AND  Visible_In_Catalog = ''True'' ORDER BY  Name ASC
		END
		ELSE
		BEGIN
			SELECT
			NId,Name,Description,Ver_Name [DI_Version],Is_Desktop,Is_Online,Online_URL,Area_Count,IUS_Count,
			Time_Periods_Count,Data_Values_Count,Start_Year,End_Year,Last_Modified,Area_NId,Sub_Nation,
			Thumbnail_Image_URL,Db_Adm_Name,Db_Adm_Institution,Db_Adm_Email,Unicef_Region,Adaptation_Year,Db_Languages,LangCode_CSVFiles
			FROM ADAPTATIONS, ADAPTATIONVERSION
			WHERE( AREA_NID =''-1'' AND( IS_DI7_ORG_SITE <>''True'' OR IS_DI7_ORG_SITE IS NULL ))AND
		DI_VERSION = VER_NID AND  Visible_In_Catalog = ''True'' ORDER BY  Name ASC
		END

		END
	ELSE
		BEGIN
			SELECT 
			NId,Name,Description,Ver_Name [DI_Version],Is_Desktop,Is_Online,Online_URL,Area_Count,IUS_Count,
			Time_Periods_Count,Data_Values_Count,Start_Year,End_Year,Last_Modified,Area_NId,Sub_Nation,
			Thumbnail_Image_URL,Db_Adm_Name,Db_Adm_Institution,Db_Adm_Email,Unicef_Region,Adaptation_Year,Db_Languages,LangCode_CSVFiles
			FROM ADAPTATIONS, ADAPTATIONVERSION
			WHERE (AREA_NID IN(@ADPT_AREA_NID)) AND
			DI_VERSION = VER_NID  AND Visible_In_Catalog = ''True'' ORDER BY Name ASC		
		END
END'
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GET_ADAPTATION_NID_AND_DELETE_INDEXED]'))
Begin
EXEC dbo.sp_executesql @statement =N'CREATE PROC [dbo].[SP_GET_ADAPTATION_NID_AND_DELETE_INDEXED]
	@AdaptationGUID NVARCHAR(4000),
	@TABLE_TYPE NVARCHAR(20)
AS
BEGIN
	DECLARE @ADPT_NID INT

	SET @ADPT_NID = (SELECT NID FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID)
	
	IF(@TABLE_TYPE = ''AREA'')
	BEGIN
		DELETE FROM INDEXEDAREAS WHERE ADPT_NID = @ADPT_NID
	END

	IF(@TABLE_TYPE = ''INDICATOR'')
	BEGIN
		DELETE FROM INDEXEDINDICATORS WHERE ADPT_NID = @ADPT_NID
	END

	SELECT @ADPT_NID 
END'
End
ELSE
BEGIN
EXEC dbo.sp_executesql @statement =N'ALTER PROC [dbo].[SP_GET_ADAPTATION_NID_AND_DELETE_INDEXED]
	@AdaptationGUID NVARCHAR(4000),
	@TABLE_TYPE NVARCHAR(20)
AS
BEGIN
	DECLARE @ADPT_NID INT

	SET @ADPT_NID = (SELECT NID FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID)
	
	IF(@TABLE_TYPE = ''AREA'')
	BEGIN
		DELETE FROM INDEXEDAREAS WHERE ADPT_NID = @ADPT_NID
	END

	IF(@TABLE_TYPE = ''INDICATOR'')
	BEGIN
		DELETE FROM INDEXEDINDICATORS WHERE ADPT_NID = @ADPT_NID
	END

	SELECT @ADPT_NID 
END'
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CATALOG_EXISTS]'))
Begin
EXEC dbo.sp_executesql @statement =N'CREATE PROC [dbo].[SP_CATALOG_EXISTS]
	@AdaptationGUID NVARCHAR(4000)
AS
BEGIN
	SELECT NId,Date_Created,Visible_in_Catalog FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID
END'
End
ELSE
BEGIN
EXEC dbo.sp_executesql @statement =N'ALTER PROC [dbo].[SP_CATALOG_EXISTS]
	@AdaptationGUID NVARCHAR(4000)
AS
BEGIN
	SELECT NId,Date_Created,Visible_in_Catalog FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID
END'
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Set_User_AsAdmin]'))
Begin
EXEC dbo.sp_executesql @statement =N'CREATE Procedure [dbo].[Set_User_AsAdmin]   
@User_NId INT,
@AdaptationGUID as NVarchar(4000)      
AS          
          
BEGIN 

BEGIN TRY

BEGIN TRANSACTION

UPDATE ProviderDetails
SET [User_Is_Provider] = ''False'',
[User_Is_Admin] = ''False''
WHERE [AdaptationNid] in (SELECT NId FROM Adaptations where [GUID]=@AdaptationGUID)
AND [User_Is_Admin] = ''True''
         
UPDATE ProviderDetails
SET [User_Is_Provider] = ''True'',
[User_Is_Admin] = ''True''
WHERE [AdaptationNid] in (SELECT NId FROM Adaptations where [GUID]=@AdaptationGUID)
AND [UserNid] = @User_NId
COMMIT

END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK     
END CATCH      

END'
End
ELSE
BEGIN
EXEC dbo.sp_executesql @statement =N'ALTER Procedure [dbo].[Set_User_AsAdmin]   
@User_NId INT,
@AdaptationGUID as NVarchar(4000)      
AS          
          
BEGIN 

BEGIN TRY

BEGIN TRANSACTION

UPDATE ProviderDetails
SET [User_Is_Provider] = ''False'',
[User_Is_Admin] = ''False''
WHERE [AdaptationNid] in (SELECT NId FROM Adaptations where [GUID]=@AdaptationGUID)
AND [User_Is_Admin] = ''True''
         
UPDATE ProviderDetails
SET [User_Is_Provider] = ''True'',
[User_Is_Admin] = ''True''
WHERE [AdaptationNid] in (SELECT NId FROM Adaptations where [GUID]=@AdaptationGUID)
AND [UserNid] = @User_NId
COMMIT

END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK     
END CATCH      

END'
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Get_Users_ByAdaptationURL]'))
Begin
EXEC dbo.sp_executesql @statement =N'CREATE Procedure [dbo].[Get_Users_ByAdaptationURL]   
@AdaptationGUID as NVarchar(4000)        
AS          
          
BEGIN         


SELECT U.[NId],U.[User_First_Name] AS "UserName", U.[User_Email_Id] AS "EmailId", P.[User_Is_Provider] AS "Is Provider" ,P.[User_Is_Admin] AS "Is Admin" 
FROM ProviderDetails P
JOIN Global_UserLogin U
ON P.[UserNid] = U.[NId]
JOIN Adaptations A
ON P.[AdaptationNid] = A.[NId]
WHERE A.[GUID] = @AdaptationGUID AND U.[IsMasterAccount] <> ''True''
--ORDER BY U.[NId], A.[NId]
ORDER BY P.[User_Is_Admin] DESC
          
END'
End
ELSE
BEGIN
EXEC dbo.sp_executesql @statement =N'ALTER Procedure [dbo].[Get_Users_ByAdaptationURL]   
@AdaptationGUID as NVarchar(4000)        
AS          
          
BEGIN         


SELECT U.[NId],U.[User_First_Name] AS "UserName", U.[User_Email_Id] AS "EmailId", P.[User_Is_Provider] AS "Is Provider" ,P.[User_Is_Admin] AS "Is Admin" 
FROM ProviderDetails P
JOIN Global_UserLogin U
ON P.[UserNid] = U.[NId]
JOIN Adaptations A
ON P.[AdaptationNid] = A.[NId]
WHERE A.[GUID] = @AdaptationGUID AND U.[IsMasterAccount] <> ''True''
--ORDER BY U.[NId], A.[NId]
ORDER BY P.[User_Is_Admin] DESC
          
END'
END
GO