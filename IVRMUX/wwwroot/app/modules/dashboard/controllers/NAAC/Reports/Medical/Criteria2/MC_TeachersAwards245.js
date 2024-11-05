﻿(function () {
    'use strict';
    angular
        .module('app')
        .controller('MC_TeachersAwards245', MC_TeachersAwards245)

    MC_TeachersAwards245.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function MC_TeachersAwards245($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {




        $scope.printflag = false;
        $scope.showflag = false;

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        //$scope.searchValue23 = "";

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
            })
        }


       

        $scope.togchkbx = function () {
            $scope.getparentidzero.every(function (options) {
                return options.select;
            });
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
        $scope.submitted = false;
        $scope.get_report = function () {
            $scope.showflag = false;
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
                
                apiService.create("Medical_Criteria2Reports/MC_245_Report", data).then(function (promise) {                  
                    debugger;
                    if (promise.reportlist.length > 0) {

                        $scope.reportlist = promise.reportlist;

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

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.isOptionsRequireddept = function () {
            return !$scope.deptlist.some(function (options) {
                return options.select;
            });
        }

        $scope.isOptionsRequireddesg = function () {
            return !$scope.desglist.some(function (options) {
                return options.select23;
            });
        }
       
        //======================For Excel sheet
        $scope.exportToExcel = function (table) {

            var excelname = 'Cat 2.4.5.xls';
            var exportHref = Excel.tableToExcel(table, '2.4.5');
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


        $scope.togchkbxdept = function () {

            $scope.selecteddept = [];
            angular.forEach($scope.deptlist, function (ff) {
                if (ff.select == true) {
                    $scope.selecteddept.push(ff);
                }
            });
            var data = {
                "selecteddept": $scope.selecteddept,
            }

            apiService.create("Medical_Criteria2Reports/get_desination", data).then(function (promise) {

                $scope.desglist = promise.desglist;

            });
        };


        

        $scope.searchchkbx23 = "";
        $scope.all_check23 = function () {
            var checkStatus = $scope.usercheck23;
            angular.forEach($scope.deptlist, function (itm) {
                itm.select = checkStatus;
            });
            if ($scope.usercheck23 == false) {
                $scope.usercheck23 = "";
                $scope.desglist = [];
            }
            else if ($scope.usercheck23 == true) {
                $scope.togchkbxdept();
                $scope.usercheck23 = true
            }
        }
        $scope.togchkbx23 = function () {
            $scope.usercheck23 = $scope.deptlist.every(function (options) {
                return options.select;
            });
        }

        $scope.all_checkDesg = function () {
            var checkStatusdesg = $scope.usercheck;
            angular.forEach($scope.desglist, function (itm) {
                itm.select23 = checkStatusdesg;
            });
        }

        $scope.togchkbxDesg = function () {            
            $scope.usercheck = $scope.desglist.every(function (options) {
                return options.select23;
            });
        }

    }
})();

