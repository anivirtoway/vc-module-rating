angular.module('virtoCommerce.Rating')
    .controller('virtoCommerce.Rating.ratingWidgetController', ['$scope', 'virtoCommerce.Rating.WebApi', 'platformWebApp.bladeNavigationService', function ($scope, ratingApi, bladeNavigationService) {
        var blade = $scope.blade;
        var filter = { take: 0 };

        function refresh() {
            $scope.loading = true;
            ratingApi.search(filter, function (data) {
                $scope.loading = false;
                $scope.totalCount = data.totalCount;
            });
        }

        $scope.openBlade = function () {
            if ($scope.loading || !$scope.totalCount)
                return;

            var newBlade = {
                id: "reviewsList",
                filter: filter,
                title: 'Customer reviews for "' + blade.title + '"',
                controller: 'virtoCommerce.Rating.ratingListController',
                template: 'Modules/$(virtoCommerce.Rating)/Scripts/blades/reviews-list.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        };

        $scope.$watch("blade.itemId", function (id) {
            filter.productIds = [id];

            if (id) refresh();
        });
    }]);
