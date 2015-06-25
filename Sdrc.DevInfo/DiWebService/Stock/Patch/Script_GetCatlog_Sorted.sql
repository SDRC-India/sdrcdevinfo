SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GET_CATALOG]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROC [dbo].[SP_GET_CATALOG]
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
END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'

ALTER PROC [dbo].[SP_GET_CATALOG]
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
END
' 
END
GO