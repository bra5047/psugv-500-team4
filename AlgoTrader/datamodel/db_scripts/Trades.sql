SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Trades](
	[TradeId] [int] IDENTITY(1,1) NOT NULL,
	[InitialQuantity] [int] NULL,
	[quantity] [int] NOT NULL,
	[price] [float] NOT NULL,
	[timestamp] [datetime] NOT NULL,
	[type] [int] NOT NULL,
	[TransactionId] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
	[SymbolName] [nvarchar](128) NULL,
	[PositionId] [int] NULL,
	[RelatedTradeId] [int] NULL,
 CONSTRAINT [PK_dbo.Trades] PRIMARY KEY CLUSTERED 
(
	[TradeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

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

ALTER TABLE [dbo].[Trades]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Trades_dbo.Trades_RelatedTradeId] FOREIGN KEY([RelatedTradeId])
REFERENCES [dbo].[Trades] ([TradeId])
GO

ALTER TABLE [dbo].[Trades] CHECK CONSTRAINT [FK_dbo.Trades_dbo.Trades_RelatedTradeId]
GO


