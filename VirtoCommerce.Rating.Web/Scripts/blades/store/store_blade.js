angular.module('virtoCommerce.Rating')
    .controller('virtoCommerce.Rating.storeBladeController', ['$scope', 'virtoCommerce.RatingApi', function ($scope, api) {
        var blade = $scope.blade;
        blade.title = 'Store rating';
        blade.isLoading = false;

        console.log('store_blade', blade);

        $scope.calculate = function () {
            blade.isLoading = true;
            api.calculateStore({ storeId: blade.storeId }, null, function (responce) {
                blade.isLoading = false;
            });
        };

    }]);
