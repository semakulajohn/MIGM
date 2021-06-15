CREATE PROCEDURE [dbo].[Mark_Supply_AsDeleted]
	@inPutSupplyId BIGINT,
	@userId NVARCHAR (128)
		
AS 
DECLARE 
@SupplyId BIGINT,
@AccountTransactionId BIGINT,
@TransactionId BIGINT

DECLARE @SupplyTransactions TABLE
(
	TransactionId BIGINT
	
)
DECLARE @SupplyAccountTransactions TABLE
(
	AccountTransactionActivityId bigint
)


BEGIN TRY
 BEGIN TRANSACTION TRA_UpdateSupplyRelatedDetails

INSERT INTO @SupplyTransactions
	SELECT TransactionId FROM [Transaction] WHERE SupplyId = @inPutSupplyId  AND Deleted = 0 

	
INSERT INTO @SupplyAccountTransactions
	SELECT AccountTransactionActivityId FROM [AccountTransactionActivity] WHERE SupplyId = @inPutSupplyId  AND Deleted = 0 


WHILE(Select Count(*) From @SupplyTransactions) > 0
BEGIN
	SELECT TOP 1 @TransactionId = TransactionId From @SupplyTransactions 

	
	Update [Transaction]
	SET Deleted = 1,DeletedBy = @userId,DeletedOn = GETDATE()
	WHERE TransactionId = @TransactionId AND Deleted = 0 AND SupplyId = @inPutSupplyId
	 Delete @SupplyTransactions Where TransactionId = @TransactionId
	
	END


WHILE(Select Count(*) From @SupplyAccountTransactions) > 0
	BEGIN
		SELECT TOP 1 @AccountTransactionId = AccountTransactionActivityId From @SupplyAccountTransactions 

	
			Update AccountTransactionActivity
			SET Deleted = 1,DeletedBy = @userId,DeletedOn = GETDATE()
			WHERE AccountTransactionActivityId =@AccountTransactionId AND Deleted = 0 AND SupplyId = @inPutSupplyId
	 
			Delete @SupplyAccountTransactions Where AccountTransactionActivityId = @SupplyAccountTransactions

		

		
	END
		
			
Update Supply
	SET Deleted =1,DeletedBy = @userId,DeletedOn = GETDATE()
	WHERE SupplyId = @inPutSupplyId AND Deleted = 0
	


 COMMIT TRANSACTION TRA_UpdateSupplyRelatedDetails

		PRINT 'Operation Successful.'
		
 END TRY
 BEGIN CATCH 
		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK TRANSACTION TRA_UpdateSupplyRelatedDetails
				PRINT 'Error detected, all changes reversed'
			END
END CATCH
