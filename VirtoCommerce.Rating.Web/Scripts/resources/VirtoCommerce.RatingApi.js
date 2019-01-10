angular.module('virtoCommerce.Rating')
.factory('virtoCommerce.RatingApi', ['$resource', function ($resource) {
    return $resource('api/rating/:action', {},
        {
            //search: { method: 'POST', action: 'search' }
            get: { method: 'GET', action: '' }
        });
}]);
