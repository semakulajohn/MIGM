CREATE TABLE [dbo].[RequistionCategory]
(
	[RequistionCategoryId] BIGINT IDENTITY(1,1) NOT NULL,	
	[Name]  [nvarchar](128) NOT NULL,
	
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	


    CONSTRAINT [PK_dbo.RequistionCategory] PRIMARY KEY CLUSTERED 
(
	[RequistionCategoryId] ASC
),
)ON [PRIMARY]
