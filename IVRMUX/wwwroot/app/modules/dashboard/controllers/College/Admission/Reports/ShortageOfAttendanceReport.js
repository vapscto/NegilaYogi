(function () {
    'use strict';
    angular.module('app').controller('ShortageOfAttedanceReportController', ShortageOfAttedanceReportController)
    ShortageOfAttedanceReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function ShortageOfAttedanceReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {

        $scope.report = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length != 0 && ivrmcofigsettings.length != null && ivrmcofigsettings.length != undefined) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length !== null && admfigsettings.length !== undefined && admfigsettings.length !== 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        } else {
            var logopath = "";
        }
        $scope.imgname = logopath;

        $scope.obj = {};

        $scope.amsT_Date = new Date();

        $scope.fromDate = new Date();
        $scope.toDate = new Date();
        $scope.frommon = "";
        $scope.tomon = "";
        $scope.fromDay = "";
        $scope.toDay = "";

        $scope.maxDatet = new Date();

        $scope.amsT_fromDate = new Date();
        $scope.maxDatefromt = new Date();

        $scope.amsT_ToDate = new Date();
        $scope.maxDatetot = new Date();

        $scope.exportToExcel = function (export_id) {

            if ($scope.AttendanceType == 0) {
                $scope.datedisplay = "Date : " + $filter('date')($scope.amsT_Date, 'dd/MM/yyyy');
            }
            else if ($scope.AttendanceType == 1) {
                $scope.datedisplay = "From Date : " + $filter('date')($scope.amsT_fromDate, 'dd/MM/yyyy') + "   To Date : " + $filter('date')($scope.amsT_ToDate, 'dd/MM/yyyy');
            } else {
                $scope.datedisplay = '';
            }


            var excelname = "Subject Wise Attendance Percentage Report " + $scope.datedisplay + ".xls";
            var exportHref = Excel.tableToExcel(export_id, 'Subject Wise Attendance Percentage Report' + $scope.datedisplay);
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);

            //  $timeout(function () { location.href = exportHref; }, 100);
        };

        $scope.all_check = function () {
            var toggleStatus = $scope.detail_checked;
            angular.forEach($scope.branchlist, function (itm) {
                itm.ambid = toggleStatus;
            });

            $scope.onselectBranch(1, 2, 3);
        };


        $scope.isOptionsRequired = function () {
            return !$scope.branchlist.some(function (options) {
                return options.ambid;
            });
        };

        $scope.albumNameArraycolumn = [];
        $scope.columnsTest1 = [];

        $scope.addColumn2 = function (role1) {

            $scope.detail_checked = $scope.branchlist.every(function (itm) { return itm.selected; });
            if (role1.selected == true) {
                $scope.albumNameArraycolumn.push(role1);
                var newCol = { AMB_Id: role1.amB_Id, checked: true, AMB_BranchName: role1.amB_BranchName }
                $scope.columnsTest1.push(newCol);
            }
            else {
                var som = $scope.albumNameArraycolumn.indexOf(role1);
                $scope.columnsTest1.splice($scope.albumNameArraycolumn.indexOf(role1), 1);
                $scope.albumNameArraycolumn.splice($scope.albumNameArraycolumn.indexOf(role1), 1);
            }

            $scope.onselectBranch(1, 2, 3);
        };


        $scope.BindData = function () {
            apiService.getDATA("CollegeDailyAttendance/getdetails").then(function (promise) {

                $scope.acdlist = promise.acdlist;
                // $scope.subjectlist = promise.subjectlist;
                //  $scope.seclist = promise.seclist;
                $scope.monthlist = promise.monthlist;
                // $scope.semlist = promise.semlist;

            });
        };

        $scope.onselectAcdYear = function (ASMAY_Id) {

            $scope.AMCO_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.ivrM_Month_Id = "";
            $scope.catreport = true;
            $scope.detail_checked = false;
            $scope.obj = {};
            $scope.subjectlist = [];
            $scope.branchlist = [];
            $scope.seclist = [];
            $scope.semlist = [];
            $scope.seclist23 = [];
            $scope.seclist234 = [];

            var data = {
                "ASMAY_Id": ASMAY_Id
            };
            apiService.create("CollegeDailyAttendance/onselectAcdYear", data).then(function (promise) {
                $scope.courselist = promise.courselist;

            });
        };

        $scope.onselectCourse = function (ASMAY_Id, AMCO_Id) {

            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.ivrM_Month_Id = "";
            $scope.catreport = true;
            $scope.detail_checked = false;
            $scope.seclist = [];
            $scope.semlist = [];
            $scope.seclist23 = [];
            $scope.obj = {};
            $scope.subjectlist = [];
            $scope.branchlist = [];
            $scope.seclist234 = [];

            var data = {
                "AMCO_Id": AMCO_Id,
                "ASMAY_Id": ASMAY_Id
            };
            apiService.create("CollegeDailyAttendance/onselectCourse", data).then(function (promise) {
                $scope.branchlist = promise.branchlist;
            });
        };

        $scope.onselectBranch = function (ASMAY_Id, AMCO_Id, AMB_Id) {

            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.ivrM_Month_Id = "";
            $scope.catreport = true;
            $scope.obj = {};
            $scope.subjectlist = [];
            $scope.albumNameArraycolumn1 = [];
            $scope.seclist = [];
            $scope.semlist = [];
            $scope.seclist23 = [];
            $scope.seclist234 = [];

            angular.forEach($scope.branchlist, function (role) {
                if (!!role.ambid) $scope.albumNameArraycolumn1.push(role);
            });

            if ($scope.albumNameArraycolumn1.length > 0) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "Temp_branchDTO": $scope.albumNameArraycolumn1
                };

                apiService.create("CollegeDailyAttendance/onselectBranch", data).then(function (promise) {
                    $scope.semlist = promise.semlist;
                });
            }

        };

        $scope.getsection = function () {

            $scope.ACMS_Id = "";
            $scope.obj = {};
            $scope.ivrM_Month_Id = "";
            $scope.albumNameArraycolumn3 = [];
            $scope.obj = {};
            $scope.subjectlist = [];
            $scope.seclist234 = [];

            angular.forEach($scope.branchlist, function (role) {
                if (!!role.ambid) $scope.albumNameArraycolumn3.push(role);
            });

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "Temp_branchDTO": $scope.albumNameArraycolumn3,
                "AMSE_Id": $scope.AMSE_Id
            };

            apiService.create("CollegeDailyAttendance/getsection", data).then(function (promise) {

                $scope.seclist23 = promise.seclist;
                if ($scope.seclist23.length > 0) {
                    $scope.seclist = promise.seclist;
                } else {
                    swal("No Section Is Found For Selected List");
                }

            });
        };

        $scope.getsubject = function () {

            $scope.albumNameArraycolumn3 = [];

            $scope.obj = {};
            $scope.subjectlist = [];

            angular.forEach($scope.branchlist, function (role) {
                if (!!role.ambid) $scope.albumNameArraycolumn3.push(role);
            });

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACMS_Id": $scope.ACMS_Id,
                "Temp_branchDTO": $scope.albumNameArraycolumn3
            };

            apiService.create("CollegeDailyAttendance/getsubject", data).then(function (promise) {

                $scope.seclist234 = promise.subjectlist;
                if ($scope.seclist234.length > 0) {
                    $scope.subjectlist = promise.subjectlist;
                } else {
                    swal("No Section Is Found For Selected List");
                }

            });
        };

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.searchValue = '';

        $scope.students = [];
        $scope.catreport = true;
        $scope.submitted = false;

        $scope.onreport = function (obj) {
            $scope.all = false;
            if ($scope.myForm.$valid) {

                if ($scope.per == 0) {
                    swal('Please Enter Percentage More Than zero');
                    return;
                }

                var per = 0;
                if ($scope.per == undefined) {
                    per = 0;
                } else {
                    per = $scope.per;
                }

                angular.forEach($scope.branchlist, function (role) {
                    if (!!role.ambid) $scope.albumNameArraycolumn.push(role);
                });

                if ($scope.AttendanceType == 0) {
                    $scope.fromdate = new Date($scope.amsT_Date).toDateString();
                    $scope.todate = new Date($scope.amsT_Date).toDateString();
                } else if ($scope.AttendanceType == 1) {
                    $scope.fromdate = new Date($scope.amsT_fromDate).toDateString();
                    $scope.todate = new Date($scope.amsT_ToDate).toDateString();
                } else {
                    $scope.fromdate = new Date().toDateString();
                    $scope.todate = new Date().toDateString();
                }


                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "ISMS_Id": obj.ismS_Id.ismS_Id,
                    "Temp_branchDTO": $scope.albumNameArraycolumn,
                    "FromDate": $scope.fromdate,
                    "ToDate": $scope.todate,
                    "Flag": $scope.AttendanceType,
                    "shortage": per
                };

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("CollegeDailyAttendance/onreportshortagepercentage", data).then(function (promise) {


                    $scope.students1 = promise.studentreport;
                    $scope.presentCountgrid = $scope.students1.length;

                    if ($scope.students1 !== null && $scope.students1 !== undefined && $scope.students1.length > 0) {

                        if ($scope.AttendanceType == 0) {
                            $scope.datedisplay = "Date : " + $filter('date')($scope.amsT_Date, 'dd/MM/yyyy');
                        }
                        else if ($scope.AttendanceType == 1) {
                            $scope.datedisplay = "From Date : " + $filter('date')($scope.amsT_fromDate, 'dd/MM/yyyy') + "   To Date : " + $filter('date')($scope.amsT_ToDate, 'dd/MM/yyyy');
                        } else {
                            $scope.datedisplay = '';
                        }

                        $scope.subjectname = $scope.students1[0].subjectname;
                        $scope.coursename = $scope.students1[0].coursename;
                        $scope.branchname = $scope.students1[0].branchname;
                        $scope.semestername = $scope.students1[0].semestername;

                        if (parseInt($scope.ACMS_Id) > 0) {
                            $scope.sectionname = $scope.students1[0].sectionname;
                        } else {
                            $scope.sectionname = "All";
                        }

                        $scope.printdatatabledate = $scope.students1;
                        $scope.catreport = false;
                        $scope.exp_excel_flag = false;
                        $scope.print_flag = false;
                        $scope.report = true;

                    }
                    else {
                        swal("No Records Found!");
                        $scope.catreport = true;
                        $scope.report = false;                       
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.cancel = function () {
            $state.reload();
        };


        $scope.printData = function () {

            var innerContents = document.getElementById("printSectionId1").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };



        $scope.printstudents = [];
        $scope.toggleAll = function () {

            var toggleStatus = $scope.all;
            angular.forEach($scope.students, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all === true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                else {
                    $scope.printstudents.splice(itm);
                }
            });
        };

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }
        };

    }

})();