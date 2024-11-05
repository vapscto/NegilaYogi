

(function () {
    'use strict';
    angular
        .module('app')
        .controller('BookArrivalReportSthomosController', BookArrivalReportController)

    BookArrivalReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$filter', 'Excel', '$timeout']
    function BookArrivalReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter, Excel, $timeout) {

        $scope.submitted = false;
        $scope.tablediv = false;;
        $scope.printd = false;
        $scope.searchvalue = '';

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
        $scope.imgname = "";
        $scope.imgname = logopath;
        $scope.obj = {};
        $scope.temptable = [];
        $scope.sublistone = [];
        //---------------Load --data
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 15;
            $scope.search = "";
           
            var pageid = 2;
            apiService.getURI("BookArrivalReport/getdetails", pageid).then(function (promise) {

                /*$scope.donorlist = promise.donorlist;*/

                $scope.deptlist = promise.deptlist;

                $scope.lib_list = promise.lib_list;
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
        //------------End---Load --data




        //---------------Get Repoet....
        $scope.get_report = function () {
           

            var fromdate = $scope.Fromdate == null ? "" : $filter('date')($scope.Fromdate, "yyyy-MM-dd");
            var todate = $scope.ToDate == null ? "" : $filter('date')($scope.ToDate, "yyyy-MM-dd");

           

            if ($scope.myForm.$valid) {
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
                var data = {
                    "Type": $scope.Type,
                    "Type2": $scope.Type2,
                    "LMD_Id": $scope.lmD_Id,
                    "Fromdate": fromdate,
                    "ToDate": todate,
                    "LMAL_Id": $scope.LMAL_Id,
                    "BookArrival": true,
                    "BookSummary": $scope.stthomosSubject,
                }
                apiService.create("BookArrivalReport/get_report", data).then(function (promise) {

                    if (promise.reportlist.length > 0) {
                        $scope.reportlist = promise.reportlist;



                    }
                    else {
                        swal("Record Not Found!");
                        $scope.tablediv = false;
                        $scope.printd = false;
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        }
        //-------------End---Get Repoet....

        $scope.reportlist = [];


        //===========print===========//
        $scope.printdata = function () {
            var innerContents = document.getElementById("printtable").innerHTML;
            var popupWinindow = window.open('');
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


        //===========----Clear Field
        $scope.Clearid = function () {

            $state.reload();
        }

        $scope.ExportToExcel = function (tableId) {
            //var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            //$timeout(function () { location.href = exportHref; }, 100);
            var excelnamemain = "Arrival Books Report";            var printSectionId = '#printexcel';            excelnamemain = excelnamemain + '.xls';            var exportHref = Excel.tableToExcel(printSectionId, 'Arrival Books Report');            $timeout(function () {                var a = document.createElement('a');                a.href = exportHref;                a.download = excelnamemain;                document.body.appendChild(a);                a.click();                a.remove();            }, 100);
        }

        //Summary
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
            $scope.obj.alls = false;
            $scope.obj.all = false;
            $scope.getdata = [];
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


    }
})();

