CREATE PROCEDURE [dbo].[UpdateSupplyOnRequistionApproval]

	@inPutSupplyId BIGINT,
	
	@isPaid bit,
	@amountToPay float,
	
	@partialAmount float,
	@partiallyPaid bit,
	@userId NVARCHAR (128)
		
AS 

DECLARE @supplyId BIGINT
BEGIN TRY
 BEGIN TRANSACTION TRA_UpdateSupplyDetails

	
	Update Supply
	SET [TimeStamp] = GETDATE(),
	UpdatedBy = @userId,
	   IsPaid = @isPaid,AmountToPay = @amountToPay, PartiallyPaid = @partiallyPaid,
       PartialAmount = @partialAmount
	WHERE SupplyId = @inPutSupplyId AND (Deleted = 0 OR Deleted = null)
	
 

 COMMIT TRANSACTION TRA_UpdateSupplyDetails

		PRINT 'Operation Successful.'
		RETURN @supplyId
		
 END TRY
 BEGIN CATCH 
		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK TRANSACTION TRA_UpdateSupplyDetails
				RETURN -1
				PRINT 'Error detected, all changes reversed'
			END
END CATCH

