CREATE TABLE [dbo].[StoreProduct]
(
	[StoreId] BIGINT NOT NULL,
	[ProductId] BIGINT NOT NULL,
	[TimeStamp] DATETIME NOT NULL,
 CONSTRAINT [PK_dbo.StoreProduct] PRIMARY KEY CLUSTERED 
(
	[StoreId] ASC,
	[ProductId] ASC
) 
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[StoreProduct]  ADD  CONSTRAINT [FK_dbo.StoreProduct_dbo.Store_StoreId] FOREIGN KEY([StoreId])
REFERENCES [dbo].[Store] ([StoreId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[StoreProduct] CHECK CONSTRAINT [FK_dbo.StoreProduct_dbo.Store_StoreId]
GO

ALTER TABLE [dbo].[StoreProduct]  ADD  CONSTRAINT [FK_dbo.StoreProduct_dbo.Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES  [dbo].[Product] ([ProductId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[StoreProduct] CHECK CONSTRAINT [FK_dbo.StoreProduct_dbo.Product_ProductId]
GO
