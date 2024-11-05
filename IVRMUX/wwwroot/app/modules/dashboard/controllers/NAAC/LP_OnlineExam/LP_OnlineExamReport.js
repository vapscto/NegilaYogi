(function () {
    'use strict';
    angular.module('app').controller('LP_OnlineExamReportController', LP_OnlineExamReportController)

    LP_OnlineExamReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter', '$sce', '$q', '$window']
    function LP_OnlineExamReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter, $sce, $q, $window) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
       

        $scope.obj = {};
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');

        var copty;

        $scope.maxdate = new Date();

        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        

        $scope.reportbtn = true;

        $scope.loaddata = function () {
            var pageid = 4;
            apiService.getURI("LP_OnlineStudentExam/getloaddatareport", pageid).then(function (promise) {
                $scope.getyearlist = promise.getyearlist;

            });

        };

        $scope.onchangeyear = function () {
            $scope.ASMCL_Id = "";
            $scope.getclasslist = [];
            $scope.ISMS_Id = "";
            $scope.getsubjectlist = [];
            $scope.ASMS_Id = "";
            $scope.getsetionlist = [];
            $scope.LPMOEEX_Id = "";
            $scope.getexamlist = [];
            $scope.result = [];
            $scope.reportbtn = true;

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("LP_OnlineStudentExam/onchangeyear", data).then(function (promise) {
                $scope.getclasslist = promise.getclasslist;
            });

        };

        $scope.onchangeclass = function () {
            $scope.ISMS_Id = "";
            $scope.getsubjectlist = [];
            $scope.ASMS_Id = "";
            $scope.getsetionlist = [];
            $scope.LPMOEEX_Id = "";
            $scope.getexamlist = [];
            $scope.result = [];
            $scope.reportbtn = true;

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            };
            apiService.create("LP_OnlineStudentExam/onchangeclass", data).then(function (promise) {
                $scope.getsectionlist = promise.getsectionlist;
            });

        };

        $scope.OnchangeSection = function () {
            $scope.ISMS_Id = "";
            $scope.getsubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.LPMOEEX_Id = "";
            $scope.getexamlist = [];
            $scope.result = [];
            $scope.reportbtn = true;
            $scope.submitted1 = false;
            $scope.submitted11 = false;
            $scope.checkall = false;

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id

            };
            apiService.create("LP_OnlineStudentExam/OnchangeSection", data).then(function (promise) {
                $scope.getsubjectlist = promise.getsubjectlist;
            });
        };

        $scope.onchangesubject = function () {

            $scope.LPMOEEX_Id = "";
            $scope.getexamlist = [];
            $scope.result = [];
            $scope.reportbtn = true;

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ISMS_Id": $scope.ISMS_Id,
                "ASMS_Id": $scope.ASMS_Id
            };
            apiService.create("LP_OnlineStudentExam/onchangesubject", data).then(function (promise) {
                $scope.getexamlist = promise.getexamlist;
            });
        };

        $scope.OnChangeExam = function () {

            angular.forEach($scope.getexamlist, function (dd) {
                if (dd.lpmoeeX_Id === parseInt($scope.LPMOEEX_Id)) {
                    $scope.obj.FMCB_fromDATE = new Date(dd.lpmoeeX_FromDateTime);
                    $scope.obj.FMCB_toDATE = new Date(dd.lpmoeeX_ToDateTime);
                    $scope.LPMOEEX_UploadExamPaperFlg = dd.lpmoeeX_UploadExamPaperFlg
                }
            });
        };


        $scope.savedata = function () {
            $scope.submitted1 = true;
            $scope.reportbtn = true;
            $scope.result = [];

            if ($scope.myForm.$valid) {
                $scope.fromdate = new Date($scope.obj.FMCB_fromDATE).toDateString();
                $scope.todate = new Date($scope.obj.FMCB_toDATE).toDateString();

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ISMS_Id": $scope.ISMS_Id,
                    "LPMOEEX_Id": $scope.LPMOEEX_Id,
                    "fromdate": $scope.fromdate,
                    "todate": $scope.todate,
                    "ASMS_Id": $scope.ASMS_Id
                };

                apiService.create("LP_OnlineStudentExam/getreport", data).then(function (promise) {
                    $scope.currentPage = 1;
                    $scope.itemsPerPage = paginationformasters;

                    if (promise.result !== null && promise.result.length > 0) {
                        $scope.reportbtn = false;
                        $scope.result = promise.result;

                        angular.forEach($scope.getyearlist, function (dd) {
                            if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                $scope.yearname = dd.asmaY_Year;
                            }
                        });

                        angular.forEach($scope.getexamlist, function (dd) {
                            if (dd.lpmoeeX_Id === parseInt($scope.LPMOEEX_Id)) {
                                $scope.examname = dd.lpmoeeX_ExamName;
                            }
                        });

                        angular.forEach($scope.getsubjectlist, function (dd) {
                            if (dd.ismS_Id === parseInt($scope.ISMS_Id)) {
                                $scope.subjectname = dd.ismS_SubjectName;
                            }
                        });

                    }
                    else {
                        swal('No Records Found');
                    }
                });
            }
            else {
                $scope.submitted1 = true;
            }
        };

        $scope.ViewStudentWiseMarks = function (objs) {

            $scope.studentname_temp = objs.StudentName;
           
            var data = {
                "AMST_Id": objs.AMST_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ISMS_Id": $scope.ISMS_Id,
                "LPMOEEX_Id": $scope.LPMOEEX_Id,
                "ASMS_Id": $scope.ASMS_Id
            };

            apiService.create("LP_OnlineStudentExam/ViewStudentWiseMarks", data).then(function (promise) {

                $scope.getexamdetails = promise.getexamdetails;
                angular.forEach($scope.getexamlist, function (dd) {
                    if (dd.lpmoeeX_Id === parseInt($scope.LPMOEEX_Id)) {
                        $scope.examname1 = dd.lpmoeeX_ExamName;
                    }
                });

                angular.forEach($scope.getsubjectlist, function (dd) {
                    if (dd.ismS_Id === parseInt($scope.ISMS_Id)) {
                        $scope.subjectname1 = dd.ismS_SubjectName;
                    }
                });

                angular.forEach($scope.getyearlist, function (dd) {
                    if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                        $scope.yearname1 = dd.asmaY_Year;
                    }
                });

                $scope.getmarksdetails = promise.getmarksdetails;
                $scope.durationtake = $scope.getmarksdetails !== null && $scope.getmarksdetails.length > 0 ? $scope.getmarksdetails[0].lpstueX_TotalTime : "";

                if ($scope.getexamdetails !== null && $scope.getexamdetails.length > 0) {

                    $scope.examflag = $scope.getexamdetails[0].lpmoeeX_UploadExamPaperFlg;
                    $scope.lpmoeeX_UploadExamPaperFlg = $scope.getexamdetails[0].lpmoeeX_UploadExamPaperFlg;

                    if ($scope.getexamdetails[0].lpmoeeX_UploadExamPaperFlg === false) {
                        $scope.start = false;
                        $scope.subject = false;

                        if (promise.getexamleveldetails !== null && promise.getexamleveldetails.length > 0 &&
                            promise.getexamquestionlist !== null && promise.getexamquestionlist.length > 0) {

                            $scope.getQuestion = promise.getexamquestionlist;
                            $scope.getexamleveldetails = promise.getexamleveldetails;
                            $scope.getconnfig = promise.getconnfig;
                            $scope.getQuestiondocuments = promise.getquestiondoclist;
                            $scope.getquestionoptionlist = promise.getquestionoptionlist;
                            $scope.getquestionmfoptionlist = promise.getquestionmfoptionlist;
                            $scope.getexamwise_ques_options_mf_marks = promise.getexamwise_ques_options_mf_marks;
                            $scope.getexamwise_ques_options_mf = promise.getexamwise_ques_options_mf;

                            angular.forEach($scope.getexamleveldetails, function (dd) {
                                $scope.tempquestions = [];
                                angular.forEach($scope.getQuestion, function (d) {
                                    if (dd.lpmoeexlvL_Id === d.lpmoeexlvL_Id) {
                                        //$scope.tempquestions.push(d);
                                        $scope.tempquestions.push({
                                            lpmoeQ_Id: d.lpmoeQ_Id, lpmoeQ_Question: d.lpmoeQ_Question,
                                            lpmoeQ_SubjectiveFlg: d.lpmoeQ_SubjectiveFlg,
                                            lpmoeQ_MatchTheFollowingFlg: d.lpmoeQ_MatchTheFollowingFlg,
                                            lpmoeeX_AnswerPapeFileName: d.lpmoeeX_AnswerPapeFileName,
                                            lpmoeexlvL_Id: d.lpmoeexlvL_Id, lpmoeexqnS_Id: d.lpmoeexqnS_Id,
                                            lpmoeexqnS_QnsOrder: d.lpmoeexqnS_QnsOrder
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
                                                    angular.forEach($scope.getexamwise_ques_options_mf, function (dd_mf) {
                                                        if (dd_mf.lpmoeqoA_Id === opts.LPMOEQOA_Id) {
                                                            if (dd_mf.lpmoeqoamF_AnswerFlag) {
                                                                opts.correctanswer = dd_mf.lpmoeqoamF_MatchtheFollowing;
                                                            }
                                                        }
                                                    });

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

                                    angular.forEach(d.qst_opt, function (ques_opts) {
                                        angular.forEach($scope.getexamwise_ques_options_mf_marks, function (mf_marks) {
                                            if (d.lpmoeQ_Id == ques_opts.LPMOEQ_Id && mf_marks.lpmoeQ_Id == ques_opts.LPMOEQ_Id
                                                && mf_marks.lpmoeqoA_Id === ques_opts.LPMOEQOA_Id) {
                                                ques_opts.QuizeQuastions = mf_marks.lpmoeqoamF_Id;
                                                ques_opts.LPSTUEXANS_CorrectAnsFlg = mf_marks.lpstuexanS_CorrectAnsFlg;
                                            }

                                            angular.forEach($scope.getexamwise_ques_options_mf, function (dd_mfs) {
                                                if (dd_mfs.lpmoeqoamF_Id === mf_marks.lpmoeqoamF_Id && d.lpmoeQ_Id == ques_opts.LPMOEQ_Id
                                                    && dd_mfs.lpmoeqoA_Id == ques_opts.LPMOEQOA_Id) {
                                                    ques_opts.studentans = dd_mfs.lpmoeqoamF_MatchtheFollowing;
                                                    ques_opts.marks = mf_marks.lpstuexanS_Marks;
                                                }
                                            });
                                        });
                                    });
                                });
                            });

                            $scope.tempdoc = [];
                            angular.forEach($scope.getexamleveldetails, function (dd) {
                                angular.forEach(dd.questions, function (d) {
                                    $scope.qst_opttemp = [];
                                    angular.forEach($scope.getQuestiondocuments, function (o) {
                                        if (o.lpmoeQ_Id === d.lpmoeQ_Id) {
                                            $scope.tempdoc.push(d);
                                        }
                                    });
                                    dd.files = $scope.tempdoc;
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
                            $scope.getallmarksdetails = promise.getallmarksdetails;

                            angular.forEach($scope.getexamleveldetails, function (d) {
                                angular.forEach(d.questions, function (dd) {
                                    angular.forEach($scope.getallmarksdetails, function (ddd) {
                                        if (ddd.LPMOEQ_Id === dd.lpmoeQ_Id) {
                                            dd.answergiven = ddd.StuQnsOptionName;
                                            dd.attemptflag = ddd.Attemptflag;
                                            dd.SubjectiveFileName = ddd.SubjectiveFileName;
                                            dd.SubjectiveFilePath = ddd.SubjectiveFilePath;
                                            if (dd.lpmoeQ_SubjectiveFlg === false) {
                                                if (ddd.LPMOEQOA_Id !== undefined && ddd.LPMOEQOA_Id !== null && ddd.LPMOEQOA_Id !== "") {
                                                    dd.QuizeQuastions = ddd.LPMOEQOA_Id;
                                                }
                                            }
                                            dd.marks = ddd.LPMOEEXQNS_Marks;
                                        }
                                    });
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

                            angular.forEach($scope.getexamleveldetails, function (dd) {
                                angular.forEach(dd.questions, function (d) {
                                    $scope.opeditstrimgans1 = "";
                                    d.imganswer1 = "";
                                    if (d.answergiven != null && d.answergiven != '') {
                                        var opsplitstreditans1 = d.answergiven.split(' ');
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
                                        d.imganswer1 = $scope.opeditstrimgans1;
                                    }
                                });
                            });

                            console.log($scope.getexamleveldetails);
                            $('#modalquestionpaper').modal('show');
                        }
                        else {
                            swal("No Questions Available for the Selected Subject");
                        }
                    }

                    else {
                        $scope.getallmarksdetails = promise.getallmarksdetails;
                        angular.forEach($scope.getallmarksdetails, function (dd) {
                            var img = dd.lpstuexaS_AnswerSheetPath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.quesfiletypeview1 = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.quesdocument_Pathnewview1 = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpstuexaS_AnswerSheetPath;
                            }
                        });
                        $('#viewmarks').modal('show');
                    }

                    $scope.StudentName = objs.StudentName;
                    $scope.AMST_AdmNo = objs.AMST_AdmNo;
                    $scope.ASMCL_ClassName = objs.ASMCL_ClassName;
                    $scope.ASMC_SectionName = objs.ASMC_SectionName;

                    $scope.Examdate = objs.Examdate;
                    $scope.examdate = objs.Examdate;

                    $scope.TotalNofQuestions = objs.TotalNofQuestions;
                    $scope.TotalNofQuestions = objs.TotalNofQuestions;

                    $scope.LPSTUEX_TotalQnsAnswered = objs.LPSTUEX_TotalQnsAnswered;

                    $scope.LPSTUEX_TotalMaxMarks = objs.LPSTUEX_TotalMaxMarks;
                    $scope.maxmarks = objs.LPSTUEX_TotalMaxMarks;

                    $scope.LPSTUEX_TotalMarks = objs.LPSTUEX_TotalMarks;
                    $scope.marksobtained = objs.LPSTUEX_TotalMarks;

                    $scope.LPSTUEX_Percentage = objs.LPSTUEX_Percentage;
                    $scope.maxmarkspercentage = objs.LPSTUEX_Percentage;
                } else {
                    swal("No Records Found");
                }

                //if (promise.getmarksdetails !== null && promise.getmarksdetails.length > 0) {                   
                //    $scope.getmarksdetails = promise.getmarksdetails;

                //    $scope.getstudentdetails = promise.getstudentdetails;
                //    $scope.maxmarks = $scope.getmarksdetails[0].lpstueX_TotalMaxMarks;
                //    $scope.marksobtained = $scope.getmarksdetails[0].lpstueX_TotalMarks;

                //    $scope.examdate = $scope.getmarksdetails[0].lpstueX_Date;
                //    $scope.durationtake = $scope.getmarksdetails[0].lpstueX_TotalTime;

                //    $scope.questionattempt = $scope.getmarksdetails[0].lpstueX_TotalQnsAnswered;
                //    $scope.questioncorrect = $scope.getmarksdetails[0].lpstueX_TotalCorrectAns;
                //    $scope.maxmarkspercentage = $scope.getmarksdetails[0].lpstueX_Percentage;

                //    $scope.getexamdetails = promise.getexamdetails;
                //    $scope.lpmoeeX_UploadExamPaperFlg = $scope.getexamdetails[0].lpmoeeX_UploadExamPaperFlg;                    

                //    if ($scope.lpmoeeX_UploadExamPaperFlg === true) {
                //        angular.forEach($scope.getallmarksdetails, function (dd) {
                //            var img = dd.lpstuexaS_AnswerSheetPath;
                //            var imagarr = img.split('.');
                //            var lastelement = imagarr[imagarr.length - 1];
                //            dd.quesfiletypeview1 = lastelement;
                //            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                //                dd.quesdocument_Pathnewview1 = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpstuexaS_AnswerSheetPath;
                //            }
                //        });
                //    }

                //} else {
                //    swal("No Records Found");
                //}
            });
        };

        $scope.OkViewMarks = function () {
            $('#viewmarks').modal('hide');
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.sort = function (key) {
            $scope.sortKey = key;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted1;
        };

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
        };

        $scope.onviewdocuments = function (filepath, filename) {
            var docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + filepath;
            $scope.detailFrame = $sce.trustAsResourceUrl(docpath);
            $('#myModaldocx').modal('show');
        };



        $scope.printdatatable = [];
        $scope.exportToExcel = function (table1) {
            $scope.sheetname = "Year_" + $scope.yearname + "- Subject_" + $scope.subjectname + " - Exam_" + $scope.examname;
            //var exportHref = Excel.tableToExcel(table1, $scope.sheetname);
            //$timeout(function () { location.href = exportHref; }, 100);

            var exportHref = Excel.tableToExcel(table1, $scope.sheetname);
            var excelname = $scope.sheetname + ".xls";
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);

        };

        $scope.printData = function (printSectionId) {

            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();

        };

        $scope.Print = function () {
            var innerContents = document.getElementById("printdetails").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/onlineexamquestionprint.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();

        };

        $scope.toggleAll = function () {
            $scope.printdatatable = [];
            var toggleStatus = $scope.all;
            angular.forEach($scope.result, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all === true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
            $scope.get_total_student_print();
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
            console.log($scope.view_videos);
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