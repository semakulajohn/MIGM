angular
    .module('homer')
    .controller('WeightNoteRangeEditController', ['$scope', '$http', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval','usSpinnerService',
        function ($scope, $http, $timeout, $modal, $state, uiGridConstants, $interval, usSpinnerService) {

            $scope.tab = {};
            if ($scope.defaultTab == 'dashboard') {
                $scope.tab.dashboard = true;
            }

            var weightNoteRangeId = $scope.weightNoteRangeId;
            var action = $scope.action;

           

            $http.get('/webapi/BranchApi/GetAllBranches').success(function (data, status) {
                $scope.branches = data;
            });

            if (action == 'create') {
                assetId = 0;

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
                var promise = $http.get('/webapi/WeightNoteRangeApi/GetWeightNoteRange?weightNoteRangeId=' + weightNoteRangeId, {});
                promise.then(
                    function (payload) {
                        var b = payload.data;
                        $scope.weightNoteRange = {
                            WeightNoteRangeId: b.WeightNoteRangeId,
                            
                            StartNumber: b.StartNumber,
                            EndNumber: b.EndNumber,
                            
                            Printed: b.Printed,
                            BranchId: b.BranchId,
                           
                            TimeStamp: b.TimeStamp,
                            CreatedOn: b.CreatedOn,
                            CreatedBy: b.CreatedBy,
                            UpdatedBy: b.UpdatedBy,
                            Deleted: b.Deleted
                        };

                    });


            }

            $scope.Save = function (weightNoteRange) {
                $scope.showMessageSave = false;
                if ($scope.form.$valid) {
                    usSpinnerService.spin('global-spinner');
                    if ((parseInt(weightNoteRange.StartNumber) == parseInt(weightNoteRange.EndNumber)) || (parseInt(weightNoteRange.StartNumber) > parseInt(weightNoteRange.EndNumber))) {
                        
                        $scope.showMessageEndNumberShouldBeGreater = true;
                        usSpinnerService.stop('global-spinner');

                        $timeout(function () {
                            $scope.showMessageEndNumberShouldBeGreater = false;

                            $state.go('weightnoteranges.list');

                        }, 4000);

                    }
                    else {
                        var promise = $http.post('/webapi/WeightNoteRangeApi/Save', {
                            WeightNoteRangeId: weightNoteRangeId,

                            StartNumber: weightNoteRange.StartNumber,
                            CreatedBy: weightNoteRange.CreatedBy,
                            CreatedOn: weightNoteRange.CreatedOn,
                            Deleted: weightNoteRange.Deleted,
                            EndNumber: weightNoteRange.EndNumber,
                            Printed: weightNoteRange.Printed,
                            BranchId: weightNoteRange.BranchId,

                        });

                        promise.then(
                            function (payload) {

                                weightNoteRangeId = payload.data;
                                if (weightNoteRangeId == -1) {
                                    $scope.showMessageRangeExists = true;
                                    usSpinnerService.stop('global-spinner');

                                    $timeout(function () {
                                        $scope.showMessageRangeExists = false;
                                        $state.go('weightnoteranges.list');

                                    }, 4000);
                                }
                                else {
                                    $scope.showMessageSave = true;
                                    usSpinnerService.stop('global-spinner');

                                    $timeout(function () {
                                        $scope.showMessageSave = false;

                                        $state.go('weightnoteranges.list');

                                    }, 4000);

                                }


                            });
                       
                    }
                   
                }

            }

            $scope.Cancel = function () {
                $state.go('weightnoteranges.list');
            };

            $scope.Delete = function (weightNoteRangeId) {
                $scope.showMessageDeleted = false;
                var promise = $http.get('/webapi/WeightNoteRangeApi/Delete?weightNoteRangeId=' + weightNoteRangeId, {});
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
    .module('homer').controller('WeightNoteRangeController', ['$scope', 'ngTableParams', '$http', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/WeightNoteRangeApi/GetAllWeightNoteRangesForWeb');
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
                    name: 'WeightNoteRangeId', field: 'WeightNoteRangeId',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    }
                },
                {
                    name: 'Start Number', field: 'StartNumber',

                },
                {
                    name: 'End Number', field: 'EndNumber',
                },
                {
                    name: 'Branch', field: 'BranchName',
                },

                {
                    name: 'Details', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/weightnoteranges/detail/{{row.entity.BranchId}}/{{row.entity.WeightNoteRangeId}}">Details</a> </div>',

                },
                {
                    name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/weightnoteranges/edit/{{row.entity.WeightNoteRangeId}}">Edit</a> </div>',

                },


            ];




        }]);


angular
    .module('homer').controller('BranchWeightNoteRangeController', ['$scope', '$http', 'uiGridConstants',
        function ($scope, $http, uiGridConstants) {
            $scope.loadingSpinner = true;
            var branchId = $scope.branchId;

            var promise = $http.get('/webapi/WeightNoteRangeApi/GetAllWeightNoteRangesForAParticularBranch?branchId=' + branchId, {});
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
                    name: 'WeightNoteRangeId', field: 'WeightNoteRangeId',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    }
                },
                {
                    name: 'Start Number', field: 'StartNumber',

                },
                {
                    name: 'End Number', field: 'EndNumber',
                },
                {
                    name: 'Branch', field: 'BranchName',
                },

                {
                    name: 'Details', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/weightnoteranges/edit/{{row.entity.WeightNoteRange}}">Details</a> </div>',

                },
               


            ];




        }]);

angular
    .module('homer')
    .controller('WeightNoteRangeDetailController', ['$scope', '$http', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'usSpinnerService',
        function ($scope, $http, $timeout, $modal, $state, uiGridConstants, $interval, usSpinnerService) {

            $scope.tab = {};
            if ($scope.defaultTab == 'dashboard') {
                $scope.tab.dashboard = true;
            }

            var weightNoteRangeId = $scope.weightNoteRangeId;
           
           
                var promise = $http.get('/webapi/WeightNoteRangeApi/GetWeightNoteRange?weightNoteRangeId=' + weightNoteRangeId, {});
                promise.then(
                    function (payload) {
                        var b = payload.data;
                        $scope.weightNoteRange = {
                            WeightNoteRangeId: b.WeightNoteRangeId,

                            StartNumber: b.StartNumber,
                            EndNumber: b.EndNumber,
                            WeightNoteNumbers : b.WeightNoteNumbers,
                            Printed: b.Printed,
                            BranchId: b.BranchId,

                            TimeStamp: b.TimeStamp,
                            CreatedOn: b.CreatedOn,
                            CreatedBy: b.CreatedBy,
                            UpdatedBy: b.UpdatedBy,
                            Deleted: b.Deleted
                        };

                    });


          

            $scope.GenerateWeightNoteNumbers = function (weightNoteRangeId) {
                $scope.showMessageNumbersGenerated = false;
                usSpinnerService.spin('global-spinner');
                var promise = $http.get('/webapi/WeightNoteRangeApi/GenerateWeightNoteNumbers?weightNoteRangeId=' + weightNoteRangeId, {});
                promise.then(
                    function (payload) {
                        $scope.showMessageNumbersGenerated = true;
                        usSpinnerService.stop('global-spinner');

                        $timeout(function () {
                            $scope.showMessageNumbersGenerated = false;
                            $state.go('weightnoteranges.list');

                        }, 2500);

                    });
                   
            }
        }
    ]);