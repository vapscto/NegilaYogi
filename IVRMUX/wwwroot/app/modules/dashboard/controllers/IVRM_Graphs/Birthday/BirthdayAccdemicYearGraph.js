(function () {
    'use strict';

    angular
        .module('app')
        .controller('BirthdayAccdemicYearGraphController', BirthdayAccdemicYearGraphController);

    BirthdayAccdemicYearGraphController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']

    function BirthdayAccdemicYearGraphController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {



        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        if (configsettings != null && configsettings.length > 0) {
            var institutionid = configsettings[0].mI_Id;
        }

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        $scope.LoadData = function () {
            var pageid = 1;
            apiService.getURI("BirthDayGraphs/", pageid).
                then(function (promise) {
                    $scope.AccdemicYear = promise.accyear;

                });
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.togchkbx = function () {
            $scope.usercheck = $scope.AccdemicYear.every(function (options) {
                return options.checked;
            });
        };
        $scope.isOptionsRequired = function () {
            return !$scope.AccdemicYear.some(function (options) {
                return options.checked;
            });
        };
        $scope.all_check = function () {
            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.AccdemicYear, function (itm) {
                itm.checked = toggleStatus;
            })
        };
        $scope.clear = function () {
            $state.reload();
        }
        //////////////"column
        //two
        function createChart1() {
            $("#chart1").kendoChart({
                title: {
                    text: "Month wise Student B'day Email and SMS Count"
                },
                legend: {
                    position: "bottom"
                },
                chartArea: {
                    background: ""
                },
                seriesDefaults: {
                    type: "line",
                    style: "smooth"
                },
                series: [{
                    name: "Email Count",
                    data: [44, 68, 72, 68, 20, 35, 74, 34, 62, 64, 44, 49]
                }, {
                    name: "SMS Count",
                    data: [40, 56, 89, 49, 100, 40, 86, 43, 43, 47, 56, 47]
                },
                ],
                valueAxis: {
                    labels: {
                        format: "{0}%"
                    },
                    line: {
                        visible: false
                    },
                    axisCrossingValue: -10
                },
                categoryAxis: {
                    categories: ["January", "February", "March", "April", "May", "June", "July", "August", "Setember", "October", "November", "December"],
                    majorGridLines: {
                        visible: false
                    },
                    labels: {
                        rotation: "auto"
                    }
                },
                tooltip: {
                    visible: true,
                    format: "{0}%",
                    template: "#= series.name #: #= value #"
                }
            });
        }
        $scope.submitted = false;
        $scope.Report = function () {
            $scope.submitted = true; $scope.sectionDrpDwn = []; $scope.sms_mail_count = [];
            $scope.ASMAY_IdList = []; $scope.Asmay_Year = []; $scope.accyear = []; $scope.templist = [];
            $scope.templisttwo = []; $scope.templistthree = []; $scope.tempvar = [];
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
                   // "ASMAY_IdList": $scope.ASMAY_IdList,
                    "ASMAY_Id": $scope.asmaY_Id
                };
                apiService.create("BirthDayGraphs/getReport", data).then(function (promise) {
                    $scope.sms_mail_count = promise.sms_mail_count;
                    $scope.sms_mail_countgrid = promise.sms_mail_count;
                    $scope.accyeartemp = promise.accyear;
                    if ($scope.sms_mail_countgrid != null && $scope.sms_mail_countgrid.length > 0) {

                        if ($scope.accyeartemp != null && $scope.accyeartemp.length > 0) {
                            angular.forEach($scope.accyeartemp, function (c) {
                                $scope.Asmay_Year.push(c.asmaY_Year);
                            });
                        }
                        if ($scope.sms_mail_count != null && $scope.sms_mail_count.length > 0) {
                            angular.forEach($scope.sms_mail_count, function (ddtt) {
                                $scope.templist.push({
                                    month_name: ddtt.month_name,
                                }
                                )
                            })
                        }

                        var names = "";
                        angular.forEach($scope.sms_mail_count, function (ddf) {
                            names = ddf.month_name;
                            angular.forEach($scope.templist, function (ddR) {
                                if (ddR.month_name == ddf.month_name) {
                                    angular.forEach($scope.accyeartemp, function (ddy) {
                                        var asmaY_Year = ddf[ddy.asmaY_Year];
                                        $scope.tempvar = [];
                                        $scope.tempvar.push(asmaY_Year);
                                        $scope.templistthree.push({
                                            name: names,
                                            data: $scope.tempvar
                                        })
                                    })
                                }

                            })

                        })
                        $("#chart123").kendoChart({
                            title: {
                                text: "Year wise Birthday Count report"
                            },
                            legend: {
                                position: "top"
                            },
                            seriesDefaults: {
                                type: "column"
                            },
                            series: $scope.templistthree,

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
                                categories: $scope.Asmay_Year,
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


                        ////////////////////////////bar
                        //////////////////////////////////// pie
                        function createChart2() {
                            $("#chart2").kendoChart({
                                title: {
                                    position: "bottom",
                                    text: "Year wise Birthday Count report "
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
                                        color: "#9de219"
                                    }, {
                                        category: "2020-2021",
                                        value: 91.1,
                                        color: "#90cc38"
                                    }, {
                                        category: "2021-2022",
                                        value: 98.3,
                                        color: "#068c35"
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
                    else {
                        swal("Record Not Found !");
                    }
                });

            } else {
                $scope.submitted1 = true;
            }
        };

    }
})();