angular
    .module('homer')
    .controller('UtilityEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval, usSpinnerService) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }


        var utilityId = $scope.utilityId;
        var action = $scope.action;
        var batchId = $scope.batchId;



        $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
            $scope.branches = data;
        });

        $http.get('/webapi/SectorApi/GetAllSectors').success(function (data, status) {
            $scope.sectors = data;
        });


        if (action == 'create') {
            inventoryId = 0;
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
            var promise = $http.get('/webapi/UtilityApi/GetUtility?utilityId=' + utilityId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;
                    $scope.utility = {
                        UtilityId: b.UtilityId,
                        BatchId: b.BatchId,
                        Amount: b.Amount,
                        BatchId: b.BatchId,
                        Description: b.Description,
                        BranchId: b.BranchId,
                        SectorId: b.SectorId,
                        TimeStamp: b.TimeStamp,
                        CreatedOn: b.CreatedOn,
                        CreatedBy: b.CreatedBy,
                        UpdatedBy: b.UpdatedBy,
                        Deleted: b.Deleted
                    };
                });


        }

        $scope.Save = function (utility) {
            $scope.showMessageSave = false;
            usSpinnerService.spin('global-spinner');
            if ($scope.form.$valid) {
                var promise = $http.post('/webapi/UtilityApi/Save', {
                    UtilityId: utilityId,
                    Amount: utility.Amount,

                    Description: utility.Description,
                    BranchId: utility.BranchId,
                    BatchId: batchId,
                    SectorId: utility.SectorId,
                    CreatedBy: utility.CreatedBy,
                    CreatedOn: utility.CreatedOn,
                    Deleted: utility.Deleted
                });

                promise.then(
                    function (payload) {

                        utilityId = payload.data;
                        $scope.showMessageSave = true;
                        usSpinnerService.stop('global-spinner');
                        $timeout(function () {
                            $scope.showMessageSave = false;

                            if (action == "create") {
                                $state.go('utility-batch-edit', { 'action': 'edit', 'utilityId': utilityId, 'batchId': batchId });
                            }

                        }, 1500);


                    });
            }

        }



        $scope.Cancel = function () {
            $state.go('utility-batch', { 'batchId': batchId });
        };

        $scope.Delete = function (utilityId) {
            $scope.showMessageDeleted = false;
            var promise = $http.get('/webapi/UtilityApi/Delete?utilityId=' + utilityId, {});
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
    .module('homer').controller('BatchUtilityController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;

            var batchId = $scope.batchId;

            var promise = $http.get('/webapi/UtilityApi/GetAllUtilitiesForAParticularBatch?batchId=' + batchId, {});
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;
                }
            );
            $scope.retrievedBatchId = $scope.batchId;

            $scope.gridData = {
                enableFiltering: false,
                columnDefs: $scope.columns,
                enableRowSelection: false
            };

            $scope.gridData.multiSelect = false;

            $scope.gridData.columnDefs = [

                {
                    name: 'Id', field: 'UtilityId',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    },
                    width: '5%'
                },
                { name: 'Description', field: 'Description' },

                { name: 'Amount', field: 'Amount' },
                { name: 'CreatedOn', field: 'CreatedOn' },
                { name: 'Branch Name', field: 'BranchName' },
                 { name: 'Department', field: 'SectorName' },
                 {
                     name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/utilities/edit/{{row.entity.UtilityId}}/' + $scope.batchId + '">Edit</a> </div>',

                 },

            ];




        }]);

