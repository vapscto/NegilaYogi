

(function () {
    'use strict';
    angular
.module('app')
        .controller('CollegeMothEndReportController', CollegeMothEndReportController)

    CollegeMothEndReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function CollegeMothEndReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {

        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (configsettings.length > 0) {
            grouporterm = configsettings[0].fmC_GroupOrTermFlg;
            autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
            receiptformat = configsettings[0].fmC_Receipt_Format;
            mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//added by kiran
        }
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;


        $scope.loaddata = function () {
            
            $scope.report = false;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            apiService.getURI("CollegeMothEndReport/getalldetails123", pageid).
        then(function (promise) {
            
            $scope.acayyearbind = promise.acayear;
            $scope.month_name = promise.fillmonth;

        })
            

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
            temp.push({ asmaY_Id: 0, asmaY_Year: s1 });
            temp.push({ asmaY_Id: 1, asmaY_Year: s2 });
            $scope.years = temp;
        }

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


            if ($scope.myForm.$valid) {


               // $scope.from_date = new Date($scope.FMCB_fromDATE).toDateString();
               // $scope.to_date = new Date($scope.FMCB_toDATE).toDateString();


                var data = {
                    "yearid": $scope.academicyr,
                    //"frmdate": $scope.from_date,
                    //"todate": $scope.to_date,
                    "type": $scope.usercheck,
                      "month": $scope.monthmodel,
                    "year": $scope.yearmodel,
                }




                apiService.create("CollegeMothEndReport/getreport", data).
            then(function (promise) {
                
                if (promise.reportdatelist.length > 0 && promise.reportdatelist != null) {
                    $scope.monthmodelvalue = $scope.from_date;
                    $scope.report = true;
                    $scope.showGrafh = false;
                    $scope.fcbbcD = promise.reportdatelist[0].cashcount;
                    $scope.fcbcc = promise.reportdatelist[0].bankcount;
                    $scope.fcboc = promise.reportdatelist[0].onlinecount;
                    $scope.fcbEc = promise.reportdatelist[0].rtgs;
                    $scope.frbcc = promise.reportdatelist[0].cardcount;
                    $scope.frbbecs = promise.reportdatelist[0].ecs;
                    $scope.frdef = promise.reportdatelist[0].defaulters;
                    $scope.frrefd = promise.reportdatelist[0].refundcash;
                    $scope.frrefdb = promise.reportdatelist[0].refundbank;
                    $scope.smscnt = promise.reportdatelist[0].smscount;
                    $scope.emailcnt = promise.reportdatelist[0].emailcount;

                    var monthNames = ["","January", "February", "March", "April", "May", "June",
                                 "July", "August", "September", "October", "November", "December"
                    ];


                    //var d = $scope.FMCB_fromDATE;
                    //var month = monthNames[d.getMonth()]
                    $scope.monthmodel = $scope.monthmodel;
                    var month = monthNames[$scope.monthmodel];
                    $scope.month = month;
                    $scope.designation = "Implimentation Engineer";
                    $scope.today = new Date();
                    $scope.acayearnow = $scope.acayear;
                    $scope.report = true;
                    // $scope.export_flag = true;



                    //$scope.acdyr = "";
                    //angular.forEach($scope.acayyearbind, function (opq) {
                    //    $scope.acdyr += opq.asmaY_Year + " ";
                    //})


                    angular.forEach($scope.acayyearbind, function (y) {
                        if (y.asmaY_Id == $scope.academicyr) {
                            $scope.acdyr = y.asmaY_Year;
                        }
                    })
                    var chart = new CanvasJS.Chart("rangeBarChat");

                    chart.options.axisX = { interval: 1, labelFontSize: 12, labelAngle: -20  };
                    chart.options.axisY = { labelFontSize: 12 };
                    chart.options.height = 260;
                    chart.options.width = 1000;
                    var series1 = {
                        type: "column",
                        name: "Count",
                        showInLegend: true,
                        dataPoints: [
                            { y: parseInt($scope.fcbbcD), label: "Fee Collection By Cash" },
                            { y: parseInt($scope.fcbcc), label: "Fee Collection By Bank" },
                            { y: parseInt($scope.fcboc), label: "Fee Collection By Online" },
                            { y: parseInt($scope.fcbEc), label: "Fee Collection By RTGS" },
                            { y: parseInt($scope.frbcc), label: "Fee Collection By Card" },
                            { y: parseInt($scope.frbbecs), label: "Fee Collection By ECS" },
                            { y: parseInt($scope.frrefd), label: "Fee Refund by Cash" },
                            { y: parseInt($scope.frrefdb), label: "Fee Refund By Bank" },
                            { y: parseInt($scope.smscnt), label: "SMS" },
                            { y: parseInt($scope.emailcnt), label: "Email" }

                        ]
                    };
                    chart.options.data = [];
                    chart.options.data.push(series1);
                    chart.render();




                 //  var chart = new CanvasJS.Chart("rangeBarChat",
                    //    {

                    //        width: 900,

                    //        axisX: {
                    //            interval:1
                    //        },
                    //        axisY2: {
                    //            interval: 1

                    //        },
                           
                          

                    //            data: [
                    //            {
                    //            type: "column",
                    //            showInLegend: true,
                    //            legendText: "Count",
                    //            color: "gold",
                    //            dataPoints: [
                    //            { x: 1, y: $scope.fcbbcD, label: "Cash" },
                    //            { x: 2, y: $scope.fcbcc, label: "Bank" },
                    //            { x: 3, y: $scope.fcboc, label: "Online" },
                    //            { x: 4, y: $scope.fcbEc, label: "RTGS" },
                    //            { x: 5, y: $scope.frbcc, label: "Card" },
                    //                { x: 6, y: $scope.frbbecs, label: "ECS" },
                    //                { x: 7, y: $scope.frbbecs, label: "Refund Cash" },
                    //                { x: 8, y: $scope.frbbecs, label: "Refund Bank" },

                    //            ]
                    //            },
                    //            {
                    //            type: "column",
                    //            showInLegend: true,
                    //            legendText: "SMS",
                    //            color: "silver",
                    //            dataPoints: [
                    //            { x: 1, y: 0, label: "SMS" },
                    //            { x: 2, y: 0, label: "SMS" },
                    //            { x: 3, y: 0, label: "SMS" },
                    //            { x: 4, y: 0, label: "SMS" },
                    //            { x: 5, y: 0, label: "SMS" },
                    //            { x: 6, y: 0, label: "SMS" },
                    //           { x: 7, y: $scope.smscnt, label: "SMS" },

                    //            ]
                    //            },
                    //            {
                    //            type: "column",
                    //            showInLegend: true,
                    //            legendText: "Email",
                    //            color: "#DCA978",
                    //            dataPoints: [
                    //            { x: 1, y: 0, label: "Cash" },
                    //            { x: 2, y: 0, label: "Bank" },
                    //            { x: 3, y: 0, label: "Online" },
                    //            { x: 4, y: 0, label: "RTGS" },
                    //            { x: 5, y: 0, label: "Card" },
                    //            { x: 6, y: 0, label: "ECS" },
                    //             { x: 7, y: $scope.emailcnt, label: "Defaulters" },
                    //            ]
                    //            }
                    //            ]

                    //    });

                    //chart.render();


                    $scope.exportToExcel = function () {
                        
                        if (promise.reportdatelist.length > 0 && promise.reportdatelist != null) {
                            var exportHref = Excel.tableToExcel(tablegrp, 'sheet name');
                            $timeout(function () { location.href = exportHref; }, 100);
                        }
                        $state.reload();
                    }

                    $scope.printData = function () {
                        
                        if (promise.reportdatelist.length > 0 && promise.reportdatelist != null) {

                   

                            var base64Image = chart.canvas.toDataURL();
                            document.getElementById('rangeBarChat').style.display = 'none';
                            document.getElementById('chartImage').src = base64Image;
                            var innerContents = document.getElementById("tablegrp").innerHTML;
                            var popupWinindow = window.open('');
                            popupWinindow.document.open();
                            popupWinindow.document.write('<html><head>' +
               '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
          '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                            $scope.showGrafh = true;
                            popupWinindow.document.close();


                        }
                        $state.reload();
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
    }
})();

