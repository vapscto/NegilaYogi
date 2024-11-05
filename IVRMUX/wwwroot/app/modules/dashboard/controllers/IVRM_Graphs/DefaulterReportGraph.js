(function () {    'use strict';    angular        .module('app')        .controller('DateWiseFeeCollectionGraph', DateWiseFeeCollectionGraph);    DateWiseFeeCollectionGraph.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache',]    function DateWiseFeeCollectionGraph($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, ) {        $scope.yearmodel = false;$scope.showmodel=false;        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var institutionid = configsettings[0].mI_Id;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));				$scope.asmaY_Id='2021-2022';			$scope.asmaY_Iddd ='2021-2022';        $scope.ShowReport = function () {$scope.showmodel=true;
            $scope.yearmodel = true;
            $scope.asmaY_Iddd = $scope.asmaY_Id;
        }        //function createChart1() {

        //    $("#chart1").kendoChart({
        //        title: {
        //            text: "Student Wise Graph"
        //        },
        //        legend: {
        //            position: "top"
        //        },
        //        seriesDefaults: {
        //            type: "column"
        //        },
        //        series: [{
        //            name: "To Be Paid",
        //            data: [3000, 3001, 3002, 3003]
        //        }, {
        //            name: "Paid",
        //            data: [10000, 10001, 10002, 10003]

        //        }, {
        //            name: "Pending",
        //            data: [0, 0, 0, 0]
        //        }],
        //        valueAxis: {
        //            labels: {
        //                format: "{0}%"
        //            },
        //            line: {
        //                visible: false
        //            },
        //            axisCrossingValue: 0
        //        },
        //        categoryAxis: {
        //            categories: ["Exam Fee", "Admission Fee", "Registration Fee", "Regular Fee"],
        //            line: {
        //                visible: false
        //            },
        //            labels: {
        //                padding: { top: 35 }
        //            }
        //        },
        //        tooltip: {
        //            visible: true,
        //            format: "{0}%",
        //            template: "#= series.name #: #= value #"
        //        }
        //    });



        //}

        //$(document).ready(createChart1);
        //$(document).bind("kendo:skinChange", createChart1);

        $("#area").kendoChart({
            title: {
                text: "Fee Defaulter"
            },
            legend: {
                position: "bottom"
            },
            seriesDefaults: {
                type: "area",
                area: {
                    line: {
                        style: "smooth"
                    }
                }
            },
            series: [{
                name: "Total Strength",
               
                data: [50, 20, 30, 48, 28, 47, 20, 50, 20, 30, 48, 28, 47, 20, 50, 20],
                color: "#2269cb",
              
            }, {
               name: "Total Paid Count",

                data: [40, 20, 15, 48, 28, 40, 20, 15, 48, 28, 40, 20, 15, 48, 28, 40],
                    color: "#d3752e",
            }, {
               name: "Before Due Date Count",
              
                data: [25, 6, 13, 48, 18, 25, 6, 13, 48, 18, 25, 6, 13, 48, 18, 25],
                    color: "#b5b5b1",
            }, {
                name: "After Due Date Count",
               
                data: [15, 14, 2, 0, 10, 15, 14, 2, 0, 10, 15, 14, 2, 0, 10, 15],
                    color: "#f3cc0c",
            },			{				name:"Pending",				data:[10,0,15,0,0,7,0,35,0,2,8,8,32,0,22,0],			}			],
            valueAxis: {
                labels: {
                    format: "{0}"
                },
                line: {
                    visible: false
                },
                axisCrossingValue: -10
            },
            categoryAxis: [
                {
                    categories: ["A", "B", "C", "D", "A", "B", "C", "D", "A", "B", "C", "D", "A", "B", "C", "D"],
                majorGridLines: {
                    visible: false
                },
                labels: {
                    rotation: "auto"
                }
                },
                {
                    categories: ["I", "II", "III", "IV"],
                    majorGridLines: {
                        visible: false
                    },
                    labels: {
                        rotation: "auto"
                    }
                },
                {
                    categories: ["2021-2022"],
                    majorGridLines: {
                        visible: false
                    },
                    labels: {
                        rotation: "auto"
                    }
                }
            ],
            tooltip: {
                visible: true,
                format: "{0}",
                template: "#= series.name #: #= value #"
            }
        });


        //$(document).ready(createChart);
        //$(document).bind("kendo:skinChange", createChart);



     
            $("#chart1").kendoChart({
                title: {
                    text: "Defaulter Graph"
                },
                legend: {
                    position: "bottom"
                },
                seriesDefaults: {
                    type: "column",
                    area: {
                        line: {
                            style: "smooth"
                        }
                    }
                },
                series: [{
                    name: "Total Strength",

                    data: [50, 20, 30, 48, 28, 47, 20, 50, 20, 30, 48, 28, 47, 20, 50, 20],
                    color:"#2269cb",

                }, {
                    name: "Total Paid Count",

                    data: [40, 20, 15, 48, 28, 40, 20, 15, 48, 28, 40, 20, 15, 48, 28, 40],
                        color:"#d3752e",
                }, {
                    name: "Before Due Date Count",

                    data: [25, 6, 13, 48, 18, 25, 6, 13, 48, 18, 25, 6, 13, 48, 18, 25],
                        color:"#b5b5b1",
                }, {
                    name: "After Due Date Count",

                        data: [15, 14, 2, 0, 10, 15, 14, 2, 0, 10, 15, 14, 2, 0, 10, 15],
                        color:"#f3cc0c",

                },				{				name:"Pending",				data:[10,0,15,0,0,7,0,35,0,2,8,8,32,0,22,0],			}],
                valueAxis: {
                    labels: {
                        format: "{0}"
                    },
                    line: {
                        visible: false
                    },
                    axisCrossingValue: -10
                },
                categoryAxis: [
                    {
                        categories: ["A", "B", "C", "D", "A", "B", "C", "D", "A", "B", "C", "D", "A", "B", "C", "D"],
                        majorGridLines: {
                            visible: false
                        },
                        labels: {
                            rotation: "auto"
                        }
                    },
                    {
                        categories: ["I	", "II", "III", "IV"],
                        majorGridLines: {
                            visible: false
                        },
                        labels: {
                            rotation: "auto"
                        }
                    },
                    {
                        categories: ["2021-2022"],
                        majorGridLines: {
                            visible: false
                        },
                        labels: {
                            rotation: "auto"
                        }
                    }
                ],
                tooltip: {
                    visible: true,
                    format: "{0}",
                    template: "#= series.name #: #= value #"
                }
            });

        
        //$(document).ready(createChart1);
        //$(document).bind("kendo:skinChange", createChart1);

        //$("#chart1").kendoChart({
        //    "chartArea": {
        //        "height": 500
        //    },
        //    "title": {
        //        "text": ""
        //    },
        //    "legend": {
        //        "labels": {
        //            "template": "#= series.name #"
        //        },
        //        "position": "top"
        //    },
        //    "series": [{
        //        "name": "Total Strength",
        //        "type": "column",
        //        "data": [50, 20, 30, 48, 28, 47, 20, 50, 20, 30, 48, 28, 47, 20, 50, 20],
             

        //        "stack": false
        //    }, {
        //        "name": "Total Paid Count",
        //        "type": "column",
        //            "data": [40,20,15,48,28,40,20,15,48,28,40,20,15,48,28,40],
        //        "stack": false
        //    }, {
        //        "name": "Before Due Date Count",
        //        "type": "column",
        //            "data": [25,6,13,48,18,25,6,13,48,18,25,6,13,48,18,25],
        //        "stack": false
        //    }, {
        //        "name": "After Due Date Count",
        //        "type": "column",
        //            "data": [15, 14, 2, 0, 10, 15, 14, 2, 0, 10, 15, 14, 2, 0, 10, 15],
        //            "stack": false
        //    }],
        //    "categoryAxis": [{
        //        "labels": {
        //            "rotation": {
        //                "angle": "auto"
        //            }
        //        },
        //        "majorGridLines": {
        //            "visible": false
        //        },
        //        "title": {
        //            // "text": "Sectors",
        //            "position": "left"
        //        },
        //        "categories": ["A", "B", "C", "D", "A", "B", "C", "D", "A", "B", "C", "D", "A", "B", "C", "D"]
        //    }, {
        //        "labels": {
        //            "rotation": {
        //                "angle": "auto"
        //            }
        //        },
        //        "majorGridLines": {
        //            "visible": true
        //        },
        //        "line": {
        //            "visible": true
        //        },
        //        "title": {
                 
        //            "position": "left"
        //        },
        //            "categories": ["Class 1	", "Class 2", "Class 3", "Class 4"]

        //    },
        //    {
        //        "labels": {
        //            "rotation": {
        //                "angle": "auto"
        //            }
        //        },
        //        "majorGridLines": {
        //            "visible": true
        //        },
        //        "line": {
        //            "visible": true
        //        },
        //        "title": {
        //            //"text": "Year",
        //            "position": "left"
        //        },
        //        "categories": ["2021-2022"]

        //    }
        //    ],
        //    "valueAxis": [{
        //        "majorGridLines": {
        //            "visible": false
        //        }
        //    }],
        //    "tooltip": {
        //        "format": "{0}%",
        //        "template": "#= series.name #: #= value #",
        //        "visible": true
        //    },
        //    "pannable": {
        //        "lock": "y"
        //    },
        //    "zoomable": {
        //        "mousewheel": {
        //            "lock": "y"
        //        },
        //        "selection": {
        //            "lock": "y"
        //        }
        //    }
        //});
        //$(document).ready(createChart);
        //$(document).bind("kendo:skinChange", createChart);

    }})();