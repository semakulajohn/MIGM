CREATE PROCEDURE [dbo].[CheckIfWeightNoteExists]
	@inPutWeightNoteNumber NVARCHAR(50)
	
AS

BEGIN
SELECT COUNT(*) FROM Supply 
        WHERE WeightNoteNumber= @inPutWeightNoteNumber;
END