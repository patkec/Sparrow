// File name is prefixed with _ because it should be loaded first (before other controllers)
module sparrow.controllers {
    'use strict';

    interface IMenuScope extends ng.IScope {
        activeViewPath: string;
    }

    // Dependencies (within []) should be defined exactly once, otherwise a new module is created each time.
    angular.module('sparrow.controllers', ['sparrow.services'])
        .controller('MenuCtrl', ['$scope', '$location', function ($scope: IMenuScope, $location: ng.ILocationService) {
            $scope.$on('$routeChangeSuccess', function () {
                $scope.activeViewPath = $location.path();
            });
        }]);
}