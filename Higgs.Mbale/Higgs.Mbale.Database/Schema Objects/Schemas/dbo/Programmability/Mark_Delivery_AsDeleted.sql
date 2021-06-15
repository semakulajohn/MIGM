CREATE PROCEDURE [dbo].[Mark_Delivery_AsDeleted]
	
@inPutDeliveryId BIGINT,
	@userId NVARCHAR (128)
		
AS 

BEGIN TRY
 BEGIN TRANSACTION TRA_UpdateDeliveryDetails

	
	Update Delivery
	SET Deleted =1,DeletedBy = @userId,DeletedOn = GETDATE()
	WHERE DeliveryId = @inPutDeliveryId AND Deleted = 0 
	
 

 COMMIT TRANSACTION TRA_UpdateDeliveryDetails

		PRINT 'Operation Successful.'
		
 END TRY
 BEGIN CATCH 
		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK TRANSACTION TRA_UpdateDeliveryDetails
				PRINT 'Error detected, all changes reversed'
			END
END CATCH
