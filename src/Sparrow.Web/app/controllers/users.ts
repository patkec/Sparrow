module sparrow.controllers {
    'use strict';

    angular.module('sparrow.controllers')
        .controller('UsersCtrl', ['$scope', 'Users', function ($scope, Users: ng.resource.IResourceClass) {
            $scope.currentPage = -1;
            $scope.totalPages = 0;

            $scope.range = function (max) {
                var ret = [];
                for (var i = 0; i < max; i++)
                    ret.push(i);
                return ret;
            };

            $scope.$watch(function () {
                return $scope.currentPage;
            }, function () {
                Users.get({ page: $scope.currentPage, pageSize: 25 }, function (data) {
                    $scope.users = data.users;
                    $scope.currentPage = data.page;
                    $scope.totalPages = data.totalPages;
                });
            });

            $scope.prevPage = function () {
                if ($scope.currentPage > 0) {
                    $scope.currentPage--;
                }
            };
            $scope.nextPage = function () {
                if ($scope.currentPage < $scope.totalPages - 1) {
                    $scope.currentPage++;
                }
            };
            $scope.setPage = function () {
                $scope.currentPage = this.page;
            };

            // Fetch data for the first page.
            $scope.currentPage = 0;
        }]);
}