//angular
//    .module('homer').controller('AgentLatestUnPaidCommissionController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
//        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
//            $scope.loadingSpinner = true;
//            //var promise = $http.get('/webapi/UserApi/GetLoggedInUser', {});
//            //promise.then(
//            //    function (payload) {
//            //        var c = payload.data;

//            //        $scope.user = {
//            //            UserName: c.UserName,
//            //            Id: c.Id,
//            //            FirstName: c.FirstName,
//            //            LastName: c.LastName,
//            //            UserRoles: c.UserRoles,
//            //            RoleName : c.RoleName,
//            //        };
//            //    }
//            //);
//            //if ($scope.user.RoleName == 'agent') {
//                var promise = $http.get('/webapi/AgentApi/GetLatestTenUnPaidAgentCommissions');

//                promise.then(
//                    function (payload) {
//                        $scope.gridData.data = payload.data;
//                        $scope.loadingSpinner = false;
//                    }
//                );

//                $scope.gridData = {
//                    enableFiltering: true,
//                    columnDefs: $scope.columns,
//                    enableRowSelection: false
//                };


//                $scope.gridData.multiSelect = true;
//                $scope.gridData.columnDefs = [


//                {
//                    name: 'Amount', field: 'CommissionAmount',
//                    sort: {
//                        direction: uiGridConstants.ASC,
//                        priority: 1
//                    }
//                },

//                { name: 'IsPaid', field: 'IsPaid' },

//                { name: 'Client', field: 'ClientName' },

//                { name: 'Service', field: 'ServiceName' },
//                ];

//                $scope.currentFocused = "";

          

//                $scope.gridData.onRegisterApi = function (gridApi) {
//                    $scope.gridApi = gridApi;
//                };
//            //}

//        }]);

//angular
//    .module('homer').controller('AgentRecentClientsController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
//        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
//            $scope.loadingSpinner = true;

//            //var promise = $http.get('/webapi/UserApi/GetLoggedInUser', {});
//            //promise.then(
//            //    function (payload) {
//            //        var c = payload.data;

//            //        $scope.user = {
//            //            UserName: c.UserName,
//            //            Id: c.Id,
//            //            FirstName: c.FirstName,
//            //            LastName: c.LastName,
//            //            UserRoles: c.UserRoles,
//            //            RoleName: c.RoleName,
//            //        };
//            //    }
//            //);
//            //if ($scope.user.RoleName == 'agent')
//            //{
//                var promise = $http.get('/webapi/AgentApi/GetAllRecentlyCreatedAgentClients');

//            promise.then(
//                function (payload) {
//                    $scope.gridData.data = payload.data;
//                    $scope.loadingSpinner = false;
//                }
//            );

//            $scope.gridData = {
//                enableFiltering: true,
//                columnDefs: $scope.columns,
//                enableRowSelection: true
//            };


//            $scope.gridData.multiSelect = true;
//            $scope.gridData.columnDefs = [

//                {
//                    name: 'Name',
//                    field: 'Name',
//                    sort: {
//                        direction: uiGridConstants.ASC,
//                        priority: 1
//                    }
//                },

//                { name: 'Description', field: 'Description' },
               
//            ];
//        //}

//        }]);



///**
//    * Data for Bar chart
//    */
//angular
//    .module('homer').controller('AgentGraphController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
//        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
//            $scope.loadingSpinner = true;

//            //var promise = $http.get('/webapi/UserApi/GetLoggedInUser', {});
//            //promise.then(
//            //    function (payload) {
//            //        var c = payload.data;

//            //        $scope.user = {
//            //            UserName: c.UserName,
//            //            Id: c.Id,
//            //            FirstName: c.FirstName,
//            //            LastName: c.LastName,
//            //            UserRoles: c.UserRoles,
//            //            RoleName : c.RoleName,
//            //        };
//            //    }
//            //);

//            $http.get('/webapi/ClientApi/GetAllServices').success(function (data, status) {
//                $scope.services = data;
//            });

//            //if ($scope.user.RoleName == 'agent') {

//                var promise = $http.get('/webapi/CommissionApi/GetGraphData');
//                promise.then(
//                    function (payload) {
//                        $scope.graphData = payload.data;

//                        $scope.barData = {

                      
//                            labels: ["Web Hosting", "SMS", "KayeDex", "Web Development"],

//                            datasets: [
//                                {
//                                    label: "My Fourth dataset",
//                                    fillColor: "rgba(98,203,49,0.5)",
//                                    strokeColor: "rgba(98,203,49,0.8)",
//                                    highlightFill: "rgba(98,203,49,0.75)",
//                                    highlightStroke: "rgba(98,203,49,1)",
//                                    data: [$scope.graphData[0].TotalCommission, $scope.graphData[1].TotalCommission, $scope.graphData[2].TotalCommission,$scope.graphData[3].TotalCommission]
                                    
//                                },
                               

//                            ]

//                        };

//                    }
//                );

//            //}          

//        }]);


////angular
//    .module('homer').controller('LatestUnPaidCommissionController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
//        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
//            $scope.loadingSpinner = true;
           
//                var promise = $http.get('/webapi/CommissionApi/GetLatestTwentyUnPaidCommissions');

//                promise.then(
//                    function (payload) {
//                        $scope.gridData.data = payload.data;
//                        $scope.loadingSpinner = false;
//                    }
//                );

//                $scope.gridData = {
//                    enableFiltering: true,
//                    columnDefs: $scope.columns,
//                    enableRowSelection: false
//                };


//                $scope.gridData.multiSelect = true;
//                $scope.gridData.columnDefs = [


//                {
//                    name: 'Amount', field: 'CommissionAmount',
//                    sort: {
//                        direction: uiGridConstants.ASC,
//                        priority: 1
//                    }
//                },

//                { name: 'IsPaid', field: 'IsPaid' },

//                { name: 'Client', field: 'ClientName' },

//                { name: 'Service', field: 'ServiceName' },
//                ];

//                $scope.currentFocused = "";

            
//                $scope.gridData.onRegisterApi = function (gridApi) {
//                    $scope.gridApi = gridApi;
//                };
            

//        }]);



angular
    .module('homer').controller('DashBoardNotificationController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {

            $scope.countOfCashTransfers = 0;
            $scope.countOfSupplies = 0;
            $scope.countOfDeliveries = 0;
            $scope.countOfOutSourcerOutPuts = 0;
            $scope.countOfTransactions = 0;
            $scope.countOfRequistions = 0;
            var promise = $http.get('/webapi/DashBoardNotificationApi/GetDashBoardNotifications');
            promise.then(
                function (payload) {
                    var p = payload.data;
                    $scope.countOfCashTransfers = p.cashtransfers;
                    $scope.countOfSupplies = p.supplies;
                    $scope.countOfDeliveries = p.deliveries;
                    $scope.countOfOutSourcerOutPuts = p.outsourceroutputs;
                    $scope.countOfTransactions = p.transactions;
                    $scope.countOfRequistions = p.requistions;
                }
            );





        }]);
