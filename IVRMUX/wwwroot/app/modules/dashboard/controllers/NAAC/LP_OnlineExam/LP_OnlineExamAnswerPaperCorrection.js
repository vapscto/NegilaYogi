(function () {
    'use strict';
    angular.module('app').controller('LP_OE_AnswerPaperCorrectionController', LP_OE_AnswerPaperCorrectionController)

    LP_OE_AnswerPaperCorrectionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter', '$sce', '$q']
    function LP_OE_AnswerPaperCorrectionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter, $sce, $q) {


        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        if (privlgs !== null && privlgs.length > 0) {
            for (var i = 0; i < privlgs.length; i++) {
                if (privlgs[i].pageId == pageid) {
                    $scope.userPrivileges = privlgs[i];
                    console.log($scope.userPrivileges);
                }
            }
        }

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.obj = {};
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');

        var copty;
        $scope.maxdate = new Date();
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 500);
        };

        $scope.imgname = logopath;
        $scope.reportbtn = true;
        $scope.showmarksgrid = false;

        $scope.loaddata = function () {
            var pageid = 4;
            apiService.getURI("LP_OnlineStudentExam/getloaddatareport", pageid).then(function (promise) {
                $scope.getyearlist = promise.getyearlist;
            });
        };

        $scope.OnClickRadioBtn = function () {
            $scope.ASMAY_Id = "";
            // $scope.getyearlist = [];
            $scope.ASMCL_Id = "";
            $scope.getclasslist = [];
            $scope.ASMS_Id = "";
            $scope.getsectionlist = [];
            $scope.getstudentdetails = [];
            $scope.ISMS_Id = "";
            $scope.getsubjectlist = [];
            $scope.submitted1 = false;
            $scope.LPMOEEX_Id = "";
            $scope.getexamlist = [];
            $scope.result = [];
            $scope.reportbtn = true;
            $scope.submitted11 = false;
        };

        $scope.onchangeyear = function () {
            $scope.ASMCL_Id = "";
            $scope.getclasslist = [];
            $scope.ASMS_Id = "";
            $scope.getsectionlist = [];
            $scope.getstudentdetails = [];
            $scope.ISMS_Id = "";
            $scope.getsubjectlist = [];
            $scope.submitted1 = false;
            $scope.LPMOEEX_Id = "";
            $scope.getexamlist = [];
            $scope.result = [];
            $scope.reportbtn = true;
            $scope.submitted11 = false;
            $scope.showmarksgrid = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "Flag": $scope.entry
            };
            apiService.create("LP_OnlineStudentExam/onchangeyear", data).then(function (promise) {
                $scope.getclasslist = promise.getclasslist;
            });

        };

        $scope.onchangeclass = function () {
            $scope.ISMS_Id = "";
            $scope.getsubjectlist = [];
            $scope.ASMS_Id = "";
            $scope.getsectionlist = [];
            $scope.getstudentdetails = [];
            $scope.LPMOEEX_Id = "";
            $scope.getexamlist = [];
            $scope.result = [];
            $scope.reportbtn = true;
            $scope.submitted1 = false;
            $scope.submitted11 = false;
            $scope.checkall = false;
            $scope.showmarksgrid = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "Flag": $scope.entry
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
            $scope.showmarksgrid = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "Flag": $scope.entry
            };
            apiService.create("LP_OnlineStudentExam/OnchangeSection", data).then(function (promise) {
                $scope.getsubjectlist = promise.getsubjectlist;
            });
        };

        $scope.onchangesubject = function () {
            $scope.getstudentdetails = [];
            $scope.LPMOEEX_Id = "";
            $scope.getexamlist = [];
            $scope.result = [];
            $scope.reportbtn = true;
            $scope.submitted1 = false;
            $scope.submitted11 = false;
            $scope.checkall = false;
            $scope.showmarksgrid = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ISMS_Id": $scope.ISMS_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "Flag": $scope.entry
            };
            apiService.create("LP_OnlineStudentExam/onchangesubject", data).then(function (promise) {
                $scope.getexamlist = promise.getexamlist;
            });
        };

        $scope.OnChangeExam = function () {
            $scope.submitted1 = false;
            $scope.submitted11 = false;
            $scope.checkall = false;
            $scope.showmarksgrid = false;
            $scope.getstudentdetails = [];
            angular.forEach($scope.getexamlist, function (dd) {
                if (dd.lpmoeeX_Id === parseInt($scope.LPMOEEX_Id)) {
                    $scope.obj.FMCB_fromDATE = new Date(dd.lpmoeeX_FromDateTime);
                    $scope.obj.FMCB_toDATE = new Date(dd.lpmoeeX_ToDateTime);
                    $scope.obj.Maxmarks = parseFloat(dd.lpmoeeX_TotalMarks);
                    $scope.obj.UploadExamFlag = dd.lpmoeeX_UploadExamPaperFlg;
                }
            });
        };

        /* SEARCH DETAILS  */
        $scope.GetSearchDetails = function () {
            $scope.submitted11 = false;
            $scope.showmarksgrid = false;
            if ($scope.myForm.$valid) {
                $scope.getstudentdetails = [];
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ISMS_Id": $scope.ISMS_Id,
                    "LPMOEEX_Id": $scope.LPMOEEX_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "Flag": $scope.entry
                };

                apiService.create("LP_OnlineStudentExam/GetSearchDetails", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.getstudentdetails = promise.getstudentdetails;
                        if ($scope.getstudentdetails === null || $scope.getstudentdetails.length === 0) {
                            swal("No Records Found");
                        } else {

                            $scope.uploadcount = 0;
                            $scope.notuploadcount = 0;

                            angular.forEach($scope.getstudentdetails, function (dd) {
                                if (dd.UploadedFlag === "Submitted") {
                                    $scope.uploadcount += 1;
                                } else {
                                    $scope.notuploadcount += 1;
                                }
                            });

                            if ($scope.obj.UploadExamFlag === true) {
                                $scope.checkall = false;
                                $scope.entry = "Marks";
                                $scope.getstudentquesansdetails_temp = promise.getstudentquesansdetails;
                                angular.forEach($scope.getstudentdetails, function (dd) {
                                    dd.checked = false;
                                    dd.marks = dd.LPSTUEX_TotalMarks;
                                    if (dd.marks !== null && dd.marks !== "") {
                                        dd.checked = true;
                                    }

                                    $scope.temp_docs = [];
                                    angular.forEach($scope.getstudentquesansdetails_temp, function (docs) {
                                        if (docs.lpstueX_Id === dd.LPSTUEX_Id) {
                                            var img = docs.lpstuexaS_AnswerSheetPath;
                                            var imagarr = img.split('.');
                                            var lastelement = imagarr[imagarr.length - 1];
                                            $scope.temp_docs.push({
                                                AMST_Id: docs.amsT_Id, LPSTUEX_Id: docs.lpstueX_Id, LPSTUEXAS_Id: docs.lpstuexaS_Id,
                                                LPSTUEXAS_AnswerSheetFile: docs.lpstuexaS_AnswerSheetFile, LPSTUEXAS_AnswerSheetPath: docs.lpstuexaS_AnswerSheetPath,
                                                LPSTUEXAS_StaffOrStudentUploadFlag: docs.lpstuexaS_StaffOrStudentUploadFlag,
                                                FileName: docs.lpstuexaS_AnswerSheetFile, FilePath: docs.lpstuexaS_AnswerSheetPath,
                                                FileType: lastelement
                                            });
                                        }
                                    });
                                    dd.docs_list = $scope.temp_docs;
                                });
                            } else {
                                $scope.entry = "Subjective";
                            }
                        }
                    }
                });
            } else {
                $scope.submitted1 = true;
            }
        };

        /* View Student Wise Question List Or Documents */
        $scope.ViewQuestion = function (dd) {
            $scope.Temp_AMST = [];
            $scope.Temp_AMST_Id_studnetdetails = [];
            $scope.studentname = dd.STUDENTNAME;
            $scope.studamno = dd.AMST_AdmNo;
            $scope.ASMCL_ClassName = dd.ASMCL_ClassName;
            $scope.ASMC_SectionName = dd.ASMC_SectionName;

            $scope.Temp_AMST_Id = dd.AMST_Id;
            $scope.Temp_AMST_Id_studnetdetails = dd;

            $scope.teacherdocuupload = [];
            $scope.teacherdocuupload = [{ id: 'Teacher1' }];

            $scope.correctedteacherdocuupload = [];
            $scope.correctedteacherdocuupload = [{ id: 'Teacher1' }];

            $scope.staff_subjective_teacherdocuupload = [];
            $scope.staff_subjective_teacherdocuupload = [{ id: 'Teacher1' }];

            $scope.showmarksgrid = false;
            var data = {
                "AMST_Id": dd.AMST_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ISMS_Id": $scope.ISMS_Id,
                "LPMOEEX_Id": $scope.LPMOEEX_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "Flag": $scope.entry
            };

            apiService.create("LP_OnlineStudentExam/ViewQuestion", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getstudentquesansdetails = promise.getstudentquesansdetails;
                    $scope.correctedteacherdocuupload = promise.getstudentquesansstaffdetails;
                    $scope.get_student_exam_details = promise.get_student_exam_details;
                    $scope.LPSTUEX_Id = 0;
                    if ($scope.get_student_exam_details !== null && $scope.get_student_exam_details.length > 0) {
                        $scope.LPSTUEX_Id = $scope.get_student_exam_details[0].lpstueX_Id;
                        $scope.TotalTimeTaken = $scope.get_student_exam_details[0].lpstueX_TotalTime;
                        $scope.TotalObtainedMarks = $scope.get_student_exam_details[0].lpstueX_TotalMarks;
                    }


                    if ($scope.correctedteacherdocuupload !== null && $scope.correctedteacherdocuupload.length > 0) {
                        angular.forEach($scope.correctedteacherdocuupload, function (dd) {
                            var img = dd.lpstuexastF_AnswerSheetPath;
                            dd.LPSTUEXASTF_AnswerSheetPath = dd.lpstuexastF_AnswerSheetPath;
                            dd.LPSTUEXASTF_AnswerSheetFile = dd.lpstuexastF_AnswerSheetFile;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.quesfiletype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.quesdocument_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpstuexastF_AnswerSheetPath;
                            }
                        });
                    } else {
                        $scope.correctedteacherdocuupload = [];
                        $scope.correctedteacherdocuupload = [{ id: 'Teacher1' }];
                    }

                    $scope.staff_subjective_teacherdocuupload = [];
                    $scope.staff_subjective_teacherdocuupload = [{ id: 'Teacher1' }];

                    if ($scope.obj.UploadExamFlag === false) {
                        //$('#questionanswermodal').modal('show');
                        $scope.getexamleveldetails = promise.getexamleveldetails;
                        $scope.getexamquestionlist = promise.getexamquestionlist;
                        $scope.getquestionoptionlist = promise.getquestionoptionlist;
                        $scope.getquestionmfoptionlist = promise.getquestionmfoptionlist;

                        $scope.getquestionsubjective_fileslist = promise.getquestionsubjective_fileslist;
                        $scope.getquestionsubjective_staff_fileslist = promise.getquestionsubjective_staff_fileslist;
                        $scope.get_examwise_ques_option_marks = promise.get_examwise_ques_option_marks;
                        $scope.get_examwise_ques_subjective_marks = promise.get_examwise_ques_subjective_marks;

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

                                    //if (level_ques.lpmoeQ_SubjectiveFlg === true) {
                                    //    $scope.btnsubject = true;
                                    //}

                                    $scope.Exam_Level_Question_Details.push({
                                        LPMOEEXLVL_Id: level_ques.lpmoeexlvL_Id,
                                        LPMOEQ_Id: level_ques.lpmoeQ_Id,
                                        LPMOEQ_Question: level_ques.lpmoeQ_Question,
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
                                            ques.marks = opts_marks.lpstuexanS_Marks == null ? 0 : opts_marks.lpstuexanS_Marks;
                                            ques.QuizeQuastions = opts_marks.lpmoeqoA_Id;
                                            ques.LPSTUEXANS_Id = opts_marks.lpstuexanS_Id;
                                            ques.LPSTUEXANS_CorrectAnsFlg = opts_marks.lpstuexanS_CorrectAnsFlg;
                                            ques.LPSTUEXANS_AttemptFlag = opts_marks.lpstuexanS_AttemptFlag;
                                            ques.attempt_color = opts_marks.lpstuexanS_AttemptFlag === 'Attempted' ? 'green' : 'red';
                                        }

                                        if (ques.LPMOEQ_SubjectiveFlg === false && ques.LPMOEQ_MatchTheFollowingFlg === true) {
                                            angular.forEach($scope.Exam_Level_Question_Options_Details, function (mf_opts_marks) {
                                                if (mf_opts_marks.LPMOEQOA_Id === opts_marks.lpmoeqoA_Id) {
                                                    mf_opts_marks.marks = opts_marks.lpstuexanS_Marks == null ? 0 : opts_marks.lpstuexanS_Marks;
                                                    mf_opts_marks.QuizeQuastions = opts_marks.lpmoeqoamF_Id;
                                                    mf_opts_marks.LPSTUEXANS_Id = opts_marks.lpstuexanS_Id;
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
                                            //ques.marks = opts_marks.lpstuexanS_AttemptFlag === 'Attempted' ? opts_marks.lpstuexanS_Marks : 0;
                                            ques.marks = opts_marks.lpstuexanS_Marks === null ? 0 : opts_marks.lpstuexanS_Marks;
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
                                AMST_Id: dd.AMST_Id,
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

                    } else {
                        angular.forEach($scope.getstudentquesansdetails, function (dd) {
                            var img = dd.lpstuexaS_AnswerSheetPath;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.quesfiletypeview1 = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.quesdocument_Pathnewview1 = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpstuexaS_AnswerSheetPath;
                            }
                        });
                        $scope.btnsubject = false;
                        $('#ViewUploadedFilesStudentwise').modal('show');
                    }
                }
            });
        };

        $scope.ViewQuestion_BackUp = function (dd) {
            $scope.Temp_AMST = [];
            $scope.Temp_AMST_Id_studnetdetails = [];
            $scope.studentname = dd.STUDENTNAME;
            $scope.studamno = dd.AMST_AdmNo;
            $scope.ASMCL_ClassName = dd.ASMCL_ClassName;
            $scope.ASMC_SectionName = dd.ASMC_SectionName;

            $scope.Temp_AMST_Id = dd.AMST_Id;
            $scope.Temp_AMST_Id_studnetdetails = dd;

            $scope.teacherdocuupload = [];
            $scope.teacherdocuupload = [{ id: 'Teacher1' }];

            $scope.correctedteacherdocuupload = [];
            $scope.correctedteacherdocuupload = [{ id: 'Teacher1' }];
            var data = {
                "AMST_Id": dd.AMST_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ISMS_Id": $scope.ISMS_Id,
                "LPMOEEX_Id": $scope.LPMOEEX_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "Flag": $scope.entry
            };

            apiService.create("LP_OnlineStudentExam/ViewQuestion", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getstudentquesansdetails = promise.getstudentquesansdetails;
                    $scope.correctedteacherdocuupload = promise.getstudentquesansstaffdetails;

                    if ($scope.correctedteacherdocuupload !== null && $scope.correctedteacherdocuupload.length > 0) {
                        angular.forEach($scope.correctedteacherdocuupload, function (dd) {
                            var img = dd.lpstuexastF_AnswerSheetPath;
                            dd.LPSTUEXASTF_AnswerSheetPath = dd.lpstuexastF_AnswerSheetPath;
                            dd.LPSTUEXASTF_AnswerSheetFile = dd.lpstuexastF_AnswerSheetFile;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.quesfiletype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.quesdocument_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpstuexastF_AnswerSheetPath;
                            }
                        });
                    } else {
                        $scope.correctedteacherdocuupload = [];
                        $scope.correctedteacherdocuupload = [{ id: 'Teacher1' }];
                    }

                    console.log($scope.getstudentquesansdetails);
                    var count = 0;

                    if ($scope.getstudentquesansdetails !== null && $scope.getstudentquesansdetails.length > 0) {
                        if ($scope.obj.UploadExamFlag === false) {
                            angular.forEach($scope.getstudentquesansdetails, function (dd) {
                                if (dd.SubjectiveOrObjective === 'Subjective') {
                                    count += 1;
                                }
                                dd.marks = dd.LPMOEEXQNS_Marks;
                                dd.questionname = dd.LPMOEQ_Question.replace(/\r?\n/g, '<br/>');
                                dd.LPSTUEXSANS_Answer = dd.StuQnsOptionName;
                            });
                            if (count > 0) {
                                $scope.btnsubject = true;
                            }
                            $('#questionanswermodal').modal('show');
                        }
                        else {
                            angular.forEach($scope.getstudentquesansdetails, function (dd) {
                                var img = dd.lpstuexaS_AnswerSheetPath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.quesfiletypeview1 = lastelement;
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.quesdocument_Pathnewview1 = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpstuexaS_AnswerSheetPath;
                                }
                            });
                            $scope.btnsubject = false;
                            $('#ViewUploadedFilesStudentwise').modal('show');
                        }

                        angular.forEach($scope.getstudentquesansdetails, function (dd) {
                            $scope.opeditstrimg3 = "";
                            dd.imgOptionCode = "";
                            if (dd.questionname != null && dd.questionname != '') {
                                var opsplitstredit = dd.questionname.split(' ');
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

                        angular.forEach($scope.getstudentquesansdetails, function (of) {
                            $scope.opeditstrimgans = "";
                            of.imganswer = "";
                            if (of.LPSTUEXSANS_Answer != null && of.LPSTUEXSANS_Answer != '') {
                                var opsplitstreditans = of.LPSTUEXSANS_Answer.split(' ');
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
                                            ques_opts.LPSTUEXANS_Marks = mf_marks.lpstuexanS_Marks;
                                        }
                                    });
                                });
                            });
                            console.log($scope.questionlist);
                        }

                    } else {
                        if ($scope.obj.UploadExamFlag === false) {
                            swal("No Records Found");
                        } else {
                            $scope.btnsubject = false;
                            $('#ViewUploadedFilesStudentwise').modal('show');
                        }
                    }
                }
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.getstudentdetails.some(function (options) {
                return options.checked;
            });
        };

        $scope.OnChangeMarks = function (dd, string) {

            if (string === "upload") {
                $scope.marksnew = parseFloat(dd.marks);

                if ($scope.marksnew > $scope.obj.Maxmarks) {
                    swal("Marks Should Be Less Than Equal To Total Marks");
                    dd.marks = "";
                }
            } else if (string === "question") {
                $scope.marksnewquestion = parseFloat(dd.marks);
                $scope.maxmarksnew = parseFloat(dd.LPMOEEXQNS_MaxMarks);
                if ($scope.marksnewquestion > $scope.maxmarksnew) {
                    swal("Marks Should Be Less Than Equal To Total Marks " + $scope.maxmarksnew);
                    dd.marks = "";
                }
            }
        };

        $scope.SaveMarks = function (objd) {

            if (objd.$valid) {

                $scope.selectedstudents = [];

                angular.forEach($scope.getstudentdetails, function (dd) {
                    if (dd.checked === true) {
                        $scope.selectedstudents.push({
                            AMST_Id: dd.AMST_Id, marks: dd.marks, LPSTUEX_Id: dd.LPSTUEX_Id,
                            LPSTUEX_CorrectedAnswerSheetPath: dd.LPSTUEX_CorrectedAnswerSheetPath,
                            LPSTUEX_CorrectedAnswerSheetFile: dd.LPSTUEX_CorrectedAnswerSheetFile
                        });
                    }
                });
                swal({
                    title: "Are you sure",
                    text: "Do You Want To Submit The Record ?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Submit",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            var data = {
                                "ASMAY_Id": $scope.ASMAY_Id,
                                "ASMCL_Id": $scope.ASMCL_Id,
                                "ISMS_Id": $scope.ISMS_Id,
                                "LPMOEEX_Id": $scope.LPMOEEX_Id,
                                "savemarks": $scope.selectedstudents,
                                "ASMS_Id": $scope.ASMS_Id,
                                "Flag": 'Marks'
                            };

                            apiService.create("LP_OnlineStudentExam/SaveMarks", data).then(function (promise) {
                                if (promise !== null) {
                                    if (promise.message === "Save") {
                                        swal("Record Saved/Updated Successfully");
                                        $state.reload();
                                    } else {
                                        swal("Failed To Save/Update Record");
                                    }
                                } else {
                                    swal("Failed To Save/Update Record");
                                }
                            });
                        }
                        else {
                            swal("Submittion Cancelled");
                        }
                    });

            } else {
                $scope.submitted11 = true;
            }
        };

        $scope.SaveSubjectiveMarks = function (objdds) {

            if (objdds.$valid) {

                $scope.savedetails = [];
                $scope.savedetails_MCQ_MF = [];

                //angular.forEach($scope.getstudentquesansdetails, function (dd) {
                //    if (dd.SubjectiveOrObjective === 'Subjective') {
                //        $scope.savedetails.push({
                //            AMST_Id: dd.AMST_Id, LPSTUEX_Id: dd.LPSTUEX_Id, LPSTUEXSANS_Id: dd.LPSTUEXSANS_Id, LPMOEQ_Id: dd.LPMOEQ_Id,
                //            LPSTUEXSANS_Marks: dd.marks, LPMOEEXQNS_Marks: dd.LPMOEEXQNS_MaxMarks,
                //            LPSTUEX_TotalMaxMarks: dd.LPSTUEX_TotalMaxMarks, LPSTUEX_TotalMarks: dd.LPSTUEX_TotalMarks
                //        });
                //    }
                //});


                angular.forEach($scope.Exam_Level_Details, function (level) {
                    angular.forEach(level.Level_Questions_List, function (level_ques) {
                        if (level_ques.LPMOEQ_SubjectiveFlg === true && level_ques.LPMOEQ_MatchTheFollowingFlg === false) {
                            $scope.savedetails.push({
                                AMST_Id: level.AMST_Id, LPSTUEX_Id: $scope.LPSTUEX_Id, LPSTUEXSANS_Id: level_ques.LPSTUEXSANS_Id,
                                LPMOEQ_Id: level_ques.LPMOEQ_Id, LPSTUEXSANS_Marks: level_ques.marks, LPMOEEXQNS_Marks: level_ques.LPMOEEXQNS_MaxMarks,
                                Temp_Staff_Ques_Subjective_Files: level_ques.Temp_Staff_Ques_Subjective_Files
                            });
                        }

                        if (level_ques.LPMOEQ_SubjectiveFlg === false && level_ques.LPMOEQ_MatchTheFollowingFlg === false) {
                            $scope.savedetails_MCQ_MF.push({
                                AMST_Id: level.AMST_Id, LPSTUEX_Id: $scope.LPSTUEX_Id, LPSTUEXANS_Id: level_ques.LPSTUEXANS_Id,
                                LPMOEQ_Id: level_ques.LPMOEQ_Id,
                                LPSTUEXANS_Marks: level_ques.marks, LPMOEEXQNS_Marks: level_ques.LPMOEEXQNS_MaxMarks
                            });
                        }

                        if (level_ques.LPMOEQ_SubjectiveFlg === false && level_ques.LPMOEQ_MatchTheFollowingFlg === true) {

                            angular.forEach(level_ques.Level_Questions_Option_List, function (level_ques_opt) {
                                $scope.savedetails_MCQ_MF.push({
                                    AMST_Id: level.AMST_Id, LPSTUEX_Id: $scope.LPSTUEX_Id, LPSTUEXANS_Id: level_ques_opt.LPSTUEXANS_Id,
                                    LPMOEQ_Id: level_ques.LPMOEQ_Id,
                                    LPSTUEXANS_Marks: level_ques_opt.marks, LPMOEEXQNS_Marks: level_ques_opt.LPMOEQOA_Marks
                                });
                            });
                        }
                    });
                });

                swal({
                    title: "Are you sure",
                    text: "Do You Want To Submit The Record ?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Submit",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            var data = {
                                "ASMAY_Id": $scope.ASMAY_Id,
                                "ASMCL_Id": $scope.ASMCL_Id,
                                "ISMS_Id": $scope.ISMS_Id,
                                "LPMOEEX_Id": $scope.LPMOEEX_Id,
                                "savedetails": $scope.savedetails,
                                "savedetails_MCQ_MF": $scope.savedetails_MCQ_MF,
                                "ASMS_Id": $scope.ASMS_Id,
                                "LPSTUEX_Id": $scope.LPSTUEX_Id,
                                "Flag": 'Marks'
                            };

                            apiService.create("LP_OnlineStudentExam/SaveSubjectiveMarks", data).then(function (promise) {
                                if (promise !== null) {
                                    if (promise.message === "Add") {
                                        swal("Record Saved/Updated Successfully");
                                        //$('#questionanswermodal').modal('hide');
                                        $scope.showmarksgrid = false;
                                        $scope.LPSTUEX_Id = 0;
                                        $scope.scroll();
                                    } else {
                                        swal("Failed To Save/Update Record");
                                    }
                                } else {
                                    swal("Failed To Save/Update Record");
                                }
                            });
                        }
                        else {
                            swal("Submittion Cancelled");
                        }
                    });

            } else {
                $scope.submitted123 = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted1;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted11;
        };

        $scope.interacted123 = function (field) {
            return $scope.submitted123;
        };

        $scope.cleartabl1 = function (field) {
            $state.reload();
        };

        $scope.toggleAll_S = function (checkall) {
            var toggleStatus = checkall;
            angular.forEach($scope.getstudentdetails, function (itm) {
                itm.checked = toggleStatus;
            });
        };

        $scope.optionToggled_S = function () {
            $scope.checkall = $scope.getstudentdetails.every(function (itm) { return itm.checked; });
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

        $scope.onviewdocuments = function (filepath, filename) {
            var docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + filepath;
            $scope.detailFrame = $sce.trustAsResourceUrl(docpath);
            $('#myModaldocx').modal('show');
        };

        $scope.onview = function (filepath, filename) {
            //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            document.getElementById("pdfviewdd").innerHTML = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var htmlElements = '<embed id="pdfview" src="' + $scope.content + '"  style="width: 100%;" height="1000" type="application/pdf" />';
                    document.getElementById("pdfviewdd").innerHTML = htmlElements;
                    $('#showpdf').modal('show');
                });
        };

        var imagedownload = "";
        $scope.downloaddirectimage = function (data, idd) {
            var studentreg = idd;
            $scope.imagedownload = data;
            imagedownload = data;

            if ($scope.studentname !== undefined && $scope.studentname !== null && $scope.studentname !== '') {
                studentreg = $scope.studentname + '_' + idd;
            }

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

        $scope.uploadtecherdocuments11 = [];

        $scope.uploadtecherdocuments1 = function (input, document) {

            $scope.uploadtecherdocuments11 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {
                //$scope.size = input.files[0].size;
                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg") { UploaddianPhoto1(document); }
                else if (input.files[0].type === "application/pdf") { UploaddianPhoto1(document); }
                else if (input.files[0].type === "application/msword") { UploaddianPhoto1(document); }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { UploaddianPhoto1(document); }
                else if (input.files[0].type === "application/vnd.ms-excel") { UploaddianPhoto1(document); }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document,") { UploaddianPhoto1(document); }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation") { UploaddianPhoto1(document); }
                else if (input.files[0].type === "application/vnd.ms-powerpoint") { UploaddianPhoto1(document); }
                else { swal("Upload MP4, Pdf, Doc, Image Files Only"); }
            }
        };

        function UploaddianPhoto1(data) {
            console.log("Teacher Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadtecherdocuments11.length; i++) {
                formData.append("File", $scope.uploadtecherdocuments11[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/lessonplannerdoc", formData,
                {
                    withCredentials: true,
                    headers: {
                        'Content-Type': undefined
                    },
                    transformRequest: angular.identity,

                })
                .success(function (d) {
                    defer.resolve(d);
                    data.LPSTUEX_CorrectedAnswerSheetPath = d;
                    data.LPSTUEX_CorrectedAnswerSheetFile = $scope.filename;
                    $('#').attr('src', data.lpmtR_Resources);
                    var img = data.lpmtR_Resources;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.quesfiletypeview1 = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.quesdocument_Pathnewview1 = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpmtR_Resources;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }


        // Missing File Upload
        $scope.SaveStudentAnswerFileByStaff = function () {
            $scope.savemessage_filemissing_ansfile = "";
            $scope.missingfiles = [];
            $scope.correctedanswerfiles = [];

            angular.forEach($scope.teacherdocuupload, function (dd) {
                if (dd.LPSTUEXAS_AnswerSheetPath !== undefined && dd.LPSTUEXAS_AnswerSheetPath !== null && dd.LPSTUEXAS_AnswerSheetPath !== "") {
                    $scope.missingfiles.push(dd);
                }
            });

            angular.forEach($scope.correctedteacherdocuupload, function (dd) {
                if (dd.LPSTUEXASTF_AnswerSheetPath !== undefined && dd.LPSTUEXASTF_AnswerSheetPath !== null && dd.LPSTUEXASTF_AnswerSheetPath !== "") {
                    $scope.correctedanswerfiles.push(dd);
                }
            });

            if ($scope.missingfiles.length === 0 && $scope.correctedanswerfiles.length === 0) {
                $scope.savemessage_filemissing_ansfile = "Not Uploaded Any File To Save This Details So Kindly Upload Atleast One File To Save Details";
                return;
            }

            if ($scope.savemessage_filemissing_ansfile === "") {
                var data = {
                    "AMST_Id": $scope.Temp_AMST_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "ISMS_Id": $scope.ISMS_Id,
                    "LPMOEEX_Id": $scope.LPMOEEX_Id,
                    "missingfiles": $scope.missingfiles,
                    "correctedanswerfiles": $scope.correctedanswerfiles,
                };

                apiService.create("LP_OnlineStudentExam/SaveStudentAnswerFileByStaff", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message === "Save") {
                            swal("Record Save/Updated Successfully");

                            angular.forEach($scope.getstudentdetails, function (stu) {
                                if (Number(stu.AMST_Id) === Number($scope.Temp_AMST_Id)) {
                                    if (stu.UploadedOrder === 0) {
                                        stu.LPSTUEX_Id = promise.lpstueX_Id;
                                    }
                                    angular.forEach($scope.missingfiles, function (dd_files) {
                                        if (dd_files.LPSTUEXAS_AnswerSheetPath !== undefined && dd_files.LPSTUEXAS_AnswerSheetPath !== null
                                            && dd_files.LPSTUEXAS_AnswerSheetPath !== '') {

                                            var img = dd_files.LPSTUEXAS_AnswerSheetPath;
                                            var imagarr = img.split('.');
                                            var lastelement = imagarr[imagarr.length - 1];

                                            stu.docs_list.push({
                                                AMST_Id: stu.AMST_Id, LPSTUEX_Id: stu.LPSTUEX_Id, LPSTUEXAS_Id: 0,
                                                LPSTUEXAS_AnswerSheetFile: dd_files.LPSTUEXAS_AnswerSheetFile,
                                                LPSTUEXAS_AnswerSheetPath: dd_files.LPSTUEXAS_AnswerSheetPath,
                                                LPSTUEXAS_StaffOrStudentUploadFlag: 'Staff',
                                                FileName: dd_files.LPSTUEXAS_AnswerSheetFile, FilePath: dd_files.LPSTUEXAS_AnswerSheetPath,
                                                FileType: lastelement
                                            });
                                        }
                                    });
                                }
                            });
                        } else {
                            swal("Failed To Save/Update Record");
                        }
                        console.log($scope.Temp_AMST_Id_studnetdetails);
                        $scope.ViewQuestion($scope.Temp_AMST_Id_studnetdetails);
                    }
                });
            }
        };

        $scope.addNewsiblingguard = function () {
            var newItemNo = $scope.teacherdocuupload.length + 1;

            if (newItemNo <= 50) {
                $scope.teacherdocuupload.push({ 'id': 'Teacher' + newItemNo });
            }
            console.log($scope.teacherdocuupload);
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
            $scope.savemessage_filemissing_ansfile = "";
            $scope.filename = input.files[0].name;
            if (input.files && input.files[0]) {
                if (input.files[0].size <= 31457280) {
                    if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg")  // 2097152 bytes = 2MB 
                    { UploaddianPhoto(document); }
                    else if (input.files[0].type === "video/mp4") { UploaddianPhoto(document); }
                    else if (input.files[0].type === "application/pdf") { UploaddianPhoto(document); }
                    else if (input.files[0].type === "application/msword") { UploaddianPhoto(document); }
                    else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { UploaddianPhoto(document); }
                    else if (input.files[0].type === "application/vnd.ms-excel") { UploaddianPhoto(document); }
                    else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") { UploaddianPhoto(document); }
                    else { swal("Upload  Pdf, Doc, Image Files Only"); }
                } else { swal("Upload File Size Should Be Less Than 30MB"); }
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
                    console.log("data : " + data);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.quesdocument_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.LPSTUEXAS_AnswerSheetPath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.correctedaddNewsiblingguard = function () {
            var newItemNo = $scope.correctedteacherdocuupload.length + 1;

            if (newItemNo <= 50) {
                $scope.correctedteacherdocuupload.push({ 'id': 'Teacher' + newItemNo });
            }
            console.log($scope.correctedteacherdocuupload);
        };

        $scope.correctedremoveNewsiblingguard = function (index) {
            var newItemNo = $scope.correctedteacherdocuupload.length - 1;
            $scope.correctedteacherdocuupload.splice(index, 1);
            if ($scope.correctedteacherdocuupload.length === 0) {
                //data
            }
        };

        $scope.corrected_uploadtecherdocuments1 = [];
        $scope.corrected_uploadtecherdocuments = function (input, document) {
            $scope.corrected_uploadtecherdocuments1 = input.files;
            $scope.savemessage_filemissing_ansfile = "";
            $scope.filename = input.files[0].name;
            if (input.files && input.files[0]) {
                if (input.files[0].size <= 31457280) {
                    if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg") { corrected_UploaddianPhoto(document); }
                    else if (input.files[0].type === "video/mp4") { corrected_UploaddianPhoto(document); }
                    else if (input.files[0].type === "application/pdf") { corrected_UploaddianPhoto(document); }
                    else if (input.files[0].type === "application/msword") { corrected_UploaddianPhoto(document); }
                    else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { corrected_UploaddianPhoto(document); }
                    else if (input.files[0].type === "application/vnd.ms-excel") { corrected_UploaddianPhoto(document); }
                    else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") { corrected_UploaddianPhoto(document); }
                    else { swal("Upload  Pdf, Doc, Image Files Only"); }
                } else { swal("Upload File Size Should Be Less Than 30MB"); }
            }
        };
        function corrected_UploaddianPhoto(data) {
            var formData = new FormData();
            for (var i = 0; i <= $scope.corrected_uploadtecherdocuments1.length; i++) {
                formData.append("File", $scope.corrected_uploadtecherdocuments1[i]);
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
                    data.LPSTUEXASTF_AnswerSheetPath = d;
                    data.LPSTUEXASTF_AnswerSheetFile = $scope.filename;
                    $('#').attr('src', data.LPSTUEXASTF_AnswerSheetPath);
                    var img = data.LPSTUEXASTF_AnswerSheetPath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.quesfiletype = lastelement;
                    console.log("data : " + data);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.quesdocument_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.LPSTUEXASTF_AnswerSheetPath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }


        $scope.staff_subjective_addNewsiblingguard = function () {
            var newItemNo = $scope.staff_subjective_teacherdocuupload.length + 1;
            if (newItemNo <= 50) {
                $scope.staff_subjective_teacherdocuupload.push({ 'id': 'Teacher' + newItemNo });
            }
            console.log($scope.staff_subjective_teacherdocuupload);
        };

        $scope.staff_subjective_removeNewsiblingguard = function (index) {
            var newItemNo = $scope.staff_subjective_teacherdocuupload.length - 1;
            $scope.staff_subjective_teacherdocuupload.splice(index, 1);
            if ($scope.staff_subjective_teacherdocuupload.length === 0) {
                //data
            }
        };

        $scope.staff_subjective_corrected_uploadtecherdocuments1 = [];
        $scope.staff_subjective_corrected_uploadtecherdocuments = function (input, document) {
            $scope.staff_subjective_corrected_uploadtecherdocuments1 = input.files;
            $scope.savemessage_filemissing_ansfile = "";
            $scope.filename = input.files[0].name;
            if (input.files && input.files[0]) {
                if (input.files[0].size <= 31457280) {
                    if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg") { staff_subjective_corrected_UploaddianPhoto(document); }
                    else if (input.files[0].type === "video/mp4") { staff_subjective_corrected_UploaddianPhoto(document); }
                    else if (input.files[0].type === "application/pdf") { staff_subjective_corrected_UploaddianPhoto(document); }
                    else if (input.files[0].type === "application/msword") { staff_subjective_corrected_UploaddianPhoto(document); }
                    else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { staff_subjective_corrected_UploaddianPhoto(document); }
                    else if (input.files[0].type === "application/vnd.ms-excel") { staff_subjective_corrected_UploaddianPhoto(document); }
                    else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") { staff_subjective_corrected_UploaddianPhoto(document); }
                    else { swal("Upload  Pdf, Doc, Image Files Only"); }
                } else { swal("Upload File Size Should Be Less Than 30MB"); }
            }
        };

        function staff_subjective_corrected_UploaddianPhoto(data) {
            var formData = new FormData();
            for (var i = 0; i <= $scope.staff_subjective_corrected_uploadtecherdocuments1.length; i++) {
                formData.append("File", $scope.staff_subjective_corrected_uploadtecherdocuments1[i]);
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
                    data.LPSTUEXSANSSFL_FilePath = d;
                    data.LPSTUEXSANSSFL_FileName = $scope.filename;
                    $('#').attr('src', data.LPSTUEXSANSSFL_FilePath);
                    var img = data.LPSTUEXSANSSFL_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.quesfiletype = lastelement;
                    console.log("data : " + data);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.quesdocument_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.LPSTUEXSANSSFL_FilePath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.AddSubjectiveStaffFiles = function () {
            $scope.Temp_files = [];
            angular.forEach($scope.staff_subjective_teacherdocuupload, function (d) {
                if (d.LPSTUEXSANSSFL_FilePath !== null && d.LPSTUEXSANSSFL_FilePath !== "") {
                    $scope.Temp_files.push({
                        LPSTUEXSANSSFL_FilePath: d.LPSTUEXSANSSFL_FilePath, LPSTUEXSANSSFL_FileName: d.LPSTUEXSANSSFL_FileName,
                        LPSTUEXSANS_Id: $scope.LPSTUEXSANS_Id_Temp
                    });
                }
            });

            angular.forEach($scope.Exam_Level_Details, function (level) {
                angular.forEach(level.Level_Questions_List, function (level_ques) {
                    if (level_ques.LPSTUEXSANS_Id === $scope.LPSTUEXSANS_Id_Temp) {
                        level_ques.Temp_Staff_Ques_Subjective_Files = $scope.Temp_files;
                    }
                });
            });

            $scope.staff_subjective_teacherdocuupload = [];
            $scope.staff_subjective_teacherdocuupload = [{ id: 'Teacher1' }];
        };

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };

        $scope.onPastedecimal = function (e) {
            var regex = /[0-9]|\./;
            var text = event.clipboardData.getData("text/plain");
            if (!regex.test(text)) {
                e.preventDefault()
            }
        };

        // View Multiple Files and Download Single File 
        $scope.ViewAnswersheet = function (user, filename, filepath, files) {

            $scope.studentname = user.STUDENTNAME;
            var img = filepath;
            var imagarr = img.split('.');
            var lastelement = imagarr[imagarr.length - 1];

            if (lastelement === 'jpg' || lastelement === 'png' || lastelement === 'jpeg' || lastelement === 'gif' || lastelement === 'svg') {
                $scope.showmothersign(filepath);
            }
            else if (lastelement === 'pdf') {
                $scope.onview(filepath, filename);
            }
            else if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xls'
                || lastelement === 'xlsx') {
                $scope.onviewdocuments(filepath, filename);
            }
            else if (lastelement === 'mp4') {
                $scope.showGuardianPhotonew(files);
            }
        };

        $scope.DownLoadAnswerSheet = function (user, filename, filepath, files) {
            $scope.studentname = user.STUDENTNAME + "_" + user.AMST_AdmNo.replace("/", "-");
            $scope.downloaddirectimage(filepath, filename);
        };

        //View Subject Wise File And Download File
        $scope.ViewAnswersheet_SubjectWise = function (filename, filepath, ) {

            $scope.studentname = $scope.studentname;
            var img = filepath;
            var imagarr = img.split('.');
            var lastelement = imagarr[imagarr.length - 1];

            if (lastelement === 'jpg' || lastelement === 'png' || lastelement === 'jpeg' || lastelement === 'gif' || lastelement === 'svg') {
                $scope.showmothersign(filepath);
            }
            else if (lastelement === 'pdf') {
                $scope.onview(filepath, filename);
            }
            else if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xls'
                || lastelement === 'xlsx') {
                $scope.onviewdocuments(filepath, filename);
            }
            else if (lastelement === 'mp4') {
                $scope.showGuardianPhotonew(files);
            }
        };

        $scope.DownLoadAnswerSheet_SubjectWise = function (filename, filepath) {
            $scope.studentname = $scope.studentname + "_" + $scope.studamno.replace("/", "-");
            $scope.downloaddirectimage(filepath, filename);
        };

        $scope.ViewSubjectiveFiles = function (objs_files, questiondetails) {
            $scope.subjective_files = objs_files;
            $scope.question_subjective_files_details_temp = questiondetails;
            $scope.LPSTUEXSANS_Id_Temp = questiondetails.LPSTUEXSANS_Id;

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

                $scope.staff_subjective_teacherdocuupload = $scope.Temp_Questions_Subjective_Staff_files;
            } else {
                $scope.staff_subjective_teacherdocuupload = [];
                $scope.staff_subjective_teacherdocuupload = [{ id: 'Teacher1' }];
            }

            $('#mymodal_subjectivefiles').modal('show');
        };

        //Merge Multiple Files And Download Into Single File
        $scope.DownloadFiles = function (user_details) {

            $scope.tempfiles = [];
            var order = 0;
            angular.forEach(user_details.docs_list, function (dd) {
                order = order + 1;
                dd.FileOrder = order;
            });

            var data = {
                "AMST_Id": user_details.AMST_Id,
                "MergeFilesDTO": user_details.docs_list,
                "StudentName": user_details.STUDENTNAME,
                "LPMOEEX_Id": $scope.LPMOEEX_Id,
                "LPSTUEX_Id": user_details.LPSTUEX_Id
            };

            apiService.create("ImageUpload/OnlineExamMergeFiles", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.filepath !== null && promise.filepath !== "") {
                        $scope.studentname = user_details.STUDENTNAME + "_" + user_details.AMST_AdmNo.replace("/", "-");
                        var studentreg = "";
                        $scope.imagedownload = data;
                        imagedownload = promise.filepath;

                        if ($scope.studentname !== undefined && $scope.studentname !== null && $scope.studentname !== '') {
                            studentreg = $scope.studentname + ".pdf";
                        }
                        var dd = promise.filepath;
                        $http.get(imagedownload, {
                            responseType: "arraybuffer"
                        })
                            .success(function (dd) {
                                var anchor = angular.element('<a/>');
                                var blob = new Blob([dd]);
                                anchor.attr({
                                    href: window.URL.createObjectURL(blob),
                                    target: '_blank',
                                    download: studentreg
                                })[0].click();
                            });

                        //$timeout(function () { $scope.DeleteMergedFolder(user_details.AMST_Id, user_details.STUDENTNAME); }, 1000);                        
                    }
                }
            });
        };

        $scope.DeleteMergedFolder = function (AMST_Id, StudentName) {
            var data = {
                "AMST_Id": AMST_Id,
                "StudentName": StudentName
            };
            apiService.create("ImageUpload/DeleteMergedFolder", data).then(function (promise) {


            });
        };

        $scope.hidemarkslist = function () {
            $scope.showmarksgrid = false;
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
                        scope.model += newVal[0];
                        var editor = $("#editor").data("kendoEditor");
                        editor.value(scope.model);
                    }
                });
            }
        };
    });
})();