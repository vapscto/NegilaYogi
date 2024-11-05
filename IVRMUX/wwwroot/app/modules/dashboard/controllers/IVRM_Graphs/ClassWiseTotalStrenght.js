(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClassWiseTotalStrenghtController', ClassWiseTotalStrenghtController)

    ClassWiseTotalStrenghtController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function ClassWiseTotalStrenghtController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {


        //////////////"column
        $("#chart123").kendoChart({
            title: {
                text: "Class Wise Total Strenght"
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column"
            },
            series: [{
                name: "Toatl Section",
                color: "#2874A6 ",
                data: [3,
                    4,
                    4,
                    3,
                    5,
                    8,
                    3,
                    4,
                    2,
                    4,
                    4,
                    2,

                ]

            }, {
                name: "Total Class Strenght",
                color: "#F3A00E",
                data: [
                    70,
                    56,
                    84,
                    95,
                    108,
                    100,
                    85,
                    69,
                    78,
                    80,
                    95,
                    85,
                ]
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
                categories: [" Pre-nursery", " Nursery	", "I ", "II", "III", "IV", "V",  "VI", "VII", "VIII", "IX", "X"],
                line: {
                    visible: false
                },
                labels: {
                    padding: { top: 35 }
                }
            },
            tooltip: {
                visible: true,
                format: "{0}%",
                template: "#= series.name #: #= value #"
            }
        });

        //////////////////////////////////// pie

        $("#pie").kendoChart({
            title: {
                position: "bottom",
                text: "Class Wise Total Strenght"
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
                    category: "Pre-nursery",
                    value: 70,
                    color: "#123456"
                }, {
                        category: "Nursery ",
                    value: 56,
                    color: "#1F618D"
                }, {
                        category: "I ",
                    value: 84,
                    color: "#148F77"
                }, {
                        category: "II ",
                    value: 95,
                    color: "#239B56"
                }, {
                        category: "III ",
                    value: 108,
                        color: "#728FCE	"
                    },
                   
                    {
                        category: "IV ",
                        value: 85,
                        color: "#4863A0"
                    },
                    {
                        category: "V ",
                        value: 69,
                        color: "#98AFC7"
                    },
                    {
                        category: "VI ",
                        value: 78,
                        color: "#838996	"
                    },
                    {
                        category: "VII ",
                        value: 80,
                        color: "#778899	"
                    },
                    {
                        category: "VIII ",
                        value: 85,
                        color: "#708090"
                    },
                    {
                        category: "XI ",
                        value: 95,
                        color: "#6D7B8D"
                    },
                    {
                        category: "X",
                        value: 85,
                        color: "#36454F"
                    },
                ]
            }],
            tooltip: {
                visible: true,
                format: "{0}"
            }
        });


        $(document).ready(createChart2);
        $(document).bind("kendo:skinChange", createChart2);






    };
})();