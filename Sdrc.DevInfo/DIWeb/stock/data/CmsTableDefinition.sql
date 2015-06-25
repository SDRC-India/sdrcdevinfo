USE [DBName]

-------------END CREATE TABLES-----------------------------------------------------------------------------------------------

--CREATE TABLE : [dbo].[DataContent]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DataContent]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DataContent](
	[ContentId] [int] IDENTITY(1,1) NOT NULL,
	[MenuCategory] [nvarchar](50) NOT NULL,
	[Title] [nvarchar](500) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Thumbnail] [nvarchar](500) NULL,
	[Summary] [nvarchar](MAX) NOT NULL,
	[Description] [ntext] NOT NULL,
	[PDFUpload] [nvarchar](500) NULL,
	[DateAdded] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[Archived] [bit] NOT NULL,
	[ArticleTagId] [int] NULL,
	[UserNameEmail] [nvarchar](300) NULL,
	[URL] [nvarchar](300) NULL,
	[LngCode] [nvarchar](2) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[IsHidden] [bit] NOT NULL,
	[Fld1] [nvarchar](255) NULL,
	[Fld2] [nvarchar](255) NULL,
	[Fld3] [nvarchar](255) NULL,
	[Fld4] [nvarchar](255) NULL,
	[Fld5] [nvarchar](255) NULL,
	[Fld6] [nvarchar](255) NULL,
	[Fld1Text] [ntext] NULL,
	[Fld2Text] [ntext] NULL,
	[Fld3Text] [ntext] NULL,
	[Fld4Text] [ntext] NULL,
	[Fld5Text] [ntext] NULL,
	[Fld6Text] [ntext] NULL,
 CONSTRAINT [PK_DataContent] PRIMARY KEY CLUSTERED 
(
	[ContentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
ELSE
BEGIN 
ALTER TABLE [dbo].[DataContent]
ALTER COLUMN [Summary] [nvarchar](MAX) NOT NULL
END
--SPSEPARATOR--

--CREATE TABLE : [dbo].[ArticleTagsMapping]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ArticleTagsMapping]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ArticleTagsMapping](
	[TagNId] [int] IDENTITY(1,1) NOT NULL,
	[TagId] [int] NOT NULL,
	[TagMasterNid] [int] NOT NULL
 CONSTRAINT [PK_ArticleTagsMapping] PRIMARY KEY CLUSTERED 
(
	[TagNId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
--SPSEPARATOR--


--CREATE TABLE : [dbo].[ArticleTagMaster]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ArticleTagMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ArticleTagMaster](
	[NId] [int] IDENTITY(1,1) NOT NULL,
	[ArticleTag] [nvarchar](60) NOT NULL,
 CONSTRAINT [PK_ArticleTagMaster] PRIMARY KEY CLUSTERED 
(
	[NId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
--SPSEPARATOR--

--CREATE TABLE : [dbo].[CategoryMaster]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CategoryMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CategoryMaster](
	[MenuCategory] [varchar](100) NOT NULL,
	[LinkText] [nvarchar](100) NOT NULL,
	[HeaderText] [nvarchar](500) NOT NULL,
	[HeaderDesc] [nvarchar](2000) NOT NULL,
	[IsVisible] [bit] NOT NULL,
	[SortingOrder] [tinyint] NOT NULL,
	[PageName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_CategoryMaster] PRIMARY KEY CLUSTERED 
(
	[MenuCategory] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
--SPSEPARATOR--
-------------END CREATE TABLES-----------------------------------------------------------------------------------------------

-------------START INSERT NEW COLUMN (PageName) TO TABLE CategoryMaster -----------------------------------------------------------------------------------------------

IF NOT EXISTS(SELECT * FROM sys.columns where Name = N'PageName' and Object_ID = Object_ID(N'CategoryMaster'))
Begin 
ALTER TABLE [dbo].[CategoryMaster] ADD PageName varchar(50);
END
-------------END INSERT NEW COLUMN (PageName) TO TABLE CategoryMaster-----------------------------------------------------------------------------------------------

---------------------------------------------------------------------------------------------------
--This below Go is Used to break the queries into 2 parts.  It is required. Please dont remove it.
--GO
---------------------------------------------------------------------------------------------------



------------------------------------ Insert Default Records in Table-----------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM [dbo].[CategoryMaster] WHERE [MenuCategory] = 'news')
    BEGIN
	   INSERT [dbo].[CategoryMaster] ([MenuCategory], [LinkText], [HeaderText], [HeaderDesc], [IsVisible], [SortingOrder],[PageName]) 
VALUES (N'news', N'News', N'News', N'Latest database releases and important announcements.', 1, 1,'news')
    END
IF NOT EXISTS(SELECT * FROM [dbo].[CategoryMaster] WHERE [MenuCategory] = 'action')
    BEGIN
        INSERT [dbo].[CategoryMaster] ([MenuCategory], [LinkText], [HeaderText], [HeaderDesc], [IsVisible], [SortingOrder],[PageName])
        VALUES (N'action', N'DevInfo In Action', N'DevInfo In Action', N'Stories from the field highlighting use of DevInfo.', 1, 2,'news')
    END
IF NOT EXISTS(SELECT * FROM [dbo].[CategoryMaster] WHERE [MenuCategory] = 'facts')
    BEGIN
	   INSERT [dbo].[CategoryMaster] ([MenuCategory], [LinkText], [HeaderText], [HeaderDesc], [IsVisible], [SortingOrder],[PageName]) 
	   VALUES (N'facts', N'Facts of the Week', N'Facts of the Week', N'Facts relating to the Millennium Development Goals.', 1, 3,'news')
    END
-------------------------------------------------------------------------------------------------------------------------------------
IF NOT EXISTS(SELECT * FROM [dbo].[CategoryMaster] WHERE [MenuCategory] = 'faq')
    BEGIN
	   INSERT [dbo].[CategoryMaster] ([MenuCategory], [LinkText], [HeaderText], [HeaderDesc],
	    [IsVisible], [SortingOrder],[PageName]) 
	   VALUES (N'FAQ', N'FAQ', N'FAQ', '', 1, 1,'FaqNKb')
    END    
IF NOT EXISTS(SELECT * FROM [dbo].[CategoryMaster] WHERE [MenuCategory] = 'KnowledgeBase')
    BEGIN
	   INSERT [dbo].[CategoryMaster] ([MenuCategory], [LinkText], [HeaderText], [HeaderDesc], [IsVisible], [SortingOrder],[PageName]) 
	   VALUES (N'KnowledgeBase', N'Knowledge Base', N'Knowledge Base', '', 1, 2,'FaqNKb')
    END    
IF NOT EXISTS(SELECT * FROM [dbo].[CategoryMaster] WHERE [MenuCategory] = 'HowToVideo')
    BEGIN
	   INSERT [dbo].[CategoryMaster] ([MenuCategory], [LinkText], [HeaderText], [HeaderDesc], [IsVisible], [SortingOrder],[PageName]) 
	   VALUES (N'HowToVideo', N'How To Video', N'How To Video', '', 1, 3,'FaqNKb')
    END  
      
IF NOT EXISTS(SELECT * FROM [dbo].[CategoryMaster] WHERE [MenuCategory] = 'Innovations')
    BEGIN
	   INSERT [dbo].[CategoryMaster] ([MenuCategory], [LinkText], [HeaderText], [HeaderDesc], [IsVisible], [SortingOrder],[PageName]) 
	   VALUES (N'Innovations', N'Innovations', N'Innovations', N'di Monitoring, di Profile, di Gameworks, di Dashboard.', 1, 1,'Innovations')
    END    
IF NOT EXISTS(SELECT * FROM [dbo].[DataContent] WHERE [MenuCategory] = 'Innovations' and [URL]='Dashboards')
    BEGIN
	   INSERT [dbo].[DataContent] ([MenuCategory],[Title],[Date],[Summary],[Description],[DateAdded],[DateModified],[Archived],[LngCode],[IsDeleted],[IsHidden],[URL]) 
VALUES (N'innovations', N'Dashboards',GETDATE(),N'Dashboards',N'Dashboards',GETDATE(),GETDATE(),'false','en','false','false',N'Dashboards')
    END    
IF NOT EXISTS(SELECT * FROM [dbo].[DataContent] WHERE [MenuCategory] = 'Innovations' and [URL]='Profile')
    BEGIN
	   INSERT [dbo].[DataContent] ([MenuCategory], [Title],[Date],[Summary],[Description],[DateAdded],[DateModified],[Archived],[LngCode],[IsDeleted],[IsHidden],[URL]) 
VALUES (N'innovations', N'Profile',GETDATE(),N'Profile',N'Profile',GETDATE(),GETDATE(),'false','en','false','false',N'Profile')
    END
IF NOT EXISTS(SELECT * FROM [dbo].[DataContent] WHERE [MenuCategory] = 'Innovations' and [URL]='Monitoring')
    BEGIN
	   INSERT [dbo].[DataContent] ([MenuCategory], [Title],[Date],[Summary],[Description],[DateAdded],[DateModified],[Archived],[LngCode],[IsDeleted],[IsHidden],[URL]) 
VALUES (N'innovations', N'Monitoring',GETDATE(),N'Monitoring',N'Monitoring',GETDATE(),GETDATE(),'false','en','false','false',N'Monitoring')
    END
------------------------------------------------------------------------------------------------------------------    
IF NOT EXISTS(SELECT PageName FROM [dbo].[CategoryMaster] WHERE [MenuCategory] = 'news' AND PageName IS NOT NULL)
    BEGIN
	   UPDATE [dbo].[CategoryMaster] SET [PageName]='News' WHERE [MenuCategory] = 'news'
    END    
IF NOT EXISTS(SELECT PageName FROM [dbo].[CategoryMaster] WHERE [MenuCategory] = 'action' AND PageName IS NOT NULL)
    BEGIN
    UPDATE [dbo].[CategoryMaster] SET [PageName]='News' WHERE [MenuCategory] = 'action'
    END    
IF NOT EXISTS(SELECT PageName FROM [dbo].[CategoryMaster] WHERE [MenuCategory] = 'facts' AND PageName IS NOT NULL)
    BEGIN
	   UPDATE [dbo].[CategoryMaster] SET [PageName]='News' WHERE [MenuCategory] = 'facts'
    END
------------------------------------ Insert Default Records in Table-----------------------------------------------------------------


-------------CREATE STORED PROCEDURES------------------------------------------------------------------------------------------------
--CREATE STORED PROCEDURE : [dbo].[sp_AddCMSContent]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_AddCMSContent]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_AddCMSContent]
(    
 @MenuCategory varchar(50),    
 @Title nvarchar(500), 
 @Date datetime ,
 @Thumbnail varchar(500),
 @Summary nvarchar(2000),
 @Description nvarchar(max),
 @PDFUpload nvarchar(500),
 @DateAdded datetime,
 @DateModified datetime,
 @Archived bit,
 @ArticleTagId int,
 @UserNameEmail nvarchar(300),
 @URL nvarchar(300),
 @LngCode nvarchar(2),
 @IsDeleted bit,
 @IsHidden bit,
 @Fld1 nvarchar(255),
 @Fld2 nvarchar(255),
 @Fld3 nvarchar(255),
 @Fld4 nvarchar(255),
 @Fld5 nvarchar(255),
 @Fld6 nvarchar(255),
 @Fld1Text nvarchar(max),
 @Fld2Text nvarchar(max),
 @Fld3Text nvarchar(max),
 @Fld4Text nvarchar(max),
 @Fld5Text nvarchar(max),
 @Fld6Text nvarchar(max)
)    
AS     
BEGIN  

Insert Into DataContent (MenuCategory
,Title
,Date,
Thumbnail
,Summary,
Description,
PDFUpload,
DateAdded,
DateModified,
Archived,
ArticleTagId,
UserNameEmail,
URL,
LngCode,
IsDeleted,
IsHidden)
Values (
@MenuCategory ,
@Title ,
@Date , 
@Thumbnail , 
@Summary , 
@Description ,
@PDFUpload , 
@DateAdded , 
@DateModified , 
@Archived , 
@ArticleTagId,
@UserNameEmail,
@URL ,
@LngCode,
@IsDeleted,
@IsHidden )
END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[sp_AddCMSContent]
(    
 @MenuCategory varchar(50),    
 @Title nvarchar(500), 
 @Date datetime ,
 @Thumbnail varchar(500),
 @Summary nvarchar(2000),
 @Description nvarchar(max),
 @PDFUpload nvarchar(500),
 @DateAdded datetime,
 @DateModified datetime,
 @Archived bit,
 @ArticleTagId int,
 @UserNameEmail nvarchar(300),
 @URL nvarchar(300),
 @LngCode nvarchar(2),
 @IsDeleted bit,
 @IsHidden bit,
 @Fld1 nvarchar(255),
 @Fld2 nvarchar(255),
 @Fld3 nvarchar(255),
 @Fld4 nvarchar(255),
 @Fld5 nvarchar(255),
 @Fld6 nvarchar(255),
 @Fld1Text nvarchar(max),
 @Fld2Text nvarchar(max),
 @Fld3Text nvarchar(max),
 @Fld4Text nvarchar(max),
 @Fld5Text nvarchar(max),
 @Fld6Text nvarchar(max)
)    
AS     
BEGIN  

Insert Into DataContent (MenuCategory
,Title
,Date,
Thumbnail
,Summary,
Description,
PDFUpload,
DateAdded,
DateModified,
Archived,
ArticleTagId,
UserNameEmail,
URL,
LngCode,
IsDeleted,
IsHidden)
Values (
@MenuCategory ,
@Title ,
@Date , 
@Thumbnail , 
@Summary , 
@Description ,
@PDFUpload , 
@DateAdded , 
@DateModified , 
@Archived , 
@ArticleTagId,
@UserNameEmail,
@URL ,
@LngCode,
@IsDeleted,
@IsHidden )
END
' 
END
--SPSEPARATOR--


--CREATE STORED PROCEDURE : [dbo].[sp_AddMenuCategory]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_AddMenuCategory]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_AddMenuCategory]
@MenuCategory varchar(100),
@LinkText nvarchar(100),
@HeaderText nvarchar(100),
@HeaderDesc nvarchar(2000),
@IsVisible bit,
@PageName nvarchar(50)
AS
BEGIN
	DECLARE  @MaxTagId tinyint
	DECLARE @MaxSortingOrder tinyint
	-- Get Max SortingOrder Of Mapping Table,
	-- If TagId Is Null not null Then Increment Max TagId With 
	-- Else If Tag Id Is Null Set TagId=1
	SET @MaxTagId=(SELECT MAX(SortingOrder) FROM CategoryMaster)
	IF (@MaxTagId<>0)
	BEGIN
	SET @MaxSortingOrder=@MaxTagId+1
	END
	ELSE
	BEGIN
    SET @MaxSortingOrder=1
    END

INSERT INTO CategoryMaster (LinkText,HeaderText,HeaderDesc ,MenuCategory,IsVisible,SortingOrder,PageName)
VALUES(@LinkText,@HeaderText,@HeaderDesc ,@MenuCategory,@IsVisible,@MaxSortingOrder,@PageName)
END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[sp_AddMenuCategory]
@MenuCategory varchar(100),
@LinkText nvarchar(100),
@HeaderText nvarchar(100),
@HeaderDesc nvarchar(2000),
@IsVisible bit,
@PageName nvarchar(50)
AS
BEGIN
	DECLARE  @MaxTagId tinyint
	DECLARE @MaxSortingOrder tinyint
	-- Get Max SortingOrder Of Mapping Table,
	-- If TagId Is Null not null Then Increment Max TagId With 
	-- Else If Tag Id Is Null Set TagId=1
	SET @MaxTagId=(SELECT MAX(SortingOrder) FROM CategoryMaster)
	IF (@MaxTagId<>0)
	BEGIN
	SET @MaxSortingOrder=@MaxTagId+1
	END
	ELSE
	BEGIN
    SET @MaxSortingOrder=1
    END

INSERT INTO CategoryMaster (LinkText,HeaderText,HeaderDesc ,MenuCategory,IsVisible,SortingOrder,PageName)
VALUES(@LinkText,@HeaderText,@HeaderDesc ,@MenuCategory,@IsVisible,@MaxSortingOrder,@PageName)
END
' 
END
--SPSEPARATOR--


--CREATE STORED PROCEDURE : [dbo].[sp_CreateAndGetTagNid]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateAndGetTagNid]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_CreateAndGetTagNid]
@Tags nvarchar(max)
AS
BEGIN
	DECLARE  @MaxTagId int
	DECLARE @Result int
	DECLARE @TagValue varchar(60)	
	DECLARE @MasterTagId int
	
	BEGIN
	-- Get Max TagID Of Mapping Table,
	-- If TagId Is Null not null Then Increment Max TagId With 
	-- Else If Tag Id Is Null Set TagId=1
	SET @MaxTagId=(SELECT MAX(TagId) FROM ArticleTagsMapping)
	IF (@MaxTagId<>0)
	BEGIN
	SET @MaxTagId=@MaxTagId+1
	END
	ELSE
	BEGIN
    SET @MaxTagId=1
	END
	
	
	---split Tag by comma and get all tags 
	DECLARE @name varchar(50);
	-- Declare a curser 
	-- Call split function to split Input Tag, since this Tags Contains multiple tags seprated with comma
	-- Assign Result of split function to Cursor
	DECLARE db_cursor CURSOR FOR select items from split(@Tags, '','')
	-- Open Cursor
	open db_cursor
	FETCH NEXT FROM db_cursor INTO @name
	-- Itterate loop through Cursor  
	WHILE @@FETCH_STATUS = 0  
	BEGIN 	
	
	-- Check If Tag Exists in table dbo.ArticleTagMaster, then get its Nid
	-- Else insert This Tag into ArticleTagMaster and Get Nid(Max Nid since auto increment is set in field Nid)
	SET @MasterTagId =(SELECT Nid FROM ArticleTagMaster WHERE ArticleTag=RTRIM(LTRIM(@name)))
	
	-------------------------------------------------------------
	IF ( @MasterTagId is NULL)-- Check if tag is not present in database 
	BEGIN
	-- Insert This Tag in to master table ArticleTagMaster
	-- Get Nid of this inserted tag
	-- Since Auto increment is set on field Nid Use Max Function To get Nid
	-- Now Insert This Nid Into Mapping Table
	INSERT INTO ArticleTagMaster(ArticleTag) VALUES(RTRIM(LTRIM(@name)))
	INSERT INTO ArticleTagsMapping(TagId, TagMasterNid) VALUES(@MaxTagId,(SELECT MAX(Nid) FROM ArticleTagMaster))
	END
	
	ELSE --Else Check if tag is present in database 
	-- Insert This Tag Id into mapping Table
	BEGIN	
	INSERT INTO ArticleTagsMapping(TagId, TagMasterNid) VALUES(@MaxTagId, @MasterTagId)
	END
	-------------------------------------------------------------
    FETCH NEXT FROM db_cursor INTO @name 
	END	
 
	END  
	
	CLOSE db_cursor  
	DEALLOCATE db_cursor 
	SET @Result =@MaxTagId

  SELECT MAX(TagId) 
	FROM ArticleTagsMapping
	--RETURN @MaxTagId
END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[sp_CreateAndGetTagNid]
@Tags nvarchar(max)
AS
BEGIN
	DECLARE  @MaxTagId int
	DECLARE @Result int
	DECLARE @TagValue varchar(60)	
	DECLARE @MasterTagId int
	
	BEGIN
	-- Get Max TagID Of Mapping Table,
	-- If TagId Is Null not null Then Increment Max TagId With 
	-- Else If Tag Id Is Null Set TagId=1
	SET @MaxTagId=(SELECT MAX(TagId) FROM ArticleTagsMapping)
	IF (@MaxTagId<>0)
	BEGIN
	SET @MaxTagId=@MaxTagId+1
	END
	ELSE
	BEGIN
    SET @MaxTagId=1
	END
	
	
	---split Tag by comma and get all tags 
	DECLARE @name varchar(50);
	-- Declare a curser 
	-- Call split function to split Input Tag, since this Tags Contains multiple tags seprated with comma
	-- Assign Result of split function to Cursor
	DECLARE db_cursor CURSOR FOR select items from split(@Tags, '','')
	-- Open Cursor
	open db_cursor
	FETCH NEXT FROM db_cursor INTO @name
	-- Itterate loop through Cursor  
	WHILE @@FETCH_STATUS = 0  
	BEGIN 	
	
	-- Check If Tag Exists in table dbo.ArticleTagMaster, then get its Nid
	-- Else insert This Tag into ArticleTagMaster and Get Nid(Max Nid since auto increment is set in field Nid)
	SET @MasterTagId =(SELECT Nid FROM ArticleTagMaster WHERE ArticleTag=RTRIM(LTRIM(@name)))
	
	-------------------------------------------------------------
	IF ( @MasterTagId is NULL)-- Check if tag is not present in database 
	BEGIN
	-- Insert This Tag in to master table ArticleTagMaster
	-- Get Nid of this inserted tag
	-- Since Auto increment is set on field Nid Use Max Function To get Nid
	-- Now Insert This Nid Into Mapping Table
	INSERT INTO ArticleTagMaster(ArticleTag) VALUES(RTRIM(LTRIM(@name)))
	INSERT INTO ArticleTagsMapping(TagId, TagMasterNid) VALUES(@MaxTagId,(SELECT MAX(Nid) FROM ArticleTagMaster))
	END
	
	ELSE --Else Check if tag is present in database 
	-- Insert This Tag Id into mapping Table
	BEGIN	
	INSERT INTO ArticleTagsMapping(TagId, TagMasterNid) VALUES(@MaxTagId, @MasterTagId)
	END
	-------------------------------------------------------------
    FETCH NEXT FROM db_cursor INTO @name 
	END	
 
	END  
	
	CLOSE db_cursor  
	DEALLOCATE db_cursor 
	SET @Result =@MaxTagId

  SELECT MAX(TagId) 
	FROM ArticleTagsMapping
	--RETURN @MaxTagId
END
' 
END
--SPSEPARATOR--


--CREATE STORED PROCEDURE : [dbo].[sp_DeleteArticleByContentId]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteArticleByContentId]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_DeleteArticleByContentId]
@ContentId int
AS
BEGIN
UPDATE DataContent SET IsDeleted=''True'' WHERE ContentId=@ContentId  
END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[sp_DeleteArticleByContentId]
@ContentId int
AS
BEGIN
UPDATE DataContent SET IsDeleted=''True'' WHERE ContentId=@ContentId  
END
' 
END
--SPSEPARATOR--


--CREATE STORED PROCEDURE : [dbo].[sp_DeleteTagsMappingsByTagId]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteTagsMappingsByTagId]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_DeleteTagsMappingsByTagId]
@TagId int
AS
BEGIN
DELETE FROM ArticleTagsMapping WHERE TagNId=@TagId  
END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[sp_DeleteTagsMappingsByTagId]
@TagId int
AS
BEGIN
DELETE FROM ArticleTagsMapping WHERE TagNId=@TagId  
END
' 
END
--SPSEPARATOR--


--CREATE STORED PROCEDURE : [dbo].[sp_DeleteMenuCategory]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteMenuCategory]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_DeleteMenuCategory]
@MenuCategory varchar(100)
AS
BEGIN
DELETE FROM CategoryMaster WHERE MenuCategory=@MenuCategory 
DELETE FROM DataContent WHERE MenuCategory=@MenuCategory 
END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[sp_DeleteMenuCategory]
@MenuCategory varchar(100)
AS
BEGIN
DELETE FROM CategoryMaster WHERE MenuCategory=@MenuCategory 
DELETE FROM DataContent WHERE MenuCategory=@MenuCategory 
END
' 
END
--SPSEPARATOR--


--CREATE STORED PROCEDURE : [dbo].[sp_GetAllTagsByMenuCategory]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetAllTagsByMenuCategory]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_GetAllTagsByMenuCategory]
@MenuCategory nvarchar(50),
@IsHiddenArticlesVisible bit
AS
BEGIN

IF @IsHiddenArticlesVisible = ''False''
BEGIN
SELECT DISTINCT ATG.Nid ,ATG.ArticleTag  FROM ArticleTagMaster ATG,ArticleTagsMapping ATM,DataContent DC 
WHERE
ATG.NId = ATM.TagMasterNid AND
ATM.TagId = DC.ArticleTagId AND
DC.MenuCategory=@MenuCategory
AND IsHidden=''False'' AND IsDeleted=''False'' ORDER BY ATG.ArticleTag ASC
END
ELSE
BEGIN
SELECT DISTINCT ATG.Nid ,ATG.ArticleTag  FROM ArticleTagMaster ATG,ArticleTagsMapping ATM,DataContent DC 
WHERE
ATG.NId = ATM.TagMasterNid AND
ATM.TagId = DC.ArticleTagId AND
DC.MenuCategory=@MenuCategory
AND IsDeleted=''False'' ORDER BY ATG.ArticleTag ASC
END
END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[sp_GetAllTagsByMenuCategory]
@MenuCategory nvarchar(50),
@IsHiddenArticlesVisible bit
AS
BEGIN

IF @IsHiddenArticlesVisible = ''False''
BEGIN
SELECT DISTINCT ATG.Nid ,ATG.ArticleTag  FROM ArticleTagMaster ATG,ArticleTagsMapping ATM,DataContent DC 
WHERE
ATG.NId = ATM.TagMasterNid AND
ATM.TagId = DC.ArticleTagId AND
DC.MenuCategory=@MenuCategory
AND IsHidden=''False'' AND IsDeleted=''False'' ORDER BY ATG.ArticleTag ASC
END
ELSE
BEGIN
SELECT DISTINCT ATG.Nid ,ATG.ArticleTag  FROM ArticleTagMaster ATG,ArticleTagsMapping ATM,DataContent DC 
WHERE
ATG.NId = ATM.TagMasterNid AND
ATM.TagId = DC.ArticleTagId AND
DC.MenuCategory=@MenuCategory
AND IsDeleted=''False'' ORDER BY ATG.ArticleTag ASC
END
END
' 
END
--SPSEPARATOR--


--CREATE STORED PROCEDURE : [dbo].[sp_EditMenuCategory]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_EditMenuCategory]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_EditMenuCategory]
@MenuCategory varchar(100),
@LinkText nvarchar(100),
@HeaderText nvarchar(100),
@HeaderDesc nvarchar(2000),
@IsVisible bit,
@PageName nvarchar(50)
AS
BEGIN
UPDATE CategoryMaster SET LinkText=@LinkText, HeaderText=@HeaderText,HeaderDesc=@HeaderDesc, IsVisible=@IsVisible
WHERE MenuCategory=@MenuCategory and PageName=@PageName
END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[sp_EditMenuCategory]
@MenuCategory varchar(100),
@LinkText nvarchar(100),
@HeaderText nvarchar(100),
@HeaderDesc nvarchar(2000),
@IsVisible bit,
@PageName nvarchar(50)
AS
BEGIN
UPDATE CategoryMaster SET LinkText=@LinkText, HeaderText=@HeaderText,HeaderDesc=@HeaderDesc, IsVisible=@IsVisible
WHERE MenuCategory=@MenuCategory 
END
' 
END
--SPSEPARATOR--


--CREATE STORED PROCEDURE : [dbo].[sp_GetArticlesByMenuCategory]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetArticlesByMenuCategory]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_GetArticlesByMenuCategory]
@RecordStartPosition int,
@NoOfRecords int,
@MenuCategory nvarchar(50),
@IsHiddenArticlesVisible bit
AS
BEGIN
IF @IsHiddenArticlesVisible=''False''
BEGIN
SELECT [ContentId]
	  ,[MenuCategory]
      ,[Title]
      ,[Date]
      ,[Thumbnail]
      ,[Summary]
      ,[Description]
      ,[PDFUpload]
      ,[DateAdded]
      ,[DateModified]
      ,[Archived]
      ,[ArticleTagId]
      ,[UserNameEmail]
      ,[URL]
      ,[LngCode] 
      ,[IsHidden] FROM
    (SELECT ROW_NUMBER() 
        OVER (ORDER BY Date DESC) AS RowIndex 
       ,[ContentId]
      ,[MenuCategory]
      ,[Title]
      ,[Date]
      ,[Thumbnail]
      ,[Summary]
      ,[Description]
      ,[PDFUpload]
      ,[DateAdded]
      ,[DateModified]
      ,[Archived]
      ,[ArticleTagId]
      ,[UserNameEmail]
      ,[URL]
      ,[LngCode] 
      ,[IsHidden]
   FROM [dbo].[DataContent] WHERE MenuCategory=@MenuCategory
   And IsHidden=''False'' And IsDeleted=''False'') AS EMP
WHERE emp.RowIndex BETWEEN @RecordStartPosition AND @RecordStartPosition+@NoOfRecords-1
END
ELSE
BEGIN
SELECT [ContentId]
      ,[MenuCategory]
      ,[Title]
      ,[Date]
      ,[Thumbnail]
      ,[Summary]
      ,[Description]
      ,[PDFUpload]
      ,[DateAdded]
      ,[DateModified]
      ,[Archived]
      ,[ArticleTagId]
      ,[UserNameEmail]
      ,[URL]
      ,[LngCode] 
      ,[IsHidden] FROM
    (SELECT ROW_NUMBER() 
        OVER (ORDER BY Date DESC) AS RowIndex 
       ,[ContentId]
      ,[MenuCategory]
      ,[Title]
      ,[Date]
      ,[Thumbnail]
      ,[Summary]
      ,[Description]
      ,[PDFUpload]
      ,[DateAdded]
      ,[DateModified]
      ,[Archived]
      ,[ArticleTagId]
      ,[UserNameEmail]
      ,[URL]
      ,[LngCode] 
      ,[IsHidden]
   FROM [dbo].[DataContent] WHERE MenuCategory=@MenuCategory
   AND IsDeleted=''False'') AS EMP
WHERE emp.RowIndex BETWEEN @RecordStartPosition AND @RecordStartPosition+@NoOfRecords-1
END
END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[sp_GetArticlesByMenuCategory]
@RecordStartPosition int,
@NoOfRecords int,
@MenuCategory nvarchar(50),
@IsHiddenArticlesVisible bit
AS
BEGIN
IF @IsHiddenArticlesVisible=''False''
BEGIN
SELECT [ContentId]
	  ,[MenuCategory]
      ,[Title]
      ,[Date]
      ,[Thumbnail]
      ,[Summary]
      ,[Description]
      ,[PDFUpload]
      ,[DateAdded]
      ,[DateModified]
      ,[Archived]
      ,[ArticleTagId]
      ,[UserNameEmail]
      ,[URL]
      ,[LngCode] 
      ,[IsHidden] FROM
    (SELECT ROW_NUMBER() 
        OVER (ORDER BY Date DESC) AS RowIndex 
       ,[ContentId]
      ,[MenuCategory]
      ,[Title]
      ,[Date]
      ,[Thumbnail]
      ,[Summary]
      ,[Description]
      ,[PDFUpload]
      ,[DateAdded]
      ,[DateModified]
      ,[Archived]
      ,[ArticleTagId]
      ,[UserNameEmail]
      ,[URL]
      ,[LngCode] 
      ,[IsHidden]
   FROM [dbo].[DataContent] WHERE MenuCategory=@MenuCategory
   And IsHidden=''False'' And IsDeleted=''False'') AS EMP
WHERE emp.RowIndex BETWEEN @RecordStartPosition AND @RecordStartPosition+@NoOfRecords-1
END
ELSE
BEGIN
SELECT [ContentId]
      ,[MenuCategory]
      ,[Title]
      ,[Date]
      ,[Thumbnail]
      ,[Summary]
      ,[Description]
      ,[PDFUpload]
      ,[DateAdded]
      ,[DateModified]
      ,[Archived]
      ,[ArticleTagId]
      ,[UserNameEmail]
      ,[URL]
      ,[LngCode] 
      ,[IsHidden] FROM
    (SELECT ROW_NUMBER() 
        OVER (ORDER BY Date DESC) AS RowIndex 
       ,[ContentId]
      ,[MenuCategory]
      ,[Title]
      ,[Date]
      ,[Thumbnail]
      ,[Summary]
      ,[Description]
      ,[PDFUpload]
      ,[DateAdded]
      ,[DateModified]
      ,[Archived]
      ,[ArticleTagId]
      ,[UserNameEmail]
      ,[URL]
      ,[LngCode] 
      ,[IsHidden]
   FROM [dbo].[DataContent] WHERE MenuCategory=@MenuCategory
   AND IsDeleted=''False'') AS EMP
WHERE emp.RowIndex BETWEEN @RecordStartPosition AND @RecordStartPosition+@NoOfRecords-1
END
END
' 
END
--SPSEPARATOR--


--CREATE STORED PROCEDURE : [dbo].[sp_GetArticlesByMenuCategoryNTags]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetArticlesByMenuCategoryNTags]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_GetArticlesByMenuCategoryNTags]
@RecordStartPosition int,
@NoOfRecords int,
@MenuCategory nvarchar(50),
@TagIds nvarchar(max),
@IsHiddenArticlesVisible bit

AS
BEGIN
IF @IsHiddenArticlesVisible = ''False''

DECLARE @LastRecordNumber int = @RecordStartPosition + @NoOfRecords - 1;

IF @IsHiddenArticlesVisible = ''False''
BEGIN
	SELECT
	  [ContentId]
	  ,[MenuCategory]
      ,[Title]
      ,[Date]
      ,[Thumbnail]
      ,[Summary]
      ,[Description]
      ,[PDFUpload]
      ,[DateAdded]
      ,[DateModified]
      ,[Archived]
      ,[ArticleTagId]
      ,[UserNameEmail]
      ,[URL]
      ,[LngCode]
      ,[IsHidden]
	 FROM
    (SELECT ROW_NUMBER() 
        OVER (ORDER BY Date DESC) AS RowIndex 
       ,ContentId
      ,MenuCategory
      ,Title
      ,Date
      ,Thumbnail
      ,Summary
      ,Description
      ,PDFUpload
      ,DateAdded
      ,DateModified
      ,Archived
      ,ArticleTagId
      ,UserNameEmail
      ,URL
      ,LngCode 
      ,IsHidden
   FROM [dbo].[DataContent] DC, 
   
(SELECT Distinct ATMap.TagId FROM dbo.ArticleTagsMapping ATMap
JOIN dbo.split(@TagIds, '','') TIds ON ATMap.TagMasterNid=TIds.items) AS MappingTagIDs  
WHERE  DC.ArticleTagId in (MappingTagIDs.TagId)
AND DC.MenuCategory= @MenuCategory AND IsHidden=''False'' AND IsDeleted=''False'') AS Articles
WHERE Articles.RowIndex BETWEEN @RecordStartPosition AND @RecordStartPosition+@NoOfRecords-1   
END
ELSE
BEGIN

	SELECT
	   [ContentId]
	  ,[MenuCategory]
      ,[Title]
      ,[Date]
      ,[Thumbnail]
      ,[Summary]
      ,[Description]
      ,[PDFUpload]
      ,[DateAdded]
      ,[DateModified]
      ,[Archived]
      ,[ArticleTagId]
      ,[UserNameEmail]
      ,[URL]
      ,[LngCode]
      ,[IsHidden]
	 FROM
    (SELECT ROW_NUMBER() 
        OVER (ORDER BY Date DESC) AS RowIndex
      ,ContentId
      ,MenuCategory
      ,Title
      ,Date
      ,Thumbnail
      ,Summary
      ,Description
      ,PDFUpload
      ,DateAdded
      ,DateModified
      ,Archived
      ,ArticleTagId
      ,UserNameEmail
      ,URL
      ,LngCode 
      ,IsHidden
   FROM [dbo].[DataContent] DC, 
   
(SELECT Distinct ATMap.TagId FROM dbo.ArticleTagsMapping ATMap
JOIN dbo.split(@TagIds, '','') TIds ON ATMap.TagMasterNid=TIds.items) AS MappingTagIDs  
WHERE  DC.ArticleTagId in (MappingTagIDs.TagId)
AND DC.MenuCategory= @MenuCategory AND IsDeleted=''False'') AS Articles
WHERE Articles.RowIndex BETWEEN @RecordStartPosition AND @RecordStartPosition+@NoOfRecords-1 

END
END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[sp_GetArticlesByMenuCategoryNTags]
@RecordStartPosition int,
@NoOfRecords int,
@MenuCategory nvarchar(50),
@TagIds nvarchar(max),
@IsHiddenArticlesVisible bit

AS
BEGIN
IF @IsHiddenArticlesVisible = ''False''

DECLARE @LastRecordNumber int = @RecordStartPosition + @NoOfRecords - 1;

IF @IsHiddenArticlesVisible = ''False''
BEGIN
	SELECT
	  [ContentId]
	  ,[MenuCategory]
      ,[Title]
      ,[Date]
      ,[Thumbnail]
      ,[Summary]
      ,[Description]
      ,[PDFUpload]
      ,[DateAdded]
      ,[DateModified]
      ,[Archived]
      ,[ArticleTagId]
      ,[UserNameEmail]
      ,[URL]
      ,[LngCode]
      ,[IsHidden]
	 FROM
    (SELECT ROW_NUMBER() 
        OVER (ORDER BY Date DESC) AS RowIndex 
       ,ContentId
      ,MenuCategory
      ,Title
      ,Date
      ,Thumbnail
      ,Summary
      ,Description
      ,PDFUpload
      ,DateAdded
      ,DateModified
      ,Archived
      ,ArticleTagId
      ,UserNameEmail
      ,URL
      ,LngCode 
      ,IsHidden
   FROM [dbo].[DataContent] DC, 
   
(SELECT Distinct ATMap.TagId FROM dbo.ArticleTagsMapping ATMap
JOIN dbo.split(@TagIds, '','') TIds ON ATMap.TagMasterNid=TIds.items) AS MappingTagIDs  
WHERE  DC.ArticleTagId in (MappingTagIDs.TagId)
AND DC.MenuCategory= @MenuCategory AND IsHidden=''False'' AND IsDeleted=''False'') AS Articles
WHERE Articles.RowIndex BETWEEN @RecordStartPosition AND @RecordStartPosition+@NoOfRecords-1   
END
ELSE
BEGIN

	SELECT
	   [ContentId]
	  ,[MenuCategory]
      ,[Title]
      ,[Date]
      ,[Thumbnail]
      ,[Summary]
      ,[Description]
      ,[PDFUpload]
      ,[DateAdded]
      ,[DateModified]
      ,[Archived]
      ,[ArticleTagId]
      ,[UserNameEmail]
      ,[URL]
      ,[LngCode]
      ,[IsHidden]
	 FROM
    (SELECT ROW_NUMBER() 
        OVER (ORDER BY Date DESC) AS RowIndex
      ,ContentId
      ,MenuCategory
      ,Title
      ,Date
      ,Thumbnail
      ,Summary
      ,Description
      ,PDFUpload
      ,DateAdded
      ,DateModified
      ,Archived
      ,ArticleTagId
      ,UserNameEmail
      ,URL
      ,LngCode 
      ,IsHidden
   FROM [dbo].[DataContent] DC, 
   
(SELECT Distinct ATMap.TagId FROM dbo.ArticleTagsMapping ATMap
JOIN dbo.split(@TagIds, '','') TIds ON ATMap.TagMasterNid=TIds.items) AS MappingTagIDs  
WHERE  DC.ArticleTagId in (MappingTagIDs.TagId)
AND DC.MenuCategory= @MenuCategory AND IsDeleted=''False'') AS Articles
WHERE Articles.RowIndex BETWEEN @RecordStartPosition AND @RecordStartPosition+@NoOfRecords-1 

END
END
' 
END
--SPSEPARATOR--


--CREATE STORED PROCEDURE : [dbo].[sp_GetArticlesCountByMenuCategory]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetArticlesCountByMenuCategory]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_GetArticlesCountByMenuCategory]
@MenuCategory nvarchar(50),
@IsHiddenArticlesVisible bit
AS
BEGIN
IF @IsHiddenArticlesVisible=''False''
BEGIN
SELECT COUNT(*) as Cnt FROM  [dbo].[DataContent] WHERE MenuCategory=@MenuCategory 
AND IsHidden=''False'' AND IsDeleted=''False''
END
ELSE
BEGIN
SELECT COUNT(*) as Cnt FROM  [dbo].[DataContent] WHERE MenuCategory=@MenuCategory 
AND IsDeleted=''False''
END
END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[sp_GetArticlesCountByMenuCategory]
@MenuCategory nvarchar(50),
@IsHiddenArticlesVisible bit
AS
BEGIN
IF @IsHiddenArticlesVisible=''False''
BEGIN
SELECT COUNT(*) as Cnt FROM  [dbo].[DataContent] WHERE MenuCategory=@MenuCategory 
AND IsHidden=''False'' AND IsDeleted=''False''
END
ELSE
BEGIN
SELECT COUNT(*) as Cnt FROM  [dbo].[DataContent] WHERE MenuCategory=@MenuCategory 
AND IsDeleted=''False''
END
END
' 
END
--SPSEPARATOR--


--CREATE STORED PROCEDURE : [dbo].[sp_GetArticlesCountByMenuCategoryNTag]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetArticlesCountByMenuCategoryNTag]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_GetArticlesCountByMenuCategoryNTag]
@MenuCategory nvarchar(50),
@TagIds varchar(max),
@IsHiddenArticlesVisible bit
AS
BEGIN
IF @IsHiddenArticlesVisible=''False''
BEGIN
 SELECT COUNT(*) As AtricleCount FROM 
 DataContent DC ,( SELECT DISTINCT ATMap.TagId FROM dbo.ArticleTagsMapping ATMap
 JOIN dbo.split(@TagIds, '','') TIds ON ATMap.TagMasterNid=TIds.items)
 AS MappingTagIDs  
 WHERE  DC.ArticleTagId = MappingTagIDs.TagId AND MenuCategory=@MenuCategory
 AND IsHidden=''False'' AND IsDeleted=''False''
END
ELSE
BEGIN
 SELECT COUNT(*) As AtricleCount FROM 
 DataContent DC ,( SELECT DISTINCT ATMap.TagId FROM dbo.ArticleTagsMapping ATMap
 JOIN dbo.split(@TagIds, '','') TIds ON ATMap.TagMasterNid=TIds.items)
 AS MappingTagIDs  
 WHERE  DC.ArticleTagId = MappingTagIDs.TagId AND MenuCategory=@MenuCategory
 AND IsDeleted=''False''
 END
END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[sp_GetArticlesCountByMenuCategoryNTag]
@MenuCategory nvarchar(50),
@TagIds varchar(max),
@IsHiddenArticlesVisible bit
AS
BEGIN
IF @IsHiddenArticlesVisible=''False''
BEGIN
 SELECT COUNT(*) As AtricleCount FROM 
 DataContent DC ,( SELECT DISTINCT ATMap.TagId FROM dbo.ArticleTagsMapping ATMap
 JOIN dbo.split(@TagIds, '','') TIds ON ATMap.TagMasterNid=TIds.items)
 AS MappingTagIDs  
 WHERE  DC.ArticleTagId = MappingTagIDs.TagId AND MenuCategory=@MenuCategory
 AND IsHidden=''False'' AND IsDeleted=''False''
END
ELSE
BEGIN
 SELECT COUNT(*) As AtricleCount FROM 
 DataContent DC ,( SELECT DISTINCT ATMap.TagId FROM dbo.ArticleTagsMapping ATMap
 JOIN dbo.split(@TagIds, '','') TIds ON ATMap.TagMasterNid=TIds.items)
 AS MappingTagIDs  
 WHERE  DC.ArticleTagId = MappingTagIDs.TagId AND MenuCategory=@MenuCategory
 AND IsDeleted=''False''
 END
END
' 
END
--SPSEPARATOR--


--CREATE STORED PROCEDURE : [dbo].[[sp_GetDataByUrl]]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetDataByUrl]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_GetDataByUrl]
@Url nvarchar(300),
@IsHiddenArticlesVisible bit
AS
BEGIN
IF @IsHiddenArticlesVisible = ''False''
BEGIN
SELECT [ContentId]
      ,[MenuCategory]
      ,[Title]
      ,[Date]
      ,[Thumbnail]
      ,[Summary]
      ,[Description]
      ,[PDFUpload]
      ,[DateAdded]
      ,[DateModified]
      ,[Archived]
      ,[ArticleTagId]
      ,[UserNameEmail]
      ,[URL]
      ,[LngCode]
      ,[IsHidden]
       FROM [dbo].[DataContent] WHERE [URL]=@Url
       AND IsHidden=''False'' AND IsDeleted=''False''
END
ELSE
BEGIN
SELECT [ContentId]
      ,[MenuCategory]
      ,[Title]
      ,[Date]
      ,[Thumbnail]
      ,[Summary]
      ,[Description]
      ,[PDFUpload]
      ,[DateAdded]
      ,[DateModified]
      ,[Archived]
      ,[ArticleTagId]
      ,[UserNameEmail]
      ,[URL]
      ,[LngCode]
      ,[IsHidden]
       FROM [dbo].[DataContent] WHERE [URL]=@Url
       AND IsDeleted=''False''
END
END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[sp_GetDataByUrl]
@Url nvarchar(300),
@IsHiddenArticlesVisible bit
AS
BEGIN
IF @IsHiddenArticlesVisible = ''False''
BEGIN
SELECT [ContentId]
      ,[MenuCategory]
      ,[Title]
      ,[Date]
      ,[Thumbnail]
      ,[Summary]
      ,[Description]
      ,[PDFUpload]
      ,[DateAdded]
      ,[DateModified]
      ,[Archived]
      ,[ArticleTagId]
      ,[UserNameEmail]
      ,[URL]
      ,[LngCode]
      ,[IsHidden]
       FROM [dbo].[DataContent] WHERE [URL]=@Url
       AND IsHidden=''False'' AND IsDeleted=''False''
END
ELSE
BEGIN
SELECT [ContentId]
      ,[MenuCategory]
      ,[Title]
      ,[Date]
      ,[Thumbnail]
      ,[Summary]
      ,[Description]
      ,[PDFUpload]
      ,[DateAdded]
      ,[DateModified]
      ,[Archived]
      ,[ArticleTagId]
      ,[UserNameEmail]
      ,[URL]
      ,[LngCode]
      ,[IsHidden]
       FROM [dbo].[DataContent] WHERE [URL]=@Url
       AND IsDeleted=''False''
END
END
' 
END
--SPSEPARATOR--

--CREATE STORED PROCEDURE : [dbo].[sp_GetMenuCategories]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetMenuCategories]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_GetMenuCategories]
@IsVisible bit
AS
BEGIN
SELECT * from CategoryMaster where IsVisible=@IsVisible ORDER BY SortingOrder ASC
END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[sp_GetMenuCategories]
@IsVisible bit
AS
BEGIN
SELECT * from CategoryMaster where IsVisible=@IsVisible ORDER BY SortingOrder ASC
END
' 
END
--SPSEPARATOR--


--CREATE STORED PROCEDURE : [dbo].[sp_GetMenuCategoriesByPageName]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetMenuCategoriesByPageName]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_GetMenuCategoriesByPageName]
@IsVisible bit,
@PageName nvarchar(50)
AS
BEGIN
SELECT * from CategoryMaster where IsVisible=@IsVisible and PageName=@PageName
 ORDER BY SortingOrder ASC
END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[sp_GetMenuCategoriesByPageName]
@IsVisible bit,
@PageName nvarchar(50)
AS
BEGIN
SELECT * from CategoryMaster where IsVisible=@IsVisible and PageName=@PageName
ORDER BY SortingOrder ASC
END
' 
END
--SPSEPARATOR--


--CREATE STORED PROCEDURE : [dbo].[sp_GetPageNameByMenuCategory]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetPageNameByMenuCategory]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_GetPageNameByMenuCategory]
@MenuCategory nvarchar(50)
AS
BEGIN
SELECT PageName From CategoryMaster WHERE MenuCategory=@MenuCategory
END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[sp_GetPageNameByMenuCategory]
@MenuCategory nvarchar(50)
AS
BEGIN
SELECT PageName From CategoryMaster WHERE MenuCategory=@MenuCategory
END
' 
END
--SPSEPARATOR--


--CREATE STORED PROCEDURE : [dbo].[sp_GetTagsById]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetTagsById]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_GetTagsById]
@TagId int
AS
BEGIN

SELECT ArticleTagMaster.ArticleTag FROM ArticleTagMaster,ArticleTagsMapping 
WHERE ArticleTagMaster.Nid=ArticleTagsMapping.TagMasterNid and ArticleTagsMapping.Tagid=@TagId
        
END

' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[sp_GetTagsById]
@TagId int
AS
BEGIN

SELECT ArticleTagMaster.ArticleTag FROM ArticleTagMaster,ArticleTagsMapping 
WHERE ArticleTagMaster.Nid=ArticleTagsMapping.TagMasterNid and ArticleTagsMapping.Tagid=@TagId
        
END
' 
END
--SPSEPARATOR--



--CREATE STORED PROCEDURE : [dbo].[sp_MoveUpNDownMenuCat]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_MoveUpNDownMenuCat]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_MoveUpNDownMenuCat]
@MenuCategory varchar(100),
@MoveUp bit,
@MoveDown bit,
@PageName nvarchar(50)
AS
BEGIN

Declare @CurrentSortinOrder tinyint
Declare @TempMenuCat varchar(100)

IF(@MoveUp <> 0)
BEGIN

SET @CurrentSortinOrder=(SELECT SortingOrder FROM CategoryMaster WHERE MenuCategory=@MenuCategory and PageName=@PageName)
SET @TempMenuCat=(SELECT MenuCategory FROM CategoryMaster WHERE SortingOrder=@CurrentSortinOrder-1 and PageName=@PageName)

UPDATE CategoryMaster SET SortingOrder=@CurrentSortinOrder WHERE MenuCategory=@TempMenuCat and PageName=@PageName
UPDATE CategoryMaster SET SortingOrder=@CurrentSortinOrder-1 WHERE MenuCategory=@MenuCategory and PageName=@PageName
END

ELSE
BEGIN

SET @CurrentSortinOrder=(SELECT SortingOrder FROM CategoryMaster WHERE MenuCategory=@MenuCategory and PageName=@PageName)
SET @TempMenuCat=(SELECT MenuCategory FROM CategoryMaster WHERE SortingOrder=@CurrentSortinOrder+1 and PageName=@PageName)

UPDATE CategoryMaster SET SortingOrder=@CurrentSortinOrder WHERE MenuCategory=@TempMenuCat and PageName=@PageName
UPDATE CategoryMaster SET SortingOrder=@CurrentSortinOrder+1 WHERE MenuCategory=@MenuCategory and PageName=@PageName

END
END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[sp_MoveUpNDownMenuCat]
@MenuCategory varchar(100),
@MoveUp bit,
@MoveDown bit,
@PageName nvarchar(50)
AS
BEGIN

Declare @CurrentSortinOrder tinyint
Declare @TempMenuCat varchar(100)

IF(@MoveUp <> 0)
BEGIN

SET @CurrentSortinOrder=(SELECT SortingOrder FROM CategoryMaster WHERE MenuCategory=@MenuCategory and PageName=@PageName)
SET @TempMenuCat=(SELECT MenuCategory FROM CategoryMaster WHERE SortingOrder=@CurrentSortinOrder-1 and PageName=@PageName)

UPDATE CategoryMaster SET SortingOrder=@CurrentSortinOrder WHERE MenuCategory=@TempMenuCat and PageName=@PageName
UPDATE CategoryMaster SET SortingOrder=@CurrentSortinOrder-1 WHERE MenuCategory=@MenuCategory and PageName=@PageName
END

ELSE
BEGIN

SET @CurrentSortinOrder=(SELECT SortingOrder FROM CategoryMaster WHERE MenuCategory=@MenuCategory and PageName=@PageName)
SET @TempMenuCat=(SELECT MenuCategory FROM CategoryMaster WHERE SortingOrder=@CurrentSortinOrder+1 and PageName=@PageName)

UPDATE CategoryMaster SET SortingOrder=@CurrentSortinOrder WHERE MenuCategory=@TempMenuCat and PageName=@PageName
UPDATE CategoryMaster SET SortingOrder=@CurrentSortinOrder+1 WHERE MenuCategory=@MenuCategory and PageName=@PageName

END
END
' 
END
--SPSEPARATOR--



--CREATE STORED PROCEDURE : [dbo].[sp_ShowHideArticlesByContentId]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ShowHideArticlesByContentId]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_ShowHideArticlesByContentId]
@ContentId int,
@IsHidden bit
AS
BEGIN
UPDATE DataContent SET IsHidden=@IsHidden WHERE ContentId=@ContentId  
END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[sp_ShowHideArticlesByContentId]
@ContentId int,
@IsHidden bit
AS
BEGIN
UPDATE DataContent SET IsHidden=@IsHidden WHERE ContentId=@ContentId   
END
' 
END
--SPSEPARATOR--


--CREATE STORED PROCEDURE : [dbo].[sp_UpdateCMSContentByContentId]
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_UpdateCMSContentByContentId]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_UpdateCMSContentByContentId]
(    
 @ContentId int,
 @MenuCategory varchar(50),    
 @Title nvarchar(500), 
 @Date datetime ,
 @Thumbnail varchar(500),
 @Summary nvarchar(2000),
 @Description nvarchar(max),
 @PDFUpload nvarchar(100),
 @DateAdded datetime,
 @DateModified datetime,
 @Archived bit,
 @ArticleTagId int,
 @UserNameEmail nvarchar(300),
 @LngCode nvarchar(2),
 @Fld1 nvarchar(255),
 @Fld2 nvarchar(255),
 @Fld3 nvarchar(255),
 @Fld4 nvarchar(255),
 @Fld5 nvarchar(255),
 @Fld6 nvarchar(255),
 @Fld1Text nvarchar(max),
 @Fld2Text nvarchar(max),
 @Fld3Text nvarchar(max),
 @Fld4Text nvarchar(max),
 @Fld5Text nvarchar(max),
 @Fld6Text nvarchar(max)
)    
AS     
BEGIN  

UPDATE DataContent set
MenuCategory=@MenuCategory
,Title=@Title
,Date=@Date
,Thumbnail=@Thumbnail
,Summary=@Summary
,Description=@Description
,PDFUpload=@PDFUpload
,DateAdded=@DateAdded
,DateModified=@DateModified
,Archived=@Archived
,ArticleTagId=@ArticleTagId
,UserNameEmail=@UserNameEmail
,LngCode=@LngCode

WHERE ContentId=@ContentId

 END
' 
END
ELSE
BEGIN
EXEC dbo.sp_executesql @statement = N'
ALTER PROCEDURE [dbo].[sp_UpdateCMSContentByContentId]
(    
 @ContentId int,
 @MenuCategory varchar(50),    
 @Title nvarchar(500), 
 @Date datetime ,
 @Thumbnail varchar(500),
 @Summary nvarchar(2000),
 @Description nvarchar(max),
 @PDFUpload nvarchar(500),
 @DateAdded datetime,
 @DateModified datetime,
 @Archived bit,
 @ArticleTagId int,
 @UserNameEmail nvarchar(300),
 @LngCode nvarchar(2),
 @Fld1 nvarchar(255),
 @Fld2 nvarchar(255),
 @Fld3 nvarchar(255),
 @Fld4 nvarchar(255),
 @Fld5 nvarchar(255),
 @Fld6 nvarchar(255),
 @Fld1Text nvarchar(max),
 @Fld2Text nvarchar(max),
 @Fld3Text nvarchar(max),
 @Fld4Text nvarchar(max),
 @Fld5Text nvarchar(max),
 @Fld6Text nvarchar(max)
)    
AS     
BEGIN  

UPDATE DataContent set
MenuCategory=@MenuCategory
,Title=@Title
,Date=@Date
,Thumbnail=@Thumbnail
,Summary=@Summary
,Description=@Description
,PDFUpload=@PDFUpload
,DateAdded=@DateAdded
,DateModified=@DateModified
,Archived=@Archived
,ArticleTagId=@ArticleTagId
,UserNameEmail=@UserNameEmail
,LngCode=@LngCode

WHERE ContentId=@ContentId

 END
' 
END
--SPSEPARATOR--


-------------END CREATE STORED PROCEDURES-----------------------------------------------------------------------------------------------


-------------CREATE USER DEFINED FUNCTION-----------------------------------------------------------------------------------------------
--CREATE UserDefinedFunction : [dbo].[Split] 
 
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
-------------END CREATE USER DEFINED FUNCTION-----------------------------------------------------------------------------------------------

-------------START INSERT DEFAULT RECORDS FOR MENU CATEGORY-----------------------------------------------------------------------------------------------




    

-------------END INSERT DEFAULT RECORDS FOR MENU CATEGORY-----------------------------------------------------------------------------------------------


