

(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeeThirdPartyReportController', FeeThirdPartyReportController123)

    FeeThirdPartyReportController123.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout']
    function FeeThirdPartyReportController123($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout) {


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

        $scope.interacted = function (field) {
            return $scope.submitted;
        };



        $scope.onclickloaddata = function () {
            if ($scope.betdates == "betdatesdisable") {

                $scope.betdatesdisable = false;
                $scope.betdatesdisable1 = false;
                $scope.asondatedisable = true;
            }
            else {
                $scope.betdatesdisable = true;
                $scope.betdatesdisable1 = true;
                $scope.asondatedisable = false;
            }
            if ($scope.rndind == "All") {

                $scope.IndivsualBC = false;

            }
            else {
                $scope.IndivsualBC = true;

            }


        };
        $scope.getTotal = function (int) {
            var total = 0;
            angular.forEach($scope.students, function (e) {
                total += e.paidamt;
            });
            return total;
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            apiService.getURI("FeeThirdPartyReport/getalldetails123", pageid).
                then(function (promise) {
                    $scope.userlistarray = promise.usersnameslist;
                    $scope.ledgerMbind = promise.ledgerlist
                    //    $scope.onclickloaddata();
                })
        }
        $scope.ShowReportdata = function () {


            if ($scope.myForm.$valid) {
                var frmdate;
                var todate;
                $scope.from_date = new Date($scope.FMCB_fromDATE).toDateString();
                $scope.to_date = new Date($scope.FMCB_toDATE).toDateString();
                var data = {
                    "fromdate": $scope.from_date,
                    "todate": $scope.to_date,
                    "acayid": $scope.academicyr,

                }
                apiService.create("FeeThirdPartyReport/getreport", data).
                    then(function (promise) {
                        if (promise.reportdatelist != null && promise.reportdatelist.length > 0) {
                            $scope.report = true;
                            $scope.export_flag = true;
                            $scope.print_flag = true;
                            $scope.students = promise.reportdatelist;
                            $scope.tot = $scope.getTotal(promise.reportdatelist);

                        }
                        else {
                            swal("No Record Found");
                            $scope.report = false;
                            $scope.export_flag = false;
                            $scope.print_flag = false;

                        }


                    })
            }
            else {
                $scope.submitted = true;

            }

        }

        $scope.Clear_Details = function () {
            $state.reload();
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


        $scope.toggleAll = function () {
            debugger;
            $scope.printdatatable = [];
            var toggleStatus = $scope.all;
            angular.forEach($scope.students, function (itm) {
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


        $scope.get_total_student_print = function () {
            var totA_p = 0;
            var totB_p = 0;
            angular.forEach($scope.printdatatable, function (gp) {
                totA_p += gp.paidamt;
            })
            $scope.totA_p = totA_p;

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

    }
})();

