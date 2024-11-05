(function () {
    'use strict';

    angular
        .module('app')
        .controller('CLGStatusController', CLGStatusController);

   
    CLGStatusController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', '$compile']
    function CLGStatusController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, $compile) {

        //Get data Initially
        $scope.BindData = function () {
            apiService.getDATA("CLGStatus/Getdetails").
                then(function (promise) {
                    if (promise.allAcademicYear.length > 0) {
                        $scope.allAcademicYear = promise.allAcademicYear;
                        $scope.statuslist = promise.statuslist;
                        $scope.onclickloaddata();
                    }
                })
        };
        $scope.otponclickloaddata = function () {
            $scope.buttonotp = true;
            if ($scope.otptype == 'M') {
                $scope.otpmobile = true;
                $scope.otpemail = false;
            }
            else if ($scope.otptype == 'E') {
                $scope.otpmobile = false;
                $scope.otpemail = true;
            }
        }
        $scope.onclickloaddata = function () {

            $scope.statuslist = [];
            $scope.ASMAY_Id = "";
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.pamsT_Id = "";
            $scope.studentlist = "";
            $scope.showPrintB = false;
            $scope.showExportB = false;
            $scope.search = "";
            //   $scope.Student_status = "786";
            if ($scope.status_type === 'Appsts') {
                $scope.statuslist = [{ pamsT_Id: 787926, pamsT_Status: "APP WAITING" }, { pamsT_Id: 787927, pamsT_Status: "APP REJECTED" }, { pamsT_Id: 787928, pamsT_Status: "APP ACCEPTED" }];
            }
            else if ($scope.status_type === 'admsts') {
                apiService.get("Status/getinitialdata/").then(function (promise) {
                    if (promise != "" && promise.statuslist.length > 0) {
                        $scope.statuslist = promise.statuslist;
                    }
                });
            }
            $scope.IsHidden1 = false;
            $scope.Student_status = "786";
            $scope.set_student_staus();
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };
        $scope.onYearCahnge = function (acdYId) {
            apiService.getURI("CLGStatus/getCourse/", acdYId).then(function (promise) {

                if (promise.courses != null) {
                    $scope.courses = promise.courses;
                }
                else {
                    swal("No Course Is Mapped To Selected Academic Year");
                    $scope.courses = "";
                }
            });
        }
        //checking min and max age details
        $scope.onBranchchange = function (branchId) {

            var selectedData = $filter('filter')($scope.branches, { 'amB_Id': branchId });
            if (branchId != "") {
                var data = {
                    "AMB_Id": branchId,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ACAYCB_Id": selectedData[0].acaycB_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("CLGStatus/getSemester/", data).then(function (promise) {

                    if (promise.message == "MaxCapacity") {
                        swal("Sorry,Branch Capacity is Full");
                    }
                    else {
                        if (promise.semesters != null) {
                            $scope.semesters = promise.semesters;
                        }
                        else {
                            swal("No Semester Is Mapped To Selected Branch");
                            $scope.semesters = "";
                        }
                    }
                })

            }
        }
        $scope.onCourseChange = function (courseId) {

            var selectedData = $filter('filter')($scope.courses, { 'amcO_Id': courseId });
            if (selectedData != "") {
                var data = {
                    "AMCO_Id": courseId,
                    "ASMAY_Id": $scope.ASMAY_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("CLGStatus/getBranch/", data).then(function (promise) {

                    if (promise.branches != null) {
                        $scope.branches = promise.branches;
                        $scope.AMCOC_Id = "";
                        if (promise.studentCategory != null) {
                            $scope.AMCOC_Id = promise.studentCategory[0].amcoC_Id;
                        }
                        else {
                            swal("To get Student Category.Please Map Selected Course to Some category");
                        }
                    }
                    else {
                        swal("No Branch Is Mapped To Selected Course");
                        $scope.branches = "";
                    }
                })
            }
        }

        $scope.set_student_staus = function () {
            if ($scope.Student_status != "786") {
                angular.forEach($scope.studentlist, function (opq) {
                    opq.pamsT_Id = $scope.Student_status;
                    $scope.abc = opq.pamsT_Id;
                })
            }

        }

        //Search Students
        $scope.searchuser = function () {
            $scope.all2 = "";
            $scope.search = "";
            $scope.yearid = $scope.allAcademicYear[0].asmaY_Id;

            if ($scope.myForm.$valid) {
                if ($scope.ASMAY_Id == "") {
                    $scope.ASMAY_Id = 0;
                }
                if ($scope.pamsT_Id == "" || $scope.pamsT_Id == undefined) {
                    $scope.pamsT_Id = 0;
                }
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "PAMST_Id": $scope.pamsT_Id,
                    "status_type": $scope.status_type
                }

                apiService.create("CLGStatus/SearchData", data).then(function (promise) {

                    if (promise.studentlist != null || promise.studentlist != undefined) {

                        $scope.studentlist = promise.studentlist;

                        $scope.albumNameArray1 = [];
                        for (var i = 0; i < $scope.studentlist.length; i++) {
                            if ($scope.studentlist[i].pasR_FirstName != '') {
                                if ($scope.studentlist[i].pasR_MiddleName != null && $scope.studentlist[i].pasR_MiddleName != '' && $scope.studentlist[i].pasR_MiddleName != "") {
                                    if ($scope.studentlist[i].pasR_LastName != null && $scope.studentlist[i].pasR_LastName != '' && $scope.studentlist[i].pasR_LastName != "") {

                                        $scope.albumNameArray1.push({ name: $scope.studentlist[i].pasR_FirstName + " " + $scope.studentlist[i].pasR_MiddleName + " " + $scope.studentlist[i].pasR_LastName, pasR_Id: $scope.studentlist[i].pasR_Id });
                                    }
                                    else {
                                        $scope.albumNameArray1.push({ name: $scope.studentlist[i].pasR_FirstName + " " + $scope.studentlist[i].pasR_MiddleName, pasR_Id: $scope.studentlist[i].pasR_Id });
                                    }
                                }
                                else {
                                    if ($scope.studentlist[i].pasR_LastName != null && $scope.studentlist[i].pasR_LastName != '' && $scope.studentlist[i].pasR_LastName != "") {
                                        $scope.albumNameArray1.push({ name: $scope.studentlist[i].pasR_FirstName + " " + $scope.studentlist[i].pasR_LastName, pasR_Id: $scope.studentlist[i].pasR_Id });
                                    }
                                    else {
                                        $scope.albumNameArray1.push({ name: $scope.studentlist[i].pasR_FirstName, pasR_Id: $scope.studentlist[i].pasR_Id });
                                    }
                                }

                                $scope.studentlist[i].name = $scope.albumNameArray1[i].name;
                            }
                        }

                        $scope.presentCountgrid = promise.studentlist.length;
                        $scope.showPrintB = true;
                        $scope.showExportB = true;
                        if ($scope.pamsT_Id == "0" && $scope.status_type === 'Appsts') {
                            $scope.abc = "787926";
                        }
                        else if ($scope.pamsT_Id == "0" && $scope.status_type === 'admsts') {
                            $scope.abc = "1";
                        }
                        else {
                            $scope.abc = $scope.pamsT_Id;
                        }
                        $scope.IsHidden1 = true;
                        if ($scope.status_type === 'Appsts') {
                            $scope.Student_status = 787926;
                            $scope.set_student_staus();
                            $scope.Student_status = 786;
                            $scope.set_student_staus();
                        }
                    }
                    else {
                        $scope.IsHidden1 = false;
                        $scope.showPrintB = false;
                        $scope.showExportB = false;
                        swal("No records Found");
                    }
                });
                $scope.sort = function (keyname) {
                    $scope.sortKey = keyname;   //set the sortKey to the param passed
                    $scope.reverse = !$scope.reverse; //if true make it false and vice versa
                }
            }
            else {
                $scope.submitted = true;
            }
        }
    }
})();
