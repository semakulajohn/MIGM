CREATE PROCEDURE [dbo].[UpdateStoreStockWithSoldOut]

	@inPutStockId BIGINT,
	@soldOut BIT,
	@inPutProductId BIGINT,
	@userId NVARCHAR (128)
		
AS 


BEGIN TRY
-- couldn't use full or reasonable name coz sql complains name is too long maximum(32) xters
 BEGIN TRANSACTION TRA_UpdateStoreStockDetails

	
	Update StoreStock
	SET SoldOut = @soldOut,[TimeStamp] = GETDATE()
	WHERE StockId = @inPutStockId AND ProductId = @inPutProductId
	
 
	Update Stock
	SET SoldOut = @soldOut,UpdatedBy = @userId,[TimeStamp] = GETDATE()
	WHERE StockId = @inPutStockId AND Deleted = 0
	

 COMMIT TRANSACTION TRA_UpdateStoreStockDetails

		PRINT 'Operation Successful.'
		
 END TRY
 BEGIN CATCH 
		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK TRANSACTION TRA_UpdateStoreStockDetails
				PRINT 'Error detected, all changes reversed'
			END

END CATCH