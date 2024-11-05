(function () {
    'use strict';
    angular
        .module('app')
        .controller('OpeningBalancetReportController', openingBalancetReportController)

    openingBalancetReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache']
    function openingBalancetReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {

        $scope.pagesrecord = {};
        $scope.currentPage = 1;
        //  $scope.itemsPerPage = 10;

        $scope.table_flag = false;
        $scope.report_flag = false;

        $scope.ddate = {};
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
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        //$scope.itemsPerPage = 10;



        $scope.printdatatable = [];
        $scope.sortBy = function (propertyName) {

            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.exportToExcel = function (tableId) {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select Records to be Exported");
            }

        }
        $scope.toggleAll = function () {

            var toggleStatus = $scope.all2;
            angular.forEach($scope.filterValue1, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all2 == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
        }

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (obj.regno).indexOf($scope.searchValue) >= 0 || (obj.admno).indexOf($scope.searchValue) >= 0 || (obj.stuFN).indexOf($scope.searchValue) >= 0 || (obj.achivement).indexOf($scope.searchValue) >= 0;
        }



        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all2 = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }

        }

        $scope.interacted = function (field) {

            return $scope.submitted;
        };



        $scope.onpageload = function () {
            var pageid = 1;
            apiService.getURI("INV_R_GRN/getdata_ob", pageid).
                then(function (promise) {
                    $scope.year_list_ob = promise.year_list_ob;
                    $scope.store_list_ob = promise.store_list_ob;
               
                   
                })
        }


        $scope.report123 = false;
        $scope.excel_flag = true;
        $scope.submitted = false;




        //$scope.Report = function (optradio) {
        $scope.Report = function () {
        
            $scope.submitted = false;
            if ($scope.myForm.$valid) {
                var data = {
                    "IMFY_Id": $scope.imfY_Id,
                    "INVMST_Id": $scope.invmsT_Id,
                  
                }


                apiService.create("INV_R_GRN/GetReport_ob", data)
                    .then(function (promise) {
                        if (promise.ob_report_list.length > 0) {
                            $scope.ob_report_list = promise.ob_report_list;
                            $scope.presentCountgrid = $scope.ob_report_list.length;
                            $scope.ob_report_list1 = promise.ob_report_list;
                            $scope.report123 = true;
                        }
                    }
                    );
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.exportToExcel = function (export_id) {
            var exportHref = "";
            exportHref = Excel.tableToExcel(export_id, 'printARL');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };

        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printARL").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();

        };

        var studclear = [];
        $scope.Clearid = function () {
            $state.reload();
            $scope.excel_flag = true;
            $scope.asmaY_Id = "";
            $scope.asmcL_Id = "";
            $scope.asmC_Id = "";
            $scope.amsTEC_Id = "";
            $scope.amsT_Id = "";
            $scope.report123 = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }


        $scope.All_Individual = function () {
            $scope.angularData =
                {
                    'nameList': []
                };

            $scope.vals = [];


            if ($scope.Admnoallind == "All") {
               
                $scope.invmsT_Id = undefined;
                $scope.report_flag = false;
                $scope.stud_name = false;
                $scope.ach_name = false;
                $scope.submitted = false;
                $scope.report123 = false;

                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }

            if ($scope.Admnoallind == "Indi") {
                $scope.asmaY_Id = "";
                $scope.asmcL_Id = "";
                $scope.asmC_Id = "";
                $scope.amsT_Id = "";
                $scope.amsTEC_Id = "";
                $scope.stud_name = true;
                $scope.ach_name = true;
                $scope.report_flag = false;
                $scope.submitted = false;
                $scope.report123 = false;

                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
        }




     




    
    }
})();

