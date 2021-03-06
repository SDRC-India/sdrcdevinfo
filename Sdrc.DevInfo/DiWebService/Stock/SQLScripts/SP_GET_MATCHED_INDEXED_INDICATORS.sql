SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[SP_GET_CATALOG_CACHE_RESULTS_EN]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE PROC [dbo].[SP_GET_MATCHED_INDEXED_INDICATORS]
@SEARCH_INDICATORS NVARCHAR(4000),
@LANGUAGE_CODE NVARCHAR(2)
AS
BEGIN

DECLARE @TMP_INDICATOR_NAME NVARCHAR(4000)
DECLARE @COUNT_SEARCHED_I INT
DECLARE @I INT

SET @I = 0

CREATE TABLE #RESULT_INDICATORS
(
INDICATOR_NID INT,
INDICATOR_NAME NVARCHAR(4000),
ADPT_NID INT
)

CREATE TABLE #SEARCHED_INDICATOR
(
	ID INT IDENTITY(0,1),
	INDICATOR NVARCHAR(4000)
)

INSERT INTO #SEARCHED_INDICATOR(INDICATOR)
SELECT ITEMS FROM DBO.SPLIT(@SEARCH_INDICATORS, '','')


SET @COUNT_SEARCHED_I = (SELECT COUNT(*) FROM #SEARCHED_INDICATOR)

IF(@COUNT_SEARCHED_I <> 0)
	BEGIN

		WHILE(@I < @COUNT_SEARCHED_I)
		BEGIN

			SET @TMP_INDICATOR_NAME = (SELECT TOP 1 INDICATOR FROM #SEARCHED_INDICATOR WHERE ID = @I)

			IF(@LANGUAGE_CODE = ''ar'')
				BEGIN
					INSERT INTO #RESULT_INDICATORS(INDICATOR_NID, INDICATOR_NAME, ADPT_NID)
					SELECT DISTINCT INDICATOR_NID, replace(INDICATOR_NAME,''"'', ''\"''), ADPT_NID FROM DBO.INDEXEDINDICATORS
					WHERE SOUNDEX(INDICATOR_NAME) LIKE ''%'' + SOUNDEX(LTRIM(RTRIM(@TMP_INDICATOR_NAME))) + ''%''
					AND LANGUAGE_CODE = @LANGUAGE_CODE					
				END			
			ELSE
				BEGIN
					INSERT INTO #RESULT_INDICATORS(INDICATOR_NID, INDICATOR_NAME, ADPT_NID)
					SELECT DISTINCT INDICATOR_NID, replace(INDICATOR_NAME,''"'', ''\"''), ADPT_NID FROM DBO.INDEXEDINDICATORS
					WHERE INDICATOR_NAME LIKE ''%'' + LTRIM(RTRIM(@TMP_INDICATOR_NAME)) + ''%''
					AND LANGUAGE_CODE = @LANGUAGE_CODE					
				END
		
			SET @I = @I + 1
		END

	END


SELECT * FROM #RESULT_INDICATORS

END

' 
END
