module sparrow.services {
    'use strict';

    // EXPORTS
    export interface IStorageService {
        store(key: string, value: any);
        get(key: string): any;
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
                update: { method: 'PUT' },
                getArchived: { method: 'GET', url: '/api/offers/archived' }
            });
        }])
        .factory('Drafts', ['$resource', function ($resource: ng.resource.IResourceService) {
            return $resource('/api/drafts/:draftId', {}, {
                update: { method: 'PUT' },
                createOffer: { url: '/api/drafts/:draftId/offer', method: 'POST', params: {draftId:'@draftId'}}
            });
        }])
        .factory('Customers', ['$resource', function ($resource: ng.resource.IResourceService) {
            return $resource('/api/customers/:customerId', {}, {
                update: { method: 'PUT' }
            });
        }])
        .factory('Products', ['$resource', function ($resource: ng.resource.IResourceService) {
            return $resource('/api/products/:productId', {}, {
                update: { method: 'PUT' }
            });
        }])
        .factory('Users', ['$resource', function ($resource: ng.resource.IResourceService) {
            return $resource('/api/users/:userId', {}, {
                update: {method: 'PUT'}
            });
        }]);
}