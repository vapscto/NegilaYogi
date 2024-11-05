(function () {
    'use strict';

    angular
        .module('app')
        .controller('subjectwisePerformanceGraph', subjectwisePerformanceGraph);

    subjectwisePerformanceGraph.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']

    function subjectwisePerformanceGraph($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {



        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        //////////////"column
        var February = "January"; var February = "February";
        // var chat = "2019-2020"; var chartwo = "2020-2021"; 
        $("#chart123").kendoChart({
            title: {
                text: "Exam Wise Student Performance 2021-2022"
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
                data: [36, 40, 72, 6],
            },
            {
                name: "Class Topper  Marks",
                color: "#FFD700",
                data: [98, 98, 68, 37],
            },
            {
                name: "Section Topper Marks",
                color: "#BDB76B",
                data: [75, 54, 98, 49],
            },
            //Section Topper
            {
                name: "Class Average Marks",
                color: "#F3A00E",
                data: [56, 86, 81, 49],
            },
            {
                name: "Section Average Marks",
                color: "#008080",
                data: [54, 46, 68, 46],
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
                categories: ["First Unit Test", "First Term Exam", "Mid Term Exam", "Final Exam"],
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

        $scope.flag = false;
        $scope.previous_document = function () {
            $scope.flag = true;
        }
        $scope.canceltab = function () {
            $scope.flag = false;
        }
        function createChart1() {
            $("#chart1").kendoChart({
                title: {
                    text: "EXAM WISE STUDENT PERFORMANCE"
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
                    data: [60, 56, 60, 54]
                }, {
                        name: "Class Topper Marks",
                    color: "#FFD700",
                    data: [40, 56, 89, 49]
                },
                {
                    name: "Section Topper  Marks",
                    color: "#BDB76B",
                    data: [27, 36, 50, 36]
                },
                {
                    name: "Class Average Marks",
                    color: "#F3A00E",
                    data: [63, 78, 12, 64]
                },
                {
                    name: "section Average Marks",
                    color: "#008080",
                    data: [34, 16, 70, 23]
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
                    categories: ["First Unit Test", "First Term Exam", "Mid Term Exam", "Final Exam"],
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

        $(document).ready(createChart1);
        $(document).bind("kendo:skinChange", createChart1);





        ///////////////////////////////////Scatter 

    }
})();