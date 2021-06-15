
angular
    .module('homer')
    .controller('CashSaleDetailController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$state', 'usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $state, usSpinnerService) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }
       
        var cashSaleId = $scope.cashSaleId;
        var storeId = $scope.storeId;
        var branchId = $scope.branchId;
        

        var promise = $http.get('/webapi/CashSaleApi/GetCashSale?cashSaleId=' + cashSaleId, {});
        promise.then(
            function (payload) {
                var b = payload.data;
               
                $scope.cashSale = {
                    CashSaleId: b.CashSaleId,
                    BranchId: b.BranchId,
                    StoreId: b.StoreId,
                    Amount: b.Amount,
                    TransactionSubTypeId: b.TransactionSubTypeId,
                    SectorId: b.SectorId,
                    PaymentModeId : b.PaymentModeId,
                    Price : b.Price,
                    TimeStamp: b.TimeStamp,
                    CreatedOn: b.CreatedOn,
                    CreatedBy: b.CreatedBy,
                    UpdatedBy: b.UpdatedBy,
                    Deleted: b.Deleted,
                    Grades: b.Grades,
                    StoreName: b.StoreName,
                    BranchName: b.BranchName,
                    PaymentModeName : b.PaymentModeName,
                    Quantity: b.Quantity,
                    ProductName: b.ProductName,
                    ProductId : b.ProductId,
                    CashSaleBatches: b.CashSaleBatches,
                    Cancelled: b.Cancelled,
                    ReceiptLimit: b.ReceiptLimit,
                    DocumentId : b.DocumentId,
                    
                };
            });

        $scope.Cancelled = function (cashSale) {

            usSpinnerService.spin('global-spinner');

            var promise = $http.post('/webapi/CashSaleApi/Cancelled', {
                CashSaleId: cashSale.CashSaleId,
                BranchId: cashSale.BranchId,
                StoreId: cashSale.StoreId,
                Amount: cashSale.Amount,
                Price: cashSale.Price,
                TransactionSubTypeId: cashSale.TransactionSubTypeId,
                SectorId: cashSale.SectorId,
                PaymentModeId: cashSale.PaymentModeId,
                TimeStamp: cashSale.TimeStamp,
                CreatedOn: cashSale.CreatedOn,
                CreatedBy: cashSale.CreatedBy,
                UpdatedBy: cashSale.UpdatedBy,
                Deleted: cashSale.Deleted,
                Grades: cashSale.Grades,
                StoreName: cashSale.StoreName,
                BranchName: cashSale.BranchName,
                PaymentModeName: cashSale.PaymentModeName,
                Quantity: cashSale.Quantity,
                ProductName: cashSale.ProductName,
                ProductId: cashSale.ProductId,
                CashSaleBatches: cashSale.CashSaleBatches,
                Cancelled: cashSale.Cancelled,
                ReceiptLimit: cashSale.ReceiptLimit,
                DocumentId : cashSale.DocumentId,

            });

            promise.then(
                function (payload) {

                    cashSaleId = payload.data;
                    usSpinnerService.stop('global-spinner');
                    $timeout(function () {

                        //$state.go('cashsalelist-branch-sale', { 'branchId': branchId });
                        $state.go('cashsalelist-branch-sale', { 'branchId': branchId });

                    }, 500);
                });
        }


    }]);


angular
    .module('homer')
    .controller('CashSaleEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval, usSpinnerService) {
        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }

        $scope.selectedGrades = [];
        $scope.batches = [];
        var transactionSubTypeId = 2;
        var departmentId = 10002;
        var paymentModeId = 2;
        var branchId = $scope.branchId;
        var cashSaleId = $scope.cashSaleId;
        var action = $scope.action;
        $scope.showMessageBatchQuantityNotEnough = false;
        $scope.showMessageNoPrices = false;

        $http.get('webapi/ProductApi/GetAllproducts').success(function (data, status) {
            $scope.products = data;
        });

       

       

        var promisebranch = $http.get('/webapi/BranchApi/GetBranch?branchId=' + branchId, {});
        promisebranch.then(
            function (payload) {
                var b = payload.data;

                $scope.branch = {
                    BranchId: b.BranchId,
                    Name: b.Name,

                };

            });

        $http.get('webapi/GradeApi/GetAllGrades').success(function (data, status) {
            $scope.grades = data;
        });

        $http.get('/webapi/StoreApi/GetAllStores').success(function (data,status) {
                   $scope.stores = data;

               });

     
        
        $scope.OnProductChange = function (cashSale) {
            //var selectedBranchId = delivery.BranchId

            $http.get('/webapi/StoreApi/GetAllBranchStores?branchId=' + branchId).then(function (responses) {
                $scope.stores = responses.data;

            });
            if (cashSale.ProductId == 2) {
                $http.get('/webapi/BatchApi/GetAllBatchesForBrandDelivery?branchId=' + branchId
                ).then(function (responses) {
                    $scope.retrievedBatches = responses.data;

                    angular.forEach($scope.retrievedBatches, function (value, key) {
                        if (value.BrandBalance > 0) {
                            $scope.batches = $scope.batches.concat(value);
                        }
                    });



                });
            }
            else {
                $http.get('/webapi/BatchApi/GetAllBatchesForAParticularBranchToTransfer?branchId=' + branchId + '&productId=' + cashSale.ProductId
                ).then(function (responses) {
                    $scope.retrievedBatches = responses.data;
                    $scope.batches = $scope.retrievedBatches;

                });
            }

        }

      

        $http.get('/webapi/AccountTransactionActivityApi/GetAllPaymentModes').success(function (data, status) {
            $scope.paymentModes = data;
        });

        if (action == 'create') {
            cashSaleId = 0;
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
            var promise = $http.get('/webapi/CashSaleApi/GetCashSale?cashSaleId=' + cashSaleId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;
                    $scope.cashSale = {
                        CashSaleId: b.CashSaleId,
                        BranchId: b.BranchId,
                        Price: b.Price,
                        Grades : b.Grades,
                        Amount: b.Amount,
                        CashSaleBatches : b.CashSaleBatches,
                        ProductId : b.ProductId,
                        StoreId: b.StoreId,
                        TransactionSubTypeId: b.TransactionSubTypeId,
                        SectorId: b.SectorId,
                        Batches: b.Batches,
                        SectorName : b.SectorName,
                        PaymentModeId: paymentModeId,
                        TimeStamp: b.TimeStamp,
                        CreatedOn: b.CreatedOn,
                        CreatedBy: b.CreatedBy,
                        UpdatedBy: b.UpdatedBy,
                        Deleted: b.Deleted,
                        PaymentModeName : b.PaymentModeName,
                       
                    };
                });

           
            
        }

        $scope.Save = function (cashSale) {
            $scope.showMessageSave = false;
            $scope.TotalGradeQuantities = 0;
            $scope.TotalBatchGradeQuantities = 0;
            $scope.TotalAmount = 0;
            $scope.TotalQuantity = 0;
            $scope.DenominationQuantity = 0;
            $scope.denominationQuantities = 0;
            $scope.denominationBatchQuantities = 0;
            $scope.DenominationAmount = 0;
            $scope.batchBrandQuantity = 0;
            $scope.showMessageSave = false;
            $scope.showMessageCheckGrade = false;
            $scope.showMessageNoPrices = false;
            if (cashSale.ProductId == 1) {
                if (cashSale.Batches != null && cashSale.Batches != "undefined") {
                    angular.forEach(cashSale.selectedGrades, function (value, key) {
                        var denominations = value.Denominations;
                        angular.forEach(denominations, function (denominations) {
                            $scope.denominationQuantities = parseFloat((denominations.Quantity) * (denominations.Value)) + parseFloat($scope.denominationQuantities);

                            $scope.DenominationAmount = (denominations.Price * denominations.Quantity) + $scope.DenominationAmount;
                            $scope.DenominationQuantity = (denominations.Quantity * denominations.Value) + $scope.DenominationQuantity;

                        });
                        $scope.TotalGradeQuantities = $scope.denominationQuantities;
                        $scope.TotalAmount = $scope.DenominationAmount;
                        $scope.TotalQuantity = $scope.DenominationQuantity;

                    });
                    angular.forEach(cashSale.batchGrades, function (value, key) {
                        var denominations = value.Denominations;
                        angular.forEach(denominations, function (denominations) {
                            $scope.denominationBatchQuantities = parseFloat((denominations.QuantityToRemove) * (denominations.Value)) + parseFloat($scope.denominationBatchQuantities);
                        });


                        $scope.TotalBatchGradeQuantities = $scope.denominationBatchQuantities;

                    });
                }
                else {
                    if (cashSale.selectedGrades) {
                        angular.forEach(cashSale.selectedGrades, function (value, key) {
                            var denominations = value.Denominations;
                            //var keepGoing = true;
                            angular.forEach(denominations, function (denominations) {

                                $scope.DenominationAmount = (denominations.Price * denominations.Quantity) + $scope.DenominationAmount;
                                $scope.DenominationQuantity = (denominations.Quantity * denominations.Value) + $scope.DenominationQuantity;


                            });
                            $scope.TotalAmount = $scope.DenominationAmount;
                            $scope.TotalQuantity = $scope.DenominationQuantity;

                        });
                    }

                    else {
                        $scope.showMessageCheckGrade = true;

                        $timeout(function () {
                            $scope.showMessageCheckGrade = false;
                            $state.go('cashsalelist-branch-sale', { 'branchId': branchId });

                        }, 2000);
                    }

                }

                if ($scope.TotalGradeQuantities == $scope.TotalBatchGradeQuantities) {
                    usSpinnerService.spin('global-spinner');
                    if ($scope.form.$valid) {
                        var promise = $http.post('/webapi/CashSaleApi/Save', {
                            CashSaleId: cashSaleId,
                            Amount: $scope.TotalAmount,
                            Price: cashSale.Price,
                            Quantity: $scope.TotalQuantity,

                            BranchId: branchId,
                            PaymentModeId: paymentModeId,
                            ProductId: cashSale.ProductId,

                            SectorId: departmentId,
                            StoreId: cashSale.StoreId,

                            SelectedGrades: cashSale.selectedGrades,
                            TransactionSubTypeId: transactionSubTypeId,
                            Batches: cashSale.Batches,
                            CreatedBy: cashSale.CreatedBy,
                            CreatedOn: cashSale.CreatedOn,
                            Deleted: cashSale.Deleted,

                        });

                        promise.then(
                            function (payload) {

                                cashSaleId = payload.data;
                                if (cashSaleId == -1) {
                                    usSpinnerService.stop('global-spinner');
                                    $scope.showMessageNoEnoughStock = true;

                                    $timeout(function () {
                                        $scope.showMessageNoEnoughStock = false;

                                    }, 3000);
                                }
                                else if (cashSaleId == -2) {
                                    usSpinnerService.stop('global-spinner');
                                    $scope.showMessageNoGradeStock = true;

                                    $timeout(function () {
                                        $scope.showMessageNoGradeStock = false;

                                    }, 3000);
                                }
                                else if (cashSaleId == -33) {
                                    usSpinnerService.stop('global-spinner');
                                    $scope.showMessageBatchNotSelected = true;

                                    $timeout(function () {
                                        $scope.showMessageBatchNotSelected = false;

                                    }, 3000);
                                }
                                else {
                                    $scope.showMessageSave = true;
                                    usSpinnerService.stop('global-spinner');
                                    $timeout(function () {
                                        $scope.showMessageSave = false;

                                        if (action == "create") {
                                            $state.go('cashsalelist-branch-sale', { 'branchId': branchId });
                                        }

                                    }, 3000);
                                }



                            });
                    }
                }
                else {
                    $scope.showMessageBatchandGradeQuantities = true;
                    usSpinnerService.stop('global-spinner');
                    $timeout(function () {
                        $scope.showMessageBatchandGradeQuantities = false;

                        $state.go('cashsalelist-branch-sale', { 'branchId': branchId });


                    }, 4000);
                }

            }
            else if (cashSale.ProductId == 10003) {
                if (cashSale.Batches != null && cashSale.Batches != "undefined") {
                    angular.forEach(cashSale.selectedGrades, function (value, key) {
                        var denominations = value.Denominations;
                        angular.forEach(denominations, function (denominations) {
                            $scope.denominationQuantities = parseFloat((denominations.Quantity) * (denominations.Value)) + parseFloat($scope.denominationQuantities);

                            $scope.DenominationAmount = (denominations.Price * denominations.Quantity) + $scope.DenominationAmount;
                            $scope.DenominationQuantity = (denominations.Quantity * denominations.Value) + $scope.DenominationQuantity;

                        });
                        $scope.TotalGradeQuantities = $scope.denominationQuantities;
                        $scope.TotalAmount = $scope.DenominationAmount;
                        $scope.TotalQuantity = $scope.DenominationQuantity;

                    });
                    angular.forEach(cashSale.batchGrades, function (value, key) {
                        var denominations = value.Denominations;
                        angular.forEach(denominations, function (denominations) {
                            $scope.denominationBatchQuantities = parseFloat((denominations.QuantityToRemove) * (denominations.Value)) + parseFloat($scope.denominationBatchQuantities);
                        });


                        $scope.TotalBatchGradeQuantities = $scope.denominationBatchQuantities;

                    });
                }
                else {
                    if (cashSale.selectedGrades) {
                        angular.forEach(cashSale.selectedGrades, function (value, key) {
                            var denominations = value.Denominations;
                            //var keepGoing = true;
                            angular.forEach(denominations, function (denominations) {

                                $scope.DenominationAmount = (denominations.Price * denominations.Quantity) + $scope.DenominationAmount;
                                $scope.DenominationQuantity = (denominations.Quantity * denominations.Value) + $scope.DenominationQuantity;


                            });
                            $scope.TotalAmount = $scope.DenominationAmount;
                            $scope.TotalQuantity = $scope.DenominationQuantity;

                        });
                    }

                    else {
                        $scope.showMessageCheckGrade = true;

                        $timeout(function () {
                            $scope.showMessageCheckGrade = false;
                            $state.go('cashsalelist-branch-sale', { 'branchId': branchId });

                        }, 2000);
                    }

                }

                if ($scope.TotalGradeQuantities == $scope.TotalBatchGradeQuantities) {
                    usSpinnerService.spin('global-spinner');
                    if ($scope.form.$valid) {
                        var promise = $http.post('/webapi/CashSaleApi/Save', {
                            CashSaleId: cashSaleId,
                            Amount: $scope.TotalAmount,
                            Price: cashSale.Price,
                            Quantity: $scope.TotalQuantity,

                            BranchId: branchId,
                            PaymentModeId: paymentModeId,
                            ProductId: cashSale.ProductId,

                            SectorId: departmentId,
                            StoreId: cashSale.StoreId,

                            SelectedGrades: cashSale.selectedGrades,
                            TransactionSubTypeId: transactionSubTypeId,
                            Batches: cashSale.Batches,
                            CreatedBy: cashSale.CreatedBy,
                            CreatedOn: cashSale.CreatedOn,
                            Deleted: cashSale.Deleted,

                        });

                        promise.then(
                            function (payload) {

                                cashSaleId = payload.data;
                                if (cashSaleId == -1) {
                                    usSpinnerService.stop('global-spinner');
                                    $scope.showMessageNoEnoughStock = true;

                                    $timeout(function () {
                                        $scope.showMessageNoEnoughStock = false;

                                    }, 3000);
                                }
                                else if (cashSaleId == -2) {
                                    usSpinnerService.stop('global-spinner');
                                    $scope.showMessageNoGradeStock = true;

                                    $timeout(function () {
                                        $scope.showMessageNoGradeStock = false;

                                    }, 3000);
                                }
                                else if (cashSaleId == -33) {
                                    usSpinnerService.stop('global-spinner');
                                    $scope.showMessageBatchNotSelected = true;

                                    $timeout(function () {
                                        $scope.showMessageBatchNotSelected = false;

                                    }, 3000);
                                }
                                else {
                                    $scope.showMessageSave = true;
                                    usSpinnerService.stop('global-spinner');
                                    $timeout(function () {
                                        $scope.showMessageSave = false;

                                        if (action == "create") {
                                            $state.go('cashsalelist-branch-sale', { 'branchId': branchId });
                                        }

                                    }, 3000);
                                }



                            });
                    }
                }
                else {
                    $scope.showMessageBatchandGradeQuantities = true;
                    usSpinnerService.stop('global-spinner');
                    $timeout(function () {
                        $scope.showMessageBatchandGradeQuantities = false;

                        $state.go('cashsalelist-branch-sale', { 'branchId': branchId });


                    }, 4000);
                }

            }
            else {
               
                    if (cashSale.Batches != null && cashSale.Batches !== 'undefined') {
                       
                        angular.forEach(cashSale.Batches, function (value, key) {
                            $scope.batchBrandQuantity = parseFloat(value.QuantityToRemove )+ $scope.batchBrandQuantity;

                        });

                        if ($scope.batchBrandQuantity == cashSale.Quantity) {
                            $scope.TotalAmount = cashSale.Price * cashSale.Quantity;
                            $scope.TotalQuantity = cashSale.Quantity;

                            usSpinnerService.spin('global-spinner');
                            if ($scope.form.$valid) {
                                var promise = $http.post('/webapi/CashSaleApi/Save', {
                                    CashSaleId: cashSaleId,
                                    Amount: $scope.TotalAmount,
                                    Price: cashSale.Price,
                                    Quantity: $scope.TotalQuantity,

                                    BranchId: branchId,
                                    PaymentModeId: paymentModeId,
                                    ProductId: cashSale.ProductId,

                                    SectorId: departmentId,
                                    StoreId: cashSale.StoreId,

                                    SelectedGrades: cashSale.selectedGrades,
                                    TransactionSubTypeId: transactionSubTypeId,
                                    Batches: cashSale.Batches,
                                    CreatedBy: cashSale.CreatedBy,
                                    CreatedOn: cashSale.CreatedOn,
                                    Deleted: cashSale.Deleted,

                                });

                                promise.then(
                                    function (payload) {

                                        cashSaleId = payload.data;
                                        
                                            $scope.showMessageSave = true;
                                            usSpinnerService.stop('global-spinner');
                                            $timeout(function () {
                                                $scope.showMessageSave = false;

                                                if (action == "create") {
                                                    $state.go('cashsalelist-branch-sale', { 'branchId': branchId });
                                                }

                                            }, 3000);
                                      

                                    });
                            }
                        }
                        else {
                           
                            $scope.showMessageBatchNotSelected = true;

                            $timeout(function () {
                                $scope.showMessageBatchNotSelected = false;

                                $state.go('cashsalelist-branch-sale', { 'branchId': branchId });

                            }, 4000);
                        }

                    }

                    else {
                        $scope.showMessageBatchNotSelected = true;

                        $timeout(function () {
                            $scope.showMessageBatchNotSelected = false;

                            $state.go('cashsalelist-branch-sale', { 'branchId': branchId });

                        }, 4000);
                    }

            }
           
        }



        $scope.Cancel = function () {
            $state.go('cashsalelist-branch-sale', { 'branchId': branchId });
        };

      

    }
    ]);

angular
    .module('homer').controller('BranchCashSaleController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            var branchId = $scope.branchId;



            var promisebranch = $http.get('/webapi/BranchApi/GetBranch?branchId=' + branchId, {});
            promisebranch.then(
                function (payload) {
                    var b = payload.data;

                    $scope.branch = {
                        BranchId: b.BranchId,
                        Name: b.Name,

                    };

                });

            $scope.topItem1 = null;
            $scope.offset = 0;
            $scope.pageSize = 2;
            $scope.counter = 0;

            $scope.showLoadMore = true;

            $scope.LoadMore = function () {
                $scope.loading = true;
                $scope.loadPageSize = 5;
                $scope.counter = 0;

                $scope.counter++;
                $scope.offset = $scope.offset + ($scope.loadPageSize * $scope.counter);

                var promise = $http.get('/webapi/CashSaleApi/GetAllCashSalesForAparticularBranch?branchId='+ branchId + '&offSet=' + $scope.offset + '&pageSize=' + $scope.loadPageSize);
                promise.then(
                    function (payload) {
                        var results = payload.data;
                        setTimeout(function () {
                            $scope.$apply(function () {
                                $scope.loading = false;
                                if (results.length > 0) {
                                    angular.forEach(results, function (obj) {
                                        $scope.data.push(obj);
                                    });
                                }
                                else {
                                    $scope.showLoadMore = false;
                                }
                            });
                        }, 500);
                        $scope.gridData.data = $scope.data;
                        $scope.loadingSpinner = false;
                    }
                );

                $scope.retrievedBranchId = $scope.branchId;
                $scope.gridData = {
                    enableFiltering: true,
                    columnDefs: $scope.columns,
                    enableRowSelection: false
                };

                $scope.gridData.multiSelect = false;


                $scope.gridData.columnDefs = [

                    {
                        name: 'Product', field: 'ProductName'

                    },
                    { name: 'TotalQuantity(kgs)', field: 'Quantity' },
                    { name: 'Price', field: 'Price' },
                    { name: 'Amount', field: 'Amount' },


                    //{ name: 'Branch', field: 'BranchName' },


                    {
                        name: 'Date', field: 'CreatedOn',
                        sort: {
                            direction: uiGridConstants.DESC,
                            priority: 1
                        }
                    },

                    {
                        name: 'Details', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/cashsale/' + $scope.branchId + '/{{row.entity.CashSaleId}}">Details</a> </div>',

                    },

                    {
                        name: 'Receipt', cellTemplate: '<div class="ui-grid-cell-contents"><a  href="/Excel/CashReceipt?documentId={{row.entity.DocumentId}}">Print</a></div>'
                    },
                    //{
                    //    name:'Receipt', cellTemplate:'CashReceipt'
                    //}
                ];


            };

            var promise = $http.get('/webapi/CashSaleApi/GetAllCashSalesForAparticularBranch?branchId='+branchId + '&offSet=' + $scope.offset + '&pageSize=' + $scope.pageSize);
            promise.then(
                function (payload) {
                    $scope.data = payload.data;
                    if ($scope.data.length > 1) {
                        $scope.topItem1 = $scope.data[0];
                        var index = $scope.data.indexOf($scope.topItem1)

                    }
                    if ($scope.data.length == 0) {
                        $scope.showLoadMore = false;
                    }
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;

                }
            );

            $scope.retrievedBranchId = $scope.branchId;
            $scope.gridData = {
                enableFiltering: true,
                columnDefs: $scope.columns,
                enableRowSelection: false
            };

            $scope.gridData.multiSelect = false;


            $scope.gridData.columnDefs = [

                {
                    name: 'Product', field: 'ProductName'

                },
                { name: 'TotalQuantity(kgs)', field: 'Quantity' },
                { name: 'Price', field: 'Price' },
                { name: 'Amount', field: 'Amount' },



                {
                    name: 'Date', field: 'CreatedOn',
                    sort: {
                        direction: uiGridConstants.DESC,
                        priority: 1
                    }
                },

                {
                    name: 'Details', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/cashsale/' + $scope.branchId + '/{{row.entity.CashSaleId}}">Details</a> </div>',

                },

                {
                    name: 'Receipt', cellTemplate: '<div class="ui-grid-cell-contents"><a  href="/Excel/CashReceipt?documentId={{row.entity.DocumentId}}">Print</a></div>'
                },

            ];


            //var promise = $http.get('/webapi/CashSaleApi/GetAllCashSalesForAparticularBranch?branchId=' + branchId, {});
            //promise.then(
            //    function (payload) {
            //        $scope.gridData.data = payload.data;
            //        $scope.loadingSpinner = false;
            //    }
            //);

            //$scope.retrievedBranchId = $scope.branchId;
            //$scope.gridData = {
            //    enableFiltering: true,
            //    columnDefs: $scope.columns,
            //    enableRowSelection: false
            //};

            //$scope.gridData.multiSelect = false;


            //$scope.gridData.columnDefs = [

            //    {
            //        name: 'Product', field: 'ProductName'

            //    },
            //     { name: 'TotalQuantity(kgs)', field: 'Quantity' },
            //     {name:'Price', field:'Price'},
            //    { name: 'Amount', field: 'Amount' },
                
            
            //    {
            //        name: 'Date', field: 'CreatedOn',
            //        sort: {
            //            direction: uiGridConstants.DESC,
            //            priority: 1
            //        }
            //    },
               
            //{
            //    name: 'Details', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/cashsale/' + $scope.branchId + '/{{row.entity.CashSaleId}}">Details</a> </div>',

            //},

            // {
            //     name: 'Receipt', cellTemplate: '<div class="ui-grid-cell-contents"><a  href="/Excel/CashReceipt?documentId={{row.entity.DocumentId}}">Print</a></div>'
            // },
           
            //];




        }]);


