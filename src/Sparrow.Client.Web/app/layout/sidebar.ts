module sparrow {
    'use strict';

    var controllerId = 'sidebar';
    angular.module('sparrow').controller(controllerId, ['$route', 'config', 'routes', sidebar]);

    function sidebar($route: ng.route.IRouteService, config: sparrow.IConfig, routes: IRouteDef[]) {
        var vm = this;
        vm.isCurrent = isCurrent;

        activate();

        function activate() { getNavRoutes(); }

        function getNavRoutes() {
            vm.navRoutes = routes
                .filter(route=> route.config.settings !== undefined && route.config.settings.nav !== undefined)
                .sort((r1, r2) => r1.config.settings.nav - r2.config.settings.nav);
        }

        function isCurrent(route) {
            var currentRouteTitle = null;

            if ($route.current) {
                currentRouteTitle = $route.current['title'];
            }

            if (!route.config.title || !currentRouteTitle) {
                return '';
            }
            var menuName = route.config.title;
            return currentRouteTitle.substr(0, menuName.length) === menuName ? 'current' : '';
        }
    }
} 