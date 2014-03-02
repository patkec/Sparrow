module sparrow {
    'use strict';

    var app = angular.module('sparrow', [
        // Angular modules
        'ngAnimate',            // animations
        'ngRoute',              // routing
        'ngSanitize',           // sanitizes html bindings

        // Custom modules
        'sparrow.common',       // common functions, logger, spinner
        //'common.bootstrap'      // boostrap dialog wrapper functions
    ]);

    app.run(['$route', ($route: ng.route.IRoute)=> {
        // Include $route to kick start the router
    }]);
}