SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Areas]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
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
) ON [PRIMARY]
) ON [PRIMARY]
END
