(function () {
    'use strict';
    angular.module('app').controller('AssignVisitorsToEmpController', AssignVisitorsToEmpController)
    AssignVisitorsToEmpController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache']
    function AssignVisitorsToEmpController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache) {

        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.AMVM_Id = 0;
        $scope.obj1 = {};
        $scope.obsupdate = {};
        $scope.obj1.VMVTMT_MetFlg = false;
        $scope.obj1.SMS_Required = true;
        $scope.obj1.Email_Required = true;

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };      

       
        $scope.radiomodal = 'int_dept';

        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };


        $scope.loadgrid = function () {
            $scope.vis_list = [];
            $scope.gridoptions = [];
            $scope.radiomodal = 'int_dept';

            var pageid = 2;

            apiService.getURI("AddVisitors/getAssignDetails/", pageid).then(function (promise) {                
                $scope.institutionlist = promise.institutionlist;
                $scope.emplist = promise.emplist;
                $scope.visitorlist = promise.visitorlist;
                if ($scope.visitorlist === null || $scope.visitorlist.length === 0) {
                    swal("Visitors Detailes Not Found");
                }

                $scope.assigned_visitorlist = promise.assigned_visitorlist;
            });
        };

        $scope.getVisitorDetails = function (bjs) {
            var data = {
                "VMMV_Id": bjs.vmmV_Id               
            };
            apiService.create("AddVisitors/getVisitorAssignDetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.gridoptions = promise.gridoptions;

                    $scope.VMMV_VisitorName = $scope.gridoptions[0].VMMV_VisitorName;
                    $scope.VMMV_FromPlace = $scope.gridoptions[0].VMMV_FromPlace;
                    $scope.VMMV_MeetingPurpose = $scope.gridoptions[0].VMMV_MeetingPurpose;
                    $scope.VMMV_MeetingDateTime = $scope.gridoptions[0].VMMV_MeetingDateTime;
                    $scope.VMMV_EntryDateTime = $scope.gridoptions[0].VMMV_EntryDateTime;
                    if ($scope.gridoptions[0].VMVTMT_DateTime !== null) {
                        $scope.VMVTMT_DateTime = new Date($scope.gridoptions[0].VMVTMT_DateTime);
                    }
                    
                    $scope.TOMEET = $scope.gridoptions[0].TOMEET;
                    $scope.ASSIGNEDBY = $scope.gridoptions[0].ASSIGNEDBY;
                    $scope.VMMV_Id = $scope.gridoptions[0].VMMV_Id;
                    $scope.VMVTMT_Id = $scope.gridoptions[0].VMVTMT_Id;
                }
            });
        };

        $scope.saveData = function (obj1s) {
            var obj = {};
            if ($scope.myForm.$valid) {
                var tomeethrmeid = 0;
                tomeethrmeid = obj1s.ToMeetHRMEId.hrmE_Id;
                

                if (obj1s.VMMV_EntryDateTimed === null || obj1s.VMMV_EntryDateTimed === "") {
                    swal("Select Meet Time");
                    return;
                }

                var ScheduleTime = $filter('date')(obj1s.VMMV_EntryDateTimed, "HH");
                var ScheduleMin = $filter('date')(obj1s.VMMV_EntryDateTimed, "HH");

                obj = {
                    "VMMV_Id": obj1s.VMMV_Id.vmmV_Id,
                    "VMVTMT_Id": $scope.VMVTMT_Id,
                    "VMVTMT_MetFlg": obj1s.VMVTMT_MetFlg,
                    "VMVTMT_Remarks": obj1s.VMVTMT_Remarks,
                    "VMVTMT_Location": obj1s.VMVTMT_Location,
                    "VMVTMT_DateTime": new Date(obj1s.VMVTMT_DateTime).toDateString(),
                    "VMVTMT_ToMeet_HRME_Id": tomeethrmeid,                    
                    "fhrors": ScheduleTime,
                    "fminutes": ScheduleMin,
                    "SMS_Required": obj1s.SMS_Required,
                    "Email_Required": obj1s.Email_Required,
                   
                };


                apiService.create("AddVisitors/saveAssignedData", obj).then(function (promise) {
                    if (promise.returnVal === 'saved') {
                        swal("Record Saved Successfully by sending SMS and Email");
                        $state.reload();

                    }
                    else if (promise.returnVal === 'updated') {
                        swal("Record Updated Successfully");
                        $state.reload();
                    }
                    else if (promise.returnVal === 'duplicate') {
                        swal("Record already exist");
                    }
                    else if (promise.returnVal === "savingFailed") {
                        swal("Failed to save record");
                    }
                    else if (promise.returnVal === "updateFailed") {
                        swal("Failed to update record");
                    }
                    else {
                        swal("Sorry...something went wrong");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        $scope.screport = false;
        $scope.submitted = false;

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
    }
})();