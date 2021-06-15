CREATE TABLE [dbo].[BuveraTransferGradeSize]
(
		[BuveraTransferId] BIGINT NOT NULL,
	[GradeId] BIGINT NOT NULL,
	[SizeId] BIGINT NOT NULL,
	[StoreId] BIGINT NOT NULL,
	[Quantity] FLOAT NOT NULL,
	[Rate]		FLOAT NOT NULL,
	[Amount]    FLOAT NOT NULL,
	[TimeStamp] DATETIME NOT NULL,
 CONSTRAINT [PK_dbo.BuveraTransferGradeSize] PRIMARY KEY CLUSTERED 
(
	[BuveraTransferId] ASC,
	[GradeId] ASC,
	[SizeId] ASC
) 
,
 CONSTRAINT [FK_dbo.BuveraTransferGradeSize_dbo.BuveraTransfer_BuveraTransferId] FOREIGN KEY([BuveraTransferId])
REFERENCES [dbo].[BuveraTransfer] ([BuveraTransferId]),

CONSTRAINT [FK_dbo.BuveraTransferGradeSize_dbo.Size_SizeId] FOREIGN KEY([SizeId])
REFERENCES  [dbo].[Size] ([SizeId]),

CONSTRAINT [FK_dbo.BuveraTransferGradeSize_dbo.Grade_GradeId] FOREIGN KEY([GradeId])
REFERENCES  [dbo].[Grade] ([GradeId]),

 CONSTRAINT [FK_dbo.BuveraTransferGradeSize_dbo.Store_StoreId] FOREIGN KEY([StoreId])
REFERENCES  [dbo].[Store] ([StoreId]),

)ON [PRIMARY]

