module sparrow.controllers {
    'use strict';

    interface IIndexScope extends ng.IScope {
        hello: string;
    }

    angular.module('sparrow.controllers')
        .controller('OffersCtrl', ['$scope', function ($scope: IIndexScope) {
            $scope.hello = 'Hello, world!';
        }]);
}