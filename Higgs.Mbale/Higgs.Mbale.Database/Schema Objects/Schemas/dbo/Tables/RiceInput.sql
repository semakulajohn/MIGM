CREATE TABLE [dbo].[RiceInput]
(
	[RiceInputId] BIGINT IDENTITY(1,1) NOT NULL,	
	[TotalQuantity] FLOAT NOT NULL,
	[Price] FLOAT NOT NULL,
	[TotalAmount]	FLOAT NOT NULL,
	[BranchId]   BIGINT NOT NULL,
	[StoreId]   BIGINT NOT NULL,
	[Approved]  BIT NULL,
	[Deleted]	[bit] NULL,
	[CreatedBy] [nvarchar](128) NULL, 
    [UpdatedBy] [nvarchar](128) NULL,     
    [DeletedBy] [nvarchar](128) NULL,
	[CreatedOn]	[datetime] NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	[DeletedOn]	[datetime] NULL,

    CONSTRAINT [PK_dbo.RiceInput] PRIMARY KEY CLUSTERED 
(
	[RiceInputId] ASC
),
CONSTRAINT [FK_RiceInput_StoreId] FOREIGN KEY([StoreId]) REFERENCES [dbo].[Store](StoreId),
CONSTRAINT [FK_RiceInput_BranchId] FOREIGN KEY([BranchId]) REFERENCES [dbo].[Branch](BranchId),
CONSTRAINT [FK_RiceInput_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_RiceInput_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_RiceInput_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),
)ON [PRIMARY]

