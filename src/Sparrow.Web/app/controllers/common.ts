// File name is prefixed with _ because it should be loaded first (before other controllers)
module sparrow.controllers {
    'use strict';

    interface IMenuScope extends ng.IScope {
        activeViewPath: string;
        isActivePath(path: string): boolean;
    }

    export interface IItemDeleteScope extends ng.IScope {
        item: any;
        confirm();
        cancel();
    };

    export function ItemDeleteCtrl($scope: IItemDeleteScope, $modalInstance, item) {
        $scope.item = item;
        $scope.confirm = function () {
            $modalInstance.close($scope.item);
        };
        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    };    

    // Dependencies (within []) should be defined exactly once, otherwise a new module is created each time.
    angular.module('sparrow.controllers', ['sparrow.services', 'xeditable', 'notifications'])
        .config(['$provide', function ($provide) {
            // http://stackoverflow.com/questions/11252780/whats-the-correct-way-to-communicate-between-controllers-in-angularjs/19498009#19498009
            $provide.decorator('$rootScope', ['$delegate', function ($delegate) {
                Object.defineProperty($delegate.constructor.prototype, '$onRootScope', {
                    value: function (name, listener) {
                        var unsubscribe = $delegate.$on(name, function () {
                            listener.apply(arguments[0], [].slice.call(arguments, 1));
                        });
                        this.$on('$destroy', unsubscribe);
                    },
                    enumerable: false
                });
                return $delegate;
            }]);
        }])
        .controller('MenuCtrl', ['$scope', '$location', function ($scope: IMenuScope, $location: ng.ILocationService) {
            $scope.$on('$routeChangeSuccess', function () {
                $scope.activeViewPath = $location.path();
            });
            $scope.isActivePath = function (path: string) {
                if (!$scope.activeViewPath)
                    return false;

                var activeViewPathRoot = $scope.activeViewPath.slice(0, path.length);
                return activeViewPathRoot === path;
            };
        }])
        .controller('LoginCtrl', ['$routeParams', 'Auth', function ($routeParams, Auth) {
            Auth.login($routeParams.redirectUrl);
        }])
        .controller('LoginCallbackCtrl', ['$location', 'Auth', function ($location, Auth) {
            Auth.loginCallback($location.hash());
        }])
        .controller('NotificationCtrl', ['$scope', '$notification', 'Events', function ($scope, $notification, Events) {
            Events.initialize();

            $scope.$onRootScope('sparrow.offers.offerSent', function (offer) {
                $notification.info('Offer Sent', 'Offer "' + offer.Title + '" sent to customer "' + offer.Customer.Name + '".', offer);
            });
        }]);
}