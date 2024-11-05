(function () {
    'use strict';
    angular
.module('app')
        .controller('ClgExamMonthEndReportController', ClgExamMonthEndReportController)

    ClgExamMonthEndReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function ClgExamMonthEndReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {

        $scope.BindData = function () {
            apiService.getDATA("ClgExamMonthEndReport/getdetails").
            then(function (promise) {

                $scope.acdlist = promise.acdlist;
                $scope.examlist = promise.examlist;
                $scope.monthlist = promise.monthlist;
            })
        };


        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.date_m = new Date();

        $scope.submitted = false;
        $scope.onreport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
            
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EME_Id": $scope.EME_Id,
                    "IVRM_Month_Id": $scope.IVRM_Month_Id
                }

                apiService.create("ClgExamMonthEndReport/onreport", data).
                then(function (promise) {
                    
                    for (var i = 0; i < $scope.monthlist.length; i++) {
                        if ($scope.monthlist[i].ivrM_Month_Id == $scope.IVRM_Month_Id) {
                            $scope.Month_Name = $scope.monthlist[i].ivrM_Month_Name;
                        }
                    }
                    for (var j = 0; j < $scope.acdlist.length; j++) {
                        if ($scope.acdlist[j].asmaY_Id == $scope.ASMAY_Id) {
                            $scope.Year_Name = $scope.acdlist[j].asmaY_Year;
                        }
                    }

                    $scope.main_list = promise.datareport;


                    $scope.stud_graph = [];
                    if (promise.datareport != "0" && promise.datareport != null) {
                        $scope.studgraph = false;
                        for (var i = 0; i < $scope.main_list.length; i++) {
                            $scope.stud_graph.push({ label: $scope.main_list[i].pass, "y": $scope.main_list[i].pass})
                        }

                        $scope.stud_graph2 = [];

                        for (var i = 0; i < $scope.main_list.length; i++) {
                            $scope.stud_graph2.push({ label: $scope.main_list[i].fail, "y": parseInt($scope.main_list[i].fail) })
                        }


                        var chart = new CanvasJS.Chart("linechart", {
                            animationEnabled: true,
                            animationDuration: 3000,
                            axisX: {
                                labelFontSize: 12,
                            },
                            axisY: {
                                labelFontSize: 12,
                            },

                            toolTip: {
                                shared: true
                            },
                            data: [{
                                name: "PASS",
                                showInLegend: true,
                                type: "column",
                                color: "green",
                                dataPoints: $scope.stud_graph
                            },
                            {
                                name: "FAIL",
                                showInLegend: true,
                                type: "column",
                                color: "red",
                                dataPoints: $scope.stud_graph2
                            }
                            ]
                        });
                        chart.render();

                        $scope.printData = function () {
                            

                            if (promise.datareport.length > 0 && promise.datareport != null) {
                                var base64Image = chart.canvas.toDataURL();
                                document.getElementById('linechart').style.display = 'none';
                                document.getElementById('chartImage').src = base64Image;
                                var innerContents = document.getElementById("table").innerHTML;
                              
                                var popupWinindow = window.open('');
                                popupWinindow.document.open();
                                popupWinindow.document.write('<html><head>' +
                   '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');

                                popupWinindow.document.close();


                            }
                          //  $state.reload();
                        };

                    }
                    else {
                        swal("No Record Found....")
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