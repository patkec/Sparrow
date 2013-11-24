module sparrow {
    'use strict';

    var app = angular.module('sparrow', [
        'ngRoute',
        'ui.bootstrap',
        'sparrow.filters',
        'sparrow.services',
        'sparrow.directives',
        'sparrow.controllers'
    ]);
    app.config([
        '$routeProvider',
        '$locationProvider',
        function ($routeProvider: ng.route.IRouteProvider, $locationProvider: ng.ILocationProvider) {
            $routeProvider
                .when('/admin', { templateUrl: 'app/partials/admin/overview.html', controller: 'AdminCtrl' })
                // Offers
                .when('/offers', { templateUrl: '/app/partials/offers/overview.html', controller: 'OffersCtrl' })
                // Customers
                .when('/customers', { templateUrl: '/app/partials/customers/list.html', controller: 'CustomersCtrl' })
                .when('/customers/create', { templateUrl: '/app/partials/customers/create.html', controller: 'CustomerCreateCtrl' })
                .when('/customers/edit/:customerId', { templateUrl: '/app/partials/customers/edit.html', controller: 'CustomerEditCtrl' })
                // Users
                .when('/users', { templateUrl: '/app/partials/users/list.html', controller: 'UsersCtrl' })
                .when('/users/create', { templateUrl: '/app/partials/users/create.html', controller: 'UserCreateCtrl' })
                .when('/users/edit/:userId', { templateUrl: '/app/partials/users/edit.html', controller: 'UserEditCtrl' })
                // Products
                .when('/products', { templateUrl: '/app/partials/products/list.html', controller: 'ProductsCtrl' })
                .when('/products/create', { templateUrl: '/app/partials/products/create.html', controller: 'ProductCreateCtrl' })
                .when('/products/edit/:productId', { templateUrl: '/app/partials/products/edit.html', controller: 'ProductEditCtrl' })
                .otherwise({ redirectTo: '/offers' });
            // Enable pretty URLs (without #)
            $locationProvider.html5Mode(true);
        }]);
}