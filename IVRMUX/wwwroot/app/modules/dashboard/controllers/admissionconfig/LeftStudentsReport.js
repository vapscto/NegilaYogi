﻿(function () {
    'use strict';
    angular.module('app').controller('LeftStudentsReportController', LeftStudentsReportController)

    LeftStudentsReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$timeout', 'Excel', '$q']
    function LeftStudentsReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $timeout, Excel, $q) {



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
            apiService.getURI("LeftStudentsReport/loaddata", pageid).then(function (promise) {

                $scope.academic = promise.academic;


            });
        };


        //

        // Academic Year

        $scope.allcheck1 = function () {

        

        $scope.academicyear = [];
        $scope.getCategory = function () {

            angular.forEach($scope.academic, function (aa) {
                if (aa.selected == true) {
                    $scope.academicyear.push({ ASMAY_Id: aa.asmaY_Id })
                }

            });


            var data = {
                "academicyears": $scope.academicyear
            }

            apiService.create("LeftStudentsReport/getCategory", data).then(function (promise) {

                $scope.category = promise.category;


            });
        };



        //


        //
        //Category


        $scope.allcheck2 = function () {

        $scope.catelist = [];
        $scope.getClass = function () {

            angular.forEach($scope.academic, function (aa) {
                if (aa.selected == true) {
                    $scope.academicyear.push({ ASMAY_Id: aa.asmaY_Id })
                }

            });

            angular.forEach($scope.category, function (aa) {
                if (aa.selected == true) {
                    $scope.catelist.push({ AMC_Id: aa.amC_Id })
                }

            });


            var data = {
                "academicyears": $scope.academicyear,
                "categorylists": $scope.catelist
            }

            apiService.create("LeftStudentsReport/getClass", data).then(function (promise) {

                $scope.classlist = promise.classlist;


            });
        };


        //

        

        //
        //Class

        $scope.allcheck3 = function () {

        $scope.classlistarray = [];
        $scope.getsection = function () {


            angular.forEach($scope.academic, function (aa) {
                if (aa.selected == true) {
                    $scope.academicyear.push({ ASMAY_Id: aa.asmaY_Id })
                }

            });

            angular.forEach($scope.category, function (aa) {
                if (aa.selected == true) {
                    $scope.catelist.push({ AMC_Id: aa.amC_Id })
                }

            });


            angular.forEach($scope.classlist, function (aa) {
                if (aa.selected == true) {
                    $scope.classlistarray.push({ ASMCL_Id: aa.asmcL_Id })
                }

            });

            if ($scope.classlistarray != null) {
                $scope.classflag = true;
            }

            var data = {
                "academicyears": $scope.academicyear,
                "categorylists": $scope.catelist,
                "classlsttwo": $scope.classlistarray
            }
            apiService.create("LeftStudentsReport/getsection", data).then(function (promise) {
                if (promise.sectionlist.length > 0 || promise.sectionlist != null) {
                    $scope.sectionlist = promise.sectionlist;
                    $scope.getclass = false;
                }
                else {
                    swal('No data Found!!!');
                }
            });
        }

        //

        
        //
        //Section

        $scope.allcheck4 = function () {




        //
       
        
  
        $scope.get_Report = function () {
            $scope.sectionlistarray = [];
            angular.forEach($scope.sectionlist, function (itm) {
                }


            $scope.academiclistarray = [];
            angular.forEach($scope.academic, function (itm) {
                }

            $scope.catelist = [];
            angular.forEach($scope.category, function (aa) {
                if (aa.selected == true) {
                    $scope.catelist.push({ AMC_Id: aa.amC_Id })
                }
            });

            $scope.classlistarray = [];
            angular.forEach($scope.classlist, function (aa) {
                if (aa.selected == true) {
                    $scope.classlistarray.push({ ASMCL_Id: aa.asmcL_Id })
                }
            });

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
 
                var data = {
                    "academicyears": $scope.academiclistarray,
                    "categorylists": $scope.catelist,
                    "classlsttwo": $scope.classlistarray,
                    "sectionlistarray": $scope.sectionlistarray  
                }

                apiService.create("LeftStudentsReport/report", data).then(function (promise) {
                    if (promise.viewlist != null && promise.viewlist.length > 0) {

                        $scope.viewlist = promise.viewlist;


                    }

                    else {
                        swal("No Record  Found..... !!");
                        $scope.viewlist = promise.viewlist;

                    }

                });

            }

            else {
                $scope.submitted = true;
            }

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