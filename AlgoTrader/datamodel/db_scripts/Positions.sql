/****** Object:  Table [dbo].[Positions]    Script Date: 6/10/2013 1:15:33 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Positions](
	[PositionId] [int] IDENTITY(1,1) NOT NULL,
	[SymbolName] [nvarchar](128) NULL,
	[PortfolioId] [int] NULL,
	[price] [float] NOT NULL,
	[quantity] [int] NOT NULL,
	[status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Positions] PRIMARY KEY CLUSTERED 
(
	[PositionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Positions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Positions_dbo.Portfolios_PortfolioId] FOREIGN KEY([PortfolioId])
REFERENCES [dbo].[Portfolios] ([PortfolioId])
GO

ALTER TABLE [dbo].[Positions] CHECK CONSTRAINT [FK_dbo.Positions_dbo.Portfolios_PortfolioId]
GO

ALTER TABLE [dbo].[Positions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Positions_dbo.Symbols_SymbolName] FOREIGN KEY([SymbolName])
REFERENCES [dbo].[Symbols] ([name])
GO

ALTER TABLE [dbo].[Positions] CHECK CONSTRAINT [FK_dbo.Positions_dbo.Symbols_SymbolName]
GO


