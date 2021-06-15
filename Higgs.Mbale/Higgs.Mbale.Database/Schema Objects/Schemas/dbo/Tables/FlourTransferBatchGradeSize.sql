CREATE TABLE [dbo].[FlourTransferBatchGradeSize]
(
	
	[BatchId] BIGINT NOT NULL,
	[FlourTransferId] BIGINT NOT NULL,
	[GradeId] BIGINT NOT NULL,
	[SizeId] BIGINT NOT NULL,
	[Quantity] FLOAT NOT NULL,
	[TimeStamp] DATETIME NOT NULL,
 CONSTRAINT [PK_dbo.FlourTransferBatchGradeSize] PRIMARY KEY CLUSTERED 
(
	[FlourTransferId] ASC,
	[BatchId] ASC,
	[GradeId] ASC,
	[SizeId] ASC
) 
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[FlourTransferBatchGradeSize]  ADD  CONSTRAINT [FK_dbo.FlourTransferBatchGradeSize_dbo.Batch_BatchId] FOREIGN KEY([BatchId])
REFERENCES [dbo].[Batch] ([BatchId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[FlourTransferBatchGradeSize] CHECK CONSTRAINT [FK_dbo.FlourTransferBatchGradeSize_dbo.Batch_BatchId]
GO

ALTER TABLE [dbo].[FlourTransferBatchGradeSize]  ADD  CONSTRAINT [FK_dbo.FlourTransferBatchGradeSize_dbo.FlourTransfer_FlourTransferId] FOREIGN KEY([FlourTransferId])
REFERENCES [dbo].[FlourTransfer] ([FlourTransferId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[FlourTransferBatchGradeSize] CHECK CONSTRAINT [FK_dbo.FlourTransferBatchGradeSize_dbo.FlourTransfer_FlourTransferId]
GO

ALTER TABLE [dbo].[FlourTransferBatchGradeSize]  ADD  CONSTRAINT [FK_dbo.FlourTransferBatchGradeSize_dbo.Size_SizeId] FOREIGN KEY([SizeId])
REFERENCES  [dbo].[Size] ([SizeId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[FlourTransferBatchGradeSize] CHECK CONSTRAINT [FK_dbo.FlourTransferBatchGradeSize_dbo.Size_SizeId]
GO


ALTER TABLE [dbo].[FlourTransferBatchGradeSize]  ADD  CONSTRAINT [FK_dbo.FlourTransferBatchGradeSize_dbo.Grade_GradeId] FOREIGN KEY([GradeId])
REFERENCES  [dbo].[Grade] ([GradeId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[FlourTransferBatchGradeSize] CHECK CONSTRAINT [FK_dbo.FlourTransferBatchGradeSize_dbo.Grade_GradeId]
GO