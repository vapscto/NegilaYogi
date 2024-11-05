(function () {
    'use strict'; angular.module('app')
        .controller('ClassWiseFeeDefaulterReportController', ClassWiseFeeDefaulterReportController)
    ClassWiseFeeDefaulterReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function ClassWiseFeeDefaulterReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {



        $("#chart123").kendoChart({
            title: {
                text: "CLASS WISE FEE DEFAULTER"
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
                name: "VI",
                data: [45000]
            }, {
                name: "VII",
                    data: [12000]
            }, {
                name: "VIII",
                    data: [25000]
            }, {
                name: "IX",
                    data: [58000]
            }, {
                name: "X",
                    data: [88000]
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
                categories: ["2021-22"],
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
                    text: "Class Wise Fee Defaulter Report, 2021-2022"
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
                        category: "X",
                        value: 88000,
                        color: "#f56942"
                    }, {
                            category: "IX",
                            value: 12000,
                            color: "#f5ec42"
                    }, {
                            category: "VIII ",
                            value: 25000,
                            color: "#15b007"
                    }, {
                            category: "VII",
                            value: 58000,
                            color: "#ec42f5"
                    }, {
                            category: "VI",
                            value: 88000,
                            color: "#42f5e0"
                    }]
                }],
                tooltip: {
                    visible: true,
                    format: "{0}"
                }
            });
        }

        $(document).ready(createChart2);
        $(document).bind("kendo:skinChange", createChart2);


        // Get form Details at onload 
        $scope.onLoadGetData = function () {

        }




    }
})();



