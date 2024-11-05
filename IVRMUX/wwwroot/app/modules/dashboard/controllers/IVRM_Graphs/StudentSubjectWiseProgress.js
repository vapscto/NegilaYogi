(function () {
    'use strict';
    angular
        .module('app')
        .controller('StudentSubjectWiseProgressController', StudentSubjectWiseProgressController)

    StudentSubjectWiseProgressController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function StudentSubjectWiseProgressController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {


 //$scope.loaddata=function(){
  //////////////"column
        $("#chart123").kendoChart({
            title: {
                text: "  Subject Wise Student Progress"
            },
            legend: {
                position: "top"
            },
            seriesDefaults: {
                type: "column"
            },
            series: [{
                name: "First Term Assessment",
                color: "#2874A6 ",
                data: [94, 80, 95, 89, 99, 86,
                ]

            }, {
                name: "First Unit Assessment",
                color: "#F3A00E",
                data: [
                    83,
                    86,
                    78,
                    88,
                    45,
                    82,]
            }, {
                name: "Final Examination",
                color: "#5D6D7E",
                data: [91,
                    92,
                    89,
                    89,
                    88,
                    91,]
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
                categories: ["Computer Science", "English Literature", "General Science	", "History/Civics", "Kannada II Lang", "Social Studies",],
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



        //////////////////////////////////// pie
     
            $("#pie").kendoChart({
                title: {
                    position: "bottom",
                    text: " Subject Wise Student SSSSProgress"
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
                        category: "History / Civics",
                        value: 16.6,
                            color: "#239B56"
                    }, {
                        category: "Kannada II Lang",
                        value: 14.2,
                            color: "#B7950B"
                    }, {
                        category: "Social Studies",
                        value: 16.6,
                            color: "#AF601A"
                    }]
                }],
                tooltip: {
                    visible: true,
                    format: "{0}"
                }
            });
        
        ///////////////////////line
        $("#line").kendoChart({
            title: {
                text: "  Subject Wise Student Progress"
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
                name: "First Term Assessment",
                color: "#2874A6 ",
                data: [94, 80, 95, 89, 99, 86,
                ]

            }, {
                name: "First Unit Assessment",
                color: "#F3A00E",
                data: [
                    83,
                    86,
                    78,
                    88,
                    45,
                    82,]
            }, {
                name: "Final Examination",
                color: "#5D6D7E",
                data: [91,
                    92,
                    89,
                    89,
                    88,
                    91,]
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
               
                categories: ["Computer Science", "English Literature", "General Science	", "History/Civics", "Kannada II Lang", "Social Studies",],
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

   // };
}
      
})();