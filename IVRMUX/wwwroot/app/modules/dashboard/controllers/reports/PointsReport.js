
(function () {
    'use strict';
    angular
.module('app')
.controller('PointsReportController', PointsReportController)

    PointsReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout']
    function PointsReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout) {

        $scope.notrequired = true;

        $scope.obj = {};
        $scope.tadprint = false;
        $scope.nameschedule = true;
        $scope.printdatatable = [];

        //Date:23-12-2016 for displaying privileges.
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        $scope.sortKey = "regno";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;

        $scope.usrname = localStorage.getItem('username');

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.loaddata = function () {
            $scope.screport = false;
            $scope.paginate1 = "paginate1";
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = paginationformasters;
            var pageid = 2;
            apiService.getURI("PointsReport/getdetails", pageid).
            then(function (promise) {
                $scope.yearlst = promise.yeardropDown;
                $scope.classlist = promise.fillclass;
                $scope.IsHiddendown = false;
            })
        };

   
        //export start
        $scope.exportToExcel = function (tableId) {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }

        };
        //export end

        $scope.printData = function () {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
               '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
           '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
            '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
           '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }
        //print end


     
        $scope.sortBy = function (keyname) {
            
            $scope.propertyName = keyname;
            $scope.reverse = !$scope.reverse;
        };


        //$scope.sortBy = function (propertyName) {
        //    $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
        //    $scope.propertyName = propertyName;
        //};


        $scope.cancel = function () {
            $scope.loaddata();
        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.submitted = false;
        $scope.showreport = function () {

        
            if ($scope.myForm.$valid) {

                var data = {
                    "ASMAY_Id": $scope.ASMAY,
                    "ASMCL_Id": $scope.ASMCL
      
                }
                apiService.create("PointsReport/Getreportdetails", data).
                then(function (promise) {
                    
                    $scope.datapages = promise.studentDetails;
                    $scope.printdatatable = promise.studentDetails;
                    $scope.presentCountgrid = promise.studentDetails.length;
                    if ($scope.printdatatable.length > 0 || $scope.printdatatable == null) {
                        $scope.IsHiddendown = true;
                        $scope.export_flag = true;
                        $scope.print_flag = false;
                      
                    } else {
                        swal("No Records Found");
                        $scope.IsHiddendown = false;
                      
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };
        //export start
        $scope.exportToExcel = function (tableId) {
            if ($scope.datapages !== null && $scope.datapages.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }

        };
        //export end

        $scope.printData = function () {
            if ($scope.datapages !== null && $scope.datapages.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
               '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
           '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
            '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
           '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
           );
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }
        $scope.Clearid = function () {
            $scope.ASMAY = "";
            $scope.ASMCL = "";
            $scope.IsHiddendown = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };
    }
})();