(function () {
    'use strict';
    angular
        .module('app')
        .controller('ExamWiseStudentPerformanceController', ExamWiseStudentPerformanceController)

    ExamWiseStudentPerformanceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function ExamWiseStudentPerformanceController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {



        //////////////"column
        $("#chart123").kendoChart({
            title: {
                text: "Subject Wise Student Performance"
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column"
            },
            series: [{
                name: "Student",
                color: "#2874A6 ",
                data: [56,62,58,60,42,50,]
            }, 
                {
                    name: "Class Topper",
                    color: "#F3A00E",
                    data: [100,100,	90,	98,	100,96,	]
                }, 
                {
                    name: "Section Topper",
                    color: "#5D6D7E",
                    data: [98,94,90,94,98,90	]
                }, 
                {
                    name: "Class Average",
                    color: "#979A9A",
                    data: [66.49,63.8,54.94,57.84,58.3,55.26]
                }, 
                {
                    name: "Section Average",
                    color: "#1D8348",
                    data: [83, 86, 78, 88, 45, 82]
                }, 
            ],
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
                categories: ["Computer Science", "English Literature", "General Science	", "History/civics", "Kannada Ii Lang", "Social Studies",],
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


        //////////////////////////////////// pie
       
            $("#pie").kendoChart({
                title: {
                    position: "bottom",
                    text: "Subject Wise Student Performance"
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
                        category: "Computer Science",
                        value: 19.8,
                        color: "#6C3483"
                    }, {
                        category: "English Literature",
                        value: 19.1,
                        color: "#1F618D"
                    }, {
                        category: "General Science",
                        value: 16.3,
                        color: "#148F77"
                    }, {
                        category: "History/civics",
                        value: 16.6,
                        color: "#239B56"
                    }, {
                        category: "Kannada II Lang",
                        value: 14.2,
                        color: "#B7950B"
                    }, {
                            category: "Lang Social Studies",
                        value: 16.6,
                        color: "#AF601A"
                    }]
                }],
                tooltip: {
                    visible: true,
                    format: "{0}%"
                }
            });
        

        ///////////////////////line
        $("#line").kendoChart({
            title: {
                text: " Subject Wise Student Performance"
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
                name: "Student",
                color: "#2874A6 ",
                data: [56, 62, 58, 60, 42, 50,]
            },
            {
                name: "Class Topper",
                color: "#F3A00E",
                data: [100, 100, 90, 98, 100, 96,]
            },
            {
                name: "Section Topper",
                color: "#5D6D7E",
                data: [98, 94, 90, 94, 98, 90]
            },
            {
                name: "Class Average",
                color: "#979A9A",
                data: [66.49, 63.8, 54.94, 57.84, 58.3, 55.26]
            },
            {
                name: "Section Average",
                color: "#1D8348",
                data: [83,	86,	78,	88,	45,	82]
               
            },
            ],
            valueAxis: {
                labels: {
                    format: "{0}%"
                },
                line: {
                    visible: false
                },
                axisCrossingValue: -10
            },
            categoryAxis: {

                categories: ["Computer Science", "English Literature", "General Science	", "History/civics", "Kannada Ii Lang", "Social Studies",],
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