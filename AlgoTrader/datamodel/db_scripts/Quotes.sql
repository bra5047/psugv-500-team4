/****** Object:  Table [dbo].[Quotes]    Script Date: 6/10/2013 1:16:05 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Quotes](
	[QuoteId] [int] IDENTITY(1,1) NOT NULL,
	[price] [float] NOT NULL,
	[timestamp] [datetime] NOT NULL,
	[SymbolName] [nvarchar](128) NULL,
	[CompanyName] [nvarchar](200) NULL,
 CONSTRAINT [PK_dbo.Quotes] PRIMARY KEY CLUSTERED 
(
	[QuoteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Quotes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Quotes_dbo.Symbols_SymbolName] FOREIGN KEY([SymbolName])
REFERENCES [dbo].[Symbols] ([name])
GO

ALTER TABLE [dbo].[Quotes] CHECK CONSTRAINT [FK_dbo.Quotes_dbo.Symbols_SymbolName]
GO


