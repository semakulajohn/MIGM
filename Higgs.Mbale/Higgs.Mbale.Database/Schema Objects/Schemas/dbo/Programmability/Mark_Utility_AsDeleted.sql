CREATE PROCEDURE [dbo].[Mark_Utility_AsDeleted]
	@inPutUtilityId BIGINT,
	@userId NVARCHAR (128)
		
AS 

BEGIN TRY
 BEGIN TRANSACTION TRA_UpdateUtilityDetails

	
	Update Utility
	SET Deleted =1,DeletedBy = @userId,DeletedOn = GETDATE()
	WHERE UtilityId = @inPutUtilityId AND Deleted = 0
	
 

 COMMIT TRANSACTION TRA_UpdateUtilityDetails

		PRINT 'Operation Successful.'
		
 END TRY
 BEGIN CATCH 
		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK TRANSACTION TRA_UpdateUtilityDetails
				PRINT 'Error detected, all changes reversed'
			END
END CATCH




