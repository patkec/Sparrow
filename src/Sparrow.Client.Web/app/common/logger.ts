module sparrow.common {
    'use strict';

    // Export definitions
    export interface ILogFn {
        (message: string, data: any, source: string, showToast?: boolean): void;
    }
    export interface ISimpleLogFn {
        (message: string, data: any, showToast?: boolean): void;
    }
    export interface ILoggerService {
        getLogFn(moduleId: string, fnName?: string): ISimpleLogFn;
        log: ILogFn;
        logError: ILogFn;
        logSuccess: ILogFn;
        logWarning: ILogFn;
    }

    // Implementation
    angular.module('sparrow.common').factory('logger', ['$log', logger]);

    function logger($log: ng.ILogService) {

        var service: ILoggerService = {
            getLogFn: getLogFn,
            log: log,
            logError: logError,
            logSuccess: logSuccess,
            logWarning: logWarning
        };

        return service;

        function getLogFn(moduleId: string, fnName?: string): ISimpleLogFn {
            fnName = fnName || 'log';
            switch (fnName.toLowerCase()) {
                case 'success':
                    fnName = 'logSuccess'; break;
                case 'error':
                    fnName = 'logError'; break;
                case 'warn':
                case 'warning':
                    fnName = 'logWarning'; break;
            }

            var logFn = service[fnName] || service.log;
            return (message: string, data: any, showToast?: boolean)=> {
                logFn(message, data, moduleId, (showToast === undefined) ? true : false);
            };
        }

        function log(message: string, data: any, source: string, showToast?: boolean) {
            logIt(message, data, source, showToast, 'info');
        }

        function logWarning(message: string, data: any, source: string, showToast?: boolean) {
            logIt(message, data, source, showToast, 'warning');
        }

        function logSuccess(message: string, data: any, source: string, showToast?: boolean) {
            logIt(message, data, source, showToast, 'success');
        }

        function logError(message: string, data: any, source: string, showToast?: boolean) {
            logIt(message, data, source, showToast, 'error');
        }

        function logIt(message: string, data: any, source: string, showToast?: boolean, toastType?: string) {
            var write = (toastType === 'error') ? $log.error : $log.log;
            source = source ? '[' + source + '] ' : '';
            write(source, message, data);
            if (showToast) {
                if (toastType === 'error') {
                    toastr.error(message);
                } else if (toastType === 'warning') {
                    toastr.warning(message);
                } else if (toastType === 'success') {
                    toastr.success(message);
                } else {
                    toastr.info(message);
                }
            }
        }
    }
} 