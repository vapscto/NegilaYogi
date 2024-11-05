(function () {
    'use strict';
    angular.module('app').controller('AttendanceRunController', AttendanceRunController)

    AttendanceRunController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$timeout', 'Excel', '$q']
    function AttendanceRunController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $timeout, Excel, $q) {



        $scope.submitted1 = false;
        $scope.indentapproveddetais = [];
        $scope.maxdate = new Date();
        $scope.obj = {};
        var data = {}
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        var temp = [];
        var year = "";
        $scope.yearfromdate = "";
        $scope.monthlist_temp = [];
        $scope.consolidata_id = [];
        $scope.get_deviationreport = [];
        $scope.imgname = logopath;
        $scope.data = [];


        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            $scope.flags = "";

            var pageid = 2;
            apiService.getURI("AttendanceRun/loaddata", pageid).then(function (promise) {

                $scope.academic = promise.academic;


            });
        };


        $scope.SaveData = function () {
            
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.from_date = new Date($scope.Fromdate).toDateString();

                var data = {
                    "ASMAY_Id": $scope.obj.ASMAY_Id,
                    "Date": $scope.from_date
                }

                apiService.create("AttendanceRun/savedetails", data).then(function (promise) {
                    if (promise.viewlist != null && promise.viewlist.length > 0) {

                        $scope.viewlist = promise.viewlist;
                        $scope.griddatafunction();

                    }

                    else {
                        swal("No Record  Found..... !!");

                    }

                });

            }

            else {
                $scope.submitted = true;
            }

        }


        $scope.griddatafunction = function () {
            $scope.griddata = [];
            angular.forEach($scope.viewlist, function (itm) {                                   $scope.griddata.push({ ASMS_Id: itm.ASMS_Id, ASMCL_Id: itm.ASMCL_Id });
                        });
           

                var data = {
                    "ASMAY_Id": $scope.obj.ASMAY_Id,
                    "studentlist": $scope.griddata,
                    "Date": $scope.from_date
                }

                apiService.create("AttendanceRun/griddetails", data).then(function (promise) {
                    if (promise.griddto != null && promise.griddto.length > 0) {

                        $scope.griddto = promise.griddto;
                 

                    }

                    else {
                        swal("No Record  Found..... !!");

                    }

                });

           
           

        }



        
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.exportToExceldetails = function (export_id) {
            var exportHref = Excel.tableToExcel(export_id, 'Student Report');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };

        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printDeviation").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };


    }
})();