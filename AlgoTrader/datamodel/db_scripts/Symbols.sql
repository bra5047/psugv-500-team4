/****** Object:  Table [dbo].[Symbols]    Script Date: 6/9/2013 11:20:30 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Symbols](
	[name] [nvarchar](128) NOT NULL,
	[CompanyName] [nvarchar](200) NULL,
 CONSTRAINT [PK_dbo.Symbols] PRIMARY KEY CLUSTERED 
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


