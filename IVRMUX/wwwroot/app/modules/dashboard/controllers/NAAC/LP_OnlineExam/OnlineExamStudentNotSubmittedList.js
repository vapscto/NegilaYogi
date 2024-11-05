(function () {
    'use strict';
    angular.module('app').controller('LP_OnlineExamReportController', LP_OnlineExamReportController)

    LP_OnlineExamReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function LP_OnlineExamReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {

        //var paginationformasters;
        //var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //if (ivrmcofigsettings.length > 0) {
        //    paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        //}

        $scope.obj = {};
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');

        var copty;

        $scope.maxdate = new Date();

        //$scope.itemsPerPage = paginationformasters;
        //$scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
       
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            }
            else {
                paginationformasters = 10;
            }
        }
        else {
            paginationformasters = 10;
        }


        $scope.imgname = logopath;
        $scope.reportbtn = true;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.itemsPerPage = 10;
        $scope.loaddata = function () {
           
            var pageid = 4;
            apiService.getURI("LP_OnlineStudentExam/getloaddatareport", pageid).then(function (promise) {
                $scope.getyearlist = promise.getyearlist;

            });

        };

        $scope.onchangeyear = function () {
            $scope.ASMCL_Id = "";
            $scope.getclasslist = [];
            $scope.ISMS_Id = "";
            $scope.getsubjectlist = [];
            $scope.ASMS_Id = "";
            $scope.getsetionlist = [];
            $scope.LPMOEEX_Id = "";
            $scope.getexamlist = [];
            $scope.result = [];
            $scope.reportbtn = true;

            var data = {
                //"ASMAY_Id": $scope.ASMAY_Id
                "ASMAY_Id": 2
            };
            apiService.create("LP_OnlineStudentExam/onchangeyear", data).then(function (promise) {
                $scope.getclasslist = promise.getclasslist;
            });

        };

        $scope.onchangeclass = function () {
            $scope.ISMS_Id = "";
            $scope.getsubjectlist = [];
            $scope.ASMS_Id = "";
            $scope.getsetionlist = [];
            $scope.LPMOEEX_Id = "";
            $scope.getexamlist = [];
            $scope.result = [];
            $scope.reportbtn = true;

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            };
            apiService.create("LP_OnlineStudentExam/onchangeclass", data).then(function (promise) {
                $scope.getsectionlist = promise.getsectionlist;
            });

        };

        $scope.OnchangeSection = function () {
            $scope.ISMS_Id = "";
            $scope.getsubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.LPMOEEX_Id = "";
            $scope.getexamlist = [];
            $scope.result = [];
            $scope.reportbtn = true;
            $scope.submitted1 = false;
            $scope.submitted11 = false;
            $scope.checkall = false;

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id

            };
            apiService.create("LP_OnlineStudentExam/OnchangeSection", data).then(function (promise) {
                $scope.getsubjectlist = promise.getsubjectlist;
            });
        };

        $scope.onchangesubject = function () {

            $scope.LPMOEEX_Id = "";
            $scope.getexamlist = [];
            $scope.result = [];
            $scope.reportbtn = true;

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ISMS_Id": $scope.ISMS_Id,
                "ASMS_Id": $scope.ASMS_Id
            };
            apiService.create("LP_OnlineStudentExam/onchangesubject", data).then(function (promise) {
                $scope.getexamlist = promise.getexamlist;
            });
        };

        $scope.OnChangeExam = function () {

            angular.forEach($scope.getexamlist, function (dd) {
                if (dd.lpmoeeX_Id === parseInt($scope.LPMOEEX_Id)) {
                    $scope.obj.FMCB_fromDATE = new Date(dd.lpmoeeX_FromDateTime);
                    $scope.obj.FMCB_toDATE = new Date(dd.lpmoeeX_ToDateTime);
                }
            });
        };


        $scope.getreport = function () {
            $scope.submitted1 = true;
            $scope.reportbtn = true;
            $scope.result = [];

            if ($scope.myForm.$valid) {
                $scope.fromdate = new Date($scope.obj.FMCB_fromDATE).toDateString();
                $scope.todate = new Date($scope.obj.FMCB_toDATE).toDateString();

                var data = {
                    "ASMAY_Id":$scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ISMS_Id": $scope.ISMS_Id,
                    "LPMOEEX_Id": $scope.LPMOEEX_Id,
                    "fromdate": $scope.fromdate,
                    "todate": $scope.todate,
                    "ASMS_Id": $scope.ASMS_Id
                };

                apiService.create("LP_OnlineStudentExam/getNonSubmittedreport", data).then(function (promise) {
                  
                    if (promise.getdetails !== null && promise.getdetails.length > 0) {
                     
                        $scope.getdetails = promise.getdetails;

               

                    }
                    else {
                        swal('No Records Found');
                    }
                });
            }
            else {
                $scope.submitted1 = true;
            }
        };

       

    

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.sort = function (key) {
            $scope.sortKey = key;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted1;
        };

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
        };


        $scope.printdatatable = [];
        $scope.exportToExcel = function (table1) {
            $scope.sheetname = "Year_" + $scope.yearname + "- Subject_" + $scope.subjectname + " - Exam_" + $scope.examname;
            //var exportHref = Excel.tableToExcel(table1, $scope.sheetname);
            //$timeout(function () { location.href = exportHref; }, 100);

            var exportHref = Excel.tableToExcel(table1, $scope.sheetname);
            var excelname = $scope.sheetname + ".xls";
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);

        };

        $scope.printData = function (printSectionId) {

            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();

        };

        $scope.Print = function () {
            var innerContents = document.getElementById("printdetails").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/onlineexamquestionprint.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();

        };

        $scope.toggleAll = function () {
            $scope.printdatatable = [];
            var toggleStatus = $scope.all;
            angular.forEach($scope.result, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all === true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
            $scope.get_total_student_print();
        };
    }
})();