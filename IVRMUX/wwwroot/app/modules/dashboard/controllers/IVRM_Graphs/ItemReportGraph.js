(function () {
    'use strict';

    angular
        .module('app')
        .controller('ItemReportGraph', ItemReportGraph);

    ItemReportGraph.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']

    function ItemReportGraph($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {



        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));


        $("#chart123").kendoChart({
            title: {
                text: "Item Report 2020-2021"
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column"
            },
            series: [{
                name: "Chairs",
                color: "#2874A6 ",
                data: [36],
            },
            {
                name: "Tables",
                color: "#FFD700",
                data: [98],
            },
            {
                name: "Desktop	",
                color: "#BDB76B",
                data: [75],
            },
            //Section Topper
            {
                name: "Printer",
                color: "#F3A00E",
                data: [56],
            },
            {
                name: "Bags",
                color: "#008080",
                data: [54],
            },
            {
                name: "Laptop",
                color: "#008080",
                data: [54],
            },
            {
                name: "Mouse",
                color: "#008080",
                data: [84],
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
                categories: ["2020-2021"],
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

        ////////////////////////////bar
        //////////////////////////////////// pie
        function createChart2() {
            $("#chart2").kendoChart({
                title: {
                    position: "bottom",
                    text: " Item Report 2020 - 2021 "
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
                        category: "Chairs",
                        value: 36,
                        color: "#2874A6 ",
                    }, {
                        category: "Tables",
                        value: 98,
                            color: "#FFD700" 
                    }, {
                        category: "Desktop",
                        value: 75,
                            color: "#F3A00E"
                    },
                    {
                        category: "Printer",
                        value: 56,
                        color: "#BDB76B"
                    },
                    {
                        category: "Books",
                        value: 56,
                        color: "#068c35"
                        },
                        {
                            category: "Bags",
                            value: 56,
                            color: "#008971"
                        },
                        {
                            category: "Laptop",
                            value: 56,
                            color: "#033080"
                        },
                        {
                            category: "Mouse",
                            value: 56,
                            color: "#F3A00E"
                        },
                    ]
                }],
                tooltip: {
                    visible: true,
                    format: "{0}"
                }
            });
        }

        $(document).ready(createChart2);
        $(document).bind("kendo:skinChange", createChart2);

        //two
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
                    format: "{0}%",
                    template: "#= series.name #: #= value #"
                }
            });
        }





        ///////////////////////////////////Scatter 

    }
})();