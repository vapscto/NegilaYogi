(function () {
    'use strict';
    angular
        .module('app')
        .controller('OverallDailyAttendanceReportNewController', OverallDailyAttendanceReportNewController)

    OverallDailyAttendanceReportNewController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter']
    function OverallDailyAttendanceReportNewController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter) {


        $scope.catreport_btn = true;
        $scope.catreport = false;
        $scope.printdatatable = [];
        $scope.printdatatable_model = [];
        $scope.currentPage = 1;        
        $scope.submitted = false;

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
            copty = 0;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.itemsPerPage_model = paginationformasters;
        $scope.coptyright = copty;

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        } else {
            logopath = "";
        }

        $scope.imgname = logopath;  

        $scope.submitted = false;    


        $scope.StuAttRptDropdownList = function () {
            apiService.get("OverallDailyAttendanceReport/getdetails/").then(function (promise) {
                $scope.yearDropdown = promise.academicList;
                $scope.allAcademicYear1 = promise.academicListdefault;
                for (var i = 0; i < $scope.yearDropdown.length; i++) {
                    name = $scope.yearDropdown[i].asmaY_Id;
                    for (var j = 0; j < $scope.allAcademicYear1.length; j++) {
                        if (parseInt(name) === parseInt($scope.allAcademicYear1[j].asmaY_Id)) {
                            $scope.yearDropdown[i].Selected = true;
                            $scope.asmaY_Id = $scope.allAcademicYear1[j].asmaY_Id;
                            $scope.minDatedof = new Date($scope.allAcademicYear1[j].asmaY_From_Date);
                            $scope.maxDatedof = new Date($scope.allAcademicYear1[j].asmaY_To_Date);
                            $scope.yearname = $scope.allAcademicYear1[j].asmaY_Year;

                        }
                    }
                }
            });
        };

        $scope.getyeardetails = function () {
            $scope.students = [];
            $scope.catreport = false;
            $scope.catreport_btn = true;
            $scope.fromdate = "";
            angular.forEach($scope.yearDropdown, function (year) {
                if (year.asmaY_Id === parseInt($scope.asmaY_Id)) {
                    $scope.minDatedof = new Date(year.asmaY_From_Date);
                    $scope.maxDatedof = new Date(year.asmaY_To_Date);
                    $scope.yearname = year.asmaY_Year;
                }
            });
        };

        $scope.fromdate = function () {
            $scope.students = [];
            $scope.catreport = false;
            $scope.catreport_btn = true;
        };

        $scope.showReport = function () {
            $scope.printdatatable = [];
            $scope.students = [];
            $scope.studentsnew = [];

            $scope.searchValue = "";
            if ($scope.myForm.$valid) {
                var fromdate = new Date($scope.fromdate).toDateString();
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "fromdate": fromdate
                };
                apiService.create("OverallDailyAttendanceReport/getoveallattendance", data)
                    .then(function (promise) {
                        $scope.students = promise.studentAttendanceList;
                        $scope.studentsnew = promise.studentAttendanceListnew;
                        $scope.institutiondetails = promise.institutiondetails;

                        $scope.instname = $scope.institutiondetails[0].mI_Name;
                        $scope.instaddress = $scope.institutiondetails[0].mI_Address1;

                        
                        if ($scope.students !== null && $scope.students.length > 0) {

                            $scope.classid = [];

                            angular.forEach($scope.students, function (cls) {
                                if ($scope.classid.length === 0) {
                                    $scope.classid.push({ ASMCL_Id: cls.ASMCL_Id, ClassName: cls.classname, presentcount: cls.present, absentcount: cls.absent });
                                } else if ($scope.classid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.classid, function (sec) {
                                        if (sec.ASMCL_Id === cls.ASMCL_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.classid.push({ ASMCL_Id: cls.ASMCL_Id, ClassName: cls.classname, presentcount: cls.present, absentcount: cls.absent });
                                    }
                                }
                            });
                            console.log($scope.classid);
                            $scope.sectionid = [];

                            angular.forEach($scope.students, function (cls) {
                                if ($scope.sectionid.length === 0) {
                                    $scope.sectionid.push({ ASMS_Id: cls.ASMS_Id, SectionName: cls.sectionname });
                                } else if ($scope.classid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.sectionid, function (sec) {
                                        if (sec.ASMS_Id === cls.ASMS_Id) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.sectionid.push({ ASMS_Id: cls.ASMS_Id, SectionName: cls.sectionname });
                                    }
                                }
                            });

                            console.log($scope.sectionid);

                            $scope.tempstudentlist = [];

                            angular.forEach($scope.classid, function (clsid) {
                                angular.forEach($scope.sectionid, function (secid) {
                                    angular.forEach($scope.studentsnew, function (stuid) {
                                        if (clsid.ASMCL_Id === stuid.ASMCL_Id && secid.ASMS_Id === stuid.ASMS_Id) {
                                            $scope.tempstudentlist.push(stuid);
                                        }
                                    });
                                });
                            });

                            $scope.templist = [];

                            angular.forEach($scope.students, function (dd) {
                                $scope.templist = [];
                                angular.forEach($scope.studentsnew, function (ddd) {
                                    if (dd.ASMCL_Id === ddd.ASMCL_Id && dd.ASMS_Id === ddd.ASMS_Id) {
                                        $scope.templist.push({ studentname: ddd.Studentname });
                                    }
                                });

                                if ($scope.templist.length === 0) {
                                    $scope.templist.push({ studentname: "No Absents" });
                                }

                                dd.studentlist = $scope.templist;
                            });

                            console.log("Student List");
                            console.log($scope.students);

                            $scope.catreport_btn = false;
                            $scope.catreport = true;
                        }
                        else {
                            swal("No Records Found!");
                            $scope.catreport_btn = true;
                            $scope.catreport = false;
                        }
                    });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.clear = function () {
            $state.reload();
        };

        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };

        $scope.print = function () {
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

        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();
