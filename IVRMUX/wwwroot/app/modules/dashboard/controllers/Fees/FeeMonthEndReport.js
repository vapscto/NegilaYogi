

(function () {
    'use strict';
    angular
        .module('app')
        .controller('MothEndReportController', MothEndReportController123)

    MothEndReportController123.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function MothEndReportController123($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {

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
            apiService.getURI("MothEndReport/getalldetails123", pageid).
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

                if ($scope.usercheck1 == undefined) {
                    $scope.usercheck1 = 0;
                }


                var data = {
                    "yearid": $scope.academicyr,
                    //"frmdate": $scope.from_date,
                    "acayid": $scope.usercheck1,
                    "type": $scope.usercheck,
                    "month": $scope.monthmodel,
                    "year": $scope.yearmodel,
                }




                apiService.create("MothEndReport/getreport", data).
                    then(function (promise) {

                        if (promise.reportdatelist.length > 0 && promise.reportdatelist != null) {
                            $scope.monthmodelvalue = $scope.from_date;
                            $scope.report = true;
                            $scope.showGrafh = true;
                            $scope.name = $scope.IEname;
                            $scope.grouptype = promise.monthpass;
                            $scope.fcbbcD = promise.reportdatelist[0].cashcount;
                            $scope.fcbcc = promise.reportdatelist[0].bankcount;
                            $scope.fcboc = promise.reportdatelist[0].onlinecount;
                            $scope.fcbEc = promise.reportdatelist[0].rtgs;
                            $scope.frbcc = promise.reportdatelist[0].cardcount;
                            $scope.frbbecs = promise.reportdatelist[0].ecs;
                            $scope.frdef = promise.reportdatelist[0].defaulters;
                            $scope.frrefd = promise.reportdatelist[0].refund;
                            $scope.frrefdb = promise.reportdatelist[0].refundbank;
                            $scope.smscnt = promise.reportdatelist[0].smscount;
                            $scope.emailcnt = promise.reportdatelist[0].emailcount;

                            var monthNames = ["", "January", "February", "March", "April", "May", "June",
                                "July", "August", "September", "October", "November", "December"
                            ];


                            //var d = $scope.FMCB_fromDATE;
                            //var month = monthNames[d.getMonth()]
                            $scope.monthmodel = $scope.monthmodel;
                            var month = monthNames[$scope.monthmodel];
                            $scope.month = month;
                            $scope.designation = "Implementation Engineer";
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


                            var chart = new CanvasJS.Chart("rangeBarChat",
                                {

                                    width: 900,

                                    axisX: {
                                        interval: 1
                                    },
                                    axisY2: {
                                        interval: 1

                                    },



                                    data: [
                                        {
                                            type: "column",
                                            showInLegend: true,
                                            legendText: "Count",
                                            color: "gold",
                                            dataPoints: [
                                                { x: 1, y: $scope.fcbbcD, label: "Cash" },
                                                { x: 2, y: $scope.fcbcc, label: "Bank" },
                                                { x: 3, y: $scope.fcboc, label: "Online" },
                                                { x: 4, y: $scope.fcbEc, label: "RTGS" },
                                                { x: 5, y: $scope.frbcc, label: "Card" },
                                                { x: 6, y: $scope.frbbecs, label: "ECS" },
                                                { x: 7, y: $scope.frrefd, label: "CashRefund" },
                                                { x: 8, y: $scope.frrefdb, label: "BankRefund" },
                                                { x: 9, y: $scope.frdef, label: "Defaulters" }
                                            ]
                                        }
                                        // {
                                        // type: "column",
                                        // showInLegend: true,
                                        // legendText: "SMS",
                                        // color: "silver",
                                        // dataPoints: [
                                        // { x: 1, y: 0, label: "SMS" },
                                        // { x: 2, y: 0, label: "SMS" },
                                        // { x: 3, y: 0, label: "SMS" },
                                        // { x: 4, y: 0, label: "SMS" },
                                        // { x: 5, y: 0, label: "SMS" },
                                        // { x: 6, y: 0, label: "SMS" },
                                        // { x: 7, y: 0, label: "SMS" },
                                        // { x: 8, y: 0, label: "SMS" },
                                        //{ x: 9, y: 0, label: "SMS" },

                                        // ]
                                        // },
                                        // {
                                        // type: "column",
                                        // showInLegend: true,
                                        // legendText: "Email",
                                        // color: "#DCA978",
                                        // dataPoints: [
                                        // { x: 1, y: 0, label: "Cash" },
                                        // { x: 2, y: 0, label: "Bank" },
                                        // { x: 3, y: 0, label: "Online" },
                                        // { x: 4, y: 0, label: "RTGS" },
                                        // { x: 5, y: 0, label: "Card" },
                                        // { x: 6, y: 0, label: "ECS" },
                                        // { x: 7, y: 0, label: "CashRefund" },
                                        // { x: 8, y: 0, label: "BankRefund" },
                                        //  { x: 9, y: 0, label: "Defaulters" },
                                        // ]
                                        // }
                                    ]

                                });

                            chart.render();


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
                                    popupWinindow.document.close();
                                }
                                $state.reload();
                            }

                            app.controller('FileController', function ($scope) {
                                $scope.exportToPDF = function () {
                                    var fileInput = document.getElementById('fileInput');
                                    var file = fileInput.files[0];

                                    if (file) {
                                        var formData = new FormData();
                                        formData.append('file', file);

                                        fetch('/api/convertToPDF', {
                                            method: 'POST',
                                            body: formData
                                        })
                                            .then(response => response.blob())
                                            .then(blob => {
                                                var url = window.URL.createObjectURL(blob);
                                                window.open(url, '_blank');
                                            })
                                            .catch(error => console.error('Error:', error));
                                    }
                                };
                            });



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

