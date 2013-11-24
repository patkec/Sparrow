module sparrow.services {
    'use strict';

    // EXPORTS
    export interface IStorageService {
        store(key: string, value: any);
        get(key: string): any;
    }
    export interface IUpdateResourceClass extends ng.resource.IResourceClass {
        update(item: any, success: Function, error: Function);
    }

    // DEFINITIONS
    angular.module('sparrow.services', ['ngResource'])
        .constant('$', $)
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
        .factory('Offers', ['$resource', function ($resource: ng.resource.IResourceService) {
            return $resource('/api/offers/:offerId', {}, {
                update: { method: 'PUT' }
            });
        }])
        .factory('Customers', ['$resource', function ($resource: ng.resource.IResourceService) {
            return $resource('/api/customers/:userId', {}, {
                update: { method: 'PUT' }
            });
        }])
        .factory('Products', ['$resource', function ($resource: ng.resource.IResourceService) {
            return $resource('/api/products/:userId', {}, {
                update: { method: 'PUT' }
            });
        }])
        .factory('Users', ['$resource', function ($resource: ng.resource.IResourceService) {
            return $resource('/api/users/:userId', {}, {
                update: {method: 'PUT'}
            });
        }]);
}