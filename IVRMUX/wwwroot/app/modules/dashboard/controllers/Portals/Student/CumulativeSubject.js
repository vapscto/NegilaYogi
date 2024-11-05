(function () {
    'use strict';
    angular
.module('app')
.controller('CumulativeSubjectController', CumulativeSubjectController)

    CumulativeSubjectController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function CumulativeSubjectController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {


        CanvasJS.addColorSet("graphcolor",
                       [//colorSet Array
                       "#3498DB",
                       "#76D7C4",
                       "#808B96",
                       "#80DEEA",
                       "#C5E1A5",
                       "#AAB7B8"
                       ]);


        $scope.hide_examg = false;
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            
            apiService.getDATA("CumulativeSubject/getloaddata").
                then(function (promise) {
                    
                    $scope.stuyearlst = promise.stuyearlist;
                    $scope.className = promise.stuyearlist[0].asmcL_ClassName;
                    $scope.sectionName = promise.stuyearlist[0].asmC_SectionName;
                    $scope.asmcl_Id = promise.stuyearlist[0].asmcL_id;
                    $scope.asms_Id = promise.stuyearlist[0].asmS_id;
                    $scope.ismS_Id = 0;
                })
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //-----------academicyear Selection
        $scope.onyearchange = function (asmaY_Id) {
            
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
            }
            apiService.create("CumulativeSubject/getSubjectsdata", data).
               then(function (promise) {
                   
                   $scope.subjectlst = "";
                   $scope.ismS_Id = 0;
                   if (promise.stuyearlist != null) {
                       $scope.className = promise.stuyearlist[0].asmcL_ClassName;
                       $scope.sectionName = promise.stuyearlist[0].asmC_SectionName;
                       $scope.asmcl_Id = promise.stuyearlist[0].asmcL_id;
                       $scope.asms_Id = promise.stuyearlist[0].asmS_id;
                       $scope.subjectlst = promise.subjectlist;
                       if ($scope.subjectlst.length == "0") {
                           swal("No Record Found....")
                       }
                   }

               })
        }
        //-----------Subject Selection
        $scope.subjectchange = function () {
            
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ISMS_Id": $scope.ismS_Id,
            }
            apiService.create("CumulativeSubject/getexamdetails", data).
               then(function (promise) {
                   
                   $scope.examgradelst = promise.examgradelist;
                   $scope.hide_examg = true;
                   //--------------------Graph
                   $scope.ehead = ["Max Marks", "Obtained Marks", "Marks Percentage (%)"]
                   $scope.examgraph = [];
                   $scope.examgraphd = [];
                   if ($scope.examgradelst != null && $scope.examgradelst.length != "0") {
                       for (var a = 0; a < $scope.examgradelst.length; a++) {
                           $scope.examgraph.push({ label: $scope.examgradelst[a].emE_ExamName, "y": $scope.examgradelst[a].estmpS_MaxMarks })
                           $scope.examgraphd.push({ indexLabel: $scope.ehead[0], y: $scope.examgradelst[a].estmpS_MaxMarks, name: "Max Marks" + '-' + $scope.examgradelst[a].emE_ExamName });
                       }
                       $scope.examgraph1 = [];
                       for (var a = 0; a < $scope.examgradelst.length; a++) {
                           $scope.examgraph1.push({ label: $scope.examgradelst[a].emE_ExamName, "y": $scope.examgradelst[a].estmpS_ObtainedMarks })
                           $scope.examgraphd.push({ indexLabel: $scope.ehead[1], y: $scope.examgradelst[a].estmpS_ObtainedMarks, name: "Obtained Marks" + '-' + $scope.examgradelst[a].emE_ExamName });
                       }
                       $scope.examgraph2 = [];
                       for (var a = 0; a < $scope.examgradelst.length; a++) {
                           $scope.percentage = ($scope.examgradelst[a].estmpS_ObtainedMarks / $scope.examgradelst[a].estmpS_MaxMarks) * 100;
                           $scope.examgraph2.push({ label: $scope.examgradelst[a].emE_ExamName, "y": $scope.percentage })
                           $scope.examgraphd.push({ indexLabel: $scope.ehead[2], y: $scope.percentage, name: "Marks Percentage (%)" + '-' + $scope.examgradelst[a].emE_ExamName });
                       }
                   }
                   else {
                       swal("No Record Found....")
                       $scope.hide_examg = false;
                   }

                   var chart = new CanvasJS.Chart("columnchart", {
                       animationEnabled: true,
                       animationDuration: 3000,
                       height: 350, 
                      
                       colorSet: "graphcolor",



                       axisX: {
                           labelFontSize: 12,
                       },
                       axisY: {
                           labelFontSize: 12,
                       },

                       toolTip: {
                           shared: true
                       },
                       data: [{
                           name: "TOTAL MARKS",
                           showInLegend: true,
                           type: "column",
                           // color: "rgba(40,175,101,0.6)",
                           dataPoints: $scope.examgraph
                       },
                       {
                           name: "MARKS OBTAINED",
                           showInLegend: true,
                           type: "column",
                           //color: "rgba(0,75,141,0.7)",
                           dataPoints: $scope.examgraph1
                       },
                        {
                            name: "PERCENTAGE (%)",
                            showInLegend: true,
                            type: "column",
                            // color: "rgba(0, 212, 255, 0.43)",
                            dataPoints: $scope.examgraph2
                        }
                       ]
                   });
                   chart.render();

                   var chart = new CanvasJS.Chart("linechart", {
                       animationEnabled: true,
                       animationDuration: 3000,
                       height: 350,
                       colorSet: "graphcolor",


                       axisX: {
                           interval: 1,
                           labelFontSize: 12,
                       },
                       axisY: {
                           labelFontSize: 12,
                       },

                       data: [
                            {
                                type: "doughnut",
                                innerRadius: 90,
                                showInLegend: true,
                                dataPoints: $scope.examgraphd
                            }
                       ]

                   });
                   chart.render();
                   function explodePie(e) {
                       if (typeof (e.dataSeries.dataPoints[e.dataPointIndex].exploded) === "undefined" || !e.dataSeries.dataPoints[e.dataPointIndex].exploded) {
                           e.dataSeries.dataPoints[e.dataPointIndex].exploded = true;
                       } else {
                           e.dataSeries.dataPoints[e.dataPointIndex].exploded = false;
                       }
                       e.chart.render();
                   }

                   //var chart = new CanvasJS.Chart("linechart", {
                   //    animationEnabled: true,
                   //    animationDuration: 3000,

                   //    axisX: {
                   //        labelFontSize: 12,
                   //    },
                   //    axisY: {
                   //        labelFontSize: 12,
                   //    },

                   //    toolTip: {
                   //        shared: true
                   //    },
                   //    data: [{
                   //        name: "TOTAL MARKS",
                   //        showInLegend: true,
                   //        type: "area",
                   //        color: "rgba(40,175,101,0.6)",
                   //        dataPoints: $scope.examgraph
                   //    },
                   //    {
                   //        name: "MARKS OBTAIN",
                   //        showInLegend: true,
                   //        type: "area",
                   //        color: "rgba(0,75,141,0.7)",
                   //        dataPoints: $scope.examgraph1
                   //    },
                   //    {
                   //        name: "PERCENTAGE (%)",
                   //        showInLegend: true,
                   //        type: "area",
                   //        color: "rgba(0, 212, 255, 0.43)",
                   //        dataPoints: $scope.examgraph2
                   //    }
                   //    ]
                   //});
                   //chart.render();
               })
        }
    };
})();