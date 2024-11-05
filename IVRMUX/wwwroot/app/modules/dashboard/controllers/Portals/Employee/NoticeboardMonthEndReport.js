﻿(function () {
    'use strict';
    angular.module('app').controller('HWMonthEndReportController', HWMonthEndReportController)
    HWMonthEndReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache']
    function HWMonthEndReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
            copty = "";
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var chart = {};

        $scope.export_flag = false;
        $scope.IsHiddendown = false;
        $scope.IsHiddenup = true;
        $scope.obj = {};
        $scope.ShowHidedown = function () {
            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        };
        $scope.ShowHideup = function () {
            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        };




        var temp = [];
        var year = "";
        $scope.yearfromdate = "";
        $scope.monthlist_temp = [];

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            apiService.getURI("HWMonthEndReport/getalldetails123", pageid).then(function (promise) {
                $scope.acayyearbind = promise.acayear;
                $scope.month_name = promise.month_array;
                $scope.monthlist_temp = promise.month_array;
            });
        };


        $scope.get_years = function () {
            $scope.month_name = [];
            $scope.monthmodel = "";
            $scope.yearmodel = "";
            $scope.monthiddd = "";

            temp = [];
            angular.forEach($scope.acayyearbind, function (itm) {
                if (parseInt(itm.asmaY_Id) === parseInt($scope.academicyr)) {
                    year = itm.asmaY_Year;
                }
            });
            var s1 = year.substring(0, 4);
            var s2 = year.substring(year.length, 5);
            temp.push({ asmaY_Id: 0, asmaY_Year: s1 });
            temp.push({ asmaY_Id: 1, asmaY_Year: s2 });

            angular.forEach($scope.acayyearbind, function (itm) {
                if (parseInt(itm.asmaY_Id) === parseInt($scope.academicyr)) {
                    $scope.yearfromdate = itm.asmaY_From_Date;
                }
            });


            $scope.asmaYFromDate = $scope.yearfromdate;

            var date = new Date($scope.asmaYFromDate);

            $scope.monthNames = ["January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            ];
            $scope.months = [];
            $scope.year = [];

            for (var i1 = 0; i1 < 12; i1++) {
                $scope.months.push($scope.monthNames[date.getMonth()]);
                date.setMonth(date.getMonth() + 1);
            }

            $scope.monthByOrder = [];
            for (var i = 0; i < $scope.months.length; i++) {
                name = $scope.months[i].trim();
                for (var j = 0; j < $scope.monthlist_temp.length; j++) {
                    var monthiddd = $scope.monthlist_temp[j].ivrM_Month_Id;
                    if (name.toLowerCase() === $scope.monthlist_temp[j].ivrM_Month_Name.toLowerCase().trim()) {
                        if (i === 0) {
                            $scope.monthiddd = $scope.monthlist_temp[j].ivrM_Month_Id;
                        }
                        $scope.monthByOrder.push($scope.monthlist_temp[j]);
                    }
                }
            }
            $scope.monthList = $scope.monthByOrder;
            $scope.month_name = $scope.monthByOrder;
        };

        $scope.get_month = function () {
            temp = [];
            angular.forEach($scope.acayyearbind, function (mm) {
                if (parseInt(mm.asmaY_Id) === parseInt($scope.academicyr)) {

                    var frommonth = (new Date(mm.asmaY_From_Date)).getMonth() + 1;
                    var tomonth = (new Date(mm.asmaY_To_Date)).getMonth() + 1;
                    var fromyear = (new Date(mm.asmaY_From_Date)).getFullYear();
                    var tomonthyear = (new Date(mm.asmaY_To_Date)).getFullYear();
                    var s1 = "";
                    var s2 = "";
                    if ($scope.monthmodel >= $scope.monthiddd && $scope.monthmodel <= 12) {
                        var year1 = mm.asmaY_Year;
                        s1 = year1.substring(0, 4);
                        s2 = year1.substring(year1.length, 5);
                        temp.push({ asmaY_Id: 0, asmaY_Year: s1 });
                        $scope.years = temp;

                    } else {
                        var year12 = mm.asmaY_Year;
                        s1 = year12.substring(0, 4);
                        s2 = year12.substring(year12.length, 5);
                        temp.push({ asmaY_Id: 1, asmaY_Year: s2 });
                        $scope.years = temp;
                    }
                }
            });
        };


        $scope.submitted = false;

        $scope.ShowReportdata = function () {

            if ($scope.myform.$valid) {
                var data = {
                    "acayid": $scope.academicyr,
                    "month": $scope.monthmodel,
                    "year": $scope.yearmodel,
                    "type": "Noticeboard"

                };
                apiService.create("HWMonthEndReport/getreport", data).then(function (promise) {
                    if (promise.reportlist !== null && promise.reportlist.length > 0) {

                        // $scope.grid_flag = true;
                        $scope.IsHiddendown = true;
                        $scope.reportlist = promise.reportlist;
                        $scope.designation = " : Implementation Engineer";
                        $scope.today = new Date();
                        angular.forEach($scope.month_name, function (itm) {
                            if (parseInt(itm.ivrM_Month_Id) === parseInt($scope.monthmodel)) {
                                $scope.monthmodelvalue = itm.ivrM_Month_Name;
                            }
                        });
                        angular.forEach($scope.acayyearbind, function (itm) {
                            if (parseInt(itm.asmaY_Id) === parseInt($scope.academicyr)) {
                                $scope.acayearnow = itm.asmaY_Year;
                            }
                        });

                        //COUNT
                        //$scope.feegraphseries1 = [];
                        //$scope.feegraphseries2 = [];
                        //$scope.feegraphseries3 = [];
                        //$scope.feegraphseries4 = [];
                        //$scope.feegraphseries5 = [];



                        //$scope.feegraphseries1.push({ label: 'Notice Upload Count', "y": $scope.tot_count });
                        //$scope.feegraphseries2.push({ label: 'Class', "y": $scope.asmcl });
                        //$scope.feegraphseries4.push({ label: 'TC Taken', "y": $scope.tc_count });
                        //$scope.feegraphseries5.push({ label: 'Count', "y": $scope.DOB_Certificate_count });


                        //console.log($scope.feegraphseries1);


                        //chart = new CanvasJS.Chart("rangeBarChat");

                        //chart.options.axisX = { interval: 1, labelFontSize: 12 };
                        //chart.options.axisY = { labelFontSize: 12 };

                        //var series1 = {
                        //    type: "column",
                        //    name: "Student Strength",
                        //    showInLegend: true
                        //};

                        //var series2 = {
                        //    type: "column",
                        //    name: "New Admission",
                        //    showInLegend: true
                        //};
                        //var series4 = {
                        //    type: "column",
                        //    name: "TC Taken",
                        //    showInLegend: true
                        //};
                        //var series5 = {
                        //    type: "column",
                        //    name: "Bonafied Certificate",
                        //    showInLegend: true
                        //};


                        //chart.options.data = [];
                        //chart.options.data.push(series1);
                        //chart.options.data.push(series2);
                        //chart.options.data.push(series4);
                        //chart.options.data.push(series5);


                        //series1.dataPoints = $scope.feegraphseries1;
                        //series2.dataPoints = $scope.feegraphseries2;
                        //series4.dataPoints = $scope.feegraphseries4;
                        //series5.dataPoints = $scope.feegraphseries5;
                        //chart.render();



                        //$scope.export_flag = true;
                    }
                    else {
                        swal("Record Not Found");
                        $scope.grid_flag = true;
                        $scope.reportlist = [];
                    }
                });
            }
            else {
                // swal("Select any Student");
                $scope.submitted = true;
            }
        };


        $scope.printData = function () {
            //var base64Image = chart.canvas.toDataURL();
            //document.getElementById('rangeBarChat').style.display = 'none';
            //document.getElementById('chartImage').src = base64Image;
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };


        $scope.exportToExcel = function (excelId) {
            $scope.export_flag = true;
            var exportHref = Excel.tableToExcel(excelId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };

        $scope.Clear_Details = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();

