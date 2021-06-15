CREATE TABLE [dbo].[AspNetUserCode]
(
	

	[Id] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
	[Code] INT NOT NULL,
	[Deleted]	[bit] NULL,
	
	[CreatedOn]	[datetime] NULL,
	


	CONSTRAINT [PK_dbo.AspNetUserCode] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	),
	CONSTRAINT [FK_AspNetUserCode_Id] FOREIGN KEY ([Id]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_AspNetUserCode_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles](Id),

) ON [PRIMARY]

GO

