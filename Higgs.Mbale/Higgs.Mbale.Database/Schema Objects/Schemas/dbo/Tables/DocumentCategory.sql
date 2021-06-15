CREATE TABLE [dbo].[DocumentCategory]
(
	[DocumentCategoryId] BIGINT IDENTITY(1,1) NOT NULL,	
	[Name]  [nvarchar](128) NOT NULL,
	
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	


    CONSTRAINT [PK_dbo.DocumentCategory] PRIMARY KEY CLUSTERED 
(
	[DocumentCategoryId] ASC
),
)ON [PRIMARY]

