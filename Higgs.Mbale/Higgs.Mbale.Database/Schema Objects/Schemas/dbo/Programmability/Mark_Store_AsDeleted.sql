CREATE PROCEDURE [dbo].[Mark_Store_AsDeleted]

@inPutStoreId BIGINT,
	@userId NVARCHAR (128)
		
AS 

BEGIN TRY
 BEGIN TRANSACTION TRA_UpdateStoreDetails

	
	Update Store
	SET Deleted =1,DeletedBy = @userId,DeletedOn = GETDATE()
	WHERE StoreId = @inPutStoreId AND Deleted = 0
	
 

 COMMIT TRANSACTION TRA_UpdateStoreDetails

		PRINT 'Operation Successful.'
		
 END TRY
 BEGIN CATCH 
		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK TRANSACTION TRA_UpdateStoreDetails
				PRINT 'Error detected, all changes reversed'
			END
END CATCH

