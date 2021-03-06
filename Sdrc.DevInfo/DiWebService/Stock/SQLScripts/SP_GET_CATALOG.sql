SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[SP_GET_CATALOG]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROC [dbo].[SP_GET_CATALOG]
	@WEBURL NVARCHAR(4000)
AS
BEGIN

	DECLARE @ADPT_AREA_NID INT	

	SET @ADPT_AREA_NID = (SELECT AREA_NID FROM ADAPTATIONS WHERE ONLINE_URL = @WEBURL)

	IF @ADPT_AREA_NID = -1
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
			DI_VERSION = VER_NID

		END
	ELSE
		BEGIN
			SELECT 
			NId,Name,Description,Ver_Name [DI_Version],Is_Desktop,Is_Online,Online_URL,Area_Count,IUS_Count,
			Time_Periods_Count,Data_Values_Count,Start_Year,End_Year,Last_Modified,Area_NId,Sub_Nation,
			Thumbnail_Image_URL,Db_Adm_Name,Db_Adm_Institution,Db_Adm_Email,Unicef_Region,Adaptation_Year,Db_Languages,LangCode_CSVFiles
			FROM ADAPTATIONS, ADAPTATIONVERSION
			WHERE ONLINE_URL = @WEBURL
			AND DI_VERSION = VER_NID
		END
END
' 
END
