CREATE TABLE [dbo].[OutSourcerOutPut]
(
	[OutSourcerOutPutId] BIGINT IDENTITY(1,1) NOT NULL,	
	[TotalQuantity] FLOAT NOT NULL,
	[Price] FLOAT NOT NULL,
	[PersonLoaded] [nvarchar](255) NULL,
	[TotalAmount]	FLOAT NOT NULL,
	[StoreId]   BIGINT NOT NULL,
	[Approved]  BIT NULL,
	[Deleted]	[bit] NULL,
	[CreatedBy] [nvarchar](128) NULL, 
    [UpdatedBy] [nvarchar](128) NULL,     
    [DeletedBy] [nvarchar](128) NULL,
	[CreatedOn]	[datetime] NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	[DeletedOn]	[datetime] NULL,

    CONSTRAINT [PK_dbo.OutSourcerOutPut] PRIMARY KEY CLUSTERED 
(
	[OutSourcerOutPutId] ASC
),
CONSTRAINT [FK_OutSourcerOutPut_StoreId] FOREIGN KEY([StoreId]) REFERENCES [dbo].[Store](StoreId),
CONSTRAINT [FK_OutSourcerOutPut_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_OutSourcerOutPut_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_OutSourcerOutPut_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),
)ON [PRIMARY]

