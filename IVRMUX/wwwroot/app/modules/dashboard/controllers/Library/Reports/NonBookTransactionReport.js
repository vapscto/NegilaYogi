
(function () {
    'use strict';
    angular
        .module('app')
        .controller('NonBookTransactionReportController', NonBookTransactionReportController)

    NonBookTransactionReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter', 'Excel', '$timeout']
    function NonBookTransactionReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter, Excel, $timeout) {

        $scope.submitted = false;

        $scope.boxfalg = false;
        // $scope.statusdata = false;
        //  $scope.minDate = new Date();
        $scope.printflag = false;

        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;



        //---------------Load --data
        $scope.loaddata = function () {
            debugger;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            debugger;
            //var pageid = 2;
            apiService.getURI("NonBookTransactionReport/getdetails", 2).then(function (promise) {
               
                $scope.classList = promise.classList;
                $scope.sectionList = promise.sectionList;
                $scope.usercheck = true;
                $scope.usercheck23 = true;

                $scope.booktype = promise.booktype;

                $scope.booklist = promise.booklist;

                $scope.parentsubjectlist = promise.parentsubjectlist;

                $scope.subsubjectlist = promise.subsubjectlist;

                $scope.lib_list = promise.lib_list;

                $scope.LMAL_Id = promise.lmaL_Id;
                angular.forEach($scope.classList, function (itm) {
                    itm.select = true;
                });
                angular.forEach($scope.sectionList, function (itm) {
                    itm.select = true;
                });
            })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
        //--------------End-Load --data


        //$scope.Type = "all";
        $scope.AGType = "Student";
        $scope.TrnType = "all";

        //---------------Get-Report
        $scope.allgrid = false;
        $scope.selectedClasslist = [];
        $scope.selectedSectionlist = [];
        $scope.get_report = function () {
            $scope.selectedClasslist = [];
            $scope.selectedSectionlist = [];
            debugger;
            $scope.allgrid = false;

            if (($scope.IssueFromDate == undefined && $scope.IssueFromDate == null) || ($scope.IssueToDate == undefined && $scope.IssueToDate == null)) {
                //$scope.IssueFromDate = "";
                //$scope.IssueToDate = "";
            }
            if (($scope.DueFromdate == undefined && $scope.DueFromdate == null) || ($scope.DueTodate == undefined && $scope.DueTodate == null)) {
                //$scope.DueFromdate = "";
                //$scope.DueTodate = "";
            }
            var fromdate1 = $scope.IssueFromDate == null ? "" : $filter('date')($scope.IssueFromDate, "yyyy-MM-dd");
            var todate1 = $scope.IssueToDate == null ? "" : $filter('date')($scope.IssueToDate, "yyyy-MM-dd");
            var fromdate2 = $scope.DueFromdate == null ? "" : $filter('date')($scope.DueFromdate, "yyyy-MM-dd");
            var todate2 = $scope.DueTodate == null ? "" : $filter('date')($scope.DueTodate, "yyyy-MM-dd");

            if ($scope.myForm.$valid) {
                angular.forEach($scope.classList, function (cls) {
                    if (cls.select == true) {
                        $scope.selectedClasslist.push({ asmcL_Id: cls.asmcL_Id });
                    }
                });

                angular.forEach($scope.sectionList, function (sect) {
                    if (sect.select == true) {
                        $scope.selectedSectionlist.push({ asmS_Id: sect.asmS_Id });
                    }
                });

                var data = {
                    //"Type": $scope.Type,
                    "AGType": $scope.AGType,
                    "TrnType": $scope.TrnType,
                    "IssueFromDate": fromdate1,
                    "IssueToDate": todate1,
                    "DueFromdate": fromdate2,
                    "DueTodate": todate2,
                    "LMAL_Id": $scope.LMAL_Id,
                    selectedClasslist: $scope.selectedClasslist,
                    selectedSectionlist: $scope.selectedSectionlist,
                }
                apiService.create("NonBookTransactionReport/get_report", data)
                    .then(function (promise) {
                        if (promise.reportlist.length > 0) {
                            $scope.reportlist = promise.reportlist;
                            if ($scope.AGType == 'all') {
                                $scope.allgrid = true;
                            }
                            else if ($scope.AGType == 'Student') {
                                $scope.allgrid = true;
                            }
                            else if ($scope.AGType == 'Staff') {
                                $scope.allgrid = true;
                            }
                            else if ($scope.AGType == 'Guest') {
                                $scope.allgrid = true;
                            }
                            else if ($scope.AGType == 'Department') {
                                $scope.allgrid = true;
                            }
                        }
                        else {
                            swal('Record is not Available!!');
                            $state.reload();
                        }                     

                        $scope.boxfalg = true;

                        $scope.printflag = true;
                    })
            }
            else {
                $scope.submitted = true;
            }

        }
        //-------------End--Get-Report


       



        //===========print===========//
        $scope.printData = function () {
            var innerContents = '';
            if ($scope.AGType == 'Student') {
                innerContents = document.getElementById("printtable1").innerHTML;
            }
            else if ($scope.AGType == 'Staff') {
                innerContents = document.getElementById("printtable2").innerHTML;
            }
            else if ($scope.AGType == 'Guest') {
                innerContents = document.getElementById("printtable3").innerHTML;
            }
            else if ($scope.AGType == 'Department') {
                innerContents = document.getElementById("printtable4").innerHTML;
            }
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/BookCirculationReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        //==============End==============//


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //--------------Clear-field.....
        $scope.Clearid = function () {
            $state.reload();
        }

        $scope.ExportToExcel = function () {
            var innerContents = '';
            if ($scope.AGType == 'Student') {
                innerContents = document.getElementById("printtable1").innerHTML;
                var exportHref = Excel.tableToExcel(printtable1, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else if ($scope.AGType == 'Staff') {
                innerContents = document.getElementById("printtable2").innerHTML;
                var exportHref = Excel.tableToExcel(printtable2, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else if ($scope.AGType == 'Guest') {
                innerContents = document.getElementById("printtable3").innerHTML;
                var exportHref = Excel.tableToExcel(printtable3, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else if ($scope.AGType == 'Department') {
                innerContents = document.getElementById("printtable4").innerHTML;
                var exportHref = Excel.tableToExcel(printtable4, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }

        }




        //////////=========================================================For House
        $scope.searchchkbx = "";
        $scope.all_check = function () {
            debugger;
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.classList, function (itm) {
                itm.select = checkStatus;
            });
            if ($scope.usercheck == false) {
                $scope.usercheck23 = "";
                angular.forEach($scope.sectionList, function (obj) {
                    obj.select = false;
                }); 
            }
            else if ($scope.usercheck == true) {
                angular.forEach($scope.sectionList, function (obj) {
                    obj.select = true;
                }); 
                $scope.togchkbx23();
            }
        }

        $scope.togchkbx = function () {
            debugger;
            $scope.usercheck = $scope.classList.every(function (options) {
                return options.select;
            });
            //if ($scope.usercheck == false) {
            //    angular.forEach($scope.sectionList, function (obj) {
            //        obj.select = true;
            //    });
            //    $scope.togchkbx23();
            //}
            //else if ($scope.usercheck == true) {
            //    angular.forEach($scope.sectionList, function (obj) {
            //        obj.select = false;
            //    });
            //}
        }

        $scope.isOptionsRequired = function () {
            return !$scope.classList.some(function (options) {
                return options.select;
            });
        }

        //$scope.filterchkbx = function (obj) {
        //    return (angular.lowercase(obj.className)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        //}

        //=============================================================== For Section

        $scope.searchchkbx23 = "";
        $scope.all_check23 = function () {
            var checkStatus = $scope.usercheck23;
            angular.forEach($scope.sectionList, function (itm) {
                itm.select = checkStatus;
            });
        }

        $scope.togchkbx23 = function () {
            $scope.usercheck23 = $scope.sectionList.every(function (options) {
                return options.select;
            });
        }

        $scope.isOptionsRequired23 = function () {
            return !$scope.sectionList.some(function (options) {
                return options.select;
            });
        }

        $scope.filterchkbx23 = function (obj) {
            return (angular.lowercase(obj.sectionName)).indexOf(angular.lowercase($scope.searchchkbx23)) >= 0;
        }

        //$scope.maxDatemf = new Date();
        $scope.gettodate = function () {

            $scope.minDatemf = new Date($scope.IssueFromDate);
            //$scope.maxDatemf = new Date();
        };


        $scope.gettodate23 = function () {

            $scope.minDatemf = new Date($scope.DueFromdate);
            //$scope.maxDatemf = new Date();
        };

    }
})();

