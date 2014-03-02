module sparrow.common {
    'use strict';

    export interface ISpinnerEvent extends ng.IAngularEvent {
        show: boolean;
    }
    export interface ISpinnerService {
        spinnerShow();
        spinnerHide();
    }

    angular.module('sparrow.common')
        .factory('spinner', ['common', 'commonConfig', spinner]);

    function spinner(common: ICommonService, commonConfig: ICommonConfigProvider) {
        var service: ISpinnerService = {
            spinnerHide: spinnerHide,
            spinnerShow: spinnerShow
        };

        return service;

        function spinnerHide() { spinnerToggle(false); }

        function spinnerShow() { spinnerToggle(true); }

        function spinnerToggle(show) {
            common.$broadcast(commonConfig.config.spinnerToggleEvent, { show: show });
        }
    }
} 