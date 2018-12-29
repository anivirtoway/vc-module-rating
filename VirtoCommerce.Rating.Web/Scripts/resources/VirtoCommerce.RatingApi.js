angular.module('VirtoCommerce.Rating')
.factory('VirtoCommerce.RatingApi', ['$resource', function ($resource) {
    return $resource('api/VirtoCommerce.Rating');
}]);
