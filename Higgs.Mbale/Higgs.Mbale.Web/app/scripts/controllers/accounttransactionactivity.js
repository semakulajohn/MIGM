angular
    .module('homer')
    .controller('AccountTransactionActivityEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval,usSpinnerService) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }
        var accountId = $scope.accountId;
        var accountType = "";
        var transactionActivityId = $scope.transactionActivityId;
        var action = $scope.action;
        $scope.accountName = "";
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

        
         if (accountId.length < 6) {
              accountType = "number";

         }
         else {
              accountType = "string";
         }

         if (accountType == "string") {
             var promise = $http.get('/webapi/UserApi/GetUser?userId=' + accountId, {});
             promise.then(
                 function (payload) {
                     var c = payload.data;
                     $scope.accountName = c.FirstName + " " + c.LastName;
                 }

             );

         }
         else if (accountType == "number") {
             var promise = $http.get('/webapi/CasualWorkerApi/GetCasualWorker?casualWorkerId=' + parseInt(accountId), {});
             promise.then(
                 function (payload) {
                     var c = payload.data;
                     $scope.accountName = c.FirstName + " " + c.LastName;
                 }

             );
         }
        
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
            var promise = $http.get('/webapi/AccountTransactionActivityApi/GetAccountTransactionActivity?transactionActivityId=' + transactionActivityId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;
                    $scope.transactionActivity = {
                        AccountTransactionActivityId: b.AccountTransactionActivityId,
                        Notes: b.Notes,
                        Balance: b.Balance,
                        TransactionSubTypeId: b.TransactionSubTypeId,
                        BranchId: b.BranchId,
                        SectorId:b.SectorId,
                        CasualWorkerId: b.CasualWorkerId,
                        AspNetUserId : b.AspNetUserId,
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

        $scope.Save = function (transactionActivity) {
            $scope.showMessageSave = false;
            if ($scope.form.$valid) {
                usSpinnerService.spin('global-spinner');
                var promise = $http.post('/webapi/DepositApi/Save', {

                    AccountTransactionActivityId: transactionActivityId,
                    Amount: transactionActivity.Amount,
                    StartAmount: transactionActivity.StartAmount,
                    Balance: transactionActivity.Balance,
                    PaymentModeId : transactionActivity.PaymentModeId,
                    BranchId: transactionActivity.BranchId,
                    AspNetUserId: (accountType == "string") ? accountId : null,
                    CasualWorkerId: (accountType == "number") ? accountId : null,
                    SectorId : transactionActivity.SectorId,
                    Notes: transactionActivity.Notes,
                    Action: transactionActivity.Action,
                    TransactionSubTypeId: transactionActivity.TransactionSubTypeId,
                    CreatedOn: transactionActivity.CreatedOn,
                    TimeStamp: transactionActivity.TimeStamp,
                    CreatedBy: transactionActivity.CreatedBy,
                    Deleted: transactionActivity.Deleted,

                });

                promise.then(
                    function (payload) {

                        transactionActivityId = payload.data;
                        $scope.showMessageSave = true;
                        usSpinnerService.stop('global-spinner');
                        $timeout(function () {
                            $scope.showMessageSave = false;

                            if (action == "create") {
                                //$state.go('account-accounttransactionactivities-edit', { 'action': 'edit', 'accountId': accountId, 'transactionActivityId': transactionActivityId });
                                $state.go('account-accounttransactionactivities-list', { 'accountId': accountId });

                            }

                        }, 1500);


                    });
            }

        }



        $scope.Cancel = function () {
           
            $state.go('account-accounttransactionactivities-list', { 'accountId': accountId });
        };

        $scope.Delete = function (transactionActivityId) {
            $scope.showMessageDeleted = false;
            var promise = $http.get('/webapi/AccountTransactionActivityApi/Delete?transactionActivityId=' + transactionActivityId, {});
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
    .module('homer').controller('AccountTransactionActivityController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/AccountTransactionActivityApi/GetAllAccountTransactionActivities');
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
                { name: 'Credit', cellTemplate: '<div ng-if="row.entity.Action ==\'+\'">{{row.entity.Amount}}</div>', cellFilter: 'number'},

             
                { name: 'Balance', field: 'Balance', cellFilter: 'number' },


            ];




        }]);



angular
    .module('homer').controller('AccountAccountTransactionActivityController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {

            var accountId = $scope.accountId;
            $scope.accountName = "";
            $scope.loadingSpinner = true;

            if (accountId.length < 6) {
                var accountType = "number";

            }
            else {
                var accountType = "string";
            }
           
            if (accountType == "string") {
                var promise = $http.get('/webapi/UserApi/GetUser?userId=' + accountId, {});
                promise.then(
                    function (payload) {
                        var c = payload.data;
                        $scope.accountName = c.FirstName + " " + c.LastName;
                    }

                );

            }
            else if (accountType == "number") {
                var promise = $http.get('/webapi/CasualWorkerApi/GetCasualWorker?casualWorkerId=' + parseInt(accountId), {});
                promise.then(
                    function (payload) {
                        var c = payload.data;
                        $scope.accountName = c.FirstName + " " + c.LastName;
                    }

                );
            }

            var promise = $http.get('/webapi/AccountTransactionActivityApi/GetAllAccountTransactionActivitiesForAParticularAccount?accountId=' + accountId, {});
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;
                    $scope.Length = payload.data.length;
                    if ($scope.Length > 0) {

                        //var lastIndex = $scope.Length - 1;
                        //$scope.accountBalance = payload.data[lastIndex].Balance;

                        var firstIndex = 0;
                        $scope.accountBalance = payload.data[firstIndex].Balance;

                       
                    }
                    else {
                        $scope.accountBalance = 0;
                    }
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

            
                 
              

                 {
                     name: 'CreatedOn', field: 'CreatedOn',width : '15%',
                     sort: {
                         direction: uiGridConstants.ASC,
                         priority: 1
                     }
                 },
                
                
                { name: 'Notes', field: 'Notes',width:'30%' },
                //{ name: 'Start Amount', field: 'StartAmount' },
                //{ name: 'Action', field: 'Action' },
                { name: 'Debit', cellFilter: 'number', cellTemplate: '<div ng-if="row.entity.Action ==\'-\'">{{row.entity.Amount}}</div>' },
                { name: 'Credit', cellFilter: 'number', cellTemplate: '<div ng-if="row.entity.Action ==\'+\'">{{row.entity.Amount}}</div>' },

                //{ name: 'Amount', field: 'Amount' },
                { name: 'Balance', field: 'Balance', cellFilter: 'number' },
                

            ];




        }]);


angular
    .module('homer').controller('AccountDashBoardAccountTransactionActivityController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {

            var accountId = $scope.accountId;
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/AccountTransactionActivityApi/GetAllAccountTransactionActivitiesForAParticularAccount?accountId=' + accountId, {});
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;
                    $scope.Length = payload.data.length;
                    if ($scope.Length > 0) {

                        //var lastIndex = $scope.Length - 1;
                        //$scope.accountBalance = payload.data[lastIndex].Balance;


                        var firstIndex = 0;
                        $scope.accountBalance = payload.data[firstIndex].Balance;


                    }
                    else {
                        $scope.accountBalance = 0;
                    }
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

           
                { name: 'Balance', field: 'Balance', cellFilter: 'number'},

                  //{ name: 'Balance', field: 'Balance', cellFilter: 'currency' },


              


            ];




        }]);

angular
    .module('homer')
    .controller('AdvancedPaymentEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval, usSpinnerService) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }
        var accountId = $scope.accountId;
        var accountType = "";
        var transactionActivityId = $scope.transactionActivityId;
        var action = $scope.action;
        $scope.accountName = "";
        var selectedAction = "+";
        var transactionSubType = 10008;
        var department = 10003;
    

        $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
            $scope.branches = data;
        });


        if (accountId.length < 6) {
            accountType = "number";

        }
        else {
            accountType = "string";
        }

        if (accountType == "string") {
            var promise = $http.get('/webapi/UserApi/GetUser?userId=' + accountId, {});
            promise.then(
                function (payload) {
                    var c = payload.data;
                    $scope.accountName = c.FirstName + " " + c.LastName;
                }

            );

        }
       

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
            var promise = $http.get('/webapi/AccountTransactionActivityApi/GetAccountTransactionActivity?transactionActivityId=' + transactionActivityId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;
                    $scope.transactionActivity = {
                        AccountTransactionActivityId: b.AccountTransactionActivityId,
                        Notes: b.Notes,
                        Quantity: b.Quantity,
                        TransactionSubTypeId: transactionSubType,
                        BranchId: b.BranchId,
                        SectorId: department,
                        CasualWorkerId: b.CasualWorkerId,
                        AspNetUserId: b.AspNetUserId,
                        Amount: b.Amount,
                        Price: b.Price,
                        Action: selectedAction,
                        CreatedOn: b.CreatedOn,
                        TimeStamp: b.TimeStamp,
                        CreatedBy: b.CreatedBy,
                        Deleted: b.Deleted,
                        UpdatedBy: b.UpdatedBy,

                    };
                });


        }

        $scope.Save = function (transactionActivity) {
            $scope.showMessageSave = false;
            if ($scope.form.$valid) {
                usSpinnerService.spin('global-spinner');
                var promise = $http.post('/webapi/DepositApi/Save', {

                    AccountTransactionActivityId: transactionActivityId,
                    Amount: transactionActivity.Amount,
                    StartAmount: transactionActivity.StartAmount,
                    Balance: transactionActivity.Balance,
                    PaymentModeId: transactionActivity.PaymentModeId,
                    BranchId: transactionActivity.BranchId,
                    AspNetUserId: (accountType == "string") ? accountId : null,
                    CasualWorkerId: (accountType == "number") ? accountId : null,
                    SectorId: department,
                    Notes: transactionActivity.Notes,
                    Price: transactionActivity.Price,
                    Quantity : transactionActivity.Quantity,
                    Action: selectedAction,
                    TransactionSubTypeId: transactionSubType,
                    CreatedOn: transactionActivity.CreatedOn,
                    TimeStamp: transactionActivity.TimeStamp,
                    CreatedBy: transactionActivity.CreatedBy,
                    Deleted: transactionActivity.Deleted,

                });

                promise.then(
                    function (payload) {

                        transactionActivityId = payload.data;
                        $scope.showMessageSave = true;
                        usSpinnerService.stop('global-spinner');
                        $timeout(function () {
                            $scope.showMessageSave = false;

                            if (action == "create") {
                               
                                $state.go('account-advancedpayments-list', { 'accountId': accountId });

                            }

                        }, 1500);


                    });
            }

        }



        $scope.Cancel = function () {

            $state.go('account-accounttransactionactivities-list', { 'accountId': accountId });
        };

        $scope.Delete = function (transactionActivityId) {
            $scope.showMessageDeleted = false;
            var promise = $http.get('/webapi/AccountTransactionActivityApi/Delete?transactionActivityId=' + transactionActivityId, {});
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
    .module('homer').controller('AdvancedPaymentListController', ['$scope', '$http', 'uiGridConstants',
        function ($scope, $http, uiGridConstants) {
            $scope.loadingSpinner = true;

            var accountId = $scope.accountId;
            var transactionSubTypeId = 10008;
            $scope.accountName = "";
            $scope.loadingSpinner = true;

            if (accountId.length < 6) {
                var accountType = "number";

            }
            else {
                var accountType = "string";
            }

            if (accountType == "string") {
                var promise = $http.get('/webapi/UserApi/GetUser?userId=' + accountId, {});
                promise.then(
                    function (payload) {
                        var c = payload.data;
                        $scope.accountName = c.FirstName + " " + c.LastName;
                    }

                );

            }
           


            var promise = $http.get('/webapi/AccountTransactionActivityApi/GetAllAdvancedPaymentsForAParticularAspNetUser?accountId=' + accountId + '&transactionSubTypeId=' + transactionSubTypeId, {});
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

                
                //{ name: 'TransactionSubTypeName', field: 'TransactionSubTypeName' },
                { name: 'Notes', field: 'Notes', field:'Notes'},
                { name: 'Price', field: 'Price' },
                 { name: 'Quantity', field: 'Quantity' },

                { name: 'Amount', field: 'Amount', cellFilter: 'number' },
              
                 { name: 'Branch', field: 'BranchName' },
              
                  {
                      name: 'CreatedOn', field: 'CreatedOn',
                      sort: {
                          direction: uiGridConstants.ASC,
                          priority: 1
                      }
                  },
                   {
                       name: 'Print', cellTemplate: '<div class="ui-grid-cell-contents" ><a  href="/Excel/Document?documentId={{row.entity.DocumentId}}">Receipt</a></div>'
                   },

              
               

            ];




        }]);


angular
    .module('homer').controller('ApprovedTransactionsListController', ['$scope', '$http',  'uiGridConstants',
        function ($scope,  $http,  uiGridConstants) {
            $scope.loadingSpinner = true;

            var accountId = $scope.accountId;
            var transactionSubTypeId = 10008;
            $scope.accountName = "";
            $scope.loadingSpinner = true;

            if (accountId.length < 6) {
                var accountType = "number";

            }
            else {
                var accountType = "string";
            }

            if (accountType == "string") {
                var promise = $http.get('/webapi/UserApi/GetUser?userId=' + accountId, {});
                promise.then(
                    function (payload) {
                        var c = payload.data;
                        $scope.accountName = c.FirstName + " " + c.LastName;
                    }

                );

            }



            var promise = $http.get('/webapi/DepositApi/GetLatestTwentyApprovedDepositsForAParticularAccount?accountId=' + accountId, {});
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
                { name: 'Notes', field: 'Notes', field: 'Notes' },
                { name: 'Price', field: 'Price' },
                { name: 'Quantity', field: 'Quantity', cellFilter: 'number'},

                { name: 'Amount', field: 'Amount', cellFilter: 'number' },
                { name: 'Action', field: 'Action' },
                { name: 'Branch', field: 'BranchName' },

                {
                    name: 'CreatedOn', field: 'CreatedOn',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    }
                },
                {
                    name: 'Print', cellTemplate: '<div class="ui-grid-cell-contents" ><a  href="/Excel/Document?documentId={{row.entity.DocumentId}}">Receipt</a></div>'
                },




            ];




        }]);


angular
    .module('homer').controller('UnApprovedTransactionsListController', ['$scope',  '$http','uiGridConstants',
        function ($scope, $http, uiGridConstants) {
            $scope.loadingSpinner = true;

            var accountId = $scope.accountId;
            var transactionSubTypeId = 10008;
            $scope.accountName = "";
            $scope.loadingSpinner = true;

            if (accountId.length < 6) {
                var accountType = "number";

            }
            else {
                var accountType = "string";
            }

            if (accountType == "string") {
                var promise = $http.get('/webapi/UserApi/GetUser?userId=' + accountId, {});
                promise.then(
                    function (payload) {
                        var c = payload.data;
                        $scope.accountName = c.FirstName + " " + c.LastName;
                    }

                );

            }



            var promise = $http.get('/webapi/DepositApi/GetAllUnApprovedDepositsForAParticularAccount?accountId=' + accountId, {});
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
               
                { name: 'Notes', field: 'Notes' },
                { name: 'Price', field: 'Price' },
                { name: 'Quantity', field: 'Quantity', cellFilter: 'number' },

                { name: 'Amount', field: 'Amount', cellFilter: 'number' },
                { name: 'Action', field: 'Action' },
                { name: 'Branch', field: 'BranchName' },

                {
                    name: 'CreatedOn', field: 'CreatedOn',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    }
                },
               

                { name: 'Details', field: 'DepositId', width: '15%', cellTemplate: '<div class="ui-grid-cell-contents"><a href="#/deposits/{{row.entity.DepositId}}">Details</a></div>' },





            ];




        }]);


angular
    .module('homer').controller('RejectedTransactionsListController', ['$scope', '$http', 'uiGridConstants',
        function ($scope, $http, uiGridConstants) {
            $scope.loadingSpinner = true;

            var accountId = $scope.accountId;
            var transactionSubTypeId = 10008;
            $scope.accountName = "";
            $scope.loadingSpinner = true;

            if (accountId.length < 6) {
                var accountType = "number";

            }
            else {
                var accountType = "string";
            }

            if (accountType == "string") {
                var promise = $http.get('/webapi/UserApi/GetUser?userId=' + accountId, {});
                promise.then(
                    function (payload) {
                        var c = payload.data;
                        $scope.accountName = c.FirstName + " " + c.LastName;
                    }

                );

            }



            var promise = $http.get('/webapi/DepositApi/GetLatestTwentyRejectedDepositsForAParticularAccount?accountId=' + accountId, {});
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
                { name: 'Notes', field: 'Notes', field: 'Notes' },
                { name: 'Price', field: 'Price' },
                { name: 'Quantity', field: 'Quantity', cellFilter: 'number' },

                { name: 'Amount', field: 'Amount', cellFilter: 'number' },
                { name: 'Action', field: 'Action' },
                { name: 'Branch', field: 'BranchName' },

                {
                    name: 'CreatedOn', field: 'CreatedOn',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    }
                },
               




            ];




        }]);


angular
    .module('homer')
    .controller('DepositDetailController', ['$scope', '$http', '$timeout', '$state','usSpinnerService',
        function ($scope, $http, $timeout, $state,  usSpinnerService) {
            $scope.tab = {};
            if ($scope.defaultTab == 'dashboard') {
                $scope.tab.dashboard = true;
            }
            var accountId = $scope.accountId;
            var accountType = "";
            var depositId = $scope.depositId;
            var action = $scope.action;
            $scope.accountName = "";
           
            var transactionSubType = 10008;
            var department = 10003;
                                  

          

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

           
                var promise = $http.get('/webapi/DepositApi/GetDeposit?depositId=' + depositId, {});
                promise.then(
                    function (payload) {
                        var b = payload.data;
                        if (b.Accept != true && b.Reject != true) {
                            $scope.hideAcceptReject = true;
                        }
                        else if (b.Accept == true || b.Reject == true) {
                            $scope.hideAcceptReject = false;
                        }

                        $scope.deposit = {
                            DepositId: b.DepositId,
                            Notes: b.Notes,
                            Quantity: b.Quantity,
                            //TransactionSubTypeId: transactionSubType,
                            TransactionSubTypeId : b.TransactionSubTypeId,
                            BranchId: b.BranchId,
                            SectorId: department,
                            CasualWorkerId: b.CasualWorkerId,
                            AspNetUserId: b.AspNetUserId,
                            Amount: b.Amount,
                            Price: b.Price,
                            Action: b.Action,
                            CreatedOn: b.CreatedOn,
                            TimeStamp: b.TimeStamp,
                            CreatedBy: b.CreatedBy,
                            Deleted: b.Deleted,
                            UpdatedBy: b.UpdatedBy,

                        };
                    });


            

            $scope.Approve = function (deposit) {
                $scope.showMessageSave = false;
               
                    usSpinnerService.spin('global-spinner');
                var promise = $http.post('/webapi/DepositApi/ApproveOrRejectDeposit', {

                        DepositId: deposit.DepositId,
                        Amount: deposit.Amount,
                        StartAmount: deposit.StartAmount,
                        Balance: deposit.Balance,
                        PaymentModeId: deposit.PaymentModeId,
                        BranchId: deposit.BranchId,
                      //  AspNetUserId: (accountType == "string") ? accountId : null,
                        AspNetUserId: deposit.AspNetUserId,
                        CasualWorkerId: deposit.CasualWorkerId,
                        SectorId: deposit.SectorId,
                        Notes: deposit.Notes,
                        Price: deposit.Price,
                        Quantity: deposit.Quantity,
                        Action: deposit.Action,
                        TransactionSubTypeId: deposit.TransactionSubTypeId,
                        CreatedOn: deposit.CreatedOn,
                        TimeStamp: deposit.TimeStamp,
                        CreatedBy: deposit.CreatedBy,
                        Deleted: deposit.Deleted,
                        Approved: true,

                    });

                    promise.then(
                        function (payload) {

                            depositId = payload.data;
                            if (depositId == -1) {
                                $scope.showMessageCantApproveOrReject = true;
                                usSpinnerService.stop('global-spinner');
                                $timeout(function () {
                                    $scope.showMessageCantApproveOrReject = false;

                                    $state.go('account-advancedpayments-list', { 'accountId': accountId });



                                }, 4000);

                            }
                            else {
                                $scope.showMessageSave = true;
                                usSpinnerService.stop('global-spinner');
                                $timeout(function () {
                                    $scope.showMessageSave = false;

                                    $state.go('account-advancedpayments-list', { 'accountId': accountId });



                                }, 4000);

                            }
                        });
                

            }   

            $scope.Reject = function (deposit) {
                $scope.showMessageSave = false;

                usSpinnerService.spin('global-spinner');
                var promise = $http.post('/webapi/DepositApi/ApproveOrRejectDeposit', {

                    DepositId: deposit.DepositId,
                    Amount: deposit.Amount,
                    StartAmount: deposit.StartAmount,
                    Balance: deposit.Balance,
                    PaymentModeId: deposit.PaymentModeId,
                    BranchId: deposit.BranchId,
                    AspNetUserId: deposit.AspNetUserId,
                    CasualWorkerId: deposit.CasualWorkerId,
                    SectorId: deposit.SectorId,
                    Notes: deposit.Notes,
                    Price: deposit.Price,
                    Quantity: deposit.Quantity,
                    Action: deposit.Action,
                    TransactionSubTypeId: deposit.TransactionSubTypeId,
                    CreatedOn: deposit.CreatedOn,
                    TimeStamp: deposit.TimeStamp,
                    CreatedBy: deposit.CreatedBy,
                    Deleted: deposit.Deleted,
                    Approve: false,

                });

                promise.then(
                    function (payload) {

                        depositId = payload.data;
                        $scope.showMessageSave = true;
                        usSpinnerService.stop('global-spinner');
                        $timeout(function () {
                            $scope.showMessageSave = false;

                          

                                $state.go('account-advancedpayments-list', { 'accountId': accountId });

                            

                        }, 1500);


                    });


            }     

        }
    ]);



angular
    .module('homer').controller('ApprovedDepositsListController', ['$scope', '$http', 'uiGridConstants',
        function ($scope, $http, uiGridConstants) {
            $scope.loadingSpinner = true;

            
            $scope.loadingSpinner = true;

            var promise = $http.get('/webapi/DepositApi/GetLatestTwentyApprovedDeposits', {});
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
                { name: 'Account', field: 'AccountName' },
                { name: 'Notes', field: 'Notes' },
                { name: 'Price', field: 'Price' },
                { name: 'Quantity', field: 'Quantity', cellFilter: 'number' },

                { name: 'Amount', field: 'Amount', cellFilter: 'number' },
                { name: 'Action', field: 'Action' },
                { name: 'Branch', field: 'BranchName' },

                {
                    name: 'CreatedOn', field: 'CreatedOn',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    }
                },

                { name: 'Details', field: 'DepositId', width: '15%', cellTemplate: '<div class="ui-grid-cell-contents"><a href="#/deposits/{{row.entity.DepositId}}">Details</a></div>' },





            ];

        }]);


angular
    .module('homer').controller('UnApprovedDepositsListController', ['$scope', '$http', 'uiGridConstants',
        function ($scope, $http, uiGridConstants) {
            $scope.loadingSpinner = true;

          
           
            $scope.loadingSpinner = true;   
       
            var promise = $http.get('/webapi/DepositApi/GetLatestTwentyUnApprovedDeposits', {});
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
                { name: 'Account', field: 'AccountName' },
                { name: 'Notes', field: 'Notes'},
                { name: 'Price', field: 'Price' },
                { name: 'Quantity', field: 'Quantity', cellFilter: 'number' },
                { name: 'Action', field: 'Action' },
                { name: 'Amount', field: 'Amount', cellFilter: 'number' },

                { name: 'Branch', field: 'BranchName' },

                {
                    name: 'CreatedOn', field: 'CreatedOn',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    }
                },
                

                { name: 'Details', field: 'DepositId', width: '15%', cellTemplate: '<div class="ui-grid-cell-contents"><a href="#/deposits/{{row.entity.DepositId}}">Details</a></div>' },


            ];




        }]);


angular
    .module('homer').controller('RejectedDepositsListController', ['$scope', '$http', 'uiGridConstants',
        function ($scope, $http, uiGridConstants) {
            $scope.loadingSpinner = true;

            $scope.loadingSpinner = true;

            var promise = $http.get('/webapi/DepositApi/GetLatestTwentyRejectedDeposits', {});
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
                { name: 'Account', field: 'AccountName' },
                { name: 'Notes', field: 'Notes'},
                { name: 'Price', field: 'Price' },
                { name: 'Quantity', field: 'Quantity', cellFilter: 'number' },
                { name: 'Action', field: 'Action' },
                { name: 'Amount', field: 'Amount', cellFilter: 'number' },

                { name: 'Branch', field: 'BranchName' },

                {
                    name: 'CreatedOn', field: 'CreatedOn',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    }
                },
                {
                    name: 'Print', cellTemplate: '<div class="ui-grid-cell-contents" ><a  href="/Excel/Document?documentId={{row.entity.DocumentId}}">Receipt</a></div>'
                },




            ];




        }]);
