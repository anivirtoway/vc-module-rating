angular.module('virtoCommerce.Rating')
    .controller('virtoCommerce.Rating.blade1Controller', ['$scope', 'virtoCommerce.RatingApi', function ($scope, api) {
        var blade = $scope.blade;
        blade.title = 'VirtoCommerce.Rating';
        blade.isLoading = false;

        blade.refresh = function () {
            api.get(function (data) {
                blade.data = data.result;
                blade.isLoading = false;
            });
        }

        //blade.refresh();
    }]);
