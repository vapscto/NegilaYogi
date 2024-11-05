(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgbmcpcmidtermreportController', ClgbmcpcmidtermreportController)

    ClgbmcpcmidtermreportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function ClgbmcpcmidtermreportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.report = false;
        //TO  GEt The Values iN Grid

        $scope.arrtemp = [
            { id: 1, name: 'THEORY' },
            { id: 2, name: 'PRACTICALS' }
        ];

        $scope.BindData = function () {
            apiService.getURI("CollegeBMCPUProgresscardReport/Getdetails/2").
                then(function (promise) {

                    $scope.yearlt = promise.yearlist;
                    $scope.clslist = promise.classlist;
                    $scope.seclist = promise.seclist;
                    $scope.amstlt = promise.amstlist;
                    $scope.exsplt = promise.exmstdlist;
                });
        };


        $scope.OnAcdyear = function () {
            $scope.report = false;
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.EME_Id = "";
            $scope.ACMS_Id = "";
            $scope.ACST_Id = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("CollegeBMCPUProgresscardReport/OnAcdyear", data).then(function (promise) {

                if (promise !== null) {
                    $scope.coursedetails = promise.getcourse;
                }
            });
        };

        $scope.onchangecourse = function () {
            $scope.report = false;
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.EME_Id = "";
            $scope.ACMS_Id = "";
            $scope.ACST_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            };

            apiService.create("CollegeBMCPUProgresscardReport/onchangecourse", data).then(function (promise) {

                if (promise !== null) {
                    $scope.branchdetails = promise.getbranch;
                }
            });
        };

        $scope.onchangebranch = function () {
            $scope.report = false;
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.EME_Id = "";
            $scope.ACMS_Id = "";
            $scope.ACST_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };

            apiService.create("CollegeBMCPUProgresscardReport/onchangebranch", data).then(function (promise) {

                if (promise !== null) {
                    $scope.semesterdetails = promise.getsemester;
                }
            });
        };
        $scope.onchangesemester = function () {
            $scope.report = false;
            $scope.ACMS_Id = "";
            $scope.EME_Id = "";
            $scope.ACMS_Id = "";
            $scope.ACST_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id
            };

            apiService.create("CollegeBMCPUProgresscardReport/onchangesemester", data).then(function (promise) {

                if (promise !== null) {
                    $scope.sectiondetails = promise.getsection;
                }
            });
        };

        $scope.onchangesection = function () {
            $scope.EME_Id = "";
            $scope.ACMS_Id = "";
            $scope.ACST_Id = "";
            $scope.report = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACMS_Id": $scope.ACMS_Id
            };

            apiService.create("CollegeBMCPUProgresscardReport/onchangesection", data).then(function (promise) {
                if (promise !== null) {
                    $scope.subjectscheme = promise.getsubjectscheme;
                }
            });
        };

        $scope.onchangesubjectscheme = function () {
            $scope.EME_Id = "";          
            $scope.ACST_Id = "";
            $scope.report = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACMS_Id": $scope.ACMS_Id,
                "ACSS_Id": $scope.ACSS_Id
            };

            apiService.create("CollegeBMCPUProgresscardReport/onchangesubjectscheme", data).then(function (promise) {
                if (promise !== null) {
                    $scope.schemetype = promise.getschemetype;
                }
            });
        };
        $scope.onchangeschemetype = function () {
            $scope.EME_Id = "";
            $scope.report = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ACMS_Id": $scope.ACMS_Id,
                "ACSS_Id": $scope.ACSS_Id,
                "ACST_Id": $scope.ACST_Id
            };

            apiService.create("CollegeBMCPUProgresscardReport/onchangeschemetype", data).then(function (promise) {
                if (promise !== null) {
                    $scope.examdetails = promise.getexam;
                }
            });
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
            angular.forEach($scope.yearlt, function (yr) {
                if (yr.asmaY_Id === parseInt($scope.asmaY_Id)) {
                    $scope.year = yr.asmaY_Year;
                }
            });
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "EME_Id": $scope.emE_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "ACSS_Id": $scope.ACSS_Id,
                    "ACST_Id": $scope.ACST_Id
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("CollegeBMCPUProgresscardReport/getreport", data).
                    then(function (promise) {

                        if (promise.savelist.length > 0) {

                            $scope.report = true;
                            //inst name binding
                            $scope.instname = promise.instname;
                            $scope.inst_name = $scope.instname[0].mI_Name;

                            var temp_list = [];
                            for (var x = 0; x < promise.savelist.length; x++) {
                                var stu_id = promise.savelist[x].amcsT_Id;
                                var stu_subj_list = [];
                                angular.forEach(promise.savelist, function (opq) {
                                    if (parseInt(opq.amcsT_Id) === parseInt(stu_id)) {
                                        stu_subj_list.push({ amcsT_Id: opq.amcsT_Id, ismS_Id: opq.ismS_Id, ismS_SubjectName: opq.ismS_SubjectName, ecstmpssS_MaxMarks: opq.ecstmpssS_MaxMarks, ecstmpS_ObtainedMarks: opq.ecstmpS_ObtainedMarks, ecstmpS_ObtainedGrade: opq.ecstmpS_ObtainedGrade, ecstmpS_PassFailFlg: opq.ecstmpS_PassFailFlg, ECSTMPS_SemAverage: opq.ecstmpS_SemAverage, ECSTMPS_SectionAverage: opq.ecstmpS_SectionAverage, ECSTMPS_SemHighest: opq.ecstmpS_SemHighest, ECSTMPS_SectionHighest: opq.ecstmpS_SectionHighest, EMSE_SubExamName: opq.emsE_SubExamName, ECSTMPSSS_ObtainedMarks: opq.ecstmpssS_ObtainedMarks });
                                    }
                                });
                                if (temp_list.length === 0) {

                                    temp_list.push({
                                        student_id: promise.savelist[x].amcsT_Id, amcsT_FirstName: promise.savelist[x].amcsT_FirstName, amcsT_DOB: promise.savelist[x].amcsT_DOB, amcsT_RegistrationNo: promise.savelist[x].amcsT_RegistrationNo, acysT_RollNo: promise.savelist[x].acysT_RollNo, amcO_CourseName: promise.savelist[x].amcO_CourseName,
                                        amB_BranchName: promise.savelist[x].amB_BranchName,
                                        amsE_SEMName: promise.savelist[x].amsE_SEMName,
                                        acmS_SectionName: promise.savelist[x].acmS_SectionName,
                                        ecstmP_TotalMaxMarks: promise.savelist[x].ecstmP_TotalMaxMarks,
                                        ecstmP_TotalObtMarks: promise.savelist[x].ecstmP_TotalObtMarks,
                                        ecstmP_Percentage: promise.savelist[x].ecstmP_Percentage,
                                        ecstmP_TotalGrade: promise.savelist[x].ecstmP_TotalGrade, ismS_SubjectName: promise.savelist[x].ismS_SubjectName,
                                        sub_list: stu_subj_list
                                    });
                                }
                                else if (temp_list.length > 0) {
                                    var already_cnt = 0;
                                    angular.forEach(temp_list, function (opq1) {

                                        if (parseInt(opq1.student_id) === parseInt(stu_id)) {
                                            already_cnt += 1;

                                        }
                                    });
                                    if (already_cnt === 0) {
                                        temp_list.push({
                                            student_id: promise.savelist[x].amcsT_Id, amcsT_FirstName: promise.savelist[x].amcsT_FirstName,
                                            amcsT_DOB: promise.savelist[x].amcsT_DOB, amcsT_RegistrationNo: promise.savelist[x].amcsT_RegistrationNo,
                                            acysT_RollNo: promise.savelist[x].acysT_RollNo,
                                            amcO_CourseName: promise.savelist[x].amcO_CourseName,
                                            amB_BranchName: promise.savelist[x].amB_BranchName,
                                            amsE_SEMName: promise.savelist[x].amsE_SEMName, acmS_SectionName: promise.savelist[x].acmS_SectionName, ecstmP_TotalMaxMarks: promise.savelist[x].ecstmP_TotalMaxMarks, ecstmP_TotalObtMarks: promise.savelist[x].ecstmP_TotalObtMarks, ecstmP_Percentage: promise.savelist[x].ecstmP_Percentage, ecstmP_TotalGrade: promise.savelist[x].ecstmP_TotalGrade,
                                            ismS_SubjectName: promise.savelist[x].ismS_SubjectName, sub_list: stu_subj_list
                                        });
                                    }
                                }
                            }

                            $scope.exam = promise.savelist[0].emE_ExamName;
                            $scope.exm_sublist = promise.subjlist;
                            $scope.processtot = promise.savelisttot;
                            $scope.subj_grade_remarks = promise.grade_details;

                            //if (promise.clstchname.length > 0) {
                            //    $scope.clastechname = promise.clstchname[0].hrmE_EmployeeFirstName;
                            //} else {
                            //    $scope.clastechname = "";
                            //}
                            //  $scope.clastechname = promise.clstchname[0].hrmE_EmployeeFirstName;

                            $scope.report_list = temp_list;
                            //$scope.stud_work_attendence = promise.work_attendence;
                            //$scope.stud_present_attendence = promise.present_attendence;
                            console.log($scope.stud_work_attendence);
                            $scope.tempss = [];
                            var ss = 0;
                            var cn = 0;


                            $scope.temparry = [];
                            $scope.temparry.push({ part: 'PART-I LANGUAGES' });
                            $scope.temparry.push({ part: 'PART-II OPTIONALS' });
                            var a1 = [];
                            a1 = [];
                            $scope.a11 = [];
                            $scope.a22 = [];

                            console.log($scope.report_list);

                            angular.forEach($scope.report_list, function (dddd) {
                                angular.forEach(dddd.sub_list, function (ddddd) {
                                    angular.forEach($scope.exm_sublist, function (ff) {
                                        if (parseInt(ddddd.ismS_Id) === parseInt(ff.ismS_Id)) {
                                            cn += 1;
                                            if (ff.ismS_SubjectName !== '') {
                                                if (ff.ismS_SubjectName.toLowerCase() === 'kannada' || ff.ismS_SubjectName.toLowerCase() === 'english' || ff.ismS_SubjectName.toLowerCase() === 'french' || ff.ismS_SubjectName.toLowerCase() === 'hindi' || ff.ismS_SubjectName.toLowerCase() === 'sanskrit' || ff.ismS_SubjectName.toLowerCase() === 'urdu') {
                                                    ff.a = 1;
                                                    console.log(ff);
                                                    $scope.a11.push(ff);
                                                }
                                                else {
                                                    $scope.a22.push(ff);
                                                }
                                            }
                                        }
                                    });
                                });
                            });


                            $scope.per_status = [];

                            angular.forEach($scope.a11, function (eps) {
                                if ($scope.per_status.length === 0) {
                                    $scope.per_status.push(eps);
                                }
                                else if ($scope.per_status.length > 0) {
                                    var al_exm_cnt = 0;
                                    angular.forEach($scope.per_status, function (exm) {
                                        if (parseInt(exm.ismS_Id) === parseInt(eps.ismS_Id)) {
                                            al_exm_cnt += 1;
                                        }
                                    });
                                    if (al_exm_cnt === 0) {
                                        $scope.per_status.push(eps);
                                    }
                                }
                            });

                            $scope.per_status1 = [];
                            angular.forEach($scope.a22, function (eps1) {

                                if ($scope.per_status1.length === 0) {
                                    $scope.per_status1.push(eps1);
                                }
                                else if ($scope.per_status1.length > 0) {
                                    var al_exm_cnt = 0;
                                    angular.forEach($scope.per_status1, function (exm) {
                                        if (parseInt(exm.ismS_Id) === parseInt(eps1.ismS_Id)) {
                                            al_exm_cnt += 1;
                                        }
                                    });
                                    if (al_exm_cnt === 0) {
                                        $scope.per_status1.push(eps1);
                                    }
                                }
                            });



                            angular.forEach($scope.report_list, function (eps1) {
                                $scope.subjectqqq = [];
                                angular.forEach(eps1.sub_list, function (sssb) {

                                    if ($scope.subjectqqq.length === 0) {
                                        $scope.subjectqqq.push(sssb);
                                    }

                                    else if ($scope.subjectqqq.length > 0) {
                                        var al_exm_cnt = 0;
                                        angular.forEach($scope.subjectqqq, function (exm) {
                                            if (parseInt(exm.ismS_Id) === parseInt(sssb.ismS_Id)) {
                                                al_exm_cnt += 1;
                                            }
                                        });
                                        if (al_exm_cnt === 0) {
                                            $scope.subjectqqq.push(sssb);
                                        }
                                    }
                                });

                                eps1.mainlist = $scope.subjectqqq;
                            });

                            console.log($scope.report_list);

                            var l = 0;
                            angular.forEach($scope.temparry, function (kk) {
                                if (l === 0) {
                                    l += 1;
                                    kk.sb = $scope.per_status;
                                    kk.gg = $scope.per_status.length;
                                }
                                else {
                                    kk.sb = $scope.per_status1;
                                    kk.oo = $scope.per_status1.length;
                                }
                            });

                            $scope.tmee = [];
                            angular.forEach($scope.report_list, function (kl) {

                                $scope.tempsubjectstd = [];
                                $scope.tempsubjectstdww = [];
                                $scope.tempsubjectstd1ddfsdf = [];
                                angular.forEach($scope.temparry, function (kj) {

                                    $scope.tempsubjectstd1 = [];


                                    angular.forEach(kj.sb, function (jk) {

                                        angular.forEach(kl.mainlist, function (lk) {

                                            if (parseInt(lk.ismS_Id) === parseInt(jk.ismS_Id) && parseInt(lk.amcsT_Id) === parseInt(kl.student_id)) {
                                                $scope.tempsubjectstd1.push({ ismd: lk.ismS_Id, subname: lk.ismS_SubjectName, eyceS_MaxMarks: lk.ecstmpssS_MaxMarks });
                                            }
                                        });
                                    });
                                    $scope.tempsubjectstd1ddfsdf.push({ partd: kj.part, kdjd: $scope.tempsubjectstd1 });
                                });

                                $scope.tempsubjectstdww = $scope.tempsubjectstd1ddfsdf;
                                $scope.tmee.push({
                                    amat: kl.student_id, ggg: $scope.tempsubjectstdww, acysT_RollNo: kl.acysT_RollNo,
                                    amcsT_RegistrationNo: kl.amcsT_RegistrationNo, amcsT_DOB: kl.amcsT_DOB, amcsT_FirstName: kl.amcsT_FirstName,
                                    acmS_SectionName: kl.acmS_SectionName, amcO_CourseName: kl.amcO_CourseName,
                                    amB_BranchName: kl.amB_BranchName,
                                    amsE_SEMName: kl.amsE_SEMName,
                                    ecstmP_Percentage: kl.ecstmP_Percentage,
                                    ecstmP_TotalGrade: kl.ecstmP_TotalGrade, ecstmP_TotalMaxMarks: kl.ecstmP_TotalMaxMarks, ecstmP_TotalObtMarks: kl.ecstmP_TotalObtMarks,
                                    ismS_SubjectName: kl.ismS_SubjectName, mainlist: kl.mainlist, student_id: kl.student_id, sub_list: kl.sub_list
                                });
                            });


                            console.log($scope.temparry);

                            console.log("dddd");
                            console.log($scope.report_list);
                            console.log("vvvmain");
                            console.log($scope.tmee);
                            console.log("pppp");
                            console.log($scope.tempsubjectstd1ddfsdf);
                            console.log("total");
                            console.log($scope.processtot);

                        } else {
                            swal('No record Found');
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
        $scope.printToCart = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BBPUProgressReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.get_totalmin = function (exm_subjs, stu_subjs) {


            $scope.stu_grandmin_marks = 0;
            angular.forEach(exm_subjs, function (itm) {
                if (itm.eyceS_AplResultFlg) {
                    angular.forEach(stu_subjs, function (itm1) {
                        if (parseInt(itm1.ismS_Id) === parseInt(itm.ismS_Id)) {
                            $scope.stu_grandmin_marks += itm.eyceS_MinMarks;
                        }
                    });
                }
            });
        };

        $scope.OnAcdyear = function () {
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.report = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("CumulativeReport/onchangeyear", data).then(function (promise) {
                if (promise !== null) {
                    $scope.clslist = promise.classlist;
                    if ($scope.clslist.length === 0) {
                        swal("No Class Is Mapped For Selected Academic Year");
                    }
                } else {
                    swal("No Class Is Mapped For Selected Academic Year");
                }
            });
        };

        $scope.onchangeclass = function () {
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.report = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            };

            apiService.create("CumulativeReport/onchangeclass", data).then(function (promise) {
                if (promise !== null) {
                    $scope.seclist = promise.seclist;
                    if ($scope.seclist.length === 0) {
                        swal("No Section Is Mapped For Selected Class");
                    }
                } else {
                    swal("No Section Is Mapped For Selected Class");
                }
            });
        };

        $scope.onchangesection = function () {
            $scope.emE_Id = "";
            $scope.report = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id
            };

            apiService.create("CumulativeReport/onchangesection", data).then(function (promise) {
                if (promise !== null) {
                    $scope.exsplt = promise.exmstdlist;
                    if ($scope.exsplt.length === 0) {
                        swal("No Exam Is Mapped For Selected Details ");
                    }
                } else {
                    swal("No Exam Is Mapped For Selected Details");
                }
            });
        };

        $scope.onselectcategory = function () {
            $scope.report = false;
        };
    }

})();