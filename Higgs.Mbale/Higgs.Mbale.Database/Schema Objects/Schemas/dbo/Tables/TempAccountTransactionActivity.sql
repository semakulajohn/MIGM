CREATE TABLE [dbo].[TempAccountTransactionActivity]
(
	
	[TempAccountTransactionActivityId]  BIGINT IDENTITY(1,1) NOT NULL,	
	[AspNetUserId] [nvarchar](128)  NULL,
	[CasualWorkerId] BIGINT NULL,
	[TransactionSubTypeId]  BIGINT NOT NULL,
	[BranchId]  BIGINT NULL,
	[SectorId]  BIGINT NOT NULL,
	[StartAmount] FLOAT NOT NULL,
	[Action] [nvarchar](128) NOT NULL,
	[Notes]   [nvarchar](max) NULL,
	[Amount] FLOAT NOT NULL,
	[SupplyId] BIGINT NULL,
	[Balance] FLOAT NOT NULL,
	[Deleted]	[bit] NULL,
	[CreatedBy] [nvarchar](128) NULL,     
    [DeletedBy] [nvarchar](128) NULL,
	[CreatedOn]	[datetime] NOT NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	[DeletedOn]	[datetime] NULL,

    CONSTRAINT [PK_dbo.TempAccountTransactionActivity] PRIMARY KEY CLUSTERED 
(
	[TempAccountTransactionActivityId] ASC
),
CONSTRAINT [FK_TempAccountTransactionActivity_BranchId] FOREIGN KEY([BranchId]) REFERENCES [dbo].[Branch](BranchId),
CONSTRAINT [FK_TempAccountTransactionActivity_SectorId] FOREIGN KEY([SectorId]) REFERENCES [dbo].[Sector](SectorId),
CONSTRAINT [FK_TempAccountTransactionActivity_TransactionSubTypeId] FOREIGN KEY([TransactionSubTypeId]) REFERENCES [dbo].[TransactionSubType](TransactionSubTypeId),
CONSTRAINT [FK_TempAccountTransactionActivity_AspNetUserId] FOREIGN KEY([AspNetUserId]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_TempAccountTransactionActivity_CasualWorkerId] FOREIGN KEY([CasualWorkerId]) REFERENCES [dbo].[CasualWorker](CasualWorkerId),
CONSTRAINT [FK_TempAccountTransactionActivity_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_TempAccountTransactionActivity_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),
)ON [PRIMARY]

