angular.module('virtoCommerce.Rating')
.factory('virtoCommerce.RatingApi', ['$resource', function ($resource) {
    return $resource('api/virtoCommerce.Rating');
}]);
