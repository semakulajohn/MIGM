angular
    .module('homer')
    .controller('AccountCasualActivityEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'selectBatchService', 'usSpinnerService',
function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval,selectBatchService,usSpinnerService) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }
        var branches = [];
        var selectedBranch;
        var batchId = $scope.batchId;
        var casualActivityId = $scope.casualActivityId;
        var action = $scope.action;
        var transactionSubTypeId = 10006;
       
        

        var selectedAction = "+";

        $http.get('/webapi/ActivityApi/GetAllActivities').success(function (data, status) {
            $scope.activities = data;
        });
           
       
        $http.get('/webapi/CasualWorkerApi/GetAllCasualWorkersForAParticularBranch').then(function (responses) {
            $scope.casualWorkers = responses.data;

        });
        
  
        $http.get('/webapi/SectorApi/GetAllSectors').success(function (data, status) {
            $scope.sectors = data;
        });

        if (action == 'create') {
            deliveryId = 0;
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
            var promise = $http.get('/webapi/CasualActivityApi/GetCasualActivity?casualActivityId=' + casualActivityId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;
                    $scope.casual = {
                        CasualActivityId: b.CasualActivityId,
                        Notes: b.Notes,
                        ActivityId : b.ActivityId,
                        TransactionSubTypeId: b.TransactionSubTypeId,
                        BranchId: b.BranchId,
                        Quantity: b.Quantity,
                        SectorId: b.SectorId,
                        CasualWorkerId: b.CasualWorkerId,
                        AspNetUserId: b.AspNetUserId,
                        Amount: b.Amount,
                        CreatedOn: b.CreatedOn,
                        TimeStamp: b.TimeStamp,
                        CreatedBy: b.CreatedBy,
                        Deleted: b.Deleted,
                        UpdatedBy: b.UpdatedBy,

                    };
                });


        }

    

     
        $scope.Save = function (casual) {
          
                $scope.showMessageSave = false;
                if ($scope.form.$valid) {
                    usSpinnerService.spin('global-spinner');
                    var promise = $http.post('/webapi/CasualActivityApi/Save', {

                        CasualActivityId: casualActivityId,
                        Amount: casual.Amount,
                        ActivityId: casual.ActivityId,
                        Quantity: casual.Quantity,
                        BranchId: casual.BranchId,
                        CasualWorkerId: casual.CasualWorkerId,
                        BatchId:batchId,
                        SectorId: casual.SectorId,
                        TransactionSubTypeId: transactionSubTypeId,
                        CreatedOn: casual.CreatedOn,
                        TimeStamp: casual.TimeStamp,
                        Action: selectedAction,
                        Notes: casual.Notes,
                        CreatedBy: casual.CreatedBy,
                        Deleted: casual.Deleted,

                    });

                    promise.then(
                        function (payload) {

                            casualActivityId = payload.data;
                            $scope.showMessageSave = true;
                            usSpinnerService.stop('global-spinner');
                            $timeout(function () {
                                $scope.showMessageSave = false;

                                if (action == "create") {
                                    $state.go('casualActivity-batch-edit', { 'action': 'edit', 'casualActivityId': casualActivityId,'batchId':batchId });
                                }

                            }, 1500);


                        });
            
           
            }

        }



        $scope.Cancel = function () {

            $state.go('casualActivities-list', { 'accountId': accountId });
            //$state.go('casualActivities-list');
        };

        $scope.Delete = function (casualActivityId) {
            $scope.showMessageDeleted = false;
            var promise = $http.get('/webapi/CasualActivityApi/Delete?casualActivityId=' + casualActivityId, {});
            promise.then(
                function (payload) {
                    $scope.showMessageDeleted = true;
                    $timeout(function () {
                        $scope.showMessageDeleted = false;
                        $scope.Cancel();
                    }, 2500);
                    $scope.showMessageDeleteFailed = false;
                },
                function (errorPayload) {
                    $scope.showMessageDeleteFailed = true;
                    $timeout(function () {
                        $scope.showMessageDeleteFailed = false;
                        $scope.Cancel();
                    }, 1500);
                });
        }


    }
    ]);


angular
    .module('homer').controller('AccountCasualActivityController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {

            var accountId = $scope.accountId;
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/CasualActivityApi/GetAllCasualActivitiesForAParticularCasualWorker?casualWorkerId=' + accountId, {});
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;
                   
                }
            );
            $scope.retrievedAccountId = $scope.accountId;
            $scope.gridData = {
                enableFiltering: true,
                columnDefs: $scope.columns,
                enableRowSelection: false
            };

            $scope.gridData.multiSelect = false;
              
            $scope.gridData.columnDefs = [
               
                { name: 'Activity', field: 'ActivityName' },
                {name:'Quantity',field:'Quantity'},
               {name:'Batch',field:'BatchNumber'},
               
                { name: 'Amount', field: 'Amount' },
                {name:'Branch',field:'BranchName'},
                 { name: 'Department', field: 'SectorName' },
                 {
                     name: 'CreatedOn', field: 'CreatedOn',
                     sort: {
                         direction: uiGridConstants.DESC,
                         priority: 1
                     }
                 },


            ];




        }]);



angular
    .module('homer').controller('BatchCasualActivityController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {

            var batchId = $scope.batchId;
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/CasualActivityApi/GetAllCasualActivitiesForAParticularBatch?batchId=' + batchId, {});
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;

                }
            );
            $scope.retrievedBatchId = $scope.batchId;
            $scope.gridData = {
                enableFiltering: true,
                columnDefs: $scope.columns,
                enableRowSelection: false
            };

            $scope.gridData.multiSelect = false;

            $scope.gridData.columnDefs = [

                { name: 'Activity', field: 'ActivityName' },
                { name: 'Quantity', field: 'Quantity' },
                { name: 'Amount', field: 'Amount' },
               { name: 'Batch', field: 'BatchNumber' },

                {name:'Worker',field:'CasualWorkerName'},
                { name: 'Branch', field: 'BranchName' },
                 { name: 'Department', field: 'SectorName' },
                 {
                     name: 'CreatedOn', field: 'CreatedOn',
                     sort: {
                         direction: uiGridConstants.DESC,
                         priority: 1
                     }
                 },


            ];




        }]);


