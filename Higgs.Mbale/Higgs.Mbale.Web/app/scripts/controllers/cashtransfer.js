
angular
    .module('homer')
    .controller('CashTransferDetailController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$state', 'usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $state, usSpinnerService) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }
        $scope.hideAcceptReject = false;

        var cashTransferId = $scope.cashTransferId;
        var branchId = $scope.branchId;



        var promisebranch = $http.get('/webapi/BranchApi/GetBranch?branchId=' + branchId, {});
        promisebranch.then(
            function (payload) {
                var b = payload.data;

                $scope.branch = {
                    BranchId: b.BranchId,
                    Name: b.Name,

                };

            });
        var promise = $http.get('/webapi/CashTransferApi/GetCashTransfer?cashTransferId=' + cashTransferId, {});
        promise.then(
            function (payload) {
                var b = payload.data;
                if ((b.Accept != true && b.Reject != true) && b.FromBranchId != b.ToReceiverBranchId) {
                    $scope.hideAcceptReject = true;
                }
                else if ((b.Accept == true || b.Reject == true) && b.FromBranchId != b.ToReceiverBranchId) {
                    $scope.hideAcceptReject = false;
                }
                $scope.cashTransfer = {
                    CashTransferId: b.CashTransferId,               
                    ToReceiverBranchId: b.ToReceiverBranchId,
                    FromBranchId: b.FromBranchId,
                    Accept: b.Accept,
                    Amount: b.Amount,
                    AmountInWords : b.AmountInWords,
                    Reject: b.Reject,
                    Notes : b.Notes,
                    Response : b.Response,
                    TimeStamp: b.TimeStamp,
                    CreatedOn: b.CreatedOn,
                    CreatedByName : b.CreatedByName,
                    CreatedBy: b.CreatedBy,
                    UpdatedBy: b.UpdatedBy,
                    Deleted: b.Deleted,
                    SectorId: b.SectorId,
                    Department : b.Department,
                    ReceiverBranch : b.ReceiverBranch,
                    FromBranch: b.FromBranch,
                   
                   
                };
            });


        $scope.Accept = function (cashTransfer) {
            usSpinnerService.spin('global-spinner');

            var promise = $http.post('/webapi/CashTransferApi/Accept', {
                CashTransferId: cashTransfer.CashTransferId,
                ToReceiverBranchId: cashTransfer.ToReceiverBranchId,
                FromBranchId: cashTransfer.FromBranchId,
                Response : cashTransfer.Response,
                Amount: cashTransfer.Amount,
                AmountInWords : cashTransfer.AmountInWords,
                BranchId: cashTransfer.BranchId,
                Notes: cashTransfer.Notes,
                CreatedBy : cashTransfer.CreatedBy,
                FromBranch: cashTransfer.FromBranch,
                ReceiverBranch: cashTransfer.ReceiverBranch,
                SectorId: cashTransfer.SectorId,
                Department : cashTransfer.Department,
              
            });

            promise.then(
                function (payload) {

                    cashTransferId = payload.data;
                     if (cashTransferId == -2) {
                        $scope.showMessageSameBranch = true;
                        usSpinnerService.stop('global-spinner');
                        $timeout(function () {
                            $scope.showMessageSameBranch = false;


                        }, 4000);
                    }
                     else if (cashTransferId == -44) {
                         $scope.showMessageSameUser= true;
                         usSpinnerService.stop('global-spinner');
                         $timeout(function () {
                             $scope.showMessageSameUser = false;


                         }, 4000);
                     }
                    else  {
                        usSpinnerService.stop('global-spinner');
                        $timeout(function () {

                            $state.go('unapprovedcashtransfers-list');


                        }, 4000);
                    }


                });
        }

        $scope.Reject = function (cashTransfer) {

            usSpinnerService.spin('global-spinner');

            var promise = $http.post('/webapi/CashTransferApi/Reject', {
                CashTransferId: cashTransfer.CashTransferId,
                ToReceiverBranchId: cashTransfer.ToReceiverBranchId,
                FromBranchId: cashTransfer.FromBranchId,
                Response: cashTransfer.Response,
                Amount: cashTransfer.Amount,
                Notes : cashTransfer.Notes,
                AmountInWords: cashTransfer.AmountInWords,
                FromBranch: cashTransfer.FromBranch,
                ReceiverBranch : cashTransfer.ReceiverBranch,
                SectorId: cashTransfer.SectorId,
                Department : cashTransfer.Department,

            });

            promise.then(
                function (payload) {

                    cashTransferId = payload.data;
                    usSpinnerService.stop('global-spinner');
                    $timeout(function () {


                        //$state.go('cashlist-branch-transfer', { 'branchId': $scope.branchId });
                         $state.go('unapprovedcashtransfers-list');

                    }, 500);
                });
        }



    }]);


angular
    .module('homer').controller('BranchCashTransferController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            var branchId = $scope.branchId;



            var promisebranch = $http.get('/webapi/BranchApi/GetBranch?branchId=' + branchId, {});
            promisebranch.then(
                function (payload) {
                    var b = payload.data;

                    $scope.branch = {
                        BranchId: b.BranchId,
                        Name: b.Name,

                    };

                });

            var promise = $http.get('/webapi/CashTransferApi/GetAllCashTransfersForAparticularBranch?branchId=' + branchId, {});
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;
                }
            );

            $scope.retrievedBranchId = $scope.branchId;
            $scope.gridData = {
                enableFiltering: true,
                columnDefs: $scope.columns,
                enableRowSelection: false
            };

            $scope.gridData.multiSelect = false;


            $scope.gridData.columnDefs = [

                {
                    name: 'CashTransferId', field: 'CashTransferId'

                },
                { name: 'Amount', field: 'Amount' },

                { name: 'From Branch', field: 'FromBranch' },
                { name: 'Receiver Branch', field: 'ReceiverBranch' },
                { name: 'Notes', field: 'Notes' },
              
                { name: 'Date', field: 'CreatedOn' },
                { name: 'Accept', field: 'Accept' },
                { name: 'Reject', field: 'Reject' },

            {
                name: 'Transfer Details', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/cashs/' + $scope.branchId + '/{{row.entity.CashTransferId}}">Transfer Details</a> </div>',

            },

            ];




        }]);


angular
    .module('homer').controller('UnApprovedCashTransferController', ['$scope', '$http',
        function ($scope, $http,  ) {
            $scope.loadingSpinner = true;
           

            var promise = $http.get('/webapi/CashTransferApi/GetAllUnApprovedCashTransfers');
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;
                }
            );

           
            $scope.gridData = {
                enableFiltering: true,
                columnDefs: $scope.columns,
                enableRowSelection: false
            };

            $scope.gridData.multiSelect = false;


            $scope.gridData.columnDefs = [

                {
                    name: 'CashTransferId', field: 'CashTransferId'

                },
                { name: 'Amount', field: 'Amount' },

                { name: 'From Branch', field: 'FromBranch' },
                { name: 'Receiver Branch', field: 'ReceiverBranch' },
                { name: 'Notes', field: 'Notes' },
              
                { name: 'Date', field: 'CreatedOn' },
                { name: 'Accept', field: 'Accept' },
                { name: 'Reject', field: 'Reject' },

            {
                name: 'Transfer Details', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/cashs/' + $scope.branchId + '/{{row.entity.CashTransferId}}">Transfer Details</a> </div>',

            },

            ];




        }]);



angular
    .module('homer')
    .controller('CashTransferIssueController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval, usSpinnerService) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }

        $scope.selectedGrades = [];
        var branches = [];
       
        var selectedBranch;
        var cashTransferId = $scope.cashTransferId;
        var action = $scope.action;
        var branchId = $scope.branchId;
        var transfer = "YES";
        var sectorId = 10003;
      
        $http.get('/webapi/SectorApi/GetAllSectors').success(function (data, status) {
            $scope.sectors = data;
        });
        var promisebranch = $http.get('/webapi/BranchApi/GetBranch?branchId=' + branchId, {});
        promisebranch.then(
            function (payload) {
                var b = payload.data;

                $scope.branch = {
                    BranchId: b.BranchId,
                    Name: b.Name,

                };

            });



        $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
            $scope.bdata = {
                branches: data,
                selectedBranch: branches[0]
            }
        });

       
        


        if (action == 'create') {
            cashTransferId = 0;
            var promise = $http.get('/webapi/UserApi/GetLoggedInUser', {});
            promise.then(
                function (payload) {
                    var c = payload.data;
                    $scope.user = {
                        UserName: c.UserName,
                        Id: c.Id,
                        FirstName: c.FirstName,
                        LastName: c.LastName,
                        UserRoles: c.UserRoles,
                    };
                }

            );
        }

        if (action == 'edit') {
            var promise = $http.get('/webapi/CashTransferApi/GetCashTransfer?cashTransferId=' + cashTransferId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;

                    $scope.cashTransfer = {
                        CashTransferId: b.CashTransferId,
                        Amount: b.Amount,
                       AmountInWords : b.AmountInWords,
                        FromBranchId: b.FromBranchId,
                        ToReceiverBranchId: b.ToReceiverBranchId,
                        Response: b.Response,
                        Notes : b.Notes,
                        SectorId : b.SectorId,
                        TimeStamp: b.TimeStamp,
                        CreatedOn: b.CreatedOn,
                        CreatedBy: b.CreatedBy,
                        UpdatedBy: b.UpdatedBy,
                        Deleted: b.Deleted,

                    };
                });

        }

        $scope.Save = function (cashTransfer) {

           
            $scope.showMessageSave = false;
            usSpinnerService.spin('global-spinner');


           
            if ($scope.form.$valid) {
                var promise = $http.post('/webapi/CashTransferApi/Save', {
                    CashTransferId: cashTransferId,
                   Response : cashTransfer.Response,
                    ToReceiverBranchId: cashTransfer.BranchId,
                    FromBranchId: branchId,
                    Notes : cashTransfer.Notes,
                    Amount: cashTransfer.Amount,
                    AmountInWords : cashTransfer.AmountInWords,
                    SectorId : sectorId,
                 
                });

                promise.then(
                    function (payload) {

                        cashTransferId = payload.data;
                        if (cashTransferId == -1) {
                            $scope.showMessageCashNotEnough = true;
                            usSpinnerService.stop('global-spinner');

                            $timeout(function () {
                                $scope.showMessageCashNotEnough = false;

                            }, 4000);
                        }
                        else if (cashTransferId == -2) {
                            $scope.showMessageSameBranch = true;
                            usSpinnerService.stop('global-spinner');
                            $timeout(function () {
                                $scope.showMessageSameBranch = false;


                            }, 4000);
                        }
                        else {
                            $scope.showMessageSave = true;
                            usSpinnerService.stop('global-spinner');
                            $timeout(function () {
                                $scope.showMessageSave = false;

                                if (action == "create") {
                                    $state.go('cashlist-branch-transfer', { 'branchId': $scope.branchId });
                                }

                            }, 1500);

                        }
                       
                    });
            }

        }


        $scope.Cancel = function () {
            $state.go('cashlist-branch-transfer', { 'branchId': $scope.branchId });
        };



    }
    ]);
