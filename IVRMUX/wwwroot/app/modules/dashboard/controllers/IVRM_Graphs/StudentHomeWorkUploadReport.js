(function () {
    'use strict'; angular.module('app')
        .controller('StudentHomeWorkUploadReportController', StudentHomeWorkUploadReportController)
    StudentHomeWorkUploadReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function StudentHomeWorkUploadReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {



        $("#chart123").kendoChart({
            title: {
                text: "Student Home Work Upload Report"
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column"
            },
            series: [{
                color: "#2874A6 ",
                color: "#F3A00E",
                color: "#5D6D7E",
                color: "#979A9A",
                color: "#7B241C",
                name: "Upload",
                data: [46, 52, 49, 39, 41]
            }, {
                name: "Not Uploaded",
                    data: [14, 8, 11, 21, 19]
            },],
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
                // categories: [Software, Accounts, ManagementAdmin, Admin, HumanResourse, HumanResourse],
                categories: ["VI", "VII", "VIII", "IX","X"],
                // categories: ids1,
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






        function createChart2() {
            $("#chart2").kendoChart({
                title: {
                    position: "bottom",
                    text: "Student Home Work Upload Report"
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
                        category: "X",
                        value: 41,
                        color: "#f56942"
                    }, {
                        category: "IX",
                            value: 39,
                        color: "#f5ec42"
                    }, {
                        category: "VIII ",
                            value: 49,
                        color: "#15b007"
                    }, {
                        category: "VII",
                            value: 52,
                        color: "#ec42f5"
                    }, {
                        category: "VI",
                            value: 46,
                        color: "#42f5e0"
                    }]
                }],
                tooltip: {
                    visible: true,
                    format: "{0}"
                }
            });
        }




            $("#chart3").kendoChart({
                title: {
                    position: "bottom",
                    text: "Student Home Work Not Uploaded Report"
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
                        category: "X",
                        value: 19,
                        color: "#f56942"
                    }, {
                        category: "IX",
                            value: 21,
                        color: "#f5ec42"
                    }, {
                        category: "VIII ",
                            value: 11,
                        color: "#15b007"
                    }, {
                        category: "VII",
                            value: 8,
                        color: "#ec42f5"
                    }, {
                        category: "VI",
                            value: 14,
                        color: "#42f5e0"
                    }]
                }],
                tooltip: {
                    visible: true,
                    format: "{0}"
                }
            });
   

        $(document).ready(createChart2);
        $(document).bind("kendo:skinChange", createChart2);


        // Get form Details at onload 
        $scope.onLoadGetData = function () {

        }




    }
})();



