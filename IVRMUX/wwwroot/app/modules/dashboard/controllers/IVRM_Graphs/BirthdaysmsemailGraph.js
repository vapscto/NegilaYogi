(function () {
    'use strict';

    angular
        .module('app')
        .controller('BirthdaysmsemailGraph', BirthdaysmsemailGraph);

    BirthdaysmsemailGraph.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']

    function BirthdaysmsemailGraph($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {



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
                text: "Month wise Student B'day Email and SMS Count"
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column"
            },
            series: [{
                name: "Email Count",
                color: "#2874A6 ",
                data: [44, 40, 72, 89, 20, 100, 74, 86, 62, 43, 44, 56],
            },
            {
                name: "SMS Count",
                color: "#F3A00E",
                data: [68, 56, 68, 49, 35, 45, 34, 43, 64, 47, 49, 47],
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
                format: "{0}%",
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
                    text: "Month wise Student B'day Email and SMS Count"
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
                    name: "Email Count",
                    data: [44, 68, 72, 68, 20, 35, 74, 34, 62, 64, 44, 49]
                }, {
                    name: "SMS Count",
                    data: [40, 56, 89, 49, 100, 40, 86, 43, 43, 47, 56, 47]
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
                    categories: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
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