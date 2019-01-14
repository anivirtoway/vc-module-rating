angular.module('virtoCommerce.Rating')
    .controller('virtoCommerce.Rating.ratingWidgetController', ['$scope', 'virtoCommerce.RatingApi', 'platformWebApp.bladeNavigationService', function ($scope, ratingApi, bladeNavigationService) {
        var blade = $scope.blade;

        function refresh() {
            $scope.loading = true;

            var params = {
                productIds: [blade.itemId],
                storeId: blade.catalog.id
            };

            ratingApi.get(params, function (data) {
                $scope.loading = false;
                var hasRating = data.ratings[0] && data.ratings[0].value;
                $scope.rating = hasRating ? data.ratings[0].value : 0;
            });
        }

        $scope.$watch("blade.itemId", function (id) {
            if (id) refresh();
        });
    }]);
