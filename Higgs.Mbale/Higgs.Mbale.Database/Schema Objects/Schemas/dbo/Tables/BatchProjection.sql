CREATE TABLE [dbo].[BatchProjection]
(
	[BatchProjectionId] BIGINT IDENTITY(1,1) NOT NULL,	
	[BatchId]   BIGINT NOT NULL,
	[FlourOutPut] FLOAT NOT NULL,
	[FlourSales] FLOAT NOT NULL,
	[BrandSales] FLOAT NOT NULL,
	[FlourPercentage] FLOAT NOT NULL,
	[BrandPercentage] FLOAT NOT NULL,
	[FlourPrice] FLOAT NOT NULL,
	[BrandPrice]  FLOAT NOT NULL,
	[BrandOutPut] FLOAT NOT NULL,
	[BranchId]   BIGINT NOT NULL,
	[UnitCost]    FLOAT NOT NULL,
	[ProductionCost]    FLOAT  NOT NULL,
	[ExpectedContribution]    FLOAT  NULL,
	[Deleted]	[bit] NULL,
	[CreatedBy] [nvarchar](128) NULL, 
    [UpdatedBy] [nvarchar](128) NULL,     
    [DeletedBy] [nvarchar](128) NULL,
	[CreatedOn]	[datetime] NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	[DeletedOn]	[datetime] NULL,

    CONSTRAINT [PK_dbo.BatchProjection] PRIMARY KEY CLUSTERED 
(
	[BatchProjectionId] ASC
),
CONSTRAINT [FK_BatchProjection_BatchId] FOREIGN KEY([BatchId]) REFERENCES [dbo].[Batch](BatchId),
CONSTRAINT [FK_BatchProjection_BranchId] FOREIGN KEY([BranchId]) REFERENCES [dbo].[Branch](BranchId),
CONSTRAINT [FK_BatchProjection_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_BatchProjection_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_BatchProjection_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),
)ON [PRIMARY]
