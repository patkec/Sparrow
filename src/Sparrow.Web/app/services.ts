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
        .factory('Events', ['$', '$rootScope', function ($, $rootScope) {
            var connection, adminHub, offersHub;

            var initialize = function() {
                connection = $.hubConnection('https://localhost:44304/signalr');
                adminHub = connection.createHubProxy('adminHub');
                offersHub = connection.createHubProxy('offersHub');

                adminHub.on('userCreated', function (user) {
                    $rootScope.$apply(function() {
                        $rootScope.$emit('sparrow.admin.userCreated', user);
                    });
                });
                adminHub.on('customerCreated', function (customer) {
                    $rootScope.$apply(function() {
                        $rootScope.$emit('sparrow.admin.customerCreated', customer);
                    });
                });
                adminHub.on('productCreated', function (product) {
                    $rootScope.$apply(function() {
                        $rootScope.$emit('sparrow.admin.productCreated', product);
                    });
                });

                offersHub.on('offerSent', function (offer) {
                    $rootScope.$apply(function() {
                        $rootScope.$emit('sparrow.offers.offerSent', offer);
                    });
                });
                offersHub.on('offerWon', function (offer) {
                    $rootScope.$apply(function() {
                        $rootScope.$emit('sparrow.offers.offerWon', offer);
                    });
                });
                offersHub.on('offerLost', function (offer) {
                    $rootScope.$apply(function() {
                        $rootScope.$emit('sparrow.offers.offerLost', offer);
                    });
                });

                connection.start();
            };

            return {
                initialize: initialize
            };
        }])
        .factory('authInterceptor', ['$q', '$location', 'Auth', function($q, $location, Auth) {
            return {
                request: function (config) {
                    if (Auth.accessToken) {
                        config.headers.Authorization = 'Bearer ' + Auth.accessToken;
                    }
                    return config;
                },
                responseError: function (response) {
                    if (response.status === 401) {
                        $location.path('/login').search('redirectUrl=' + $location.absUrl());
                        window.location.href = $location.absUrl();
                    }
                    return $q.reject(response);
                }
            };
        }])
        .factory('Auth', ['$rootScope', '$location', '$window', function ($rootScope, $location, $window) {
            var accessToken = '',
                authorizeUrl = 'https://authzsrv.cloudapp.net/sparrow/oauth/authorize',
                clientId = 'sparrow-web',
                responseType = 'token',
                scope = 'customers users offers products drafts';

            var parseHash = hash=> {
                var params = { state: '', access_token: '' },
                    regex = /([^&=]+)=([^&]*)/g,
                    match;

                while (match = regex.exec(hash)) {
                    params[decodeURIComponent(match[1])] = decodeURIComponent(match[2]);
                }

                if (params['error']) {
                    throw params['error'];
                }

                return params;
            };

            return {
                accessToken: this.accessToken,
                login: function (redirectUrl) {
                    $location.url('/loginCallback');

                    var state = Date.now() + '' + Math.random(),
                        url = authorizeUrl + '?' +
                            'client_id=' + encodeURI(clientId) + '&' +
                            'redirect_uri=' + encodeURI($location.absUrl()) + '&' +
                            'response_type=' + encodeURI(responseType) + '&' +
                            'scope=' + encodeURI(scope) + '&' +
                            'state=' + encodeURI(state);
                    sessionStorage['loginState'] = state;
                    $window.location.href = url;
                },
                loginCallback: function (hash) {
                    var params = parseHash(hash);

                    if (params.state !== sessionStorage['loginState']) {
                        throw 'Error: bad state!';
                    }
                    sessionStorage.removeItem('loginState');

                    this.accessToken = params.access_token;

                    $location.url('/');
                },
                logout: function () { }
            };
        }])
        // RESOURCES
        .factory('Offers', ['$resource', function ($resource: ng.resource.IResourceService) {
            return $resource('https://localhost:44304/api/offers/:offerId', {}, {
                update: { method: 'PUT' },
                getArchived: { method: 'GET', url: 'https://localhost:44304/api/offers/archived' }
            });
        }])
        .factory('Drafts', ['$resource', function ($resource: ng.resource.IResourceService) {
            return $resource('https://localhost:44304/api/drafts/:draftId', {}, {
                update: { method: 'PUT' },
                createOffer: { url: 'https://localhost:44304/api/drafts/:draftId/offer', method: 'POST', params: {draftId:'@draftId'}}
            });
        }])
        .factory('Customers', ['$resource', function ($resource: ng.resource.IResourceService) {
            return $resource('https://localhost:44304/api/customers/:customerId', {}, {
                update: { method: 'PUT' }
            });
        }])
        .factory('Products', ['$resource', function ($resource: ng.resource.IResourceService) {
            return $resource('https://localhost:44304/api/products/:productId', {}, {
                update: { method: 'PUT' }
            });
        }])
        .factory('Users', ['$resource', function ($resource: ng.resource.IResourceService) {
            return $resource('https://localhost:44304/api/users/:userId', {}, {
                update: {method: 'PUT'}
            });
        }]);
}