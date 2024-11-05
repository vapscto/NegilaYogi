(function () {
    'use strict';

    angular
        .module('app')
        .controller('CoEAccdemicYearGraph', CoEAccdemicYearGraph);

    CoEAccdemicYearGraph.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']

    function CoEAccdemicYearGraph($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {



        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));


        $("#chart123").kendoChart({
            title: {
                text: "YEAR WISE COE COUNT REPORT"
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column"
            },
            series: [{
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
                //}, {
                //    name: "February",
                //    color: "#F3A00E",
                //    data: [68, 56]
                //}, {
                //    name: "March",
                //    color: "#FF00FF",
                //    data: [72, 89]
                //},
                //{
                //    name: "April",
                //    color: "#5D6D7E",
                //    data: [68, 49]
                //},
                //{
                //    name: "May",
                //    color: "#BC8F8F",
                //    data: [20, 100]
                //},
                //{
                //    name: "June",
                //    color: "#7B241C",
                //    data: [35, 40]
                //},

                //{
                //    name: "July",
                //    color: "#482525",
                //    data: [74, 86]
                //},
                //{
                //    name: "August",
                //    color: "#808000",
                //    data: [34, 43]
                //},
                //{
                //    name: "Setember",
                //    color: "#800080",
                //    data: [62, 43]
                //},
                //{
                //    name: "October",
                //    color: "#000080",
                //    data: [64, 47]
                //},
                //{
                //    name: "November",
                //    color: "#808000",
                //    data: [44, 56]
                //},
                //{
                //    name: "December",
                //    color: "#32CD32",
                //    data: [49, 47]
                //},
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
                categories: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
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



        ///////////////////////////////////Scatter 




        ///////////////////////////////////Scatter 
        ////////////////////////////bar
        //////////////////////////////////// pie
        function createChart2() {
            $("#chart2").kendoChart({
                title: {
                    position: "bottom",
                    text: " YEAR WISE COE COUNT PERCENTAGE REPORT"
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
                        template: "#= category #: \n #= value#"
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
                    format: "{0}"
                }
            });
        }

        $(document).ready(createChart2);
        $(document).bind("kendo:skinChange", createChart2);

        //two

    }
})();