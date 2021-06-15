CREATE TABLE [dbo].[RiceInputGradeSize]
(
	[RiceInputId] BIGINT NOT NULL,
	[GradeId] BIGINT NOT NULL,
	[SizeId] BIGINT NOT NULL,
	[Quantity] FLOAT NOT NULL,
	[Amount]   FLOAT NOT NULL,
	[Price]		FLOAT NOT NULL,
	[TimeStamp] DATETIME NOT NULL,
 CONSTRAINT [PK_dbo.RiceInputGradeSize] PRIMARY KEY CLUSTERED 
(
	[RiceInputId] ASC,
	[GradeId] ASC,
	[SizeId] ASC
) 
,
 CONSTRAINT [FK_dbo.RiceInputGradeSize_dbo.RiceInput_RiceInputId] FOREIGN KEY([RiceInputId])
REFERENCES [dbo].[RiceInput] ([RiceInputId]),

CONSTRAINT [FK_dbo.RiceInputGradeSize_dbo.Size_SizeId] FOREIGN KEY([SizeId])
REFERENCES  [dbo].[Size] ([SizeId]),

CONSTRAINT [FK_dbo.RiceInputGradeSize_dbo.Grade_GradeId] FOREIGN KEY([GradeId])
REFERENCES  [dbo].[Grade] ([GradeId]),

)ON [PRIMARY]
