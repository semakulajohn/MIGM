angular
    .module('homer')
    .controller('RiceInPutEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'usSpinnerService',
        function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval, usSpinnerService) {

            $scope.tab = {};
            if ($scope.defaultTab == 'dashboard') {
                $scope.tab.dashboard = true;
            }
            $scope.selectedGrades = [];
            var branches = [];
            //var branchId = $scope.branchId;
            var selectedBranchId = 0;
            var riceInputId = $scope.riceInputId;
            var action = $scope.action;

            $scope.showMessageRiceInput = false;


            $http.get('webapi/GradeApi/GetAllGrades').success(function (data, status) {
                $scope.grades = data;
            });
            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.xdata = {
                    branches: data,
                    selectedBranch: branches[0]
                }
            });

           

            $scope.OnBranchChange = function (riceInput) {
                selectedBranchId = riceInput.BranchId

                $http.get('/webapi/StoreApi/GetAllBranchStores?branchId=' + selectedBranchId).then(function (responses) {
                    $scope.stores = responses.data;

                });
               

            }

            if (action == 'create') {
                riceInputId = 0;
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

                var promise = $http.get('/webapi/RiceInputApi/GetRiceInput?riceInputId=' + riceInputId, {});
                promise.then(
                    function (payload) {
                        var b = payload.data;

                        $scope.riceInput = {
                            RiceInputId: b.RiceInputId,
                            TotalQuantity: b.TotalQuantity,
                            TotalAmount: b.TotalAmount,
                            StoreId: b.StoreId,
                            Price: b.Price,
                            BranchId: b.BranchId,
                            Approved: b.Approved,
                            TimeStamp: b.TimeStamp,
                            CreatedOn: b.CreatedOn,
                            CreatedBy: b.CreatedBy,
                            UpdatedBy: b.UpdatedBy,
                            Deleted: b.Deleted,
                            Grades: b.Grades

                        };

                    });


            }



            $scope.Save = function (riceInput) {

                $scope.TotalGradeKgs = 0;
                $scope.TotalAmount = 0;
                $scope.DenominationKgs = 0;
                $scope.DenominationAmount = 0;
                $scope.showMessageSave = false;

                angular.forEach($scope.selectedGrades, function (value, key) {
                    var denominations = value.Denominations;
                    angular.forEach(denominations, function (denominations) {
                        $scope.DenominationAmount = (riceInput.Price * denominations.Quantity * denominations.Value) + $scope.DenominationAmount;

                        $scope.DenominationKgs = (denominations.Value * denominations.Quantity) + $scope.DenominationKgs;
                    });
                    $scope.TotalGradeKgs = $scope.DenominationKgs;
                    $scope.TotalAmount = $scope.DenominationAmount;
                });

                $scope.showMessageRiceInput = false;

                if ($scope.form.$valid) {
                    usSpinnerService.spin('global-spinner');
                    var promise = $http.post('/webapi/RiceInputApi/Save', {
                        RiceInputId: riceInputId,
                        TotalQuantity: $scope.TotalGradeKgs,
                        TotalAmount: $scope.TotalAmount,
                        StoreId: riceInput.StoreId,
                        BranchId: selectedBranchId,
                        Price: riceInput.Price,
                        Grades: riceInputId == 0 ? $scope.selectedGrades : riceInput.Grades
                    });

                    promise.then(
                        function (payload) {

                            riceInputId = payload.data;

                            if (riceInputId == 0) {
                                $scope.showMessageNoGradeSelected = true;
                                usSpinnerService.stop('global-spinner');

                                $timeout(function () {
                                    $scope.showMessageNoGradeSelected = false;

                                }, 4000);
                            }
                            else if (riceInputId == -1) {
                                $scope.showMessageNotEnoughBuvera = true;
                                usSpinnerService.stop('global-spinner');

                                $timeout(function () {
                                    $scope.showMessageNotEnoughBuvera = false;

                                }, 4000);
                            }
                            
                            else if (riceInputId == -2) {
                                $scope.showMessageNoGradeBuvera = true;
                                usSpinnerService.stop('global-spinner');

                                $timeout(function () {
                                    $scope.showMessageNoGradeBuvera = false;

                                }, 4000);
                            }
                            else {


                                $scope.showMessageSave = true;
                                usSpinnerService.stop('global-spinner');

                                $timeout(function () {
                                    $scope.showMessageSave = false;

                                    $state.go('store-flourStanding', { 'storeId': storeId });

                                }, 1500);
                            }

                        });
                }



            }




            $scope.Cancel = function () {
                $state.go('store-flourStanding', { 'storeId': storeId });
            };



        }
    ]);




angular
    .module('homer').controller('RiceInputController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;

            var branchId = $scope.branchId;

           // var promise = $http.get('/webapi/RiceInputApi/GetAllRiceInputsForAParticularBranch?branchId=' + branchId, {});

            var promise = $http.get('/webapi/RiceInputApi/GetAllRiceInputs');
                promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;
                }
            );

            $scope.gridData = {
                enableColumnResizing: true,
                enableFiltering: false,
                columnDefs: $scope.columns,
                enableRowSelection: false
            };

            $scope.gridData.multiSelect = false;

            $scope.gridData.columnDefs = [

                {
                    name: 'Id', cellTemplate: '<div class="ui-grid-cell-contents"> {{row.entity.RiceInputId}} </div>',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    },
                    width: '5%'
                },

                { name: 'Rice(kgs)', field: 'TotalQuantity', width: '15%' },

                { name: 'Price/kg', field: 'Price' ,width : '10%'},
                { name: 'Cost', field: 'TotalAmount', width: '20%' },
                { name: 'Branch', field: 'BranchName', width: '20%'},

                { name: 'Store', field: 'StoreName', width: '20%' },

                {
                    name: 'Details', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/riceinputs/edit/{{row.entity.RiceInputId}}> Details</a> </div>',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    },
                    width: '5%'
                },

            ];




        }]);


angular
    .module('homer').controller('UnApprovedRiceInputsController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;


            var promise = $http.get('/webapi/RiceInputApi/GetAllUnApprovedRiceInputs', {});
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;
                }
            );

            $scope.gridData = {
                enableColumnResizing: true,
                enableFiltering: false,
                columnDefs: $scope.columns,
                enableRowSelection: false
            };

            $scope.gridData.multiSelect = false;

            $scope.gridData.columnDefs = [

               

                { name: 'Flour(kgs)', field: 'TotalQuantity' },
                { name: 'Price/kg', field: 'Price' },
                { name: 'Cost', field: 'TotalAmount' },
                { name: 'Branch', field: 'BranchName' },

                { name: 'Store', field: 'StoreName' },
                { name: ' Details', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/unapproveddetail/rice/unapproved/{{row.entity.RiceInputId}}">Details</a> </div>' },


            ];




        }]);

angular
    .module('homer')
    .controller('RiceInputUnApprovedDetailController', ['$scope', '$http', '$filter', '$log', '$timeout', '$state', 'usSpinnerService',
        function ($scope, $http, $filter, $log, $timeout, $state, usSpinnerService) {
            $scope.tab = {};
            if ($scope.defaultTab == 'dashboard') {
                $scope.tab.dashboard = true;
            }

            var riceInPutId = $scope.riceInPutId;

            var promise = $http.get('/webapi/RiceInputApi/GetRiceInput?riceInPutId=' + riceInPutId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;
                    if (b.Accept != true && b.Reject != true) {
                        $scope.hideAcceptReject = true;
                    }
                    else if (b.Accept == true || b.Reject == true) {
                        $scope.hideAcceptReject = false;
                    }

                    $scope.riceInPut = {
                        RiceInputId: b.RiceInputId,

                        TotalQuantity: b.TotalQuantity,
                        StoreId: b.StoreId,
                        BranchId : b.BranchId,
                        TotalAmount: b.TotalAmount,
                        Price: b.Price,
                        StoreName: b.StoreName,
                        TimeStamp: b.TimeStamp,
                        Grades: b.Grades,
                        CreatedById: b.CreatedById,

                        Approved: b.Approved,
                    };
                });

            $scope.Approve = function (riceInput) {
                $scope.showMessageApproved = false;

                if ($scope.form.$valid) {

                    usSpinnerService.spin('global-spinner');
                    if ($scope.form.$valid) {
                        var promise = $http.post('/webapi/RiceInputApi/Save', {
                            RiceInputId: riceInput.RiceInputId,

                            TotalQuantity: riceInput.TotalQuantity,
                            StoreId: riceInput.StoreId,
                            TotalAmount: riceInput.TotalAmount,
                            Price: riceInput.Price,
                            BranchId: riceInput.BranchId,
                            TimeStamp: riceInput.TimeStamp,
                            Grades: riceInput.Grades,
                            CreatedById: riceInput.CreatedById,

                            Approved: true,
                        });

                        promise.then(
                            function (payload) {

                                riceInputId = payload.data;
                                if (riceInputId == -22) {
                                    $scope.showMessageYouCreated = true;
                                    usSpinnerService.stop('global-spinner');
                                    $timeout(function () {
                                        $scope.showMessageYouCreated = false;
                                        $state.go('unapprovedRiceInputs-list');

                                    }, 4000);
                                }
                                else {
                                    $scope.showMessageApproved = true;
                                    usSpinnerService.stop('global-spinner');
                                    $timeout(function () {
                                        $scope.showMessageApproved = false;

                                        $state.go('unapprovedRiceInputs-list');

                                    }, 3000);
                                }

                            });
                    }

                }

            }

            $scope.Reject = function (riceInput) {
                $scope.showMessageRejected = false;

                if ($scope.form.$valid) {

                    usSpinnerService.spin('global-spinner');
                    if ($scope.form.$valid) {
                        var promise = $http.post('/webapi/RiceInputApi/Save', {
                            RiceInputId: riceInput.RiceInputId,

                            TotalQuantity: riceInput.TotalQuantity,
                            StoreId: riceInput.StoreId,
                            TotalAmount: riceInput.TotalAmount,
                            Price: riceInput.Price,
                            BranchId: riceInput.BranchId,
                            TimeStamp: riceInput.TimeStamp,
                            Grades: riceInput.Grades,
                            CreatedById: riceInput.CreatedById,

                            Approved: false,

                        });

                        promise.then(
                            function (payload) {

                                riceInputId = payload.data;


                                usSpinnerService.stop('global-spinner');
                                $timeout(function () {
                                    $scope.showMessageRejected = false;


                                    $state.go('unapprovedRiceInputs-list');

                                }, 3000);
                                if (RiceInputId = -22) {
                                    $scope.showMessageRejectYouCreated = true;
                                    usSpinnerService.stop('global-spinner');
                                    $timeout(function () {
                                        $scope.showMessageRejectYouCreated = false;
                                        $state.go('unapprovedRiceInputs-list');

                                    }, 4000);
                                }
                                else {
                                    $scope.showMessageRejected = true;
                                    usSpinnerService.stop('global-spinner');
                                    $timeout(function () {
                                        $scope.showMessageRejected = false;

                                        $state.go('unapprovedRiceInputs-list');

                                    }, 3000);
                                }
                            });
                    }
                }

            }

        }]);
angular
    .module('homer')
    .controller('RiceInputDetailController', ['$scope', '$http', '$filter', '$log', '$timeout', '$state', 'usSpinnerService',
        function ($scope, $http, $filter, $log, $timeout, $state, usSpinnerService) {
            $scope.tab = {};
            if ($scope.defaultTab == 'dashboard') {
                $scope.tab.dashboard = true;
            }

            var riceInputId = $scope.riceInputId;

            var promise = $http.get('/webapi/RiceInputApi/GetRiceInPut?riceInputId=' + riceInputId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;

                    $scope.riceInput = {
                        RiceInputId: b.RiceInputId,

                        TotalQuantity: b.TotalQuantity,
                        StoreId: b.StoreId,
                        TotalAmount: b.TotalAmount,
                        Price: b.Price,
                        StoreName: b.StoreName,
                        BranchId: b.BranchId,
                        BranchName : b.BranchName,
                        TimeStamp: b.TimeStamp,
                        Grades: b.Grades,
                        CreatedById: b.CreatedById,

                        Approved: b.Approved,
                    };
                });
        }]);
