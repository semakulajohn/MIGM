CREATE PROCEDURE [dbo].[UpdateOrderWithInProgressStatus]

	@inPutOrderId BIGINT,
	@statusId BIGINT,
	@balance FLOAT,
	@userId NVARCHAR (128)
		
AS 


BEGIN TRY
 BEGIN TRANSACTION TRA_UpdateOrderDetails

	
	Update [Order]
	SET StatusId = @statusId,Balance = @balance,UpdatedBy = @userId,[TimeStamp] = GETDATE()
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

