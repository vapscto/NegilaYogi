(function () {
    'use strict';
    angular
.module('app')
.controller('StudentPerformanceReportController', StudentPerformanceReportController)

    StudentPerformanceReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function StudentPerformanceReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.BindData = function () {
            apiService.getDATA("StudentPerformanceReport/getdetails").
            then(function (promise) {

                $scope.acdlist = promise.acdlist;
                $scope.catlist = promise.catlist;
                $scope.ctlist = promise.ctlist;
                $scope.seclist = promise.seclist;
                $scope.sublist = promise.sublist;
                $scope.examlist = promise.examlist;
                $scope.studentlist = promise.studentlist;
            })
        };

        $scope.onselectCategory = function (ASMAY_Id, EMCA_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id,
                "EMCA_Id": EMCA_Id
            }
            apiService.create("StudentPerformanceReport/onselectCategory", data).
            then(function (promise) {
                $scope.ctlist = promise.ctlist;

            })
        };

        $scope.onselectclass = function (ASMCL_Id, ASMAY_Id, EMCA_Id) {
            var data = {
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id,
                "EMCA_Id": EMCA_Id
            }
            apiService.create("StudentPerformanceReport/onselectclass", data).
            then(function (promise) {
                $scope.seclist = promise.seclist;
            })
        };

        $scope.onselectSection = function () {
            var data = {
                "ASMS_Id": $scope.ASMS_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("PercentagewiseDetailsReport/onselectSection", data).
            then(function (promise) {
                $scope.examlist = promise.examlist;
                $scope.studentlist = promise.studentlist;
            })
        };

        $scope.submitted = false;
        $scope.onshow = function () {
            $scope.submitted = true;
            //$scope.studgraph = true;
            
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "EMCA_Id": $scope.EMCA_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "ISMS_Id": $scope.ISMS_Id,
                "AMST_Id": $scope.AMST_Id,
                "EME_Id": $scope.EME_Id
            }
            apiService.create("StudentPerformanceReport/onshow", data).
               then(function (promise) {
                   
                   $scope.Main_list = promise.showgraph;
                   $scope.stud_graph = [];
                   if (promise.showgraph != "0" && promise.showgraph != null) {
                       $scope.studgraph = false;
                       for (var i = 0; i < $scope.Main_list.length; i++) {
                           $scope.stud_graph.push({ label: $scope.Main_list[i].estmpS_ObtainedMarks, "y": $scope.Main_list[i].estmpS_ObtainedMarks })
                       }

                       $scope.stud_graph2 = [];

                       for (var i = 0; i < $scope.Main_list.length; i++) {
                           $scope.stud_graph2.push({ label: $scope.Main_list[i].estmpS_SectionHighest, "y": parseInt($scope.Main_list[i].estmpS_SectionHighest) })
                       }

                       $scope.stud_graph1 = [];

                       for (var i = 0; i < $scope.Main_list.length; i++) {
                           $scope.stud_graph1.push({ label: $scope.Main_list[i].estmpS_ClassHighest, "y": parseInt($scope.Main_list[i].estmpS_ClassHighest) })
                       }

                       

                       var chart = new CanvasJS.Chart("linechart", {
                           animationEnabled: true,
                           animationDuration: 3000,
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
                               name: "STUDENT PERCENT",
                               showInLegend: true,
                               type: "column",
                               color: "red",
                               dataPoints: $scope.stud_graph
                           },
                           {
                               name: "SECTION TOPPER",
                               showInLegend: true,
                               type: "column",
                               color: "green",
                               dataPoints: $scope.stud_graph2
                           },
                           {
                               name: "CLASS TOPPER",
                               showInLegend: true,
                               type: "column",
                               color: "blue",
                               dataPoints: $scope.stud_graph1
                           }
                           ]
                       });
                       chart.render();                       
                   }
                   else {
                       swal("No Record Found....")
                   }

               })

        };

        $scope.cancel = function () {
            $scope.ASMAY_Id = '';
            $scope.ASMCL_Id = '';
            $scope.ASMS_Id = '';
            $scope.EMCA_Id = '';
            $scope.EME_Id = '';
            $scope.AMST_Id = '';
            $scope.ISMS_Id = '';
            $scope.Main_list = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }

})();