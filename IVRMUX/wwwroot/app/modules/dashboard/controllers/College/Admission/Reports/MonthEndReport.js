(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgMonthEndReportController', ClgMonthEndReportController)

    ClgMonthEndReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache']
    function ClgMonthEndReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {



        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

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
        $scope.yearfromdate = "";
        $scope.monthlist_temp = [];

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            apiService.getURI("ClgMonthEndReport/getalldetails", pageid).
                then(function (promise) {
                    $scope.acayyearbind = promise.acayear;
                    $scope.month_name = promise.month_array;
                    $scope.monthlist_temp = promise.month_array;
                })
        }


        $scope.get_years = function () {
            $scope.month_name = [];
            $scope.monthmodel = "";
            $scope.yearmodel = "";
            $scope.monthiddd = "";

            temp = [];
            angular.forEach($scope.acayyearbind, function (itm) {
                if (itm.asmaY_Id == $scope.academicyr) {
                    year = itm.asmaY_Year
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
            $scope.years = [];
            temp = [];
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
                        //$scope.years = temp;
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
                    "AMCOC_Id": 0
                };

                apiService.create("ClgMonthEndReport/getreport", data).
                    then(function (promise) {
                        if (promise.reportdatelist.length > 0 && promise.reportdatelist != null) {

                            $scope.grid_flag = true;
                            $scope.IsHiddendown = true;
                            $scope.tot_strength = promise.reportdatelist[0].total_strength;
                            $scope.newadmission = promise.reportdatelist[0].newadmission;
                            $scope.missing_pic = promise.reportdatelist[0].missing_pic;
                            $scope.sent_sms = promise.reportdatelist[0].sent_sms_count;
                            $scope.sent_email = promise.reportdatelist[0].sent_email_count;
                            $scope.missing_email = promise.reportdatelist[0].missing_email;
                            $scope.missing_phone = promise.reportdatelist[0].missing_phone;
                            $scope.missingphotonew = promise.reportdatelist[0].missingphoto_new;
                            $scope.missingemailnew = promise.reportdatelist[0].missingemail_new;
                            $scope.missingphonenew = promise.reportdatelist[0].missingphone_new;
                            $scope.tc_count = promise.reportdatelist[0].tc_count;
                            $scope.tot_absent = promise.reportdatelist[0].tot_absent;
                            $scope.DOB_Certificate_count = promise.reportdatelist[0].DOB_Certificate_count;
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


                            $scope.feegraphseries1.push({ label: 'Student Strength', "y": $scope.tot_strength })
                            $scope.feegraphseries2.push({ label: 'New Admission', "y": $scope.newadmission })
                            $scope.feegraphseries3.push({ label: 'Absent Students', "y": $scope.tot_absent })
                            $scope.feegraphseries4.push({ label: 'TC Taken', "y": $scope.tc_count })
                            $scope.feegraphseries5.push({ label: 'Count', "y": $scope.DOB_Certificate_count })



                            ////sms
                            //$scope.feegraphseries6.push({ label: 'SMS', "y": $scope.sent_sms })
                            //$scope.feegraphseries7.push({ label: 'new admission', })
                            //$scope.feegraphseries8.push({ label: 'Absent Students', })
                            //$scope.feegraphseries9.push({ label: 'TC Taken', })
                            //$scope.feegraphseries10.push({ label: 'SMS', })

                            ////Email
                            //$scope.feegraphseries11.push({ label: 'Email', "y": $scope.sent_email })
                            //$scope.feegraphseries12.push({ label: 'new admission', })
                            //$scope.feegraphseries13.push({ label: 'Absent Students', })
                            //$scope.feegraphseries14.push({ label: 'TC Taken', })
                            //$scope.feegraphseries15.push({ label: 'EMAIL', })


                            console.log($scope.feegraphseries1);


                            chart = new CanvasJS.Chart("rangeBarChat");

                            chart.options.axisX = { interval: 1, labelFontSize: 12 };
                            chart.options.axisY = { labelFontSize: 12 };

                            // chart.options.title = { text: "Fruits sold in First & Second Quarter" };

                            var series1 = { //dataSeries - first quarter
                                type: "column",
                                name: "Student Strength",
                                showInLegend: true
                            };



                            var series2 = { //dataSeries - second quarter
                                type: "column",
                                name: "New Admission",
                                showInLegend: true
                            };

                            var series3 = { //dataSeries - third quarter
                                type: "column",
                                name: "Absent Students",
                                showInLegend: true
                            };
                            var series4 = { //dataSeries - fourth quarter
                                type: "column",
                                name: "TC Taken",
                                showInLegend: true
                            };
                            var series5 = { //dataSeries - fifth quarter
                                type: "column",
                                name: "Bonafied Certificate",
                                showInLegend: true
                            };


                            chart.options.data = [];
                            chart.options.data.push(series1);
                            chart.options.data.push(series2);
                            chart.options.data.push(series3);
                            chart.options.data.push(series4);
                            chart.options.data.push(series5);


                            series1.dataPoints = $scope.feegraphseries1;
                            series2.dataPoints = $scope.feegraphseries2;
                            series3.dataPoints = $scope.feegraphseries3;
                            series4.dataPoints = $scope.feegraphseries4;
                            series5.dataPoints = $scope.feegraphseries5;







                            chart.render();

                            //sms
                            //var chart = new CanvasJS.Chart("rangeBarChat1");

                            //chart.options.axisX = { interval: 1, labelFontSize: 12 };
                            //chart.options.axisY = { labelFontSize: 12 };

                            //// chart.options.title = { text: "Fruits sold in First & Second Quarter" };

                            //var series1 = { //dataSeries - first quarter
                            //    type: "column",
                            //    name: "Student Strength",
                            //    showInLegend: true
                            //};



                            //var series2 = { //dataSeries - second quarter
                            //    type: "column",
                            //    name: "New Admission",
                            //    showInLegend: true
                            //};

                            //var series3 = { //dataSeries - third quarter
                            //    type: "column",
                            //    name: "Absent Students",
                            //    showInLegend: true
                            //};
                            //var series4 = { //dataSeries - fourth quarter
                            //    type: "column",
                            //    name: "TC Taken",
                            //    showInLegend: true
                            //};
                            //var series5 = { //dataSeries - fifth quarter
                            //    type: "column",
                            //    name: "Bonafied Certificate",
                            //    showInLegend: true
                            //};


                            //chart.options.data = [];
                            //chart.options.data.push(series1);
                            //chart.options.data.push(series2);
                            //chart.options.data.push(series3);
                            //chart.options.data.push(series4);
                            //chart.options.data.push(series5);


                            //series1.dataPoints = $scope.feegraphseries6;
                            //series2.dataPoints = $scope.feegraphseries7;
                            //series3.dataPoints = $scope.feegraphseries8;
                            //series4.dataPoints = $scope.feegraphseries9;
                            //series5.dataPoints = $scope.feegraphseries10;


                            //chart.render();


                            ////email
                            //var chart = new CanvasJS.Chart("rangeBarChat2");

                            //chart.options.axisX = { interval: 1, labelFontSize: 12 };
                            //chart.options.axisY = { labelFontSize: 12 };

                            //// chart.options.title = { text: "Fruits sold in First & Second Quarter" };

                            //var series1 = { //dataSeries - first quarter
                            //    type: "column",
                            //    name: "Student Strength",
                            //    showInLegend: true
                            //};



                            //var series2 = { //dataSeries - second quarter
                            //    type: "column",
                            //    name: "New Admission",
                            //    showInLegend: true
                            //};

                            //var series3 = { //dataSeries - third quarter
                            //    type: "column",
                            //    name: "Absent Students",
                            //    showInLegend: true
                            //};
                            //var series4 = { //dataSeries - fourth quarter
                            //    type: "column",
                            //    name: "TC Taken",
                            //    showInLegend: true
                            //};
                            //var series5 = { //dataSeries - fifth quarter
                            //    type: "column",
                            //    name: "Bonafied Certificate",
                            //    showInLegend: true
                            //};


                            //chart.options.data = [];
                            //chart.options.data.push(series1);
                            //chart.options.data.push(series2);
                            //chart.options.data.push(series3);
                            //chart.options.data.push(series4);
                            //chart.options.data.push(series5);


                            //series1.dataPoints = $scope.feegraphseries11;
                            //series2.dataPoints = $scope.feegraphseries12;
                            //series3.dataPoints = $scope.feegraphseries13;
                            //series4.dataPoints = $scope.feegraphseries14;
                            //series5.dataPoints = $scope.feegraphseries15;


                            //chart.render();





                            // $scope.acayearnow = $scope.academicyr;
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
        }

        $scope.exportToExcel = function (printSectionId) {
            var exportHref = Excel.tableToExcel(printSectionId, 'sheet name');
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

