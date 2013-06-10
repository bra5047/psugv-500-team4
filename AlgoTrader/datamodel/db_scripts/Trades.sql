/****** Object:  Table [dbo].[Trades]    Script Date: 6/10/2013 1:16:37 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Trades](
	[TradeId] [int] IDENTITY(1,1) NOT NULL,
	[quantity] [int] NOT NULL,
	[price] [float] NOT NULL,
	[timestamp] [datetime] NOT NULL,
	[type] [int] NOT NULL,
	[SymbolName] [nvarchar](128) NULL,
	[PositionId] [int] NULL,
 CONSTRAINT [PK_dbo.Trades] PRIMARY KEY CLUSTERED 
(
	[TradeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Trades]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Trades_dbo.Positions_PositionId] FOREIGN KEY([PositionId])
REFERENCES [dbo].[Positions] ([PositionId])
GO

ALTER TABLE [dbo].[Trades] CHECK CONSTRAINT [FK_dbo.Trades_dbo.Positions_PositionId]
GO

ALTER TABLE [dbo].[Trades]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Trades_dbo.Symbols_SymbolName] FOREIGN KEY([SymbolName])
REFERENCES [dbo].[Symbols] ([name])
GO

ALTER TABLE [dbo].[Trades] CHECK CONSTRAINT [FK_dbo.Trades_dbo.Symbols_SymbolName]
GO


