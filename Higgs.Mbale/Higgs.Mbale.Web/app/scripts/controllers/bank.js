angular
    .module('homer')
    .controller('BankEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval, usSpinnerService) {

        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }
        $scope.isDisabled = false;
        var bankId = $scope.bankId;
        var action = $scope.action;




        if (action == 'create') {
            bankId = 0;

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



            var promise = $http.get('/webapi/BankApi/GetBank?bankId=' + bankId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;

                    $scope.bank = {
                        BankId: b.BankId,
                        Name: b.Name,
                        AccountNumber : b.AccountNumber,
                        TimeStamp: b.TimeStamp,
                        
                        CreatedOn: b.CreatedOn,
                        CreatedBy: b.CreatedBy,
                        UpdatedBy: b.UpdatedBy,
                        Deleted: b.Deleted,


                    };

                });


        }

        $scope.Save = function (bank) {
            $scope.isDisabled = true;
            $scope.showMessageSave = false;
            if ($scope.form.$valid) {
                usSpinnerService.spin('global-spinner');
                var promise = $http.post('/webapi/BankApi/Save', {
                    BankId: bankId,
                    Name: bank.Name,
                   
                    CreatedBy: bank.CreatedBy,
                    CreatedOn: bank.CreatedOn,
                    AccountNumber: bank.AccountNumber,
                    Deleted: bank.Deleted,

                });

                promise.then(
                    function (payload) {

                        bankId = payload.data;
                        usSpinnerService.stop('global-spinner');
                        $scope.showMessageSave = true;

                        $timeout(function () {
                            $scope.showMessageSave = false;


                          
                                $state.go('banks.list');
                            

                        }, 1500);


                    });
            }

        }



        $scope.Cancel = function () {
            $state.go('banks.list');

        };

        $scope.Delete = function (bankId) {
            $scope.showMessageDeleted = false;
            var promise = $http.get('/webapi/BankApi/Delete?bankId=' + bankId, {});
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
    .module('homer').controller('BankController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/BankApi/GetAllBanks');
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
                    name: 'Name', field: 'Name',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    }
                },

                { name: 'Account Number', field: 'AccountNumber' },

              {
                     name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/banks/edit/{{row.entity.BankId}}">Edit</a> </div>',

                 },


            ];




        }]);





