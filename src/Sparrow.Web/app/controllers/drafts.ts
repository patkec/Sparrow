/// <reference path="common.ts" />

module sparrow.controllers {
    'use strict';

    interface IEditScope extends ng.IScope {
        alerts: any;
        draft: any;
        getCustomers(name: string);
        getOwners(name: string);
        confirm();
        cancel();
        closeAlert(index: number);
    };

    var draftControllers = angular.module('sparrow.controllers');

    draftControllers.controller('DraftsCtrl', ['$scope', '$modal', '$location', 'Drafts', function ($scope, $modal, $location, Drafts) {
        $scope.totalItems = 0;
        $scope.currentPage = 0;
        $scope.drafts = [];

        $scope.showDetails = function (draft) {
            $location.url('/drafts/' + draft.id);
        };
        $scope.createOffer = function (draft) {
            Drafts.createOffer({ draftId: draft.id });
        };
        $scope.editDraft = function (draft) {
            $location.url('/drafts/edit/' + draft.id);
        };
        $scope.deleteDraft = function (draft) {
            var modalInstance = $modal.open({
                templateUrl: 'draftDeleteDialog',
                controller: ItemDeleteCtrl,
                windowClass: 'show', // Workaround for bootstrap 3 - dialog is not shown without this class
                resolve: {
                    item: function () {
                        return draft;
                    }
                }
            });
            modalInstance.result.then(function () {
                Drafts.delete({ draftId: draft.id }, function () {
                    getDrafts($scope.currentPage);
                });
            });
        };

        $scope.$watch('showDrafts', function () {
            if ($scope.currentPage == 0) {
                $scope.currentPage = 1;
            }
        });
        $scope.$watch('currentPage', function (page: number) {
            getDrafts(page);
        });

        function getDrafts(page: number) {
            Drafts.get({ page: page, pageSize: $scope.pageSize, sort: 'CreatedOn', orderAscending: false }, function (data) {
                $scope.drafts = data.items;
                $scope.currentPage = data.page;
                $scope.totalItems = data.totalItems;
            });
        }
    }]);

    draftControllers.controller('DraftDetailsCtrl', [
        '$scope',
        '$routeParams',
        '$http',
        'Drafts',
        function ($scope, $routeParams, $http, Drafts) {
            $scope.draft = Drafts.get({ draftId: $routeParams.draftId });

            $scope.addItem = function () {
                if (!!!$scope.draft.items)
                    $scope.draft.items = [];

                $scope.inserted = {
                    productTitle: '',
                    productPrice: null,
                    discount: null,
                    quantity: 1
                };
                $scope.draft.items.push($scope.inserted);
            };
            $scope.getProducts = function (title) {
                return $http.get('/api/products?sort=Title&orderAscending=true&filter=' + title).then(function (response) {
                    return response.data.items;
                });
            };
            $scope.updateProductTitle = function (item) {
                item.productTitle = item.product.title;
            };
            $scope.saveItem = function (data, item) {
                data = {
                    id: item.id,
                    productId: item.id ? null : data.product.id, // Product cannot be updated for existing items
                    quantity: data.quantity
                };
                var promise = $http.put('/api/drafts/' + $scope.draft.id + '/items', data);
                promise.success(function (response) {
                    console.log(response);
                    item.id = response.id;
                });
                return promise;
            };
            $scope.deleteItem = function (index, item) {
                $http.delete('/api/drafts/' + $scope.draft.id + '/items/' + item.id)
                    .success(function () {
                        $scope.draft.items.splice(index, 1);
                    });
            };
        }
    ]);

    draftControllers.controller('DraftCreateCtrl', [
        '$scope',
        '$location',
        '$http',
        'Drafts',
        function ($scope: IEditScope, $location, $http: ng.IHttpService, Drafts: sparrow.services.IUpdateResourceClass) {
            $scope.alerts = [];
            $scope.draft = {};
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
                Drafts.save(mapToDto($scope.draft),
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