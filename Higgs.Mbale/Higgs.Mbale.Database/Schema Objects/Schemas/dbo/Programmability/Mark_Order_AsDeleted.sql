CREATE PROCEDURE [dbo].[Mark_Order_AsDeleted]
	@inPutOrderId BIGINT,
	@userId NVARCHAR (128)
		
AS 

BEGIN TRY
 BEGIN TRANSACTION TRA_UpdateOrderDetails

	
	Update [Order]
	SET Deleted =1,DeletedBy = @userId,DeletedOn = GETDATE()
	WHERE OrderId = @inPutOrderId AND Deleted = 0
	
 

 COMMIT TRANSACTION TRA_UpdateOrderDetails

		PRINT 'Operation Successful.'
		
 END TRY
 BEGIN CATCH 
		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK TRANSACTION TRA_UpdateOrderDetails
				PRINT 'Error detected, all changes reversed'
			END
END CATCH
