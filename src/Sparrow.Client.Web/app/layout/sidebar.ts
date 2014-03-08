module sparrow {
    'use strict';

    interface ISidebarCtrl {
        navRoutes: any[];
        isCurrent(route: any): string;
    }

    class SidebarCtrl implements  ISidebarCtrl {
        static controllerId: string = 'sidebar';

        private _routes: IRouteDef[];
        private $_routeService: ng.route.IRouteService;

        navRoutes = [];

        constructor($route: ng.route.IRouteService, routes: IRouteDef[]) {
            this.$_routeService = $route;
            this._routes = routes;

            this.fillNavRoutes();
        }

        private fillNavRoutes(): void {
            this.navRoutes = this._routes
                .filter(route=> route.config.settings !== undefined && route.config.settings.nav !== undefined)
                .sort((r1, r2) => r1.config.settings.nav - r2.config.settings.nav);
        }

        isCurrent(route: any): string {
            var currentRouteTitle = null;

            if (this.$_routeService.current) {
                currentRouteTitle = this.$_routeService.current['title'];
            }

            if (!route.config.title || !currentRouteTitle) {
                return '';
            }
            var menuName = route.config.title;
            return currentRouteTitle.substr(0, menuName.length) === menuName ? 'current' : '';
        }
    }

    app.controller(SidebarCtrl.controllerId, ['$route', 'routes', ($route, routes) => new SidebarCtrl($route, routes)]);
} 