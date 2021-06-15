CREATE TABLE [dbo].[CashSaleBatch]
(
	[BatchId]    BIGINT NOT NULL,
	[CashSaleId] BIGINT NOT NULL,
	[BatchQuantity] FLOAT  NULL,
	[Price]         FLOAT NULL,
	[CreatedOn]	[datetime] NOT NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	[Amount] FLOAT NULL, 
	[ProductId] BIGINT NULL,

  CONSTRAINT [PK_dbo.CashSaleBatch] PRIMARY KEY CLUSTERED 
(
	[CashSaleId] ASC,
	[BatchId]  ASC
),
CONSTRAINT [FK_CashSaleBatch_BatchId] FOREIGN KEY([BatchId]) REFERENCES [dbo].[Batch](BatchId),
CONSTRAINT [FK_CashSaleBatch_DeliveryId] FOREIGN KEY([CashSaleId]) REFERENCES [dbo].[CashSale](CashSaleId),
CONSTRAINT [FK_CashSaleBatch_ProductId] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Product](ProductId),

)ON [PRIMARY]
