CREATE TABLE [dbo].[CashSale]
(
	[CashSaleId] BIGINT IDENTITY(1,1) NOT NULL,	
	[Price]      FLOAT NULL,
	[ProductId]   BIGINT NOT NULL,
	[PaymentModeId] BIGINT NOT NULL,
	[TransactionSubTypeId] BIGINT NOT NULL,
	[BranchId]  BIGINT NOT NULL,
	[SectorId] BIGINT NOT NULL,
	[Amount]   FLOAT  NULL,
	[StoreId]	BIGINT NOT NULL,
	[Quantity]  FLOAT NULL,
	[ReceiptLimit] INT NULL,
	[Cancelled]  [bit] NOT NULL DEFAULT(0),
	[Deleted]	[bit] NULL,
	[CreatedBy] [nvarchar](128) NULL, 
    [UpdatedBy] [nvarchar](128) NULL,     
    [DeletedBy] [nvarchar](128) NULL,
	[CreatedOn]	[datetime] NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	[DeletedOn]	[datetime] NULL,

    CONSTRAINT [PK_dbo.CashSale] PRIMARY KEY CLUSTERED 
(
	[CashSaleId] ASC
),

CONSTRAINT [FK_CashSale_StoreId] FOREIGN KEY([StoreId]) REFERENCES [dbo].[Store] (StoreId),
CONSTRAINT [FK_CashSale_PaymentModeId] FOREIGN KEY ([PaymentModeId]) REFERENCES [dbo].[PaymentMode](PaymentModeId),

CONSTRAINT [FK_CashSale_ProductId] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Product] (ProductId),
CONSTRAINT [FK_CashSale_BranchId] FOREIGN KEY([BranchId]) REFERENCES [dbo].[Branch](BranchId),
CONSTRAINT [FK_CashSale_SectorId] FOREIGN KEY([SectorId]) REFERENCES [dbo].[Sector](SectorId),
CONSTRAINT [FK_CashSale_TransactionSubTypeId] FOREIGN KEY([TransactionSubTypeId]) REFERENCES [dbo].[TransactionSubType](TransactionSubTypeId),
CONSTRAINT [FK_CashSale_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_CashSale_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_CashSale_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),
)ON [PRIMARY]

