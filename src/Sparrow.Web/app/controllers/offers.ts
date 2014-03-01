/// <reference path="common.ts" />
module sparrow.controllers {
    'use strict';

    interface IListScope extends ng.IScope {
        totalItems: number;
        currentPage: number;
        pageSize: number;
        offers: any;
        searchText: string;
        addOffer();
        search();
        editOffer(offer: any);
        deleteOffer(offer: any);
    }

    var offerControllers = angular.module('sparrow.controllers');

    offerControllers.controller('OffersCtrl', [
        '$scope',
        '$location',
        '$routeParams',
        'Offers',
        function ($scope, $location: ng.ILocationService, $routeParams, Offers) {
            $scope.pageSize = 20;
            $scope.totalItems = 0;
            $scope.searchText = $routeParams.search;
            $scope.currentPage = $routeParams.page || 1;

            $scope.$watch('currentPage', function (page) {
                getOffers(page);
            });
            $scope.search = function () {
                getOffers($scope.currentPage);
            };

            function getOffers(page) {
                if (page > 1) {
                    $location.search('page', page);
                }
                if ($scope.searchText) {
                    $location.search('search', $scope.searchText);
                }
                Offers.get({ page: page, pageSize: $scope.pageSize, sort: 'Title', orderAscending: true, filter: $scope.searchText }, function (data) {
                    $scope.offers = data.items;
                    $scope.currentPage = data.page;
                    $scope.totalItems = data.totalItems;
                });
            };
        }]);

    offerControllers.controller('OffersArchiveCtrl', [
        '$scope',
        '$location',
        '$routeParams',
        'Offers',
        function ($scope, $location: ng.ILocationService, $routeParams, Offers) {
            $scope.pageSize = 20;
            $scope.totalItems = 0;
            $scope.searchText = $routeParams.search;
            $scope.currentPage = $routeParams.page || 1;

            $scope.$watch('currentPage', function (page) {
                getOffers(page);
            });
            $scope.search = function () {
                getOffers($scope.currentPage);
            };

            function getOffers(page) {
                if (page > 1) {
                    $location.search('page', page);
                }
                if ($scope.searchText) {
                    $location.search('search', $scope.searchText);
                }
                Offers.getArchived({ page: page, pageSize: $scope.pageSize, sort: 'Title', orderAscending: true, filter: $scope.searchText }, function (data) {
                    $scope.offers = data.items;
                    $scope.currentPage = data.page;
                    $scope.totalItems = data.totalItems;
                });
            };
        }]);

    offerControllers.controller('OfferDetailsCtrl', [
        '$scope',
        '$routeParams',
        '$http',
        '$location',
        'Offers',
        function ($scope, $routeParams, $http, $location, Offers) {
            $scope.offer = Offers.get({ offerId: $routeParams.offerId });

            $scope.archiveOffer = function () {
                $http.put('https://localhost:44304/api/offers/' + $scope.offer.id + '/archive').then(function () {
                    $scope.offer.status = 2;
                });
            };
            $scope.cloneOffer = function () {
                $http.post('https://localhost:44304/api/drafts/create/' + $scope.offer.id).then(function (response) {
                    $location.path('/drafts/' + response.data.replace(/"/g, ''));
                });
            };
        }
    ]);

}