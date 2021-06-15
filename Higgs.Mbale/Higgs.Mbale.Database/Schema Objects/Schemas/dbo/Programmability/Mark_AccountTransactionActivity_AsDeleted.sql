CREATE PROCEDURE [dbo].[Mark_AccountTransactionActivity_AsDeleted]

	@inPutTransactionId BIGINT,
	@userId NVARCHAR (128)
		
AS 

BEGIN TRY
 BEGIN TRANSACTION TRA_UpdateTransactionDetails

	
	Update AccountTransactionActivity
	SET Deleted =1,DeletedBy = @userId,DeletedOn = GETDATE()
	WHERE AccountTransactionActivityId = @inPutTransactionId AND Deleted = 0 	
 

 COMMIT TRANSACTION TRA_UpdateTransactionDetails

		PRINT 'Operation Successful.'
		
 END TRY
 BEGIN CATCH 
		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK TRANSACTION TRA_UpdateTransactionDetails
				PRINT 'Error detected, all changes reversed'
			END
END CATCH

	