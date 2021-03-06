SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[IndexedIndicators]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
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
END
