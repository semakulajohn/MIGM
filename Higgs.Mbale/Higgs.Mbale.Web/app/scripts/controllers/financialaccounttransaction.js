angular
    .module('homer')
    .controller('FinancialAccountTransactionEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval, usSpinnerService) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }
        
        
        var financialAccountTransactionId = $scope.financialAccountTransactionId;
        var financialAccountId = $scope.financialAccountId;
        
        var action = $scope.action;
        
        $scope.actions = ["+", "-"];

         if (action == 'create') {
            financialAccountTransactionId = 0;
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
            var promise = $http.get('/webapi/FinancialAccountTransactionApi/GetFinancialAccountTransaction?transactionActivityId=' + BankTransactionId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;
                    $scope.transactionActivity = {
                        FinancailAccountTransactionId: b.FinancialAccountTransactionId,
                        Notes: b.Notes,
                        Balance: b.Balance,
                                              
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

        $scope.Save = function (financialAccountTransaction) {
            $scope.showMessageSave = false;
            if ($scope.form.$valid) {
                usSpinnerService.spin('global-spinner');
                var promise = $http.post('/webapi/FinancialAccountTransactionApi/Save', {

                    FinancialAccountTransactionId: financialAccountTransactionId,
                    Amount: financialAccountTransaction.Amount,
                    StartAmount: financialAccountTransaction.StartAmount,
                    Balance: financialAccountTransaction.Balance,
                    FinancialAccountId : financialAccountId,
                    Notes: financialAccountTransaction.Notes,
                    Action: financialAccountTransaction.Action,
                    CreatedOn: financialAccountTransaction.CreatedOn,
                    TimeStamp: financialAccountTransaction.TimeStamp,
                    CreatedBy: financialAccountTransaction.CreatedBy,
                    Deleted: financialAccountTransaction.Deleted,

                });

                promise.then(
                    function (payload) {

                        financialAccountTransactionId = payload.data;
                        $scope.showMessageSave = true;
                        usSpinnerService.stop('global-spinner');
                        $timeout(function () {
                            $scope.showMessageSave = false;

                            if (action == "create") {
                                $state.go('financialAccountTransaction-list', { 'financialAccountId':financialAccountId });

                            }

                        }, 1500);


                    });
            }

        }



        $scope.Cancel = function () {

            $state.go('financialAccountTransaction-list', { 'financialAccountId': financialAccountId });
        };

        $scope.Delete = function (financialAccountTransactionId) {
            $scope.showMessageDeleted = false;
            var promise = $http.get('/webapi/FinancialAcountTransactionApi/Delete?financialAccountTransactionId=' + financialAccountTransactionId, {});
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
    .module('homer').controller('FinancialAccountTransactionController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {

            $scope.retrievedFinancialAccountId = $scope.financialAccountId;

        }]);

angular
    .module('homer').controller('FinancialAccountFinancialAccountTransactionController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            var financialAccountId = $scope.financialAccountId;


            var promise = $http.get('/webapi/FinancialAccountApi/GetFinancialAccount?financialAccountId=' + financialAccountId, {});
            promise.then(
                function (payload) {
                    var c = payload.data;
                    $scope.accountName = c.Name;
                }

            );
            var promise = $http.get('/webapi/FinancialAccountTransactionApi/GetLatestTwentyFinancialAccountTransactionsForAParticularFinancialAccount?financialAccountId=' + financialAccountId, {});
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

                { name: 'Debit', cellTemplate: '<div ng-if="row.entity.Action ==\'-\'">{{row.entity.Amount}}</div>', cellFilter: 'number' },
                { name: 'Credit', cellTemplate: '<div ng-if="row.entity.Action ==\'+\'">{{row.entity.Amount}}</div>', cellFilter: 'number' },


                { name: 'Balance', field: 'Balance', cellFilter: 'number' },


            ];





        }]);
