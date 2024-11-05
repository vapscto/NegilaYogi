(function () {
    'use strict';
    angular.module('app').controller('HallTicketGenerationCollegeController', HallTicketGenerationController)
    HallTicketGenerationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function HallTicketGenerationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.all123 = false;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.searchValue1 = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("HallTicketGenerationCollege/getdetails", pageid).then(function (promise) {
                $scope.acdlist = promise.acdlist;
                $scope.alldata = promise.alldata;
                $scope.course_list = promise.courseslist;
                $scope.ASMAY_Id = promise.asmaY_Id;
               

            });
        };




        $scope.onselectBranch = function () {
            $scope.branch_list = [];
            $scope.AMB_Id = "";
            $scope.seclist = [];
            $scope.semisters_list = [];
            $scope.AMSE_Id = "";
            var data = {
                "AMCO_Id": $scope.AMCO_Id
            };
            apiService.create("HallTicketGenerationCollege/onselectclass", data).then(function (promise) {
                if (promise.branchlist != null && promise.branchlist.length > 0) {
                    $scope.branch_list = promise.branchlist;
                }
                else {
                    swal("Record Not Found  !");
                }


            });
        };
        $scope.OnchangeSemester = function () {
            $scope.seclist = [];
            $scope.semisters_list = [];
            $scope.AMSE_Id = "";
            $scope.exam_list = [];
            $scope.EME_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };
            apiService.create("HallTicketGenerationCollege/onselectAcdYear", data).then(function (promise) {
                if (promise.semisters != null && promise.semisters.length > 0) {
                    $scope.semisters_list = promise.semisters;
                }
                else {
                    swal("Record Not Found !");
                }

            });
        };
        //viewrecordspopup

        $scope.get_exams = function () {
            $scope.exam_list = [];
            $scope.seclist = [];
            $scope.EME_Id = "";
            if ($scope.AMCO_Id !== "" && $scope.AMCO_Id !== undefined && $scope.AMSE_Id !== "" && $scope.AMSE_Id !== undefined && $scope.AMB_Id !== ""
                && $scope.AMB_Id !== undefined) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "AMB_Id": $scope.AMB_Id
                };
                apiService.create("HallTicketGenerationCollege/onselectsection", data).then(function (promise) {
                    if (promise.examlist != null && promise.examlist.length > 0) {
                        $scope.exam_list = promise.examlist;
                    }
                    else {
                        swal("Exam Are Not Found !")
                    }
                    $scope.seclist = promise.sectionlist;
                });
            }
        };


        $scope.submitted = false;
        $scope.savedetail = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                $scope.checkseclist = [];
                angular.forEach($scope.seclist, function (option3) {
                    if (option3.section === true) {
                        $scope.checkseclist.push(option3);
                    }

                });

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,

                    "section_Array": $scope.checkseclist,
                    "EME_Id": $scope.EME_Id,
                    "prefixstr": $scope.prefix,
                    "startno": $scope.startno,
                    "increment": $scope.increment,
                    "leadingzeros": $scope.preno
                };

                apiService.create("HallTicketGenerationCollege/savedetail", data).then(function (promise) {
                    if (promise.returnval === true) {

                        swal('Data Saved successfully');
                        $scope.cancel();
                        $scope.BindData();
                    }
                    else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                        swal('Records Already Exist !');
                    }
                    else {
                        swal('Data Not Saved !');
                    }
                });
            }
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.seclist.some(function (option3) {
                return option3.acmS_Id;
            });
        };

        $scope.cancel = function () {
            $scope.ASMAY_Id = '';
            $scope.ASMCL_Id = '';
            $scope.ASMS_Id = '';
            $scope.EME_Id = '';
            $scope.prefix = '';
            $scope.startno = '';
            $scope.increment = '';
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.ViewStudentDetails = function (viweobj) {

            $scope.yearname = "";

            $scope.coursename = "";
            $scope.sectioname = "";
            $scope.examname = "";
            $scope.branchname = "";
            $scope.semestername = "";
            $scope.datareport = [];

            var data = {
                "ASMAY_Id": viweobj.ASMAY_Id,
                "AMCO_Id": viweobj.AMCO_Id,
                "AMB_Id": viweobj.AMB_Id,
                "AMSE_Id": viweobj.AMSE_Id,
                "ACMS_Id": viweobj.ACMS_Id,
                "EME_Id": viweobj.EME_Id,

            };

            apiService.create("HallTicketGenerationCollege/ViewStudentDetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.datareport = promise.getStudent;
                    if ($scope.datareport !== null && $scope.datareport.length > 0) {
                        angular.forEach($scope.datareport, function (dd) {
                            dd.checkedvalue = false;
                        });
                        $scope.all123 = false;
                        $scope.yearname = viweobj.ASMAY_Year;
                        $scope.sectioname = viweobj.ACMS_SectionName;
                        $scope.examname = viweobj.EME_ExamName;
                        $scope.coursename = viweobj.AMCO_CourseName;
                        $scope.branchname = viweobj.AMB_BranchName;
                        $scope.semestername = viweobj.AMSE_SEMName;

                        $('#popup4546').modal('show');
                    } else {
                        swal("No Records Found");
                    }
                }
            });
        };

        $scope.SaveStudentStatus = function (saveobj) {

            $scope.selectedstudents = [];

            angular.forEach($scope.datareport, function (dd) {
                if (dd.checkedvalue === true) {
                    $scope.selectedstudents.push({
                        AMCST_Id: dd.AMCST_Id,
                        EHTC_Id: dd.EHTC_Id
                    });
                }
            });

            if ($scope.selectedstudents.length > 0) {
                var data = {
                    "section_Array": $scope.selectedstudents
                    
                };
                apiService.create("HallTicketGenerationCollege/SaveStudentStatus", data).then(function (promise) {
                    if (promise.returnval === true) {
                        swal("Records Updated");
                       
                        $('#popup4546').modal('hide');
                    } else {
                        swal("Failed To Update Record");
                    }
                });
            } else {
                swal("Select The Students To Change The Status");
            }
        };

        $scope.optionToggled123 = function () {
            $scope.all123 = $scope.datareport.every(function (itm) { return itm.checkedvalue; });
        };

        $scope.toggleAll123 = function () {
            angular.forEach($scope.datareport, function (dd) {
                dd.checkedvalue = $scope.all123;
            });
        };
    }
})();