var mainApp = angular.module('mainApp', []);

mainApp.controller('MainCtrl', ['$scope', '$http', '$window', '$q', '$location',
function ($scope, $http, $window, $q, $location) {
    $scope.homes = undefined;

    $scope.loadHomes = function () {
        var url = "/api/HomesApi";
        $http.get(url).success(function (homes) {
            $scope.homes = homes;

        });
    };

    $scope.loadHomes();
}]);