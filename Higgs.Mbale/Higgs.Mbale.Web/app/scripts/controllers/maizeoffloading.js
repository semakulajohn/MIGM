angular
    .module('homer')
    .controller('MaizeOffloadingEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval, usSpinnerService) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }
        
        var maizeOffloadingId = $scope.maizeOffloadingId;
        var action = $scope.action;
        
        $scope.actions = ["+", "-"];

        $http.get('/webapi/TransactionSubTypeApi/GetAllTransactionSubTypes').success(function (data, status) {
            $scope.transactionSubTypes = data;
        });

        $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
            $scope.branches = data;
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
            var promise = $http.get('/webapi/MaizeOffloadingApi/GetMaizeOffloading?maizeOffloadingId=' + maizeOffloadingId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;
                    $scope.maizeOffloading = {
                        MaizeOffloadingId: b.MaizeOffloadingId,
                        Notes: b.Notes,
                        Balance: b.Balance,
                        TransactionSubTypeId: b.TransactionSubTypeId,
                        BranchId: b.BranchId,
                        SectorId: b.SectorId,
                        CasualWorkerId: b.CasualWorkerId,
                        AspNetUserId: b.AspNetUserId,
                        Amount: b.Amount,
                        StartAmount: b.StartAmount,
                        Action: b.Action,
                        CreatedOn: b.CreatedOn,
                        TimeStamp: b.TimeStamp,
                        CreatedBy: b.CreatedBy,
                        Deleted: b.Deleted,
                        UpdatedBy: b.UpdatedBy,
                        WeightNoteNumber: b.WeightNoteNumber,
                        SupplyId : b.SupplyId,

                    };
                });


        }

        $scope.Save = function (maizeOffloading) {
            $scope.showMessageSave = false;
            if ($scope.form.$valid) {
                usSpinnerService.spin('global-spinner');
                var promise = $http.post('/webapi/MaizeOffloadingApi/Save', {

                    MaizeOffloadingId: maizeOffloadingId,
                    Amount: maizeOffloading.Amount,
                    StartAmount: maizeOffloading.StartAmount,
                    Balance: maizeOffloading.Balance,
                    PaymentModeId: maizeOffloading.PaymentModeId,
                    BranchId: maizeOffloading.BranchId,
                    SectorId: maizeOffloading.SectorId,
                    Notes: maizeOffloading.Notes,
                    Action: maizeOffloading.Action,
                    TransactionSubTypeId: maizeOffloading.TransactionSubTypeId,
                    CreatedOn: maizeOffloading.CreatedOn,
                    TimeStamp: maizeOffloading.TimeStamp,
                    CreatedBy: maizeOffloading.CreatedBy,
                    Deleted: maizeOffloading.Deleted,
                    WeightNoteNumber: maizeOffloading.WeightNoteNumber,
                    SupplyId : maizeOffloading.SupplyId,

                });

                promise.then(
                    function (payload) {

                        maizeOffloadingId = payload.data;
                        $scope.showMessageSave = true;
                        usSpinnerService.stop('global-spinner');
                        $timeout(function () {
                            $scope.showMessageSave = false;

                            if (action == "create") {
                                $state.go('maizeOffloadings-list', { 'maizeOffloadingId': maizeOffloadingId });

                            }

                        }, 1500);


                    });
            }

        }



        $scope.Cancel = function () {

            $state.go('maizeOffloadings-list', { 'maizeOffloadingId': maizeOffloadingId });
        };

        $scope.Delete = function (maizeOffloadingId) {
            $scope.showMessageDeleted = false;
            var promise = $http.get('/webapi/MaizeOffloadingApi/Delete?maizeOffloadingId=' + maizeOffloadingId, {});
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
    .module('homer').controller('MaizeOffloadingController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/MaizeOffloadingApi/GetAllMaizeOffloadings');
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
                      name: 'CreatedOn', field: 'CreatedOn',
                      sort: {
                          direction: uiGridConstants.ASC,
                          priority: 1
                      }
                  },


                { name: 'Notes', field: 'Notes' },

                { name: 'Debit', cellTemplate: '<div ng-if="row.entity.Action ==\'-\'">{{row.entity.Amount}}</div>' },
                 { name: 'Credit', cellTemplate: '<div ng-if="row.entity.Action ==\'+\'">{{row.entity.Amount}}</div>' },


                 { name: 'Balance', field: 'Balance' },


            ];




        }]);



