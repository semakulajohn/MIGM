angular
    .module('homer')
    .controller('InventoryEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval,usSpinnerService) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }

       
        var inventoryId = $scope.inventoryId;
        var action = $scope.action;
           


        $http.get('/webapi/InventoryApi/GetAllInventoryCategories').success(function (data, status) {
            $scope.categories = data;
        });

      
        if (action == 'create') {
            inventoryId = 0;
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
            var promise = $http.get('/webapi/InventoryApi/GetInventory?inventoryId=' + inventoryId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;
                    $scope.inventory = {
                        InventoryId: b.InventoryId,
                        ItemName: b.ItemName,
                       
                       
                        Price: b.Price,
                       
                        InventoryCategoryId : b.InventoryCategoryId,
                        Description: b.Description,
                       
                        TimeStamp: b.TimeStamp,
                        CreatedOn: b.CreatedOn,
                        CreatedBy: b.CreatedBy,
                        UpdatedBy: b.UpdatedBy,
                        Deleted: b.Deleted
                    };
                });


        }

        $scope.Save = function (inventory) {
            $scope.showMessageSave = false;
            usSpinnerService.spin('global-spinner');
            if ($scope.form.$valid) {
                var promise = $http.post('/webapi/InventoryApi/Save', {
                    InventoryId: inventoryId,
                    ItemName: inventory.ItemName,
                                     
                    Description: inventory.Description,
                   
                    Price: inventory.Price,
                   
                    InventoryCategoryId : inventory.InventoryCategoryId,
                    CreatedBy: inventory.CreatedBy,
                    CreatedOn: inventory.CreatedOn,
                    Deleted: inventory.Deleted
                });

                promise.then(
                    function (payload) {

                        inventoryId = payload.data;
                        $scope.showMessageSave = true;
                        usSpinnerService.stop('global-spinner');
                        $timeout(function () {
                            $scope.showMessageSave = false;

                            if (action == "create") {
                                $state.go('inventory-store-edit', { 'action': 'edit', 'inventoryId': inventoryId ,'storeId':storeId });
                            }

                        }, 1500);


                    });
            }

        }



        $scope.Cancel = function () {
            $state.go('inventory-store', { 'storeId': storeId });
        };

        $scope.Delete = function (inventoryId) {
            $scope.showMessageDeleted = false;
            var promise = $http.get('/webapi/InventoryApi/Delete?inventoryId=' + inventoryId, {});
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
    .module('homer').controller('InventoryController', ['$scope', '$http','uiGridConstants',
        function ($scope, $http, uiGridConstants) {
            $scope.loadingSpinner = true;
            var inventoryCategoryId = $scope.inventoryCategoryId;
            //var promise = $http.get('/webapi/InventoryApi/GetAllInventoriesForAParticularInventoryCategory?categoryId=' + inventoryCategoryId);
            //  promise.then(
            //      function (payload) {
            //          $scope.gridData.data = payload.data;
            //          $scope.loadingSpinner = false;
            //      }
            //  );
            var promise = $http.get('/webapi/InventoryApi/GetAllInventories');
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;
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
                    name: 'Id', field:'InventoryId',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    },
                    width:'5%'
                },
                { name: 'Item Name', field: 'ItemName',width:'10%' },
                { name: 'Description', field: 'Description' },
               { name: 'Price', field: 'Price' },
   
              
               {namet : 'Category',field:'CategoryName'},
              
               
                   {
                       name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/inventories/edit/{{row.entity.InventoryId}}">Edit</a> </div>',
                     
                   },

            ];




        }]);



angular
    .module('homer').controller('StoreInventoryController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;

            var storeId = $scope.storeId;
            
            var promise = $http.get('/webapi/InventoryApi/GetAllInventoriesForParticularStore?storeId=' + storeId, {});
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;
                }
            );
            $scope.retrievedStoreId = $scope.storeId;

            $scope.gridData = {
                enableFiltering: false,
                columnDefs: $scope.columns,
                enableRowSelection: false
            };

            $scope.gridData.multiSelect = false;

            $scope.gridData.columnDefs = [

                {
                    name: 'Id', field:'InventoryId',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    },
                    width: '5%'
                },
                { name: 'Item Name', field: 'ItemName', width: '10%' },
                { name: 'Description', field: 'Description' },
                   { name: 'Price', field: 'Price' },
             
              
                {name: 'Category',field:'CategoryName'},
              
                  {
                      name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/inventories/edit/{{row.entity.InventoryId}}">Edit</a> </div>',

                  },

            ];




        }]);

angular
    .module('homer')
    .controller('InventoryPurchaseEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'usSpinnerService',
        function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval, usSpinnerService) {
            $scope.tab = {};
            if ($scope.defaultTab == 'dashboard') {
                $scope.tab.dashboard = true;
            }


            var inventoryPurchaseId = $scope.inventoryPurchaseId;
            var action = $scope.action;



            $http.get('/webapi/InventoryApi/GetAllInventories').success(function (data, status) {
                $scope.categories = data;
            });


            if (action == 'create') {
                inventoryId = 0;
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
                var promise = $http.get('/webapi/InventoryApi/GetInventoryPurchase?inventoryPurchaseId=' + inventoryPurchaseId, {});
                promise.then(
                    function (payload) {
                        var b = payload.data;
                        $scope.inventory = {
                            InventoryId: b.InventoryId,
                            InventoryPurchaseId: b.InventoryPurchaseId,

                            StoreId: b.StoreId,
                            BranchId: b.BranchId,
                            PurchaseDate : b.PurchaseDate,
                            Price: b.Price,
                            Quantity : b.Quantity,
                            
                            Description: b.Description,

                            TimeStamp: b.TimeStamp,
                            CreatedOn: b.CreatedOn,
                            CreatedBy: b.CreatedBy,
                            UpdatedBy: b.UpdatedBy,
                            Deleted: b.Deleted
                        };
                    });


            }

            $scope.Save = function (inventory) {
                $scope.showMessageSave = false;
                usSpinnerService.spin('global-spinner');
                if ($scope.form.$valid) {
                    var promise = $http.post('/webapi/InventoryApi/Save', {
                        InventoryId: inventoryPurchase.InventoryId,
                        ItemName: inventoryPurchase.ItemName,
                        StoreId : inventoryPurchase.StoreId,
                        Description: inventoryPurchase.Description,
                        BranchId : inventoryPurchase.BranchId,
                        Price: inventoryPurchase.Price,
                        Quantity : inventoryPurchase.Quantity,
                        InventoryPurchaseId: inventoryPurchaseId,
                        CreatedBy: inventoryPurchase.CreatedBy,
                        CreatedOn: inventoryPurchase.CreatedOn,
                        Deleted: inventoryPurchase.Deleted
                    });

                    promise.then(
                        function (payload) {

                            inventoryPurchaseId = payload.data;
                            $scope.showMessageSave = true;
                            usSpinnerService.stop('global-spinner');
                            $timeout(function () {
                                $scope.showMessageSave = false;

                                if (action == "create") {
                                    $state.go('inventory-store', { 'action': 'edit', 'inventoryId': inventoryId, 'storeId': storeId });
                                }

                            }, 1500);


                        });
                }

            }



            $scope.Cancel = function () {
                $state.go('inventory-store', { 'storeId': storeId });
            };

            $scope.Delete = function (inventoryId) {
                $scope.showMessageDeleted = false;
                var promise = $http.get('/webapi/InventoryApi/Delete?inventoryId=' + inventoryId, {});
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
    .module('homer').controller('StoreInventoryPurchaseController', ['$scope', 'ngTableParams', '$http', '$filter', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;

            var storeId = $scope.storeId;

            var promise = $http.get('/webapi/InventoryPurchaseApi/GetAllInventoryPurchasesForParticularStore?storeId=' + storeId, {});
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;
                }
            );
            $scope.retrievedStoreId = $scope.storeId;

            $scope.gridData = {
                enableFiltering: false,
                columnDefs: $scope.columns,
                enableRowSelection: false
            };

            $scope.gridData.multiSelect = false;

            $scope.gridData.columnDefs = [

                {
                    name: 'Id', field: 'InventoryId',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    },
                    width: '5%'
                },
                { name: 'Item Name', field: 'InventoryName', width: '10%' },
                { name: 'Description', field: 'Description' },
                { name: 'Price', field: 'Price' },
                {name:'Quantity',field:'Quantity'},

                { name: 'Amount', field: 'Amount' },
                { name: 'PurchaseDate', field: 'PurchaseDate' },
               

            ];




        }]);
