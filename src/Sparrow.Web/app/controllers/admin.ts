module sparrow.controllers {
    'use strict';

    interface IAdminScope extends ng.IScope {
        messages: any;
    }

    angular.module('sparrow.controllers')
        .controller('AdminCtrl', ['$scope', '$', function ($scope: IAdminScope, $) {
            $scope.messages = [];

            var adminHub = $.connection.adminHub;
            adminHub.client.sendMessage = function (message) {
                $scope.$apply(function () {
                    $scope.messages.push(message);
                });                
            };
            $.connection.hub.start();

            $scope.messages.push('Started');
        }]);
}