//Call this to register our module to main application
var moduleTemplateName = "VirtoCommerce.Rating";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleTemplateName);
}

angular.module(moduleTemplateName, [])
.config(['$stateProvider', '$urlRouterProvider',
    function ($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('workspace.VirtoCommerce.Rating', {
                url: '/VirtoCommerce.Rating',
                templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
                controller: [
                    '$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                        var newBlade = {
                            id: 'blade1',
                            controller: 'VirtoCommerce.Rating.blade1Controller',
                            template: 'Modules/$(VirtoCommerce.Rating)/Scripts/blades/helloWorld_blade1.tpl.html',
                            isClosingDisabled: true
                        };
                        bladeNavigationService.showBlade(newBlade);
                    }
                ]
            });
    }
])
.run(['$rootScope', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state',
    function ($rootScope, mainMenuService, widgetService, $state) {
        //Register module in main menu
        var menuItem = {
            path: 'browse/VirtoCommerce.Rating',
            icon: 'fa fa-cube',
            title: 'VirtoCommerce.Rating',
            priority: 100,
            action: function () { $state.go('workspace.VirtoCommerce.Rating') },
            permission: 'VirtoCommerce.RatingPermission'
        };
        mainMenuService.addMenuItem(menuItem);
    }
]);
