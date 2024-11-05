
(function () {
    'use strict';
    angular
.module('app')
.controller('MsterSessionController', MsterSessionController)

    MsterSessionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MsterSessionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.masterlist = false;
        $scope.sortKey = 'trdC_Id';
        $scope.sortReverse = true;

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("MsterSession/getdata/", pageid).then(function (promise) {
                if (promise.getloaddata.length > 0) {
                    $scope.masterdistancerate = promise.getloaddata;
                    $scope.presentCountgrid = promise.getloaddata.length;
                    $scope.masterlist = true;
                }
                else {
                    swal("No Records Found");
                    $scope.masterlist = false;
                }
            })
        }
        $scope.submitted = false;
      

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
  
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "TRMS_Id": $scope.TRMS_Id,
                    "TRMS_SessionName": $scope.TRMS_SessionName,
                    "TRMS_SessionDesc": $scope.TRMS_SessionDesc,
                    "TRMS_Flag": $scope.TRMS_Flag
                }
                apiService.create("MsterSession/savedata/", data).then(function (promise) {
                    if (promise.message == "Add") {
                        if (promise.returnval == true) {
                            swal("Record Saved Successfully");
                        }
                        else {
                            swal("Failed To Save Record");
                        }
                    }
                    else if (promise.message == "Update") {
                        if (promise.returnval == true) {
                            swal("Record Updated Successfully");
                        }
                        else {
                            swal("Failed To Update Record");
                        }
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exists");
                    }
                    $state.reload();
                })

            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.edit = function (user) {
            var data={
                "TRMS_Id":user.trmS_Id,
            }
            apiService.create("MsterSession/edit/", data).then(function (promise) {
                if (promise.geteditdata.length > 0) {
                    $scope.TRMS_Id = promise.geteditdata[0].trmS_Id;
                    $scope.TRMS_SessionName = promise.geteditdata[0].trmS_SessionName;
                    $scope.TRMS_SessionDesc = promise.geteditdata[0].trmS_SessionDesc;
                    $scope.TRMS_Flag = promise.geteditdata[0].trmS_Flag;
                }
                else {
                    swal("No Records Found");
                }
            })
        }

        //--Active Deactive---//
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.trmS_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";

            }
            else {

                mgs = "Active";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {
                apiService.create("MsterSession/activedeactive/", user).
                then(function (promise) {
                    if (promise.message != null) {
                        swal(promise.message);
                    }
                    else {
                        if (promise.returnval == true) {
                            swal(confirmmgs + " " + "Successfully.");
                            $state.reload();
                        }
                        else {
                            swal(confirmmgs + " " + " Successfully");
                            $state.reload();
                        }
                    }
                })
            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
            $state.reload();
        });
        }


        $scope.clear = function () {
            $state.reload();
        }
        //--Sorting--//     
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            
            return (angular.lowercase(obj.trmS_SessionName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.trmS_SessionDesc)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.trmS_Flag)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
        
    };
})();


