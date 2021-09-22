

angular
    .module('homer').controller('ReportAccountTransactionController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;
            $scope.accounts = [];

            $scope.AccountTransactionsForThisMonth = function () {
                $scope.data = [];
                var promise = $http.get('/webapi/ReportApi/GenerateAccountTransactionCurrentMonthReport', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 2;
                     if ($scope.data.length > 0) {
                         $scope.showDownloadLink = true;
                     }
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { TransactionId: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.AccountTodaysTransactions = function () {
                $scope.data = [];
                var promise = $http.get('/webapi/ReportApi/GenerateAccountTransactionTodaysReport', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 1;
                     if ($scope.data.length > 0) {
                         $scope.showDownloadLink = true;
                     }
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { TransactionId: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.AccountWeeksTransactions = function () {
                $scope.data = [];
                var promise = $http.get('/webapi/ReportApi/GenerateAccountTransactionCurrentWeekReport', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 3;
                     if ($scope.data.length > 0) {
                         $scope.showDownloadLink = true;
                     }
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { TransactionId: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }



            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });

            $http.get('/webapi/SupplierApi/GetAllSuppliers').success(function (data, status) {
                $scope.suppliers = data;
                //$scope.accounts = $scope.accounts.concat(data);
            });

            $http.get('/webapi/CustomerApi/GetAllCustomers').success(function (data, status) {
                $scope.customers = data;
                //$scope.accounts = $scope.accounts.concat(data);
            });

            $scope.SearchAccountTransactions = function (accountTransaction) {
                $scope.data = [];
               
                var promise = $http.post('/webapi/ReportApi/GenerateAccountTransactionsBetweenTheSpecifiedDates',
                        {
                            FromDate: accountTransaction.FromDate,
                            ToDate: accountTransaction.ToDate,
                            SupplierId: accountTransaction.Id,
                            BranchId: accountTransaction.BranchId,

                        });
                promise.then(
                 function (payload) {

                     $scope.data = payload.data;
                     $scope.reportType = 4;

                     $scope.tableParams = new ngTableParams({
                         page: 1,
                         count: 10,
                         
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


            $scope.DownloadExcelFile = function () {
                $window.open("/Excel/Index/" + $scope.reportType);
            };

        }]);
angular
    .module('homer').controller('ReportSupplyController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
           

            $scope.totalMaize = 0;
            $scope.totalAmount = 0;
            $scope.totalNormalBags = 0;
            $scope.totalStoneBags = 0;
            $scope.totalYellowBags = 0;
            $scope.SuppliesForThisMonth = function () {
                $scope.data = [];
                $scope.totalMaize = 0;
                $scope.totalAmount = 0;
                $scope.totalNormalBags = 0;
                $scope.totalStoneBags = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateSupplyCurrentMonthReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
             
                     $scope.data = payload.data.Supplies;
                     $scope.totalMaize = payload.data.TotalMaize;
                     $scope.totalAmount = payload.data.TotalAmount;
                     $scope.totalNormalBags = payload.data.TotalNormalBags;
                     $scope.totalStoneBags = payload.data.TotalStoneBags;
                     $scope.totalYellowBags = payload.data.TotalYellowBags;
                     $scope.reportType = 2;
                    
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { SupplyNumber: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.TodaysSupplies = function () {
                $scope.data = [];
                $scope.totalMaize = 0;
                $scope.totalAmount = 0;
                $scope.totalNormalBags = 0;
                $scope.totalStoneBags = 0;
                $scope.totalYellowBags = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateSupplyTodaysReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     //$scope.data = payload.data;
                     $scope.data = payload.data.Supplies;
                     $scope.totalMaize = payload.data.TotalMaize;
                     $scope.totalAmount = payload.data.TotalAmount;
                     $scope.totalNormalBags = payload.data.TotalNormalBags;
                     $scope.totalStoneBags = payload.data.TotalStoneBags;
                     $scope.totalYellowBags = payload.data.TotalYellowBags;
                     $scope.reportType = 1;
                   
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { SupplyNumber: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.WeeksSupplies = function () {
                $scope.data = [];
                $scope.totalMaize = 0;
                $scope.totalAmount = 0;
                $scope.totalNormalBags = 0;
                $scope.totalStoneBags = 0;
                $scope.totalYellowBags = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateSupplyCurrentWeekReportForBranch', {});
                
                promise.then(
                 function (payload) {
                     //$scope.data = payload.data;
                     $scope.data = payload.data.Supplies;
                     $scope.totalMaize = payload.data.TotalMaize;
                     $scope.totalAmount = payload.data.TotalAmount;
                     $scope.totalNormalBags = payload.data.TotalNormalBags;
                     $scope.totalStoneBags = payload.data.TotalStoneBags;
                     $scope.totalYellowBags = payload.data.TotalYellowBags;
                     $scope.reportType = 3;
                    
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { SupplyNumber: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }


           

            $http.get('/webapi/SupplierApi/GetAllSuppliers').success(function (data, status) {
                $scope.suppliers = data;
            });

            $scope.SearchSupplies = function (supply) {
                $scope.data = [];
                $scope.totalMaize = 0;
                $scope.totalAmount = 0;
                $scope.totalNormalBags = 0;
                $scope.totalStoneBags = 0;
                $scope.totalYellowBags = 0;


                $scope.fromDate = supply.FromDate;
                $scope.toDate = supply.ToDate;
                var promise = $http.post('/webapi/ReportApi/GetAllSuppliesBetweenTheSpecifiedDatesForBranch',
                        {
                            FromDate: supply.FromDate,
                            ToDate: supply.ToDate,
                            SupplierId: supply.Id,
                            BranchId : supply.BranchId,
                            
                        });
                promise.then(
                 function (payload) {

                     //$scope.data = payload.data;
                     $scope.data = payload.data.Supplies;
                     $scope.totalMaize = payload.data.TotalMaize;
                     $scope.totalAmount = payload.data.TotalAmount;
                     $scope.totalNormalBags = payload.data.TotalNormalBags;
                     $scope.totalStoneBags = payload.data.TotalStoneBags;
                     $scope.totalYellowBags = payload.data.TotalYellowBags;
                     $scope.reportType = 4;
                    
                     $scope.tableParams = new ngTableParams({
                         page: 1,
                         count: 10,
                         sorting: { SupplyDate: 'desc' }
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

            $scope.downloadPDF = function () {
                var supplyData = [];

                var supply_branchName;
                angular.forEach($scope.data, function (value, key) {
                    supplyData.push({
                        WeightNoteNumber: value.WeightNoteNumber, Quantity: value.Quantity, Price: value.Price,
                        Amount: value.Amount, BranchName: value.BranchName, SupplyDate: moment(value.SupplyDate).format('DD-MMM-YYYY'),
                        SupplierName: value.SupplierName, NormalBags: value.NormalBags, BagsOfStones: value.BagsOfStones,
                        YellowBags: value.YellowBags
                    });

                });

                supply_branchName = $filter('uppercase')($scope.data[0].BranchName);
                var items = supplyData.map(function (item) {
                    return [item.WeightNoteNumber, item.Quantity.toString(), item.Price.toString(),
                    item.Amount.toString(),item.SupplyDate, item.SupplierName, item.NormalBags.toString()
                        , item.BagsOfStones.toString(), item.YellowBags.toString()];
                });

                var docDefinition = {
                    pageOrientation: 'landscape',
                    header: function () {
                        return [
                            {
                                style: 'table',
                                margin: [62, 35, 62, 35],
                                table: {
                                    widths: ['*'],
                                    headerRows: 0,
                                    body: [
                                        [
                                            { text: 'MBALE INVESTMENT SUPPLIES\' REPORT OF ' + supply_branchName + ' FROM '  + $scope.fromDate.toString() + ' ' + ' T0   ' + $scope.toDate.toString(), style: 'topHeader', alignment: 'center' },


                                        ]
                                    ]
                                },
                                layout: 'noBorders'
                            }
                        ]
                    },
                    footer: function (currentPage, pageCount) {
                        return [
                            { text: currentPage.toString() + ' of ' + pageCount, alignment: 'center', style: 'footer' }
                        ]
                    },
                    content: [
                        {
                            style: 'topTable',
                            table: {
                                widths: ['*', '*', '*', '*', '*', '*', '*', '*', '*', '*'],
                                heights: [18],
                                headerRows: 1,
                                body: [

                                    [
                                        { text: 'WNN', style: 'tableLabel' }, { text: 'Qty', style: 'tableLabel' },
                                        { text: 'Price', style: 'tableLabel' }, { text: 'Amount', style: 'tableLabel' },
                                        { text: 'Date', style: 'tableLabel' },
                                        //{ text: 'SNO', style: 'tableLabel' },
                                        { text: 'Supplier', style: 'tableLabel' }, { text: 'NBags', style: 'tableLabel' },
                                        { text: 'SBags', style: 'tableLabel' }, { text: 'YBags', style: 'tableLabel' },
                                    ],





                                ].concat(items)
                            },



                            layout: {
                                paddingLeft: function (i, node) { return 6; },
                                paddingRight: function (i, node) { return 6; },
                                paddingTop: function (i, node) { return 6; },
                                paddingBottom: function (i, node) { return 6; },
                                fillColor: function (i, node) {
                                    return (i % 2 === 0) ? '#F5F5F5' : null;
                                }
                            }
                        },

                        {
                            table: {
                                widths: ['*', '*', '*', '*', '*', '*', '*', '*', '*', '*'],
                                heights: [18],
                                headerRows: 1,
                                body: [

                                    //[
                                    //    { text: 'WNN', style: 'tableLabel' }, { text: 'Qty', style: 'tableLabel' },
                                    //    { text: 'Price', style: 'tableLabel' }, { text: 'Amount', style: 'tableLabel' },
                                    //    { text: 'Branch', style: 'tableLabel' }, { text: 'Date', style: 'tableLabel' },
                                    //    //{ text: 'SNO', style: 'tableLabel' },
                                    //    { text: 'Supplier', style: 'tableLabel' }, { text: 'NBags', style: 'tableLabel' },
                                    //    { text: 'SBags', style: 'tableLabel' }, { text: 'YBags', style: 'tableLabel' },
                                    //],



                                    [
                                        { text: 'TOTAL', style: 'tableLabel', bold: true }, { text: $scope.totalMaize.toString() + ' Kgs', style: 'tableLabel', bold: true },
                                        { text: '', style: 'tableLabel' }, { text: $scope.totalAmount.toString(), style: 'tableLabel', bold: true },
                                         { text: ' ', style: 'tableLabel' },
                                        { text: ' ', style: 'tableLabel' }, { text: $scope.totalNormalBags.toString(), style: 'tableLabel', bold: true },
                                        { text: $scope.totalStoneBags.toString(), style: 'tableLabel', bold: true },
                                        { text: $scope.totalYellowBags.toString(), style: 'tableLabel', bold: true },
                                    ],

                                ]
                            },
                            layout: {
                                paddingLeft: function (i, node) { return 6; },
                                paddingRight: function (i, node) { return 6; },
                                paddingTop: function (i, node) { return 6; },
                                paddingBottom: function (i, node) { return 6; },
                                fillColor: function (i, node) {
                                    return (i % 2 === 0) ? '#F5F5F5' : null;
                                }
                            }
                        }


                    ],
                    pageSize: 'A4',
                    pageMargins: [62, 80, 62, 80],
                    styles: {
                        topHeader: {
                            fontSize: 15,
                            bold: true,
                            margin: [0, 6, 0, 30],
                            alignment: 'center'
                        },
                        table: {
                            fontSize: 4,
                            alignment: 'center',
                            color: 'black',
                            margin: [0, 5, 0, 15]
                        },
                        header: {
                            fontSize: 12,
                            bold: true,
                            margin: [0, 10, 0, 15],
                            alignment: 'center'
                        },
                        footer: {
                            fontSize: 8,
                            margin: [0, 25, 0, 17],
                            alignment: 'center'
                        }
                    }
                };
                var date = new Date();
                date = moment(date).format('DD_MMM_YYYY_HH_mm_ss');
                pdfMake.createPdf(docDefinition).download('supplies_' + date + '.pdf');
                // pdfMake.createPdf(docDefinition).download("supplies.pdf");
            };


           

        }]);

angular
    .module('homer').controller('ReportDepositController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            var transactionSubTypeId = 10008;



            $http.get('/webapi/CustomerApi/GetAllCustomers').success(function (data, status) {
                $scope.customers = data;

            });

            $scope.SearchDeposits = function (deposit) {
                $scope.data = [];
                $scope.totalAmount = 0;
                $scope.totalQuantity = 0;

                var promise = $http.post('/webapi/ReportApi/GenerateDepositsReport',
                        {
                            FromDate: deposit.FromDate,
                            ToDate: deposit.ToDate,

                           
                            SupplierId: deposit.Id,
                            

                        });
                promise.then(
                 function (payload) {

                     $scope.data = payload.data.Deposits;
                     $scope.totalAmount = payload.data.TotalAmount;
                     $scope.totalQuantity = payload.data.TotalQuantity;
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

angular
    .module('homer').controller('ReportRecoveryController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;


            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });

            $http.get('/webapi/CustomerApi/GetAllCustomers').success(function (data, status) {
                $scope.customers = data;

            });

            $scope.SearchRecoveries = function (deposit) {
                $scope.data = [];
                $scope.totalAmount = 0;


                var promise = $http.post('/webapi/ReportApi/GenerateRecoveriesReport',
                    {
                        FromDate: deposit.FromDate,
                        ToDate: deposit.ToDate,

                 
                        SupplierId: deposit.Id,

                    });
                promise.then(
                    function (payload) {

                        $scope.data = payload.data.Deposits;
                        $scope.totalAmount = payload.data.TotalAmount;
                        $scope.totalQuantity = payload.data.TotalQuantity;
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
angular
    .module('homer').controller('ReportDiscountController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;



            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });

            $http.get('/webapi/CustomerApi/GetAllCustomers').success(function (data, status) {
                $scope.customers = data;

            });

            $scope.SearchDiscounts = function (deposit) {
                $scope.data = [];
                $scope.totalAmount = 0;

                var promise = $http.post('/webapi/ReportApi/GenerateDiscountsReport',
                    {
                        FromDate: deposit.FromDate,
                        ToDate: deposit.ToDate,

                    
                        SupplierId: deposit.Id,


                    });
                promise.then(
                    function (payload) {

                        $scope.data = payload.data.Deposits;
                        $scope.totalAmount = payload.data.TotalAmount;

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

angular
    .module('homer').controller('ReportBatchController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;


            $scope.BatchesForThisMonth = function () {
                $scope.data = [];
                $scope.totalMaize = 0;
                $scope.totalFactoryExpenses = 0;
                $scope.totalBrandKgs = 0;
                $scope.totalFlourkgs = 0;
                $scope.totalLabourCosts = 0;
                $scope.totalMillingBalance = 0;
                $scope.totalMillingCharge = 0;
                $scope.totalBuveraCosts = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateBatchCurrentMonthReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.Batches;
                     $scope.totalMaize = payload.data.TotalMaize;
                     $scope.totalFactoryExpenses = payload.data.TotalFactoryExpenses;
                     $scope.totalBrandKgs = payload.data.TotalBrandKgs;
                     $scope.totalFlourkgs = payload.data.TotalFlourKgs;
                     $scope.totalLabourCosts = payload.data.TotalLabourCosts;
                     $scope.totalMillingBalance = payload.data.TotalMillingBalance;
                     $scope.totalMillingCharge = payload.data.TotalMillingCharge;
                     $scope.totalBuveraCosts = payload.data.TotalBuveraCosts;

                     $scope.reportType = 2;
                    
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.TodaysBatches = function () {
                $scope.data = [];
                $scope.totalMaize = 0;
                $scope.totalFactoryExpenses = 0;
                $scope.totalBrandKgs = 0;
                $scope.totalFlourkgs = 0;
                $scope.totalLabourCosts = 0;
                $scope.totalMillingBalance = 0;
                $scope.totalMillingCharge = 0;
                $scope.totalBuveraCosts = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateBatchTodaysReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.Batches;
                     $scope.totalMaize = payload.data.TotalMaize;
                     $scope.totalFactoryExpenses = payload.data.TotalFactoryExpenses;
                     $scope.totalBrandKgs = payload.data.TotalBrandKgs;
                     $scope.totalFlourkgs = payload.data.TotalFlourKgs;
                     $scope.totalLabourCosts = payload.data.TotalLabourCosts;
                     $scope.totalMillingBalance = payload.data.TotalMillingBalance;
                     $scope.totalMillingCharge = payload.data.TotalMillingCharge;
                     $scope.totalBuveraCosts = payload.data.TotalBuveraCosts;

                  
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.WeeksBatches = function () {
                $scope.data = [];
                $scope.totalMaize = 0;
                $scope.totalFactoryExpenses = 0;
                $scope.totalBrandKgs = 0;
                $scope.totalFlourkgs = 0;
                $scope.totalLabourCosts = 0;
                $scope.totalMillingBalance = 0;
                $scope.totalMillingCharge = 0;
                $scope.totalBuveraCosts = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateBatchCurrentWeekReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.Batches;
                     $scope.totalMaize = payload.data.TotalMaize;
                     $scope.totalFactoryExpenses = payload.data.TotalFactoryExpenses;
                     $scope.totalBrandKgs = payload.data.TotalBrandKgs;
                     $scope.totalFlourkgs = payload.data.TotalFlourKgs;
                     $scope.totalLabourCosts = payload.data.TotalLabourCosts;
                     $scope.totalMillingBalance = payload.data.TotalMillingBalance;
                     $scope.totalMillingCharge = payload.data.TotalMillingCharge;
                     $scope.totalBuveraCosts = payload.data.TotalBuveraCosts;

                     $scope.reportType = 3;
                  
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }


         
          

            $scope.SearchBatches = function (batch) {
                $scope.data = [];
                $scope.totalMaize = 0;
                $scope.totalFactoryExpenses = 0;
                $scope.totalBrandKgs = 0;
                $scope.totalFlourkgs = 0;
                $scope.totalLabourCosts = 0;
                $scope.totalMillingBalance = 0;
                $scope.totalMillingCharge = 0;
                $scope.totalBuveraCosts = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllBatchesBetweenTheSpecifiedDatesForBranch',
                        {
                            FromDate: batch.FromDate,
                            ToDate: batch.ToDate,
                          
                            BranchId: batch.BranchId,

                        });
                promise.then(
                 function (payload) {

                     $scope.data = payload.data.Batches;
                     $scope.totalMaize = payload.data.TotalMaize;
                     $scope.totalFactoryExpenses = payload.data.TotalFactoryExpenses;
                     $scope.totalBrandKgs = payload.data.TotalBrandKgs;
                     $scope.totalFlourkgs = payload.data.TotalFlourKgs;
                     $scope.totalLabourCosts = payload.data.TotalLabourCosts;
                     $scope.totalMillingBalance = payload.data.TotalMillingBalance;
                     $scope.totalMillingCharge = payload.data.TotalMillingCharge;
                     $scope.totalBuveraCosts = payload.data.TotalBuveraCosts;

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

            $scope.downloadPDF = function () {
                var batchesData = [];

                var batches_branchName;
                angular.forEach($scope.data, function (value, key) {
                    batchesData.push({
                        Name: value.Name, Quantity: value.Quantity.toString(), FlourOutPut: value.FlourOutPut.toString(), BrandOutPut: value.BrandOutPut.toString(),
                        MillingCharge: value.MillingCharge.toString(), TotalFactoryExpenseCost: value.TotalFactoryExpenseCost.toString(), MillingChargeBalance: value.MillingChargeBalance.toString(),
                        TotalBuveraCost: value.TotalBuveraCost.toString(), TotalLabourCosts: value.TotalLabourCosts.toString(),
                        CreatedOn: moment(value.CreatedOn).format('DD-MMM-YYYY')

                    });

                });

                batches_branchName = $filter('uppercase')($scope.data[0].BranchName);
                var items = batchesData.map(function (item) {
                    return [item.Name, item.Quantity, item.FlourOutPut, item.BrandOutPut, item.MillingCharge, item.TotalFactoryExpenseCost,
                    item.MillingChargeBalance, item.TotalBuveraCost, item.TotalLabourCosts, item.CreatedOn];
                });

                var docDefinition = {
                    pageOrientation: 'landscape',
                    header: function () {
                        return [
                            {
                                style: 'table',
                                margin: [62, 35, 62, 35],
                                table: {
                                    widths: ['*'],
                                    headerRows: 0,
                                    body: [
                                        [
                                            { text: 'MBALE INVESTMENT BATCHES REPORT  OF ' + batches_branchName, style: 'topHeader', alignment: 'center' },


                                        ]
                                    ]
                                },
                                layout: 'noBorders'
                            }
                        ]
                    },
                    footer: function (currentPage, pageCount) {
                        return [
                            { text: currentPage.toString() + ' of ' + pageCount, alignment: 'center', style: 'footer' }
                        ]
                    },
                    content: [
                        {
                            style: 'topTable',
                            table: {
                                widths: ['*', '*', '*', '*', '*', '*', '*', '*', '*', '*'],
                                heights: [18],
                                headerRows: 1,
                                body: [


                                    [{ text: 'BATCH No.', style: 'tableLabel' }, { text: 'MAIZE(kgs) ', style: 'tableLabel' },
                                    { text: 'M/F(kgs)', style: 'tableLabel' }, { text: 'M/B(kgs)', style: 'tableLabel' },
                                    { text: 'MCHARGE', style: 'tabLabel' }, { text: 'F/EXPENSES', style: 'tableLabel' },
                                    { text: 'MBALANCE', style: 'tableLabel' }, { text: 'Buvera', style: 'tableLabel' },
                                    { text: 'LABOUR', style: 'tableLabel' }, { text: 'DATE', style: 'tableLabel' },
                                    ],

                                ].concat(items)
                            },



                            layout: {
                                paddingLeft: function (i, node) { return 6; },
                                paddingRight: function (i, node) { return 6; },
                                paddingTop: function (i, node) { return 6; },
                                paddingBottom: function (i, node) { return 6; },
                                fillColor: function (i, node) {
                                    return (i % 2 === 0) ? '#F5F5F5' : null;
                                }
                            }
                        },

                        {
                            table: {
                                widths: ['*', '*', '*', '*', '*', '*', '*', '*', '*', '*'],
                                heights: [18],
                                headerRows: 1,
                                body: [




                                    [
                                        { text: 'TOTAL', style: 'secondTable', bold: true },
                                        { text: $scope.totalMaize.toString() + ' kgs', style: 'tableLabel', bold: true },
                                        { text: $scope.totalFlourkgs.toString() + ' kgs', style: 'tableLabel', bold: true },
                                        { text: $scope.totalBrandKgs.toString() + ' kgs', style: 'tableLabel', bold: true },
                                        { text: $scope.totalMillingCharge.toString() + ' Ugx', style: 'tableLabel', bold: true },
                                        { text: $scope.totalFactoryExpenses.toString() + ' Ugx', style: 'tableLabel', bold: true },
                                        { text: $scope.totalMillingBalance.toString() + ' Ugx', style: 'tableLabel', bold: true },
                                        { text: $scope.totalBuveraCosts.toString() + ' Ugx', style: 'tableLabel', bold: true },
                                        { text: $scope.totalLabourCosts.toString() + ' Ugx', style: 'tableLabel', bold: true },
                                        { text: ' ', style: 'tableLabel', bold: true },

                                    ],

                                ]
                            },
                            layout: {
                                paddingLeft: function (i, node) { return 6; },
                                paddingRight: function (i, node) { return 6; },
                                paddingTop: function (i, node) { return 6; },
                                paddingBottom: function (i, node) { return 6; },
                                fillColor: function (i, node) {
                                    return (i % 2 === 0) ? '#F5F5F5' : null;
                                }
                            }
                        }


                    ],
                    pageSize: 'A4',
                    pageMargins: [62, 80, 62, 80],
                    styles: {
                        topHeader: {
                            fontSize: 15,
                            bold: true,
                            margin: [0, 6, 0, 30],
                            alignment: 'center'
                        },
                        secondTable: {
                            fontSize: 20.4,
                            color: 'black'
                        },
                        table: {
                            fontSize: 4,
                            alignment: 'center',
                            color: 'black',
                            margin: [0, 5, 0, 15]
                        },
                        header: {
                            fontSize: 12,
                            bold: true,
                            margin: [0, 10, 0, 15],
                            alignment: 'center'
                        },
                        footer: {
                            fontSize: 8,
                            margin: [0, 25, 0, 17],
                            alignment: 'center'
                        }
                    }
                };
                var date = new Date();
                date = moment(date).format('DD_MMM_YYYY_HH_mm_ss');
                pdfMake.createPdf(docDefinition).download('Batches_' + date + '.pdf');
                //pdfMake.createPdf(docDefinition).download("income.pdf");
            };

            $scope.DownloadExcelFile = function () {
                $window.open("/Excel/Batch/" + $scope.reportType);
            };

        }]);

angular
    .module('homer').controller('ReportDeliveryController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;
           

            $http.get('webapi/ProductApi/GetAllproducts').success(function (data, status) {
                $scope.products = data;
            });


            $scope.DeliveriesForThisMonth = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateDeliveryCurrentMonthReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 2;
                     $scope.data = payload.data.Deliveries;

                     $scope.totalAmount = payload.data.TotalAmount;
                     $scope.totalQuantity = payload.data.TotalQuantity;

                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.TodaysDeliveries = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateDeliveryTodaysReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 1;
                     $scope.data = payload.data.Deliveries;

                     $scope.totalAmount = payload.data.TotalAmount;
                     $scope.totalQuantity = payload.data.TotalQuantity;

                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.WeeksDeliveries = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateDeliveryCurrentWeekReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 3;
                     $scope.data = payload.data.Deliveries;

                     $scope.totalAmount = payload.data.TotalAmount;
                     $scope.totalQuantity = payload.data.TotalQuantity;

                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }


           

            $http.get('/webapi/CustomerApi/GetAllCustomers').success(function (data, status) {
                $scope.customers = data;
            });

            $scope.SearchDeliveries = function (delivery) {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllDeliveriesBetweenTheSpecifiedDatesForAParticularProductForBranch',
                        {
                            FromDate: delivery.FromDate,
                            ToDate: delivery.ToDate,
                            CustomerId: delivery.CustomerId,
                            BranchId: delivery.BranchId,
                            ProductId : delivery.ProductId,

                        });
                promise.then(
                 function (payload) {

                     $scope.data = payload.data;
                     $scope.reportType = 4;
                     $scope.data = payload.data.Deliveries;

                     $scope.totalAmount = payload.data.TotalAmount;
                     $scope.totalQuantity = payload.data.TotalQuantity;

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

            $scope.downloadPDF = function () {
                var deliveriesData = [];

                var deliveries_branchName;
                var deliveries_productName;

                angular.forEach($scope.data, function (value, key) {
                    deliveriesData.push({
                        Location: value.Location, VehicleNumber: value.VehicleNumber, OrderId: value.OrderId.toString(),
                        Quantity: value.Quantity.toString(), Amount: value.Amount.toString(), CustomerName: value.CustomerName,
                        CreatedOn: moment(value.CreatedOn).format('DD-MMM-YYYY')

                    });

                });

                deliveries_branchName = $filter('uppercase')($scope.data[0].BranchName);
                deliveries_productName = $filter('uppercase')($scope.data[0].ProductName);

                var items = deliveriesData.map(function (item) {
                    return [item.Location, item.VehicleNumber, item.OrderId, item.Quantity, item.Amount, item.CustomerName, item.CreatedOn];
                });

                var docDefinition = {
                    pageOrientation: 'landscape',
                    header: function () {
                        return [
                            {
                                style: 'table',
                                margin: [62, 35, 62, 35],
                                table: {
                                    widths: ['*'],
                                    headerRows: 0,
                                    body: [
                                        [
                                            { text: 'MBALE INVESTMENT DELIVERIES REPORT  OF ' + deliveries_productName + ' AT ' + deliveries_branchName, style: 'topHeader', alignment: 'center' },


                                        ]
                                    ]
                                },
                                layout: 'noBorders'
                            }
                        ]
                    },
                    footer: function (currentPage, pageCount) {
                        return [
                            { text: currentPage.toString() + ' of ' + pageCount, alignment: 'center', style: 'footer' }
                        ]
                    },
                    content: [
                        {
                            style: 'topTable',
                            table: {
                                widths: ['*', '*', '*', '*', '*', '*', '*'],
                                heights: [18],
                                headerRows: 1,
                                body: [


                                    [{ text: 'DESTINATION', style: 'tableLabel' }, { text: 'TRUCK NO.', style: 'tableLabel' },
                                    { text: 'ORDER NO.', style: 'tableLabel' }, { text: 'QUANTITY', style: 'tableLabel' },
                                    { text: 'AMOUNT', style: 'tabLabel' }, { text: 'CUSTOMER NAME', style: 'tableLabel' },
                                    { text: 'DATE', style: 'tableLabel' },
                                    ],

                                ].concat(items)
                            },



                            layout: {
                                paddingLeft: function (i, node) { return 6; },
                                paddingRight: function (i, node) { return 6; },
                                paddingTop: function (i, node) { return 6; },
                                paddingBottom: function (i, node) { return 6; },
                                fillColor: function (i, node) {
                                    return (i % 2 === 0) ? '#F5F5F5' : null;
                                }
                            }
                        },

                        {
                            table: {
                                widths: ['*', '*', '*', '*', '*', '*', '*'],
                                heights: [18],
                                headerRows: 1,
                                body: [




                                    [
                                        { text: 'TOTAL', style: 'tableLabel', bold: true },
                                        { text: ' ', style: 'tableLabel', bold: true },
                                        { text: ' ', style: 'tableLabel', bold: true },
                                        { text: $scope.totalQuantity.toString() + ' kgs', style: 'tableLabel', bold: true },
                                        { text: $scope.totalAmount.toString() + ' Ugx', style: 'tableLabel', bold: true },
                                        { text: '', style: 'tableLabel', bold: true },
                                        { text: ' ', style: 'tableLabel', bold: true },

                                    ],

                                ]
                            },
                            layout: {
                                paddingLeft: function (i, node) { return 6; },
                                paddingRight: function (i, node) { return 6; },
                                paddingTop: function (i, node) { return 6; },
                                paddingBottom: function (i, node) { return 6; },
                                fillColor: function (i, node) {
                                    return (i % 2 === 0) ? '#F5F5F5' : null;
                                }
                            }
                        }


                    ],
                    pageSize: 'A4',
                    pageMargins: [62, 80, 62, 80],
                    styles: {
                        topHeader: {
                            fontSize: 15,
                            bold: true,
                            margin: [0, 6, 0, 30],
                            alignment: 'center'
                        },
                        secondTable: {
                            fontSize: 20.4,
                            color: 'black'
                        },
                        table: {
                            fontSize: 4,
                            alignment: 'center',
                            color: 'black',
                            margin: [0, 5, 0, 15]
                        },
                        header: {
                            fontSize: 12,
                            bold: true,
                            margin: [0, 10, 0, 15],
                            alignment: 'center'
                        },
                        footer: {
                            fontSize: 8,
                            margin: [0, 25, 0, 17],
                            alignment: 'center'
                        }
                    }
                };
                var date = new Date();
                date = moment(date).format('DD_MMM_YYYY_HH_mm_ss');
                pdfMake.createPdf(docDefinition).download('deliveries_' + date + '.pdf');
                //pdfMake.createPdf(docDefinition).download("income.pdf");
            };


        }]);

angular
    .module('homer').controller('ReportCashController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;


            $scope.CashForThisMonth = function () {
                $scope.data = [];
                var promise = $http.get('/webapi/ReportApi/GenerateCashCurrentMonthReport', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 2;
                     if ($scope.data.length > 0) {
                         $scope.showDownloadLink = true;
                     }
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.TodaysCash = function () {
                $scope.data = [];
                var promise = $http.get('/webapi/ReportApi/GenerateCashTodaysReport', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 1;
                     if ($scope.data.length > 0) {
                         $scope.showDownloadLink = true;
                     }
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.WeeksCash = function () {
                $scope.data = [];
                var promise = $http.get('/webapi/ReportApi/GenerateCashCurrentWeekReport', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 3;
                     if ($scope.data.length > 0) {
                         $scope.showDownloadLink = true;
                     }
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }




            $scope.SearchCash = function (cash) {
                $scope.data = [];
                var promise = $http.post('/webapi/ReportApi/GetAllCashBetweenTheSpecifiedDatesForBranch',
                        {
                            FromDate: cash.FromDate,
                            ToDate: cash.ToDate,

                            BranchId: cash.BranchId,

                        });
                promise.then(
                 function (payload) {

                     $scope.data = payload.data;
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


            $scope.DownloadExcelFile = function () {
                $window.open("/Excel/Cash/" + $scope.reportType);
            };

        }]);

angular
    .module('homer').controller('ReportOrderController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;


            $scope.OrdersForThisMonth = function () {
                $scope.data = [];
                var promise = $http.get('/webapi/ReportApi/GenerateOrderCurrentMonthReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 2;
                     if ($scope.data.length > 0) {
                         $scope.showDownloadLink = true;
                     }
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.TodaysOrders = function () {
                $scope.data = [];
                var promise = $http.get('/webapi/ReportApi/GenerateOrderTodaysReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 1;
                     if ($scope.data.length > 0) {
                         $scope.showDownloadLink = true;
                     }
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.WeeksOrders = function () {
                $scope.data = [];
                var promise = $http.get('/webapi/ReportApi/GenerateOrderCurrentWeekReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 3;
                     if ($scope.data.length > 0) {
                         $scope.showDownloadLink = true;
                     }
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }


         

            $http.get('/webapi/CustomerApi/GetAllCustomers').success(function (data, status) {
                $scope.customers = data;
            });

            $scope.SearchOrders = function (order) {
                $scope.data = [];
                var promise = $http.post('/webapi/ReportApi/GetAllOrdersBetweenTheSpecifiedDatesForBranch',
                        {
                            FromDate: order.FromDate,
                            ToDate: order.ToDate,
                            CustomerId: order.CustomerId,
                            BranchId: order.BranchId,

                        });
                promise.then(
                 function (payload) {

                     $scope.data = payload.data;
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


            $scope.DownloadExcelFile = function () {
                $window.open("/Excel/Order/" + $scope.reportType);
            };

        }]);

angular
    .module('homer').controller('ReportFactoryExpenseController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;
            var branches = [];
            var selectedBranch;

            $scope.FactoryExpenseForThisMonth = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateFactoryExpenseCurrentMonthReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.FactoryExpenses;
                     $scope.totalAmount = payload.data.TotalAmount;
                     $scope.reportType = 2;
                   
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.TodaysFactoryExpense = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateFactoryExpenseTodaysReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.FactoryExpenses;
                     $scope.totalAmount = payload.data.TotalAmount;
                     $scope.reportType = 1;
                   
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.WeeksFactoryExpense = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateFactoryExpenseCurrentWeekReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.FactoryExpenses;
                     $scope.totalAmount = payload.data.TotalAmount;
                     $scope.reportType = 3;
                    
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

        
            $scope.SearchFactoryExpense = function (factoryExpense) {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllFactoryExpensesBetweenTheSpecifiedDatesForBranch',
                        {
                            FromDate: factoryExpense.FromDate,
                            ToDate: factoryExpense.ToDate,

                            BranchId: factoryExpense.BranchId,

                        });
                promise.then(
                 function (payload) {

                     $scope.data = payload.data.FactoryExpenses;
               
                     $scope.totalAmount = payload.data.TotalAmount;

                    
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

angular
    .module('homer').controller('ReportOtherExpenseController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;


            $scope.OtherExpenseForThisMonth = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateOtherExpenseCurrentMonthReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 2;
                     if ($scope.data.length > 0) {
                         $scope.showDownloadLink = true;

                         angular.forEach($scope.data, function (value, key) {
                             $scope.totalAmount = value.Amount + $scope.totalAmount;

                         });
                     }
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.TodaysOtherExpense = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateOtherExpenseTodaysReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 1;
                     if ($scope.data.length > 0) {
                         $scope.showDownloadLink = true;

                         angular.forEach($scope.data, function (value, key) {
                             $scope.totalAmount = value.Amount + $scope.totalAmount;

                         });
                     }
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.WeeksOtherExpense = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateOtherExpenseCurrentWeekReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 3;
                     if ($scope.data.length > 0) {
                         $scope.showDownloadLink = true;
                         angular.forEach($scope.data, function (value, key) {
                             $scope.totalAmount = value.Amount + $scope.totalAmount;

                         });
                     }
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }


          
            $scope.SearchOtherExpense = function (otherExpense) {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllOtherExpensesBetweenTheSpecifiedDatesForBranch',
                        {
                            FromDate: otherExpense.FromDate,
                            ToDate: otherExpense.ToDate,

                            BranchId: otherExpense.BranchId,

                        });
                promise.then(
                 function (payload) {

                     $scope.data = payload.data;
                     angular.forEach($scope.data, function (value, key) {
                         $scope.totalAmount = value.Amount + $scope.totalAmount;

                     });
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


            $scope.DownloadExcelFile = function () {
                $window.open("/Excel/OtherExpense/" + $scope.reportType);
            };

        }]);

angular
    .module('homer').controller('ReportFlourTransferController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;


            $scope.status = ["Accepted", "Rejected","OutStanding"];
            $scope.position = ["Transfered", "Received"];


            $scope.FlourTransferForThisMonth = function () {
                $scope.data = [];
                $scope.totalQuantity = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateFlourTransferCurrentMonthReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.FlourTransfers;
                     $scope.totalQuantity = payload.data.TotalQuantity;

                     $scope.reportType = 2;
                    
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.TodaysFlourTransfer = function () {
                $scope.data = [];
                $scope.totalQuantity = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateFlourTransferTodaysReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.FlourTransfers;
                     $scope.totalQuantity = payload.data.TotalQuantity;

                     $scope.reportType = 1;
                   
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.WeeksFlourTransfer = function () {
                $scope.data = [];
                $scope.totalQuantity = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateFlourTransferCurrentWeekReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.FlourTransfers;
                     $scope.totalQuantity = payload.data.TotalQuantity;

                     $scope.reportType = 3;
                    
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }



            $scope.SearchFlourTransfer = function (flourTransfer) {
                $scope.data = [];
                $scope.totalQuantity = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllFlourTransfersBetweenTheSpecifiedDatesForBranch',
                        {
                            FromDate: flourTransfer.FromDate,
                            ToDate: flourTransfer.ToDate,
                            Status: flourTransfer.Status,
                            BranchId: flourTransfer.BranchId,
                            Position : flourTransfer.Position,

                        });
                promise.then(
                 function (payload) {

                     $scope.data = payload.data.FlourTransfers;
                     $scope.totalQuantity = payload.data.TotalQuantity;

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


angular
    .module('homer').controller('ReportBuveraTransferController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;


            $scope.status = ["Accepted", "Rejected", "OutStanding"];
            $scope.position = ["Transfered", "Received"];


         
       

            $scope.SearchFlourTransfer = function (flourTransfer) {
                $scope.data = [];
                $scope.totalQuantity = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllFlourTransfersBetweenTheSpecifiedDatesForBranch',
                        {
                            FromDate: flourTransfer.FromDate,
                            ToDate: flourTransfer.ToDate,
                            Status: flourTransfer.Status,
                            BranchId: flourTransfer.BranchId,
                            Position: flourTransfer.Position,

                        });
                promise.then(
                 function (payload) {

                     $scope.data = payload.data.FlourTransfers;
                     $scope.totalQuantity = payload.data.TotalQuantity;

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

angular
    .module('homer').controller('ReportUtilityController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;


            $scope.UtilityForThisMonth = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateUtilityCurrentMonthReport', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 2;
                     if ($scope.data.length > 0) {
                         $scope.showDownloadLink = true;
                         angular.forEach($scope.data, function (value, key) {
                             $scope.totalAmount = value.Amount + $scope.totalAmount;

                         });
                     }
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.TodaysUtility = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateUtilityTodaysReport', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 1;
                     if ($scope.data.length > 0) {
                         $scope.showDownloadLink = true;
                         angular.forEach($scope.data, function (value, key) {
                             $scope.totalAmount = value.Amount + $scope.totalAmount;

                         });
                     }
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.WeeksUtility = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateUtilityCurrentWeekReport', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 3;
                     if ($scope.data.length > 0) {
                         $scope.showDownloadLink = true;
                         angular.forEach($scope.data, function (value, key) {
                             $scope.totalAmount = value.Amount + $scope.totalAmount;

                         });
                     }
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }


            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });



            $scope.SearchUtility = function (utility) {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllUtilitiesBetweenTheSpecifiedDates',
                        {
                            FromDate: utility.FromDate,
                            ToDate: utility.ToDate,

                            BranchId: utility.BranchId,

                        });
                promise.then(
                 function (payload) {

                     $scope.data = payload.data;
                     angular.forEach($scope.data, function (value, key) {
                         $scope.totalAmount = value.Amount + $scope.totalAmount;

                     });
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


            $scope.DownloadExcelFile = function () {
                $window.open("/Excel/Utility/" + $scope.reportType);
            };

        }]);


angular
    .module('homer').controller('ReportMachineRepairController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;


            $scope.MachineRepairForThisMonth = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateMachineRepairCurrentMonthReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 2;
                     if ($scope.data.length > 0) {
                         $scope.showDownloadLink = true;
                         angular.forEach($scope.data, function (value, key) {
                             $scope.totalAmount = value.Amount + $scope.totalAmount;

                         });
                     }
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.TodaysMachineRepair = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateMachineRepairTodaysReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 1;
                     if ($scope.data.length > 0) {
                         $scope.showDownloadLink = true;
                         angular.forEach($scope.data, function (value, key) {
                             $scope.totalAmount = value.Amount + $scope.totalAmount;

                         });
                     }
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.WeeksMachineRepair = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateMachineRepairCurrentWeekReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 3;
                     if ($scope.data.length > 0) {
                         $scope.showDownloadLink = true;
                         angular.forEach($scope.data, function (value, key) {
                             $scope.totalAmount = value.Amount + $scope.totalAmount;

                         });
                     }
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }




            $scope.SearchMachineRepair = function (machineRepair) {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllMachineRepairsBetweenTheSpecifiedDatesForBranch',
                        {
                            FromDate: machineRepair.FromDate,
                            ToDate: machineRepair.ToDate,

                            BranchId: machineRepair.BranchId,

                        });
                promise.then(
                 function (payload) {

                     $scope.data = payload.data;
                     angular.forEach($scope.data, function (value, key) {
                         $scope.totalAmount = value.Amount + $scope.totalAmount;

                     });
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


            $scope.DownloadExcelFile = function () {
                $window.open("/Excel/MachineRepair/" + $scope.reportType);
            };

        }]);

angular
    .module('homer').controller('ReportBatchOutPutController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
           

            $scope.BatchOutPutForThisMonth = function () {
                $scope.data = [];
               
                var promise = $http.get('/webapi/ReportApi/GenerateBatchCurrentMonthReportForBranch', {});
               
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.Batches;
                   
                     $scope.reportType = 2;

                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.TodaysBatchOutPut = function () {
                $scope.data = [];
               
                var promise = $http.get('/webapi/ReportApi/GenerateBatchTodaysReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.Batches;
                    

                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.WeeksBatchOutPut = function () {
                $scope.data = [];
              
                var promise = $http.get('/webapi/ReportApi/GenerateBatchCurrentWeekReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.Batches;
                   
                     $scope.reportType = 3;

                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.SearchBatchOutPut = function (batchOutPut) {
                $scope.data = [];

                var promise = $http.post('/webapi/ReportApi/GetAllBatchesBetweenTheSpecifiedDatesForBranch',
                    {
                        FromDate: batchOutPut.FromDate,
                        ToDate: batchOutPut.ToDate,

                        BranchId: batchOutPut.BranchId,

                    });
                promise.then(
                    function (payload) {

                        $scope.data = payload.data.Batches;

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



 //           $scope.SearchBatchOutPut = function (batchOutPut) {
 //               $scope.data = [];
               
 //              // var promise = $http.post('/webapi/ReportApi/GetAllBatchesBetweenTheSpecifiedDatesForBranch',
 //var promise = $http.post('/webapi/ReportApi/GetAllBatchOutPutsBetweenTheSpecifiedDatesForBranch',
                                    
 //     {
 //                           FromDate: batchOutPut.FromDate,
 //                           ToDate: batchOutPut.ToDate,

 //                           BranchId: batchOutPut.BranchId,

 //                       });
 //               promise.then(
 //                function (payload) {

 //                    $scope.data = payload.data.Batches;
                   
 //                    $scope.reportType = 4;

 //                    $scope.tableParams = new ngTableParams({
 //                        page: 1,
 //                        count: 10,
 //                        sorting: { CreatedOn: 'desc' }
 //                    },
 //                        {
 //                        getData: function ($defer, params) {
 //                            var filteredData = $filter('filter')($scope.data, $scope.filter);
 //                            var orderedData = params.sorting() ?
 //                                                $filter('orderBy')(filteredData, params.orderBy()) :
 //                                                filteredData;

 //                            params.total(orderedData.length);
 //                            $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
 //                        },
 //                        $scope: $scope

 //                    });
 //                });
 //           }


            $scope.downloadPDF = function () {
                var batchOutputData = [];

                var branch_Name;



                angular.forEach($scope.data, function (value, key) {
                    batchOutputData.push({
                        Loss: value.Loss, LossPercentage: value.LossPercentage, FlourOutPut: value.FlourOutPut,
                        FlourPercentage: value.FlourPercentage, BrandOutPut: value.BrandOutPut, BrandPercentage: value.BrandPercentage,
                        Name: value.Name, BranchName: value.BranchName, CreatedOn: moment(value.CreatedOn).format('DD-MMM-YYYY')
                    });


                });

                branch_Name = $filter('uppercase')(batchOutputData[0].BranchName);

                //angular.forEach(batchOutputData[0], function (value, key) {
                //    branch_Name = value.BranchName;
                //    //batchOutputData.push({
                //    //    Loss: value.Loss, LossPercentage: value.LossPercentage, FlourOutPut: value.FlourOutPut,
                //    //    FlourPercentage: value.FlourPercentage, BrandOutPut: value.BrandOutPut, BrandPercentage: value.BrandPercentage,
                //    //    Name: value.Name, BranchName: value.BranchName, CreatedOn: moment(value.CreatedOn).format('DD-MMM-YYYY')
                //    //});

                //});


                var items = batchOutputData.map(function (item) {
                    return [item.Loss.toString(), item.LossPercentage.toFixed(2).toString(), item.FlourOutPut.toString(), item.FlourPercentage.toFixed(2).toString(), item.BrandOutPut.toString(),
                    item.BrandPercentage.toFixed(2).toString(), item.Name, item.CreatedOn
                    ];
                });

                var docDefinition = {
                    pageOrientation: 'landscape',
                    header: function () {
                        return [
                            {
                                style: 'table',
                                margin: [62, 35, 62, 35],
                                table: {
                                    widths: ['*'],
                                    headerRows: 0,
                                    body: [
                                        [
                                            { text: 'MBALE INVESTMENT BATCHOUPUTS\' REPORT  OF ' + branch_Name + ' BRANCH', style: 'topHeader', alignment: 'center', uppercase: true },


                                        ]
                                    ]
                                },
                                layout: 'noBorders'
                            }
                        ]
                    },
                    footer: function (currentPage, pageCount) {
                        return [
                            { text: currentPage.toString() + ' of ' + pageCount, alignment: 'center', style: 'footer' }
                        ]
                    },
                    content: [
                        {
                            style: 'topTable',
                            table: {
                                widths: ['*', '*', '*', '*', '*', '*', '*', '*'],
                                heights: [18],
                                headerRows: 1,
                                body: [

                                    [
                                        { text: 'LOSS' + '(kgs)', style: 'tabLabel', bold: true }, { text: 'LOSS' + '(%)', style: 'tabLabel', bold: true },
                                        { text: 'FLOUR' + '(kgs)', style: 'tabLabel', bold: true }, { text: 'FLOUR' + '(%)', style: 'tabLabel', bold: true },
                                        { text: 'BRAND' + '(kgs)', style: 'tabLabel', bold: true }, { text: 'BRAND' + '(%)', style: 'tabLabel', bold: true },
                                        { text: 'BATCH', style: 'tabLabel', bold: true },
                                        { text: 'DATE', style: 'tableLabel', bold: true },
                                    ],

                                ].concat(items)
                            },



                            layout: {
                                paddingLeft: function (i, node) { return 6; },
                                paddingRight: function (i, node) { return 6; },
                                paddingTop: function (i, node) { return 6; },
                                paddingBottom: function (i, node) { return 6; },
                                fillColor: function (i, node) {
                                    return (i % 2 === 0) ? '#F5F5F5' : null;
                                }
                            }
                        },

                    ],
                    pageSize: 'A4',
                    pageMargins: [62, 80, 62, 80],
                    styles: {
                        topHeader: {
                            fontSize: 15,
                            bold: true,
                            margin: [0, 6, 0, 30],
                            alignment: 'center'
                        },
                        table: {
                            fontSize: 4,
                            alignment: 'center',
                            color: 'black',
                            margin: [0, 5, 0, 15]
                        },
                        header: {
                            fontSize: 12,
                            bold: true,
                            margin: [0, 10, 0, 15],
                            alignment: 'center'
                        },
                        footer: {
                            fontSize: 8,
                            margin: [0, 25, 0, 17],
                            alignment: 'center'
                        }
                    }
                };
                var date = new Date();
                date = moment(date).format('DD_MMM_YYYY_HH_mm_ss');
                pdfMake.createPdf(docDefinition).download('BATCHOUTPUT_' + date + '.pdf');
            };


          

        }]);

angular
    .module('homer').controller('ReportLabourCostController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;


            $scope.LabourCostForThisMonth = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateLabourCostCurrentMonthReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.LabourCosts;
                     $scope.totalAmount = payload.data.TotalAmount;
                    
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.TodaysLabourCost = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateLabourCostTodaysReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.LabourCosts;
                     $scope.totalAmount = payload.data.TotalAmount; 
                    
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.WeeksLabourCost = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateLabourCostCurrentWeekReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.LabourCosts;
                     $scope.totalAmount = payload.data.TotalAmount;
                    
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }




            $scope.SearchLabourCost = function (labourCost) {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllLabourCostsBetweenTheSpecifiedDatesForBranch',
                        {
                            FromDate: labourCost.FromDate,
                            ToDate: labourCost.ToDate,

                            BranchId: labourCost.BranchId,

                        });
                promise.then(
                 function (payload) {

                     $scope.data = payload.data.LabourCosts;
                     $scope.totalAmount = payload.data.TotalAmount;
                   
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


            //$scope.DownloadExcelFile = function () {
            //    $window.open("/Excel/LabourCost/" + $scope.reportType);
            //};

        }]);

angular
    .module('homer').controller('ReportCasualWorkerAccountTransactionController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;
            $scope.accounts = [];

           
         


            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });

            $http.get('/webapi/CasualWorkerApi/GetAllCasualWorkers').success(function (data, status) {
                $scope.casualWorkers = data;
                $scope.accounts = $scope.accounts.concat(data);
            });

           
            $scope.SearchCasualAccountTransactions = function (accountTransaction) {
                $scope.data = [];

                var promise = $http.post('/webapi/ReportApi/GenerateCasualAccountTransactionsBetweenTheSpecifiedDates',
                        {
                            FromDate: accountTransaction.FromDate,
                            ToDate: accountTransaction.ToDate,
                            SupplierId: accountTransaction.CasualWorkerId,
                            BranchId: accountTransaction.BranchId,

                        });
                promise.then(
                 function (payload) {

                     $scope.data = payload.data;
                     $scope.reportType = 4;

                     $scope.tableParams = new ngTableParams({
                         page: 1,
                         count: 10,

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


            $scope.DownloadExcelFile = function () {
                $window.open("/Excel/CasualTransactionsS/" + $scope.reportType);
            };

        }]);
angular
    .module('homer').controller('ReportCashSaleController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            
            $http.get('webapi/ProductApi/GetAllproducts').success(function (data, status) {
                $scope.products = data;
            });


            $scope.CashSalesForThisMonth = function () {
                $scope.data = [];
               
                $scope.totalAmount = 0;
                $scope.totalQuantity = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateCashSaleCurrentMonthReportForBranch', {});
               
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.CashSales;
                     $scope.totalAmount = payload.data.TotalAmount;
                     $scope.totalQuantity = payload.data.TotalQuantity;
                     $scope.reportType = 2;
                    
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.TodaysCashSales = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                $scope.totalQuantity = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateCashSaleTodaysReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.CashSales;
                     $scope.totalAmount = payload.data.TotalAmount;
                     $scope.totalQuantity = payload.data.TotalQuantity;
                     $scope.reportType = 1;
                    
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.WeeksCashSales = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                $scope.totalQuantity = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateCashSaleCurrentWeekReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.CashSales;
                     $scope.totalAmount = payload.data.TotalAmount;
                     $scope.totalQuantity = payload.data.TotalQuantity;
                     $scope.reportType = 3;
                   
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }


         


            $scope.SearchCashSales = function (cashSale) {
                $scope.data = [];
                $scope.totalAmount = 0;
                $scope.totalQuantity = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllCashSalesBetweenTheSpecifiedDatesForParticularProductForBranch',
                        {
                            FromDate: cashSale.FromDate,
                            ToDate: cashSale.ToDate,
                            
                            BranchId: cashSale.BranchId,
                            ProductId : cashSale.ProductId,
                        });
                promise.then(
                 function (payload) {

                     $scope.data = payload.data.CashSales;
                     $scope.totalAmount = payload.data.TotalAmount;
                     $scope.totalQuantity = payload.data.TotalQuantity;
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

angular
    .module('homer').controller('ReportCashTransferController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;


            $scope.status = ["Accepted", "Rejected", "OutStanding"];
            $scope.position = ["Transfered", "Received"];


            $scope.CashTransfersForThisMonth = function () {
                $scope.data = [];

                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateCashTransferCurrentMonthReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.CashTransfers;
                     $scope.totalAmount = payload.data.TotalAmount;
                     $scope.reportType = 2;
                 
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.TodaysCashTransfers = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateCashTransferTodaysReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.CashTransfers;
                     $scope.totalAmount = payload.data.TotalAmount;
                     $scope.reportType = 1;
                  
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }

            $scope.WeeksCashTransfers = function () {
                $scope.data = [];
                $scope.totalAmount = 0;

                var promise = $http.get('/webapi/ReportApi/GenerateCashTransferCurrentWeekReportForBranch', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.CashTransfers;
                     $scope.totalAmount = payload.data.TotalAmount;
                     $scope.reportType = 3;
                 
                     $scope.tableParams = new ngTableParams({ page: 1, count: 20, sorting: { CreatedOn: 'desc' } }, {
                         total: $scope.data.length, getData: function ($defer, params) {
                             var orderData = params.sorting() ?
                                                 $filter('orderBy')($scope.data, params.orderBy()) :
                                                 $scope.data;
                             $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                         }
                     });
                 });
            }




            $scope.SearchCashTransfers = function (cashTransfer) {
                $scope.data = [];
                $scope.totalAmount = 0;

                var promise = $http.post('/webapi/ReportApi/GetAllCashTransfersBetweenTheSpecifiedDatesForBranch',
                        {
                            FromDate: cashTransfer.FromDate,
                            ToDate: cashTransfer.ToDate,

                            BranchId: cashTransfer.BranchId,
                            Status: cashTransfer.Status,
                            
                            Position: cashTransfer.Position,


                        });
                promise.then(
                 function (payload) {

                     $scope.data = payload.data.CashTransfers;
                     $scope.totalAmount = payload.data.TotalAmount;
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

            $scope.downloadPDF = function () {
                var cashTransferData = [];

                //var batches_branchName;
                angular.forEach($scope.data, function (value, key) {
                    cashTransferData.push({
                        FromBranch: value.FromBranch, ReceiverBranch: value.ReceiverBranch, Amount: value.Amount,
                        Notes: value.Notes, Accept: value.Accept, Reject: value.Reject,
                        CreatedOn: moment(value.CreatedOn).format('DD-MMM-YYYY')

                    });

                });

                // batches_branchName = $filter('uppercase')($scope.data[0].BranchName);
                var items = cashTransferData.map(function (item) {
                    return [item.FromBranch, item.ReceiverBranch, item.Amount.toString(), item.Notes, item.Accept.toString(), item.Reject.toString(), item.CreatedOn];
                });

                var docDefinition = {
                    pageOrientation: 'landscape',
                    header: function () {
                        return [
                            {
                                style: 'table',
                                margin: [62, 35, 62, 35],
                                table: {
                                    widths: ['*'],
                                    headerRows: 0,
                                    body: [
                                        [
                                            { text: 'MBALE INVESTMENT CASHTRANSFER  REPORT', style: 'topHeader', alignment: 'center' },


                                        ]
                                    ]
                                },
                                layout: 'noBorders'
                            }
                        ]
                    },
                    footer: function (currentPage, pageCount) {
                        return [
                            { text: currentPage.toString() + ' of ' + pageCount, alignment: 'center', style: 'footer' }
                        ]
                    },
                    content: [
                        {
                            style: 'topTable',
                            table: {
                                widths: ['*', '*', '*', '*', '*', '*', '*'],
                                heights: [18],
                                headerRows: 1,
                                body: [


                                    [{ text: 'FROM', style: 'tableLabel' }, { text: 'RECIEVING', style: 'tableLabel' },
                                    { text: 'AMOUNT', style: 'tableLabel' }, { text: 'NOTES', style: 'tableLabel' },
                                    { text: 'ACCEPTED', style: 'tabLabel' }, { text: 'REJECTED', style: 'tableLabel' },
                                    { text: 'DATE', style: 'tableLabel' },
                                    ],

                                ].concat(items)
                            },



                            layout: {
                                paddingLeft: function (i, node) { return 6; },
                                paddingRight: function (i, node) { return 6; },
                                paddingTop: function (i, node) { return 6; },
                                paddingBottom: function (i, node) { return 6; },
                                fillColor: function (i, node) {
                                    return (i % 2 === 0) ? '#F5F5F5' : null;
                                }
                            }
                        },

                        {
                            table: {
                                widths: ['*', '*', '*', '*', '*', '*', '*'],
                                heights: [18],
                                headerRows: 1,
                                body: [




                                    [
                                        { text: 'TOTAL', style: 'secondTable', bold: true },
                                        { text: ' ', style: 'tableLabel', bold: true },
                                        { text: $scope.totalAmount.toString() + ' Ugx', style: 'tableLabel', bold: true },
                                        { text: ' ', style: 'tableLabel', bold: true },
                                        { text: ' ', style: 'tableLabel', bold: true },

                                        { text: '', style: 'tableLabel', bold: true },
                                        { text: ' ', style: 'tableLabel', bold: true },

                                    ],

                                ]
                            },
                            layout: {
                                paddingLeft: function (i, node) { return 6; },
                                paddingRight: function (i, node) { return 6; },
                                paddingTop: function (i, node) { return 6; },
                                paddingBottom: function (i, node) { return 6; },
                                fillColor: function (i, node) {
                                    return (i % 2 === 0) ? '#F5F5F5' : null;
                                }
                            }
                        }


                    ],
                    pageSize: 'A4',
                    pageMargins: [62, 80, 62, 80],
                    styles: {
                        topHeader: {
                            fontSize: 15,
                            bold: true,
                            margin: [0, 6, 0, 30],
                            alignment: 'center'
                        },
                        secondTable: {
                            fontSize: 20.4,
                            color: 'black'
                        },
                        table: {
                            fontSize: 4,
                            alignment: 'center',
                            color: 'black',
                            margin: [0, 5, 0, 15]
                        },
                        header: {
                            fontSize: 12,
                            bold: true,
                            margin: [0, 10, 0, 15],
                            alignment: 'center'
                        },
                        footer: {
                            fontSize: 8,
                            margin: [0, 25, 0, 17],
                            alignment: 'center'
                        }
                    }
                };
                var date = new Date();
                date = moment(date).format('DD_MMM_YYYY_HH_mm_ss');
                pdfMake.createPdf(docDefinition).download('CashTransfers_' + date + '.pdf');
                //pdfMake.createPdf(docDefinition).download("income.pdf");
            };

           

        }]);
angular
    .module('homer').controller('ReportExpenseController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;


            $http.get('/webapi/RequistionApi/GetAllRequistionCategories').success(function (data, status) {
                $scope.requistionCategories = data;
            });
          
          
            $scope.SearchCash = function (cash) {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllExpensesBetweenTheSpecifiedDatesForBranchForAParticularRequistionCategory',
                        {
                            FromDate: cash.FromDate,
                            ToDate: cash.ToDate,
                            RequistionCategoryId : cash.RequistionCategoryId,
                            BranchId: cash.BranchId,

                        });
                promise.then(
                 function (payload) {

                     $scope.data = payload.data.Cashs;
                     $scope.totalAmount = payload.data.TotalAmount;
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


            $scope.downloadPDF = function () {
                var expensesData = [];

                var branch_name;

                angular.forEach($scope.data, function (value, key) {
                    expensesData.push({
                        Notes: value.Notes,
                        Amount: value.Amount, CreatedOn: moment(value.CreatedOn).format('DD-MMM-YYYY')

                    });

                });

                branch_name = $filter('uppercase')($scope.data[0].BranchName)

                var items = expensesData.map(function (item) {
                    return [item.Notes, item.Amount.toString(), item.CreatedOn];
                });

                var docDefinition = {
                    pageOrientation: 'landscape',
                    header: function () {
                        return [
                            {
                                style: 'table',
                                margin: [62, 35, 62, 35],
                                table: {
                                    widths: ['*'],
                                    headerRows: 0,
                                    body: [
                                        [
                                            { text: 'MBALE INVESTMENT EXPENSES\'S REPORT  OF ' + branch_name, style: 'topHeader', alignment: 'center' },


                                        ]
                                    ]
                                },
                                layout: 'noBorders'
                            }
                        ]
                    },
                    footer: function (currentPage, pageCount) {
                        return [
                            { text: currentPage.toString() + ' of ' + pageCount, alignment: 'center', style: 'footer' }
                        ]
                    },
                    content: [
                        {
                            style: 'topTable',
                            table: {
                                widths: ['*', '*', '*'],
                                heights: [18],
                                headerRows: 1,
                                body: [

                                    [
                                        { text: 'NOTES', style: 'tableLabel' },
                                        { text: 'AMOUNT', style: 'tableLabel' }, { text: 'DATE', style: 'tableLabel' },
                                    ],

                                ].concat(items)
                            },



                            layout: {
                                paddingLeft: function (i, node) { return 6; },
                                paddingRight: function (i, node) { return 6; },
                                paddingTop: function (i, node) { return 6; },
                                paddingBottom: function (i, node) { return 6; },
                                fillColor: function (i, node) {
                                    return (i % 2 === 0) ? '#F5F5F5' : null;
                                }
                            }
                        },

                        {
                            table: {
                                widths: ['*', '*', '*'],
                                heights: [18],
                                headerRows: 1,
                                body: [




                                    [
                                        { text: 'TOTAL', style: 'tableLabel', bold: true },
                                        { text: $scope.totalAmount.toString() + 'Ugshs', style: 'tableLabel', bold: true }, { text: '', style: 'tableLabel' },

                                    ],

                                ]
                            },
                            layout: {
                                paddingLeft: function (i, node) { return 6; },
                                paddingRight: function (i, node) { return 6; },
                                paddingTop: function (i, node) { return 6; },
                                paddingBottom: function (i, node) { return 6; },
                                fillColor: function (i, node) {
                                    return (i % 2 === 0) ? '#F5F5F5' : null;
                                }
                            }
                        }


                    ],
                    pageSize: 'A4',
                    pageMargins: [62, 80, 62, 80],
                    styles: {
                        topHeader: {
                            fontSize: 15,
                            bold: true,
                            margin: [0, 6, 0, 30],
                            alignment: 'center'
                        },
                        table: {
                            fontSize: 4,
                            alignment: 'center',
                            color: 'black',
                            margin: [0, 5, 0, 15]
                        },
                        header: {
                            fontSize: 12,
                            bold: true,
                            margin: [0, 10, 0, 15],
                            alignment: 'center'
                        },
                        footer: {
                            fontSize: 8,
                            margin: [0, 25, 0, 17],
                            alignment: 'center'
                        }
                    }
                };
                var date = new Date();
                date = moment(date).format('DD_MMM_YYYY_HH_mm_ss');
                pdfMake.createPdf(docDefinition).download('expenses_' + date + '.pdf');
                //pdfMake.createPdf(docDefinition).download("expenses.pdf");
            };


            $scope.DownloadExcelFile = function () {
                $window.open("/Excel/Cash/" + $scope.reportType);
            };

        }]);
angular
    .module('homer').controller('ReportIncomeController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;



            $scope.SearchCash = function (cash) {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllIncomesBetweenTheSpecifiedDatesForBranch',
                    {
                        FromDate: cash.FromDate,
                        ToDate: cash.ToDate,

                        BranchId: cash.BranchId,

                    });
                promise.then(
                    function (payload) {

                        $scope.data = payload.data.Cashs;
                        $scope.totalAmount = payload.data.TotalAmount;
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

                $scope.downloadPDF = function () {
                    var incomeData = [];

                    var income_branchName;
                    angular.forEach($scope.data, function (value, key) {
                        incomeData.push({
                            TransactionSubTypeName: value.TransactionSubTypeName, Notes: value.Notes,
                            Amount: value.Amount, CreatedOn: moment(value.CreatedOn).format('DD-MMM-YYYY')

                        });

                    });

                    income_branchName = $filter('uppercase')($scope.data[0].BranchName);
                    var items = incomeData.map(function (item) {
                        return [item.TransactionSubTypeName, item.Notes, item.Amount.toString(), item.CreatedOn];
                    });

                    var docDefinition = {
                        pageOrientation: 'landscape',
                        header: function () {
                            return [
                                {
                                    style: 'table',
                                    margin: [62, 35, 62, 35],
                                    table: {
                                        widths: ['*'],
                                        headerRows: 0,
                                        body: [
                                            [
                                                { text: 'MBALE INVESTMENT INCOMES\' REPORT  OF ' + income_branchName, style: 'topHeader', alignment: 'center' },


                                            ]
                                        ]
                                    },
                                    layout: 'noBorders'
                                }
                            ]
                        },
                        footer: function (currentPage, pageCount) {
                            return [
                                { text: currentPage.toString() + ' of ' + pageCount, alignment: 'center', style: 'footer' }
                            ]
                        },
                        content: [
                            {
                                style: 'topTable',
                                table: {
                                    widths: ['*', '*', '*', '*'],
                                    heights: [18],
                                    headerRows: 1,
                                    body: [

                                        [
                                            { text: 'TRANSACTIONN NAME', style: 'tabLabel' },
                                            { text: 'NOTES', style: 'tableLabel' },
                                            { text: 'AMOUNT', style: 'tableLabel' }, { text: 'DATE', style: 'tableLabel' },
                                        ],

                                    ].concat(items)
                                },



                                layout: {
                                    paddingLeft: function (i, node) { return 6; },
                                    paddingRight: function (i, node) { return 6; },
                                    paddingTop: function (i, node) { return 6; },
                                    paddingBottom: function (i, node) { return 6; },
                                    fillColor: function (i, node) {
                                        return (i % 2 === 0) ? '#F5F5F5' : null;
                                    }
                                }
                            },

                            {
                                table: {
                                    widths: ['*', '*', '*', '*'],
                                    heights: [18],
                                    headerRows: 1,
                                    body: [




                                        [
                                            { text: 'TOTAL', style: 'tableLabel', bold: true }, { text: '', style: 'tableLabel', bold: true },
                                            { text: $scope.totalAmount.toString() + ' Ug shs', style: 'tableLabel', bold: true }, { text: '', style: 'tableLabel' },

                                        ],

                                    ]
                                },
                                layout: {
                                    paddingLeft: function (i, node) { return 6; },
                                    paddingRight: function (i, node) { return 6; },
                                    paddingTop: function (i, node) { return 6; },
                                    paddingBottom: function (i, node) { return 6; },
                                    fillColor: function (i, node) {
                                        return (i % 2 === 0) ? '#F5F5F5' : null;
                                    }
                                }
                            }


                        ],
                        pageSize: 'A4',
                        pageMargins: [62, 80, 62, 80],
                        styles: {
                            topHeader: {
                                fontSize: 15,
                                bold: true,
                                margin: [0, 6, 0, 30],
                                alignment: 'center'
                            },
                            table: {
                                fontSize: 4,
                                alignment: 'center',
                                color: 'black',
                                margin: [0, 5, 0, 15]
                            },
                            header: {
                                fontSize: 12,
                                bold: true,
                                margin: [0, 10, 0, 15],
                                alignment: 'center'
                            },
                            footer: {
                                fontSize: 8,
                                margin: [0, 25, 0, 17],
                                alignment: 'center'
                            }
                        }
                    };
                    var date = new Date();
                    date = moment(date).format('DD_MMM_YYYY_HH_mm_ss');
                    pdfMake.createPdf(docDefinition).download('incomes_' + date + '.pdf');
                    //pdfMake.createPdf(docDefinition).download("income.pdf");
                };


            //$scope.DownloadExcelFile = function () {
            //    $window.open("/Excel/Cash/" + $scope.reportType);
            //};

            }]);

angular
    .module('homer').controller('ReportBuveraTransferController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;

            $scope.status = [{ Name: "Accepted", value: "Accepted" },
                { Name: "Rejected", value: "Rejected" },
                { Name: "OutStanding", value: "OutStanding" }];

            $scope.position = ["Transfered", "Received"];


            $scope.SearchBuveraTransfers = function (buveraTransfer) {
                $scope.data = [];
                $scope.totalQuantity = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllBuveraTransfersBetweenTheSpecifiedDatesForBranch',
                    {
                        FromDate: buveraTransfer.FromDate,
                        ToDate: buveraTransfer.ToDate,
                        Status: buveraTransfer.Status,
                        BranchId: buveraTransfer.BranchId,
                        Position : buveraTransfer.Position,

                    });
                promise.then(
                    function (payload) {

                        $scope.data = payload.data.BuveraTransfers;
                        $scope.totalQuantity = payload.data.TotalQuantity;

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

angular
    .module('homer').controller('ReportBuveraController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.SearchBuvera = function (buvera) {
                $scope.data = [];
                $scope.totalQuantity = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllBuverasBetweenTheSpecifiedDatesForBranch',
                    {
                        FromDate: buvera.FromDate,
                        ToDate: buvera.ToDate,

                        BranchId: buvera.BranchId,

                    });
                promise.then(
                    function (payload) {

                        $scope.data = payload.data.Buveras;
                        $scope.totalQuantity = payload.data.TotalQuantity;

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

angular
    .module('homer')
    .controller('BuveraTransferTotalsController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$state', 'usSpinnerService',
        function ($scope, $http, $filter, $location, $log, $timeout, $state, usSpinnerService) {
            $scope.tab = {};
            if ($scope.defaultTab == 'dashboard') {
                $scope.tab.dashboard = true;
            }

            $scope.status = ["Accepted", "Rejected", "OutStanding"];


            $scope.position = ["Transfered", "Received"];



            $scope.SearchBuveraTransfer = function (buveraTransfer) {
                $scope.data = [];

                var promise = $http.post('/webapi/ReportApi/GetAllBuveraTransferTotalsBetweenTheSpecifiedDatesForBranch',
                    {
                        FromDate: buveraTransfer.FromDate,
                        ToDate: buveraTransfer.ToDate,
                        Status: buveraTransfer.Status,
                        Position : buveraTransfer.Position,
                        BranchId: buveraTransfer.BranchId,

                    });
                promise.then(
                    function (payload) {

                        $scope.data = payload.data.BuveraTransfers;
                        $scope.buveraTransfer = payload.data;

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
angular
    .module('homer')
    .controller('BuveraTotalsController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$state', 'usSpinnerService',
        function ($scope, $http, $filter, $location, $log, $timeout, $state, usSpinnerService) {
            $scope.tab = {};
            if ($scope.defaultTab == 'dashboard') {
                $scope.tab.dashboard = true;
            }




            $scope.SearchBuvera = function (buvera) {
                $scope.data = [];

                var promise = $http.post('/webapi/ReportApi/GetAllBuveraTotalsBetweenTheSpecifiedDatesForBranch',
                    {
                        FromDate: buvera.FromDate,
                        ToDate: buvera.ToDate,
                      

                    });
                promise.then(
                    function (payload) {

                        $scope.data = payload.data.Buveras;
                        $scope.buvera = payload.data;

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
angular
    .module('homer')
    .controller('DeliveryTotalsController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$state', 'usSpinnerService',
        function ($scope, $http, $filter, $location, $log, $timeout, $state, usSpinnerService) {
            $scope.tab = {};
            if ($scope.defaultTab == 'dashboard') {
                $scope.tab.dashboard = true;
            }

            var productId = 1;

          

            $scope.SearchDelivery = function (delivery) {
                $scope.data = [];

                var promise = $http.post('/webapi/ReportApi/GetAllDeliveryTotalsBetweenTheSpecifiedDatesForAParticularProduct',
                    {
                        FromDate: delivery.FromDate,
                        ToDate: delivery.ToDate,
                        ProductId: productId,
                        BranchId: delivery.BranchId,

                    });
                promise.then(
                    function (payload) {

                        $scope.data = payload.data.Deliveries;
                        $scope.delivery = payload.data;

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

angular
    .module('homer')
    .controller('CashSaleTotalsController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$state', 'usSpinnerService',
        function ($scope, $http, $filter, $location, $log, $timeout, $state, usSpinnerService) {
            $scope.tab = {};
            if ($scope.defaultTab == 'dashboard') {
                $scope.tab.dashboard = true;
            }



            var productId = 1;

            $scope.SearchCashSale = function (cashSale) {
                $scope.data = [];

                var promise = $http.post('/webapi/ReportApi/GetAllCashSaleTotalsBetweenTheSpecifiedDatesForParticularProduct',
                    {
                        FromDate: cashSale.FromDate,
                        ToDate: cashSale.ToDate,
                        Status: cashSale.Status,
                        ProductId: productId,
                        BranchId: cashSale.BranchId,

                    });
                promise.then(
                    function (payload) {

                        $scope.data = payload.data.CashSales;
                        $scope.cashSale = payload.data;

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

angular
    .module('homer')
    .controller('FlourTotalsController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$state', 'usSpinnerService',
        function ($scope, $http, $filter, $location, $log, $timeout, $state, usSpinnerService) {
            $scope.tab = {};
            if ($scope.defaultTab == 'dashboard') {
                $scope.tab.dashboard = true;
            }





            $scope.SearchFlour = function (flour) {
                $scope.data = [];

                var promise = $http.post('/webapi/ReportApi/GetAllFlourTotalsBetweenTheSpecifiedDatesForABranch',
                    {
                        FromDate: flour.FromDate,
                        ToDate: flour.ToDate,

                      

                    });
                promise.then(
                    function (payload) {

                        $scope.data = payload.data.FlourOutPuts;
                        $scope.flour = payload.data;

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
angular
    .module('homer')
    .controller('FlourTransferTotalsController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$state', 'usSpinnerService',
        function ($scope, $http, $filter, $location, $log, $timeout, $state, usSpinnerService) {
            $scope.tab = {};
            if ($scope.defaultTab == 'dashboard') {
                $scope.tab.dashboard = true;
            }

            $scope.status = ["Accepted", "Rejected", "OutStanding"];
            $scope.position = ["Transfered", "Received"];

           


            $scope.SearchFlourTransfer = function (flourTransfer) {
                $scope.data = [];

                var promise = $http.post('/webapi/ReportApi/GetAllFlourTransferTotalsBetweenTheSpecifiedDatesForBranch',
                    {
                        FromDate: flourTransfer.FromDate,
                        ToDate: flourTransfer.ToDate,
                        Status: flourTransfer.Status,
                        BranchId: flourTransfer.BranchId,
                        Position : flourTransfer.Position,

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

angular
    .module('homer').controller('ReportWeightLossController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;


            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });

            $http.get('/webapi/CustomerApi/GetAllCustomers').success(function (data, status) {
                $scope.customers = data;
            });

            $scope.SearchWeightLosses = function (weightLoss) {
                $scope.data = [];
                $scope.totalAmount = 0;
                $scope.totalQuantity = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllWeightLossesBetweenTheSpecifiedDatesForBranch',
                    {
                        FromDate: weightLoss.FromDate,
                        ToDate: weightLoss.ToDate,
                        CustomerId: weightLoss.Id,
                       


                    });
                promise.then(
                    function (payload) {

                        $scope.data = payload.data.WeightLosses;
                        $scope.totalQuantity = payload.data.TotalQuantity;
                        //$scope.totalAmount = payload.data.TotalAmount;

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

            $scope.downloadPDF = function () {
                var weightLossData = [];

                var weightloss_branchName;
                angular.forEach($scope.data, function (value, key) {
                    weightLossData.push({
                        CustomerName: value.CustomerName,
                        Quantity: value.Quantity.toString(),
                        CreatedOn: moment(value.CreatedOn).format('DD-MMM-YYYY')

                    });

                });

                weightloss_branchName = $filter('uppercase')($scope.data[0].BranchName);
                var items = weightLossData.map(function (item) {
                    return [item.CustomerName, item.Quantity, item.CreatedOn];
                });

                var docDefinition = {
                    pageOrientation: 'landscape',
                    header: function () {
                        return [
                            {
                                style: 'table',
                                margin: [62, 35, 62, 35],
                                table: {
                                    widths: ['*'],
                                    headerRows: 0,
                                    body: [
                                        [
                                            { text: 'MBALE INVESTMENT WEIGHT LOSS REPORT  OF ' + weightloss_branchName, style: 'topHeader', alignment: 'center' },


                                        ]
                                    ]
                                },
                                layout: 'noBorders'
                            }
                        ]
                    },
                    footer: function (currentPage, pageCount) {
                        return [
                            { text: currentPage.toString() + ' of ' + pageCount, alignment: 'center', style: 'footer' }
                        ]
                    },
                    content: [
                        {
                            style: 'topTable',
                            table: {
                                widths: ['*', '*', '*'],
                                heights: [18],
                                headerRows: 1,
                                body: [

                                    [
                                        { text: 'CUSTOMER NAME', style: 'tableLabel' },
                                        { text: 'QUANTITY', style: 'tabLabel' },
                                        { text: 'DATE', style: 'tableLabel' },
                                    ],

                                ].concat(items)
                            },



                            layout: {
                                paddingLeft: function (i, node) { return 6; },
                                paddingRight: function (i, node) { return 6; },
                                paddingTop: function (i, node) { return 6; },
                                paddingBottom: function (i, node) { return 6; },
                                fillColor: function (i, node) {
                                    return (i % 2 === 0) ? '#F5F5F5' : null;
                                }
                            }
                        },

                        {
                            table: {
                                widths: ['*', '*', '*'],
                                heights: [18],
                                headerRows: 1,
                                body: [




                                    [
                                        { text: 'TOTAL', style: 'tableLabel', bold: true },
                                        { text: $scope.totalQuantity.toString() + ' kgs', style: 'tableLabel', bold: true },
                                        { text: '', style: 'tableLabel' },

                                    ],

                                ]
                            },
                            layout: {
                                paddingLeft: function (i, node) { return 6; },
                                paddingRight: function (i, node) { return 6; },
                                paddingTop: function (i, node) { return 6; },
                                paddingBottom: function (i, node) { return 6; },
                                fillColor: function (i, node) {
                                    return (i % 2 === 0) ? '#F5F5F5' : null;
                                }
                            }
                        }


                    ],
                    pageSize: 'A4',
                    pageMargins: [62, 80, 62, 80],
                    styles: {
                        topHeader: {
                            fontSize: 15,
                            bold: true,
                            margin: [0, 6, 0, 30],
                            alignment: 'center'
                        },
                        table: {
                            fontSize: 4,
                            alignment: 'center',
                            color: 'black',
                            margin: [0, 5, 0, 15]
                        },
                        header: {
                            fontSize: 12,
                            bold: true,
                            margin: [0, 10, 0, 15],
                            alignment: 'center'
                        },
                        footer: {
                            fontSize: 8,
                            margin: [0, 25, 0, 17],
                            alignment: 'center'
                        }
                    }
                };
                var date = new Date();
                date = moment(date).format('DD_MMM_YYYY_HH_mm_ss');
                pdfMake.createPdf(docDefinition).download('WeightLoss_' + date + '.pdf');
                //pdfMake.createPdf(docDefinition).download("income.pdf");
            };




        }]);


angular
    .module('homer')
    .controller('DailyReportController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', '$log', '$timeout', '$state', 'usSpinnerService',
        function ($scope, ngTableParams,$http, $filter, $location, $log, $timeout, $state, usSpinnerService) {
            $scope.tab = {};
            if ($scope.defaultTab == 'dashboard') {
                $scope.tab.dashboard = true;
            }

            $scope.SummaryDailyReport = function (daily) {
                $scope.data = [];

                var promise = $http.post('/webapi/ReportApi/GetAllActivitiesForAParticularBranchForASpecificPeriod',
                    {
                        FromDate: daily.FromDate,
                        ToDate: daily.ToDate,
                                           

                    });
                promise.then(
                    function (payload) {

                        $scope.data = payload.data;
                        //$scope.flourTransfer = payload.data.FlourTransfers;
                        $scope.supplies = payload.data.Supplies.Supplies;
                        $scope.totalMaize = payload.data.Supplies.TotalMaize;
                        $scope.maizeAmount = payload.data.Supplies.TotalAmount;
                        $scope.incomes = payload.data.Income;
                        $scope.expenses = payload.data.Expenses;
                        $scope.brandCashSalesAmount = payload.data.BrandCashSales.TotalAmount;
                        $scope.brandCashSalesQuantity = payload.data.BrandCashSales.TotalQuantity;
                        $scope.flourCashSalesAmount = payload.data.FlourCashSales.TotalAmount;
                        $scope.flourCashSalesQuantity = payload.data.FlourCashSales.TotalQuantity;
                        $scope.totalFactoryExpenses = payload.data.FactoryExpenses.TotalAmount;
                        $scope.totalLabourCosts = payload.data.LabourCosts.TotalAmount;
                        $scope.cashBalance = payload.data.CashBalance;
                       
                        $scope.receivedFlour = payload.data.ReceivedAcceptedFlourTransfers.TotalQuantity;
                        $scope.transferedFlour = payload.data.TransferedAcceptedFlourTransfers.TotalQuantity;

                        $scope.deliveredFlour = payload.data.FlourDeliveries.TotalQuantity;
                        $scope.flourDeliveries = payload.data.FlourDeliveries;

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