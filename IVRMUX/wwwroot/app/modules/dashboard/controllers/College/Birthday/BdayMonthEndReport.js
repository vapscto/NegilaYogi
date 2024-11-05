(function () {
    'use strict';
    angular
        .module('app')
        .controller('BdayMonthEndReportController', BdayMonthEndReportController);
    BdayMonthEndReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function BdayMonthEndReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {



        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.printsection = false;

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        $scope.sortKey = "regno";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;


        $scope.imgdiv = false;
        var temp = [];
        var year = "";
        $scope.yearfromdate = "";
        $scope.monthlist_temp = [];

        $scope.clgloaddata = function () {
            var pageid = 2;
            apiService.getURI("BdayMonthEndReport/getloaddata", pageid).then(function (promise) {
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
                if (itm.asmaY_Id == $scope.academicyr) {
                    year = itm.asmaY_Year;
                }
            });
            var s1 = year.substring(0, 4);
            var s2 = year.substring(year.length, 5);
            temp.push({ asmaY_Id: 0, asmaY_Year: s1 });
            temp.push({ asmaY_Id: 1, asmaY_Year: s2 });
            // $scope.years = temp;

            angular.forEach($scope.acayyearbind, function (itm) {
                if (itm.asmaY_Id == $scope.academicyr) {
                    $scope.yearfromdate = itm.asmaY_From_Date
                }
            });


            $scope.asmaYFromDate = $scope.yearfromdate;

            var date = new Date($scope.asmaYFromDate);

            $scope.monthNames = ["January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            ];
            $scope.months = [];
            $scope.year = [];

            for (var i = 0; i < 12; i++) {
                $scope.months.push($scope.monthNames[date.getMonth()]);
                //$scope.year.push(date.getFullYear());
                // Add a month each time
                date.setMonth(date.getMonth() + 1);
            }
            $scope.monthByOrder = [];
            for (var i = 0; i < $scope.months.length; i++) {
                name = $scope.months[i].trim();
                for (var j = 0; j < $scope.monthlist_temp.length; j++) {
                    var monthiddd = $scope.monthlist_temp[j].ivrM_Month_Id;
                    if (name.toLowerCase() == $scope.monthlist_temp[j].ivrM_Month_Name.toLowerCase().trim()) {
                        if (i == 0) {
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
            $scope.years = [];
            angular.forEach($scope.acayyearbind, function (mm) {
                if (mm.asmaY_Id == $scope.academicyr) {

                    var frommonth = (new Date(mm.asmaY_From_Date)).getMonth() + 1;
                    var tomonth = (new Date(mm.asmaY_To_Date)).getMonth() + 1;
                    var fromyear = (new Date(mm.asmaY_From_Date)).getFullYear();
                    var tomonths = (new Date(mm.asmaY_To_Date)).getFullYear();

                    if ($scope.monthmodel >= $scope.monthiddd && $scope.monthmodel <= 12) {
                        var year1 = mm.asmaY_Year;

                        var s1 = year1.substring(0, 4);
                        var s2 = year1.substring(year1.length, 5);
                        temp.push({ asmaY_Id: 0, asmaY_Year: s1 });
                        //temp.push({ asmaY_Id: 1, asmaY_Year: s2 });
                        // $scope.years = temp;
                        $scope.years.push({ asmaY_Id: 0, asmaY_Year: s1 });
                        if (parseInt($scope.monthmodel) === tomonth) {
                            $scope.years.push({ asmaY_Id: 1, asmaY_Year: s2 });
                        }

                    } else {
                        var year12 = mm.asmaY_Year;

                        var s1 = year12.substring(0, 4);
                        var s2 = year12.substring(year12.length, 5);
                        temp.push({ asmaY_Id: 1, asmaY_Year: s2 });
                        //temp.push({ asmaY_Id: 1, asmaY_Year: s2 });

                        $scope.years.push({ asmaY_Id: 1, asmaY_Year: s2 });
                        // $scope.years = temp;
                    }
                }
            });
        };




        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.Clear = function () {
            $state.reload();
        }

        $scope.showReportdata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.academicyr,
                    "month": $scope.monthmodel,
                    "year": $scope.yearmodel,
                }

                apiService.create("BdayMonthEndReport/getmonthreport", data).
                    then(function (promise) {
                        if (promise.studentlist.length > 0) {

                            $scope.report = true;
                            $scope.showGrafh = true;
                            $scope.totalCount = promise.studentlist[0].total;
                            $scope.emailCount = promise.studentlist[0].email;
                            $scope.smsCount = promise.studentlist[0].sms;

                            angular.forEach($scope.acayyearbind, function (yy) {
                                if (yy.asmaY_Id == $scope.academicyr) {
                                    $scope.yearname = yy.asmaY_Year;
                                }
                            });

                            angular.forEach($scope.month_name, function (mnth) {
                                if (mnth.ivrM_Month_Id == $scope.monthmodel) {
                                    $scope.month = mnth.ivrM_Month_Name;
                                }
                            });

                            $scope.designation = "Implementation Engineer";
                            $scope.today = new Date();
                            $scope.report = true;
                            var chart = new CanvasJS.Chart("rangeBarChat");

                            chart.options.axisX = { interval: 1, labelFontSize: 12 };
                            chart.options.axisY = { labelFontSize: 12 };
                            chart.options.height = 260;
                            chart.options.width = 1000;
                            var series1 = {
                                type: "column",
                                name: "Count",
                                showInLegend: true,
                                dataPoints: [
                                    { y: parseInt($scope.totalCount), label: "BIRTHDAY COUNT" },
                                    { y: parseInt($scope.smsCount), label: "SMS COUNT" },
                                    { y: parseInt($scope.emailCount), label: "EMAIL COUNT" }
                                ]
                            };
                            chart.options.data = [];
                            chart.options.data.push(series1);
                            chart.render();

                            $scope.exportToExcel = function () {
                                if (promise.studentlist.length > 0) {
                                    var exportHref = Excel.tableToExcel(tablegrp, 'sheet name');
                                    $timeout(function () { location.href = exportHref; }, 100);
                                }
                            }
                            $scope.printData = function () {

                                if (promise.studentlist.length > 0) {
                                    var base64Image = chart.canvas.toDataURL();
                                    document.getElementById('rangeBarChat').style.display = 'none';
                                    document.getElementById('chartImage').src = base64Image;
                                    var innerContents = document.getElementById("tablegrp").innerHTML;
                                    var popupWinindow = window.open('');
                                    popupWinindow.document.open();
                                    popupWinindow.document.write('<html><head>' +
                                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/CollegeBirthday/MonthEndReportPdf.css" />' +
                                        '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                                    $scope.imgdiv = true;
                                    popupWinindow.document.close();
                                }
                            }
                        }
                        else {
                            swal("Record Not Found....!!");
                        }
                    })
            }
            else {
                $scope.submitted = true;
            }
        }





    }
})();

