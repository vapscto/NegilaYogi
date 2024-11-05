(function () {
    'use strict';

    angular
        .module('app')
        .controller('CoEAccdemicYearGraphController', CoEAccdemicYearGraphController);
    CoEAccdemicYearGraphController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function CoEAccdemicYearGraphController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {
        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        $scope.LoadData = function () {
            var pageid = 1;
            apiService.getURI("CoeReportGraph/getdata", pageid).
                then(function (promise) {
                    $scope.AccdemicYear = promise.fillyear;
                });
        }
        $scope.Report = function () {
            $scope.submitted = true; $scope.coereport = []; $scope.accyeartemp = []; $scope.ASMAY_IdList = [];
            $scope.monthname = []; $scope.temparray = [];
            if ($scope.myForm.$valid) {
                if ($scope.AccdemicYear != null && $scope.AccdemicYear.length > 0) {
                    angular.forEach($scope.AccdemicYear, function (c) {
                        if (c.checked == true) {
                            $scope.ASMAY_IdList.push({
                                ASMAY_Id: c.asmaY_Id
                            })
                        }
                    });
                }
                var data = {
                    "ASMAY_IdList": $scope.ASMAY_IdList,
                    // "ASMAY_Id": $scope.asmaY_Id
                };
                apiService.create("CoeReportGraph/getReport", data).then(function (promise) {
                    $scope.coereport = promise.coereport;
                    $scope.accyeartemp = promise.fillyear;
                    if ($scope.coereport != null && $scope.coereport.length > 0) {
                        angular.forEach($scope.coereport, function (ddy) {
                            $scope.monthname.push(ddy.IVRM_Month_Name);
                        })
                        angular.forEach($scope.coereport, function (ddyt) {
                            angular.forEach($scope.accyeartemp, function (dd) {
                                $scope.pusharray = [];
                                var asmaY_Year = ddyt[dd.asmaY_Year];
                                $scope.pusharray.push(asmaY_Year);
                                //temparray
                                $scope.temparray.push({
                                    name: ddyt.IVRM_Month_Name,
                                    data: $scope.pusharray
                                })
                            })
                           
                        })


                      
                        //angular.forEach($scope.templist, function (ddR) {
                        //    if (ddR.month_name == ddf.month_name) {
                               
                        //    }

                        //})

                        $("#chart123").kendoChart({
                            title: {
                                text: "Year wise Coe Count report"
                            },
                            legend: {
                                position: "top"
                            },
                            seriesDefaults: {
                                type: "column"
                            },
                            // series:
                            series: [
                                {
                                    name: "2019-2020",
                                    color: "#808080",
                                    data: [44, 68, 72, 89, 20, 100, 74, 86, 62, 43, 44, 56],
                                },
                                {
                                    name: "2020-2021",
                                    color: "#F3A00E",
                                    data: [40, 56, 89, 49, 35, 45, 34, 43, 64, 47, 49, 47],
                                },
                                {
                                    name: "2021-2022",
                                    color: "#2874A6 ",
                                    data: [50, 96, 63, 89, 20, 100, 74, 86, 62, 43, 44, 56],
                                },

                            ],
                            valueAxis: {
                                labels: {
                                    format: "{0}%"
                                },
                                line: {
                                    visible: false
                                },
                                axisCrossingValue: 0
                            },
                            categoryAxis: {
                                // categories: ["January", "February", "March", "April", "May", "June", "July", "August", "Setember", "October", "November", "December"],
                                categories: $scope.monthname,
                                line: {
                                    visible: false
                                },
                                labels: {
                                    padding: { top: 10 }
                                }
                            },
                            tooltip: {
                                visible: true,
                                format: "{0}%",
                                template: "#= series.name #: #= value #"
                            }
                        });



                        function createChart2() {
                            $("#chart2").kendoChart({
                                title: {
                                    position: "bottom",
                                    text: "Year wise Coe Count report"
                                },
                                legend: {
                                    visible: false
                                },
                                chartArea: {
                                    background: ""
                                },
                                seriesDefaults: {
                                    labels: {
                                        visible: true,
                                        background: "transparent",
                                        template: "#= category #: \n #= value#%"
                                    }
                                },
                                series: [{
                                    type: "pie",
                                    startAngle: 150,
                                    data: [{
                                        category: "2019-2020",
                                        value: 83.8,
                                        color: "#FFD700"
                                    }, {
                                        category: "2020-2021",
                                        value: 91.1,
                                        color: "#FFA500"
                                    }, {
                                        category: "2021-2022",
                                        value: 98.3,
                                        color: "#808080"
                                    },
                                    ]
                                }],
                                tooltip: {
                                    visible: true,
                                    format: "{0}%"
                                }
                            });
                        }

                        $(document).ready(createChart2);
                        $(document).bind("kendo:skinChange", createChart2);
                    }



                });
            }
            else {
                $scope.submitted1 = true;
            }
        };
        $scope.clear = function () {
            $state.reload();
        }


    }
})();