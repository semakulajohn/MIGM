CREATE TABLE [dbo].[WeightLoss]
(
	[WeightLossId] BIGINT IDENTITY(1,1) NOT NULL,	
	[DeliveryId]	BIGINT not NULL,
	[Quantity]		Float NOT NULL,
	[CustomerId]    nvarchar(128) NOT NULL,
	[Price]			Float NOT NULL,
	[BranchId]		BIGINT NOT NULL,
	[DeliveryDate]   [datetime] NOT NULL,
	[Deleted]	[bit] NULL,
	[CreatedBy] [nvarchar](128) NULL, 
    [UpdatedBy] [nvarchar](128) NULL,     
    [DeletedBy] [nvarchar](128) NULL,
	[CreatedOn]	[datetime] NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	[DeletedOn]	[datetime] NULL,
	[Approved]  [bit]		NULL,

    CONSTRAINT [PK_dbo.WeightLoss] PRIMARY KEY CLUSTERED 
(
	[WeightLossId] ASC
),
CONSTRAINT [FK_WeightLoss_BranchId] FOREIGN KEY([BranchId]) REFERENCES [dbo].[Branch](BranchId),
CONSTRAINT [FK_WeightLoss_DeliveryId] FOREIGN KEY([DeliveryId]) REFERENCES [dbo].[Delivery](DeliveryId),
CONSTRAINT [FK_WeightLoss_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_WeightLoss_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_WeightLoss_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_WeightLoss_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),
)ON [PRIMARY]


