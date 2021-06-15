angular
    .module('homer')
    .controller('AssetEditController', ['$scope', '$http', '$timeout', '$modal', '$state', 'uiGridConstants', '$interval',
        function ($scope, $http, $timeout, $modal, $state, uiGridConstants, $interval) {

            $scope.tab = {};
            if ($scope.defaultTab == 'dashboard') {
                $scope.tab.dashboard = true;
            }
            $scope.isDisabled = false;
            var assetId = $scope.assetId;
            var action = $scope.action;

            $http.get('/webapi/AssetCategoryApi/GetAllAssetCategories').success(function (data, status) {
                $scope.assetCategories = data;
            });

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
                var promise = $http.get('/webapi/AssetApi/GetAsset?assetId=' + assetId, {});
                promise.then(
                    function (payload) {
                        var b = payload.data;
                        $scope.asset = {
                            AssetId : b.AssetId,
                            AssetCategoryId: b.AssetCategoryId,
                            Name: b.Name,
                            Amount: b.Amount,
                            PurchaseDate: b.PurchaseDate != null ? moment(b.PurchaseDate).format('YYYY-MM-DD HH:mm:ss') : null,

                            Notes: b.Notes,
                            BranchId: b.BranchId,
                            AssetCount : b.AssetCount,
                            TimeStamp: b.TimeStamp,
                            CreatedOn: b.CreatedOn,
                            CreatedBy: b.CreatedBy,
                            UpdatedBy: b.UpdatedBy,
                            Deleted: b.Deleted
                        };

                    });


            }

            $scope.Save = function (asset) {
                $scope.showMessageSave = false;
                $scope.isDisabled = true;
                if ($scope.form.$valid) {
                    $scope.loadingSpinner = true;
                    var promise = $http.post('/webapi/AssetApi/Save', {
                        AssetCategoryId: asset.AssetCategoryId,
                        AssetId: assetId,
                        Notes : asset.Notes,
                        Name: asset.Name,
                        CreatedBy: asset.CreatedBy,
                        CreatedOn: asset.CreatedOn,
                        Deleted: asset.Deleted,
                        PurchaseDate: asset.PurchaseDate,
                        AssetCount: asset.AssetCount,
                        Amount: asset.Amount,
                        BranchId : asset.BranchId,

                    });

                    promise.then(
                        function (payload) {

                            assetId = payload.data;
                            $scope.showMessageSave = true;
                            $scope.loadingSpinner = false;
                            $timeout(function () {
                                $scope.showMessageSave = false;
                                if (action == "create") {
                                    $state.go('asset-edit', { 'action': 'edit', 'assetId': assetId });
                                }

                            }, 1500);


                        });
                }

            }

            $scope.Cancel = function () {
                $state.go('assets.list');
            };

            $scope.Delete = function (assetId) {
                $scope.showMessageDeleted = false;
                var promise = $http.get('/webapi/AssetApi/Delete?assetId=' + assetId, {});
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
    .module('homer').controller('AssetController', ['$scope', 'ngTableParams', '$http', 'Utils', 'uiGridConstants',
        function ($scope, ngTableParams, $http, Utils, uiGridConstants) {
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/AssetApi/GetAllAssets');
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
                    name: 'AssetCount', field: 'AssetCount',
                
                },
                {
                    name:'Category',field:'AssetCategoryName',
                },
                {
                    name: 'Purchase Date', field: 'PurchaseDate',
                },

                {
                    name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/assetCategories/edit/{{row.entity.AssetCategoryId}}">Edit</a> </div>',

                },


            ];




        }]);


angular
    .module('homer').controller('BranchAssetController', ['$scope', '$http', 'uiGridConstants',
        function ($scope, $http, uiGridConstants) {
            $scope.loadingSpinner = true;
            var branchId = $scope.branchId;

            var promise = $http.get('/webapi/AssetApi/GetAllAssetsForAParticularBranch?branchId=' + branchId, {});
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
                    name: 'AssetCount', field: 'AssetCount',
                
                },
                {
                    name: 'Category', field: 'AssetCategoryName',
                },
                {
                    name:'Amount',field:'Amount',
                },
                {
                    name: 'Purchase Date', field: 'PurchaseDate',
                },

                {
                    name: 'Action', cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/assets/edit/{{row.entity.AssetId}}">Edit</a> </div>',

                },


            ];




        }]);
