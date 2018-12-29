angular.module('VirtoCommerce.Rating')
.controller('VirtoCommerce.Rating.blade1Controller', ['$scope', 'VirtoCommerce.RatingApi', function ($scope, api) {
    var blade = $scope.blade;
    blade.title = 'VirtoCommerce.Rating';

    blade.refresh = function () {
        api.get(function (data) {
            blade.data = data.result;
            blade.isLoading = false;
        });
    }

    blade.refresh();
}]);
