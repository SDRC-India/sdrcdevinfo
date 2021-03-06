SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Adaptations]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
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
) ON [PRIMARY]
) ON [PRIMARY]
END
