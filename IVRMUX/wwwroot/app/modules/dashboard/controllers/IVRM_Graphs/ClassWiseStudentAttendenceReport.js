(function () {
    'use strict'; angular.module('app')
        .controller('ClassWiseStudentAttendenceReportController', ClassWiseStudentAttendenceReportController)
    ClassWiseStudentAttendenceReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function ClassWiseStudentAttendenceReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {

        

        $("#chart123").kendoChart({
            title: {
                text: "CLASS WISE STUDENT ATTENDANCE"
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
                data: [75, 78, 82]
            }, {
                name: "VII",
                data: [47, 82, 59]
            }, {
                name: "VIII",
                data: [74, 64, 76]
            }, {
                name: "IX",
                data: [92, 89, 66]
            }, {
                name: "X",
                data: [67, 79, 54]
            },],
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
                // categories: [Software, Accounts, ManagementAdmin, Admin, HumanResourse, HumanResourse],
                categories: ["2019-20", "2020-21", "2021-22"],
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
                format: "{0}%",
                template: "#= series.name #: #= value #"
            }
        });


        function createChart1() {
            $("#chart1").kendoChart({
                title: {
                    text: "CLASS WISE STUDENT ATTENDANCE"
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
                    color: "#2874A6 ",
                    color: "#F3A00E",
                    color: "#5D6D7E",
                    color: "#979A9A",
                    color: "#7B241C",
                    name: "VI",
                    data: [75, 78, 82]
                }, {
                    name: "VII",
                    data: [47, 82, 59]
                }, {
                    name: "VIII",
                    data: [74, 64, 76]
                }, {
                    name: "IX",
                    data: [92, 89, 66]
                }, {
                    name: "X",
                    data: [67, 79, 54]
                },],
                valueAxis: {
                    labels: {
                        format: "{0}%"
                    },
                    line: {
                        visible: false
                    },
                    axisCrossingValue: -20
                },
                categoryAxis: {
                    categories: ["2019-20", "2020-21", "2021-22"],
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


        // Get form Details at onload 
        $scope.onLoadGetData = function () {

        }




    }
})();



