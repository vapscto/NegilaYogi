(function () {
    'use strict';
    angular
        .module('app')
        .controller('MonthEndReportTrnsController', MonthEndReportTrnsController)

    MonthEndReportTrnsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function MonthEndReportTrnsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {
        $scope.type = 'T';
        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("TrnsMonthEndReport/getdata1", pageid).then(function (promise) {
                if (promise != null) {

                    $scope.acayyearbind = promise.acdlist;

                    $scope.monthlist = promise.monthlist;

                }
                else {
                    swal("No Records Found")
                }
            })

        }
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.date_m = new Date();

        var temp = [];
        var year = "";

        $scope.onselectAcdYear = function () {
            temp = [];
            angular.forEach($scope.acayyearbind, function (itm) {
                if (itm.asmaY_Id == $scope.ASMAY_Id) {
                    year = itm.asmaY_Year
                }
            });
            var s1 = year.substring(0, 4);
            var s2 = year.substring(year.length, 5);
            temp.push({ asmaY_Id: 0, asmaY_Year: s1 });
            temp.push({ asmaY_Id: 1, asmaY_Year: s2 });
            $scope.years = temp;
        }
        $scope.report = '';
        $scope.submitted = false;
        $scope.onreport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "IVRM_Month_Id": $scope.IVRM_Month_Id,
                    "year": $scope.yearmodel,
                    "type": $scope.type,
                }

                apiService.create("TrnsMonthEndReport/savedata1", data).
                    then(function (promise) {

                        for (var i = 0; i < $scope.monthlist.length; i++) {
                            if ($scope.monthlist[i].ivrM_Month_Id == $scope.IVRM_Month_Id) {
                                $scope.Month_Name = $scope.monthlist[i].ivrM_Month_Name;
                            }
                        }
                        for (var j = 0; j < $scope.acayyearbind.length; j++) {
                            if ($scope.acayyearbind[j].asmaY_Id == $scope.ASMAY_Id) {
                                $scope.Year_Name = $scope.acayyearbind[j].asmaY_Year;
                            }
                        }
                        $scope.main_list = promise.griddata;
                        debugger;



                        $scope.temparraylist = [];
                        angular.forEach($scope.main_list, function (ff) {

                            if ($scope.type == 'T') {
                                $scope.report ='TRANSPORT/VEHICLE HIRE'

                                if (ff.MFIELDS == 'TRNSMSCOUNT') {
                                    $scope.temparraylist.push({ ENAME: 'TRNSMSCOUNT', TNAME: 'TRANSPORT SMS', COUNT: ff.MCOUNT })
                                }
                                if (ff.MFIELDS == 'TRNEMAILCOUNT') {
                                    $scope.temparraylist.push({ ENAME: 'TRNEMAILCOUNT', TNAME: 'TRANSPORT EMAIL', COUNT: ff.MCOUNT })
                                }

                                if (ff.MFIELDS == 'BUSSMSCOUNT') {
                                    $scope.temparraylist.push({ ENAME: 'BUSSMSCOUNT', TNAME: 'BUS HIRE SMS', COUNT: ff.MCOUNT })
                                }
                                if (ff.MFIELDS == 'BUSEMAILCOUNT') {
                                    $scope.temparraylist.push({ ENAME: 'TRNEMAILCOUNT', TNAME: 'BUS HIRE EMAIL', COUNT: ff.MCOUNT })
                                }

                                if (ff.MFIELDS == 'APPLTOTALCOUNT') {
                                    $scope.temparraylist.push({ ENAME: 'APPLTOTALCOUNT', TNAME: 'TOTAL TRANSPORT APPLICATION', COUNT: ff.MCOUNT })
                                }
                                if (ff.MFIELDS == 'APRVLCOUNT') {
                                    $scope.temparraylist.push({ ENAME: 'APRVLCOUNT', TNAME: 'APPROVED', COUNT: ff.MCOUNT })
                                }
                                if (ff.MFIELDS == 'REJCOUNT') {
                                    $scope.temparraylist.push({ ENAME: 'REJCOUNT', TNAME: 'REJECTED', COUNT: ff.MCOUNT })
                                }
                                if (ff.MFIELDS == 'WAITCOUNT') {
                                    $scope.temparraylist.push({ ENAME: 'WAITCOUNT', TNAME: 'WAITING', COUNT: ff.MCOUNT })
                                }

                                if (ff.MFIELDS == 'TOTALTRIP') {
                                    $scope.temparraylist.push({ ENAME: 'TOTALTRIP', TNAME: 'TOTAL TRIP BOOKED', COUNT: ff.MCOUNT })
                                }
                                if (ff.MFIELDS == 'VCOUNT') {
                                    $scope.temparraylist.push({ ENAME: 'VCOUNT', TNAME: 'TOTAL VEHICLES', COUNT: ff.MCOUNT })
                                }

                            }
                            else if ($scope.type == 'F') {
                                $scope.report = 'VEHICLE FUEL'
                                if (ff.MFIELDS == 'TOTALKM') {
                                    $scope.temparraylist.push({ ENAME: 'TOTALKM', TNAME: 'TOTAL K.M', COUNT: ff.MCOUNT })
                                }
                                if (ff.MFIELDS == 'TOTALMILEAGE') {
                                    $scope.temparraylist.push({ ENAME: 'TOTALMILEAGE', TNAME: 'TOTAL MILEAGE', COUNT: ff.MCOUNT })
                                }

                                if (ff.MFIELDS == 'NOOFLTR') {
                                    $scope.temparraylist.push({ ENAME: 'NOOFLTR', TNAME: 'TOTAL NO OF LTR.', COUNT: ff.MCOUNT })
                                }
                                if (ff.MFIELDS == 'TOTALAMOUNT') {
                                    $scope.temparraylist.push({ ENAME: 'TOTALAMOUNT', TNAME: 'TOTAL AMOUNT', COUNT: ff.MCOUNT })
                                }
                                if (ff.MFIELDS == 'GROSSAMOUNT') {
                                    $scope.temparraylist.push({ ENAME: 'GROSSAMOUNT', TNAME: 'TOTAL GROSS AMOUNT', COUNT: ff.MCOUNT })
                                }
                            }
                           
                           
                        })

                        console.log($scope.temparraylist);

                        console.log($scope.main_list);
                        $scope.stud_graph = [];
                        if ($scope.temparraylist != "0" && $scope.temparraylist != null) {
                            $scope.studgraph = false;
                            for (var i = 0; i < $scope.temparraylist.length; i++) {
                                $scope.stud_graph.push({ label: $scope.temparraylist[i].TNAME, "y": $scope.temparraylist[i].COUNT })
                            }


                            var chart = new CanvasJS.Chart("linechart", {
                                height: 350,
                                width: 1065,
                                axisX: {
                                    labelFontSize: 12,
                                    interval: 1,
                                    labelAngle: -20,
                                },
                                axisY: {
                                    labelFontSize: 12,
                                    //    maximum: 90
                                    //scaleBreaks: {
                                    //    customBreaks: [{
                                    //        startValue: 100,
                                    //        endValue: 400,
                                    //        color: "orange"
                                    //    }]
                                    //}

                                },

                                data: [
                                    {
                                        type: "column",
                                        showInLegend: true,
                                        color: "Blue",
                                        dataPoints: $scope.stud_graph
                                    }
                                ]
                            });

                            chart.render();

                            $scope.printData = function () {
                                if ($scope.temparraylist.length > 0 && $scope.temparraylist != null) {
                                    var base64Image = chart.canvas.toDataURL();
                                    document.getElementById('linechart').style.display = 'none';
                                    document.getElementById('chartImage').src = base64Image;
                                    var innerContents = document.getElementById("table").innerHTML;

                                    var popupWinindow = window.open('');
                                    popupWinindow.document.open();
                                    popupWinindow.document.write('<html><head>' +
                                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/LIbMonthEndReportPdf.css" />' +
                                        '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 1000);">' + innerContents + '</html>');

                                    popupWinindow.document.close();


                                }
                                //  $state.reload();
                            };

                        }
                        else {
                            swal("No Record Found....");
                        }

                    })
            }
        };



        $scope.cancel = function () {
            $scope.ASMAY_Id = '';
            $scope.EME_Id = '';
            $scope.IVRM_Month_Id = '';
            $scope.main_list = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };



    }

})();