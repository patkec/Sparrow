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
        '$httpProvider',
        function ($routeProvider: ng.route.IRouteProvider, $locationProvider: ng.ILocationProvider, $httpProvider) {
            $routeProvider
                .when('/admin', { templateUrl: 'app/partials/admin/overview.html', controller: 'AdminCtrl' })
                .when('/overview', { templateUrl: 'app/partials/offers/overview.html', controller: 'OverviewCtrl' })
                .when('/login', { template: ' ', controller: 'LoginCtrl' })
                .when('/loginCallback', { template: ' ', controller: 'LoginCallbackCtrl' })
                // Offers - Drafts
                .when('/drafts', { templateUrl: '/app/partials/drafts/list.html', controller: 'DraftsCtrl' })
                .when('/drafts/create', { templateUrl: '/app/partials/drafts/create.html', controller: 'DraftCreateCtrl' })
                .when('/drafts/:draftId', { templateUrl: '/app/partials/drafts/details.html', controller: 'DraftDetailsCtrl' })
                // Offers
                .when('/offers', { templateUrl: '/app/partials/offers/list.html', controller: 'OffersCtrl' })
                .when('/offers/archive', { templateUrl: '/app/partials/offers/archive.html', controller: 'OffersArchiveCtrl' })
                .when('/offers/:offerId', { templateUrl: '/app/partials/offers/details.html', controller: 'OfferDetailsCtrl' })
                // Customers
                .when('/customers', { templateUrl: '/app/partials/customers/list.html', controller: 'CustomersCtrl' })
                .when('/customers/create', { templateUrl: '/app/partials/customers/create.html', controller: 'CustomerCreateCtrl' })
                .when('/customers/edit/:customerId', { templateUrl: '/app/partials/customers/edit.html', controller: 'CustomerEditCtrl' })
                // Users
                .when('/users', { templateUrl: '/app/partials/users/list.html', controller: 'UsersCtrl' })                
                .when('/users/create', { templateUrl: '/app/partials/users/create.html', controller: 'UserCreateCtrl' })
                .when('/users/edit/:userId', { templateUrl: '/app/partials/users/edit.html', controller: 'UserEditCtrl' })
                .when('/users/:userId', { templateUrl: '/app/partials/users/details.html', controller: 'UserDetailsCtrl' })
                // Products
                .when('/products', { templateUrl: '/app/partials/products/list.html', controller: 'ProductsCtrl' })
                .when('/products/create', { templateUrl: '/app/partials/products/create.html', controller: 'ProductCreateCtrl' })
                .when('/products/edit/:productId', { templateUrl: '/app/partials/products/edit.html', controller: 'ProductEditCtrl' })
                .otherwise({ redirectTo: '/overview' });
            // Enable pretty URLs (without #)
            $locationProvider.html5Mode(true);

            $httpProvider.interceptors.push('authInterceptor');
        }]);
    app.run(function (editableOptions) {
        editableOptions.theme = 'bs3';
    });
}