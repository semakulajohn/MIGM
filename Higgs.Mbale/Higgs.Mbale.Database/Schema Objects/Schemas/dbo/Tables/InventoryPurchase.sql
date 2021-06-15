CREATE TABLE [dbo].[InventoryPurchase]
(
	[InventoryPurchaseId] BIGINT IDENTITY(1,1) NOT NULL,
	[InventoryId] BIGINT NOT NULL,
	[ItemName] [nvarchar](128) not NULL,
	[Amount] FLOAT NOT NULL,
	[Price]   FLOAT NOT NULL,
	[Quantity] FLOAT NOT NULL,
	[Description]    [nvarchar](max) NOT NULL,
	[BranchId]   BIGINT NOT NULL,
	[SectorId] BIGINT NOT NULL,
	[StoreId] BIGINT NOT NULL,
	[TransactionSubTypeId] BIGINT NOT NULL,
	[PurchaseDate]  [datetime] NOT NULL,
	[Deleted]	[bit] NULL,
	[CreatedBy] [nvarchar](128) NULL, 
    [UpdatedBy] [nvarchar](128) NULL,     
    [DeletedBy] [nvarchar](128) NULL,
	[CreatedOn]	[datetime] NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	[DeletedOn]	[datetime] NULL,

    CONSTRAINT [PK_dbo.InventoryPurchase] PRIMARY KEY CLUSTERED 
(
	[InventoryPurchaseId] ASC
),
CONSTRAINT [FK_InventoryPurchase_SectorId] FOREIGN KEY ([SectorId]) REFERENCES [dbo].[Sector](SectorId),
CONSTRAINT [FK_InventoryPurchase_InventoryId] FOREIGN KEY ([InventoryId]) REFERENCES [dbo].[Inventory](InventoryId),
CONSTRAINT [FK_InventoryPurchase_BranchId] FOREIGN KEY ([BranchId]) REFERENCES [dbo].[Branch](BranchId),
CONSTRAINT [FK_InventoryPurchase_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [dbo].[Store](StoreId),
CONSTRAINT [FK_InventoryPurchase_TransactionSubTypeId] FOREIGN KEY ([TransactionSubTypeId]) REFERENCES [dbo].[TransactionSubType](TransactionSubTypeId),
CONSTRAINT [FK_InventoryPurchase_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_InventoryPurchase_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_InventoryPurchase_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),
)ON [PRIMARY]


