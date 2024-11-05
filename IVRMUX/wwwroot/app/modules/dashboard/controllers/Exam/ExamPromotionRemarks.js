
(function () {
    'use strict';
    angular.module('app').controller('ExamPromotionRemarksController', ExamPromotionRemarksController)

    ExamPromotionRemarksController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$filter']
    function ExamPromotionRemarksController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $filter) {


        $scope.select_cat = false;
        $scope.examtype = 1;
        $scope.checkall = false;

        $scope.BindData = function () {
            var pageid = 1;
            apiService.getURI("ExamPromotionRemarks/Getdetails", pageid).then(function (promise) {
                $scope.year_list = promise.getyear;
            });
        };

        $scope.get_class = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("ExamPromotionRemarks/get_class", data).then(function (promise) {
                if (promise != null) {
                    $scope.classlist = promise.getclass;
                    if ($scope.classlist.length > 0) {
                        $scope.classlist = promise.getclass;
                    } else {
                        swal("Class Teacher Is Not Mapped For This Staff");
                    }

                } else {
                    swal("No Records Found");
                }
            });
        };

        $scope.get_section = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            };
            apiService.create("ExamPromotionRemarks/get_section", data).then(function (promise) {
                if (promise != null) {
                    $scope.getsection = promise.getsection;
                    if ($scope.getsection.length > 0) {
                        $scope.sectionlist = promise.getsection;
                    } else {
                        swal("No Records Found");
                    }
                }
                else {
                    swal("No Records Found");
                }
            });
        };
        //get_Remarks
        $scope.get_Remarks = function (user) {
            if (user.model != null && user.model != "") {
                user.eprD_Remarks = user.model;
            }
        }
        $scope.get_exam = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };
            apiService.create("ExamPromotionRemarks/get_exam", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getexam = promise.getexam;
                    if ($scope.getexam.length > 0) {
                        $scope.exam_list = promise.getexam;
                    } else {
                        swal("No Exam Is Mapped For this Combination");
                    }
                } else {
                    swal("No Records Found");
                }

            });
        };

        $scope.submitted = false;
        $scope.search_student = function () {

            if ($scope.myForm.$valid) {

                if ($scope.examtype == 1) {
                    $scope.EME_Id = 0;
                } else {
                    $scope.EME_Id = $scope.EME_Id;
                }

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "EME_Id": $scope.EME_Id,
                    "examtype": $scope.examtype
                };

                apiService.create("ExamPromotionRemarks/search_student", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.student = promise.getstudentdetails;
                        $scope.getsavedetails = promise.getsavedetails;
                        if ($scope.student !== null && $scope.student.length > 0) {
                            $scope.getstudentmarks = promise.getstudentmarks;
                            angular.forEach($scope.student, function (dd) {
                                dd.checkedvalue = false;
                                dd.eprD_PromotionName = "";
                                dd.eprD_Remarks = "";
                                dd.eprD_ClassPromoted = "";

                                angular.forEach($scope.getstudentmarks, function (mrks) {
                                    if (dd.amsT_Id === mrks.amsT_Id) {
                                        if ($scope.examtype == 1) {
                                            dd.obtainedmarks = mrks.estmpP_TotalObtMarks;
                                            dd.totalmaxmarks = mrks.estmpP_TotalMaxMarks;
                                            dd.percentage = mrks.estmpP_Percentage;
                                        } else {
                                            dd.obtainedmarks = mrks.estmP_TotalObtMarks;
                                            dd.totalmaxmarks = mrks.estmP_TotalMaxMarks;
                                            dd.percentage = mrks.estmP_Percentage;
                                        }                                        
                                    }
                                });
                            });

                            

                            if ($scope.getsavedetails !== null &&  $scope.getsavedetails.length > 0) {
                                angular.forEach($scope.getsavedetails, function (studsave) {
                                    angular.forEach($scope.student, function (stud) {
                                        if (parseInt(stud.amsT_Id) === parseInt(studsave.amsT_Id)) {
                                            stud.checkedvalue = true;
                                            stud.eprD_PromotionName = studsave.eprD_PromotionName;
                                            stud.eprD_Remarks = studsave.eprD_Remarks;
                                            stud.eprD_ClassPromoted = studsave.eprD_ClassPromoted;
                                            stud.eprD_Id = studsave.eprD_Id;
                                        }
                                    });                                  
                                });
                            }
                            $scope.select_cat = true;
                        }
                    } else {
                        swal("No Records Found");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.sortKey = 'studentname';
        $scope.sortReverse = false;

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.submitted1 = false;
        $scope.save_details = function () {

            $scope.temp = [];

            if ($scope.examtype == 1) {
                $scope.EME_Id = 0;
            } else {
                $scope.EME_Id = $scope.EME_Id;
            }


            angular.forEach($scope.student, function (stu) {
                if (stu.checkedvalue == true) {
                    $scope.temp.push(stu);
                }
            });
            if ($scope.temp.length == 0) {
                swal("Select Alteast One Student To Save The Details");
                return;
            }

            if ($scope.myForm1.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "EME_Id": $scope.EME_Id,
                    "examtype": $scope.examtype,
                    "examtype_temp": $scope.temp
                };
                apiService.create("ExamPromotionRemarks/save_details", data).then(function (promise) {

                    if (promise !== null) {

                        if (promise.message == "Add") {
                            if (promise.returnval == true) {
                                swal("Record Saved /Update  Successfully");
                            } else {
                                swal("Failed To Save /Update Record");
                            }
                        } else if (promise.message == "Update") {
                            if (promise.returnval == true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else {
                            swal("Failed To Save /Update Record");
                        }

                    } else {
                        swal("Failed To Save /Update Record");
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted1 = true;
            }
        };

        $scope.clear = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.getlist = function () {
            $scope.select_cat = false;
            if ($scope.examtype == 0) {
                $scope.student = [];
                $scope.getsavedetails = [];
            } else {
                $scope.EME_Id = "";
                $scope.student = [];
                $scope.getsavedetails = [];
            }
        };

        $scope.toggleAll_S = function (stas) {
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.student, function (itm) {
                itm.checkedvalue = toggleStatus;

            });
        };

        $scope.optionToggled = function () {
            $scope.checkall = $scope.student.every(function (itm) { return itm.checkedvalue; });
        };
    }

})();