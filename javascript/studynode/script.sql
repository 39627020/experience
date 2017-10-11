USE [MF]
GO
/****** Object:  Table [dbo].[Award]    Script Date: 2017/4/22 14:38:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Award](
	[awardID] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](128) NULL,
	[type] [tinyint] NULL,
	[isAuto] [bit] NULL,
	[propID] [int] NULL,
	[volume] [int] NULL,
	[images] [nvarchar](200) NULL,
	[smallImg] [nvarchar](200) NULL,
	[address_img] [nvarchar](200) NULL,
 CONSTRAINT [PK_MAward] PRIMARY KEY NONCLUSTERED 
(
	[awardID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
