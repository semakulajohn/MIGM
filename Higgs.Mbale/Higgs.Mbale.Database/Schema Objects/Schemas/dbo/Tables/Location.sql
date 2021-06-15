CREATE TABLE [dbo].[Location]
(
	[LocationId] BIGINT IDENTITY(1,1) NOT NULL,	
	[Name]  [nvarchar](128) NOT NULL,
	[CreatedOn]	[datetime] NOT NULL,
	[Deleted] bit Default(0),
	[Initials]  [nvarchar](50) NULL,
	[RegionId]  bigint not null,
    CONSTRAINT [PK_dbo.Location] PRIMARY KEY CLUSTERED 
(
	[LocationId] ASC
),
CONSTRAINT [FK_Location_RegionId] FOREIGN KEY([RegionId]) REFERENCES [dbo].[Region](RegionId),

)ON [PRIMARY]