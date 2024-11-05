(function () {
    'use strict';

    angular
        .module('app')
        .controller('HODStudentSearchController', HODStudentSearchController);

    HODStudentSearchController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache'];

    function HODStudentSearchController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache) {
        $scope.LoadData = function () {

            //apiService.getDATA("HODStudentSearch/getdetails")
            //   .then(function (promise) {
            //       $scope.studentlst = promise.fillstudent;
            //   })
            apiService.getDATA("HODExamReport/getloaddata").
                then(function (promise) {

                    $scope.studetiallist = promise.studetiallist;
                    $scope.className = promise.studetiallist[0].asmcL_ClassName;
                    $scope.sectionName = promise.studetiallist[0].asmC_SectionName;
                    $scope.asmcl_Id = promise.studetiallist[0].asmcL_id;
                    $scope.asms_Id = promise.studetiallist[0].asmS_id;
                    $scope.ismS_Id = 0;

                    $scope.asmaY_Id = promise.studetiallist[0].asmaY_Id;
                    $scope.onyearchange($scope.asmaY_Id);

                })
        };

        $scope.searchfilter = function (studentlst) {

            var studid = studentlst.amsT_Id;
            var yearid = $scope.asmaY_Id;
            var data = {
                "Amst_Id": studid,
                "ASMAY_Id": yearid
            }

            apiService.create("HODStudentSearch/getstudentdetails", data).
                then(function (promise) {

                    $scope.showFeeD = true;
                    $scope.studentlistall = promise.fillstudentalldetails;
                    $scope.examdetails = promise.examlist;
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

                        swal("Student did't attend any Exam....!!")
                    }

                })
        };

        //=======================Change of Academic Year...
        $scope.onyearchange = function (ASMAY_Id) {

            $scope.showStudentD = false;
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.amst_Id = "";
            $scope.classlist = [];
            $scope.fillstudent = [];
            $scope.sectionlist = [];

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
        //====================== Change of Class Name
        $scope.onclasschange = function () {

            $scope.showStudentD = false;
            $scope.asmS_Id = "";
            $scope.amst_Id = "";
            $scope.fillstudent = [];
            $scope.sectionlist = [];

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

        //============================Section Change..
        $scope.sectionchange = function () {

            $scope.showStudentD = false;
            $scope.fillstudent = [];


            var data = {
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
            }

            apiService.create("HODExamReport/getstudentdata", data).then(function (promise) {

                $scope.examlist = "";
                $scope.ismS_Id = 0;
                $scope.fillstudent = promise.fillstudent;

            })
        }

    };
})();
