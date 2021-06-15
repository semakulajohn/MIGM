

angular
    .module('homer').controller('StockController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/StockApi/GetAllStocks');
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
                    name: 'Product', field:'ProductName'

                },
                { name: 'Batch Number', field: 'BatchNumber' },
                { name: 'In/Out', field: 'StockInOrOut' },
                { name: 'Branch Name', field: 'BranchName' },
                {name:'SoldOutOrNot',field:'SoldOutOrNot'},
                { name: 'Stock Details', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/stock/detail/{{row.entity.StockId}}">Stock Details</a> </div>' },
            

            ];




        }]);


angular
    .module('homer')
    .controller('StockDetailController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$state',
    function ($scope, $http, $filter, $location, $log, $timeout, $state) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }

        var stockId = $scope.stockId;

        var promise = $http.get('/webapi/StockApi/GetStock?stockId=' + stockId, {});
        promise.then(
            function (payload) {
                var b = payload.data;
                $scope.stock = {
                    StockId: b.StockId,
                    BatchNumber: b.BatchNumber,
                    ProductId: b.ProductId,
                    BranchId: b.BranchId,
                    SectorId : b.SectorId,
                    SectorName : b.SectorName,
                    TimeStamp: b.TimeStamp,
                    CreatedOn: b.CreatedOn,
                    CreatedBy: b.CreatedBy,
                    UpdatedBy: b.UpdatedBy,
                    Deleted: b.Deleted,
                    Grades: b.Grades,
                    InOrOut: b.InOrOut,
                    SoldOutOrNot : b.SoldOutOrNot,
                    BranchName: b.BranchName,
                    ProductName: b.ProductName,
                    ProductOutPut : b.ProductOutPut
                };
            });

    }]);


angular
    .module('homer').controller('StoreStockController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;

            var storeId = $scope.storeId;
            
            var promise = $http.get('/webapi/StockApi/GetAllStocksForAparticularStore?storeId=' + storeId, {});
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
             name: 'Product', field: 'ProductName',width:'8%'
         },
         { name: 'Batch', field: 'BatchNumber', width:'7%' },
       
         { name: 'Start Stock(kgs)', field: 'StartStock' },
           { name: 'In/Out', field: 'StockInOrOut',width:'8%'},

         {name:'Quantity(kgs)',field:'Quantity'},
         { name: 'End Stock(kgs)', field: 'StockBalance' },
         //{ name: 'Sold Amount(kgs)', field: 'SoldAmount' },
         //{name:'Balance On Stock(kgs)',field:'Balance'},
        //{ name: 'SoldOutOrNot', field: 'SoldOutOrNot' },
        //{ name: 'SoldOutOrNot', cellTemplate: '<div ng-if="row.entity.SoldOut"><style="background-color:Red;"></style>{{row.entity.SoldOutOrNot}}</div><div ng-if="!row.entity.SoldOut"><style="background-color:Green;"></style>{{row.entity.SoldOutOrNot}}</div>' },
               
        {
            name: 'CreatedOn', field: 'TimeStamp',
            sort: {
                direction: uiGridConstants.DESC,
                priority: 1
            }

        }
            ];




        }]);



angular
    .module('homer')
    .controller('FlourTransferController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval, usSpinnerService) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }

        $scope.selectedGrades = [];
        var branches = [];
        var selectedBranch;
        var stockId = $scope.stockId;
        var action = $scope.action;
        var storeId = $scope.storeId;
        var issuing = "YES"

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

        $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
            $scope.bdata = {
                branches: data,
                selectedBranch: branches[0]
            }
        });

        $scope.OnBranchChange = function (stock) {
            var selectedBranchId = buvera.BranchId
            $http.get('/webapi/StoreApi/GetAllBranchStores?branchId=' + selectedBranchId).then(function (responses) {
                $scope.stores = responses.data;

            });
        }


        if (action == 'edit') {

            var promise = $http.get('/webapi/BatchOutPutApi/GetBatchOutPut?batchOutPutId=' + batchOutPutId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;

                    $scope.batchOutPut = {
                        BatchId: b.BatchId,
                        Loss: b.Loss,
                        BrandPercentage: b.BrandPercentage,
                        FlourPercentage: b.FlourPercentage,
                        LossPercentage: b.LossPercentage,
                        BrandOutPut: b.BrandOutPut,
                        FlourOutPut: b.FlourOutPut,
                        BranchId: b.BranchId,
                        StoreId: b.StoreId,
                        SectorId: b.SectorId,
                        TimeStamp: b.TimeStamp,
                        CreatedOn: b.CreatedOn,
                        CreatedBy: b.CreatedBy,
                        UpdatedBy: b.UpdatedBy,
                        Deleted: b.Deleted,
                        Grades: b.Grades

                    };

                });


        }



        $scope.Save = function (flour) {

            $scope.TotalGradeKgs = 0;
            $scope.DenominationKgs = 0;
            $scope.showMessageSave = false;          

                if ($scope.form.$valid) {
                    usSpinnerService.spin('global-spinner');
                    var promise = $http.post('/webapi/StockApi/Save', {
                        StockId: stockId,
                        Issuing: issuing,
                        StoreId: storeId,
                        ToReceiver: stock.StoreId,
                        TotalQuantity: $scope.TotalGradeQuantities,
                        BranchId: stock.BranchId,
                        Grades: stockId == 0 ? $scope.selectedGrades : stock.Grades,
                        
                        
                        StoreId: stock.StoreId,

                        Grades: stockId == 0 ? $scope.selectedGrades : stock.Grades
                    });

                    promise.then(
                        function (payload) {

                            stockId = payload.data;

                            $scope.showMessageSave = true;
                            usSpinnerService.stop('global-spinner');

                            $timeout(function () {
                                $scope.showMessageSave = false;

                                if (action == "create") {
                                    $state.go('stock-edit', { 'action': 'edit', 'stockId': stockId});
                                }

                            }, 1500);


                        });
                }
         
           

        }




        $scope.Cancel = function () {
            $state.go('batchoutput-batch', { 'batchId': batchId });
        };

       


      

    }
    ]);
