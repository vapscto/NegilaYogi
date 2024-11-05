(function () {
    'use strict';
    angular.module('app').controller('LP_OnlineStudentExamController', LP_OnlineStudentExamController)

    LP_OnlineStudentExamController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter', '$sce', '$q', '$window']
    function LP_OnlineStudentExamController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter, $sce, $q, $window) {

        $scope.submitted = false;
        $scope.submitted3 = false;
        $scope.btnsave = false;
        $scope.obj = {};
        var t;
        var t1;
        $scope.teacherdocuupload = {};

        $scope.start = true;
        $scope.startexam = false;

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        $scope.examstart_time = false;

        $('.modal').on('hide.bs.modal', function (e) {            e.stopPropagation();            $('body').css('padding-right', '');        });

        // Auto Refresh Page For Every 15 Sec
        var stopped;
        $scope.countdown = function () {
            stopped = $timeout(function () {
                if ($scope.examstart_time === false) {
                    var pageurl = window.location.href;
                    var res = pageurl.split("/");
                    if (res.indexOf("LP_OnlineStudentExam") !== -1) {
                        $scope.Submit1();
                    }
                }
            }, 15000);
        };

        //document.addEventListener('contextmenu', event => event.preventDefault());
        //$(document).keydown(function (event) {
        //    if (event.keyCode == 27) {
        //        return false;
        //    }
        //    else if (event.keyCode == 27) {
        //        return false;
        //    }
        //    else if (event.keyCode == 91) {
        //        return false;
        //    }
        //    else if (event.keyCode == 122) {
        //        return false;
        //    }
        //    else if (event.keyCode == 16 || event.keyCode == 17 || event.keyCode == 18) {
        //        return false;
        //    }
        //    else if (event.ctrlKey && event.shiftKey && event.keyCode == 73) {
        //        return false;
        //    }
        //    else if (event.ctrlKey && event.shiftKey && event.keyCode == 'I'.charCodeAt(0)) {
        //        return false;
        //    }
        //    else if (event.ctrlKey && event.shiftKey && event.keyCode == 'J'.charCodeAt(0)) {
        //        return false;
        //    }
        //    else if (event.ctrlKey && event.keyCode == 'U'.charCodeAt(0)) {
        //        return false;
        //    }
        //});

        //window.onbeforeunload = function () {
        //    return "Your form progress will be lost, are you sure you want to reload this page?";
        //}

        $scope.startTimer = function () {
            var countDownTime = 0;
            $scope.minutes = 0;
            $scope.seconds = 0;
            var result = "";
            clearInterval(countDownTime);
            document.getElementById("demo").innerHTML = "";
            countDownTime = parseInt($scope.timeforeachquestion);
            countDownTime = countDownTime * 60;
            var count = 0;
            function countDownTimer() {
                $scope.minutes = parseInt(countDownTime / 60) % 60;
                $scope.seconds = countDownTime % 60;
                result = ($scope.minutes < 10 ? "0" + $scope.minutes : $scope.minutes) + ":" + ($scope.seconds < 10 ? "0" + $scope.seconds : $scope.seconds);
                document.getElementById("demo").innerHTML = result;
                if (countDownTime !== 0 && count === 0) {
                    countDownTime = countDownTime - 1;
                }
                else if (count === 0) {
                    count += 1;
                    clearInterval(countDownTime);
                    document.getElementById("demo").innerHTML = "TIME EXPIRED";
                    $scope.text = "End";
                }
                setTimeout(function () { countDownTimer(); }, 1000);
            }
            countDownTimer();
        };

        // Down Timer from given time
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

                $scope.secondsoverall = countDownTimeoverall % 60; // Seconds that cannot be written in minutes
                var secondsInMinutes = (countDownTimeoverall - $scope.secondsoverall) / 60; // Gives the seconds that COULD be given in minutes
                $scope.minutesoverall = secondsInMinutes % 60; // Minutes that cannot be written in hours
                $scope.hoursoverall = (secondsInMinutes - $scope.minutesoverall) / 60;

                result = ($scope.hoursoverall < 10 ? "0" + $scope.hoursoverall : $scope.hoursoverall) + ":" +
                    ($scope.minutesoverall < 10 ? "0" + $scope.minutesoverall : $scope.minutesoverall) + ":" + ($scope.secondsoverall < 10 ? "0" + $scope.secondsoverall : $scope.secondsoverall);

                document.getElementById("demooverall").innerHTML = "Time left: " + result;
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
                t = setTimeout(function () { countDownTimeroverall(); }, 1000);
            }
            countDownTimeroverall();
        };

        // Up Timer from 0
        $scope.UpstartTimeroverall = function () {
            UpcountDownTimeoverall = 0;
            $scope.Upminutesoverall = 0;
            $scope.Upsecondsoverall = 0;
            var Upresult = "";
            document.getElementById("Updemooverall").innerHTML = "";
            var UpcountDownTimeoverall = 0;
            UpcountDownTimeoverall = UpcountDownTimeoverall * 60;
            var Upcountoverall = 0;
            var minutesLabel = "";
            var hourslabel = "";
            var secondsLabel = "";
            var totalSeconds = 0;
            $scope.Upresultd = "";
            document.getElementById("Updemooverall").innerHTML = "";

            function setTime() {
                ++totalSeconds;
                secondsLabel = pad(totalSeconds % 60);
                minutesLabel = pad(parseInt(totalSeconds / 60));
                hourslabel = pad(parseInt(totalSeconds / (60 * 60)));
                $scope.Upresultd = hourslabel + ":" + minutesLabel + ":" + secondsLabel;
                if (minutesLabel === $scope.timeforoverallquestion) {
                    clearInterval(totalSeconds);
                }
                t1 = setTimeout(function () { setTime(); }, 1000);
            }
            setTime();
            function pad(val) {
                var valString = val + "";
                if (valString.length < 2) {
                    return "0" + valString;
                }
                else {
                    return valString;
                }
            }
        };

        $scope.examsubmittedlist = [];
        $scope.examnotsubmittedlist = [];
        $scope.examtodayslist = [];
        $scope.examnotstartedlist = [];
        $scope.html = "";

        //function handleVisibilityChange() {            
        //    if (document.hidden) {
        //        //window.location.hash = "";
        //        $scope.alertdata();
        //    }
        //}     

        //document.addEventListener("visibilitychange", handleVisibilityChange, true);

        //var alertcnt = 0;
        //$scope.alertdata = function () {
        //    alertcnt += 1;
        //    alert("You are Not allowed to open new tab while writing online exam!!!!!");
        //    if (alertcnt === 3) {
        //        alert("Exam Submited");
        //    }
        //};

        //-------------------Load Data 
        $scope.Submit1 = function () {
            var pageid = 2;
            $scope.count = 0;
            $scope.subject = false;
            apiService.getURI("LP_OnlineStudentExam/getloaddata", pageid).then(function (promise) {
                if (promise !== null) {
                    if (promise.message === null || promise.message === "") {
                        $scope.getsubjectdetails = promise.getsubjectdetails;
                        $scope.gettodaysexamdetails = promise.gettodaysexamdetails;
                        $scope.getallexamdetails = promise.getallexamdetails;
                        $scope.getexamcompleteddetails = promise.getexamcompleteddetails;
                        $scope.getexamsubmitteddetails = promise.getexamsubmitteddetails;
                        var examdateflag = 0;
                        angular.forEach($scope.getallexamdetails, function (dd) {
                            examdateflag = 0;
                            dd.allowdowndolad_quespaper_flag = 0;
                            angular.forEach($scope.gettodaysexamdetails, function (d) {
                                if (dd.lpmoeeX_Id === d.lpmoeeX_Id) {
                                    examdateflag = 1;
                                }
                            });
                            dd.examdateflag = examdateflag;
                        });

                        angular.forEach($scope.getallexamdetails, function (dd) {
                            angular.forEach($scope.getexamsubmitteddetails, function (d) {
                                if (dd.lpmoeeX_Id === d.lpmoeeX_Id) {
                                    dd.examdateflag = -2;
                                    dd.viewmarkscount = d.viewmarkscount;
                                    dd.viewmarkscountnew = d.viewmarkscountnew;
                                }
                            });
                        });

                        angular.forEach($scope.getallexamdetails, function (dd) {
                            angular.forEach($scope.getexamcompleteddetails, function (d) {
                                if (dd.lpmoeeX_Id === d.lpmoeeX_Id) {
                                    dd.examdateflag = -1;
                                }
                            });
                        });

                        $scope.examsubmittedlist = [];
                        $scope.examnotsubmittedlist = [];
                        $scope.examtodayslist = [];
                        $scope.examnotstartedlist = [];

                        angular.forEach($scope.getallexamdetails, function (dd) {
                            if (dd.examdateflag === 0) {

                                $scope.GetDateTime = new Date();
                                $scope.ExamStartDate = new Date(dd.examStartDateTime);

                                if (dd.lpmoeeX_AllowDownloadQnsPaperBeforeExamFlg) {
                                    $scope.DurationTime = dd.lpmoeeX_Duration;
                                    if (dd.lpmoeeX_DurationFlag === "Mins") {
                                        var diff_mins = ($scope.ExamStartDate.getTime() - $scope.GetDateTime.getTime()) / 1000;
                                        diff_mins /= 60; //Mins
                                        var diffmins = Math.abs(Math.round(diff_mins));
                                        if (Number(diffmins) <= Number($scope.DurationTime)) {
                                            dd.allowdowndolad_quespaper_flag = 1;
                                        }
                                    } else if (dd.lpmoeeX_DurationFlag === "Hours") {
                                        var diff_mins = ($scope.ExamStartDate.getTime() - $scope.GetDateTime.getTime()) / 1000;
                                        diff_mins /= (60); //Mins
                                        var diffmins = Math.abs(Math.round(diff_mins));
                                        var Duration = Number($scope.DurationTime) * 60; // Duration Hours Converting To Mins
                                        if (Number(diffmins) <= Number(Duration)) {
                                            dd.allowdowndolad_quespaper_flag = 1;
                                        }
                                    } else if (dd.lpmoeeX_DurationFlag === "Days") {
                                        var Duration = 1440 * Number($scope.DurationTime);  // Converting Days  Into Mins                                        
                                        var diff_mins = ($scope.ExamStartDate.getTime() - $scope.GetDateTime.getTime()) / 1000;
                                        diff_mins /= (60); //Mins
                                        var diffmins = Math.abs(Math.round(diff_mins));
                                        if (Number(diffmins) <= Number(Duration)) {
                                            dd.allowdowndolad_quespaper_flag = 1;
                                        }
                                    }
                                }
                                $scope.examnotstartedlist.push(dd);
                            }

                            else if (dd.examdateflag === 1) {
                                $scope.examtodayslist.push(dd);
                            }

                            else if (dd.examdateflag === -1) {
                                $scope.examnotsubmittedlist.push(dd);
                            }

                            else if (dd.examdateflag === -2) {
                                $scope.examsubmittedlist.push(dd);
                            }
                        });

                        $scope.subject = true;
                        if ($scope.getsubjectdetails === null || $scope.getsubjectdetails.length === 0) {
                            $scope.subject = false;
                            swal("Subject Are Not Mapped, Kindly Contact Administrator");
                        } else {
                            $scope.examstart_time = false;
                            $scope.countdown();
                        }
                    } else {
                        $scope.subject = false;
                        swal("Student Details Not Found, Kindly Contact Administrator");
                    }
                }
            });
        };


        var elem = "";

        function onPaste(e) {
            kendo.htmlEncode(e.html);
        }

        function editor_change() {
            $scope.datass = this.value();
            document.getElementById("deff").value = $scope.datass;
        }

        $scope.onselectsubject = function () {
            var data = {
                "ISMS_Id": $scope.ISMS_Id
            };
            apiService.create("LP_OnlineStudentExam/onselectsubject", data).then(function (promise) {
                $scope.getexamlist = promise.getexamlist;
                if ($scope.getexamlist === null || $scope.getexamlist.length === 0) {
                    swal("No Exam Is Mapped For Selected Subject, Kindly Contact Administrator");
                }
            });
        };

        $scope.loaddata = function () {
            $scope.subject = false;
        };

        // Click Function For To Start Exam
        $scope.Submit = function (details) {

            $scope.ISMS_Id = details.ismS_Id;
            $scope.LPMOEEX_Id = details.lpmoeeX_Id;

            $scope.examdetailsstudent = "";
            $scope.examdetailsstudent = "Exam : " + details.lpmoeeX_ExamName;

            var data = {
                "LPMOEEX_Id": details.lpmoeeX_Id,
                "ISMS_Id": details.ismS_Id
            };
            apiService.create("LP_OnlineStudentExam/getQuestion", data).then(function (promise) {

                $scope.getexamdetails = promise.getexamdetails;

                if ($scope.getexamdetails !== null && $scope.getexamdetails.length > 0) {
                    $scope.examstart_time = true;
                    $scope.examflag = $scope.getexamdetails[0].lpmoeeX_UploadExamPaperFlg;

                    if ($scope.getexamdetails[0].lpmoeeX_UploadExamPaperFlg === false) {
                        $scope.start = false;
                        $scope.subject = false;

                        if (promise.getexamleveldetails !== null && promise.getexamleveldetails.length > 0 &&
                            promise.getexamquestionlist !== null && promise.getexamquestionlist.length > 0) {

                            if (promise.message === "Time Crossed") {
                                //swal("You Can Not Take Exam Now Already Time Crossed To Start Exam");
                                //return;
                            }

                            elem = document.documentElement;
                            var methodToBeInvoked = elem.requestFullscreen || elem.webkitRequestFullScreen || elem['mozRequestFullscreen']
                                || elem['msRequestFullscreen'];
                            if (methodToBeInvoked) methodToBeInvoked.call(elem);

                            $scope.getQuestion = promise.getexamquestionlist;
                            $scope.getexamleveldetails = promise.getexamleveldetails;

                            $scope.getconnfig = promise.getconnfig;
                            $scope.getQuestiondocuments = promise.getquestiondoclist;
                            $scope.timeforoverallquestion = details.lpmoeeX_ExamDuration;
                            $scope.getquestionoptionlist = promise.getquestionoptionlist;
                            $scope.getquestionmfoptionlist = promise.getquestionmfoptionlist;

                            // Level Wise Questions
                            angular.forEach($scope.getexamleveldetails, function (dd) {
                                $scope.tempquestions = [];
                                angular.forEach($scope.getQuestion, function (d) {
                                    if (dd.lpmoeexlvL_Id === d.lpmoeexlvL_Id) {
                                        $scope.tempquestions.push({
                                            lpmoeQ_Id: d.lpmoeQ_Id, lpmoeQ_Question: d.lpmoeQ_Question,
                                            lpmoeQ_SubjectiveFlg: d.lpmoeQ_SubjectiveFlg,
                                            lpmoeQ_MatchTheFollowingFlg: d.lpmoeQ_MatchTheFollowingFlg,
                                            lpmoeeX_AnswerPapeFileName: d.lpmoeeX_AnswerPapeFileName,
                                            lpmoeexlvL_Id: d.lpmoeexlvL_Id, lpmoeexqnS_Id: d.lpmoeexqnS_Id,
                                            lpmoeexqnS_QnsOrder: d.lpmoeexqnS_QnsOrder,
                                            teacherdocuupload_subjective: [{ id: 'Teacher_subjective1' }]

                                        });
                                    }
                                });
                                dd.questions = $scope.tempquestions;
                            });

                            // Level Wise Questions And Options Mapping
                            angular.forEach($scope.getexamleveldetails, function (dd) {
                                angular.forEach(dd.questions, function (d) {
                                    $scope.qst_opttemp = [];
                                    $scope.Temp_Distinct_cols = [];
                                    angular.forEach($scope.getquestionoptionlist, function (o) {
                                        if (o.lpmoeQ_Id === d.lpmoeQ_Id) {
                                            $scope.qst_opttemp.push({
                                                LPMOEQOA_Id: o.lpmoeqoA_Id, LPMOEQOA_Option: o.lpmoeqoA_Option, LPMOEQ_Id: o.lpmoeQ_Id,
                                                LPMOEQOA_OptionCode: o.lpmoeqoA_OptionCode, LPMOEQOA_AnswerFlag: o.lpmoeqoA_AnswerFlag
                                            });
                                        }
                                    });

                                    $scope.Temp_array = [];
                                    if (d.lpmoeQ_MatchTheFollowingFlg) {
                                        angular.forEach($scope.qst_opttemp, function (opts) {
                                            $scope.Temp_array = [];
                                            angular.forEach($scope.getquestionmfoptionlist, function (mf_opts) {
                                                if (opts.LPMOEQOA_Id === mf_opts.lpmoeqoA_Id && mf_opts.lpmoeQ_Id === opts.LPMOEQ_Id) {
                                                    $scope.Temp_array.push({
                                                        lpmoeQ_Id: mf_opts.lpmoeQ_Id, lpmoeqoA_Id: mf_opts.lpmoeqoA_Id,
                                                        lpmoeqoamF_Id: mf_opts.lpmoeqoamF_Id, lpmoeqoamF_MatchtheFollowing: mf_opts.lpmoeqoamF_MatchtheFollowing,
                                                        lpmoeqoA_AnswerFlag: mf_opts.lpmoeqoA_AnswerFlag,
                                                        lpmoeqoamF_AnswerFlag: mf_opts.lpmoeqoamF_AnswerFlag,
                                                    });
                                                    $scope.Temp_Distinct_cols.push(mf_opts);
                                                }
                                            });
                                            opts.cols_rows_array = $scope.Temp_array;
                                        });
                                    }

                                    $scope.Temp_Distinctcols = [];
                                    if ($scope.Temp_Distinct_cols.length > 0) {
                                        $scope.Temp_Distinctcols = $scope.Temp_Distinct_cols.filter((item, i, arr) => arr.findIndex((t) => t.lpmoeqoamF_MatchtheFollowing === item.lpmoeqoamF_MatchtheFollowing) === i);

                                        $scope.colms_array = [];
                                        angular.forEach($scope.Temp_Distinct_cols, function (dd, i) {
                                            $scope.colms_array.push({ id: i, colname: dd.lpmoeqoamF_MatchtheFollowing });
                                        });
                                    }
                                    d.qst_opt = $scope.qst_opttemp;
                                    d.colms_array = $scope.Temp_Distinctcols;
                                });
                            });

                            $scope.tempdoc = [];

                            angular.forEach($scope.getexamleveldetails, function (dd) {
                                $scope.tempdoc = [];
                                angular.forEach(dd.questions, function (d) {
                                    $scope.tempdoc = [];
                                    angular.forEach($scope.getQuestiondocuments, function (o) {
                                        if (o.lpmoeQ_Id === d.lpmoeQ_Id) {
                                            $scope.tempdoc.push(o);
                                        }
                                    });
                                    d.files = $scope.tempdoc;
                                });
                            });


                            $scope.tempdocoptions = [];
                            $scope.getoptionwisefiles = promise.getoptionwisefiles;

                            angular.forEach($scope.getexamleveldetails, function (dd) {
                                angular.forEach(dd.questions, function (d) {
                                    $scope.tempdocoptions = [];
                                    if (d.lpmoeQ_SubjectiveFlg === false) {
                                        angular.forEach(d.qst_opt, function (of) {
                                            $scope.tempdocoptions = [];
                                            angular.forEach($scope.getoptionwisefiles, function (o) {
                                                if (o.lpmoeqoA_Id === of.LPMOEQOA_Id) {
                                                    $scope.tempdocoptions.push(o);
                                                }
                                            });
                                            of.opt_files = $scope.tempdocoptions;
                                        });
                                    }
                                });
                            });


                            angular.forEach($scope.getexamleveldetails, function (dd) {
                                angular.forEach(dd.questions, function (dd, index) {
                                    $scope.opeditstrimg3 = "";
                                    dd.imgOptionCode = "";
                                    if (dd.lpmoeQ_Question != null && dd.lpmoeQ_Question != '') {
                                        var opsplitstredit = dd.lpmoeQ_Question.split(' ');
                                        $scope.opeditstrimg1 = opsplitstredit[3];
                                        if ($scope.opeditstrimg1 != undefined) {
                                            var opeditstrimg2 = $scope.opeditstrimg1.split('"');
                                            $scope.opeditstrimg3 = opeditstrimg2[1];
                                        }
                                        else {
                                            $scope.opeditstrimg3 = undefined;
                                        }
                                    }
                                    if ($scope.opeditstrimg3 != undefined) {
                                        dd.imgOptionCode = $scope.opeditstrimg3;
                                    }
                                });
                            });

                            angular.forEach($scope.getexamleveldetails, function (dd) {
                                angular.forEach(dd.questions, function (d) {
                                    angular.forEach(d.qst_opt, function (of) {
                                        $scope.opeditstrimgans = "";
                                        of.imganswer = "";
                                        if (of.LPMOEQOA_OptionCode != null && of.LPMOEQOA_OptionCode != '') {
                                            var opsplitstreditans = of.LPMOEQOA_OptionCode.split(' ');
                                            $scope.opeditstrimgans = opsplitstreditans[3];
                                            if ($scope.opeditstrimgans != undefined) {
                                                var opeditstrimgans = $scope.opeditstrimgans.split('"');
                                                $scope.opeditstrimgans = opeditstrimgans[1];
                                            }
                                            else {
                                                $scope.opeditstrimgans = undefined;
                                            }
                                        }
                                        if ($scope.opeditstrimgans != undefined) {
                                            of.imganswer = $scope.opeditstrimgans;
                                        }
                                    });
                                });
                            });

                            /* Commented Code For To Display Next next Type of questions */
                            //$scope.test = $scope.getQuestion;
                            //$scope.count = 0;
                            //$scope.q_list1 = [];
                            //$scope.temp_recp_list1 = [];
                            //$scope.q_rpt1 = $scope.getQuestion;

                            //$scope.getquestionoptionlist = promise.getquestionoptionlist;

                            //angular.forEach($scope.q_rpt1, function (dd, index) {
                            //    if (index === 0) {
                            //        dd.class = "current";
                            //    }

                            //    $scope.qst_opttemp = [];
                            //    angular.forEach($scope.getquestionoptionlist, function (d) {
                            //        if (dd.lpmoeQ_Id === d.lpmoeQ_Id) {
                            //            $scope.qst_opttemp.push({
                            //                LPMOEQOA_Id: d.lpmoeqoA_Id, LPMOEQOA_Option: d.lpmoeqoA_Option, LPMOEQ_Id: d.lpmoeQ_Id,
                            //                LPMOEQOA_OptionCode: d.lpmoeqoA_OptionCode, LPMOEQOA_AnswerFlag: d.lpmoeqoA_AnswerFlag
                            //            });
                            //        }
                            //    });
                            //    dd.qst_opt = $scope.qst_opttemp;
                            //});

                            //$scope.tempdoc = [];

                            //angular.forEach($scope.q_rpt1, function (dd) {
                            //    $scope.tempdoc = [];
                            //    angular.forEach($scope.getQuestiondocuments, function (d) {
                            //        if (dd.lpmoeQ_Id === d.lpmoeQ_Id) {
                            //            $scope.tempdoc.push(d);
                            //        }
                            //    });
                            //    dd.files = $scope.tempdoc;
                            //});
                            //console.log($scope.q_rpt1);
                            console.log($scope.getexamleveldetails);

                            $scope.getstudentlist = promise.getstudentlist;
                            $scope.studentname = $scope.getstudentlist[0].studentname;
                            $scope.classname = $scope.getstudentlist[0].asmcL_ClassName;
                            $scope.sectionname = $scope.getstudentlist[0].asmC_SectionName;
                            $scope.subjectname = details.ismS_SubjectName;
                            $scope.totalmaxmarks = details.lpmoeeX_TotalMarks;

                            $scope.html = "";
                            $scope.startTimeroverall();
                            $scope.UpstartTimeroverall();
                            $scope.startexam = true;
                        }
                        else {
                            swal("No Questions Available for the Selected Subject");
                        }
                    }

                    else {
                        $scope.start = false;
                        $scope.subject = false;
                        $scope.timeforoverallquestion = details.lpmoeeX_ExamDuration;
                        $scope.LPMOEEX_AnswerPapeFileNameView = $scope.getexamdetails[0].lpmoeeX_AnswerPapeFileName;
                        $scope.LPMOEEX_AnswerSheetView = $scope.getexamdetails[0].lpmoeeX_AnswerSheet;
                        $scope.LPMOEEX_QuestionPapeFileNameView = $scope.getexamdetails[0].lpmoeeX_QuestionPapeFileName;
                        $scope.LPMOEEX_QuestionPaperView = $scope.getexamdetails[0].lpmoeeX_QuestionPaper;

                        if ($scope.LPMOEEX_QuestionPaperView !== null && $scope.LPMOEEX_QuestionPaperView !== "") {
                            var img = $scope.LPMOEEX_QuestionPaperView;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            $scope.quesfiletypeview = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                $scope.quesdocument_Pathnewview = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.LPMOEEX_QuestionPaperView;
                            }
                        }

                        $scope.getstudentlist = promise.getstudentlist;
                        $scope.studentname = $scope.getstudentlist[0].studentname;
                        $scope.classname = $scope.getstudentlist[0].asmcL_ClassName;
                        $scope.sectionname = $scope.getstudentlist[0].asmC_SectionName;
                        $scope.subjectname = details.ismS_SubjectName;
                        $scope.totalmaxmarks = details.lpmoeeX_TotalMarks;
                        $scope.teacherdocuupload = [];
                        $scope.teacherdocuupload = [{ id: 'Teacher1' }];

                        $scope.teacherdocuupload_subjective = [];
                        $scope.teacherdocuupload_subjective = [{ id: 'Teacher_subjective1' }];

                        $scope.startTimeroverall();
                        $scope.UpstartTimeroverall();
                        $scope.startexam = true;
                    }
                }
            });
        };

        $scope.nextqst = function () {
            $scope.submitted3 = false;
            $scope.count += 1;
            var pageid = 2;
            apiService.getURI("LP_OnlineStudentExam/savedanswers", pageid).then(function (promise) {
                $scope.savedanswer = promise.savedanswer;
                var PAMOEQ_Id = $scope.q_list1[$scope.count].PAMOEQ_Id;
                angular.forEach($scope.savedanswer, function (itm1) {
                    if (itm1.pamoeQ_Id === PAMOEQ_Id) {
                        // $scope.QuizeQuastions = itm1.LMSMOEQOA_Id;
                        $scope.q_list1[$scope.count].QuizeQuastions = itm1.pamoeqoA_Id;
                    }
                });
                //$scope.startTimer();
            });
        };

        // Click Function For Skip
        $scope.skipquestion = function () {
            $scope.submitted3 = false;
            $scope.count += 1;

            var data = {
                "LPMOEEX_Id": $scope.LPMOEEX_Id,
                "ISMS_Id": $scope.ISMS_Id
            };

            $scope.q_list_answrd = $scope.q_rpt1[$scope.count];
            var out = $scope.q_list_answrd;
            $scope.q_rpt1[$scope.count].class = "skipped";
            if (out.QuizeQuastions !== undefined && out.QuizeQuastions !== null) {
                $scope.q_rpt1[$scope.count].class = "answered";
            } else if (out.answeredques !== undefined && out.answeredques !== null) {
                $scope.q_rpt1[$scope.count].class = "answered";
            } else {
                $scope.q_rpt1[$scope.count].class = "correct";
            }
            $scope.btnsave = false;
        };

        // Click Function For Previous 
        $scope.prevqst = function () {
            $scope.submitted3 = false;
            $scope.count -= 1;
            $scope.q_list_answrd = $scope.q_rpt1[$scope.count];
            var out = $scope.q_list_answrd;
            $scope.q_rpt1[$scope.count].class = "skipped";
            if (out.QuizeQuastions !== undefined && out.QuizeQuastions !== null) {
                $scope.q_rpt1[$scope.count].class = "answered";
            } else if (out.answeredques !== undefined && out.answeredques !== null) {
                $scope.q_rpt1[$scope.count].class = "answered";
            } else {
                $scope.q_rpt1[$scope.count].class = "correct";
            }
            $scope.btnsave = false;
            //$scope.count -= 1;
            var pageid = 2;
            var data = {
                "LPMOEEX_Id": $scope.LPMOEEX_Id,
                "ISMS_Id": $scope.ISMS_Id
            };

            //apiService.create("LP_OnlineStudentExam/savedanswers", data).then(function (promise) {
            //    $scope.btnsave = false;
            //    $scope.getsavedanswer = promise.getsavedanswer;
            //    var lpmoeQ_Id = $scope.q_rpt1[$scope.count].lpmoeQ_Id;
            //    var subjectiveflag = $scope.q_rpt1[$scope.count].lpmoeQ_SubjectiveFlg;
            //    if (subjectiveflag === false) {
            //        angular.forEach($scope.getsavedanswer, function (itm1) {
            //            if (itm1.lpmoeQ_Id === lpmoeQ_Id) {
            //                $scope.q_rpt1[$scope.count].QuizeQuastions = itm1.lpmoeqoA_Id;
            //                $scope.q_rpt1[$scope.count].class = "correct";
            //            }
            //        });
            //    } else {
            //        $scope.q_rpt1[$scope.count].answeredques = itm1.lpmoeqoA_Id;
            //        $scope.q_rpt1[$scope.count].class = "correct";
            //    }
            //});
        };

        // Click Function For Save And Next
        $scope.Saveanswer = function (objuser) {
            if (objuser.$valid) {
                $scope.q_list_answrd = $scope.q_rpt1[$scope.count];
                var LMSMOEQOA_Id = 0;
                var QuizeQuastions = 0;
                var PAMOEQ_Id = 0;
                var q_name = '';
                $scope.term_list = [];
                var out = $scope.q_list_answrd;
                var data = "";
                $scope.q_rpt1[$scope.count].class = "answerd";
                data = {
                    "LPSTUEX_TotalTime": $scope.Upresultd,
                    "ISMS_Id": $scope.ISMS_Id,
                    "LPMOEEX_Id": $scope.LPMOEEX_Id,
                    "LPMOEQ_SubjectiveFlg": out.lpmoeQ_SubjectiveFlg,
                    "LPMOEQ_Id": out.lpmoeQ_Id
                };

                if (out.lpmoeQ_SubjectiveFlg === false) {

                    angular.forEach(out.qst_opt, function (dd) {
                        if (parseInt(out.QuizeQuastions) === dd.LPMOEQOA_Id) {
                            out.LPMOEQOA_AnswerFlag = dd.LPMOEQOA_AnswerFlag;
                        }
                    });

                    $scope.term_list.push({
                        LPMOEQ_Id: out.lpmoeQ_Id, LPMOEQOA_Id: out.QuizeQuastions, QuizeQuastions: out.QuizeQuastions,
                        LPMOEQ_Question: out.lpmoeQ_Question, LPMOEQOA_AnswerFlag: out.LPMOEQOA_AnswerFlag
                    });
                    data.saveanswerlsttemp = $scope.term_list;
                }

                else {
                    data.LPSTUEXSANS_Answer = out.answeredques;
                }


                apiService.create("LP_OnlineStudentExam/Saveanswer", data).then(function (promise) {
                    $scope.getsavedanswer = promise.getsavedanswer;

                    $scope.count += 1;
                    if ($scope.count === $scope.q_rpt1.length) {
                        $scope.btnsave = true;
                    } else {
                        $scope.q_rpt1[$scope.count].class = "current";
                        $scope.btnsave = false;
                    }

                    var lpmoeQ_Id = $scope.q_rpt1[$scope.count].lpmoeQ_Id;
                    var subjectiveflag = $scope.q_rpt1[$scope.count].lpmoeQ_SubjectiveFlg;

                    angular.forEach($scope.getsavedanswer, function (itm1) {
                        if (itm1.lpmoeQ_Id === lpmoeQ_Id) {
                            if (subjectiveflag === false) {
                                $scope.q_rpt1[$scope.count].QuizeQuastions = itm1.lpmoeqoA_Id;
                            } else {
                                $scope.q_rpt1[$scope.count].answeredques = itm1.lpstuexsanS_Answer;
                            }
                        }
                    });
                    $scope.submitted3 = false;
                });
            } else {
                $scope.submitted3 = true;
            }
        };

        // Click Function For To Submit Exam
        $scope.Quit = function () {
            var data = "";

            $scope.LP_OnlineFinalDetails = [];
            var optionid = 0;
            var optioncorrectflag = false;
            var answerdesc = "";
            var attemptornot = "";
            var filename = "";
            var filepath = "";

            $scope.examstart_time = true;

            angular.forEach($scope.getexamleveldetails, function (ddd) {
                angular.forEach(ddd.questions, function (dd) {
                    answerdesc = "";
                    filename = "";
                    filepath = "";
                    attemptornot = "Not Attempted";
                    optionid = 0;
                    optioncorrectflag = false;

                    $scope.Temp_MF_Options = [];

                    if (dd.lpmoeQ_SubjectiveFlg === false && dd.lpmoeQ_MatchTheFollowingFlg !== true) {
                        if (dd.QuizeQuastions !== undefined && dd.QuizeQuastions !== null && dd.QuizeQuastions !== "") {
                            attemptornot = "Attempted";
                            optionid = parseInt(dd.QuizeQuastions);
                            angular.forEach(dd.qst_opt, function (d) {
                                if (parseInt(dd.QuizeQuastions) === d.LPMOEQOA_Id) {
                                    optioncorrectflag = d.LPMOEQOA_AnswerFlag;
                                }
                            });
                        }
                    } else if (dd.lpmoeQ_SubjectiveFlg === false && dd.lpmoeQ_MatchTheFollowingFlg === true) {
                        angular.forEach(dd.qst_opt, function (d_options) {
                            optionid = 0;
                            optioncorrectflag = false;
                            attemptornot = "Not Attempted";
                            filename = "";
                            filepath = "";
                            if (d_options.QuizeQuastions !== undefined && d_options.QuizeQuastions !== null && d_options.QuizeQuastions !== "") {
                                attemptornot = "Attempted";
                                optionid = parseInt(d_options.QuizeQuastions);
                                angular.forEach(d_options.cols_rows_array, function (d) {
                                    if (parseInt(d_options.QuizeQuastions) === d.lpmoeqoamF_Id) {
                                        optioncorrectflag = d.lpmoeqoamF_AnswerFlag;
                                    }
                                });
                            }

                            $scope.Temp_MF_Options.push({
                                LPMOEQ_Id: dd.lpmoeQ_Id, QuizeQuastions: optionid, LPMOEQOA_Id: d_options.LPMOEQOA_Id,
                                LPMOEQOA_AnswerFlag: optioncorrectflag,
                                answer: answerdesc, LPMOEQ_SubjectiveFlg: dd.lpmoeQ_SubjectiveFlg,
                                LPSTUEXANS_AttemptFlag: attemptornot,
                                LPSTUEXSANS_FilePath: filepath,
                                LPSTUEXSANS_FileName: filename,
                                LPMOEQ_MatchTheFollowingFlg: dd.lpmoeQ_MatchTheFollowingFlg
                            });
                        });
                    }
                    else {

                        $scope.Temp_Ques_Subjective_Files = [];
                        angular.forEach(dd.teacherdocuupload_subjective, function (dd_subj_files) {
                            if (dd_subj_files.LPSTUEXSANSFNFL_FilePath !== undefined && dd_subj_files.LPSTUEXSANSFNFL_FilePath !== null && dd_subj_files.LPSTUEXSANSFNFL_FilePath !== '') {
                                $scope.Temp_Ques_Subjective_Files.push({
                                    LPMOEQ_Id: dd.lpmoeQ_Id,
                                    LPSTUEXSANSFNFL_FilePath: dd_subj_files.LPSTUEXSANSFNFL_FilePath,
                                    LPSTUEXSANSFL_FileName: dd_subj_files.LPSTUEXSANSFL_FileName,
                                    LPSTUEXSANSFNFL_Id: dd_subj_files.LPSTUEXSANSFNFL_Id === undefined
                                        || dd_subj_files.LPSTUEXSANSFNFL_Id === null ? 0 : dd_subj_files.LPSTUEXSANSFNFL_Id,
                                });
                            }
                        });

                        if ((dd.answeredques !== undefined && dd.answeredques !== null && dd.answeredques !== "") || $scope.Temp_Ques_Subjective_Files.length > 0) {
                            attemptornot = "Attempted";
                            answerdesc = dd.answeredques === undefined || dd.answeredques === null || dd.answeredques === "" ? "" : dd.answeredques;

                            filepath = dd.LPSTUEXSANS_FilePath !== undefined && dd.LPSTUEXSANS_FilePath !== null && dd.LPSTUEXSANS_FilePath !== "" ? dd.LPSTUEXSANS_FilePath : "";

                            filename = dd.LPSTUEXSANS_FilePath !== undefined && dd.LPSTUEXSANS_FilePath !== null && dd.LPSTUEXSANS_FilePath !== "" ? dd.LPSTUEXSANS_FileName : "";
                        }
                    }
                    $scope.LP_OnlineFinalDetails.push({
                        LPMOEQ_Id: dd.lpmoeQ_Id, QuizeQuastions: optionid, LPMOEQOA_AnswerFlag: optioncorrectflag,
                        answer: answerdesc, LPMOEQ_SubjectiveFlg: dd.lpmoeQ_SubjectiveFlg, LPSTUEXANS_AttemptFlag: attemptornot,
                        LPMOEQ_MatchTheFollowingFlg: dd.lpmoeQ_MatchTheFollowingFlg,
                        LPSTUEXSANS_FilePath: filepath,
                        LPSTUEXSANS_FileName: filename,
                        Temp_Ques_Subjective_Files : $scope.Temp_Ques_Subjective_Files,
                        LP_OnlineFinalDetails_MF: $scope.Temp_MF_Options
                    });
                });
            });

            console.log($scope.LP_OnlineFinalDetails);
            data = {
                "LPSTUEX_TotalTime": $scope.Upresultd,
                "LPMOEEX_Id": $scope.LPMOEEX_Id,
                "ISMS_Id": $scope.ISMS_Id,
                "LP_OnlineFinalDetails": $scope.LP_OnlineFinalDetails
            };
            if ($scope.textoverall === "End") {
                swal({
                    title: "Time Out",
                    text: "Select Yes To Submit Your Exam.",
                    type: "warning",
                    showCancelButton: false,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            clearTimeout(t);
                            clearTimeout(t1);
                            $scope.timeforoverallquestion = 0;
                            apiService.create("LP_OnlineStudentExam/submitexam", data).then(function (promise) {
                                //$('#QuizQuastions').modal('hide');
                                swal("Exam Is Submitted");
                            });
                            if (document.exitFullscreen) {
                                document.exitFullscreen();
                            } else if (document.mozCancelFullScreen) {
                                /* Firefox */
                                document.mozCancelFullScreen();
                            } else if (document.webkitExitFullscreen) {
                                /* Chrome, Safari and Opera */
                                document.webkitExitFullscreen();
                            } else if (document.msExitFullscreen) {
                                /* IE/Edge */
                                document.msExitFullscreen();
                            }
                            $state.reload();
                        }
                        else {
                            swal("Exam Submission Cancelled");
                        }
                    });
            }
            else {
                swal({
                    title: "Are you sure?",
                    text: "Do You Want To Submit the Test?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            clearTimeout(t);
                            clearTimeout(t1);
                            $scope.timeforoverallquestion = 0;

                            apiService.create("LP_OnlineStudentExam/submitexam", data).then(function (promise) {
                                //if (promise.result.length > 0) {
                                //$('#QuizQuastions').modal('hide');
                                swal("Exam Is Submitted");
                                //}
                                //else {
                                //    $('#QuizQuastions').modal('hide');
                                //    swal("Exam Is Submitted");
                                //}

                                if (document.exitFullscreen) {
                                    document.exitFullscreen();
                                } else if (document.mozCancelFullScreen) {
                                    /* Firefox */
                                    document.mozCancelFullScreen();
                                } else if (document.webkitExitFullscreen) {
                                    /* Chrome, Safari and Opera */
                                    document.webkitExitFullscreen();
                                } else if (this.document.msExitFullscreen) {
                                    /* IE/Edge */
                                    document.msExitFullscreen();
                                }
                                $state.reload();
                            });
                        }
                        else {
                            swal("Exam Submission Cancelled");
                        }
                    });
            }
        };

        $scope.Ok = function () {
            $scope.examstart_time = false;
            $('#finalwindow').modal('hide');
        };

        $scope.OkViewMarks = function () {
            $scope.examstart_time = false;
            $scope.countdown();
            $('#viewmarks').modal('hide');
        };

        // View Marks
        $scope.GetViewMarks = function (detailss) {
            $scope.examstart_time = true;
            var data = {
                "LPMOEEX_Id": detailss.lpmoeeX_Id,
                "ISMS_Id": detailss.ismS_Id
            };
            apiService.create("LP_OnlineStudentExam/GetViewMarks", data).then(function (promise) {
                if (promise !== null) {

                    $scope.getexamdetails = promise.getexamdetails;                    

                    $scope.get_student_exam_details = promise.getmarksdetails;
                    $scope.getallmarksdetails = promise.getallmarksdetails;
                    $scope.getstudentquesansstaffdetailsview = promise.getstudentquesansstaffdetailsview;

                    if ($scope.get_student_exam_details !== null && $scope.get_student_exam_details.length > 0) {
                        $scope.LPSTUEX_Id = $scope.get_student_exam_details[0].lpstueX_Id;
                        $scope.TotalTimeTaken = $scope.get_student_exam_details[0].lpstueX_TotalTime;
                        $scope.TotalObtainedMarks = $scope.get_student_exam_details[0].lpstueX_TotalMarks;

                        $scope.examname = detailss.lpmoeeX_ExamName;
                        $scope.subjectname = detailss.ismS_SubjectName;

                        $scope.AMST_Id_Temp = $scope.get_student_exam_details[0].amsT_Id;
                        $scope.maxmarks = $scope.get_student_exam_details[0].lpstueX_TotalMaxMarks;
                        $scope.marksobtained = $scope.get_student_exam_details[0].lpstueX_TotalMarks;
                        $scope.examdate = $scope.get_student_exam_details[0].lpstueX_Date;
                        $scope.durationtake = $scope.get_student_exam_details[0].lpstueX_TotalTime;
                        $scope.questionattempt = $scope.get_student_exam_details[0].lpstueX_TotalQnsAnswered;
                        $scope.questioncorrect = $scope.get_student_exam_details[0].lpstueX_TotalCorrectAns;
                        $scope.maxmarkspercentage = $scope.get_student_exam_details[0].lpstueX_Percentage;
                    }

                    $scope.obj.UploadExamFlag = $scope.getexamdetails[0].lpmoeeX_UploadExamPaperFlg;
                    $scope.lpmoeeX_UploadExamPaperFlg = $scope.getexamdetails[0].lpmoeeX_UploadExamPaperFlg;

                    if ($scope.getstudentquesansstaffdetailsview !== null && $scope.getstudentquesansstaffdetailsview.length > 0) {
                        angular.forEach($scope.getstudentquesansstaffdetailsview, function (dd) {
                            var img = dd.lpstuexastF_AnswerSheetPath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.quesfiletypeview1 = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.quesdocument_Pathnewview1 = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpstuexastF_AnswerSheetPath;
                            }
                        });
                    }                   

                    if ($scope.obj.UploadExamFlag === false) {                         
                        $scope.getexamleveldetails = promise.getexamleveldetails;
                        $scope.getexamquestionlist = promise.getexamquestionlist;
                        $scope.getquestionoptionlist = promise.getquestionoptionlist;
                        $scope.getquestionmfoptionlist = promise.getquestionmfoptionlist;

                        $scope.getquestionsubjective_fileslist = promise.getquestionsubjective_fileslist;
                        $scope.get_examwise_ques_option_marks = promise.get_examwise_ques_option_marks;
                        $scope.get_examwise_ques_subjective_marks = promise.get_examwise_ques_subjective_marks;
                        $scope.getquestionsubjective_staff_fileslist = promise.getquestionsubjective_staff_fileslist;

                        $scope.Exam_Level_Details = [];
                        $scope.Exam_Level_Question_Details = [];
                        $scope.Exam_Level_Question_Options_Details = [];
                        $scope.Exam_Level_Question_Options_MF_Details = [];
                        $scope.Exam_Level_Question_Options_MF_Distinct_Details = [];

                        angular.forEach($scope.getexamleveldetails, function (level) {
                            $scope.Exam_Level_Question_Details = [];
                            angular.forEach($scope.getexamquestionlist, function (level_ques) {
                                if (level_ques.lpmoeexlvL_Id === level.lpmoeexlvL_Id) {
                                    var questionname = level_ques.lpmoeQ_Question.replace(/\r?\n/g, '<br/>');
                                    $scope.opeditstrimg3 = "";
                                    $scope.imgOptionCode = "";
                                    if (questionname != null && questionname != '') {
                                        var opsplitstredit = questionname.split(' ');
                                        $scope.opeditstrimg1 = opsplitstredit[3];
                                        if ($scope.opeditstrimg1 != undefined) {
                                            var opeditstrimg2 = $scope.opeditstrimg1.split('"');
                                            $scope.opeditstrimg3 = opeditstrimg2[1];
                                        }
                                        else {
                                            $scope.opeditstrimg3 = undefined;
                                        }
                                    }
                                    if ($scope.opeditstrimg3 != undefined) {
                                        $scope.imgOptionCode = $scope.opeditstrimg3;
                                    }

                                    if (level_ques.lpmoeQ_SubjectiveFlg === true) {
                                        $scope.btnsubject = true;
                                    }

                                    $scope.Exam_Level_Question_Details.push({
                                        LPMOEEXLVL_Id: level_ques.lpmoeexlvL_Id,
                                        LPMOEQ_Id: level_ques.lpmoeQ_Id,
                                        LPMOEQ_Question: level_ques.lpmoeQ_Question,
                                        LPMOEQ_Answer: level_ques.lpmoeQ_Answer,
                                        LPMOEQ_Marks: level_ques.lpmoeQ_Marks,
                                        LPMOEEXQNS_MaxMarks: level_ques.lpmoeQ_Marks,
                                        LPMOEEXQNS_Id: level_ques.lpmoeexqnS_Id,
                                        LPMOEQ_SubjectiveFlg: level_ques.lpmoeQ_SubjectiveFlg,
                                        LPMOEQ_MatchTheFollowingFlg: level_ques.lpmoeQ_MatchTheFollowingFlg,
                                        LPMOEEXQNS_QnsOrder: level_ques.lpmoeexqnS_QnsOrder,
                                        LPMOEQ_StructuralFlg: level_ques.lpmoeQ_StructuralFlg,
                                        imgOptionCode: $scope.imgOptionCode
                                    });
                                }
                            });

                            //Questions Option List
                            angular.forEach($scope.Exam_Level_Question_Details, function (ques) {

                                $scope.Exam_Level_Question_Options_Details = [];
                                $scope.Exam_Level_Question_Options_MF_Distinct_Details = [];
                                $scope.Exam_Level_Question_SubjectiveFiles_Details = [];
                                $scope.Exam_Level_Question_SubjectiveFiles_Staff_Details = [];

                                angular.forEach($scope.getquestionoptionlist, function (ques_options) {
                                    if (ques.LPMOEQ_Id === ques_options.lpmoeQ_Id) {
                                        $scope.opeditstrimgans = "";
                                        $scope.imganswer = "";
                                        if (ques_options.lpmoeqoA_OptionCode != null && ques_options.lpmoeqoA_OptionCode != '') {
                                            var opsplitstreditans = ques_options.lpmoeqoA_OptionCode.split(' ');
                                            $scope.opeditstrimgans = opsplitstreditans[3];
                                            if ($scope.opeditstrimgans != undefined) {
                                                var opeditstrimgans = $scope.opeditstrimgans.split('"');
                                                $scope.opeditstrimgans = opeditstrimgans[1];
                                            }
                                            else {
                                                $scope.opeditstrimgans = undefined;
                                            }
                                        }
                                        if ($scope.opeditstrimgans != undefined) {
                                            $scope.imganswer = $scope.opeditstrimgans;
                                        }

                                        $scope.correct_answer_option = "";
                                        if (ques_options.lpmoeqoA_AnswerFlag === true) {
                                            ques.correct_answer_option = ques_options.lpmoeqoA_Option;
                                        }

                                        $scope.Exam_Level_Question_Options_Details.push({
                                            LPMOEQ_Id: ques_options.lpmoeQ_Id,
                                            LPMOEQOA_Id: ques_options.lpmoeqoA_Id,
                                            LPMOEQOA_Option: ques_options.lpmoeqoA_Option,
                                            LPMOEQOA_OptionCode: ques_options.lpmoeqoA_OptionCode,
                                            LPMOEQOA_AnswerFlag: ques_options.lpmoeqoA_AnswerFlag,
                                            LPMOEQOA_Marks: ques_options.lpmoeqoA_Marks,
                                            imganswer: $scope.imganswer,
                                        });
                                    }
                                });

                                //Question Option Match The Following
                                angular.forEach($scope.Exam_Level_Question_Options_Details, function (ques_opts) {
                                    $scope.Exam_Level_Question_Options_MF_Details = [];
                                    angular.forEach($scope.getquestionmfoptionlist, function (ques_opts_mf) {
                                        if (ques_opts.LPMOEQOA_Id === ques_opts_mf.lpmoeqoA_Id) {
                                            $scope.Exam_Level_Question_Options_MF_Distinct_Details.push({
                                                LPMOEQ_Id: ques_opts_mf.lpmoeQ_Id,
                                                LPMOEQOA_Id: ques_opts_mf.lpmoeqoA_Id,
                                                LPMOEQOAMF_Id: ques_opts_mf.lpmoeqoamF_Id,
                                                LPMOEQOAMF_MatchtheFollowing: ques_opts_mf.lpmoeqoamF_MatchtheFollowing,
                                                LPMOEQOAMF_AnswerFlag: ques_opts_mf.lpmoeqoamF_AnswerFlag,
                                                LPMOEQOAMF_Order: ques_opts_mf.lpmoeqoamF_Order,
                                            });

                                            $scope.Exam_Level_Question_Options_MF_Details.push({
                                                LPMOEQ_Id: ques_opts_mf.lpmoeQ_Id,
                                                LPMOEQOA_Id: ques_opts_mf.lpmoeqoA_Id,
                                                LPMOEQOAMF_Id: ques_opts_mf.lpmoeqoamF_Id,
                                                LPMOEQOAMF_MatchtheFollowing: ques_opts_mf.lpmoeqoamF_MatchtheFollowing,
                                                LPMOEQOAMF_AnswerFlag: ques_opts_mf.lpmoeqoamF_AnswerFlag,
                                                LPMOEQOAMF_Order: ques_opts_mf.lpmoeqoamF_Order,
                                            });
                                        }
                                    });
                                    ques_opts.Level_Questions_Option_MF_List = $scope.Exam_Level_Question_Options_MF_Details;
                                });

                                $scope.Temp_Distinct_cols = $scope.Exam_Level_Question_Options_MF_Distinct_Details.filter((item, i, arr) => arr.findIndex((t) => t.LPMOEQOAMF_MatchtheFollowing === item.LPMOEQOAMF_MatchtheFollowing) === i);


                                //Student Wise Choose The Correct Answer/Match The Following Marks
                                angular.forEach($scope.get_examwise_ques_option_marks, function (opts_marks) {
                                    if (opts_marks.lpmoeQ_Id === ques.LPMOEQ_Id) {

                                        if (ques.LPMOEQ_SubjectiveFlg === false && ques.LPMOEQ_MatchTheFollowingFlg !== true) {
                                            ques.marks = opts_marks.lpstuexanS_Marks;
                                            ques.QuizeQuastions = opts_marks.lpmoeqoA_Id;

                                            angular.forEach($scope.Exam_Level_Question_Options_Details, function (opts_temp) {
                                                if (opts_marks.lpmoeqoA_Id === opts_temp.LPMOEQOA_Id) {
                                                    ques.selected_answer = opts_temp.LPMOEQOA_Option;
                                                }
                                            });
                                            ques.LPSTUEXANS_CorrectAnsFlg = opts_marks.lpstuexanS_CorrectAnsFlg;
                                            ques.LPSTUEXANS_AttemptFlag = opts_marks.lpstuexanS_AttemptFlag;
                                            ques.attempt_color = opts_marks.lpstuexanS_AttemptFlag === 'Attempted' ? 'green' : 'red';
                                        }

                                        if (ques.LPMOEQ_SubjectiveFlg === false && ques.LPMOEQ_MatchTheFollowingFlg === true) {
                                            angular.forEach($scope.Exam_Level_Question_Options_Details, function (mf_opts_marks) {
                                                if (mf_opts_marks.LPMOEQOA_Id === opts_marks.lpmoeqoA_Id) {
                                                    mf_opts_marks.marks = opts_marks.lpstuexanS_Marks;
                                                    mf_opts_marks.QuizeQuastions = opts_marks.lpmoeqoamF_Id;
                                                    mf_opts_marks.LPSTUEXANS_CorrectAnsFlg = opts_marks.lpstuexanS_CorrectAnsFlg;
                                                    mf_opts_marks.LPSTUEXANS_AttemptFlag = opts_marks.lpstuexanS_AttemptFlag;
                                                    mf_opts_marks.attempt_color = opts_marks.lpstuexanS_AttemptFlag === 'Attempted' ? 'green' : 'red';

                                                    angular.forEach(mf_opts_marks.Level_Questions_Option_MF_List, function (mf_opts_check) {
                                                        mf_opts_check.checked = false;
                                                        if (mf_opts_check.LPMOEQOAMF_Id === opts_marks.lpmoeqoamF_Id) {
                                                            mf_opts_check.checked = true;
                                                        }
                                                    });
                                                }
                                            });
                                        }
                                    }
                                });

                                //Student Wise Choose The Subjective Marks
                                angular.forEach($scope.get_examwise_ques_subjective_marks, function (opts_marks) {
                                    if (opts_marks.lpmoeQ_Id === ques.LPMOEQ_Id) {
                                        if (ques.LPMOEQ_SubjectiveFlg === true && ques.LPMOEQ_MatchTheFollowingFlg !== true) {
                                            ques.marks =  opts_marks.lpstuexanS_Marks;
                                            ques.LPSTUEXSANS_Id = opts_marks.lpstuexsanS_Id;
                                            ques.QuizeQuastions = opts_marks.lpstuexsanS_Answer;
                                            ques.LPSTUEXANS_AttemptFlag = opts_marks.lpstuexanS_AttemptFlag;
                                            ques.attempt_color = opts_marks.lpstuexanS_AttemptFlag === 'Attempted' ? 'green' : 'red';
                                        }
                                    }
                                });

                                // Subjective Questions With Files List
                                angular.forEach($scope.getquestionsubjective_fileslist, function (ques_subjtive_files) {
                                    if (ques.LPMOEQ_Id === ques_subjtive_files.lpmoeQ_Id) {
                                        if (ques_subjtive_files.filePath !== null && ques_subjtive_files.filePath !== '') {
                                            var img = ques_subjtive_files.filePath;
                                            var imagarr = img.split('.');
                                            $scope.quesdocument_Pathnew = "";
                                            $scope.lastelement = imagarr[imagarr.length - 1];
                                            if ($scope.lastelement === 'doc' || $scope.lastelement === 'docx' || $scope.lastelement === 'ppt' || $scope.lastelement === 'pptx' || $scope.lastelement === 'xlsx' || $scope.lastelement === 'xls') {
                                                $scope.quesdocument_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + ques_subjtive_files.filePath;
                                            }

                                            $scope.Exam_Level_Question_SubjectiveFiles_Details.push({
                                                LPMOEQ_Id: ques_subjtive_files.lpmoeQ_Id,
                                                FileName: ques_subjtive_files.fileName,
                                                FilePath: ques_subjtive_files.filePath,
                                                LPSTUEXSANS_Id: ques_subjtive_files.lpstuexsanS_Id,
                                                quesdocument_Pathnew: $scope.quesdocument_Pathnew,
                                                quesfiletypeview1: $scope.lastelement
                                            });
                                        }
                                    }
                                });

                                // Subjective Questions With Staff Files List
                                angular.forEach($scope.getquestionsubjective_staff_fileslist, function (ques_subjtive_files) {
                                    if (ques.LPMOEQ_Id === ques_subjtive_files.lpmoeQ_Id) {
                                        if (ques_subjtive_files.filePath !== null && ques_subjtive_files.filePath !== '') {
                                            var img = ques_subjtive_files.filePath;
                                            var imagarr = img.split('.');
                                            $scope.quesdocument_Pathnew = "";
                                            $scope.lastelement = imagarr[imagarr.length - 1];
                                            if ($scope.lastelement === 'doc' || $scope.lastelement === 'docx' || $scope.lastelement === 'ppt' || $scope.lastelement === 'pptx' || $scope.lastelement === 'xlsx' || $scope.lastelement === 'xls') {
                                                $scope.quesdocument_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + ques_subjtive_files.filePath;
                                            }

                                            $scope.Exam_Level_Question_SubjectiveFiles_Staff_Details.push({
                                                LPMOEQ_Id: ques_subjtive_files.lpmoeQ_Id,
                                                LPSTUEXSANSSFL_FileName: ques_subjtive_files.fileName,
                                                LPSTUEXSANSSFL_FilePath: ques_subjtive_files.filePath,
                                                LPSTUEXSANS_Id: ques_subjtive_files.lpstuexsanS_Id,
                                                quesdocument_Pathnew: $scope.quesdocument_Pathnew,
                                                quesfiletypeview1: $scope.lastelement
                                            });
                                        }
                                    }
                                });
                                
                                ques.MF_Distinct_Colms = $scope.Temp_Distinct_cols
                                ques.Level_Questions_Option_List = $scope.Exam_Level_Question_Options_Details;
                                ques.Level_Questions_SubjectiveFiles_List = $scope.Exam_Level_Question_SubjectiveFiles_Details;
                                ques.Temp_Staff_Ques_Subjective_Files = $scope.Exam_Level_Question_SubjectiveFiles_Staff_Details;
                            });

                            $scope.Exam_Level_Details.push({
                                AMST_Id: $scope.AMST_Id_Temp,
                                LPMOEEXLVL_Id: level.lpmoeexlvL_Id,
                                lpmoeexlvL_LevelDesc: level.lpmoeexlvL_LevelDesc,
                                lpmoeexlvL_TotalNoOfQns: level.lpmoeexlvL_TotalNoOfQns,
                                lpmoeexlvL_LevelTotalMarks: level.lpmoeexlvL_LevelTotalMarks,
                                lpmoeexlvL_MarksPerQns: level.lpmoeexlvL_MarksPerQns,
                                lpmoeexlvL_LevelOrder: level.lpmoeexlvL_LevelOrder,
                                Level_Questions_List: $scope.Exam_Level_Question_Details
                            });
                        });

                        $scope.showmarksgrid = true;
                        console.log($scope.Exam_Level_Details);

                    }
                    else {
                        angular.forEach($scope.getallmarksdetails, function (dd) {
                            var img = dd.lpstuexaS_AnswerSheetPath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.quesfiletypeview1 = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.quesdocument_Pathnewview1 = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpstuexaS_AnswerSheetPath;
                            }
                        });                       
                    }

                    $scope.examstart_time = true;
                    $('#viewmarks').modal('show');
                }
            });
        };

        $scope.ViewSubjectiveFiles = function (objs_files, questiondetails) {
            $scope.subjective_files = objs_files;


            $scope.Temp_Questions_Subjective_Staff_files = questiondetails.Temp_Staff_Ques_Subjective_Files;

            if ($scope.Temp_Questions_Subjective_Staff_files !== undefined && $scope.Temp_Questions_Subjective_Staff_files !== null && $scope.Temp_Questions_Subjective_Staff_files.length > 0) {

                angular.forEach($scope.Temp_Questions_Subjective_Staff_files, function (dd) {
                    if (dd.LPSTUEXSANSSFL_FilePath !== null && dd.LPSTUEXSANSSFL_FilePath !== "") {
                        var img = dd.LPSTUEXSANSSFL_FilePath;
                        var imagarr = img.split('.');
                        var lastelement = imagarr[imagarr.length - 1];
                        dd.quesfiletype = lastelement;
                        console.log("data : " + dd);
                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                            dd.quesdocument_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.LPSTUEXSANSSFL_FilePath;
                        }

                        // $scope.staff_subjective_teacherdocuupload.push(dd);
                    }
                });               
            }

            $('#mymodal_subjectivefiles').modal('show');
        };


        $scope.GetViewMarks_Backup = function (detailss) {

            var data = {
                "LPMOEEX_Id": detailss.lpmoeeX_Id,
                "ISMS_Id": detailss.ismS_Id
            };

            apiService.create("LP_OnlineStudentExam/GetViewMarks", data).then(function (promise) {
                if (promise !== null) {

                    if (promise.getmarksdetails !== null && promise.getmarksdetails.length > 0) {

                        $scope.getmarksdetails = promise.getmarksdetails;

                        $scope.examname = detailss.lpmoeeX_ExamName;
                        $scope.subjectname = detailss.ismS_SubjectName;

                        $scope.maxmarks = $scope.getmarksdetails[0].lpstueX_TotalMaxMarks;
                        $scope.marksobtained = $scope.getmarksdetails[0].lpstueX_TotalMarks;
                        $scope.examdate = $scope.getmarksdetails[0].lpstueX_Date;
                        $scope.durationtake = $scope.getmarksdetails[0].lpstueX_TotalTime;
                        $scope.questionattempt = $scope.getmarksdetails[0].lpstueX_TotalQnsAnswered;
                        $scope.questioncorrect = $scope.getmarksdetails[0].lpstueX_TotalCorrectAns;
                        $scope.maxmarkspercentage = $scope.getmarksdetails[0].lpstueX_Percentage;

                        $scope.getexamdetails = promise.getexamdetails;
                        $scope.lpmoeeX_UploadExamPaperFlg = $scope.getexamdetails[0].lpmoeeX_UploadExamPaperFlg;

                        $scope.getallmarksdetails = promise.getallmarksdetails;
                        $scope.getstudentquesansstaffdetailsview = promise.getstudentquesansstaffdetailsview;

                        angular.forEach($scope.getallmarksdetails, function (of) {
                            $scope.opeditstrimgans = "";
                            of.imganswer = "";
                            if (of.LPMOEQ_Question != null && of.LPMOEQ_Question != '') {
                                var opsplitstreditans = of.LPMOEQ_Question.split(' ');
                                $scope.opeditstrimgans = opsplitstreditans[3];
                                if ($scope.opeditstrimgans != undefined) {
                                    var opeditstrimgans = $scope.opeditstrimgans.split('"');
                                    $scope.opeditstrimgans = opeditstrimgans[1];
                                }
                                else {
                                    $scope.opeditstrimgans = undefined;
                                }
                            }
                            if ($scope.opeditstrimgans != undefined) {
                                of.imganswer = $scope.opeditstrimgans;
                            }
                        });

                        angular.forEach($scope.getallmarksdetails, function (of) {
                            $scope.opeditstrimgans1 = "";
                            of.imganswer1 = "";
                            if (of.StuQnsOptionName != null && of.StuQnsOptionName != '') {
                                var opsplitstreditans1 = of.StuQnsOptionName.split(' ');
                                $scope.opeditstrimgans1 = opsplitstreditans1[3];
                                if ($scope.opeditstrimgans1 != undefined) {
                                    var opeditstrimgans1 = $scope.opeditstrimgans1.split('"');
                                    $scope.opeditstrimgans1 = opeditstrimgans1[1];
                                }
                                else {
                                    $scope.opeditstrimgans1 = undefined;
                                }
                            }
                            if ($scope.opeditstrimgans1 != undefined) {
                                of.imganswer1 = $scope.opeditstrimgans1;
                            }
                        });

                        angular.forEach($scope.getallmarksdetails, function (of) {
                            $scope.opeditstrimgans2 = "";
                            of.imganswer2 = "";
                            if (of.StuQnsOptionName != null && of.StuQnsOptionName != '') {
                                var opsplitstreditans2 = of.StuQnsOptionName.split(' ');
                                $scope.opeditstrimgans2 = opsplitstreditans2[3];
                                if ($scope.opeditstrimgans2 != undefined) {
                                    var opeditstrimgans2 = $scope.opeditstrimgans2.split('"');
                                    $scope.opeditstrimgans2 = opeditstrimgans2[1];
                                }
                                else {
                                    $scope.opeditstrimgans2 = undefined;
                                }
                            }
                            if ($scope.opeditstrimgans2 != undefined) {
                                of.imganswer2 = $scope.opeditstrimgans2;
                            }
                        });

                        if ($scope.lpmoeeX_UploadExamPaperFlg === true) {
                            angular.forEach($scope.getallmarksdetails, function (dd) {
                                var img = dd.lpstuexaS_AnswerSheetPath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.quesfiletypeview1 = lastelement;
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.quesdocument_Pathnewview1 = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpstuexaS_AnswerSheetPath;
                                }
                            });
                        }

                        angular.forEach($scope.getstudentquesansstaffdetailsview, function (dd) {
                            var img = dd.lpstuexastF_AnswerSheetPath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.quesfiletypeview1 = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.quesdocument_Pathnewview1 = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpstuexastF_AnswerSheetPath;
                            }
                        });


                        if (promise.getexamwise_mfquestions !== null && promise.getexamwise_mfquestions.length > 0) {

                            $scope.getexamwise_mfquestions = promise.getexamwise_mfquestions;
                            $scope.getexamwise_mfques_options = promise.getexamwise_mfques_options;
                            $scope.getexamwise_ques_options_mf = promise.getexamwise_ques_options_mf;
                            $scope.getexamwise_ques_options_mf_marks = promise.getexamwise_ques_options_mf_marks;

                            $scope.cols_array = [];
                            $scope.rows_array = [];

                            $scope.question_options = [];

                            $scope.questionlist = [];

                            angular.forEach($scope.getexamwise_mfquestions, function (dd) {
                                $scope.opeditstrimgans = "";
                                dd.imganswer = "";
                                if (dd.lpmoeQ_Question != null && dd.lpmoeQ_Question != '') {
                                    var opsplitstreditans = dd.lpmoeQ_Question.split(' ');
                                    $scope.opeditstrimgans = opsplitstreditans[3];
                                    if ($scope.opeditstrimgans != undefined) {
                                        var opeditstrimgans = $scope.opeditstrimgans.split('"');
                                        $scope.opeditstrimgans = opeditstrimgans[1];
                                    }
                                    else {
                                        $scope.opeditstrimgans = undefined;
                                    }
                                }
                                if ($scope.opeditstrimgans != undefined) {
                                    dd.imganswer = $scope.opeditstrimgans;
                                }

                                $scope.questionlist.push({
                                    lpmoeQ_Id: dd.lpmoeQ_Id, lpmoeQ_Question: dd.lpmoeQ_Question,
                                    LPMOEQ_SubjectiveFlg: dd.lpmoeQ_SubjectiveFlg, LPMOEQ_StructuralFlg: dd.lpmoeQ_StructuralFlg,
                                    LPMOEQ_MatchTheFollowingFlg: dd.lpmoeQ_MatchTheFollowingFlg, imganswer: dd.imganswer
                                });
                            });

                            angular.forEach($scope.questionlist, function (ques) {
                                $scope.question_options = [];
                                angular.forEach($scope.getexamwise_mfques_options, function (ques_opts) {
                                    if (ques.lpmoeQ_Id === ques_opts.lpmoeQ_Id) {
                                        $scope.question_options.push({
                                            lpmoeqoA_Id: ques_opts.lpmoeqoA_Id, lpmoeqoA_Option: ques_opts.lpmoeqoA_Option,
                                            lpmoeqoA_AnswerFlag: ques_opts.lpmoeqoA_AnswerFlag, lpmoeQ_Id: ques_opts.lpmoeQ_Id
                                        });
                                    }
                                });
                                ques.options = $scope.question_options;
                            });

                            $scope.ques_opts_mf = [];
                            $scope.ques_opts_mf_temp = [];
                            angular.forEach($scope.questionlist, function (ques) {
                                $scope.ques_opts_mf_temp = [];
                                angular.forEach(ques.options, function (ques_opts) {
                                    $scope.ques_opts_mf = [];
                                    angular.forEach($scope.getexamwise_ques_options_mf, function (ques_opts_mf) {
                                        if (ques_opts.lpmoeqoA_Id === ques_opts_mf.lpmoeqoA_Id) {
                                            if (ques_opts_mf.lpmoeqoamF_AnswerFlag) {
                                                ques_opts.correctanswer = ques_opts_mf.lpmoeqoamF_MatchtheFollowing;
                                            }

                                            $scope.ques_opts_mf.push({
                                                lpmoeqoamF_Id: ques_opts_mf.lpmoeqoamF_Id,
                                                lpmoeqoA_Id: ques_opts_mf.lpmoeqoA_Id,
                                                lpmoeqoamF_MatchtheFollowing: ques_opts_mf.lpmoeqoamF_MatchtheFollowing,
                                                lpmoeqoamF_AnswerFlag: ques_opts_mf.lpmoeqoamF_AnswerFlag, checked: false
                                            });

                                            $scope.ques_opts_mf_temp.push({
                                                lpmoeqoamF_Id: ques_opts_mf.lpmoeqoamF_Id,
                                                lpmoeqoamF_MatchtheFollowing: ques_opts_mf.lpmoeqoamF_MatchtheFollowing,
                                                lpmoeqoamF_AnswerFlag: ques_opts_mf.lpmoeqoamF_AnswerFlag
                                            });
                                        }
                                    });
                                    ques_opts.matchfollowing = $scope.ques_opts_mf;
                                });

                                $scope.cols_array = $scope.ques_opts_mf_temp.filter((item, i, arr) => arr.findIndex((t) => t.lpmoeqoamF_MatchtheFollowing === item.lpmoeqoamF_MatchtheFollowing) === i);

                                ques.cols_array = $scope.cols_array;
                            });

                            angular.forEach($scope.questionlist, function (ques) {
                                angular.forEach(ques.options, function (ques_opts) {
                                    angular.forEach($scope.getexamwise_ques_options_mf_marks, function (mf_marks) {
                                        if (ques.lpmoeQ_Id == ques_opts.lpmoeQ_Id && mf_marks.lpmoeQ_Id == ques_opts.lpmoeQ_Id
                                            && mf_marks.lpmoeqoA_Id === ques_opts.lpmoeqoA_Id) {
                                            ques_opts.QuizeQuastions = mf_marks.lpmoeqoamF_Id;
                                            ques_opts.LPSTUEXANS_CorrectAnsFlg = mf_marks.lpstuexanS_CorrectAnsFlg;
                                            ques_opts.marks = mf_marks.lpstuexanS_Marks;
                                        }
                                    });
                                });
                            });
                            console.log($scope.questionlist);
                        }

                        $scope.examstart_time = true;
                        $('#viewmarks').modal('show');
                    } else {
                        swal("No Records Found");
                    }
                }
            });
        };




        $scope.cancel = function () {
            clearTimeout(t);
            clearTimeout(t1);
            $scope.timeforoverallquestion = 0;
            $state.reload();
        };

        $scope.cancel1 = function () {
            clearTimeout(t);
            clearTimeout(t1);
            $scope.timeforoverallquestion = 0;
            $state.reload();
        };

        $scope.interacted3 = function (field) {
            return $scope.submitted3;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.pauseOrPlay = function (ele) {
            $('#popup15').modal({ show: false }).on('hidden.bs.modal', function () {
                $(this).find('video')[0].pause();
            });
        };

        $scope.showGuardianPhotonew = function (data) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.lpmoeeX_QuestionPaperView;
            $scope.videdfd = data.lpmoeeX_QuestionPaperView;
            $scope.movie = { src: data.lpmoeeX_QuestionPaperView };
            $scope.movie1 = { src: data.lpmoeeX_QuestionPaperView };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.lpmoeeX_QuestionPaperView });
        };

        $scope.onview = function (filepath, filename) {
            //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            document.getElementById("viewpdf").innerHTML = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var htmlElements = '<embed id="pdfview" src="' + $scope.content + '"  style="width: 100%;" height="1000" type="application/pdf" />';
                    document.getElementById("viewpdf").innerHTML = htmlElements;
                    $('#showpdf').modal('show');
                });
        };

        $scope.onviewdocuments = function (filepath, filename) {
            var docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + filepath;
            $scope.detailFrame = $sce.trustAsResourceUrl(docpath);
            $('#myModaldocx').modal('show');
        };

        var imagedownload = "";
        $scope.downloaddirectimage = function (data, idd) {

            var studentreg = idd;
            $scope.examstart_time = true;
            $scope.imagedownload = data;
            imagedownload = data;

            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg
                    })[0].click();
                });
        };

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };

        //Timmer 
        $scope.clicktimmer = function () {
            $scope.timeforoverallquestion = 61;
            $scope.startTimeroverall();
        };

        $scope.clicktimmerstop = function () {
            clearTimeout(t);
            $scope.timeforoverallquestion = 0;
        };

        $scope.stoptimmer = function (t1) {
            swal("OK", t1);
        };

        $scope.teacherdocuupload = [{ id: 'Teacher1' }];

        $scope.addNewsiblingguard = function () {
            var newItemNo = $scope.teacherdocuupload.length + 1;

            if (newItemNo <= 50) {
                $scope.teacherdocuupload.push({ 'id': 'Teacher' + newItemNo });
            }
        };

        $scope.removeNewsiblingguard = function (index) {
            var newItemNo = $scope.teacherdocuupload.length - 1;
            $scope.teacherdocuupload.splice(index, 1);

            if ($scope.teacherdocuupload.length === 0) {
                //data
            }
        };

        $scope.uploadtecherdocuments1 = [];

        $scope.uploadtecherdocuments = function (input, document) {

            $scope.uploadtecherdocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].size <= 31457280) {
                    if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg")  // 2097152 bytes = 2MB 
                    {
                        UploaddianPhoto(document);
                    }
                    else if (input.files[0].type === "video/mp4") {
                        UploaddianPhoto(document);
                    }
                    else if (input.files[0].type === "application/pdf") {
                        UploaddianPhoto(document);
                    }
                    else if (input.files[0].type === "application/msword") {
                        UploaddianPhoto(document);
                    }
                    else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                        UploaddianPhoto(document);
                    }
                    else if (input.files[0].type === "application/vnd.ms-excel") {
                        UploaddianPhoto(document);
                    }
                    else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                        UploaddianPhoto(document);
                    }
                    else {
                        swal("Upload  Pdf, Doc, Image Files Only");
                    }
                } else {
                    swal("Upload File Size Should Be Less Than 30MB");
                }
            }
        };

        function UploaddianPhoto(data) {
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadtecherdocuments1.length; i++) {
                formData.append("File", $scope.uploadtecherdocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/lessonplannerdoc", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.LPSTUEXAS_AnswerSheetPath = d;
                    data.LPSTUEXAS_AnswerSheetFile = $scope.filename;
                    $('#').attr('src', data.LPSTUEXAS_AnswerSheetPath);
                    var img = data.LPSTUEXAS_AnswerSheetPath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.quesfiletype = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.quesdocument_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.LPSTUEXAS_AnswerSheetPath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        //Subjective Upload

        $scope.teacherdocuupload_subjective = [{ id: 'Teacher_subjective1' }];
        $scope.addNewsiblingguard_subjective = function (ques_temp) {
            var newItemNo = ques_temp.teacherdocuupload_subjective.length + 1;
            if (newItemNo <= 50) {
                ques_temp.teacherdocuupload_subjective.push({ 'id': 'Teacher_subjective' + newItemNo });
            }
        };

        $scope.removeNewsiblingguard_subjective = function (index, ques_temp) {
            var newItemNo = ques_temp.teacherdocuupload_subjective.length - 1;
            ques_temp.teacherdocuupload_subjective.splice(index, 1);

            if (ques_temp.teacherdocuupload_subjective.length === 0) {
                //data
            }
        };

        $scope.uploadtecherdocuments1_subjective = [];
        $scope.uploadtecherdocuments_subjective = function (input, document, ques_temp) {
            $scope.examstart_time = true;
            $scope.uploadtecherdocuments1_subjective = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].size <= 31457280) {
                    if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg")  // 2097152 bytes = 2MB 
                    { UploaddianPhoto_subjective(document, ques_temp); }
                    else if (input.files[0].type === "video/mp4") { UploaddianPhoto_subjective(document, ques_temp); }
                    else if (input.files[0].type === "application/pdf") { UploaddianPhoto_subjective(document, ques_temp); }
                    else if (input.files[0].type === "application/msword") { UploaddianPhoto_subjective(document, ques_temp); }
                    else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { UploaddianPhoto_subjective(document, ques_temp); }
                    else if (input.files[0].type === "application/vnd.ms-excel") { UploaddianPhoto_subjective(document, ques_temp); }
                    else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") { UploaddianPhoto_subjective(document, ques_temp); }
                    else { swal("Upload  Pdf, Doc, Image Files Only"); }
                } else { swal("Upload File Size Should Be Less Than 30MB"); }
            }
        };

        function UploaddianPhoto_subjective(data, temp_ques) {
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadtecherdocuments1_subjective.length; i++) {
                formData.append("File", $scope.uploadtecherdocuments1_subjective[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/lessonplannerdoc", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.LPSTUEXSANSFNFL_FilePath = d;
                    data.LPSTUEXSANSFL_FileName = $scope.filename;
                    $('#').attr('src', data.LPSTUEXSANSFNFL_FilePath);
                    var img = data.LPSTUEXSANSFNFL_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.quesfiletype = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.quesdocument_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.LPSTUEXSANSFNFL_FilePath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.OnClickHomePage = function () {
            var HostName = location.host;
            $window.location.href = 'http://' + HostName + '/#/app/StudentDashboard';
        };

        $scope.SaveAnswerSheet = function (objformanssheet) {

            if (objformanssheet.$valid) {
                var confirmmgs = "";
                $scope.examstart_time = true;
                var data = {
                    "LPSTUEX_TotalTime": $scope.Upresultd,
                    "LPMOEEX_Id": $scope.LPMOEEX_Id,
                    "ISMS_Id": $scope.ISMS_Id,
                    "SaveAnswerSheet": $scope.teacherdocuupload
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                swal({
                    title: "Are you sure?",
                    text: "Do You Want To Submit the Test?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            clearTimeout(t);
                            clearTimeout(t1);
                            $scope.timeforoverallquestion = 0;

                            apiService.create("LP_OnlineStudentExam/SaveAnswerSheet", data).then(function (promise) {

                                if (promise.message === "Saved") {
                                    swal("Exam Is Submitted");
                                } else if (promise.message === "Updated") {
                                    swal("Exam Is Submitted");
                                } else {
                                    swal("Failed To Submit");
                                }
                                $('#myModalView').modal('hide');

                                $state.reload();
                            });
                        }
                        else {
                            swal("Exam Submission Cancelled");
                        }
                    });

            } else {
                $scope.submitted3 = true;
            }
        };
    }

    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });

    angular.module('app').directive('txtArea', function () {
        return {
            restrict: 'AE',
            replace: 'true',
            scope: { data: '=', model: '=ngModel' },
            template: "<textarea></textarea>",
            link: function (scope, elem) {
                scope.$watch('data', function (newVal) {
                    if (newVal) {
                        debugger;
                        scope.model += newVal[0];
                        var editor = $("#editor").data("kendoEditor");

                        editor.value(scope.model);

                    }
                });
            }
        };
    });
})();