(function () {
    'use strict';
    angular
        .module('app')
        .controller('AvailableBooksReportStthomosController', AvailableBooksReportController)

    AvailableBooksReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter', '$timeout', 'Excel']
    function AvailableBooksReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter, $timeout, Excel) {

        $scope.submitted = false;
        $scope.tablediv = false;
        $scope.printd = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
        $scope.statuscount = false;
        $scope.all = false;
        $scope.alls = false;
        $scope.obj = {};
        $scope.usrname = localStorage.getItem('username');
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.usrname = localStorage.getItem('username');
        $scope.itemsPerPage = paginationformasters;
        if ($scope.itemsPerPage == undefined || $scope.itemsPerPage == null) {
            $scope.itemsPerPage = 5;
        }

        $scope.coptyright = copty;
        $scope.ddate = new Date();
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = "";
        $scope.imgname = logopath;
        $scope.searchchkbx = "";
        $scope.searchchkbxxx = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //-------------Load-data...
        $scope.loaddata = function () {
            $scope.getdata = [];
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;
            $scope.sublist = [];
            $scope.sublistone = [];
            $scope.sublistwo = [];
            $scope.sublisbacktwo = [];
            $scope.sublisbackone = [];
            $scope.sublisbacthree = [];
            apiService.getURI("AvailableBooksReport/getdetails", pageid).then(function (promise) {
                $scope.deptlist = promise.deptlist;
                $scope.msterliblist1 = promise.msterliblist1;
                $scope.temptable = promise.griddata;
                
                if (promise.griddata != null && promise.griddata.length > 0) {
                    angular.forEach(promise.griddata, function (itm1) {
                        if (itm1.lmS_Level == '1') {
                            $scope.sublistone.push({
                                LMS_SubjectName: itm1.lmS_SubjectName,
                                LMS_SubjectNo: itm1.lmS_SubjectNo,
                                LMS_Id: itm1.lmS_ParentId,
                            })
                        }
                    })
                    $scope.sublisbackone = $scope.sublistone;
                }
                
            })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
        //------------End-Load-data...

        //---------------Get-Report

        //
        $scope.OnClickAll = function () {
            $scope.getdata = [];
            angular.forEach($scope.sublist, function (dd) {
                dd.lmS_Ids = $scope.obj.all;
            });
        };
        $scope.isOptionsRequired1 = function () {
            return !$scope.sublist.some(function (options) {
                return options.lmS_Ids;
            });
        };
        //isOptionsRequired2
        $scope.isOptionsRequired2 = function () {
            return !$scope.sublistwo.some(function (options) {
                return options.lmS_Idss;
            });
        };
        $scope.obj.alls = false;
        $scope.obj.all = false;
        $scope.OnClickAlltwo = function () {
            $scope.getdata = [];
            angular.forEach($scope.sublistwo, function (dd) {
                dd.lmS_Idss = $scope.obj.alls;
            });
            $scope.OnselctSummarytwo();
        };
        $scope.OnselctSummary = function () {
            $scope.getdata = [];
            $scope.obj.alls = false;
            $scope.obj.all = false;
            if ($scope.lmS_Id > 0 && $scope.lmS_Id != 10000123) {
                $scope.sublistwo = [];
                angular.forEach($scope.temptable, function (dd) {
                    if (dd.lmS_ParentId == $scope.lmS_Id && dd.lmS_Level == '2') {
                        $scope.sublistwo.push({
                            LMS_SubjectName: dd.lmS_SubjectName,
                            LMS_SubjectNo: dd.lmS_SubjectNo,
                            LMS_Id: dd.lmS_Id,
                        })
                    }
                });
            }

        }
        //
        $scope.OnselctSummarytwo = function () {
            $scope.getdata = [];
            $scope.tempthirdTable = [];
            $scope.sublist = [];
            if ($scope.sublistwo != null && $scope.sublistwo.length > 0) {
                angular.forEach($scope.sublistwo, function (dd) {
                    if (dd.lmS_Idss == true) {
                        $scope.tempthirdTable.push({
                            LMS_Id: dd.LMS_Id
                        })
                    }
                });
            }
            if ($scope.tempthirdTable != null && $scope.tempthirdTable.length > 0) {
                angular.forEach($scope.temptable, function (itm1) {
                    if (itm1.lmS_Level == '3') {
                        angular.forEach($scope.tempthirdTable, function (dd) {
                            if (itm1.lmS_ParentId == dd.LMS_Id) {
                                $scope.sublist.push({
                                    lmS_SubjectName: itm1.lmS_SubjectName,
                                    lmS_Id: itm1.lmS_Id,
                                    lmS_SubjectNo: itm1.lmS_SubjectNo,
                                })
                            }
                        });
                    }

                });
            }
        }
        $scope.get_report = function () {
            $scope.getdata = [];
            if ($scope.myForm.$valid) {
                var data = {};
                $scope.stthomosSubject = [];
                if ($scope.sublist != null && $scope.sublist.length > 0) {
                    angular.forEach($scope.sublist, function (dd) {
                        if (dd.lmS_Ids == true) {
                            $scope.stthomosSubject.push({
                                LMS_SubjectName: dd.lmS_SubjectName,
                                LMS_Id: dd.lmS_Id,

                            })
                        }
                    });
                }
                else {
                    $scope.stthomosSubject = $scope.temptable;
                }

                if ($scope.statuscount == true) {
                    //sublist


                    var fromdate1 = $scope.IssueFromDate == null ? "" : $filter('date')($scope.IssueFromDate, "yyyy-MM-dd");
                    var todate1 = $scope.IssueToDate == null ? "" : $filter('date')($scope.IssueToDate, "yyyy-MM-dd");
                    var data = {
                        "Type": $scope.Type,
                        "Type2": $scope.Type2,
                        "Fromdate": fromdate1,
                        "ToDate": todate1,
                        "LMD_Id": $scope.lmD_Id,
                        "statuscount": $scope.statuscount,
                        "LMAL_Id": $scope.LMAL_Id,
                        "stthomosSubject": $scope.stthomosSubject,
                        "subjectsthomos": true,
                    }
                }
                else {
                    var data = {
                        "Type": $scope.Type,
                        "Type2": $scope.Type2,
                        // "Issue_Date": fromdate1,
                        //"IssueToDate": todate1,
                        "LMD_Id": $scope.lmD_Id,
                        "statuscount": $scope.statuscount,
                        "LMAL_Id": $scope.LMAL_Id,
                        "stthomosSubject": $scope.stthomosSubject,
                        "subjectsthomos": true,
                    }
                }



                apiService.create("AvailableBooksReport/get_report", data).then(function (promise) {
                    if (promise.griddata != null && promise.griddata.length > 0) {

                        $scope.getdata = promise.griddata;



                    }
                    else {
                        swal('Record Not Available!!!');
                        //$state.reload();
                    }
                })

            }
            else {
                $scope.submitted = true;
            }

        }
        //-------------End--Get-Report

        $scope.getdata = [];

        //===========print===========//
        $scope.printData = function () {
            var innerContents = document.getElementById("printtable").innerHTML;
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/BookArrivalReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        //==============End==============//

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

    
        $scope.exportToExcel = function (tabel1) {            var excelnamemain = "Available Books Report";            var printSectionId = '#printexcel';                        excelnamemain = excelnamemain + '.xls';            var exportHref = Excel.tableToExcel(printSectionId, 'Available Books Report');            $timeout(function () {                var a = document.createElement('a');                a.href = exportHref;                a.download = excelnamemain;                document.body.appendChild(a);                a.click();                a.remove();            }, 100);                   };

        //-----------clear-field
        $scope.Clearid = function () {
            $state.reload();
        }

        $scope.chagData = function () {
            $scope.getdata = [];
        }

    }
})();

