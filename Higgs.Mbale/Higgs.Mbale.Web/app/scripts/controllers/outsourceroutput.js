angular
    .module('homer')
    .controller('OutSourcerOutPutEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'usSpinnerService',
        function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval, usSpinnerService) {

            $scope.tab = {};
            if ($scope.defaultTab == 'dashboard') {
                $scope.tab.dashboard = true;
            }
            $scope.selectedGrades = [];
            var storeId = $scope.storeId;
            var outSourcerOutPutId = $scope.outSourcerOutPutId;
            var action = $scope.action;
            
            $scope.showMessageFlourOutPut = false;


            $http.get('webapi/GradeApi/GetAllGrades').success(function (data, status) {
                $scope.grades = data;
            });
            

            if (action == 'create') {
                outSourcerOutPutId = 0;
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

                var promise = $http.get('/webapi/OutSourcerOutPutApi/GetOutSourcerOutPut?outSourcerOutPutId=' + outSourcerOutPutId, {});
                promise.then(
                    function (payload) {
                        var b = payload.data;

                        $scope.outSourcerOutPut = {
                            OutSourcerOutPutId : b.OutSourcerOutPutId,
                            TotalQuantity: b.TotalQuantity,
                            TotalAmount: b.TotalAmount,
                            StoreId: b.StoreId,
                            Price: b.Price,
                            PersonLoaded : b.PersonLoaded,
                            Approved : b.Approved,
                            TimeStamp: b.TimeStamp,
                            CreatedOn: b.CreatedOn,
                            CreatedBy: b.CreatedBy,
                            UpdatedBy: b.UpdatedBy,
                            Deleted: b.Deleted,
                            Grades: b.Grades

                        };

                    });


            }



            $scope.Save = function (outSourcerOutPut) {

                $scope.TotalGradeKgs = 0;
                $scope.TotalAmount = 0;
                $scope.DenominationKgs = 0;
                $scope.DenominationAmount = 0;
                $scope.showMessageSave = false;

                angular.forEach($scope.selectedGrades, function (value, key) {
                    var denominations = value.Denominations;
                    angular.forEach(denominations, function (denominations) {
                        $scope.DenominationAmount = (outSourcerOutPut.Price * denominations.Quantity * denominations.Value) + $scope.DenominationAmount;
                       
                        $scope.DenominationKgs = (denominations.Value * denominations.Quantity) + $scope.DenominationKgs;
                    });
                    $scope.TotalGradeKgs = $scope.DenominationKgs;
                    $scope.TotalAmount = $scope.DenominationAmount;
                });
               
                    $scope.showMessageFlourOutPut = false;

                    if ($scope.form.$valid) {
                        usSpinnerService.spin('global-spinner');
                        var promise = $http.post('/webapi/OutSourcerOutPutApi/Save', {
                            OutSourcerOutPutId: outSourcerOutPutId,
                            PersonLoaded : outSourcerOutPut.PersonLoaded,                                                    
                            TotalQuantity: $scope.TotalGradeKgs,
                            TotalAmount : $scope.TotalAmount,
                            StoreId: storeId,
                            Price : outSourcerOutPut.Price,
                            Grades: outSourcerOutPutId == 0 ? $scope.selectedGrades : outSourcerOutPut.Grades
                        });

                        promise.then(
                            function (payload) {

                                outSourcerOutPutId = payload.data;

                                if (outSourcerOutPutId == 0) {
                                    $scope.showMessageNoGradeSelected = true;
                                    usSpinnerService.stop('global-spinner');

                                    $timeout(function () {
                                        $scope.showMessageNoGradeSelected = false;

                                    }, 4000);
                                }
                                else if (outSourcerOutPutId == -1) {
                                    $scope.showMessageNotEnoughBuvera = true;
                                    usSpinnerService.stop('global-spinner');

                                    $timeout(function () {
                                        $scope.showMessageNotEnoughBuvera = false;

                                    }, 4000);
                                }
                                else if (outSourcerOutPutId == -3) {
                                    $scope.showMessageBatchAlreadyHasOutput = true;
                                    usSpinnerService.stop('global-spinner');

                                    $timeout(function () {
                                        $scope.showMessageBatchAlreadyHasOutput = false;

                                    }, 4000);
                                }
                                else if (outSourcerOutPutId == -2) {
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
    .module('homer').controller('OutSourcerOutPutController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;

           var storeId = $scope.storeId;

            var promise = $http.get('/webapi/OutSourcerOutPutApi/GetAllOutSourcerOutPutsForAParticularOutSourcerStore?storeId=' + storeId, {});
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
                    name: 'Id', cellTemplate: '<div class="ui-grid-cell-contents"> {{row.entity.BatchOutPutId}} </div>',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    },
                    //width: '5%'
                },
               
                { name: 'Flour(kgs)', field: 'TotalQuantity' },

                { name: 'Price/kg' , field: 'Price'},
                { name: 'Cost', field: 'TotalAmount' },
                { name: 'Client', field: 'PersonLoaded' },

                { name: 'OutSourcer', field: 'OutSourcerName' },
               
                {
                    name: 'Details', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/outputs/edit/{{row.entity.OutSourcerOutPutId}}> Details</a> </div>',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    },
                    //width: '5%'
                },

            ];




        }]);


angular
    .module('homer').controller('UnApprovedOutSourcerOutPutsController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;


            var promise = $http.get('/webapi/OutSourcerOutPutApi/GetAllUnApprovedOutSourcerOutPuts', {});
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

                //{
                //    name: 'Id', cellTemplate: '<div class="ui-grid-cell-contents"> {{row.entity.OutSourcerOutPutId}} </div>',
                //    sort: {
                //        direction: uiGridConstants.ASC,
                //        priority: 1
                //    },
                //    //width: '5%'
                //},

                { name: 'Flour(kgs)', field: 'TotalQuantity' },
                { name: 'Price/kg' , field: 'Price'},
                { name: 'Cost', field: 'TotalAmount' },
                { name: 'Client', field: 'PersonLoaded' },

                { name: 'OutSourcer', field: 'StoreName' },
                { name: ' Details', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/unapproveddetail/output/{{row.entity.OutSourcerOutPutId}}">Details</a> </div>' },
               

            ];




        }]);

angular
    .module('homer')
    .controller('OutSourcerOutPutUnApprovedDetailController', ['$scope', '$http', '$filter', '$log', '$timeout', '$state', 'usSpinnerService',
        function ($scope, $http, $filter, $log, $timeout, $state, usSpinnerService) {
            $scope.tab = {};
            if ($scope.defaultTab == 'dashboard') {
                $scope.tab.dashboard = true;
            }

            var outSourcerOutPutId = $scope.outSourcerOutPutId;

            var promise = $http.get('/webapi/OutSourcerOutPutApi/GetOutSourcerOutPut?outSourcerOutPutId=' + outSourcerOutPutId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;
                    if (b.Accept != true && b.Reject != true) {
                        $scope.hideAcceptReject = true;
                    }
                    else if (b.Accept == true || b.Reject == true) {
                        $scope.hideAcceptReject = false;
                    }

                    $scope.outPut = {
                        OutSourcerOutPutId: b.OutSourcerOutPutId,
                        
                        TotalQuantity: b.TotalQuantity,
                        StoreId: b.StoreId,
                        TotalAmount: b.TotalAmount,
                        Price : b.Price,
                        StoreName: b.StoreName,
                        PersonLoaded : b.PersonLoaded,
                        TimeStamp: b.TimeStamp,
                        Grades: b.Grades,
                        CreatedById: b.CreatedById,
                        
                        Approved: b.Approved,
                    };
                });

            $scope.Approve = function (outPut) {
                $scope.showMessageApproved = false;

                if ($scope.form.$valid) {

                    usSpinnerService.spin('global-spinner');
                    if ($scope.form.$valid) {
                        var promise = $http.post('/webapi/OutSourcerOutPutApi/Save', {
                            OutSourcerOutPutId: outPut.OutSourcerOutPutId,

                            TotalQuantity: outPut.TotalQuantity,
                            StoreId: outPut.StoreId,
                            TotalAmount: outPut.TotalAmount,
                            Price: outPut.Price,
                            PersonLoaded : outPut.PersonLoaded,
                            TimeStamp: outPut.TimeStamp,
                            Grades: outPut.Grades,
                            CreatedById: outPut.CreatedById,

                            Approved: true,
                        });

                        promise.then(
                            function (payload) {

                                outSourcerOutPutId = payload.data;
                                if (outSourcerOutPutId = -22) {
                                    $scope.showMessageYouCreated = true;
                                    usSpinnerService.stop('global-spinner');
                                    $timeout(function () {
                                        $scope.showMessageYouCreated = false;
                                        $state.go('unapprovedoutSourcerOutputs-list');

                                    }, 4000);
                                }
                                else {
                                    $scope.showMessageApproved = true;
                                    usSpinnerService.stop('global-spinner');
                                    $timeout(function () {
                                        $scope.showMessageApproved = false;

                                        $state.go('unapprovedoutSourcerOutputs-list');

                                    }, 3000);
                                }

                            });
                    }
                   
                }

            }

            $scope.Reject = function (outPut) {
                $scope.showMessageRejected = false;

                if ($scope.form.$valid) {

                    usSpinnerService.spin('global-spinner');
                    if ($scope.form.$valid) {
                        var promise = $http.post('/webapi/OutSourcerOutPutApi/Save', {
                            OutSourcerOutPutId: outPut.OutSourcerOutPutId,

                            TotalQuantity: outPut.TotalQuantity,
                            StoreId: outPut.StoreId,
                            TotalAmount: outPut.TotalAmount,
                            Price: outPut.Price,
                           PersonLoaded : outPut.PersonLoaded,
                            TimeStamp: outPut.TimeStamp,
                            Grades: outPut.Grades,
                            CreatedById: outPut.CreatedById,

                            Approved: false,
                            
                        });

                        promise.then(
                            function (payload) {

                                outSourcerOutPutId = payload.data;

                               
                                usSpinnerService.stop('global-spinner');
                                $timeout(function () {
                                    $scope.showMessageRejected = false;


                                    $state.go('unapprovedoutSourcerOutputs-list');

                                }, 3000);
                                if (outSourcerOutPutId == -22) {
                                    $scope.showMessageRejectYouCreated = true;
                                    usSpinnerService.stop('global-spinner');
                                    $timeout(function () {
                                        $scope.showMessageRejectYouCreated = false;
                                        $state.go('unapprovedoutSourcerOutputs-list');

                                    }, 4000);
                                }
                                else {
                                    $scope.showMessageRejected = true;
                                    usSpinnerService.stop('global-spinner');
                                    $timeout(function () {
                                        $scope.showMessageRejected = false;

                                        $state.go('unapprovedoutSourcerOutputs-list');

                                    }, 3000);
                                }
                            });
                    }
                }

            }

        }]);
angular
    .module('homer')
    .controller('OutSourcerOutPutDetailController', ['$scope', '$http', '$filter', '$log', '$timeout', '$state', 'usSpinnerService',
        function ($scope, $http, $filter, $log, $timeout, $state, usSpinnerService) {
            $scope.tab = {};
            if ($scope.defaultTab == 'dashboard') {
                $scope.tab.dashboard = true;
            }

            var outSourcerOutPutId = $scope.outSourcerOutPutId;

            var promise = $http.get('/webapi/OutSourcerOutPutApi/GetOutSoucerOutPut?outSourcerOutPutId=' + outSourcerOutPutId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;
                   
                    $scope.outPut = {
                        OutSourcerOutPutId: b.OutSourcerOutPutId,

                        TotalQuantity: b.TotalQuantity,
                        StoreId: b.StoreId,
                        TotalAmount: b.TotalAmount,
                        Price : b.Price,
                        StoreName: b.StoreName,
                        PersonLoaded : b.PersonLoaded,
                        TimeStamp: b.TimeStamp,
                        Grades: b.Grades,
                        CreatedById: b.CreatedById,

                        Approved: b.Approved,
                    };
                });
        }]);
