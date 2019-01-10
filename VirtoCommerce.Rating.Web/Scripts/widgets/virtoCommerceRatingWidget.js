angular.module('virtoCommerce.Rating')
    .controller('virtoCommerce.Rating.ratingWidgetController', ['$scope', 'virtoCommerce.RatingApi', 'platformWebApp.bladeNavigationService', function ($scope, ratingApi, bladeNavigationService) {
        var blade = $scope.blade;

        function refresh() {
            $scope.loading = true;

            var params = {
                productId: blade.itemId,
                storeId: blade.catalog.id
            };

            ratingApi.get(params, function (data) {
                $scope.loading = false;
                $scope.rating = data.value;
            });
        }

        $scope.$watch("blade.itemId", function (id) {
            if (id) refresh();
        });
    }]);
