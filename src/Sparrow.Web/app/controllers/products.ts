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
        deleteProduct(product: any);
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
            $scope.deleteProduct = function (product) {
                var modalInstance = $modal.open({
                    templateUrl: 'productDeleteDialog',
                    controller: ItemDeleteCtrl,
                    windowClass: 'show', // Workaround for bootstrap 3 - dialog is not shown without this class
                    resolve: {
                        item: function () {
                            return product;
                        }
                    }
                });
                modalInstance.result.then(function () {
                    Products.delete({ productId: product.id }, function () {
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
                Products.update($scope.product,
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