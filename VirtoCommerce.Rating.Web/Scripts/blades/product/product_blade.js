angular.module('virtoCommerce.Rating')
    .controller('virtoCommerce.Rating.productBladeController', ['$scope', 'virtoCommerce.RatingApi', function ($scope, ratingApi) {
        var blade = $scope.blade;
        blade.title = 'Product rating';
        $scope.ratings = blade.ratings;
        blade.isLoading = false;

        console.log('product_blade', blade);

        //function refresh() {
        //    blade.isLoading = true;

        //    var params = {
        //        productIds: [blade.productId],
        //        catalogId: blade.catalogId
        //    };

        //    ratingApi.get(params, function (data) {
        //        blade.isLoading = false;
        //        $scope.ratings = data.ratings;
        //        //var hasRating = data.ratings[0] && data.ratings[0].value;
        //        //$scope.rating = hasRating ? data.ratings[0].value : 0;
        //    });
        //}

        
    }]);
