
angular
    .module('homer')
    .controller('FlourTransferDetailController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$state','usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $state, usSpinnerService) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }
        $scope.hideAcceptReject = false;
       
        var flourTransferId = $scope.flourTransferId;
        var storeId = $scope.storeId;

        var promise = $http.get('/webapi/FlourTransferApi/GetFlourTransfer?flourTransferId=' + flourTransferId, {});
        promise.then(
            function (payload) {
                var b = payload.data;
                if((b.Accept != true && b.Reject != true) && b.FromSupplierStoreId != storeId){
                    $scope.hideAcceptReject = true;
                }
                else if((b.Accept == true || b.Reject == true) && b.FromSupplierStoreId != storeId){
                    $scope.hideAcceptReject = false;
                }
                $scope.flourTransfer = {
                    FlourTransferId: b.FlourTransferId,
                    BranchId: b.BranchId,
                    StoreId: b.StoreId,
                    ReceiverStoreName: b.ReceiverStoreName,
                    ToReceiverStoreId: b.ToReceiverStoreId,
                    FromSupplierStoreId : b.FromSupplierStoreId,
                    Accept: b.Accept,
                    Reject : b.Reject,
                    TimeStamp: b.TimeStamp,
                    CreatedOn: b.CreatedOn,
                    CreatedBy: b.CreatedBy,
                    UpdatedBy: b.UpdatedBy,
                    Deleted: b.Deleted,
                    Grades: b.Grades,
                    StoreName: b.StoreName,
                    BranchName: b.BranchName,
                    TotalQuantity: b.TotalQuantity,
                    Batches: b.Batches,
                    FlourTransferBatches : b.FlourTransferBatches,
                   
                };
            });


        $scope.Accept = function (flourTransfer) {        
            usSpinnerService.spin('global-spinner');

                var promise = $http.post('/webapi/FlourTransferApi/Accept', {
                    FlourTransferId: flourTransfer.FlourTransferId,
                    ToReceiverStoreId: flourTransfer.ToReceiverStoreId,
                    FromSupplierStoreId: flourTransfer.FromSupplierStoreId,
                    TotalQuantity: flourTransfer.TotalQuantity,
                    BranchId: flourTransfer.BranchId,
                    Grades: flourTransfer.Grades,
                    StoreId: flourTransfer.StoreId,
                    Batches: flourTransfer.Batches,
                    FlourTransferBatches : flourTransfer.FlourTransferBatches,

                });

                promise.then(
                    function (payload) {

                        flourTransferId = payload.data;                       
                        usSpinnerService.stop('global-spinner');
                        $timeout(function () {


                            $state.go('flourlist-store-transfer', { 'storeId': $scope.storeId });


                        }, 500);

                    });
            }

        $scope.Reject = function (flourTransfer) {

            usSpinnerService.spin('global-spinner');

            var promise = $http.post('/webapi/FlourTransferApi/Reject', {
                FlourTransferId: flourTransfer.FlourTransferId,
                ToReceiverStoreId: flourTransfer.ToReceiverStoreId,
                FromSupplierStoreId: flourTransfer.FromSupplierStoreId,
                TotalQuantity: flourTransfer.TotalQuantity,
                BranchId: flourTransfer.BranchId,
                Grades: flourTransfer.Grades,
                StoreId: flourTransfer.StoreId,
                Batches: flourTransfer.Batches,
                FlourTransferBatches : flourTransfer.FlourTransferBatches,

            });

            promise.then(
                function (payload) {

                    flourTransferId = payload.data;
                    usSpinnerService.stop('global-spinner');
                    $timeout(function () {


                        $state.go('flourlist-store-transfer', { 'storeId': $scope.storeId });


                    }, 500);
                });
        }



    }]);


angular
    .module('homer').controller('StoreFlourTransferController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
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

            var promise = $http.get('/webapi/FlourTransferApi/GetAllFlourTransfersForAparticularStore?storeId=' + storeId, {});
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
                    name: 'FlourTransferId', field: 'FlourTransferId'

                },
                { name: 'Quantity(kgs)', field: 'TotalQuantity' },
               
                { name: 'From Store', field: 'StoreName' },
                { name: 'Receiver Store', field: 'ReceiverStoreName' },

                { name: 'Branch Name', field: 'BranchName' },
                {
                    name: 'Date', field: 'CreatedOn', sort: {
                        direction: uiGridConstants.DESC,
                        priority: 1
                    }
                },
                { name: 'Accept', field: 'Accept' },
                { name: 'Reject', field: 'Reject' },
           
            {
                name: 'Transfer Details', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/flours/' + $scope.storeId + '/{{row.entity.FlourTransferId}}">Transfer Details</a> </div>',

            },

            ];




        }]);


angular
    .module('homer')
    .controller('StoreFlourTransferStandingController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval) {

        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }

        var storeId = $scope.storeId;




        var promise = $http.get('/webapi/FlourTransferApi/GetStoreFlourTransferStock?storeId=' + storeId, {});
        promise.then(
            function (payload) {
                var b = payload.data;

                $scope.retrievedStoreId = $scope.storeId;

                $scope.storeFlourTransferGradeSize = {

                    StoreFlourTransferSizeGrades: b.StoreFlourTransferSizeGrades,

                };


            });

    }
    ]);

angular
    .module('homer')
    .controller('FlourTransferIssueController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval, usSpinnerService) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }

        $scope.selectedGrades = [];
        $scope.batchGrades = [];
        var branches = [];
        var batchBranch;
        var selectedBranch;
        var flourTransferId = $scope.flourTransferId;
        var action = $scope.action;
        var storeId = $scope.storeId;
        var issuing = "YES";
        var storeBranchId = 0;
        var productId = 1;

        var promisestore = $http.get('/webapi/StoreApi/GetStore?storeId=' + storeId, {});
        promisestore.then(
            function (payload) {
                var b = payload.data;

                $scope.store = {
                    StoreId: b.StoreId,
                    Name: b.Name,
                    BranchId : b.BranchId,

                };
               
                $scope.batchBranch = {
                    BranchId: $scope.store.BranchId,
                    ProductId: productId,
                };
                
               
                $http.get('/webapi/BatchApi/GetAllBatchesForAParticularBranchToTransfer?branchId=' + $scope.store.BranchId + '&productId=' + productId).then(function (responses) {
                        $scope.batches = responses.data;

                });
               
            });

       

        $http.get('webapi/GradeApi/GetAllGrades').success(function (data, status) {
            $scope.grades = data;
        });

        $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
            $scope.bdata = {
                branches: data,
                selectedBranch: branches[0]
            }
        });

        $scope.OnBranchChange = function (flourTransfer) {
            var selectedBranchId = flourTransfer.BranchId
            $http.get('/webapi/StoreApi/GetAllBranchStores?branchId=' + selectedBranchId).then(function (responses) {
                $scope.stores = responses.data;

            });
        }      
        

        if (action == 'create') {
            flourTransferId = 0;
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
            var promise = $http.get('/webapi/FlourTransferApi/GetFlourTransfer?flourTransferId=' + flourTransferId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;
                    
                    $scope.flourTransfer = {
                        FlourTransferId: b.FlourTransferId,
                        TotalQuantity: b.TotalQuantity,
                        BranchId: b.BranchId,
                        StoreId: b.StoreId,
                        FromSupplierStoreId: b.FromSupplierStoreId,
                        ToReceiverStoreId: b.ToReceiverStoreId,
                        Issuing: b.Issuing,
                        TimeStamp: b.TimeStamp,
                        CreatedOn: b.CreatedOn,
                        CreatedBy: b.CreatedBy,
                        UpdatedBy: b.UpdatedBy,
                        Deleted: b.Deleted,

                        Grades: b.Grades,
                        Batches : b.Batches,
                    };
                });

        }
       
        $scope.Save = function (flourTransfer) {
           
            $scope.TotalGradeQuantities = 0;
            $scope.TotalBatchGradeQuantities = 0;
            $scope.denominationQuantities = 0;
            $scope.denominationBatchQuantities = 0;
            $scope.showMessageSave = false;
            usSpinnerService.spin('global-spinner');

            if (flourTransfer.Batches != null && flourTransfer.Batches != "undefined") {
                angular.forEach($scope.selectedGrades, function (value, key) {
                    var denominations = value.Denominations;
                    angular.forEach(denominations, function (denominations) {
                        $scope.denominationQuantities = parseFloat((denominations.Quantity) * (denominations.Value)) + parseFloat($scope.denominationQuantities);
                    });
                    $scope.TotalGradeQuantities = $scope.denominationQuantities;

                });
                angular.forEach($scope.batchGrades, function (value, key) {
                   var denominations = value.Denominations;
                    angular.forEach(denominations, function (denominations) {
                        $scope.denominationBatchQuantities = parseFloat((denominations.QuantityToRemove) * (denominations.Value)) + parseFloat($scope.denominationBatchQuantities);
                    });


                    $scope.TotalBatchGradeQuantities = $scope.denominationBatchQuantities;

                });
                if ($scope.TotalGradeQuantities == $scope.TotalBatchGradeQuantities) {
                    if ($scope.form.$valid) {
                        var promise = $http.post('/webapi/FlourTransferApi/Save', {
                            FlourTransferId: flourTransferId,
                            Issuing: issuing,
                            StoreId: storeId,
                            ToReceiverStoreId: flourTransfer.StoreId,
                            FromSupplierStoreId: storeId,
                            TotalQuantity: $scope.TotalGradeQuantities,
                            BranchId: flourTransfer.BranchId,
                            Grades: flourTransferId == 0 ? $scope.selectedGrades : flourTransfer.Grades,
                            Batches: flourTransfer.Batches,
                        });

                        promise.then(
                            function (payload) {

                                flourTransferId = payload.data;
                                if (flourTransferId > 0) {
                                    $scope.showMessageSave = true;
                                    usSpinnerService.stop('global-spinner');
                                    $timeout(function () {
                                        $scope.showMessageSave = false;

                                        if (action == "create") {
                                            $state.go('store-flourStanding', { 'storeId': $scope.storeId });
                                        }

                                    }, 2000);
                                }


                                else if (flourTransferId == -1) {
                                    $scope.showMessageQuantityNotEnough = true;
                                    usSpinnerService.stop('global-spinner');

                                    $timeout(function () {
                                        $scope.showMessageQuantityNotEnough = false;

                                    }, 4000);
                                }
                                else if (flourTransferId == -3) {
                                    $scope.showMessageSameStore = true;
                                    usSpinnerService.stop('global-spinner');

                                    $timeout(function () {
                                        $scope.showMessageSameStore = false;

                                    }, 4000);
                                }
                                else if (flourTransferId == -2) {
                                    $scope.showMessageGradeNotExist = true;
                                    usSpinnerService.stop('global-spinner');

                                    $timeout(function () {
                                        $scope.showMessageGradeNotExist = false;

                                    }, 4000);
                                }

                            });
                    }
                }
                else {
                    $scope.showMessageBatchandGradeQuantities = true;
                    usSpinnerService.stop('global-spinner');
                    $timeout(function () {
                        $scope.showMessageBatchandGradeQuantities = false;

                        if (action == "create" || action == "edit") {
                            $state.go('store-flourStanding', { 'storeId': $scope.storeId });
                        }

                    }, 4000);
                }
            }
            else {
               
                $scope.showMessageBatchDontExist = true;
                usSpinnerService.stop('global-spinner');
                $timeout(function () {
                    $scope.showMessageBatchDontExist = false;

                    if (action == "create" || action == "edit") {
                        $state.go('store-flourStanding', { 'storeId': $scope.storeId });
                    }

                }, 4000);
            }
      

        }


        $scope.Cancel = function () {
            $state.go('store-flourStanding', { 'storeId': $scope.storeId });
        };

       

    }
    ]);


angular
    .module('homer')
    .controller('FlourTransferTotalsController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$state', 'usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $state, usSpinnerService) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }
      
        $scope.status = ["Accepted", "Rejected"];

        $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
            $scope.branches = data;
        });


        $scope.SearchFlourTransfer = function (flourTransfer) {
            $scope.data = [];
          
            var promise = $http.post('/webapi/ReportApi/GetAllFlourTransferTotalsBetweenTheSpecifiedDates',
                    {
                        FromDate: flourTransfer.FromDate,
                        ToDate: flourTransfer.ToDate,
                        Status: flourTransfer.Status,
                        BranchId: flourTransfer.BranchId,

                    });
            promise.then(
             function (payload) {

                 $scope.data = payload.data.FlourTransfers;
                 $scope.flourTransfer = payload.data;

                 $scope.reportType = 4;

                 $scope.tableParams = new ngTableParams({
                     page: 1,
                     count: 10,
                     sorting: { CreatedOn: 'desc' }
                 }, {
                     getData: function ($defer, params) {
                         var filteredData = $filter('filter')($scope.data, $scope.filter);
                         var orderedData = params.sorting() ?
                                             $filter('orderBy')(filteredData, params.orderBy()) :
                                             filteredData;

                         params.total(orderedData.length);
                         $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                     },
                     $scope: $scope

                 });
             });
        }




    }]);

