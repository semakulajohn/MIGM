CREATE TABLE [dbo].[FlourTransferBatch]
(

	[BatchId]    BIGINT NOT NULL,
	[FlourTransferId] BIGINT NOT NULL,
	[BatchQuantity] FLOAT NOT  NULL,
	[CreatedOn]	[datetime] NOT NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	

  CONSTRAINT [PK_dbo.FlourTransferBatch] PRIMARY KEY CLUSTERED 
(
	[FlourTransferId] ASC,
	[BatchId]  ASC
),
CONSTRAINT [FK_FlourTransferBatch_BatchId] FOREIGN KEY([BatchId]) REFERENCES [dbo].[Batch](BatchId),
CONSTRAINT [FK_FlourTransferBatch_DeliveryId] FOREIGN KEY([FlourTransferId]) REFERENCES [dbo].[FlourTransfer](FlourTransferId),
)ON [PRIMARY]
