USE [master]
GO
/****** Object:  Database [DIWorldWide]    Script Date: 05/21/2012 12:05:43 ******/
CREATE DATABASE [DIWorldWide] ON  PRIMARY 
( NAME = N'DIWorldWide_Data', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL\data\DIWorldWide_Data.MDF' , SIZE = 3328KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
 LOG ON 
( NAME = N'DIWorldWide_Log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL\data\DIWorldWide_Log.LDF' , SIZE = 29504KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
GO
ALTER DATABASE [DIWorldWide] SET COMPATIBILITY_LEVEL = 80
GO
ALTER DATABASE [DIWorldWide] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [DIWorldWide] SET ANSI_NULLS OFF
GO
ALTER DATABASE [DIWorldWide] SET ANSI_PADDING OFF
GO
ALTER DATABASE [DIWorldWide] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [DIWorldWide] SET ARITHABORT OFF
GO
ALTER DATABASE [DIWorldWide] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [DIWorldWide] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [DIWorldWide] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [DIWorldWide] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [DIWorldWide] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [DIWorldWide] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [DIWorldWide] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [DIWorldWide] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [DIWorldWide] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [DIWorldWide] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [DIWorldWide] SET  READ_WRITE
GO
ALTER DATABASE [DIWorldWide] SET RECOVERY FULL
GO
ALTER DATABASE [DIWorldWide] SET  MULTI_USER
GO
ALTER DATABASE [DIWorldWide] SET PAGE_VERIFY TORN_PAGE_DETECTION
GO
ALTER DATABASE [DIWorldWide] SET DB_CHAINING OFF
GO
USE [DIWorldWide]
GO
/****** Object:  Table [dbo].[Adaptations]    Script Date: 05/21/2012 12:05:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Adaptations](
	[NId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](4000) NOT NULL,
	[Description] [nvarchar](4000) NULL,
	[DI_Version] [nvarchar](50) NOT NULL,
	[Is_Desktop] [bit] NOT NULL,
	[Is_Online] [bit] NOT NULL,
	[Online_URL] [nvarchar](4000) NOT NULL,
	[Area_Count] [int] NULL,
	[IUS_Count] [int] NULL,
	[Time_Periods_Count] [int] NULL,
	[Data_Values_Count] [int] NULL,
	[Start_Year] [nvarchar](50) NULL,
	[End_Year] [nvarchar](50) NULL,
	[Last_Modified] [nvarchar](50) NULL,
	[Area_NId] [int] NOT NULL,
	[Sub_Nation] [nvarchar](4000) NULL,
	[Thumbnail_Image_URL] [nvarchar](4000) NULL,
 CONSTRAINT [PK_Adaptations] PRIMARY KEY CLUSTERED 
(
	[NId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_CATALOG_CACHE_RESULTS_EN]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_GET_CATALOG_CACHE_RESULTS_EN]
(
@SearchAreas NVARCHAR(4000),
@SearchIndicators NVARCHAR(4000)
)
AS
BEGIN

	DECLARE @I INT
	DECLARE @MAX INT

declare @tmp_Area_Name nvarchar(4000)
declare @tmp_Indicator_Name nvarchar(4000)

	SET @I = 0

	--SET @AREAS = 'AFRICA' --'ASIA AFRICA SUDAN'
	--SET @INDICATORS = 'CHILD INFANT' -- 'INFANT CHILD'

	CREATE TABLE #SEARCHED_AREA_INDICATOR_COMBINATIONS
	(
		ID INT IDENTITY(0,1),
		AREA NVARCHAR(4000),
		INDICATOR NVARCHAR(4000)
	)

	CREATE TABLE #SEARCHED_AREA
	(
		ID INT IDENTITY(0,1),
		AREA NVARCHAR(4000)
	)
	CREATE TABLE #SEARCHED_INDICATOR
	(
		ID INT IDENTITY(0,1),
		INDICATOR NVARCHAR(4000)
	)
	CREATE TABLE #CACHE_AREA_INDICATOR_NIDS
	(
		ID INT IDENTITY(0,1),
		AREA_ADPT_NID NVARCHAR(4000),
		INDICATOR_ADPT_NID NVARCHAR(4000)
	)

	CREATE TABLE #RESULT_ADAPTATIONS
	(
	ID INT IDENTITY(0,1),
	ADPT_NID INT,
	INDICATOR_NID INT,
	AREA_NID INT
	)


	INSERT INTO #SEARCHED_AREA(area)
	SELECT ITEMS FROM DBO.SPLIT(@SEARCHAREAS, ',')

	INSERT INTO #SEARCHED_INDICATOR(indicator)
	SELECT ITEMS FROM DBO.SPLIT(@SEARCHINDICATORS, ',')


	INSERT INTO #SEARCHED_AREA_INDICATOR_COMBINATIONS(AREA, INDICATOR)
	SELECT A.Area AS 'AREA', I.indicator AS 'INDICATORS'
	FROM #SEARCHED_AREA A, #SEARCHED_INDICATOR I

	DECLARE @COUNT_SEARCHED_A INT
	DECLARE @COUNT_SEARCHED_I INT
	DECLARE @COUNT_SEARCHED_AI INT

	SET @COUNT_SEARCHED_A = (SELECT COUNT(*) FROM #SEARCHED_AREA)
	SET @COUNT_SEARCHED_I = (SELECT COUNT(*) FROM #SEARCHED_INDICATOR)
	SET @COUNT_SEARCHED_AI = (SELECT COUNT(*) FROM #SEARCHED_AREA_INDICATOR_COMBINATIONS)

	IF (@COUNT_SEARCHED_AI <> 0)

	BEGIN
	
		WHILE(@I < @COUNT_SEARCHED_AI)
			BEGIN				

				SET @TMP_AREA_NAME = (SELECT TOP 1 AREA FROM #SEARCHED_AREA_INDICATOR_COMBINATIONS WHERE ID = @I)
				SET @TMP_indicator_NAME = (SELECT TOP 1 indicator FROM #SEARCHED_AREA_INDICATOR_COMBINATIONS WHERE ID = @I)
				
				INSERT INTO #RESULT_ADAPTATIONS(ADPT_NID)
				SELECT I.ADPT_NID
				FROM DBO.INDEXEDAREAS A, DBO.INDEXEDINDICATORS I
				WHERE A.ADPT_NID = I.ADPT_NID
				AND SOUNDEX(A.AREA_NAME) = SOUNDEX(LTRIM(RTRIM(@TMP_AREA_NAME))) --A.AREA_NAME LIKE '%' + LTRIM(RTRIM(@TMP_AREA_NAME)) + '%'
				AND SOUNDEX(I.INDICATOR_NAME) = SOUNDEX(LTRIM(RTRIM(@TMP_INDICATOR_NAME))) --I.INDICATOR_NAME LIKE '%' + LTRIM(RTRIM(@TMP_INDICATOR_NAME)) + '%'	

				SET @I = @I + 1		

			END
	
	END

	ELSE

	BEGIN
	
		-- Only areas are searched
		IF(@COUNT_SEARCHED_A <> 0)
		BEGIN

			WHILE(@I < @COUNT_SEARCHED_A)
			BEGIN

				SET @TMP_AREA_NAME = (SELECT TOP 1 AREA FROM #SEARCHED_AREA WHERE ID = @I)

				INSERT INTO #RESULT_ADAPTATIONS(ADPT_NID)
				SELECT DISTINCT ADPT_NID FROM DBO.INDEXEDAREAS
				WHERE SOUNDEX(AREA_NAME) = SOUNDEX(LTRIM(RTRIM(@TMP_AREA_NAME))) --AREA_NAME LIKE '%' + LTRIM(RTRIM(@TMP_AREA_NAME)) + '%'

				SET @I = @I + 1
			END

		END

		SET @I = 0	

		-- Only indicators are searched
		IF(@COUNT_SEARCHED_I <> 0)
		BEGIN

			WHILE(@I < @COUNT_SEARCHED_I)
			BEGIN

				SET @TMP_INDICATOR_NAME = (SELECT TOP 1 INDICATOR FROM #SEARCHED_INDICATOR WHERE ID = @I)

				INSERT INTO #RESULT_ADAPTATIONS(ADPT_NID)
				SELECT DISTINCT ADPT_NID FROM DBO.INDEXEDINDICATORS
				WHERE SOUNDEX(INDICATOR_NAME) = SOUNDEX(LTRIM(RTRIM(@TMP_INDICATOR_NAME))) --INDICATOR_NAME LIKE '%' + LTRIM(RTRIM(@TMP_INDICATOR_NAME)) + '%'

				SET @I = @I + 1
			END

		END
	
	END

SELECT * FROM #RESULT_ADAPTATIONS



	DROP TABLE #SEARCHED_AREA_INDICATOR_COMBINATIONS
	DROP TABLE #CACHE_AREA_INDICATOR_NIDS

END
GO
/****** Object:  UserDefinedFunction [dbo].[Split]    Script Date: 05/21/2012 12:05:47 ******/
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Split](@String varchar(4000), @Delimiter char(1))       
    returns @temptable TABLE (items varchar(4000))       
    as       
    begin       
        declare @idx int       
        declare @slice varchar(4000)       
          
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
    end
GO
/****** Object:  Table [dbo].[IndexedAreas]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IndexedAreas](
	[Area_NId] [int] NOT NULL,
	[Area_Parent_NId] [int] NULL,
	[Area_ID] [nvarchar](255) NULL,
	[Area_Name] [nvarchar](60) NULL,
	[Area_GId] [nvarchar](60) NULL,
	[Area_Level] [int] NULL,
	[Area_Map] [nvarchar](60) NULL,
	[Area_Block] [ntext] NULL,
	[Area_Global] [bit] NOT NULL,
	[Data_Exist] [bit] NULL,
	[AreaShortName] [nvarchar](255) NULL,
	[Adpt_NId] [int] NULL,
	[Language_Code] [nvarchar](2) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[sp_get_matched_indexed_areas]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_get_matched_indexed_areas]
@SEARCH_AREAS NVARCHAR(4000)
AS
BEGIN

DECLARE @TMP_AREA_NAME NVARCHAR(4000)
DECLARE @COUNT_SEARCHED_A INT
DECLARE @I INT

SET @I = 0

CREATE TABLE #RESULT_AREAS
(
AREA_NID INT,
AREA_PARENT_NID INT,
AREA_NAME NVARCHAR(4000),
ADPT_NID INT
)

CREATE TABLE #SEARCHED_AREA
(
	ID INT IDENTITY(0,1),
	AREA NVARCHAR(4000)
)

INSERT INTO #SEARCHED_AREA(area)
SELECT ITEMS FROM DBO.SPLIT(@SEARCH_AREAS, ',')


SET @COUNT_SEARCHED_A = (SELECT COUNT(*) FROM #SEARCHED_AREA)

IF(@COUNT_SEARCHED_A <> 0)
	BEGIN

		WHILE(@I < @COUNT_SEARCHED_A)
		BEGIN

			SET @TMP_AREA_NAME = (SELECT TOP 1 AREA FROM #SEARCHED_AREA WHERE ID = @I)

			INSERT INTO #RESULT_AREAS(AREA_NID, AREA_PARENT_NID,AREA_NAME,ADPT_NID)
			SELECT DISTINCT AREA_NID, AREA_PARENT_NID, AREA_NAME, ADPT_NID FROM DBO.INDEXEDAREAS
			WHERE AREA_NAME LIKE '%' + LTRIM(RTRIM(@TMP_AREA_NAME)) + '%' --SOUNDEX(AREA_NAME) = SOUNDEX(LTRIM(RTRIM(@TMP_AREA_NAME)))

			SET @I = @I + 1
		END

	END


select * from #RESULT_AREAS

end
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_MATCHED_INDEXED_INDICATORS]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_GET_MATCHED_INDEXED_INDICATORS]
@SEARCH_INDICATORS NVARCHAR(4000)
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
SELECT ITEMS FROM DBO.SPLIT(@SEARCH_INDICATORS, ',')


SET @COUNT_SEARCHED_I = (SELECT COUNT(*) FROM #SEARCHED_INDICATOR)

IF(@COUNT_SEARCHED_I <> 0)
	BEGIN

		WHILE(@I < @COUNT_SEARCHED_I)
		BEGIN

			SET @TMP_INDICATOR_NAME = (SELECT TOP 1 INDICATOR FROM #SEARCHED_INDICATOR WHERE ID = @I)

			INSERT INTO #RESULT_INDICATORS(INDICATOR_NID, INDICATOR_NAME, ADPT_NID)
			SELECT DISTINCT INDICATOR_NID, INDICATOR_NAME, ADPT_NID FROM DBO.INDEXEDINDICATORS
			WHERE INDICATOR_NAME LIKE '%' + LTRIM(RTRIM(@TMP_INDICATOR_NAME)) + '%' --SOUNDEX(AREA_NAME) = SOUNDEX(LTRIM(RTRIM(@TMP_AREA_NAME)))

			SET @I = @I + 1
		END

	END


SELECT * FROM #RESULT_INDICATORS

END
GO
/****** Object:  Table [dbo].[mRegion]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[mRegion](
	[Region_NId] [bigint] IDENTITY(1,1) NOT NULL,
	[Region_Name] [varchar](100) NOT NULL,
	[Region_ID] [varchar](50) NULL,
 CONSTRAINT [PK_RegionList] PRIMARY KEY CLUSTERED 
(
	[Region_NId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[dw_OnlineUsers]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[dw_OnlineUsers]          
          
As          
          
Begin          
          
Select mUserInfo.FirstName + ' ' + mUserInfo.LastName as login,mUserInfo.Organization,        
mCountry.Country_Name,mCountry.Longitude,mCountry.Latitude,          
mUserInfo.Email,mCountry.CountryFlag_URL          
from mUserInfo          
inner join mCountry          
on mCountry.Country_Nid=mUserInfo.Country_NId          
where IsOnline=1         
          
End
GO
/****** Object:  StoredProcedure [dbo].[dw_Activations]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[dw_Activations]          
          
As          
          
Begin          
          
select mUserInfo.FirstName + ' ' + mUserInfo.LastName as login,mUserInfo.Organization,mCountry.Country_Name,          
mCountry.Longitude,mCountry.Latitude,Email,mCountry.CountryFlag_URL,    
  mAdaptation.Adaptation_Name,mUserInfo.Isonline       
from mUserInfo           
inner join mCountry           
on mCountry.Country_NId=mUserInfo.Country_NId     
inner join ruseradaptation    
on ruseradaptation.User_Nid=mUserInfo.User_Nid    
inner join mAdaptation    
on ruseradaptation.Adaptation_Nid=mAdaptation.Adaptation_Nid    
             
          
End
GO
/****** Object:  StoredProcedure [dbo].[dw_StatsCount]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[dw_StatsCount]      
      
As      
      
Begin      
      
Select count(*) as Counts   
from rCountryAdaptation A    
inner join mCountry B    
on A.Country_NId=B.Country_Nid    
inner join mAdaptation C    
on C.Adaptation_NId=A.Adaptation_NId    
union all
Select count(*) as Counts     
from mUserInfo        
inner join mCountry        
on mCountry.Country_Nid=mUserInfo.Country_NId        
where IsOnline='1'        
union all
select count(*) as Counts
from mUserInfo         
inner join mCountry         
on mCountry.Country_NId=mUserInfo.Country_NId   
inner join ruseradaptation  
on ruseradaptation.User_Nid=mUserInfo.User_Nid  
inner join mAdaptation  
on ruseradaptation.Adaptation_Nid=mAdaptation.Adaptation_Nid   
      
End
GO
/****** Object:  Table [dbo].[Sheet1$]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sheet1$](
	[id] [float] NULL,
	[name] [nvarchar](255) NULL,
	[c_id] [nvarchar](255) NULL,
	[Country] [nvarchar](255) NULL,
	[Longitude] [float] NULL,
	[Latitude] [float] NULL,
	[F7] [nvarchar](255) NULL,
	[Country1] [nvarchar](255) NULL,
	[Latitude1] [float] NULL,
	[Longitude1] [float] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[mCountry]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[mCountry](
	[Country_NId] [bigint] IDENTITY(1,1) NOT NULL,
	[Country_Name] [varchar](100) NULL,
	[Country_ID] [varchar](50) NULL,
	[Longitude] [nvarchar](255) NULL,
	[Latitude] [nvarchar](255) NULL,
	[CountryFlag_URL] [nvarchar](255) NULL,
	[CountryFactSheet_URL] [varchar](255) NULL,
 CONSTRAINT [PK_mCountry] PRIMARY KEY CLUSTERED 
(
	[Country_NId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[mAdaptation]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[mAdaptation](
	[Adaptation_NId] [int] IDENTITY(1,1) NOT NULL,
	[Adaptation_Name] [varchar](255) NULL,
	[Adaptation_Ver] [varchar](20) NULL,
	[Adaptation_Logo] [varchar](150) NULL,
 CONSTRAINT [PK_mAdaptation] PRIMARY KEY CLUSTERED 
(
	[Adaptation_NId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[rUserAdaptation]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rUserAdaptation](
	[UserAdaptation_NId] [int] IDENTITY(1,1) NOT NULL,
	[User_NId] [int] NOT NULL,
	[Adaptation_NId] [int] NOT NULL,
 CONSTRAINT [PK_rUserAdaptation] PRIMARY KEY CLUSTERED 
(
	[UserAdaptation_NId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rCountryAdaptation]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[rCountryAdaptation](
	[CountryAdaptation_NId] [int] NOT NULL,
	[Country_NId] [int] NULL,
	[Adaptation_NId] [int] NULL,
	[FocalPoints] [nvarchar](255) NULL,
	[FocalPoint_Emails] [nvarchar](255) NULL,
	[Implementing_Agency] [varchar](255) NULL,
	[Homepage_URL] [varchar](255) NULL,
	[Databases] [varchar](255) NULL,
	[WebSite] [varchar](255) NULL,
	[LastUpdated] [nvarchar](255) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[mUserInfo]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[mUserInfo](
	[User_NId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[Login] [varchar](20) NULL,
	[Email] [nvarchar](100) NULL,
	[ContactDetails] [nvarchar](255) NULL,
	[GUID] [nvarchar](255) NULL,
	[Password] [nvarchar](20) NULL,
	[Organization] [varchar](50) NULL,
	[Organization_Type] [varchar](50) NULL,
	[Longitude] [nvarchar](255) NULL,
	[Latitude] [nvarchar](255) NULL,
	[Country_NId] [nvarchar](50) NULL,
	[Region_NId] [varchar](50) NULL,
	[IP] [nvarchar](20) NULL,
	[IsOnline] [bit] NULL,
	[IsFocalPoint] [bit] NULL,
	[LastLoggedIn] [datetime] NULL,
 CONSTRAINT [PK_mUserInfo] PRIMARY KEY CLUSTERED 
(
	[User_NId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Global_UserLogin]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Global_UserLogin](
	[NId] [int] IDENTITY(1,1) NOT NULL,
	[User_Email_Id] [varchar](200) NOT NULL,
	[User_Password] [varchar](200) NOT NULL,
	[User_First_Name] [varchar](200) NULL,
	[User_Last_Name] [varchar](200) NULL,
	[User_AreaNid] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProviderDetails]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProviderDetails](
	[UserNid] [int] NOT NULL,
	[AdaptationNid] [int] NOT NULL,
	[User_Is_Provider] [nvarchar](10) NOT NULL,
	[User_Is_Admin] [nvarchar](10) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IndexedIndicators]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IndexedIndicators](
	[Indicator_NId] [int] IDENTITY(1,1) NOT NULL,
	[Indicator_Name] [nvarchar](255) NOT NULL,
	[Indicator_GId] [nvarchar](60) NULL,
	[Indicator_Global] [bit] NOT NULL,
	[Short_Name] [nvarchar](50) NULL,
	[Keywords] [nvarchar](255) NULL,
	[Indicator_Order] [int] NULL,
	[Data_Exist] [bit] NULL,
	[HighIsGood] [bit] NULL,
	[Adpt_NId] [int] NOT NULL,
	[Language_Code] [nvarchar](2) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Areas]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Areas](
	[AreaNId] [int] IDENTITY(1,1) NOT NULL,
	[AreaName] [nvarchar](4000) NOT NULL,
	[ParentNId] [int] NOT NULL,
	[AreaID] [nvarchar](50) NOT NULL,
	[AreaLevel] [int] NOT NULL,
	[Block] [nvarchar](4000) NULL,
 CONSTRAINT [PK_Areas] PRIMARY KEY CLUSTERED 
(
	[AreaNId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[SP_INSERT_CATALOG]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_INSERT_CATALOG]	
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
	@THUMBNAIL_IMAGE_URL NVARCHAR(4000)
AS
	DECLARE @REC_COUNT INT

BEGIN
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
				THUMBNAIL_IMAGE_URL		
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
				@THUMBNAIL_IMAGE_URL		
			)	
		END
	
	IF(@IS_ONLINE=1)
		BEGIN
			SET @REC_COUNT = (SELECT COUNT(*) FROM ADAPTATIONS 
				WHERE ONLINE_URL = @ONLINE_URL)

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
						THUMBNAIL_IMAGE_URL		
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
						@THUMBNAIL_IMAGE_URL		
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
						TIME_PERIODS_COUNT = @TIME_PERIODS_COUNT,
						DATA_VALUES_COUNT = @DATA_VALUES_COUNT,		
						START_YEAR = @START_YEAR,
						END_YEAR = @END_YEAR,
						LAST_MODIFIED = @LAST_MODIFIED,
						AREA_NID = @AREA_NID,
						SUB_NATION = @SUB_NATION,
						THUMBNAIL_IMAGE_URL = @THUMBNAIL_IMAGE_URL
						WHERE ONLINE_URL = @ONLINE_URL
				END
			END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_ADAPTATION_NID_AND_DELETE_INDEXED]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_GET_ADAPTATION_NID_AND_DELETE_INDEXED]
	@ONLINE_URL NVARCHAR(4000),
	@TABLE_TYPE NVARCHAR(20)
AS
BEGIN
	DECLARE @ADPT_NID INT

	SET @ADPT_NID = (SELECT NID FROM ADAPTATIONS WHERE ONLINE_URL = @ONLINE_URL)
	
	IF(@TABLE_TYPE = 'AREA')
	BEGIN
		DELETE FROM INDEXEDAREAS WHERE ADPT_NID = @ADPT_NID
	END

	IF(@TABLE_TYPE = 'INDICATOR')
	BEGIN
		DELETE FROM INDEXEDINDICATORS WHERE ADPT_NID = @ADPT_NID
	END

	SELECT @ADPT_NID 
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DELETE_CATALOG]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_DELETE_CATALOG]
	@ADAPTATION_NID AS INT
AS
BEGIN
	DELETE FROM ADAPTATIONS WHERE NID = @ADAPTATION_NID;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UPDATE_CATALOG]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_UPDATE_CATALOG]	
	@ADAPTATION_NID INT,
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
	@THUMBNAIL_IMAGE_URL NVARCHAR(4000)
AS
BEGIN
	UPDATE ADAPTATIONS SET
		NAME = @NAME,
		DESCRIPTION = @DESCRIPTION,
		DI_VERSION = @DI_VERSION, 
		IS_DESKTOP = @IS_DESKTOP,
		IS_ONLINE = @IS_ONLINE,				
		AREA_COUNT = @AREA_COUNT,
		IUS_COUNT = @IUS_COUNT,
		TIME_PERIODS_COUNT = @TIME_PERIODS_COUNT,
		DATA_VALUES_COUNT = @DATA_VALUES_COUNT,		
		START_YEAR = @START_YEAR,
		END_YEAR = @END_YEAR,
		LAST_MODIFIED = @LAST_MODIFIED,
		AREA_NID = @AREA_NID,
		SUB_NATION = @SUB_NATION,
		THUMBNAIL_IMAGE_URL = @THUMBNAIL_IMAGE_URL
		WHERE NID = @ADAPTATION_NID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_ADAPTATION_AREAS]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_GET_ADAPTATION_AREAS]
	@WEBURL NVARCHAR(4000)
AS
BEGIN 
	DECLARE @ADPT_AREA_NID INT

	SET @ADPT_AREA_NID = (SELECT AREA_NID FROM ADAPTATIONS WHERE ONLINE_URL = @WEBURL)

	IF @ADPT_AREA_NID = -1
		BEGIN
			SELECT * FROM (
			SELECT TOP 10000 AREANID, AREANAME, AREAID, PARENTNID FROM AREAS 
				WHERE PARENTNID = -1			
				ORDER BY AREANAME) A
				UNION ALL
			SELECT * FROM (
				SELECT TOP 10000 AREANID, AREANAME, AREAID, PARENTNID 
				FROM AREAS
				WHERE AREALEVEL = 2 AND AREANID IN (SELECT DISTINCT AREA_NID FROM ADAPTATIONS WHERE AREA_NID>0)
				ORDER BY AREANAME
			) B
		END
	ELSE
		BEGIN
			SELECT AREANID, AREANAME, AREAID, PARENTNID FROM AREAS 
			WHERE AREANID = @ADPT_AREA_NID OR PARENTNID = @ADPT_AREA_NID
			ORDER BY AREANAME
		END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_CATALOG]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_GET_CATALOG]
	@WEBURL NVARCHAR(4000)
AS
BEGIN
--	IF @WEBURL = ''
--		BEGIN
--			SELECT * FROM ADAPTATIONS
--		END
--	ELSE
--		BEGIN
--			SELECT * FROM ADAPTATIONS
--			WHERE ONLINE_URL = @WEBURL
--		END


	DECLARE @ADPT_AREA_NID INT	

	SET @ADPT_AREA_NID = (SELECT AREA_NID FROM ADAPTATIONS WHERE ONLINE_URL = @WEBURL)

	IF @ADPT_AREA_NID = -1
		BEGIN
			SELECT * FROM ADAPTATIONS
			WHERE AREA_NID IN(@ADPT_AREA_NID) OR
			AREA_NID IN(
			SELECT AREANID FROM AREAS WHERE PARENTNID IN(
				SELECT AREANID FROM AREAS WHERE PARENTNID=-1)
			)
		END
	ELSE
		BEGIN
			SELECT * FROM ADAPTATIONS
			WHERE ONLINE_URL = @WEBURL
		END
END
GO
/****** Object:  StoredProcedure [dbo].[dw_Adaptations]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[dw_Adaptations]          
          
As          
          
Begin          
          
Select B.Country_Id,B.Country_Name,
IsNull(B.Longitude,'') as Longitude,
IsNull(B.Latitude,'') as Latitude, 
C.Adaptation_Name + ' ' + IsNull(C.Adaptation_Ver,'') as Adaptation_Name,           
IsNull(B.CountryFlag_URL,'') as CountryFlag_URL, 
IsNull(A.Implementing_Agency,'') as Implementing_Agency,        
IsNull(A.Homepage_URL,'') as Homepage_URL,
IsNull(A.Databases,'') as Databases,A.Website,
IsNull(B.countryFactSheet_URL,'') as countryFactSheet_URL,
IsNull(A.LastUpdated,'') as LastUpdated,          
ISNull(A.Focalpoint_Emails,'') as Focalpoint_Emails,
IsNull(A.Focalpoints,'') as  Focalpoints  
        
from rCountryAdaptation A        
inner join mCountry B        
on A.Country_NId=B.Country_Nid        
inner join mAdaptation C        
on C.Adaptation_NId=A.Adaptation_NId       
          
End
GO
/****** Object:  StoredProcedure [dbo].[dw_UpdateOnlineUsersStatus]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[dw_UpdateOnlineUsersStatus]       
    -- Add the parameters for the stored procedure here       
     @KMLType as Varchar(50)      
          
AS   
BEGIN  
  
    Declare @Object as Int;   
    Declare @Url as Varchar(8000);  
    Declare @RowsAffected as Int  
  
-- To Update The Status for Online Users whose LastloggedInTime is more than 8 hours for Online Users --  
  
Update mUserInfo Set IsOnline='0' where User_NId in  
(Select User_NId from muserinfo where DATEDIFF(Minute, LastLoggedIn, Getdate()) > 480 and IsOnline='1');  
  
Select @RowsAffected=@@Rowcount  
  
--If Records are Updated Then Generate KML  
If @RowsAffected > 0  
Begin  
  
-- To Generate KML For Online Users --  
  
Select @Url = 'http://dg22/diworldwide/Utility.asmx/GenerateKML?KMLType='+ @KMLType;  
    Exec sp_OACreate 'MSXML2.XMLHTTP', @Object OUT;   
    Exec sp_OAMethod @Object, 'open', NULL, 'get', @Url, 'false'   
    Exec sp_OAMethod @Object, 'send'       
    Exec sp_OADestroy @Object  
End  
  
End
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_M49_COUNTRIES]    Script Date: 05/21/2012 12:05:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_GET_M49_COUNTRIES]
AS
BEGIN
	SELECT AREANID, AREANAME, AREAID FROM AREAS WHERE AREALEVEL = 2
	ORDER BY AREANAME
END
GO
/****** Object:  Default [DF_mUserInfo_IsOnline]    Script Date: 05/21/2012 12:05:47 ******/
ALTER TABLE [dbo].[mUserInfo] ADD  CONSTRAINT [DF_mUserInfo_IsOnline]  DEFAULT (0) FOR [IsOnline]
GO
/****** Object:  Default [DF_mUserInfo_IsFocalPoint]    Script Date: 05/21/2012 12:05:47 ******/
ALTER TABLE [dbo].[mUserInfo] ADD  CONSTRAINT [DF_mUserInfo_IsFocalPoint]  DEFAULT (0) FOR [IsFocalPoint]
GO
