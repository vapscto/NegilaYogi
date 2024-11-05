(function () {
    'use strict';
    angular.module('app').controller('VikasaHallTicketReportController', VikasaHallTicketReportController)

    VikasaHallTicketReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$compile']
    function VikasaHallTicketReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $compile) {

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.items = new Array(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
        $scope.imgname = logopath;
        $scope.printdata = false;
        $scope.gridlength = false;

        $scope.BindData = function () {
            apiService.getDATA("VikasaHallTicketReport/getdetails").then(function (promise) {
                $scope.acdlist = promise.acdlist;
                $scope.ctlist = promise.ctlist;
                $scope.seclist = promise.seclist;
                $scope.examlist = promise.examlist;
            });
        };

        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.onselectAcdYear = function (ASMAY_Id) {
            $scope.gridlength = false;
            $scope.printdata = false;
            $scope.main_list = [];
            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            $scope.EME_Id = "";

            var data = {
                "ASMAY_Id": ASMAY_Id
            };
            apiService.create("VikasaHallTicketReport/onselectAcdYear", data).then(function (promise) {
                $scope.ctlist = promise.ctlist;

            });
        };

        $scope.onselectclass = function (ASMCL_Id, ASMAY_Id) {
            $scope.gridlength = false;
            $scope.printdata = false;
            $scope.main_list = [];
            $scope.ASMS_Id = "";
            $scope.EME_Id = "";

            var data = {
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id
            };
            apiService.create("VikasaHallTicketReport/onselectclass", data).then(function (promise) {
                $scope.seclist = promise.seclist;
            });
        };

        $scope.onselectSection = function () {

            $scope.gridlength = false;
            $scope.printdata = false;
            $scope.main_list = [];
            $scope.EME_Id = "";

            var data = {
                "ASMS_Id": $scope.ASMS_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("VikasaHallTicketReport/onselectSection", data).then(function (promise) {
                $scope.examlist = promise.examlist;
                $scope.studentlistnew = promise.getstudentlist;

                angular.forEach($scope.studentlistnew, function (dd) {
                    dd.amsT_IdSelected = true;
                });
                $scope.all1 = true;
            });
        };

        $scope.report = function () {
            var selected_student = [];
            if ($scope.myForm.$valid) {
                angular.forEach($scope.studentlistnew, function (stu3) {
                    if (stu3.amsT_IdSelected === true) {
                        selected_student.push(stu3);
                    }
                });

                var data = {
                    "ASMS_Id": $scope.ASMS_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EME_Id": $scope.EME_Id,
                    "studentlist": selected_student
                };

                apiService.create("VikasaHallTicketReport/report", data).then(function (promise) {

                    $scope.main_list = promise.datareport;
                    if ($scope.main_list !== null && $scope.main_list.length > 0) {

                        $scope.gridlength = true;
                        $scope.print = false;
                        $scope.printdata = true;

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

                        angular.forEach($scope.examlist, function (yyy) {
                            if (yyy.emE_Id === parseInt($scope.EME_Id)) {
                                $scope.examname = yyy.emE_ExamName;
                            }
                        });

                        $scope.dynamichtml = false;
                        if (promise.htmldata !== null && promise.htmldata !== "") {
                            var e1 = angular.element(document.getElementById("report"));
                            $scope.dynamichtml = true;
                            $compile(e1.html(promise.htmldata))(($scope));
                        }

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
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.Toggle_header1 = function () {
            $scope.gridlength = false;
            $scope.printdata = false;
            var toggleStatus1 = $scope.all1;
            angular.forEach($scope.studentlistnew, function (itm) {
                itm.amsT_IdSelected = toggleStatus1;
            });
        };

        $scope.albumNameArraycolumn = [];
        $scope.columnsTest = [];
        $scope.addColumn1 = function (role) {
            $scope.gridlength = false;
            $scope.printdata = false;
            $scope.all1 = $scope.studentlistnew.every(function (itm) { return itm.amsT_IdSelected; });
            if (role.amsT_IdSelected === true) {
                $scope.albumNameArraycolumn.push(role);
                $scope.columnsTest.push(role);
            }
            else {
                var som = $scope.albumNameArraycolumn.indexOf(role);
                $scope.columnsTest.splice($scope.albumNameArraycolumn.indexOf(role), 1);
                $scope.albumNameArraycolumn.splice($scope.albumNameArraycolumn.indexOf(role), 1);
            }
        };

        $scope.onclickdates = function () {
            $scope.gridlength = false;
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.studentlistnew.some(function (options) {
                return options.amsT_IdSelected;
            });
        };

        $scope.printData = function () {
            if ($scope.dailybtedates === "daily") {
                var innerContents = document.getElementById("table").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();

            } else {
                var innerContents1 = document.getElementById("printformat1").innerHTML;
                var popupWinindow1 = window.open('');
                popupWinindow1.document.open();
                popupWinindow1.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Vikasa/VIKASATCREPORT/VIKASATCReportPdf1.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents1 + '</html>');
                popupWinindow1.document.close();
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };
    }
})();