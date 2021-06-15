CREATE TABLE [dbo].[BatchTransferGradeSize]
(
	[BatchId] BIGINT NOT NULL,
	[FlourTransferId] BIGINT NOT NULL,
	[GradeId] BIGINT NOT NULL,
	[SizeId] BIGINT NOT NULL,
	[Quantity] FLOAT NOT NULL,
	[TimeStamp] DATETIME NOT NULL,
 CONSTRAINT [PK_dbo.BatchTransferGradeSize] PRIMARY KEY CLUSTERED 
(
	[FlourTransferId] ASC,
	[BatchId] ASC,
	[GradeId] ASC,
	[SizeId] ASC
) 
) ON [PRIMARY]

GO