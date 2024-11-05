(function () {
    'use strict';

    angular
        .module('app')
        .controller('SubjectWiseGraphG', SubjectWiseGraphG);

    SubjectWiseGraphG.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']

    function SubjectWiseGraphG($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {



        $("#chart123").kendoChart({
            title: {
                text: "Year Wise Student Performance"
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column"
            },
            series: [{
                name: "Student Marks",
                color: "#2874A6 ",
                data: [60, 56, 60, 54, 52, 58],
            },
            {
                name: "Class Topper Marks",
                color: "#FFD700",
                data: [40, 56, 89, 49, 100, 40],
            },
            {
                name: "Section Topper Marks",
                color: "#BDB76B",
                data: [27, 36, 50, 36, 52, 48],
            },
            //Section Topper
            {
                name: "Class Average Marks",
                color: "#F3A00E",
                data: [63, 78, 12, 64, 92, 38],
            },
            {
                name: "Section Average Marks",
                color: "#008080",
                data: [54, 56, 60, 54, 52, 58],
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
                categories: ["V (2016-2017)", "VI (2017-2018)", "VII (2018-2019)", "VIII (2019-2020)", "IX (2020-2021)", "X (2021-2022)"],
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



        ///////////////////////////////////Scatter 

        function createChart1() {
            $("#chart1").kendoChart({
                title: {
                    text: "YEAR WISE STUDENT PERFORMANCE"
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
                    name: "Student Marks",
                    color: "#2874A6",
                    data: [60, 56, 60, 54, 52, 58]
                }, {
                        name: "Class Topper Marks",
                        color: "#FFD700",
                    data: [40, 56, 89, 49, 100, 40]
                },
                {
                    name: "Section Topper Marks",
                    color:"#BDB76B",
                    data: [27, 36, 50, 36, 52, 48]
                    },
                    {
                        name: "Class Average Marks",
                        color: "#F3A00E",
                        data: [63, 78, 12, 64, 92, 38]
                    },
                    {
                        name: "section Average Marks",
                        color: "#008080",
                        data: [54, 56, 60, 54, 52, 58]
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
                    categories: ["V (2016-2017)", "VI (2017-2018)", "VII (2018-2019)", "VIII (2019-2020)", "IX (2020-2021)", "X (2021-2022)"],
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

        $(document).ready(createChart1);
        $(document).bind("kendo:skinChange", createChart1);

    }
})();