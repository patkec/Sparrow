module sparrow.directives {
    'use strict';

    angular.module('sparrow.directives', [])
        .directive('editableDisabled', function () {
            return {
                restrict: 'A',
                link: function (scope, elem, attrs) {
                    scope.$watch(attrs.editableDisabled, function (val) {
                        if (val === true) {                            
                            elem.attr('disabled', 'disabled');
                        }
                        else {
                            elem.removeAttr('disabled');
                        }
                    });
                }
            };
        })
        .directive('starRating', function () {
            return {
                restrict: 'A',
                template: '<ul class="rating">' +
                    '<li ng-repeat="star in stars" ng-class="star" ng-click="toggle($index)">\u2605</li>' +
                    '</ul>',
                scope: {
                    max: '=',
                    ratingValue: '=',
                    readonly: '@'
                },
                link: function (scope) {
                    var updateStars = function () {
                        scope.stars = [];
                        for (var i = 0; i < scope.max; i++) {
                            scope.stars.push({
                                readonly: scope.readonly,
                                filled: i < scope.ratingValue
                            });
                        }
                    };

                    scope.toggle = function (index) {
                        if (scope.readonly && scope.readonly === 'true') {
                            return;
                        }
                        scope.ratingValue = index + 1;
                    };

                    scope.$watch('ratingValue', function () {
                        updateStars();
                    });
                }
            };
        });
}