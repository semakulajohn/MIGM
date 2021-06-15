CREATE PROCEDURE [dbo].[Mark_Activity_AsDeleted]
		@inPutActivityId BIGINT,
	@userId NVARCHAR (128)
		
AS 
DECLARE 
@ActivityId BIGINT,
@CasualActivityId BIGINT

DECLARE @ActivityBatchCasuals TABLE(
	ActivityId BIGINT
)


DECLARE @CasualActivities TABLE(
	CasualActivityId BIGINT
)
BEGIN TRY
 BEGIN TRANSACTION TRA_UpdateActivityDetails

 
INSERT INTO @ActivityBatchCasuals

	SELECT ActivityId FROM ActivityBatchCasual  WHERE ActivityId = @inPutActivityId  AND Deleted = 0 

WHILE(Select Count(*) From @ActivityBatchCasuals) > 0
BEGIN
	SELECT TOP 1 @ActivityId = ActivityId From @ActivityBatchCasuals 


	Update ActivityBatchCasual
	SET Deleted = 1,DeletedBy = @userId, DeletedOn = GETDATE() 
	WHERE ActivityId = @inPutActivityId AND Deleted = 0
	
	Delete @ActivityBatchCasuals  Where ActivityId = @ActivityId

		
	END
	
 
INSERT INTO @CasualActivities

	SELECT CasualActivityId FROM CasualActivity  WHERE ActivityId = @inPutActivityId  AND Deleted = 0 

WHILE(Select Count(*) From @CasualActivities) > 0
BEGIN
	SELECT TOP 1 @CasualActivityId = CasualActivityId From @CasualActivities 


	Update CasualActivity
	SET Deleted = 1,DeletedBy = @userId, DeletedOn = GETDATE() 
	WHERE ActivityId = @inPutActivityId AND Deleted = 0
	
	Delete @CasualActivities  Where CasualActivityId = @CasualActivityId

		
	END
	
		
	Update Activity
	SET Deleted =1,DeletedBy = @userId,DeletedOn = GETDATE()
	WHERE ActivityId = @inPutActivityId AND Deleted = 0
	

	

 COMMIT TRANSACTION TRA_UpdateActivityDetails

		PRINT 'Operation Successful.'
		
 END TRY
 BEGIN CATCH 
		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK TRANSACTION TRA_UpdateActivityDetails
				PRINT 'Error detected, all changes reversed'
			END
END CATCH