
angular
    .module('homer').controller('ReportTransactionController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;


            $scope.TransactionsForThisMonth = function () {
                $scope.data = [];
                var promise = $http.get('/webapi/ReportApi/GenerateTransactionCurrentMonthReport', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 2;
                     if ($scope.data.length > 0) {
                         //$scope.showDownloadLink = true;
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

            $scope.TodaysTransactions = function () {
                $scope.data = [];
                var promise = $http.get('/webapi/ReportApi/GenerateTransactionTodaysReport', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 1;
                     if ($scope.data.length > 0) {
                         //$scope.showDownloadLink = true;
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

            $scope.WeeksTransactions = function () {
                $scope.data = [];
                var promise = $http.get('/webapi/ReportApi/GenerateTransactionCurrentWeekReport', {});
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
            });

            $scope.SearchTransactions = function (accountTransaction) {
                $scope.data = [];
                var promise = $http.post('/webapi/ReportApi/GetAllTransactionsBetweenTheSpecifiedDates',
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


            $scope.DownloadExcelFile = function () {
                $window.open("/Excel/Index/" + $scope.reportType);
            };    
                      
            }]);

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

            $http.get('/webapi/OutSourcerApi/GetAllOutSourcers').success(function (data, status) {
                $scope.outSourcers = data;
                
            });
            $http.get('/webapi/CustomerApi/GetAllCustomers').success(function (data, status) {
                $scope.customers = data;
               
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
            $scope.showDownloadLink = false;
            $scope.data = [];
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
                $scope.totalYellowBags = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateSupplyCurrentMonthReport', {});
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
                var promise = $http.get('/webapi/ReportApi/GenerateSupplyTodaysReport', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.Supplies;
                     $scope.totalMaize = payload.data.TotalMaize;
                     $scope.totalAmount = payload.data.TotalAmount;
                     $scope.totalNormalBags = payload.data.TotalNormalBags;
                     $scope.totalStoneBags = payload.data.TotalStoneBags;
                     $scope.totalYellowBags = payload.data.TotalYellowBags;
                     $scope.reportType = 1;
                     if ($scope.data.length > 0) {
                         //$scope.showDownloadLink = true;
                        
                     }
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
                var promise = $http.get('/webapi/ReportApi/GenerateSupplyCurrentWeekReport', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.Supplies;
                     $scope.totalMaize = payload.data.TotalMaize;
                     $scope.totalAmount = payload.data.TotalAmount;
                     $scope.totalNormalBags = payload.data.TotalNormalBags;
                     $scope.totalStoneBags = payload.data.TotalStoneBags;
                     $scope.totalYellowBags = payload.data.TotalYellowBags;
                     $scope.reportType = 3;
                     if ($scope.data.length > 0) {
                         //$scope.showDownloadLink = true;

                      
                     }
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


            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });

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
                var promise = $http.post('/webapi/ReportApi/GetAllSuppliesBetweenTheSpecifiedDates',
                        {
                            FromDate: supply.FromDate,
                            ToDate: supply.ToDate,
                            SupplierId: supply.Id,
                            BranchId : supply.BranchId,
                            
                        });
                promise.then(
                 function (payload) {

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
            //var items = $scope.data;
            //function buildTableBody(items, columns) {
            //    var body = [];

            //    body.push(columns);

            //    data.forEach(function (row) {
            //        var dataRow = [];

            //        columns.forEach(function (column) {
            //            dataRow.push(row[column].toString());
            //        })

            //        body.push(dataRow);
            //    });

            //    return body;
            //}
            //function table(data, columns) {
            //    return {
            //        table: {
            //            headerRows: 1,
            //            body: buildTableBody(data, columns)
            //        }
            //    };
            //}

            //var dd = {
            //    content: [
            //        { text: 'Dynamic parts', style: 'header' },
            //        table(items, [{ text: 'WNN', style: 'header' }, { text: 'Quantity', style: 'header' },
            //                   { text: 'Price', style: 'header' }, { text: 'Amount', style: 'header' },
            //                    { text: 'Branch', style: 'header' }, { text: 'Date', style: 'header' },
            //                   { text: 'SNo', style: 'header' }, { text: 'Supplier', style: 'header' },
            //                    { text: 'NBags', style: 'header' }, { text: 'SBags', style: 'header' },
            //                    { text: 'YBags', style: 'header' }
            //        ],
            //        )
            //    ]
            //}

            //var docDefinition = {
                
            //    content: [
            //        {
            //            text: 'Supplies'
            //        },
            //        {
            //            style: 'demoTable',
            //            table: {
            //                widths: ['*', '*', '*', '*', '*', '*', '*', '*', '*', '*', '*'],
            //                body: items
            //                //[
            //                //    [{ text: 'WNN', style: 'header' }, { text: 'Quantity', style: 'header' },
            //                //    { text: 'Price', style: 'header' }, { text: 'Amount', style: 'header' },
            //                //    { text: 'Branch', style: 'header' }, { text: 'Date', style: 'header' },
            //                //    { text: 'SNo', style: 'header' }, { text: 'Supplier', style: 'header' },
            //                //    { text: 'NBags', style: 'header' }, { text: 'SBags', style: 'header' },
            //                //    { text: 'YBags', style: 'header' }
            //                //    ],
            //                //    //angular.forEach(denominations, function (denominations) {
            //                //    //    //$scope.DenominationAmount = ((price * denominations.Value) * denominations.Quantity) + $scope.DenominationAmount;
            //                //    //    $scope.DenominationAmount = (denominations.Price * denominations.Quantity) + $scope.DenominationAmount;
            //                //    //    $scope.DenominationQuantity = (denominations.Quantity * denominations.Value) + $scope.DenominationQuantity;
            //                //    //});
            //                //    [';pikh', '344', '52', '344', '52', '344', '52', '344', '52', '344', '52'],
            //                //    ['Sanga', '320', '89', '344', '52', '344', '52', '344', '52', '344', '52'],
            //                //    ['Total', ' ', $scope.totalMaize, ' ', $scope.totalAmount, ' ', ' ', ' ', $scope.totalNormalBags, $scope.totalStoneBags, $scope.totalYellowBags]
            //                //]
            //            }
            //        }
            //    ],
            //    styles: {
            //        header: {
            //            bold: true,
            //            color: '#000',
            //            fontSize: 11
            //        },
            //        demoTable: {
            //            color: '#666',
            //            fontSize: 10
            //        }
            //    }
            //};

            //$scope.openPdf = function () {
            //    pdfMake.createPdf(docDefinition).open();
            //};
            //$scope.downloadPdf = function () {
            //    pdfMake.createPdf(dd).download();
            //    //pdfMake.createPdf(docDefinition).download();
            //};
           
            $scope.DownloadExcelFile = function () {
                $window.open("/Excel/Supply/" + $scope.reportType);
                //$window.open("/Excel/ExportSupplyAsPDF/" + $scope.reportType);
                // $window.open("/Excel/ExportSupplyAsPDF/"+$scope.reportType);

                //href = "/Report/ExportMarriageAsPDF?marriageId=@ViewBag.marriageId"
            };

        }]);

angular
    .module('homer').controller('ReportDashBoardSupplyController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;
            var supplierId = $scope.supplierId;

            $scope.SuppliesForThisMonth = function () {
                $scope.data = [];
                var promise = $http.get('/webapi/ReportApi/GenerateSupplyCurrentMonthReportForAParticularSupplier?supplierId=' + supplierId, {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 2;
                     $scope.supplierReport = {
                         ReportTypeId: $scope.reportType,
                         SupplierId: supplierId,

                     };
                     if ($scope.data.length > 0) {
                         $scope.showDownloadLink = true;
                     }
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
                var promise = $http.get('/webapi/ReportApi/GenerateSupplyTodaysReportForAParticularSupplier?supplierId='+supplierId, {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 1;
                     $scope.supplierReport = {
                         ReportTypeId: $scope.reportType,
                         SupplierId: supplierId,

                     };
                     if ($scope.data.length > 0) {
                         $scope.showDownloadLink = true;
                     }
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
                var promise = $http.get('/webapi/ReportApi/GenerateSupplyCurrentWeekReportForAParticularSupplier?supplierId='+supplierId, {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data;
                     $scope.reportType = 3;
                     $scope.supplierReport = {
                         ReportTypeId: $scope.reportType,
                         SupplierId: supplierId,

                     };
                     if ($scope.data.length > 0) {
                         $scope.showDownloadLink = true;
                     }
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

            $scope.SearchSupplies = function (supply) {
                $scope.data = [];
                var promise = $http.post('/webapi/ReportApi/GetAllSuppliesBetweenTheSpecifiedDatesForAParticularSupplier?supplierId='+supplierId,
                        {
                            FromDate: supply.FromDate,
                            ToDate: supply.ToDate,

                        });
                promise.then(
                 function (payload) {

                     $scope.data = payload.data;
                     $scope.reportType = 4;
                     $scope.supplierReport = {
                         ReportTypeId: $scope.reportType,
                         SupplierId : supplierId,

                     };

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


            $scope.DownloadExcelFile = function () {
                $window.open("/Excel/SupplierSupply2/" + $scope.reportType + "/"+ supplierId);
              
            };

        }]);

angular
    .module('homer').controller('ReportBatchController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            //$scope.showDownloadLink = false;


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
                var promise = $http.get('/webapi/ReportApi/GenerateBatchCurrentMonthReport', {});
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
                var promise = $http.get('/webapi/ReportApi/GenerateBatchTodaysReport', {});
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
                var promise = $http.get('/webapi/ReportApi/GenerateBatchCurrentWeekReport', {});
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


            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });

          

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
                var promise = $http.post('/webapi/ReportApi/GetAllBatchesBetweenTheSpecifiedDates',
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
                $scope.totalQuantity = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateDeliveryCurrentMonthReport', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.Deliveries;
                   
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

            $scope.TodaysDeliveries = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                $scope.totalQuantity = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateDeliveryTodaysReport', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.Deliveries;
                     $scope.totalQuantity = payload.data.TotalQuantity;
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

            $scope.WeeksDeliveries = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                $scope.totalQuantity = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateDeliveryCurrentWeekReport', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.Deliveries;
                     $scope.totalQuantity = payload.data.TotalQuantity;
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


            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });

            $http.get('/webapi/CustomerApi/GetAllCustomers').success(function (data, status) {
                $scope.customers = data;
            });

            $scope.SearchDeliveries = function (delivery) {
                $scope.data = [];
                $scope.totalAmount = 0;
                $scope.totalQuantity = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllDeliveriesBetweenTheSpecifiedDatesForAParticularProduct',
                        {
                            FromDate: delivery.FromDate,
                            ToDate: delivery.ToDate,
                            CustomerId: delivery.Id,
                            BranchId: delivery.BranchId,
                            ProductId : delivery.ProductId,

                        });
                promise.then(
                 function (payload) {

                     $scope.data = payload.data.Deliveries;
                     $scope.totalQuantity = payload.data.TotalQuantity;
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
    .module('homer').controller('ReportRiceInputController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;               
           
            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });

           
            $scope.SearchRiceInputs = function (input) {
                $scope.data = [];
                $scope.totalAmount = 0;
                $scope.totalQuantity = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllRiceInputsBetweenTheSpecifiedDatesForBranch',
                    {
                        FromDate: input.FromDate,
                        ToDate: input.ToDate,
                       
                        BranchId: input.BranchId,
                       
                    });
                promise.then(
                    function (payload) {

                        $scope.data = payload.data.RiceInputs;
                        $scope.totalQuantity = payload.data.TotalQuantity;
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
    .module('homer').controller('ReportOutSourcerOutPutController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;
            $scope.branchId = 20013;
            //$scope.branchId = 20011;

            $http.get('/webapi/StoreApi/GetAllBranchStores?branchId='+ $scope.branchId).success(function (data, status) {
                $scope.stores = data;
            });

            

            $scope.SearchOutSourcerOutPuts = function (outPut) {
                $scope.data = [];
                $scope.totalAmount = 0;
                $scope.totalQuantity = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllOutSourcerOutPutsBetweenTheSpecifiedDates',
                    {
                        FromDate: outPut.FromDate,
                        ToDate: outPut.ToDate,
                        BranchId: outPut.StoreId,


                    });
                promise.then(
                    function (payload) {

                        $scope.data = payload.data.OutSourcerOutPuts;
                        $scope.totalQuantity = payload.data.TotalQuantity;
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
    .module('homer').controller('ReportOutSourcerOutPutTotalsController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;
            $scope.branchId = 20013;
            //$scope.branchId = 20011;

            $http.get('/webapi/StoreApi/GetAllBranchStores?branchId=' + $scope.branchId).success(function (data, status) {
                $scope.stores = data;
            });



            $scope.SearchOutSourcerOutPuts = function (flour) {
                $scope.data = [];
                $scope.totalAmount = 0;
                $scope.totalQuantity = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllOutPutTotalsBetweenTheSpecifiedDates',
                    {
                        FromDate: flour.FromDate,
                        ToDate: flour.ToDate,
                        BranchId: flour.StoreId,


                    });
                promise.then(
                    function (payload) {

                        $scope.data = payload.data.OutSourcerOutPuts;
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
    .module('homer').controller('ReportOutSourcerDeliveryTotalsController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;
            $scope.branchId = 20013;
            //$scope.branchId = 20011;
            $scope.productId = 1;

            $http.get('/webapi/StoreApi/GetAllBranchStores?branchId=' + $scope.branchId).success(function (data, status) {
                $scope.stores = data;
            });



            $scope.SearchDelivery = function (delivery) {
                $scope.data = [];
                $scope.totalAmount = 0;
                $scope.totalQuantity = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllDeliveryTotalsBetweenTheSpecifiedDates',
                    {
                        FromDate: delivery.FromDate,
                        ToDate: delivery.ToDate,
                        BranchId: delivery.StoreId,
                        ProductId : $scope.productId,

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
    .module('homer').controller('ReportOutSourcerDeliveryController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;
            $scope.branchId = 20013;
            //$scope.branchId = 20011;
            $scope.productId = 1;

            $http.get('/webapi/StoreApi/GetAllBranchStores?branchId=' + $scope.branchId).success(function (data, status) {
                $scope.stores = data;
            });


            $http.get('/webapi/CustomerApi/GetAllCustomers').success(function (data, status) {
                $scope.customers = data;
            });

            $scope.SearchOutSourcerDeliveries = function (delivery) {
                $scope.data = [];
                $scope.totalAmount = 0;
                $scope.totalQuantity = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllOutSourcerDeliveriesBetweenTheSpecifiedDatesForAParticularProduct',
                    {
                        FromDate: delivery.FromDate,
                        ToDate: delivery.ToDate,
                        BranchId : delivery.StoreId,
                        CustomerId: delivery.Id,
                        ProductId: $scope.productId,

                    });
                promise.then(
                    function (payload) {

                        $scope.data = payload.data.Deliveries;
                        $scope.totalQuantity = payload.data.TotalQuantity;
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
                var promise = $http.post('/webapi/ReportApi/GetAllWeightLossesBetweenTheSpecifiedDates',
                    {
                        FromDate: weightLoss.FromDate,
                        ToDate: weightLoss.ToDate,
                        CustomerId: weightLoss.Id,
                        BranchId: weightLoss.BranchId,
                       

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


            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });



            $scope.SearchCash = function (cash) {
                $scope.data = [];
                var promise = $http.post('/webapi/ReportApi/GetAllCashBetweenTheSpecifiedDates',
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
                var promise = $http.get('/webapi/ReportApi/GenerateOrderCurrentMonthReport', {});
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
                var promise = $http.get('/webapi/ReportApi/GenerateOrderTodaysReport', {});
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
                var promise = $http.get('/webapi/ReportApi/GenerateOrderCurrentWeekReport', {});
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


            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });

            $http.get('/webapi/CustomerApi/GetAllCustomers').success(function (data, status) {
                $scope.customers = data;
            });

            $scope.SearchOrders = function (order) {
                $scope.data = [];
                var promise = $http.post('/webapi/ReportApi/GetAllOrdersBetweenTheSpecifiedDates',
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
          
            var branches = [];
            var selectedBranch;

            $scope.FactoryExpenseForThisMonth = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateFactoryExpenseCurrentMonthReport', {});
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
                var promise = $http.get('/webapi/ReportApi/GenerateFactoryExpenseTodaysReport', {});
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
                var promise = $http.get('/webapi/ReportApi/GenerateFactoryExpenseCurrentWeekReport', {});
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


            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });



        
            $scope.SearchFactoryExpense = function (factoryExpense) {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllFactoryExpensesBetweenTheSpecifiedDates',
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
                var promise = $http.get('/webapi/ReportApi/GenerateOtherExpenseCurrentMonthReport', {});
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
                var promise = $http.get('/webapi/ReportApi/GenerateOtherExpenseTodaysReport', {});
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
                var promise = $http.get('/webapi/ReportApi/GenerateOtherExpenseCurrentWeekReport', {});
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



            $scope.SearchOtherExpense = function (otherExpense) {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllOtherExpensesBetweenTheSpecifiedDates',
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

            $scope.status = ["Accepted", "Rejected"];
           
            $scope.FlourTransferForThisMonth = function () {
                $scope.data = [];
                $scope.totalQuantity = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateFlourTransferCurrentMonthReport', {});
               
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
                var promise = $http.get('/webapi/ReportApi/GenerateFlourTransferTodaysReport', {});
               
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
                var promise = $http.get('/webapi/ReportApi/GenerateFlourTransferCurrentWeekReport', {});
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


            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });



            $scope.SearchFlourTransfer = function (flourTransfer) {
                $scope.data = [];
                $scope.totalQuantity = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllFlourTransfersBetweenTheSpecifiedDates',
                        {
                            FromDate: flourTransfer.FromDate,
                            ToDate: flourTransfer.ToDate,
                            Status : flourTransfer.Status,
                            BranchId: flourTransfer.BranchId,

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
                var promise = $http.get('/webapi/ReportApi/GenerateMachineRepairCurrentMonthReport', {});
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
                var promise = $http.get('/webapi/ReportApi/GenerateMachineRepairTodaysReport', {});
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
                var promise = $http.get('/webapi/ReportApi/GenerateMachineRepairCurrentWeekReport', {});
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



            $scope.SearchMachineRepair = function (machineRepair) {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllMachineRepairsBetweenTheSpecifiedDates',
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
              
                var promise = $http.get('/webapi/ReportApi/GenerateBatchCurrentMonthReport', {});
                $scope.showDownloadLink = false;
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
               
                var promise = $http.get('/webapi/ReportApi/GenerateBatchTodaysReport', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.Batches;
                    
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

            $scope.WeeksBatchOutPut = function () {
                $scope.data = [];
               
                var promise = $http.get('/webapi/ReportApi/GenerateBatchCurrentWeekReport', {});
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


            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });



            $scope.SearchBatchOutPut = function (batchOutPut) {
                $scope.data = [];
              
                var promise = $http.post('/webapi/ReportApi/GetAllBatchesBetweenTheSpecifiedDates',
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

            //$scope.SearchBatches = function (batch) {
            //    $scope.data = [];
            //    $scope.totalMaize = 0;
            //    $scope.totalFactoryExpenses = 0;
            //    $scope.totalBrandKgs = 0;
            //    $scope.totalFlourkgs = 0;
            //    $scope.totalLabourCosts = 0;
            //    $scope.totalMillingBalance = 0;
            //    $scope.totalMillingCharge = 0;
            //    $scope.totalBuveraCosts = 0;
            //    var promise = $http.post('/webapi/ReportApi/GetAllBatchesBetweenTheSpecifiedDates',
            //        {
            //            FromDate: batch.FromDate,
            //            ToDate: batch.ToDate,

            //            BranchId: batch.BranchId,

            //        });
            //    promise.then(
            //        function (payload) {

            //            $scope.data = payload.data.Batches;
            //            $scope.totalMaize = payload.data.TotalMaize;
            //            $scope.totalFactoryExpenses = payload.data.TotalFactoryExpenses;
            //            $scope.totalBrandKgs = payload.data.TotalBrandKgs;
            //            $scope.totalFlourkgs = payload.data.TotalFlourKgs;
            //            $scope.totalLabourCosts = payload.data.TotalLabourCosts;
            //            $scope.totalMillingBalance = payload.data.TotalMillingBalance;
            //            $scope.totalMillingCharge = payload.data.TotalMillingCharge;
            //            $scope.totalBuveraCosts = payload.data.TotalBuveraCosts;

            //            $scope.reportType = 4;

            //            $scope.tableParams = new ngTableParams({
            //                page: 1,
            //                count: 10,
            //                sorting: { CreatedOn: 'desc' }
            //            }, {
            //                    getData: function ($defer, params) {
            //                        var filteredData = $filter('filter')($scope.data, $scope.filter);
            //                        var orderedData = params.sorting() ?
            //                            $filter('orderBy')(filteredData, params.orderBy()) :
            //                            filteredData;

            //                        params.total(orderedData.length);
            //                        $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
            //                    },
            //                    $scope: $scope

            //                });
            //        });
            //}



        }]);

angular
    .module('homer').controller('ReportLabourCostController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
          


            $scope.LabourCostForThisMonth = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateLabourCostCurrentMonthReport', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.LabourCosts;
                     scope.totalAmount = payload.data.TotalAmount;
                    
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
                var promise = $http.get('/webapi/ReportApi/GenerateLabourCostTodaysReport', {});
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
                var promise = $http.get('/webapi/ReportApi/GenerateLabourCostCurrentWeekReport', {});
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


            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });



            $scope.SearchLabourCost = function (labourCost) {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllLabourCostsBetweenTheSpecifiedDates',
                        {
                            FromDate: labourCost.FromDate,
                            ToDate: labourCost.ToDate,

                            BranchId: labourCost.BranchId,

                        });
                promise.then(
                 function (payload) {

                     $scope.data = payload.data.LabourCosts;
                     $scope.totalAmount = payload.data.TotalAmount;
                     //angular.forEach($scope.data, function (value, key) {
                     //    $scope.totalAmount = value.Amount + $scope.totalAmount;

                     //});
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
    .module('homer').controller('ReportCreditorController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;        
            $scope.GenerateCreditorList = function (searchDate) {
                $scope.data = [];
                $scope.creditors = [];
                $scope.totalAmount = 0;
                if (searchDate == null || searchDate == "undefined") {
                    var promise = $http.get('/webapi/ReportApi/GenerateCreditorReport',{});

                }
                else {
                    var promise = $http.post('/webapi/ReportApi/GenerateCreditorReportForAParticularDate',
                        {
                            ToDate: searchDate.ToDate,


                        });


                }
               
                promise.then(
                 function (payload) {
                    
                     $scope.data = payload.data.Creditors;
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

        }]);

angular
    .module('homer').controller('ReportDebtorController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;   

            $scope.GenerateDebtorList = function (searchDate) {
                $scope.data = [];
                $scope.creditors = [];
                $scope.totalAmount = 0;
                if (searchDate == null || searchDate == "undefined") {
                    var promise = $http.get('/webapi/ReportApi/GenerateDebtorReport',{});
                }
                else {
                    var promise = $http.post('/webapi/ReportApi/GenerateDebtorReportForAParticularDate',
                        {
                            ToDate: searchDate.ToDate,

                        });
                }
               
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {

                     $scope.data = payload.data.Debtors;
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



          
        }]);

angular
    .module('homer').controller('ReportAdvancePaymentController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;



            $scope.GenerateAdvancePaymentList = function (searchDate) {
                $scope.data = [];
                $scope.creditors = [];
                $scope.totalAmount = 0;

                if (searchDate == null || searchDate == "undefined") {
                    var promise = $http.get('/webapi/ReportApi/GenerateAdvancePaymentReport', {});

                }
                else {
                    var promise = $http.post('/webapi/ReportApi/GenerateAdvancePaymentReportForAParticularDate',
                        {
                            ToDate: searchDate.ToDate,

                        });
                }
                    $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {

                     $scope.data = payload.data.Debtors;
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
                var promise = $http.get('/webapi/ReportApi/GenerateCashSaleCurrentMonthReport', {});
               
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
                var promise = $http.get('/webapi/ReportApi/GenerateCashSaleTodaysReport', {});
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
              
                var promise = $http.get('/webapi/ReportApi/GenerateCashSaleCurrentWeekReport', {});
              
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


            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });



            $scope.SearchCashSales = function (cashSale) {
                $scope.data = [];
                $scope.totalAmount = 0;
                $scope.totalQuantity = 0;
               
                var promise = $http.post('/webapi/ReportApi/GetAllCashSalesBetweenTheSpecifiedDatesForParticularProduct',
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

            $scope.status = [
                       { Name: "Accepted", value: "Accepted" },
                       { Name: "Rejected", value: "Rejected" },
            ];
            $scope.CashTransfersForThisMonth = function () {
                $scope.data = [];

                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateCashTransferCurrentMonthReport', {});
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
                var promise = $http.get('/webapi/ReportApi/GenerateCashTransferTodaysReport', {});
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

                var promise = $http.get('/webapi/ReportApi/GenerateCashTransferCurrentWeekReport', {});
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


            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });



            $scope.SearchCashTransfers = function (cashTransfer) {
                $scope.data = [];
                $scope.totalAmount = 0;

                var promise = $http.post('/webapi/ReportApi/GetAllCashTransfersBetweenTheSpecifiedDates',
                        {
                            FromDate: cashTransfer.FromDate,
                            ToDate: cashTransfer.ToDate,
                            FromBranchId: cashTransfer.FromBranchId,
                            ToBranchId : cashTransfer.ToBranchId,
                            Status: cashTransfer.Status,
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


          

        }]);

angular
    .module('homer').controller('ReportExpenseController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;


            $scope.CashForThisMonth = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateExpensesCurrentMonthReport', {});
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

            $scope.TodaysCash = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateExpensesTodaysReport', {});
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

            $scope.WeeksCash = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateExpensesCurrentWeekReport', {});
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



            $scope.SearchCash = function (cash) {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllExpensesBetweenTheSpecifiedDates',
                        {
                            FromDate: cash.FromDate,
                            ToDate: cash.ToDate,

                            BranchId: cash.BranchId,

                        });
                promise.then(
                 function (payload) {

                     $scope.data = payload.data;
                     $scope.reportType = 4;
                     angular.forEach($scope.data, function (value, key) {
                         $scope.totalAmount = value.Amount + $scope.totalAmount;

                     });

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
    .module('homer').controller('ReportExpenseRequistionCategoryController', ['$scope', 'ngTableParams', '$http', '$filter', '$window',
        function ($scope, ngTableParams, $http, $filter, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            $scope.showDownloadLink = false;


            $http.get('/webapi/RequistionApi/GetAllRequistionCategories').success(function (data, status) {
                $scope.requistionCategories = data;
            });
           

            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });



            $scope.SearchCash = function (cash) {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllExpensesBetweenTheSpecifiedDatesForAParticularRequistionCategory',
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


            $scope.CashForThisMonth = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateIncomesCurrentMonthReport', {});
                $scope.showDownloadLink = false;
                promise.then(
                 function (payload) {
                     $scope.data = payload.data.Cashs;
                     $scope.TotalAmount = payload.data.TotalAmount;
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
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateIncomesTodaysReport', {});
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

            $scope.WeeksCash = function () {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.get('/webapi/ReportApi/GenerateIncomesCurrentWeekReport', {});
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

            
            $scope.SearchCash = function (cash) {
                $scope.data = [];
                $scope.totalAmount = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllIncomesBetweenTheSpecifiedDates',
                        {
                            FromDate: cash.FromDate,
                            ToDate: cash.ToDate,

                            BranchId: cash.BranchId,

                        });
                promise.then(
                 function (payload) {

                     $scope.data = payload.data.Cashs;
                     $scope.TotalAmount = payload.data.TotalAmount;
                     $scope.reportType = 4;

                     angular.forEach($scope.data, function (value, key) {
                         $scope.totalAmount = value.Amount + $scope.totalAmount;

                     });
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
    .module('homer').controller('ReportUtilityAccountController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
          


            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });

            $http.get('/webapi/UtilityAccountApi/GetAllUtilityCategories').success(function (data, status) {
                $scope.utilityCategories = data;
            });



            $scope.SearchUtilityAccount = function (utilityAccount) {
                $scope.data = [];
                var promise = $http.post('/webapi/ReportApi/GetAllUtilityAccountsBetweenTheSpecifiedDatesForAParticularCategory',
                        {
                            FromDate: utilityAccount.FromDate,
                            ToDate: utilityAccount.ToDate,

                            BranchId: utilityAccount.BranchId,
                            CategoryId : utilityAccount.CategoryId,

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




        }]);
angular
    .module('homer').controller('ReportDepositController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;
            //var transactionSubTypeId = 10008;
              

            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });

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

                            BranchId: deposit.BranchId,
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

                        BranchId: deposit.BranchId,
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

                        BranchId: deposit.BranchId,
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
    .module('homer').controller('ReportBuveraTransferController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants', '$window',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants, $window) {
            $scope.loadingSpinner = true;

            $scope.reportType = 0;

            $scope.status = [{ Name: "Accepted", value: "Accepted" },
                { Name: "Rejected", value: "Rejected" },];

           

            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });



            $scope.SearchBuveraTransfers = function (buveraTransfer) {
                $scope.data = [];
                $scope.totalQuantity = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllBuveraTransfersBetweenTheSpecifiedDates',
                    {
                        FromDate: buveraTransfer.FromDate,
                        ToDate: buveraTransfer.ToDate,
                        Status: buveraTransfer.Status,
                        BranchId: buveraTransfer.BranchId,

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

           
            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });



            $scope.SearchBuvera = function (buvera) {
                $scope.data = [];
                $scope.totalQuantity = 0;
                var promise = $http.post('/webapi/ReportApi/GetAllBuverasBetweenTheSpecifiedDates',
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
    .controller('DeliveryTotalsController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$state', 'usSpinnerService',
        function ($scope, $http, $filter, $location, $log, $timeout, $state, usSpinnerService) {
            $scope.tab = {};
            if ($scope.defaultTab == 'dashboard') {
                $scope.tab.dashboard = true;
            }

            var productId = 1;

            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });


            $scope.SearchDelivery = function (delivery) {
                $scope.data = [];

                var promise = $http.post('/webapi/ReportApi/GetAllDeliveryTotalsBetweenTheSpecifiedDatesForAParticularProduct',
                    {
                        FromDate: delivery.FromDate,
                        ToDate: delivery.ToDate,
                        ProductId : productId,
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

          

            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });

            var productId = 1;

            $scope.SearchCashSale = function (cashSale) {
                $scope.data = [];

                var promise = $http.post('/webapi/ReportApi/GetAllCashSaleTotalsBetweenTheSpecifiedDatesForParticularProduct',
                    {
                        FromDate: cashSale.FromDate,
                        ToDate: cashSale.ToDate,
                        Status: cashSale.Status,
                        ProductId : productId,
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

            

            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });


            $scope.SearchFlour = function (flour) {
                $scope.data = [];

                var promise = $http.post('/webapi/ReportApi/GetAllFlourTotalsBetweenTheSpecifiedDates',
                    {
                        FromDate: flour.FromDate,
                        ToDate: flour.ToDate,
                       
                        BranchId: flour.BranchId,

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



