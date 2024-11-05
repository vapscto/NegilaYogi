(function () {
    'use strict';
    angular.module('app').controller('OnlineExamController', OnlineExamController)

    OnlineExamController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function OnlineExamController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {

        $scope.startTimer = function () {
            // var countDownTime = parseInt($scope.getQdetails[0].LMSMOES_TotalDuration) * 60;
            var countDownTime = 1 * 60;
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

        $scope.startTimeroverall = function () {
            countDownTimeoverall = 0;
            $scope.minutesoverall = 0;
            $scope.secondsoverall = 0;
            var result = "";
            document.getElementById("demooverall").innerHTML = "";

            var countDownTimeoverall = parseInt($scope.timeforoverallquestion);
            countDownTimeoverall = countDownTimeoverall * 60;
            var countoverall = 0;
            clearInterval(countDownTimeoverall);
            function countDownTimeroverall() {
                $scope.minutesoverall = parseInt(countDownTimeoverall / 60) % 60;
                $scope.secondsoverall = countDownTimeoverall % 60;
                result = ($scope.minutesoverall < 10 ? "0" + $scope.minutesoverall : $scope.minutesoverall) + ":" + ($scope.secondsoverall < 10 ? "0" + $scope.secondsoverall : $scope.secondsoverall);
                document.getElementById("demooverall").innerHTML = "Total Time Left :" + result;
                if (countDownTimeoverall !== 0 && countoverall === 0) {
                    countDownTimeoverall = countDownTimeoverall - 1;
                }
                else if (countoverall === 0) {
                    countoverall += 1;
                    clearInterval(countDownTimeoverall);
                    document.getElementById("demooverall").innerHTML = "TIME EXPIRED";
                    $scope.textoverall = "End";
                    $scope.Quit();
                }
                setTimeout(function () { countDownTimeroverall(); }, 1000);
            }
            countDownTimeroverall();
        };

        //-------------------Load Data
        $scope.loaddata = function () {
            var pageid = 2;
            $scope.count = 0;
            $scope.subject = false;
            apiService.getURI("OnlineExam/getloaddata", pageid).then(function (promise) {
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
            apiService.create("OnlineExam/getclass", data).then(function (promise) {
                $scope.getSubjects = promise.getSubjects;
            });
        };

        $scope.Submit1 = function () {
            $scope.subject = true;
        };

        $scope.Submit = function () {
            var data = {
                "ASMCL_Id": $scope.asmcL_Id,
                "ISMS_Id": $scope.ismS_Id
            };
            apiService.create("OnlineExam/getQuestion", data).then(function (promise) {

                if (promise.getQuestion.length > 0) {
                    $scope.getQuestion = promise.getQuestion;
                    $scope.startTimer();
                    $scope.test = $scope.getQuestion;
                    $scope.count = 0;
                    $scope.q_list1 = [];
                    $scope.temp_recp_list1 = [];
                    $scope.q_rpt1 = $scope.getQuestion;

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

                    $scope.tempdoc = [];
                    $scope.getQuestiondocuments = promise.getQuestiondocuments;

                    angular.forEach($scope.q_list1, function (dd) {
                        $scope.tempdoc = [];
                        angular.forEach($scope.getQuestiondocuments, function (d) {
                            if (dd.LMSMOEQ_Id === d.lmsmoeQ_Id) {
                                $scope.tempdoc.push(d);
                            }
                        });
                        dd.files = $scope.tempdoc;
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
            apiService.getURI("OnlineExam/savedanswers", pageid).then(function (promise) {
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
            apiService.getURI("OnlineExam/savedanswers", pageid).then(function (promise) {
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
            $scope.term_list.push({ LMSMOEQ_Id: out.LMSMOEQ_Id, LMSMOEQOA_Id: out.QuizeQuastions, QuizeQuastions: out.QuizeQuastions, q_name: out.q_name });
            var data = {
                saveanswerlst: $scope.q_list1
            };
            apiService.create("OnlineExam/Saveanswer", data).then(function (promise) {
                $scope.savedanswer = promise.savedanswer;

                $scope.count += 1;
                var LMSMOEQ_Id = $scope.q_list1[$scope.count].LMSMOEQ_Id;
                angular.forEach($scope.savedanswer, function (itm1) {
                    if (itm1.lmsmoeQ_Id === LMSMOEQ_Id) {                       
                        $scope.q_list1[$scope.count].QuizeQuastions = itm1.pamoeqoA_Id;
                    }
                });
                //$scope.startTimer();
                $scope.submitted3 = false;
                //$scope.nextqst();
                //$('#QuizQuastions').modal('hide');
                //swal("Exam Is Submitted");               
            });
        };

        $scope.Quit = function () {
            var data = "";
            if ($scope.textoverall === "End") {
                data = {
                    "LMSSTE_TotalTime": $scope.minutesoverall,
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
                            apiService.create("OnlineExam/submitexam", data).then(function (promise) {
                                if (promise.result !== "" || promise.result !== null) {
                                    $scope.result = promise.result;
                                    $('#QuizQuastions').modal('hide');
                                    swal("Exam Is Submitted");
                                    //$('#finalwindow').modal('show');
                                }
                            });
                        }
                        else {
                            swal("Exam Is Submitted");
                        }
                    });
            }
            else {
                var confirmmgs = "";
                data = {
                    "LMSSTE_TotalTime": $scope.minutesoverall,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ISMS_Id": $scope.ismS_Id
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
                            apiService.create("OnlineExam/submitexam", data).then(function (promise) {
                                if (promise.result.length > 0) {
                                    $scope.LMSSTE_TotalQnsAnswered = promise.result[0].LMSSTE_TotalQnsAnswered;
                                    $scope.LMSSTE_TotalCorrectAns = promise.result[0].LMSSTE_TotalCorrectAns;
                                    $scope.LMSSTE_TotalMaxMarks = promise.result[0].LMSSTE_TotalMaxMarks;
                                    $scope.LMSSTE_TotalMarks = promise.result[0].LMSSTE_TotalMarks;
                                    $scope.LMSSTE_Percentage = promise.result[0].LMSSTE_Percentage;
                                    $scope.LMSSTE_TotalTime = promise.result[0].LMSSTE_TotalTime;
                                    $('#QuizQuastions').modal('hide');
                                    swal("Exam Is Submitted");
                                    //$('#finalwindow').modal('show');
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