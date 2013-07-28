SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EmailLog](
	[EmailId] [int] IDENTITY(1,1) NOT NULL,
	[UserEmail] [nvarchar](80) NULL,
	[quantity] [int] NOT NULL,
	[Symbol] [nvarchar](10) NOT NULL,
	[timestamp] [datetime] NOT NULL,
	[TradeType] [int] NOT NULL,
	[Approved] [nvarchar](5) NULL,
 CONSTRAINT [PK_dbo.Email] PRIMARY KEY CLUSTERED 
(
	[EmailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO