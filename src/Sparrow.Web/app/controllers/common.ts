// File name is prefixed with _ because it should be loaded first (before other controllers)
module sparrow.controllers {
    'use strict';

    interface IMenuScope extends ng.IScope {
        activeViewPath: string;
    }

    export interface IItemDeleteScope extends ng.IScope {
        item: any;
        confirm();
        cancel();
    };

    export function ItemDeleteCtrl($scope: IItemDeleteScope, $modalInstance, item) {
        $scope.item = item;
        $scope.confirm = function () {
            $modalInstance.close($scope.item);
        };
        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    };    

    // Dependencies (within []) should be defined exactly once, otherwise a new module is created each time.
    angular.module('sparrow.controllers', ['sparrow.services', 'xeditable'])
        .controller('MenuCtrl', ['$scope', '$location', function ($scope: IMenuScope, $location: ng.ILocationService) {
            $scope.$on('$routeChangeSuccess', function () {
                $scope.activeViewPath = $location.path();
            });
        }]);
}