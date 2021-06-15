CREATE PROCEDURE [dbo].[Mark_FinancialAccountTransaction_AsDeleted]
	@inPutFinancialAccountId BIGINT,
	@userId NVARCHAR (128),
	@inPutFinancialAccountTransactionId BIGINT
		
AS 

BEGIN TRY
 BEGIN TRANSACTION TRA_UpdateFinancialDetails

	
	Update FinancialAccountTransaction
	SET Deleted =1,DeletedBy = @userId,DeletedOn = GETDATE()
	WHERE FinancialAccountId = @inPutFinancialAccountId AND Deleted = 0 AND FinancialAccountTransactionId = @inPutFinancialAccountTransactionId
	
 

 COMMIT TRANSACTION TRA_UpdateFinancialDetails

		PRINT 'Operation Successful.'
		
 END TRY
 BEGIN CATCH 
		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK TRANSACTION TRA_UpdateFinancialDetails
				PRINT 'Error detected, all changes reversed'
			END
END CATCH
