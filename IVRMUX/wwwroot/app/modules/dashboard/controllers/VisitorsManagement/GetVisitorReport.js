(function () {
    'use strict';
    angular
        .module('app')
        .controller('GetVisitorReportController', GetVisitorReportController)

    GetVisitorReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache']
    function GetVisitorReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache) {


        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //if (ivrmcofigsettings.length > 0) {
        //    paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
        //    copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        //}
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;



        $scope.searchValue = "";

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }



        $scope.qwe = {};
        $scope.loadgrid = function () {
            $scope.columnall = true;

            apiService.getURI("GatePassReport/loaddata/", 5).then(function (promise) {

                $scope.month_list = promise.month_list;
            });
           
        }
        

        $scope.columnSort = false;

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.datevis = false;
       
        $scope.change = function () {
            if ($scope.searchby == 'VMMV_VisitorName') {
                $scope.columnall = false;
                $scope.datevis = false;
                $scope.txtbox = "";
                $scope.Date_Visit = "";
                $scope.Cumureport = false;
                $scope.column = 'Visitor Name';


            }
            else if ($scope.searchby == 'VMMV_MeetingDateTime') {
                $scope.columnall = true;
                $scope.datevis = true;
                $scope.txtbox = "";
                $scope.Date_Visit = "";
                $scope.Cumureport = false;
                $scope.column = 'Meeting Date';
            }
            else if ($scope.searchby == 'VMMV_VisitorContactNo') {
                $scope.columnall = false;
                $scope.datevis = false;
                $scope.txtbox = "";
                $scope.Date_visit = "";
                $scope.Cumureport = false;
                $scope.column = 'Contact No';
            }
            else if ($scope.searchby == 'VMMV_VisitorEmailid') {
                $scope.columnall = false;
                $scope.datevis = false;
                $scope.txtbox = "";
                $scope.Date_visit = "";
                $scope.Cumureport = false;
                $scope.column = 'Email Id';
            }
            else if ($scope.searchby == 'AMVM_To_Meet') {
                $scope.columnall = false;
                $scope.datevis = false;
                $scope.txtbox = "";
                $scope.Date_visit = "";
                $scope.Cumureport = false;
                $scope.column = 'Meet Person';
            }
            else if ($scope.searchby == 'all') {
                $scope.columnall = true;
                $scope.datevis = false;
                $scope.txtbox = "All";
            }

        }


        // TO Show The Data
        $scope.submitted = false;
        $scope.report = function (qwe) {
        
            var fromdate1 = $scope.startfromdate == null ? "" : $filter('date')($scope.startfromdate, "yyyy-MM-dd");
            var todate1 = $scope.startenddate == null ? "" : $filter('date')($scope.startenddate, "yyyy-MM-dd");
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var value = '';

                if ($scope.searchby == 'VMMV_MeetingDateTime') {
                    $scope.datevis = true;
                    var date = new Date(qwe.VMMV_MeetingDateTime);
                    var year = date.getFullYear();
                    var month = date.getMonth() + 1;
                    var day = date.getDate();
                    var value = month + '/' + day + '/' + year;

                }
                else {
                    value = $scope.txtbox;
                }

                var data = {
                    "searchby": $scope.searchby,
                    "txtbox": value,
                    "fromdate": fromdate1,
                    "todate": todate1,
                    "all1": $scope.all1,
                    "month_id": $scope.month,
                }

                apiService.create("GetVisitorReport/report", data).
                    then(function (promise) {
                        if (promise.viewlist.length > 0) {
                            $scope.newuser = promise.viewlist;
                            $scope.presentCountgrid = $scope.newuser.length;
                            $scope.Cumureport = true;
                            $scope.screport = true;
                            $scope.export = true;
                        }
                        else {
                            swal("No Records Found");

                            $scope.screport = false;
                            $scope.export = false;
                            $scope.Cumureport = false;
                            $scope.Cumureport1 = false;
                        }

                    })
            }
        };

        $scope.cancel = function () {
            $scope.searchby = "";
            $scope.txtbox = "";
            $scope.Cumureport = false;
            $scope.screport = false;
            $scope.export = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();

            $scope.startfromdate = "";
            $scope.startenddate = "";
            $scope.month = "";
            $scope.all1 = "";

            $state.reload();
        }

        //for print
        $scope.Print = function () {

            if ($scope.newuser !== null && $scope.newuser.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/Visitor_Management/InwardReportPdf.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
        }

        // end for print

        $scope.exportToExcel = function (table) {
            var exportHref = Excel.tableToExcel(printSectionId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);

        }

       // ============================================================================Old Code==================================================
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        //$scope.qwe = {};
        //$scope.loadgrid = function () {
        //    $scope.columnall = true;
        //}

        //$scope.columnSort = false;

        //$scope.sort = function (keyname) {
        //    $scope.sortKey = keyname;   //set the sortKey to the param passed
        //    $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        //}

        //$scope.interacted = function (field) {
        //    return $scope.submitted;
        //};

        //$scope.datevis = false;

        //$scope.change = function () {
        //    if ($scope.searchby == 'AMVM_Name') {
        //        $scope.columnall = false;
        //        $scope.datevis = false;
        //        $scope.txtbox = "";
        //        $scope.Date_Visit = "";
        //        $scope.Cumureport = false;
        //        $scope.column = 'Name';


        //    }
        //    else if ($scope.searchby == 'Date_Visit') {
        //        $scope.columnall = true;
        //        $scope.datevis = true;
        //        $scope.txtbox = "";
        //        $scope.Date_Visit = "";
        //        $scope.Cumureport = false;
        //        $scope.column = 'Date';
        //    }
        //    else if ($scope.searchby == 'AMVM_Contact_No') {
        //        $scope.columnall = false;
        //        $scope.datevis = false;
        //        $scope.txtbox = "";
        //        $scope.Date_visit = "";
        //        $scope.Cumureport = false;
        //        $scope.column = 'Contact No';
        //    }
        //    else if ($scope.searchby == 'AMVM_To_Meet') {
        //        $scope.columnall = false;
        //        $scope.datevis = false;
        //        $scope.txtbox = "";
        //        $scope.Date_visit = "";
        //        $scope.Cumureport = false;
        //        $scope.column = 'Meet Person';
        //    }
        //    else if ($scope.searchby == 'all') {
        //        $scope.columnall = true;
        //        $scope.datevis = false;
        //    }

        //}


        //// TO Show The Data
        //$scope.submitted = false;
        //$scope.report = function (qwe) {

        //    $scope.submitted = true;
        //    if ($scope.myForm.$valid) {
               
        //        var value = '';
                
        //        if ($scope.searchby == 'Date_Visit') {
        //            $scope.datevis = true;
        //            var date = new Date(qwe.Date_visit);
        //            var year = date.getFullYear();
        //            var month = date.getMonth() + 1;
        //            var day = date.getDate();
        //            var value = month + '/' + day + '/' + year;

        //        }
        //        else {
        //            value = $scope.txtbox;
        //        }

        //        var data = {
        //            "searchby": $scope.searchby,
        //            "txtbox": value
        //        }

        //        apiService.create("GetVisitorReport/report", data).
        //            then(function (promise) {
        //                if (promise.viewlist.length > 0) {
        //                    $scope.newuser = promise.viewlist;
        //                    $scope.presentCountgrid = $scope.newuser.length;
        //                    $scope.Cumureport = true;
        //                    $scope.screport = true;
        //                    $scope.export = true;
        //                }
        //                else {
        //                    swal("No Records Found");
        //                }

        //            })
        //    }
        //};

        //$scope.cancel = function () {
        //    $scope.searchby = "";
        //    $scope.txtbox = "";
        //    $scope.Cumureport = false;
        //    $scope.screport = false;
        //    $scope.export = false;
        //    $scope.submitted = false;
        //    $scope.myForm.$setPristine();
        //    $scope.myForm.$setUntouched();

        //}

        ////for print
        //$scope.Print = function () {

        //    if ($scope.newuser !== null && $scope.newuser.length > 0) {
        //        var innerContents = document.getElementById("printSectionId").innerHTML;
        //        var popupWinindow = window.open('');
        //        popupWinindow.document.open();
        //        popupWinindow.document.write('<html><head>' +
        //            '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        //            '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
        //            '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
        //            '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
        //        );
        //        popupWinindow.document.close();
        //    }
        //}

        //// end for print

        //$scope.exportToExcel = function (table) {
        //    var exportHref = Excel.tableToExcel(table, 'sheet name');
        //    $timeout(function () { location.href = exportHref; }, 100);

        //}


    }

})();