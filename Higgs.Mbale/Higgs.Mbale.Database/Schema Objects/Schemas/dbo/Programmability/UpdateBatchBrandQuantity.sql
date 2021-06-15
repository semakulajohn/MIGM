CREATE PROCEDURE [dbo].[UpdateBatchBrandQuantity]

	@inPutBatchId BIGINT,
	@quantity Float,
	@userId NVARCHAR (128)
		
AS 


BEGIN TRY
 BEGIN TRANSACTION TRA_UpdateBatchDetails

	
	Update Batch
	SET BrandBalance = @quantity,UpdatedBy = @userId,[TimeStamp] = GETDATE()
	WHERE BatchId = @inPutBatchId AND Deleted = 0
	
 

 COMMIT TRANSACTION TRA_UpdateBatchDetails

		PRINT 'Operation Successful.'
		
 END TRY
 BEGIN CATCH 
		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK TRANSACTION TRA_UpdateBatchDetails
				PRINT 'Error detected, all changes reversed'
			END
END CATCH
