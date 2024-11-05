(function () {
    'use strict';
    angular
        .module('app')
        .controller('Hostel_Student_GatePassController', Hostel_Student_GatePassController)
    Hostel_Student_GatePassController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function Hostel_Student_GatePassController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {

     
        $scope.editflag = false;
        $scope.hlhstgP_GoingOutDate = new Date();
        $scope.hlhstgP_ComingBackDate = new Date();
        $scope.hstep = 1;
        $scope.mstep = 1;
      
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        // Get form Details at onload 
        $scope.onLoadGetData = function () {

            var pageid = 2;
            apiService.getURI("Hostel_Student_GatePass/getalldetails", pageid).then(function (promise) {
               
                    $scope.gridOptions = promise.gridlistdata;
            })
        }


        // clear form data
        $scope.cancel = function () {
            $state.reload();

        }
        //saving/updating Record
        $scope.submitted = false;
        $scope.saveData = function () {           
            $scope.submitted = true;
            if ($scope.myForm.$valid) {  
                var data = {
                    "HLHSTGP_Id": $scope.hlhstgP_Id,
                    "HLHSTGP_TypeFlg": $scope.hlhstgP_TypeFlg,
                    "HLHSTGP_GoingOutDate": $scope.hlhstgP_GoingOutDate,
                    "HLHSTGP_ComingBackDate": $scope.hlhstgP_ComingBackDate,
                    "HLHSTGP_GoingOutTime": $filter('date')($scope.hlhstgP_GoingOutTime, "HH:mm"),
                    "HLHSTGP_Reason": $scope.hlhstgP_Reason,                  
                }
                apiService.create("Hostel_Student_GatePass/save", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {

                            if (promise.retrunMsg == "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }

                            else if (promise.retrunMsg == "false") {
                                swal("Record Not saved / Updated..", 'Fail');

                            }
                            else if (promise.retrunMsg == "Add") {
                                swal("Record Saved Successfully..");
                                $state.reload();
                            }
                            else if (promise.retrunMsg == "Update") {
                                swal("Record Updated Successfully..");
                                $state.reload();
                            }                          
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                                return;
                            }
                            $scope.cancel();
                        }
                    })
            }
        };

        $scope.Edit = function (item) {
            var data = {
                "HLHSTGP_Id": item.HLHSTGP_Id,
            };

            apiService.create("Hostel_Student_GatePass/Edit", data).then(function (promise) {
                if (promise !== null) {
                    $scope.editdata = promise.editdata;
                    $scope.editflag = true;
                    
                    $scope.hlhstgP_TypeFlg = promise.editdata[0].hlhstgP_TypeFlg;
                    $scope.hlhstgP_Reason = promise.editdata[0].hlhstgP_Reason;  
                    $scope.hlhstgP_GoingOutDate = promise.editdata[0].hlhstgP_GoingOutDate;
                    $scope.hlhstgP_ComingBackDate = promise.editdata[0].hlhstgP_ComingBackDate;
                    $scope.hlhstgP_GoingOutTime = promise.editdata[0].hlhstgP_GoingOutTime;
                                 
                }
            })
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.obj = {};

        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.HLHSTGP_ActiveFlg == false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Record..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Hostel_Student_GatePass/ActiveDeactiveRecord", data).then(function (promise) {
                                if (promise.retrunMsg !== "") {
                                    if (promise.retrunMsg === "Activated") {
                                        swal("Record Activated successfully");
                                        $state.reload();
                                    }
                                    else if (promise.retrunMsg === "Deactivated") {
                                        swal("Record Deactivated successfully");
                                        $state.reload();
                                    }
                                    else {
                                        swal("Record Not Activated/Deactivated", 'Fail');
                                    }                                   
                                }

                            })
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }

            );
        }
    }
})();