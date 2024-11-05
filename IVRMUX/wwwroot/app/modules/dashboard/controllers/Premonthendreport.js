(function () {
    'use strict';
    angular
.module('app')
.controller('PremonthendreportController', PremonthendreportController123)

    PremonthendreportController123.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache']
    function PremonthendreportController123($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.graphofmonth = false;
        $scope.exportflg = false;

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings!=null && ivrmcofigsettings.length > 0) {
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
        //$scope.itemsPerPage = 10;
        var chart = {};

        $scope.export_flag = false;
        $scope.IsHiddendown = false;
        $scope.IsHiddenup = true;
        $scope.obj = {};
        $scope.ShowHidedown = function () {
            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        }
        $scope.ShowHideup = function () {
            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        }




        var temp = [];
        var year = "";
        $scope.get_years = function () {
            temp = [];
            angular.forEach($scope.acayyearbind, function (itm) {
                if (itm.asmaY_Id == $scope.academicyr) {
                    year = itm.asmaY_Year
                }
            });
            var s1 = year.substring(0, 4);
            var s2 = year.substring(year.length, 5);
            temp.push({ asmaY_Id: 1, asmaY_Year: s1 - 1 });
            temp.push({ asmaY_Id: 0, asmaY_Year: s1 });
            temp.push({ asmaY_Id: 1, asmaY_Year: s2 });
            
            $scope.years = temp;
        }

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            apiService.getURI("Premonthendreport/getalldetails123", pageid).
        then(function (promise) {
            $scope.acayyearbind = promise.acayear;
            $scope.month_name = promise.month_array;
        })
        }

        $scope.submitted = false;
        $scope.ShowReportdata = function () {
            
            if ($scope.myform.$valid) {
                var data = {
                    "acayid": $scope.academicyr,
                    "month": $scope.monthmodel,
                    "year": $scope.yearmodel,
                }
                apiService.create("Premonthendreport/getreport", data).
            then(function (promise) {
                if (promise.reportdatelist.length > 0 && promise.reportdatelist != null) {
                    
                    $scope.grid_flag = true;
                    $scope.IsHiddendown = true;
                    $scope.graphofmonth = true;
                    $scope.total_Reg = promise.reportdatelist[0].total_Reg;
                    $scope.total_filled = promise.reportdatelist[0].total_filled;
                    $scope.total_notfilled = promise.reportdatelist[0].total_notfilled;
                    $scope.total_regpaid = promise.reportdatelist[0].total_regpaid;
                    $scope.offline_pay = promise.reportdatelist[0].offline_pay;
                    $scope.total_notregpaid = promise.reportdatelist[0].total_notregpaid;
                    $scope.total_transferd = promise.reportdatelist[0].total_transferd;
                    $scope.sent_email_count = promise.reportdatelist[0].sent_email_count;
                    $scope.sent_sms_count = promise.reportdatelist[0].sent_sms_count;
                    $scope.missing_photo = promise.reportdatelist[0].missing_photo;
                    $scope.missing_no = promise.reportdatelist[0].missing_no;
                    $scope.missing_email = promise.reportdatelist[0].missing_email;
                    $scope.tot_online = promise.reportdatelist[0].totonline;
                    $scope.tot_offline = promise.reportdatelist[0].totoffline;
                    $scope.tot_entries = promise.reportdatelist[0].tot_entries;

                    //$scope.tot_absent = promise.reportdatelist[0].tot_absent;
                    //$scope.DOB_Certificate_count = promise.reportdatelist[0].DOB_Certificate_count;
                    $scope.designation = "Implementation Engineer";
                    $scope.today = new Date();
                    angular.forEach($scope.month_name, function (itm) {
                        if (itm.ivrM_Month_Id == $scope.monthmodel) {
                            $scope.monthmodelvalue = itm.ivrM_Month_Name
                        }
                    });
                    angular.forEach($scope.acayyearbind, function (itm) {
                        if (itm.asmaY_Id == $scope.academicyr) {
                            $scope.acayearnow = itm.asmaY_Year
                        }
                    });

                    //COUNT
                    $scope.feegraphseries1 = [];
                    $scope.feegraphseries2 = [];
                    $scope.feegraphseries3 = [];
                    $scope.feegraphseries4 = [];
                    $scope.feegraphseries5 = [];

                    //SMS
                    $scope.feegraphseries6 = [];
                    $scope.feegraphseries7 = [];
                    $scope.feegraphseries8 = [];
                    $scope.feegraphseries9 = [];
                    $scope.feegraphseries10 = [];

                    //Email
                    $scope.feegraphseries11 = [];
                    $scope.feegraphseries12 = [];
                    $scope.feegraphseries13 = [];
                    $scope.feegraphseries14 = [];
                    $scope.feegraphseries15 = [];


                    $scope.feegraphseries1.push({ label: 'Total Registered', "y": $scope.total_Reg })
                    $scope.feegraphseries2.push({ label: 'Total Filled', "y": $scope.total_filled })
                    $scope.feegraphseries3.push({ label: 'Reg. not Filled', "y": $scope.total_notfilled })
                    $scope.feegraphseries4.push({ label: 'Reg Payment Done', "y": $scope.total_regpaid })
                    $scope.feegraphseries5.push({ label: 'Reg Payment Not Done', "y": $scope.total_notregpaid })
                    $scope.feegraphseries5.push({ label: 'Total Transferred', "y": $scope.total_transferd })




                    console.log($scope.feegraphseries1);


                     chart = new CanvasJS.Chart("rangeBarChat");

                    chart.options.axisX = { interval: 1, labelFontSize: 12 };
                    chart.options.axisY = { labelFontSize: 12 };

                    // chart.options.title = { text: "Fruits sold in First & Second Quarter" };

                    var series1 = { //dataSeries - first quarter
                        type: "column",
                        name: "Total Registered",
                        showInLegend: true
                    };



                    var series2 = { //dataSeries - second quarter
                        type: "column",
                        name: "Total Filled",
                        showInLegend: true
                    };

                    var series3 = { //dataSeries - third quarter
                        type: "column",
                        name: "Reg. not Filled",
                        showInLegend: true
                    };
                    var series4 = { //dataSeries - fourth quarter
                        type: "column",
                        name: "Reg Payment Done",
                        showInLegend: true
                    };
                    var series5 = { //dataSeries - fifth quarter
                        type: "column",
                        name: "Reg Payment Not Done",
                        showInLegend: true
                    };
                    var series6 = { //dataSeries - fifth quarter
                        type: "column",
                        name: "Total Transferred",
                        showInLegend: true
                    };


                    chart.options.data = [];
                    chart.options.data.push(series1);
                    chart.options.data.push(series2);
                    chart.options.data.push(series3);
                    chart.options.data.push(series4);
                    chart.options.data.push(series5);
                    chart.options.data.push(series6);


                    series1.dataPoints = $scope.feegraphseries1;
                    series2.dataPoints = $scope.feegraphseries2;
                    series3.dataPoints = $scope.feegraphseries3;
                    series4.dataPoints = $scope.feegraphseries4;
                    series5.dataPoints = $scope.feegraphseries5;
                    series6.dataPoints = $scope.feegraphseries6;
                    chart.render();


                    $scope.export_flag = true;
                }
                else {
                    swal("Record Not Found");
                    $scope.grid_flag = true;
                }
            })
            }
            else {
                // swal("Select any Student");
                $scope.submitted = true;
            }
        };


        $scope.printData = function () {
            $scope.graphofmonth = true;
            var base64Image = chart.canvas.toDataURL();
            document.getElementById('rangeBarChat').style.display = 'none';
            document.getElementById('chartImage').src = base64Image;
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
            '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
             '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
            //$state.reload();
        }

        $scope.exportToExcel = function (printSectionId) {
            $scope.graphofmonth = false;
            $scope.exportflg = true;
            var exportHref = Excel.tableToExcel(printSectionIdexport, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }






        $scope.Clear_Details = function () {
            $state.reload();
        }


        $scope.interacted = function (field) {

            return $scope.submitted;
        };
    }
})();

