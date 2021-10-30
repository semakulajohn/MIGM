CREATE PROCEDURE [dbo].[GetBranchDashboardNotifications]
	@branchId bigint 
	
AS

BEGIN

	select
	cashtransfers=(select count(1) from CashTransfer Where (Reject =  0 AND Accept = 0 AND Deleted = 0) AND ToReceiverBranchId = @branchId),
	requistions = (select count(1) from Requistion Where (StatusId = 10002 AND( Rejected = 0 AND Approved = 0) AND Deleted =0 AND BranchId = @branchId)),
	orders = (select count(1) from [Order] Where (StatusId = 10002 AND Deleted = 0) AND BranchId = @branchId)

END 