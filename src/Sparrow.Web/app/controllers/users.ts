module sparrow.controllers {
    'use strict';

    angular.module('sparrow.controllers')
        .controller('UsersCtrl', ['$scope', 'Users', function ($scope, Users: ng.resource.IResourceClass) {            
            $scope.totalItems = 0;
            $scope.currentPage = 0;
            $scope.pageSize = 20;

            $scope.$watch('currentPage', function (page) {
                Users.get({ page: page, pageSize: $scope.pageSize }, function (data) {
                    $scope.users = data.users;
                    $scope.currentPage = data.page;
                    $scope.totalItems = data.totalItems;
                });
            });

            // Fetch data for the first page.
            $scope.currentPage = 1;
        }]);
}