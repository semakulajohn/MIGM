CREATE TABLE [dbo].[InventoryCategory]
(
	[InventoryCategoryId] BIGINT IDENTITY(1,1) NOT NULL,	
	[Name]     [nvarchar](255) NOT NULL,
	[CreatedOn]	[datetime] NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	[DeletedOn]	[datetime] NULL,

    CONSTRAINT [PK_dbo.InventoryCategory] PRIMARY KEY CLUSTERED 
(
	[InventoryCategoryId] ASC
),

)ON [PRIMARY]
