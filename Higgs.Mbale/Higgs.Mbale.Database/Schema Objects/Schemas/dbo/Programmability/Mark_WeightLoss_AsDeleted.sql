CREATE PROCEDURE [dbo].[Mark_WeightLoss_AsDeleted]

	@inPutWeightLossId BIGINT,
	@userId NVARCHAR (128)
		
AS 

BEGIN TRY
 BEGIN TRANSACTION TRA_UpdateWeightLossDetails
 	
	Update WeightLoss
	SET Deleted =1,DeletedBy = @userId,DeletedOn = GETDATE()
	WHERE WeightLossId = @inPutWeightLossId AND Deleted = 0 	
 

 COMMIT TRANSACTION TRA_UpdateWeightLossDetails

		PRINT 'Operation Successful.'
		
 END TRY
 BEGIN CATCH 
		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK TRANSACTION TRA_UpdateWeightLossDetails
				PRINT 'Error detected, all changes reversed'
			END
END CATCH

