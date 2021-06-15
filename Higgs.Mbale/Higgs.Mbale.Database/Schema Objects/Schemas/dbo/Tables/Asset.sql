CREATE TABLE [dbo].[Asset]
(
	[AssetId]  BIGINT IDENTITY(1,1) NOT NULL,	
	[AssetCategoryId]  BIGINT NOT NULL,
	[BranchId]  BIGINT NULL,
	[AssetCount] FLOAT NOT NULL,
	[PurchaseDate] datetime NOT NULL,
	[Notes]   [nvarchar](max) NULL,
	[Amount] FLOAT NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Deleted]	[bit] NULL,
	[CreatedBy] [nvarchar](128) NULL,  
	[UpdatedBy] [nvarchar](128) NULL,
    [DeletedBy] [nvarchar](128) NULL,
	[CreatedOn]	[datetime] NOT NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	[DeletedOn]	[datetime] NULL,
	

    CONSTRAINT [PK_dbo.AssetId] PRIMARY KEY CLUSTERED 
(
	[AssetId] ASC
),
CONSTRAINT [FK_Asset_BranchId] FOREIGN KEY([BranchId]) REFERENCES [dbo].[Branch](BranchId),
CONSTRAINT [FK_Asset_AssetCategoryId] FOREIGN KEY([AssetCategoryId]) REFERENCES [dbo].[AssetCategory](AssetCategoryId),
CONSTRAINT [FK_Asset_UpdatedBy] FOREIGN KEY([UpdatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_Asset_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_Asset_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),
)ON [PRIMARY]
