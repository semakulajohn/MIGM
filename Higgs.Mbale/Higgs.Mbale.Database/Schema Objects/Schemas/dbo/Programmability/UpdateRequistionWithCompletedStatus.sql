CREATE PROCEDURE [dbo].[UpdateRequistionWithCompletedStatus]
	@inPutRequistionId BIGINT,
	@statusId BIGINT,
	@userId NVARCHAR (128)
		
AS 


BEGIN TRY
 BEGIN TRANSACTION TRA_UpdateRequistionDetails

	
	Update Requistion
	SET StatusId = @statusId,ApprovedById = @userId,Approved = 1,[TimeStamp] = GETDATE()
	WHERE RequistionId = @inPutRequistionId AND Deleted = 0
	
 

 COMMIT TRANSACTION TRA_UpdateRequistionDetails

		PRINT 'Operation Successful.'
		
 END TRY
 BEGIN CATCH 
		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK TRANSACTION TRA_UpdateRequistionDetails
				PRINT 'Error detected, all changes reversed'
			END
END CATCH