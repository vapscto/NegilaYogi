(function () {
    'use strict';
    angular.module('app').controller('PC_Master_ParticularsController', PC_Master_ParticularsController)

    PC_Master_ParticularsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function PC_Master_ParticularsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;

        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";

            var pageid = 2;
            apiService.getURI("PC_Master_Particulars/onloaddata", pageid).then(function (promise) {
                $scope.getloaddetails = promise.getloaddetails;
            });
        };

        $scope.saverecord = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "PCMPART_Id": $scope.PCMPART_Id,
                    "PCMPART_ParticularName": $scope.PCMPART_ParticularName,
                    "PCMPART_ParticularDesc": $scope.PCMPART_ParticularDesc
                };

                apiService.create("PC_Master_Particulars/saverecord", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message === "Duplicate") {
                            swal("Record Already Exists");
                        } else if (promise.message === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.message === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else if (promise.message === "Error") {
                            swal("Failed To Save/ Update Record");
                        } else {
                            swal("Something Went Wrong Contact Administrator");
                        }
                        $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.EditData = function (user) {
            $scope.PCMPART_Id = user.pcmparT_Id;
            $scope.PCMPART_ParticularName = user.pcmparT_ParticularName;
            $scope.PCMPART_ParticularDesc = user.pcmparT_ParticularDesc;
        };

        $scope.deactiveY = function (user, SweetAlert) {
            

            var data = {
                "PCMPART_Id": user.pcmparT_Id
            };

            var dystring = "";
            if (user.pcmparT_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (user.pcmparT_ActiveFlg === false) {
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
                        apiService.create("PC_Master_Particulars/deactiveY", data).then(function (promise) {
                            if (promise.message === "Mapped") {
                                swal("Record Is Already Mapped, So You Can Not Deactive The Record");
                            } else {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            }
                        });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }
                });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();

