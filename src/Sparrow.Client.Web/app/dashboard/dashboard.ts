module sparrow {
    'use strict';

    interface IDashboardCtrl {
        news: any;
        people: any[];
        title: string;
        messageCount: number;
    }

    class DashboardCtrl implements IDashboardCtrl {
        static controllerId: string = 'dashboard';

        news = {};
        messageCount = 0;
        people = [];
        title = '';

        constructor() {
            var vm = this;
            vm.news = {
                title: 'Sparrow',
                description: 'Sparrow is based on Hot Towel Angular.'
            };
            vm.messageCount = 0;
            vm.people = [];
            vm.title = 'Dashboard';
        }
    }

    app.controller(DashboardCtrl.controllerId, [() => new DashboardCtrl()]);
} 