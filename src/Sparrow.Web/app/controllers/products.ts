module sparrow.controllers {
    'use strict';

    interface IListScope extends ng.IScope {
        totalItems: number;
        currentPage: number;
        pageSize: number;
        searchText: string;
        products: any;
        search();
        addProduct();
        editProduct(product: any);
        deleteProducts();
        selectProduct(product: any, $event: any);
    }
    interface IEditScope extends ng.IScope {
        alerts: any;
        product: any;
        confirm();
        cancel();
        closeAlert(index: number);
    };

    // Global controllers
    var productControllers = angular.module('sparrow.controllers');
    productControllers.controller('ProductsCtrl', [
        '$scope',
        '$location',
        '$modal',
        'Products',
        '$routeParams',
        function ($scope: IListScope, $location: ng.ILocationService, $modal, Products: ng.resource.IResourceClass, $routeParams) {
            var lastSelectedIdx;
            $scope.totalItems = 0;
            $scope.pageSize = 20;
            $scope.searchText = $routeParams.search;
            $scope.currentPage = $routeParams.page || 1;

            $scope.addProduct = function () {
                $location.url('/products/create');
            };
            $scope.editProduct = function (product) {
                $location.url('/products/edit/' + product.id);
            };
            $scope.selectProduct = function (product, $event) {
                if ($event.shiftKey) {
                    var idx = lastSelectedIdx;
                    lastSelectedIdx = $scope.products.indexOf(product);

                    var productsToSelect = $scope.products.slice(Math.min(idx, lastSelectedIdx), Math.max(idx, lastSelectedIdx) + 1);

                    var selected = !!!product.selected;
                    $.each(productsToSelect, function (i, p) {
                        p.selected = selected;
                    });
                    return;
                }
                lastSelectedIdx = $scope.products.indexOf(product);
            };
            $scope.deleteProducts = function () {
                var modalInstance = $modal.open({
                    templateUrl: 'productDeleteDialog',
                    controller: ItemDeleteCtrl,
                    windowClass: 'show', // Workaround for bootstrap 3 - dialog is not shown without this class
                    resolve: {
                        item: function () {
                            return null;
                        }
                    }
                });
                modalInstance.result.then(function () {
                    var products = [];
                    $.each($scope.products, function (i, product) {
                        if (product.selected) products.push(product.id);
                    });

                    Products.delete({ ids: products }, function () {
                        getProducts($scope.currentPage);
                    });
                });
            };
            $scope.search = function () {
                getProducts($scope.currentPage);
            };

            var getProducts = function (page) {
                Products.get({ page: page, pageSize: $scope.pageSize, sort: 'Title', orderAscending: true, filter: $scope.searchText }, function (data) {
                    $scope.products = data.items;
                    $scope.currentPage = data.page;
                    $scope.totalItems = data.totalItems;
                });
            };
            $scope.$watch('currentPage', getProducts);
        }]);

    productControllers.controller('ProductCreateCtrl', [
        '$scope',
        '$location',
        'Products',
        function ($scope: IEditScope, $location, Products: sparrow.services.IUpdateResourceClass) {
            $scope.alerts = [];
            $scope.product = {};
            $scope.confirm = function () {
                Products.save($scope.product,
                    function () {
                        $location.path('/products');
                    },
                    function () {
                        $scope.alerts.push({});
                    });
            };
            $scope.cancel = function () {
                $location.path('/products');
            };
            $scope.closeAlert = function (index) {
                $scope.alerts.splice(index, 1);
            };
        }]);

    productControllers.controller('ProductEditCtrl', [
        '$scope',
        '$routeParams',
        '$location',
        'Products',
        function ($scope: IEditScope, $routeParams, $location, Products: sparrow.services.IUpdateResourceClass) {
            $scope.alerts = [];
            $scope.product = Products.get({ productId: $routeParams.productId });
            $scope.confirm = function () {
                $scope.product.$update().then(
                    function () {
                        $location.path('/products');
                    },
                    function () {
                        $scope.alerts.push({});
                    });
            };
            $scope.cancel = function () {
                $location.path('/products');
            };
            $scope.closeAlert = function (index) {
                $scope.alerts.splice(index, 1);
            };
        }]);
}