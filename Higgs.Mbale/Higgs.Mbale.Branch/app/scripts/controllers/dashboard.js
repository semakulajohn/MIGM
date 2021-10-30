
angular
    .module('homer').controller('DashBoardNotificationController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {

            $scope.countOfCashTransfers = 0;
           
            $scope.countOfBuveraTransfers = 0;
            $scope.countOfFlourTransfers = 0;
            var promise = $http.get('/webapi/DashBoardNotificationApi/GetDashBoardNotificationsForBranch');
            promise.then(
                function (payload) {
                    var p = payload.data;
                    $scope.countOfCashTransfers = p.cashtransfers;
                    
                    $scope.countOfBuveraTransfers = p.buveratransfers;
                    $scope.countOfFlourTransfers = p.flourtransfers;
                }
            );





        }]);



angular
    .module('homer').controller('BranchDashBoardNotificationController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {

            $scope.countOfCashTransfers = 0;
            $scope.countOfRequistions = 0;
            $scope.countOfOrders = 0;
            var promise = $http.get('/webapi/BranchDashBoardNotificationApi/GetBranchDashBoardNotification');
            promise.then(
                function (payload) {
                    var p = payload.data;
                    $scope.countOfCashTransfers = p.cashtransfers;
                    $scope.countOfRequistions = p.requistions;
                    $scope.countOfOrders = p.orders;
                }
            );





        }]);
