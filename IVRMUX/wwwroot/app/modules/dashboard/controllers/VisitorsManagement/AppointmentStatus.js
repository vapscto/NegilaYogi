(function () {
    'use strict';
    angular.module('app').controller('AppointmentStatusController', AppointmentStatusController)
    AppointmentStatusController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache']
    function AppointmentStatusController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache) {
                
        $scope.searchValue = "";
        $scope.VMMV_CkeckedInOutStatus = "Checked Out";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.SPCCESTR_Id = 0;
        $scope.maxDatemft = new Date();
        $scope.VMMV_ExitDate = new Date();

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        $scope.loadgrid = function () {
            apiService.getURI("AppointmentStatus/getDetails/", 1).then(function (promise) {
                if (promise.visitorlist.length > 0) {
                    $scope.VisitorList = promise.visitorlist;
                }
                $scope.cancel();
            });
        }

        $scope.onselectname = function () {
            var obj = {
                "VMMV_Id": $scope.vmmV_Id,
            }
       
            apiService.create("AppointmentStatus/Edit/", obj).then(function (promise) {
               
                $scope.VMMV_Id = promise.editDetails[0].vmmV_Id;
                $scope.empname = promise.editDetails[0].empname;
                $scope.VMMV_MeetingDateTime = new Date(promise.editDetails[0].vmmV_MeetingDateTime);
                $scope.minDatemft = new Date(promise.editDetails[0].vmmV_MeetingDateTime);
                $scope.VMMV_EntryDateTime = moment(promise.editDetails[0].vmmV_EntryDateTime, 'HH:mm').format();
                $scope.VMMV_Remarks = promise.editDetails[0].vmmV_Remarks;
            });
        }

        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.submitted = false;
        $scope.saveData = function () {
            debugger;
            if ($scope.myForm.$valid) {

                var obj = {
                    "VMMV_Id": $scope.VMMV_Id,                    
                    "VMMV_CkeckedInOutStatus": $scope.VMMV_CkeckedInOutStatus,
                    "VMMV_ExitDate": new Date($scope.VMMV_ExitDate).toDateString(),
                    "VMMV_ExitDateTime": $filter('date')($scope.VMMV_ExitDateTime, "HH:mm"),                    
                }
                apiService.create("AppointmentStatus/saveData", obj).then(function (promise) {
                        if (promise.returnVal == 'saved') {
                            swal("Record Saved Successfully");
                            $scope.reload();
                        }
                        else if (promise.returnVal == 'updated') {
                            swal("SMS & Mail Send Successfully");
                            $scope.reload();
                        }
                        else if (promise.returnVal == 'duplicate') {
                            swal("Record already exist");
                        }
                        else if (promise.returnVal == "savingFailed") {
                            swal("Failed to save record");
                        }
                        else if (promise.returnVal == "updateFailed") {
                            swal("Failed to update record");
                        }
                        else {
                            swal("Sorry...something went wrong");
                        }
                        $scope.reload();
                    });
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.cancel = function () {           
            $scope.vmmV_Id = "";
            $scope.empname = "";
            $scope.VMMV_MeetingDateTime = "";
            $scope.VMMV_CkeckedInOutStatus = "Checked Out";
            $scope.VMMV_EntryDateTime = "";
            $scope.VMMV_ExitDateTime = "";
            $scope.VMMV_Remarks = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


/////////////////////////////////=============================================Old Code.............................////////////////



//$scope.searchValue = "";
        //$scope.currentPage = 1;
        //$scope.itemsPerPage = 10;
        //$scope.SPCCESTR_Id = 0;
        //$scope.sort = function (key) {
        //    $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
        //    $scope.sortKey = key;
        //}

        //$scope.loadgrid = function () {
        //    apiService.getURI("AppointmentStatus/getDetails/", 1).then(function (promise) {

        //        if (promise.visitorlist.length > 0) {
        //            $scope.VisitorList = promise.visitorlist;
        //        }

        //        $scope.cancel();
        //    });
        //}

        //$scope.onselectname = function (data) {

        //    $scope.AMVM_Id = data;
        //    apiService.getURI("AppointmentStatus/Edit/", $scope.AMVM_Id).then(function (promise) {
        //        $scope.editDetails = promise.editDetails;
        //        $scope.empname = promise.editDetails[0].empname;
        //        $scope.Date_Visit = new Date(promise.editDetails[0].date_Visit);
        //        $scope.Time_Visit = moment(promise.editDetails[0].time_Visit, 'HH:mm').format();
        //        $scope.AMVM_Remarks = promise.editDetails[0].amvM_Remarks;
        //    });
        //}

        //$scope.hstep = 1;
        //$scope.mstep = 1;


        //$scope.submitted = false;
        //$scope.saveData = function () {
        //    debugger;
        //    if ($scope.myForm.$valid) {

        //        var obj = {
        //            "AMVM_Id": $scope.AMVM_Id,
        //            "AMVM_Name": $scope.empname,
        //            "AMVM_Status": $scope.AMVM_Status,
        //            "AMVM_Out_Time": $filter('date')($scope.AMVM_Out_Time, "HH:mm")
        //        }
        //        apiService.create("AppointmentStatus/saveData", obj).
        //            then(function (promise) {
                       
        //                if (promise.returnVal == 'saved') {
        //                    swal("Record Saved Successfully");
                           
        //                    $scope.loadgrid();
        //                }
        //                else if (promise.returnVal == 'updated') {
        //                    swal("Record Updated Successfully");
                            
        //                    $scope.loadgrid();
        //                }
        //                else if (promise.returnVal == 'duplicate') {
        //                    swal("Record already exist");
        //                }
        //                else if (promise.returnVal == "savingFailed") {
        //                    swal("Failed to save record");
        //                }
        //                else if (promise.returnVal == "updateFailed") {
        //                    swal("Failed to update record");
        //                }
        //                else {
        //                    swal("Sorry...something went wrong");
        //                }
        //                $scope.loadgrid();
        //            });
        //    }
        //    else {
        //        $scope.submitted = true;
        //    }
        //}

        //$scope.cancel = function () {
        //    $scope.AMVM_Id = 0;
        //    $scope.empname = "";
        //    $scope.AMVM_To_Meet = "";
        //    $scope.Date_Visit = "";
        //    $scope.Time_Visit = "";
        //    $scope.AMVM_Out_Time = "";
        //    $scope.AMVM_Status = "";
        //    $scope.AMVM_Remarks = "";

        //    $scope.submitted = false;
        //    $scope.myForm.$setPristine();
        //    $scope.myForm.$setUntouched();

        //};


        //$scope.interacted = function (field) {
        //    return $scope.submitted;
        //};
    }
})();