CREATE TABLE [dbo].[FinancialAccountTransaction]
(
	[FinancialAccountTransactionId]  BIGINT IDENTITY(1,1) NOT NULL,	
	[BranchId]  BIGINT NULL,
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
	[FinancialAccountId]  [bigint] NOT NULL,

    CONSTRAINT [PK_dbo.FinancialAccountTransaction] PRIMARY KEY CLUSTERED 
(
	[FinancialAccountTransactionId] ASC
),
CONSTRAINT [FK_FinancialAccountTransaction_BranchId] FOREIGN KEY([BranchId]) REFERENCES [dbo].[Branch](BranchId),
CONSTRAINT [FK_FinancialAccountTransaction_FinancialAccountId] FOREIGN KEY([FinancialAccountId]) REFERENCES [dbo].[FinancialAccount](FinancialAccountId),

CONSTRAINT [FK_FinancailAccountTransaction_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_FinancialAccountTransaction_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),
)ON [PRIMARY]