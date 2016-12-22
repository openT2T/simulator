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


    $scope.switchOff = function (homeId, thing) {
        console.log("Switch off " + thing.Name);

        $scope.setThingProperty(homeId, thing, "Switch", false);
    }

    $scope.switchOn = function (homeId, thing) {
        console.log("Switch on " + thing.Name);

        $scope.setThingProperty(homeId, thing, "Switch", true);
    }

    $scope.dim = function (homeId, thing) {
        console.log("Set " + thing.Name + " to " + thing.Dim);

        $scope.setThingProperty(homeId, thing, "Dim", parseInt(thing.Dim));
    }

    $scope.setThingProperty = function(homeId, thing, propertyName, value) {
        var postData = {
            id: homeId,
            deviceId: thing.Id,
            propertyName: propertyName,
            value: value
        };

        var postObj = {
            url: '/api/HomesApi',
            method: 'POST',
            data: postData
        }

        console.log(postData);
        console.log(postObj);

        $http(postObj).success(function (data, status) {
            console.log(data);
            console.log(status);
            thing.Dim = data.Dim;
        }).error(function (data, status) {
            console.log(data);
            console.log(status);
        });
    }

    $scope.range = function (min, max, step) {
        step = step || 1;
        var input = [];
        for (var i = min; i <= max; i += step) {
            input.push(i);
        }
        return input;
    };

    $scope.loadHomes();
}]);