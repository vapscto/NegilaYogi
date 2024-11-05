
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgCumulativeReportController', ClgCumulativeReportController)

    ClgCumulativeReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function ClgCumulativeReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {


        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("ClgCumulativeReport/Getdetails").then(function (promise) {
                //$scope.course_list = promise.courseslist;
                //$scope.subjectschema_list = promise.subjectshemalist;
                //$scope.semisters_list = promise.semisters;
                //$scope.branch_list = promise.branchlist;
                //$scope.schmetype_list = promise.schmetypelist;
                //$scope.seclist = promise.sections;
                $scope.yearlist = promise.yearlist;
                $scope.datagriv = false;
            });
        };


        $scope.onchangeyear = function () {
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.EME_Id = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";
            $scope.datagriv = false;
            $scope.studentslt1 = [];
            $scope.studentslt = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("ClgCumulativeReport/onchangeyear", data).then(function (promise) {
                $scope.course_list = promise.courseslist;
            });
        };

        $scope.onchangecourse = function () {
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.EME_Id = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";
            $scope.datagriv = false;
            $scope.studentslt1 = [];
            $scope.studentslt = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            };
            apiService.create("ClgCumulativeReport/onchangecourse", data).then(function (promise) {
                $scope.branch_list = promise.branchlist;
            });
        };

        $scope.onchangebranch = function () {

            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.EME_Id = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";
            $scope.datagriv = false;
            $scope.studentslt1 = [];
            $scope.studentslt = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };
            apiService.create("ClgCumulativeReport/onchangebranch", data).then(function (promise) {
                $scope.semisters_list = promise.semisters;
            });
        };

        $scope.onchangesemester = function () {
            $scope.ACMS_Id = "";
            $scope.EME_Id = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";
            $scope.datagriv = false;
            $scope.studentslt1 = [];
            $scope.studentslt = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id
            };
            apiService.create("ClgCumulativeReport/onchangesemester", data).then(function (promise) {
                $scope.seclist = promise.sections;
                $scope.subjectschema_list = promise.subjectshemalist;
            });
        };

        $scope.onchangesubjectscheme = function () {
            $scope.EME_Id = "";
            $scope.ACST_Id = "";
            $scope.datagriv = false;
            $scope.studentslt1 = [];
            $scope.studentslt = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACSS_Id": $scope.ACSS_Id
            };
            apiService.create("ClgCumulativeReport/onchangesubjectscheme", data).then(function (promise) {
                $scope.schmetype_list = promise.schmetypelist;
            });
        };


        $scope.onchangeschemetype = function () {
            $scope.EME_Id = "";
            $scope.datagriv = false;
            $scope.studentslt1 = [];
            $scope.studentslt = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACSS_Id": $scope.ACSS_Id,
                "ACST_Id": $scope.ACST_Id
            };
            apiService.create("ClgCumulativeReport/onchangeschemetype", data).then(function (promise) {
                $scope.exam_list = promise.exmstdlist;
            });
        };



        // TO Save The Data
        $scope.submitted = false;
        $scope.Getreport = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "EME_ID": $scope.EME_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "ACSS_Id": $scope.ACSS_Id,
                    "ACST_Id": $scope.ACST_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ASMAY_Id": $scope.ASMAY_Id


                };

                apiService.create("ClgCumulativeReport/Getcmreport", data).
                    then(function (promise) {
                        $scope.report = true;
                        if (promise.savelist !== null) {
                            $scope.datagriv = true;
                            $scope.masterinst = promise.configuration;
                            var count = 0;
                            if ($scope.masterinst.length > 0) {
                                if ($scope.masterinst[0].exmConfig_RegnoColumnDisplay === true) {
                                    $scope.regno = true;
                                    count = count + 1;

                                } else {
                                    $scope.regno = false;
                                }

                                if ($scope.masterinst[0].exmConfig_AdmnoColumnDisplay === true) {
                                    $scope.admno = true;
                                    count = count + 1;
                                } else {
                                    $scope.admno = false;
                                }

                                if ($scope.masterinst[0].exmConfig_RollnoColumnDisplay === true) {
                                    $scope.rollno = true;
                                    count = count + 1;
                                } else {
                                    $scope.rollno = false;
                                }

                                if (count === 0) {
                                    $scope.admno = true;
                                    $scope.rollno = true;
                                }


                            } else {
                                $scope.admno = true;
                                $scope.rollno = true;
                            }



                            if (promise.savelist.length > 0) {
                                $scope.report = true;

                                $scope.inst_name = promise.savelist[0].mI_name;

                                angular.forEach($scope.course_list, function (itm) {
                                    if (itm.amcO_Id === parseInt($scope.AMCO_Id)) {
                                        $scope.cour = itm.amcO_CourseName;
                                    }
                                });
                                angular.forEach($scope.branch_list, function (itm) {
                                    if (itm.amB_Id === parseInt($scope.AMB_Id)) {
                                        $scope.branc = itm.amB_BranchName;
                                    }
                                });
                                angular.forEach($scope.exam_list, function (itm) {
                                    if (itm.emE_Id === parseInt($scope.EME_Id)) {
                                        $scope.exmmid = itm.emE_ExamName;
                                    }
                                });

                                angular.forEach($scope.yearlist, function (itm) {
                                    if (itm.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                        $scope.yearname = itm.asmaY_Year;
                                    }
                                });

                                angular.forEach($scope.seclist, function (itm) {
                                    if (itm.acmS_Id === parseInt($scope.ACMS_Id)) {
                                        $scope.sectionname = itm.acmS_SectionName;
                                    }
                                });

                                angular.forEach($scope.semisters_list, function (itm) {
                                    if (itm.amsE_Id === parseInt($scope.AMSE_Id)) {
                                        $scope.semestername = itm.amsE_SEMName;
                                    }
                                });


                                $scope.studentslt = promise.savelist;
                                $scope.studentslt1 = promise.subjlist;



                                var temp_list = [];
                                for (var x = 0; x < promise.savelist.length; x++) {
                                    var stu_id = promise.savelist[x].amsT_Id;
                                    var stu_subj_list = [];
                                    angular.forEach(promise.savelist, function (opq) {
                                        if (opq.amsT_Id == stu_id) {
                                            stu_subj_list.push({
                                                amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id,
                                                ismS_SubjectName: opq.ismS_SubjectName,
                                                ecstmpssS_MaxMarks: opq.ecstmpssS_MaxMarks,
                                                ecstmpS_ObtainedMarks: opq.ecstmpS_ObtainedMarks,
                                                ecstmpS_ObtainedGrade: opq.ecstmpS_ObtainedGrade,
                                                ecstmpS_PassFailFlg: opq.ecstmpS_PassFailFlg
                                            });
                                        }
                                    });

                                    if (temp_list.length == 0) {
                                        temp_list.push({
                                            student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName,
                                            amsT_AdmNo: promise.savelist[x].amsT_AdmNo,
                                            amaY_RollNo: promise.savelist[x].amaY_RollNo,
                                            amsT_RegistrationNo: promise.savelist[x].amsT_RegistrationNo,
                                            ecstmP_TotalObtMarks: promise.savelist[x].ecstmP_TotalObtMarks,
                                            ecstmP_SectionRank: promise.savelist[x].ecstmP_SectionRank,
                                            classheld: promise.savelist[x].classheld,
                                            classatt: promise.savelist[x].classatt,
                                            sub_list: stu_subj_list
                                        });
                                    }
                                    else if (temp_list.length > 0) {
                                        var already_cnt = 0;
                                        angular.forEach(temp_list, function (opq1) {
                                            if (opq1.student_id == stu_id) {
                                                already_cnt += 1;
                                            }
                                        })
                                        if (already_cnt === 0) {
                                            temp_list.push({
                                                student_id: promise.savelist[x].amsT_Id,
                                                amsT_FirstName: promise.savelist[x].amsT_FirstName,
                                                amsT_LastName: promise.savelist[x].amsT_LastName,
                                                amsT_AdmNo: promise.savelist[x].amsT_AdmNo,
                                                amaY_RollNo: promise.savelist[x].amaY_RollNo,
                                                amsT_RegistrationNo: promise.savelist[x].amsT_RegistrationNo,
                                                ecstmP_TotalObtMarks: promise.savelist[x].ecstmP_TotalObtMarks,
                                                ecstmP_SectionRank: promise.savelist[x].ecstmP_SectionRank,
                                                classheld: promise.savelist[x].classheld,
                                                classatt: promise.savelist[x].classatt,
                                                sub_list: stu_subj_list
                                            });
                                        }
                                    }

                                }
                                $scope.studentslt = temp_list;

                                console.log($scope.studentslt);

                                angular.forEach($scope.studentslt, function (oobj) {

                                    $scope.tmparrry = [];
                                    angular.forEach($scope.studentslt1, function (oobj1) {
                                        var ccount = 0;
                                        angular.forEach(oobj.sub_list, function (oobj2) {
                                            if (oobj1.ismS_Id == oobj2.ismS_Id) {
                                                ccount += 1;
                                                if (oobj2.ecstmpS_PassFailFlg != 'AB') {
                                                    oobj2.hema_ecstmpS_ObtainedMarks = oobj2.ecstmpS_ObtainedMarks;
                                                    oobj2.hema_ecstmpS_ObtainedGrade = oobj2.ecstmpS_ObtainedGrade;
                                                }
                                                else if (oobj2.ecstmpS_PassFailFlg == 'AB') {
                                                    oobj2.hema_ecstmpS_ObtainedMarks = oobj2.ecstmpS_PassFailFlg;
                                                }
                                                $scope.tmparrry.push(oobj2);
                                            }
                                        })
                                        if (ccount === 0) {
                                            var obj = {};
                                            obj.hema_ecstmpS_ObtainedMarks = "";
                                            obj.hema_ecstmpS_ObtainedGrade = "";
                                            $scope.tmparrry.push(obj);
                                        }
                                    })
                                    oobj.sub_list = $scope.tmparrry;
                                })

                                //if (promise.savenonsubjlist != null) {
                                //    if (promise.savenonsubjlist.length > 0) {
                                //        angular.forEach($scope.course_list, function (itm) {
                                //            if (itm.amcO_Id == $scope.amcO_Id) {
                                //                $scope.cour = itm.amcO_CourseName;
                                //            }
                                //        })
                                //        angular.forEach($scope.yearlt, function (itm) {
                                //            if (itm.asmaY_Id == $scope.asmaY_Id) {
                                //                $scope.yr = itm.asmaY_Year;
                                //            }
                                //        })
                                //        angular.forEach($scope.seclist, function (itm) {
                                //            if (itm.asmS_Id == $scope.asmS_Id) {
                                //                $scope.sec = itm.asmC_SectionName;
                                //            }
                                //        })
                                //        angular.forEach($scope.exsplt, function (itm) {
                                //            if (itm.emE_Id == $scope.emE_Id) {
                                //                $scope.exmmid = itm.emE_ExamName;
                                //            }
                                //        })
                                //        $scope.electivestd = promise.savenonsubjlist;
                                //        $scope.electivesub = promise.nonsubjlist;
                                //        var temp_list = [];
                                //        for (var x = 0; x < promise.savenonsubjlist.length; x++) {
                                //            var stu_id = promise.savenonsubjlist[x].amsT_Id;
                                //            var stu_subj_list = [];
                                //            angular.forEach(promise.savenonsubjlist, function (opq) {
                                //                if (opq.amsT_Id == stu_id) {
                                //                    stu_subj_list.push({
                                //                        amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id,
                                //                        ismS_SubjectName: opq.amsT_Id.ismS_SubjectName,
                                //                        ecstmpS_ObtainedMarks: opq.ecstmpS_ObtainedMarks,
                                //                        ecstmpS_ObtainedMarks: opq.ecstmpS_ObtainedMarks,
                                //                        ecstmpS_ObtainedGrade: opq.ecstmpS_ObtainedGrade,
                                //                        ecstmpS_PassFailFlg: opq.ecstmpS_PassFailFlg,
                                //                        ecyseS_SubjectOrder: opq.ecyseS_SubjectOrder
                                //                    });
                                //                }
                                //            })
                                //            if (temp_list.length == 0) {
                                //                temp_list.push({
                                //                    student_id: promise.savenonsubjlist[x].amsT_Id,
                                //                    amsT_FirstName: promise.savenonsubjlist[x].amsT_FirstName,
                                //                    amsT_LastName: promise.savenonsubjlist[x].amsT_LastName,
                                //                    amsT_AdmNo: promise.savenonsubjlist[x].amsT_AdmNo,
                                //                    amaY_RollNo: promise.savenonsubjlist[x].amaY_RollNo,
                                //                    amsT_RegistrationNo: promise.savenonsubjlist[x].amsT_RegistrationNo,
                                //                    ecstmP_TotalObtMarks: promise.savenonsubjlist[x].ecstmP_TotalObtMarks,
                                //                    ecstmP_SectionRank: promise.savelist[x].ecstmP_SectionRank,
                                //                    classheld: promise.savelist[x].classheld,
                                //                    classatt: promise.savelist[x].classatt,
                                //                    sub_list: stu_subj_list
                                //                });
                                //            }
                                //            else if (temp_list.length > 0) {
                                //                var already_cnt = 0;
                                //                angular.forEach(temp_list, function (opq1) {
                                //                    if (opq1.student_id == stu_id) {
                                //                        already_cnt += 1;
                                //                    }
                                //                })
                                //                if (already_cnt == 0) {
                                //                    temp_list.push({
                                //                        student_id: promise.savenonsubjlist[x].amsT_Id,
                                //                        amsT_FirstName: promise.savenonsubjlist[x].amsT_FirstName,
                                //                        amsT_LastName: promise.savenonsubjlist[x].amsT_LastName,
                                //                        amsT_AdmNo: promise.savenonsubjlist[x].amsT_AdmNo,
                                //                        amaY_RollNo: promise.savenonsubjlist[x].amaY_RollNo,
                                //                        amsT_RegistrationNo: promise.savenonsubjlist[x].amsT_RegistrationNo,
                                //                        ecstmP_TotalObtMarks: promise.savenonsubjlist[x].ecstmP_TotalObtMarks,
                                //                        ecstmP_SectionRank: promise.savelist[x].ecstmP_SectionRank,
                                //                        classheld: promise.savelist[x].classheld,
                                //                        classatt: promise.savelist[x].classatt,
                                //                        sub_list: stu_subj_list
                                //                    });
                                //                }
                                //            }
                                //        }
                                //        $scope.nonstudentslt = temp_list;
                                //    }
                                //}

                            }

                        } else {
                            swal("No Record Found");
                        }

                        //else if (promise.savenonsubjlist != null) {
                        //    $scope.report = true;
                        //    if (promise.savenonsubjlist.length > 0) {

                        //        angular.forEach($scope.course_list, function (itm) {
                        //            if (itm.amcO_Id == $scope.amcO_Id) {
                        //                $scope.cour = itm.amcO_CourseName;
                        //            }
                        //        })
                        //        angular.forEach($scope.yearlt, function (itm) {
                        //            if (itm.asmaY_Id == $scope.asmaY_Id) {
                        //                $scope.yr = itm.asmaY_Year;
                        //            }
                        //        })
                        //        angular.forEach($scope.seclist, function (itm) {
                        //            if (itm.asmS_Id == $scope.asmS_Id) {
                        //                $scope.sec = itm.asmC_SectionName;
                        //            }
                        //        })
                        //        angular.forEach($scope.exsplt, function (itm) {
                        //            if (itm.emE_Id == $scope.emE_Id) {
                        //                $scope.exmmid = itm.emE_ExamName;
                        //            }
                        //        })


                        //        $scope.electivestd = promise.savenonsubjlist;
                        //        $scope.electivesub = promise.nonsubjlist;

                        //        var temp_list = [];
                        //        for (var x = 0; x < promise.savenonsubjlist.length; x++) {

                        //            $scope.mI_name = promise.savenonsubjlist[0].mI_name;

                        //            var stu_id = promise.savenonsubjlist[x].amsT_Id;
                        //            var stu_subj_list = [];
                        //            angular.forEach(promise.savenonsubjlist, function (opq) {
                        //                if (opq.amsT_Id == stu_id) {
                        //                    stu_subj_list.push({ amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id, ismS_SubjectName: opq.amsT_Id.ismS_SubjectName, ecstmpssS_MaxMarks: opq.ecstmpssS_MaxMarks, ecstmpS_ObtainedMarks: opq.ecstmpS_ObtainedMarks, ecstmpS_ObtainedGrade: opq.ecstmpS_ObtainedGrade, ecstmpS_PassFailFlg: opq.ecstmpS_PassFailFlg });
                        //                }
                        //            })
                        //            if (temp_list.length == 0) {
                        //                temp_list.push({
                        //                    student_id: promise.savenonsubjlist[x].amsT_Id, amsT_FirstName: promise.savenonsubjlist[x].amsT_FirstName, amsT_LastName: promise.savenonsubjlist[x].amsT_LastName, ecstmP_TotalObtMarks: promise.savenonsubjlist[x].ecstmP_TotalObtMarks, classheld: promise.savenonsubjlist[x].classheld, classatt: promise.savenonsubjlist[x].classatt, sub_list: stu_subj_list
                        //                });
                        //            }
                        //            else if (temp_list.length > 0) {
                        //                var already_cnt = 0;
                        //                angular.forEach(temp_list, function (opq1) {
                        //                    if (opq1.student_id == stu_id) {
                        //                        already_cnt += 1;
                        //                    }
                        //                })
                        //                if (already_cnt == 0) {
                        //                    temp_list.push({ student_id: promise.savenonsubjlist[x].amsT_Id, amsT_FirstName: promise.savenonsubjlist[x].amsT_FirstName, amsT_LastName: promise.savenonsubjlist[x].amsT_LastName, ecstmP_TotalObtMarks: promise.savenonsubjlist[x].ecstmP_TotalObtMarks, classheld: promise.savenonsubjlist[x].classheld, classatt: promise.savenonsubjlist[x].classatt, sub_list: stu_subj_list });
                        //                }
                        //            }

                        //        }
                        //        $scope.nonstudentslt = temp_list;
                        //        $scope.studentslt = [];
                        //        angular.forEach(temp_list, function (xyz) {
                        //            $scope.studentslt.push({ student_id: xyz.student_id, amsT_FirstName: xyz.amsT_FirstName, amsT_LastName: xyz.amsT_LastName, classheld: xyz.classheld, classatt: xyz.classatt });
                        //        })


                        //    }
                        //    else if (promise.savenonsubjlist == 0 || null && promise.savelist == 0 || null) {
                        //        swal('No record Found');
                        //    }
                        //}


                        //console.log($scope.electivesub);
                        //angular.forEach($scope.studentslt, function (oobj) {
                        //    angular.forEach($scope.nonstudentslt, function (oobj2) {
                        //        if (oobj2.student_id == oobj.student_id) {
                        //            $scope.tmparrry1 = [];
                        //            angular.forEach($scope.electivesub, function (oobj1) {
                        //                var ccount1 = 0;
                        //                angular.forEach(oobj2.sub_list, function (oobj3) {
                        //                    if (oobj1.ismS_Id == oobj3.ismS_Id) {
                        //                        ccount1 += 1;
                        //                        if (oobj3.ecstmpS_PassFailFlg != 'AB') {
                        //                            oobj3.hema_ecstmpS_ObtainedMarks = oobj3.ecstmpS_ObtainedMarks;
                        //                            oobj3.hema_ecstmpS_ObtainedGrade = oobj3.ecstmpS_ObtainedGrade;
                        //                        }
                        //                        else if (oobj3.ecstmpS_PassFailFlg == 'AB') {
                        //                            oobj3.hema_ecstmpS_ObtainedMarks = oobj3.ecstmpS_PassFailFlg;
                        //                        }
                        //                        $scope.tmparrry1.push(oobj3);
                        //                    }

                        //                })
                        //                if (ccount1 == 0) {
                        //                    var obj1 = {};
                        //                    obj1.hema_ecstmpS_ObtainedMarks = "";
                        //                    obj1.hema_ecstmpS_ObtainedGrade = "";
                        //                    $scope.tmparrry1.push(obj1);
                        //                }
                        //            })
                        //            oobj.sub_list_e = $scope.tmparrry1;
                        //        }
                        //    })

                        //})

                    })
            }
        };






        $scope.exportToExcel = function (tableIds) {
            var exportHref = Excel.tableToExcel(tableIds, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }


        // create the list of themes  
        $scope.fonts = [
            { name: '10px', size: '10px ', class: 'font10' },
            { name: '11px', size: '11px ', class: 'font11' },
            { name: '12px', size: '12px ', class: 'font12' },
            { name: '13px', size: '13px ', class: 'font13' },
            { name: '14px', size: '14px ', class: 'font14' },
            { name: '15px', size: '15px', class: 'font15' },
            { name: '16px', size: '16px', class: 'font16' },
            { name: '17px', size: '17px', class: 'font17' }, { name: '18px', size: '18px', class: 'font18' }, { name: '25px', size: '25px', class: 'font25' }
        ];
        $scope.font_size = 'font25';








        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;


        //Record pop up 
        $scope.viewrecordspopup = function (employee, SweetAlert) {

            $scope.editEmployee = employee.amsT_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("CumulativeReport/getalldetailsviewrecords", pageid).
                then(function (promise) {
                    $scope.viewrecordspopupdisplay = promise.gtdetailsview
                })

        };
        $scope.clearpopupgrid = function () {
            $scope.viewrecordspopupdisplay = "";
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        //to active or deactive 
        $scope.deactive = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.estsU_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        var config = {
                            headers: {
                                'Content-Type': 'application/json;'
                            }
                        }

                        apiService.create("CumulativeReport/deactivate", deactiveRecord).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal(confirmmgs + ' Successfully');
                                }
                                else {
                                    swal('Record Not  Activated/Deactivated');
                                }
                                $scope.BindData();
                                $scope.clearid1();
                            })
                    }
                    else {
                        swal("Record" + mgs + "Cancelled");
                    }
                });
        };




        // to Edit Data
        $scope.getorgvalue = function (EditRecord) {
            var MEditId = EditRecord.amsT_Id;
            apiService.getURI("CumulativeReport/editdetails", MEditId).
                then(function (promise) {
                    if (promise.editlist.length > 0) {
                        $scope.te_sub_ls = promise.editlist;
                        $scope.AMST_Id = promise.editlist[0].amsT_Id;
                        $scope.asmaY_Id = promise.editlist[0].asmaY_Id;
                        $scope.asmcL_Id = promise.editlist[0].asmcL_Id;
                        $scope.asmS_Id = promise.editlist[0].asmS_Id;
                        $scope.emcA_Id = promise.edclasslist[0].emcA_Id;
                        $scope.emG_Id = promise.editlist[0].emG_Id;
                        $scope.Onsubjectchange($scope.emG_Id);
                        $scope.valid_Section($scope.asmcL_Id, $scope.emcA_Id, $scope.asmaY_Id, $scope.asmS_Id);
                        //  $scope.Onsubjectchange($scope.emG_Id);

                        $scope.studentlist = true;
                    } else {
                        swal('No Record Found')
                    }
                })
        };

        $scope.sortKey = 'amsT_FirstName';

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //$scope.subchkbx = function (record1,record,mainary) {
        $scope.subchkbx = function (column, obj, user) {
            //  $scope.gridview2 = true;

            if (obj.abc == true) {
                angular.forEach($scope.subjectlt1, function (itm1) {
                    if (itm1.amsT_Id == user.amsT_Id) {
                        angular.forEach($scope.subjectlt, function (itm2) {
                            if (itm2.ismS_Id == column.ismS_Id) {
                                itm1.sub_list.push({ id: itm2.ismS_Id, name: itm2.ismS_SubjectName });
                            }
                        })
                    }
                })
            }
            else {
                angular.forEach($scope.subjectlt1, function (itm1) {
                    if (itm1.amsT_Id == user.amsT_Id) {
                        for (var i = 0; i < itm1.sub_list.length; i++) {


                            if (itm1.sub_list[i].id == column.ismS_Id) {

                                itm1.sub_list.splice(i, 1);
                            }
                        }
                    }
                })

            }
        };


        $scope.printToCart = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportBBPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.cancel = function () {
            $scope.asmcL_Id = ""
            $scope.emcA_Id = ""
            $scope.asmaY_Id = ""
            $scope.emG_Id = ""
            $scope.asmS_Id = ""
            $scope.subjectlt = ""
            $scope.subjectlt1 = ""
            $scope.studentlist = false;
            $state.reload();
        }
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.subjectlt1, function (itm) {
                itm.xyz = toggleStatus;
            });
        }

        $scope.optionToggled = function (chk_box) {
            $scope.all = $scope.subjectlt1.every(function (itm) { return itm.xyz; })
        }
    }

})();