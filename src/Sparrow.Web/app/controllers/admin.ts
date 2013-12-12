module sparrow.controllers {
    'use strict';

    interface IAdminScope extends ng.IScope {
        messages: any;
        events: any;
    }

    angular.module('sparrow.controllers')
        .controller('AdminCtrl', ['$scope', '$', function ($scope: IAdminScope, $) {
            $scope.events = [];

            var adminHub = $.connection.adminHub;
            adminHub.client.userCreated = function (user) {
                $scope.$apply(function () {
                    var data = $.extend({}, user, { type: 'user', action: 'New User' });
                    $scope.events.push(data);
                });
            };
            adminHub.client.customerCreated = function (customer) {
                $scope.$apply(function () {
                    var data = $.extend({}, customer, { type: 'customer', action: 'New Customer' });
                    $scope.events.push(data);
                });
            };
            adminHub.client.productCreated = function (product) {
                $scope.$apply(function () {
                    var data = $.extend({}, product, { type: 'product', action: 'New Product' });
                    $scope.events.push(data);
                });
            };

            var offersHub = $.connection.offersHub;
            offersHub.client.offerWon = function (offer) {
                $scope.$apply(function () {
                    var data = $.extend({}, offer, { type: 'offer', action: 'Offer Accepted' });
                    $scope.events.push(data);
                });
            };
            offersHub.client.offerLost = function (offer) {
                $scope.$apply(function () {
                    var data = $.extend({}, offer, { type: 'offer', action: 'Offer Lost' });
                    $scope.events.push(data);
                });
            };
            offersHub.client.offerSent = function (offer) {
                $scope.$apply(function () {
                    var data = $.extend({}, offer, { type: 'offer', action: 'Offer Sent' });
                    $scope.events.push(data);
                });
            };

            $.connection.hub.start();
        }]);
}