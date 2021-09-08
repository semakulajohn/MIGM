CREATE TABLE [dbo].[RequistionSupply]
(
	[RequistionId] BIGINT NOT NULL,
	[SupplyId] BIGINT NOT NULL,
	
	[TimeStamp] DATETIME NOT NULL,
 CONSTRAINT [PK_dbo.RequistionSupply] PRIMARY KEY CLUSTERED 
(
	[RequistionId] ASC,
	[SupplyId] ASC
	
) 
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[RequistionSupply]  ADD  CONSTRAINT [FK_dbo.RequistionSupply_dbo.Requistion_RequistionId] FOREIGN KEY([RequistionId])
REFERENCES [dbo].[Requistion] ([RequistionId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[RequistionSupply] CHECK CONSTRAINT [FK_dbo.RequistionSupply_dbo.Requistion_RequistionId]
GO

ALTER TABLE [dbo].[RequistionSupply]  ADD  CONSTRAINT [FK_dbo.RequistionSupply_dbo.Supply_SupplyId] FOREIGN KEY([SupplyId])
REFERENCES  [dbo].[Supply] ([SupplyId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[RequistionSupply] CHECK CONSTRAINT [FK_dbo.RequistionSupply_dbo.Supply_SupplyId]
GO

