angular
    .module('homer')
    .controller('MillingChargeEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval, usSpinnerService) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }
        
        var millingChargeId = $scope.millingChargeId;
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
            var promise = $http.get('/webapi/MillingChargeApi/GetMillingCharge?millingChargeId=' + millingChargeId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;
                    $scope.millingCharge = {
                        MillingChargeId: b.MillingChargeId,
                        Notes: b.Notes,
                        Balance: b.Balance,
                        TransactionSubTypeId: b.TransactionSubTypeId,
                        BranchId: b.BranchId,
                        SectorId: b.SectorId,
                        Amount: b.Amount,
                        StartAmount: b.StartAmount,
                        Action: b.Action,
                        CreatedOn: b.CreatedOn,
                        TimeStamp: b.TimeStamp,
                        CreatedBy: b.CreatedBy,
                        Deleted: b.Deleted,
                        UpdatedBy: b.UpdatedBy,

                    };
                });


        }

        $scope.Save = function (millingCharge) {
            $scope.showMessageSave = false;
            if ($scope.form.$valid) {
                usSpinnerService.spin('global-spinner');
                var promise = $http.post('/webapi/MillingChargeApi/Save', {

                    MillingChargeId: millingChargeId,
                    Amount: millingCharge.Amount,
                    StartAmount: millingCharge.StartAmount,
                    Balance: millingCharge.Balance,
                    PaymentModeId: millingCharge.PaymentModeId,
                    BranchId: millingCharge.BranchId,
                    SectorId: millingCharge.SectorId,
                    Notes: millingCharge.Notes,
                    Action: millingCharge.Action,
                    TransactionSubTypeId: millingCharge.TransactionSubTypeId,
                    CreatedOn: millingCharge.CreatedOn,
                    TimeStamp: millingCharge.TimeStamp,
                    CreatedBy: millingCharge.CreatedBy,
                    Deleted: millingCharge.Deleted,

                });

                promise.then(
                    function (payload) {

                        millingChargeId = payload.data;
                        $scope.showMessageSave = true;
                        usSpinnerService.stop('global-spinner');
                        $timeout(function () {
                            $scope.showMessageSave = false;

                            if (action == "create") {
                               $state.go('millingCharges-list', { 'millingChargeId': millingChargeId });

                            }

                        }, 1500);


                    });
            }

        }



        $scope.Cancel = function () {

            $state.go('millingCharges-list', { 'millingChargeId': millingChargeId });
        };

        $scope.Delete = function (millingChargeId) {
            $scope.showMessageDeleted = false;
            var promise = $http.get('/webapi/MillingChargeApi/Delete?millingChargeId=' + millingChargeId, {});
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
    .module('homer').controller('MillingChargeController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/MillingChargeApi/GetAllMillingCharges');
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





