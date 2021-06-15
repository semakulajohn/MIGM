CREATE TABLE [dbo].[Buvera]
(
	[BuveraId] BIGINT IDENTITY(1,1) NOT NULL,	
	[TotalCost] FLOAT NULL,
	[StoreId]  BIGINT NOT NULL,
	[ProductId]  BIGINT NOT NULL,
	[InvoiceNumber] [nvarchar](50) NULL,
	[BranchId]   BIGINT NOT NULL,
	[Deleted]	[bit] NULL,
	[ToReceiver]  [nvarchar](255) NULL,
	[FromSupplier] [nvarchar](255) NOT NULL,
	[TotalQuantity] FLOAT NOT NULL,
	[CreatedBy] [nvarchar](128) NULL, 
    [UpdatedBy] [nvarchar](128) NULL,     
    [DeletedBy] [nvarchar](128) NULL,
	[CreatedOn]	[datetime] NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	[DeletedOn]	[datetime] NULL,
	[BuveraCategoryId] [bigint] NOT NULL,
	[Approved]     [bit] NULL,
	[Rejected]     [bit] NULL,

    CONSTRAINT [PK_dbo.Buvera] PRIMARY KEY CLUSTERED 
(
	[BuveraId] ASC
),
CONSTRAINT [FK_Buvera_StoreId] FOREIGN KEY([StoreId]) REFERENCES [dbo].[Store](StoreId),
CONSTRAINT [FK_Buvera_ProductId] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Product](ProductId),
CONSTRAINT [FK_Buvera_BranchId] FOREIGN KEY([BranchId]) REFERENCES [dbo].[Branch](BranchId),
CONSTRAINT [FK_Buvera_BuveraCategoryId] FOREIGN KEY([BuveraCategoryId]) REFERENCES [dbo].[BuveraCategory](BuveraCategoryId),
CONSTRAINT [FK_Buvera_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_Buvera_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_Buvera_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),
)ON [PRIMARY]