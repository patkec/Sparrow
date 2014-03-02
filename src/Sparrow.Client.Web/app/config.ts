module sparrow {
    'use strict';

    // Export definitions
    export interface IEvents {
        controllerActivateSuccess: string;
        spinnerToggle: string;
    }
    export interface IConfig {
        appErrorPrefix: string;
        docTitle: string;
        events: IEvents;
        remoteResourceUrl: string;
        version: string;
    }
    
    var app = angular.module('sparrow');

    // Configure Toastr
    toastr.options.timeOut = 4000;
    toastr.options.positionClass = 'toast-bottom-right';

    var events: IEvents = {
        controllerActivateSuccess: 'controller.activateSuccess',
        spinnerToggle: 'spinner.toggle'
    };

    var config: IConfig = {
        appErrorPrefix: '[Sparrow Error] ', //Configure the exceptionHandler decorator
        docTitle: 'Sparrow: ',
        events: events,
        remoteResourceUrl: 'https://localhost:44304/',
        version: '0.1.0'
    };

    app.value('config', config);

    app.config(['$logProvider', ($logProvider)=> {
        // turn debugging off/on (no info or warn)
        if ($logProvider.debugEnabled) {
            $logProvider.debugEnabled(true);
        }
    }]);

    app.config(['commonConfigProvider', (cfg: sparrow.common.ICommonConfigProvider)=> {
        cfg.config.controllerActivateSuccessEvent = config.events.controllerActivateSuccess;
        cfg.config.spinnerToggleEvent = config.events.spinnerToggle;
    }]);
}