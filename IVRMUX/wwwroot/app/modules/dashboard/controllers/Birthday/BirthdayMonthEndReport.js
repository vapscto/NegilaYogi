(function () {
    'use strict';

    angular
        .module('app')
        .controller('BirthdayMonthEndReport', BirthdayMonthEndReport);

    BirthdayMonthEndReport.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']

    function BirthdayMonthEndReport($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {



        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (configsettings != null && configsettings.length > 0) {
            var institutionid = configsettings[0].mI_Id;
            grouporterm = configsettings[0].fmC_GroupOrTermFlg;
            autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
            receiptformat = configsettings[0].fmC_Receipt_Format;
            mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//newly Added
        }
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
            $scope.imgname = logopath;
        }





        $scope.loadData = function () {
            var id = 2;

            apiService.getURI("BirthDay/", id).
                then(function (promise) {

                    
                    $scope.fillmonth = promise.fillmonth;
                    $scope.fillyear = promise.fillyear;
                    $scope.monthlist_temp = promise.fillmonth;
                })
        };



        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clear_Details = function () {
            $state.reload();
            $scope.loaddata();
        }

        $scope.user_check = function () {
            if ($scope.user_check == 1) {
                $scope.userl = 1;
            }
            else {
                $scope.userl = 0;
            }
        }



        $scope.ShowReportdata = function () {
            //fillmonth
            $scope.monthtemp = "";
            if ($scope.fillmonth != null && $scope.fillmonth.length > 0 && $scope.month > 0) {
                angular.forEach($scope.fillmonth, function (dd) {

                    if (dd.monthid == $scope.month) {
                        $scope.monthtemp = dd.monthname;
                    }
                })
            }
           

            if ($scope.myForm.$valid) {

                $scope.from_date = new Date($scope.FMCB_fromDATE).toDateString();
                $scope.to_date = new Date($scope.FMCB_toDATE).toDateString();

               
                var data = {
                    //"yearid": $scope.academicyr,
                    "year": $scope.year,
                    "month": $scope.month,
                    //"type": $scope.usercheck
                    "ayear": $scope.yearmodel

                }

                apiService.create("BirthDay/getmonthreport", data).
                    then(function (promise) {
                     
                if ((promise.count1 > 0 && promise.count1 != null) || (promise.count2 > 0 && promise.count2 != null)) {
                    $scope.monthmodelvalue = $scope.from_date; 
                    $scope.report = true;
                    $scope.showGrafh = true;
                    $scope.totalCount1 = promise.count1;
                    $scope.totalCount2 = promise.count2;
                    $scope.emailCount1 = promise.amsT_emailId1;
                    $scope.emailCount2 = promise.amsT_emailId2;
                    $scope.smsCount1 = promise.smsStatus1;
                    $scope.smsCount2 = promise.smsStatus2;

                    //var monthNames = ["January", "February", "March", "April", "May", "June",
                    //             "July", "August", "September", "October", "November", "December"
                    //];
                    //var d = $scope.FMCB_fromDATE;
                    //var month = monthNames[d.getMonth()]
                    angular.forEach($scope.fillmonth, function (itm) {
                        if (parseInt(itm.ivrM_Month_Id) === parseInt($scope.month)) {
                            $scope.monthtemp = itm.ivrM_Month_Name;
                        }
                    });
                  
                    angular.forEach($scope.fillyear, function (yy) {
                        if (yy.hrmlY_Id == $scope.year) {
                            $scope.yearName = yy.hrmlY_LeaveYear;
                        }
                    })

                   // $scope.month = month;

                    $scope.designation = "Implementation Engineer";
                    $scope.today = new Date();
                   // $scope.acayearnow = $scope.acayear;
                    $scope.report = true;
                    // $scope.export_flag = true;

                    //var chart = new CanvasJS.Chart("rangeBarChat", {
                    //    title: {
                    //        text: "My First Chart in CanvasJS",

                    //    },
                       
                    //    data: [
                    //    {
                    //        // Change type to "doughnut", "line", "splineArea", etc.
                    //        type: "column",
                    //        dataPoints: [
                    //            { label: "BirthDayCount", y: $scope.totalCount },
                    //            { label: "SMS Count", y: parseInt ($scope.smsCount) },
                    //            { label: "Email Count", y: parseInt($scope.emailCount) },
                    //        ]
                    //    }
                    //    ]
                    //});
                    //chart.render();

                    //$scope.acdyr = "";
                    //angular.forEach($scope.acayyearbind, function (opq) {
                    //    $scope.acdyr += opq.asmaY_Year + " ";
                    //})
                   

                    var chart = new CanvasJS.Chart("rangeBarChat");

                    chart.options.axisX = {
                        labelAngle: -20,
                        interval: 1,
                        labelFontColor: "black",
      
                        labelFontSize: 12,
                    };
                    chart.options.axisY = { labelFontSize: 12 };
                    //// chart.options.title = { text: "Fruits sold in First & Second Quarter" };
                    chart.options.height = 260;
                    chart.options.width = 1000;

                    var series1 = { //dataSeries - first quarter
                        type: "column",
                        name: "Count",
                        showInLegend: true,
                       dataPoints: [
                           { y: $scope.totalCount1, label: "Student BirthDay Count" },
                           { y: parseInt($scope.smsCount1), label: "Student SMS Count" },
                           { y: parseInt($scope.emailCount1), label: "Student EMAIL Count" },
                           { y: $scope.totalCount2, label: "Staff BirthDay Count" },
                           { y: parseInt($scope.smsCount2), label: "Staff SMS Count" },
                           { y: parseInt($scope.emailCount2), label: "Staff EMAIL Count" }
                        ]
                    };
                    


                    chart.options.data = [];
                    chart.options.data.push(series1);
                

                   chart.render();
                    // 


                    //chart.print();

                    // }
                    // end deepak chart

                    $scope.exportToExcel = function () {
                        
                        if ((promise.count1 > 0 && promise.count1 != null) || (promise.count2 > 0 && promise.count2 != null)) {
                            var exportHref = Excel.tableToExcel(tablegrp, 'sheet name');
                            $timeout(function () { location.href = exportHref; }, 100);
                        }
                    }

                    $scope.printData = function () {
                        
                        if ((promise.count1 > 0 && promise.count1 != null) || (promise.count2 > 0 && promise.count2 != null)) {
                            var base64Image = chart.canvas.toDataURL();
                            document.getElementById('rangeBarChat').style.display = 'none';
                            document.getElementById('chartImage').src = base64Image;
                            var innerContents = document.getElementById("tablegrp").innerHTML;
                            var popupWinindow = window.open('');
                            popupWinindow.document.open();
                            popupWinindow.document.write('<html><head>' +
               '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Birthday/MonthEndReportPdf.css" />' +
          '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');

                            popupWinindow.document.close();

                            //                  var popupWinindow1 = window.open('');
                            //                  popupWinindow1.document.open();
                            //                  popupWinindow1.document.write('<html><head>' +
                            //     '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                            //      '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                            //'<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                            //  '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContentsGraph + '</html>');
                            //                  popupWinindow1.document.close();



                        }
                    }
                }
                else {
                    swal("Record Not Found");
                }
            })
                

            }
            else {
                $scope.submitted = true;
            }
        }





        var temp = [];
        var year12 = "";
        $scope.yearfromdate = "";
        $scope.monthlist_temp = [];

        $scope.get_years = function () {
            $scope.month_name = [];
            $scope.monthmodel = "";
            $scope.yearmodel = "";
            $scope.monthiddd = "";

            temp = [];
            angular.forEach($scope.fillyear, function (itm) {
                if (parseInt(itm.hrmlY_Id) === parseInt($scope.year)) {
                    year12 = itm.hrmlY_LeaveYear;
                }
            });
            var s1 = year12.substring(0, 4);
            var s2 = year12.substring(year12.length, 5);
            temp.push({ asmaY_Id: 0, asmaY_Year: s1 });
            temp.push({ asmaY_Id: 1, asmaY_Year: s2 });

            angular.forEach($scope.fillyear, function (itm) {
                if (parseInt(itm.hrmlY_Id) === parseInt($scope.year)) {
                    $scope.yearfromdate = itm.asmaY_From_Date;
                }
            });


            $scope.asmaYFromDate = $scope.yearfromdate;

            var date = new Date($scope.asmaYFromDate);

            $scope.monthNames = ["January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            ];
            $scope.months = [];
            $scope.year12 = [];

            for (var i1 = 0; i1 < 12; i1++) {
                $scope.months.push($scope.monthNames[date.getMonth()]);
                date.setMonth(date.getMonth() + 1);
            }

            $scope.monthByOrder = [];
            for (var i = 0; i < $scope.months.length; i++) {
                name = $scope.months[i].trim();
                for (var j = 0; j < $scope.monthlist_temp.length; j++) {
                    var monthiddd = $scope.monthlist_temp[j].monthid;
                    if (name.toLowerCase() === $scope.monthlist_temp[j].monthname.toLowerCase().trim()) {
                        if (i === 0) {
                            $scope.monthiddd = $scope.monthlist_temp[j].monthid;
                        }
                        $scope.monthByOrder.push($scope.monthlist_temp[j]);
                    }
                }
            }
            $scope.monthList = $scope.monthByOrder;
            $scope.month_name = $scope.monthByOrder;
            $scope.fillmonth = $scope.monthByOrder;
        };

        //added by roopa
        $scope.get_month = function () {
            temp = [];
            angular.forEach($scope.fillyear, function (mm) {
                if (parseInt(mm.hrmlY_Id) === parseInt($scope.year)) {

                    var frommonth = (new Date(mm.asmaY_From_Date)).getMonth() + 1;
                    var tomonth = (new Date(mm.asmaY_To_Date)).getMonth() + 1;
                    var fromyear = (new Date(mm.asmaY_From_Date)).getFullYear();
                    var tomonthyear = (new Date(mm.asmaY_To_Date)).getFullYear();
                    var s1 = "";
                    var s2 = "";
                    if ($scope.month >= $scope.monthiddd && $scope.month <= 12) {
                        var year1 = mm.hrmlY_LeaveYear;
                        s1 = year1.substring(0, 4);
                        s2 = year1.substring(year1.length, 5);
                        temp.push({ asmaY_Id: 0, hrmlY_LeaveYear: s1 });
                        $scope.years = temp;

                    } else {
                        var year12 = mm.hrmlY_LeaveYear;
                        s1 = year12.substring(0, 4);
                        s2 = year12.substring(year12.length, 5);
                        temp.push({ asmaY_Id: 1, hrmlY_LeaveYear: s2 });
                        $scope.years = temp;
                    }
                }
            });
        };



    }
})();