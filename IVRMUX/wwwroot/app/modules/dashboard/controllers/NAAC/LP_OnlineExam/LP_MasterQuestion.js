(function () {
    'use strict';
    angular.module('app').controller('LP_OnlineExamMasterQuestionController', LP_OnlineExamMasterQuestionController)

    LP_OnlineExamMasterQuestionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter', '$q', '$sce', '$window']
    function LP_OnlineExamMasterQuestionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter, $q, $sce, $window) {

        $scope.groups = [{ title: 'Dynamic Group Header - 1', content: 'Dynamic Group Body - 1' },
        { title: 'Dynamic Group Header - 2', content: 'Dynamic Group Body - 2' },
        { title: 'Dynamic Group Header - 3', content: 'Dynamic Group Body - 3' },
        { title: 'Dynamic Group Header - 4', content: 'Dynamic Group Body - 4' }];
        $scope.language = "";       
        $scope.alphabetsarray = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
        $scope.noraml = true;
        $scope.barmodal = false;
        $scope.LPMOEQ_Question1 = "abc";
        $scope.searc_button = true;
        $scope.changecheck = false;
        $scope.questionsource = true;
        $scope.sortKey = 'LMSMOEQ_Id';
        $scope.sortReverse = true;
        $scope.LPMOEQ_SubjectiveFlg = false;
        $scope.mathtype = false;
        $scope.LPMOEQ_StructuralFlg = false;
        $scope.LPMOEQ_MatchTheFollowingFlg = false;
        $scope.edit = false;
        $scope.getexamhappenedcounttemp = false;
        $scope.editflag == false;

        $scope.show = false;
        $scope.modlanguage = "";

        $scope.answer = "";
        $scope.show_ansOption = false;
        $scope.questiondisplay = false;
        $scope.obj = {};
        $scope.obj.searchValueddd = "";

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.currentPage1 = 1;
        $scope.currentPage2 = 1;
        $scope.currentPage3 = 1;
        $scope.btn = false;

        $scope.itemsPerPage1 = paginationformasters;
        $scope.itemsPerPage2 = paginationformasters;
        $scope.itemsPerPage3 = paginationformasters;

        $scope.teacherdocuupload = {};
        $scope.teacherdocuupload = [{ id: 'Teacher1' }];

        $scope.teacherdocuuploadopts = {};
        $scope.teacherdocuuploadopts = [{ id: 'Teacheropts1' }];

        $scope.totalgrid = [];
        $scope.colms_array = [];
        $scope.rows_array = [];

        //***** MASTER QUESTION ******//

        $scope.getlanguagepage = function (lang) {
            if (lang != "") {
                $scope.language = lang;
                var langurl = "https://dcampusstrg.blob.core.windows.net/language/" + lang + ".html";
                $scope.langurl = $sce.trustAsResourceUrl(langurl);
            }
        };

        $scope.loaddata = function () {
            var pageid = 2;

            apiService.getURI("LP_OnlineExam/getmasterquestionloaddata", pageid).then(function (promise) {
                $scope.yearlist = promise.getyearlist;
                $scope.getMasterQuestiondetails = promise.getMasterQuestiondetails;

                angular.forEach($scope.getMasterQuestiondetails, function (dd) {
                    $scope.strimg3 = "";
                    dd.imgvalue = "";
                    if (dd.lpmoeQ_Question != null && dd.lpmoeQ_Question != '') {
                        var splitstr = dd.lpmoeQ_Question.split(' ');
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
                        dd.imgvalue = $scope.strimg3;
                    }
                });


                $scope.getConfigurationSettings = promise.getConfigurationSettings;
                $scope.getarratcomplexities = promise.getarratcomplexities;

                if ($scope.getarratcomplexities !== null && $scope.getarratcomplexities.length > 0) {
                    if (promise.lpmcomP_Id !== null && promise.lpmcomP_Id > 0) {
                        $scope.LPMCOMP_Id = promise.lpmcomP_Id;
                    } else {
                        $scope.LPMCOMP_Id = $scope.getarratcomplexities[0].lpmcomP_Id;
                    }
                }

                //$scope.btn = false;

                if (promise.getclasslist !== null && promise.getclasslist.length > 0) {
                    $scope.classlist = promise.getclasslist;
                }

                $scope.LPMOEQ_NoOfOptions = 4;
                $scope.questiondisplay = true;
                $scope.OnchangeNoofOptions();
                //$scope.onlanguagechange();
                $scope.NoOfRows = 4;
                $scope.NoOfColumns = 4;
                $scope.ViewMatchTheFollowing();

                $timeout(function () { $scope.addeditors(); }, 1000);
            });
        };

        $scope.getclasslist = function () {
            $scope.ASMCL_Id = "";
            $scope.classlist = [];
            $scope.ISMS_Id = "";
            $scope.getSubjects = [];
            $scope.LPMT_Id = "";
            $scope.gettopiclist = [];
            $scope.LPMOEQ_Question = "";
            $scope.LPMOEQ_QuestionDesc = "";
            $scope.LPMOEQ_Marks = "";
            $scope.totalgrid = [];

            $scope.colms_array = [];
            $scope.rows_array = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("LP_OnlineExam/getclasslist", data).then(function (promise) {
                if (promise.getclasslist !== null && promise.getclasslist.length > 0) {
                    $scope.classlist = promise.getclasslist;
                }
                $scope.LPMOEQ_NoOfOptions = 4;
                $scope.OnchangeNoofOptions();

                $scope.NoOfRows = 4;
                $scope.NoOfColumns = 4;
                $scope.ViewMatchTheFollowing();
            });
        };

        $scope.getsubjectlist = function () {
            $scope.ISMS_Id = "";
            $scope.getSubjects = [];
            $scope.LPMT_Id = "";
            $scope.gettopiclist = [];
            $scope.totalgrid = [];
            $scope.colms_array = [];
            $scope.rows_array = [];
            var data = {
                "ASMCL_Id": $scope.ASMCL_Id
            };

            apiService.create("LP_OnlineExam/getsubjectlist", data).then(function (promise) {
                if (promise.getsubjectlist !== null && promise.getsubjectlist.length > 0) {
                    $scope.getSubjects = promise.getsubjectlist;
                }
                $scope.LPMOEQ_NoOfOptions = 4;
                $scope.OnchangeNoofOptions();

                $scope.NoOfRows = 4;
                $scope.NoOfColumns = 4;
                $scope.ViewMatchTheFollowing();
            });
        };

        $scope.getsubjecttopiclist = function () {
            $scope.LPMT_Id = "";
            $scope.gettopiclist = [];
            $scope.totalgrid = [];
            $scope.colms_array = [];
            $scope.rows_array = [];
            var data = {
                "ASMCL_Id": $scope.ASMCL_Id,
                "ISMS_Id": $scope.ISMS_Id
            };
            apiService.create("LP_OnlineExam/gettopiclist", data).then(function (promise) {
                if (promise.gettopiclist !== null && promise.gettopiclist.length > 0) {
                    $scope.gettopiclist = promise.gettopiclist;
                }
                $scope.LPMOEQ_NoOfOptions = 4;
                $scope.OnchangeNoofOptions();

                $scope.NoOfRows = 4;
                $scope.NoOfColumns = 4;
                $scope.ViewMatchTheFollowing();
            });
        };

        $scope.OnChangeCheckbox = function () {
            $scope.questiondisplay = true;
            $scope.LPMOEQ_MatchTheFollowingFlg = false;
            if ($scope.LPMOEQ_SubjectiveFlg === false) {
                $scope.LPMOEQ_NoOfOptions = 4;
                $scope.OnchangeNoofOptions();

                $scope.NoOfRows = 4;
                $scope.NoOfColumns = 4;
                $scope.ViewMatchTheFollowing();
            } else {
                $scope.LPMOEQ_NoOfOptions = 0;
                $scope.questiondisplay = false;
                $scope.totalgrid = [];
                $scope.colms_array = [];
                $scope.rows_array = [];
            }
        };

        $scope.OnChangeMFCheckbox = function () {
            $scope.questiondisplay = true;
            $scope.LPMOEQ_SubjectiveFlg = false;
            if ($scope.LPMOEQ_MatchTheFollowingFlg === false) {
                $scope.LPMOEQ_NoOfOptions = 4;
                $scope.OnchangeNoofOptions();
            } else {
                $scope.LPMOEQ_NoOfOptions = 0;
                $scope.questiondisplay = false;
                $scope.totalgrid = [];
                $scope.colms_array = [];
                $scope.rows_array = [];

                $scope.NoOfRows = 4;
                $scope.NoOfColumns = 4;
                $scope.ViewMatchTheFollowing();
            }
        };

        $scope.OnchangeNoofOptions = function () {

            $scope.noofoptions = parseInt($scope.LPMOEQ_NoOfOptions);
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

                    $scope.totalgrid.push({ LPMOEQOA_Option: name, LPMOEQOA_OptionCode: '', LPMOEQOA_AnswerFlag: false, LPMOEQOA_Id: 0 });

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

                        $scope.totalgrid.push({ LPMOEQOA_Option: name, LPMOEQOA_OptionCode: '', LPMOEQOA_AnswerFlag: false, LPMOEQOA_Id: 0 });
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

        $scope.showtool = function () {
            if ($scope.edittool == true) {
                $scope.addeditors();
            }
            $scope.edittool = false;
        };

        $scope.UploadFiles = function (objupload, index) {

            $scope.optionsindex = index;
            if (objupload.tempoptionsfiles !== undefined && objupload.tempoptionsfiles !== null && objupload.tempoptionsfiles.length > 0) {
                $scope.teacherdocuuploadopts = objupload.tempoptionsfiles;

                angular.forEach($scope.teacherdocuuploadopts, function (d) {
                    var img = d.lpmoeqoaF_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    d.filetype = lastelement;
                    console.log("data.filetype : " + d.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        d.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + d.lpmoeqoaF_FilePath;
                    }
                });
            } else {
                $scope.teacherdocuuploadopts = [{ id: 'Teacheropts1' }];
            }
            $('#optsfilesupload').modal('show');
        };

        $scope.changed = function (dd) {
            document.getElementById('main').src = dd;
        };

        $scope.OnClickOptsFilesUpload = function () {
            $scope.tempoptsfiles = [];
            angular.forEach($scope.teacherdocuuploadopts, function (dd) {
                var id = 0;
                if (dd.lpmoeqoaF_FilePath !== undefined && dd.lpmoeqoaF_FilePath !== null && dd.lpmoeqoaF_FilePath !== "") {
                    if (dd.lpmoeqoaF_Id !== undefined && dd.lpmoeqoaF_Id !== null) {
                        id = dd.lpmoeqoaF_Id;
                    }
                    $scope.tempoptsfiles.push({
                        lpmoeqoaF_FilePath: dd.lpmoeqoaF_FilePath, lpmoeqoaF_FileName: dd.lpmoeqoaF_FileName,
                        lpmoeqoaF_Id: id
                    });
                }
            });

            if ($scope.tempoptsfiles.length > 0) {
                angular.forEach($scope.totalgrid, function (dd, index) {
                    if (index === $scope.optionsindex) {
                        dd.tempoptionsfiles = $scope.tempoptsfiles;
                    }
                });
            }
        };

        $scope.checkvalue = function (img) {
            $scope.LPMOEQ_Question2 = img;
        };

        $scope.SaveMasterQuestionDetails = function () {
            $scope.submitted1 = true;
            $scope.submitted2 = false;
            $scope.submitted3 = false;

            if ($scope.myForm.$valid) {
                var desc = "";
                if ($scope.LPMOEQ_QuestionDesc !== undefined && $scope.LPMOEQ_QuestionDesc !== null && $scope.LPMOEQ_QuestionDesc !== '') {
                    desc = $scope.LPMOEQ_QuestionDesc;
                } else {
                    desc = "";
                }

                if ($scope.mathtype == true) {
                    var htmlEditor = document.getElementById('htmlEditor');
                    $scope.innerHTML = htmlEditor.innerHTML;
                    //$scope.innerHTML = $scope.innerHTML.replace('data-custom-editor="chemistry" ', "");
                    if ($scope.innerHTML == null || $scope.innerHTML == "") {
                        swal("Kindly Enter Question!!");
                        return;
                    }
                }
                else if ($scope.LPMOEQ_StructuralFlg == true) {
                    if ($scope.LPMOEQ_Question2 == null || $scope.LPMOEQ_Question2 == "") {
                        swal("Kindly Enter Question!!");
                        return;
                    }
                    $scope.innerHTML = '<img align="middle" class="Wirisformula" src="" role="math" style="max-width: none; vertical-align: -5px;">';
                    $scope.innerHTML = $scope.innerHTML.replace('""', '"' + $scope.LPMOEQ_Question2 + '"');

                }
                else {
                    if ($scope.normalquestiontext != undefined) {
                        if ($scope.normalquestiontext == null || $scope.normalquestiontext == "") {
                            swal("Kindly Enter Question!!");
                            return;
                        }

                        $scope.innerHTML = $scope.normalquestiontext;
                    }
                    else {
                        var htmlEditor = document.getElementById('data');
                        $scope.innerHTML = htmlEditor.value;
                        if ($scope.innerHTML == null || $scope.innerHTML == "") {
                            swal("Kindly Enter Question!!");
                            return;
                        }
                    }
                }

                if ($scope.LPMOEQ_SubjectiveFlg === false && $scope.LPMOEQ_MatchTheFollowingFlg == false) {
                    var checkansflag = 0;
                    var checkallansfill = 0;
                    angular.forEach($scope.totalgrid, function (d, index) {
                        if ($scope.noraml == false) {
                            var htmlEditor = document.getElementById('htmlEditorlable' + index);
                            $scope.ansinnerHTML = htmlEditor.innerHTML;
                            //$scope.ansinnerHTML = $scope.ansinnerHTML.replace('data-custom-editor="chemistry" ', "");
                            d.LPMOEQOA_OptionCode = $scope.ansinnerHTML;
                        }
                        if (d.LPMOEQOA_AnswerFlag === true) {
                            checkansflag += 1;
                        }
                        //if ($scope.ansinnerHTML == "" || $scope.ansinnerHTML == null) {
                        //    checkallansfill += 1;
                        //}
                    });

                    if (checkallansfill > 0) {
                        swal("Kindly Enter all answers!!");
                        return;
                    }
                    if (checkansflag === 0) {
                        swal("Select One Correct Answer Flag");
                        return;
                    }
                }
                var mf_savecount = 0;

                if ($scope.LPMOEQ_SubjectiveFlg === false && $scope.LPMOEQ_MatchTheFollowingFlg === true) {
                    $scope.totalgrid = [];

                    var cols_order_count = 0;
                    angular.forEach($scope.colms_array, function (cols_ords) {
                        cols_order_count += 1;
                        cols_ords.cols_order = cols_order_count;
                    });


                    angular.forEach($scope.rows_array, function (rows) {
                        var LPMOEQOA_Id_Temp = 0;
                        LPMOEQOA_Id_Temp = rows.lpmoeqoA_Id !== undefined && rows.lpmoeqoA_Id !== null ? rows.lpmoeqoA_Id : 0;

                        $scope.Temp_Colms_Array = [];
                        angular.forEach(rows.cols_rows_array, function (dd_rows_cols, index_rows_cols) {
                            angular.forEach($scope.colms_array, function (dd_cols, index_cols) {
                                if (index_rows_cols === index_cols) {
                                    var LPMOEQOAMF_Id_Temp = 0;
                                    LPMOEQOAMF_Id_Temp = dd_rows_cols.lpmoeqoamF_Id !== undefined && dd_rows_cols.lpmoeqoamF_Id !== null ?
                                        dd_rows_cols.lpmoeqoamF_Id : 0;

                                    $scope.Temp_Colms_Array.push({
                                        LPMOEQOAMF_Id: LPMOEQOAMF_Id_Temp,
                                        LPMOEQOAMF_MatchtheFollowing: dd_cols.colname,
                                        LPMOEQOAMF_AnswerFlag: dd_rows_cols.MF_CorrectedAns,
                                        LPMOEQOA_Id: LPMOEQOA_Id_Temp,
                                        LPMOEQOAMF_Order: dd_cols.cols_order
                                    });
                                }
                            });
                        });

                        $scope.totalgrid.push({
                            LPMOEQOA_Option: rows.rowname, LPMOEQOA_OptionCode: rows.rowname, LPMOEQOA_AnswerFlag: false,
                            LPMOEQOA_Id: LPMOEQOA_Id_Temp, Temp_MF_OptionsDTO: $scope.Temp_Colms_Array, LPMOEQOA_Marks: rows.LPMOEQOA_Marks
                        });
                    });
                }

                var questionanswer = "";
                if ($scope.LPMOEQ_SubjectiveFlg === true) {
                    questionanswer = $scope.LPMOEQ_Answer !== undefined && $scope.LPMOEQ_Answer !== null && $scope.LPMOEQ_Answer !== "" ? $scope.LPMOEQ_Answer : "";
                }

                var noofoptionsnew = 0;
                if ($scope.LPMOEQ_SubjectiveFlg === false) {
                    noofoptionsnew = $scope.LPMOEQ_NoOfOptions !== undefined && $scope.LPMOEQ_NoOfOptions !== null && $scope.LPMOEQ_NoOfOptions !== "" ? $scope.LPMOEQ_NoOfOptions : 0;
                }


                if ($scope.LPMOEQ_MatchTheFollowingFlg === true) {
                    angular.forEach($scope.rows_array, function (d) {
                        var count_answer = 0;
                        angular.forEach(d.cols_rows_array, function (dd) {
                            if (dd.MF_CorrectedAns) {
                                count_answer += 1;
                            }
                        });

                        if (count_answer === 0) {
                            mf_savecount = 1;
                            swal("Kindly Select Correct Answer For " + d.rowname + " And Cross Check Others Also");
                            return false;
                        }
                    });


                    $scope.mf_marks = 0;
                    angular.forEach($scope.rows_array, function (d) {
                        if (d.LPMOEQOA_Marks !== undefined && d.LPMOEQOA_Marks !== null && d.LPMOEQOA_Marks !== '') {
                            $scope.mf_marks += Number(d.LPMOEQOA_Marks);
                        }
                    });

                    if ($scope.mf_marks !== Number($scope.LPMOEQ_Marks)) {
                        mf_savecount = 1;
                        swal("Match The Following Option Marks Should Be Equal To Total Question Marks");
                        return false;
                    }
                }

                var question_type = "";
                if ($scope.noraml === true) {
                    question_type = "Text";
                }
                else if ($scope.mathtype === true) {
                    question_type = "Math/Chem";
                    var valuetrue = $scope.innerHTML.includes('data-custom-editor="chemistry"');
                    if (valuetrue === true) {
                        question_type = 'Chem';
                    } else {
                        question_type = 'Math';
                    }
                }
                else if ($scope.LPMOEQ_StructuralFlg === true) {
                    question_type = "Structural";
                }

                var data = {
                    //"ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ISMS_Id": $scope.ISMS_Id,
                    "LPMT_Id": $scope.LPMT_Id,
                    "LPMOEQ_Question": $scope.innerHTML,
                    "LPMOEQ_QuestionDesc": desc,
                    "LPMOEQ_Id": $scope.LPMOEQ_Id,
                    "LPMOEQ_Answer": questionanswer,
                    "LPMOEQ_SubjectiveFlg": $scope.LPMOEQ_SubjectiveFlg,
                    "LPMOEQ_StructuralFlg": question_type,
                    "LPMOEQ_MatchTheFollowingFlg": $scope.LPMOEQ_MatchTheFollowingFlg,
                    "LPMOEQ_NoOfOptions": noofoptionsnew,
                    "LPMOEQ_MFRowCount": $scope.NoOfRows,
                    "LPMOEQ_MFColumnCount": $scope.NoOfColumns,
                    "LPMOEQ_Marks": $scope.LPMOEQ_Marks,
                    "LPMCOMP_Id": $scope.LPMCOMP_Id,
                    "tempfilesDTO": $scope.teacherdocuupload,
                    "tempoptionsDTO": $scope.totalgrid
                };

                if (mf_savecount === 0) {
                    apiService.create("LP_OnlineExam/SaveMasterQuestionDetails", data).then(function (promise) {
                        if (promise.returnduplicatestatus === 'Duplicate') {
                            swal('Record Already Exist');
                        } else {
                            if (promise.message === "Add") {
                                if (promise.returnval === true) {
                                    swal("Record Saved Successfully");
                                    $state.reload();
                                } else {
                                    swal("Failed To Save Record");
                                }
                            } else if (promise.message === "Update") {
                                if (promise.returnval === true) {
                                    swal("Record Updated Successfully");
                                    $state.reload();
                                } else {
                                    swal("Failed To Update Record");
                                }
                            } else {
                                swal("Failed To Save/Update Record");
                            }
                        }
                        if ($scope.LPMOEQ_Id > 0) {
                            $state.reload();
                        } else {
                            $scope.LPMOEQ_Question = "";
                            $scope.LPMOEQ_QuestionDesc = "";
                            $scope.LPMOEQ_NoOfOptions = "";
                            $scope.LPMOEQ_Answer = "";
                            $scope.LPMCOMP_Id = "";
                            $scope.LPMOEQ_Marks = "";
                            $scope.LPMOEQ_SubjectiveFlg = false;
                            $scope.totalgrid = [];
                            $scope.teacherdocuupload = {};
                            $scope.teacherdocuupload = [{ id: 'Teacher1' }];
                            $scope.teacherdocuuploadopts = {};
                            $scope.teacherdocuuploadopts = [{ id: 'Teacheropts1' }];
                            $scope.loaddata();
                        }
                        //$scope.cleartabl1();
                    });
                }

            }
            else {
                angular.forEach($scope.myForm.$error.required, function (dd) {
                    if (dd.$name == 'qus1') {
                        $scope.flagg = true;
                    }
                })
                if ($scope.flagg == true && $scope.mathtype == true) {
                    swal("Kindly Enter Question!!");
                }
                $scope.submitted1 = true;
            }
        };

        $scope.clicimage = function () {
            $scope.questionsource = true;
        };

        $scope.checkclicimage = function (checkvalue) {
            if (checkvalue == true) {
                $scope.mathtype = false;
                $scope.noraml = false;
            }
        };

        $scope.checkmath = function (checkvalue) {
            if (checkvalue == true) {
                $scope.LPMOEQ_StructuralFlg = false;
                $scope.noraml = false;
                //document.getElementById("editorIcon").click(); 
            }
        };

        $scope.checknoraml = function (checkvalue) {
            if (checkvalue == true) {
                $scope.LPMOEQ_StructuralFlg = false;
                $scope.mathtype = false;
            }
        };

        $scope.EditMasterQuestion = function (id) {
            $scope.totalgrid = [];
            $scope.colms_array = [];
            $scope.rows_array = [];
            $scope.teacherdocuupload = [{ id: 'Teacher1' }];
            $scope.teacherdocuuploadopts = [{ id: 'Teacheropts1' }];
            $scope.ASMCL_Id = "";
            $scope.ISMS_Id = "";
            $scope.LPMT_Id = "";
            $scope.LPMCOMP_Id = "";
            $scope.LPMOEQ_Marks = "";
            $scope.LPMOEQ_Question = "";
            $scope.LPMOEQ_QuestionDesc = "";
            $scope.LPMOEQ_Id = "";
            $scope.LPMOEQ_Answer = "";
            $scope.LPMOEQ_NoOfOptions = "";
            $scope.LPMOEQ_SubjectiveFlg = false;
            $scope.questiondisplay = false;
            $scope.edit = false;
            $scope.edittool = true;
            $scope.getSubjects = [];
            $scope.gettopiclist = [];
            $scope.getViewSavedOptionsFiles = [];
            $scope.getSavedOptions = [];
            $scope.questionsource = false;
            $scope.LPMOEQ_Question2 = "";
            $scope.LPMOEQ_Question1 = "";
            var data = {
                "LPMOEQ_Id": id
            };

            apiService.create("LP_OnlineExam/EditMasterQuestion", data).then(function (promise) {
                if (promise.geteditmasterquestion !== null && promise.geteditmasterquestion.length > 0) {

                    $scope.editflag = false;
                    if (promise.examconducted_count !== undefined && promise.examconducted_count !== null && promise.examconducted_count !== null && promise.examconducted_count > 0) {
                        $scope.editflag = true;
                        swal("Question Can Not Be Edit , Exam Is Conduted For This Question");
                    }
                    else {
                        $scope.questiondisplay = true;
                        //$scope.ASMAY_Id = promise.geteditmasterquestion[0].asmaY_Id;
                        $scope.ASMCL_Id = promise.geteditmasterquestion[0].asmcL_Id;
                        $scope.ISMS_Id = promise.geteditmasterquestion[0].ismS_Id;
                        $scope.LPMT_Id = promise.geteditmasterquestion[0].lpmT_Id;
                        $scope.LPMCOMP_Id = promise.geteditmasterquestion[0].lpmcomP_Id;
                        $scope.LPMOEQ_Marks = promise.geteditmasterquestion[0].lpmoeQ_Marks;
                        $scope.LPMOEQ_Question = promise.geteditmasterquestion[0].lpmoeQ_Question;

                        $scope.LPMOEQ_QuestionDesc = promise.geteditmasterquestion[0].lpmoeQ_QuestionDesc;
                        $scope.LPMOEQ_Id = promise.geteditmasterquestion[0].lpmoeQ_Id;
                        $scope.LPMOEQ_SubjectiveFlg = promise.geteditmasterquestion[0].lpmoeQ_SubjectiveFlg;
                        $scope.LPMOEQ_StructuralFlg = promise.geteditmasterquestion[0].lpmoeQ_StructuralFlg === 'Structural' ? true : false;
                        $scope.noraml = promise.geteditmasterquestion[0].lpmoeQ_StructuralFlg === 'Text' ? true : false;
                        $scope.mathtype = promise.geteditmasterquestion[0].lpmoeQ_StructuralFlg === 'Math' || promise.geteditmasterquestion[0].lpmoeQ_StructuralFlg === 'Chem' ? true : false;

                        $scope.LPMOEQ_MatchTheFollowingFlg = promise.geteditmasterquestion[0].lpmoeQ_MatchTheFollowingFlg == null ? false : promise.geteditmasterquestion[0].lpmoeQ_MatchTheFollowingFlg;

                        if ($scope.LPMOEQ_StructuralFlg == true) {

                            var chemsplitstredit = promise.geteditmasterquestion[0].lpmoeQ_Question.split(' ');
                            $scope.chemstrimg1 = chemsplitstredit[3];

                            if ($scope.chemstrimg1 != undefined) {
                                var chemstrimg2 = $scope.chemstrimg1.split('"');
                                $scope.LPMOEQ_Question2 = chemstrimg2[1];
                            }
                            //$scope.LPMOEQ_Question2 = promise.geteditmasterquestion[0].lpmoeQ_Question;
                            $scope.LPMOEQ_Question1 = "abx";
                        }
                        else if ($scope.mathtype === true) {
                            var htmlEditor = document.getElementById('htmlEditor');
                            htmlEditor.innerHTML = WirisPlugin.Parser.initParse(promise.geteditmasterquestion[0].lpmoeQ_Question);
                        } else {
                            $scope.normalquestiontext = $scope.LPMOEQ_Question;
                        }

                        //else {
                        //    var chemsplitstreditnormal = promise.geteditmasterquestion[0].lpmoeQ_Question.split(' ');
                        //    $scope.chemstrimgnormal = chemsplitstreditnormal[3];

                        //    if ($scope.chemstrimgnormal != undefined) {
                        //        var chemstrimg2 = $scope.chemstrimgnormal.split('"');
                        //        if (chemstrimg2[0] == "src=") {
                        //            var htmlEditor = document.getElementById('htmlEditor');
                        //            htmlEditor.innerHTML = WirisPlugin.Parser.initParse($scope.LPMOEQ_Question);
                        //            $scope.LPMOEQ_Question1 = $scope.LPMOEQ_Question;                                   
                        //        }
                        //        else {                                    
                        //            $scope.normalquestiontext = $scope.LPMOEQ_Question;
                        //        }
                        //    }
                        //    else {                                
                        //        $scope.normalquestiontext = $scope.LPMOEQ_Question;
                        //    }
                        //}

                        $scope.LPMOEQ_Answer = promise.geteditmasterquestion[0].lpmoeQ_Answer;
                        $scope.LPMOEQ_NoOfOptions = promise.geteditmasterquestion[0].lpmoeQ_NoOfOptions;
                        $scope.NoOfRows = promise.geteditmasterquestion[0].lpmoeQ_MFRowCount;
                        $scope.NoOfColumns = promise.geteditmasterquestion[0].lpmoeQ_MFColumnCount;

                        $scope.teacherdocuupload = promise.geteditdocuments;
                        $scope.edit = true;

                        $scope.classlist = promise.geteditclasslist;
                        $scope.getSubjects = promise.geteditsubjectlist;
                        $scope.gettopiclist = promise.getedittopiclist;

                        if ($scope.teacherdocuupload !== null && $scope.teacherdocuupload.length > 0) {
                            angular.forEach($scope.teacherdocuupload, function (dd) {
                                var img = dd.lpmoeqF_FilePath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("data.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpmoeqF_FilePath;
                                }
                            });
                        } else {
                            $scope.teacherdocuupload = [{ id: 'Teacher1' }];
                        }

                        if ($scope.LPMOEQ_SubjectiveFlg === false && $scope.LPMOEQ_MatchTheFollowingFlg == false) {
                            $scope.getSavedOptions = promise.getSavedOptions;

                            if ($scope.getSavedOptions !== null && $scope.getSavedOptions.length > 0) {
                                angular.forEach($scope.getSavedOptions, function (dd) {
                                    $scope.totalgrid.push({
                                        LPMOEQOA_Option: dd.lpmoeqoA_Option, LPMOEQOA_OptionCode: dd.lpmoeqoA_OptionCode,
                                        LPMOEQOA_AnswerFlag: dd.lpmoeqoA_AnswerFlag, LPMOEQOA_Id: dd.lpmoeqoA_Id
                                    });
                                });
                                $scope.getViewSavedOptionsFiles = promise.getViewSavedOptionsFiles;
                                $scope.tempoptsfiles = [];
                                angular.forEach($scope.totalgrid, function (ddd) {
                                    $scope.tempoptsfiles = [];
                                    angular.forEach($scope.getViewSavedOptionsFiles, function (df) {
                                        if (ddd.LPMOEQOA_Id === df.lpmoeqoA_Id) {
                                            $scope.tempoptsfiles.push({
                                                lpmoeqoaF_FilePath: df.lpmoeqoaF_FilePath, lpmoeqoaF_FileName: df.lpmoeqoaF_FileName,
                                                lpmoeqoaF_Id: df.lpmoeqoaF_Id
                                            });
                                        }
                                    });
                                    if ($scope.tempoptsfiles.length > 0) {
                                        ddd.tempoptionsfiles = $scope.tempoptsfiles;
                                    }
                                });
                                $scope.changecheck = true;
                                $scope.noofoptionstemp = 0;
                                $timeout(function () { $scope.addeditors(); }, 1000);
                            } else {
                                $scope.totalgrid = [];
                                for (var i = 0; i < $scope.noofoptions; i++) {
                                    $scope.totalgrid.push({ LPMOEQOA_Option: '', LPMOEQOA_OptionCode: '', LPMOEQOA_AnswerFlag: false, LPMOEQOA_Id: 0 });
                                }
                            }
                            // Match The Following
                        }

                        else if ($scope.LPMOEQ_SubjectiveFlg === false && $scope.LPMOEQ_MatchTheFollowingFlg == true) {
                            $scope.questiondisplay = false;
                            $scope.getSavedOptions = promise.getSavedOptions;
                            $scope.getviewsavedmfoptions = promise.getviewsavedmfoptions;

                            $scope.colms_array = [];
                            $scope.Temp_Distinct_cols = [];

                            $scope.Temp_Distinct_cols = $scope.getviewsavedmfoptions.filter((item, i, arr) => arr.findIndex((t) => t.lpmoeqoamF_MatchtheFollowing === item.lpmoeqoamF_MatchtheFollowing) === i);

                            angular.forEach($scope.Temp_Distinct_cols, function (dd, i) {
                                $scope.colms_array.push({ id: i, colname: dd.lpmoeqoamF_MatchtheFollowing });
                            });

                            $scope.rows_array = [];
                            angular.forEach($scope.getSavedOptions, function (d, i) {
                                d.rowname = d.lpmoeqoA_Option;
                                d.LPMOEQOA_Marks = d.lpmoeqoA_Marks;
                                d.id = i;
                                $scope.Temp_array = [];
                                angular.forEach($scope.getviewsavedmfoptions, function (dd) {
                                    if (d.lpmoeqoA_Id === dd.lpmoeqoA_Id) {
                                        //dd.MF_CorrectedAns = dd.lpmoeqoamF_AnswerFlag
                                        $scope.Temp_array.push({
                                            lpMOEQOAMF_Order: dd.lpmoeqoamF_Order,
                                            MF_CorrectedAns: dd.lpmoeqoamF_AnswerFlag,
                                            lpmoeqoamF_Id: dd.lpmoeqoamF_Id,
                                            lpmoeqoamF_MatchtheFollowing: dd.lpmoeqoamF_MatchtheFollowing,
                                            lpmoeqoamF_AnswerFlag: dd.lpmoeqoamF_AnswerFlag,
                                            lpmoeqoA_Id: dd.lpmoeqoA_Id
                                        });
                                    }
                                });
                                d.cols_rows_array = $scope.Temp_array;
                                $scope.rows_array.push(d);
                            });
                        }
                    }
                    $scope.scroll();
                    $timeout(function () { $scope.editeditors(); }, 1000);
                }
            });
        };

        $scope.editeditors = function () {
            angular.forEach($scope.totalgrid, function (dd, index) {
                var htmlEditorlable = document.getElementById('htmlEditorlable' + index);
                htmlEditorlable.innerHTML = WirisPlugin.Parser.initParse(dd.LPMOEQOA_OptionCode);
            });
        }

        $scope.vieweditors1 = function () {
            angular.forEach($scope.getoptiondetails, function (dd, index) {
                $scope.opeditstrimg3 = "";
                dd.imgOptionCode = "";
                if (dd.lpmoeqoA_OptionCode != null && dd.lpmoeqoA_OptionCode != '') {
                    var opsplitstredit = dd.lpmoeqoA_OptionCode.split(' ');
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

        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        }

        $scope.ViewMasterQuesDoc = function (dd) {
            var data = {
                "LPMOEQ_Id": dd.lpmoeQ_Id
            };
            apiService.create("LP_OnlineExam/ViewMasterQuesDoc", data).then(function (promise) {
                if (promise !== null) {

                    $scope.viewdocarray = promise.getviedocarray;

                    angular.forEach($scope.viewdocarray, function (dd, index) {
                        if (index === 0) {
                            dd.classname = 'active';
                            dd.classnamed = "item active";
                        } else {
                            dd.classnamed = "item";
                        }
                        var img = dd.lpmoeqF_FilePath;
                        var imagarr = img.split('.');
                        var lastelement = imagarr[imagarr.length - 1];
                        dd.filetype = lastelement;
                        console.log("data.filetype : " + dd.filetype);
                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                            dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpmoeqF_FilePath;
                        }
                    });



                    //for (var i = 0; i < m.length; i++) {
                    //    $('<div class="item"><img src="' + $scope.viewdocarray[i].lpmoeqF_FilePath + '"><div class="carousel-caption"></div></div>').appendTo('.carousel-inner');
                    //    $('<li data-target="#carousel-example-generic" data-slide-to="' + i + '"></li>').appendTo('.carousel-indicators')

                    //}
                    //$('.item').first().addClass('active');
                    //$('.carousel-indicators > li').first().addClass('active');
                    //$('#carousel-example-generic').carousel();


                }
            });
        };

        $scope.ViewMasterQuesOptions = function (dd) {
            $scope.Temp_Distinct_cols = [];
            $scope.Temp_Distinct_rows = [];
            $scope.Question = dd.lpmoeQ_Question;

            $scope.editstrimg3 = "";
            $scope.imgQuestion = "";
            if ($scope.Question != null && $scope.Question != '') {
                var splitstredit = $scope.Question.split(' ');
                $scope.editstrimg1 = splitstredit[3];
                if ($scope.editstrimg1 != undefined) {
                    var editstrimg2 = $scope.editstrimg1.split('"');
                    $scope.editstrimg3 = editstrimg2[1];
                }
                else {
                    $scope.editstrimg3 = undefined;
                }
            }
            if ($scope.editstrimg3 != undefined) {
                $scope.imgQuestion = $scope.editstrimg3;
            }

            $scope.Subject = dd.ismS_SubjectName;
            $scope.TopicName = dd.lpmT_TopicName;
            $scope.ClassName = dd.asmcL_ClassName;
            $scope.lpmoeQ_SubjectiveFlgdetails = dd.lpmoeQ_SubjectiveFlg;
            $scope.lpmoeQ_MatchTheFollowingFlg = dd.lpmoeQ_MatchTheFollowingFlg;

            if (dd.lpmoeQ_SubjectiveFlg === true) {
                $scope.answerdetails = dd.lpmoeQ_Answer;
                return;
            }

            var data = {
                "LPMOEQ_Id": dd.lpmoeQ_Id,
                "LPMOEQ_MatchTheFollowingFlg": dd.lpmoeQ_MatchTheFollowingFlg,
            };

            apiService.create("LP_OnlineExam/ViewMasterQuesOptions", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getoptiondetails = promise.getViewSavedOptions;
                    $scope.vieweditors1();

                    if (dd.lpmoeQ_MatchTheFollowingFlg === true) {
                        $scope.getviewsavedmfoptions = promise.getviewsavedmfoptions;

                        $scope.Temp_Distinct_cols = $scope.getviewsavedmfoptions.filter((item, i, arr) => arr.findIndex((t) => t.lpmoeqoamF_MatchtheFollowing === item.lpmoeqoamF_MatchtheFollowing) === i);


                        angular.forEach($scope.getoptiondetails, function (rows) {
                            $scope.cols_array_Temp = [];
                            angular.forEach($scope.getviewsavedmfoptions, function (rows_mf) {
                                if (rows.lpmoeqoA_Id === rows_mf.lpmoeqoA_Id) {
                                    $scope.cols_array_Temp.push(rows_mf);
                                }
                            });
                            rows.cols_rows_array = $scope.cols_array_Temp;
                        });
                    }
                    //$timeout(function () { $scope.vieweditors(); }, 500);                  

                }
            });
        };

        $scope.ViewUploadOptionFiles = function (dd) {
            var data = {
                "LPMOEQOA_Id": dd.lpmoeqoA_Id,
                "LPMOEQ_Id": dd.lpmoeQ_Id
            };

            apiService.create("LP_OnlineExam/ViewUploadOptionFiles", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getViewSavedOptionsFiles = promise.getViewSavedOptionsFiles;

                    angular.forEach($scope.getViewSavedOptionsFiles, function (dd) {
                        var img = dd.lpmoeqoaF_FilePath;
                        var imagarr = img.split('.');
                        var lastelement = imagarr[imagarr.length - 1];
                        dd.filetype = lastelement;
                        console.log("data.filetype : " + dd.filetype);
                        if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                            dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpmoeqoaF_FilePath;
                        }
                    });
                }
            });
        };

        $scope.DeactivateActivateQuestion = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.lpmoeQ_ActiveFlg === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            var data = {
                "LPMOEQ_Id": deactiveRecord.lpmoeQ_Id
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
                        apiService.create("LP_OnlineExam/DeactivateActivateQuestion", data).then(function (promise) {
                            if (promise.message !== 'Mapped') {
                                if (promise.returnval === true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                                $state.reload();
                            } else {
                                swal("Record Is Already Mapped, It Can Not Be Deactivate");
                            }
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.DeactivateActivateDocument = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.lpmoeqF_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            var data = {
                "LPMOEQF_Id": deactiveRecord.lpmoeqF_Id,
                "LPMOEQ_Id": deactiveRecord.lpmoeQ_Id
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
                        apiService.create("LP_OnlineExam/DeactivateActivateDocument", data).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record " + confirmmgs + " " + "successfully");
                            }
                            else {
                                swal("Record " + mgs + " Failed");
                            }

                            $scope.viewdocarray = promise.getviedocarray;

                            angular.forEach($scope.viewdocarray, function (dd) {
                                var img = dd.lpmoeqF_FilePath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("data.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpmoeqF_FilePath;
                                }
                            });
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.DeactivateActivateQuesOption = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.lpmoeqoA_ActiveFlg === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            var data = {
                "LPMOEQOA_Id": deactiveRecord.lpmoeqoA_Id,
                "LPMOEQ_Id": deactiveRecord.lpmoeQ_Id
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
                        apiService.create("LP_OnlineExam/DeactivateActivateQuesOption", data).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record " + confirmmgs + " " + "successfully");
                            }
                            else {
                                swal("Record " + mgs + " Failed");
                            }
                            $scope.getoptiondetails = promise.getViewSavedOptions;
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.DeactivateActivateOptionsDocument = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.lpmoeqoaF_ActiveFlag === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            var data = {
                "LPMOEQOA_Id": deactiveRecord.lpmoeqoA_Id,
                "LPMOEQOAF_Id": deactiveRecord.lpmoeqoaF_Id
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
                        apiService.create("LP_OnlineExam/DeactivateActivateOptionsDocument", data).then(function (promise) {
                            if (promise.returnval === true) {
                                swal("Record " + confirmmgs + " " + "successfully");
                            }
                            else {
                                swal("Record " + mgs + " Failed");
                            }
                            $scope.getViewSavedOptionsFiles = promise.getViewSavedOptionsFiles;

                            angular.forEach($scope.getViewSavedOptionsFiles, function (dd) {
                                var img = dd.lpmoeqoaF_FilePath;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                dd.filetype = lastelement;
                                console.log("data.filetype : " + dd.filetype);
                                if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                    dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpmoeqoaF_FilePath;
                                }
                            });
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.cleartabl1 = function () {
            $state.reload();
            $scope.scroll();
        };

        $scope.searchValue1 = function (obj) {
            return (angular.lowercase(obj.lpmoeQ_Question)).indexOf(angular.lowercase($scope.obj.searchValueddd)) >= 0 ||
                (angular.lowercase(obj.lpmoeQ_QuestionDesc)).indexOf(angular.lowercase($scope.obj.searchValueddd)) >= 0 ||
                (angular.lowercase(obj.asmcL_ClassName)).indexOf(angular.lowercase($scope.obj.searchValueddd)) >= 0 ||
                (angular.lowercase(obj.lpmT_TopicName)).indexOf(angular.lowercase($scope.obj.searchValueddd)) >= 0 ||
                (angular.lowercase(obj.ismS_SubjectName)).indexOf(angular.lowercase($scope.obj.searchValueddd)) >= 0 ||
                (JSON.stringify(obj.lpmoeQ_Marks)).indexOf($scope.searchValueddd) >= 0;
        };

        $scope.interacted = function (field) {
            return $scope.submitted1;
        };

        $scope.showmodaldetails = function (data) {
            $('#preview').attr('src', data.document_Path);
        };

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

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };

        $scope.pauseOrPlay = function (ele) {
            $('#popup15').modal({
                show: false
            }).on('hidden.bs.modal', function () {
                $(this).find('video')[0].pause();
            });
        };

        $scope.showGuardianPhotonew = function (data) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.lpmoeqF_FilePath;
            $scope.videdfd = data.lpmoeqF_FilePath;
            $scope.movie = { src: data.lpmoeqF_FilePath };
            $scope.movie1 = { src: data.lpmoeqF_FilePath };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.lpmoeqF_FilePath });
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

        $scope.addColumn = function (role, indexx, totalgrid) {
            angular.forEach(totalgrid, function (subscription, index) {
                if (indexx !== index)
                    subscription.LPMOEQOA_AnswerFlag = false;
            });
        };

        $scope.OnChangeOptionCode = function (user, index) {
            angular.forEach($scope.totalgrid, function (dd, i) {

                if (index !== i) {
                    if (user.LPMOEQOA_Option === dd.LPMOEQOA_Option) {
                        user.LPMOEQOA_Option = "";
                        swal("Answer Option Already Exists");
                    }
                }
            });
        };

        /* Questions Files */
        $scope.uploadtecherdocuments1 = [];
        $scope.uploadtecherdocuments = function (input, document) {

            $scope.uploadtecherdocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

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
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload  Pdf, Doc, Image Files Only");
                }
            }
        };
        function UploaddianPhoto(data) {
            console.log("Teacher Upload  :" + data);
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
                    data.lpmoeqF_FilePath = d;
                    data.lpmoeqF_FileName = $scope.filename;
                    $('#').attr('src', data.lpmoeqF_FilePath);
                    var img = data.lpmoeqF_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    console.log("data.filetype : " + data.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpmoeqF_FilePath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        /* Options Files */
        $scope.uploadtecheroptsdocuments1 = [];
        $scope.uploadtecheroptsdocuments = function (input, document) {

            $scope.uploadtecheroptsdocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" || input.files[0].type === "image/png" || input.files[0].type === "image/jpg")  // 2097152 bytes = 2MB 
                {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                    UploaddianPhotoopts(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload  Pdf, Doc, Image Files Only");
                }
            }
        };
        function UploaddianPhotoopts(data) {
            console.log("Teacher Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadtecheroptsdocuments1.length; i++) {
                formData.append("File", $scope.uploadtecheroptsdocuments1[i]);
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
                    data.lpmoeqoaF_FilePath = d;
                    data.lpmoeqoaF_FileName = $scope.filename;
                    $('#').attr('src', data.lpmoeqoaF_FilePath);
                    var img = data.lpmoeqoaF_FilePath;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    console.log("data.filetype : " + data.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpmoeqoaF_FilePath;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
        };

        $scope.sort = function (key) {
            $scope.sortKey = key;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
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

            //if ($scope.colms !== $scope.rows) {
            //    $scope.hide = true;
            //    swal("No.Of Rows And No.Of Columns Should Be Equal");
            //    return;
            //}

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

                        angular.forEach(dd.cols_rows_array, function (dd_cols_rws) {
                            dd_cols_rws.MF_CorrectedAns = false;
                        });

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
            angular.forEach($scope.rows_array, function (d) {
                if (d.LPMOEQOA_Marks !== undefined && d.LPMOEQOA_Marks !== null && d.LPMOEQOA_Marks !== '') {
                    $scope.mf_marks += Number(d.LPMOEQOA_Marks);
                }
            });


            if ($scope.mf_marks > $scope.ques_marks) {
                dd.LPMOEQOA_Marks = "";
                swal("Match The Following Option Marks Should Be Equal To Total Question Marks");
            }
        };

        $scope.isOptionsRequired = function () {
            angular.forEach($scope.rows_array, function (dd) {
                return !dd.cols_rows_array.some(function (options) {
                    return options.MF_CorrectedAns;
                });
            });
        }

        $scope.copydata = function () {
            var copyTextareaBtn = document.getElementById('data').value;
            //var x = localStorage.getItem("languageedit");
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