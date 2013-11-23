module sparrow.services {
    'use strict';

    angular.module('sparrow.services', ['ngResource'])
        .factory('Users', ['$resource', function ($resource: ng.resource.IResourceService) {
            return $resource('api/users/:userId');
        }]);
}