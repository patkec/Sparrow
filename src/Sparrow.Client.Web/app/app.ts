module sparrow {
    'use strict';

    export var app = angular.module('sparrow', [
        // Angular modules
        'ngAnimate',            // animations
        'ngRoute',              // routing
        'ngSanitize',           // sanitizes html bindings

        // Custom modules
        'sparrow.common',       // common functions, logger, spinner
        //'common.bootstrap'      // boostrap dialog wrapper functions

        // 3rd party modules
        'breeze.angular',
        'breeze.directives',
        'ui.bootstrap'
    ]);

    app.run(['$route', ($route: ng.route.IRoute)=> {
        // Include $route to kick start the router
    }]);
}