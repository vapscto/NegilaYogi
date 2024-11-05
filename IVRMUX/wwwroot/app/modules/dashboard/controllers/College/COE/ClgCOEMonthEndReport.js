
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgCOEReportController', ClgCOEReportController)

    ClgCOEReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', 'Excel', '$timeout', '$stateParams', '$filter', '$sce']
    function ClgCOEReportController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, Excel, $timeout, $stateParams, $filter, $sce) {




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
        if (ivrmcofigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;



        $scope.imgdiv = false;


        $scope.loaddata = function () {
            var id = 2;
            apiService.getURI("ClgCOEReport/", id).
                then(function (promise) {
                    $scope.fillyear = promise.fillyear;

                });
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.Clear = function () {
            $state.reload();
        }

        $scope.showReportdata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.year,
                    "monthid": $scope.month,
                }

                apiService.create("ClgCOEReport/mothreport", data).
                    then(function (promise) {
                        if (promise.count > 0 && promise.count != null) {

                            $scope.report = true;
                            // $scope.showGrafh = true;

                            $scope.totalCount = promise.count;
                            $scope.emailCount = promise.eventDesc;
                            $scope.imgname = promise.yearName;
                            $scope.smsCount = promise.eventName;
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
                                name: "COUNT",
                                showInLegend: true,

                                dataPoints: [
                                    { y: $scope.totalCount, label: "COE COUNT" },
                                    { y: parseInt($scope.smsCount), label: "SMS COUNT" },
                                    { y: parseInt($scope.emailCount), label: "EMAIL COUNT" }
                                ]
                            };
                            chart.options.data = [];
                            chart.options.data.push(series1);
                            chart.render();

                            $scope.exportToExcel = function () {
                                if (promise.count > 0 && promise.count != null) {
                                    var exportHref = Excel.tableToExcel(tablegrp, 'sheet name');
                                    $timeout(function () { location.href = exportHref; }, 100);
                                }
                            }
                            $scope.printData = function () {

                                if (promise.count > 0 && promise.count != null) {
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