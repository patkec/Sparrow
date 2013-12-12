module sparrow.filters {
    'use strict';

    angular.module('sparrow.filters', [])
        .filter('offerStatus', function () {
            return function (input) {
                switch (input) {
                    case 0: return 'Offered';
                    case 1: return 'Accepted';
                    case 2: return 'Lost';
                    default: return 'N/A';
                }
            };
        });
}