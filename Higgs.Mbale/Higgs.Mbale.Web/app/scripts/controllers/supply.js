angular
    .module('homer')
    .controller('SupplyEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval,usSpinnerService) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }

        $scope.showMessageWeightNoteExists = false;

        var supplierId = $scope.supplierId;
        var supplyId = $scope.supplyId;
        var action = $scope.action;
        $scope.SupplierName = "";
        $scope.offloadingOptions = ["YES", "NO"];
        $scope.weightNoteNumbers = [];
       
        $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
            $scope.branches = data;
        });

        $scope.OnBranchChange = function (supply) {
            var selectedBranchId = supply.BranchId

            $http.get('/webapi/StoreApi/GetAllBranchStores?branchId=' + selectedBranchId).then(function (responses) {
                $scope.stores = responses.data;

            });
            $http.get('/webapi/SupplyApi/GetLatestFiftyNotUsedWeightNoteValuesForAParticularBranch?branchId='+ selectedBranchId).success(function (data, status) {
                $scope.weightNoteNumbers = data;
            });
           
        }
       

        //$http.get('/webapi/StoreApi/GetAllStores').success(function (data, status) {
        //    $scope.stores = data;
        //});
        var promise = $http.get('/webapi/UserApi/GetUser?userId=' + supplierId, {});
        promise.then(
            function (payload) {
                var c = payload.data;
                $scope.SupplierName = c.FirstName + " " + c.LastName;
            }

        );

         if (action == 'create') {
            supplyId = 0;
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
            var promise = $http.get('/webapi/SupplyApi/GetSupply?supplyId=' + supplyId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;
                    $scope.supply = {
                        SupplyId : b.SupplyId,
                        Quantity : b.Quantity,
                        SupplyDate: b.SupplyDate != null ? moment(b.SupplyDate).format('YYYY-MM-DD HH:mm:ss') : null,
                        BranchId : b.BranchId,
                        SupplierId :b.SupplierId,
                        Amount: b.Amount,
                        Used: b.Used,
                        StoreId : b.StoreId,
                        MoistureContent: b.MoistureContent,
                        WeightNoteNumber: b.WeightNoteNumber,
                       // WeightNoteNumbers : weightNoteNumbers,
                        NormalBags: b.NormalBags,
                        BagsOfStones: b.BagsOfStones,
                        TruckNumber : b.TruckNumber,
                        Price: b.Price,
                        YellowBags : b.YellowBags,
                        Offloading : b.Offloading,
                        CreatedOn :b.CreatedOn,
                        TimeStamp : b.TimeStamp,
                        CreatedBy : b.CreatedBy,
                        Deleted  : b.Deleted, 
                        UpdatedBy: b.UpdatedBy,
                        Approved: b.Approved,

                    };
                });


        }

        $scope.Save = function (supply) {
            $scope.showMessageSave = false;
           
            if ($scope.form.$valid) {
                usSpinnerService.spin('global-spinner');
                var promise = $http.post('/webapi/SupplyApi/Save', {

                    SupplyId  : supplyId,
                    Quantity : supply.Quantity,
                    SupplyDate : supply.SupplyDate,
                    BranchId : supply.BranchId,
                    SupplierId : supplierId,
                    Amount: supply.Amount,
                    StoreId : supply.StoreId,
                    MoistureContent: supply.MoistureContent,
                    WeightNoteNumber: supply.WeightNoteNumber,
                    NormalBags: supply.NormalBags,
                    BagsOfStones: supply.BagsOfStones,
                    TruckNumber : supply.TruckNumber,
                    Price: supply.Price,
                    Offloading : supply.Offloading,
                    CreatedOn : supply.CreatedOn,
                    TimeStamp : supply.TimeStamp,
                    CreatedBy : supply.CreatedBy,
                    Deleted: supply.Deleted,
                    YellowBags: supply.YellowBags,
                    Approved: supply.Approved,
                    WeightNoteNumbers : supply.WeightNoteNumbers,
                   
                });

                promise.then(
                    function (payload) {

                        supplyId = payload.data;
                        if (supplyId == -1) {
                            usSpinnerService.stop('global-spinner');
                            $scope.showMessageWeightNoteExists = true;

                            $timeout(function () {
                                $scope.showMessageWeightNoteExists = false;

                            }, 2000);
                        }
                        else {
                            $scope.showMessageSave = true;
                            usSpinnerService.stop('global-spinner');
                            $timeout(function () {
                                $scope.showMessageSave = false;

                                if (action == "create") {
                                    $state.go('supplier-supply-list', { 'supplierId': supplierId });
                                }

                            }, 1500);

                        }
                       

                    });
            }

        }

     
     


        $scope.Cancel = function () {
            $state.go('supplier-supply-list', { 'supplierId': supplierId});
           // $state.go('supplies.list');
        };

        $scope.Delete = function (supplyId) {
            $scope.showMessageDeleted = false;
            var promise = $http.get('/webapi/SupplyApi/Delete?supplyId=' + supplyId, {});
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
    .module('homer').controller('SupplyController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/SupplyApi/GetAllSupplies');
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
                    name: 'SupplyDate', field: 'SupplyDate', type: 'date', cellFilter: 'date:\'yy-MM-dd\'',
                    sort: {
                        direction: uiGridConstants.DESC,
                        priority: 1
                    }
                },
             
                { name: 'Truck', field: 'TruckNumber' },
                { name: 'Supplier Name',field:'SupplierName'},
                { name: 'Branch Name', field: 'BranchName' },
                { name: 'Quantity(kgs)', field: 'Quantity' },
                { name: 'Price', field: 'Price' },
                { name: 'Amount', field: 'Amount' },
                { name: 'Used', field: 'Used' },
            { name: 'WeightNoteNumber', field: 'WeightNoteNumber' },
                 { name: 'AmountToPay', field: 'AmoutToPay' },
                 { name: 'Normal Bags', field: 'NormalBags' },
                 { name: 'Stone Bags', field: 'BagsOfStones' },
                 {name : 'Yellow', field:'YellowBags'},
                 { name: 'Status', field: 'StatusName' },
                
                 { name: 'Action',  cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/supplies/edit/{{row.entity.SupplyId}}">Edit</a> </div>' },

            ];




        }]);


angular
    .module('homer').controller('UnApprovedSupplyController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/SupplyApi/GetAllUnApprovedSupplies');
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
                    name: 'SupplyDate', field: 'SupplyDate', type: 'date', cellFilter: 'date:\'yy-MM-dd\'',
                    sort: {
                        direction: uiGridConstants.DESC,
                        priority: 1
                    }
                },

                { name: 'Truck', field: 'TruckNumber' },
                { name: 'Supplier Name', field: 'SupplierName' },
                { name: 'Branch Name', field: 'BranchName' },
                { name: 'Quantity(kgs)', field: 'Quantity' },
                { name: 'Price', field: 'Price' },
                { name: 'Amount', field: 'Amount' },
               
                { name: 'WeightNoteNumber', field: 'WeightNoteNumber' },
             
                { name: 'Normal Bags', field: 'NormalBags' },
                { name: 'Stone Bags', field: 'BagsOfStones' },
                { name: 'Yellow', field: 'YellowBags' },
                { name: 'Edit', cellTemplate: '<div class="ui-grid-cell-contents"><a href="#/supplies/edit/euxied/{{row.entity.SupplyId}}">Edit</a></div>' },
              
                { name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/supplies/details/euxied/{{row.entity.SupplyId}}">Details</a> </div>' },

            ];




        }]);



angular
    .module('homer').controller('SupplierSupplyController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {

            var supplierId = $scope.supplierId;
            $scope.loadingSpinner = true;
            $scope.SupplierName = "";
            var promise = $http.get('/webapi/SupplyApi/GetAllSuppliesForAParticularSupplier?supplierId='+ supplierId,{});
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;
                }
            );
            $scope.retrievedSupplierId = $scope.supplierId;
            $scope.gridData = {
                enableFiltering: true,
                columnDefs: $scope.columns,
                enableRowSelection: false
            };

            var promise = $http.get('/webapi/UserApi/GetUser?userId='+supplierId, {});
            promise.then(
                function (payload) {
                    var c = payload.data;
                    $scope.SupplierName = c.FirstName + " " + c.LastName;
                }

            );
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
            $scope.gridData.multiSelect = false;

            $scope.gridData.columnDefs = [

                {
                    name: 'SupplyDate', field: 'SupplyDate', type: 'date', cellFilter: 'date:\'yy-MM-dd\'',
                    sort: {
                        direction: uiGridConstants.DESC,
                        priority: 1
                    }
                },
               // { name: 'Supply Number', field: 'SupplyNumber' },
                { name: 'Truck', field: 'TruckNumber' },
                { name: 'Branch Name', field: 'BranchName' },
                { name: 'Quantity(kgs)', field: 'Quantity' },
                { name: 'Price', field: 'Price' },
                { name: 'Amount', field: 'Amount' },
                { name: 'Used', field: 'Used' },
                {name:'IsPaid',field:'IsPaid'},
                  { name: 'WeightNoteNumber', field: 'WeightNoteNumber' },
                 { name: 'AmountToPay', field: 'AmountToPay' },
                 { name: 'Normal Bags', field: 'NormalBags' },
                 { name: 'Stone Bags', field: 'BagsOfStones' },
                 {name:'Yellow',field:'YellowBags'},
                 { name: 'Status', field: 'StatusName' },

                  { name: 'Edit', cellTemplate: '<div class="ui-grid-cell-contents"><a href="#/supplies/edit/' + supplierId + '/{{row.entity.SupplyId}}">Edit</a></div>' },
                     { name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/supplies/details/'+supplierId+'/{{row.entity.SupplyId}}">Details</a> </div>' },
            ];




        }]);


angular
    .module('homer').controller('SupplierUnPaidSupplyController', ['$scope', 'ngTableParams', '$http', '$filter', '$state','$location', '$timeout', 'Utils', 'uiGridConstants','usSpinnerService',
        function ($scope, ngTableParams, $http, $filter,$state, $location, $timeout, Utils, uiGridConstants, usSpinnerService) {

            var supplierId = $scope.supplierId;
            $scope.supplyAmount = 0;
            $scope.selectedSupplies = null;
            var transactionSubTypeId = 30009;
            var sectorId = 2;
            var action = "-";
            $scope.loadingSpinner = true;
            $scope.isDisabled = false;

            var promise = $http.get('/webapi/SupplyApi/GetAllUnPaidSuppliesForAParticularSupplier?supplierId=' + supplierId, {});
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;
                }
            );
            $scope.retrievedSupplierId = $scope.supplierId;
            $scope.gridData = {
                enableFiltering: true,
                columnDefs: $scope.columns,
                enableRowSelection: true
            };

            $scope.gridData.multiSelect = true;

            $scope.gridData.columnDefs = [

                {
                    name: 'SupplyDate', field: 'SupplyDate', type: 'date', cellFilter: 'date:\'yy-MM-dd\'',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    }
                },
              //  { name: 'Supply Number', field: 'SupplyNumber' },
                { name: 'Truck', field: 'TruckNumber' },
                { name: 'Branch Name', field: 'BranchName' },
                { name: 'Quantity(kgs)', field: 'Quantity' },
                { name: 'Price', field: 'Price' },
                { name: 'Amount', field: 'Amount' },
               // { name: 'Used', field: 'Used',width:"5%"},
                {name:'IsPaid',field:'IsPaid'},
                  { name: 'WeightNoteNumber', field: 'WeightNoteNumber' },
                 { name: 'AmountToPay', field: 'AmountToPay' },
                 { name: 'Normal Bags', field: 'NormalBags' },
                 { name: 'Stone Bags', field: 'BagsOfStones' },
                 {name:'Yellow',field:'YellowBags'},
                 { name: 'Status', field: 'StatusName' },
                  //{ name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/supplies/edit/' + supplierId + '/{{row.entity.SupplyId}}">Edit</a> </div>' },
   { name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/supplies/edit/{{row.entity.SupplyId}}">Details</a> </div>' },


            ];


            
            $http.get('/webapi/AccountTransactionActivityApi/GetAllPaymentModes').success(function (data, status) {
                $scope.paymentModes = data;
            });

            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });

            $scope.currentFocused = "";

           
            $scope.UpdateSupplyAmount = function (value) {
                $scope.supplyAmount += value;
                $scope.supply.PaymentAmount = $scope.supplyAmount;
                //console.log($scope.supply.PaymentAmount);

            };
            $scope.DecreaseSupplyAmount = function (value) {
                $scope.supplyAmount -= value;
                $scope.supply.PaymentAmount = $scope.supplyAmount;
               // console.log($scope.supply.PaymentAmount);

            };
            $scope.supply = {
               
                PaymentAmount:"",
                

            };
           
            $scope.gridData.onRegisterApi = function (gridApi) {
                $scope.gridApi = gridApi;

                gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                    if (row.isSelected) {
                        $scope.UpdateSupplyAmount(row.entity.AmountToPay);
                    }
                    else {
                        $scope.DecreaseSupplyAmount(row.entity.AmountToPay);
                    }
                    var msg = 'row selected ' + row.isSelected;
                  
                });

                   
            };



            $scope.Cancel = function () {
                $state.go('supplier-supply-list', { 'supplierId': supplierId });

            };
            $scope.reloadSupplier = function(){
                $state.go('suppliers.list');
            };
            
            $scope.PaySupply = function () {
                $scope.isDisabled = true;
                usSpinnerService.spin('global-spinner');
                $scope.selectedSupplies = $scope.gridApi.selection.getSelectedRows($scope.gridData);
                if ($scope.selectedSupplies != null) {

                    var accountActivity = {

                        Amount: $scope.supply.PaymentAmount,
                        SectorId: sectorId,
                        Notes : $scope.supply.Notes,
                        BranchId : $scope.supply.BranchId,
                        TransactionSubTypeId : transactionSubTypeId,
                        PaymentModeId: $scope.supply.PaymentModeId,
                        Deleted: false,
                        AspNetUserId: supplierId,
                        Action : action,
                    };

                    var promise = $http.post('/webapi/SupplyApi/PayMultipleSupplies', { Supplies: $scope.selectedSupplies, AccountActivity: accountActivity });
                    promise.then(
                        function (payload) {
                            var Id = payload.data;
                            $scope.showMessagePaymentMade = true;
                            usSpinnerService.stop('global-spinner');
                            $timeout(function () {
                                $scope.showMessagePaymentMade = false;
                                //  $scope.Cancel();
                                $scope.reloadSupplier();
                                
                                 }, 1500);
                           
                        });
                }
            }



        }]);

angular
    .module('homer').controller('SupplierPaidSupplyController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {

            var supplierId = $scope.supplierId;
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/SupplyApi/GetAllPaidSuppliesForAParticularSupplier?supplierId=' + supplierId, {});
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;
                }
            );
            $scope.retrievedSupplierId = $scope.supplierId;
            $scope.gridData = {
                enableFiltering: true,
                columnDefs: $scope.columns,
                enableRowSelection: false
            };

            $scope.gridData.multiSelect = false;

            $scope.gridData.columnDefs = [

                {
                    name: 'SupplyDate', field: 'SupplyDate', type: 'date', cellFilter: 'date:\'yy-MM-dd\'',
                    sort: {
                        direction: uiGridConstants.DESC,
                        priority: 1
                    }
                },
             //   { name: 'Supply Number', field: 'SupplyNumber' },
                { name: 'Truck', field: 'TruckNumber' },
                { name: 'Branch Name', field: 'BranchName' },
                { name: 'Quantity(kgs)', field: 'Quantity' },
                { name: 'Price', field: 'Price' },
                { name: 'Amount', field: 'Amount' },
                { name: 'Used', field: 'Used' },
                { name: 'IsPaid', field: 'IsPaid' },
                  { name: 'WeightNoteNumber', field: 'WeightNoteNumber' },
                 { name: 'AmountToPay', field: 'AmountToPay' },
                 { name: 'Normal Bags', field: 'NormalBags' },
                 { name: 'Stone Bags', field: 'BagsOfStones' },
                 {name:'Yellow',field:'YellowBags'},
                 { name: 'Status', field: 'StatusName' },
                  //{ name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/supplies/edit/' + supplierId + '/{{row.entity.SupplyId}}">Edit</a> </div>' },

                   { name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/supplies/edit/{{row.entity.SupplyId}}">Details</a> </div>' },

            ];




        }]);

angular
    .module('homer').controller('StoreMaizeStockController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;

            var storeId = $scope.storeId;

            var promise = $http.get('/webapi/SupplyApi/GetAllMaizeStocksForAparticularStore?storeId=' + storeId, {});
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;

                    $scope.Length = payload.data.length;
                    if ($scope.Length > 0) {

                        var lastIndex = $scope.Length - 1;
                        $scope.maizeBalance = payload.data[lastIndex].StockBalance;
                       
                    }
                    else {
                        $scope.maizeBalance = 0;
                    }
                }
            );
            $scope.retrievedStoreId = $scope.storeId;

            $scope.gridData = {
                enableFiltering: false,
                columnDefs: $scope.columns,
                enableRowSelection: false
            };

      
        }]);


angular
    .module('homer').controller('BranchSupplyController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            var branchId = $scope.branchId;
            var promise = $http.get('/webapi/SupplyApi/GetAllSuppliesForAParticularBranch?branchId=' + branchId, {});
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
                    name: 'SupplyDate', field: 'SupplyDate', type: 'date', cellFilter: 'date:\'yy-MM-dd\'',
                    sort: {
                        direction: uiGridConstants.DESC,
                        priority: 1
                    }
                },
              //  { name: 'Supply Number', field: 'SupplyNumber' },
                { name: 'Truck', field: 'TruckNumber' },
                { name: 'Supplier Name', field: 'SupplierName' },
                { name: 'Branch Name', field: 'BranchName' },
                { name: 'Quantity(kgs)', field: 'Quantity' },
                { name: 'Price', field: 'Price' },
                { name: 'Amount', field: 'Amount' },
                { name: 'Used', field: 'Used' },
            { name: 'WeightNoteNumber', field: 'WeightNoteNumber' },
                 { name: 'AmountToPay', field: 'AmountToPay' },
                 { name: 'Normal Bags', field: 'NormalBags' },
                 { name: 'Stone Bags', field: 'BagsOfStones' },
                 {name:'Yellow',field:'YellowBags'},
                 { name: 'Status', field: 'StatusName' },

                 //{ name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/supplies/edit/{{row.entity.SupplyId}}">Edit</a> </div>' },
                  { name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/supplies/edit/{{row.entity.SupplyId}}">Details</a> </div>' },


            ];




        }]);



angular
    .module('homer').controller('SupplierDashBoardUnPaidSupplyController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', '$timeout', 'Utils', 'uiGridConstants', 'usSpinnerService',
        function ($scope, ngTableParams, $http, $filter, $location, $timeout, Utils, uiGridConstants, usSpinnerService) {

            var supplierId = $scope.supplierId;
            $scope.supplyAmount = 0;
            $scope.selectedSupplies = null;
            var transactionSubTypeId = 30009;
            var sectorId = 2;
            var action = "-";
            $scope.loadingSpinner = true;
            $scope.isDisabled = false;

            var promise = $http.get('/webapi/SupplyApi/GetAllUnPaidSuppliesForAParticularSupplier?supplierId=' + supplierId, {});
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;
                }
            );
            $scope.retrievedSupplierId = $scope.supplierId;
            $scope.gridData = {
                enableFiltering: true,
                columnDefs: $scope.columns,
                enableRowSelection: true
            };

            $scope.gridData.multiSelect = true;

            $scope.gridData.columnDefs = [

                {
                    name: 'SupplyDate', field: 'SupplyDate', type: 'date', cellFilter: 'date:\'yy-MM-dd\'',
                    sort: {
                        direction: uiGridConstants.DESC,
                        priority: 1
                    }
                },
              //  { name: 'Supply Number', field: 'SupplyNumber' },
                { name: 'Truck', field: 'TruckNumber' },
                { name: 'Branch Name', field: 'BranchName' },
                { name: 'Quantity(kgs)', field: 'Quantity' },
                { name: 'Price', field: 'Price', width: "5%" },
                { name: 'Amount', field: 'Amount' },
                {name:'AmountToPay',field:'AmountToPay'},
                { name: 'IsPaid', field: 'IsPaid' },
                  { name: 'WeightNoteNumber', field: 'WeightNoteNumber' },
               //  { name: 'MoistureContent', field: 'MoistureContent' },
                 { name: 'Normal Bags', field: 'NormalBags' },
                 { name: 'Stone Bags', field: 'BagsOfStones' },
                 {name:'Yellow',field:'YellowBags'},
                


            ];


         


        }]);

angular
    .module('homer').controller('SupplierDashBoardPaidSupplyController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {

            var supplierId = $scope.supplierId;
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/SupplyApi/GetAllPaidSuppliesForAParticularSupplier?supplierId=' + supplierId, {});
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;
                }
            );
            $scope.retrievedSupplierId = $scope.supplierId;
            $scope.gridData = {
                enableFiltering: true,
                columnDefs: $scope.columns,
                enableRowSelection: false
            };

            $scope.gridData.multiSelect = false;

            $scope.gridData.columnDefs = [

                {
                    name: 'SupplyDate', field: 'SupplyDate', type: 'date', cellFilter: 'date:\'yy-MM-dd\'',
                    sort: {
                        direction: uiGridConstants.DESC,
                        priority: 1
                    }
                },
               // { name: 'Supply Number', field: 'SupplyNumber' },
                { name: 'Truck', field: 'TruckNumber' },
                { name: 'Branch Name', field: 'BranchName' },
                { name: 'Quantity(kgs)', field: 'Quantity' },
                { name: 'Price', field: 'Price' },
                { name: 'Amount', field: 'Amount' },
              
                { name: 'AmountToPay', field: 'AmountToPay' },
                  { name: 'WeightNoteNumber', field: 'WeightNoteNumber' },
             //    { name: 'MoistureContent', field: 'MoistureContent' },
                 { name: 'Normal Bags', field: 'NormalBags' },
                 { name: 'Stone Bags', field: 'BagsOfStones' },
                 {name:'Yellow',field:'YellowBags'},
                            ];




        }]);


angular
    .module('homer')
    .controller('SupplyDetailsController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval, usSpinnerService) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }

       
        var supplierId = $scope.supplierId;
        var supplyId = $scope.supplyId;
        var action = $scope.action;
        $scope.SupplierName = "";
        $scope.offloadingOptions = ["YES", "NO"];


      

        var promise = $http.get('/webapi/UserApi/GetUser?userId=' + supplierId, {});
        promise.then(
            function (payload) {
                var c = payload.data;
                $scope.SupplierName = c.FirstName + " " + c.LastName;
            }

        );

        if (action == 'create') {
            supplyId = 0;
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

        
            var promise = $http.get('/webapi/SupplyApi/GetSupply?supplyId=' + supplyId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;

                    if (b.Approved == null) {
                        $scope.hideAcceptReject = true;
                    }
                    else if (b.Approved != null ) {
                        $scope.hideAcceptReject = false;
                    }
                    $scope.supply = {
                        SupplyId: b.SupplyId,
                        Quantity: b.Quantity,
                        SupplyDate: b.SupplyDate != null ? moment(b.SupplyDate).format('YYYY-MM-DD HH:mm:ss') : null,
                        SupplierName : b.SupplierName,
                        BranchId: b.BranchId,
                        SupplierId: b.SupplierId,
                        Amount: b.Amount,
                        StatusId : b.StatusId,
                        AmountToPay : b.AmountToPay,
                        Used: b.Used,
                        IsPaid : b.IsPaid,
                        StoreId: b.StoreId,
                        MoistureContent: b.MoistureContent,
                        WeightNoteNumber: b.WeightNoteNumber,
                        NormalBags: b.NormalBags,
                        BagsOfStones: b.BagsOfStones,
                        TruckNumber: b.TruckNumber,
                        Price: b.Price,
                        Offloading: b.Offloading,
                        CreatedOn: b.CreatedOn,
                        TimeStamp: b.TimeStamp,
                        Deleted : b.Deleted,
                        CreatedBy: b.CreatedBy,
                        UpdatedBy : b.UpdatedBy,
                        YellowBags : b.YellowBags,
                        BranchName: b.BranchName,
                        StoreName : b.StoreName,

                    };
                });


        $scope.Approve = function (supply) {
            $scope.showMessageSave = false;

            if ($scope.form.$valid) {
                usSpinnerService.spin('global-spinner');
                var promise = $http.post('/webapi/SupplyApi/Save', {

                    SupplyId: supply.SupplyId,
                    Quantity: supply.Quantity,
                    SupplyDate: supply.SupplyDate,
                    AmountToPay : supply.AmountToPay,
                    BranchId: supply.BranchId,
                    StatusId : supply.StatusId,
                    SupplierId: supply.SupplierId,
                    Amount: supply.Amount,
                    StoreId: supply.StoreId,
                    MoistureContent: supply.MoistureContent,
                    WeightNoteNumber: supply.WeightNoteNumber,
                    NormalBags: supply.NormalBags,
                    BagsOfStones: supply.BagsOfStones,
                    TruckNumber: supply.TruckNumber,
                    Price: supply.Price,
                    Offloading: supply.Offloading,
                    CreatedOn: supply.CreatedOn,
                    TimeStamp: supply.TimeStamp,
                    CreatedBy: supply.CreatedBy,
                    UpdatedBy : supply.UpdatedBy,
                    Deleted: supply.Deleted,
                    YellowBags: supply.YellowBags,
                    Approved: true,

                });

                promise.then(
                    function (payload) {

                        supplyId = payload.data;
                        if (supplyId == -1) {
                            usSpinnerService.stop('global-spinner');
                            $scope.showMessageWeightNoteExists = true;

                            $timeout(function () {
                                $scope.showMessageWeightNoteExists = false;

                            }, 2000);
                        }
                        else {
                            $scope.showMessageSave = true;
                            usSpinnerService.stop('global-spinner');
                            $timeout(function () {
                                $scope.showMessageSave = false;

                               
                                    $state.go('unapprovedsupplies-list');
                                

                            }, 1500);

                        }


                    });
            }

        }

        $scope.Reject = function (supply) {
            $scope.showMessageSave = false;

            if ($scope.form.$valid) {
                var newWeightNoteNumber = supply.WeightNoteNumber + supply.TruckNumber;
                usSpinnerService.spin('global-spinner');
                var promise = $http.post('/webapi/SupplyApi/Save', {

                    SupplyId: supply.SupplyId,
                    Quantity: supply.Quantity,
                    SupplyDate: supply.SupplyDate,
                    AmountToPay : supply.AmountToPay,
                    BranchId: supply.BranchId,
                    SupplierId: supply.SupplierId,
                    Amount: supply.Amount,
                    StatusId : supply.StatusId,
                    StoreId: supply.StoreId,
                    MoistureContent: supply.MoistureContent,
                    WeightNoteNumber: newWeightNoteNumber,
                    NormalBags: supply.NormalBags,
                    BagsOfStones: supply.BagsOfStones,
                    TruckNumber: supply.TruckNumber,
                    Price: supply.Price,
                    Offloading: supply.Offloading,
                    CreatedOn: supply.CreatedOn,
                    TimeStamp: supply.TimeStamp,
                    CreatedBy: supply.CreatedBy,
                    UpdatedBy : supply.UpdatedBy,
                    Deleted: supply.Deleted,
                    YellowBags: supply.YellowBags,
                    Approved: false,

                });

                promise.then(
                    function (payload) {

                        supplyId = payload.data;
                        if (supplyId == -1) {
                            usSpinnerService.stop('global-spinner');
                            $scope.showMessageWeightNoteExists = true;

                            $timeout(function () {
                                $scope.showMessageWeightNoteExists = false;

                            }, 2000);
                        }
                        else {
                            $scope.showMessageSave = true;
                            usSpinnerService.stop('global-spinner');
                            $timeout(function () {
                                $scope.showMessageSave = false;

                               
                                $state.go('unapprovedsupplies-list');
                                

                            }, 1500);

                        }


                    });
            }

        }

        $scope.Cancel = function () {
            $state.go('supplier-supply-list', { 'supplierId': supplierId });
            // $state.go('supplies.list');
        };
       
     

    }
    ]);


angular
    .module('homer').controller('BranchUnUsedSupplyController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            var branchId = $scope.branchId;
            var promise = $http.get('/webapi/SupplyApi/GetAllSuppliesToBeUsedForAParticularBranch?branchId=' + branchId, {});
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
                    name: 'SupplyDate', field: 'SupplyDate', type: 'date', cellFilter: 'date:\'yy-MM-dd\'',
                    sort: {
                        direction: uiGridConstants.DESC,
                        priority: 1
                    }
                },
    
                { name: 'Supplier Name', field: 'SupplierName' },
              
                { name: 'Quantity(kgs)', field: 'Quantity' },
              
                { name: 'Used', field: 'Used' },
                { name: 'WeightNoteNumber', field: 'WeightNoteNumber' },
               
                { name: 'Normal Bags', field: 'NormalBags' },
                { name: 'Stone Bags', field: 'BagsOfStones' },
                { name: 'Yellow', field: 'YellowBags' },
                { name: 'Status', field: 'StatusName' },

                

            ];




        }]);
