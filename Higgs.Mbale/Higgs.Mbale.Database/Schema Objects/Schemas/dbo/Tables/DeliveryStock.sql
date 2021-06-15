CREATE TABLE [dbo].[DeliveryStock]
(
	[DeliveryId] BIGINT NOT NULL,
	[StockId] BIGINT NOT NULL,
	[CreatedOn]  [datetime] NOT NULL,
	[TimeStamp]  [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.DeliveryStock] PRIMARY KEY CLUSTERED 
(
	[DeliveryId] ASC,
	[StockId] ASC
) 
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[DeliveryStock]  ADD  CONSTRAINT [FK_dbo.DeliveryStock_dbo.Delivery_DeliveryId] FOREIGN KEY([DeliveryId])
REFERENCES [dbo].[Delivery] ([DeliveryId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[DeliveryStock] CHECK CONSTRAINT [FK_dbo.DeliveryStock_dbo.Delivery_DeliveryId]
GO

ALTER TABLE [dbo].[DeliveryStock]  ADD  CONSTRAINT [FK_dbo.DeliveryStock_dbo.Stock_StockId] FOREIGN KEY([StockId])
REFERENCES  [dbo].[Stock] ([StockId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[DeliveryStock] CHECK CONSTRAINT [FK_dbo.DeliveryStock_dbo.Stock_StockId]
GO
