(function () {
    'use strict';
    angular
        .module('app')
        .controller('HODExamReportController', HODExamReportController)

    HODExamReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function HODExamReportController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

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

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //-----------academicyear Selection
        //$scope.onyearchange = function (asmaY_Id) {
        //    
        //    var data = {
        //        "ASMAY_Id": $scope.asmaY_Id,
        //      //  "AMST_ID": $scope.amsT_Id,
        //    }
        //    apiService.create("HODExamReport/getsectiondata", data).
        //       then(function (promise) {
        //           
        //           // $scope.subjectlst = "";
        //           $scope.examlist = "";
        //           $scope.ismS_Id = 0;
        //           $scope.studentlist = promise.studentlist;
        //           if (promise.studetiallist != null) {
        //               $scope.className = promise.studetiallist[0].asmcL_ClassName;
        //               $scope.sectionName = promise.studetiallist[0].asmC_SectionName;
        //               $scope.asmcl_Id = promise.studetiallist[0].asmcL_id;
        //               $scope.asms_Id = promise.studetiallist[0].asmS_id;
        //               $scope.examlist = promise.examlist;
        //               if ($scope.examlist.length == "0") {
        //                   swal("No Record Found....")
        //               }
        //           }

        //       })
        //}

        $scope.onyearchange = function (ASMAY_Id) {

            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.asmcL_Id = "";
            $scope.amsT_Id = "";

            $scope.examlist = [];
            $scope.classlist = [];
            $scope.sectionlist = [];
            $scope.fillstudent = [];

            $scope.hide_examg = false;

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("HODExamReport/get_classes", data).then(function (promise) {
                $scope.classlist = promise.classlist;

            })

        }

        //===================================Class Change..
        $scope.onclasschange = function () {

            $scope.emE_Id = "";
            $scope.amsT_Id = "";

            $scope.examlist = [];
            $scope.sectionlist = [];
            $scope.fillstudent = [];

            $scope.hide_examg = false;


            var data = {
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
            }
            apiService.create("HODExamReport/getsectiondata", data).
                then(function (promise) {

                    $scope.examlist = "";
                    $scope.ismS_Id = 0;
                    $scope.sectionlist = promise.sectionlist;



                })
        }

        //=================================Section Change..
        $scope.sectionchange = function () {


            $scope.emE_Id = "";
            $scope.amsT_Id = "";
            $scope.examlist = [];
            $scope.fillstudent = [];
            $scope.hide_examg = false;

            var data = {
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
            }
            apiService.create("HODExamReport/getstudentdata", data).
                then(function (promise) {

                    $scope.examlist = "";
                    $scope.ismS_Id = 0;
                    $scope.fillstudent = promise.fillstudent;



                })
        }

        // ================Change Student
        $scope.onStudentchange = function () {

            $scope.emE_Id = "";
            $scope.examlist = [];
            $scope.hide_examg = false;

            var data = {
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                "AMST_Id": $scope.amsT_Id,
            }
            apiService.create("HODExamReport/getexamdata", data).
                then(function (promise) {
                    if (promise.examlist.length > 0) {
                        $scope.examlist = promise.examlist;
                    }
                    else {
                        swal('Record Not Available!');
                    }


                })
        }

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;

            apiService.getDATA("HODExamReport/getloaddata").
                then(function (promise) {

                    $scope.studetiallist = promise.studetiallist;
                    $scope.className = promise.studetiallist[0].asmcL_ClassName;
                    $scope.sectionName = promise.studetiallist[0].asmC_SectionName;
                    $scope.asmcl_Id = promise.studetiallist[0].asmcL_id;
                    $scope.asms_Id = promise.studetiallist[0].asmS_id;
                    $scope.ismS_Id = 0;


                    $scope.asmaY_Id = $scope.studetiallist[0].asmaY_Id;
                    $scope.onyearchange($scope.asmaY_Id);

                })
        };

        //-----------Subject Selection
        $scope.examchange = function (emE_Id) {

            var data = {
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                "AMST_Id": $scope.amsT_Id,
                "EME_Id": $scope.emE_Id,
            }
            apiService.create("HODExamReport/getexamdetails", data).
                then(function (promise) {

                    $scope.examsubjdetails = promise.examsubjdetails;
                    $scope.hide_examg = true;

                    //--------------------Graph

                    $scope.examrptgraph = [];
                    $scope.examrptgraph1 = [];
                    if ($scope.examsubjdetails.length != "0" && $scope.examsubjdetails != null) {
                        $scope.attdetails = true;
                        for (var i = 0; i < $scope.examsubjdetails.length; i++) {
                            $scope.examrptgraph.push({ label: $scope.examsubjdetails[i].ismS_SubjectName, "y": $scope.examsubjdetails[i].estmpS_ObtainedMarks, name: $scope.examsubjdetails[i].ismS_SubjectName })
                        }

                        var chart = new CanvasJS.Chart("columnchart", {
                            animationEnabled: true,
                            animationDuration: 3000,
                            height: 350,
                            width: 520,
                            colorSet: "graphcolor",
                            axisX: {
                                labelFontSize: 12,
                                interval: 1
                            },
                            axisY: {
                                labelFontSize: 12,
                            },

                            toolTip: {
                                shared: true
                            },
                            data: [{
                                showInLegend: false,
                                type: "column",
                                dataPoints: $scope.examrptgraph
                            }

                            ]
                        });
                        chart.render();
                        var chart = new CanvasJS.Chart("linechart", {
                            animationEnabled: true,
                            animationDuration: 2000,
                            height: 350,
                            width: 520,
                            colorSet: "graphcolor",
                            axisX: {
                                labelFontSize: 12,
                                interval: 1
                            },
                            axisY: {
                                labelFontSize: 12,
                                interval: 1
                            },
                            toolTip: {
                                shared: true
                            },
                            data: [{

                                showInLegend: true,
                                type: "doughnut",
                                radius: "100%",
                                dataPoints: $scope.examrptgraph
                            }

                            ]
                        });
                        chart.render();
                    }
                    else {
                        swal("No Record Found....")
                        $scope.hide_examg = false;

                    }

                })
        }
    };
})();