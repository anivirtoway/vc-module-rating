angular.module('virtoCommerce.Rating')
.factory('virtoCommerce.RatingApi', ['$resource', function ($resource) {
    return $resource('api/rating', {},
        {
            calculateStore: { method: 'POST', url: 'api/rating/calculateStore' },
            get: { method: 'GET' }
        });
}]);
