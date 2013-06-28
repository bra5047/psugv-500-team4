SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Alerts](
	[AlertId] [uniqueidentifier] NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[SymbolName] [nvarchar](128) NULL,
	[Type] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [float] NOT NULL,
	[SentTo] [nvarchar](max) NULL,
	[ResponseCode] [int] NULL,
	[Response] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Alerts] PRIMARY KEY CLUSTERED 
(
	[AlertId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Alerts]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Alerts_dbo.Symbols_SymbolName] FOREIGN KEY([SymbolName])
REFERENCES [dbo].[Symbols] ([name])
GO

ALTER TABLE [dbo].[Alerts] CHECK CONSTRAINT [FK_dbo.Alerts_dbo.Symbols_SymbolName]
GO


