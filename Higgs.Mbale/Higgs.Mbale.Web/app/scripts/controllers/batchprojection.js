angular
    .module('homer')
    .controller('BatchProjectionEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'usSpinnerService',
        function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval, usSpinnerService) {

            $scope.tab = {};
            if ($scope.defaultTab == 'dashboard') {
                $scope.tab.dashboard = true;
            }
            
            var batchId = $scope.batchId;
            var batchProjectionId = $scope.batchProjectionId;
            var action = $scope.action;
           
            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });

            

            if (action == 'create') {
                batchProjectionId = 0;
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

                var promise = $http.get('/webapi/BatchProjectionApi/GetBatchProjection?batchProjectionId=' + batchProjectionId, {});
                promise.then(
                    function (payload) {
                        var b = payload.data;

                        $scope.batchProjection = {
                            BatchId: b.BatchId,
                            BatchProjectionId : b.BatchProjectionId,
                            FlourOutPut: b.FlourOutPut,
                            BrandPercentage: b.BrandPercentage,
                            FlourPercentage: b.FlourPercentage,
                            FlourSales: b.FlourSales,
                            BrandSales: b.BrandSales,
                            FlourPrice: b.FlourPrice,
                            BrandPrice: b.BrandPrice,
                            BrandOutPut : b.BrandOutPut,
                            BranchId: b.BranchId,
                           ProductionCost : b.ProductionCost,
                            UnitCost: b.UnitCost,
                            ExpectedContribution : b.ExpectedContribution,
                            TimeStamp: b.TimeStamp,
                            CreatedOn: b.CreatedOn,
                            CreatedBy: b.CreatedBy,
                            UpdatedBy: b.UpdatedBy,
                            Deleted: b.Deleted,
                          
                        };

                    });


            }



            $scope.Save = function (batchProjection) {

             
                $scope.showMessageSave = false;

              

                    if ($scope.form.$valid) {
                        usSpinnerService.spin('global-spinner');
                        var promise = $http.post('/webapi/BatchProjectionApi/Save', {
                            BatchId: batchId,
                            BatchProjectionId: batchProjectionId,
                            BranchId: batchProjection.BranchId,
                            BrandPercentage : batchProjection.BrandPercentage,
                            CreatedBy: batchProjection.CreatedBy,
                            CreatedOn: batchProjection.CreatedOn,
                            Deleted: batchProjection.Deleted,
                            BrandPrice: batchProjection.BrandPrice,
                            FlourPrice: batchProjection.FlourPrice,
                            FlourPercentage: batchProjection.FlourPercentage,
                            UnitCost : batchProjection.UnitCost,
                            

                           
                        });

                        promise.then(
                            function (payload) {

                                batchProjectionId = payload.data;

                                if (batchOutPutId == -1) {
                                    $scope.showMessageEnoughProjections = true;
                                    usSpinnerService.stop('global-spinner');

                                    $timeout(function () {
                                        $scope.showMessageEnoughProjections = false;

                                    }, 4000);
                                }
                               
                                else {


                                    $scope.showMessageSave = true;
                                    usSpinnerService.stop('global-spinner');

                                    $timeout(function () {
                                        $scope.showMessageSave = false;

                                        if (action == "create") {
                                            $state.go('batchProjection-batch-detail', { 'action': 'edit', 'batchProjectionId': batchProjectionId, 'batchId': batchId });
                                        }

                                    }, 1500);
                                }

                            });
                    }
                
                

            }




            $scope.Cancel = function () {
                $state.go('batchProjection-batch', { 'batchId': batchId });
            };

            $scope.Delete = function (batchProjectionId) {
                $scope.showMessageDeleted = false;
                var promise = $http.get('/webapi/BatchProjectionApi/Delete?batchProjectionId=' + batchProjectionId, {});
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
    .module('homer').controller('BatchProjectionController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;

            var batchId = $scope.batchId;

            var promise = $http.get('/webapi/BatchProjectionApi/GetAllBatchProjectionsForAParticularBatch?batchId=' + batchId, {});
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;
                }
            );
            $scope.retrievedBatchId = $scope.batchId;

            $scope.gridData = {
                enableColumnResizing: true,
                enableFiltering: false,
                columnDefs: $scope.columns,
                enableRowSelection: false
            };

            $scope.gridData.multiSelect = false;

            $scope.gridData.columnDefs = [

                //{
                //    name: 'Id', 
                //    sort: {
                //        direction: uiGridConstants.ASC,
                //        priority: 1
                //    },
                //    //width: '5%'
                //},
                { name: 'Quantity(kgs)', field: 'MaizeQuantity' },

                { name: 'Flour(kgs)', field: 'FlourOutPut' },
                { name: 'Flour(%)', field: 'FlourPercentage' },
                { name: 'Brand(kgs)', field: 'BrandOutPut' },
                { name: 'Brand(%)', field: 'BrandPercentage' },
                { name: 'Branch', field: 'BranchName' },
                {name:'Expected Contribution',field:'ExpectedContribution'},
                {
                    name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/batchProjections/{{row.entity.BatchProjectionId}}/' + $scope.batchId + '">Details</a> </div>',

                },

            ];




        }]);


angular
    .module('homer')
    .controller('BatchProjectionDetailController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval',
        function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval) {

            $scope.tab = {};
            if ($scope.defaultTab == 'dashboard') {
                $scope.tab.dashboard = true;
            }

            var batchProjectionId = $scope.batchProjectionId;




            var promise = $http.get('/webapi/BatchProjectionApi/GetBatchProjection?batchProjectionId=' + batchProjectionId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;

                    $scope.batchProjection = {
                        BatchProjectionId: b.BatchProjectionId,
                        BatchNumber: b.BatchNumber,
                        MaizeQuantity: b.MaizeQuantity,
                        
                        BrandPercentage: b.BrandPercentage,
                        FlourPercentage: b.FlourPercentage,
                        ExpectedContribution: b.ExpectedContribution,
                        FlourPrice: b.FlourPrice,
                        FlourSales: b.FlourSales,
                        UnitCost: b.UnitCost,
                        BrandSales: b.BrandSales,
                        BrandPrice : b.BrandPrice,
                        BranchName: b.BranchName,
                        BranchId: b.BranchId,
                        Supplies: b.Supplies,
                        ProductionCost : b.ProductionCost,
                        BrandOutPut: b.BrandOutPut,
                        FlourOutPut: b.FlourOutPut,
                        TimeStamp: b.TimeStamp,
                        CreatedOn: b.CreatedOn,
                        CreatedBy: b.CreatedBy,
                        TotalExpectedSales: b.TotalExpectedSales,
                        TotalSupplyAmount: b.TotalSupplyAmount,
                        
                        TotalProductionCost: b.TotalProductionCost,
                      
                        
                    };

                });




            //$scope.Cancel = function () {
            //    $state.go('batchProjection-batch', { 'batchId': batchId });
            //};



          
        }
    ]);

