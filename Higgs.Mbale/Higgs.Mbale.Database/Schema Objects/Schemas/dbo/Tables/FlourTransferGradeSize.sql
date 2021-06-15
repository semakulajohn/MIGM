CREATE TABLE [dbo].[FlourTransferGradeSize]
(
	[FlourTransferId] BIGINT NOT NULL,
	[GradeId] BIGINT NOT NULL,
	[SizeId] BIGINT NOT NULL,
	[StoreId] BIGINT NOT NULL,
	[Quantity] FLOAT NOT NULL,
	[Rate]		FLOAT NOT NULL,
	[Amount]    FLOAT NOT NULL,
	[TimeStamp] DATETIME NOT NULL,
 CONSTRAINT [PK_dbo.FlourTransferGradeSize] PRIMARY KEY CLUSTERED 
(
	[FlourTransferId] ASC,
	[GradeId] ASC,
	[SizeId] ASC
) 
,
 CONSTRAINT [FK_dbo.FlourTransferGradeSize_dbo.FlourTransfer_FlourTransferId] FOREIGN KEY([FlourTransferId])
REFERENCES [dbo].[FlourTransfer] ([FlourTransferId]),

CONSTRAINT [FK_dbo.FlourTransferGradeSize_dbo.Size_SizeId] FOREIGN KEY([SizeId])
REFERENCES  [dbo].[Size] ([SizeId]),

CONSTRAINT [FK_dbo.FlourTransferGradeSize_dbo.Grade_GradeId] FOREIGN KEY([GradeId])
REFERENCES  [dbo].[Grade] ([GradeId]),

 CONSTRAINT [FK_dbo.FlourTransferGradeSize_dbo.Store_StoreId] FOREIGN KEY([StoreId])
REFERENCES  [dbo].[Store] ([StoreId]),

)ON [PRIMARY]

