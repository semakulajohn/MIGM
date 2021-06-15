CREATE TABLE [dbo].[AspNetUserProduct]
(
	[Id] [nvarchar](128) NOT NULL,
	[ProductId] BIGINT NOT NULL,
	
	[Deleted]	[bit] NULL,
	
	[CreatedOn]	[datetime] NULL,
	


	CONSTRAINT [PK_dbo.AspNetUserProduct] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	),
	CONSTRAINT [FK_AspNetUserProduct_Id] FOREIGN KEY ([Id]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_AspNetUserProduct_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product](ProductId),

) ON [PRIMARY]

GO