(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmployeeTotalStrenghtController', EmployeeTotalStrenghtController)

    EmployeeTotalStrenghtController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function EmployeeTotalStrenghtController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

        

        $("#chart123").kendoChart({
            title: {
                text: "Employee New/Left Ratio Report"
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column"
            },
            series: [{
                name: "New",
                color: "#2874A6 ",
                data: [6, 3, 2, 1]
            }, {
                    name: "Left",
                    color: "#F3A00E",
                data: [3, 1, 1, 0]
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
                categories: ["HIGH SCHOOL", "MIDDLE SCHOOL", "PRIMARY SCHOOL", "PE DEPT	"],
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

        //////pie

  
        $("#pie1").kendoChart({
                title: {
                    position: "bottom",
                    text: "Employee Newly Joined  Report"
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
                        category: "HIGH SCHOOL",
                        value: 6,
                        color: "#2874A6 ",
                    }, {
                            category: "MIDDLE SCHOOL",
                        value: 3,
                            color: "#F3A00E",
                    },  {
                            category: "PRIMARY SCHOOL",
                        value: 2,
                            color: "#979A9A",
                    }, {
                            category: "PE DEPT",
                        value: 1,
                            color: "#1D8348",
                        },
                      
                    ]
                }],
                tooltip: {
                    visible: true,
                    format: "{0}"
                }
            });
      
        
        $("#pie2").kendoChart({
            title: {
                position: "bottom",
                text: "Employee Left  Report"
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
                    category: "HIGH SCHOOL",
                    value: 3,
                    color: "#2874A6 ",
                }, {
                    category: "MIDDLE SCHOOL",
                    value: 1,
                        color: "#F3A00E",
                }, {
                    category: "PRIMARY SCHOOL",
                    value: 1,
                        color: "#979A9A",
                }, {
                    category: "PE DEPT",
                    value: 0,
                        color: "#1D8348",
                },
               
                ]
            }],
            tooltip: {
                visible: true,
                format: "{0}"
            }
        });





       
            $("#line").kendoChart({
                title: {
                    text: " Employee New / Left Ratio Report"
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
                    name: "New",
                    //data: [3.907, 7.943, 7.848, 9.284, 9.263, 9.801, 3.890, 8.238, 9.552, 6.855]
                    data: [6,3,2,1]
                }, {
                        name: "Left",
                       // data: [1.988, 2.733, 3.994, 3.464, 4.001, 3.939, 1.333, -2.245, 4.339, 2.727]
                        data: [3, 1, 1, 0]
                }, ],
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
                   /// categories: [2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011],
                    categories: ["HIGH SCHOOL", "MIDDLE SCHOOL", "MIDDLE AND HIGH SCHOOL", "PRIMARY SCHOOL", "PE DEPT", "NCC", ],
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