CREATE TABLE [dbo].[CashSaleBatchGradeSize]
(
	
	[BatchId] BIGINT NOT NULL,
	[CashSaleId] BIGINT NOT NULL,
	[GradeId] BIGINT NOT NULL,
	[SizeId] BIGINT NOT NULL,
	[Quantity] FLOAT NOT NULL,
	[Amount]   FLOAT NOT NULL,
	[Price]		FLOAT NOT NULL,
	[TimeStamp] DATETIME NOT NULL,
 CONSTRAINT [PK_dbo.CashSaleBatchGradeSize] PRIMARY KEY CLUSTERED 
(
	[CashSaleId] ASC,
	[BatchId] ASC,
	[GradeId] ASC,
	[SizeId] ASC
) 
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[CashSaleBatchGradeSize]  ADD  CONSTRAINT [FK_dbo.CashSaleBatchGradeSize_dbo.Batch_BatchId] FOREIGN KEY([BatchId])
REFERENCES [dbo].[Batch] ([BatchId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CashSaleBatchGradeSize] CHECK CONSTRAINT [FK_dbo.CashSaleBatchGradeSize_dbo.Batch_BatchId]
GO

ALTER TABLE [dbo].[CashSaleBatchGradeSize]  ADD  CONSTRAINT [FK_dbo.CashSaleBatchGradeSize_dbo.CashSale_CashSaleId] FOREIGN KEY([CashSaleId])
REFERENCES [dbo].[CashSale] ([CashSaleId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CashSaleBatchGradeSize] CHECK CONSTRAINT [FK_dbo.CashSaleBatchGradeSize_dbo.CashSale_CashSaleId]
GO

ALTER TABLE [dbo].[CashSaleBatchGradeSize]  ADD  CONSTRAINT [FK_dbo.CashSaleBatchGradeSize_dbo.Size_SizeId] FOREIGN KEY([SizeId])
REFERENCES  [dbo].[Size] ([SizeId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CashSaleBatchGradeSize] CHECK CONSTRAINT [FK_dbo.CashSaleBatchGradeSize_dbo.Size_SizeId]
GO


ALTER TABLE [dbo].[CashSaleBatchGradeSize]  ADD  CONSTRAINT [FK_dbo.CashSaleBatchGradeSize_dbo.Grade_GradeId] FOREIGN KEY([GradeId])
REFERENCES  [dbo].[Grade] ([GradeId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CashSaleBatchGradeSize] CHECK CONSTRAINT [FK_dbo.CashSaleBatchGradeSize_dbo.Grade_GradeId]
GO
