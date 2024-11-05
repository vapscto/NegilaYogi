(function () {
    'use strict';
    angular.module('app')
        .controller('AlumniRegistrationReportcontroller', AlumniRegistrationReportcontroller)
    AlumniRegistrationReportcontroller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout']
    function AlumniRegistrationReportcontroller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout) {

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


        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.itemsPerPage1 = 10;
        $scope.currentPage1 = 1;

        $scope.imgname = logopath;

        $scope.sortKey = "regno";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa
        $scope.alupayment = false;
        $scope.aludetails = false;
        $scope.ndate = new Date();
        $scope.presentCountgrid = 0;

        $scope.tadprint = false;
        $scope.nameschedule = true;
        $scope.printdatatable = [];

        $scope.alumnichanges = function () {
            $scope.alupayment = false;
            $scope.aludetails = false;
            $scope.alumnidetails = [];
            $scope.alumnipayment = [];
            $scope.fromdate = "";
            $scope.todate = "";
        }
        $scope.Clearid = function () {
            $scope.alupayment = false;
            $scope.aludetails = false;
            $scope.alumnidetails = [];
            $scope.alumnipayment = [];
            $scope.fromdate = "";
            $scope.todate = "";
        }

        $scope.submitted = false;
        $scope.showreport = function () {

            if ($scope.myForm.$valid) {

                $scope.fromdate = new Date($scope.fromdate).toDateString();
                $scope.todate = new Date($scope.todate).toDateString();
                var data = {
                    "fromdate": $scope.fromdate,
                    "todate": $scope.todate,
                    "alumniregister": $scope.alumniregister,
                }
                apiService.create("AlumniDonation/alumnidetails/", data).
                    then(function (promise) {
                        if (promise.alumnidetails > 0 || promise.alumnidetails !== null) {
                            if ($scope.alumniregister == "AlumniDetails") {
                                if (promise.alumnidetails != null && promise.alumnidetails.length > 0) {
                                    $scope.alumnidetails = promise.alumnidetails;
                                    $scope.alupayment = false;
                                    $scope.aludetails = true;
                                }
                                else {
                                    swal('No Data Found..!!')
                                }

                            }
                            else if ($scope.alumniregister == "AlumniPayment") {
                                if (promise.alumnidetails != null && promise.alumnidetails.length > 0) {
                                    $scope.alumnipayment = promise.alumnidetails;
                                    $scope.alupayment = true;
                                    $scope.aludetails = false;
                                }
                                else {
                                    swal('No Data Found..!!')
                                }
                            }
                        }
                        else {
                            swal('No Data Found!!!')
                        }

                    })
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.toggleAll = function () {
            $scope.printstudents = [];
            var toggleStatus = $scope.all;
            angular.forEach($scope.donationlist, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.printdatatable.indexOf(itm) === -1) {
                        $scope.printdatatable.push(itm);

                    }
                }
                else {
                    $scope.printdatatable.splice(itm);

                }
            });
        }
        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all = $scope.donationlist.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);


            }
            else {
                $scope.printdatatable.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);

            }

        }
        //export start
        $scope.exportToExcel = function (tableId) {
            $scope.alupayment = false;
            $scope.aludetails = true;
            if ($scope.alumnidetails !== null && $scope.alumnidetails.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }

        };
        $scope.exportToExcel1 = function (tableId) {
            $scope.alupayment = true;
            $scope.aludetails = false;
            if ($scope.alumnipayment !== null && $scope.alumnipayment.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }

        };

        $scope.printData = function () {
            $scope.alupayment = false;
            $scope.aludetails = true;
            if ($scope.alumnidetails !== null && $scope.alumnidetails.length > 0) {
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
        $scope.printData1 = function () {
            $scope.alupayment = true;
            $scope.aludetails = false;
            if ($scope.alumnipayment !== null && $scope.alumnipayment.length > 0) {
                var innerContents = document.getElementById("printSectionId1").innerHTML;
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



        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();