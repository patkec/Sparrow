module sparrow.controllers {
    'use strict';

    interface IAdminScope extends ng.IScope {
        messages: any;
        events: any;
        $onRootScope: any;
    }

    angular.module('sparrow.controllers')
        .controller('AdminCtrl', ['$scope', function ($scope: IAdminScope) {
            $scope.events = [];

            $scope.$onRootScope('sparrow.admin.userCreated', function(user) {
                var data = $.extend({}, user, { type: 'user', action: 'New User' });
                $scope.events.push(data);
            });
            $scope.$onRootScope('sparrow.admin.customerCreated', function (customer) {
                var data = $.extend({}, customer, { type: 'customer', action: 'New Customer' });
                $scope.events.push(data);
            });
            $scope.$onRootScope('sparrow.admin.productCreated', function(product) {
                var data = $.extend({}, product, { type: 'product', action: 'New Product' });
                $scope.events.push(data);
            });

            $scope.$onRootScope('sparrow.offers.offerWon', function(offer) {
                var data = $.extend({}, offer, { type: 'offer', action: 'Offer Accepted' });
                $scope.events.push(data);
            });
            $scope.$onRootScope('sparrow.offers.offerLost', function (offer) {
                var data = $.extend({}, offer, { type: 'offer', action: 'Offer Lost' });
                $scope.events.push(data);
            });
            $scope.$onRootScope('sparrow.offers.offerSent', function (offer) {
                var data = $.extend({}, offer, { type: 'offer', action: 'Offer Sent' });
                $scope.events.push(data);
            });
        }]);
}