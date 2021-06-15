CREATE TABLE [dbo].[Application]
(
	[ApplicationId] BIGINT IDENTITY(1,1) NOT NULL,	
	[Name] [nvarchar](max) NULL,
	[TotalCash]  FLOAT NOT NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	
    CONSTRAINT [PK_dbo.Application] PRIMARY KEY CLUSTERED 
(
	[ApplicationId] ASC
),

)ON [PRIMARY]
