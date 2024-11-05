(function () {
    'use strict';
    angular
.module('app')
        .controller('AdvancedHolidayMasterController', AdvancedHolidayMasterController)

    AdvancedHolidayMasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache','$filter']
    function AdvancedHolidayMasterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $filter) {

        $scope.advloaddata = function () {
            $scope.page2 = "page2";
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 5;
            var pageid = 1;
            $scope.printstudents = [];
            apiService.getURI("MasterHoliday/advloaddata", pageid).then(function (promise) {
                $scope.days_list = promise.dayslist;

                if (promise.gridviewdetails != undefined && promise.gridviewdetails.length > 0) {
                    angular.forEach(promise.gridviewdetails, function (itm) {
                        if (itm.fomeH_Date != null) {
                            itm.fomeH_Date = new Date(promise.gridviewdetails[0].fomeH_Date);
                        }
                        else {
                            itm.fomeH_Date = null;
                        }
                    });
                }
                $scope.gridviewDetails = promise.gridviewdetails;
                console.log($scope.gridviewDetails);

                if (promise.employeelist != null) {
                    angular.forEach(promise.employeelist, function (itm) {
                        if (itm.hrmE_EmployeeMiddleName == null) { itm.hrmE_EmployeeMiddleName = ""; }
                        if (itm.hrmE_EmployeeLastName == null) { itm.hrmE_EmployeeLastName = ""; }
                    });
                }
                $scope.employeelist = promise.employeelist;

            });
        };

        $scope.selectedDaysDate = [];
        $scope.saveadvmasterHolidaydata = function () {
            if ($scope.myForm.$valid) {

                if ($scope.allind == 'Date') { $scope.fomD_DayName = ""; }
                else if ($scope.allind == 'Day') { $scope.fomeH_Date = ""; }

                if ($scope.fomeH_Date != undefined && $scope.fomeH_Date != "") {
                    $scope.fomeH_Date = new Date($scope.fomeH_Date).toDateString();
                }
                else {
                    $scope.fomeH_Date = "";
                }

                if ($scope.fomD_DayName == "SUN") { $scope.fomD_DayName = "Sunday"; }
                else if ($scope.fomD_DayName == "MON") { $scope.fomD_DayName = "Monday"; }
                else if ($scope.fomD_DayName == "TUE") { $scope.fomD_DayName = "Tuesday"; }
                else if ($scope.fomD_DayName == "WED") { $scope.fomD_DayName = "Wednesday"; }
                else if ($scope.fomD_DayName == "THU") { $scope.fomD_DayName = "Thursday"; }
                else if ($scope.fomD_DayName == "FRI") { $scope.fomD_DayName = "Friday"; }
                else if ($scope.fomD_DayName == "SAT") { $scope.fomD_DayName = "Saturday"; }

                var data = {
                    "HRME_Id": $scope.hrmE_Id,
                    "FOMEH_Date": $scope.fomeH_Date,
                    "FOMEH_Day": $scope.fomD_DayName
                };

                apiService.create("MasterHoliday/saveadvmasterHolidaydata", data).then(function (promise) {
                    $scope.gridviewDetails = promise.gridviewdetails;
                    if (promise.message == "Duplicate") {
                        swal('Record Already Exists !!');
                        $state.reload();
                    }
                    else if (promise.message === "Update") {
                        swal('Record Updated successfully.');
                        $state.reload();
                    }
                    else if (promise.message === "Add") {
                        swal('Record Added successfully.');
                        $state.reload();
                    }
                    else {
                        swal('Failed to save, please contact administrator.');
                    }                    
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.clear1 = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Deletedata = function (del_id) {
            swal({
                title: "Are you sure?",
                text: "Do you want to Delete This Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: true,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.getURI("MasterHoliday/advdelete", del_id.fomeH_Id).
                        then(function (promise) {
                            if (promise.returnval == true) {
                                swal("Record Deleted Successfully");
                                $state.reload();
                            }
                            else {
                                swal("Failed to Delete, please contact administrator.");
                            }
                        });
                }
                else {
                    swal("Cancelled");
                }
            });
        };

        $scope.Editdata = function (edt_id) {
            var data = {
                "FOMEH_Id": edt_id.fomeH_Id
            };

            apiService.create("MasterHoliday/editadvmasterHoliday", data).then(function (promise) {
                if (promise.report_list[0].fomeH_Day != null && promise.report_list[0].fomeH_Day != "") {
                    $scope.allind = 'Day';
                    if (promise.report_list[0].fomeH_Day == "Sunday") { $scope.fomD_DayName = "SUN"; }
                    if (promise.report_list[0].fomeH_Day == "Monday") { $scope.fomD_DayName = "MON"; }
                    if (promise.report_list[0].fomeH_Day == "Tuesday") { $scope.fomD_DayName = "TUE"; }
                    if (promise.report_list[0].fomeH_Day == "Wednesday") { $scope.fomD_DayName = "WED"; }
                    if (promise.report_list[0].fomeH_Day == "Thursday") { $scope.fomD_DayName = "THU"; }
                    if (promise.report_list[0].fomeH_Day == "Friday") { $scope.fomD_DayName = "FRI"; }
                    if (promise.report_list[0].fomeH_Day == "Saturday") { $scope.fomD_DayName = "SAT"; }
                }
            
                $scope.hrmE_Id = promise.report_list[0].hrmE_Id;
                if (promise.report_list[0].fomeH_Date != null) {
                    $scope.allind = 'Date';
                    $scope.fomeH_Date = new Date(promise.report_list[0].fomeH_Date);
                }
                else {
                    $scope.fomeH_Date = null;
                }

                });
        };

        $scope.search = '';
        $scope.filterValue = function (obj) {
            return angular.lowercase(obj.hrmE_EmployeeFirstName).indexOf(angular.lowercase($scope.search)) >= 0 ||
                ($filter('date')(obj.fomeH_Date, 'dd/MM/yyyy').indexOf($scope.search) >= 0) ||
                (obj.fomeH_Day).indexOf(angular.lowercase($scope.search)) >= 0;
        };

        $scope.sort1 = function (keyname) {
            $scope.sortKey1 = keyname;   //set the sortKey to the param passed
            $scope.reverse1 = !$scope.reverse; //if true make it false and vice versa
        };
    }
})();


















