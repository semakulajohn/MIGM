CREATE PROCEDURE [dbo].[UpdateOrderGradeSizes]
	@orderId BIGINT,
	@gradeId BIGINT,
	@sizeId BIGINT,
	@quantity FLOAT,
	@balance FLOAT
		
AS 


BEGIN TRY
 BEGIN TRANSACTION TRA_UpdateOrderGradeSize

	
	insert into OrderGradeSize (OrderId,GradeId,SizeId,Quantity,[TimeStamp],Balance)
	Values(@orderId,@gradeId,@sizeId,@quantity,GETDATE(), @balance)
	
 COMMIT TRANSACTION TRA_UpdateOrderGradeSize

		PRINT 'Operation Successful.'
		
 END TRY
 BEGIN CATCH 
		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK TRANSACTION TRA_UpdateOrderGradeSize
				PRINT 'Error detected, all changes reversed'
			END
END CATCH