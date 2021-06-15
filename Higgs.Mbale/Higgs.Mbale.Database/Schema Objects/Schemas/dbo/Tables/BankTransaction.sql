CREATE TABLE [dbo].[BankTransaction]
(
	[BankTransactionId]  BIGINT IDENTITY(1,1) NOT NULL,	
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
	[BankId]  [bigint] NOT NULL,

    CONSTRAINT [PK_dbo.BankTransaction] PRIMARY KEY CLUSTERED 
(
	[BankTransactionId] ASC
),
CONSTRAINT [FK_BankTransaction_BranchId] FOREIGN KEY([BranchId]) REFERENCES [dbo].[Branch](BranchId),
CONSTRAINT [FK_BankTransaction_BankId] FOREIGN KEY([BankId]) REFERENCES [dbo].[Bank](BankId),
CONSTRAINT [FK_BankTransaction_SectorId] FOREIGN KEY([SectorId]) REFERENCES [dbo].[Sector](SectorId),
CONSTRAINT [FK_BankTransaction_TransactionSubTypeId] FOREIGN KEY([TransactionSubTypeId]) REFERENCES [dbo].[TransactionSubType](TransactionSubTypeId),

CONSTRAINT [FK_BankTransaction_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_BankTransaction_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),
)ON [PRIMARY]