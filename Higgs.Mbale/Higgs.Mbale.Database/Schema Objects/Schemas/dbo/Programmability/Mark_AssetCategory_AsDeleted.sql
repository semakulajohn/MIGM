CREATE PROCEDURE [dbo].[Mark_AssetCategory_AsDeleted]
		
@inPutAssetCategoryId BIGINT,
@userId NVARCHAR (128)
		
AS 
DECLARE 
@AssetCategoryId BIGINT,
@AssetId BIGINT

DECLARE @AssetCategoryAssets TABLE
(
		AssetId bigint
	)


BEGIN TRY
 BEGIN TRANSACTION TRA_UpdateAssetCategoryDetails

INSERT INTO @AssetCategoryAssets
	SELECT AssetId FROM [Asset] WHERE AssetCategoryId = @inPutAssetCategoryId  AND Deleted = 0 

WHILE(Select Count(*) From @AssetCategoryAssets) > 0
BEGIN
	SELECT TOP 1 @AssetId = AssetId From @AssetCategoryAssets 
				
		
	 Update [Asset]
	 SET Deleted = 1,DeletedBy = @userId,DeletedOn = GETDATE()
	 WHERE AssetId =@AssetId AND Deleted = 0
	 
	 Delete @AssetCategoryAssets Where AssetId = @AssetId

	
	
 END
 
	Update [AssetCategory]
	SET Deleted =1,DeletedBy = @userId,DeletedOn = GETDATE()
	WHERE AssetCategoryId = @inPutAssetCategoryId AND Deleted = 0

 COMMIT TRANSACTION TRA_UpdateAssetCategoryDetails

		PRINT 'Operation Successful.'
		
 END TRY
 BEGIN CATCH 
		IF (@@TRANCOUNT > 0)
			BEGIN
				ROLLBACK TRANSACTION TRA_UpdateAssetCategoryDetails
				PRINT 'Error detected, all changes reversed'
			END
END CATCH


