(function () {
    'use strict';
    angular
        .module('app')
        .controller('SalaryIncreementReportController', SalaryIncreementReportController)

    SalaryIncreementReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function SalaryIncreementReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object

        //Employeee
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.interacted = "false";
        $scope.month = true;

        $scope.SalaryFromDay = "";
        $scope.SalaryToDay = "";
        //$scope.startdate = new Date();
        //$scope.enddate = new Date();
        $scope.Obj = {};
        $scope.employeedropdown = [];
        $scope.exp_excel_flag = true;
        $scope.print_flag = true;

        //=====================================Load Data
        $scope.loaddata = function () {
            apiService.getURI("EmployeeSalaryIncreementProcess/getalldetails/", 1).then(function (promise) {
                $scope.employeedropdown = promise.employeelist;
                $scope.monthlist = promise.monthdropdown;
                $scope.leaveyeardropdown = promise.leaveyeardropdown;

            });
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        }




        $scope.getreport = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                var fromdate1;
                var todate1;
                var monthId;
                var yearId;
                var empID;
                if ($scope.allind == "All") {
                    empID = 0
                } else {
                    empID = $scope.Obj.hrmE_Id.hrmE_Id;
                }

                if ($scope.mallorindii == "MONTHWISE") {
                    monthId = $scope.Obj.hreS_Month;
                    yearId = $scope.Obj.hreS_Year;
                    var data = {
                        "Type": $scope.allind,
                        "Option": $scope.mallorindii,
                        "selected_hrmeID": empID,
                        "Month": monthId,
                        "Year": yearId
                    };

                    apiService.create("EmployeeSalaryIncreementProcess/getReport", data).
                        then(function (promise) {
                            if (promise.reportdata !== null && promise.reportdata.length > 0) {
                                $scope.get_Increment = promise.reportdata;
                            }
                            else {
                                swal("Record not found.");
                                $state.reload();
                            }
                        });
                }
                else {
                    monthId = 0;
                    yearId = 0;
                    fromdate1 = $filter('date')($scope.Obj.fromdate, "yyyy-MM-dd");
                    todate1 = $filter('date')($scope.Obj.todate, "yyyy-MM-dd");
                    var data = {
                        "Type": $scope.allind,
                        "Option": $scope.mallorindii,
                        "Fromdate": fromdate1,
                        "Todate": todate1,
                        "selected_hrmeID": empID,
                        "Month": monthId,
                        "Year": yearId,
                    };

                    apiService.create("EmployeeSalaryIncreementProcess/getReport", data).
                        then(function (promise) {
                            if (promise.reportdata !== null && promise.reportdata.length > 0) {
                                $scope.get_Increment = promise.reportdata;
                            }
                            else {
                                swal("Record not found.");
                                $state.reload();
                            }
                        });
                }
            }
            else {
                $scope.submitted = true;
            }

        }

        // ==========printdata===========
        //$scope.printData = function (printSectionId) {
        //    if ($scope.get_Increment !== null && $scope.get_Increment.length > 0) {
        //        var innerContents = document.getElementById("printSectionId").innerHTML;
        //        var popupWinindow = window.open('');
        //        popupWinindow.document.open();
        //        popupWinindow.document.write('<html><head>' +
        //            '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        //            '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
        //            '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
        //            '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
        //        );
        //        popupWinindow.document.close();
        //    }
        //    else {
        //        swal("Please Select Records to be Printed");
        //    }
        //}

        $scope.printData = function () {
            var divToPrint;
            if ($scope.rdopunch == 'LIEO') {
                divToPrint = document.getElementById("table_n");
            }
            else {
                divToPrint = document.getElementById("table");
            }
            var newWin = window.open("");
            newWin.document.write(divToPrint.outerHTML);
            newWin.print();
            newWin.close();
        };


        // ===========Excell===============
        //$scope.exportToExcel = function (tableId) {
        //    if ($scope.get_Increment !== null && $scope.get_Increment.length > 0) {
        //        var exportHref = Excel.tableToExcel(tableId, 'sheet name');
        //        $timeout(function () { location.href = exportHref; }, 100);
        //        //$state.reload();
        //    }
        //    else {
        //        swal("Please Select Records to be Exported");
        //    }

        //}

        $scope.exptoex = function () {
            var divToPrint;
            if ($scope.rdopunch == 'LIEO') {
                divToPrint = document.getElementById("table_n");
            }
            else {
                divToPrint = document.getElementById("table");
            }

            var blob = new Blob([divToPrint.outerHTML], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            saveAs(blob, "SalaryIncrementReport.xls");
        };




    }

})();