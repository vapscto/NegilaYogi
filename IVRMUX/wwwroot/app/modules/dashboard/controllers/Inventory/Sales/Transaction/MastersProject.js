(function () {
    'use strict';
    angular
        .module('app')
        .controller('MastersProjectController', MastersProjectController)

    MastersProjectController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MastersProjectController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;
        $scope.editflag = false;
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }


        //=====================Load--data.............
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";

            var pageid = 2;
            apiService.getURI("MastersProject/getdetails", pageid).then(function (promise) {
                $scope.alldata = promise.alldata;
                $scope.getinstitution = promise.getinstitution;
                $scope.MI_Id = promise.mI_Id;
            })

        }

        $scope.OnChangeInstitution = function () {
            $scope.alldata = [];
            var data = {
                "MI_Id": $scope.MI_Id
            };

            apiService.create("MastersProject/OnChangeInstitution", data).then(function (promise) {
                $scope.alldata = promise.alldata;
            });
        };


        //=====================save--record....
        $scope.saverecord = function () {

            if ($scope.myForm.$valid) {
                var data = {
                    "ISMMPR_Id": $scope.ismmpR_Id,
                    "ISMMPR_ProjectName": $scope.ISMMPR_ProjectName,
                    "ISMMPR_Desc": $scope.ISMMPR_Desc,
                    "ISMMPR_InternalProjectFlg": $scope.ISMMPR_InternalProjectFlg,
                    "MI_Id": $scope.MI_Id
                }
                apiService.create("MastersProject/saverecord", data).then(function (promise) {
                    if (promise.returnval != null && promise.duplicate != null) {
                        if (promise.duplicate == false) {
                            if (promise.returnval == true) {
                                if ($scope.ismmpR_Id > 0) {
                                    swal('Record Updated Successfully!!!');
                                }
                                else {
                                    swal('Record Saved Successfully!!!');
                                }
                            }
                            else {
                                if (promise.returnval == false) {
                                    if ($scope.ismmpR_Id > 0) {
                                        swal('Record Not Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Not Saved Successfully!!!');
                                    }
                                }
                            }
                        }
                        else {
                            swal("Record already exist");
                        }
                        $state.reload();
                    }
                    else {
                        swal("Kindly Contact Administrator!!!");
                    }

                })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.EditData = function (user) {

            $scope.ismmpR_Id = user.ismmpR_Id;
            $scope.ISMMPR_ProjectName = user.ismmpR_ProjectName;
            $scope.ISMMPR_Desc = user.ismmpR_Desc;
            $scope.ISMMPR_InternalProjectFlg = user.ismmpR_InternalProjectFlg
            $scope.MI_Id = user.mI_Id
            $scope.editflag = true;
        };

        $scope.deactiveY = function (user, SweetAlert) {

            $scope.ismmpR_Id = user.ismmpR_Id;

            var dystring = "";
            if (user.ismmpR_ActiveFlg == 1) {
                dystring = "Deactivate";
            }
            else if (user.ismmpR_ActiveFlg == 0) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("MastersProject/deactiveY", user).then(function (promise) {
                            if (promise.returnval == true) {
                                swal("Record " + dystring + "d Successfully!!!");

                            }
                            else {
                                swal("Record Not " + dystring + "d Successfully!!!");

                            }
                            $state.reload();
                        })

                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }

                });
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        //===========----Clear Field
        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();