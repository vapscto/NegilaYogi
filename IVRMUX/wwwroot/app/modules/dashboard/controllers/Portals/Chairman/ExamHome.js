
(function () {
    'use strict';
    angular
.module('app')
.controller('ExamHomeController', ExamHomeController)

    ExamHomeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function ExamHomeController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

        $scope.graph = false;
        $scope.graphgrd = false;

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.graphstudlist = [];

        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            apiService.getDATA("ExamHome/Getdetails").
       then(function (promise) {

           $scope.yearlt = promise.yearlist;

       })
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.OnAcdyear = function (asmaY_Id) {
            $scope.emcA_Id = '';
            $scope.emE_Id = '';

            $scope.asmcL_Id = '';
            $scope.fillcategory = [];
            $scope.classlist = [];
            $scope.exmstdlist = [];
            apiService.getURI("ExamHome/getcategory", asmaY_Id).
                         then(function (promise) {
                             if (promise.fillcategory.length > 0) {
                                 $scope.fillcategory = promise.fillcategory;

                             }
                             //else {
                             //    swal("No Record Found")
                             //}

                         });

        }

        $scope.onselectcategory = function (emcA_Id) {
            $scope.classlist = [];
            $scope.exmstdlist = [];
            $scope.emE_Id='';
                    
            $scope.asmcL_Id = '';
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "EMCA_Id": emcA_Id,
               
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("ExamHome/getclassexam", data).
                     then(function (promise) {

                         if (promise.classlist.length > 0) {
                             $scope.classlist = promise.classlist

                         }

                         //else {
                         //                 swal('No Class Found');
                         //}

                         if (promise.exmstdlist.length > 0) {
                             $scope.exmstdlist = promise.exmstdlist

                         }

                         //else {
                         //    swal('No Exam Found');
                         //}
                     })


        }
        $scope.clsname = '';
        // TO Save The Data
        $scope.submitted = false;
        $scope.showreport = function () {
            $scope.clsname = '';
            $scope.graph = false;
            $scope.graphgrd = false;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                
                var data = {
                    "EME_Id": $scope.emE_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "EMCA_Id": $scope.emcA_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("ExamHome/showreport", data).
                         then(function (promise) {

                             
                             $scope.studlist = promise.studlist;
                             if ($scope.studlist[0].total_Count != 0) {

                                 angular.forEach($scope.classlist,function (ff) {
                                     if ($scope.asmcL_Id == ff.asmcL_Id) {
                                         $scope.clsname = ff.asmcL_ClassName;
                                     }

                                 })



                                 $scope.graphstudlist = promise.graphstudlist;
                                 $scope.graph = true;
                                 $scope.graphgrd = true;

                                 $scope.loadchart();
                             }
                             else {
                                              swal('No record Found');
                                          }
                         })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.showstudentGrid = function () {
            var data = {
                "EME_Id": $scope.emE_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("ExamHome/showsectioncount", data).
                     then(function (promise) {

                         if (promise.seclist.length > 0) {

                             $scope.seclist = promise.seclist;
                             $scope.sectionlist = [];
                             if ($scope.seclist != null) {

                                 for (var i = 0; i < $scope.seclist.length; i++) {
                                     $scope.sectionlist.push({ sec: $scope.seclist[i].asmC_SectionName, total: $scope.seclist[i].pass_Count + $scope.seclist[i].fail_Count, pass: $scope.seclist[i].pass_Count, fail: $scope.seclist[i].fail_Count })
                                 }
                             }
                         }
                         else {
                             swal('No record Found');
                         }
                     })
        }


        $scope.cancel = function () {
            $scope.asmcL_Id = ""
            $scope.emcA_Id = ""
            $scope.asmaY_Id = ""
            $scope.emG_Id = ""
            $scope.asmS_Id = ""
            $scope.subjectlt = ""
            $scope.subjectlt1 = ""
            $scope.studentlist = false;
            $state.reload();
        }


        $scope.loadchart = function () {
            $scope.stdgraphseries1 = [];
            if ($scope.graphstudlist != null) {

                for (var i = 0; i < $scope.graphstudlist.length; i++) {
                    $scope.stdgraphseries1.push({ label: $scope.graphstudlist[i].asmcL_ClassName, "y": $scope.graphstudlist[i].pass_Count + $scope.graphstudlist[i].fail_Count })
                }
            }
            console.log($scope.stdgraphseries1);

            $scope.stdgraphseries2 = [];
            if ($scope.graphstudlist != null) {

                for (var i = 0; i < $scope.graphstudlist.length; i++) {
                    $scope.stdgraphseries2.push({ label: $scope.graphstudlist[i].asmcL_ClassName, "y": $scope.graphstudlist[i].pass_Count })
                }
            }
            console.log($scope.stdgraphseries2);

            $scope.stdgraphseries3 = [];
            if ($scope.graphstudlist != null) {

                for (var i = 0; i < $scope.graphstudlist.length; i++) {
                    $scope.stdgraphseries3.push({ label: $scope.graphstudlist[i].asmcL_ClassName, "y": $scope.graphstudlist[i].fail_Count })
                }
            }
            console.log($scope.stdgraphseries3);


            var chart = new CanvasJS.Chart("rangeBarChat");
           
            chart.options.axisX = { interval: 1, labelFontSize: 12 };
            chart.options.axisY = { labelFontSize: 12 };
            // chart.options.title = { text: "Fruits sold in First & Second Quarter" };

            var series1 = { //dataSeries - first quarter
                type: "column",
                name: "Total Student",
                showInLegend: true,
                color: "#778899"
            };



            var series2 = { //dataSeries - second quarter
                type: "column",
                name: "Pass",
                showInLegend: true,
                color: "#77D7EF"
            };

            var series3 = { //dataSeries - second quarter
                type: "column",
                name: "Fail",
                showInLegend: true,
                color: "#C77575"
            };
            chart.options.height = 350;

            chart.options.data = [];
            chart.options.data.push(series1);
            chart.options.data.push(series2);
            chart.options.data.push(series3);


            series1.dataPoints = $scope.stdgraphseries1;

            series2.dataPoints = $scope.stdgraphseries2;
            series3.dataPoints = $scope.stdgraphseries3;

            chart.render();

            var chart = new CanvasJS.Chart("chartContainer",
   {
       axisY: {
           suffix: " %",
           labelFontSize: 12
       },
       axisX: {
           labelFontSize: 11,
           height: 350,
           interval: 1,
           labelFontColor: "black",
       },
     height:350,
       data: [
       {
           type: "stackedColumn100",
           indexLabel: "#percent%",
           percentFormatString: "#0.##",
           toolTipContent: "{y} (#percent%)",
               color: "#77D7EF",
           name: "Pass",
           showInLegend: true,
           dataPoints: $scope.stdgraphseries2,
           indexLabelFontSize: 12,
       }, {
           type: "stackedColumn100",
           indexLabel: "#percent%",
           percentFormatString: "#0.##",
           toolTipContent: "{y} (#percent%)",
               color: "#C77575",
           name: "Fail",
           showInLegend: true,
           dataPoints: $scope.stdgraphseries3,
           indexLabelFontSize: 12,
       }

       ]
   });

            chart.render();



        }



    }

})();