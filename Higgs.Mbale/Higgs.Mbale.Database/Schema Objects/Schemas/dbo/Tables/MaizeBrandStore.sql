CREATE TABLE [dbo].[MaizeBrandStore]
(
	[MaizeBrandStoreId] BIGINT IDENTITY(1,1) NOT NULL,	
	[Quantity] FLOAT NULL,
	[StoreId]  BIGINT NOT NULL,
	[Action] [nvarchar](128) NOT NULL,
	[BranchId]   BIGINT NOT NULL,
	[BatchId] BIGINT NOT NULL,
	[Deleted]	[bit] NULL,
	[Balance]  FLOAT NOT NULL,
	[StartQuantity] FLOAT NOT NULL,
	[CreatedBy] [nvarchar](128) NULL, 
    [UpdatedBy] [nvarchar](128) NULL,     
    [DeletedBy] [nvarchar](128) NULL,
	[CreatedOn]	[datetime] NOT NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	[DeletedOn]	[datetime] NULL,

    CONSTRAINT [PK_dbo.MaizeBrandStore] PRIMARY KEY CLUSTERED 
(
	[MaizeBrandStoreId] ASC
),
CONSTRAINT [FK_MaizeBrandStore_StoreId] FOREIGN KEY([StoreId]) REFERENCES [dbo].[Store](StoreId),
CONSTRAINT [FK_MaizeBrandStore_BranchId] FOREIGN KEY([BranchId]) REFERENCES [dbo].[Branch](BranchId),
CONSTRAINT [FK_MaizeBrandStore_BatchId] FOREIGN KEY([BatchId]) REFERENCES [dbo].[Batch](BatchId),
CONSTRAINT [FK_MaizeBrandStore_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_MaizeBrandStore_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_MaizeBrandStore_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),
)ON [PRIMARY]