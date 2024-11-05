(function () {
    'use strict';
    angular
        .module('app')
        .controller('StudentPerFeeDeatilsController', StudentPerFeeDeatilsController)

    StudentPerFeeDeatilsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function StudentPerFeeDeatilsController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {


   
        //////////////"column
        $("#chart123").kendoChart({
            title: {
                text: "Student Fee Deatils "
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column"
            },
            series: [ 
                {
                    name: "2021-2022",
                    color: "#2874A6 ",
                    data: [10000, 5000,]
                }, 
                {
                    name: "2020-2021",
                    color: "#F3A00E",
                    data: [5000, 8000,]
                }, 
                {
                    name: "2019-2020",
                    color: "#5D6D7E",
                    data: [2000,0]
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
                categories: ["Concession", "Opening Balance",],
                line: {
                    visible: false
                },
                labels: {
                    padding: { top: 35 }
                }
            },
            tooltip: {
                visible: true,
                format: "{0}",
                template: "#= series.name #: #= value #"
            }
        });

       
        ///////////////////////line
        $("#line").kendoChart({
            title: {
                text: "Student Fee Deatils"
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
            series: [
                {
                    name: "2021-2022",
                    color: "#2874A6 ",
                    data: [10000, 5000,]
                },
                {
                    name: "2020-2021",
                    color: "#F3A00E",
                    data: [5000, 8000,]
                },
                {
                    name: "2019-2020",
                    color: "#5D6D7E",
                    data: [2000, 0]
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

                categories: ["Concession", "Opening Balance",],
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

       


    };
})();