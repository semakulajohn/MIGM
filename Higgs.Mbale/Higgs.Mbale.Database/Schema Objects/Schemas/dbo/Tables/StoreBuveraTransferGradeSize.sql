CREATE TABLE [dbo].[StoreBuveraTransferGradeSize]
(
	--this table stores data for storebuveratransferGradeSize

	[GradeId] BIGINT NOT NULL,
	[StoreId] BIGINT NOT NULL,
	[SizeId] BIGINT NOT NULL,
	[Quantity] FLOAT NOT NULL,
	[TimeStamp] DATETIME NOT NULL,
 CONSTRAINT [PK_dbo.StoreBuveraTransferGradeSize] PRIMARY KEY CLUSTERED 
(
	
	[GradeId] ASC,
	[SizeId] ASC
) ,


  CONSTRAINT [FK_dbo.StoreBuveraTransferGradeSize_dbo.Grade_GradeId] FOREIGN KEY([GradeId])
REFERENCES  [dbo].[Grade] ([GradeId]),


 CONSTRAINT [FK_dbo.StoreBuveraTransferGradeSize_dbo.Size_SizeId] FOREIGN KEY([SizeId])
REFERENCES  [dbo].[Size] ([SizeId]),

 CONSTRAINT [FK_dbo.StoreBuveraTransferGradeSize_dbo.Store_StoreId] FOREIGN KEY([StoreId])
REFERENCES  [dbo].[Store] ([StoreId]),


) ON [PRIMARY]

