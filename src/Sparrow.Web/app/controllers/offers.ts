module sparrow.controllers {
    'use strict';

    interface IOverviewScope extends ng.IScope {
        latestOffers: any;
        offersToExpire: any;
    }
    interface IListScope extends ng.IScope {
        totalItems: number;
        currentPage: number;
        pageSize: number;
        offers: any;
        addOffer();
        editOffer(offer: any);
        deleteOffer(offer: any);
    }
    interface IEditScope extends ng.IScope {
        alerts: any;
        offer: any;
        getCustomers(name: string);
        getOwners(name: string);
        confirm();
        cancel();
        closeAlert(index: number);
    };
    interface IDeleteScope extends ng.IScope {
        offer: any;
        confirm();
        cancel();
    };

    // A private controller for deleting users
    var OfferDeleteCtrl = function ($scope: IDeleteScope, $modalInstance, offer) {
        $scope.offer = offer;
        $scope.confirm = function () {
            $modalInstance.close($scope.offer);
        };
        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    };    

    var offerControllers = angular.module('sparrow.controllers');
    offerControllers.controller('OffersOverviewCtrl', ['$scope', '$http', function ($scope: IOverviewScope, $http: ng.IHttpService) {
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

    offerControllers.controller('OffersCtrl', [
        '$scope',
        '$location',
        '$modal',
        'Offers',
        'storageService',
        function ($scope: IListScope, $location: ng.ILocationService, $modal, Offers: ng.resource.IResourceClass, storageService: sparrow.services.IStorageService) {
            $scope.totalItems = 0;
            $scope.currentPage = 0;
            $scope.pageSize = 20;

            $scope.addOffer = function () {
                storageService.store('offers\page', $scope.currentPage);
                $location.url('/offers/create');
            };
            $scope.editOffer = function (offer) {
                storageService.store('offers\page', $scope.currentPage);
                $location.url('/offers/edit/' + offer.id);
            };
            $scope.deleteOffer = function (offer) {
                var modalInstance = $modal.open({
                    templateUrl: 'offerDeleteDialog',
                    controller: OfferDeleteCtrl,
                    windowClass: 'show', // Workaround for bootstrap 3 - dialog is not shown without this class
                    resolve: {
                        offer: function () {
                            return offer;
                        }
                    }
                });
                modalInstance.result.then(function () {
                    Offers.delete({ offerId: offer.id }, function () {
                        getOffers($scope.currentPage);
                    });
                });
            };

            var getOffers = function (page) {
                Offers.get({ page: page, pageSize: $scope.pageSize, sort: 'Title', orderAscending: true }, function (data) {
                    $scope.offers = data.items;
                    $scope.currentPage = data.page;
                    $scope.totalItems = data.totalItems;
                });
            };
            $scope.$watch('currentPage', getOffers);

            // Fetch data for the first page.
            $scope.currentPage = storageService.get('offers\page') || 1;
        }]);

    offerControllers.controller('OfferCreateCtrl', [
        '$scope',
        '$location',
        '$http',
        'Offers',
        function ($scope: IEditScope, $location, $http: ng.IHttpService, Offers: sparrow.services.IUpdateResourceClass) {
            $scope.alerts = [];
            $scope.offer = {};
            $scope.getCustomers = function (name) {
                return $http.get('/api/customers?sort=Name&orderAscending=true&filter=' + name).then(function (response) {
                    return response.data.items;
                });
            };
            $scope.getOwners = function (name) {
                return $http.get('/api/users?sort=Name&orderAscending=true&filter=' + name).then(function (response) {
                    return response.data.items;
                });
            };
            $scope.confirm = function () {
                Offers.save(mapToDto($scope.offer),
                    function () {
                        $location.path('/offers');
                    },
                    function () {
                        $scope.alerts.push({});
                    });
            };
            $scope.cancel = function () {
                $location.path('/offers');
            };
            $scope.closeAlert = function (index) {
                $scope.alerts.splice(index, 1);
            };
        }]);

    offerControllers.controller('OfferEditCtrl', [
        '$scope',
        '$routeParams',
        '$location',
        '$http',
        'Offers',
        function ($scope: IEditScope, $routeParams, $location, $http: ng.IHttpService, Offers: sparrow.services.IUpdateResourceClass) {
            $scope.alerts = [];
            $scope.offer = Offers.get({ offerId: $routeParams.offerId });
            $scope.getCustomers = function (name) {
                return $http.get('/api/customers?sort=Name&orderAscending=true&filter=' + name).then(function (response) {
                    return response.data.items;
                });
            };
            $scope.confirm = function () {
                Offers.update(mapToDto($scope.offer),
                    function () {
                        $location.path('/offers');
                    },
                    function () {
                        $scope.alerts.push({});
                    });
            };
            $scope.cancel = function () {
                $location.path('/offers');
            };
            $scope.closeAlert = function (index) {
                $scope.alerts.splice(index, 1);
            };
        }]);

    function mapToDto(entity): any {
        var dto: any = {};
        angular.extend(dto, entity);
        dto.ownerId = entity.owner.id;
        dto.customerId = entity.customer.id;
        delete dto.owner;
        delete dto.customer;
        return dto;
    }
}