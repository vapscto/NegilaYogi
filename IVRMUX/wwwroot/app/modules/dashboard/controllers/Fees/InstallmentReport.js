(function () {
    'use strict';
    angular
.module('app')
.controller('InstallmentReportController', InstallmentReportController)
    InstallmentReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout','$filter']
    function InstallmentReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter) {
        $scope.exportsheet = false;
        $scope.IsHiddenup = true;
        $scope.IsHiddendown = true;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.grid_flag = false;
        $scope.file_type_flag = true;
        $scope.record_sorting_flag = true;
        $scope.exportsheet = false;
        $scope.printdatatable = [];
        $scope.propertyName = 'fmI_Installment_Type';
        $scope.search = "";


        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');


        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };


        $scope.sortBydropdown = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }


        $scope.Clearid = function () {
            $state.reload();
           // $scope.loadbasicdata();
        }
        $scope.exportToExcel = function (tableId) {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }

        }

        $scope.printData = function () {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                 '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.filterValue1, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            
            $scope.all = $scope.feeinstallmentlist.every(function (itm)
            { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }

        }


        $scope.ShowHideup = function () {
            
            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        }


       
        $scope.ShowHidedown = function () {


            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        }


       
        $scope.showreport = function () {
           
            apiService.getURI("FeeInstallmentDetails/getalldetails", 1).then(function (promise) {
                if (promise.installmentDatalist !=null)
                {
               
                    angular.forEach(promise.installmentDatalist, function (itm) {
                        itm.from_Date = $filter('date')(new Date(itm.from_Date), 'dd/MM/yyyy'); 
                        itm.to_Date = $filter('date')(new Date(itm.to_Date), 'dd/MM/yyyy');
                        itm.applicable_Date = $filter('date')(new Date(itm.applicable_Date), 'dd/MM/yyyy');
                        itm.due_Date = $filter('date')(new Date(itm.due_Date), 'dd/MM/yyyy');
                    })



                    $scope.feeinstallmentlist = promise.installmentDatalist;
                    $scope.totcountfirst = promise.installmentDatalist.length;
                    $scope.grid_flag = true;
                    $scope.exportsheet = true;
                    $scope.file_type_flag = false;
                    $scope.record_sorting_flag = false;
                }
                else{
                    swal("Record are not found");
                    $scope.grid_flag = false;
                    $scope.exportsheet = false;
                    $scope.file_type_flag = true;
                    $scope.record_sorting_flag = true;
                    }
               
            })
                
            }
        
        $("#tableExport").click(function (e) {
            window.open('data:application/vnd.ms-excel,' + $('#table1').html());
            e.preventDefault();
        });

    }

    })();