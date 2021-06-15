CREATE TABLE [dbo].[WeightNoteRange]
(
	[WeightNoteRangeId] BIGINT IDENTITY(1,1) NOT NULL,	
	[StartNumber] float NOT NULL,
	[EndNumber] float NOT NULL,
	[BranchId] bigint NOT NULL,
	[Printed]	[bit] NOT NULL,
	[Deleted]	[bit] NULL,
	[CreatedBy] [nvarchar](128) NULL, 
    [UpdatedBy] [nvarchar](128) NULL,     
    [DeletedBy] [nvarchar](128) NULL,
	[CreatedOn]	[datetime] NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	[DeletedOn]	[datetime] NULL,

    CONSTRAINT [PK_dbo.WeightNoteRange] PRIMARY KEY CLUSTERED 
(
	[WeightNoteRangeId] ASC
),
CONSTRAINT [FK_WeightNoteRange_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),

CONSTRAINT [FK_WeightNoteRange_BranchId] FOREIGN KEY ([BranchId]) REFERENCES [dbo].[Branch](BranchId),
CONSTRAINT [FK_WeightNoteRange_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_WeightNoteRange_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),
)ON [PRIMARY]


