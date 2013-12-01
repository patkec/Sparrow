module sparrow.controllers {
    'use strict';

    interface IAdminScope extends ng.IScope {
        messages: any;
    }

    angular.module('sparrow.controllers')
        .controller('AdminCtrl', ['$scope', '$', function ($scope: IAdminScope, $) {
            $scope.messages = [];

            var adminHub = $.connection.adminHub;
            adminHub.client.userCreated = function (user) {
                $scope.$apply(function () {
                    $scope.messages.push('User ' + user.Name + ' has been created.');
                });
            };
            adminHub.client.customerCreated = function (customer) {
                $scope.$apply(function () {
                    $scope.messages.push('Customer ' + customer.Name + ' has been created.');
                });
            };
            adminHub.client.productCreated = function (product) {
                $scope.$apply(function () {
                    $scope.messages.push('Product ' + product.Title + ' has been added to the system.');
                });
            };

            var offersHub = $.connection.offersHub;
            offersHub.client.offerWon = function (offer) {
                $scope.$apply(function () {
                    $scope.messages.push('Offer ' + offer.Title + ' has been WON and an order has been created.');
                });
            };
            offersHub.client.offerLost = function (offer) {
                $scope.$apply(function () {
                    $scope.messages.push('Offer ' + offer.Title + ' has been LOST. Better luck next time.');
                });
            };
            offersHub.client.offerSent = function (offer) {
                $scope.$apply(function () {
                    $scope.messages.push('Offer ' + offer.Title + ' has been sent to customer ' + offer.Customer.Name +
                        '. Wait until ' + offer.ExpiresOn + ' to see the outcome.');
                });
            };

            $.connection.hub.start();
        }]);
}