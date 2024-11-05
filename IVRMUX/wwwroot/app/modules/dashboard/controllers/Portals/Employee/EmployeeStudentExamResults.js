(function () {
    'use strict';
    angular
.module('app')
.controller('EmployeeStudentExamResultsController', EmployeeStudentExamResultsController)

    EmployeeStudentExamResultsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    //dashboard.controller("EmployeeDashboardController", ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache',
    function EmployeeStudentExamResultsController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {



        $scope.LoadData = function () {
            
            //var sal_list = [];
            //var TT_list = [];
            apiService.getDATA("EmployeeStudentExamResults/getalldetails")
                .then(function (promise) {
                   
                    //$scope.classDropdown = promise.classlist;
                    $scope.yearlt = promise.academicList;
                    $scope.remarks_list = promise.remarklist;
                    //$scope.sectionDropdown = promise.sectionList;
                    //$scope.studentDropdown = promise.studentList;
                    $scope.asmcL_Id = "";
                    $scope.asmS_Id = "";
                    $scope.amsT_Id = "";
                    $scope.emE_Id = "";
                    $scope.classDropdown = "";
                    $scope.sectionDropdown = "";
                    $scope.studentDropdown = "";
                    $scope.exsplt = "";

                })
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.get_class = function () {
            
           
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.amsT_Id = "";
            $scope.emE_Id = "";
           
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            }
            apiService.create("EmployeeStudentExamResults/get_class", data)
                .then(function (promise) {
                    $scope.classDropdown = promise.classlist;

                })
        }
        $scope.get_section = function () {
            
            $scope.amsT_Id = "";
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.studentDropdown = "";
            $scope.exsplt = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            }
            apiService.create("EmployeeStudentExamResults/get_section", data)
                .then(function (promise) {
                    $scope.sectionDropdown = promise.sectionList;


                })
        }
        $scope.get_student = function (asmS_Id) {
            
            $scope.amsT_Id = "";
            $scope.emE_Id = "";
            $scope.exsplt = "";
            //alert(asmS_Id)
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id
            }
            apiService.create("EmployeeStudentExamResults/get_student", data)
                .then(function (promise) {
                    $scope.studentDropdown = promise.studentList;


                })

        }

        $scope.get_Exam = function () {
            
            $scope.emE_Id = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "AMST_Id": $scope.amsT_Id
            }
            apiService.create("EmployeeStudentExamResults/get_exam", data)
                .then(function (promise) {
                    $scope.exsplt = promise.exmstdlist;

                })

        }
        $scope.submitted = false;
        $scope.Save = function () {
            
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMS_Id": $scope.asmS_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "Amst_Id": $scope.amsT_Id,
                    "EME_Id": $scope.emE_Id,
                    "TXTRemark": $scope.txtRemark,
                    "EMER_ActiveFlag": true

                }
                apiService.create("EmployeeStudentExamResults/saveRemark", data)
                    .then(function (promise) {
                        
                        if (promise.returnval == true) {
                            swal('Data successfully Saved');
                            $state.reload();
                        }
                        else if (promise.returnduplicatestatus == 'Duplicate') {
                            swal('Records Already Exist !');
                            $state.reload();
                        }
                        else if (promise.returnval == false) {
                            swal('Data Not Saved !');
                            $state.reload();
                        }
                    })
            }else
            {
                $scope.submitted = true;
            }
           

        }

        $scope.showRemarksGrid = function (data) {
            
            //var data = {
            //    "ASMS_Id": $scope.asmS_Id,
            //    "ASMAY_Id": $scope.asmaY_Id,
            //    "ASMCL_Id": $scope.asmcL_Id,
            //    "Amst_Id": $scope.amsT_Id,
            //    "EME_Id": $scope.emE_Id

            //}
            apiService.create("EmployeeStudentExamResults/getremarkdetails", data)
           .then(function (promise) {
               $scope.remarklistS = promise.remarklistS;
              
           })
        }

        //$scope.Editremarkdata = function (data) {
        //    
           
        //    apiService.getURI("EmployeeStudentExamResults/editdetails/", data).
        //    then(function (promise) {
               
        //        $scope.EME_ID = promise.editlist[0].emE_Id;
        //        $scope.excode = promise.editlist[0].emE_ExamCode;
        //        $scope.exname = promise.editlist[0].emE_ExamName;
        //        $scope.EME_FinalExamFlag = promise.editlist[0].emE_FinalExamFlag;
        //        $scope.EME_ActiveFlag = promise.editlist[0].emE_ActiveFlag;
        //        $scope.exorder = promise.editlist[0].emE_ExamOrder;
        //        if (promise.editlist[0].emE_FinalExamFlag == true) {
        //            $scope.final_exm_count = 0;
        //        }
        //    })
        //};

    };
})();