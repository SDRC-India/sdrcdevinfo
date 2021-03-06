USE [di7webservices]
GO
/****** Object:  Table [dbo].[ProviderDetails]    Script Date: 01/04/2013 14:20:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProviderDetails](
	[UserNid] [int] NOT NULL,
	[AdaptationNid] [int] NOT NULL,
	[User_Is_Provider] [nvarchar](10) NOT NULL,
	[User_Is_Admin] [nvarchar](10) NULL,
	[Send_Updates] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IndexedIndicators]    Script Date: 01/04/2013 14:20:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IndexedIndicators](
	[Indicator_NId] [int] NOT NULL,
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
/****** Object:  Table [dbo].[IndexedAreas]    Script Date: 01/04/2013 14:20:55 ******/
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
/****** Object:  Table [dbo].[Global_UserLogin]    Script Date: 01/04/2013 14:20:55 ******/
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
	[User_AreaNid] [int] NOT NULL,
	[IsMasterAccount] [nvarchar](10) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Global_UserLogin] ON
INSERT [dbo].[Global_UserLogin] ([NId], [User_Email_Id], [User_Password], [User_First_Name], [User_Last_Name], [User_AreaNid], [IsMasterAccount]) VALUES (1, N'webmaster@xyz.com', N'a3e5bcf0f24122d3335606d4976d89e7f9fa58421d58edf44f76c88902a777b9', N'Support', NULL, 1, N'True')
SET IDENTITY_INSERT [dbo].[Global_UserLogin] OFF
/****** Object:  Table [dbo].[Areas]    Script Date: 01/04/2013 14:20:55 ******/
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
SET IDENTITY_INSERT [dbo].[Areas] ON
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (1, N'Global', -1, N'03M49WLD', 1, NULL)
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (2, N'Morocco', 1, N'MAR', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (3, N'Kenya', 1, N'KEN', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (4, N'Ghana', 1, N'GHA', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (5, N'Djibouti', 1, N'DJI', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (6, N'Mauritania', 1, N'MRT', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (7, N'Réunion', 1, N'REU', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (8, N'Lesotho', 1, N'LSO', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (9, N'Cameroon', 1, N'CMR', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (10, N'Algeria', 1, N'DZA', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (11, N'Comoros', 1, N'COM', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (12, N'Nigeria', 1, N'NGA', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (13, N'Guinea', 1, N'GIN', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (14, N'Libya', 1, N'LBY', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (15, N'Gabon', 1, N'GAB', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (16, N'Congo', 1, N'COG', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (17, N'Seychelles', 1, N'SYC', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (18, N'Gambia', 1, N'GMB', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (19, N'Rwanda', 1, N'RWA', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (20, N'Central African Republic', 1, N'CAF', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (21, N'Liberia', 1, N'LBR', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (22, N'Burkina Faso', 1, N'BFA', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (23, N'Egypt', 1, N'EGY', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (24, N'Botswana', 1, N'BWA', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (25, N'Malawi', 1, N'MWI', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (26, N'Benin', 1, N'BEN', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (27, N'Namibia', 1, N'NAM', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (28, N'Cape Verde', 1, N'CPV', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (29, N'Mozambique', 1, N'MOZ', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (30, N'Saint Helena', 1, N'SHN', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (31, N'Ethiopia', 1, N'ETH', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (32, N'Mauritius', 1, N'MUS', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (33, N'Eritrea', 1, N'ERI', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (34, N'Madagascar', 1, N'MDG', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (35, N'Mayotte', 1, N'MYT', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (36, N'Côte d''Ivoire', 1, N'CIV', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (37, N'Equatorial Guinea', 1, N'GNQ', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (38, N'Sao Tome and Principe', 1, N'STP', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (39, N'Democratic Republic of the Congo', 1, N'COD', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (40, N'Guinea-Bissau', 1, N'GNB', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (41, N'Chad', 1, N'TCD', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (42, N'Angola', 1, N'AGO', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (43, N'Burundi', 1, N'BDI', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (44, N'Senegal', 1, N'SEN', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (45, N'Niger', 1, N'NER', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (46, N'Sierra Leone', 1, N'SLE', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (47, N'Mali', 1, N'MLI', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (48, N'Jordan', 1, N'JOR', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (49, N'Republic of Korea', 1, N'KOR', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (50, N'Myanmar', 1, N'MMR', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (51, N'Singapore', 1, N'SGP', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (52, N'China, Macao Special Administrative Region', 1, N'MAC', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (53, N'Oman', 1, N'OMN', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (54, N'Cambodia', 1, N'KHM', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (55, N'Philippines', 1, N'PHL', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (56, N'Maldives', 1, N'MDV', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (57, N'Malaysia', 1, N'MYS', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (58, N'Qatar', 1, N'QAT', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (59, N'China, Hong Kong Special Administrative Region', 1, N'HKG', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (60, N'Armenia', 1, N'ARM', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (61, N'Azerbaijan', 1, N'AZE', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (62, N'Democratic People''s Republic of Korea', 1, N'PRK', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (63, N'Nepal', 1, N'NPL', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (64, N'India', 1, N'IND', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (65, N'Kuwait', 1, N'KWT', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (66, N'Saudi Arabia', 1, N'SAU', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (67, N'Japan', 1, N'JPN', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (68, N'Lebanon', 1, N'LBN', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (69, N'Occupied Palestinian Territory', 1, N'PSE', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (70, N'Kyrgyzstan', 1, N'KGZ', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (71, N'Cyprus', 1, N'CYP', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (72, N'Brunei Darussalam', 1, N'BRN', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (73, N'Bahrain', 1, N'BHR', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (74, N'Kazakhstan', 1, N'KAZ', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (75, N'Pakistan', 1, N'PAK', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (76, N'Afghanistan', 1, N'AFG', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (77, N'Iran (Islamic Republic of)', 1, N'IRN', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (78, N'China', 1, N'CHN', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (79, N'Iraq', 1, N'IRQ', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (80, N'Mongolia', 1, N'MNG', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (81, N'Bhutan', 1, N'BTN', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (82, N'Georgia', 1, N'GEO', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (83, N'Israel', 1, N'ISR', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (84, N'Indonesia', 1, N'IDN', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (85, N'Lao People''s Democratic Republic', 1, N'LAO', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (86, N'Bangladesh', 1, N'BGD', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (87, N'Italy', 1, N'ITA', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (88, N'Andorra', 1, N'AND', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (89, N'Norway', 1, N'NOR', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (90, N'Montenegro', 1, N'MNE', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (91, N'San Marino', 1, N'SMR', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (92, N'Luxembourg', 1, N'LUX', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (93, N'France', 1, N'FRA', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (94, N'Romania', 1, N'ROU', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (95, N'Hungary', 1, N'HUN', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (96, N'Malta', 1, N'MLT', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (97, N'Guernsey', 1, N'GGY', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (98, N'Germany', 1, N'DEU', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (99, N'Slovenia', 1, N'SVN', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (100, N'Holy See', 1, N'VAT', 2, N'')
GO
print 'Processed 100 total records'
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (101, N'Ireland', 1, N'IRL', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (102, N'Slovakia', 1, N'SVK', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (103, N'Czech Republic', 1, N'CZE', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (104, N'Åland Islands', 1, N'ALA', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (105, N'Denmark', 1, N'DNK', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (106, N'Gibraltar', 1, N'GIB', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (107, N'Croatia', 1, N'HRV', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (108, N'Isle of Man', 1, N'IMN', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (109, N'Latvia', 1, N'LVA', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (110, N'Netherlands', 1, N'NLD', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (111, N'Poland', 1, N'POL', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (112, N'Liechtenstein', 1, N'LIE', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (113, N'Belarus', 1, N'BLR', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (114, N'Russian Federation', 1, N'RUS', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (115, N'Austria', 1, N'AUT', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (116, N'Channel Islands', 1, N'830', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (117, N'Estonia', 1, N'EST', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (118, N'Belgium', 1, N'BEL', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (119, N'Bosnia and Herzegovina', 1, N'BIH', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (120, N'Jersey', 1, N'JEY', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (121, N'Faeroe Islands', 1, N'FRO', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (122, N'Republic of Moldova', 1, N'MDA', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (123, N'Sark', 1, N'680', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (124, N'Albania', 1, N'ALB', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (125, N'Iceland', 1, N'ISL', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (126, N'Serbia', 1, N'SRB', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (127, N'Greece', 1, N'GRC', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (128, N'Finland', 1, N'FIN', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (129, N'Monaco', 1, N'MCO', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (130, N'Lithuania', 1, N'LTU', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (131, N'Bulgaria', 1, N'BGR', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (132, N'Portugal', 1, N'PRT', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (133, N'Canada', 1, N'CAN', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (134, N'Bermuda', 1, N'BMU', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (135, N'Saint Pierre and Miquelon', 1, N'SPM', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (136, N'Greenland', 1, N'GRL', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (137, N'Samoa', 1, N'WSM', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (138, N'Norfolk Island', 1, N'NFK', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (139, N'Marshall Islands', 1, N'MHL', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (140, N'Solomon Islands', 1, N'SLB', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (141, N'Fiji', 1, N'FJI', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (142, N'Niue', 1, N'NIU', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (143, N'Kiribati', 1, N'KIR', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (144, N'Cook Islands', 1, N'COK', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (145, N'French Polynesia', 1, N'PYF', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (146, N'New Caledonia', 1, N'NCL', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (147, N'Palau', 1, N'PLW', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (148, N'Papua New Guinea', 1, N'PNG', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (149, N'Guam', 1, N'GUM', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (150, N'Northern Mariana Islands', 1, N'MNP', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (151, N'Nauru', 1, N'NRU', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (152, N'Pitcairn', 1, N'PCN', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (153, N'American Samoa', 1, N'ASM', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (154, N'New Zealand', 1, N'NZL', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (155, N'Micronesia (Federated States of)', 1, N'FSM', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (156, N'Australia', 1, N'AUS', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (157, N'Puerto Rico', 1, N'PRI', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (158, N'Argentina', 1, N'ARG', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (159, N'Haiti', 1, N'HTI', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (160, N'Guyana', 1, N'GUY', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (161, N'Brazil', 1, N'BRA', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (162, N'Panama', 1, N'PAN', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (163, N'Aruba', 1, N'ABW', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (164, N'Montserrat', 1, N'MSR', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (165, N'Nicaragua', 1, N'NIC', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (166, N'El Salvador', 1, N'SLV', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (167, N'Belize', 1, N'BLZ', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (168, N'Saint Vincent and the Grenadines', 1, N'VCT', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (169, N'Ecuador', 1, N'ECU', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (170, N'Sint Maarten (Dutch part)', 1, N'SXM', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (171, N'Guatemala', 1, N'GTM', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (172, N'Paraguay', 1, N'PRY', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (173, N'Chile', 1, N'CHL', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (174, N'Saint-Martin (French part)', 1, N'MAF', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (175, N'Bahamas', 1, N'BHS', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (176, N'Cuba', 1, N'CUB', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (177, N'Falkland Islands (Malvinas)', 1, N'FLK', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (178, N'Dominica', 1, N'DMA', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (179, N'Honduras', 1, N'HND', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (180, N'Anguilla', 1, N'AIA', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (181, N'Saint-Barthélemy', 1, N'BLM', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (182, N'Jamaica', 1, N'JAM', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (183, N'Curaçao', 1, N'CUW', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (184, N'French Guiana', 1, N'GUF', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (185, N'Dominican Republic', 1, N'DOM', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (186, N'Costa Rica', 1, N'CRI', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (187, N'Saint Lucia', 1, N'LCA', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (188, N'Bolivia (Plurinational State of)', 1, N'BOL', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (189, N'Colombia', 1, N'COL', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (190, N'Grenada', 1, N'GRD', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (191, N'Martinique', 1, N'MTQ', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (192, N'Peru', 1, N'PER', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (193, N'British Virgin Islands', 1, N'VGB', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (194, N'Saint Kitts and Nevis', 1, N'KNA', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (195, N'Mexico', 1, N'MEX', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (196, N'Bonaire, Saint Eustatius and Saba', 1, N'BES', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (197, N'Guadeloupe', 1, N'GLP', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (198, N'Cayman Islands', 1, N'CYM', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (199, N'Antigua and Barbuda', 1, N'ATG', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (200, N'Barbados', 1, N'BRB', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (201, N'United Republic of Tanzania', 1, N'TZA', 2, N'')
GO
print 'Processed 200 total records'
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (202, N'Tunisia', 1, N'TUN', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (203, N'Uganda', 1, N'UGA', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (204, N'South Africa', 1, N'ZAF', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (205, N'Sudan', 1, N'SDN', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (206, N'Zimbabwe', 1, N'ZWE', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (207, N'Somalia', 1, N'SOM', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (208, N'Zambia', 1, N'ZMB', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (209, N'Swaziland', 1, N'SWZ', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (210, N'Togo', 1, N'TGO', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (211, N'Western Sahara', 1, N'ESH', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (212, N'South Sudan', 1, N'SSD', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (213, N'Viet Nam', 1, N'VNM', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (214, N'Turkey', 1, N'TUR', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (215, N'United Arab Emirates', 1, N'ARE', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (216, N'Tajikistan', 1, N'TJK', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (217, N'Syrian Arab Republic', 1, N'SYR', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (218, N'Sri Lanka', 1, N'LKA', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (219, N'Timor-Leste', 1, N'TLS', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (220, N'Turkmenistan', 1, N'TKM', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (221, N'Yemen', 1, N'YEM', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (222, N'Thailand', 1, N'THA', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (223, N'Uzbekistan', 1, N'UZB', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (224, N'Ukraine', 1, N'UKR', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (225, N'The former Yugoslav Republic of Macedonia', 1, N'MKD', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (226, N'Svalbard and Jan Mayen Islands', 1, N'SJM', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (227, N'Switzerland', 1, N'CHE', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (228, N'Spain', 1, N'ESP', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (229, N'Sweden', 1, N'SWE', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (230, N'United Kingdom', 1, N'GBR', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (231, N'United States', 1, N'USA', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (232, N'Tonga', 1, N'TON', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (233, N'Tokelau', 1, N'TKL', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (234, N'Vanuatu', 1, N'VUT', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (235, N'Tuvalu', 1, N'TUV', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (236, N'Wallis and Futuna Islands', 1, N'WLF', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (237, N'United States Virgin Islands', 1, N'VIR', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (238, N'Suriname', 1, N'SUR', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (239, N'Turks and Caicos Islands', 1, N'TCA', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (240, N'Uruguay', 1, N'URY', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (241, N'Trinidad and Tobago', 1, N'TTO', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (242, N'Venezuela (Bolivarian Republic of)', 1, N'VEN', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (243, N'Regional', 1, N'REG', 2, N'')


INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (244, N'CEE/CIS', 1, N'f1b7de3d-f9d9-489b-8cdc-bea05452e549', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (245, N'East Asia and Pacific', 1, N'9e2a841a-cc1a-4536-a9de-910d419a64d0', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (246, N'Eastern and Southern Africa', 1, N'd4b3e21a-7a8a-4ea9-89f8-df6cf3ab888b', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (247, N'Industrialized Countries/Territories', 1, N'f149eeec-f6bf-46e0-a5a7-b6d793afc72c ', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (248, N'Latin America and Caribbean', 1, N'c4321b1f-d7bf-4403-b782-84ae47eef96f', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (249, N'Middle East and North Africa', 1, N'00a65094-d9d4-48bf-a8df-77f7437da1f7', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (250, N'South Asia', 1, N'19a7fed1-ad86-46fb-9706-ae33f48d54da', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (251, N'Western and Central Africa', 1, N'ab243a9d-309a-4f75-8b5c-1eb52e70fa1c', 2, N'')
INSERT [dbo].[Areas] ([AreaNId], [AreaName], [ParentNId], [AreaID], [AreaLevel], [Block]) VALUES (252, N'World', 1, N'be3238e4-d92c-4242-a06d-4f26ac4b5be8', 2, N'')

SET IDENTITY_INSERT [dbo].[Areas] OFF
GO
print 'Processed 200 total records'
/****** Object:  Table [dbo].[AdaptationVersion]    Script Date: 01/04/2013 14:20:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdaptationVersion](
	[Ver_NId] [int] IDENTITY(1,1) NOT NULL,
	[Ver_Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_AdaptationVersion] PRIMARY KEY CLUSTERED 
(
	[Ver_NId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[AdaptationVersion] ON
INSERT [dbo].[AdaptationVersion] ([Ver_NId], [Ver_Name]) VALUES (1, N'Child Info')
INSERT [dbo].[AdaptationVersion] ([Ver_NId], [Ver_Name]) VALUES (2, N'DevInfo 4')
INSERT [dbo].[AdaptationVersion] ([Ver_NId], [Ver_Name]) VALUES (3, N'DevInfo 5')
INSERT [dbo].[AdaptationVersion] ([Ver_NId], [Ver_Name]) VALUES (4, N'DevInfo 6')
INSERT [dbo].[AdaptationVersion] ([Ver_NId], [Ver_Name]) VALUES (5, N'DevInfo 7')
SET IDENTITY_INSERT [dbo].[AdaptationVersion] OFF
/****** Object:  Table [dbo].[Adaptations]    Script Date: 01/04/2013 14:20:55 ******/
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
	[Db_Adm_Name] [nvarchar](4000) NULL,
	[Db_Adm_Institution] [nvarchar](4000) NULL,
	[Db_Adm_Email] [nvarchar](4000) NULL,
	[Unicef_Region] [nvarchar](4000) NULL,
	[Adaptation_Year] [nvarchar](4) NULL,
	[Db_Languages] [nvarchar](4000) NULL,
	[IS_DI7_ORG_SITE] [bit] NULL,
	[LangCode_CSVFiles] [nvarchar](3000) NULL,
	[Adapted_For] [nvarchar](100) NULL,
	[Country] [nvarchar](100) NULL,
	[GUID] [nvarchar](50) NULL,
	[Date_Created] [nvarchar](100) NULL,
	[Visible_In_Catalog] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_Adaptations] PRIMARY KEY CLUSTERED 
(
	[NId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TokenInformation]    Script Date: 01/04/2013 14:20:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TokenInformation](
	[User_Nid] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[TokenKey] [nvarchar](100) NOT NULL,
	[IsRegistration] [nvarchar](10) NOT NULL,
	[IsDataviewFlow] [nvarchar](10) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[Split]    Script Date: 01/04/2013 14:20:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create FUNCTION [dbo].[Split](@String varchar(4000), @Delimiter char(1))       
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
/****** Object:  StoredProcedure [dbo].[dw_UpdateOnlineUsersStatus]    Script Date: 01/04/2013 14:20:57 ******/
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
/****** Object:  StoredProcedure [dbo].[dw_OnlineUsers]    Script Date: 01/04/2013 14:20:57 ******/
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
/****** Object:  StoredProcedure [dbo].[dw_Adaptations]    Script Date: 01/04/2013 14:20:57 ******/
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
/****** Object:  StoredProcedure [dbo].[dw_Activations]    Script Date: 01/04/2013 14:20:57 ******/
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
/****** Object:  StoredProcedure [dbo].[Update_Password]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[Update_Password]   
@User_NId INT,
@Password VARCHAR(200)
AS          
          
BEGIN         
          
UPDATE [Global_UserLogin]
SET [User_Password] = @Password
WHERE [NId] = @User_NId
         
END
GO
/****** Object:  StoredProcedure [dbo].[Update_GlobalUsers]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[Update_GlobalUsers]   
@User_First_Name VARCHAR(200),       
@User_IsProvider NVARCHAR(10),
@User_IsAdmin NVARCHAR(10),
@User_NId INT,
@Adaptation_NId INT
AS          
          
BEGIN         
          
UPDATE ProviderDetails
SET [User_Is_Provider] = @User_IsProvider,
[User_Is_Admin] = @User_IsAdmin
WHERE [AdaptationNid] = @Adaptation_NId
AND [UserNid] = @User_NId


UPDATE Global_UserLogin
SET [User_First_Name] = @User_First_Name
WHERE NId = @User_NId
         
END
GO
/****** Object:  StoredProcedure [dbo].[Update_Database_Metadata_Description]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Update_Database_Metadata_Description]   
	@AdaptationURL as NVarchar(4000),
	@Description as NVARCHAR(200)
AS
BEGIN
	UPDATE [dbo].[Adaptations]
SET [Description] = @Description
WHERE [Online_URL] = @AdaptationURL
END
GO
/****** Object:  StoredProcedure [dbo].[Delete_GlobalUser]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[Delete_GlobalUser]          
@User_NId INT,
@Adaptation_NId INT
AS          
          
BEGIN         
          
DELETE FROM  ProviderDetails
WHERE [AdaptationNid] = @Adaptation_NId
AND [UserNid] = @User_NId

DELETE FROM Global_UserLogin
WHERE [NId] = @User_NId
         
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UPDATE_LangCode_CSVFiles_CATALOG]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_UPDATE_LangCode_CSVFiles_CATALOG]	
	@AdaptationGUID NVARCHAR(4000),
	@LangCode_CSVFiles NVARCHAR(4000)
AS
BEGIN
	UPDATE ADAPTATIONS SET
		LangCode_CSVFiles = @LangCode_CSVFiles
		WHERE [GUID] = @AdaptationGUID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UPDATE_CATALOG_INFO]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_UPDATE_CATALOG_INFO]	
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
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UPDATE_CATALOG]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[SP_UPDATE_CATALOG]
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
	@THUMBNAIL_IMAGE_URL NVARCHAR(4000),
	@DB_ADM_NAME NVARCHAR(4000),
	@DB_ADM_INSTITUTION NVARCHAR(4000),
	@DB_ADM_EMAIL NVARCHAR(4000),
	@UNICEF_REGION NVARCHAR(4000),
	@ADAPTATION_YEAR NVARCHAR(4),
	@DB_LANGUAGES NVARCHAR(4000)
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
		THUMBNAIL_IMAGE_URL = @THUMBNAIL_IMAGE_URL,
		DB_ADM_NAME = @DB_ADM_NAME,
		DB_ADM_INSTITUTION = @DB_ADM_INSTITUTION,
		DB_ADM_EMAIL = @DB_ADM_EMAIL,
		UNICEF_REGION = @UNICEF_REGION,
		ADAPTATION_YEAR = @ADAPTATION_YEAR,
		DB_LANGUAGES = @DB_LANGUAGES
		WHERE NID = @ADAPTATION_NID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_INSERT_CATALOG]    Script Date: 01/04/2013 14:20:57 ******/
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
	IF(@DI_VERSION = '-1')
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
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_MATCHED_INDEXED_INDICATORS]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
SELECT ITEMS FROM DBO.SPLIT(@SEARCH_INDICATORS, ',')


SET @COUNT_SEARCHED_I = (SELECT COUNT(*) FROM #SEARCHED_INDICATOR)

IF(@COUNT_SEARCHED_I <> 0)
	BEGIN

		WHILE(@I < @COUNT_SEARCHED_I)
		BEGIN

			SET @TMP_INDICATOR_NAME = (SELECT TOP 1 INDICATOR FROM #SEARCHED_INDICATOR WHERE ID = @I)
			
			IF(@LANGUAGE_CODE = 'ar')
				BEGIN
					INSERT INTO #RESULT_INDICATORS(INDICATOR_NID, INDICATOR_NAME, ADPT_NID)
					SELECT DISTINCT INDICATOR_NID, replace(INDICATOR_NAME,'"', '\"'), ADPT_NID FROM DBO.INDEXEDINDICATORS
					WHERE SOUNDEX(INDICATOR_NAME) LIKE '%' + SOUNDEX(LTRIM(RTRIM(@TMP_INDICATOR_NAME))) + '%'
					AND LANGUAGE_CODE = @LANGUAGE_CODE					
				END			
			ELSE
				BEGIN
					INSERT INTO #RESULT_INDICATORS(INDICATOR_NID, INDICATOR_NAME, ADPT_NID)
					SELECT DISTINCT INDICATOR_NID, replace(INDICATOR_NAME,'"', '\"'), ADPT_NID FROM DBO.INDEXEDINDICATORS
					WHERE INDICATOR_NAME LIKE '%' + LTRIM(RTRIM(@TMP_INDICATOR_NAME)) + '%'
					AND LANGUAGE_CODE = @LANGUAGE_CODE					
				END

			SET @I = @I + 1
		END

	END


SELECT * FROM #RESULT_INDICATORS

END
GO
/****** Object:  StoredProcedure [dbo].[sp_get_matched_indexed_areas]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_get_matched_indexed_areas]
@SEARCH_AREAS NVARCHAR(4000),
@LANGUAGE_CODE NVARCHAR(2)
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

			IF(@LANGUAGE_CODE = 'ar')
				BEGIN
					INSERT INTO #RESULT_AREAS(AREA_NID, AREA_PARENT_NID,AREA_NAME,ADPT_NID)
					SELECT DISTINCT AREA_NID, AREA_PARENT_NID, AREA_NAME, ADPT_NID FROM DBO.INDEXEDAREAS
					WHERE SOUNDEX(AREA_NAME) = SOUNDEX(LTRIM(RTRIM(@TMP_AREA_NAME)))
					AND LANGUAGE_CODE = @LANGUAGE_CODE			
				END			
			ELSE
				BEGIN
					INSERT INTO #RESULT_AREAS(AREA_NID, AREA_PARENT_NID,AREA_NAME,ADPT_NID)
					SELECT DISTINCT AREA_NID, AREA_PARENT_NID, AREA_NAME, ADPT_NID FROM DBO.INDEXEDAREAS
					WHERE AREA_NAME LIKE '%' + LTRIM(RTRIM(@TMP_AREA_NAME)) + '%'
					AND LANGUAGE_CODE = @LANGUAGE_CODE				
				END

			SET @I = @I + 1
		END

	END


select * from #RESULT_AREAS

end
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_MASTER_ACCOUNT]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_GET_MASTER_ACCOUNT]

AS
BEGIN 
	SELECT NId FROM Global_UserLogin where IsMasterAccount='True'
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_M49_COUNTRIES]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[SP_GET_M49_COUNTRIES]
AS
BEGIN
	SELECT AREANID, AREANAME, AREAID FROM AREAS WHERE AREALEVEL = 2
	ORDER BY AREANAME
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_DATE_CREATED]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_GET_DATE_CREATED]
	@AdaptationURL NVARCHAR(4000)
AS
BEGIN
	SELECT Date_Created FROM ADAPTATIONS WHERE [GUID] = @AdaptationURL
END
GO
/****** Object:  StoredProcedure [dbo].[sp_get_catalog_cache_results_XX]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[sp_get_catalog_cache_results_XX]
(
@AREAS NVARCHAR(MAX),
@INDICATORS NVARCHAR(MAX)
)
AS
BEGIN

	DECLARE @I INT
	DECLARE @MAX INT

	SET @I = 0

	CREATE TABLE #SEARCHED_AREA_INDICATOR_COMBINATIONS
	(
		ID INT IDENTITY(0,1),
		AREA NVARCHAR(MAX),
		INDICATOR NVARCHAR(MAX)
	)
	CREATE TABLE #CACHE_AREA_INDICATOR_NIDS
	(
		ID INT IDENTITY(0,1),
		AREA_NID NVARCHAR(MAX),
		INDICATOR_NID NVARCHAR(MAX)
	)

	INSERT INTO #SEARCHED_AREA_INDICATOR_COMBINATIONS(AREA, INDICATOR)
	SELECT A.ITEMS AS 'AREA', I.ITEMS AS 'INDICATORS'
	FROM DBO.SPLIT(@AREAS, ' ') A, DBO.SPLIT(@INDICATORS, ' ') I
			
	SET @MAX = (SELECT COUNT(*) FROM #SEARCHED_AREA_INDICATOR_COMBINATIONS)

	WHILE(@I < @MAX)
	BEGIN
		
		DECLARE @TMP_AREA NVARCHAR(MAX)
		DECLARE @TMP_INDICATOR NVARCHAR(MAX)

		SET @TMP_AREA = (SELECT TOP 1 AREA FROM #SEARCHED_AREA_INDICATOR_COMBINATIONS WHERE ID = @I)
		SET @TMP_INDICATOR = (SELECT TOP 1 INDICATOR FROM #SEARCHED_AREA_INDICATOR_COMBINATIONS WHERE ID = @I)

		INSERT INTO #CACHE_AREA_INDICATOR_NIDS(AREA_NID, INDICATOR_NID)
		SELECT A.AREA_NID, I.INDICATOR_NID
		FROM DBO.UT_AREA_XX A, DBO.UT_INDICATOR_XX I
		WHERE A.AREA_NAME LIKE '%' + @TMP_AREA + '%'
		AND I.INDICATOR_NAME LIKE '%' + @TMP_INDICATOR + '%'
		
		SET @I = @I + 1
	END


	SELECT NID,	
	SEARCHLANGUAGE,
	C.INDICATORNID,
	UNITNID,
	C.AREANID,
	ISAREANUMERIC,
	INDICATOR,
	UNIT,
	AREA,
	DEFAULTSG,
	MRDTP,
	MRD,
	AREACOUNT,
	SGCOUNT,
	SOURCECOUNT,
	TPCOUNT,
	DVCOUNT,
	AREANIDS,
	SGNIDS,
	SOURCENIDS,
	TPNIDS,
	DVSERIES
	FROM DBO.DI_SEARCH_RESULTS C, -- C = CACHE TABLE
	#CACHE_AREA_INDICATOR_NIDS IA 
	WHERE C.INDICATORNID = IA.INDICATOR_NID
	AND C.AREANID = IA.AREA_NID
	ORDER BY C.AREANID DESC
	

	DROP TABLE #SEARCHED_AREA_INDICATOR_COMBINATIONS
	DROP TABLE #CACHE_AREA_INDICATOR_NIDS

END
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_CATALOG_CACHE_RESULTS_EN]    Script Date: 01/04/2013 14:20:57 ******/
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
/****** Object:  StoredProcedure [dbo].[SP_GET_CATALOG]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec SP_GET_CATALOG 'http://localhost:50158/di web'
CREATE PROC [dbo].[SP_GET_CATALOG]
	@AdaptationGUID NVARCHAR(4000)
AS
BEGIN

	DECLARE @ADPT_AREA_NID INT	

	SET @ADPT_AREA_NID = (SELECT AREA_NID FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID)

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
			DI_VERSION = VER_NID AND  Visible_In_Catalog = 'True'  ORDER BY Name ASC

		END
	ELSE
		BEGIN
			SELECT 
			NId,Name,Description,Ver_Name [DI_Version],Is_Desktop,Is_Online,Online_URL,Area_Count,IUS_Count,
			Time_Periods_Count,Data_Values_Count,Start_Year,End_Year,Last_Modified,Area_NId,Sub_Nation,
			Thumbnail_Image_URL,Db_Adm_Name,Db_Adm_Institution,Db_Adm_Email,Unicef_Region,Adaptation_Year,Db_Languages,LangCode_CSVFiles
			FROM ADAPTATIONS, ADAPTATIONVERSION
			WHERE (AREA_NID IN(@ADPT_AREA_NID)) AND
			DI_VERSION = VER_NID  AND Visible_In_Catalog = 'True'  ORDER BY Name ASC		
		END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_ADAPTATION_VERSIONS]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_GET_ADAPTATION_VERSIONS]
AS
BEGIN
	SELECT * FROM DBO.ADAPTATIONVERSION
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GET_ADAPTATION_NID_AND_DELETE_INDEXED]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_GET_ADAPTATION_NID_AND_DELETE_INDEXED]
	@AdaptationGUID NVARCHAR(4000),
	@TABLE_TYPE NVARCHAR(20)
AS
BEGIN
	DECLARE @ADPT_NID INT

	SET @ADPT_NID = (SELECT NID FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID)
	
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
/****** Object:  StoredProcedure [dbo].[SP_GET_ADAPTATION_AREAS]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_GET_ADAPTATION_AREAS]
	@AdaptationGUID NVARCHAR(4000)
AS
BEGIN 
	DECLARE @ADPT_AREA_NID INT

	SET @ADPT_AREA_NID = (SELECT AREA_NID FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID)

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
/****** Object:  StoredProcedure [dbo].[SP_DELETE_CATALOG]    Script Date: 01/04/2013 14:20:57 ******/
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
/****** Object:  StoredProcedure [dbo].[SP_CATALOG_EXISTS]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_CATALOG_EXISTS]
	@AdaptationGUID NVARCHAR(4000)
AS
BEGIN
	SELECT NId,Date_Created,Visible_in_Catalog FROM ADAPTATIONS WHERE [GUID] = @AdaptationGUID
END
GO
/****** Object:  StoredProcedure [dbo].[Set_User_AsAdmin]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[Set_User_AsAdmin]   
@User_NId INT,
@AdaptationGUID as NVarchar(4000)      
AS          
          
BEGIN 

BEGIN TRY

BEGIN TRANSACTION

UPDATE ProviderDetails
SET [User_Is_Provider] = 'False',
[User_Is_Admin] = 'False'
WHERE [AdaptationNid] in (SELECT NId FROM Adaptations where [GUID]=@AdaptationGUID)
AND [User_Is_Admin] = 'True'
         
UPDATE ProviderDetails
SET [User_Is_Provider] = 'True',
[User_Is_Admin] = 'True'
WHERE [AdaptationNid] in (SELECT NId FROM Adaptations where [GUID]=@AdaptationGUID)
AND [UserNid] = @User_NId
COMMIT

END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK     
END CATCH      

END
GO
/****** Object:  StoredProcedure [dbo].[GetMasterSiteURL]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetMasterSiteURL]	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT online_url,is_di7_org_site from adaptations
END
GO
/****** Object:  StoredProcedure [dbo].[Get_Users_ByAdaptationURL]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[Get_Users_ByAdaptationURL]   
@AdaptationGUID as NVarchar(4000)        
AS          
          
BEGIN         


SELECT U.[NId],U.[User_First_Name] AS "UserName", U.[User_Email_Id] AS "EmailId", P.[User_Is_Provider] AS "Is Provider" ,P.[User_Is_Admin] AS "Is Admin" 
FROM ProviderDetails P
JOIN Global_UserLogin U
ON P.[UserNid] = U.[NId]
JOIN Adaptations A
ON P.[AdaptationNid] = A.[NId]
WHERE A.[GUID] = @AdaptationGUID AND U.[IsMasterAccount] <> 'True'
--ORDER BY U.[NId], A.[NId]
ORDER BY P.[User_Is_Admin] DESC
          
END
GO
/****** Object:  StoredProcedure [dbo].[Get_GlobalUsers]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[Get_GlobalUsers]          
@AdaptationNId INT
AS          
          
BEGIN         

IF(@AdaptationNId = 0)
BEGIN     

SELECT U.[NId],U.[User_First_Name] AS "UserName", U.[User_Email_Id] AS "EmailId",A.[Name] AS "Adaptation" ,A.[Online_URL] AS "Adaptation URL", P.[User_Is_Provider] AS "Is Provider" ,P.[User_Is_Admin] AS "Is Admin" 
FROM [ProviderDetails] P
JOIN [Global_UserLogin] U
ON P.[UserNid] = U.[NId]
JOIN [Adaptations] A
ON P.[AdaptationNid] = A.[NId]
ORDER BY U.[NId], A.[NId]
END

ELSE
BEGIN
SELECT U.[NId],U.[User_First_Name] AS "UserName", U.[User_Email_Id] AS "EmailId",A.[Name] AS "Adaptation" ,A.[Online_URL] AS "Adaptation URL", P.[User_Is_Provider] AS "Is Provider" ,P.[User_Is_Admin] AS "Is Admin" 
FROM [ProviderDetails] P
JOIN [Global_UserLogin] U
ON P.[UserNid] = U.[NId]
JOIN [Adaptations] A
ON P.[AdaptationNid] = A.[NId]
WHERE A.[NId] = @AdaptationNId
ORDER BY U.[NId], A.[NId]
END
          
END
GO
/****** Object:  StoredProcedure [dbo].[Get_Global_Users]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[Get_Global_Users]          
AS          
          
BEGIN         
SELECT [NId],[User_Email_Id]
FROM Global_UserLogin
          
END
GO
/****** Object:  StoredProcedure [dbo].[Get_AreaFromAreaNId]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[Get_AreaFromAreaNId]          
@AreaNId INT
AS          
          
BEGIN         

SELECT [AreaName]
FROM [Areas] 
WHERE [AreaNId] = @AreaNId


          
END
GO
/****** Object:  StoredProcedure [dbo].[Get_AreaForUser]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[Get_AreaForUser]          
@UserNId INT
AS          
          
BEGIN         

SELECT A.[AreaName]
FROM [Areas] A
JOIN [Global_UserLogin] U
ON A.[AreaNId] = U.[User_AreaNid]
WHERE U.[NId] = @UserNId


          
END
GO
/****** Object:  StoredProcedure [dbo].[Get_AdaptationName]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[Get_AdaptationName]          
AS          
          
BEGIN         
          
SELECT [NId],[Name] FROM [dbo].[Adaptations]
--FROM [Adaptations] 
          
END
GO
/****** Object:  Default [DF_ProviderDetails_Send_Updates]    Script Date: 01/04/2013 14:20:55 ******/
ALTER TABLE [dbo].[ProviderDetails] ADD  CONSTRAINT [DF_ProviderDetails_Send_Updates]  DEFAULT (N'False') FOR [Send_Updates]
GO
/****** Object:  Default [DF_Global_UserLogin_IsMasterAccount]    Script Date: 01/04/2013 14:20:55 ******/
ALTER TABLE [dbo].[Global_UserLogin] ADD  CONSTRAINT [DF_Global_UserLogin_IsMasterAccount]  DEFAULT ('False') FOR [IsMasterAccount]
GO
/****** Object:  Default [DF_Adaptations_Visible_In_Catalog]    Script Date: 01/04/2013 14:20:55 ******/
ALTER TABLE [dbo].[Adaptations] ADD  CONSTRAINT [DF_Adaptations_Visible_In_Catalog]  DEFAULT ('False') FOR [Visible_In_Catalog]
GO
/****** Object:  Default [DF_TokenInformation_IsDataviewFlow]    Script Date: 01/04/2013 14:20:55 ******/
ALTER TABLE [dbo].[TokenInformation] ADD  CONSTRAINT [DF_TokenInformation_IsDataviewFlow]  DEFAULT (N'false') FOR [IsDataviewFlow]
GO
/diworldwide/Utility.asmx/GenerateKML?KMLType='+ @KMLType;  
    Exec sp_OACreate 'MSXML2.XMLHTTP', @Object OUT;   
    Exec sp_OAMethod @Object, 'open', NULL, 'get', @Url, 'false'   
    Exec sp_OAMethod @Object, 'send'       
    Exec sp_OADestroy @Object  
End  
  
End
GO
/****** Object:  StoredProcedure [dbo].[dw_OnlineUsers]    Script Date: 01/04/2013 14:20:57 ******/
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
/****** Object:  StoredProcedure [dbo].[dw_Adaptations]    Script Date: 01/04/2013 14:20:57 ******/
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
/****** Object:  StoredProcedure [dbo].[dw_Activations]    Script Date: 01/04/2013 14:20:57 ******/
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
/****** Object:  StoredProcedure [dbo].[Update_Password]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[Update_Password]   
@User_NId INT,
@Password VARCHAR(200)
AS          
          
BEGIN         
          
UPDATE [Global_UserLogin]
SET [User_Password] = @Password
WHERE [NId] = @User_NId
         
END
GO
/****** Object:  StoredProcedure [dbo].[Update_GlobalUsers]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[Update_GlobalUsers]   
@User_First_Name VARCHAR(200),       
@User_IsProvider NVARCHAR(10),
@User_IsAdmin NVARCHAR(10),
@User_NId INT,
@Adaptation_NId INT
AS          
          
BEGIN         
          
UPDATE ProviderDetails
SET [User_Is_Provider] = @User_IsProvider,
[User_Is_Admin] = @User_IsAdmin
WHERE [AdaptationNid] = @Adaptation_NId
AND [UserNid] = @User_NId


UPDATE Global_UserLogin
SET [User_First_Name] = @User_First_Name
WHERE NId = @User_NId
         
END
GO
/****** Object:  StoredProcedure [dbo].[Update_Database_Metadata_Description]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Update_Database_Metadata_Description]   
	@AdaptationURL as NVarchar(4000),
	@Description as NVARCHAR(200)
AS
BEGIN
	UPDATE [dbo].[Adaptations]
SET [Description] = @Description
WHERE [Online_URL] = @AdaptationURL
END
GO
/****** Object:  StoredProcedure [dbo].[Delete_GlobalUser]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[Delete_GlobalUser]          
@User_NId INT,
@Adaptation_NId INT
AS          
          
BEGIN         
          
DELETE FROM  ProviderDetails
WHERE [AdaptationNid] = @Adaptation_NId
AND [UserNid] = @User_NId

DELETE FROM Global_UserLogin
WHERE [NId] = @User_NId
         
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UPDATE_LangCode_CSVFiles_CATALOG]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_UPDATE_LangCode_CSVFiles_CATALOG]	
	@AdaptationGUID NVARCHAR(4000),
	@LangCode_CSVFiles NVARCHAR(4000)
AS
BEGIN
	UPDATE ADAPTATIONS SET
		LangCode_CSVFiles = @LangCode_CSVFiles
		WHERE [GUID] = @AdaptationGUID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UPDATE_CATALOG_INFO]    Script Date: 01/04/2013 14:20:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_UPDATE_CATALOG_INFO]	
	@NAME NVARC