CREATE PROCEDURE [dbo].[Mark_Asset_AsDeleted]

	@inPutAssetId BIGINT,
	@userId NVARCHAR (128)
		
AS 

BEGIN TRY
 BEGIN TRANSACTION TRA_UpdateAssetDetails
 	
	Update Asset
	SET Deleted =1,DeletedBy = @userId,DeletedOn = GETDATE()
	WHERE AssetId = @inPutAssetId AND Deleted = 0 	
 

 COMMIT TRANSACTION TRA_UpdateAssetDetails

		PRINT 'Operation Successful.'
		
 END TRY
 BEGIN CATCH 
		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK TRANSACTION TRA_UpdateAssetDetails
				PRINT 'Error detected, all changes reversed'
			END
END CATCH

