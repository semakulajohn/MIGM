CREATE TABLE [dbo].[Delivery]
(
	[DeliveryId] BIGINT IDENTITY(1,1) NOT NULL,	
	[CustomerId] [nvarchar](128) NOT NULL,
	[DriverName]   [nvarchar](500) NOT NULL,
	[Price]      FLOAT NULL,
	[DeliveryDate] [datetime]  NULL DEFAULT GETDATE(),	
	[ProductId]   BIGINT NOT NULL,
	[PaymentModeId] BIGINT NOT NULL,
	[DeliveryCost] FLOAT NOT NULL,
	[DriverNIN]  [nvarchar](max) NOT NULL,
	[VehicleNumber] [nvarchar](128) NOT NULL,
	[OrderId]     BIGINT NOT NULL,
	[TransactionSubTypeId] BIGINT NOT NULL,
	[MediaId]     BIGINT NOT NULL,
	[BranchId]  BIGINT NOT NULL,
	[SectorId] BIGINT NOT NULL,
	[Amount]   FLOAT NOT NULL,
	[StoreId]	BIGINT NOT NULL,
	[Location]   [nvarchar](max) not null,
	[Deleted]	[bit] NULL,
	[CreatedBy] [nvarchar](128) NULL, 
    [UpdatedBy] [nvarchar](128) NULL,     
    [DeletedBy] [nvarchar](128) NULL,
	[CreatedOn]	[datetime] NULL,
	[TimeStamp]	[datetime] NOT NULL DEFAULT GETDATE(),	
	[DeletedOn]	[datetime] NULL,
	[Approved]  [bit]		NULL,

    CONSTRAINT [PK_dbo.Delivery] PRIMARY KEY CLUSTERED 
(
	[DeliveryId] ASC
),
CONSTRAINT [FK_Delivery_OrderId] FOREIGN KEY([OrderId]) REFERENCES [dbo].[Order](OrderId),
CONSTRAINT [FK_Delivery_StoreId] FOREIGN KEY([StoreId]) REFERENCES [dbo].[Store] (StoreId),
CONSTRAINT [FK_Delivery_PaymentModeId] FOREIGN KEY ([PaymentModeId]) REFERENCES [dbo].[PaymentMode](PaymentModeId),

CONSTRAINT [FK_Delivery_ProductId] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Product] (ProductId),
CONSTRAINT [FK_Delivery_BranchId] FOREIGN KEY([BranchId]) REFERENCES [dbo].[Branch](BranchId),
CONSTRAINT [FK_Delivery_SectorId] FOREIGN KEY([SectorId]) REFERENCES [dbo].[Sector](SectorId),
CONSTRAINT [FK_Delivery_TransactionSubTypeId] FOREIGN KEY([TransactionSubTypeId]) REFERENCES [dbo].[TransactionSubType](TransactionSubTypeId),
CONSTRAINT [FK_Delivery_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_Delivery_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AspNetUsers](Id),
CONSTRAINT [FK_Delivery_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[AspNetUsers](Id),
)ON [PRIMARY]

