(function () {
    'use strict';
    angular.module('app').controller('School_Student_Allotment_ReportController', School_Student_Allotment_ReportController)

    School_Student_Allotment_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', '$window', '$http', 'Excel', '$timeout']

    function School_Student_Allotment_ReportController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, uiCalendarConfig, superCache, $window, $http, Excel, $timeout) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.tempcldrlst = [];
        $scope.GetReportList = [];
        $scope.searchbtn = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";

        $scope.ESAABSTU_MaxExamDate = new Date();
        $scope.ESAEDATE_ExamDate = new Date();

        $scope.Getroomdetails = [];
        $scope.GetExamDateloaddata = function () {
            var id = 1;
            apiService.getURI("School_Exam_Date_Room/GetSeatAllotedReport", id).then(function (promise) {
                $scope.yearlst = promise.getAcademicYearList;
                $scope.ASMAY_Id = promise.asmaY_Id;
                $scope.GetExamSlotList = promise.getExamSlotList;
                $scope.GetExamList = promise.getExamList;
                angular.forEach($scope.yearlst, function (dd) {
                    if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                        $scope.mindate = new Date(dd.asmaY_From_Date);
                        $scope.maxdate = new Date(dd.asmaY_To_Date);
                    }
                });
            });
        };

        $scope.OnChangeYear = function () {
            angular.forEach($scope.yearlst, function (dd) {
                if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                    $scope.mindate = new Date(dd.asmaY_From_Date);
                    $scope.maxdate = new Date(dd.asmaY_To_Date);
                }
            });
            $scope.GetReportList = [];
            $scope.classlist = [];
            $scope.ASMCL_Id = "";
            $scope.sectionlist = [];
            $scope.ASMS_Id = "";
            $scope.subjectlist = [];
            $scope.ISMS_Id = "";
            $scope.EME_Id = "";
            $scope.ESAESLOT_Id = "";
            $scope.ESAROOM_Id = "";
            $scope.ExamDate = null;           
        };

        $scope.GetExam = function () {
            $scope.ESAESLOT_Id = "";
            $scope.GetReportList = [];
            $scope.ExamDate = null; 
        };

        $scope.GetSlot = function () {            
            $scope.GetReportList = [];
            $scope.ExamDate = null;
        };

        $scope.GetAbsentStudentReport = function () {
            if ($scope.myForm.$valid) {
                $scope.yearname = "";
                $scope.examname = "";
                $scope.universityexam = "";
                $scope.classname = "";
                $scope.sectioname = "";
                $scope.GetReportList = [];
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EME_Id": $scope.EME_Id,
                    "ESAESLOT_Id": $scope.ESAESLOT_Id,
                    "ExamDate": new Date($scope.ExamDate).toDateString()
                    
                };

                apiService.create("School_Exam_Date_Room/GetSchoolSeatAllotementReport", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.getSeatAllotedReport !== null && promise.getSeatAllotedReport.length > 0) {
                            $scope.GetReportList = promise.getSeatAllotedReport;

                            $scope.temproomlist = [];
                            angular.forEach($scope.GetReportList, function (dd) {
                                if ($scope.temproomlist.length === 0) {
                                    $scope.temproomlist.push({ ESAROOM_Id: dd.ESAROOM_Id, ESAROOM_RoomName: dd.ESAROOM_RoomName });
                                } else if ($scope.temproomlist.length > 0) {
                                    var count = 0;

                                    angular.forEach($scope.temproomlist, function (d) {
                                        if (d.ESAROOM_Id === dd.ESAROOM_Id) {
                                            count += 1;
                                        }
                                    });

                                    if (count === 0) {
                                        $scope.temproomlist.push({ ESAROOM_Id: dd.ESAROOM_Id, ESAROOM_RoomName: dd.ESAROOM_RoomName });
                                    }
                                }
                            });

                            angular.forEach($scope.temproomlist, function (dd) {
                                $scope.list = [];
                                angular.forEach($scope.GetReportList, function (d) {
                                    if (dd.ESAROOM_Id === d.ESAROOM_Id) {
                                        $scope.list.push(d);
                                    }
                                });
                                dd.studentdetails = $scope.list;
                            });

                            angular.forEach($scope.yearlst, function (dd) {
                                if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                    $scope.yearname = dd.asmaY_Year;
                                }
                            });

                            angular.forEach($scope.GetExamList, function (dd) {
                                if (dd.emE_Id === parseInt($scope.EME_Id)) {
                                    $scope.examname = dd.emE_ExamName;
                                }
                            });

                            angular.forEach($scope.GetExamSlotList, function (dd) {
                                if (dd.esaesloT_Id === parseInt($scope.ESAESLOT_Id)) {
                                    $scope.slotname = dd.esaesloT_SlotName;
                                }
                            });


                        } else {
                            swal("No Recrds Found");
                        }
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.Print = function () {

            var innerContents = document.getElementById("printdata").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();

        };

        $scope.Excel = function (table1) {
            $scope.sheetname = "Student Absent Report Year_" + $scope.yearname;
            var exportHref = Excel.tableToExcel(table1, $scope.sheetname);
            var excelname = $scope.sheetname + ".xlsx";
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.cleardata = function () {
            $scope.ASMAY_Id = "";
            $scope.AMCO_Id = "";
            $scope.EME_Id = "";
            $scope.ESAESLOT_Id = "";
            $scope.ESAUE_Id = "";
            $scope.Room_Temp_Details = [];
            $scope.ESAABSTU_ExamDate = null;
            $scope.Getroomdetails = [];
            $scope.Getsavedroomdetails = [];
            $scope.yearlst = [];
            $scope.examlist = [];
            $scope.university_examlist = [];
            $scope.coursedetails = [];
            $scope.submitted = false;
            $scope.searchbtn = false;
            $scope.slotlist = [];
            $scope.GetExamDateloaddata();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();