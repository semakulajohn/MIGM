CREATE TABLE [dbo].[UtilityAccount]
(
	[UtilityAccountId] BIGINT IDENTITY(1,1) NOT NULL,
	[UtilityCategoryId] BIGINT NOT NULL,	
	[Amount] FLOAT NOT NULL,
	[InvoiceNumber]  [nvarchar](max) NOT NULL,
	[Description]  [nvarchar](max) NULL,
	[Action] [nvarchar](128) NOT NULL,
	[BranchId]   BIGINT NOT NULL,
	[Deleted]	[bit] NULL,
	[Balance]  FLOAT NOT NULL,
	[StartAmount] FLOAT NOT NULL,
	[CreatedBy] [nvarchar](128) NULL, 
    [UpdatedBy] [nvarchar](128) NULL,     
    [DeletedBy] [nvarchar](128) NULL,
	[CreatedOn]	[datetime] NOT NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	[DeletedOn]	[datetime] NULL,

    CONSTRAINT [PK_dbo.UtilityAccount] PRIMARY KEY CLUSTERED 
(
	[UtilityAccountId] ASC
),

CONSTRAINT [FK_UtilityAccount_BranchId] FOREIGN KEY([BranchId]) REFERENCES [dbo].[Branch](BranchId),
CONSTRAINT [FK_UtilityAccount_CategoryId] FOREIGN KEY([UtilityCategoryId]) REFERENCES [dbo].[UtilityCategory](UtilityCategoryId),

CONSTRAINT [FK_UtilityAccount_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_UtilityAccount_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_UtilityAccount_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),
)ON [PRIMARY]