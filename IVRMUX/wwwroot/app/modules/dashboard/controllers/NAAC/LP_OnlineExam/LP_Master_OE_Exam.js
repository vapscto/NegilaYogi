(function () {
    'use strict';
    angular.module('app').controller('LP_OnlineExamMasterExamController', LP_OnlineExamMasterExamController)
    LP_OnlineExamMasterExamController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter', '$q', '$sce', '$window']
    function LP_OnlineExamMasterExamController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter, $q, $sce, $window) {

        $scope.groups = [{ title: 'Dynamic Group Header - 1', content: 'Dynamic Group Body - 1' },
        { title: 'Dynamic Group Header - 2', content: 'Dynamic Group Body - 2' },
        { title: 'Dynamic Group Header - 3', content: 'Dynamic Group Body - 3' },
        { title: 'Dynamic Group Header - 4', content: 'Dynamic Group Body - 4' }];

        $scope.searc_button = true;
        $scope.show = false;
        $scope.modlanguage = ""; 

        $scope.alphabetsarray = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
        $scope.sortKey = 'LMSMOEQ_Id';
        $scope.sortReverse = true;
        $scope.topicuploadexam = false;
        $scope.GetCartDetails = [];
        $scope.getexamhappenedcounttemp = false;
        $scope.LPMOEEX_AllowDownloadQnsPaperBeforeExamFlg = false;
        $scope.answer = "";
        $scope.show_ansOption = false;
        $scope.obj = {};
        $scope.objs = {};
        $scope.obj.searchValueddd = "";
        $scope.LPMOEEX_RandomFlg = false;
        $scope.LPMOEEX_UploadExamPaperFlg = false;
        $scope.LPMOEEX_AutoPublishFlg = false;
        $scope.Subjectivefilter = false;
        $scope.minDatefP = new Date();
        $scope.searchchkbxsec = "";
        $scope.searchchkbx = "";

        $scope.format = '1';
        $scope.obj.noraml = true;
        $scope.obj.mathtype = false;
        $scope.obj.LPMOEQ_StructuralFlg = false;

        $scope.obj.LPMOEEXQNS_SubjectiveFlg = false;
        $scope.obj.LPMOEEXQNS_MatchTheFollowingFlg = false;

        $scope.NoOfRows = 4;
        $scope.NoOfColumns = 4;
        $scope.LPMOEEXQNS_NoOfOptions = 4;
        $scope.totalgrid = [];
        $scope.GetQuestions_TempData = [];

        $scope.language = "";
        //$scope.modlanguage = "kn";

        $scope.teacherdocuupload = {};
        $scope.teacherdocuupload = [{ id: 'Teacher1' }];

        $scope.teacherdocuuploadopts = {};
        $scope.teacherdocuuploadopts = [{ id: 'Teacheropts1' }];

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.currentPage1 = 1;
        $scope.edit = false;

        $scope.itemsPerPage1 = paginationformasters;


        $scope.getlanguagepage = function (lang) {
            if (lang != "") {
                $scope.language = lang;
                var langurl = "https://dcampusstrg.blob.core.windows.net/language/" + lang + ".html";
                $scope.langurl = $sce.trustAsResourceUrl(langurl);
            }
        };

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("LP_OnlineExam/getexammasterload", pageid).then(function (promise) {
                $scope.yearlist = promise.getyearlist;
                $scope.getConfigurationSettings = promise.getConfigurationSettings;
                $scope.getMasterExamQuestiondetails = promise.getMasterExamQuestiondetails;
                $scope.getarratcomplexities = promise.getarratcomplexities;

                $scope.ASMAY_Id = promise.asmaY_Id;
                $scope.OnchangeNoofOptions();
                $timeout(function () { $scope.addeditors(); }, 1000);
                $timeout(function () { $scope.getexamclasslist('OnLoad'); }, 1000);
            });
        };

        $scope.getexamclasslist = function (onchangeoronload) {
            $scope.totalgrid = [];
            $scope.GetCartDetails = [];
            $scope.ASMCL_Id = "";
            $scope.classlist = [];
            $scope.ISMS_Id = "";
            $scope.getSubjects = [];
            $scope.getmasterexamdetails = [];
            $scope.EME_Id = "";
            $scope.LPMT_Id = "";
            $scope.gettopiclist = [];
            $scope.LPMOEQ_Question = "";
            $scope.LPMOEQ_QuestionDesc = "";
            $scope.LPMOEQ_Marks = "";
            $scope.questiontemp = [];
            $scope.temptopics = [];
            $scope.getquestionlist = [];
            $scope.tempgetquestionlist = [];
            $scope.topicuploadexam = false;
            $scope.getsectionlist = [];
            $scope.selectedsection = "";
            $scope.searchchkbxsec = "";
            $scope.searchchkbx = "";
            $scope.totalgrid = [];
            $scope.GetQuestions_TempData = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("LP_OnlineExam/getexamclasslist", data).then(function (promise) {
                if (promise.getclasslist !== null && promise.getclasslist.length > 0) {
                    $scope.classlist = promise.getclasslist;

                    //if (onchangeoronload === "OnLoad") {
                    $scope.ASMCL_Id = $scope.classlist[0].asmcL_Id;
                    $scope.OnchangeNoofOptions();
                    $timeout(function () { $scope.getexamsectionslist('OnLoad'); }, 1000);
                    //}
                }
            });
        };

        $scope.getexamsectionslist = function (onchangeoronload) {
            $scope.getsectionlist = [];
            $scope.selectedsection = "";
            $scope.EME_Id = "";
            $scope.getmasterexamdetails = [];
            $scope.searchchkbxsec = "";
            $scope.searchchkbx = "";
            $scope.ISMS_Id = "";
            $scope.getSubjects = [];
            $scope.gettopiclist = [];
            $scope.totalgrid = [];
            $scope.GetQuestions_TempData = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            };

            apiService.create("LP_OnlineExam/getexamsectionslist", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getsectionlist !== null && promise.getsectionlist.length > 0) {
                        $scope.getsectionlist = promise.getsectionlist;
                        angular.forEach($scope.getsectionlist, function (sec) {
                            sec.checkedsec = true;
                        });
                        $scope.OnchangeNoofOptions();
                        $timeout(function () { $scope.getexamsubjectlist('OnLoad'); }, 1000);
                    }
                }
            });
        };

        $scope.getexamsubjectlist = function (onchangeoronload) {
            $scope.totalgrid = [];
            $scope.questiontemp = [];
            $scope.temptopics = [];
            $scope.getquestionlist = [];
            $scope.tempgetquestionlist = [];
            $scope.GetCartDetails = [];
            $scope.topicuploadexam = false;
            $scope.getSubjects = [];
            $scope.getmasterexamdetails = [];
            $scope.EME_Id = "";
            $scope.sectiondetailslist = [];
            $scope.searchchkbxsec = "";
            $scope.searchchkbx = "";
            $scope.gettopiclist = [];
            $scope.totalgrid = [];
            $scope.GetQuestions_TempData = [];

            angular.forEach($scope.getsectionlist, function (d) {
                if (d.checkedsec) {
                    $scope.sectiondetailslist.push({ ASMS_Id: d.asmS_Id });
                }
            });

            if ($scope.sectiondetailslist.length === 0) {
                return;
            }

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "sectiondetailslist": $scope.sectiondetailslist
            };

            apiService.create("LP_OnlineExam/getexamsubjectlist", data).then(function (promise) {
                if (promise.getsubjectlist !== null && promise.getsubjectlist.length > 0) {
                    $scope.getSubjects = promise.getsubjectlist;
                }
                if (promise.getmasterexamdetails !== null && promise.getmasterexamdetails.length > 0) {
                    $scope.getmasterexamdetails = promise.getmasterexamdetails;
                }

                $scope.OnchangeNoofOptions();
                $scope.ISMS_Id = $scope.getSubjects[0].ismS_Id;
                $timeout(function () { $scope.SearchTopics('OnLoad'); }, 1000);

            });
        };

        $scope.SearchTopics = function (onchangeoronload) {
            $scope.temptopics = [];
            $scope.getquestionlist = [];
            $scope.tempgetquestionlist = [];
            $scope.GetCartDetails = [];
            $scope.topicuploadexam = false;
            $scope.sectiondetailslist = [];
            $scope.gettopiclist = [];
            $scope.searchchkbxsec = "";
            $scope.searchchkbx = "";
            $scope.totalgrid = [];
            $scope.GetQuestions_TempData = [];

            angular.forEach($scope.getsectionlist, function (d) {
                if (d.checkedsec) {
                    $scope.sectiondetailslist.push({ ASMS_Id: d.asmS_Id });
                }
            });

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ISMS_Id": $scope.ISMS_Id,
                "sectiondetailslist": $scope.sectiondetailslist
            };

            apiService.create("LP_OnlineExam/GetSearchTopics", data).then(function (promise) {
                if (promise.gettopiclist !== null && promise.gettopiclist.length > 0) {
                    $scope.gettopiclist = promise.gettopiclist;
                } else {
                    if ($scope.format == '0') {
                        swal("No Topics Mapped For Selected Details");
                    }
                }
                $scope.OnchangeNoofOptions();
            });
        };

        $scope.SearchQuestions = function (dd) {
            $scope.totalgrid = [];
            $scope.questiontemp = [];
            $scope.getquestionlist = [];
            $scope.tempgetquestionlist = [];
            $scope.temptopics = [];
            $scope.GetCartDetails = [];
            $scope.topicuploadexam = false;
            $scope.sectiondetailslist = [];
            $scope.totalgrid = [];
            $scope.GetQuestions_TempData = [];

            angular.forEach($scope.getsectionlist, function (d) {
                if (d.checkedsec) {
                    $scope.sectiondetailslist.push({ ASMS_Id: d.asmS_Id });
                }
            });

            angular.forEach($scope.gettopiclist, function (dd) {
                if (dd.checkedsub === true) {
                    $scope.temptopics.push({ LPMT_Id: dd.lpmT_Id });
                }
            });

            if ($scope.temptopics.length === 0) {
                return;
            }

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ISMS_Id": $scope.ISMS_Id,
                "temptopics": $scope.temptopics,
                "LPMOEEX_NoOfQuestion": $scope.LPMOEEX_NoOfQuestion,
                "LPMOEEX_RandomFlg": $scope.LPMOEEX_RandomFlg,
                "sectiondetailslist": $scope.sectiondetailslist
            };

            apiService.create("LP_OnlineExam/SearchQuestions", data).then(function (promise) {
                if (promise.message === 'Duplicate') {
                    swal("Exam Name Already Exists");
                    return;
                }

                if (promise.getquestionlist !== null && promise.getquestionlist.length > 0) {

                    $scope.getquestionlist = promise.getquestionlist;
                    $scope.vieweimages($scope.getquestionlist);
                    var count = 0;
                    angular.forEach($scope.getquestionlist, function (dd) {
                        count = count + 1;
                        dd.lpmoeexqnS_QnsOrder = count;
                        dd.lpmoeexqnS_Marks = dd.lpmoeQ_Marks
                        dd.disable = false;
                    });

                    $scope.tempgetquestionlist = $scope.getquestionlist;

                    $scope.topicuploadexam = true;
                } else {
                    $scope.topicuploadexam = false;
                    swal("No Questions Mapped For This Class And Subject");
                }
                $scope.OnchangeNoofOptions();
            });
        };

        $scope.OnChangeRandomFlag = function (dd) {
            $scope.temptopics = [];
            $scope.totalgrid = [];
            $scope.questiontemp = [];
            $scope.getquestionlist = [];
            $scope.tempgetquestionlist = [];
            angular.forEach($scope.gettopiclist, function (dd) {
                if (dd.checkedsub === true) {
                    $scope.temptopics.push({ LPMT_Id: dd.lpmT_Id });
                }
            });

            if ($scope.temptopics.length === 0) {
                return;
            }

            $scope.sectiondetailslist = [];

            //angular.forEach(dd, function (d) {
            //    $scope.sectiondetailslist.push({ ASMS_Id: d });
            //});
            angular.forEach($scope.getsectionlist, function (d) {
                if (d.checkedsec) {
                    $scope.sectiondetailslist.push({ ASMS_Id: d.asmS_Id });
                }
            });

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ISMS_Id": $scope.ISMS_Id,
                "temptopics": $scope.temptopics,
                "LPMOEEX_NoOfQuestion": $scope.LPMOEEX_NoOfQuestion,
                "LPMOEEX_RandomFlg": $scope.LPMOEEX_RandomFlg,
                "sectiondetailslist": $scope.sectiondetailslist
            };

            apiService.create("LP_OnlineExam/SearchQuestions", data).then(function (promise) {
                if (promise.message === 'Duplicate') {
                    swal("Exam Name Already Exists");
                    return;
                }

                if (promise.getquestionlist !== null && promise.getquestionlist.length > 0) {
                    $scope.getquestionlist = promise.getquestionlist;
                    $scope.vieweimages($scope.getquestionlist);
                    $scope.tempgetquestionlist = promise.tempgetquestionlist;
                } else {
                    swal("No Questions Mapped For This Class And Subject");
                }
            });
        };

        $scope.vieweimages = function (arraylist) {
            angular.forEach(arraylist, function (dd, index) {
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
        }

        $scope.OnChangeMarks = function (objmarks) {
            if (parseFloat(objmarks.lpmoeexqnS_Marks) === 0) {
                objmarks.lpmoeexqnS_Marks = "";
                swal("Marks Should Be Greather Than 0");
            }

            $scope.gettotal_count_marks();
        };

        $scope.OnChangeFromDate = function () {
            $scope.LPMOEEX_ToDateTime = null;
            $scope.LPMOEEX_ToDateTime = $scope.LPMOEEX_FromDateTime;
        };

        $scope.OnClickUploadFlag = function (flag) {
            $scope.getquestionlist = [];
            $scope.tempgetquestionlist = [];
            angular.forEach($scope.gettopiclist, function (dd) {
                dd.checkedsub = false;
                if (flag === true) {
                    dd.uploadexamflag = true;
                } else {
                    dd.uploadexamflag = false;
                }
            });
        };

        /* Add To Cart Details */
        $scope.AddExamMasterDetails = function (dd) {
            if ($scope.myForm.$valid) {

                $scope.btn = false;
                $scope.tempselectedquestions = [];
                $scope.levelquestionwisemrks = 0;
                var order = 0;

                if ($scope.format === "0") {
                    angular.forEach($scope.getquestionlist, function (dd) {
                        if (dd.checked) {
                            order += 1;
                            $scope.levelquestionwisemrks += Number(dd.lpmoeexqnS_Marks);
                            $scope.tempselectedquestions.push({
                                LPMOEQ_Id: dd.lpmoeQ_Id, LPMOEEXQNS_Marks: dd.lpmoeexqnS_Marks, LPMOEEXQNS_Id: dd.lpmoeexqnS_Id,
                                LPMOEEXQNS_QnsOrder: order, lpmoeQ_Question: dd.lpmoeQ_Question
                            });
                        }
                    });
                }
                else if ($scope.format === "1") {

                    $scope.levelquestionwisemrks = $scope.GetQuestions_TempData.map(bill => Number(bill.LPMOEEXQNS_Marks)).reduce((acc, bill) => bill + acc);
                    $scope.tempselectedquestions = $scope.GetQuestions_TempData;
                }

                var count_final = 0;
                if ($scope.GetCartDetails.length > 0) {
                    var count = 0;
                    angular.forEach($scope.GetCartDetails, function (d, index) {
                        if ($scope.editindex !== index) {
                            if (d.LPMOEEXLVL_LevelDesc === $scope.objs.LPMOEEXLVL_LevelDesc) {
                                count += 1;
                            }
                            if (Number(d.LPMOEEXLVL_LevelOrder) === Number($scope.objs.LPMOEEXLVL_LevelOrder)) {
                                count += 1;
                            }
                        }
                    });

                    if (count === 0) {
                        if ($scope.levelquestionwisemrks !== Number($scope.objs.LPMOEEXLVL_LevelTotalMarks)) {
                            swal("Marks Should Be Equal To Level Total Marks");
                            count_final += 1;
                            return;
                        }
                        if (Number($scope.objs.LPMOEEXLVL_TotalNoOfQns) !== $scope.tempselectedquestions.length) {
                            swal("Selected Questions Should Be Equal To Total No.Of Questions");
                            count_final += 1;
                            return;
                        }

                    } else {
                        $scope.btn = true;
                        swal("Level Name Or Order Already Exists");
                    }
                }

                else {
                    if ($scope.levelquestionwisemrks !== Number($scope.objs.LPMOEEXLVL_LevelTotalMarks)) {
                        swal("Marks Should Be Equal To Level Total Marks");
                        count_final += 1;
                        return;
                    }
                    if (Number($scope.objs.LPMOEEXLVL_TotalNoOfQns) !== $scope.tempselectedquestions.length) {
                        swal("Selected Questions Should Be Equal To Total No.Of Questions");
                        count_final += 1;
                        return;
                    }
                }

                if (count_final === 0) {

                    if ($scope.editindex_temp > 0) {
                        $scope.GetCartDetails.splice($scope.editindex, 1);
                    }

                    var LPMOEEXLVL_MarksPerQns = $scope.objs.LPMOEEXLVL_MarksPerQns === undefined || $scope.objs.LPMOEEXLVL_MarksPerQns === null ||
                        $scope.objs.LPMOEEXLVL_MarksPerQns === "" ? 0 : $scope.objs.LPMOEEXLVL_MarksPerQns;

                    var LPMOEEXLVL_MaxQns = $scope.objs.LPMOEEXLVL_MaxQns === undefined || $scope.objs.LPMOEEXLVL_MaxQns === null ||
                        $scope.objs.LPMOEEXLVL_MaxQns === "" ? 0 : $scope.objs.LPMOEEXLVL_MaxQns;

                    $scope.GetCartDetails.push({
                        LPMOEEXLVL_LevelDesc: $scope.objs.LPMOEEXLVL_LevelDesc, LPMOEEXLVL_TotalNoOfQns: $scope.objs.LPMOEEXLVL_TotalNoOfQns,
                        LPMOEEXLVL_MaxQns: LPMOEEXLVL_MaxQns, LPMOEEXLVL_LevelTotalMarks: $scope.objs.LPMOEEXLVL_LevelTotalMarks,
                        LPMOEEXLVL_MarksPerQns: LPMOEEXLVL_MarksPerQns ,
                        LPMOEEXLVL_LevelOrder: $scope.objs.LPMOEEXLVL_LevelOrder,
                        questionlist: $scope.tempselectedquestions
                    });

                    $scope.clearadddetails();

                    $scope.editindex = null;
                    $scope.editindex_temp = null;

                    $scope.getquestionlist = $scope.tempgetquestionlist;
                    angular.forEach($scope.getquestionlist, function (dd) {

                        angular.forEach($scope.GetCartDetails, function (d) {
                            angular.forEach(d.questionlist, function (ddd) {
                                if (dd.lpmoeQ_Id === ddd.LPMOEQ_Id) {
                                    dd.lpmoeexqnS_Marks = "";
                                    dd.disable = true;
                                    dd.checked = false;
                                }
                            });
                        });
                    });

                    var total_marks_incart = 0;
                    var total_questions_incart = 0;

                    $scope.leveltotalquestions = "";
                    $scope.leveltotalquestionsmarks = "";

                    angular.forEach($scope.GetCartDetails, function (dd) {
                        total_questions_incart += dd.questionlist.length;
                        angular.forEach(dd.questionlist, function (d) {
                            total_marks_incart += Number(d.LPMOEEXQNS_Marks);
                        });
                    });

                    $scope.totalquestions = total_questions_incart;
                    $scope.totalquestionsmarks = total_marks_incart;

                    $scope.GetQuestions_TempData = [];
                }

                console.log($scope.GetCartDetails);
            } else {
                $scope.btn = true;
                $scope.submitted1 = true;
            }
        };

        $scope.OnChangeComplexities = function (dd) {
            var countques = 0;
            $scope.getquestionlist = [];
            angular.forEach(dd, function (d) {
                angular.forEach($scope.tempgetquestionlist, function (ques) {
                    if (d === ques.lpmcomP_Id) {
                        countques += 1;
                        $scope.getquestionlist.push(ques);
                    }
                });
            });

            if (countques === 0 && dd.length === 0) {
                $scope.getquestionlist = $scope.tempgetquestionlist;
            }
        };

        /* Search Filter Based On Complexitites or subjective flag */
        $scope.SearchQuestionfilter = function (selected, subjectiveflag) {

            $scope.getquestionlist = [];
            $scope.temptopics = [];
            $scope.tempgetquestionlist = [];
            $scope.tempcomplexitites = [];
            angular.forEach(selected, function (dd) {
                $scope.tempcomplexitites.push({ LPMCOMP_Id: dd });
            });


            $scope.sectiondetailslist = [];
            //angular.forEach($scope.selectedsection, function (d) {
            //    $scope.sectiondetailslist.push({ ASMS_Id: d });
            //});

            angular.forEach($scope.getsectionlist, function (d) {
                if (d.checkedsec) {
                    $scope.sectiondetailslist.push({ ASMS_Id: d.asmS_Id });
                }
            });

            angular.forEach($scope.gettopiclist, function (dd) {
                if (dd.checkedsub === true) {
                    $scope.temptopics.push({ LPMT_Id: dd.lpmT_Id });
                }
            });

            if ($scope.temptopics.length === 0) {
                return;
            }

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ISMS_Id": $scope.ISMS_Id,
                "tempcomplexitites": $scope.tempcomplexitites,
                "subjectiveflag": subjectiveflag,
                "temptopics": $scope.temptopics,
                "LPMOEEX_NoOfQuestion": $scope.LPMOEEX_NoOfQuestion,
                "LPMOEEX_RandomFlg": $scope.LPMOEEX_RandomFlg,
                "sectiondetailslist": $scope.sectiondetailslist
            };

            apiService.create("LP_OnlineExam/SearchQuestionfilter", data).then(function (promise) {
                if (promise.getquestionlist !== null && promise.getquestionlist.length > 0) {

                    $scope.getquestionlist = promise.getquestionlist;
                    $scope.vieweimages($scope.getquestionlist);
                    var count = 0;
                    angular.forEach($scope.getquestionlist, function (dd) {
                        count = count + 1;
                        dd.lpmoeexqnS_QnsOrder = count;
                        dd.lpmoeexqnS_Marks = dd.lpmoeQ_Marks;
                        dd.checked = false;
                    });

                    $scope.tempgetquestionlist = $scope.getquestionlist;

                    if ($scope.GetCartDetails !== null && $scope.GetCartDetails.length > 0) {
                        angular.forEach($scope.getquestionlist, function (dd) {
                            angular.forEach($scope.GetCartDetails, function (d) {
                                angular.forEach(d.questionlist, function (ddd) {
                                    if (dd.lpmoeQ_Id === dd.LPMOEQ_Id) {
                                        dd.lpmoeexqnS_Marks = "";
                                        dd.disable = true;
                                        dd.checked = false;
                                    }
                                });
                            });
                        });
                    }
                    $scope.topicuploadexam = true;
                } else {
                    $scope.topicuploadexam = false;
                    swal("No Questions Mapped For This Class And Subject");
                }
            });
        };

        $scope.clearadddetails = function () {
            $scope.objs.LPMOEEXLVL_LevelDesc = "";
            $scope.objs.LPMOEEXLVL_TotalNoOfQns = "";
            $scope.objs.LPMOEEXLVL_MaxQns = "";
            $scope.objs.LPMOEEXLVL_LevelTotalMarks = "";
            $scope.objs.LPMOEEXLVL_MarksPerQns = "";
            $scope.objs.LPMOEEXLVL_LevelOrder = "";
            $scope.tempselectedquestions = [];
            $scope.submitted1 = false;
            $scope.searchchkbxsec = "";
            $scope.searchchkbx = "";
            angular.forEach($scope.getquestionlist, function (dd) {
                dd.checked = false;
                //dd.lpmoeexqnS_Marks = "";
            });
        };

        $scope.optionToggled_S = function (ques) {
            if ($scope.editindex_temp > 0) {
                angular.forEach($scope.GetCartDetails, function (dd, index) {
                    if (index !== $scope.editindex) {
                        angular.forEach(dd.questionlist, function (d) {
                            if (parseInt(d.LPMOEQ_Id) === parseInt(ques.lpmoeQ_Id)) {
                                swal("Question Is Already Add To Cart, You Cannot Add Same Question Again");
                                ques.checked = false;
                                ques.lpmoeexqnS_Marks = "";
                            }
                        });
                    }
                });
            } else {
                angular.forEach($scope.GetCartDetails, function (dd) {
                    angular.forEach(dd.questionlist, function (d) {
                        if (parseInt(d.LPMOEQ_Id) === parseInt(ques.lpmoeQ_Id)) {
                            swal("Question Is Already Add To Cart, You Cannot Add Same Question Again");
                            ques.checked = false;
                            ques.lpmoeexqnS_Marks = "";
                        }
                    });
                });
            }

            $scope.gettotal_count_marks();

            //  $scope.obj.checkall = $scope.getquestionlist.every(function (itm) { return itm.checked; });
        };

        $scope.gettotal_count_marks = function () {
            var countlevel_ques = 0;
            var level_quesmarks = 0;
            angular.forEach($scope.getquestionlist, function (dd) {
                if (dd.checked) {
                    countlevel_ques += 1;
                    if (dd.lpmoeexqnS_Marks !== undefined && dd.lpmoeexqnS_Marks !== null && dd.lpmoeexqnS_Marks !== "" && dd.lpmoeexqnS_Marks !== "NaN") {
                        level_quesmarks += Number(dd.lpmoeexqnS_Marks);
                    }
                }
            });

            $scope.leveltotalquestions = countlevel_ques;
            $scope.leveltotalquestionsmarks = level_quesmarks;

        };

        $scope.DeleteLevelDetails = function (dd, index) {
            $scope.GetCartDetails.splice(index, 1);

            $scope.getquestionlist = $scope.tempgetquestionlist;

            angular.forEach($scope.getquestionlist, function (dd) {
                dd.lpmoeexqnS_Marks = "";
                dd.disable = false;
                dd.checked = false;
                angular.forEach($scope.GetCartDetails, function (d) {
                    angular.forEach(d.questionlist, function (ddd) {
                        if (dd.lpmoeQ_Id === ddd.LPMOEQ_Id) {
                            dd.lpmoeexqnS_Marks = "";
                            dd.disable = true;
                            dd.checked = false;
                        }
                    });
                });
            });
        };

        $scope.ViewLevelQuestons = function (dd, index) {
            $scope.levelname = dd.LPMOEEXLVL_LevelDesc;
            $scope.level_notlinked = $scope.format;
            $scope.cartquestiondetails = dd.questionlist;
            $scope.vieweimages($scope.cartquestiondetails);
            $('#myModalviewaddcartdetails').modal('show');
        };

        $scope.EditLevelQuestons = function (dd, index) {

            $scope.editindex = index;
            $scope.editindex_temp = index + 1;

            $scope.objs.LPMOEEXLVL_LevelDesc = dd.LPMOEEXLVL_LevelDesc;
            $scope.objs.LPMOEEXLVL_LevelOrder = dd.LPMOEEXLVL_LevelOrder;
            $scope.objs.LPMOEEXLVL_LevelTotalMarks = dd.LPMOEEXLVL_LevelTotalMarks;
            $scope.objs.LPMOEEXLVL_MaxQns = dd.LPMOEEXLVL_MaxQns;
            $scope.objs.LPMOEEXLVL_TotalNoOfQns = dd.LPMOEEXLVL_TotalNoOfQns;
            $scope.objs.LPMOEEXLVL_MarksPerQns = dd.LPMOEEXLVL_MarksPerQns;

            if ($scope.format === "0") {
                angular.forEach(dd.questionlist, function (d) {
                    angular.forEach($scope.getquestionlist, function (q) {
                        if (d.LPMOEQ_Id === q.lpmoeQ_Id) {
                            q.lpmoeexqnS_Marks = d.LPMOEEXQNS_Marks;
                            q.lpmoeexqnS_QnsOrder = d.LPMOEEXQNS_QnsOrder;
                            q.lpmoeexqnS_Id = d.LPMOEEXQNS_Id == undefined || d.LPMOEEXQNS_Id == null ? 0 : d.LPMOEEXQNS_Id;
                            q.checked = true;
                            q.disable = false;
                        }
                    });
                });
            } else if ($scope.format === "1") {
                $scope.GetQuestions_TempData = dd.questionlist;
            };


            $scope.edit_manual_ques_index = null;
            $scope.edit_temp_manual_ques_index = null;
            $scope.normalquestiontext = "";
            $scope.LPMOEQ_Marks = "";
            $scope.LPMOEEXQNS_NoOfOptions = 4;
            $scope.obj.noraml = true;
            $scope.obj.mathtype = false;
            $scope.obj.LPMOEQ_StructuralFlg = false;

            $scope.obj.LPMOEEXQNS_MatchTheFollowingFlg = false;
            $scope.obj.LPMOEEXQNS_SubjectiveFlg = false;

            $scope.totalgrid = [];
            // $scope.OnchangeNoofOptions();

            $scope.rows_array = [];
            $scope.colms_array = [];
           // $scope.ViewMatchTheFollowing();

            $scope.scroll_header();
        };

        $scope.getOrder = function (orderarray) {
            angular.forEach(orderarray, function (value, key) {
                orderarray[key].LPMOEEXQNS_QnsOrder = key + 1;
            });
            $('#myModal').modal('hide');
        };

        /* Save Details  */
        $scope.SaveMasterExamQuestionDetails = function (dd) {
            $scope.submitted1 = true;

            if ($scope.myForm.$valid) {

                $scope.markstotal = 0;
                $scope.questiontotal = 0;
                $scope.questiontemp = [];

                //angular.forEach($scope.getquestionlist, function (dd) {
                //    if (dd.checked) {
                //        $scope.markstotal += parseFloat(dd.lpmoeexqnS_Marks);
                //        $scope.questiontemp.push({ LPMOEQ_Id: dd.lpmoeQ_Id, LPMOEEXQNS_Marks: dd.lpmoeexqnS_Marks, LPMOEEXQNS_Id: dd.lpmoeexqnS_Id });
                //    }
                //});


                $scope.sectiondetailslist = [];

                //angular.forEach(dd, function (d) {
                //    $scope.sectiondetailslist.push({ ASMS_Id: d });
                //});

                angular.forEach($scope.getsectionlist, function (d) {
                    if (d.checkedsec) {
                        $scope.sectiondetailslist.push({ ASMS_Id: d.asmS_Id });
                    }
                });

                angular.forEach($scope.GetCartDetails, function (dd) {
                    $scope.markstotal += Number(dd.LPMOEEXLVL_LevelTotalMarks);
                    $scope.questiontotal += Number(dd.LPMOEEXLVL_TotalNoOfQns);
                });



                if ($scope.LPMOEEX_UploadExamPaperFlg === false) {
                    if ($scope.GetCartDetails.length === 0) {
                        swal("Add Atleast One Record To Cart Save Details");
                        return;
                    }
                    if ($scope.questiontotal > Number($scope.LPMOEEX_NoOfQuestion)) {
                        swal("Select Question Should Be Equal To No Of Question");
                        return;
                    }
                    if ($scope.questiontotal < Number($scope.LPMOEEX_NoOfQuestion)) {
                        swal("Select Question Should Be Equal To No Of Question");
                        return;
                    }

                    if ($scope.markstotal !== Number($scope.LPMOEEX_TotalMarks)) {
                        swal("Sum Of Each Question Marks Should Be Equal To Total Marks");
                        return;
                    }
                }

                if ($scope.LPMOEEX_RandomFlg !== true) {
                    $scope.LPMOEEX_RandomFlg = false;
                }

                $scope.LPMOEEX_QuestionPaper = $scope.LPMOEEX_QuestionPaper !== undefined && $scope.LPMOEEX_QuestionPaper !== null
                    && $scope.LPMOEEX_QuestionPaper !== "" ? $scope.LPMOEEX_QuestionPaper : "";

                $scope.LPMOEEX_QuestionPapeFileName = $scope.LPMOEEX_QuestionPapeFileName !== undefined && $scope.LPMOEEX_QuestionPapeFileName !== null
                    && $scope.LPMOEEX_QuestionPapeFileName !== "" ? $scope.LPMOEEX_QuestionPapeFileName : "";

                $scope.LPMOEEX_AnswerSheet = $scope.LPMOEEX_AnswerSheet !== undefined && $scope.LPMOEEX_AnswerSheet !== null
                    && $scope.LPMOEEX_AnswerSheet !== "" ? $scope.LPMOEEX_AnswerSheet : "";

                $scope.LPMOEEX_AnswerPapeFileName = $scope.LPMOEEX_AnswerPapeFileName !== undefined && $scope.LPMOEEX_AnswerPapeFileName !== null
                    && $scope.LPMOEEX_AnswerPapeFileName !== "" ? $scope.LPMOEEX_AnswerPapeFileName : "";

                $scope.temptpoics = [];
                angular.forEach($scope.gettopiclist, function (dd) {
                    if (dd.checkedsub === true) {
                        dd.lpmoeextoP_Id = dd.lpmoeextoP_Id === undefined ? 0 : dd.lpmoeextoP_Id;
                        $scope.temptpoics.push({ LPMT_Id: dd.lpmT_Id, LPMOEEXTOP_Id: dd.lpmoeextoP_Id });
                    }
                });

                $scope.ScheduleTime = $scope.ScheduleTime_24;
                var ScheduleTime = $filter('date')($scope.ScheduleTime, "HH");
                var ScheduleTimem = $filter('date')($scope.ScheduleTime, "mm");
                var ScheduleTimesec = "00";

                $scope.ScheduleTimeTo = $scope.ScheduleTimeTo_24;
                var ScheduleTimeTo = $filter('date')($scope.ScheduleTimeTo, "HH");
                var ScheduleTimeTom = $filter('date')($scope.ScheduleTimeTo, "mm");
                var ScheduleTimeTosec = "00";

                var from_month = new Date($scope.LPMOEEX_FromDateTime).getMonth() + 1;
                var from_day = new Date($scope.LPMOEEX_FromDateTime).getDate();
                var from_year = new Date($scope.LPMOEEX_FromDateTime).getFullYear();

                var to_month = new Date($scope.LPMOEEX_ToDateTime).getMonth() + 1;
                var to_day = new Date($scope.LPMOEEX_ToDateTime).getDate();
                var to_year = new Date($scope.LPMOEEX_ToDateTime).getFullYear();

                var timeStart = new Date(from_year + ',' + from_month + ',' + from_day + ' ' + ScheduleTime + ':' + ScheduleTimem + ':' + ScheduleTimesec).getTime();
                var timeEnd = new Date(to_year + ',' + to_month + ',' + to_day + ' ' + ScheduleTimeTo + ':' + ScheduleTimeTom + ':' + ScheduleTimeTosec).getTime();

                var hourDiff = timeEnd - timeStart; //in ms
                var secDiff = hourDiff / 1000; //in s
                var minDiff = hourDiff / 60 / 1000; //in minutes
                var hDiff = hourDiff / 3600 / 1000; //in hours

                var humanReadable = {};
                humanReadable.hours = Math.floor(hDiff);
                humanReadable.minutes = minDiff - 60 * humanReadable.hours;
                humanReadable.total = (humanReadable.hours * 60) + (humanReadable.minutes);
                console.log(humanReadable);

                if (humanReadable.total > 0) {
                    if (humanReadable.total !== parseInt($scope.LPMOEEX_ExamDuration)) {
                        swal("Difference Between Exam Start Time And End Time Should Be Equal To Total Duration Of Exam");
                        return;
                    }
                }
                else {
                    swal("Exam Start Time Should Be Less Than End Time");
                    return;
                }
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
                                "LPMOEEX_Id": $scope.LPMOEEX_Id,
                                "ASMAY_Id": $scope.ASMAY_Id,
                                "ASMCL_Id": $scope.ASMCL_Id,
                                "ISMS_Id": $scope.ISMS_Id,
                                "LPMOEEX_ExamName": $scope.LPMOEEX_ExamName,
                                "LPMOEEX_NoOfQuestion": $scope.LPMOEEX_NoOfQuestion,
                                "LPMOEEX_TotalMarks": $scope.LPMOEEX_TotalMarks,
                                "LPMOEEX_ExamDuration": $scope.LPMOEEX_ExamDuration,
                                "LPMOEEX_RandomFlg": $scope.LPMOEEX_RandomFlg,
                                "LPMOEEX_UploadExamPaperFlg": $scope.LPMOEEX_UploadExamPaperFlg,
                                "tempquestiondto": $scope.questiontemp,
                                "LPMOEEX_QuestionPaper": $scope.LPMOEEX_QuestionPaper,
                                "LPMOEEX_QuestionPapeFileName": $scope.LPMOEEX_QuestionPapeFileName,
                                "LPMOEEX_AnswerSheet": $scope.LPMOEEX_AnswerSheet,
                                "LPMOEEX_AnswerPapeFileName": $scope.LPMOEEX_AnswerPapeFileName,
                                "LPMOEEX_AutoPublishFlg": $scope.LPMOEEX_AutoPublishFlg,
                                "fhrors": ScheduleTime,
                                "thrors": ScheduleTimeTo,
                                "fminutes": ScheduleTimem,
                                "tminutes": ScheduleTimeTom,
                                "fsec": ScheduleTimesec,
                                "tsec": ScheduleTimeTosec,
                                "temptopicDTO": $scope.temptpoics,
                                "LPMOEEX_FromDateTime": new Date($scope.LPMOEEX_FromDateTime).toDateString(),
                                "LPMOEEX_ToDateTime": new Date($scope.LPMOEEX_ToDateTime).toDateString(),
                                "ExamLevelDetails": $scope.GetCartDetails,
                                "sectiondetailslist": $scope.sectiondetailslist,
                                "EME_Id": $scope.EME_Id,
                                "LPMOEEX_AllowDownloadQnsPaperBeforeExamFlg": $scope.LPMOEEX_AllowDownloadQnsPaperBeforeExamFlg,
                                "LPMOEEX_DurationFlag": $scope.LPMOEEX_DurationFlag == undefined || $scope.LPMOEEX_DurationFlag == null
                                    || $scope.LPMOEEX_DurationFlag == "" ? "" : $scope.LPMOEEX_DurationFlag,
                                "LPMOEEX_Duration": $scope.LPMOEEX_Duration == undefined || $scope.LPMOEEX_Duration == null
                                    || $scope.LPMOEEX_Duration == "" ? "" : $scope.LPMOEEX_Duration,
                                "LPMOEEX_NotLinkedToQnsBankFlg": $scope.format === "1" ? true : false
                            };

                            apiService.create("LP_OnlineExam/SaveMasterExamQuestionDetails", data).then(function (promise) {
                                if (promise.message === 'Duplicate') {
                                    swal('Record Already Exist');
                                } else if (promise.message === 'TimeSlot') {
                                    swal("Time Slot Already Exists");
                                } else {
                                    $scope.duplicatemessage = "";
                                    if (promise.duplicatemessage !== undefined && promise.duplicatemessage !== null && promise.duplicatemessage !== "") {
                                        alert("Time Slot Is For This Sections " + promise.duplicatemessage + " Already Mapped");
                                    }
                                    if (promise.message === "Add") {
                                        if (promise.returnval === true) {
                                            swal("Record Saved Successfully");
                                        } else {
                                            swal("Failed To Save Record");
                                        }
                                    } else if (promise.message === "Update") {
                                        if (promise.returnval === true) {
                                            swal("Record Updated Successfully");
                                        } else {
                                            swal("Failed To Update Record");
                                        }
                                    } else {
                                        swal("Failed To Save/Update Record");
                                    }
                                }
                                //if ($scope.LPMOEEX_Id > 0) {
                                $state.reload();
                                //} else {
                                //    $scope.ClearSaveData();
                                //}
                            });
                        }
                        else {
                            swal("Submittion Cancelled");
                        }
                    });
            }
            else {
                $scope.submitted1 = true;
            }
        };

        $scope.ClearSaveData = function () {
            $scope.ISMS_Id = "";
            $scope.gettopiclist = [];
            $scope.LPMOEEX_ExamName = "";
            $scope.LPMOEEX_NoOfQuestion = "";
            $scope.LPMOEEX_TotalMarks = "";
            $scope.LPMOEEX_ExamDuration = "";
            $scope.LPMOEEX_RandomFlg = false;
            $scope.LPMOEEX_AutoPublishFlg = false;
            $scope.LPMOEEX_UploadExamPaperFlg = false;
            $scope.LPMOEEX_FromDateTime = null;
            $scope.LPMOEEX_ToDateTime = null;
            $scope.ScheduleTime_24 = "";
            $scope.ScheduleTimeTo_24 = "";
            $scope.LPMOEEX_QuestionPaper = "";
            $scope.quesfiletype = "";
            $scope.LPMOEEX_AnswerSheet = "";
            $scope.ansfiletype = "";
            $scope.getquestionlist = [];
            $scope.submitted1 = false;
            $scope.GetCartDetails = [];
            $scope.getarratcomplexities = [];
            $scope.searchchkbxsec = "";
            $scope.searchchkbx = "";
            $scope.loaddata();
        };

        $scope.EditMasterExamQuestion = function (id) {
            var data = {
                "LPMOEEX_Id": id
            };

            apiService.create("LP_OnlineExam/EditMasterExamQuestion", data).then(function (promise) {
                if (promise.geteditmasteroeexam !== null && promise.geteditmasteroeexam.length > 0) {
                    $scope.edit = true;
                    $scope.totalgrid = [];
                    $scope.classlist = promise.getclasslist;
                    $scope.getsectionlist = promise.getsectionlist;
                    $scope.getSubjects = promise.getsubjectlist;
                    $scope.getmasterexamdetails = promise.getmasterexamdetails;
                    $scope.gettopiclist = promise.gettopiclist;
                    $scope.getquestionlist = promise.getquestionlist;

                    $scope.ASMAY_Id = promise.geteditmasteroeexam[0].asmaY_Id;
                    $scope.ASMCL_Id = promise.geteditmasteroeexam[0].asmcL_Id;
                    $scope.ASMS_Id = promise.geteditmasteroeexam[0].asmS_Id;

                    $scope.ISMS_Id = promise.geteditmasteroeexam[0].ismS_Id;
                    $scope.LPMOEEX_Id = promise.geteditmasteroeexam[0].lpmoeeX_Id;

                    $scope.LPMOEEX_ExamName = promise.geteditmasteroeexam[0].lpmoeeX_ExamName;
                    $scope.LPMOEEX_NoOfQuestion = promise.geteditmasteroeexam[0].lpmoeeX_NoOfQuestion;

                    $scope.LPMOEEX_TotalMarks = promise.geteditmasteroeexam[0].lpmoeeX_TotalMarks;
                    $scope.LPMOEEX_ExamDuration = promise.geteditmasteroeexam[0].lpmoeeX_ExamDuration;

                    $scope.EME_Id = promise.geteditmasteroeexam[0].emE_Id !== null && promise.geteditmasteroeexam[0].emE_Id !== ''
                        && promise.geteditmasteroeexam[0].emE_Id !== 0 ? promise.geteditmasteroeexam[0].emE_Id : '';

                    $scope.LPMOEEX_RandomFlg = promise.geteditmasteroeexam[0].lpmoeeX_RandomFlg;
                    $scope.LPMOEEX_UploadExamPaperFlg = promise.geteditmasteroeexam[0].lpmoeeX_UploadExamPaperFlg;

                    $scope.LPMOEEX_QuestionPaper = promise.geteditmasteroeexam[0].lpmoeeX_QuestionPaper;
                    $scope.LPMOEEX_QuestionPapeFileName = promise.geteditmasteroeexam[0].lpmoeeX_QuestionPapeFileName;
                    $scope.LPMOEEX_AutoPublishFlg = promise.geteditmasteroeexam[0].lpmoeeX_AutoPublishFlg === null ? false : promise.geteditmasteroeexam[0].lpmoeeX_AutoPublishFlg;

                    $scope.LPMOEEX_AllowDownloadQnsPaperBeforeExamFlg = promise.geteditmasteroeexam[0].lpmoeeX_AllowDownloadQnsPaperBeforeExamFlg === null ? false :
                        promise.geteditmasteroeexam[0].lpmoeeX_AllowDownloadQnsPaperBeforeExamFlg;

                    $scope.LPMOEEX_DurationFlag = promise.geteditmasteroeexam[0].lpmoeeX_DurationFlag == undefined
                        || promise.geteditmasteroeexam[0].lpmoeeX_DurationFlag == null
                        || promise.geteditmasteroeexam[0].lpmoeeX_DurationFlag == "" ? "" : promise.geteditmasteroeexam[0].lpmoeeX_DurationFlag;

                    $scope.LPMOEEX_Duration = promise.geteditmasteroeexam[0].lpmoeeX_Duration == undefined || promise.geteditmasteroeexam[0].lpmoeeX_Duration == null
                        || promise.geteditmasteroeexam[0].lpmoeeX_Duration == "" ? "" : promise.geteditmasteroeexam[0].lpmoeeX_Duration;

                    $scope.LPMOEEX_NotLinkedToQnsBankFlg = promise.geteditmasteroeexam[0].lpmoeeX_NotLinkedToQnsBankFlg === true ? '1' : '0';
                    $scope.format = promise.geteditmasteroeexam[0].lpmoeeX_NotLinkedToQnsBankFlg === true ? '1' : '0';

                    if ($scope.LPMOEEX_QuestionPaper !== null && $scope.LPMOEEX_QuestionPaper !== "") {
                        var img = $scope.LPMOEEX_QuestionPaper;
                        var imagarr = img.split('.');
                        var lastelement = imagarr[imagarr.length - 1];
                        $scope.quesfiletype = lastelement;
                        console.log("data.filetype : " + $scope.quesfiletype);
                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                            $scope.quesdocument_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.LPMOEEX_QuestionPaper;
                        }
                    }

                    $scope.LPMOEEX_AnswerSheet = promise.geteditmasteroeexam[0].lpmoeeX_AnswerSheet;
                    $scope.LPMOEEX_AnswerPapeFileName = promise.geteditmasteroeexam[0].lpmoeeX_AnswerPapeFileName;

                    if ($scope.LPMOEEX_AnswerSheet !== null && $scope.LPMOEEX_AnswerSheet !== "") {
                        var img1 = $scope.LPMOEEX_AnswerSheet;
                        var imagarr1 = img1.split('.');
                        var lastelement1 = imagarr1[imagarr.length - 1];
                        $scope.ansfiletype = lastelement1;
                        console.log("data.filetype : " + $scope.ansfiletype);
                        if (lastelement1 === 'doc' || lastelement1 === 'docx' || lastelement1 === 'ppt' || lastelement1 === 'pptx' || lastelement1 === 'xlsx'
                            || lastelement1 === 'xls') {
                            $scope.ansdocument_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.LPMOEEX_AnswerSheet;
                        }
                    }

                    $scope.gettopiclist = promise.gettopiclist;
                    $scope.geteditexamtopiclist = promise.geteditexamtopiclist;

                    angular.forEach($scope.getsectionlist, function (sec) {
                        sec.checkedsec = false;
                        if (sec.asmS_Id === Number($scope.ASMS_Id)) {
                            sec.checkedsec = true;
                        }
                    });

                    angular.forEach($scope.gettopiclist, function (dd) {
                        angular.forEach($scope.geteditexamtopiclist, function (d) {
                            if (dd.lpmT_Id === d.lpmT_Id) {
                                dd.checkedsub = true;
                                dd.lpmoeextoP_Id = d.lpmoeextoP_Id;
                            }
                        });
                    });

                    if ($scope.LPMOEEX_UploadExamPaperFlg === false) {
                        $scope.getquestionlist = promise.getquestionlist;
                        $scope.vieweimages($scope.getquestionlist);

                        console.log($scope.getquestionlist);

                        $scope.tempgetquestionlist = $scope.getquestionlist;

                        $scope.getediteleveldetails = promise.getediteleveldetails;
                        $scope.geteditelevelquestions = promise.geteditelevelquestions;

                        $scope.GetCartDetails = [];
                        angular.forEach($scope.getediteleveldetails, function (dd) {
                            $scope.GetCartDetails.push({
                                LPMOEEXLVL_LevelDesc: dd.lpmoeexlvL_LevelDesc, LPMOEEXLVL_TotalNoOfQns: dd.lpmoeexlvL_TotalNoOfQns,
                                LPMOEEXLVL_MaxQns: dd.lpmoeexlvL_MaxQns, LPMOEEXLVL_LevelTotalMarks: dd.lpmoeexlvL_LevelTotalMarks,
                                LPMOEEXLVL_MarksPerQns: dd.lpmoeexlvL_MarksPerQns, LPMOEEXLVL_LevelOrder: dd.lpmoeexlvL_LevelOrder,
                                LPMOEEXLVL_Id: dd.lpmoeexlvL_Id
                            });
                        });

                        $scope.questionlist = [];

                        angular.forEach($scope.GetCartDetails, function (dd) {
                            $scope.questionlist = [];
                            angular.forEach($scope.geteditelevelquestions, function (d) {
                                if (dd.LPMOEEXLVL_Id === d.lpmoeexlvL_Id) {
                                    if ($scope.LPMOEEX_NotLinkedToQnsBankFlg === "1") {
                                        $scope.questionlist.push({
                                            LPMOEEXQNS_Question: d.lpmoeexqnS_Question, LPMOEEXQNS_Marks: d.lpmoeexqnS_Marks, LPMOEEXQNS_Id: d.lpmoeexqnS_Id,
                                            LPMOEEXQNS_QuestionType: d.lpmoeexqnS_QuestionType,
                                            LPMOEEXQNS_QnsOrder: d.lpmoeexqnS_QnsOrder, LPMOEEXQNS_SubjectiveFlg: d.lpmoeexqnS_SubjectiveFlg,
                                            LPMOEEXQNS_MatchTheFollowingFlg: d.lpmoeexqnS_MatchTheFollowingFlg,
                                            LPMOEEXQNS_NoOfRows: d.lpmoeexqnS_NoOfRows, LPMOEEXQNS_NoOfColumns: d.lpmoeexqnS_NoOfColumns,
                                            LPMOEEXQNS_NoOfOptions: d.lpmoeexqnS_NoOfOptions,
                                            ques_mf_columns: $scope.colms_array, Temp_Manual_Ques_Options: $scope.totalgrid,
                                            questiontype: d.lpmoeexqnS_QuestionType,

                                        });
                                    } else {
                                        $scope.questionlist.push({
                                            LPMOEQ_Question: d.lpmoeQ_Question, LPMOEEXQNS_Marks: d.lpmoeexqnS_Marks,
                                            LPMOEEXQNS_QnsOrder: d.lpmoeexqnS_QnsOrder, LPMOEEXQNS_Id: d.lpmoeexqnS_Id,
                                            LPMOEQ_Id: d.lpmoeQ_Id
                                        });
                                    }
                                }
                            });
                            dd.questionlist = $scope.questionlist;
                        });

                        $scope.options = [];
                        $scope.rows_array = [];
                        $scope.cols_array = [];

                        if ($scope.LPMOEEX_NotLinkedToQnsBankFlg === "1") {
                            $scope.geteditelevelquestionsoptions = promise.geteditelevelquestionsoptions;
                            $scope.geteditelevelquestionsoptionsmf = promise.geteditelevelquestionsoptionsmf;

                            $scope.geteditelevelquestionsfiles = promise.geteditelevelquestionsfiles;
                            $scope.geteditelevelquestionsoptionsfiles = promise.geteditelevelquestionsoptionsfiles;

                            angular.forEach($scope.GetCartDetails, function (levels) {
                                angular.forEach(levels.questionlist, function (level_ques) {

                                    //Question Wise Files List
                                    $scope.questions_files_Temp = [];
                                    angular.forEach($scope.geteditelevelquestionsfiles, function (level_ques_files) {
                                        if (level_ques.LPMOEEXQNS_Id === level_ques_files.lpmoeexqnS_Id) {
                                            if (level_ques_files.lpmoeexqnsF_FilePath !== null && level_ques_files.lpmoeexqnsF_FilePath !== "") {
                                                var img = level_ques_files.lpmoeexqnsF_FilePath;
                                                var imagarr = img.split('.');
                                                var lastelement = imagarr[imagarr.length - 1];
                                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                                    level_ques_files.quesdocument_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + level_ques_files.lpmoeexqnsF_FilePath;
                                                }

                                                $scope.questions_files_Temp.push({
                                                    LPMOEEXQNSF_FilePath: level_ques_files.lpmoeexqnsF_FilePath,
                                                    LPMOEEXQNSF_FileName: level_ques_files.lpmoeexqnsF_FileName,
                                                    LPMOEEXQNSF_Id: level_ques_files.lpmoeexqnsF_Id,
                                                    LPMOEEXQNS_Id: level_ques_files.lpmoeexqnS_Id,
                                                    document_Pathnew: level_ques_files.quesdocument_Pathnew,
                                                    filetype: lastelement,
                                                });
                                            }
                                        }
                                    });
                                    level_ques.Temp_Manual_Ques_Files = $scope.questions_files_Temp;

                                    // Question Wise Options 
                                    $scope.totalgrid = [];
                                    $scope.totalgrid_optionsmf = [];
                                    angular.forEach($scope.geteditelevelquestionsoptions, function (level_ques_opt) {
                                        if (level_ques.LPMOEEXQNS_Id === level_ques_opt.lpmoeexqnS_Id) {
                                            $scope.totalgrid.push({
                                                LPMOEEXQNSOPT_Option: level_ques_opt.lpmoeexqnsopT_Option,
                                                LPMOEEXQNSOPT_OptionCode: level_ques_opt.lpmoeexqnsopT_OptionCode,
                                                LPMOEEXQNSOPT_AnswerFlag: level_ques_opt.lpmoeexqnsopT_AnswerFlag,
                                                LPMOEEXQNSOPT_Id: level_ques_opt.lpmoeexqnsopT_Id, LPMOEEXQNSOPT_Marks: level_ques_opt.lpmoeexqnsopT_Marks,
                                                rowname: level_ques_opt.lpmoeexqnsopT_Option
                                            });
                                        }
                                    });

                                    if (level_ques.LPMOEEXQNS_MatchTheFollowingFlg === true && level_ques.LPMOEEXQNS_SubjectiveFlg !== true) {
                                        angular.forEach($scope.totalgrid, function (mf_options) {
                                            $scope.Temp_Colms_Array = [];
                                            angular.forEach($scope.geteditelevelquestionsoptionsmf, function (mf) {
                                                if (mf_options.LPMOEEXQNSOPT_Id === mf.lpmoeexqnsopT_Id) {

                                                    $scope.Temp_Colms_Array.push({
                                                        LPMOEEXQNSOPTMF_Id: mf.lpmoeexqnsoptmF_Id,
                                                        LPMOEEXQNSOPTMF_MatchtheFollowing: mf.lpmoeexqnsoptmF_MatchtheFollowing,
                                                        LPMOEEXQNSOPTMF_Answer_Flg: mf.lpmoeexqnsoptmF_Answer_Flg,
                                                        LPMOEEXQNSOPT_Id: mf.lpmoeexqnsopT_Id,
                                                        LPMOEEXQNSOPTMF_Order: mf.lpmoeexqnsoptmF_Order,
                                                        MF_CorrectedAns: mf.lpmoeexqnsoptmF_Answer_Flg,
                                                    });

                                                    $scope.totalgrid_optionsmf.push({
                                                        LPMOEEXQNSOPTMF_Id: mf.lpmoeexqnsoptmF_Id,
                                                        LPMOEEXQNSOPTMF_MatchtheFollowing: mf.lpmoeexqnsoptmF_MatchtheFollowing,
                                                        LPMOEEXQNSOPTMF_Answer_Flg: mf.lpmoeexqnsoptmF_Answer_Flg,
                                                        LPMOEEXQNSOPT_Id: mf.lpmoeexqnsopT_Id,
                                                        LPMOEEXQNSOPTMF_Order: mf.lpmoeexqnsoptmF_Order
                                                    });
                                                }
                                            });
                                            mf_options.Temp_Manual_Ques_Options_Mf = $scope.Temp_Colms_Array;
                                            mf_options.cols_rows_array = $scope.Temp_Colms_Array;
                                        });
                                    }

                                    $scope.colms_array = [];
                                    $scope.Temp_Distinct_cols = [];
                                    $scope.Temp_Distinct_cols = $scope.totalgrid_optionsmf.filter((item, i, arr) => arr.findIndex((t) => t.LPMOEEXQNSOPTMF_MatchtheFollowing === item.LPMOEEXQNSOPTMF_MatchtheFollowing) === i);

                                    angular.forEach($scope.Temp_Distinct_cols, function (dd, i) {
                                        $scope.colms_array.push({
                                            id: i, colname: dd.LPMOEEXQNSOPTMF_MatchtheFollowing,
                                            LPMOEEXQNSOPTMF_MatchtheFollowing: dd.LPMOEEXQNSOPTMF_MatchtheFollowing
                                        });
                                    });


                                    //Question Option Wise Files List
                                    angular.forEach($scope.totalgrid, function (level_ques_opts) {
                                        $scope.questions_opts_files_Temp = [];
                                        angular.forEach($scope.geteditelevelquestionsoptionsfiles, function (level_ques_opts_files) {
                                            if (level_ques_opts.LPMOEEXQNSOPT_Id === level_ques_opts_files.lpmoeexqnsopT_Id) {

                                                if (level_ques_opts_files.lpmoeexqnsoptF_FilePath !== null && level_ques_opts_files.lpmoeexqnsoptF_FilePath !== "") {
                                                    var img = level_ques_opts_files.lpmoeexqnsoptF_FilePath;
                                                    var imagarr = img.split('.');
                                                    var lastelement = imagarr[imagarr.length - 1];
                                                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                                        level_ques_opts_files.quesdocument_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + level_ques_opts_files.lpmoeexqnsoptF_FilePath;
                                                    }

                                                    $scope.questions_opts_files_Temp.push({
                                                        LPMOEEXQNSOPTF_FilePath: level_ques_opts_files.lpmoeexqnsoptF_FilePath,
                                                        LPMOEEXQNSOPTF_FileName: level_ques_opts_files.lpmoeexqnsoptF_FileName,
                                                        LPMOEEXQNSOPTF_Id: level_ques_opts_files.lpmoeexqnsoptF_Id,
                                                        LPMOEEXQNS_Id: level_ques_opts_files.lpmoeexqnS_Id,
                                                        LPMOEEXQNSOPT_Id: level_ques_opts_files.lpmoeexqnsopT_Id,
                                                        document_Pathnew: level_ques_opts_files.quesdocument_Pathnew,
                                                        filetype: lastelement,
                                                    });
                                                }
                                            }
                                        });
                                        level_ques_opts.filecount = $scope.questions_opts_files_Temp.length;
                                        level_ques_opts.Temp_Manual_Ques_Opts_Files = $scope.questions_opts_files_Temp;
                                    });

                                    level_ques.Temp_Manual_Ques_Options = $scope.totalgrid;
                                    level_ques.ques_mf_columns = $scope.colms_array;
                                });
                            });
                        }

                        // $scope.OnchangeNoofOptions();
                    }

                    if (promise.geteditmasteroeexam[0].lpmoeeX_FromDateTime !== null) {
                        $scope.minDatefP = new Date(promise.geteditmasteroeexam[0].lpmoeeX_FromDateTime);
                        $scope.LPMOEEX_FromDateTime = new Date(promise.geteditmasteroeexam[0].lpmoeeX_FromDateTime);
                        $scope.totimehr = $filter('date')(promise.geteditmasteroeexam[0].lpmoeeX_FromDateTime, 'HH');
                        $scope.totimemin = $filter('date')(promise.geteditmasteroeexam[0].lpmoeeX_FromDateTime, 'mm');
                        $scope.totimesec = $filter('date')(promise.geteditmasteroeexam[0].lpmoeeX_FromDateTime, 'ss');
                        var fromtime = new Date();
                        fromtime.setHours($scope.totimehr);
                        fromtime.setMinutes($scope.totimemin);
                        fromtime.setSeconds($scope.totimesec);
                        $scope.fromtimebind = fromtime;
                        $scope.ScheduleTime_24 = $scope.fromtimebind;

                    }

                    if (promise.geteditmasteroeexam[0].lpmoeeX_ToDateTime !== null) {
                        $scope.LPMOEEX_ToDateTime = new Date(promise.geteditmasteroeexam[0].lpmoeeX_ToDateTime);
                        $scope.totimehr = $filter('date')(promise.geteditmasteroeexam[0].lpmoeeX_ToDateTime, 'HH');
                        $scope.totimemin = $filter('date')(promise.geteditmasteroeexam[0].lpmoeeX_ToDateTime, 'mm');
                        $scope.totimesec = $filter('date')(promise.geteditmasteroeexam[0].lpmoeeX_ToDateTime, 'ss');
                        var totime = new Date();
                        totime.setHours($scope.totimehr);
                        totime.setMinutes($scope.totimemin);
                        totime.setSeconds($scope.totimesec);
                        $scope.totimebind = totime;
                        $scope.ScheduleTimeTo_24 = $scope.totimebind;
                    }

                    console.log($scope.GetCartDetails);
                    $scope.totalgrid = [];
                    $scope.scroll();
                    $scope.getexamhappenedcounttemp = false;
                    if (promise.getexamhappenedcount > 0) {
                        $scope.getexamhappenedcounttemp = true;
                    }
                }
            });
        };

        $scope.ViewMasterExamQuesOptions = function (dd) {

            $scope.Subject = dd.ismS_SubjectName;
            $scope.ClassName = dd.asmcL_ClassName;
            $scope.YearName = dd.asmaY_Year;
            $scope.ExamName = dd.lpmoeeX_ExamName;

            var data = {
                "LPMOEEX_Id": dd.lpmoeeX_Id
            };

            apiService.create("LP_OnlineExam/ViewMasterExamQuesOptions", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getviewexamquestiondetails = promise.getviewexamquestiondetails;
                    $scope.getexamhappenedcountques = promise.getexamhappenedcount;
                }
            });
        };

        $scope.ViewMasterExamLevelDetails = function (dd) {
            $scope.Subject = dd.ismS_SubjectName;
            $scope.ClassName = dd.asmcL_ClassName;
            $scope.YearName = dd.asmaY_Year;
            $scope.ExamName = dd.lpmoeeX_ExamName;
            $scope.lpmoeeXId_Id = dd.lpmoeeX_Id;
            $scope.getarrayofleveldetails = [];
            $scope.getarrayofleveldetails = [];
            var data = {
                "LPMOEEX_Id": dd.lpmoeeX_Id
            };

            apiService.create("LP_OnlineExam/ViewMasterExamLevelDetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getarrayofleveldetails = promise.getarrayofleveldetails;
                }
            });

        };

        $scope.ViewSavedLevelQuestons = function (dd) {
            $scope.getarrayoflevelquestiondetails = [];
            $scope.levelname = dd.lpmoeexlvL_LevelDesc;
            $scope.levelname_id = dd.lpmoeexlvL_Id;
            $scope.levelname_marks = dd.lpmoeexlvL_LevelTotalMarks;

            var data = {
                "LPMOEEXLVL_Id": dd.lpmoeexlvL_Id
            };

            apiService.create("LP_OnlineExam/ViewSavedLevelQuestons", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getarrayoflevelquestiondetails = promise.getarrayoflevelquestiondetails;
                    $scope.level_notlinked = promise.lpmoeeX_NotLinkedToQnsBankFlg === true ? 1 : 0;
                    if (promise.lpmoeeX_NotLinkedToQnsBankFlg === true) {

                        $scope.getarrayoflevelquestionoptiondetails = promise.getarrayoflevelquestionoptiondetails;
                        $scope.getarrayoflevelquestionoptionmfdetails = promise.getarrayoflevelquestionoptionmfdetails;

                        $scope.getarrayoflevelquestiondetailsfiles = promise.getarrayoflevelquestiondetailsfiles;
                        $scope.getarrayoflevelquestionoptiondetailsfiles = promise.getarrayoflevelquestionoptiondetailsfiles;

                        $scope.questionlist = [];
                        angular.forEach($scope.getarrayoflevelquestiondetails, function (d) {

                            $scope.questionlist.push({
                                LPMOEEXQNS_Question: d.lpmoeexqnS_Question, LPMOEEXQNS_Marks: d.lpmoeexqnS_Marks, LPMOEEXQNS_Id: d.lpmoeexqnS_Id,
                                LPMOEEXQNS_QuestionType: d.lpmoeexqnS_QuestionType,
                                LPMOEEXQNS_QnsOrder: d.lpmoeexqnS_QnsOrder, LPMOEEXQNS_SubjectiveFlg: d.lpmoeexqnS_SubjectiveFlg,
                                LPMOEEXQNS_MatchTheFollowingFlg: d.lpmoeexqnS_MatchTheFollowingFlg,
                                LPMOEEXQNS_NoOfRows: d.lpmoeexqnS_NoOfRows, LPMOEEXQNS_NoOfColumns: d.lpmoeexqnS_NoOfColumns,
                                LPMOEEXQNS_NoOfOptions: d.lpmoeexqnS_NoOfOptions,
                                LPMOEEXQNS_Answer: d.lpmoeexqnS_Answer,
                                ques_mf_columns: $scope.colms_array, Temp_Manual_Ques_Options: $scope.totalgrid,
                                questiontype: d.lpmoeexqnS_QuestionType
                            });
                        });


                        angular.forEach($scope.questionlist, function (level_ques) {

                            //Question Wise Files List
                            $scope.questions_files_Temp = [];
                            angular.forEach($scope.getarrayoflevelquestiondetailsfiles, function (level_ques_files) {
                                if (level_ques.LPMOEEXQNS_Id === level_ques_files.lpmoeexqnS_Id) {
                                    if (level_ques_files.lpmoeexqnsF_FilePath !== null && level_ques_files.lpmoeexqnsF_FilePath !== "") {
                                        var img = level_ques_files.lpmoeexqnsF_FilePath;
                                        var imagarr = img.split('.');
                                        var lastelement = imagarr[imagarr.length - 1];
                                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                            level_ques_files.quesdocument_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + level_ques_files.lpmoeexqnsF_FilePath;
                                        }

                                        $scope.questions_files_Temp.push({
                                            LPMOEEXQNSF_FilePath: level_ques_files.lpmoeexqnsF_FilePath,
                                            LPMOEEXQNSF_FileName: level_ques_files.lpmoeexqnsF_FileName,
                                            LPMOEEXQNSF_Id: level_ques_files.lpmoeexqnsF_Id,
                                            LPMOEEXQNS_Id: level_ques_files.lpmoeexqnS_Id,
                                            document_Pathnew: level_ques_files.quesdocument_Pathnew,
                                            filetype: lastelement,
                                        });
                                    }
                                }
                            });
                            level_ques.Temp_Manual_Ques_Files = $scope.questions_files_Temp;

                            $scope.totalgrid = [];
                            $scope.totalgrid_optionsmf = [];
                            angular.forEach($scope.getarrayoflevelquestionoptiondetails, function (level_ques_opt) {
                                if (level_ques.LPMOEEXQNS_Id === level_ques_opt.lpmoeexqnS_Id) {
                                    $scope.totalgrid.push({
                                        LPMOEEXQNSOPT_Option: level_ques_opt.lpmoeexqnsopT_Option,
                                        LPMOEEXQNSOPT_OptionCode: level_ques_opt.lpmoeexqnsopT_OptionCode,
                                        LPMOEEXQNSOPT_AnswerFlag: level_ques_opt.lpmoeexqnsopT_AnswerFlag,
                                        LPMOEEXQNSOPT_Id: level_ques_opt.lpmoeexqnsopT_Id, LPMOEEXQNSOPT_Marks: level_ques_opt.lpmoeexqnsopT_Marks,
                                        rowname: level_ques_opt.lpmoeexqnsopT_Option
                                    });
                                }
                            });

                            if (level_ques.LPMOEEXQNS_MatchTheFollowingFlg === true && level_ques.LPMOEEXQNS_SubjectiveFlg !== true) {
                                angular.forEach($scope.totalgrid, function (mf_options) {
                                    $scope.Temp_Colms_Array = [];
                                    angular.forEach($scope.getarrayoflevelquestionoptionmfdetails, function (mf) {
                                        if (mf_options.LPMOEEXQNSOPT_Id === mf.lpmoeexqnsopT_Id) {

                                            $scope.Temp_Colms_Array.push({
                                                LPMOEEXQNSOPTMF_Id: mf.lpmoeexqnsoptmF_Id,
                                                LPMOEEXQNSOPTMF_MatchtheFollowing: mf.lpmoeexqnsoptmF_MatchtheFollowing,
                                                LPMOEEXQNSOPTMF_Answer_Flg: mf.lpmoeexqnsoptmF_Answer_Flg,
                                                LPMOEEXQNSOPT_Id: mf.lpmoeexqnsopT_Id,
                                                LPMOEEXQNSOPTMF_Order: mf.lpmoeexqnsoptmF_Order
                                            });

                                            $scope.totalgrid_optionsmf.push({
                                                LPMOEEXQNSOPTMF_Id: mf.lpmoeexqnsoptmF_Id,
                                                LPMOEEXQNSOPTMF_MatchtheFollowing: mf.lpmoeexqnsoptmF_MatchtheFollowing,
                                                LPMOEEXQNSOPTMF_Answer_Flg: mf.lpmoeexqnsoptmF_Answer_Flg,
                                                LPMOEEXQNSOPT_Id: mf.lpmoeexqnsopT_Id,
                                                LPMOEEXQNSOPTMF_Order: mf.lpmoeexqnsoptmF_Order
                                            });
                                        }
                                    });
                                    mf_options.Temp_Manual_Ques_Options_Mf = $scope.Temp_Colms_Array;
                                    mf_options.cols_rows_array = $scope.Temp_Colms_Array;
                                });
                            }

                            $scope.colms_array = [];
                            $scope.Temp_Distinct_cols = [];
                            $scope.Temp_Distinct_cols = $scope.totalgrid_optionsmf.filter((item, i, arr) => arr.findIndex((t) => t.LPMOEEXQNSOPTMF_MatchtheFollowing === item.LPMOEEXQNSOPTMF_MatchtheFollowing) === i);

                            angular.forEach($scope.Temp_Distinct_cols, function (dd, i) {
                                $scope.colms_array.push({
                                    id: i, colname: dd.LPMOEEXQNSOPTMF_MatchtheFollowing,
                                    LPMOEEXQNSOPTMF_MatchtheFollowing: dd.LPMOEEXQNSOPTMF_MatchtheFollowing
                                });
                            });

                            //Question Option Wise Files List
                            angular.forEach($scope.totalgrid, function (level_ques_opts) {
                                $scope.questions_opts_files_Temp = [];
                                angular.forEach($scope.getarrayoflevelquestionoptiondetailsfiles, function (level_ques_opts_files) {
                                    if (level_ques_opts.LPMOEEXQNSOPT_Id === level_ques_opts_files.lpmoeexqnsopT_Id) {

                                        if (level_ques_opts_files.lpmoeexqnsoptF_FilePath !== null && level_ques_opts_files.lpmoeexqnsoptF_FilePath !== "") {
                                            var img = level_ques_opts_files.lpmoeexqnsoptF_FilePath;
                                            var imagarr = img.split('.');
                                            var lastelement = imagarr[imagarr.length - 1];
                                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                                level_ques_opts_files.quesdocument_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + level_ques_opts_files.lpmoeexqnsoptF_FilePath;
                                            }

                                            $scope.questions_opts_files_Temp.push({
                                                LPMOEEXQNSOPTF_FilePath: level_ques_opts_files.lpmoeexqnsoptF_FilePath,
                                                LPMOEEXQNSOPTF_FileName: level_ques_opts_files.lpmoeexqnsoptF_FileName,
                                                LPMOEEXQNSOPTF_Id: level_ques_opts_files.lpmoeexqnsoptF_Id,
                                                LPMOEEXQNS_Id: level_ques_opts_files.lpmoeexqnS_Id,
                                                LPMOEEXQNSOPT_Id: level_ques_opts_files.lpmoeexqnsopT_Id,
                                                document_Pathnew: level_ques_opts_files.quesdocument_Pathnew,
                                                filetype: lastelement,
                                            });
                                        }
                                    }
                                });
                                level_ques_opts.filecount = $scope.questions_opts_files_Temp.length;
                                level_ques_opts.Temp_Manual_Ques_Opts_Files = $scope.questions_opts_files_Temp;
                            });


                            level_ques.Temp_Manual_Ques_Options = $scope.totalgrid;
                            level_ques.ques_mf_columns = $scope.colms_array;
                        });
                    } else {
                        $scope.vieweimages($scope.getarrayoflevelquestiondetails);
                    }
                    $('#myModalviewlevelquesdetails').modal('show');
                }
            });
        };

        $scope.ViewMasterQuestionExamTopic = function (dd) {

            $scope.Subject = dd.ismS_SubjectName;
            $scope.ClassName = dd.asmcL_ClassName;
            $scope.YearName = dd.asmaY_Year;
            $scope.ExamName = dd.lpmoeeX_ExamName;

            var data = {
                "LPMOEEX_Id": dd.lpmoeeX_Id
            };

            apiService.create("LP_OnlineExam/ViewMasterQuestionExamTopic", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getviewexamquestiontopicdetails = promise.getviewexamquestiontopicdetails;
                    $scope.getexamhappenedcounttopics = promise.getexamhappenedcount;
                }
            });
        };

        $scope.ViewMasterExamDocs = function (dd) {
            $scope.Subject = dd.ismS_SubjectName;
            $scope.ClassName = dd.asmcL_ClassName;
            $scope.YearName = dd.asmaY_Year;
            $scope.ExamName = dd.lpmoeeX_ExamName;

            $scope.LPMOEEX_AnswerPapeFileNameView = dd.lpmoeeX_AnswerPapeFileName;
            $scope.LPMOEEX_AnswerSheetView = dd.lpmoeeX_AnswerSheet;
            $scope.LPMOEEX_QuestionPapeFileNameView = dd.lpmoeeX_QuestionPapeFileName;
            $scope.LPMOEEX_QuestionPaperView = dd.lpmoeeX_QuestionPaper;

            if ($scope.LPMOEEX_QuestionPaperView !== null && $scope.LPMOEEX_QuestionPaperView !== "") {
                var img = $scope.LPMOEEX_QuestionPaperView;
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                $scope.quesfiletypeview = lastelement;
                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                    $scope.quesdocument_Pathnewview = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.LPMOEEX_QuestionPaperView;
                }
            }

            if ($scope.LPMOEEX_AnswerSheetView !== null && $scope.LPMOEEX_AnswerSheetView !== "") {
                var img1 = $scope.LPMOEEX_AnswerSheetView;
                var imagarr1 = img1.split('.');
                var lastelement1 = imagarr1[imagarr.length - 1];
                $scope.ansfiletypeview = lastelement1;
                if (lastelement1 === 'doc' || lastelement1 === 'docx' || lastelement1 === 'ppt' || lastelement1 === 'pptx' || lastelement1 === 'xlsx'
                    || lastelement1 === 'xls') {
                    $scope.ansdocument_Pathnewview = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.LPMOEEX_AnswerSheetView;
                }
            }

        };

        $scope.ViewQuestionPaper = function (dd) {

            var data = {
                "LPMOEEX_Id": dd.lpmoeeX_Id
            };
            apiService.create("LP_OnlineExam/ViewQuestionPaper", data).then(function (promise) {
                $scope.getexamdetails = promise.getexamdetails;

                if ($scope.getexamdetails !== null && $scope.getexamdetails.length > 0) {

                    $scope.examflag = $scope.getexamdetails[0].lpmoeeX_UploadExamPaperFlg;

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

                            // Level Wise Questions
                            angular.forEach($scope.getexamleveldetails, function (dd) {
                                $scope.tempquestions = [];
                                angular.forEach($scope.getQuestion, function (d) {
                                    if (dd.lpmoeexlvL_Id === d.lpmoeexlvL_Id) {
                                        $scope.tempquestions.push(d);
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
                                                    $scope.Temp_array.push(mf_opts);
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

                            console.log($scope.getexamleveldetails);

                            $('#modalquestionpaper').modal('show');

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
                    }

                    $scope.asmaYYear = dd.asmaY_Year;
                    $scope.asmcLClassName = dd.asmcL_ClassName;
                    $scope.ismSSubjectName = dd.ismS_SubjectName;
                    $scope.lpmoeeXExamName = dd.lpmoeeX_ExamName;
                    $scope.lpmoeeXExamDuration = dd.lpmoeeX_ExamDuration;
                    $scope.lpmoeeXFromDateTime = dd.lpmoeeX_FromDateTime;
                    $scope.lpmoeeXTotalMarks = dd.lpmoeeX_TotalMarks;
                }
            });
        };

        $scope.DeactivateActivateMasterExam = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.lpmoeeX_ActiveFlg === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            var data = {
                "LPMOEEX_Id": deactiveRecord.lpmoeeX_Id
            };
            swal({
                title: "Are you sure",
                text: "Do You Want To " + mgs + " Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("LP_OnlineExam/DeactivateActivateMasterExam", data).then(function (promise) {
                            if (promise.message !== "Mapped" && promise.message !== 'Duplicate') {
                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                                $state.reload();
                            } else if (promise.message === 'Duplicate') {
                                swal("You Can Not Activate This Record, Record Already Exists For Selected Details");
                            } else {
                                swal("You Can Not " + mgs + " This Record Its Already Mapped");
                            }
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.DeactivateActivateExamQues = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.lpmoeexqnS_ActiveFlg === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            var data = {
                "LPMOEEXQNS_Id": deactiveRecord.lpmoeexqnS_Id,
                "LPMOEEX_Id": deactiveRecord.lpmoeeX_Id
            };
            swal({
                title: "Are you sure",
                text: "Do You Want To " + mgs + " Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("LP_OnlineExam/DeactivateActivateExamQues", data).then(function (promise) {
                            if (promise.message === "Mapped") {
                                swal("Question Is Already Mapped You Can Not Deactivate");
                            } else {
                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                            }
                            $scope.getviewexamquestiondetails = promise.getviewexamquestiondetails;
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.DeactivateActivateExamQuesTopic = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.lpmoeextoP_ActiveFlg === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            var data = {
                "LPMOEEXTOP_Id": deactiveRecord.lpmoeextoP_Id,
                "LPMOEEX_Id": deactiveRecord.lpmoeeX_Id
            };
            swal({
                title: "Are you sure",
                text: "Do You Want To " + mgs + " Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("LP_OnlineExam/DeactivateActivateExamQuesTopic", data).then(function (promise) {
                            if (promise.message === "Mapped") {
                                swal("Question Is Already Mapped You Can Not Deactivate");
                            } else {
                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                            }
                            $scope.getviewexamquestiontopicdetails = promise.getviewexamquestiontopicdetails;
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.OnChangeMasterExam = function () {

            if ($scope.EME_Id > 0) {
                $scope.temptopics = [];
                $scope.sectiondetailslist = [];

                angular.forEach($scope.getsectionlist, function (d) {
                    if (d.checkedsec) {
                        $scope.sectiondetailslist.push({ ASMS_Id: d.asmS_Id });
                    }
                });
                angular.forEach($scope.gettopiclist, function (dd) {
                    if (dd.checkedsub === true) {
                        $scope.temptopics.push({ LPMT_Id: dd.lpmT_Id });
                    }
                });

                var data = {
                    "LPMOEEX_Id": $scope.LPMOEEX_Id,
                    "EME_Id": $scope.EME_Id,
                    "ISMS_Id": $scope.ISMS_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "temptopics": $scope.temptopics,
                    "sectiondetailslist": $scope.sectiondetailslist
                };

                apiService.create("LP_OnlineExam/OnChangeMasterExam", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message === "Duplicate") {
                            $scope.EME_Id = "";
                            swal("Selected Exam Is Already Mapped For Selected Subject");
                        }
                    }
                });
            }
        };

        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        }

        $scope.scroll_header = function () {
            $("html, body").animate({ scrollTop: 0 }, 500);
        }

        $scope.cleartabl1 = function () {
            $state.reload();
        };

        $scope.searchValue1 = function (obj) {
            return (angular.lowercase(obj.lpmoeeX_ExamName)).indexOf(angular.lowercase($scope.obj.searchValueddd)) >= 0 ||
                (angular.lowercase(obj.asmaY_Year)).indexOf(angular.lowercase($scope.obj.searchValueddd)) >= 0 ||
                (angular.lowercase(obj.asmcL_ClassName)).indexOf(angular.lowercase($scope.obj.searchValueddd)) >= 0 ||
                (angular.lowercase(obj.ismS_SubjectName)).indexOf(angular.lowercase($scope.obj.searchValueddd)) >= 0 ||
                (angular.lowercase(obj.lpmoeeX_ExamDuration)).indexOf(angular.lowercase($scope.obj.searchValueddd)) >= 0 ||
                ($filter('date')(obj.lpmoeeX_FromDateTime, 'dd/MM/yyyy HH:mm').indexOf($scope.obj.searchValueddd) >= 0) ||
                ($filter('date')(obj.lpmoeeX_ToDateTime, 'dd/MM/yyyy HH:mm').indexOf($scope.obj.searchValueddd) >= 0) ||
                (JSON.stringify(obj.lpmoeeX_NoOfQuestion)).indexOf($scope.obj.searchValueddd) >= 0 ||
                (JSON.stringify(obj.lpmoeeX_TotalMarks)).indexOf($scope.obj.searchValueddd) >= 0 ||
                (JSON.stringify(obj.lpmoeeX_MarksPerQns)).indexOf($scope.obj.searchValueddd) >= 0;
        };

        $scope.interacted = function (field) {
            return $scope.submitted1;
        };

        $scope.SaveLevelQuestionOrder = function (dd, order_flag) {

            var data = {
                "LPMOEEXLVL_Id": $scope.levelname_id,
                "LPMOEEX_Id": $scope.lpmoeeXId_Id,
                "Order_Flag": order_flag
            };

            if (order_flag === "Question_Order") {
                data.temporderquestiondto = dd;
            }
            else if (order_flag === "Level_Order") {
                data.ExamOrderLevelDetails = dd;
            }

            apiService.create("LP_OnlineExam/SaveLevelQuestionOrder", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.message === "Update") {
                        swal("Records Updated");
                    } else {
                        swal("Failed To Updte");
                    }
                    if (order_flag === "Question_Order") {
                        $scope.getarrayoflevelquestiondetails = promise.getarrayoflevelquestiondetails;
                        $scope.vieweimages($scope.getarrayoflevelquestiondetails);
                    }
                    else if (order_flag === "Level_Order") {
                        $scope.getarrayofleveldetails = promise.getarrayofleveldetails;
                    }
                }
            });
        };

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
        };

        $scope.sort = function (key) {
            $scope.sortKey = key;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.toggleAll_S = function (checkall) {
            var toggleStatus = checkall;
            angular.forEach($scope.getquestionlist, function (itm) {
                itm.checked = toggleStatus;
            });
        };

        $scope.showmodaldetails = function (data) {
            $('#preview').attr('src', data.document_Path);
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

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
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

        $scope.uploadtecherdocuments1 = [];
        $scope.uploadtecherdocuments = function (input, document) {
            $scope.uploadtecherdocuments1 = input.files;
            $scope.filename = input.files[0].name;
            if (input.files[0].size <= 31457280) {
                if (input.files && input.files[0]) {
                    if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg"
                        || input.files[0].type === "video/mp4" || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword"
                        || input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                        || input.files[0].type === "application/vnd.ms-excel" || input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") { UploaddianPhoto(document); }
                    else { swal("Upload  Pdf, Doc, Image Files Only"); }
                }
            } else {
                swal("Upload File Size Should Be Less Than 30MB");
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
                    $scope.LPMOEEX_QuestionPaper = d;
                    $scope.LPMOEEX_QuestionPapeFileName = $scope.filename;
                    $('#').attr('src', $scope.LPMOEEX_QuestionPaper);
                    var img = $scope.LPMOEEX_QuestionPaper;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    $scope.quesfiletype = lastelement;
                    console.log("data.filetype : " + $scope.quesfiletype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        $scope.quesdocument_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.LPMOEEX_QuestionPaper;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.uploadtecherdocumentsans1 = [];
        $scope.uploadtecherdocumentsanswer = function (input, document) {
            $scope.uploadtecherdocumentsans1 = input.files;
            $scope.filenameans = input.files[0].name;
            if (input.files[0].size <= 31457280) {
                if (input.files && input.files[0]) {
                    if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg"
                        || input.files[0].type === "video/mp4" || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword"
                        || input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                        || input.files[0].type === "application/vnd.ms-excel" || input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") { UploaddianPhotoans(document); }
                    else { swal("Upload  Pdf, Doc, Image Files Only"); }
                }
            } else {
                swal("Upload File Size Should Be Less Than 30MB");
            }
        };
        function UploaddianPhotoans(data) {
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadtecherdocumentsans1.length; i++) {
                formData.append("File", $scope.uploadtecherdocumentsans1[i]);
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
                    $scope.LPMOEEX_AnswerSheet = d;
                    $scope.LPMOEEX_AnswerPapeFileName = $scope.filenameans;
                    $('#').attr('src', $scope.LPMOEEX_AnswerSheet);
                    var img = $scope.LPMOEEX_AnswerSheet;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    $scope.ansfiletype = lastelement;
                    console.log("data.filetype : " + $scope.ansfiletype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        $scope.quesdocument_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.LPMOEEX_AnswerSheet;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }


        $scope.onPastedecimal = function (e) {
            var regex = /[0-9]|\./;
            var text = event.clipboardData.getData("text/plain");
            if (!regex.test(text)) {
                e.preventDefault()
            }
        };

        $scope.isOptionsRequired4 = function () {
            return !$scope.getsectionlist.some(function (options) {
                return options.checkedsec;
            });
        };

        $scope.isOptionsRequired3 = function () {
            return !$scope.gettopiclist.some(function (options) {
                return options.checkedsub;
            });
        };

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.lpmT_TopicName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };

        $scope.searchchkbxsec = "";
        $scope.filterchkbxsec = function (obj) {
            return angular.lowercase(obj.asmC_SectionName).indexOf(angular.lowercase($scope.searchchkbxsec)) >= 0;
        };

        // Functions For Manual Question Creation
        $scope.clicimage = function () {
            $scope.questionsource = true;
        };

        $scope.checkclicimage = function (checkvalue) {
            if (checkvalue == true) {
                $scope.obj.mathtype = false;
                $scope.obj.noraml = false;
            }
        };

        $scope.checkmath = function (checkvalue) {
            if (checkvalue == true) {
                $scope.obj.LPMOEQ_StructuralFlg = false;
                $scope.obj.noraml = false;
            }
        };

        $scope.checknoraml = function (checkvalue) {
            if (checkvalue == true) {
                $scope.obj.LPMOEQ_StructuralFlg = false;
                $scope.obj.mathtype = false;
            }
        };

        $scope.showmodal = function () {
            $scope.show = true;
            $scope.barmodal = false;
        };

        $scope.hidebarmodal = function () {
            $scope.barmodal = false;
        };

        $scope.hidemodal = function () {
            $scope.show = false;
            $scope.barmodal = false;
        };

        $scope.minimizemodal = function () {
            $scope.show = false;
            $scope.barmodal = true;
        };

        $scope.maximizemodal = function () {
            $scope.show = true;
            $scope.barmodal = false;
        };

        $scope.copydata = function () {
            var copyTextareaBtn = document.getElementById('data').value;
            document.getElementById("sel6c").value = copyTextareaBtn;
        };

        $scope.onlanguagechange = function () {
            if ($scope.language !== "") {
                $scope.languagehtml = $scope.language;
            }
            else {
                $scope.languagehtml = "";
            }
        };

        $scope.OnChangeCheckbox = function () {
            $scope.questiondisplay = true;
            $scope.obj.LPMOEEXQNS_MatchTheFollowingFlg = false;
            if ($scope.obj.LPMOEEXQNS_SubjectiveFlg === false) {
                $scope.LPMOEEXQNS_NoOfOptions = 4;
                $scope.OnchangeNoofOptions();

                $scope.NoOfRows = 4;
                $scope.NoOfColumns = 4;
                $scope.ViewMatchTheFollowing();
            } else {
                $scope.LPMOEEXQNS_NoOfOptions = 0;
                $scope.questiondisplay = false;
                $scope.totalgrid = [];
                $scope.colms_array = [];
                $scope.rows_array = [];
            }
        };

        $scope.OnchangeNoofOptions = function () {

            $scope.noofoptions = parseInt($scope.LPMOEEXQNS_NoOfOptions);
            $scope.noofoptionstemp = $scope.totalgrid.length;
            $scope.changecheck = true;
            if ($scope.totalgrid === null || $scope.totalgrid.length === 0) {
                $scope.totalgrid = [];

                for (var i = 0; i < $scope.noofoptions; i++) {
                    var name = "";
                    for (var j = 0; j < $scope.alphabetsarray.length; j++) {
                        if (i === j) {
                            name = $scope.alphabetsarray[j];
                        }
                    }

                    $scope.totalgrid.push({ LPMOEEXQNSOPT_Option: name, LPMOEEXQNSOPT_OptionCode: '', LPMOEEXQNSOPT_AnswerFlag: false, LPMOEEXQNSOPT_Id: 0 });

                }
            }
            else {
                if ($scope.totalgrid.length < $scope.noofoptions) {
                    var count = $scope.noofoptions - $scope.totalgrid.length;

                    for (var i = $scope.totalgrid.length; i < $scope.noofoptions; i++) {
                        var name = "";
                        for (var j = $scope.totalgrid.length; j < $scope.alphabetsarray.length; j++) {
                            if (i === j) {
                                name = $scope.alphabetsarray[j];
                            }
                        }

                        $scope.totalgrid.push({ LPMOEEXQNSOPT_Option: name, LPMOEEXQNSOPT_OptionCode: '', LPMOEEXQNSOPT_AnswerFlag: false, LPMOEEXQNSOPT_Id: 0 });
                    }
                }
                else if ($scope.totalgrid.length > $scope.noofoptions) {
                    var newItemNo = $scope.totalgrid.length;

                    var count = $scope.totalgrid.length - $scope.noofoptions;

                    for (var i = 0; i < count; i++) {
                        newItemNo = newItemNo - 1;
                        $scope.totalgrid.splice(newItemNo, 1);
                    }
                }
                //else if ($scope.LPMOEQ_NoOfOptions == undefined) {
                //    $scope.totalgrid.splice(0, $scope.totalgrid.length);
                //}
            }
            $timeout(function () { $scope.addeditors(); }, 1000);
        };

        $scope.addeditors = function () {
            if ($scope.changecheck) {
                if ($scope.totalgrid.length > $scope.noofoptionstemp) {
                    for (var i = $scope.noofoptionstemp; i < $scope.totalgrid.length; i++) {
                        var genericIntegrationProperties = {};
                        document.getElementById('toolbar' + i).innerHTML = "";
                        genericIntegrationProperties.target = document.getElementById('htmlEditorlable' + i);
                        genericIntegrationProperties.toolbar = document.getElementById('toolbar' + i);
                        // GenericIntegration instance.
                        var genericIntegrationInstance = new WirisPlugin.GenericIntegration(genericIntegrationProperties);
                        genericIntegrationInstance.init();
                        genericIntegrationInstance.listeners.fire('onTargetReady', {});

                        var htmlEditor = document.getElementById('htmlEditorlable' + i);
                        var data = ''
                        htmlEditor.innerHTML = WirisPlugin.Parser.initParse(data);
                    }
                }
                $scope.changecheck = false;
            }
        };

        $scope.editeditors = function () {
            angular.forEach($scope.totalgrid, function (dd, index) {
                var htmlEditorlable = document.getElementById('htmlEditorlable' + index);
                htmlEditorlable.innerHTML = WirisPlugin.Parser.initParse(dd.LPMOEEXQNSOPT_OptionCode);
            });
        };

        $scope.OnChangeMFCheckbox = function () {
            $scope.questiondisplay = true;
            $scope.obj.LPMOEEXQNS_SubjectiveFlg = false;
            if ($scope.obj.LPMOEEXQNS_MatchTheFollowingFlg === false) {
                $scope.LPMOEEXQNS_NoOfOptions = 4;
                $scope.OnchangeNoofOptions();
            } else {
                $scope.LPMOEEXQNS_NoOfOptions = 0;
                $scope.questiondisplay = false;
                $scope.totalgrid = [];
                $scope.colms_array = [];
                $scope.rows_array = [];

                $scope.NoOfRows = 4;
                $scope.NoOfColumns = 4;
                $scope.ViewMatchTheFollowing();
            }
        };

        $scope.OnChangeNoRowsMF = function () {
            $scope.hide = true;
            if (Number($scope.NoOfRows) > 0 && Number($scope.NoOfColumns)) {
                $scope.ViewMatchTheFollowing();
            }
        };

        $scope.OnChangeNoColumnsMF = function () {
            $scope.hide = true;
            if (Number($scope.NoOfRows) > 0 && Number($scope.NoOfColumns)) {
                $scope.ViewMatchTheFollowing();
            }
        };

        $scope.ViewMatchTheFollowing = function () {
            $scope.hide = false;

            $scope.colms = Number($scope.NoOfColumns);
            $scope.rows = Number($scope.NoOfRows);

            if ($scope.colms_array.length === 0 && $scope.rows_array.length === 0) {
                for (var icols = 0; icols < $scope.colms; icols++) {
                    $scope.colms_array.push({ id: icols, colname: "", col_order: icols + 1 });
                }

                for (var irows = 0; irows < $scope.rows; irows++) {
                    $scope.temp_cols_array = [];
                    for (var icols = 0; icols < $scope.colms; icols++) {
                        $scope.temp_cols_array.push({ id: icols, colname: "", MF_CorrectedAns: false });
                    }
                    $scope.rows_array.push({ id: irows, rowname: "", cols_rows_array: $scope.temp_cols_array });
                }
            } else {
                // Columsn Array
                if ($scope.colms_array.length > 0) {
                    var colms_length = $scope.colms_array.length;

                    if ($scope.colms_array.length < $scope.colms) {
                        var count = $scope.colms - $scope.colms_array.length;

                        for (var i = $scope.colms_array.length; i < $scope.colms; i++) {
                            var name = "";
                            for (var j = $scope.colms_array.length; j < $scope.alphabetsarray.length; j++) {
                                if (i === j) {
                                    //name = $scope.alphabetsarray[j];
                                }
                            }
                            $scope.colms_array.push({ id: icols, colname: "", col_order: i + 1 });
                        }
                    }

                    else if ($scope.colms_array.length > $scope.colms) {
                        var newItemNo = $scope.colms_array.length;

                        var count = $scope.colms_array.length - $scope.colms;

                        for (var i = 0; i < count; i++) {
                            newItemNo = newItemNo - 1;
                            $scope.colms_array.splice(newItemNo, 1);
                        }
                    }
                }
                // Rows Array
                if ($scope.rows_array.length > 0) {
                    var rows_length = $scope.rows_array.length;

                    angular.forEach($scope.rows_array, function (dd) {

                        //angular.forEach(dd.cols_rows_array, function (dd_cols_rws) {
                        //    dd_cols_rws.MF_CorrectedAns = false;
                        //});

                        if (dd.cols_rows_array.length < $scope.colms_array.length) {
                            for (var i = dd.cols_rows_array.length; i < $scope.colms_array.length; i++) {
                                dd.cols_rows_array.push({ id: icols, colname: "", MF_CorrectedAns: false });
                            }
                        }

                        if (dd.cols_rows_array.length > $scope.colms_array.length) {

                            var count = dd.cols_rows_array.length - $scope.colms_array.length;
                            for (var i = 0; i < count; i++) {
                                newItemNo = newItemNo - 1;
                                dd.cols_rows_array.splice(newItemNo, 1);
                            }
                        }
                    });

                    if ($scope.rows_array.length < $scope.rows) {
                        var count = $scope.rows - $scope.rows_array.length;
                        for (var i = $scope.rows_array.length; i < $scope.rows; i++) {
                            var name = "";
                            $scope.temp_cols_array = [];
                            for (var icols = 0; icols < $scope.colms; icols++) {
                                $scope.temp_cols_array.push({ id: icols, colname: "", MF_CorrectedAns: false, col_order: icols + 1 });
                            }
                            $scope.rows_array.push({ id: irows, rowname: "", cols_rows_array: $scope.temp_cols_array });
                        }
                    }

                    else if ($scope.rows_array.length > $scope.rows) {
                        var newItemNo = $scope.rows_array.length;

                        var count = $scope.rows_array.length - $scope.rows;

                        for (var i = 0; i < count; i++) {
                            newItemNo = newItemNo - 1;
                            $scope.rows_array.splice(newItemNo, 1);
                        }
                    }
                }
            }
        };

        $scope.OnChangeCorrectAnsMFCheckbox = function (rows, columns, index, cols_array) {
            angular.forEach(cols_array, function (dd, i) {
                if (index !== i) {
                    dd.MF_CorrectedAns = false;
                }
            });
        };

        $scope.OnChangeMFMarks = function (dd) {
            $scope.ques_marks = Number($scope.LPMOEQ_Marks);
            $scope.mf_marks = 0;
            $scope.mf_marks = $scope.rows_array.map(bill => Number(bill.LPMOEQOA_Marks) > 0 ? Number(bill.LPMOEQOA_Marks) : 0).reduce((acc, bill) => bill + acc);

            if ($scope.mf_marks > $scope.ques_marks) {
                dd.LPMOEQOA_Marks = "";
                swal("Match The Following Option Marks Should Be Equal To Total Question Marks");
            }
        };

        $scope.OnChangeColsMF = function (cols, index) {
            angular.forEach($scope.colms_array, function (dd, i) {
                if (dd.colname !== null && dd.colname !== "") {
                    if (index !== i) {
                        if (dd.colname.toUpperCase() === cols.colname.toUpperCase()) {
                            cols.colname = "";
                            swal("Value Already Entered");
                        }
                    }
                }
            });
        };

        $scope.OnChangeRowsMF = function (rows, index) {
            angular.forEach($scope.rows_array, function (dd, i) {
                if (dd.rowname !== null && dd.rowname !== "") {
                    if (index !== i) {
                        if (dd.rowname.toUpperCase() === rows.rowname.toUpperCase()) {
                            rows.rowname = "";
                            swal("Value Already Entered");
                        }
                    }
                }
            });
        };

        $scope.AddQuestionList = function () {
            if ($scope.myForm.$valid) {
                $scope.btn = false;
                var question = "";
                var question_type = "";
                if ($scope.obj.noraml === true) {
                    question = $scope.normalquestiontext;
                    question_type = "Text";
                } else if ($scope.obj.mathtype === true) {
                    question_type = "Math/Chem";
                    var htmlEditor = document.getElementById('htmlEditor');
                    $scope.innerHTML = htmlEditor.innerHTML;

                    var valuetrue = $scope.innerHTML.includes('data-custom-editor="chemistry"');
                    if (valuetrue === true) {
                        question_type = 'Chem';
                    } else {
                        question_type = 'Math';
                    }
                    $scope.innerHTML = $scope.innerHTML.replace('data-custom-editor="chemistry" ', "");
                    if ($scope.innerHTML == null || $scope.innerHTML == "") {
                        swal("Kindly Enter Question!!");
                        return;
                    } else {
                        question = $scope.innerHTML;
                    }
                } else if ($scope.obj.LPMOEQ_StructuralFlg === true) {
                    question_type = "Structural";
                    if ($scope.LPMOEQ_Question2 == null || $scope.LPMOEQ_Question2 == "") {
                        swal("Kindly Enter Question!!");
                        return;
                    }
                    $scope.innerHTML = '<img align="middle" class="Wirisformula" src="" role="math" style="max-width: none; vertical-align: -5px;">';
                    $scope.innerHTML = $scope.innerHTML.replace('""', '"' + $scope.LPMOEQ_Question2 + '"');
                    question = $scope.innerHTML;
                }

                var ques_answer = "";
                ques_answer = $scope.LPMOEEXQNS_Answer === undefined || $scope.LPMOEEXQNS_Answer === null || $scope.LPMOEEXQNS_Answer === "" ? "" : $scope.LPMOEEXQNS_Answer;                

                if (question !== undefined && question !== null && question !== '') {
                    if ($scope.obj.LPMOEEXQNS_SubjectiveFlg === false && $scope.obj.LPMOEEXQNS_MatchTheFollowingFlg === true) {
                        $scope.totalgrid = [];

                        var cols_order_count = 0;
                        angular.forEach($scope.colms_array, function (cols_ords) {
                            cols_order_count += 1;
                            cols_ords.cols_order = cols_order_count;
                            cols_ords.LPMOEEXQNSOPTMF_MatchtheFollowing = cols_ords.colname
                        });


                        angular.forEach($scope.rows_array, function (rows) {
                            var LPMOEEXQNSOPT_Id_Temp = 0;
                            LPMOEEXQNSOPT_Id_Temp = rows.lpmoeexqnsopT_Id !== undefined && rows.lpmoeexqnsopT_Id !== null ? rows.lpmoeexqnsopT_Id : 0;

                            $scope.Temp_Colms_Array = [];
                            angular.forEach(rows.cols_rows_array, function (dd_rows_cols, index_rows_cols) {
                                angular.forEach($scope.colms_array, function (dd_cols, index_cols) {
                                    if (index_rows_cols === index_cols) {
                                        var LPMOEEXQNSOPTMF_Id_Temp = 0;
                                        LPMOEEXQNSOPTMF_Id_Temp = dd_rows_cols.LPMOEEXQNSOPTMF_Id !== undefined && dd_rows_cols.LPMOEEXQNSOPTMF_Id !== null ?
                                            dd_rows_cols.LPMOEEXQNSOPTMF_Id : 0;

                                        $scope.Temp_Colms_Array.push({
                                            LPMOEEXQNSOPTMF_Id: LPMOEEXQNSOPTMF_Id_Temp,
                                            LPMOEEXQNSOPTMF_MatchtheFollowing: dd_cols.colname,
                                            LPMOEEXQNSOPTMF_Answer_Flg: dd_rows_cols.MF_CorrectedAns,
                                            MF_CorrectedAns: dd_rows_cols.MF_CorrectedAns,
                                            LPMOEEXQNSOPT_Id: LPMOEEXQNSOPT_Id_Temp,
                                            LPMOEEXQNSOPTMF_Order: dd_cols.cols_order
                                        });
                                    }
                                });
                            });

                            $scope.totalgrid.push({
                                LPMOEEXQNSOPT_Option: rows.rowname, LPMOEEXQNSOPT_OptionCode: rows.rowname, LPMOEEXQNSOPT_AnswerFlag: false,
                                rowname: rows.rowname,
                                LPMOEEXQNSOPT_Id: LPMOEEXQNSOPT_Id_Temp, Temp_Manual_Ques_Options_Mf: $scope.Temp_Colms_Array,
                                cols_rows_array: $scope.Temp_Colms_Array,
                                LPMOEEXQNSOPT_Marks: rows.LPMOEEXQNSOPT_Marks
                            });
                        });
                    }
                    else {
                        var checkansflag = 0;
                        var checkallansfill = 0;
                        angular.forEach($scope.totalgrid, function (d, index) {
                            if ($scope.obj.noraml === false && $scope.obj.LPMOEQ_StructuralFlg === false) {
                                var htmlEditor = document.getElementById('htmlEditorlable' + index);
                                $scope.ansinnerHTML = htmlEditor.innerHTML;
                                $scope.LPMOEEXQNSOPT_OptionCode = $scope.ansinnerHTML.replace('data-custom-editor="chemistry" ', "");
                                d.LPMOEEXQNSOPT_OptionCode = $scope.ansinnerHTML;

                                d.imgOptionCode = "";
                                if (d.LPMOEEXQNSOPT_OptionCode != null && d.LPMOEEXQNSOPT_OptionCode != '') {
                                    var splitstr = d.LPMOEEXQNSOPT_OptionCode.split(' ');
                                    $scope.strimg1 = splitstr[3];
                                    if ($scope.strimg1 != undefined) {
                                        var strimg2 = $scope.strimg1.split('"');
                                        $scope.strimg3 = strimg2[1];
                                    }
                                    else {
                                        $scope.strimg3 = undefined;
                                    }
                                }
                                if ($scope.strimg3 != undefined) {
                                    d.imgOptionCode = $scope.strimg3;
                                }
                            }
                            if (d.LPMOEEXQNSOPT_AnswerFlag === true) {
                                checkansflag += 1;
                            }                           
                        });

                        if (checkallansfill > 0) {
                            swal("Kindly Enter all answers!!");
                            return;
                        }
                        if (checkansflag === 0 && $scope.obj.LPMOEEXQNS_SubjectiveFlg === false) {
                            swal("Select One Correct Answer Flag");
                            return;
                        }
                    }

                    if ($scope.edit_temp_manual_ques_index > 0) {
                        $scope.GetQuestions_TempData.splice($scope.edit_manual_ques_index, 1);
                    }

                    var question_order = $scope.GetQuestions_TempData.length + 1;

                    $scope.imgvalue = "";
                    if (question != null && question != '') {
                        var splitstr = question.split(' ');
                        $scope.strimg1 = splitstr[3];
                        if ($scope.strimg1 != undefined) {
                            var strimg2 = $scope.strimg1.split('"');
                            $scope.strimg3 = strimg2[1];
                        }
                        else {
                            $scope.strimg3 = undefined;
                        }
                    }
                    if ($scope.strimg3 != undefined) {
                        $scope.imgvalue = $scope.strimg3;
                    }

                    $scope.Temp_Manual_Ques_Files_Temp = [];
                    angular.forEach($scope.teacherdocuupload, function (files) {
                        if (files.LPMOEEXQNSF_FilePath !== undefined && files.LPMOEEXQNSF_FilePath !== null && files.LPMOEEXQNSF_FilePath !== '') {
                            $scope.Temp_Manual_Ques_Files_Temp.push({
                                LPMOEEXQNSF_FilePath: files.LPMOEEXQNSF_FilePath,
                                LPMOEEXQNSF_FileName: files.LPMOEEXQNSF_FileName,
                                filetype: files.filetype,
                                document_Pathnew: files.document_Pathnew,
                                LPMOEEXQNSF_Id: files.LPMOEEXQNSF_Id === undefined || files.LPMOEEXQNSF_Id === null
                                    || files.LPMOEEXQNSF_Id === '' ? 0 : files.LPMOEEXQNSF_Id === undefined 
                            });
                        }
                    });

                    $scope.GetQuestions_TempData.push({
                        LPMOEEXQNS_Question: question, LPMOEEXQNS_Marks: $scope.LPMOEQ_Marks, LPMOEEXQNS_Id: 0, LPMOEEXQNS_QuestionType: question_type,
                        LPMOEEXQNS_QnsOrder: question_order, LPMOEEXQNS_SubjectiveFlg: $scope.obj.LPMOEEXQNS_SubjectiveFlg,
                        LPMOEEXQNS_MatchTheFollowingFlg: $scope.obj.LPMOEEXQNS_MatchTheFollowingFlg,
                        LPMOEEXQNS_NoOfRows: $scope.NoOfRows, LPMOEEXQNS_NoOfColumns: $scope.NoOfColumns, LPMOEEXQNS_NoOfOptions: $scope.LPMOEEXQNS_NoOfOptions,
                        LPMOEEXQNS_Answer: ques_answer, imgvalue: $scope.imgvalue,
                        ques_mf_columns: $scope.colms_array, Temp_Manual_Ques_Options: $scope.totalgrid, 
                        Temp_Manual_Ques_Files: $scope.Temp_Manual_Ques_Files_Temp
                    });

                    $scope.edit_manual_ques_index = null;
                    $scope.edit_temp_manual_ques_index = null;
                    $scope.normalquestiontext = "";
                    $scope.LPMOEQ_Question2 = "";
                    document.getElementById('htmlEditor').innerHTML = "";
                    $scope.LPMOEQ_Marks = "";
                    $scope.LPMOEEXQNS_NoOfOptions = 4;
                    $scope.obj.noraml = true;
                    $scope.obj.mathtype = false;
                    $scope.obj.LPMOEQ_StructuralFlg = false;

                    $scope.obj.LPMOEEXQNS_MatchTheFollowingFlg = false;
                    $scope.obj.LPMOEEXQNS_SubjectiveFlg = false;

                    $scope.teacherdocuupload = {};
                    $scope.teacherdocuupload = [{ id: 'Teacher1' }];

                    $scope.teacherdocuuploadopts = {};
                    $scope.teacherdocuuploadopts = [{ id: 'Teacheropts1' }];

                    $scope.totalgrid = [];
                    $scope.OnchangeNoofOptions();

                    $scope.rows_array = [];
                    $scope.colms_array = [];
                    $scope.ViewMatchTheFollowing();
                } else {
                    swal("Enter The Questions");
                }

            } else {
                $scope.submitted1 = true;
            }
        };

        $scope.EditTempQuestions = function (ques_obj, index) {

            $scope.edit_temp_manual_ques_index = index + 1;
            $scope.edit_manual_ques_index = index;
            $scope.obj.noraml = ques_obj.LPMOEEXQNS_QuestionType === 'Text' ? true : false;
            $scope.obj.mathtype = ques_obj.LPMOEEXQNS_QuestionType === 'Math' || ques_obj.LPMOEEXQNS_QuestionType === 'Chem' ? true : false;
            $scope.obj.LPMOEQ_StructuralFlg = ques_obj.LPMOEEXQNS_QuestionType === 'Structural' ? true : false;

            $scope.obj.LPMOEEXQNS_MatchTheFollowingFlg = ques_obj.LPMOEEXQNS_MatchTheFollowingFlg;
            $scope.obj.LPMOEEXQNS_SubjectiveFlg = ques_obj.LPMOEEXQNS_SubjectiveFlg;
            $scope.LPMOEEXQNS_Answer = ques_obj.LPMOEEXQNS_Answer;

            $scope.LPMOEQ_Marks = ques_obj.LPMOEEXQNS_Marks;

            if ($scope.obj.noraml === true) {
                $scope.normalquestiontext = ques_obj.LPMOEEXQNS_Question;                
            } else if ($scope.obj.mathtype === true) {

                var chemsplitstreditnormal = ques_obj.LPMOEEXQNS_Question.split(' ');
                $scope.chemstrimgnormal = chemsplitstreditnormal[3];
                if ($scope.chemstrimgnormal != undefined) {
                    var chemstrimg2 = $scope.chemstrimgnormal.split('"');
                    if (chemstrimg2[0] == "src=") {
                        var htmlEditor = document.getElementById('htmlEditor');
                        htmlEditor.innerHTML = WirisPlugin.Parser.initParse(ques_obj.LPMOEEXQNS_Question);
                        $scope.LPMOEQ_Question1 = ques_obj.LPMOEEXQNS_Question;
                        $scope.LPMOEQ_StructuralFlg = false;
                        $scope.noraml = false;
                        $scope.mathtype = true;
                    }
                    else {
                        $scope.normalquestiontext = ques_obj.LPMOEEXQNS_Question;
                    }
                }
                else {
                    $scope.normalquestiontext = ques_obj.LPMOEEXQNS_Question;
                }
            } else if ($scope.obj.LPMOEQ_StructuralFlg === true) {
                var chemsplitstredit = ques_obj.LPMOEEXQNS_Question.split(' ');
                $scope.chemstrimg1 = chemsplitstredit[3];

                if ($scope.chemstrimg1 != undefined) {
                    var chemstrimg2 = $scope.chemstrimg1.split('"');
                    $scope.LPMOEQ_Question2 = chemstrimg2[1];
                }
                $scope.LPMOEQ_Question1 = "abx";
            }           

            $scope.LPMOEEXQNS_NoOfOptions = ques_obj.LPMOEEXQNS_NoOfOptions;
            $scope.NoOfRows = ques_obj.LPMOEEXQNS_NoOfRows;
            $scope.NoOfColumns = ques_obj.LPMOEEXQNS_NoOfColumns;
            $scope.rows_array = [];
            $scope.colms_array = [];
            $scope.totalgrid = [];


            if (ques_obj.Temp_Manual_Ques_Files !== undefined && ques_obj.Temp_Manual_Ques_Files !== null && ques_obj.Temp_Manual_Ques_Files.length > 0) {
                $scope.teacherdocuupload = ques_obj.Temp_Manual_Ques_Files;

            } else {
                $scope.teacherdocuupload = {};
                $scope.teacherdocuupload = [{ id: 'Teacher1' }];
            }

            //$scope.normalquestiontext = $scope.obj.noraml == true ? ques_obj.LPMOEEXQNS_Question : '';

            if (ques_obj.LPMOEEXQNS_MatchTheFollowingFlg === true && ques_obj.LPMOEEXQNS_SubjectiveFlg === false) {
                $scope.rows_array = ques_obj.Temp_Manual_Ques_Options;
                $scope.colms_array = ques_obj.ques_mf_columns;
            } else if (ques_obj.LPMOEEXQNS_MatchTheFollowingFlg !== true && ques_obj.LPMOEEXQNS_SubjectiveFlg === false) {
                $scope.totalgrid = ques_obj.Temp_Manual_Ques_Options;

                if ($scope.obj.noraml === false) {
                    $timeout(function () { $scope.editeditors(); }, 1000);
                }                
            }
            $scope.OnchangeNoofOptions();
            $scope.ViewMatchTheFollowing();
        };

        $scope.DeleteTempQuestions = function (ques_obj_del, index) {

            swal({
                title: "Are you sure",
                text: "Do You Want To Delete This Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Delete It",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        $scope.GetQuestions_TempData.splice(index, 1);
                        swal("Record Deleted");
                    }
                    else {
                        swal("Deletion Cancelled");
                    }
                });
        };

        $scope.ViewTempQuestions = function (ques_obj_view, index) {

            $scope.obj.normal_view = ques_obj_view.LPMOEEXQNS_QuestionType === 'Text' ? true : false;
            $scope.obj.mathtype_view = ques_obj_view.LPMOEEXQNS_QuestionType === 'Math' || ques_obj_view.LPMOEEXQNS_QuestionType==='Chem' ? true : false;
            $scope.obj.LPMOEQ_StructuralFlg_view = ques_obj_view.LPMOEEXQNS_QuestionType === 'Structural' ? true : false;
            $scope.LPMOEEXQNS_QuestionType_view = ques_obj_view.LPMOEEXQNS_QuestionType;

            $scope.LPMOEQ_Marks_view = ques_obj_view.LPMOEEXQNS_Marks;

            $scope.LPMOEEXQNS_NoOfOptions_view = ques_obj_view.LPMOEEXQNS_NoOfOptions;
            $scope.NoOfRows_view = ques_obj_view.LPMOEEXQNS_NoOfRows;
            $scope.NoOfColumns_view = ques_obj_view.LPMOEEXQNS_NoOfColumns;
            $scope.rows_array_view = [];
            $scope.colms_array_view = [];
            $scope.totalgrid_view = [];

            $scope.normalquestiontext_view = $scope.obj.normal_view == true ? ques_obj_view.LPMOEEXQNS_Question : '';

            $scope.LPMOEEXQNS_MatchTheFollowingFlg_view = ques_obj_view.LPMOEEXQNS_MatchTheFollowingFlg;
            $scope.LPMOEEXQNS_SubjectiveFlg_view = ques_obj_view.LPMOEEXQNS_SubjectiveFlg;
            if (ques_obj_view.LPMOEEXQNS_MatchTheFollowingFlg === true && ques_obj_view.LPMOEEXQNS_SubjectiveFlg === false) {
                $scope.rows_array_view = ques_obj_view.Temp_Manual_Ques_Options;
                $scope.colms_array_view = ques_obj_view.ques_mf_columns;
            } else if (ques_obj_view.LPMOEEXQNS_MatchTheFollowingFlg !== true && ques_obj_view.LPMOEEXQNS_SubjectiveFlg === false) {
                $scope.totalgrid_view = ques_obj_view.Temp_Manual_Ques_Options;
            }

            if (ques_obj_view.Temp_Manual_Ques_Files !== undefined && ques_obj_view.Temp_Manual_Ques_Files !== null && ques_obj_view.Temp_Manual_Ques_Files.length > 0) {
                $scope.Temp_questions_files = ques_obj_view.Temp_Manual_Ques_Files;
            }          

            $('#mymodaltempques').modal('show');
        };

        $scope.UploadFiles = function (objupload, index) {
            $scope.optionsindex = index;
            if (objupload.Temp_Manual_Ques_Opts_Files !== undefined && objupload.Temp_Manual_Ques_Opts_Files !== null
                && objupload.Temp_Manual_Ques_Opts_Files.length > 0) {
                $scope.teacherdocuuploadopts = objupload.Temp_Manual_Ques_Opts_Files;
                angular.forEach($scope.teacherdocuuploadopts, function (d) {
                    var img = d.LPMOEEXQNSOPTF_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    d.filetype = lastelement;                     
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        d.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + d.LPMOEEXQNSOPTF_FilePath;
                    }
                });
            } else {
                $scope.teacherdocuuploadopts = [{ id: 'Teacheropts1' }];
            }
            $('#optsfilesupload').modal('show');
        };

        $scope.OnClickOptsFilesUpload = function () {
            $scope.Temp_Manual_Ques_Opts_Files_Temp = [];
            angular.forEach($scope.teacherdocuuploadopts, function (dd) {
                var id = 0;
                if (dd.LPMOEEXQNSOPTF_FilePath !== undefined && dd.LPMOEEXQNSOPTF_FilePath !== null && dd.LPMOEEXQNSOPTF_FilePath !== "") {
                    if (dd.LPMOEEXQNSOPTF_Id !== undefined && dd.LPMOEEXQNSOPTF_Id !== null) {
                        id = dd.LPMOEEXQNSOPTF_Id;
                    }
                    $scope.Temp_Manual_Ques_Opts_Files_Temp.push({
                        LPMOEEXQNSOPTF_FilePath: dd.LPMOEEXQNSOPTF_FilePath, LPMOEEXQNSOPTF_FileName: dd.LPMOEEXQNSOPTF_FileName,
                        LPMOEEXQNSOPTF_Id: id,
                        filetype: dd.filetype,
                        document_Pathnew: dd.document_Pathnew,
                    });
                }
            });

            if ($scope.Temp_Manual_Ques_Opts_Files_Temp.length > 0) {
                angular.forEach($scope.totalgrid, function (dd, index) {
                    if (index === $scope.optionsindex) {
                        dd.filecount = $scope.Temp_Manual_Ques_Opts_Files_Temp.length;
                        dd.Temp_Manual_Ques_Opts_Files = $scope.Temp_Manual_Ques_Opts_Files_Temp;
                    }
                });
            }

            $scope.teacherdocuuploadopts = {};
            $scope.teacherdocuuploadopts = [{ id: 'Teacheropts1' }];
        };

        $scope.ViewTempQuestionsOptionsFiles = function (obj_ques_opts_files_view, index) {

            if (obj_ques_opts_files_view.Temp_Manual_Ques_Opts_Files !== undefined && obj_ques_opts_files_view.Temp_Manual_Ques_Opts_Files !== null && obj_ques_opts_files_view.Temp_Manual_Ques_Opts_Files.length > 0) {
                $scope.Temp_questions_options_files = obj_ques_opts_files_view.Temp_Manual_Ques_Opts_Files;
                $('#optsfilesuploadview').modal('show');
            }
        };

        $scope.OnChangeOptionCode = function (user, index) {
            angular.forEach($scope.totalgrid, function (dd, i) {

                if (index !== i) {
                    if (user.LPMOEEXQNSOPT_Option.toUpperCase() === dd.LPMOEEXQNSOPT_Option.toUpperCase()) {
                        user.LPMOEEXQNSOPT_Option = "";
                        swal("Answer Option Already Exists");
                    }
                }
            });
        };

        //Manual Question Paper Creation File Upload
        $scope.uploadtecherdocuments_manual_ques = [];
        $scope.uploadtecherdocuments_manualques = function (input, document) {
            $scope.uploadtecherdocuments_manual_ques = input.files;
            $scope.filename = input.files[0].name;
            if (input.files[0].size <= 31457280) {
                if (input.files && input.files[0]) {
                    if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg"
                        || input.files[0].type === "video/mp4" || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword"
                        || input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                        || input.files[0].type === "application/vnd.ms-excel" || input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") { UploaddianPhoto_manualques(document); }
                    else { swal("Upload  Pdf, Doc, Image Files Only"); }
                }
            } else {
                swal("Upload File Size Should Be Less Than 30MB");
            }
        };
        function UploaddianPhoto_manualques(data) {
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadtecherdocuments_manual_ques.length; i++) {
                formData.append("File", $scope.uploadtecherdocuments_manual_ques[i]);
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
                    data.LPMOEEXQNSF_FilePath = d;
                    data.LPMOEEXQNSF_FileName = $scope.filename;
                    $('#').attr('src', data.LPMOEEXQNSF_FilePath);
                    var img = data.LPMOEEXQNSF_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;                   
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.quesdocument_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.LPMOEEXQNSF_FilePath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.addNewsiblingguard = function () {
            var newItemNo = $scope.teacherdocuupload.length + 1;
            if (newItemNo <= 10) {
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


        /* Manual Question Paper Creation File Upload Options Files */
        $scope.uploadtecheroptsdocuments_manual_ques_opts = [];
        $scope.uploadtecheroptsdocuments_manual_ques_option = function (input, document) {
            $scope.uploadtecheroptsdocuments_manual_ques_opts = input.files;
            $scope.filename = input.files[0].name;
            if (input.files && input.files[0]) {
                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png"
                    || input.files[0].type === "image/jpg" || input.files[0].type === "video/mp4" || input.files[0].type === "application/pdf"
                    || input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    || input.files[0].type === "application/vnd.ms-excel" || input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") { UploaddianPhotoopts_manual_ques_opts(document); }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload  Pdf, Doc, Image Files Only");
                }
            }
        };
        function UploaddianPhotoopts_manual_ques_opts(data) {
            console.log("Teacher Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadtecheroptsdocuments_manual_ques_opts.length; i++) {
                formData.append("File", $scope.uploadtecheroptsdocuments_manual_ques_opts[i]);
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
                    data.LPMOEEXQNSOPTF_FilePath = d;
                    data.LPMOEEXQNSOPTF_FileName = $scope.filename;
                    $('#').attr('src', data.LPMOEEXQNSOPTF_FilePath);
                    var img = data.LPMOEEXQNSOPTF_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;                     
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.LPMOEEXQNSOPTF_FilePath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.addNewsiblingguardopts = function () {
            var newItemNo = $scope.teacherdocuuploadopts.length + 1;
            if (newItemNo <= 10) {
                $scope.teacherdocuuploadopts.push({ 'id': 'Teacheropts1' + newItemNo });
            }
            console.log($scope.teacherdocuuploadopts);
        };
        $scope.removeNewsiblingguardopts = function (index) {
            var newItemNo = $scope.teacherdocuuploadopts.length - 1;
            $scope.teacherdocuuploadopts.splice(index, 1);
            if ($scope.teacherdocuuploadopts.length === 0) {
                //data
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
                        scope.model += newVal[0];
                    }
                });
            }
        };
    });
})();