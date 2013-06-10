/****** Object:  Table [dbo].[WatchListItems]    Script Date: 6/10/2013 1:17:02 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WatchListItems](
	[SymbolName] [nvarchar](128) NOT NULL,
	[ListName] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.WatchListItems] PRIMARY KEY CLUSTERED 
(
	[SymbolName] ASC,
	[ListName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[WatchListItems]  WITH CHECK ADD  CONSTRAINT [FK_dbo.WatchListItems_dbo.Symbols_SymbolName] FOREIGN KEY([SymbolName])
REFERENCES [dbo].[Symbols] ([name])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[WatchListItems] CHECK CONSTRAINT [FK_dbo.WatchListItems_dbo.Symbols_SymbolName]
GO

ALTER TABLE [dbo].[WatchListItems]  WITH CHECK ADD  CONSTRAINT [FK_dbo.WatchListItems_dbo.WatchLists_ListName] FOREIGN KEY([ListName])
REFERENCES [dbo].[WatchLists] ([ListName])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[WatchListItems] CHECK CONSTRAINT [FK_dbo.WatchListItems_dbo.WatchLists_ListName]
GO


