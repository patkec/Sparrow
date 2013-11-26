module sparrow.controllers {
    'use strict';

    interface IListScope extends ng.IScope {
        totalItems: number;
        currentPage: number;
        pageSize: number;
        users: any;
        addUser();
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
    interface IDeleteScope extends ng.IScope {
        user: any;
        confirm();
        cancel();
    };

    // A private controller for deleting users
    var UserDeleteCtrl = function ($scope: IDeleteScope, $modalInstance, user) {
        $scope.user = user;
        $scope.confirm = function () {
            $modalInstance.close($scope.user);
        };
        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    };

    // Global controllers
    var userControllers = angular.module('sparrow.controllers');
    userControllers.controller('UsersCtrl', [
        '$scope',
        '$location',
        '$modal',
        'Users',
        'storageService',
        function ($scope: IListScope, $location: ng.ILocationService, $modal, Users: ng.resource.IResourceClass, storageService: sparrow.services.IStorageService) {
            $scope.totalItems = 0;
            $scope.currentPage = 0;
            $scope.pageSize = 20;

            $scope.addUser = function () {
                storageService.store('users\page', $scope.currentPage);
                $location.url('/users/create');
            };
            $scope.editUser = function (user) {
                storageService.store('users\page', $scope.currentPage);
                $location.url('/users/edit/' + user.id);
            };
            $scope.deleteUser = function (user) {
                var modalInstance = $modal.open({
                    templateUrl: 'userDeleteDialog',
                    controller: UserDeleteCtrl,
                    windowClass: 'show', // Workaround for bootstrap 3 - dialog is not shown without this class
                    resolve: {
                        user: function () {
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

            var getUsers = function (page) {
                Users.get({ page: page, pageSize: $scope.pageSize, sort: 'Name', orderAscending: true }, function (data) {
                    $scope.users = data.items;
                    $scope.currentPage = data.page;
                    $scope.totalItems = data.totalItems;
                });
            };
            $scope.$watch('currentPage', getUsers);

            // Fetch data for the first page.
            $scope.currentPage = storageService.get('users\page') || 1;
        }]);

    userControllers.controller('UserCreateCtrl', [
        '$scope',
        '$location',
        'Users',
        function ($scope: IEditScope, $location, Users: sparrow.services.IUpdateResourceClass) {
            $scope.alerts = [];
            $scope.user = {};
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