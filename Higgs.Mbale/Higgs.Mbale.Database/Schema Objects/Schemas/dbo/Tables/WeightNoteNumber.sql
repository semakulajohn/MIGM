CREATE TABLE [dbo].[WeightNoteNumber]
(
	[WeightNoteNumberId] BIGINT IDENTITY(1,1) NOT NULL,	
	[WeightNoteValue] float NOT NULL,
	[WeightNoteRangeId] BIGINT NOT NULL,
	[BranchId] bigint NOT NULL,
	[Used]	[bit] NOT NULL,
	[Deleted]	[bit] NULL,
	[CreatedBy] [nvarchar](128) NULL, 
    [UpdatedBy] [nvarchar](128) NULL,     
    [DeletedBy] [nvarchar](128) NULL,
	[CreatedOn]	[datetime] NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	[DeletedOn]	[datetime] NULL,
	[Notes]	   [nvarchar](max) NULL,
	[NotUsed]	[bit] NOT NULL DEFAULT ((0)),

    CONSTRAINT [PK_dbo.WeightNoteNumber] PRIMARY KEY CLUSTERED 
(
	[WeightNoteNumberId] ASC
),
CONSTRAINT [FK_WeightNoteNumber_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_WeightNoteNumber_WeightNoteRange] FOREIGN KEY ([WeightNoteRangeId]) REFERENCES [dbo].[WeightNoteRange](WeightNoteRangeId),
CONSTRAINT [FK_WeightNoteNumber_BranchId] FOREIGN KEY ([BranchId]) REFERENCES [dbo].[Branch](BranchId),

CONSTRAINT [FK_WeightNoteNumber_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_WeightNoteNumber_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),
)ON [PRIMARY]

