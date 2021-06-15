CREATE PROCEDURE [dbo].[UpdateMaizeBrandBatchQuantity]

	@inPutMaizeStoreId BIGINT,
	@quantity Float,
	@userId NVARCHAR (128)
		
AS 


BEGIN TRY
 BEGIN TRANSACTION TRA_UpdateMaizeBrandStoreDetails

	
	Update MaizeBrandStore
	SET Quantity = @quantity,UpdatedBy = @userId,[TimeStamp] = GETDATE()
	WHERE MaizeBrandStoreId = @inPutMaizeStoreId AND Deleted = 0
	
 

 COMMIT TRANSACTION TRA_UpdateMaizeBrandStoreDetails

		PRINT 'Operation Successful.'
		
 END TRY
 BEGIN CATCH 
		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK TRANSACTION TRA_UpdateMaizeBrandStoreDetails
				PRINT 'Error detected, all changes reversed'
			END
END CATCH
