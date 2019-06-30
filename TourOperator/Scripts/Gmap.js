var app = angular.module('myApp', ['uiGmapgoogle-maps']);
app.controller('mapController', function ($scope, $http) {

    //this is for default map focus when load first time
    $scope.map = { center: { latitude: 7.6364877, longitude: 80.4212718 }, zoom: 6 }

    $scope.markers = [];
    $scope.locations = [];
    $scope.locs = [];

    var province = $('#SearchProvince').val();
    var district = $('#SearchDistrict').val();

    //Populate all location
    $http.get('/Locations/GetAllLocation?province=' + province + '&district=' + district).then(function (data) {
        $scope.locations = data.data;
    }, function () {
        alert('Error in loading');
    });

    //populate locations
   


    //$scope.ShowLocation = function (SearchDistrict, SearchProvince) {
    //    $http.get('/Locations/GetAllLocation', {
    //        params: {
    //            SearchDistrict: SearchDistrict,
    //            SearchProvince: SearchProvince
    //        }
    //    }).then(function (data) {
    //        $scope.locations = data.data;
    //    }, function () {
    //        alert('Error in loading');
    //    });
    //}

    //get marker info
    
    $scope.ShowLocation = function (locationID) {
        $http.get('/Locations/GetMarkerInfo', {
            params: {
                locationID: locationID
            }
        }).then(function (data) {
            //clear markers 
            $scope.markers = [];
            $scope.markers.push({
                id: data.data.LocationID,
                coords: { latitude: data.data.Lat, longitude: data.data.Long },
                title: data.data.title,
                address: data.data.Address,
                image: data.data.ImagePath
            });

            //set map focus to center
            $scope.map.center.latitude = data.data.Lat;
            $scope.map.center.longitude = data.data.Long;

        }, function () {
            alert('Error in loading');
        });
    }

    //Show / Hide marker on map
    $scope.windowOptions = {
        show: true
    };

});