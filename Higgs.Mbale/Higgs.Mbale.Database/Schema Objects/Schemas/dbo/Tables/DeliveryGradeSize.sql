CREATE TABLE [dbo].[DeliveryGradeSize]
(
	[DeliveryId] BIGINT NOT NULL,
	[GradeId] BIGINT NOT NULL,
	[SizeId] BIGINT NOT NULL,
	[Quantity] FLOAT NOT NULL,
	[Price]		FLOAT NOT NULL,
	[Amount]    FLOAT NOT NULL,
	[TimeStamp] DATETIME NOT NULL,
 CONSTRAINT [PK_dbo.DeliveryGradeSize] PRIMARY KEY CLUSTERED 
(
	[DeliveryId] ASC,
	[GradeId] ASC,
	[SizeId] ASC
) 
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[DeliveryGradeSize]  ADD  CONSTRAINT [FK_dbo.DeliveryGradeSize_dbo.Delivery_DeliveryId] FOREIGN KEY([DeliveryId])
REFERENCES [dbo].[Delivery] ([DeliveryId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[DeliveryGradeSize] CHECK CONSTRAINT [FK_dbo.DeliveryGradeSize_dbo.Delivery_DeliveryId]
GO

ALTER TABLE [dbo].[DeliveryGradeSize]  ADD  CONSTRAINT [FK_dbo.DeliveryGradeSize_dbo.Size_SizeId] FOREIGN KEY([SizeId])
REFERENCES  [dbo].[Size] ([SizeId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[DeliveryGradeSize] CHECK CONSTRAINT [FK_dbo.DeliveryGradeSize_dbo.Size_SizeId]
GO


ALTER TABLE [dbo].[DeliveryGradeSize]  ADD  CONSTRAINT [FK_dbo.DeliveryGradeSize_dbo.Grade_GradeId] FOREIGN KEY([GradeId])
REFERENCES  [dbo].[Grade] ([GradeId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[DeliveryGradeSize] CHECK CONSTRAINT [FK_dbo.DeliveryGradeSize_dbo.Grade_GradeId]
GO



