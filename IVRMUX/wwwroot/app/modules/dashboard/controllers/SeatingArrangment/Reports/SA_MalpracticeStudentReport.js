(function () {
    'use strict';
    angular.module('app').controller('SA_MalpracticeStudentReportController', SA_MalpracticeStudentReportController)

    SA_MalpracticeStudentReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', '$window', '$http', 'Excel','$timeout']

    function SA_MalpracticeStudentReportController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, uiCalendarConfig, superCache, $window, $http, Excel, $timeout) {

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
            apiService.getURI("SA_Report/GetExamDateloaddata", id).then(function (promise) {
                $scope.yearlst = promise.getyearlist;
                $scope.examlist = promise.getexamlisst;
                $scope.university_examlist = promise.getuniversityexamlist;
                $scope.slotlist = promise.getslotlist;
                $scope.ASMAY_Id = promise.asmaY_Id;
                $scope.coursedetails = promise.getcourselist;
                 
            });
        };

        $scope.OnChangeyear = function () {
            $scope.GetReportList = [];
            $scope.coursedetails = [];
            $scope.EME_Id = "";
            $scope.ESAUE_Id = "";
            $scope.ESAESLOT_Id = "";
            $scope.AMCO_Id = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("SA_Report/OnChangeyear", data).then(function (promise) {

                if (promise !== null) {
                    if (promise.getcourselist !== null && promise.getcourselist.length > 0) {
                        $scope.coursedetails = promise.getcourselist;
                    } else {
                        swal("No Records Found");
                    }
                }
            });
        };

        $scope.OnChangeexam = function () {
            $scope.GetReportList = [];          
            $scope.ESAUE_Id = "";
            $scope.ESAESLOT_Id = "";
            $scope.AMCO_Id = "";
        };

        $scope.OnChangeuniversityexam = function () {
            $scope.GetReportList = [];            
            $scope.ESAESLOT_Id = "";
            $scope.AMCO_Id = "";
        };

        $scope.OnChangeslot = function () {
            $scope.GetReportList = [];            
            $scope.AMCO_Id = "";
        };

        $scope.Onchangecourse = function () {
            $scope.GetReportList = [];           
        };


        $scope.GetMalpracticeStudentReport = function () {
            if ($scope.myForm.$valid) {
                $scope.yearname = "";
                $scope.examname = "";
                $scope.universityexam = "";

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EME_Id": $scope.EME_Id,
                    "ESAUE_Id": $scope.ESAUE_Id,
                    "ESAESLOT_Id": $scope.ESAESLOT_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "ESAEDATE_ExamDate": new Date().toDateString()
                };
                apiService.create("SA_Report/GetMalpracticeStudentReport", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.getReportList !== null && promise.getReportList.length > 0) {
                            $scope.GetReportList = promise.getReportList;

                            angular.forEach($scope.yearlst, function (d) {
                                if (parseInt($scope.ASMAY_Id) === d.asmaY_Id) {
                                    $scope.yearname = d.asmaY_Year;
                                }
                            });

                            angular.forEach($scope.examlist, function (d) {
                                if (parseInt($scope.EME_Id) === d.emE_Id) {
                                    $scope.examname = d.emE_ExamName;
                                }
                            });

                            angular.forEach($scope.university_examlist, function (d) {
                                if (parseInt($scope.ESAUE_Id) === d.esauE_Id) {
                                    $scope.universityexam = d.esauE_ExamName;
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
            $scope.sheetname = "Student Malpractice Report Year_" + $scope.yearname ;
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
            $scope.searchbtn = false;
            $scope.slotlist = [];
            $scope.GetExamDateloaddata();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();