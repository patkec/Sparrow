module sparrow.controllers {
    'use strict';

    interface IListScope extends ng.IScope {
        totalItems: number;
        currentPage: number;
        pageSize: number;
        searchText: string;
        users: any;
        addUser();
        search();
        editUser(user: any);
        deleteUser(user: any);
    }
    interface IEditScope extends ng.IScope {
        alerts: any;
        user: any;
        confirm();
        cancel();
        closeAlert(index: number);
    };

    // Global controllers
    var userControllers = angular.module('sparrow.controllers');
    userControllers.controller('UsersCtrl', [
        '$scope',
        '$location',
        '$modal',
        'Users',
        '$routeParams',
        function ($scope: IListScope, $location: ng.ILocationService, $modal, Users: ng.resource.IResourceClass, $routeParams) {
            $scope.totalItems = 0;
            $scope.pageSize = 10;       
            $scope.searchText = $routeParams.search;
            $scope.currentPage = $routeParams.page || 1;

            $scope.addUser = function () {
                $location.url('/users/create');
            };
            $scope.deleteUser = function (user) {
                var modalInstance = $modal.open({
                    templateUrl: 'userDeleteDialog',
                    controller: ItemDeleteCtrl,
                    windowClass: 'show', // Workaround for bootstrap 3 - dialog is not shown without this class
                    resolve: {
                        item: function () {
                            return user;
                        }
                    }
                });
                modalInstance.result.then(function () {
                    Users.delete({ userId: user.id }, function () {
                        getUsers($scope.currentPage);
                    });
                });
            };
            $scope.search = function () {
                getUsers($scope.currentPage);
            };

            var getUsers = function (page) {
                if (page > 1) {
                    $location.search('page', page);
                }
                if ($scope.searchText) {
                    $location.search('search', $scope.searchText);
                }

                Users.get({ page: page, pageSize: $scope.pageSize, sort: 'FirstName', orderAscending: true, filter: $scope.searchText }, function (data) {
                    $scope.users = data.items;                                        
                    $scope.totalItems = data.totalItems;
                    $scope.currentPage = data.page;
                });
            };

            $scope.$watch('currentPage', getUsers);
        }]);

    userControllers.controller('UserDetailsCtrl', [
        '$scope',
        '$routeParams',
        'Users',
        function ($scope, $routeParams, Users) {
            $scope.user = Users.get({ userId: $routeParams.userId });
        }
    ]);

    userControllers.controller('UserCreateCtrl', [
        '$scope',
        '$location',
        'Users',
        function ($scope: IEditScope, $location, Users: sparrow.services.IUpdateResourceClass) {
            $scope.alerts = [];
            $scope.user = {};
            $scope.confirm = function () {
                Users.save($scope.user,
                    function () {
                        $location.path('/users');
                    },
                    function () {
                        $scope.alerts.push({});
                    });
            };
            $scope.cancel = function () {
                $location.path('/users');
            };
            $scope.closeAlert = function (index) {
                $scope.alerts.splice(index, 1);
            };
        }]);

    userControllers.controller('UserEditCtrl', [
        '$scope',
        '$routeParams',
        '$location',
        'Users',
        function ($scope: IEditScope, $routeParams, $location, Users: sparrow.services.IUpdateResourceClass) {
            $scope.alerts = [];
            $scope.user = Users.get({ userId: $routeParams.userId });
            $scope.confirm = function () {
                Users.update($scope.user,
                    function () {
                        $location.path('/users');
                    },
                    function () {
                        $scope.alerts.push({});
                    });
            };
            $scope.cancel = function () {
                $location.path('/users');
            };
            $scope.closeAlert = function (index) {
                $scope.alerts.splice(index, 1);
            };
        }]);
}