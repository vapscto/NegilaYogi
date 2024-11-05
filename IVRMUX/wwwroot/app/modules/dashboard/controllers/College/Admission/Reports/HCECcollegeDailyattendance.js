(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegeDailyAttendanceController', CollegeDailyAttendanceController)

    CollegeDailyAttendanceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function CollegeDailyAttendanceController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

        $scope.report = false;
        $scope.maxDatemf = new Date();
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length !== 0 && ivrmcofigsettings.length !== null && ivrmcofigsettings.length !== undefined) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length !== 0 && admfigsettings.length !== null && admfigsettings.length !== undefined) {
            logopath = admfigsettings[0].asC_Logo_Path;
        } else {
            logopath = "";
        }

        $scope.imgname = logopath;

        $scope.obj = {};
        $scope.searchValue = '';
        $scope.students = [];
        $scope.catreport = true;
        $scope.submitted = false;

        $scope.gettodate = function () {

            $scope.minDatemf = new Date($scope.fromdate);
            $scope.maxDatemf = new Date();
        };
        $scope.fromdate = new Date();
        $scope.todate = new Date();
        $scope.BindData = function () {
            apiService.getDATA("CollegeDailyAttendance/getdetails").
                then(function (promise) {
                    $scope.acdlist = promise.acdlist;
                    $scope.subjectlist = promise.subjectlist;
                    $scope.seclist = promise.seclist;
                  $scope.monthlist = promise.monthlist;
                  $scope.semlist = promise.semlist;

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

            var data = {
                "ASMAY_Id": ASMAY_Id
            };

            apiService.create("CollegeDailyAttendance/onselectAcdYear", data).
                then(function (promise) {

                    $scope.courselist1234 = promise.courselist;
                    if ($scope.courselist1234.length > 0) {
                        $scope.courselist = promise.courselist;
                    } else {
                        swal("No Course Is Mapped For The Selected Year");
                    }
                });
        };

        $scope.onselectCourse = function (ASMAY_Id, AMCO_Id) {

            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.ivrM_Month_Id = "";
            $scope.catreport = true;
            $scope.detail_checked = false;

            $scope.obj = {};
            $scope.subjectlist = [];

            $scope.branchlist = [];

            var data = {
                "AMCO_Id": AMCO_Id,
                "ASMAY_Id": ASMAY_Id
            };

            apiService.create("CollegeDailyAttendance/onselectCourse", data).
                then(function (promise) {

                    $scope.branchlist123 = promise.branchlist;
                    if ($scope.branchlist123.length > 0) {
                        $scope.branchlist = promise.branchlist;

                        angular.forEach($scope.branchlist, function (dd) {

                            dd.ambid = false;
                        });
                    } else {
                        swal("No Records Found For Selected Details");
                    }

                });
        };

        $scope.onselectBranch = function (ASMAY_Id, AMCO_Id, AMB_Id) {

            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.ivrM_Month_Id = "";
            $scope.catreport = true;
            $scope.obj = {};
            $scope.subjectlist = [];
          
               angular.forEach($scope.branchlist, function (role) {
                   if (!!role.AMB_Id) $scope.AMB_Id.push(role);
            });


            var data1 = {
                "ASMAY_Id": ASMAY_Id,
                "AMCO_Id": AMCO_Id,
                "AMB_Id": AMB_Id,
              
            };

            apiService.create("CollegeDailyAttendance/onselectBranch", data1).
                then(function (promise) {
                    $scope.semlist = promise.semlist;
                });


        };

        $scope.getsection = function () {

            $scope.ACMS_Id = "";
            $scope.obj = {};
            $scope.ivrM_Month_Id = "";
            $scope.albumNameArraycolumn3 = [];
            $scope.obj = {};
            $scope.subjectlist = [];

            //angular.forEach($scope.branchlist, function (role) {
            //    if (!!role.AMB_Id) $scope.AMB_Id.push(role);
            //});

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
               // "Temp_branchDTO": $scope.albumNameArraycolumn3,
                "AMSE_Id": $scope.AMSE_Id,
                "AMB_Id": $scope.AMB_Id
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

            //angular.forEach($scope.branchlist, function (role) {
            //    if (!!role.ambid) $scope.albumNameArraycolumn3.push(role);
            //});

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACMS_Id": $scope.ACMS_Id,
               // "Temp_branchDTO": $scope.albumNameArraycolumn3
                "AMB_Id": $scope.AMB_Id
            };

            apiService.create("CollegeDailyAttendance/getsubject", data).then(function (promise) {

                $scope.seclist234 = promise.subjectlist;
                if ($scope.seclist234.length > 0) {
                    $scope.subjectlist = promise.subjectlist;
                } else {
                    swal("No Subject Is Found For Selected List");
                }

            });
        };

        $scope.onreport = function (obj) {

            $scope.all = false;
            if ($scope.myForm.$valid) {

                $scope.albumNameArraycolumn = [];

                angular.forEach($scope.branchlist, function (role) {
                    if (!!role.AMB_Id) $scope.AMB_Id.push(role);
                });

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                //  "ISMS_Id": obj.ismS_Id.ismS_Id,
                   // "IVRM_Month_Id": $scope.ivrM_Month_Id,
                    //"Temp_branchDTO": $scope.albumNameArraycolumn,
                    "AMB_Id": $scope.AMB_Id,
                    "FromDate": $scope.fromdate,
                    "ToDate": $scope.todate

                };

                apiService.create("CollegeDailyAttendance/onreport", data).
                    then(function (promise) {

                       $scope.columnsTest = promise.datelist;
                        $scope.students1 = promise.studentreport;


                        if ($scope.students1.length > 0 && $scope.students1 !== null && $scope.students1 !== undefined) {

                                  //  $scope.monthname = dd.ivrM_Month_Name;
                                 $scope.subjectname = $scope.students1[0].subjectname;
                                    $scope.coursename = $scope.students1[0].coursename;
                                    $scope.branchname = $scope.students1[0].branchname;
                                    $scope.semestername = $scope.students1[0].semestername;
                                    $scope.sectionname = $scope.students1[0].sectionname;
                                
                           
                            $scope.printdatatabledate = $scope.students1;

                            $scope.studentdetails = [];

                            angular.forEach($scope.students1, function (dd) {
                                if ($scope.studentdetails.length === 0) {
                                    $scope.studentdetails.push({
                                        student: dd.student, admno: dd.admno, regno: dd.regno, rollno: dd.rollno, coursename: dd.coursename,
                                        branchname: dd.branchname, semestername: dd.semestername, sectionname: dd.sectionname, AMCST_Id: dd.AMCST_Id
                                    });
                                } else {
                                    var count = 0;
                                    angular.forEach($scope.studentdetails, function (d) {
                                        if (d.AMCST_Id === dd.AMCST_Id) {
                                            count += 1;
                                        }
                                    });
                                    if (count === 0) {
                                        $scope.studentdetails.push({
                                            student: dd.student, admno: dd.admno, regno: dd.regno, rollno: dd.rollno, coursename: dd.coursename,
                                            branchname: dd.branchname, semestername: dd.semestername, sectionname: dd.sectionname, AMCST_Id: dd.AMCST_Id
                                        });
                                    }
                                }
                            });

                            $scope.temp_att = [];
                            angular.forEach($scope.studentdetails, function (dd) {
                                $scope.temp_att = [];
                                angular.forEach($scope.students1, function (d) {
                                    if (d.AMCST_Id === dd.AMCST_Id) {
                                        $scope.temp_att.push(d);
                                    }
                                });
                                dd.columnsTest12 = $scope.temp_att;
                            });

                            $scope.presentCountgrid = $scope.students1.length;
                            $scope.catreport = false;
                            $scope.exp_excel_flag = false;
                            $scope.print_flag = false;
                            $scope.report = true;

                        }
                        else {
                            swal("No Records Found!");
                            $scope.catreport = true;
                            $scope.report = false;
                            $state.reload();
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
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

        $scope.exportToExcel = function (export_id) {
            //if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
            var exportHref = Excel.tableToExcel(export_id, 'studentattendancereport');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
            //}
            //else {
            //    swal("Please Select Records to be Exported");
            //}
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