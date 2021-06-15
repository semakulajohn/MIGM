angular
    .module('homer')
    .controller('BatchEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'selectSupplyService', 'usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval, selectSupplyService,usSpinnerService) {

        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }
        $scope.selectedGrades = [];
        var batchId = $scope.batchId;
        var action = $scope.action;
        $scope.showMessageSupplySelected = false;
        $scope.showMessageFlourOutPut = false;

       

        $http.get('webapi/GradeApi/GetAllGrades').success(function (data, status) {
            $scope.grades = data;
        });

        $http.get('/webapi/SectorApi/GetAllSectors').success(function (data, status) {
            $scope.sectors = data;
        });

        $http.get('/webapi/StoreApi/GetAllStores').success(function (data, status) {
            $scope.stores = data;
        });

        if (action == 'create') {
            batchId = 0;
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

            var promise = $http.get('/webapi/BatchApi/GetBatch?batchId=' + batchId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;

                    $scope.batch = {
                        BatchId: b.BatchId,
                        Name: b.Name,
                        Quantity: b.Quantity,
                        BranchId: b.BranchId,
                        SupplyIds: b.SupplyIds,
                        SectorId: b.SectorId,
                        TimeStamp: b.TimeStamp,
                        CreatedOn: b.CreatedOn,
                        CreatedBy: b.CreatedBy,
                        UpdatedBy: b.UpdatedBy,
                        Deleted: b.Deleted,
                        Supplies : b.Supplies,
                        

                    };

                });


        }

        $scope.selectSupply = function () {
            $scope.modalInstance = $modal.open({
                templateUrl: '/app/views/modal/modalSupply.html' + CACHE_BUST_SUFFIX,
                size: 'large',
                controller: 'ModalInstanceCtrl',
                resolve: {
                    items: function () {
                        $scope.dataItems = {

                        }
                        return $scope.dataItems;
                    },
                },
                windowClass: 'contentModalBox'
            });
        };

        $scope.$on('selectSupplyServiceBroadcastHandler', function () {


            $scope.selectedSupplies = [];
            $scope.filteredSupplies = [];
            var currentDate = new Date();
            //if ($scope.selectedSupplies.length > 0) {
                selectSupplyService.item.forEach(function (supply) {
                    var supplies = {
                        SupplyId: supply.SupplyId,
                        Quantity: supply.Quantity,
                    };

                    $scope.selectedSupplies.push(supplies);


                });
                $scope.showMessageSupplySelected = true;
                $timeout(function () {
                    $scope.showMessageSave = false;
                    //$scope.showMessageSupplySelected = false;
                    if ($scope.modalInstance != null)
                        $scope.modalInstance.close();
                }, 1500);
            //}
            //else {

            //    $scope.showMessageSupplyNotSelected = true;
            //    $timeout(function () {
                    
            //        $scope.showMessageSupplyNotSelected = false;
            //        if ($scope.modalInstance != null)
            //            $scope.modalInstance.close();
            //    }, 1500);
            //}

        });

        $scope.Save = function (batch) {
            if (($scope.selectedSupplies == null || $scope.selectedSupplies == undefined)) {
                $scope.showMessageSupplyNotSelected = true;
                $timeout(function () {
                    $scope.showMessageSupplyNotSelected = false;

                }, 2000);

            }
            else {
                $scope.quantity = 0;
                $scope.showMessageSave = false;
                angular.forEach($scope.selectedSupplies, function (value, key) {
                    $scope.quantity = value.Quantity + $scope.quantity;
                });
               
              //;
                   
                if ($scope.form.$valid) {
                    usSpinnerService.spin('global-spinner');
                        var promise = $http.post('/webapi/BatchApi/Save', {
                            BatchId: batchId,
                            Name: batch.Name,
                            Quantity: $scope.quantity,
                            BranchId: batch.BranchId,
                            SectorId: batch.SectorId,
                            CreatedBy: batch.CreatedBy,
                            CreatedOn: batch.CreatedOn,
                            Deleted: batch.Deleted,
                            Supplies: $scope.selectedSupplies,
                            StoreId : batch.StoreId,
                            
                        });

                        promise.then(
                            function (payload) {

                                batchId = payload.data;
                                usSpinnerService.stop('global-spinner');
                                $scope.showMessageSave = true;
                                $scope.showMessageSupplySelected = true;

                                $timeout(function () {
                                    $scope.showMessageSave = false;
                                    $scope.showMessageSupplySelected = false;
                                    if ($scope.modalInstance != null)
                                        $scope.modalInstance.close();

                                    if (action == "create") {
                                        $state.go('batch-edit', { 'action': 'edit', 'batchId': batchId });
                                    }

                                }, 1500);


                            });
                    
                }
               
           }

        }



        $scope.Cancel = function () {
            $state.go('batches.list');

        };

        $scope.Delete = function (batchId) {
            $scope.showMessageDeleted = false;
            var promise = $http.get('/webapi/BatchApi/Delete?batchId=' + batchId, {});
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

        var broadCastBatchSuppliersCount = function (supplierCount) {
            $rootScope.$broadcast('updateBatchSuppliersCount',
                { SupplierCount: supplierCount });
        };
    }
    ]);




angular
    .module('homer')
    .controller("SuppliesPickerController", ['$scope', '$modal', '$http', '$timeout', '$filter', 'Utils', '$window', 'selectSupplyService', '$sessionStorage', 'uiGridConstants',
function ($scope, $modal, $http, $timeout, $filter, Utils, $window, selectSupplyService, $sessionStorage, uiGridConstants) {

    var branches = [];
    var selectedBranch;
    $scope.$sessionStorage = $sessionStorage;
    $scope.error = false;
    $scope.gridData = {
        enableFiltering: true,
        columnDefs: $scope.columns,
        enableRowSelection: true,

    };

    $scope.gridData.multiSelect = true;

    $scope.gridData.columnDefs = [


          {
              name: 'SupplyDate',
              sort: {
                  direction: uiGridConstants.ASC,
                  priority: 1
              }
          },
                { name: 'WeightNote Number', field: 'WeightNoteNumber' },
                { name: 'Truck', field: 'TruckNumber' ,width:'15%'},
                { name: 'Supplier Name', field: 'SupplierName' },
                {name: 'Supplier Number',field:'SupplierNumber'},
                { name: 'Quantity(kgs)', field: 'Quantity' },
                { name: 'Used', field: 'Used',width:'10%' },


    ];


    $scope.gridData.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
    };


    //$http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
    //    $scope.xdata = {
    //        branches: data,
    //        selectedBranch: branches[0]
    //    }
    //});

 
    //$scope.OnBranchChange = function (supply) {
    //    var selectedBranchId = supply.BranchId
       var promise = $http.get('/webapi/SupplyApi/GetAllSuppliesToBeUsedForAParticularBranch')
       promise.then(function (payload) {
           $scope.gridData.data = payload.data
           
        });

       
    //}


   

    $scope.selectBatchSupplies = function () {
        $scope.selectedSupplies = $scope.gridApi.selection.getSelectedRows($scope.gridData);
        if ($scope.selectedSupplies != null) {
            selectSupplyService.prepForBroadcast($scope.selectedSupplies);

        }
    }


}]);



angular
    .module('homer')
    .controller('BatchDetailController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'selectSupplyService',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval, selectSupplyService) {

        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }
        
        var batchId = $scope.batchId;
              

       

            var promise = $http.get('/webapi/BatchApi/GetBatch?batchId=' + batchId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;

                    $scope.batch = {
                        BatchId: b.BatchId,
                        Name: b.Name,
                        Quantity: b.Quantity,
                        Loss: b.Loss,
                        BrandPercentage: b.BrandPercentage,
                        FlourPercentage: b.FlourPercentage,
                        LossPercentage: b.LossPercentage,
                        BranchName : b.BranchName,
                        BranchId: b.BranchId,
                        Supplies : b.Supplies,
                        SectorId: b.SectorId,
                        BrandOutPut: b.BrandOutPut,
                        FlourOutPut: b.FlourOutPut,
                        TimeStamp: b.TimeStamp,
                        CreatedOn: b.CreatedOn,
                        CreatedBy: b.CreatedBy,
                        UpdatedBy: b.UpdatedBy,
                        Deleted: b.Deleted,
                        Grades: b.Grades,
                        TotalSupplyAmount: b.TotalSupplyAmount,
                        MillingCharge: b.MillingCharge,
                        BranchMillingChargeRate: b.BranchMillingChargeRate,
                        TotalBuveraCost: b.TotalBuveraCost,
                        TotalFactoryExpenseCost: b.TotalFactoryExpenseCost,
                        FactoryExpenseCost: b.FactoryExpenseCost,
                        TotalMachineCost : b.TotalMachineCost,
                        FactoryExpenses: b.FactoryExpenses,
                        TotalLabourCosts: b.TotalLabourCosts,
                        LabourCosts: b.LabourCosts,
                        MillingChargeBalance: b.MillingChargeBalance,
                        TotalProductionCost: b.TotalProductionCost,
                        MachineRepairs: b.MachineRepairs,
                        OtherExpenses: b.OtherExpenses,
                        TotalOtherExpenseCost: b.TotalOtherExpenseCost,
                        TotalFlourKgsDelivered: b.TotalFlourKgsDelivered,
                        TotalFlourDeliveryAmount: b.TotalFlourDeliveryAmount,
                        TotalBrandDelivered: b.TotalBrandDelivered,
                        TotalBrandDeliveryAmount: b.TotalBrandDeliveryAmount,
                        BatchBrandDeliveries: b.BatchBrandDeliveries,
                        BatchFlourDeliveries: b.BatchFlourDeliveries,
                        BatchFlourGradesValues: b.BatchFlourGradesValues,
                        BatchFlourDeliveryGradesValues: b.BatchFlourDeliveryGradesValues,
                        BatchFlourTransfers: b.BatchFlourTransfers,
                        TotalFlourKgsTransfered: b.TotalFlourKgsTransfered,
                        TotalCashSaleFlourAmount: b.TotalCashSaleFlourAmount,
                        TotalCashSaleBrand: b.TotalCashSaleBrand,
                        TotalCashSaleBrandAmount: b.TotalCashSaleBrandAmount,
                        TotalCashSaleFlour: b.TotalCashSaleFlour,
                        BatchFlourCashSales: b.BatchFlourCashSales,
                        BatchBrandCashSales: b.BatchBrandCashSales,
                    };

                });


        
            $scope.Cancel = function () {
                $state.go('batches.list');

            };


            $scope.printBatch = function (batchDetails) {
                var innerContents = document.getElementById(batchDetails).innerHTML;
                var popupWinindow = window.open('', '_blank', 'width=600,height=700,scrollbars=no,menubar=no,toolbar=no,location=no,status=no,titlebar=no');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                   '<link rel="stylesheet" type="text/css" href="~/bower_components/angular-notify/dist/angular-notify.min.css" media="print"/>' +
                 '<link rel="stylesheet" type="text/css" href="~/bower_components/fontawesome/css/font-awesome.css" media="print"/>' +
                 '<link rel="stylesheet" type="text/css" href="~/bower_components/metisMenu/dist/metisMenu.css" media="print"/>' +
                 '<link rel="stylesheet" type="text/css" href="~/bower_components/animate.css/animate.css" media="print"/>' +
                 '<link rel="stylesheet" type="text/css" href="~/bower_components/bootstrap/dist/css/bootstrap.css" media="print"/>' +
                 '<link rel="stylesheet" type="text/css" href="~/bower_components/summernote/dist/summernote.css" media="print"/>' +
                 '<link rel="stylesheet" type="text/css" href="~/bower_components/angular-ui-tree/dist/angular-ui-tree.min.css" media="print"/>' +
                 '<link rel="stylesheet" type="text/css" href="~/bower_components/bootstrap-tour/build/css/bootstrap-tour.min.css" media="print"/>' +
                 '<link rel="stylesheet" type="text/css" href="~/bower_components/datatables_plugins/integration/bootstrap/3/dataTables.bootstrap.css" media="print"/>' +
                 '<link rel="stylesheet" type="text/css" href="~/bower_components/angular-xeditable/dist/css/xeditable.css" media="print"/>' +
                 '<link rel="stylesheet" type="text/css" href="~/bower_components/ui-select/dist/select.min.css" media="print"/>' +
                 '<link rel="stylesheet" type="text/css" href="~/bower_components/bootstrap-touchspin/dist/jquery.bootstrap-touchspin.min.css" media="print"/>' +
                 '<link rel="stylesheet" type="text/css" href="~/bower_components/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css" media="print"/>' +
                 '<link rel="stylesheet" type="text/css" href="~/bower_components/iCheck/skins/square/green.css" media="print"/>' +
                 '<link rel="stylesheet" type="text/css" href="~/bower_components/iCheck/skins/square/purple.css" media="print"/>' +
                 '<link rel="stylesheet" type="text/css" href="~/fonts/pe-icon-7-stroke/css/pe-icon-7-stroke.css" media="print"/>' +
                 '<link rel="stylesheet" type="text/css" href="~/fonts/pe-icon-7-stroke/css/helper.css" media="print"/>' +
                 '<link rel="stylesheet" type="text/css" href="~/styles/ng-table.min.css" media="print"/>' +
                 '<link rel="stylesheet" type="text/css" href="~/app/styles/isteven-multi-select.css" media="print"/>' +
                    '<link rel="stylesheet" type="text/css" href="~/styles/style.css" media="print" />' +
                    '</head><body onload="window.print()">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
        
       
    }
    ]);

angular
    .module('homer').controller('BranchBatchController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;

            var branchId = $scope.branchId;
            var promise = $http.get('/webapi/BatchApi/GetLatestBatchesForAParticularBranch', {});
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
                    name: 'Batch Number', field: 'Name',

                },
                 {
                     //this works to remove time section from a datetime string
                     // field: 'CreatedOn',type: 'date', cellFilter: 'date:\'yyyy-MM-dd\'',
                     name: 'CreatedOn', field: 'CreatedOn',
                     sort: {
                         direction: uiGridConstants.DESC,
                         priority: 1
                     }
                 },
                { name: 'Quantity(kgs)', field: 'Quantity', cellFilter: 'number' },

                { name: 'Batch Details', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/batch/detail/{{row.entity.BatchId}}">Batch Costing {{row.entity.Name}}</a> </div>' },


                {
                    name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/batches/edit/{{row.entity.BatchId}}">Edit</a> </div>',

                },



            ];




        }]);

