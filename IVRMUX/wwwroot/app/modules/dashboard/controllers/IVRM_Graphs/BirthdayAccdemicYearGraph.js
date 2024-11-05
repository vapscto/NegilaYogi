(function () {
    'use strict';

    angular
        .module('app')
        .controller('BirthdayAccdemicYearGraph', BirthdayAccdemicYearGraph);

    BirthdayAccdemicYearGraph.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']

    function BirthdayAccdemicYearGraph($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {



        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

      
        //////////////"column
        var chat = "2019-2020"; var chartwo = "2020-2021"; var chartthree = "2021-2022";
        $("#chart123").kendoChart({
            title: {
                text: "YEAR WISE BIRTHDAY COUNT REPORT "
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column"
            },
            series: [{
                name: "January",
                data: [44, 40, 50]
            }, {
                name: "February",
                data: [68, 56, 96]
            }, {
                name: "March",
                data: [72, 89, 63]
            },
            {
                name: "April",
                data: [68, 49, 59]
            },
            {
                name: "May",
                data: [20, 100, 93]
            },
            {
                name: "June",
                data: [35, 40, 50]
            },

            {
                name: "July",
                data: [74, 86, 46]
            },
            {
                name: "August",
                data: [34, 43, 56]
            },
            {
                name: "September",
                data: [62, 43, 53]
            },
            {
                name: "October",
                data: [64, 47, 53]
            },
            {
                name: "November",
                data: [44, 56, 75]
            },
            {
                name: "December",
                data: [49, 47, 23]
            },
            ],
            valueAxis: {
                labels: {
                    format: "{0}"
                },
                line: {
                    visible: false
                },
                axisCrossingValue: 0
            },
            categoryAxis: {
                categories: [chat, chartwo, chartthree],
                line: {
                    visible: false
                },
                labels: {
                    padding: { top: 10 }
                }
            },
            tooltip: {
                visible: true,
                format: "{0}",
                template: "#= series.name #: #= value #"
            }
        });


        ////////////////////////////bar
        //////////////////////////////////// pie
        function createChart2() {
            $("#chart2").kendoChart({
                title: {
                    position: "bottom",
                    text: "YEAR WISE BIRTHDAY PERCENTAGE REPORT"
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
                    format: "{0}"
                }
            });
        }

        $(document).ready(createChart2);
        $(document).bind("kendo:skinChange", createChart2);

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
                        format: "{0}"
                    },
                    line: {
                        visible: false
                    },
                    axisCrossingValue: -10
                },
                categoryAxis: {
                    categories: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
                    majorGridLines: {
                        visible: false
                    },
                    labels: {
                        rotation: "auto"
                    }
                },
                tooltip: {
                    visible: true,
                    format: "{0}",
                    template: "#= series.name #: #= value #"
                }
            });
        }
    }
})();