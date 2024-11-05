(function () {
    'use strict';

    angular.module('app').controller('StudentTransaction', StudentTransaction);

    StudentTransaction.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter'];

    function StudentTransaction($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.obj = {};

        $scope.Onload = function () {
            apiService.getURI("StudentTransaction/Onload/", 1).then(function (promise) {
                $scope.academicYear = promise.academicYear;
            });
        };

        $scope.classteacher = function () {
            $scope.ASMS_Id = "";
            $scope.ECT_Id = "";
            $scope.EME_Id = "";
            $scope.Skills = "";
            $scope.ECACT_Id = "";
            $scope.ECS_Id = "";
            $scope.studentList = [];
            $scope.StudentScoreData = [];
            $scope.SelectedStudentScore = [];
        };

        $scope.onchangeyear = function () {
            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            $scope.ECT_Id = "";
            $scope.EME_Id = "";
            $scope.Skills = "";
            $scope.ECACT_Id = "";
            $scope.ECS_Id = "";
            $scope.studentList = [];
            $scope.StudentScoreData = [];
            $scope.SelectedStudentScore = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "exam_termwise_flag": $scope.all
            };

            apiService.create("StudentTransaction/onchangeyear", data).then(function (promise) {
                $scope.classList = promise.classList;

            });
        };

        $scope.onchangeclass = function () {
            $scope.ASMS_Id = "";
            $scope.ECT_Id = "";
            $scope.Skills = "";
            $scope.ECACT_Id = "";
            $scope.ECS_Id = "";
            $scope.EME_Id = "";
            $scope.studentList = [];
            $scope.StudentScoreData = [];
            $scope.SelectedStudentScore = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "exam_termwise_flag": $scope.all
            };

            apiService.create("StudentTransaction/onchangeclass", data).then(function (promise) {
                $scope.sectionList = promise.sectionList;

            });
        };

        $scope.onchangesection = function () {
            $scope.ECT_Id = "";
            $scope.Skills = "";
            $scope.ECACT_Id = "";
            $scope.ECS_Id = "";
            $scope.EME_Id = "";
            $scope.studentList = [];
            $scope.StudentScoreData = [];
            $scope.SelectedStudentScore = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "exam_termwise_flag": $scope.all
            };

            apiService.create("StudentTransaction/onchangesection", data).then(function (promise) {
                if (promise !== null) {
                    if ($scope.all === "ExamWise") {
                        $scope.exammaster = promise.exammaster;
                    } else {
                        $scope.examTerms = promise.examTerms;
                    }                   
                }
            });
        };

        $scope.onchangeterm = function () {
            $scope.studentList = [];
            $scope.StudentScoreData = [];
            $scope.SelectedStudentScore = [];
            $scope.Skills = "";
            $scope.ECACT_Id = "";
            $scope.ECACTA_Id = "";
            $scope.ECS_Id = "";
            $scope.ECSA_Id = "";
        };

        $scope.onchangeactivitesskillflag = function () {
            $scope.ECACT_Id = "";
            $scope.ECS_Id = "";
            $scope.studentList = [];
            $scope.StudentScoreData = [];
            $scope.SelectedStudentScore = [];
            var data = {
                "Skills": $scope.Skills,
                "ASMAY_Id": $scope.ASMAY_Id,
                "exam_termwise_flag": $scope.all
            };

            apiService.create("StudentTransaction/onchangeactivitesskillflag", data).then(function (promise) {
                if (promise !== null) {
                    if ($scope.Skills === "Skills") {
                        $scope.skillsListtemp = promise.skillsList;
                        if ($scope.skillsListtemp !== null && $scope.skillsListtemp.length > 0) {
                            $scope.skillsList = $scope.skillsListtemp;
                        } else {
                            swal("No Records Found");
                        }
                    } else if ($scope.Skills === "Activities") {
                        $scope.activitiesListtemp = promise.activitiesList;
                        if ($scope.activitiesListtemp !== null && $scope.activitiesListtemp.length > 0) {
                            $scope.activitiesList = $scope.activitiesListtemp;
                        } else {
                            swal("No Records Found");
                        }
                    }
                } else {
                    swal("No Records Found");
                }
            });
        };

        $scope.onchangeactivites = function () {
            $scope.studentList = [];
            $scope.StudentScoreData = [];
            $scope.SelectedStudentScore = [];
            $scope.areasList = [];
            $scope.ECACTA_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id,                
                "ECACT_Id": $scope.ECACT_Id,
                "exam_termwise_flag": $scope.all
            };

            if ($scope.all === "ExamWise") {
                data.EME_Id = $scope.EME_Id;
            } else {
                data.ECT_Id = $scope.ECT_Id;
            }

            apiService.create("StudentTransaction/onchangeactivites", data).then(function (promise) {

                if (promise.areasList !== null && promise.areasList.length > 0) {
                    $scope.areasList = promise.areasList;
                }
            });
        };

        $scope.onchangeskills = function () {
            $scope.studentList = [];
            $scope.StudentScoreData = [];
            $scope.SelectedStudentScore = [];
            $scope.areasList = [];
            $scope.ECSA_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "ECS_Id": $scope.ECS_Id,
                "exam_termwise_flag": $scope.all
            };

            if ($scope.all === "ExamWise") {
                data.EME_Id = $scope.EME_Id;
            } else {
                data.ECT_Id = $scope.ECT_Id;
            }

            apiService.create("StudentTransaction/onchangeskills", data).then(function (promise) {
                if (promise.areasList !== null && promise.areasList.length > 0) {
                    $scope.areasList = promise.areasList;
                }

            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.StudentScoreData.some(function (options) {
                return options.checked;
            });
        };

        $scope.OnChangeActivitesArea = function () {
            $scope.StudentScoreData = [];
            $scope.studentList = [];
        };

        $scope.OnChangeSkillArea = function () {
            $scope.StudentScoreData = [];
            $scope.studentList = [];
        };


        $scope.submitted = false;

        $scope.StudentScoreData = [];

        $scope.getStudentList = function () {

            $scope.studentList = [];
            $scope.StudentScoreData = [];
            $scope.SelectedStudentScore = [];

            if ($scope.myForm.$valid) {

                if ($scope.Skills === 'Activities') {
                    $scope.ECS_Id = 0;
                    $scope.ECSA_Id = 0;
                } else {
                    $scope.ECACT_Id = 0;
                    $scope.ECACTA_Id = 0;
                }


                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                   // "ECT_Id": $scope.ECT_Id,
                    "ECACT_Id": $scope.ECACT_Id,
                    "ECS_Id": $scope.ECS_Id,
                    "ECACTA_Id": $scope.ECACTA_Id,
                    "ECSA_Id": $scope.ECSA_Id,
                    "Skills": $scope.Skills,
                    "exam_termwise_flag": $scope.all
                };

                if ($scope.all === "ExamWise") {
                    data.EME_Id = $scope.EME_Id;
                } else {
                    data.ECT_Id = $scope.ECT_Id;
                }

                apiService.create("StudentTransaction/getStudentList", data).then(function (promise) {
                    if (promise.studentList !== null) {
                        $scope.studentList = promise.studentList;
                        $scope.getsaveddata = promise.getsaveddata;

                        $scope.StudentScoreData.length = 0;

                        if ($scope.Skills === 'Skills') {
                            angular.forEach($scope.studentList, function (dd) {
                                angular.forEach($scope.getsaveddata, function (save) {
                                    if (dd.amsT_Id === save.amsT_Id) {
                                        dd.areaName = save.ecsA_Id;
                                        dd.ECSACTT_Score = save.ecsT_Score;
                                        dd.ECST_Id = save.ecsT_Id;
                                        dd.checked = true;

                                    }
                                });
                            });

                            $scope.StudentScoreData = $scope.studentList;
                            console.log($scope.StudentScoreData);
                        }
                        else if ($scope.Skills === 'Activities') {

                            angular.forEach($scope.studentList, function (dd) {
                                angular.forEach($scope.getsaveddata, function (save) {
                                    if (dd.amsT_Id === save.amsT_Id) {
                                        dd.areaName = save.ecactA_Id;
                                        dd.ECSACTT_Score = save.ecsactT_Score;
                                        dd.ECSACTT_Id = save.ecsactT_Id;
                                        dd.checked = true;
                                    }
                                });
                            });

                            $scope.StudentScoreData = $scope.studentList;
                        }

                        $scope.presentCountgrid = $scope.StudentScoreData.length;                       
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.submitted1 = false;
        $scope.SelectedStudentScore = [];

        $scope.save = function () {
            if ($scope.myForm1.$valid) {
                $scope.SelectedStudentScore.length = 0;
                if ($scope.Skills === 'Skills') {
                    $scope.ECACTA_Id = 0;
                    for (var j = 0; j < $scope.StudentScoreData.length; j++) {                       
                        if ($scope.StudentScoreData[j].checked === true) {
                            
                            $scope.SelectedStudentScore.push({
                                "AMST_Id": $scope.StudentScoreData[j].amsT_Id,
                                "ECSACTT_Score": $scope.StudentScoreData[j].ECSACTT_Score,
                                "ECSA_Id": $scope.StudentScoreData[j].ecsA_Id,
                                "areaName": $scope.ECSA_Id,
                                "EMGR_Id": $scope.StudentScoreData[j].emgR_Id,
                                "ECST_Id": $scope.StudentScoreData[j].ECST_Id
                            });
                        }
                    }
                }
                else if ($scope.Skills === 'Activities') {
                    $scope.ECSA_Id = 0;
                    for (var i2 = 0; i2 < $scope.StudentScoreData.length; i2++) {                       
                        if ($scope.StudentScoreData[i2].checked === true) {
                            $scope.SelectedStudentScore.push({
                                "AMST_Id": $scope.StudentScoreData[i2].amsT_Id,
                                "ECSACTT_Score": $scope.StudentScoreData[i2].ECSACTT_Score,
                                "ECACTA_Id": $scope.StudentScoreData[i2].ecactA_Id,
                                "areaName": $scope.ECACTA_Id,
                                "EMGR_Id": $scope.StudentScoreData[i2].emgR_Id,
                                "ECSACTT_Id": $scope.StudentScoreData[i2].ECSACTT_Id
                            });
                        }
                    }
                }

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                   // "ECT_Id": $scope.ECT_Id,
                    "Skills": $scope.Skills,
                    "ECACT_Id": $scope.ECACT_Id,
                    "ECS_Id": $scope.ECS_Id,
                    "ECACTA_Id": $scope.ECACTA_Id,
                    "ECSA_Id": $scope.ECSA_Id,
                    "SelectedStudentScore": $scope.SelectedStudentScore,
                    "exam_termwise_flag": $scope.all
                };

                if ($scope.all === "ExamWise") {
                    data.EME_Id = $scope.EME_Id;
                } else {
                    data.ECT_Id = $scope.ECT_Id;
                }

                apiService.create("StudentTransaction/saveRecord", data).then(function (promise) {
                    if (promise.returnVal === 'saved') {
                        swal("Record Saved Successfully");
                        $scope.StudentScoreData = [];
                        $scope.studentList = [];
                    }
                    else if (promise.returnVal === "savingFailed") {
                        swal("Failed to save record");
                    }
                    else {
                        swal("Sorry...something went wrong");
                    }
                });
            }
            else {
                $scope.submitted1 = true;
            }
        };

        $scope.edit = function (data) {
            $scope.SPCCE_Id = data;
            apiService.getURI("StudentTransaction/Edit/", $scope.SPCCE_Id).then(function (promise) {
                $scope.editDetails = promise.editDetails;
                $scope.ASMAY_Id = promise.editDetails[0].asmaY_Id;
                $scope.SPCCME_Id = promise.editDetails[0].spccmE_Id;

                $scope.SPCCMEV_Id = promise.editDetails[0].spccmeV_Id;
                $scope.SPCCE_StartDate = new Date(promise.editDetails[0].spccE_StartDate);
                $scope.SPCCE_StartTime = moment(promise.editDetails[0].spccE_StartTime, 'h:mm a').format();

                $scope.SPCCE_EndDate = new Date(promise.editDetails[0].spccE_EndDate);
                $scope.SPCCE_EndTime = moment(promise.editDetails[0].spccE_EndTime, 'h:mm a').format();
                $scope.SPCCE_Remarks = promise.editDetails[0].spccE_Remarks;
            });
        };

        $scope.deactive = function (data) {
            var obj = "";
            if (data.spccE_ActiveFlag === false) {
                obj = {
                    "SPCCE_Id": data.spccE_Id,
                    "SPCCE_ActiveFlag": true
                };
            }
            else if (data.spccE_ActiveFlag === true) {
                obj = {
                    "SPCCE_Id": data.spccE_Id,
                    "SPCCE_ActiveFlag": false
                };
            }
            apiService.create("StudentTransaction/deactivate/", obj).then(function (promise) {
                if (promise.returnVal !== '' && promise !== null) {
                    swal(promise.returnVal);
                    $state.reload();
                }
                else {
                    swal("Something went wrong");
                }
            });

        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted1 = function (field1) {
            return $scope.submitted1;
        };

        $scope.toggleAll_S = function (stas) {
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.StudentScoreData, function (itm) {
                itm.checked = toggleStatus;

            });
        };


        $scope.optionToggled_S = function () {
            $scope.checkall = $scope.StudentScoreData.every(function (itm) { return itm.checked; });
        };

    }
})();




