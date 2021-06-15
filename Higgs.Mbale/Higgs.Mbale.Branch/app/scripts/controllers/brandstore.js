
angular
    .module('homer')
    .controller('MaizeBrandStoreDashBoardController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$state', 'usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $state, usSpinnerService) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }

        var maizeBrandStoreId = $scope.maizeBrandStoreId;
        var storeId = $scope.storeId;


        

    }]);




angular
    .module('homer')
    .controller('MaizeBrandStoreEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval, usSpinnerService) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }

       
        var branchId = $scope.branchId;
        $scope.brandBalance = 0;
        var action = $scope.action;



        var promisebranch = $http.get('/webapi/BranchApi/GetBranch?branchId=' + branchId, {});
        promisebranch.then(
            function (payload) {
                var b = payload.data;

                $scope.branch = {
                    BranchId: b.BranchId,
                    Name: b.Name,

                };

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

       
        var promise = $http.get('/webapi/MaizeBrandStoreApi/GetBalanceForMaizeBrandStoreForABranch?branchId=' + branchId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;
                    $scope.brandBalance = b;
                });
        

    }
    ]);
