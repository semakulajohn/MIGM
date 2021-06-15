CREATE TABLE [dbo].[Cash]
(
	[CashId]  BIGINT IDENTITY(1,1) NOT NULL,	
	[TransactionSubTypeId]  BIGINT NOT NULL,
	[BranchId]  BIGINT NOT NULL,
	[SectorId]  BIGINT NOT NULL,
	[StartAmount] FLOAT NOT NULL,
	[CustomerId]  [nvarchar](128) NULL,
	[Action] [nvarchar](128) NOT NULL,
	[Notes]   [nvarchar](max) NULL,
	[Amount] FLOAT NOT NULL,
	[Balance] FLOAT NOT NULL,
	[RequistionCategoryId]  BIGINT NOT NULL,
	[Deleted]	[bit] NULL,
	[CreatedBy] [nvarchar](128) NULL,     
    [DeletedBy] [nvarchar](128) NULL,
	[CreatedOn]	[datetime] NOT NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	[DeletedOn]	[datetime] NULL,

    CONSTRAINT [PK_dbo.Cash] PRIMARY KEY CLUSTERED 
(
	[CashId] ASC
),
CONSTRAINT [FK_Cash_BranchId] FOREIGN KEY([BranchId]) REFERENCES [dbo].[Branch](BranchId),
CONSTRAINT [FK_Cash_SectorId] FOREIGN KEY([SectorId]) REFERENCES [dbo].[Sector](SectorId),
CONSTRAINT [FK_Cash_RequistionCategoryId] FOREIGN KEY([RequistionCategoryId]) REFERENCES [dbo].[RequistionCategory](RequistionCategoryId),
CONSTRAINT [FK_Cash_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_Cash_TransactionSubTypeId] FOREIGN KEY([TransactionSubTypeId]) REFERENCES [dbo].[TransactionSubType](TransactionSubTypeId),
CONSTRAINT [FK_Cash_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_Cash_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),
)ON [PRIMARY]

