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
            $.connection.hub.start();
        }]);
}