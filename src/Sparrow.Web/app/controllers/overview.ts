module sparrow.controllers {
    'use strict';

    interface IOverviewScope extends ng.IScope {
        latestOffers: any;
        offersToExpire: any;
    }

    angular.module('sparrow.controllers')
        .controller('OverviewCtrl', ['$scope', '$http', function ($scope: IOverviewScope, $http: ng.IHttpService) {
            $scope.latestOffers = [];
            $scope.offersToExpire = [];

            $http
                .get('/api/offers/latest')
                .success(function (data) {
                    $scope.latestOffers = data;
                });

            $http
                .get('/api/offers/soonToExpire')
                .success(function (data) {
                    $scope.offersToExpire = data;
                });
        }]);
}