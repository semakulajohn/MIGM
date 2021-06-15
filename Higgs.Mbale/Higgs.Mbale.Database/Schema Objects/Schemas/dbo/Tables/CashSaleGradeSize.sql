CREATE TABLE [dbo].[CashSaleGradeSize]
(
	[CashSaleId] BIGINT NOT NULL,
	[GradeId] BIGINT NOT NULL,
	[SizeId] BIGINT NOT NULL,
	[Quantity] FLOAT NOT NULL,
	[Price]		FLOAT NOT NULL,
	[Amount]    FLOAT NOT NULL,
	[TimeStamp] DATETIME NOT NULL,
 CONSTRAINT [PK_dbo.CashSaleGradeSize] PRIMARY KEY CLUSTERED 
(
	[CashSaleId] ASC,
	[GradeId] ASC,
	[SizeId] ASC
) 
,
 CONSTRAINT [FK_dbo.CashSaleGradeSize_dbo.Grade_GradeId] FOREIGN KEY([GradeId])
REFERENCES  [dbo].[Grade] ([GradeId]),

 CONSTRAINT [FK_dbo.CashSaleGradeSize_dbo.CashSale_CashSaleId] FOREIGN KEY([CashSaleId])
REFERENCES [dbo].[CashSale] ([CashSaleId]),
 
 CONSTRAINT [FK_dbo.CashSaleGradeSize_dbo.Size_SizeId] FOREIGN KEY([SizeId])
REFERENCES  [dbo].[Size] ([SizeId]),




)ON [PRIMARY]











