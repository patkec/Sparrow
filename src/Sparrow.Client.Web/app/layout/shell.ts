module sparrow {
    'use strict';

    var controllerId = 'shell';
    angular.module('sparrow').controller(controllerId, ['$rootScope', 'common', 'config', shell]);

    function shell($rootScope: ng.IScope, common: sparrow.common.ICommonService, config: sparrow.IConfig) {
        var events = config.events;
        var logSuccess = common.logger.getLogFn(controllerId, 'success');

        var vm = this;
        vm.isBusy = true;
        vm.busyMessage = "Please wait ...";
        vm.spinnerOptions = {
            radius: 40,
            lines: 7,
            length: 0,
            width: 30,
            speed: 1.7,
            corners: 1.0,
            trail: 100,
            color: '#F58A00'
        };

        activate();

        function activate() {
            logSuccess('Sparrow loaded!', null, true);
            common.activateController([], controllerId);
        }

        function toggleSpinner(on) {
             vm.isBusy = on;
        }

        $rootScope.$on('$routeChangeStart', (event, next, current) => {
            toggleSpinner(true);
        });

        $rootScope.$on(events.controllerActivateSuccess, data=> {
            toggleSpinner(false);
        });

        $rootScope.$on(events.spinnerToggle, (data: sparrow.common.ISpinnerEvent)=> {
            toggleSpinner(data.show);
        });
    }
}