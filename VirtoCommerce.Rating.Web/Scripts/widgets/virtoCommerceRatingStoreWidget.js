angular.module('virtoCommerce.Rating')
    .controller('virtoCommerce.Rating.ratingStoreWidgetController', ['$scope', 'virtoCommerce.RatingApi', 'platformWebApp.bladeNavigationService', function ($scope, ratingApi, bladeNavigationService) {
        var blade = $scope.blade;
        console.log('store_blade_widget', blade);

        $scope.openBlade = function () {
            var newBlade = {
                id: "rating_store_blade",
                storeId: blade.currentEntityId,
                title: 'Rating for "' + blade.title + '"',
                isClosingDisabled: true,
                controller: 'virtoCommerce.Rating.storeBladeController',
                template: 'Modules/$(virtoCommerce.Rating)/Scripts/blades/store/store_blade.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        };

    }]);
