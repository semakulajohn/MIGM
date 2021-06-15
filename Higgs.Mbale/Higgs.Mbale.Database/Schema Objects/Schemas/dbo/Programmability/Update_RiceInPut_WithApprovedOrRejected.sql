CREATE PROCEDURE [dbo].[Update_RiceInPut_WithApprovedOrRejected]
	@inPutId BIGINT,
	@approved BIT,
	@userId NVARCHAR (128)
		
AS 


BEGIN TRY
 BEGIN TRANSACTION TRA_UpdateInPutDetails

	
	Update RiceInput
	SET Approved = @approved,UpdatedBy = @userId,[TimeStamp] = GETDATE()
	WHERE RiceInputId = @inPutId AND Deleted = 0
	
 

 COMMIT TRANSACTION TRA_UpdateInPutDetails

		PRINT 'Operation Successful.'
		
 END TRY
 BEGIN CATCH 
		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK TRANSACTION TRA_UpdateInPutDetails
				PRINT 'Error detected, all changes reversed'
			END
END CATCH
