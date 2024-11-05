
(function () {
    'use strict';
    angular
.module('app')
.controller('SMSReportController', SMSReportController)

    SMSReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache']
    function SMSReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache) {

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
            apiService.getURI("SMSReport/getdetails", pageid).
            then(function (promise) {
                $scope.yearlst = promise.yeardropDown;

            })
        };

        //Radio button switching function
        $scope.onclickloaddata = function () {
            if ($scope.yearwiseorbtwdates === 'yearwise') {
                $scope.ASMAY = "";
                $scope.frdatetodate = true;
                $scope.print_flag = false;
                $scope.sms_flag = false;
                $scope.mail_flag = false;
                $scope.export_flag = false;
                $scope.IsHiddendown = false;
                $scope.submitted = false;

            }
            else if ($scope.yearwiseorbtwdates === 'btwdates') {

                $scope.errMessage = "";
                $scope.obj.FromDate = "";
                $scope.obj.ToDate = "";
                $scope.frdatetodate = false;
                $scope.print_flag = false;
                $scope.sms_flag = false;
                $scope.mail_flag = false;
                $scope.export_flag = false;
                $scope.submitted = false;
                $scope.IsHiddendown = false;
            }
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

        $scope.toggleAll = function () {
            if ($scope.searchValue == '') {
                var toggleStatus = $scope.all;
                angular.forEach($scope.students, function (itm) {
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

            if ($scope.searchValue != '') {
                var toggleStatus = $scope.all;
                angular.forEach($scope.filterValue1, function (itm) {
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
        };

        $scope.selected = function (SelectedStudentRecord, index) {
            if ($scope.searchValue == '') {
                $scope.all = $scope.students.every(function (itm)
                { return itm.selected; });
                if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                    $scope.printdatatable.push(SelectedStudentRecord);
                }
                else {
                    $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                }
            }
            if ($scope.searchValue != '') {
                $scope.all = $scope.filterValue1.every(function (itm)
                { return itm.selected; });
                if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                    $scope.printdatatable.push(SelectedStudentRecord);
                }
                else {
                    $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                }
            }
        };

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };





        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.submitted = false;
        $scope.showreport = function (obj) {

            $scope._fromdate = "";
            $scope._todate = "";
            $scope._ASMAY = "";
            if ($scope.myForm.$valid) {
                if (obj.yearwiseorbtwdates == "yearwise") {
                    $scope._fromdate = new Date();
                    $scope._todate = new Date();
                    $scope._ASMAY = obj.ASMAY;
                }
                else if (obj.yearwiseorbtwdates == "btwdates") {
                    $scope._fromdate = obj.FromDate;
                    $scope._todate = obj.ToDate;
                    $scope._ASMAY = "0";

                }

                var data = {
                    "asmayid": $scope._ASMAY,
                    "from_date": $scope._fromdate,
                    "to_date": $scope._todate,
                }
                apiService.create("SMSReport/Getreportdetails", data).
                then(function (promise) {
                    
                    $scope.datapages = promise.sms_mial_listarray;
                    $scope.printdatatable = promise.sms_mial_listarray;
                    $scope.presentCountgrid = promise.sms_mial_listarray.length;
                    if ($scope.printdatatable.length > 0 || $scope.printdatatable == null) {
                        $scope.IsHiddendown = true;
                      
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

        $scope.Clearid = function () {
            // $state.reload();
            $scope.obj.yearwiseorbtwdates = "";
            $scope.obj.ASMAY = "";
            $scope.IsHiddendown = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };
    }
})();