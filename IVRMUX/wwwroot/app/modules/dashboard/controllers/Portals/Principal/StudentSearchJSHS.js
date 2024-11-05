(function () {
    'use strict';
    angular
.module('app')
        .controller('StudentSearchJSHSController', StudentSearchJSHSController)

    StudentSearchJSHSController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache']
    function StudentSearchJSHSController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache) {
        $scope.LoadData = function () {
            
            apiService.getDATA("StudentSearch/getdetails")
               .then(function (promise) {
                   $scope.studentlst = promise.fillstudent;

                   $scope.fillacademiyear = promise.yearlist;
                   console.log($scope.fillacademiyear)
                   $scope.currentYear = promise.currentYear;
                   for (var i = 0; i < $scope.fillacademiyear.length; i++) {
                       if ($scope.currentYear[0].asmaY_Id == $scope.fillacademiyear[i].asmaY_Id) {
                           $scope.ASMAY_Id = $scope.currentYear[0].asmaY_Id;
                       }
                       $scope.classlist = promise.classlist;
                       $scope.ASMCL_Id = $scope.classlist[0].asmcL_Id;
                       $scope.sectionlist = promise.sectionlist;
                       $scope.asmS_Id = $scope.sectionlist[0].asmS_Id;

                       if (promise.studentCount > 0) {
                           $scope.count = promise.studentCount;
                           $scope.studentList = promise.studentlist;
                           console.log($scope.studentList);
                       }
                       else {
                           swal("No records found for selected academicYear,class and section");
                           $scope.count = 0;
                       }
                   }
               })
        };

        $scope.showsectionGrid = function (emp) {

            var data = {
                "Amst_Id": $scope.Amst_Id.amst_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "EME_Id": emp.emE_Id,
            }

            apiService.create("StudentSearch/showsectionGrid", data).
                then(function (promise) {
                    $scope.subwiseexmlist = promise.subwiseexmlist;
                    
                })

        }

        $scope.studentlistall = [];
        $scope.examdetails = [];
        $scope.attendencelist = [];
        $scope.termwisefeelist = [];
        $scope.termttl = 0;
        $scope.searchfilter = function (studentlst) {
            $scope.studentlistall = [];
            $scope.examdetails = [];
            $scope.attendencelist = [];
            $scope.termwisefeelist = [];
            $scope.termttl = 0;
            debugger;
            var a = $scope.amst_Id;
            var studid = studentlst.amst_Id;
            var data = {
                "Amst_Id": studid,
                "ASMAY_Id": $scope.ASMAY_Id
            }
            
            apiService.create("StudentSearch/getstudentdetails", data).
                then(function (promise) {
                    debugger;
                    $scope.showFeeD = true;
                    $scope.studentlistall = promise.fillstudentalldetails;
                    console.log($scope.studentlistall);
                    $scope.examdetails = promise.examlist;
                    $scope.attendencelist = promise.attendencelist;
                    $scope.termwisefeelist = promise.termwisefeelist;
                    $scope.getfeedetail = promise.getfeedetails;
                    if ($scope.studentlistall != null) {
                        $scope.showStudentD = true;

                    } else {
                        $scope.showStudentD = false;

                    }
                    if ($scope.examdetails != null && $scope.examdetails != 0) {
                        $scope.showExamD = true;

                    } else {
                        $scope.showExamD = false;

                       // swal("Student did't attend any Exam....!!")
                    }

                    if ($scope.termwisefeelist.length>0) {
                        angular.forEach($scope.termwisefeelist, function (dd) {

                            $scope.termttl += dd.BalanceAmount;
                        })

                       
                    }

                })
        };
        //$scope.searchfilter = function (data) {
        //    
        //    apiService.getURI("EmployeeStudentSearch/getstudentdetails", data).
        //        then(function (promise) {
        //        $scope.studentlistall = promise.fillstudentalldetails;
        //        if ($scope.studentlistall != null) {
        //            $scope.showStudentD = true;
        //        } else {
        //            $scope.showStudentD = false;
        //        }

        //    })
        //};

        $scope.onSelectclass = function (classId) {
            if ($scope.ASMAY_Id > 0 && $scope.ASMCL_Id > 0 && $scope.asmS_Id > 0) {
                $scope.GetStudentDetails1();
            }
        }
        $scope.onSelectyear = function () {
            if ($scope.ASMAY_Id > 0 && $scope.ASMCL_Id > 0 && $scope.asmS_Id > 0) {
                $scope.GetStudentDetails1();
            }
        }

        $scope.GetStudentDetails1 = function () {
            debugger;

            var j = $scope.amst_Id;
            $scope.Amst_Id = [];
            $scope.studentlst = [];
            $scope.studentlst.amst_Id = '';
            $scope.showStudentD = false;
            $scope.showExamD = false;
            $scope.showExamD = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.asmS_Id
            }
            apiService.create("StudentSearch/GetStudentDetails1/", data).
                then(function (promise) {

                    if (promise.studentCount > 0) {
                        $scope.count = promise.studentCount;
                        $scope.studentlst = promise.fillstudent;
                    } else {
                        swal("No records found for selected academicYear,class and section");
                        $scope.studentlst = [];
                        $scope.count = 0;
                    }
                });
        }
    };
})();