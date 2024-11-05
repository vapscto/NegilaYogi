//////////////"column

(function () {
    'use strict';
    angular
        .module('app')
        .controller('YearwisegraphReportController', YearwisegraphReportController)

    YearwisegraphReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', '$compile']
    function YearwisegraphReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, $compile) {
        $("#chart123").kendoChart({
            title: {
                text: "Year Wise Admission Count"
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column"
            },
            series: [{
                name: "PREKG",
                data: [48, 45, 55, 51],
                color: "#c6d9ed"
            }, {
                name: "LKG",
                data: [80, 75, 95, 82],
                    color: "#3770ad"
            }, {
                name: "UKG",
                data: [93, 77, 85, 73],
                    color: "#0c437e"
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
                categories: ["2021-2022", "2020-2021", "2019-2020", "2018-2019"],
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
       

        //////////////////////////////////// pie
        function createChart2() {
            $("#chart2").kendoChart({
                title: {
                    position: "top",
                    text: "YearWise Overall Admission Percentage"
                
                },
                legend: {
                    visible: false
                },
                chartArea: {
                    background: "transparent"
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
                        category: "2021-2022",
                        value: 25.57,
                        color: "#e1f15d"
                    }, {
                        category: "2020-2021",
                        value: 22.93,
                            color: "#72b954"
                    }, {
                        category: "2019-2020",
                        value: 26.25,
                            color: "#2f85cf"
                    }, {
                        category: "2018-2019",
                        value: 23.01,
                            color: "#562323"
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
})();