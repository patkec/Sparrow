/// <reference path="common.ts" />
module sparrow.controllers {
    'use strict';

    interface IListScope extends ng.IScope {
        totalItems: number;
        currentPage: number;
        pageSize: number;
        offers: any;
        addOffer();
        editOffer(offer: any);
        deleteOffer(offer: any);
    }

    var offerControllers = angular.module('sparrow.controllers');

    offerControllers.controller('ActiveOffersCtrl', ['$scope', 'Offers', function ($scope, Offers) {
        $scope.totalItems = 0;
        $scope.currentPage = -1;
        $scope.offers = [];

        $scope.$watch('showActive', function () {
            if ($scope.currentPage < 1) {
                $scope.currentPage++;
            }
        });
        $scope.$watch('currentPage', function (page) {
            console.log('get active: ' + page);
            if (page > 0) {
                Offers.get({ page: page, pageSize: $scope.pageSize, sort: 'Title', orderAscending: true }, function (data) {
                    console.log(data);
                    $scope.offers = data.items;
                    $scope.currentPage = data.page;
                    $scope.totalItems = data.totalItems;
                });
            }
        });
    }]);

    offerControllers.controller('CompletedOffersCtrl', ['$scope', 'Offers', function ($scope, Offers) {
        $scope.totalItems = 0;
        $scope.currentPage = 0;
        $scope.offers = [];

        $scope.$watch('showCompleted', function () {
            if ($scope.currentPage == 0) {
                $scope.currentPage = 1;
            }
        });
        $scope.$watch('currentPage', function (page) {
            Offers.get({ page: page, pageSize: $scope.pageSize, sort: 'Title', orderAscending: true }, function (data) {
                $scope.offers = data.items;
                $scope.currentPage = data.page;
                $scope.totalItems = data.totalItems;
            });
        });
    }]);

    offerControllers.controller('OffersCtrl', [
        '$scope',
        '$location',
        'storageService',
        function ($scope, $location: ng.ILocationService, storageService: sparrow.services.IStorageService) {
            $scope.pageSize = 20;

            $scope.addDraft = function () {
                $location.url('/offers/create');
            };
            $scope.$watch('showDrafts', function (value) {
                storageService.store('offers\tab\drafts', value);
            });
            $scope.$watch('showActive', function (value) {
                storageService.store('offers\tab\active', value);
            });
            $scope.$watch('showCompleted', function (value) {
                storageService.store('offers\tab\completed', value);
            });

            $scope.showDrafts = storageService.get('offers\tab\drafts') || true;
            $scope.showActive = storageService.get('offers\tab\active');
            $scope.showCompleted = storageService.get('offers\tab\completed');
        }]);
}