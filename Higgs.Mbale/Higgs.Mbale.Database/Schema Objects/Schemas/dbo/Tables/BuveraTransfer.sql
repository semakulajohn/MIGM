CREATE TABLE [dbo].[BuveraTransfer]
(
	[BuveraTransferId] BIGINT IDENTITY(1,1) NOT NULL,	
	[StoreId]  BIGINT NOT NULL,
	[ProductId]  BIGINT NOT NULL,
	[BranchId]   BIGINT NOT NULL,
	[Deleted]	[bit] NULL,
	[ToReceiverStoreId] BIGINT NOT NULL,
	[Accept]  [bit] NOT NULL,
	[Reject]   [bit] NOT NULL,
	[FromSupplierStoreId] BIGINT NOT NULL,
	[TotalQuantity] FLOAT NOT NULL,
	[CreatedBy] [nvarchar](128) NULL, 
    [UpdatedBy] [nvarchar](128) NULL,     
    [DeletedBy] [nvarchar](128) NULL,
	[CreatedOn]	[datetime] NOT NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	[DeletedOn]	[datetime] NULL,

    CONSTRAINT [PK_dbo.BuveraTransfer] PRIMARY KEY CLUSTERED 
(
	[BuveraTransferId] ASC
),
CONSTRAINT [FK_BuveraTransfer_StoreId] FOREIGN KEY([StoreId]) REFERENCES [dbo].[Store](StoreId),
CONSTRAINT [FK_BuveraTransfer_ProductId] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Product](ProductId),
CONSTRAINT [FK_BuveraTransfer_ToReceiverStoreId] FOREIGN KEY([ToReceiverStoreId]) REFERENCES [dbo].[Store](StoreId),
CONSTRAINT [FK_BuveraTransfer_FromSupplierStoreId] FOREIGN KEY([FromSupplierStoreId]) REFERENCES [dbo].[Store](StoreId),
CONSTRAINT [FK_BuveraTransfer_BranchId] FOREIGN KEY([BranchId]) REFERENCES [dbo].[Branch](BranchId),
CONSTRAINT [FK_BuveraTransfer_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_BuveraTransfer_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_BuveraTransfer_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),
)ON [PRIMARY]
