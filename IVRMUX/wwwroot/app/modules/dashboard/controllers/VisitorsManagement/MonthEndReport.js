(function () {
    'use strict';

    angular
        .module('app')
        .controller('MonthEndReportController', MonthEndReportController);

    MonthEndReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']

    function MonthEndReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {


        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //var institutionid = configsettings[0].mI_Id;
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
           
            var id = 2;
            apiService.getURI("MonthEndReport/getdeatils", id).
                then(function (promise) {
                    $scope.fillmonth = promise.fillmonth;
                    $scope.fillyear = promise.fillyear;
                })
        }

        $scope.GetReport = function () {
            
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.academicyr,
                    "IVRM_Month_Id": $scope.IVRM_Month_Id,
                    "year": $scope.yearmodel,
                }

                apiService.create("MonthEndReport/get_month_report", data).
                    then(function (promise) {
                        debugger;
                        if (promise.griddata.length > 0) {
                            $scope.report = true;
                            $scope.griddata = promise.griddata;
                            $scope.total_visitors = promise.griddata[0].total_visitors;
                            $scope.total_inwards = promise.griddata[0].total_inwards;
                            $scope.total_outwards = promise.griddata[0].total_outwards;
                            $scope.total_student_gatepass = promise.griddata[0].total_student_gatepass;
                            $scope.total_staff_gatepass = promise.griddata[0].total_staff_gatepass;
                            $scope.total_visitors_checkinout = promise.griddata[0].total_visitors_checkinout;

                            for (var j = 0; j < $scope.fillyear.length; j++) {
                                if ($scope.fillyear[j].asmaY_Id == $scope.academicyr) {
                                    $scope.Year_Name = $scope.fillyear[j].asmaY_Year;
                                }
                            }

                            //$scope.total_count_student = promise.total_count_student;
                            //$scope.not_patr_std = promise.not_patr_std;
                            //$scope.total_partic_student = promise.total_partic_student;
                            //$scope.total_winner_student = promise.total_winner_student;



                            angular.forEach($scope.fillmonth, function (dd) {
                                if (dd.month == $scope.IVRM_Month_Id) {
                                    $scope.month1 = dd.monthname;
                                }
                            })

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
                                    { y: $scope.total_visitors, label: "Total Visitors" },
                                    { y: parseInt($scope.total_inwards), label: "Total Inwards" },
                                    { y: parseInt($scope.total_outwards), label: "Total Outwards" },
                                    { y: parseInt($scope.total_student_gatepass), label: "Total Student GatePass" },
                                    { y: parseInt($scope.total_staff_gatepass), label: "Total Staff GatePass" }
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
                                debugger;
                                if (promise.griddata.length > 0) {

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

        var temp = [];
        var year = "";
        $scope.onselectAcdYear = function () {
            temp = [];
            angular.forEach($scope.fillyear, function (itm) {
                if (itm.asmaY_Id == $scope.academicyr) {
                    year = itm.asmaY_Year
                }
            });
            var s1 = year.substring(0, 4);
            var s2 = year.substring(year.length, 5);
            temp.push({ asmaY_Id: 0, asmaY_Year: s1 });
            temp.push({ asmaY_Id: 1, asmaY_Year: s2 });
            $scope.years = temp;
        }


        $scope.Clear_Details = function () {
            $state.reload();
        }

    }
})();
