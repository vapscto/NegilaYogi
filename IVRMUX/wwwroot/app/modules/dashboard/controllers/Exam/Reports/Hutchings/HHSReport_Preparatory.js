(function () {
    'use strict';
    angular.module('app').controller('HHSReport_PreparatoryController', HHSReport_PreparatoryController)
    HHSReport_PreparatoryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function HHSReport_PreparatoryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
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
        $scope.grandavgtotal = 0;
        $scope.exm_sub_mrks_list = [];
        $scope.submitted = false;
        $scope.halfyearatt = [];
        $scope.fullyearatt = [];

        $scope.BindData = function () {
            apiService.getDATA("HHSReport_5to7/Getdetails").then(function (promise) {
                $scope.yearlt = promise.yearlist;
                $scope.clslist = promise.classlist;
                $scope.seclist = promise.seclist;
                $scope.amstlt = promise.amstlist;
                $scope.studlist = promise.hhstudlist;
                $scope.grade_list = promise.grade_list;
            });
        };

        $scope.OnAcdyear = function (asmaY_Id) {
            $scope.asmcL_Id = '';
            $scope.asmS_Id = '';
            $scope.section = [];
            $scope.classarray = [];
            $scope.AMST_Id = '';
            $scope.fillstudents = [];
            $scope.emgR_Id = "";
            $scope.HHS_I_IV_grid = false;
            apiService.getURI("HHSReport_5to7/getclass", asmaY_Id).then(function (promise) {
                $scope.classarray = promise.fillclass;
            });
        };

        $scope.OnClass = function (asmcL_Id) {
            $scope.asmS_Id = '';
            $scope.asmcL_Id = asmcL_Id;
            $scope.section = [];
            $scope.AMST_Id = '';
            $scope.fillstudents = [];
            $scope.emgR_Id = "";
            $scope.HHS_I_IV_grid = false;
            var data = {
                "ASMCL_Id": asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id
            };
            apiService.create("HHSReport_5to7/Getsection", data).then(function (promise) {
                $scope.section = promise.fillsection;
            });
        };

        $scope.OnSection = function (asmS_Id) {
            $scope.AMST_Id = '';
            $scope.asmS_Id = asmS_Id;
            $scope.fillstudents = [];
            $scope.HHS_I_IV_grid = false;
            $scope.emgR_Id = "";
            var data = {
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMS_Id": asmS_Id,
            };

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("HHSReport_5to7/GetAttendence", data).then(function (promise) {
                $scope.fillstudents = promise.fillstudents;
            });
        };      

        $scope.onselectcategory = function () {
            $scope.HHS_I_IV_grid = false;
        };       
       
        $scope.saveddata = function () {
            $scope.submitted = true;
            $scope.HHS_I_IV_grid = false;
            if ($scope.myForm.$valid) {
                var data = {
                    "AMST_Id": $scope.AMST_Id.amsT_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "EMGR_Id": $scope.emgR_Id
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("HHSReport_5to7/Get_primary_savedetails", data).then(function (promise) {

                    $scope.exm_sub_mrks_list = promise.eam_sub_mrks_list;
                    $scope.personality_status = promise.personality_status;
                    $scope.co_curricular_activity = promise.co_curricular_activity;

                    if ($scope.exm_sub_mrks_list.length != 0 && $scope.exm_sub_mrks_list != null) {
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
                        angular.forEach($scope.exm_sub_mrks_list, function (st) {
                            if ($scope.exam_list.length == 0) {
                                $scope.exam_list.push({ emE_Id: st.emeid, emE_ExamName: st.examname });
                            }
                            else if ($scope.exam_list.length > 0) {
                                var al_exm_cnt = 0;
                                angular.forEach($scope.exam_list, function (exm) {
                                    if (exm.emE_Id == st.emeid) {
                                        al_exm_cnt += 1;
                                    }
                                })
                                if (al_exm_cnt == 0) {
                                    $scope.exam_list.push({ emE_Id: st.emeid, emE_ExamName: st.examname });
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
                                            })
                                            if (sssubj_cnt == 0) {
                                                ssubj_list.push({ EMSS_Id: main.ssubj, SubSbuject: main.SubsubjectName });
                                            }
                                        }
                                    }

                                })
                                $scope.subject_list.push({ ISMS_Id: st.subid, subjectname: st.subjectname, subsubjects: ssubj_list, flag: st.flag });
                            }

                            else if ($scope.subject_list.length > 0) {
                                var al_subj_cnt = 0;
                                angular.forEach($scope.subject_list, function (subj) {
                                    if (subj.ISMS_Id == st.subid) {
                                        al_subj_cnt += 1;
                                    }
                                })
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
                                                })
                                                if (sssubj_cnt == 0) {
                                                    ssubj_list.push({ EMSS_Id: main.ssubj, SubSbuject: main.SubsubjectName });
                                                }
                                            }
                                        }
                                    });
                                    $scope.subject_list.push({ ISMS_Id: st.subid, subjectname: st.subjectname, subsubjects: ssubj_list, flag: st.flag });
                                }
                            }
                        })

                        //for total
                        $scope.total_subwise = [];
                        angular.forEach($scope.subject_list, function (subj) {
                            var total_ssubj_marks = [];
                            angular.forEach(subj.subsubjects, function (ssubj) {
                                var Ssubj_marks = 0;
                                var Ssubj_mxmmarks = 0;
                                var Ssubj_grade = "";

                                angular.forEach($scope.exm_sub_mrks_list, function (marks) {
                                    if (marks.subid == subj.ISMS_Id && marks.ssubj == ssubj.EMSS_Id) {

                                        if (marks.flag == true) {
                                            Ssubj_marks += Number(marks.obtainmarks);
                                            Ssubj_mxmmarks += Number(marks.maxmarks);
                                            Ssubj_grade = marks.ObtainedGrade
                                        }
                                    }
                                })
                                total_ssubj_marks.push({
                                    EMSS_Id: ssubj.EMSS_Id, SubSbuject: ssubj.SubSbuject, total_ss: Ssubj_marks,
                                    subsbmaxmrk: Ssubj_mxmmarks, subsubgrade: Ssubj_grade
                                });
                            })
                            var Subj_marks = 0;
                            var Subj_mxmmarks = 0;
                            angular.forEach($scope.exm_sub_mrks_list, function (marks) {
                                if (marks.subid == subj.ISMS_Id && marks.ssubj == '' && marks.SubsubjectName == '') {
                                    if (marks.flag == true) {
                                        Subj_marks += Number(marks.obtainmarks);
                                        Subj_mxmmarks += Number(marks.maxmarks);

                                    }
                                }
                            });

                            $scope.total_subwise.push({ ISMS_Id: subj.ISMS_Id, subjectname: subj.subjectname, total_s: Subj_marks, submaxmarks: Subj_mxmmarks, Ssubj_total: total_ssubj_marks })
                        });

                        var pergrandtotal = 0
                        var totalpertotal = 0;
                        angular.forEach($scope.total_subwise, function (xx) {

                            if (xx.submaxmarks != 0) {

                                pergrandtotal += ((xx.total_s / xx.submaxmarks) * 100);
                                totalpertotal += 100;
                            }
                            if (xx.Ssubj_total.length > 0) {

                                angular.forEach(xx.Ssubj_total, function (tt) {

                                    if (tt.subsbmaxmrk != 0) {
                                        pergrandtotal += ((tt.total_ss / tt.subsbmaxmrk) * 100);
                                        totalpertotal += 100;
                                    }
                                })
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
                                })
                                $scope.mainexam_list.push({ emE_Id: me.emE_Id, emE_ExamName: me.emE_ExamName, count_se: sub_exm_cnt });
                            }
                            else if ($scope.mainexam_list.length > 0) {
                                var main_exm_cnt = 0;
                                angular.forEach($scope.mainexam_list, function (exm) {
                                    if (exm.emE_Id == me.emE_Id) {
                                        main_exm_cnt += 1;
                                    }
                                })
                                if (main_exm_cnt == 0) {
                                    var sub_exm_cnt = 0;
                                    angular.forEach($scope.exam_subexam_W, function (sd) {
                                        if (sd.emE_Id == me.emE_Id) {
                                            sub_exm_cnt += 1;
                                        }
                                    })
                                    $scope.mainexam_list.push({ emE_Id: me.emE_Id, emE_ExamName: me.emE_ExamName, count_se: sub_exm_cnt });
                                }
                            }
                        });

                        $scope.subexam_list = [];
                        angular.forEach($scope.exam_subexam_W, function (sme) {
                            if ($scope.subexam_list.length == 0) {
                                $scope.subexam_list.push({ emsE_Id: sme.emsE_Id, emsE_SubExamName: sme.emsE_SubExamName, emE_Id: sme.emE_Id });
                            }
                            else if ($scope.subexam_list.length > 0) {
                                var sub_exm_cnt = 0;
                                angular.forEach($scope.subexam_list, function (exm) {
                                    if (exm.emE_Id == sme.emE_Id && exm.emsE_Id == sme.emsE_Id) {
                                        sub_exm_cnt += 1;
                                    }
                                })
                                if (sub_exm_cnt == 0) {
                                    $scope.subexam_list.push({ emsE_Id: sme.emsE_Id, emsE_SubExamName: sme.emsE_SubExamName, emE_Id: sme.emE_Id });
                                }
                            }

                        })
                        $scope.subexam_list11 = $scope.subexam_list;
                        $scope.marksprocess_Pro_subwise = promise.marksprocess_Pro_subwise;
                        $scope.marksprocess_Pro_markscal = promise.marksprocess_Pro_markscal;
                        $scope.subexwithtotal = [];

                        angular.forEach($scope.mainexam_list, function (xx) {
                            var zz = 0;
                            angular.forEach($scope.subexam_list, function (ww) {
                                if (xx.emE_Id == ww.emE_Id) {
                                    $scope.subexwithtotal.push({ emE_Id: xx.emE_Id, emsE_Id: ww.emsE_Id, emsE_SubExamName: ww.emsE_SubExamName })
                                }
                            });
                        });

                        $scope.tempsub = [];
                        angular.forEach($scope.subject_list, function (cc) {
                            angular.forEach($scope.mainexam_list, function (vv) {
                                $scope.tempsub.push({ ismS_Id: cc.ISMS_Id, emE_Id: vv.emE_Id })
                            })
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
                                })
                                if (al_ct == 0) {
                                    $scope.selectedsection.push({ EME_ExamName: stu2.EME_ExamName, EME_Id: stu2.EME_Id, EMSE_Id: stu2.EMSE_Id, EMSE_SubExamName: stu2.EMSE_SubExamName, ESTMPSSS_ObtainedMarks: stu2.ESTMPSSS_ObtainedMarks, ESTMPS_PassFailFlg: stu2.ESTMPS_PassFailFlg, ISMS_Id: stu2.ISMS_Id, MaxMarks: stu2.MaxMarks });
                                }
                            }
                        });


                        $scope.marksprocess_Pro_markscal = $scope.selectedsection;
                        console.log($scope.marksprocess_Pro_markscal)
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
                                })
                                if (al_ct == 0) {
                                    $scope.selectedsub.push({ ISMS_Id: stu2.ISMS_Id });
                                }
                            }
                        });

                        $scope.tempsub1 = [];
                        angular.forEach($scope.tempsub, function (t1) {
                            angular.forEach($scope.selectedsub, function (s1) {
                                if (s1.ISMS_Id == t1.ismS_Id) {
                                    $scope.tempsub1.push({ emE_Id: t1.emE_Id, ismS_Id: s1.ISMS_Id })
                                }
                            });
                        });

                        $scope.subexwithtotal_mrks = [];
                        angular.forEach($scope.tempsub1, function (xx) {
                            var totalbotained = 0;
                            var totalmaxMarks = 0;
                            angular.forEach($scope.marksprocess_Pro_markscal, function (ww) {
                                if (xx.emE_Id == ww.EME_Id && xx.ismS_Id == ww.ISMS_Id) {
                                    totalbotained += ww.ESTMPSSS_ObtainedMarks
                                    if (ww.ESTMPS_PassFailFlg != 'M') {
                                        totalmaxMarks += ww.MaxMarks;
                                    }
                                    $scope.subexwithtotal_mrks.push({
                                        emE_Id: xx.emE_Id, emsE_Id: ww.EMSE_Id, obtainedMarks: ww.ESTMPSSS_ObtainedMarks, ismS_Id: ww.ISMS_Id, emsE_Id: ww.EMSE_Id, maxMarks: ww.MaxMarks
                                        , estmpS_PassFailFlg: ww.ESTMPS_PassFailFlg
                                    });
                                }
                            });
                            $scope.subexwithtotal_mrks.push({ emE_Id: xx.emE_Id, ismS_Id: xx.ismS_Id, emsE_SubExamName: 'Total', obtainedMarks: totalbotained, maxMarks: totalmaxMarks });
                        });

                        $scope.subexwithtotal_mrks1 = [];
                        angular.forEach($scope.tempsub1, function (xx) {
                            var totalbotained = 0;
                            var totalmaxMarks = 0;

                            angular.forEach($scope.marksprocess_Pro_markscal, function (ww) {
                                if (xx.emE_Id == ww.EME_Id && xx.ismS_Id == ww.ISMS_Id) {
                                    totalbotained += ww.ESTMPSSS_ObtainedMarks
                                    totalmaxMarks += ww.MaxMarks
                                    $scope.subexwithtotal_mrks1.push({
                                        emE_Id: xx.emE_Id, emsE_Id: ww.EMSE_Id, obtainedMarks: ww.ESTMPSSS_ObtainedMarks, ismS_Id: ww.ISMS_Id,
                                        emsE_Id: ww.EMSE_Id, maxMarks: ww.MaxMarks, estmpS_PassFailFlg: ww.ESTMPS_PassFailFlg
                                    });
                                }
                            });
                            $scope.subexwithtotal_mrks1.push({ emE_Id: xx.emE_Id, ismS_Id: xx.ismS_Id, emsE_SubExamName: 'Total', obtainedMarks: totalbotained, maxMarks: totalmaxMarks });
                        });

                        angular.forEach($scope.exm_sub_mrks_list, function (ii) {
                            angular.forEach($scope.subexwithtotal_mrks, function (cc) {
                                if (cc.ismS_Id == ii.subid && cc.emE_Id == ii.emeid && cc.emsE_SubExamName == 'Total'
                                    && ii.ssubj == '' && ii.SubsubjectName == '') {
                                    ii.maxmarks = cc.maxMarks;
                                }
                            });
                        })

                        $scope.total_subwise = [];
                        angular.forEach($scope.subject_list, function (subj) {
                            var total_ssubj_marks = [];
                            angular.forEach(subj.subsubjects, function (ssubj) {
                                var Ssubj_marks = 0;
                                var Ssubj_mxmmarks = 0;
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
                            })
                            var Subj_marks = 0;
                            var Subj_mxmmarks = 0;
                            angular.forEach($scope.exm_sub_mrks_list, function (marks) {
                                if (marks.subid == subj.ISMS_Id && marks.ssubj == '' && marks.SubsubjectName == '') {
                                    if (marks.flag == true) {
                                        Subj_marks += Number(marks.obtainmarks);
                                        if (marks.PassFailFlg == 'M' && marks.obtainmarks == 0) {
                                            Subj_mxmmarks += 0;
                                        }
                                        else {
                                            Subj_mxmmarks += Number(marks.maxmarks);
                                        }
                                    }
                                }
                            });
                            $scope.total_subwise.push({ ISMS_Id: subj.ISMS_Id, subjectname: subj.subjectname, total_s: Subj_marks, submaxmarks: Subj_mxmmarks, Ssubj_total: total_ssubj_marks });
                        });

                        var pergrandtotal = 0
                        var totalpertotal = 0;
                        angular.forEach($scope.total_subwise, function (xx) {
                            if (xx.submaxmarks != 0) {
                                pergrandtotal += ((xx.total_s / xx.submaxmarks) * 100);
                                totalpertotal += 100;
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

                        //attenadance
                        $scope.Present_attendence = promise.present_attendence;

                       
                        $scope.grade_detailslist = promise.grade_detailslist;

                        $scope.examwiseremarks = promise.examwiseremarks;

                        $scope.promotionstatus = promise.promotionstatus;
                        if ($scope.promotionstatus !== undefined && $scope.promotionstatus !== null && $scope.promotionstatus.length > 0) {
                            $scope.EPRD_PromotionName = $scope.promotionstatus[0].eprD_PromotionName;
                            $scope.EPRD_ClassPromoted = $scope.promotionstatus[0].eprD_ClassPromoted;
                            $scope.EPRD_Remarks = $scope.promotionstatus[0].eprD_Remarks;
                        }
                    }
                    else {
                        $scope.HHS_I_IV_grid = false;
                        swal("No Record found....")
                    }
                });
            };
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.print_HHS02 = function () {
            var innerContents = document.getElementById("BCOESAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/ProgressReport/HHS03/hhprogress03Pdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };  
    }
})();