CREATE TABLE [dbo].[BatchDeliveryGradeSize]
(
	[BatchId] BIGINT NOT NULL,
	[DeliveryId] BIGINT NOT NULL,
	[GradeId] BIGINT NOT NULL,
	[SizeId] BIGINT NOT NULL,
	[Quantity] FLOAT NOT NULL,
	[Amount]   FLOAT NOT NULL,
	[Price]		FLOAT NOT NULL,
	[TimeStamp] DATETIME NOT NULL,
 CONSTRAINT [PK_dbo.BatchDeliveryGradeSize] PRIMARY KEY CLUSTERED 
(
	[DeliveryId] ASC,
	[BatchId] ASC,
	[GradeId] ASC,
	[SizeId] ASC
) 
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[BatchDeliveryGradeSize]  ADD  CONSTRAINT [FK_dbo.BatchDeliveryGradeSize_dbo.Batch_BatchId] FOREIGN KEY([BatchId])
REFERENCES [dbo].[Batch] ([BatchId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[BatchDeliveryGradeSize] CHECK CONSTRAINT [FK_dbo.BatchDeliveryGradeSize_dbo.Batch_BatchId]
GO

ALTER TABLE [dbo].[BatchDeliveryGradeSize]  ADD  CONSTRAINT [FK_dbo.BatchDeliveryGradeSize_dbo.Delivery_DeliveryId] FOREIGN KEY([DeliveryId])
REFERENCES [dbo].[Delivery] ([DeliveryId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[BatchDeliveryGradeSize] CHECK CONSTRAINT [FK_dbo.BatchDeliveryGradeSize_dbo.Delivery_DeliveryId]
GO

ALTER TABLE [dbo].[BatchDeliveryGradeSize]  ADD  CONSTRAINT [FK_dbo.BatchDeliveryGradeSize_dbo.Size_SizeId] FOREIGN KEY([SizeId])
REFERENCES  [dbo].[Size] ([SizeId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[BatchDeliveryGradeSize] CHECK CONSTRAINT [FK_dbo.BatchDeliveryGradeSize_dbo.Size_SizeId]
GO


ALTER TABLE [dbo].[BatchDeliveryGradeSize]  ADD  CONSTRAINT [FK_dbo.BatchDeliveryGradeSize_dbo.Grade_GradeId] FOREIGN KEY([GradeId])
REFERENCES  [dbo].[Grade] ([GradeId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[BatchDeliveryGradeSize] CHECK CONSTRAINT [FK_dbo.BatchDeliveryGradeSize_dbo.Grade_GradeId]
GO
