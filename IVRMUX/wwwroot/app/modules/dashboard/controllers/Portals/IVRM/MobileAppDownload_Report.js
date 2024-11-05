(function () {
    'use strict';
    angular.module('app')
        .controller('MobileAppDownload_ReportController', MobileAppDownload_ReportController)
    MobileAppDownload_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', 'Excel', '$timeout']
    function MobileAppDownload_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, Excel, $timeout) {

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        //for (var i = 0; i < privlgs.length; i++) {
        //    if (privlgs[i].pageId == pageid) {
        //        $scope.userPrivileges = privlgs[i];
        //    }
        //}

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }


        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.sortKey = "regno";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;
        $scope.searchValue = "";
        $scope.ddate = new Date();
        $scope.fromdate = new Date();
        $scope.todate = new Date();

        $scope.tadprint = false;
        $scope.nameschedule = true;
        $scope.printdatatable = [];
        $scope.loaddata = function () {
            $scope.screport = false;

            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            var pageid = 2;
            apiService.getURI("Interaction_Delete_Report/mobload", pageid).
                then(function (promise) {
                    $scope.yearlst = promise.fillyear;
                    $scope.ASMAY_Id = promise.asmaY_Id;

                })
        };


        $scope.submitted = false;

        $scope.showreport = function () {

            if ($scope.myForm.$valid) {

                $scope.fromdate1 = $filter('date')($scope.fromdate, 'yyyy-MM-dd');
                $scope.todate1 = $filter('date')($scope.todate, 'yyyy-MM-dd');

                if ($scope.Active == undefined) {
                    $scope.Active = 0;
                }
                if ($scope.DeActive == undefined) {
                    $scope.DeActive = 0;
                }
                if ($scope.Left == undefined) {
                    $scope.Left = 0;
                }

                var data = {
                    "fromdate": $scope.fromdate1,
                    "todate": $scope.todate1,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "Active": $scope.Active,
                    "DeActive": $scope.DeActive,
                    "Left": $scope.Left,
                    "optionflag": $scope.optionflag
                }
                apiService.create("Interaction_Delete_Report/mobreport/", data).
                    then(function (promise) {

                        if (promise.mobapplist != null && promise.mobapplist.length > 0) {
                            if ($scope.optionflag == 'Download') {
                                $scope.mobapplist = promise.mobapplist;
                                $scope.mobapplist1 = promise.mobapplist.length;
                                $scope.mobapplistnotcount = promise.mobapplistnotcount[0].notdownload;
                                $scope.mobapplisttotalcount = promise.mobapplisttotalcount[0].totalcount;

                                $scope.total_Dp = ($scope.mobapplist1 / $scope.mobapplisttotalcount) * 100;
                                $scope.total_NDp = 100 - $scope.total_Dp;
                            }
                            else {
                                $scope.mobapplist = promise.mobapplist;
                                $scope.mobapplisttotalcount = promise.mobapplist.length;
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
            angular.forEach($scope.mobapplist, function (itm) {
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

            $scope.all = $scope.mobapplist.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);


            }
            else {
                $scope.printdatatable.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);

            }

        }
        //export start
        $scope.exportToExcel = function (tableId) {
            var dwn = "";
            if ($scope.optionflag == 'Download') {
                dwn = "MobAppDownloadReport.xls";
            }
            else {
                dwn = "MobAppNotDownloadReport.xls";
            }
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'MobAppDownloadReport');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = dwn;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        };

        $scope.optchange = function () {
            $scope.mobapplist = [];

        }




        $scope.printData = function () {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
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
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();