angular
    .module('homer')
    .controller('UtilityAccountEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval, usSpinnerService) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }
        
        var accountType = "";
        var utilityAccountId = $scope.utilityAccountId;
        var action = $scope.action;
        $scope.accountName = "";
        $scope.actions = ["+", "-"];

        $http.get('/webapi/UtilityAccountApi/GetAllUtilityCategories').success(function (data, status) {
            $scope.utilityCategories = data;
        });

        $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
            $scope.branches = data;
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
            var promise = $http.get('/webapi/UtilityAccountApi/GetUtilityAccount?utilityAccountId=' + utilityAccountId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;
                    $scope.utilityAccount = {
                        UtilityAccountId: b.UtilityAccountId,
                        Description: b.Description,
                        Balance: b.Balance,
                        UtilityCategoryId: b.UtilityCategoryId,
                        BranchId: b.BranchId,
                        //SectorId: b.SectorId,
                        Action : b.Action,
                        InvoiceNumber : b.InvoiceNumber,
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

        $scope.Save = function (utilityAccount) {
            $scope.showMessageSave = false;
            if ($scope.form.$valid) {
                usSpinnerService.spin('global-spinner');
                var promise = $http.post('/webapi/UtilityAccountApi/Save', {

                    UtilityAccountId: utilityAccountId,
                    Amount: utilityAccount.Amount,
                    StartAmount: utilityAccount.StartAmount,
                    Balance: utilityAccount.Balance,
                    InvoiceNumber : utilityAccount.InvoiceNumber,
                    BranchId: utilityAccount.BranchId,
                    //SectorId: transactionActivity.SectorId,
                    Description: utilityAccount.Description,
                    Action: utilityAccount.Action,
                    UtilityCategoryId: utilityAccount.UtilityCategoryId,
                    CreatedOn: utilityAccount.CreatedOn,
                    TimeStamp: utilityAccount.TimeStamp,
                    CreatedBy: utilityAccount.CreatedBy,
                    Deleted: utilityAccount.Deleted,

                });

                promise.then(
                    function (payload) {

                        utilityAccountId = payload.data;
                        $scope.showMessageSave = true;
                        usSpinnerService.stop('global-spinner');
                        $timeout(function () {
                            $scope.showMessageSave = false;

                            if (action == "create") {
                                $state.go('category-utility-view', {'branchId':utilityAccount.BranchId, 'categoryId': utilityAccount.UtilityCategoryId });
                            }

                        }, 1500);


                    });
            }

        }



        $scope.Cancel = function () {
            $state.go('category-utility-view', { 'branchId': utilityAccount.BranchId, 'categoryId': utilityAccount.UtilityCategoryId });

            //$state.go('utilityaccounts-list');
        };

        $scope.Delete = function (utilityAccountId) {
            $scope.showMessageDeleted = false;
            var promise = $http.get('/webapi/UtilityAccountApi/Delete?UtilityAccountId=' + utilityAccountId, {});
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
    .module('homer').controller('UtilityAccountController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;

            $scope.retrievedBranchId = $scope.branchId;
            var promise = $http.get('/webapi/UtilityAccountApi/GetAllUtilityAccounts');
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
                   name: 'UtilityName',field:'UtilityCategoryName'
               },
                  {
                      name: 'CreatedOn', field: 'CreatedOn',
                      sort: {
                          direction: uiGridConstants.ASC,
                          priority: 1
                      }
                  },

                  {name:'Invoice',field:'InvoiceNumber'},
                { name: 'Description', field: 'Description' },
                { name: 'Credit', cellTemplate: '<div ng-if="row.entity.Action ==\'+\'">{{row.entity.Amount}}</div>', cellFilter: 'number' },


                { name: 'Debit', cellTemplate: '<div ng-if="row.entity.Action ==\'-\'">{{row.entity.Amount}}</div>', cellFilter: 'number'},
               

                { name: 'Balance', field: 'Balance', cellFilter: 'number' },


            ];




        }]);



angular
    .module('homer').controller('UtilityAccountCategoryController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            var categoryId = $scope.categoryId;
            var branchId = $scope.branchId;
            var promise = $http.get('/webapi/UtilityAccountApi/GetLatestTwentyUtilityAccountsForAParticularBranchAndCategory?branchId=' + branchId + '&categoryId=' + categoryId, {});
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
                   name: 'UtilityName', field: 'UtilityCategoryName'
               },
                  {
                      name: 'CreatedOn', field: 'CreatedOn',
                      sort: {
                          direction: uiGridConstants.ASC,
                          priority: 1
                      }
                  },

                  { name: 'Invoice', field: 'InvoiceNumber' },
                  { name: 'Description', field: 'Description' },
                { name: 'Credit', cellTemplate: '<div ng-if="row.entity.Action ==\'+\'">{{row.entity.Amount}}</div>', cellFilter: 'number' },


                { name: 'Debit', cellTemplate: '<div ng-if="row.entity.Action ==\'-\'">{{row.entity.Amount}}</div>', cellFilter: 'number' },


                { name: 'Balance', field: 'Balance', cellFilter: 'number' },


            ];




        }]);


