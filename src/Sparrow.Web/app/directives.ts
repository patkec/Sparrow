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
        })
        // Courtesy of https://github.com/rootux/angular-highcharts-directive/blob/master/src/directives/highchart.js
        .directive('chart', function () {
            return {
                restrict: 'E',
                template: '<div></div>',
                scope: {
                    chartData: "=value"
                },
                transclude: true,
                replace: true,

                link: function (scope, element, attrs) {
                    var chartsDefaults = {
                        chart: {
                            renderTo: element[0],
                            type: attrs.type || null,
                            height: attrs.height || null,
                            width: attrs.width || null
                        }
                    };

                    //Update when charts data changes
                    scope.$watch(function () { return scope.chartData; }, function (value) {
                        if (!value) return;
                        // We need deep copy in order to NOT override original chart object.
                        // This allows us to override chart data member and still the keep
                        // our original renderTo will be the same
                        var deepCopy = true;
                        var newSettings = {};
                        $.extend(deepCopy, newSettings, chartsDefaults, scope.chartData);
                        var chart = new Highcharts.Chart(newSettings);
                    });
                }
            };

        });;
}