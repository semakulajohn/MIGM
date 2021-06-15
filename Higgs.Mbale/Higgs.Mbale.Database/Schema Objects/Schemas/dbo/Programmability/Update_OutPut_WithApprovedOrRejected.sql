CREATE PROCEDURE [dbo].[Update_OutPut_WithApprovedOrRejected]

	@inPutOutSourcerOutPutId BIGINT,
	@approved BIT,
	@userId NVARCHAR (128)
		
AS 


BEGIN TRY
 BEGIN TRANSACTION TRA_UpdateOutPutDetails

	
	Update OutSourcerOutPut
	SET Approved = @approved,UpdatedBy = @userId,[TimeStamp] = GETDATE()
	WHERE OutSourcerOutPutId = @inPutOutSourcerOutPutId AND Deleted = 0
	
 

 COMMIT TRANSACTION TRA_UpdateOutPutDetails

		PRINT 'Operation Successful.'
		
 END TRY
 BEGIN CATCH 
		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK TRANSACTION TRA_UpdateOutPutDetails
				PRINT 'Error detected, all changes reversed'
			END
END CATCH
