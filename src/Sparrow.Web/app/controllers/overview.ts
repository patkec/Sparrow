module sparrow.controllers {
    'use strict';

    interface IOverviewScope extends ng.IScope {
        latestOffers: any;
        offersToExpire: any;
        offersChart: any;
    }

    angular.module('sparrow.controllers')
        .controller('OverviewCtrl', ['$scope', '$http', function ($scope: IOverviewScope, $http: ng.IHttpService) {
            $scope.latestOffers = [];
            $scope.offersToExpire = [];

            $http
                .get('/api/offers/latest/5')
                .success(function (data) {
                    $scope.latestOffers = data;
                });

            $http
                .get('/api/offers/soonToExpire/3')
                .success(function (data) {
                    $scope.offersToExpire = data;
                });

            // Some dummy data
            $scope.offersChart = {
                title: {
                    text: 'Offers Through Time'
                },
                colors: [
                    '#0e76a8',
                    '#008000',
                    '#e22a2a'
                ],
                xAxis: {
                    categories: ['Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                    tickmarkPlacement: 'on',
                    title: {
                        enabled: false
                    }
                },
                yAxis: {
                    title: {
                        text: '# of Offers'
                    }
                },
                plotOptions: {
                    area: {
                        stacking: 'normal',
                        lineColor: '#666666',
                        lineWidth: 1,
                        marker: {
                            lineWidth: 1,
                            lineColor: '#666666'
                        }
                    }
                },
                series: [
                    {
                        name: 'New Offers',
                        data: [50, 63, 80, 94, 140, 363, 526]
                    }, {
                        name: 'Accepted Offers',
                        data: [10, 10, 11, 13, 22, 76, 176]
                    }, {
                        name: 'Lost Offers',
                        data: [16, 20, 27, 40, 54, 72, 62]
                    }]
            };
        }]);
}