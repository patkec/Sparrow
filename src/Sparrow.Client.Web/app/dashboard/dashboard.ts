module sparrow {
    'use strict';

    var controllerId = 'dashboard';
    angular.module('sparrow').controller(controllerId, [dashboard]);

    function dashboard() {
        var vm = this;
        vm.news = {
            title: 'Sparrow',
            description: 'Sparrow is based on Hot Towel Angular.'
        };
        vm.messageCount = 0;
        vm.people = [];
        vm.title = 'Dashboard';

        activate();

        function activate() {
        }
    }
} 