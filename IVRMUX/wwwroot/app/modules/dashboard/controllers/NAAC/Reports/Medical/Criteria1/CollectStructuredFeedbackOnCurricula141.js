﻿(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollectStructuredFeedbackOnCurricula141', CollectStructuredFeedbackOnCurricula141)

    CollectStructuredFeedbackOnCurricula141.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function CollectStructuredFeedbackOnCurricula141($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {


        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        //$scope.searchValue23 = "";

        $scope.showflag = false;

        $scope.sortReverse = true;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }


        $scope.cancel = function () {
            $state.reload();
        }
        $scope.loaddata = function ()
        {

            var pageid = 2;
            apiService.getURI("Medical_Criteria1Reports/getdata", pageid).then(function (promise) {

                $scope.getinstitutioncycle = promise.getinstitutioncycle;
                $scope.cycleid = promise.getinstitutioncycle[0].cycleid;
                $scope.getparentidzero = promise.getinstitution;

                $scope.naacsL_InstitutionTypeFlg = promise.naacsL_InstitutionTypeFlg;

                angular.forEach($scope.getparentidzero, function (options) {
                    options.select = true;
                })
            })
        }




        $scope.togchkbx = function () {
            $scope.getparentidzero.every(function (options) {
                return options.select;
            });
        }


        $scope.exportToExcel = function (table) {

            var excelname = 'Cat 1.4.1.xls';
            var exportHref = Excel.tableToExcel(table, '1.4.1');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        }

        //========================print details
        $scope.printData = function () {
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/ExchangableCoursePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        $scope.ddate = new Date();
        //$scope.usrname = localStorage.getItem('username');
        //var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //var paginationformasters;
        //var copty;
        //var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //if (ivrmcofigsettings.length > 0) {
        //    paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
        //    copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        //}
        //$scope.coptyright = copty;
        //var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        //if (admfigsettings.length > 0) {
        //    var logopath = admfigsettings[0].asC_Logo_Path;
        //}
        //$scope.imgname = logopath;

        $scope.isOptionsRequired = function () {
            return !$scope.getparentidzero.some(function (options) {
                return options.select;
            });
        }

        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //======================report.
        $scope.get_report = function () {
            $scope.showflag = false;

            $scope.selected_Inst = [];
            angular.forEach($scope.getparentidzero, function (mm) {
                if (mm.select) {
                    $scope.selected_Inst.push(mm);
                }
            })

            if ($scope.myForm.$valid) {
                var data = {
                    //selectedYear: $scope.selectedYear,
                    "cycleid": $scope.cycleid,
                    selected_Inst: $scope.selected_Inst,
                }
                apiService.create("Medical_Criteria1Reports/report_MC_141", data).then(function (promise) { 

                    
                   // $scope.reportlist2 = promise.reportlist2;

                    if (promise.reportlist.length > 0) {
                        //angular.forEach($scope.reportlist, function (tt) {
                        //    $scope.mainArray = [];
                        //    //var count = 0;
                        //    angular.forEach($scope.reportlist2, function (ss) {
                        //        if (tt.NCACMPR112_Id == ss.ncacpR112_Id) {
                        //            $scope.mainArray.push(ss);
                        //        }
                        //    })
                        //    tt.listdata = $scope.mainArray;
                        //})
                        $scope.reportlist = promise.reportlist;
                        console.log($scope.reportlist);
                        $scope.showflag = true;

                    }
                    else {
                        $scope.showflag = false;
                        swal('Record Not Found!');
                    }    
                })
            }
            else {
                $scope.submitted = true;
            }
        }



    }
})();
