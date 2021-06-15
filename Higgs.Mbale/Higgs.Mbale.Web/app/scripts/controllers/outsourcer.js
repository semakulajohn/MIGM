angular
    .module('homer').controller('OutSourcerController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/OutSourcerApi/GetAllOutSourcers');
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
                    name: 'First Name', field: 'FirstName',

                },

                { name: 'LastName', field: 'LastName', width: '15%', },
               
                { name: 'PhoneNumber', field: 'PhoneNumber', width: '15%', },
                //{ name: 'Store', field: 'Id', width: '15%', cellTemplate: '<div class="ui-grid-cell-contents"><a href="#/supplies/{{row.entity.Id}}">Manage Supplies</a></div>' },

                { name: 'Account', field: 'Id', width: '15%', cellTemplate: '<div class="ui-grid-cell-contents"><a href="#/accounttransactionactivities/{{row.entity.Id}}">Manage Account</a></div>' },

            ];




        }]);