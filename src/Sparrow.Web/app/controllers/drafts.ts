/// <reference path="../viewModels/drafts.ts" />
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

    function SendOfferCtrl($scope, $modalInstance) {
        $scope.dt = new Date();
        $scope.confirm = function () {
            $modalInstance.close($scope.dt);
        };
        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    };    

    draftControllers.controller('DraftsCtrl', [
        '$scope',
        '$modal',
        '$location',
        '$routeParams',
        'Drafts', function ($scope, $modal, $location, $routeParams, Drafts) {
        $scope.totalItems = 0;
        $scope.drafts = [];
        $scope.searchText = $routeParams.search;
        $scope.currentPage = $routeParams.page || 1;

        $scope.showDetails = function (draft) {
            $location.url('/drafts/' + draft.id);
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
        $scope.search = function () {
            getDrafts($scope.currentPage);
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
            if (page > 1) {
                $location.search('page', page);
            }
            if ($scope.searchText) {
                $location.search('search', $scope.searchText);
            }
            Drafts.get({ page: page, pageSize: $scope.pageSize, sort: 'CreatedOn', orderAscending: false, filter: $scope.searchText }, function (data) {
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
        '$location',
        '$modal',
        'Drafts',
        function ($scope, $routeParams, $http, $location, $modal, Drafts) {
            Drafts.get({ draftId: $routeParams.draftId }, function (data) {
                // Use the view model for ease of use.
                $scope.draft = new sparrow.viewModels.DraftViewModel(data);
                $scope.draft.addNewItem();
            });

            $scope.getProducts = function (title) {
                return $http.get('/api/products?sort=Title&orderAscending=true&filter=' + title).then(function (response) {
                    return response.data.items;
                });
            };
            $scope.getCustomers = function (name) {
                return $http.get('/api/customers?sort=Name&orderAscending=true&filter=' + name).then(function (response) {
                    return response.data.items;
                });
            };
            $scope.saveItem = function ($data, item) {
                var data = $.extend({}, $data, { id: item.id, productId: $data.product.id });
                return $http.put('/api/drafts/' + $scope.draft.id + '/items', data)
                    .then(function (response) {
                        item.endEdit(response.data.item);
                        $.extend($scope.draft, response.data.draft);
                        if (!!!item.id) {
                            item.id = response.data.item.id;
                            $scope.draft.addNewItem();
                        }
                    });
            };
            $scope.deleteItem = function (index, item) {
                $http.delete('/api/drafts/' + $scope.draft.id + '/items/' + item.id)
                    .success(function (response) {
                        $scope.draft.items.splice(index, 1);
                        $.extend($scope.draft, response);
                    });
            };
            $scope.saveDraftHeader = function ($data) {
                var data = $.extend({}, $data, { id: $scope.draft.id, discount: $scope.draft.discount, customerId: $data.customer.id });
                return $http.put('/api/drafts', data);
            };
            $scope.saveDraftDiscount = function ($data) {
                var data = $.extend({}, $data, { id: $scope.draft.id, title: $scope.draft.title, customerId: $scope.draft.customer.id })                
                return $http.put('/api/drafts', data)
                    .then(function (response) {
                        $scope.draft.endEdit();
                        $.extend($scope.draft, response.data);
                    });
            };
            $scope.sendOffer = function () {
                var modalInstance = $modal.open({
                    templateUrl: 'sendOfferDialog',
                    controller: SendOfferCtrl,
                    windowClass: 'show' // Workaround for bootstrap 3 - dialog is not shown without this class
                });
                modalInstance.result.then(function (date) {
                    var url = '/api/drafts/' + $scope.draft.id + '/offer';
                    $http.post(url, { expiresOn: date })
                        .success(function () {
                            $location.path('/offers');
                        });
                });
            };
            $scope.discardDraft = function () {
                var modalInstance = $modal.open({
                    templateUrl: 'draftDeleteDialog',
                    controller: ItemDeleteCtrl,
                    windowClass: 'show', // Workaround for bootstrap 3 - dialog is not shown without this class
                    resolve: {
                        item: function () {
                            return $scope.draft;
                        }
                    }
                });
                modalInstance.result.then(function () {
                    Drafts.delete({ draftId: $scope.draft.id }, function () {
                        $location.path('/drafts');
                    });
                });
            };
        }
    ]);

    draftControllers.controller('DraftCreateCtrl', [
        '$scope',
        '$location',
        '$http',
        'Drafts',
        function ($scope: IEditScope, $location, $http: ng.IHttpService, Drafts: any) {
            $scope.alerts = [];
            $scope.draft = {};
            $scope.getCustomers = function (name) {
                return $http.get('/api/customers?sort=Name&orderAscending=true&filter=' + name).then(function (response) {
                    return response.data.items;
                });
            };
            $scope.getOwners = function (name) {
                return $http.get('/api/users?sort=FirstName&orderAscending=true&filter=' + name).then(function (response) {
                    return response.data.items;
                });
            };
            $scope.confirm = function () {
                Drafts.save(mapToDto($scope.draft),
                    function (data) {
                        $location.path('/drafts/' + data.id);
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