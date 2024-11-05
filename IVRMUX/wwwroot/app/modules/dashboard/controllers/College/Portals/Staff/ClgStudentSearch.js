(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgStudentSearchController', ClgStudentSearchController)

    ClgStudentSearchController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function ClgStudentSearchController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

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
            apiService.getDATA("ClgStudentSearch/getloaddata").
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
            apiService.create("ClgStudentSearch/getcoursedata", data).
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

        $scope.branchview = false;
        //=========================Course Selection Change
        $scope.oncoursechange = function () {
            var data = {
                "AMCO_Id": $scope.amcO_Id,
                "ASMAY_Id": $scope.asmaY_Id
            }
            apiService.create("ClgStudentSearch/getbranchdata", data).
                then(function (promise) {
                    if (promise.branch_list.length > 0) {
                        $scope.branch_list = promise.branch_list;
                        $scope.branchview = true;
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
            apiService.create("ClgStudentSearch/getsemdata", data).
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
            apiService.create("ClgStudentSearch/getstudent", data).
                then(function (promise) {
                    if (promise.student_list.length > 0) {
                        $scope.student_list = promise.student_list;                                         
                    }
                    else {
                        $scope.student_list.length = 0;
                        $scope.get_studentsearch.length = 0;
                        swal("No Record found for selected Semester....!!")
                        
                    }
                })
        }

        //==========================Get Report

        $scope.getreport = function () {          
            if ($scope.myForm.$valid) {                   
                $scope.stu_id = $scope.obj.amcsT_Id.amcsT_Id;
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMCST_Id": $scope.stu_id,                  
                }
                apiService.create("ClgStudentSearch/getreport", data).
                    then(function (promise) {
                        $scope.get_studentsearch = promise.get_studentsearch;   
                        $scope.studentname = $scope.get_studentsearch[0].studentname;
                        $scope.ACYST_RollNo = $scope.get_studentsearch[0].ACYST_RollNo;
                        $scope.AMCST_RegistrationNo = $scope.get_studentsearch[0].AMCST_RegistrationNo;
                        $scope.AMCST_AdmNo = $scope.get_studentsearch[0].AMCST_AdmNo;
                        $scope.AMCST_MobileNo = $scope.get_studentsearch[0].AMCST_MobileNo;
                        $scope.AMCST_emailId = $scope.get_studentsearch[0].AMCST_emailId;
                        $scope.AMCST_DOB = $scope.get_studentsearch[0].AMCST_DOB;
                        $scope.AMCST_Sex = $scope.get_studentsearch[0].AMCST_Sex;
                        $scope.fatherName = $scope.get_studentsearch[0].fatherName;
                        $scope.mothername = $scope.get_studentsearch[0].mothername;
                        $scope.AMCST_FatherMobleNo = $scope.get_studentsearch[0].AMCST_FatherMobleNo;
                        $scope.AMCST_MotherMobleNo = $scope.get_studentsearch[0].AMCST_MotherMobleNo;
                        $scope.AMCST_FatheremailId = $scope.get_studentsearch[0].AMCST_FatheremailId;
                        $scope.AMCST_MotherEmailId = $scope.get_studentsearch[0].AMCST_MotherEmailId;
                        $scope.AMCST_PerStreet = $scope.get_studentsearch[0].AMCST_PerStreet;
                        $scope.AMCST_PerArea = $scope.get_studentsearch[0].AMCST_PerArea;
                        $scope.AMCST_PerCity = $scope.get_studentsearch[0].AMCST_PerCity;
                        $scope.AMCST_PerState = $scope.get_studentsearch[0].AMCST_PerState;
                        $scope.AMCST_PerPincode = $scope.get_studentsearch[0].AMCST_PerPincode;   
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

