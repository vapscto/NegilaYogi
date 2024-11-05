(function () {
    'use strict';
    angular
        .module('app')
        .controller('CLGAlumnistudentsearchController', CLGAlumnistudentsearchController)
    CLGAlumnistudentsearchController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter']
    function CLGAlumnistudentsearchController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter) {

        $scope.searchValue = '';
        $scope.ddate = {};
        $scope.ddate = new Date();

        var pageid = 1;
        $scope.loaddata = function () {
            apiService.getURI("CLGAlumniMembership/get_intial_data", pageid).
                then(function (promise) {
                    $scope.yearlst = promise.fillyear;
                })
        }

        $scope.onYearCahnge = function (acdYId) {
            var data = {
                "ASMAY_Id": $scope.obj.ASMAY_Id
            }
            apiService.create("ApplicationForm/getCourse/", data).then(function (promise) {

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
                    "ASMAY_Id": $scope.obj.ASMAY_Id,
                    "ACAYCB_Id": selectedData[0].acaycB_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("ApplicationForm/getSemester/", data).then(function (promise) {

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
                    "ASMAY_Id": $scope.obj.ASMAY_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("ApplicationForm/getBranch/", data).then(function (promise) {

                    if (promise.branches != null) {
                        $scope.branches = promise.branches;
                        $scope.obj.AMCOC_Id = "";
                        if (promise.studentCategory != null) {
                            $scope.obj.AMCOC_Id = promise.studentCategory[0].amcoC_Id;
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
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.grid_flag = false;
        $scope.tadprint = false;
        $scope.items = {};
        $scope.result = [
            {
                "operator": [
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" },
                    { "value": ">", "name": "Greater than" },
                    { "value": "<", "name": "Less than" },
                ],
                "fields": [
                    { "value": "ALCMST_FirstName", "name": "FirstName" },
                    { "value": "ALCMST_RegistrationNo", "name": "RegNo" },
                    { "value": "ALCMST_AdmNo", "name": "AdmissionNo" },
                    { "value": "ALCMST_MobileNo", "name": "MobileNo" },
                    { "value": "ALCMST_Sex", "name": "Sex" },
                    { "value": "ALCMST_Date", "name": "Date" },
                    { "value": "ALCMST_emailId", "name": "Email id" },
                    { "value": "ALCMST_FatherName", "name": "Father Name" },
                    { "value": "ALCMST_MotherName", "name": "Mother Name" },
                    { "value": "StudentName", "name": "Student Name" }
                ],
                "condition": [
                    { "value": "AND", "name": "AND" },
                    { "value": "OR", "name": "OR" },
                ]

            }]
        $scope.addNew = function (index) {
            $scope['condflag' + index] = true;
            $scope.result.push({
                "operator": [
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" },
                    { "value": ">", "name": "Greater than" },
                    { "value": "<", "name": "Less than" },
                ],
                "fields": [
                    { "value": "ALCMST_FirstName", "name": "First Name" },
                    { "value": "ALCMST_RegistrationNo", "name": "Reg.No" },
                    { "value": "ALCMST_AdmNo", "name": "Admission No" },
                    { "value": "ALCMST_AadharNo", "name": "AadharNo" },
                    { "value": "ALCMST_MobileNo", "name": "MobileNo" },
                    { "value": "ALCMST_Sex", "name": "Sex" },
                    { "value": "ALCMST_Date", "name": "Date" },
                    { "value": "ALCMST_emailId", "name": "Email id" },
                    { "value": "ALCMST_FatherName", "name": "Father Name" },
                    { "value": "ALCMST_MotherName", "name": "Mother Name" },
                    { "value": "StudentName", "name": "Student Name" }
                ],
                "condition": [
                    { "value": "AND", "name": "AND" },
                    { "value": "OR", "name": "OR" }
                ]
            });
        };
        $scope.removeRow = function (index) {

            $scope.result.pop();
            $scope['condflag' + (index - 1)] = false;
            if ($scope.items.val != '' && $scope.items.val != null) {
                $scope.items.val[index] = '';
            }
            if ($scope.items.oprtr != '' && $scope.items.oprtr != null) {
                $scope.items.oprtr[index] = '';
            }
            if ($scope.items.field != '' && $scope.items.field != null) {
                $scope.items.field[index] = '';
            }
            if ($scope.items.conditn != '' && $scope.items.conditn != null) {
                $scope.items.conditn[index] = '';
            }
        }
        var abc = "";
        $scope.minall = abc;
        $scope.filterOperator = function (field, index) {
            if (field == "ALCMST_FirstName") {
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" }
                );
            }
            else if (field == "ALCMST_RegistrationNo") {
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" }
                );
            }
            else if (field == "ALCMST_AdmNo") {
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" }
                );
            }
            else if (field == "ALCMST_AadharNo") {
                abc = "12";
                $scope.minall = abc;
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" }
                );
            }
            else if (field == "ALCMST_MobileNo") {
                abc = "10";
                $scope.minall = abc;
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" }
                );
            }

            else if (field == "ALCMST_Sex") {
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" }
                );
            }
            else if (field == "ALCMST_Date") {
                swal("Please Enter Date in 'yyyy-mm-dd' format");
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": "like", "name": "Like" },
                    { "value": ">", "name": "Greater than" },
                    { "value": "<", "name": "Less than" }
                );

            }
            else if (field == "PASR_Age") {
                $scope.result[index].operator.splice(0, 5);
                $scope.result[index].operator.push(
                    { "value": "=", "name": "Equal to" },
                    { "value": "!=", "name": "Not Equal to" },
                    { "value": ">", "name": "Greater than" },
                    { "value": "<", "name": "Less than" }
                );
            }
            if ($scope.items.val != '' && $scope.items.val != null) {
                $scope.items.val[index] = '';
            }
        }

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
        }
        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }
        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        }
        $scope.interacted = function (field) {
            return $scope.submitted ;
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.searchStud = function (inputs) {
            $scope.currentPage = 1;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var output = [];
                for (var key in inputs.field) {
                    var tempObj = {};
                    tempObj[key] = inputs.field[key];
                    output.push(tempObj);
                }

                var output1 = [];
                for (var key in inputs.oprtr) {
                    var tempObj = {};
                    tempObj[key] = inputs.oprtr[key];
                    output1.push(tempObj);
                }

                var output2 = [];
                for (var key in inputs.val) {
                    var tempObj = {};
                    tempObj[key] = inputs.val[key];
                    output2.push(tempObj);
                }

                var output3 = [];
                for (var key in inputs.conditn) {
                    var tempObj = {};
                    tempObj[key] = inputs.conditn[key];
                    output3.push(tempObj);
                }

                var data = {
                    output,
                    output1,
                    output2,
                    output3,
                    "stuStatus": "",
                    "ASMAY_Id": $scope.obj.ASMAY_Id,
                    "AMCO_Id": $scope.obj.AMCO_Id,
                    "AMB_Id": $scope.obj.AMB_Id,
                    "AMSE_Id":$scope.obj.AMSE_Id
                };

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("CLGAlumnistudentsearch/", data).
                    then(function (promise) {
                        if (promise.count == 0) {
                            swal("No Records Found.....!!");
                            $scope.grid_flag = false;
                            $state.reload();
                        }
                        else {
                            $scope.searchResult = promise.searchResult;
                            $scope.presentCountgrid = $scope.searchResult.length;
                            $scope.grid_flag = true;
                        }
                    })
            };
        }

        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.printstudents = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.searchResult, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                else {
                    $scope.printstudents.splice(itm);
                }
            });
        }
        $scope.exportToExcel = function (tableId) {
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select records to be exported");
            }

        }
        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.searchResult.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }
        }

        $("#btnExport").click(function (e) {
            window.open('data:application/vnd.ms-excel,' + $('#Table').html());
            e.preventDefault();
        });
    }
})
    ();