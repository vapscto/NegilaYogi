(function () {
    'use strict';
    angular
        .module('app')
        .controller('TrainingFeedbackController', TrainingFeedbackController)

    TrainingFeedbackController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function TrainingFeedbackController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.sortKey = 'HRTRQNS_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        //get data
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("Training_Feedback/loaddata", pageid)
                .then(function (promise) {
                    $scope.traninglisttrainer = promise.traninglisttrainer;
                    $scope.traninglisttrainee = promise.traninglisttrainee;
                });
        };

        $scope.getQuestions = function () {
            $scope.Type = "";
            $scope.mappedquestionlist = [];
            $scope.mappedoptionlist = [];
            $scope.trainerlist = [];

            var data = {
                "HRTCR_Id": $scope.hrtcR_Id,
                "Type": $scope.feedbacktype
            };
            apiService.create("Training_Feedback/getQuestions", data).
                then(function (promise) {

                    if (promise !== null) {
                        $scope.trainingFeedbacklist = promise.trainingFeedbacklist;
                        if ($scope.trainingFeedbacklist.length > 0) {
                            swal("Feedback Is Already Given");
                            $scope.temparray = [];
                            $scope.tempoptionarray = [];
                            return;
                        }
                        if ((promise.mappedquestionlist !== null && promise.mappedquestionlist.length > 0)
                            && (promise.mappedoptionlist !== null && promise.mappedoptionlist.length > 0)) {

                            $scope.tempoptionarray = [];
                            $scope.mappedquestionlist = promise.mappedquestionlist;
                            $scope.mappedoptionlist = promise.mappedoptionlist;
                            $scope.trainerlist = promise.trainerlist;
                            $scope.traineelist = promise.traineelist;

                            $scope.tempoptionarray = [];
                            angular.forEach($scope.mappedquestionlist, function (ddd) {
                                angular.forEach($scope.mappedoptionlist, function (dddd) {
                                    if (ddd.hrmfqnS_Id == dddd.hrmfqnS_Id) {
                                        $scope.tempoptionarray.push({ hrmfqnS_Id: dddd.hrmfqnS_Id, hrmfopT_Id: dddd.hrmfopT_Id, hrmfopT_OptionName: dddd.hrmfopT_OptionName });
                                    }
                                });
                            });
                            console.log($scope.tempoptionarray);
                        } else {
                            $scope.tempoptionarray = [];
                            swal("No Records Found");
                        }
                    } else {
                        $scope.tempoptionarray = [];
                        swal("Something Went Wrong Kidndly Contact Administrator");
                    }
                });
        };

        //save
        $scope.submitted = false;
        $scope.savetrainerfeedback = function () {
            $scope.question_Option = [];

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "HRTCR_Id": $scope.hrtcR_Id,
                    "HRTFEED_TrainerId": $scope.hrtfeeD_TrainerId,
                    "question_Answer": $scope.mappedquestionlist
                };
                apiService.create("Training_Feedback/savetrainerfeedback", data).
                    then(function (promise) {
                        if (promise.returnvales !== "") {
                            if (promise.returnvales == "Update") {
                                swal('Record Updated Successfully');
                            }
                            else if (promise.returnvales == "Add") {
                                swal('Record Saved Successfully');
                            }
                            else if (promise.returnvales == "false") {
                                swal('Record Not Saved/Updated successfully');
                            }
                            else if (promise.returnvales === "Duplicate") {
                                swal('Category Already Exist');
                            }
                            $state.reload();
                        }
                        else {
                            swal('Operation Failed');
                            $state.reload();
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        //save
        $scope.submitted = false;
        $scope.savetraineefeedback = function () {
            $scope.question_Option = [];
            angular.forEach($scope.question_list, function (cls) {
                if (cls.selectedd2 === true) {
                    $scope.question_Option.push(cls);
                }
            });
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "HRTCR_Id": $scope.HRTCR_Id,
                    "HRME_Id": $scope.hrmE_Id,
                    "question_Answer": $scope.mappedquestionlist
                };
                apiService.create("Training_Feedback/savetraineefeedback", data).
                    then(function (promise) {
                        if (promise.returnvales !== "") {
                            if (promise.returnvales == "Update") {
                                swal('Record Updated Successfully');
                            }
                            else if (promise.returnvales == "Add") {
                                swal('Record Saved Successfully');
                            }
                            else if (promise.returnvales == "false") {
                                swal('Record Not Saved/Updated successfully');
                            }
                            else if (promise.returnvales === "Duplicate") {
                                swal('Category Already Exist');
                            }
                            $state.reload();
                        }
                        else {
                            swal('Operation Failed');
                            $state.reload();
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.Clearid = function () {
            $state.reload();
        };
    }

})();



