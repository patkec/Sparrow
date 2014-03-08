module sparrow {
    'use strict';

    // Export definitions
    export interface IRouteSettings {
        nav: number;
        content: string;
    }
    export interface IRouteDef {
        url: string;
        config: {
            settings: IRouteSettings;
        };
    }

    // Collect the routes
    app.constant('routes', getRoutes());

    // Configure the routes and route resolvers
    app.config(['$routeProvider', '$locationProvider', 'routes', routeConfigurator]);
    function routeConfigurator($routeProvider: ng.route.IRouteProvider, $locationProvider: ng.ILocationProvider, routes: IRouteDef[]) {
        routes.forEach(route=> {
            $routeProvider.when(route.url, route.config);
        });
        $routeProvider.otherwise({ redirectTo: '/' });

        // Enable pretty URLs (without #)
        $locationProvider.html5Mode(true);
    }

    // Define the routes 
    function getRoutes(): IRouteDef[] {
        return [
            {
                url: '/',
                config: {
                    templateUrl: 'app/dashboard/dashboard.html',
                    title: 'dashboard',
                    settings: {
                        nav: 1,
                        content: '<i class="fa fa-dashboard"></i> Dashboard'
                    }
                }
            }, {
                url: '/admin',
                config: {
                    title: 'admin',
                    templateUrl: 'app/admin/admin.html',
                    settings: {
                        nav: 2,
                        content: '<i class="fa fa-lock"></i> Admin'
                    }
                }
            }
        ];
    }
} 