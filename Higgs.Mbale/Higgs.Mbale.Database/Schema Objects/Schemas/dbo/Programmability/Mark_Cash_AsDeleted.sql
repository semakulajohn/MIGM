CREATE PROCEDURE [dbo].[Mark_Cash_AsDeleted]

@inPutCashId BIGINT,
	@userId NVARCHAR (128),
	@branchId BIGINT 
		
AS 

BEGIN TRY
 BEGIN TRANSACTION TRA_UpdateCashDetails

	
	Update Cash
	SET Deleted =1,DeletedBy = @userId,DeletedOn = GETDATE()
	WHERE CashId = @inPutCashId AND Deleted = 0 AND BranchId = @branchId
	
 

 COMMIT TRANSACTION TRA_UpdateCashDetails

		PRINT 'Operation Successful.'
		
 END TRY
 BEGIN CATCH 
		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK TRANSACTION TRA_UpdateCashDetails
				PRINT 'Error detected, all changes reversed'
			END
END CATCH
