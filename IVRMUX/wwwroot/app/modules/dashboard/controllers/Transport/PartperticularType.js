
(function () {
    'use strict';
    angular
.module('app')
        .controller('PartperticularTypeController', PartperticularTypeController)

    PartperticularTypeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function PartperticularTypeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.gridshow = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 10;
        }
        $scope.sortKey = 'trpapT_Id';
        $scope.sortReverse = true;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.searchValue = "";
        $scope.loaddata = function () {
            $scope.gridshow = false;
            var pageid = 2;
            apiService.getURI("MasterServiceStation/loadparttype", pageid).then(function (promise) {
                if (promise != null) {
                    if (promise.parttypegrig.length > 0) {
                        $scope.gridshow = true;
                        $scope.parttypegrig = promise.parttypegrig;
                        $scope.presentCountgrid = $scope.parttypegrig.length;
                    }
                    else {
                        swal("No Records Found")
                    }
                }
                else {
                    swal("No Records Found")
                }
            })



        }

        $scope.submitted = false;


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //-------Save Data-------//
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                
                var data = {
                    "TRPAPT_Id": $scope.TRPAPT_Id,
                    "TRPAPT_PType": $scope.TRPAPT_PType
                }
                apiService.create("MasterServiceStation/saveparttype", data).then(function (promise) {
                    if (promise != null) {
                        if (promise.returnVal == 'saved') {
                            swal("Record Saved Successfully");
                            $state.reload();
                        }
                        else if (promise.returnVal == 'updated') {
                            swal("Record Updated Successfully");
                            $state.reload();
                        }
                        else if (promise.returnVal == 'duplicate') {
                            swal("Record already exist");
                        }
                        else if (promise.returnVal == "savingFailed") {
                            swal("Failed to save record");
                        }
                        else if (promise.returnVal == "failedUpdate") {
                            swal("Failed to update record");
                        }
                        else {
                            swal("Sorry...something went wrong");
                        }
                    }
                    else {

                    }
                    $state.reload();
                })
            } else {
                $scope.submitted = true;
            }
        }

        //--Cancel--//
        $scope.cancle = function () {
            $state.reload();
        }

        $scope.searchValue = '';
       

        //---Edit Data--//
        $scope.edit = function (user) {
            var data = {
                "TRPAPT_Id": user.trpapT_Id
            }

            var id = user.trpapT_Id
            apiService.getURI("MasterServiceStation/Editparttype", id).then(function (Promise) {
                if (Promise != null) {
                    $scope.TRPAPT_PType = Promise.editDataList[0].trpapT_PType;

                    $scope.TRPAPT_Id = Promise.editDataList[0].trpapT_Id;
                }
            })
        }

        //--Active Deactive---//
        $scope.deactive = function (user, SweetAlert) {

            var did = user.trpapT_Id;
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.trpapT_ActiveFlag === true) {
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
                apiService.create("MasterServiceStation/deactivateparttype/", user).
                then(function (promise) {
                    if (promise.returnVal =='exist') {
                        swal('You Can Not Deactivate this Record It Already Mapped');
                    }
                    else {
                        if (promise.retval == true) {
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

        //--Sorting--//     
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

    };
})();


