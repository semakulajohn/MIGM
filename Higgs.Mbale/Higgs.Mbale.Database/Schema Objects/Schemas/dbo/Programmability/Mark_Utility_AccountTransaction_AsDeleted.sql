CREATE PROCEDURE [dbo].[Mark_Utility_AccountTransaction_AsDeleted]

@inPutUtilityAccountId BIGINT,
	@userId NVARCHAR (128),
	@utilityCategoryId BIGINT
		
AS 

BEGIN TRY
 BEGIN TRANSACTION TRA_UpdateUtilityAccountDetails

	
	Update UtilityAccount
	SET Deleted =1,DeletedBy = @userId,DeletedOn = GETDATE()
	WHERE UtilityAccountId = @inPutUtilityAccountId AND Deleted = 0 AND UtilityCategoryId = @utilityCategoryId
	
 

 COMMIT TRANSACTION TRA_UpdateUtilityAccountDetails

		PRINT 'Operation Successful.'
		
 END TRY
 BEGIN CATCH 
		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK TRANSACTION TRA_UpdateUtilityAccountDetails
				PRINT 'Error detected, all changes reversed'
			END
END CATCH
