(function () {
    'use strict';
    angular.module('app').controller('StudentHallticketController', StudentHallticketController);

    StudentHallticketController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window','$compile'];
    function StudentHallticketController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, $compile) {

        $scope.printflag = true;
        $scope.showreport = 0;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null) {
            if (admfigsettings.length > 0) {
                var logopath = admfigsettings[0].asC_Logo_Path;
            }
        }

        $scope.items = new Array(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
        $scope.imgname = logopath;
        //$scope.printdata = false;

        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("StudentHallticket/GetLoadData", pageid).then(function (promise) {
                $scope.getyearlist = promise.getyearlist;
                $scope.getcurrentyearlist = promise.getcurrentyearlist;
                $scope.getexamlist = promise.getexamlist;
                $scope.ASMAY_Id = promise.asmaY_Id;
                if ($scope.getexamlist === null || $scope.getexamlist.length === 0) {
                    swal("Exam Details Not Found");
                }

            });
        };

        $scope.GetExamDetails = function () {
            $scope.getexamlist = [];
            $scope.main_list = [];
            $scope.printflag = true;
            $scope.showreport = 0;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("StudentHallticket/GetExamDetails", data).then(function (promise) {

                $scope.getexamlist = promise.getexamlist;
                if ($scope.getexamlist === null || $scope.getexamlist.length === 0) {
                    swal("Exam Details Not Found");
                }
            });
        };

        $scope.GetReport = function () {
            $scope.printflag = true;
            $scope.main_list = [];
            $scope.showreport = 0;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "EME_Id": $scope.EME_Id
            };

            apiService.create("StudentHallticket/GetReport", data).then(function (promise) {
                $scope.main_list = promise.datareport;
                if ($scope.main_list.length > 0) {
                  
                    $scope.printflag = false;                  
                    $scope.showreport= 1;

                    $scope.dailybtedates = 'btwdates';
                        //$scope.gridlength = true;
                        //$scope.print = false;
                        //$scope.printdata = true;


                    angular.forEach($scope.main_list, function (dd) {
                        var str = dd.ehT_HallTicketNo;
                        dd.arr3 = new Array(...str);
                    });

                    $scope.configuraion = promise.configuraion;

                    $scope.studentlist = promise.studarray;
                    $scope.sublist = promise.subarray;

                    $scope.intitutelist = promise.institute;

                    angular.forEach($scope.acdlist, function (yy) {
                        if (yy.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                            $scope.accyear = yy.asmaY_Year;
                        }
                    });

                    angular.forEach($scope.getexamlist, function (yyy) {
                        if (yyy.emE_Id === parseInt($scope.EME_Id)) {
                            $scope.examname = yyy.emE_ExamName;
                        }
                    });

                    var e1 = angular.element(document.getElementById("report"));
                    $compile(e1.html(promise.htmldata))(($scope));

                    if ($scope.configuraion.length > 0) {

                        $scope.principal = $scope.configuraion[0].ivrmgC_PrincipalSign;
                    }
                    else {
                        $scope.principal = "";
                    }

                } else {
                    swal("No Records Found");
                }

            });
        };

        $scope.Cancel = function () {
            $scope.EME_Id = "";
            $scope.printflag = true;
            $scope.main_list = [];
            $scope.BindData();
        };

        $scope.submitted = false;

        $scope.interacted = function () {
            return $scope.submitted;
        };

        $scope.printData = function () {

            var innerContents1 = document.getElementById("printformat1").innerHTML;
            var popupWinindow1 = window.open('');
            popupWinindow1.document.open();
            popupWinindow1.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Vikasa/VIKASATCREPORT/VIKASATCReportPdf1.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents1 + '</html>');
            popupWinindow1.document.close();
        };
    }
})();