(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgStudentAttendanceController', ClgStudentAttendanceController)

    ClgStudentAttendanceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function ClgStudentAttendanceController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

        CanvasJS.addColorSet("graphcolor",
            [//colorSet Array
                "#2471A3",
                "#76D7C4",                
                "#DAF7A6",
                "#FFC300",
                "#FF5733",
            ]);
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            apiService.getDATA("ClgStudentAttendance/getloaddata").
                then(function (promise) {
                    $scope.yearlist = promise.yearlist;
                })
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        }


        //====================Academic Year Selection
        $scope.onselectYear = function () {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            }
            apiService.create("ClgStudentAttendance/getcoursedata", data).
                then(function (promise) {
                    if (promise.course_list.length > 0) {
                        $scope.course_list = promise.course_list;
                    }
                    else {
                        swal("No Record Found....!!")
                        $scope.course_list.length = 0;
                    }
                })
        }

        //=========================Course Selection Change
        $scope.oncoursechange = function () {
            var data = {
                "AMCO_Id": $scope.amcO_Id,
                "ASMAY_Id": $scope.asmaY_Id
            }
            apiService.create("ClgStudentAttendance/getbranchdata", data).
                then(function (promise) {
                    if (promise.branch_list.length > 0) {
                        $scope.branch_list = promise.branch_list;
                    }
                    else {
                        swal("No Branch found for selected course....!!")
                        $scope.branch_list.length = 0;
                    }
                })
        }

        //=========================Branch All Check
        $scope.toggleAllB = function (allB) {
            angular.forEach($scope.branch_list, function (brch) {
                brch.branchck = $scope.allB;
            })
            $scope.onbranchchange();

        };
      

        $scope.isOptionsRequiredb = function () {
            return !$scope.branch_list.some(function (options) {
                return options.branchck;
            });
        }
        //=========================Branch Selection Change
        $scope.onbranchchange = function () {
            $scope.brancharray = [];
            $scope.sem_list = "";
            $scope.allB = $scope.branch_list.every(function (brh) { return brh.branchck; });
            angular.forEach($scope.branch_list, function (brch) {
                if (brch.branchck == true) {
                    $scope.brancharray.push(brch);
                }
            })
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "branchArray": $scope.brancharray,
            }
            apiService.create("ClgStudentAttendance/getsemdata", data).
                then(function (promise) {
                    if (promise.sem_list.length > 0) {
                        $scope.sem_list = promise.sem_list;
                    }
                    else {
                        $scope.sem_list.length = 0;
                    }
                })
        }

        //=========================Semester All Check
        $scope.toggleAllS = function (allS) {
            angular.forEach($scope.sem_list, function (sm) {
                sm.semck = $scope.allS;
            })
            $scope.onsemesterchange();
        };

        $scope.isOptionsRequiredS = function () {
            return !$scope.sem_list.some(function (options) {
                return options.semck;
            });
        }

        //=========================Semester Selection Change
        $scope.onsemesterchange = function () {
            $scope.brancharray = [];          
          
            angular.forEach($scope.branch_list, function (brch) {
                if (brch.branchck == true) {
                    $scope.brancharray.push(brch);
                }
            })
            $scope.semesterArray = [];           
           
            angular.forEach($scope.sem_list, function (sm) {
                if (sm.semck == true) {
                    $scope.semesterArray.push(sm);
                }
            })
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id,
                "branchArray": $scope.brancharray,
                "semesterArray": $scope.semesterArray,
            }
            apiService.create("ClgStudentAttendance/getstudent", data).
                then(function (promise) {
                    if (promise.student_list.length > 0) {
                        $scope.student_list = promise.student_list;
                    }
                    else {
                        $scope.student_list.length = 0;
                        swal("No Record found for selected Semester....!!")
                        
                    }
                })
        }

        //==========================Get Report

        $scope.getattendance = function () {
          
            if ($scope.myForm.$valid) {                   
                $scope.stu_id = $scope.obj.amcsT_Id.amcsT_Id;
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMCST_Id": $scope.stu_id,                  
                }

                apiService.create("ClgStudentAttendance/getreport", data).
                    then(function (promise) {
                        $scope.attendancereport = promise.attendancereport;
                        $scope.attmonth = [];                       
                        $scope.attpersent = [];
                        $scope.attclassheld = [];

                        if ($scope.attendancereport.length > 0) {
                           
                            angular.forEach($scope.attendancereport, function (att) {
                                $scope.attmonth.push({ label: att.MONTH_NAME })                              
                                $scope.attpersent.push({ label: att.MONTH_NAME, "y": parseInt(att.TOTAL_PRESENT) })
                                $scope.attclassheld.push({ label: att.MONTH_NAME, "y": parseInt(att.CLASS_HELD) })
                            })
                            $scope.newarytm = [];
                            $scope.clspersnt = [];
                            angular.forEach($scope.attendancereport, function (qw) {
                                $scope.newarytm = [];
                                $scope.newarytm = [qw.TOTAL_PRESENT, qw.CLASS_HELD];
                                $scope.clspersnt = ["TOTAL PRESENT", "CLASS HELD"];
                                qw.na = $scope.newarytm;
                                qw.cp = $scope.clspersnt;
                            })
                            $scope.grddata = [];
                            angular.forEach($scope.attendancereport, function (ag) {
                                angular.forEach(ag.na, function (attg) {
                                    $scope.grddata.push(attg);
                                })
                            })
                            $scope.clsheldpersnt = [];
                            angular.forEach($scope.attendancereport, function (cg) {
                                angular.forEach(cg.cp, function (cpg) {
                                    $scope.clsheldpersnt.push(cpg);
                                })
                            })
                            //===================GRAPH 1
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
                                    name: "TOTAL PERSENT",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.attpersent
                                },
                                {
                                    name: "CLASS HELD",
                                    showInLegend: true,
                                    type: "column",
                                    dataPoints: $scope.attclassheld
                                }
                                ]
                            });
                            chart.render();
                            //===================GRAPH 2
                            var chart = new CanvasJS.Chart("linechart", {
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
                                    name: "TOTAL PERSENT",
                                    showInLegend: true,
                                    type: "area",
                                    dataPoints: $scope.attpersent
                                },
                                {
                                    name: "CLASS HELD",
                                    showInLegend: true,
                                    type: "area",
                                    dataPoints: $scope.attclassheld
                                }
                                ]
                            });
                            chart.render();
                        }
                        else {
                            swal("No Record Found....!!")
                            $scope.attendancereport.length = 0;
                        }                     
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };

    };
})();

