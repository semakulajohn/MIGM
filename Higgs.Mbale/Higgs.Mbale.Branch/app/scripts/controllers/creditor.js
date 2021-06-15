
angular
    .module('homer').controller('CreditorController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {

            $scope.totalCreditBalance = 0;
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/CreditorApi/GetAllCreditors');
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;

                    angular.forEach($scope.gridData.data, function (value, key) {
                        $scope.totalCreditBalance = value.CreditBalance + $scope.totalCreditBalance;
                        
                        });

                }
            );

            $scope.gridData = {
                enableFiltering: false,
                columnDefs: $scope.columns,
                enableRowSelection: false
            };

            $scope.gridData.multiSelect = false;

            $scope.gridData.columnDefs = [
                
                {
                    name: 'Supplier Number',field:'AccountUniqueNumber', width: '25%', cellTemplate: '<div class="ui-grid-cell-contents"><a href="#/accounttransactionactivities/{{row.entity.AspNetUserId}}">{{row.entity.AccountUniqueNumber}}</a></div>',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    }
                   
                },
                {
                    name: 'AccountName', field: 'AccountName'
                },
                {
                    name: 'Amount', field: 'CreditBalance', cellFilter: 'number'
                },
               // {name:'Department',field:'SectorName'},
              
               //{
               //    name: 'Branch', field: 'BranchName'
               //},
              
            
            ];


            $scope.printCreditors = function (creditors) {
                var innerContents = document.getElementById(creditors).innerHTML;
                var popupWinindow = window.open('', '_blank', 'width=600,height=700,scrollbars=no,menubar=no,toolbar=no,location=no,status=no,titlebar=no');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head><link rel="stylesheet" type="text/css" href="~/styles/style.css" /></head><body onload="window.print()">' + innerContents + '</html>');
                popupWinindow.document.close();
            }


        }]);



angular
    .module('homer').controller('BranchCreditorController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;

            var branchId = $scope.branchId;
            var promise = $http.get('/webapi/CreditorApi/GetAllBranchCreditors?branchId=' + branchId, {});
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
                    name: 'Supplier Number', field:'AccountUniqueNumber',
                    sort: {
                        direction: uiGridConstants.DESC,
                        priority: 1
                    }
                    
                },
                {
                    name: 'AccountName', field: 'AccountName'
                },
                {
                    name: 'Amount', field: 'CreditBalance', cellFilter: 'number'
                },
                //{ name: 'Department', field: 'SectorName' },
               //{
               //    name: 'CreatedOn', field: 'CreatedOn'
               //},
               {
                   name: 'Branch', field: 'BranchName'
               },
               

            ];




        }]);



angular
    .module('homer').controller('CreditorViewController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {

            $scope.totalCreditBalance = 0;
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/CreditorApi/GetCreditorView');
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;

                    angular.forEach($scope.gridData.data, function (value, key) {
                        $scope.totalCreditBalance = value.Amount + $scope.totalCreditBalance;

                    });

                }
            );

            $scope.gridData = {
                enableFiltering: false,
                columnDefs: $scope.columns,
                enableRowSelection: false
            };

            $scope.gridData.multiSelect = false;

            $scope.gridData.columnDefs = [

                {
                    name: 'Supplier Number', field: 'CreditorNumber', width: '25%', cellTemplate: '<div class="ui-grid-cell-contents"><a href="#/accounttransactionactivities/{{row.entity.Id}}">{{row.entity.CreditorNumber}}</a></div>',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    }

                },
                {
                    name: 'AccountName', field: 'CreditorName'
                },
                {
                    name: 'Amount', field: 'Amount', cellFilter: 'number'
                },
             

            ];


            $scope.printCreditors = function (creditors) {
                var innerContents = document.getElementById(creditors).innerHTML;
                var popupWinindow = window.open('', '_blank', 'width=600,height=700,scrollbars=no,menubar=no,toolbar=no,location=no,status=no,titlebar=no');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head><link rel="stylesheet" type="text/css" href="~/styles/style.css" /></head><body onload="window.print()">' + innerContents + '</html>');
                popupWinindow.document.close();
            }


        }]);


