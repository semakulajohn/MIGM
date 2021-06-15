CREATE PROCEDURE [dbo].[Update_Delivery_WithApprovedOrRejected]
	@inPutDeliveryId BIGINT,
	@approved BIT,
	@userId NVARCHAR (128)
		
AS 


BEGIN TRY
 BEGIN TRANSACTION TRA_UpdateDeliveryDetails

	
	Update Delivery
	SET Approved = @approved,UpdatedBy = @userId,[TimeStamp] = GETDATE()
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
