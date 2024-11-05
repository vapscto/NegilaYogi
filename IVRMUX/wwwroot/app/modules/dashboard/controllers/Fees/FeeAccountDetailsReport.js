(function () {
    'use strict';
    angular
        .module('app')
        .controller('DailyFeeCollReportController', DailyFeeCollReportController)
    DailyFeeCollReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache']
    function DailyFeeCollReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {

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

        $scope.loaddata = function () {

            var pageid = 1;
            apiService.getURI("DailyFeeCollReport/getalldetails", pageid).
                then(function (promise) {
                    $scope.yearlst = promise.fillyear;
                   
         })
        }


        $scope.total1 = function (e) {
            var totalc = 0;
            angular.forEach($scope.students, function (e) {
                totalc += e.Netamount;
            });
            return totalc;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.ShowReportdata = function () {

            if ($scope.myForm.$valid) {
                $scope.printdatatable = [];
                $scope.from_date = new Date($scope.FMCB_fromDATE).toDateString();
                $scope.to_date = new Date($scope.FMCB_toDATE).toDateString();
                    var data = {
                        "AMAY_Id": $scope.ASMAY,
                        "Fromdate": $scope.from_date,
                        "Todate": $scope.to_date,
                    }
                
                    apiService.create("DailyFeeCollReport/FeeAccountDetailsReport", data).
                    then(function (promise) {
                        debugger;
                        if (promise.alllist != null && promise.alllist != "") {
                                $scope.Grid_view = true;
                                $scope.export_flag = true;
                                $scope.print_flag = true;
                                $scope.totcountfirst = promise.alllist.length;
                                $scope.students = promise.alllist;
                                $scope.totA = $scope.total1(promise.alllist);
                               
                               

                            }
                            else {
                                swal("No Record Found");
                                $scope.Grid_view = false;
                                $scope.export_flag = false;
                                $scope.print_flag = false;
                                //$scope.file_disable = true;
                            }
                        
                    })
            }
            else {
                $scope.submitted = true;

            }
        }
        $scope.clear_feeconss = function () {
            debugger;
            $state.reload();


        }

        $scope.toggleAll = function () {
            debugger;
            $scope.printdatatable = [];
            var toggleStatus = $scope.all;
            angular.forEach($scope.students1, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
            $scope.get_total_student_print();

        }

        $scope.printdatatable = [];
        $scope.exportToExcel = function (table1) {
            debugger;
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(table1, 'WireWorkbenchDataExport');

                $timeout(function () { location.href = exportHref; }, 100);
                $timeout(function () {
                    location.href = exportHref;
                }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }
            //$state.reload();

        }

        $scope.printData = function (printSectionId) {
            debugger;
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

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            debugger;
            $scope.all = $scope.students.every(function (itm) { return itm.selected; });

            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatable.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
            $scope.get_total_student_print();
        }

        $scope.get_total_student_print = function () {
            var totA_p = 0;
            angular.forEach($scope.printdatatable, function (gp) {
                totA_p += gp.FTP_Paid_Amt;
              
            })
            $scope.totA_p = totA_p;
          
        }
    }
})();
