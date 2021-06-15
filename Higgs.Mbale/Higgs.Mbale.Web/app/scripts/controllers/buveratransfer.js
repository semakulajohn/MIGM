
angular
    .module('homer')
    .controller('BuveraTransferDetailController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$state','usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $state, usSpinnerService) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }
        $scope.hideAcceptReject = false;

        var buveraTransferId = $scope.buveraTransferId;
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

      

        var promise = $http.get('/webapi/BuveraTransferApi/GetBuveraTransfer?buveraTransferId=' + buveraTransferId, {});
        promise.then(
            function (payload) {
                var b = payload.data;
                if ((b.Accept != true && b.Reject != true) && b.FromSupplierStoreId != storeId) {
                    $scope.hideAcceptReject = true;
                }
                else if ((b.Accept == true || b.Reject == true) && b.FromSupplierStoreId != storeId) {
                    $scope.hideAcceptReject = false;
                }
                $scope.buveraTransfer = {
                    BuveraTransferId: b.BuveraTransferId,
                    BranchId: b.BranchId,
                    StoreId: b.StoreId,
                    ProductId : b.ProductId,
                    ReceiverStoreName: b.ReceiverStoreName,
                    ToReceiverStoreId: b.ToReceiverStoreId,
                    FromSupplierStoreId: b.FromSupplierStoreId,
                    Accept: b.Accept,
                    Reject: b.Reject,
                    TimeStamp: b.TimeStamp,
                    CreatedOn: b.CreatedOn,
                    CreatedBy: b.CreatedBy,
                    UpdatedBy: b.UpdatedBy,
                    Deleted: b.Deleted,
                    Grades: b.Grades,
                    StoreName: b.StoreName,
                    BranchName: b.BranchName,
                    TotalQuantity: b.TotalQuantity,
                    ProductName : b.ProductName,

                };
            });


        $scope.Accept = function (buveraTransfer) {
            usSpinnerService.spin('global-spinner');


            var promise = $http.post('/webapi/BuveraTransferApi/Accept', {
                BuveraTransferId: buveraTransfer.BuveraTransferId,
                ToReceiverStoreId: buveraTransfer.ToReceiverStoreId,
                FromSupplierStoreId: buveraTransfer.FromSupplierStoreId,
                TotalQuantity: buveraTransfer.TotalQuantity,
                ProductId : buveraTransfer.ProductId,
                BranchId: buveraTransfer.BranchId,
                Grades: buveraTransfer.Grades,
                StoreId: buveraTransfer.StoreId,

            });

            promise.then(
                function (payload) {

                    buveraTransferId = payload.data;
                    usSpinnerService.stop('global-spinner');
                    $timeout(function () {
                       
                       
                            $state.go('buveralist-store-transfer', { 'storeId': $scope.storeId });
                        

                    }, 500);


                });
        }

        $scope.Reject = function (buveraTransfer) {
            usSpinnerService.spin('global-spinner');


            var promise = $http.post('/webapi/BuveraTransferApi/Reject', {
                BuveraTransferId: buveraTransfer.BuveraTransferId,
                ToReceiverStoreId: buveraTransfer.ToReceiverStoreId,
                FromSupplierStoreId: buveraTransfer.FromSupplierStoreId,
                TotalQuantity: buveraTransfer.TotalQuantity,
                BranchId: buveraTransfer.BranchId,
                Grades: buveraTransfer.Grades,
                StoreId: buveraTransfer.StoreId,
                ProductId : buveraTransfer.ProductId,

            });

            promise.then(
                function (payload) {

                    buveraTransferId = payload.data;
                    usSpinnerService.stop('global-spinner');
                    $timeout(function () {

                        $state.go('buveralist-store-transfer', { 'storeId': $scope.storeId });



                    }, 1500);

                });
        }



    }]);


angular
    .module('homer').controller('StoreBuveraTransferController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
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
            var promise = $http.get('/webapi/BuveraTransferApi/GetAllBuveraTransfersForAparticularStore?storeId=' + storeId, {});
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;
                }
            );

            $scope.retrievedStoreId = $scope.storeId;
            $scope.gridData = {
                enableFiltering: true,
                columnDefs: $scope.columns,
                enableRowSelection: false
            };

            $scope.gridData.multiSelect = false;


            $scope.gridData.columnDefs = [

                {
                    name: 'BuveraTransferId', field: 'BuveraTransferId'

                },
                { name: 'Quantity(kgs)', field: 'TotalQuantity' },

                { name: 'From Store', field: 'StoreName' },
                { name: 'Receiver Store', field: 'ReceiverStoreName' },

                { name: 'Branch Name', field: 'BranchName' },
                { name: 'Date', field: 'CreatedOn' },
                { name: 'Accept', field: 'Accept' },
                { name: 'Reject', field: 'Reject' },

            {
                name: 'Transfer Details', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/buvera/' + $scope.storeId + '/{{row.entity.BuveraTransferId}}">Transfer Details</a> </div>',

            },

            ];




        }]);


angular
    .module('homer')
    .controller('StoreBuveraTransferStandingController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval) {

        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }

        var storeId = $scope.storeId;




        var promise = $http.get('/webapi/BuveraTransferApi/GetStoreBuveraTransferStock?storeId=' + storeId, {});
        promise.then(
            function (payload) {
                var b = payload.data;

                $scope.retrievedStoreId = $scope.storeId;

                $scope.storeBuveraTransferGradeSize = {

                    StoreBuveraTransferSizeGrades: b.StoreBuveraTransferSizeGrades,

                };


            });

    }
    ]);

angular
    .module('homer')
    .controller('BuveraTransferIssueController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval, usSpinnerService) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }

        $scope.selectedGrades = [];
        var branches = [];
        var selectedBranch;
        var buveraTransferId = $scope.buveraTransferId;
        var action = $scope.action;
        var storeId = $scope.storeId;
        var issuing = "YES"
       // var buveraCategoryId = 

        var promisestore = $http.get('/webapi/StoreApi/GetStore?storeId=' + storeId, {});
        promisestore.then(
            function (payload) {
                var b = payload.data;

                $scope.store = {
                    StoreId: b.StoreId,
                    Name: b.Name,

                };

            });

        $http.get('webapi/GradeApi/GetAllGrades').success(function (data, status) {
            $scope.grades = data;
        });
        $http.get('webapi/ProductApi/GetAllProducts').success(function (data, status) {
            $scope.products = data;
        });

        $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
            $scope.bdata = {
                branches: data,
                selectedBranch: branches[0]
            }
        });

        $scope.OnBranchChange = function (BuveraTransfer) {
            var selectedBranchId = BuveraTransfer.BranchId
            $http.get('/webapi/StoreApi/GetAllBranchStores?branchId=' + selectedBranchId).then(function (responses) {
                $scope.stores = responses.data;

            });
        }


        if (action == 'create') {
            buveraTransferId = 0;
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
            var promise = $http.get('/webapi/BuveraTransferApi/GetBuveraTransfer?buveraTransferId=' + buveraTransferId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;

                    $scope.buveraTransfer = {
                        BuveraTransferId: b.BuveraTransferId,
                        TotalQuantity: b.TotalQuantity,
                        BranchId: b.BranchId,
                        StoreId: b.StoreId,
                        ProductId : b.ProductId,
                        FromSupplierStoreId: b.FromSupplierStoreId,
                        ToReceiverStoreId: b.ToReceiverStoreId,
                        Issuing: b.Issuing,
                        TimeStamp: b.TimeStamp,
                        CreatedOn: b.CreatedOn,
                        CreatedBy: b.CreatedBy,
                        UpdatedBy: b.UpdatedBy,
                        Deleted: b.Deleted,

                        Grades: b.Grades,
                      
                    };
                });

        }

        $scope.Save = function (buveraTransfer) {

            $scope.TotalGradeQuantities = 0;
            $scope.denominationQuantities = 0;
            $scope.showMessageSave = false;
            usSpinnerService.spin('global-spinner');


            angular.forEach($scope.selectedGrades, function (value, key) {
                var denominations = value.Denominations;
                angular.forEach(denominations, function (denominations) {
                    $scope.denominationQuantities = parseFloat(denominations.Quantity) + parseFloat($scope.denominationQuantities);
                });
                $scope.TotalGradeQuantities = $scope.denominationQuantities;

            });
            if ($scope.form.$valid) {
                var promise = $http.post('/webapi/BuveraTransferApi/Save', {
                    BuveraTransferId: buveraTransferId,
                    Issuing: issuing,
                    StoreId: storeId,
                    ToReceiverStoreId: buveraTransfer.StoreId,
                    ProductId : buveraTransfer.ProductId,
                    FromSupplierStoreId: storeId,
                    TotalQuantity: $scope.TotalGradeQuantities,
                    BranchId: buveraTransfer.BranchId,
                    Grades: buveraTransferId == 0 ? $scope.selectedGrades : buveraTransfer.Grades,
                    
                });

                promise.then(
                    function (payload) {

                        buveraTransferId = payload.data;
                        $scope.showMessageSave = true;
                        usSpinnerService.stop('global-spinner');
                        $timeout(function () {
                            $scope.showMessageSave = false;

                            if (action == "create") {
                                $state.go('buveralist-store-transfer', { 'storeId': $scope.storeId });

                            }

                        }, 1500);

                    });
            }

        }


        $scope.Cancel = function () {
            $state.go('store-buveraStanding', { 'storeId': $scope.storeId });
        };



    }
    ]);
