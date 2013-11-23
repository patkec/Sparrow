module sparrow {
    'use strict';

    angular.module('sparrow', [
        'ngRoute',
        'ui.bootstrap',
        'sparrow.filters',
        'sparrow.services',
        'sparrow.directives',
        'sparrow.controllers'
    ])
    .config(['$routeProvider', '$locationProvider', function ($routeProvider: ng.route.IRouteProvider, $locationProvider: ng.ILocationProvider) {
        $routeProvider
            .when('/offers', { templateUrl: '/app/partials/offers/overview.html', controller: 'OffersCtrl' })
            .when('/customers', { templateUrl: 'app/partials/customers/list.html', controller: 'CustomersCtrl' })
            .when('/users', { templateUrl: 'app/partials/users/list.html', controller: 'UsersCtrl' })
            .when('/products', { templateUrl: 'app/partials/products/list.html', controller: 'ProductsCtrl' })
            .otherwise({ redirectTo: '/offers' });
        // Enable pretty URLs (without #)
        $locationProvider.html5Mode(true);
    }]);
}