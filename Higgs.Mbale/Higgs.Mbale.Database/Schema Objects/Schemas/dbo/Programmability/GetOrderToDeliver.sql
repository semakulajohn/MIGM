CREATE PROCEDURE [dbo].[GetOrderToDeliver]
	@orderId BIGINT
	
AS 


BEGIN TRY
 BEGIN TRANSACTION TRA_GetOrderDetails

	
	select * FROM [Order] WHERE OrderId = @orderId
	
 

 COMMIT TRANSACTION TRA_GetOrderDetails

		PRINT 'Operation Successful.'
		
 END TRY
 BEGIN CATCH 
		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK TRANSACTION TRA_GetOrderDetails
				PRINT 'Error detected, all changes reversed'
			END
END CATCH