angular.module('virtoCommerce.Rating')
    .controller('virtoCommerce.Rating.ratingProductWidgetController',
        ['$scope', 'virtoCommerce.RatingApi', 'platformWebApp.bladeNavigationService',
            function ($scope, ratingApi, bladeNavigationService) {

                var blade = $scope.blade;

                function init() {
                    $scope.loading = false;
                    $scope.ratingRange = '0';
                    $scope.ratings = null;
                }

                function refresh() {
                    $scope.loading = true;

                    var params = {
                        productIds: [blade.itemId],
                        catalogId: blade.catalog.id
                    };

                    ratingApi.get(params, function (data) {
                        $scope.loading = false;
                        $scope.ratings = data.ratings;
                        $scope.ratingRange = getRatingRange(data.ratings);
                    });
                }

                function getRatingRange(ratings) {
                    var ratingValues = ratings.map(s => s.value);

                    if (ratings.length === 1) {
                        return ratingValues[0].toFixed(1);
                    }

                    if ($scope.ratings.length > 1) {
                        var max = Math.max(...ratingValues).toFixed(1);
                        var min = Math.min(...ratingValues).toFixed(1);

                        if (min === max) {
                            return `${min}`;
                        }

                        return `${min}-${max}`;
                    }

                    return 0;
                }

                $scope.openBlade = function () {
                    if ($scope.loading) return;

                    var newBlade = {
                        id: "rating_product_blade",
                        ratings: $scope.ratings,
                        title: 'Rating for "' + blade.title + '"',
                        controller: 'virtoCommerce.Rating.productBladeController',
                        template: 'Modules/$(virtoCommerce.Rating)/Scripts/blades/product/product_blade.tpl.html'
                    };
                    bladeNavigationService.showBlade(newBlade, blade);
                };

                $scope.$watch("blade.itemId", function (id) {
                    if (id) refresh();
                });

                init();

            }]);
