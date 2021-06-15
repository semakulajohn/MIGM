CREATE TABLE [dbo].[CashTransfer]
(

	[CashTransferId] BIGINT IDENTITY(1,1) NOT NULL,	
	[Response]   nvarchar(max) NULL,
	[Deleted]	[bit] NULL,
	[Notes]		nvarchar(max) NULL,
	[ToReceiverBranchId] BIGINT NOT NULL,
	[Accept]  [bit] NOT NULL,
	[Reject]   [bit] NOT NULL,
	[FromBranchId] BIGINT NOT NULL,
	[Amount] FLOAT NOT NULL,
	[AmountInWords] nvarchar(max) NOT NULL,
	[CreatedBy] [nvarchar](128) NULL, 
    [UpdatedBy] [nvarchar](128) NULL,     
    [DeletedBy] [nvarchar](128) NULL,
	[CreatedOn]	[datetime] NOT NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	[DeletedOn]	[datetime] NULL,
	[SectorId] BIGINT NOT NULL,
	

    CONSTRAINT [PK_dbo.CashTransfer] PRIMARY KEY CLUSTERED 
(
	[CashTransferId] ASC
),

CONSTRAINT [FK_CashTransfer_ToReceiverBranchId] FOREIGN KEY([ToReceiverBranchId]) REFERENCES [dbo].[Branch](BranchId),
CONSTRAINT [FK_CashTransfer_FromBranchId] FOREIGN KEY([FromBranchId]) REFERENCES [dbo].[Branch](BranchId),
CONSTRAINT [FK_CashTransfer_SectorId] FOREIGN KEY([SectorId]) REFERENCES [dbo].[Sector](SectorId),
CONSTRAINT [FK_CashTransfer_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_CashTransfer_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_CashTransfer_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),
)ON [PRIMARY]
