//Call this to register our module to main application
var moduleTemplateName = "virtoCommerce.Rating";

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleTemplateName);
}

angular.module(moduleTemplateName, [])
.config(['$stateProvider', '$urlRouterProvider',
    function ($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('workspace.VirtoCommerceRating', {
                url: '/virtoCommerce.Rating',
                templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
                controller: [
                    '$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                        var newBlade = {
                            id: 'blade1',
                            controller: 'virtoCommerce.Rating.blade1Controller',
                            template: 'Modules/$(virtoCommerce.Rating)/Scripts/blades/helloWorld_blade1.tpl.html',
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
            path: 'browse/virtoCommerce.Rating',
            icon: 'fa fa-cube',
            title: 'VirtoCommerce.Rating',
            priority: 100,
            action: function () { $state.go('workspace.VirtoCommerceRating'); },
            permission: 'rating:read'
        };
        mainMenuService.addMenuItem(menuItem);

        //Register rating widget inside product blade
        var ratingWidget = {
            controller: 'virtoCommerce.Rating.ratingProductWidgetController',
            template: 'modules/$(virtoCommerce.Rating)/scripts/widgets/virtoCommerceRatingProductWidget.tpl.html'
        };
        widgetService.registerWidget(ratingWidget, 'itemDetail');

        //Register rating widget inside store blade
        var ratingStoreWidget = {
            controller: 'virtoCommerce.Rating.ratingStoreWidgetController',
            template: 'modules/$(virtoCommerce.Rating)/scripts/widgets/virtoCommerceRatingStoreWidget.tpl.html'
        };
        widgetService.registerWidget(ratingStoreWidget, 'storeDetail');
    }
]);
