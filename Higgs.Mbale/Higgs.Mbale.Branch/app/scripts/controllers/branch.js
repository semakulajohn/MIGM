angular
    .module('homer')
    .controller('BranchEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval', 'usSpinnerService',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval,usSpinnerService) {

        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }

        var branchId = $scope.branchId;
        var action = $scope.action;

       


        if (action == 'create') {
            branchId = 0;

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



            var promise = $http.get('/webapi/BranchApi/GetBranch?branchId=' + branchId, {});
            promise.then(
                function (payload) {
                    var b = payload.data;

                    $scope.branch = {
                        BranchId: b.BranchId,
                        Name: b.Name,
                        PhoneNumber : b.PhoneNumber,
                        Location: b.Location,
                        TimeStamp: b.TimeStamp,
                        MillingChargeRate : b.MillingChargeRate,
                        CreatedOn: b.CreatedOn,
                        CreatedBy: b.CreatedBy,
                        UpdatedBy: b.UpdatedBy,
                        Deleted : b.Deleted,


                    };

                });


        }

        $scope.Save = function (branch) {

            $scope.showMessageSave = false;
            if ($scope.form.$valid) {
                usSpinnerService.spin('global-spinner');
                var promise = $http.post('/webapi/BranchApi/Save', {
                    BranchId: branchId,
                    Name: branch.Name,
                    PhoneNumber: branch.PhoneNumber,
                    MillingChargeRate : branch.MillingChargeRate,
                    CreatedBy: branch.CreatedBy,
                    CreatedOn: branch.CreatedOn,
                    Location: branch.Location,
                    Deleted : branch.Deleted,

                });

                promise.then(
                    function (payload) {

                        branchId = payload.data;
                        usSpinnerService.stop('global-spinner');
                        $scope.showMessageSave = true;

                        $timeout(function () {
                            $scope.showMessageSave = false;


                            if (action == "create") {
                                $state.go('branch-edit', { 'action': 'edit', 'branchId': branchId });
                            }

                        }, 1500);


                    });
            }

        }



        $scope.Cancel = function () {
            $state.go('branches.list');

        };

        $scope.Delete = function (branchId) {
            $scope.showMessageDeleted = false;
            var promise = $http.get('/webapi/BranchApi/Delete?branchId=' + branchId, {});
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
    .module('homer').controller('BranchController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, $filter, $location, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/BranchApi/GetAllBranches');
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
                    name: 'Name',field:'Name',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    }
                },

                { name: 'Phone Number', field: 'PhoneNumber' },

                 { name: 'Location', field: 'Location' },
                 {name: 'Milling Charge Rate',field : 'MillingChargeRate'},

  { name: 'View', field: 'Id', width: '15%', cellTemplate: '<div class="ui-grid-cell-contents"><a href="#/branches/{{row.entity.BranchId}}">Dashboard</a></div>' },

                 {
                     name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/branches/edit/{{row.entity.BranchId}}">Edit</a> </div>',
                    
                 },


            ];




        }]);

angular
    .module('homer')
    .controller('BranchDashBoardController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, $state, uiGridConstants, $interval) {

        $scope.tab = {};
        if ($scope.defaultTab == 'dashboard') {
            $scope.tab.dashboard = true;
        }

        var branchId = $scope.branchId;
       

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
                    var promise = $http.get('/webapi/UserApi/GetLoggedInUserBranch?userBranchId=' + $scope.user.Id, {});
                    promise.then(
                        function (payload) {
                            var c = payload.data;

                            $scope.userBranch = {
                                UserId: c.UserId,
                                BranchId: c.BranchId,
                            };

                           
                            if ($scope.userBranch != null) {
                                branchId = $scope.userBranch.BranchId
                                populateDashboard(branchId);
                            }
                        });

                }

            );

           
         
            function populateDashboard(branchId) {
              

                var promise = $http.get('/webapi/BranchApi/GetBranch?branchId=' + branchId, {});
                promise.then(
                    function (payload) {
                        var b = payload.data;

                        $scope.branch = {
                            BranchId: b.BranchId,
                            Name: b.Name,
                            PhoneNumber: b.PhoneNumber,
                            Location: b.Location,
                            TimeStamp: b.TimeStamp,
                            CreatedOn: b.CreatedOn,
                            CreatedBy: b.CreatedBy,
                            UpdatedBy: b.UpdatedBy,
                            Deleted: b.Deleted,


                        };

                    });


                //var promise = $http.get('/webapi/CasualWorkerApi/GetAllCasualWorkersForAParticularBranch?branchId=' + branchId, {});
                //promise.then(
                //    function (payload) {
                //        $scope.gridData.data = payload.data;
                //        $scope.loadingSpinner = false;
                //    }
                //);

                //$scope.gridData = {
                //    enableFiltering: true,
                //    columnDefs: $scope.columns,
                //    enableRowSelection: true
                //};

                //$scope.gridData.multiSelect = true;

                //$scope.gridData.columnDefs = [
                //    { name: 'Id Number', field: 'UniqueNumber' },
                //    {
                //        name: 'FirstName', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/casualWorkers/edit/{{row.entity.CasualWorkerId}}">{{row.entity.FirstName}}</a> </div>',
                //        sort: {
                //            direction: uiGridConstants.ASC,
                //            priority: 1
                //        }
                //    },
                //    {
                //        name: 'LastName', field: 'LastName'
                //    },
                //    {
                //        name: 'Phone', field: 'PhoneNumber'
                //    },
                //   {
                //       name: 'Address', field: 'Address'
                //   },
                //   { name: 'Nin Number', field: 'NINNumber' },
                //   { name: 'Next Of keen', field: 'NextOfKeen' },
                //   { name: 'Email', field: 'EmailAddress' },
                //   {
                //       name: 'Branch', field: 'BranchName'
                //   },
                //   { name: 'Casual Activities', field: 'Id', width: '10%', cellTemplate: '<div class="ui-grid-cell-contents"><a href="#/casualActivities/{{row.entity.CasualWorkerId}}">Manage Casual Activities</a></div>' },
                //{ name: 'Transaction Activities', field: 'Id', width: '15%', cellTemplate: '<div class="ui-grid-cell-contents"><a href="#/accounttransactionactivities/{{row.entity.CasualWorkerId}}">Manage Transaction Activities</a></div>' },

                //];

              


            }

              
    }
        
    ]);



