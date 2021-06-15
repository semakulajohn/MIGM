CREATE TABLE [dbo].[Utility]
(
	[UtilityId] BIGINT IDENTITY(1,1) NOT NULL,	
	[Description] [nvarchar](max) not NULL,
	[Amount]   FLOAT NOT NULL,
	[BatchId] BIGINT  NULL,
	[SectorId] BIGINT NOT NULL,
	[BranchId] BIGINT NOT NULL,
	[Deleted]	[bit] NULL,
	[CreatedBy] [nvarchar](128) NULL, 
    [UpdatedBy] [nvarchar](128) NULL,     
    [DeletedBy] [nvarchar](128) NULL,
	[CreatedOn]	[datetime] NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	[DeletedOn]	[datetime] NULL,

    CONSTRAINT [PK_dbo.Utility] PRIMARY KEY CLUSTERED 
(
	[UtilityId] ASC
),

CONSTRAINT [FK_Utility_Batch] FOREIGN KEY ([BatchId]) REFERENCES [dbo].[Batch](BatchId),
CONSTRAINT [FK_Utility_Branch] FOREIGN KEY ([BranchId]) REFERENCES [dbo].[Branch](BranchId),
CONSTRAINT [FK_Utility_Sector] FOREIGN KEY ([SectorId]) REFERENCES [dbo].[Sector](SectorId),
CONSTRAINT [FK_Utility_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_Utility_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_Utility_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),
)ON [PRIMARY]


