CREATE TABLE [dbo].[DocumentGradeSize]
(

	[DocumentId] BIGINT NOT NULL,
	[GradeId] BIGINT NOT NULL,
	[SizeId] BIGINT NOT NULL,
	
	[Quantity] FLOAT NOT NULL,
	[Price]		FLOAT NOT NULL,
	[Amount]    FLOAT NOT NULL,
	[TimeStamp] DATETIME NOT NULL,
 CONSTRAINT [PK_dbo.DocumentGradeSize] PRIMARY KEY CLUSTERED 
(
	[DocumentId] ASC,
	[GradeId] ASC,
	[SizeId] ASC
),
 CONSTRAINT [FK_dbo.DocumentGradeSize_dbo.Document_DocumentId] FOREIGN KEY([DocumentId])
REFERENCES [dbo].[Document] ([DocumentId]),

  CONSTRAINT [FK_dbo.DocumentGradeSize_dbo.Size_SizeId] FOREIGN KEY([SizeId])
REFERENCES  [dbo].[Size] ([SizeId]),


  CONSTRAINT [FK_dbo.DocumentGradeSize_dbo.Grade_GradeId] FOREIGN KEY([GradeId])
REFERENCES  [dbo].[Grade] ([GradeId]),


) ON [PRIMARY]

