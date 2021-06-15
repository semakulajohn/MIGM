CREATE TABLE [dbo].[BuveraCategory]
(
	[BuveraCategoryId] BIGINT IDENTITY(1,1) NOT NULL,	
	[Name] [nvarchar](128) not NULL,

	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),
	[Deleted]   [bit] NOT NULL DEFAULT ((0))
  CONSTRAINT [PK_dbo.BuveraCategory] PRIMARY KEY CLUSTERED 
(
	[BuveraCategoryId] ASC
),

)ON [PRIMARY]