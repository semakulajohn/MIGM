angular
    .module('homer')
    .controller('StoreEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval) {

        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }

        var storeId = $scope.storeId;
        var action = $scope.action;


      

        if (action == 'create') {
            storeId = 0;

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



            var promise = $http.get('/webapi/StoreApi/GetStore?storeId=' + storeId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;
                    
                    $scope.store = {
                        StoreId: b.StoreId,
                        Name: b.Name,
                        BranchId : b.BranchId,
                        TimeStamp: b.TimeStamp,
                        CreatedOn: b.CreatedOn,
                        CreatedBy: b.CreatedBy,
                        UpdatedBy: b.UpdatedBy,
                        Deleted: b.Deleted,


                    };

                });


        }

        $scope.Save = function (store) {

            $scope.showMessageSave = false;
            if ($scope.form.$valid) {
                $scope.loadingSpinner = true;
                var promise = $http.post('/webapi/StoreApi/Save', {
                    StoreId: storeId,
                    Name: store.Name,
                    BranchId : store.BranchId,
                    CreatedBy: store.CreatedBy,
                    CreatedOn: store.CreatedOn,
                    Location: store.Location,
                    Deleted: store.Deleted,

                });

                promise.then(
                    function (payload) {

                        storeId = payload.data;

                        $scope.showMessageSave = true;
                        $scope.loadingSpinner = false;
                        $timeout(function () {
                            $scope.showMessageSave = false;


                            if (action == "create") {
                                $state.go('store-edit', { 'action': 'edit', 'storeId': storeId });
                            }

                        }, 1500);


                    });
            }

        }



        $scope.Cancel = function () {
            $state.go('stores.list');

        };

        $scope.Delete = function (storeId) {
            $scope.showMessageDeleted = false;
            var promise = $http.get('/webapi/StoreApi/Delete?storeId=' + storeId, {});
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
    .module('homer').controller('BranchStoreController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            var branchId = $scope.branchId;
            var promise = $http.get('/webapi/StoreApi/GetAllBranchStores');
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;
                }
            );

            $scope.gridData = {
                enableFiltering: true,
                columnDefs: $scope.columns,
                enableRowSelection: true
            };

            $scope.gridData.multiSelect = true;

            $scope.gridData.columnDefs = [

                {
                    name: 'Name', field: 'Name',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    }
                },

               { name: 'Maize', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/maize/{{row.entity.StoreId}}">Maize</a> </div>' },

                  { name: 'Flour Stock', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/store/storeStanding/{{row.entity.StoreId}}">Flour Stock</a> </div>' },
                { name: 'Brand', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/brand/brandStanding/{{row.entity.BranchId}}">Brand </a> </div>' },

                  { name: 'Buvera Stock', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/store/storeBuveraStanding/{{row.entity.StoreId}}">Buvera</a> </div>' },

                   { name: 'Inventory', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/inventories/{{row.entity.StoreId}}">Other Inventories</a> </div>' },

                   {
                       name: 'Store Details', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/stores/edit/{{row.entity.StoreId}}">Store Details</a> </div>',

                   },

            ];




        }]);


angular
    .module('homer')
    .controller('StoreFlourStandingController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval) {

        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }

        var storeId = $scope.storeId;


        var promisestore = $http.get('/webapi/StoreApi/GetStore?storeId=' + storeId, {});
        promisestore.then(
            function (payload) {
                var b = payload.data;
                    
                $scope.store = {
                    StoreId: b.StoreId,
                    Name: b.Name,
                   
                };

            });


    

        var promise = $http.get('/webapi/StockApi/GetStoreFlourStock');
        promise.then(
            function (payload) {
                var b = payload.data;
                $scope.retrievedStoreId = $scope.storeId;
               
                  
                $scope.storeGradeSize = {
                                      
                    StoreSizeGrades: b.StoreSizeGrades,
                    
                };
               

            });

    }
    ]);



angular
    .module('homer')
    .controller('StoreBuveraStandingController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval) {

        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }

        var storeId = $scope.storeId;
        var promisestore = $http.get('/webapi/StoreApi/GetStore?storeId=' + storeId, {});
        promisestore.then(
            function (payload) {
                var b = payload.data;

                $scope.store = {
                    StoreId: b.StoreId,
                    Name: b.Name,

                };

            });

        var promise = $http.get('/webapi/BuveraApi/GetStoreBuveraStock');
        promise.then(
            function (payload) {
                var b = payload.data;

                $scope.storeGradeSize = {

                    StoreSizeGrades: b.StoreSizeGrades,

                };


            });

    }
    ]);