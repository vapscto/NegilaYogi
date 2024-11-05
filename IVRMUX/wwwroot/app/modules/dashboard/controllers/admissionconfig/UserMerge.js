(function () {
    'use strict';
    angular.module('app').controller('UserMergeController', UserMergeController)

    UserMergeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function UserMergeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.samplearry = [
            { nname: 'I', amsT_ORDER: 1 },
            { nname: 'II', amsT_ORDER: 2 },
            { nname: 'III', amsT_ORDER: 3 },
            { nname: 'IV', amsT_ORDER: 4 },
            { nname: 'V', amsT_ORDER: 5 },
            { nname: 'VI', amsT_ORDER: 6 }
        ];

        $scope.motherchecked = false;
        $scope.fatherchecked = false;

        $scope.totalgrid = [];
        $scope.maingrid = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.sortKey = "amsS_Id";
        $scope.reverse = true;
        $scope.searchthird = "";
        $scope.searchthirdd = "";

        $scope.remflg = false;
        $scope.addnewbtn = true;

        $scope.formload = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;

            var data = {
                "pageid": 1
            };

            apiService.create("UserMerge/getalldetails", data).then(function (promise) {               
                $scope.getstudentdetails = promise.getstudentdetails;

                if ($scope.getstudentdetails!==null && $scope.getstudentdetails.length > 0) {
                    $scope.maingrid = true;                  
                }
                else {
                    swal("No records found");
                    $scope.maingrid = false;
                }
            });

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            };
        };
      
        $scope.onstudentnamechange = function (emptyrow) {
            $scope.totalgrid = [];
            $scope.getstudentlistsavedd = [];
            var data = {
                "AMST_HRME_Id": $scope.AMST_Id.amsT_Id
            };
            apiService.create("UserMerge/onstudentnamechange", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getuserdetails !== null && promise.getuserdetails.length > 0) {
                        $scope.getuserdetails = promise.getuserdetails;
                    } else {
                        swal("No Records Found");
                    }
                } else {
                    swal("Something Went Wrong Kindly Contact Administrator");
                }
            });
        };
     
        $scope.cleardata = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.submitted = false;        
    }

})();