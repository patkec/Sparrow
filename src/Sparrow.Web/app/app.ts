module sparrow {
    'use strict';

    interface IIndexScope extends ng.IScope {
        hello: string;
    }

    angular.module('sparrow', ['ngRoute'])
        .config(['$routeProvider', function ($routeProvider: ng.route.IRouteProvider) {
            $routeProvider
                .when('/', { templateUrl: '/app/partials/index.html', controller: 'IndexController' })
                .otherwise({ redirectTo: '/' });
        }])
        .controller('IndexController', ['$scope', function ($scope: IIndexScope) {
            $scope.hello = 'Hello, world!';
        }]);
}