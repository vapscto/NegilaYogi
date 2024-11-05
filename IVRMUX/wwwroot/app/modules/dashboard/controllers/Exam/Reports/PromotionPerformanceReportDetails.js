(function () {
    'use strict';
    angular.module('app').controller('PromotionPerformanceReportDetailsController', PromotionPerformanceReportDetailsController)

    PromotionPerformanceReportDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function PromotionPerformanceReportDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

        var paginationformasters = '';
        var ivrmcofigsettings = [];
        var copty;
        ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = [];

        $scope.coptyright = copty;
        $scope.ddate = new Date();
        var logopath = "";
        admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        $scope.Flag = "all";
        $scope.print_flag = true;


        //***** COLOR ARRAY
        CanvasJS.addColorSet("graphcolor",
            [
                "#3498DB",
                "#76D7C4",
                "#808B96",
                "#80DEEA",
                "#C5E1A5",
                "#AAB7B8"
            ]);

        $scope.reportdata = [];
        $scope.onpageload = function () {
            var pageid = 2;
            apiService.getURI("PromotionReportDetails/onpageload", pageid).then(function (promise) {
                $scope.classlist = promise.allclasslist;
            });
        };

        $scope.onchangeclass = function () {
            $scope.reportdata = [];
        };;

        $scope.PromotionPerformanceReport = function () {
            $scope.reportdata = [];
            if ($scope.myForm.$valid) {

                var data = {
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "allorindi": 'overall'
                };
                apiService.create("PromotionReportDetails/PromotionPerformanceReport", data).then(function (promise) {

                    if (promise !== null) {
                        $scope.reportdata = promise.reportdata;

                        $scope.feegraph1 = [];
                        if ($scope.reportdata !== undefined && $scope.reportdata !== null && $scope.reportdata.length > 0) {
                            for (var a = 0; a < $scope.reportdata.length; a++) {
                                $scope.feegraph1.push({ label: $scope.reportdata[a].ASMAY_Year, "y": $scope.reportdata[a].PassCount });
                            }
                        }
                      
                        $scope.feegraph2 = [];
                        if ($scope.reportdata !== undefined && $scope.reportdata !== null && $scope.reportdata.length > 0) {
                            for (var a = 0; a < $scope.reportdata.length; a++) {
                                $scope.feegraph2.push({ label: $scope.reportdata[a].ASMAY_Year, "y": $scope.reportdata[a].FailedCount });
                            }
                        }                     
                        

                        var chart = new CanvasJS.Chart("rangeBarChat", {
                            animationEnabled: true,
                            animationDuration: 3000,
                            height: 350,
                            colorSet: "graphcolor",
                            axisX: {
                                labelFontSize: 13,
                            },
                            axisY: {
                                labelFontSize: 13,
                            },

                            toolTip: {
                                shared: true
                            },
                            data: [{
                                name: "Pass",
                                showInLegend: true,
                                type: "column",                                
                                dataPoints: $scope.feegraph1
                            },
                            {
                                name: "Fail",
                                showInLegend: true,
                                type: "column",                               
                                dataPoints: $scope.feegraph2
                            }]
                        });
                        chart.render();


                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $state.interacted = function () {
            return $scope.submitted;
        };


        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();