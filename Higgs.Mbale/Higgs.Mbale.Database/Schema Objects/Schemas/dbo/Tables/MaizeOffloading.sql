CREATE TABLE [dbo].[MaizeOffloading]
(
	[MaizeOffloadingId]  BIGINT IDENTITY(1,1) NOT NULL,	
	[TransactionSubTypeId]  BIGINT NOT NULL,
	[BranchId]  BIGINT NULL,
	[SectorId]  BIGINT NOT NULL,
	[StartAmount] FLOAT NOT NULL,
	[Action] [nvarchar](128) NOT NULL,
	[Notes]   [nvarchar](max) NULL,
	[Amount] FLOAT NOT NULL,
	[Balance] FLOAT NOT NULL,
	[Deleted]	[bit] NULL,
	[CreatedBy] [nvarchar](128) NULL,     
    [DeletedBy] [nvarchar](128) NULL,
	[CreatedOn]	[datetime] NOT NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	[DeletedOn]	[datetime] NULL,
	[SupplyId]  [bigint]  NULL,
	[WeightNoteNumber] [nvarchar](max),

    CONSTRAINT [PK_dbo.MaizeOffloading] PRIMARY KEY CLUSTERED 
(
	[MaizeOffloadingId] ASC
),
CONSTRAINT [FK_MaizeOffloading_BranchId] FOREIGN KEY([BranchId]) REFERENCES [dbo].[Branch](BranchId),
CONSTRAINT [FK_MaizeOffloading_SupplyId] FOREIGN KEY([SupplyId]) REFERENCES [dbo].[Supply](SupplyId),
CONSTRAINT [FK_MaizeOffloading_SectorId] FOREIGN KEY([SectorId]) REFERENCES [dbo].[Sector](SectorId),
CONSTRAINT [FK_MaizeOffloading_TransactionSubTypeId] FOREIGN KEY([TransactionSubTypeId]) REFERENCES [dbo].[TransactionSubType](TransactionSubTypeId),

CONSTRAINT [FK_MaizeOffloading_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_MaizeOffloading_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),
)ON [PRIMARY]