(function () {
    'use strict';
    angular.module('app').controller('HHSStudentProgressCardController', HHSStudentProgressCardController)

    HHSStudentProgressCardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter','$compile']
    function HHSStudentProgressCardController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter,$compile) {

        $scope.percounttotal = 0;
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.HHS_I_IV_grid = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.grandfinaltotal = 0;
        $scope.grandfinalmaxtotal = 0;
        $scope.grandtotalperc = 0;
        //TO  GEt The Values iN Grid
        $scope.grandavgtotal = 0;
        $scope.exm_sub_mrks_list = [];

        $scope.halfyearatt = [];
        $scope.fullyearatt = [];


        $scope.BindData = function () {
            var pageid = 2;

            apiService.getURI("StudentProgressCardReport/stmarygetdetails", pageid).then(function (promise) {
                $scope.btn = false;
                $scope.getstudentdetails = promise.getstudentdetails;
                $scope.year_list = promise.getyear;
                $scope.classlist = promise.getclass;
                $scope.examorterm = promise.examorterm;

                if ($scope.getstudentdetails !== null && $scope.getstudentdetails.length > 0) {

                    $scope.ASMCL_Id = $scope.getstudentdetails[0].asmcL_Id;
                    $scope.asmS_Id = $scope.getstudentdetails[0].asmS_Id;
                    $scope.asmaY_Id = $scope.getstudentdetails[0].asmaY_Id;

                    if ($scope.examorterm === 'Exam' || $scope.examorterm === 'Promotion') {
                        $scope.getexamtermlist = promise.getexamtermlist;
                        $scope.examflag = false;
                        $scope.termflag = false;                       
                    }              
                    else {
                        $scope.btn = true;
                        swal("Report Format Not Mapped Contact Administrator");
                    }
                } else {
                    $scope.btn = true;
                    swal("No Studnet Data Found");
                }
            });
        };

        $scope.onchangeclass = function () {
            $scope.getexamtermlist = [];
            $scope.obj.ECT_Id = "";
            $scope.obj.EME_Id = "";
            $scope.examflag = false;
            $scope.HHS_I_IV_grid = false;
            $scope.termflag = false;
            $scope.reportshowcount = 0;
            $scope.btn = false;
            if ($scope.ASMCL_Id !== undefined && $scope.ASMCL_Id !== null && $scope.ASMCL_Id !== "") {
                var data = {
                    "ASMCL_Id": $scope.ASMCL_Id
                };

                apiService.create("StudentProgressCardReport/stmaryonchangeclass", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.getstudentdetails = promise.getstudentdetails;

                        if ($scope.getstudentdetails !== null && $scope.getstudentdetails.length > 0) {

                            $scope.examorterm = promise.examorterm;

                            if ($scope.examorterm === 'Exam' || $scope.examorterm === 'Promotion') {
                                $scope.getexamtermlist = promise.getexamtermlist;
                                $scope.examflag = false;
                                $scope.termflag = false;
                            }
                            else {
                                $scope.btn = true;
                                swal("Report Format Not Mapped Contact Administrator");
                            }
                        } else {
                            $scope.btn = true;
                            swal("No Studnet Data Found");
                        }
                    }
                });
            }
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        // TO Save The Data
        $scope.submitted = false;

        $scope.saveddata = function () {
            $scope.HHS_I_IV_grid = false;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.grandavgtotal = 0;

                var data = {
                    "ASMCL_Id": $scope.ASMCL_Id
                };


                apiService.create("StudentProgressCardReport/HHSStudentProgressCardReport", data).then(function (promise) {

                    $scope.exm_sub_mrks_list = promise.eam_sub_mrks_list;
                    $scope.personality_status = promise.personality_status;
                    $scope.co_curricular_activity = promise.co_curricular_activity;

                    $scope.examwiseremarks = promise.examwiseremarks;

                    if ($scope.exm_sub_mrks_list !== null && $scope.exm_sub_mrks_list.length > 0) {
                        $scope.HHS_I_IV_grid = true;
                        $scope.stu_details = promise.stu_details;
                        $scope.stuname = promise.stu_details[0].stuname;
                        $scope.asmcL_ClassName = promise.stu_details[0].asmcL_ClassName;
                        $scope.asmC_SectionName = promise.stu_details[0].asmC_SectionName;
                        $scope.amaY_RollNo = promise.stu_details[0].amaY_RollNo;
                        $scope.amsT_RegistrationNo = promise.stu_details[0].amsT_RegistrationNo;
                        if (promise.stu_details[0].amsT_FatherName !== null && promise.stu_details[0].amsT_FatherName !== "") {
                            $scope.amsT_FatherName = promise.stu_details[0].amsT_FatherName;
                        }
                        else {
                            $scope.amsT_FatherName = promise.stu_details[0].amsT_MotherName;
                        }
                        $scope.amsT_PerStreet = promise.stu_details[0].amsT_PerStreet;
                        $scope.amsT_PerArea = promise.stu_details[0].amsT_PerArea;
                        $scope.amsT_PerCity = promise.stu_details[0].amsT_PerCity;
                        $scope.ivrmmS_Name = promise.stu_details[0].ivrmmS_Name;
                        $scope.ivrmmC_CountryName = promise.stu_details[0].ivrmmC_CountryName;
                        $scope.amsT_PerPincode = promise.stu_details[0].amsT_PerPincode;
                        $scope.amsT_DOB = promise.stu_details[0].amsT_DOB;
                        $scope.asmaY_Year = promise.stu_details[0].asmaY_Year;
                        $scope.amsT_EmailId = promise.stu_details[0].amsT_EmailId;
                        $scope.amsT_Mobile = promise.stu_details[0].amsT_Mobile;
                        $scope.AMST_Photoname = promise.stu_details[0].amsT_Photoname;

                        angular.forEach($scope.yearlt, function (y) {
                            if (y.asmaY_Id == $scope.asmaY_Id) {
                                $scope.yearname = y.asmaY_Year;
                            }
                        });

                        angular.forEach($scope.clslist, function (c) {
                            if (c.asmcL_Id == $scope.asmcL_Id) {
                                $scope.clasname = c.asmcL_ClassName;
                            }
                        });

                        angular.forEach($scope.seclist, function (s) {
                            if (s.asmS_Id == $scope.asmS_Id) {
                                $scope.sectioname = s.asmC_SectionName;
                            }
                        });

                        $scope.exam_list = [];
                        $scope.overalltotalmax = 0;
                        angular.forEach($scope.exm_sub_mrks_list, function (st) {
                            if ($scope.exam_list.length === 0) {
                                $scope.overalltotalmax = parseFloat($scope.overalltotalmax) + parseFloat(st.maxmarks);
                                $scope.exam_list.push({ emE_Id: st.emeid, emE_ExamName: st.examname, maxmarks: st.maxmarks });
                            }
                            else if ($scope.exam_list.length > 0) {
                                var al_exm_cnt = 0;
                                angular.forEach($scope.exam_list, function (exm) {
                                    if (exm.emE_Id == st.emeid) {
                                        al_exm_cnt += 1;
                                    }
                                });
                                if (al_exm_cnt === 0) {
                                    $scope.overalltotalmax = parseFloat($scope.overalltotalmax) + parseFloat(st.maxmarks);
                                    $scope.exam_list.push({ emE_Id: st.emeid, emE_ExamName: st.examname, maxmarks: st.maxmarks });
                                }
                            }
                        });

                        $scope.subject_list = [];
                        angular.forEach($scope.exm_sub_mrks_list, function (st) {
                            if ($scope.subject_list.length == 0) {
                                var ssubj_list = [];
                                angular.forEach($scope.exm_sub_mrks_list, function (main) {
                                    if (main.subid == st.subid && main.ssubj != '' && main.SubsubjectName != '') {
                                        if (ssubj_list.length == 0) {
                                            ssubj_list.push({ EMSS_Id: main.ssubj, SubSbuject: main.SubsubjectName });
                                        }
                                        else if (ssubj_list.length > 0) {
                                            var sssubj_cnt = 0;
                                            angular.forEach(ssubj_list, function (ssub) {
                                                if (ssub.EMSS_Id == main.ssubj) {
                                                    sssubj_cnt += 1;
                                                }
                                            });
                                            if (sssubj_cnt == 0) {
                                                ssubj_list.push({ EMSS_Id: main.ssubj, SubSbuject: main.SubsubjectName });
                                            }
                                        }
                                    }
                                });
                                $scope.subject_list.push({ ISMS_Id: st.subid, subjectname: st.subjectname, subsubjects: ssubj_list, flag: st.flag });
                            }
                            else if ($scope.subject_list.length > 0) {
                                var al_subj_cnt = 0;
                                angular.forEach($scope.subject_list, function (subj) {
                                    if (subj.ISMS_Id == st.subid) {
                                        al_subj_cnt += 1;
                                    }
                                });
                                if (al_subj_cnt == 0) {
                                    var ssubj_list = [];
                                    angular.forEach($scope.exm_sub_mrks_list, function (main) {
                                        if (main.subid == st.subid && main.ssubj != '' && main.SubsubjectName != '') {
                                            if (ssubj_list.length == 0) {
                                                ssubj_list.push({ EMSS_Id: main.ssubj, SubSbuject: main.SubsubjectName });
                                            }
                                            else if (ssubj_list.length > 0) {
                                                var sssubj_cnt = 0;
                                                angular.forEach(ssubj_list, function (ssub) {
                                                    if (ssub.EMSS_Id == main.ssubj) {
                                                        sssubj_cnt += 1;
                                                    }
                                                });
                                                if (sssubj_cnt == 0) {
                                                    ssubj_list.push({ EMSS_Id: main.ssubj, SubSbuject: main.SubsubjectName });
                                                }
                                            }
                                        }
                                    });
                                    $scope.subject_list.push({ ISMS_Id: st.subid, subjectname: st.subjectname, subsubjects: ssubj_list, flag: st.flag });
                                }
                            }
                        });

                        //for total
                        $scope.total_subwise = [];
                        angular.forEach($scope.subject_list, function (subj) {
                            var total_ssubj_marks = [];
                            angular.forEach(subj.subsubjects, function (ssubj) {
                                var Ssubj_marks = 0;
                                var Ssubj_mxmmarks = 0;
                                var Subj_per = 0;
                                var Ssubj_grade = "";
                                angular.forEach($scope.exm_sub_mrks_list, function (marks) {
                                    if (marks.subid == subj.ISMS_Id && marks.ssubj == ssubj.EMSS_Id) {
                                        if (marks.flag == true) {
                                            Ssubj_marks += Number(marks.obtainmarks);
                                            Ssubj_mxmmarks += Number(marks.maxmarks);
                                            Ssubj_grade = marks.ObtainedGrade
                                        }
                                    }
                                });

                                total_ssubj_marks.push({
                                    EMSS_Id: ssubj.EMSS_Id, SubSbuject: ssubj.SubSbuject, total_ss: Ssubj_marks,
                                    subsbmaxmrk: Ssubj_mxmmarks, subsubgrade: Ssubj_grade
                                });
                            });
                            var Subj_marks = 0;
                            var Subj_mxmmarks = 0;
                            var Subj_per = 0;
                            angular.forEach($scope.exm_sub_mrks_list, function (marks) {
                                if (marks.subid == subj.ISMS_Id && marks.ssubj == '' && marks.SubsubjectName == '') {
                                    if (marks.flag == true) {
                                        Subj_marks += Number(marks.obtainmarks);
                                        Subj_mxmmarks += Number(marks.Medicalmaxmarks);

                                    }
                                }
                            });
                            Subj_per = (Subj_marks / Subj_mxmmarks) * 100;
                            Subj_per = Subj_per.toFixed(2);
                            $scope.total_subwise.push({ ISMS_Id: subj.ISMS_Id, subjectname: subj.subjectname, total_s: Subj_marks, submaxmarks: Subj_mxmmarks, subjper: Subj_per, Ssubj_total: total_ssubj_marks });
                        });

                        console.log($scope.total_subwise);
                      
                        var pergrandtotal = 0;
                        var totalpertotal = 0;
                        angular.forEach($scope.total_subwise, function (xx) {

                            if (xx.submaxmarks != 0) {
                                if (xx.subjper !== "NA") {                                   
                                    pergrandtotal += Number(xx.subjper);
                                    totalpertotal += 100;
                                }
                            }
                            if (xx.Ssubj_total.length > 0) {
                                angular.forEach(xx.Ssubj_total, function (tt) {
                                    if (tt.subsbmaxmrk != 0) {
                                        pergrandtotal += ((tt.total_ss / tt.subsbmaxmrk) * 100);
                                        totalpertotal += 100;
                                    }
                                });
                            }
                        });

                        $scope.grandavgtotal = pergrandtotal;
                        $scope.percounttotal = totalpertotal;


                        /// grand total 
                        $scope.grandfinaltotal = 0;
                        $scope.grandfinalmaxtotal = 0;
                        $scope.grandtotalperc = 0;
                        angular.forEach($scope.total_subwise, function (qq) {                          
                            $scope.grandfinaltotal += qq.total_s;
                            $scope.grandfinalmaxtotal += qq.submaxmarks;                            
                        });

                        $scope.grandtotalperc = 0;
                        if ($scope.grandfinalmaxtotal != 0) {
                            $scope.grandtotalperc = ($scope.grandfinaltotal / $scope.grandfinalmaxtotal) * 100;
                        }

                        $scope.exmtpr = promise.exmTPR;

                        $scope.exam_subexamlist = promise.exam_subexam;
                        $scope.exam_subexam_W = promise.exam_subexam_W;

                        $scope.mainexam_list = [];
                        angular.forEach($scope.exam_subexam_W, function (me) {
                            if ($scope.mainexam_list.length == 0) {
                                var sub_exm_cnt = 0;
                                angular.forEach($scope.exam_subexam_W, function (sd) {
                                    if (sd.emE_Id == me.emE_Id) {
                                        sub_exm_cnt += 1;
                                    }
                                });
                                $scope.mainexam_list.push({ emE_Id: me.emE_Id, emE_ExamName: me.emE_ExamName, count_se: sub_exm_cnt });
                            }
                            else if ($scope.mainexam_list.length > 0) {
                                var main_exm_cnt = 0;
                                angular.forEach($scope.mainexam_list, function (exm) {
                                    if (exm.emE_Id == me.emE_Id) {
                                        main_exm_cnt += 1;
                                    }
                                });
                                if (main_exm_cnt == 0) {
                                    var sub_exm_cnt = 0;
                                    angular.forEach($scope.exam_subexam_W, function (sd) {
                                        if (sd.emE_Id == me.emE_Id) {
                                            sub_exm_cnt += 1;
                                        }
                                    });
                                    $scope.mainexam_list.push({ emE_Id: me.emE_Id, emE_ExamName: me.emE_ExamName, count_se: sub_exm_cnt });
                                }
                            }
                        });
                        $scope.subexam_list = [];
                        angular.forEach($scope.exam_subexam_W, function (sme) {
                            if ($scope.subexam_list.length == 0) {
                                $scope.subexam_list.push({
                                    emsE_Id: sme.emsE_Id, emsE_SubExamName: sme.emsE_SubExamName, emE_Id: sme.emE_Id, eycessS_MaxMarks: sme.eycessS_MaxMarks
                                });
                            }
                            else if ($scope.subexam_list.length > 0) {
                                var sub_exm_cnt = 0;
                                angular.forEach($scope.subexam_list, function (exm) {
                                    if (exm.emE_Id == sme.emE_Id && exm.emsE_Id == sme.emsE_Id) {
                                        sub_exm_cnt += 1;
                                    }
                                });
                                if (sub_exm_cnt == 0) {
                                    $scope.subexam_list.push({ emsE_Id: sme.emsE_Id, emsE_SubExamName: sme.emsE_SubExamName, emE_Id: sme.emE_Id, eycessS_MaxMarks: sme.eycessS_MaxMarks });
                                }
                            }
                        });
                        $scope.subexam_list11 = $scope.subexam_list;


                        $scope.marksprocess_Pro_subwise = promise.marksprocess_Pro_subwise;
                        $scope.marksprocess_Pro_markscal = promise.marksprocess_Pro_markscal;

                        $scope.subexwithtotal = [];
                        angular.forEach($scope.mainexam_list, function (xx) {
                            var zz = 0;
                            var subexamtotal = 0;
                            angular.forEach($scope.subexam_list, function (ww) {
                                if (xx.emE_Id == ww.emE_Id) {
                                    subexamtotal = subexamtotal + ww.eycessS_MaxMarks;
                                    $scope.subexwithtotal.push({ emE_Id: xx.emE_Id, emsE_Id: ww.emsE_Id, emsE_SubExamName: ww.emsE_SubExamName, eycessS_MaxMarks: ww.eycessS_MaxMarks });
                                }
                            });

                            $scope.subexwithtotal.push({ emE_Id: xx.emE_Id, emsE_SubExamName: 'Total', eycessS_MaxMarks: subexamtotal })
                        });

                        $scope.tempsub = [];
                        angular.forEach($scope.subject_list, function (cc) {
                            angular.forEach($scope.mainexam_list, function (vv) {
                                $scope.tempsub.push({ ismS_Id: cc.ISMS_Id, emE_Id: vv.emE_Id });
                            });
                        });

                        $scope.selectedsection = [];
                        angular.forEach($scope.marksprocess_Pro_markscal, function (stu2) {
                            if ($scope.selectedsection.length == 0) {
                                $scope.selectedsection.push({ EME_ExamName: stu2.EME_ExamName, EME_Id: stu2.EME_Id, EMSE_Id: stu2.EMSE_Id, EMSE_SubExamName: stu2.EMSE_SubExamName, ESTMPSSS_ObtainedMarks: stu2.ESTMPSSS_ObtainedMarks, ESTMPS_PassFailFlg: stu2.ESTMPS_PassFailFlg, ISMS_Id: stu2.ISMS_Id, MaxMarks: stu2.MaxMarks });
                            }
                            else if ($scope.selectedsection.length > 0) {
                                var al_ct = 0;
                                angular.forEach($scope.selectedsection, function (uf) {
                                    if (uf.EME_Id == stu2.EME_Id && uf.EMSE_Id == stu2.EMSE_Id && uf.ISMS_Id == stu2.ISMS_Id) {
                                        al_ct += 1;
                                    }
                                });
                                if (al_ct == 0) {
                                    $scope.selectedsection.push({ EME_ExamName: stu2.EME_ExamName, EME_Id: stu2.EME_Id, EMSE_Id: stu2.EMSE_Id, EMSE_SubExamName: stu2.EMSE_SubExamName, ESTMPSSS_ObtainedMarks: stu2.ESTMPSSS_ObtainedMarks, ESTMPS_PassFailFlg: stu2.ESTMPS_PassFailFlg, ISMS_Id: stu2.ISMS_Id, MaxMarks: stu2.MaxMarks });
                                }
                            }
                        });

                        $scope.marksprocess_Pro_markscal = $scope.selectedsection;

                        console.log($scope.marksprocess_Pro_markscal);
                        $scope.selectedsub = [];

                        angular.forEach($scope.marksprocess_Pro_markscal, function (stu2) {
                            if ($scope.selectedsub.length == 0) {
                                $scope.selectedsub.push({ ISMS_Id: stu2.ISMS_Id });
                            }
                            else if ($scope.selectedsub.length > 0) {
                                var al_ct = 0;
                                angular.forEach($scope.selectedsub, function (uf) {
                                    if (uf.ISMS_Id == stu2.ISMS_Id) {
                                        al_ct += 1;
                                    }
                                });
                                if (al_ct == 0) {
                                    $scope.selectedsub.push({ ISMS_Id: stu2.ISMS_Id });
                                }
                            }
                        });

                        console.log($scope.selectedsub);
                        console.log($scope.tempsub);
                        $scope.tempsub1 = [];

                        angular.forEach($scope.tempsub, function (t1) {
                            angular.forEach($scope.selectedsub, function (s1) {
                                if (s1.ISMS_Id == t1.ismS_Id) {
                                    $scope.tempsub1.push({ emE_Id: t1.emE_Id, ismS_Id: s1.ISMS_Id })
                                }
                            });
                        });

                        console.log($scope.tempsub1);
                        $scope.subexwithtotal_mrks = [];
                        angular.forEach($scope.tempsub1, function (xx) {
                            var totalbotained = 0;
                            var totalmaxMarks = 0;
                            angular.forEach($scope.marksprocess_Pro_markscal, function (ww) {
                                if (xx.emE_Id == ww.EME_Id && xx.ismS_Id == ww.ISMS_Id) {
                                    totalbotained += parseFloat(ww.ESTMPSSS_ObtainedMarks);
                                    if (ww.ESTMPS_PassFailFlg != 'M') {
                                        totalmaxMarks += ww.MaxMarks;
                                    }
                                    $scope.subexwithtotal_mrks.push({
                                        emE_Id: xx.emE_Id, emsE_Id: ww.EMSE_Id, obtainedMarks: ww.ESTMPSSS_ObtainedMarks, ismS_Id: ww.ISMS_Id, emsE_Id: ww.EMSE_Id, maxMarks: ww.MaxMarks
                                        , estmpS_PassFailFlg: ww.ESTMPS_PassFailFlg
                                    });
                                }
                            });
                            if (totalmaxMarks > 0) {
                                $scope.subexwithtotal_mrks.push({ emE_Id: xx.emE_Id, ismS_Id: xx.ismS_Id, emsE_SubExamName: 'Total', obtainedMarks: totalbotained, maxMarks: totalmaxMarks })
                            }
                        });


                        $scope.subexwithtotal_mrks1 = [];
                        angular.forEach($scope.tempsub1, function (xx) {
                            var totalbotained = 0;
                            var totalmaxMarks = 0;
                            angular.forEach($scope.marksprocess_Pro_markscal, function (ww) {
                                if (xx.emE_Id == ww.EME_Id && xx.ismS_Id == ww.ISMS_Id) {
                                    totalbotained += parseFloat(ww.ESTMPSSS_ObtainedMarks);
                                    totalmaxMarks += ww.MaxMarks
                                    $scope.subexwithtotal_mrks1.push({
                                        emE_Id: xx.emE_Id, emsE_Id: ww.EMSE_Id, obtainedMarks: ww.ESTMPSSS_ObtainedMarks, ismS_Id: ww.ISMS_Id,
                                        emsE_Id: ww.EMSE_Id, maxMarks: ww.MaxMarks, estmpS_PassFailFlg: ww.ESTMPS_PassFailFlg
                                    });
                                }
                            });

                            $scope.subexwithtotal_mrks1.push({
                                emE_Id: xx.emE_Id, ismS_Id: xx.ismS_Id, emsE_SubExamName: 'Total',
                                obtainedMarks: totalbotained, maxMarks: totalmaxMarks
                            });
                        });

                        angular.forEach($scope.exm_sub_mrks_list, function (ii) {
                            angular.forEach($scope.subexwithtotal_mrks, function (cc) {
                                if (cc.ismS_Id == ii.subid && cc.emE_Id == ii.emeid && cc.emsE_SubExamName == 'Total' && ii.ssubj == ''
                                    && ii.SubsubjectName == '') {
                                    ii.maxmarks = cc.maxMarks;
                                }
                            });
                        });

                        $scope.total_subwise = [];
                        angular.forEach($scope.subject_list, function (subj) {
                            var total_ssubj_marks = [];
                            angular.forEach(subj.subsubjects, function (ssubj) {
                                var Ssubj_marks = 0;
                                var Ssubj_mxmmarks = 0;
                                var subper1 = 0;
                                var Ssubj_gradee = "";

                                angular.forEach($scope.exm_sub_mrks_list, function (marks) {
                                    if (marks.subid == subj.ISMS_Id && marks.ssubj == ssubj.EMSS_Id) {
                                        if (marks.flag == true) {
                                            Ssubj_marks += Number(marks.obtainmarks);
                                            if (marks.PassFailFlg == 'M') {
                                                Ssubj_mxmmarks += 0;
                                            }
                                            else {
                                                Ssubj_mxmmarks += Number(marks.maxmarks);
                                            }
                                            Ssubj_gradee = marks.ObtainedGrade;
                                        }
                                    }
                                });
                                total_ssubj_marks.push({
                                    EMSS_Id: ssubj.EMSS_Id, SubSbuject: ssubj.SubSbuject, total_ss: Ssubj_marks,
                                    subsbmaxmrk: Ssubj_mxmmarks, subsubgrade: Ssubj_gradee
                                });
                            });

                            var Subj_marks = 0;
                            var Subj_mxmmarks = 0;
                            var subper1 = 0;
                            angular.forEach($scope.exm_sub_mrks_list, function (marks) {
                                if (marks.subid == subj.ISMS_Id && marks.ssubj == '' && marks.SubsubjectName == '') {
                                    if (marks.flag == true) {
                                        Subj_marks += Number(marks.obtainmarks);
                                        Subj_mxmmarks += Number(marks.Medicalmaxmarks);
                                    }
                                }
                            });

                            subper1 = (Subj_marks / Subj_mxmmarks) * 100;
                            subper1 = subper1.toFixed(2);

                            $scope.total_subwise.push({ ISMS_Id: subj.ISMS_Id, subjectname: subj.subjectname, total_s: Subj_marks, submaxmarks: Subj_mxmmarks, subjper: subper1, Ssubj_total: total_ssubj_marks });
                        });

                        var pergrandtotal = 0;
                        var totalpertotal = 0;
                        angular.forEach($scope.total_subwise, function (xx) {
                            if (xx.submaxmarks != 0) {
                                if (xx.subjper !== "NA") {
                                    pergrandtotal += Number(xx.subjper);
                                    totalpertotal += 100;
                                }
                            }

                            if (xx.Ssubj_total.length > 0) {
                                angular.forEach(xx.Ssubj_total, function (tt) {
                                    if (tt.subsbmaxmrk != 0) {
                                        pergrandtotal += ((tt.total_ss / tt.subsbmaxmrk) * 100);
                                        totalpertotal += 100;
                                    }
                                });
                            }
                        });

                        $scope.grandavgtotal = pergrandtotal;
                        $scope.percounttotal = totalpertotal;

                        $scope.grandfinaltotal = 0;
                        $scope.grandfinalmaxtotal = 0;
                        $scope.grandtotalperc = 0;
                        angular.forEach($scope.total_subwise, function (qq) {
                            $scope.grandfinaltotal += qq.total_s;
                            $scope.grandfinalmaxtotal += qq.submaxmarks;
                        });

                        $scope.grandtotalperc = 0;
                        if ($scope.grandfinalmaxtotal != 0) {
                            $scope.grandtotalperc = ($scope.grandfinaltotal / $scope.grandfinalmaxtotal) * 100;
                        }


                        if (promise.examflag === 1) {

                            //---------------Personlity
                            $scope.per_status = [];
                            $scope.month_status = promise.personalitymonth;
                            $scope.remarkslist = promise.remarkslist;
                            $scope.remarks_status = [];

                            if ($scope.personality_status.length != 0) {
                                angular.forEach($scope.personality_status, function (eps) {
                                    $scope.per_status.push({ eP_Id: eps.eP_Id, eP_PersonlaityName: eps.eP_PersonlaityName, epcR_Id: eps.epcR_Id });
                                });
                            }

                            $scope.temppersonality = [];

                            angular.forEach($scope.month_status, function (dd) {
                                angular.forEach($scope.remarkslist, function (ddd) {
                                    $scope.temppersonality.push({ emE_Id: dd.emE_Id, epcR_Id: ddd.epcR_Id, epcR_RemarksName: ddd.epcR_RemarksName });
                                });
                            });

                            angular.forEach($scope.personality_status, function (eps) {
                                if ($scope.per_status.length == 0) {
                                    $scope.per_status.push({ eP_Id: eps.eP_Id, eP_PersonlaityName: eps.eP_PersonlaityName, epcR_Id: eps.epcR_Id });
                                }
                                else if ($scope.per_status.length > 0) {
                                    var al_exm_cnt = 0;
                                    angular.forEach($scope.per_status, function (exm) {
                                        if (exm.eP_Id == eps.eP_Id) {
                                            al_exm_cnt += 1;
                                        }
                                    });
                                    if (al_exm_cnt == 0) {
                                        $scope.per_status.push({ eP_Id: eps.eP_Id, eP_PersonlaityName: eps.eP_PersonlaityName, epcR_Id: eps.epcR_Id });
                                    }
                                }
                            });

                            console.log($scope.personality_status);
                            console.log($scope.temppersonality);

                            $scope.co_activity = [];
                            $scope.month_activity = promise.cocurillarymonth;
                            $scope.remarks_activity = [];

                            angular.forEach($scope.co_curricular_activity, function (cca) {
                                if ($scope.co_activity.length == 0) {
                                    $scope.co_activity.push({ ecC_Id: cca.ecC_Id, ecC_CoCurricularName: cca.ecC_CoCurricularName });
                                }
                                else if ($scope.per_status.length > 0) {
                                    var al_exm_cnt = 0;
                                    angular.forEach($scope.co_activity, function (exm) {
                                        if (exm.ecC_Id == cca.ecC_Id) {
                                            al_exm_cnt += 1;
                                        }
                                    });
                                    if (al_exm_cnt == 0) {
                                        $scope.co_activity.push({ ecC_Id: cca.ecC_Id, ecC_CoCurricularName: cca.ecC_CoCurricularName });
                                    }
                                }
                            });

                            //attenadance
                            $scope.halfyearatt = promise.halfyearatt;
                            $scope.fullyearatt = promise.fullyearatt;

                            if ($scope.halfyearatt.length > 0) {
                                $scope.halfyearattworking = $scope.halfyearatt[0].totalworkingday;
                                $scope.halfyearattpresent = $scope.halfyearatt[0].totalpresentday;
                            }

                            if ($scope.fullyearatt.length > 0) {
                                $scope.fullyearattworking = $scope.fullyearatt[0].totalworkingday;
                                $scope.fullyearattpresent = $scope.fullyearatt[0].totalpresentday;
                            }
                        }
                        else {
                            $scope.Present_attendence = promise.present_attendence;
                        }

                        $scope.grade_detailslist = promise.grade_detailslist;
                        $scope.promotionrank = promise.promotionrank;
                        if ($scope.promotionrank !== undefined && $scope.promotionrank !== null && $scope.promotionrank.length > 0) {
                            $scope.finalsectionrank = $scope.promotionrank[0].estmpP_SectionRank;
                        }


                        $scope.promotionstatus = promise.promotionstatus;

                        if ($scope.promotionstatus !== undefined && $scope.promotionstatus !== null && $scope.promotionstatus.length > 0) {
                            $scope.EPRD_PromotionName = $scope.promotionstatus[0].eprD_PromotionName;
                            $scope.EPRD_ClassPromoted = $scope.promotionstatus[0].eprD_ClassPromoted;
                            $scope.EPRD_Remarks = $scope.promotionstatus[0].eprD_Remarks;
                        }

                        var e1 = angular.element(document.getElementById("report"));
                        $compile(e1.html(promise.htmlstring))(($scope));
                    }
                    else {
                        $scope.HHS_I_IV_grid = false;
                        swal("No Record found....");
                    }
                });
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.subjectlt1, function (itm) {
                itm.xyz = toggleStatus;
            });
        };

        $scope.optionToggled = function (chk_box) {
            $scope.all = $scope.subjectlt1.every(function (itm) { return itm.xyz; });
        };

        //to print
        $scope.print_HHS02 = function () {
            var innerContents = document.getElementById("HHS02").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/ProgressReport/HHS02/HHSIIVReportCardPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
    }
})();
