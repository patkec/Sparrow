module sparrow {
    'use strict';

    interface IShellCtrl {
        isBusy: boolean;
        busyMessage: string;
        spinnerOptions: {
            radius: number;
            lines: number;
            length: number;
            width: number;
            speed: number;
            corners: number;
            trail: number;
            color: string;
        };
        toggleSpinner(on: boolean): void;
    }

    class ShellCtrl implements IShellCtrl {
        static controllerId: string = 'shell';

        private _common: sparrow.common.ICommonService;
        private _logSuccess: sparrow.common.ISimpleLogFn;

        isBusy = true;
        busyMessage = 'Please wait ...';
        spinnerOptions = {
            radius: 40,
            lines: 7,
            length: 0,
            width: 30,
            speed: 1.7,
            corners: 1.0,
            trail: 100,
            color: '#F58A00'
        };

        constructor($rootScope: ng.IScope, common: sparrow.common.ICommonService, config: sparrow.IConfig) {
            this._common = common;
            this._logSuccess = common.logger.getLogFn(ShellCtrl.controllerId, 'success');

            this.activate();

            var events = config.events;

            $rootScope.$on('$routeChangeStart', (event, next, current) => {
                this.toggleSpinner(true);
            });

            $rootScope.$on(events.controllerActivateSuccess, data=> {
                this.toggleSpinner(false);
            });

            $rootScope.$on(events.spinnerToggle, (data: sparrow.common.ISpinnerEvent) => {
                this.toggleSpinner(data.show);
            });
        }

        private activate() {
            this._logSuccess('Sparrow loaded!', null, true);
            this._common.activateController([], ShellCtrl.controllerId);
        }

        toggleSpinner(on: boolean) {
            this.isBusy = on;
        }
    }

    app.controller(ShellCtrl.controllerId, ['$rootScope', 'common', 'config', ($rootScope, common, config) => new ShellCtrl($rootScope, common, config)]);
}