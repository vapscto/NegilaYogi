﻿(function () {
        'use strict';
        angular
            .module('app')
            .controller('MC_ExpOFullTimeTeacher243', MC_ExpOFullTimeTeacher243)

        MC_ExpOFullTimeTeacher243.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
        function MC_ExpOFullTimeTeacher243($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {            

            $scope.printflag = false;
            $scope.showflag = false;

            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
         

            $scope.sortReverse = true;
            $scope.sort = function (key) {
                $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
                $scope.sortKey = key;
            }


            $scope.loaddata = function () {
                $scope.printflag = false;
                $scope.showflag = false;
                var pageid = 2;
                apiService.getURI("Medical_Criteria2Reports/getdata", pageid).then(function (promise) {               
                    $scope.getinstitutioncycle = promise.getinstitutioncycle;
                    $scope.cycleid = promise.getinstitutioncycle[0].cycleid;
                    $scope.getparentidzero = promise.getinstitution;

                    $scope.naacsL_InstitutionTypeFlg = promise.naacsL_InstitutionTypeFlg;

                    angular.forEach($scope.getparentidzero, function (options) {
                        options.select = true;
                    })

                    //$scope.yearlist = promise.yeardata;
                    //var s = 0;
                    //angular.forEach($scope.yearlist, function (pp) {
                    //    if (s < $scope.noofyear) {
                    //        pp.select = true;
                    //    }
                    //    s += 1;
                    //})

                })
            }

            //=========================year selection
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
            //=================single checkbox selection
            $scope.togchkbx = function () {
                $scope.noofyear = 0;
                angular.forEach($scope.yearlist, function (ff) {
                    if (ff.select == true) {
                        $scope.noofyear += 1;
                    }
                })
            }

            //============================For Print Option
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

            //======================Get Report.
            $scope.get_report = function () {
                $scope.showflag = false;
                //$scope.selectedYear = [];
                //angular.forEach($scope.yearlist, function (yy) {
                //    if (yy.select) {
                //        $scope.selectedYear.push(yy);
                //    }
                //})

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
                    apiService.create("Medical_Criteria2Reports/MC_243_Report", data).then(function (promise) {
                        $scope.reportlist = promise.reportlist;
                        if ($scope.reportlist.length > 0) {
                            $scope.showflag = true;
                            $scope.printflag = true;
                        }
                        else {
                            $scope.showflag = false;
                            $scope.printflag = false;
                            swal('Records Not Found!');
                        }
                    })
                }
                else {
                    $scope.submitted = true;
                }               
            }
            //======================For Excel sheet
            $scope.exportToExcel = function (table) {

                var excelname = 'Cat 2.4.3.xls';
                var exportHref = Excel.tableToExcel(table, '2.4.3');
                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = excelname;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);
            }


            //======================for Cancel Button
            $scope.cancel = function () {
                $state.reload();
            }
            $scope.submitted = false;
            $scope.interacted = function (field) {
                return $scope.submitted;
            };

            $scope.isOptionsRequired = function () {
                return !$scope.yearlist.some(function (options) {
                    return options.select;
                });
            }



        }
    })();
