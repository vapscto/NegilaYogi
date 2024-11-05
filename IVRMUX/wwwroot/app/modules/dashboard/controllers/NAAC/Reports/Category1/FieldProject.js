﻿(function () {
    'use strict';
    angular
        .module('app')
        .controller('FieldProjectController', FieldProjectController)
    FieldProjectController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function FieldProjectController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sortReverse = true;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.printflag = false;
        $scope.ListofItems = [];
        $scope.ListofItems = [{ name: "BC0031", program: "Program-E", certificateName: "Automation Certificate", year: "2018-2019" },
        { name: "BC0032", program: "Program-D", certificateName: " Certificate-D", year: "2017-2018" },
        { name: "BC0033", program: "Program-C", certificateName: " Certificate-C", year: "2016-2017" },
        { name: "BC0034", program: "Program-B", certificateName: " Certificate-B", year: "2015-2016" },
        { name: "BC0035", program: "Program-A", certificateName: " Certificate-A", year: "2014-2015" },
        ]
        $scope.exportToExcel = function (table) {
            var excelname = 'Cat 1.3.3.xls';
            var exportHref = Excel.tableToExcel(table, '1.3.3');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        }
        $scope.loaddata = function () {
            var pageid = 0;
            $scope.printflag = false;
            apiService.getURI("CurricularAspects/getdata", pageid).then(function (promise) {
                debugger;
                $scope.getinstitutioncycle = promise.getinstitutioncycle;
                $scope.cycleid = promise.getinstitutioncycle[0].cycleid;
                $scope.getparentidzero = promise.getinstitution;
                angular.forEach($scope.getparentidzero, function (pp) {
                    pp.select = true;
                })
            })
        }
        $scope.get_selcetYear = function (data) {

            var nofyear = Number($scope.noofyear);
            angular.forEach($scope.yearlist, function (tt) {
                tt.select = false;
            })
            var s = 0;
            angular.forEach($scope.yearlist, function (pp) {
                if (s < nofyear) {
                    pp.select = true;
                }
                s += 1;
            })
        }
        $scope.togchkbx = function () {
            $scope.noofyear = 0;
            angular.forEach($scope.yearlist, function (ff) {
                if (ff.select == true) {
                    $scope.noofyear += 1;
                }
            })
        }
        $scope.cancel = function () {
            $state.reload();
        }
        $scope.printData = function () {
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ExchangableCoursePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length !== 0 && ivrmcofigsettings.length !== undefined) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        var logopath = "";
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length !== 0 && admfigsettings.length !== undefined) {
            logopath = admfigsettings[0].asC_Logo_Path;
        } else {
            logopath = "";
        }
        $scope.imgname = logopath;
        //======================report.
        $scope.showflag = false;
        $scope.showdetails = function () {
            $scope.selected_Inst = [];
            angular.forEach($scope.getparentidzero, function (yy) {
                if (yy.select) {
                    $scope.selected_Inst.push(yy);
                }
            })
            if ($scope.myForm.$valid) {
                var data = {
                    "cycleid": $scope.cycleid,
                    selected_Inst: $scope.selected_Inst,
                }
                apiService.create("CurricularAspects/get_report_133", data).then(function (promise) {
                    $scope.reportlist = promise.reportlist;
                    $scope.reportlist2 = promise.reportlist2;
                    if (promise.reportlist.length > 0) {
                        angular.forEach($scope.reportlist, function (tt) {
                            $scope.mainArray = [];
                            angular.forEach($scope.reportlist2, function (ss) {
                                if (tt.NCACSPR133_Id == ss.ncacspR133_Id) {
                                    $scope.mainArray.push(ss);
                                }
                            })
                            tt.listdata = $scope.mainArray;
                        })
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
        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();
