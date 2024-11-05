(function () {
    'use strict';
    angular.module('app').controller('OnlineExamCollegeController', OnlineExamCollegeController)

    OnlineExamCollegeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function OnlineExamCollegeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {

        $scope.startTimer = function () {
            var countDownTime = parseInt($scope.getQdetails[0].LMSMOES_TotalDuration) * 60;
            // var countDownTime = 1 * 60;
            var count = 0;
            function countDownTimer() {
                $scope.minutes = parseInt(countDownTime / 60) % 60;
                $scope.seconds = countDownTime % 60;
                var result = ($scope.minutes < 10 ? "0" + $scope.minutes : $scope.minutes) + ":" + ($scope.seconds < 10 ? "0" + $scope.seconds : $scope.seconds);
                document.getElementById("demo").innerHTML = result;
                if (countDownTime != 0 && count == 0) {
                    countDownTime = countDownTime - 1;
                }
                else if (count == 0) {
                    count += 1;
                    clearInterval(countDownTime);
                    document.getElementById("demo").innerHTML = "TIME EXPIRED";
                    $scope.text = "End";
                    $scope.Quit();
                }
                setTimeout(function () { countDownTimer(); }, 1000);
            }
            countDownTimer();
        };




        //-------------------Load Data
        $scope.loaddata = function () {
            var pageid = 2;
            $scope.count = 0;
            $scope.subject = false;
            apiService.getURI("OnlineExamCollege/getloaddata", pageid).then(function (promise) {
                $scope.getclass = promise.getclass;
                $scope.getQdetails = promise.getQdetails;
                $scope.asmcL_Id = promise.asmcL_Id;
                $scope.Amst_ID = promise.amst_ID;
                $scope.getSubjects = promise.getSubjects;
            });
        };

        $scope.onselectclass = function () {
            var data = {
                "ASMCL_Id": $scope.asmcL_Id
            };
            apiService.create("OnlineExamCollege/getclass", data).then(function (promise) {
                $scope.getSubjects = promise.getSubjects;
            });
        };

        $scope.Submit1 = function () {
            $scope.subject = true;
        };

        $scope.Submit = function () {
            var data = {
                "ASMCL_Id": $scope.asmcL_Id,
                "ISMS_Id": $scope.ismS_Id,
            };
            apiService.create("OnlineExamCollege/getQuestion", data).then(function (promise) {

                if (promise.getQuestion.length > 0) {
                    $scope.getQuestion = promise.getQuestion;
                    $scope.startTimer();
                    $scope.test = $scope.getQuestion;
                    $scope.count = 0;
                    $scope.q_list1 = [];
                    $scope.temp_recp_list1 = [];
                    $scope.q_rpt1 = $scope.getQuestion;
                    // var temp_recp_list1 = $scope.getQuestion;

                    //for (var m = 0; m < $scope.getQuestion.length; m++) {
                    //    var LMSMOEQ_Id = $scope.getQuestion[m].lmsmoeQ_Id;
                    //    var q_name = $scope.getQuestion[m].lmsmoeQ_Question;
                    //    var already_cnt = 0;
                    //    angular.forEach($scope.q_list1, function (itm1) {
                    //        if (itm1.lmsmoeQ_Id == LMSMOEQ_Id) {
                    //            already_cnt += 1;
                    //        }
                    //    })
                    //    if (already_cnt == 0) {
                    //        $scope.q_rpt1 = [];
                    //        angular.forEach(temp_recp_list1, function (itm) {

                    //            if (itm.lmsmoeQ_Id == LMSMOEQ_Id) {
                    //                $scope.q_rpt1.push(itm);
                    //            }
                    //        })
                    //        $scope.q_list1.push({ LMSMOEQ_Id: LMSMOEQ_Id, q_name: q_name, qst_opt: $scope.q_rpt1 });
                    //    }
                    //}


                    angular.forEach($scope.getQuestion, function (e) {
                        var LMSMOEQ_Id = e.lmsmoeQ_Id;
                        var q_name = e.lmsmoeQ_Question;
                        if ($scope.q_list1.length === 0) {

                            $scope.q_list1.push({
                                LMSMOEQ_Id: LMSMOEQ_Id, q_name: q_name, qst_opt: $scope.q_rpt1

                            });
                        }
                        else if ($scope.q_list1.length > 0) {
                            var count = 0;
                            angular.forEach($scope.q_list1, function (dd) {
                                if (dd.LMSMOEQ_Id === e.lmsmoeQ_Id) {
                                    count += 1;
                                }
                            });
                            if (count === 0) {
                                $scope.q_list1.push({
                                    LMSMOEQ_Id: LMSMOEQ_Id, q_name: q_name, qst_opt: $scope.q_rpt1
                                });
                            }
                        }
                    });
                }

                else {
                    swal("No Questions Available for the Selected Subject");
                }

            });
        };

        $scope.nextqst = function () {
            $scope.count += 1;
            var pageid = 2;
            apiService.getURI("OnlineExamCollege/savedanswers", pageid).then(function (promise) {
                $scope.savedanswer = promise.savedanswer;
                var LMSMOEQ_Id = $scope.q_list1[$scope.count].LMSMOEQ_Id;
                angular.forEach($scope.savedanswer, function (itm1) {
                    if (itm1.LMSMOEQ_Id == LMSMOEQ_Id) {
                        // $scope.QuizeQuastions = itm1.LMSMOEQOA_Id;
                        $scope.q_list1[$scope.count].QuizeQuastions = itm1.LMSMOEQOA_Id;
                    }
                });
            });
        };


        $scope.prevqst = function () {
            $scope.count -= 1;
            var pageid = 2;
            apiService.getURI("OnlineExamCollege/savedanswers", pageid).then(function (promise) {
                $scope.savedanswer = promise.savedanswer;
                var LMSMOEQ_Id = $scope.q_list1[$scope.count].LMSMOEQ_Id;
                angular.forEach($scope.savedanswer, function (itm1) {
                    if (itm1.LMSMOEQ_Id == LMSMOEQ_Id) {
                        // $scope.QuizeQuastions = itm1.LMSMOEQOA_Id;
                        $scope.q_list1[$scope.count].QuizeQuastions = itm1.LMSMOEQOA_Id;
                    }
                });
            });
        };

        $scope.Saveanswer = function () {
            $scope.q_list_answrd = $scope.q_list1[$scope.count];
            var LMSMOEQOA_Id = 0;
            var QuizeQuastions = 0;
            var LMSMOEQ_Id = 0;
            var q_name = '';
            $scope.term_list = [];
            var out = $scope.q_list_answrd;

            //angular.forEach(out, function (itm) {
            //    out.LMSMOEQOA_Id = itm.LMSMOEQ_Id;
            //    out.QuizeQuastions = itm.QuizeQuastions;
            //    out.q_name = itm.q_name;
            //})

            $scope.term_list.push({ LMSMOEQ_Id: out.LMSMOEQ_Id, LMSMOEQOA_Id: out.QuizeQuastions, q_name: out.q_name });


            var data = {
                saveanswerlst: $scope.q_list1
            };
            apiService.create("OnlineExamCollege/Saveanswer", data).then(function (promise) {
                $scope.savedanswer = promise.savedanswer;

                $('#QuizQuastions').modal('hide');
                swal("Exam Is Submitted");
                $('#finalwindow').modal('show');
            });
        };


        $scope.Quit = function () {
            if ($scope.text == "End") {
                var data = {
                    "LMSSTE_TotalTime": $scope.minutes,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ISMS_Id": $scope.ismS_Id
                };
                swal({
                    title: "Are you sure?",
                    text: "Time Up !!!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("OnlineExamCollege/submitexam", data).then(function (promise) {
                                if (promise.result != "" || promise.result != null) {
                                    $scope.result = promise.result;
                                    $('#QuizQuastions').modal('hide');
                                    swal("Exam Is Submitted");
                                    $('#finalwindow').modal('show');
                                }
                            });
                        }
                        else {
                            swal("Exam Is Submitted");
                        }
                    }
                );
            }
            else {
                var confirmmgs = "";
                var data = {
                    "LMSSTE_TotalTime": $scope.minutes,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ISMS_Id": $scope.ismS_Id
                    // "ISMS_Id": $scope.ismS_Id,
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                swal({
                    title: "Are you sure?",
                    text: "Do You Want To Quit the Test, If yes only the Saved Answers will be Considered For Test?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            apiService.create("OnlineExamCollege/submitexam", data).then(function (promise) {
                                if (promise.result.length > 0) {
                                    $scope.LMSSTE_TotalQnsAnswered = promise.result[0].LMSSTE_TotalQnsAnswered;
                                    $scope.LMSSTE_TotalCorrectAns = promise.result[0].LMSSTE_TotalCorrectAns;
                                    $scope.LMSSTE_TotalMaxMarks = promise.result[0].LMSSTE_TotalMaxMarks;
                                    $scope.LMSSTE_TotalMarks = promise.result[0].LMSSTE_TotalMarks;
                                    $scope.LMSSTE_Percentage = promise.result[0].LMSSTE_Percentage;
                                    $scope.LMSSTE_TotalTime = promise.result[0].LMSSTE_TotalTime;
                                    //$scope.LMSSTE_TotalQnsAnswered = promise.result[0].LMSSTE_TotalQnsAnswered;

                                    $('#QuizQuastions').modal('hide');
                                    swal("Exam Is Submitted");
                                    $('#finalwindow').modal('show');
                                }
                                else {
                                    $('#QuizQuastions').modal('hide');
                                    swal("Exam Is Submitted");
                                }

                            });
                        }
                        else {
                            swal("Exam Is Submitted");
                        }
                    });
            }
        };


        $scope.Ok = function () {
            $('#finalwindow').modal('hide');
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.cancel1 = function () {
            $state.reload();
        };
    }


})();