(function () {
    'use strict';
    angular
        .module('app')
        .controller('AmountEntryReportController', AmountEntryReportController)

    AmountEntryReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function AmountEntryReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {

       
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.loaddata = function () {
            $scope.Grid_View = false;
            var pageid = 1;
                     
            apiService.getURI("AmountEntryReport/getalldetails", pageid).
                then(function (promise) {
                    $scope.colcategory = promise.category;
                    $scope.colquota = promise.quota;
                    $scope.colsemisterlist = promise.semisterlist;
                    $scope.coladmyr = promise.yearlst;
                    $scope.colgroup = promise.grouplist;

                })
        }


        $scope.hrdallcheck = function () {
            var toggleStatus1 = $scope.checkallhrd;
            angular.forEach($scope.colsemisterlist, function (itm) {
                itm.selected = toggleStatus1;
            });
        }

        $scope.hrgsinglecheck = function () {
            $scope.checkallhrd = $scope.colsemisterlist.every(function (itm) {
                return itm.selected;
            });
        };

        $scope.onselectcat = function (obj) {
            var data = {
                "AMCOC_Id": $scope.amcoC_Id,
            }
            apiService.create("AmountEntryReport/get_courses", data).
                then(function (promise) {
                    $scope.coursecount = promise.courselist;
                })
        };

        $scope.get_branches = function (obj) {

            var AMCO_Ids = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
            }
            apiService.create("AmountEntryReport/get_branches", data).
                then(function (promise) {
                    $scope.branchcount = promise.branchlist;
                    angular.forEach($scope.branchcount, function (fy) {
                        fy.selectedbranch = true;
                    })
                    $scope.binddatagrp();
                })

        };

        $scope.get_total_student_print = function () {
            var totA_p = 0;
            var totB_p = 0;
            angular.forEach($scope.printdatatable, function (gp) {
                totA_p += gp.fcsA_Amount;
            })
            $scope.totA_p = totA_p;
        }

        $scope.clear_feedef = function () {
            $state.reload();
        }

        $scope.toggleAll = function () {
            debugger;
            $scope.printdatatable = [];
            var toggleStatus = $scope.all;
            angular.forEach($scope.reportdetails1, function (itm) {
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

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            debugger;
            $scope.all = $scope.reportdetails1.every(function (itm) { return itm.selected; });

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

        $scope.ShowReport = function () {
            var totA_p = 0;
            var AMCO_Ids = [];
            var AMB_Ids = [];
            var FMG_Ids = [];

            angular.forEach($scope.colquota, function (ty) {
                if (ty.selectedqt) {
                    FMG_Ids.push(ty.acQ_Id);
                }
            })
            angular.forEach($scope.branchcount, function (ty) {
                if (ty.selectedbranch) {
                    AMB_Ids.push(ty.amB_Id);
                }
            })

            var semid = [];
            angular.forEach($scope.colsemisterlist, function (crs) {
                if (crs.selected) {

                    semid.push(crs.amsE_Id);
                }
            })

            if ($scope.myForm.$valid) {

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    AMB_Ids: AMB_Ids,
                    FMG_Ids: FMG_Ids,
                    "FMG_Id": $scope.fmG_Id,
                   // "AMSE_Id": $scope.amsE_Id,
                    "AMCOC_Id": $scope.amcoC_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    AMSE_Ids: semid
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("AmountEntryReport/radiobtndata", data).
                    then(function (promise) {
                        if (promise.savedrecord != null && promise.savedrecord != "") {
                            $scope.reportdetails = promise.savedrecord;
                            $scope.Grid_View = true;
                            $scope.export_flag = true;
                            $scope.print_flag = true;

                            angular.forEach($scope.reportdetails, function (gp) {
                                totA_p += gp.fcsA_Amount;
                            })
                            $scope.totA_p = totA_p;
                        }
                        else {
                            swal("No Record Found");
                            $scope.export_flag = false;
                            $scope.print_flag = false;
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



        $scope.isOptionsRequired_brch = function () {
            return !$scope.branchcount.some(function (options) {
                return options.selectedbranch;
            });
          

        }
        $scope.isOptionsRequired_qt = function () {
            return !$scope.colquota.some(function (options) {
                return options.selectedqt;
            });
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
          //  $state.reload();

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
               // $state.reload();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }


       
    }
})();