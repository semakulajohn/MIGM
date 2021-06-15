CREATE TABLE [dbo].[UtilityCategory]
(
	[UtilityCategoryId] BIGINT IDENTITY(1,1) NOT NULL,	
	[Name]  [nvarchar](128) NOT NULL,
	
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	


    CONSTRAINT [PK_dbo.UtilityCategory] PRIMARY KEY CLUSTERED 
(
	[UtilityCategoryId] ASC
),
)ON [PRIMARY]
