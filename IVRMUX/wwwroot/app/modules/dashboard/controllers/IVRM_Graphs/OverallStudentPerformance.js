(function () {
    'use strict';
    angular
        .module('app')
        .controller('OverallStudentPerformanceController', OverallStudentPerformanceController)

    OverallStudentPerformanceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function OverallStudentPerformanceController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {



        //////////////"column
        $("#chart123").kendoChart({
            title: {
                text: "Overall Student Performance"
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column"
            },
            series: [{
                name: "Total No of Students",
                color: "#2874A6 ",
                data: [70,60,]
            }, 
                {
                    name: "Passed students",
                    color: "#F3A00E",
                    data: [56,52,]
                }, 
                {
                    name: "Failed Students",
                    color: "#5D6D7E",
                    data: [14,8,	]
                }, 
                {
                    name: "Toppers Marks",
                    color: "#979A9A",
                    data: [95,96,]
                }, 
                {
                    name: "Section Average",
                    color: "#1D8348",
                    data: [85,79,]
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
            categoryAxis: [{
                categories: ["VIII-A", "VIII-B",],
                line: {
                    visible: false
                },
                labels: {
                    padding: { top: 35 }
                }
            },],
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
                    text: "Overall Student Performance"
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
                        category: "Total No of Students",
                        value: 130,
                        color: "#6C3483"
                    }, {
                            category: "Passed students",
                        value: 108,
                        color: "#1F618D"
                    }, {
                            category: "Failed Students",
                        value: 20,
                        color: "#148F77"
                    }, {
                            category: "Toppers Marks",
                        value: 96,
                        color: "#239B56"
                    }, {
                            category: "Section Average",
                        value: 85,
                        color: "#B7950B"
                    },]
                }],
                tooltip: {
                    visible: true,
                    format: "{0}"
                }
            });
        

        ///////////////////////line
        $("#line").kendoChart({
            title: {
                text: " Overall Student Performance"
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
                name: "Total No of Students",
                color: "#2874A6 ",
                data: [70, 60,]
            },
            {
                name: "Passed students",
                color: "#F3A00E",
                data: [56, 52,]
            },
            {
                name: "Failed Students",
                color: "#5D6D7E",
                data: [14, 8,]
            },
            {
                name: "Toppers Marks",
                color: "#979A9A",
                data: [95, 96,]
            },
            {
                name: "Section Average",
                color: "#1D8348",
                data: [85, 79,]
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

                   categories: ["VIII-A", "VIII-B",],
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

       




    };
})();