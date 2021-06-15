CREATE TABLE [dbo].[Document]
(
	[DocumentId] BIGINT IDENTITY(1,1) NOT NULL,	
	[Amount] FLOAT NOT NULL,
	[UserId] [nvarchar](128) NULL,
	[ItemId]  BIGINT NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[DocumentNumber] BIGINT NOT NULL,
	[DocumentCategoryId] BIGINT NOT NULL,
	[Quantity] FLOAT  NULL,
	[AmountINWords] [nvarchar](MAX) NULL,
	[BranchId]   BIGINT NOT NULL,
	[Deleted]	[bit] NULL,
	[CreatedBy] [nvarchar](128) NULL, 
    [UpdatedBy] [nvarchar](128) NULL,     
    [DeletedBy] [nvarchar](128) NULL,
	[CreatedOn]	[datetime] NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	[DeletedOn]	[datetime] NULL,

    CONSTRAINT [PK_dbo.Document] PRIMARY KEY CLUSTERED 
(
	[DocumentId] ASC
),
CONSTRAINT [FK_Document_DocumentCategoryId] FOREIGN KEY([DocumentCategoryId]) REFERENCES [dbo].[DocumentCategory](DocumentCategoryId),
CONSTRAINT [FK_Document_BranchId] FOREIGN KEY([BranchId]) REFERENCES [dbo].[Branch](BranchId),
CONSTRAINT [FK_Document_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_Document_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_Document_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),
)ON [PRIMARY]

