module sparrow.controllers {
    'use strict';

    interface IListScope extends ng.IScope {
        totalItems: number;
        currentPage: number;
        pageSize: number;
        customers: any;
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
    interface IDeleteScope extends ng.IScope {
        customer: any;
        confirm();
        cancel();
    };

    // A private controller for deleting users
    var CustomerDeleteCtrl = function ($scope: IDeleteScope, $modalInstance, customer) {
        $scope.customer = customer;
        $scope.confirm = function () {
            $modalInstance.close($scope.customer);
        };
        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    };

    // Global controllers
    var customerControllers = angular.module('sparrow.controllers');
    customerControllers.controller('CustomersCtrl', [
        '$scope',
        '$location',
        '$modal',
        'Customers',
        'storageService',
        function ($scope: IListScope, $location: ng.ILocationService, $modal, Customers: ng.resource.IResourceClass, storageService: sparrow.services.IStorageService) {
            $scope.totalItems = 0;
            $scope.currentPage = 0;
            $scope.pageSize = 20;

            $scope.addCustomer = function () {
                storageService.store('customers\page', $scope.currentPage);
                $location.url('/customers/create');
            };
            $scope.editCustomer = function (customer) {
                storageService.store('customers\page', $scope.currentPage);
                $location.url('/customers/edit/' + customer.id);
            };
            $scope.deleteCustomer = function (customer) {
                var modalInstance = $modal.open({
                    templateUrl: 'customerDeleteDialog',
                    controller: CustomerDeleteCtrl,
                    windowClass: 'show', // Workaround for bootstrap 3 - dialog is not shown without this class
                    resolve: {
                        customer: function () {
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

            var getCustomers = function (page) {
                Customers.get({ page: page, pageSize: $scope.pageSize, sort: 'Name', orderAscending: true }, function (data) {
                    $scope.customers = data.items;
                    $scope.currentPage = data.page;
                    $scope.totalItems = data.totalItems;
                });
            };
            $scope.$watch('currentPage', getCustomers);

            // Fetch data for the first page.
            $scope.currentPage = storageService.get('customers\page') || 1;
        }]);

    customerControllers.controller('CustomerCreateCtrl', [
        '$scope',
        '$location',
        'Customers',
        function ($scope: IEditScope, $location, Customers: sparrow.services.IUpdateResourceClass) {
            $scope.alerts = [];
            $scope.customer = {};
            $scope.confirm = function () {
                Customers.update($scope.customer,
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
            $scope.customer = Customers.get({ productId: $routeParams.productId });
            $scope.confirm = function () {
                Customers.update($scope.customer,
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