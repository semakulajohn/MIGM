angular
    .module('homer')
    .controller('OrderEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval,usSpinnerService) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }

        $scope.selectedGrades = [];

        var orderId = $scope.orderId;
        var action = $scope.action;
        var customerId = $scope.customerId;
        var statusId = "10002";

        $http.get('webapi/ProductApi/GetAllproducts').success(function (data, status) {
            $scope.products = data;
        });

        $http.get('webapi/GradeApi/GetAllGrades').success(function (data, status) {
            $scope.grades = data;
        });

        $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
            $scope.branches = data;
        });

        $http.get('/webapi/StatusApi/GetAllStatuses').success(function (data, status) {
            $scope.statuses = data;
        });

        if (action == 'create') {
            orderId = 0;
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
            var promise = $http.get('/webapi/OrderApi/GetOrder?orderId=' + orderId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;
                    $scope.order = {
                        OrderId: b.OrderId,
                        CustomerName: b.CustomerName,
                        Amount: b.Amount,
                        ProductId: b.ProductId,
                        BranchId: b.BranchId,
                        Price : b.Price,
                        StatusId: b.StatusId,
                        TimeStamp: b.TimeStamp,
                        CreatedOn: b.CreatedOn,
                        CreatedBy: b.CreatedBy,
                        UpdatedBy: b.UpdatedBy,
                        Deleted: b.Deleted,
                        CustomerId: b.CustomerId,
                        Grades: b.Grades
                    };
                });

        }

        $scope.Save = function (order) {
            $scope.showMessageSave = false;
            usSpinnerService.spin('global-spinner');
            if ($scope.form.$valid) {
                var promise = $http.post('/webapi/OrderApi/Save', {
                    OrderId: orderId,
                    Amount: order.Amount,
                    ProductId: order.ProductId,
                    BranchId: order.BranchId,
                    StatusId: statusId,
                    Price : order.Price,
                    Grades: orderId == 0 ? $scope.selectedGrades : order.Grades,
                    CustomerId : customerId
                });

                promise.then(
                    function (payload) {

                        orderId = payload.data;
                        $scope.showMessageSave = true;
                        usSpinnerService.stop('global-spinner');
                        $timeout(function () {
                            $scope.showMessageSave = false;

                            if (action == "create") {
                                $state.go('order-edit', { 'action': 'edit', 'customerId':customerId,'orderId': orderId });
                            }

                        }, 1500);

                    });
            }

        }


        $scope.Cancel = function () {
            $state.go('customerorders-list', {'customerId':customerId});
        };

        $scope.Delete = function (orderId) {
            $scope.showMessageDeleted = false;
            var promise = $http.get('/webapi/OrderApi/Delete?orderId=' + orderId, {});
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
    .module('homer').controller('OrderController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/OrderApi/GetAllOrders');
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

                //{
                //    name: 'Product Name', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/orders/edit/{{row.entity.OrderId}}">{{row.entity.ProductName}}</a> </div>'

                //},
                { name: 'Customer Name', field: 'CustomerName' },
                { name: 'Order Number', field: 'OrderId' },
                { name: 'Status', field: 'StatusName' },
                { name: 'Quantity', field: 'Amount' },
                { name: 'Price', field: 'Price' },
                { name: 'CreatedBy', field: 'CreatedBy' },
                { name: 'Branch Name', field: 'BranchName' },
                { name: 'Order Details', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/order/detail/{{row.entity.OrderId}}"> Order Detail</a> </div>' },
            { name: 'Delivery', field: 'Id', width: '15%', cellTemplate: '<div class="ui-grid-cell-contents"><a href="#/deliveries/{{row.entity.OrderId}}">Make Delivery</a></div>' },

            ];




        }]);


angular
    .module('homer')
    .controller('OrderDetailController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$state',
    function ($scope, $http, $filter, $location, $log, $timeout, $state) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }

        var orderId = $scope.orderId;

        var promise = $http.get('/webapi/OrderApi/GetOrder?orderId=' + orderId, {});
        promise.then(
            function (payload) {
                var b = payload.data;
                $scope.order = {
                    OrderId: b.OrderId,
                    CustomerName: b.CustomerName,
                    Amount: b.Amount,
                    ProductId: b.ProductId,
                    BranchId: b.BranchId,
                    StatusId: b.StatusId,
                    TimeStamp: b.TimeStamp,
                    CreatedOn: b.CreatedOn,
                    Price : b.Price,
                    CreatedBy: b.CreatedBy,
                    UpdatedBy: b.UpdatedBy,
                    Deleted: b.Deleted,
                    Grades: b.Grades,
                    StatusName: b.StatusName,
                    BranchName: b.BranchName,
                    ProductName: b.ProductName,
                    TotalQuantity: b.TotalQuantity
                };
            });

    }]);


angular
    .module('homer').controller('CustomerOrderController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;


            var customerId = $scope.customerId;
            $scope.accountName = "";
            $scope.loadingSpinner = true;

          
            var promise = $http.get('/webapi/UserApi/GetUser?userId=' + customerId, {});
                promise.then(
                    function (payload) {
                        var c = payload.data;
                        $scope.accountName = c.FirstName + " " + c.LastName;
                    }

                );

          
           
            var promise = $http.get('/webapi/OrderApi/GetAllOrdersForAParticularCustomer?customerId=' + customerId, {});
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
            $scope.retrievedCustomerId = customerId;
            $scope.deliveryId = 0;
            $scope.gridData.multiSelect = true;

            $scope.gridData.columnDefs = [

                //{
                //    name: 'Product Name', field:'ProductName'

                //},
                { name: 'Customer Name', field: 'CustomerName' },
                //{ name: 'Order Number', field: 'OrderId' },
                { name: 'Status', field: 'StatusName' },
                
                { name: 'Quantity', cellTemplate: '<div ng-if="row.entity.ProductId == 1 || row.entity.Amount == null">{{row.entity.TotalQuantity}}</div><div ng-if="row.entity.Amount != 0">{{row.entity.Amount}}</div>' },
                { name: 'Price', field: 'Price' },
                { name: 'Balance', cellTemplate: '<div ng-if="row.entity.ProductId == 1 || row.entity.Amount == null">{{row.entity.TotalQuantity}}</div><div ng-if="row.entity.Amount != 0 && row.entity.Amount != null">{{row.entity.Balance}}</div>' },
                { name: 'CreatedBy', field: 'CreatedBy' },
               { name: 'Branch Name', field: 'BranchName' },
                { name: 'Order Details', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/order/detail/{{row.entity.OrderId}}"> Order Detail</a> </div>' },
            { name: 'Delivery', field: 'Id', width: '15%', cellTemplate: '<div class="ui-grid-cell-contents"><a href="#/deliveries/edit/'+$scope.retrievedCustomerId+'/{{row.entity.OrderId}}/'+$scope.deliveryId+'">Make Delivery</a></div>' },

            {
                name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/orders/edit/' + $scope.retrievedCustomerId + '/{{row.entity.OrderId}}">Edit</a> </div>'

            },
            ];




        }]);


angular
    .module('homer').controller('CustomerOpenOrderController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            
            var customerId = $scope.customerId;
            $scope.accountName = "";
            $scope.loadingSpinner = true;


            var promise = $http.get('/webapi/UserApi/GetUser?userId=' + customerId, {});
            promise.then(
                function (payload) {
                    var c = payload.data;
                    $scope.accountName = c.FirstName + " " + c.LastName;
                }

            );
            var promise = $http.get('/webapi/OrderApi/GetAllOpenOrdersForAParticularCustomer?customerId=' + customerId, {});
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
            $scope.retrievedCustomerId = customerId;
            $scope.deliveryId = 0;
            $scope.gridData.multiSelect = true;

            $scope.gridData.columnDefs = [

                //{
                //    name: 'Product Name', field: 'ProductName'

                //},
                { name: 'Customer Name', field: 'CustomerName' },
                //{ name: 'Order Number', field: 'OrderId' },
                { name: 'Status', field: 'StatusName' },

                { name: 'Quantity', cellTemplate: '<div ng-if="row.entity.ProductId == 1 || row.entity.Amount == null">{{row.entity.TotalQuantity}}</div><div ng-if="row.entity.Amount != 0 && row.entity.Amount != null">{{row.entity.Amount}}</div>' },
                { name: 'Price', field: 'Price' },
                { name: 'Balance', cellTemplate: '<div ng-if="row.entity.ProductId == 1 || row.entity.Amount == null">{{row.entity.TotalQuantity}}</div><div ng-if="row.entity.Amount != 0 && row.entity.Amount != null">{{row.entity.Balance}}</div>' },
                { name: 'CreatedBy', field: 'CreatedBy' },
                { name: 'Branch Name', field: 'BranchName' },
                { name: 'Order Details', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/order/detail/{{row.entity.OrderId}}"> Order Detail</a> </div>' },
            { name: 'Delivery', field: 'Id', width: '15%', cellTemplate: '<div class="ui-grid-cell-contents"><a href="#/deliveries/edit/' + $scope.retrievedCustomerId + '/{{row.entity.OrderId}}/' + $scope.deliveryId + '">Make Delivery</a></div>' },
             
            {
                name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/orders/edit/' + $scope.retrievedCustomerId + '/{{row.entity.OrderId}}">Edit</a> </div>'

            },
            ];




        }]);

angular
    .module('homer').controller('BranchOrderController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            var branchId = $scope.branchId;
            var promise = $http.get('/webapi/OrderApi/GetAllOrdersForAparticularBranch?branchId=' + branchId, {});
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

                //{
                //    name: 'Product Name', field:'ProductName'

                //},
                { name: 'Customer Name', field: 'CustomerName' },
                //{ name: 'Order Number', field: 'OrderId' },
                { name: 'Status', field: 'StatusName' },
                { name: 'Quantity', field: 'Amount' },
                { name: 'Price', field: 'Price' },
                { name: 'CreatedBy', field: 'CreatedBy' },
                { name: 'Branch Name', field: 'BranchName' },
                { name: 'Order Details', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/order/detail/{{row.entity.OrderId}}"> Order Detail</a> </div>' },
            { name: 'Delivery', field: 'Id', width: '15%', cellTemplate: '<div class="ui-grid-cell-contents"><a href="#/deliveries/{{row.entity.OrderId}}">Delivery</a></div>' },

             {
                 name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/orders/edit/{{row.entity.OrderId}}">Edit</a> </div>'

             },
            ];




        }]);


angular
    .module('homer').controller('CustomerCompletedOrderController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
           

            var customerId = $scope.customerId;
            $scope.accountName = "";
            $scope.loadingSpinner = true;


            var promise = $http.get('/webapi/UserApi/GetUser?userId=' + customerId, {});
            promise.then(
                function (payload) {
                    var c = payload.data;
                    $scope.accountName = c.FirstName + " " + c.LastName;
                }

            );
            var promise = $http.get('/webapi/OrderApi/GetAllCompletedOrdersForAParticularCustomer?customerId=' + customerId, {});
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
            $scope.retrievedCustomerId = customerId;
            $scope.deliveryId = 0;
            $scope.gridData.multiSelect = false;

            $scope.gridData.columnDefs = [

                //{
                //    name: 'Product Name', field: 'ProductName'

                //},
                { name: 'Customer Name', field: 'CustomerName' },
                { name: 'Order Number', field: 'OrderId' },
                { name: 'Status', field: 'StatusName' },

                { name: 'Quantity', cellTemplate: '<div ng-if="row.entity.Amount == 0 || row.entity.Amount == null">{{row.entity.TotalQuantity}}</div><div ng-if="row.entity.Amount != 0">{{row.entity.Amount}}</div>' },
                { name: 'Price', field: 'Price' },
                { name: 'Balance', cellTemplate: '<div ng-if="row.entity.ProductId == 1 || row.entity.Amount == null">{{row.entity.TotalQuantity}}</div><div ng-if="row.entity.Amount != 0 && row.entity.Amount != null">{{row.entity.Balance}}</div>' },
                { name: 'CreatedBy', field: 'CreatedBy' },
               { name: 'Branch Name', field: 'BranchName' },
                { name: 'Order Details', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/order/detail/{{row.entity.OrderId}}"> Order Detail</a> </div>' },
            { name: 'Delivery', field: 'Id', width: '15%', cellTemplate: '<div class="ui-grid-cell-contents"><a href="#/deliveries/{{row.entity.OrderId}}"> Deliveries</a></div>' },

            ];




        }]);

angular
    .module('homer').controller('CustomerInProgressOrderController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            var customerId = $scope.customerId;

            $scope.accountName = "";
            $scope.loadingSpinner = true;


            var promise = $http.get('/webapi/UserApi/GetUser?userId=' + customerId, {});
            promise.then(
                function (payload) {
                    var c = payload.data;
                    $scope.accountName = c.FirstName + " " + c.LastName;
                }

            );
            var promise = $http.get('/webapi/OrderApi/GetAllInProgressOrdersForAParticularCustomer?customerId=' + customerId, {});
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
            $scope.retrievedCustomerId = customerId;
            $scope.deliveryId = 0;
            $scope.gridData.multiSelect = false;

            $scope.gridData.columnDefs = [

                //{
                //    name: 'Product Name', field: 'ProductName'

                //},
                { name: 'Customer Name', field: 'CustomerName' },
                //{ name: 'Order Number', field: 'OrderId' },
                { name: 'Status', field: 'StatusName' },

                { name: 'Quantity', cellTemplate: '<div ng-if="row.entity.Amount == 0 || row.entity.Amount == null">{{row.entity.TotalQuantity}}</div><div ng-if="row.entity.Amount != 0">{{row.entity.Amount}}</div>' },
                { name: 'Price', field: 'Price' },
                { name: 'Balance', cellTemplate: '<div ng-if="row.entity.ProductId == 1 || row.entity.Amount == null">{{row.entity.TotalQuantity}}</div><div ng-if="row.entity.Amount != 0 && row.entity.Amount != null">{{row.entity.Balance}}</div>' },
                { name: 'CreatedBy', field: 'CreatedBy' },
               { name: 'Branch Name', field: 'BranchName' },
                { name: 'Order Details', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/order/detail/{{row.entity.OrderId}}"> Order Detail</a> </div>' },
                 { name: 'Make Delivery', field: 'Id', width: '15%', cellTemplate: '<div class="ui-grid-cell-contents"><a href="#/deliveries/edit/' + $scope.retrievedCustomerId + '/{{row.entity.OrderId}}/' + $scope.deliveryId + '">Make Delivery</a></div>' },

            { name: 'Delivery', field: 'Id', width: '15%', cellTemplate: '<div class="ui-grid-cell-contents"><a href="#/deliveries/{{row.entity.OrderId}}"> Deliveries</a></div>' },

            ];




        }]);




angular
    .module('homer').controller('CustomerViewOpenOrderController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            var customerId = $scope.customerId;

            $scope.accountName = "";
            $scope.loadingSpinner = true;


            var promise = $http.get('/webapi/UserApi/GetUser?userId=' + customerId, {});
            promise.then(
                function (payload) {
                    var c = payload.data;
                    $scope.accountName = c.FirstName + " " + c.LastName;
                }

            );
            var promise = $http.get('/webapi/OrderApi/GetAllOpenOrdersForAParticularCustomer?customerId=' + customerId, {});
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
            $scope.retrievedCustomerId = customerId;
            $scope.deliveryId = 0;
            $scope.gridData.multiSelect = true;

            $scope.gridData.columnDefs = [

                {
                    name: 'Product Name', field: 'ProductName'

                },
                { name: 'Customer Name', field: 'CustomerName' },
                { name: 'Order Number', field: 'OrderId' },
                { name: 'Status', field: 'StatusName' },

                { name: 'Quantity', cellTemplate: '<div ng-if="row.entity.ProductId == 1 || row.entity.Amount == null">{{row.entity.TotalQuantity}}</div><div ng-if="row.entity.Amount != 0 && row.entity.Amount != null">{{row.entity.Amount}}</div>' },
                { name: 'Price', field: 'Price' },
                { name: 'Branch Name', field: 'BranchName' },
                { name: 'Order Details', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/order/detail/{{row.entity.OrderId}}"> Order Detail</a> </div>' },
           
            ];




        }]);

