CREATE TABLE [dbo].[WeightNoteSupply]
(
	[WeightNoteNumberId] BIGINT NOT NULL,
	[SupplyId] BIGINT NOT NULL,
	[CreatedOn]  [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.WeightNoteSupply] PRIMARY KEY CLUSTERED 
(
	[WeightNoteNumberId] ASC,
	[SupplyId] ASC
) 
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[WeightNoteSupply]  ADD  CONSTRAINT [FK_dbo.WeightSupply_dbo.WeightNoteNumber_WeightNoteNumberId] FOREIGN KEY([WeightNoteNumberId])
REFERENCES [dbo].[WeightNoteNumber] ([WeightNoteNumberId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[WeightNoteSupply]  ADD  CONSTRAINT [FK_dbo.WeightNoteSupply_dbo.Supply_SupplyId] FOREIGN KEY([SupplyId])
REFERENCES  [dbo].[Supply] ([SupplyId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[WeightNoteSupply] CHECK CONSTRAINT [FK_dbo.WeightNoteSupply_dbo.Supply_SupplyId]
GO

