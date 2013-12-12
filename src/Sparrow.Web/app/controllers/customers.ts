module sparrow.controllers {
    'use strict';

    interface IListScope extends ng.IScope {
        totalItems: number;
        currentPage: number;
        pageSize: number;
        customers: any;
        searchText: string;
        search();
        addCustomer();
        editCustomer(customer: any);
        deleteCustomer(customer: any);
    }
    interface IEditScope extends ng.IScope {
        alerts: any;
        customer: any;
        confirm();
        cancel();
        closeAlert(index: number);
    };

    // Global controllers
    var customerControllers = angular.module('sparrow.controllers');
    customerControllers.controller('CustomersCtrl', [
        '$scope',
        '$location',
        '$modal',
        'Customers',
        '$routeParams',
        function ($scope: IListScope, $location: ng.ILocationService, $modal, Customers: ng.resource.IResourceClass, $routeParams) {
            $scope.totalItems = 0;
            $scope.pageSize = 20;
            $scope.searchText = $routeParams.search;
            $scope.currentPage = $routeParams.page || 1;

            $scope.addCustomer = function () {
                $location.url('/customers/create');
            };
            $scope.editCustomer = function (customer) {
                $location.url('/customers/edit/' + customer.id);
            };
            $scope.deleteCustomer = function (customer) {
                var modalInstance = $modal.open({
                    templateUrl: 'customerDeleteDialog',
                    controller: ItemDeleteCtrl,
                    windowClass: 'show', // Workaround for bootstrap 3 - dialog is not shown without this class
                    resolve: {
                        item: function () {
                            return customer;
                        }
                    }
                });
                modalInstance.result.then(function () {
                    Customers.delete({ productId: customer.id }, function () {
                        getCustomers($scope.currentPage);
                    });
                });
            };
            $scope.search = function () {
                getCustomers($scope.currentPage);
            };

            var getCustomers = function (page) {
                if (page > 1) {
                    $location.search('page', page);
                }
                if ($scope.searchText) {
                    $location.search('search', $scope.searchText);
                }

                Customers.get({ page: page, pageSize: $scope.pageSize, sort: 'Name', orderAscending: true, filter: $scope.searchText }, function (data) {
                    $scope.customers = data.items;
                    $scope.currentPage = data.page;
                    $scope.totalItems = data.totalItems;
                });
            };
            $scope.$watch('currentPage', getCustomers);
        }]);

    customerControllers.controller('CustomerCreateCtrl', [
        '$scope',
        '$location',
        'Customers',
        function ($scope: IEditScope, $location, Customers: sparrow.services.IUpdateResourceClass) {
            $scope.alerts = [];
            $scope.customer = {};
            $scope.confirm = function () {
                Customers.save($scope.customer,
                    function () {
                        $location.path('/customers');
                    },
                    function () {
                        $scope.alerts.push({});
                    });
            };
            $scope.cancel = function () {
                $location.path('/customers');
            };
            $scope.closeAlert = function (index) {
                $scope.alerts.splice(index, 1);
            };
        }]);

    customerControllers.controller('CustomerEditCtrl', [
        '$scope',
        '$routeParams',
        '$location',
        'Customers',
        function ($scope: IEditScope, $routeParams, $location, Customers: sparrow.services.IUpdateResourceClass) {
            $scope.alerts = [];
            $scope.customer = Customers.get({ customerId: $routeParams.customerId });
            $scope.confirm = function () {
                $scope.customer.$update().then(
                    function () {
                        $location.path('/customers');
                    },
                    function () {
                        $scope.alerts.push({});
                    });
            };
            $scope.cancel = function () {
                $location.path('/customers');
            };
            $scope.closeAlert = function (index) {
                $scope.alerts.splice(index, 1);
            };
        }]);
}