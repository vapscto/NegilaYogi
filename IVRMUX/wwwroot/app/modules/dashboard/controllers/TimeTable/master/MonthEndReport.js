
(function () {
    'use strict';
    angular
        .module('app')
        .controller('TTMonthEndReportController', TTMonthEndReportController)

    TTMonthEndReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function TTMonthEndReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.loaddata = function () {
            $scope.report = false;
            apiService.getDATA("TTMonthEndReport/getalldetails123").
                then(function (promise) {
                    $scope.acayyearbind = promise.acdlist;
                    $scope.month_name = promise.monthlist;
                })
        }

     

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clear_Details = function () {
            $state.reload();
            $scope.loaddata();
        }


        $scope.ShowReportdata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "month": $scope.monthmodel,
                    "ASMAY_ID": $scope.academicyr,
                }
                apiService.create("TTMonthEndReport/getreport", data).
                    then(function (promise) {
                        if (promise.reportdatelist.length > 0 && promise.reportdatelist != null) {
                            $scope.report = true;
                            $scope.showGrafh = true;
                            $scope.datapages = promise.reportdatelist;

                            angular.forEach(promise.reportdatelist, function (y) {
                                y.label = y.ClassName;
                                y.y = y.count;
                            })


                            var monthNames = ["", "January", "February", "March", "April", "May", "June",
                                "July", "August", "September", "October", "November", "December"
                            ];
                            $scope.monthmodel = $scope.monthmodel;
                            var month = monthNames[$scope.monthmodel];
                            $scope.month = month;
                            $scope.designation = "Implimentation Engineer";
                            $scope.today = new Date();
                            $scope.acayearnow = $scope.acayear;
                            $scope.report = true;                           
                            angular.forEach($scope.acayyearbind, function (y) {
                                if (y.asmaY_Id === $scope.academicyr) {
                                    $scope.acdyr = y.asmaY_Year;
                                }
                            })
                            var chart = new CanvasJS.Chart("linechart",
                                {
                                    width: 900,
                                    axisX: {interval: 1 },
                                    axisY2: {interval: 1},
                                    data: [
                                        {
                                            type: "column",
                                            name: "SMS",
                                            dataPoints: promise.reportdatelist
                                        },
                                        {
                                            type: "column",
                                            name: "EMAIL",
                                            dataPoints: promise.reportdatelist
                                        }
                                    ]
                                });

                            chart.render();


                            $scope.exportToExcel = function () {

                                if (promise.reportdatelist.length > 0 && promise.reportdatelist != null) {
                                    var exportHref = Excel.tableToExcel(tablegrp, 'sheet name');
                                    $timeout(function () { location.href = exportHref; }, 100);
                                }
                              //  $state.reload();
                            }

                            $scope.printData = function () {
                                if (promise.reportdatelist.length > 0 && promise.reportdatelist != null) {
                                    var base64Image = chart.canvas.toDataURL();  
                                    document.getElementById('linechart').style.display = 'none';
                                    document.getElementById('chartContainer').src = base64Image;
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

