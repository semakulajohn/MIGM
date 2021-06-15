CREATE TABLE [dbo].[OutSourcerOutPutGradeSize]
(
	
	[OutSourcerOutPutId] BIGINT NOT NULL,
	[GradeId] BIGINT NOT NULL,
	[SizeId] BIGINT NOT NULL,
	[Quantity] FLOAT NOT NULL,
	[Amount]   FLOAT NOT NULL,
	[Price]		FLOAT NOT NULL,
	[TimeStamp] DATETIME NOT NULL,
 CONSTRAINT [PK_dbo.OutSourcerOutPutGradeSize] PRIMARY KEY CLUSTERED 
(
	[OutSourcerOutPutId] ASC,
	[GradeId] ASC,
	[SizeId] ASC
) 
, CONSTRAINT [FK_dbo.OutSourcerOutPutGradeSize_dbo.OutSourcerOutPut_OutSourcerOutPutId] FOREIGN KEY([OutSourcerOutPutId])
REFERENCES [dbo].[OutSourcerOutPut] ([OutSourcerOutPutId]),


  CONSTRAINT [FK_dbo.OutSourcerOutPutGradeSize_dbo.Size_SizeId] FOREIGN KEY([SizeId])
REFERENCES  [dbo].[Size] ([SizeId]),


 CONSTRAINT [FK_dbo.OutSourcerOutPutGradeSize_dbo.Grade_GradeId] FOREIGN KEY([GradeId])
REFERENCES  [dbo].[Grade] ([GradeId]),
) ON [PRIMARY]






