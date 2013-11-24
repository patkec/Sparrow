module sparrow.services {
    'use strict';

    // EXPORTS
    export interface IStorageService {
        store(key: string, value: any);
        get(key: string): any;
    }
    export interface IUserResourceClass extends ng.resource.IResourceClass {
        update(user: any, success: Function, error: Function);
    }

    // DEFINITIONS
    angular.module('sparrow.services', ['ngResource'])
        .factory('storageService', function () {
            var data = {};

            return {
                store: function (key, value) {
                    data[key] = value;
                },
                get: function (key) {
                    return data[key];
                }
            };
        })
        // RESOURCES
        .factory('Users', ['$resource', function ($resource: ng.resource.IResourceService) {
            return $resource('/api/users/:userId', {}, {
                update: {method: 'PUT'}
            });
        }]);
}