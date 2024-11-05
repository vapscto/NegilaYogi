//By prashant latest file
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClassWiseDailyAttendanceController', ClassWiseDailyAttendanceController)

    ClassWiseDailyAttendanceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$stateParams', 'Excel', '$timeout']
    function ClassWiseDailyAttendanceController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $stateParams, Excel, $timeout) {

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
            copty = "";
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        } else {
            logopath = "";
        }

        $scope.imgname = logopath;

        $scope.IsHiddenup = true;
        $scope.IsHiddendown = false;
        $scope.pagination = false;
        $scope.currentPage = 1;
        $scope.export_flag = true;
        $scope.Print_flag = false;
        $scope.printdatatable = [];


        //TO  Get The Values in the Grid

        $scope.BindData = function () {
            apiService.getDATA("ClassWiseDailyAttendance/Getdetails").

                then(function (promise) {
                    $scope.newuser1 = promise.yearList;
                    $scope.allAcademicYear1 = promise.defalutYearList;

                    for (var i = 0; i < $scope.newuser1.length; i++) {
                        name = $scope.newuser1[i].asmaY_Id;
                        for (var j = 0; j < $scope.allAcademicYear1.length; j++) {
                            if (name == $scope.allAcademicYear1[j].asmaY_Id) {
                                $scope.newuser1[i].Selected = true;
                                $scope.asmaY_Id = $scope.allAcademicYear1[j].asmaY_Id;
                                //$scope.yearId = $scope.allAcademicYear1[j].asmaY_Id;
                            }
                        }
                    }
                    $scope.newuser2 = promise.classList;
                    // $scope.newuser3 = promise.sectionList;
                    $scope.FromDate = new Date();
                    //  $scope.fromdate = new Date();
                    $scope.maxDatef = new Date(
                        $scope.FromDate.getFullYear(),
                        $scope.FromDate.getMonth(),
                        $scope.FromDate.getDate());
                });
        };

        $scope.setfromdate = function (iddata) {

            var data = {
                "ASMAY_Id": iddata
            };

            apiService.create("ClassWiseDailyAttendance/setfromdate", data).then(function (promise) {
                if (promise !== null) {

                    $scope.newuser2 = promise.classList;

                    for (var k = 0; k < $scope.newuser1.length; k++) {

                        if ($scope.newuser1[k].asmaY_Id == iddata) {
                            var data = $scope.newuser1[k].asmaY_Year;
                        }
                    }
                    if (data != null) {
                        console.log(data);
                        var name, name1;
                        for (var i = 0; i < data.length; i++) {
                            if (i < 4) {
                                if (i == 0) {
                                    name = data[i];
                                } else {
                                    name += data[i];
                                }
                            }
                            if (i > 4) {
                                if (i == 5) {
                                    name1 = data[5];
                                } else {
                                    name1 += data[i];
                                }
                            }
                        }
                        $scope.fromDate = name;
                        $scope.toDate = name1;
                        $scope.frommon = "";
                        $scope.tomon = "";
                        $scope.fromDay = "";
                        $scope.toDay = "";

                        // For Academic From Date
                        $scope.minDatef = new Date(
                            $scope.fromDate,
                            $scope.frommon,
                            $scope.fromDay + 1);

                        $scope.maxDatef = new Date(
                            $scope.toDate,
                            $scope.tomon,
                            $scope.toDay + 365);
                    }
                }
            });
        };


        $scope.getsection = function (asmaY_Id, asmcL_Id) {
            var data = {
                "ASMAY_Id": asmaY_Id,
                "ASMCL_Id": asmcL_Id,
            };
            apiService.create("ClassWiseDailyAttendance/getsection", data).then(function (promise) {
                $scope.newuser3 = promise.sectionList;
            });
        };

        $scope.ShowReport = function (asmaY_Id, FromDate, asmcL_Id, asmC_Id) {
            $scope.printdatatable = [];
            $scope.searchValue = "";

            if ($scope.myform.$valid) {
                var fromdate123 = new Date(FromDate).toDateString();
                var data = {
                    "MI_Id": 2,
                    "ASMAY_Id": asmaY_Id,
                    "ASA_FromDate": fromdate123,
                    "ASMCL_Id": asmcL_Id,
                    "ASMS_Id": asmC_Id
                };

                apiService.create("ClassWiseDailyAttendance/Getdetailsreport", data).
                    then(function (promise) {

                        if (promise.searchstudentDetails.length > 0) {

                            for (var i = 0; i < promise.searchstudentDetails.length; i++) {
                                if (promise.searchstudentDetails[i].ASA_Class_Attended === 1) {
                                    promise.searchstudentDetails[i].ASA_AttendanceFlag = 'Present';
                                }

                                if (promise.searchstudentDetails[i].ASA_Class_Attended === 0.5) {
                                    promise.searchstudentDetails[i].ASA_AttendanceFlag = 'Half Day Present';
                                }
                                if (promise.searchstudentDetails[i].ASA_Class_Attended === 0) {
                                    promise.searchstudentDetails[i].ASA_AttendanceFlag = 'Absent';
                                }
                                $scope.reportdetails = promise.searchstudentDetails;
                                $scope.presentCountgrid = $scope.reportdetails.length;
                            }
                            $scope.presentCount = $filter('filter')($scope.reportdetails, { ASA_Class_Attended: 1 }).length;
                            $scope.Half_Day_Present_Count = $filter('filter')($scope.reportdetails, { ASA_AttendanceFlag: 'Half Day Present' }).length;
                            $scope.AbsentCount = $filter('filter')($scope.reportdetails, { ASA_AttendanceFlag: 'Absent' }).length;

                            $scope.pagination = true;
                            $scope.Print_flag = true;
                            $scope.IsHiddendown = true;
                            $scope.export_flag = false;


                            angular.forEach($scope.newuser1, function (y) {
                                if (parseInt(y.asmaY_Id) === parseInt($scope.asmaY_Id)) {
                                    $scope.yearname = y.asmaY_Year;
                                }
                            });
                            angular.forEach($scope.newuser2, function (y) {
                                if (parseInt(y.asmcL_Id) === parseInt($scope.asmcL_Id)) {
                                    $scope.classname = y.asmcL_ClassName;
                                }
                            });

                            if ($scope.asmC_Id === "0") {
                                $scope.sectionname = "All";
                            } else {
                                angular.forEach($scope.newuser3, function (y) {
                                    if (parseInt(y.asmS_Id) === parseInt($scope.asmC_Id)) {
                                        $scope.sectionname = y.asmC_SectionName;
                                    }
                                });
                            }
                        }
                        else {
                            swal("Records Not Found");
                            $scope.pagination = false;
                            $scope.Print_flag = false;
                            $scope.IsHiddendown = false;
                            $scope.export_flag = true;
                            $state.reload();
                        }
                    });
            }
            else {

                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.exportToExcel = function (tableId) {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        };

        $scope.sendsms = function () {
            if ($scope.printdatatable.length > 0) {

               // var fromdate = new Date($scope.fromdate).toDateString();
                var data = {
                    "absentlist": $scope.printdatatable,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "FromDate": $scope.FromDate
                }
                apiService.create("ClassWiseDailyAttendance/absentsendsms", data).then(function (promise) {
                    if (promise != null) {
                        swal(promise.message);
                        $state.reload();
                    }
                })
            } else {
                swal("Select The Student List To Send The SMS");
            }
        }

        $scope.printData = function () {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        };

        //Toggling all the check box
        $scope.toggleAll = function () {
            $scope.printdatatable = [];
            var toggleStatus = $scope.all2;
            angular.forEach($scope.filterValue, function (itm) {
                itm.selected = toggleStatus;

                if ($scope.all2 == true) {
                    $scope.printdatatable.push(itm);
                    $scope.presentCountgrid = $filter('filter')($scope.printdatatable, { ASA_Class_Attended: 1 }).length;
                    $scope.AbsentCountgrid = $filter('filter')($scope.printdatatable, { ASA_AttendanceFlag: 'Absent' }).length;
                    $scope.Half_Day_Present_Count_grid = $filter('filter')($scope.printdatatable, { ASA_Class_Attended: 0.5 }).length;
                }

                //if ($scope.all2 == true) {
                //    $scope.printdatatable.push(itm);
                //    $scope.presentCountgrid = $filter('filter')($scope.printdatatable, { ASA_Class_Attended: 1 }).length;
                //    $scope.AbsentCountgrid = $filter('filter')($scope.printdatatable, { ASA_AttendanceFlag: 'Absent' }).length;
                //    $scope.Half_Day_Present_Count_grid = $filter('filter')($scope.printdatatable, { ASA_Class_Attended: 0.5 }).length;
                //}
                else {
                    $scope.printdatatable.splice(itm);
                    $scope.presentCountgrid = 0;
                    $scope.AbsentCountgrid = 0;
                    $scope.Half_Day_Present_Count = 0;
                }

            });





        };

        $scope.searchValue = '';
        $scope.filterValue = function () {
            return ($scope.AMST_FirstName + ' ' + $scope.AMST_MiddleName + ' ' + $scope.Amst_LastName).indexOf($scope.searchValue) >= 0 ||
                ($scope.AMST_AdmNo).indexOf($scope.searchValue) >= 0 ||
                ($scope.AMST_RegistrationNo).indexOf($scope.searchValue) >= 0 ||
                ($scope.asmcl_classname).indexOf($scope.searchValue) >= 0
                || ($scope.asmc_sectionname).indexOf($scope.searchValue) >= 0 ||
                ($scope.classes).indexOf($scope.searchValue) >= 0;
        };

        $scope.presentCountgrid = 0; $scope.AbsentCountgrid = 0;

        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all2 = $scope.reportdetails.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
                $scope.presentCountgrid = $filter('filter')($scope.printdatatable, { ASA_Class_Attended: 1 }).length;
                $scope.AbsentCountgrid = $filter('filter')($scope.printdatatable, { ASA_AttendanceFlag: 'Absent' }).length;
                $scope.Half_Day_Present_Count_grid = $filter('filter')($scope.printdatatable, { ASA_Class_Attended: 0.5 }).length;

            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                $scope.presentCountgrid = $filter('filter')($scope.printdatatable, { ASA_Class_Attended: 1 }).length;
                $scope.AbsentCountgrid = $filter('filter')($scope.printdatatable, { ASA_AttendanceFlag: 'Absent' }).length;
                $scope.Half_Day_Present_Count_grid = $filter('filter')($scope.printdatatable, { ASA_Class_Attended: 0.5 }).length;
            }

        };

        $scope.ShowHideup = function () {
            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        };

        $scope.ShowHidedown = function () {
            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        };

        $scope.submitted = false;

        $scope.propertyName = 'AMST_FirstName';
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.ShowHideup = function () {
            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        };

        $scope.clear = function () {
            $scope.asmaY_Id = "";
            $scope.FromDate = "";
            $scope.asmcL_Id = "";
            $scope.asmC_Id = "";
            $scope.reportdetails = "";
            $scope.submitted = false;

            $scope.IsHiddendown = false;
            $scope.export_flag = true;

            $scope.myform.$setPristine();
            $scope.myform.$setUntouched();
        };
    }

})();