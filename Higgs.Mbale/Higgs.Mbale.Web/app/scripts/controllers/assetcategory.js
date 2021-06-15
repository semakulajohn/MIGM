angular
    .module('homer')
    .controller('AssetCategoryEditController', ['$scope', '$http', '$timeout', '$modal', '$state', 'usSpinnerService', '$interval',
        function ($scope, $http, $timeout, $modal, $state, usSpinnerService, $interval) {

            $scope.tab = {};
            if ($scope.defaultTab == 'dashboard') {
                $scope.tab.dashboard = true;
            }
            $scope.isDisabled = false;
            var assetCategoryId = $scope.assetCategoryId;
            var action = $scope.action;

            if (action == 'create') {
                assetCategoryId = 0;

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
                var promise = $http.get('/webapi/AssetCategoryApi/GetAssetCategory?assetCategoryId=' + assetCategoryId, {});
                promise.then(
                    function (payload) {
                        var b = payload.data;
                        $scope.assetCategory = {
                            AssetCategoryId: b.AssetCategoryId,
                            Name: b.Name,
                            TimeStamp: b.TimeStamp,
                            CreatedOn: b.CreatedOn,
                            CreatedBy: b.CreatedBy,
                            UpdatedBy: b.UpdatedBy,
                            Deleted: b.Deleted
                        };

                    });


            }

            $scope.Save = function (assetCategory) {
                $scope.showMessageSave = false;
                $scope.isDisabled = true;
                usSpinnerService.spin('global-spinner');
                if ($scope.form.$valid) {
                    $scope.loadingSpinner = true;
                    var promise = $http.post('/webapi/AssetCategoryApi/Save', {
                        AssetCategoryId: assetCategoryId,
                        Name: assetCategory.Name,
                        CreatedBy: assetCategory.CreatedBy,
                        CreatedOn: assetCategory.CreatedOn,
                        Deleted: assetCategory.Deleted,

                    });

                    promise.then(
                        function (payload) {

                            assetCategoryId = payload.data;
                            $scope.showMessageSave = true;
                            $scope.loadingSpinner = false;
                            usSpinnerService.stop('global-spinner');
                            $timeout(function () {
                                $scope.showMessageSave = false;
                                if (action == "create") {
                                    $state.go('assetCategory-edit', { 'action': 'edit', 'assetCategoryId': assetCategoryId });
                                }

                            }, 1500);


                        });
                }

            }

            $scope.Cancel = function () {
                $state.go('assetCategories.list');
            };

            $scope.Delete = function (assetCategoryId) {
                $scope.showMessageDeleted = false;
                var promise = $http.get('/webapi/AssetCategoryApi/Delete?assetCategoryId=' + assetCategoryId, {});
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
    .module('homer').controller('AssetCategoryController', ['$scope', 'ngTableParams', '$http', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/AssetCategoryApi/GetAllAssetCategories');
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
                    name: 'Name', field: 'Name',
                    sort: {
                        direction: uiGridConstants.ASC,
                        priority: 1
                    }
                },
                {
                    name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/assetCategories/edit/{{row.entity.AssetCategoryId}}">Edit</a> </div>',

                },


            ];




        }]);

