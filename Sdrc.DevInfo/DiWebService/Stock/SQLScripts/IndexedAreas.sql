SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[IndexedAreas]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
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
END
