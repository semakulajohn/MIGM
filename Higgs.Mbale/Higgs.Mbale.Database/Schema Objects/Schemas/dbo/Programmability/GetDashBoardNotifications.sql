
CREATE PROCEDURE [dbo].[GetDashBoardNotifications]
	AS
BEGIN
	select
	cashtransfers=(select count(1) from CashTransfer Where (Reject =  0 AND Accept = 0)),
	supplies=(select count(1) from Supply Where  Approved IS NULL),
	deliveries = (select count(1) from Delivery Where Approved IS NULL),
	outsourceroutputs = (select count(1) from OutSourcerOutPut Where Approved IS NULL),
	transactions = (select count(1) from Deposit Where Approved IS NULL),
	requistions = (select count(1) from Requistion Where (StatusId = 10002 AND( Rejected = 0 AND Approved = 0)))
		

END