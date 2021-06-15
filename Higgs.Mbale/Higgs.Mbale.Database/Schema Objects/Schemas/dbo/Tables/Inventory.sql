CREATE TABLE [dbo].[Inventory]
(
	[InventoryId] BIGINT IDENTITY(1,1) NOT NULL,	
	[ItemName] [nvarchar](128) not NULL,
	[Price]   FLOAT NOT NULL,
	[InventoryCategoryId]  BIGINT NOT NULL,
	[Description]    [nvarchar](max) NOT NULL,

	[Deleted]	[bit] NULL,
	[CreatedBy] [nvarchar](128) NULL, 
    [UpdatedBy] [nvarchar](128) NULL,     
    [DeletedBy] [nvarchar](128) NULL,
	[CreatedOn]	[datetime] NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	[DeletedOn]	[datetime] NULL,

    CONSTRAINT [PK_dbo.Inventory] PRIMARY KEY CLUSTERED 
(
	[InventoryId] ASC
),
CONSTRAINT [FK_Inventory_InventoryCategoryId] FOREIGN KEY ([InventoryCategoryId]) REFERENCES [dbo].[InventoryCategory](InventoryCategoryId),
CONSTRAINT [FK_Inventory_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_Inventory_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_Inventory_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),
)ON [PRIMARY]

