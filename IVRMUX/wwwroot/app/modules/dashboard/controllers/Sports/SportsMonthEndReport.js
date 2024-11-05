(function () {
    'use strict';

    angular
        .module('app')
        .controller('SportsMonthEndReport', SportsMonthEndReport);

    SportsMonthEndReport.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']

    function SportsMonthEndReport($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {


        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (configsettings.length > 0) {
            grouporterm = configsettings[0].fmC_GroupOrTermFlg;
            autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
            receiptformat = configsettings[0].fmC_Receipt_Format;
            mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//newly Added
        }
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;



        $scope.loadData = function () {
            debugger;
            var id = 2;
            apiService.getURI("SportsMonthEndReport/getdeatils", id).
                then(function (promise) {
                    $scope.fillmonth = promise.fillmonth;
                    $scope.fillyear = promise.fillyear;
                })
        }

        $scope.GetReport = function () {
            debugger;
            $scope.mnthname = '';
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "month": $scope.month_id,
                }
                apiService.create("SportsMonthEndReport/GetReport", data).
                    then(function (promise) {
                        if (promise.count > 0) {
                            $scope.report = true;
                            $scope.total_count_student = promise.total_count_student;
                            $scope.not_patr_std = promise.not_patr_std;
                            $scope.total_partic_student = promise.total_partic_student;
                            $scope.total_winner_student = promise.total_winner_student;



                            angular.forEach($scope.fillmonth, function (dd) {
                                if (dd.month == $scope.month_id) {
                                    $scope.mnthname = dd.monthname;
                                }
                            })
                            angular.forEach($scope.fillyear, function (dd) {
                                if (dd.asmaY_Id == $scope.asmaY_Id) {
                                    $scope.yearname = dd.asmaY_Year;
                                }
                            })
                            console.log($scope.mnthname);
                            console.log($scope.yearname);
                            $scope.designation = "Implimentation Engineer";
                            $scope.today = new Date();

                            var chart = new CanvasJS.Chart("rangeBarChat");

                            chart.options.axisX = { interval: 1, labelFontSize: 12 };
                            chart.options.axisY = { labelFontSize: 12 };
                            //// chart.options.title = { text: "Fruits sold in First & Second Quarter" };
                            chart.options.height = 260;
                            chart.options.width = 1000;

                            var series1 = { //dataSeries - first quarter
                                type: "column",
                                name: "Count",
                                showInLegend: true,
                                dataPoints: [
                                    { y: $scope.total_count_student, label: "Total" },
                                    { y: parseInt($scope.not_patr_std), label: "Not participated" },
                                    { y: parseInt($scope.total_partic_student), label: "Participated student" },
                                    { y: parseInt($scope.total_winner_student), label: "Winners" }
                                ]
                            };



                            chart.options.data = [];
                            chart.options.data.push(series1);


                            chart.render();

                            $scope.exportToExcel = function () {

                                //if (promise.count > 0 && promise.count != null) {
                                var exportHref = Excel.tableToExcel(tablegrp, 'sheet name');
                                $timeout(function () { location.href = exportHref; }, 100);
                                //}
                            }

                            $scope.printData = function () {

                                if (promise.count > 0 && promise.count != null) {
                                    debugger;
                                    var base64Image = chart.canvas.toDataURL();
                                    document.getElementById('rangeBarChat').style.display = 'none';
                                    document.getElementById('chartImage').src = base64Image;//chartImage
                                    var innerContents = document.getElementById("tablegrp").innerHTML;
                                    var popupWinindow = window.open('');
                                    popupWinindow.document.open();
                                    popupWinindow.document.write('<html><head>' +
                                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                                        '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');

                                    popupWinindow.document.close();



                                }
                            }

                        }
                        else {
                            swal("No Records Found");
                            $scope.report = false;
                            //$state.reload();
                        }
                    })
            }
        }

        $scope.Clear_Details = function () {
            $state.reload();
        }

    }
})();
