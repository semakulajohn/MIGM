CREATE TABLE [dbo].[RiceTransferGradeSize]
(
	[RiceTransferId] BIGINT NOT NULL,
	[GradeId] BIGINT NOT NULL,
	[SizeId] BIGINT NOT NULL,
	[StoreId] BIGINT NOT NULL,
	[Quantity] FLOAT NOT NULL,
	[TimeStamp] DATETIME NOT NULL,
 CONSTRAINT [PK_dbo.RiceTransferGradeSize] PRIMARY KEY CLUSTERED 
(
	[RiceTransferId] ASC,
	[GradeId] ASC,
	[SizeId] ASC
) 
,
 CONSTRAINT [FK_dbo.RiceTransferGradeSize_dbo.RiceTransfer_RiceTransferId] FOREIGN KEY([RiceTransferId])
REFERENCES [dbo].[RiceTransfer] ([RiceTransferId]),

CONSTRAINT [FK_dbo.RiceTransferGradeSize_dbo.Size_SizeId] FOREIGN KEY([SizeId])
REFERENCES  [dbo].[Size] ([SizeId]),

CONSTRAINT [FK_dbo.RiceTransferGradeSize_dbo.Grade_GradeId] FOREIGN KEY([GradeId])
REFERENCES  [dbo].[Grade] ([GradeId]),

 CONSTRAINT [FK_dbo.RiceTransferGradeSize_dbo.Store_StoreId] FOREIGN KEY([StoreId])
REFERENCES  [dbo].[Store] ([StoreId]),

)ON [PRIMARY]