angular
    .module('homer')
    .controller('RequistionEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval','usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval, usSpinnerService) {

        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }

        var requistionId = $scope.requistionId;
        var action = $scope.action;
        var statusId = "10002";
        var approvedStatusId = "2";

        $http.get('/webapi/RequistionApi/GetAllRequistionCategories').success(function (data, status) {
            $scope.requistionCategories = data;
        });

        $http.get('/webapi/InventoryApi/GetAllInventoryCategories').success(function (data, status) {
            $scope.inventorycategories = data;
        });
        $http.get('/webapi/ActivityApi/GetAllActivities').success(function (data, status) {
            $scope.activities = data;
        });
         $http.get('/webapi/UtilityAccountApi/GetAllUtilityCategories').success(function (data, status) {
                $scope.utilityCategories = data;
            });

        $http.get('/webapi/SupplyApi/GetAllUnPaidSupplies').success(function (data, status) {
            $scope.supplies = data;
        });

        $http.get('/webapi/SupplierApi/GetAllSuppliers').success(function (data, status) {
            $scope.suppliers = data;
        });

        $scope.OnSupplierChange = function (requistion) {
            var selectedSupplierId = requistion.Id
            $http.get('/webapi/SupplyApi/GetAllUnPaidSuppliesForAParticularSupplier?supplierId=' + selectedSupplierId).success(function (data, status) {
                $scope.weightNotes = data;
            });

        }
        $http.get('/webapi/StatusApi/GetAllStatuses').success(function (data, status) {
            $scope.statuses = data;
        });
        
        $http.get('/webapi/BatchApi/GetLatestBatchesForAParticularBranch').success(function (data, status) {
            $scope.batches = data;
        });

        $http.get('/webapi/CasualWorkerApi/GetAllCasualWorkersForAParticularBranch').then(function (responses) {
            $scope.casualWorkers = responses.data;

        });

      
        if (action == 'create') {
            requistionId = 0;

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



            var promise = $http.get('/webapi/RequistionApi/GetRequistion?requistionId=' + requistionId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;
                    if (b.Accept != true && b.Reject != true) {
                        $scope.hideAcceptReject = true;
                    }
                    else if (b.Accept == true || b.Reject == true) {
                        $scope.hideAcceptReject = false;
                    }
                    $scope.requistion = {
                        RequistionId: b.RequistionId,
                        Response: b.Response,
                        StatusId: b.StatusId,
                        Amount : b.Amount,
                        BranchId: b.BranchId,
                        Approved: b.Approved,
                        Rejected: b.Rejected,
                        DocumentId : b.DocumentId,
                        Description: b.Description,
                        ApprovedById: b.ApprovedById,
                        AmountInWords : b.AmountInWords,
                        RequistionNumber : b.RequistionNumber,
                        TimeStamp: b.TimeStamp,
                        CreatedOn: b.CreatedOn,
                        CreatedBy: b.CreatedBy,
                        UpdatedBy: b.UpdatedBy,
                        Deleted: b.Deleted,
                                               
                        BranchName: b.BranchName,
                      
                        ActivityId: b.ActivityId,
                        ActivityName: b.ActivityName,
                        BatchId: b.BatchId,
                        BatchName: b.BatchName,
                        SupplyId: b.SupplyId,
                        Quantity: b.Quantity,
                        WeightNoteNumber: b.WeightNoteNumber,
                        CasualWorkerId: b.CasualWorkerId,
                        RequistionCategoryId: b.RequistionCategoryId,
                        PartPayment: b.PartPayment,
                        CasualWorkerName: b.CasualWorkerName,
                        RequistionCategoryName: b.RequistionCategoryName,
                        RepairerName: b.RepairerName,
                        RepairDate: b.RepairDate != null ? moment(b.RepairDate).format('YYYY-MM-DD HH:mm:ss') : null,
                        UtilityCategoryId: b.UtilityCategoryId,
                        BankId: b.BankId,
                        BankName: b.BankName,
                        UtilityCategoryName : b.UtilityCategoryName,


                    };

                });


        }

        $scope.Save = function (requistion) {

            $scope.showMessageSave = false;
            if ($scope.form.$valid) {
                usSpinnerService.spin('global-spinner');
                var promise = $http.post('/webapi/RequistionApi/Save', {
                    RequistionId: requistionId,
                    BranchId: requistion.BranchId,
                    Description : requistion.Description,
                    Response: requistion.Response,
                    Amount: requistion.Amount,
                    AmountInWords : requistion.AmountInWords,
                    StatusId : statusId,
                    ApprovedById: requistion.ApprovedById,
                    RequistionNumber : requistion.RequistionNumber,
                    CreatedBy: requistion.CreatedBy,
                    CreatedOn: requistion.CreatedOn,
                    Deleted: requistion.Deleted,
                    ActivityId: requistion.ActivityId,
                    RepairerName: requistion.RepairerName,
                    BatchId: requistion.BatchId,
                    RepairDate: requistion.RepairDate,
                    SupplyId: requistion.SupplyId,

                    CasualWorkerId: requistion.CasualWorkerId,
                    Quantity: requistion.Quantity,
                    PartPayment: requistion.PartPayment,
                    RequistionCategoryId: requistion.RequistionCategoryId,
                    BankId: requistion.BankId,
                    UtilityCategoryId : requistion.UtilityCategoryId,


                });

                promise.then(
                    function (payload) {

                        requistionId = payload.data;

                        $scope.showMessageSave = true;
                        usSpinnerService.stop('global-spinner');
                        $timeout(function () {
                            $scope.showMessageSave = false;


                            if (action == "create" || action == "edit") {
                                $state.go('status-requistions', { 'statusId': 10002, 'action': status, 'branchId': requistion.BranchId });

                            }

                        }, 1500);


                    });
            }

        }

         $scope.Approve = function (requistion) {

            $scope.showMessageSave = false;
            if ($scope.form.$valid) {
                usSpinnerService.spin('global-spinner');
                var promise = $http.post('/webapi/RequistionApi/Save', {
                    RequistionId: requistionId,
                    BranchId: requistion.BranchId,
                    Description : requistion.Description,
                    Response: requistion.Response,
                    Amount: requistion.Amount,
                    AmountInWords : requistion.AmountInWords,
                    StatusId: approvedStatusId,
                    ApprovedById: $scope.user.Id,
                    Approved: true,
                    Rejected : requistion.Rejected,
                    RequistionNumber : requistion.RequistionNumber,
                    CreatedBy: requistion.CreatedBy,
                    CreatedOn: requistion.CreatedOn,
                    Deleted: requistion.Deleted,

                    ActivityId: requistion.ActivityId,
                    Quantity: requistion.Quantity,
                    BatchId: requistion.BatchId,
                    RepairerName: requistion.RepairerName,
                    RepairDate: requistion.RepairDate,
                    SupplyId: requistion.SupplyId,

                    CasualWorkerId: requistion.CasualWorkerId,

                    PartPayment: requistion.PartPayment,
                    RequistionCategoryId: requistion.RequistionCategoryId,
                    BankId: requistion.BankId,
                    UtilityCategoryId: requistion.UtilityCategoryId,


                });

                promise.then(
                    function (payload) {

                        requistionId = payload.data;
                        if (requistionId == -1) {
                            $scope.showMessageCashNotEnough = true;
                            usSpinnerService.stop('global-spinner');

                            $timeout(function () {
                                $scope.showMessageCashNotEnough = false;

                            }, 4000);
                        }

                        else if (requistionId == -2) {
                            $scope.showMessageSupplyDoesntExist = true;
                            usSpinnerService.stop('global-spinner');

                            $timeout(function () {
                                $scope.showMessageSupplyDoesntExist = false;

                            }, 6000);
                        }
                        else if (requistionId == -4) {
                            $scope.showMessagePayingMore = true;
                            usSpinnerService.stop('global-spinner');

                            $timeout(function () {
                                $scope.showMessagePayingMore = false;

                            }, 6000);
                        }
                        else if (requistionId == -5) {
                            $scope.showMessageLabourCostHasNoQuantity = true;
                            usSpinnerService.stop('global-spinner');

                            $timeout(function () {
                                $scope.showMessageLabourCostHasNoQuantity = false;

                            }, 6000);
                        }
                        else if (requistionId == -6) {
                            $scope.showMessageSupplyAlreadyPaid = true;
                            usSpinnerService.stop('global-spinner');

                            $timeout(function () {
                                $scope.showMessageSupplyAlreadyPaid = false;

                            }, 6000);
                        }
                        else if (requistionId == -7) {
                            $scope.showMessageFactoryExpense = true;
                            usSpinnerService.stop('global-spinner');

                            $timeout(function () {
                                $scope.showMessageFactoryExpense = false;

                            }, 6000);
                        }
                        else if (requistionId == -8) {
                            $scope.showMessageMachine = true;
                            usSpinnerService.stop('global-spinner');

                            $timeout(function () {
                                $scope.showMessageMachine = false;

                            }, 6000);
                        }
                        else if (requistionId == -9) {
                            $scope.showMessageAllowance = true;
                            usSpinnerService.stop('global-spinner');

                            $timeout(function () {
                                $scope.showMessageAllowance = false;

                            }, 6000);
                        }
                        else if (requistionId == -22) {
                            $scope.showMessageActivityDont = true;
                            usSpinnerService.stop('global-spinner');

                            $timeout(function () {
                                $scope.showMessageActivityDont = false;

                            }, 6000);
                        }
                        else if (requistionId == -23) {
                            $scope.showMessageCasual = true;
                            usSpinnerService.stop('global-spinner');

                            $timeout(function () {
                                $scope.showMessageCasual = false;

                            }, 6000);
                        }
                        else if (requistionId == -44) {
                            $scope.showMessageUtility = true;
                            usSpinnerService.stop('global-spinner');

                            $timeout(function () {
                                $scope.showMessageUtility = false;

                            }, 6000);
                        }
                        else if (requistionId == -33) {
                            $scope.showMessageAdvance = true;
                            usSpinnerService.stop('global-spinner');

                            $timeout(function () {
                                $scope.showMessageAdvance = false;

                            }, 6000);
                        }
                        else {
                            $scope.showMessageApprove = true;
                            usSpinnerService.stop('global-spinner');
                            $timeout(function () {
                                $scope.showMessageApprove = false;

                                $state.go('status-requistions', { 'statusId': 10002, 'action': status, 'branchId': requistion.BranchId });


                            }, 6000);

                        }


                       


                    });
            }

        }

         $scope.Reject = function (requistion) {

             usSpinnerService.spin('global-spinner');

             var promise = $http.post('/webapi/RequistionApi/Save', {
                 RequistionId: requistionId,
                 BranchId: requistion.BranchId,
                 Description: requistion.Description,
                 Response: requistion.Response,
                 Amount: requistion.Amount,
                 AmountInWords : requistion.AmountInWords,
                 Approved: requistion.Approved,
                 Rejected : true,
                 StatusId: statusId,
                 ApprovedById: $scope.user.Id,
                 RequistionNumber: requistion.RequistionNumber,
                 CreatedBy: requistion.CreatedBy,
                 CreatedOn: requistion.CreatedOn,
                 Deleted: requistion.Deleted,

                 ActivityId: requistion.ActivityId,
                 RepairerName: requistion.RepairerName,
                 BatchId: requistion.BatchId,
                 RepairDate: requistion.RepairDate,
                 SupplyId: requistion.SupplyId,

                 CasualWorkerId: requistion.CasualWorkerId,
                 Quantity: requistion.Quantity,
                 PartPayment: requistion.PartPayment,
                 RequistionCategoryId: requistion.RequistionCategoryId,
                 BankId: requistion.BankId,
                 UtilityCategoryId: requistion.UtilityCategoryId,



             });

             promise.then(
                 function (payload) {

                     requistionId = payload.data;
                     usSpinnerService.stop('global-spinner');
                     $timeout(function () {

                         if (action == "create" || action == "edit") {
                             $state.go('status-requistions', { 'statusId': 10002, 'action': status, 'branchId': requistion.BranchId });

                         }

                     }, 500);
                 });
         }


        $scope.Cancel = function () {
            $state.go('status-requistions', { 'statusId': 10002, 'action': status, 'branchId': requistion.BranchId });

        };

        $scope.Delete = function (requistionId) {
            $scope.showMessageDeleted = false;
            var promise = $http.get('/webapi/RequistionApi/Delete?requistionId=' + requistionId, {});
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
    .module('homer').controller('RequistionController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/RequistionApi/GetAllRequistions');
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
                    name: 'Requistion Number', field:'RequistionNumber',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    }
                },
                { name: 'Amount', field: 'Amount' },
                {name : 'AmountInWords',field: 'AmountInWords'},
                { name: 'Description', field: 'Description' },
                { name: 'Response', field: 'Response' },
                { name: 'Status', field: 'StatusName' },
                { name: 'ApprovedBy', field: 'ApprovedByName' },
                 {
                     name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/requistions/edit/{{row.entity.RequistionId}}">Edit</a> </div>',
                    
                 },
                  {
                      name: 'Print', cellTemplate: '<div class="ui-grid-cell-contents" ng-if="row.entity.StatusId == 2"><a  href="/Excel/Document?documentId={{row.entity.DocumentId}}">Print</a></div>'
                  },

                  

            ];




        }]);

angular
    .module('homer').controller('StatusRequistionController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
           
            var statusId = $scope.statusId;
            var promise = $http.get('/webapi/RequistionApi/GetAllRequistionsForAParticularStatusForBranch?statusId=' + statusId, {});
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
                    name: 'Requistion Number', field:'RequistionNumber',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    }
                },
                { name: 'Amount', field: 'Amount' },
                  { name: 'AmountInWords', field: 'AmountInWords' },
                { name: 'Description', field: 'Description' },
                { name: 'Response', field: 'Response' },
                //{ name: 'Status', field: 'StatusName' },
                 //{ name: 'Status', cellTemplate: '<div ng-if="row.entity.Approved ==\'True\'">Approved</div><div ng-if="row.entity.Rejected ==\'True\'">Rejected</div><div ng-if="row.entity.Rejected ==\'False\' || row.entity.Approved ==\'False\'">Open</div>' },
                 { name: 'Status', cellTemplate: '<div ng-if="row.entity.Approved">Approved</div><div ng-if="row.entity.Rejected">Rejected</div><div ng-if="!row.entity.Rejected && !row.entity.Approved">Open</div>' },

                { name: 'ApprovedBy', field: 'ApprovedByName' },
                  {
                      name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"ng-if="!row.entity.Approved"> <a href="#/requistions/edit/{{row.entity.RequistionId}}">Edit</a> </div>',

                  },
                   
                {
                    name: 'Details', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/requistions/edit/details/{{row.entity.RequistionId}}">Details</a> </div>',

                },

                    {
                        name: 'Print', cellTemplate: '<div class="ui-grid-cell-contents" ng-if="row.entity.StatusId == 2"><a  href="/Excel/Document?documentId={{row.entity.DocumentId}}">Print</a></div>'
                    },


            ];




        }]);



angular
    .module('homer').controller('BranchRequistionController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;

            var branchId = $scope.branchId;
            var promise = $http.get('/webapi/RequistionApi/GetAllRequistionsForAParticularBranch?branchId=' + branchId, {});
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
                    name: 'Requistion Number',field:'RequistionNumber',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    }
                },
                { name: 'Amount', field: 'Amount' },
                 { name: 'AmountInWords', field: 'AmountInWords' },
                { name: 'Description', field: 'Description' },
                { name: 'Response', field: 'Response' },
                { name: 'Status', field: 'StatusName' },
                { name: 'ApprovedBy', field: 'ApprovedByName' },
                  {
                      name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/requistions/edit/{{row.entity.RequistionId}}">Edit</a> </div>',

                  },

            ];




        }]);
