module sparrow {
    'use strict';

    var app = angular.module('sparrow');

    app.directive('ccWidgetClose', ()=> {
        // Usage:
        // <a data-cc-widget-close></a>
        // Creates:
        // <a data-cc-widget-close="" href="#" class="wclose">
        //     <i class="fa fa-remove"></i>
        // </a>
        var directive = {
            link: link,
            template: '<i class="fa fa-remove"></i>',
            restrict: 'A'
        };
        return directive;

        function link(scope: ng.IScope, element: JQuery, attrs: ng.IAttributes) {
            attrs.$set('href', '#');
            attrs.$set('wclose', undefined);
            element.click(close);

            function close(e: ng.IAngularEvent) {
                e.preventDefault();
                element.parent().parent().parent().hide(100);
            }
        }
    });

    app.directive('ccWidgetMinimize', ()=> {
        // Usage:
        // <a data-cc-widget-minimize></a>
        // Creates:
        // <a data-cc-widget-minimize="" href="#"><i class="fa fa-chevron-up"></i></a>
        var directive = {
            link: link,
            template: '<i class="fa fa-chevron-up"></i>',
            restrict: 'A'
        };
        return directive;

        function link(scope: ng.IScope, element: JQuery, attrs: ng.IAttributes) {
            attrs.$set('href', '#');
            attrs.$set('wminimize', undefined);
            element.click(minimize);

            function minimize(e: ng.IAngularEvent) {
                e.preventDefault();
                var $wcontent = element.parent().parent().next('.widget-content');
                var iElement = element.children('i');
                if ($wcontent.is(':visible')) {
                    iElement.removeClass('fa fa-chevron-up');
                    iElement.addClass('fa fa-chevron-down');
                } else {
                    iElement.removeClass('fa fa-chevron-down');
                    iElement.addClass('fa fa-chevron-up');
                }
                $wcontent.toggle(500);
            }
        }
    });

    app.directive('ccWidgetHeader', ()=> {
        //Usage:
        //<div data-cc-widget-header title="vm.map.title"></div>
        var directive = {
            link: link,
            scope: {
                'title': '@',
                'subtitle': '@',
                'rightText': '@',
                'allowCollapse': '@'
            },
            templateUrl: '/app/layout/widget-header.html',
            restrict: 'A',
        };
        return directive;

        function link(scope: ng.IScope, element: JQuery, attrs: ng.IAttributes) {
            attrs.$set('class', 'widget-head');
        }
    });
} 