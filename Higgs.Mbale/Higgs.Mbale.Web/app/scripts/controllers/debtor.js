
angular
    .module('homer').controller('DebtorController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {

            $scope.totalDebtBalance = 0;
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/DebtorApi/GetAllDebtors');
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;


                    angular.forEach($scope.gridData.data, function (value, key) {
                        $scope.totalDebtBalance = value.DebtBalance + $scope.totalDebtBalance;

                    });
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
                    name: 'AccountName', field: 'AccountName'
                },
                {
                    name: 'Amount', field: 'Amount', cellFilter: 'currency'
                },
               
               
               {
                   name: 'Branch', field: 'BranchName'
               },
              
            ];




        }]);


angular
    .module('homer').controller('BranchDebtorController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;

            var branchId = $scope.branchId;
            var promise = $http.get('/webapi/DebtorApi/GetAllBranchDebtors?branchId=' + branchId, {});
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
                    name: 'AccountName', field: 'AccountName'
                },
                {
                    name: 'Amount', field: 'DebtBalance', cellFilter: 'number'
                },
              

               {
                   name: 'Branch', field: 'BranchName'
               },
              
            ];




        }]);


angular
    .module('homer').controller('DebtorViewController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {

            $scope.totalDebtBalance = 0;
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/DebtorApi/GetDebtorView');
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;

                    angular.forEach($scope.gridData.data, function (value, key) {
                        $scope.totalDebtBalance = value.Amount + $scope.totalDebtBalance;

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
                    name: 'AccountName', field: 'DebtorName'
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
